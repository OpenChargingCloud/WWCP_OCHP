/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
    /// An OCHP resource related to the charge point or charging station.
    /// </summary>
    public class RelatedResource
    {

        #region Data

        /// <summary>
        /// The regular expression for verifying an URI.
        /// </summary>
        public static readonly Regex URI_RegEx = new Regex(@"^(http|https):\/\/.+",
                                                           RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// Referencing uri to the resource.
        /// Must begin with a protocol of the list: http, https.
        /// </summary>
        public String            URI      { get; }

        /// <summary>
        /// The class of the related resource.
        /// </summary>
        public RelatedResources  Class    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP resource related to a charge point or charging station.
        /// </summary>
        /// <param name="URI">The machine-readable result code.</param>
        /// <param name="Class">A human-readable error description.</param>
        public RelatedResource(String            URI,
                               RelatedResources  Class)
        {

            #region Initial checks

            if (URI.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(URI),  "The given resource URI must not be null or empty!");

            if (!URI_RegEx.IsMatch(URI))
                throw new ArgumentException("The given resource URI does not start with 'http' or 'https'!", nameof(URI));

            #endregion

            this.URI    = URI;
            this.Class  = Class;

        }

        #endregion


        #region Documentation

        // <ns:relatedResource>
        //    <ns:uri>?</ns:uri>
        //    <ns:class>?</ns:class>
        // </ns:relatedResource>

        #endregion

        #region (static) Parse(RelatedResourceXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP resource related to a charge point or charging station.
        /// </summary>
        /// <param name="RelatedResourceXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static RelatedResource Parse(XElement             RelatedResourceXML,
                                            OnExceptionDelegate  OnException = null)
        {

            RelatedResource _RelatedResource;

            if (TryParse(RelatedResourceXML, out _RelatedResource, OnException))
                return _RelatedResource;

            return null;

        }

        #endregion

        #region (static) Parse(RelatedResourceText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP resource related to a charge point or charging station.
        /// </summary>s
        /// <param name="RelatedResourceText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static RelatedResource Parse(String               RelatedResourceText,
                                            OnExceptionDelegate  OnException = null)
        {

            RelatedResource _RelatedResource;

            if (TryParse(RelatedResourceText, out _RelatedResource, OnException))
                return _RelatedResource;

            return null;

        }

        #endregion

        #region (static) TryParse(RelatedResourceXML,  out RelatedResource, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP resource related to a charge point or charging station.
        /// </summary>
        /// <param name="RelatedResourceXML">The XML to parse.</param>
        /// <param name="RelatedResource">The parsed resource.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             RelatedResourceXML,
                                       out RelatedResource  RelatedResource,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (RelatedResourceXML.Name != OCHPNS.Default + "relatedResource")
                    throw new ArgumentException("The given XML element is invalid!", nameof(RelatedResourceXML));

                RelatedResource = new RelatedResource(

                                      RelatedResourceXML.ElementValueOrFail(OCHPNS.Default + "uri"),

                                      RelatedResourceXML.MapValueOrFail    (OCHPNS.Default + "class",
                                                                            XML_IO.AsRelatedResource)

                                  );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, RelatedResourceXML, e);

                RelatedResource = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(RelatedResourceText, out RelatedResource, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP resource related to a charge point or charging station.
        /// </summary>
        /// <param name="RelatedResourceText">The text to parse.</param>
        /// <param name="RelatedResource">The parsed resource.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               RelatedResourceText,
                                       out RelatedResource  RelatedResource,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(RelatedResourceText).Root,
                             out RelatedResource,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, RelatedResourceText, e);
            }

            RelatedResource = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "relatedResource",

                   new XElement(OCHPNS.Default + "uri",    URI),
                   new XElement(OCHPNS.Default + "class",  XML_IO.AsText(Class))

               );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Class, " from ", URI);

        #endregion

    }

}
