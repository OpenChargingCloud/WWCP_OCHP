/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// OCHP geo coordinate with additional information.
    /// </summary>
    public class ExtendedGeoCoordinate
    {

        #region Properties

        /// <summary>
        /// Name of the point in local language or as written at the location.
        /// For example the street name of a parking lot entrance or it's number.
        /// </summary>
        public String              Name                 { get; }

        /// <summary>
        /// The type of this geo point for categorization and right usage.
        /// </summary>
        public GeoCoordinateTypes  GeoCoordinateType    { get; }

        /// <summary>
        /// The geo coordinate.
        /// </summary>
        public GeoCoordinate       GeoCoordinate        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create new ratings of a charge point.
        /// </summary>
        /// <param name="Name">Name of the point in local language or as written at the location. For example the street name of a parking lot entrance or it's number.</param>
        /// <param name="GeoCoordinateType">The type of this geo point for categorization and right usage.</param>
        /// <param name="GeoCoordiante">The geo coordinate.</param>
        public ExtendedGeoCoordinate(String              Name,
                                     GeoCoordinateTypes  GeoCoordinateType,
                                     GeoCoordinate       GeoCoordiante)
        {

            #region Initial checks

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),  "The given name must not be null or empty!");

            #endregion

            this.Name               = Name;
            this.GeoCoordinateType  = GeoCoordinateType;
            this.GeoCoordinate      = GeoCoordiante;

        }

        #endregion


        #region Documentation

        // <OCHP:relatedLocation lat="?" lon="?" name="?" type="?"/>

        #endregion

        #region (static) Parse(ExtendedGeoCoordinateXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP geo coordinate with additional information.
        /// </summary>
        /// <param name="ExtendedGeoCoordinateXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ExtendedGeoCoordinate Parse(XElement             ExtendedGeoCoordinateXML,
                                                  OnExceptionDelegate  OnException = null)
        {

            ExtendedGeoCoordinate _ExtendedGeoCoordinate;

            if (TryParse(ExtendedGeoCoordinateXML, out _ExtendedGeoCoordinate, OnException))
                return _ExtendedGeoCoordinate;

            return null;

        }

        #endregion

        #region (static) Parse(ExtendedGeoCoordinateText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP geo coordinate with additional information.
        /// </summary>s
        /// <param name="ExtendedGeoCoordinateText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ExtendedGeoCoordinate Parse(String               ExtendedGeoCoordinateText,
                                                  OnExceptionDelegate  OnException = null)
        {

            ExtendedGeoCoordinate _ExtendedGeoCoordinate;

            if (TryParse(ExtendedGeoCoordinateText, out _ExtendedGeoCoordinate, OnException))
                return _ExtendedGeoCoordinate;

            return null;

        }

        #endregion

        #region (static) TryParse(ExtendedGeoCoordinateXML,  out ExtendedGeoCoordinate, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP geo coordinate with additional information.
        /// </summary>
        /// <param name="ExtendedGeoCoordinateXML">The XML to parse.</param>
        /// <param name="ExtendedGeoCoordinate">The parsed geo coordinate with additional information.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                   ExtendedGeoCoordinateXML,
                                       out ExtendedGeoCoordinate  ExtendedGeoCoordinate,
                                       OnExceptionDelegate        OnException  = null)
        {

            try
            {

                if (ExtendedGeoCoordinateXML.Name != OCHPNS.Default + "EvseImageUrlType")
                    throw new ArgumentException("The given XML element is invalid!", nameof(ExtendedGeoCoordinateXML));

                ExtendedGeoCoordinate = new ExtendedGeoCoordinate(

                                            ExtendedGeoCoordinateXML.AttributeValueOrFail   (OCHPNS.Default + "name"),

                                            ExtendedGeoCoordinateXML.MapAttributeValueOrFail(OCHPNS.Default + "type",
                                                                                             XML_IO.AsGeoCoordinateType),

                                            GeoCoordinate.Create(

                                                ExtendedGeoCoordinateXML.MapAttributeValueOrFail(OCHPNS.Default + "lat",
                                                                                                 Latitude.Parse),

                                                ExtendedGeoCoordinateXML.MapAttributeValueOrFail(OCHPNS.Default + "lon",
                                                                                                 Longitude.Parse)

                                            )

                                        );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, ExtendedGeoCoordinateXML, e);

                ExtendedGeoCoordinate = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ExtendedGeoCoordinateText, out ExtendedGeoCoordinate, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP geo coordinate with additional information.
        /// </summary>
        /// <param name="ExtendedGeoCoordinateText">The text to parse.</param>
        /// <param name="ExtendedGeoCoordinate">The parsed geo coordinate with additional information.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                     ExtendedGeoCoordinateText,
                                       out ExtendedGeoCoordinate  ExtendedGeoCoordinate,
                                       OnExceptionDelegate        OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ExtendedGeoCoordinateText).Root,
                             out ExtendedGeoCoordinate,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, ExtendedGeoCoordinateText, e);
            }

            ExtendedGeoCoordinate = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "relatedLocation",
                               new XAttribute(OCHPNS.Default + "lat",   GeoCoordinate.Latitude. Value),
                               new XAttribute(OCHPNS.Default + "lon",   GeoCoordinate.Longitude.Value),
                               new XAttribute(OCHPNS.Default + "name",  Name),
                               new XAttribute(OCHPNS.Default + "type",  XML_IO.AsText(GeoCoordinateType))
                           );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Returns a string representation of the given object.
        /// </summary>
        public override String ToString()

            => String.Concat(GeoCoordinate,
                             ", Name = ", Name,
                             ", Type = ", GeoCoordinateType);

        #endregion

    }

}
