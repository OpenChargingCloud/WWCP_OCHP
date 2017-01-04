/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
 * This file is part of WWCP OCHP <https://github.com/OpenChargingCloud/WWCP_OCHP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Xml.Linq;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// An image related to an EVSE.
    /// </summary>
    public class EVSEImageURL
    {

        #region Data

        /// <summary>
        /// The regular expression for verifying an URI.
        /// </summary>
        public static readonly Regex URI_RegEx = new Regex(@"^(http|https|ftp):\/\/.+",
                                                           RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// URI from where the image data can be fetched.
        /// Must begin with a protocol of the list: http, https, file, ftp.
        /// </summary>
        public String            URI         { get; }

        /// <summary>
        /// The image class.
        /// </summary>
        public EVSEImageClasses  Class       { get; }

        /// <summary>
        /// The image type like: gif, jpeg, png, svg.
        /// </summary>
        public String            Type        { get; }

        /// <summary>
        /// The optional width of the full scale image.
        /// </summary>
        public UInt16?           Width       { get; }

        /// <summary>
        /// The optional height of the full scale image.
        /// </summary>
        public UInt16?           Height      { get; }

        /// <summary>
        /// An optional URI from where a thumbnail of the image can be fetched.
        /// Must begin with a protocol of the list: http, https, file, ftp.
        /// </summary>
        public String            ThumbURI    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new EVSE related image.
        /// </summary>
        /// <param name="URI">URI from where the image data can be fetched. Must begin with a protocol of the list: http, https, file, ftp.</param>
        /// <param name="Class">The image class.</param>
        /// <param name="Type">The image type like: gif, jpeg, png, svg.</param>
        /// <param name="Width">The optional width of the full scale image.</param>
        /// <param name="Height">The optional height of the full scale image.</param>
        /// <param name="ThumbURI">An optional URI from where a thumbnail of the image can be fetched. Must begin with a protocol of the list: http, https, file, ftp.</param>
        public EVSEImageURL(String            URI,
                            EVSEImageClasses  Class,
                            String            Type,
                            UInt16?           Width     = null,
                            UInt16?           Height    = null,
                            String            ThumbURI  = null)
        {

            #region Initial checks

            if (URI.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(URI),   "The given image URI must not be null or empty!");

            if (!URI_RegEx.IsMatch(URI))
                throw new ArgumentException("The given URI does not start with 'http', 'https' or 'ftp'!", nameof(URI));

            if (Type.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Type),  "The given image type must not be null or empty!");

            #endregion

            this.URI       = URI;
            this.Class     = Class;
            this.Type      = Type;
            this.Width     = Width  ?? new UInt16?();
            this.Height    = Height ?? new UInt16?();
            this.ThumbURI  = ThumbURI;

        }

        #endregion


        #region Documentation

        // ?

        #endregion

        #region (static) Parse(EVSEImageURLXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP EVSE reference image.
        /// </summary>
        /// <param name="EVSEImageURLXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEImageURL Parse(XElement             EVSEImageURLXML,
                                         OnExceptionDelegate  OnException = null)
        {

            EVSEImageURL _EVSEImageURL;

            if (TryParse(EVSEImageURLXML, out _EVSEImageURL, OnException))
                return _EVSEImageURL;

            return null;

        }

        #endregion

        #region (static) Parse(EVSEImageURLText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP EVSE reference image.
        /// </summary>s
        /// <param name="EVSEImageURLText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEImageURL Parse(String               EVSEImageURLText,
                                         OnExceptionDelegate  OnException = null)
        {

            EVSEImageURL _EVSEImageURL;

            if (TryParse(EVSEImageURLText, out _EVSEImageURL, OnException))
                return _EVSEImageURL;

            return null;

        }

        #endregion

        #region (static) TryParse(EVSEImageURLXML,  out EVSEImageURL, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP EVSE reference image.
        /// </summary>
        /// <param name="EVSEImageURLXML">The XML to parse.</param>
        /// <param name="EVSEImageURL">The parsed EVSE reference image.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             EVSEImageURLXML,
                                       out EVSEImageURL     EVSEImageURL,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (EVSEImageURLXML.Name != OCHPNS.Default + "EvseImageUrlType")
                    throw new ArgumentException("The given XML element is invalid!", nameof(EVSEImageURLXML));

                EVSEImageURL = new EVSEImageURL(

                                   EVSEImageURLXML.ElementValueOrFail(OCHPNS.Default + "uri"),

                                   EVSEImageURLXML.MapValueOrFail    (OCHPNS.Default + "class",
                                                                      XML_IO.AsEVSEImageClasses),

                                   EVSEImageURLXML.ElementValueOrFail(OCHPNS.Default + "type"),

                                   EVSEImageURLXML.MapValueOrNull    (OCHPNS.Default + "width",
                                                                      UInt16.Parse),

                                   EVSEImageURLXML.MapValueOrNull    (OCHPNS.Default + "height",
                                                                      UInt16.Parse)

                               );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, EVSEImageURLXML, e);

                EVSEImageURL = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(EVSEImageURLText, out EVSEImageURL, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP EVSE reference image.
        /// </summary>
        /// <param name="EVSEImageURLText">The text to parse.</param>
        /// <param name="EVSEImageURL">The parsed EVSE reference image.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               EVSEImageURLText,
                                       out EVSEImageURL     EVSEImageURL,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(EVSEImageURLText).Root,
                             out EVSEImageURL,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, EVSEImageURLText, e);
            }

            EVSEImageURL = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "EvseImageUrlType",

                   new XElement(OCHPNS.Default + "uri",             URI),

                   ThumbURI.IsNotNullOrEmpty()
                       ? new XElement(OCHPNS.Default + "thumbUri",  ThumbURI)
                       : null,

                   new XElement(OCHPNS.Default + "class",           XML_IO.AsText(Class)),
                   new XElement(OCHPNS.Default + "type",            Type),

                   Width.HasValue
                       ? new XElement(OCHPNS.Default + "width",     Width)
                       : null,

                   Height.HasValue
                       ? new XElement(OCHPNS.Default + "height",    Height)
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (EVSEImageURL1, EVSEImageURL2)

        /// <summary>
        /// Compares two EVSE images for equality.
        /// </summary>
        /// <param name="EVSEImageURL1">An EVSE image.</param>
        /// <param name="EVSEImageURL2">Another EVSE image.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EVSEImageURL EVSEImageURL1, EVSEImageURL EVSEImageURL2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEImageURL1, EVSEImageURL2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEImageURL1 == null) || ((Object) EVSEImageURL2 == null))
                return false;

            return EVSEImageURL1.Equals(EVSEImageURL2);

        }

        #endregion

        #region Operator != (EVSEImageURL1, EVSEImageURL2)

        /// <summary>
        /// Compares two EVSE images for inequality.
        /// </summary>
        /// <param name="EVSEImageURL1">An EVSE image.</param>
        /// <param name="EVSEImageURL2">Another EVSE image.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EVSEImageURL EVSEImageURL1, EVSEImageURL EVSEImageURL2)

            => !(EVSEImageURL1 == EVSEImageURL2);

        #endregion

        #endregion

        #region IEquatable<EVSEImageURL> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is an EVSE image.
            var EVSEImageURL = Object as EVSEImageURL;
            if ((Object) EVSEImageURL == null)
                return false;

            return this.Equals(EVSEImageURL);

        }

        #endregion

        #region Equals(EVSEImageURL)

        /// <summary>
        /// Compares two EVSE images for equality.
        /// </summary>
        /// <param name="EVSEImageURL">An EVSE image to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEImageURL EVSEImageURL)
        {

            if ((Object) EVSEImageURL == null)
                return false;

            return URI.  Equals(EVSEImageURL.URI)   &&
                   Class.Equals(EVSEImageURL.Class) &&
                   Type. Equals(EVSEImageURL.Type)  &&

                   ((!Width. HasValue && !EVSEImageURL.Width. HasValue) ||
                     (Width. HasValue &&  EVSEImageURL.Width. HasValue && Width. Value.Equals(EVSEImageURL.Width. Value))) &&

                   ((!Height.HasValue && !EVSEImageURL.Height.HasValue) ||
                     (Height.HasValue &&  EVSEImageURL.Height.HasValue && Height.Value.Equals(EVSEImageURL.Height.Value))) &&

                   ((ThumbURI == null && EVSEImageURL.ThumbURI == null) ||
                    (ThumbURI != null && EVSEImageURL.ThumbURI != null && ThumbURI.Equals(EVSEImageURL.ThumbURI)));


        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return URI.  GetHashCode() * 17 ^
                       Class.GetHashCode() * 13 ^
                       Type. GetHashCode() * 11 ^

                       (Width.HasValue
                           ? Width.GetHashCode()
                           : 0) * 7 ^

                       (Height.HasValue
                           ? Height.GetHashCode()
                           : 0) * 5 ^

                       (ThumbURI != null
                           ? ThumbURI.GetHashCode()
                           : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Class, " from ", URI);

        #endregion

    }

}
