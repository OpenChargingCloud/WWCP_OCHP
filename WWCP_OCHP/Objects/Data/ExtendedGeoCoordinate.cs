﻿/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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
    /// OCHP geo coordinate with additional information
    /// </summary>
    public class ExtendedGeoCoordinate : GeoCoordinate
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

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create new ratings of a charge point.
        /// </summary>
        /// <param name="Name">Name of the point in local language or as written at the location. For example the street name of a parking lot entrance or it's number.</param>
        /// <param name="GeoCoordinateType">The type of this geo point for categorization and right usage.</param>
        public ExtendedGeoCoordinate(String              Name,
                                     GeoCoordinateTypes  GeoCoordinateType,
                                     Latitude            Latitude,
                                     Longitude           Longitude,
                                     Altitude?           Altitude    = null,
                                     GravitationalModel  Projection  = GravitationalModel.WGS84,
                                     Planets             Planet      = Planets.Earth)

            : base(Latitude,
                   Longitude,
                   Altitude,
                   Projection,
                   Planet)

        {

            #region Initial checks

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),  "The given name must not be null or empty!");

            #endregion

            this.Name               = Name;
            this.GeoCoordinateType  = GeoCoordinateType;

        }

        #endregion


    }

}
