/*
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
using System.Text.RegularExpressions;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// Static POI data regarding a parking spot.
    /// </summary>
    public class ParkingSpotInfo
    {

        #region Data

        public static readonly Regex FloorLevel_RegExpr         = new Regex(@"[A-Z0-9\-\+/]{1,4}", RegexOptions.IgnorePatternWhitespace);

        public static readonly Regex ParkingSpotNumber_RegExpr  = new Regex(@"[A-Z0-9\-\+/]{1,5}", RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of the parking space.
        /// </summary>
        public Parking_Id               ParkingId              { get; }

        /// <summary>
        /// Restrictions to the usage of this parking spot.
        /// </summary>
        public ParkingRestrictions  ParkingRestrictions    { get; }

        /// <summary>
        /// Level on which the charge station is located (in garage buildings)
        /// in the locally displayed numbering scheme.
        /// </summary>
        /// <example>"-2","P-5", "2", "+5"</example>
        public String                   FloorLevel             { get; }

        /// <summary>
        /// Locally displayed parking slot number.
        /// </summary>
        /// <example>"10", "251","B25", "P-234"</example>
        public String                   ParkingSpotNumber      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create new static POI data regarding a parking spot.
        /// </summary>
        /// <param name="ParkingId">The unique identification of the parking space.</param>
        /// <param name="ParkingRestrictions">Restrictions to the usage of this parking spot.</param>
        /// <param name="FloorLevel">Level on which the charge station is located (in garage buildings) in the locally displayed numbering scheme. Examples: "-2","P-5", "2", "+5"</param>
        /// <param name="ParkingSpotNumber">Locally displayed parking slot number. Examples: "10", "251","B25", "P-234"</param>
        public ParkingSpotInfo(Parking_Id               ParkingId,
                               ParkingRestrictions  ParkingRestrictions,
                               String                   FloorLevel,
                               String                   ParkingSpotNumber)

        {

            #region Initial checks

            if (ParkingId == null)
                throw new ArgumentNullException(nameof(ParkingId), "The given unique identification of a parking space must not be null!");

            if (FloorLevel        != null && !FloorLevel_RegExpr.       IsMatch(FloorLevel))
                throw new ArgumentException("The given floor level is invalid!",          nameof(FloorLevel));

            if (ParkingSpotNumber != null && !ParkingSpotNumber_RegExpr.IsMatch(ParkingSpotNumber))
                throw new ArgumentException("The given parking spot number is invalid!",  nameof(ParkingSpotNumber));

            #endregion

            this.ParkingId            = ParkingId;
            this.ParkingRestrictions  = ParkingRestrictions;
            this.FloorLevel           = FloorLevel;
            this.ParkingSpotNumber    = ParkingSpotNumber;

        }

        #endregion


    }

}
