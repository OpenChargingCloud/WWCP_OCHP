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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// Static POI data regarding a charge point / EVSE.
    /// </summary>
    public class ChargePointInfo
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charge point identification.
        /// </summary>
        public static readonly Regex LocationId_RegEx  = new Regex(@"^[A-Z0-9 ]{1,15}$",
                                                                   RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for parsing a charge point identification.
        /// </summary>
        public static readonly Regex TelephoneNumber_RegEx = new Regex(@"^[\+]{0,1}[1-9]{0,1}[0-9 \-]{1,17}$",
                                                                   RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// Globally unique identifier.
        /// </summary>
        public EVSE_Id                              EVSEId                  { get; }

        /// <summary>
        /// Identifies a location/pool of EVSEs. Unique within one EVSE Operator.
        /// </summary>
        public String                               LocationId              { get; }

        /// <summary>
        /// Date and time of the latest data update for this ChargePointInfo.
        /// When set it must be updated if any value is changed.
        /// </summary>
        public DateTime?                            Timestamp               { get; }

        /// <summary>
        /// Official name; should be unique in the geographical area. [100]
        /// </summary>
        public String                               LocationName            { get; }

        /// <summary>
        /// ISO-639-3 language code defining the language of the location name (Alpha3Code).
        /// </summary>
        public String                               LocationNameLang        { get; }

        /// <summary>
        /// Links to images related to the EVSE such as photos or logos.
        /// </summary>
        public IEnumerable<EVSEImageURLs>           Images                  { get; }

        /// <summary>
        /// Links to be visited by the user, related to the charge point or charging station.
        /// </summary>
        public IEnumerable<RelatedResource>         RelatedResources        { get; }

        /// <summary>
        /// Address of the charge point, consisting of housenumber, street, zipcode, city, country.
        /// </summary>
        public Address                              ChargePointAddress      { get; }

        /// <summary>
        /// Geographical location of the charge point itself (power outlet).
        /// </summary>
        public GeoCoordinate                        ChargePointLocation     { get; }

        /// <summary>
        /// Geographical location of related points relevant to the user.
        /// </summary>
        public IEnumerable<ExtendedGeoCoordinate>   RelatedLocations        { get; }

        /// <summary>
        /// One of IANA tzdata's TZ-values representing the time zone of the location.
        /// http://www.iana.org/time-zones
        /// </summary>
        /// <example>"Europe/Oslo", "Europe/Zurich"</example>
        public String                               TimeZone                { get; }

        /// <summary>
        /// The times the EVSE is operating and can be used for charging.
        /// Must nor be provided if operating hours are unsure/unknown.
        /// </summary>
        public Hours                                OpeningTimes            { get; }

        /// <summary>
        /// The current status of the charge point.
        /// </summary>
        public ChargePointStatusTypes               Status                  { get; }

        /// <summary>
        /// Planned status changes in the future. If a time span matches with
        /// the current or displayed date, the corresponding value overwrites
        /// "status".
        /// </summary>
        public IEnumerable<ChargePointSchedule>     ChargePointSchedule     { get; }

        /// <summary>
        /// Service hotline for charging station to be displayed to the EV user.
        /// Separators recommended. Must include country code (e.g. +49).
        /// </summary>
        public String                               TelephoneNumber         { get; }

        /// <summary>
        /// The general type of the charge point location.
        /// </summary>
        public GeneralLocationTypes                 Location                { get; }

        /// <summary>
        /// Information regarding a parking spot that can be used to access the EVSE.
        /// </summary>
        public IEnumerable<ParkingSpotInfo>         ParkingSpots            { get; }

        /// <summary>
        /// Restrictions applying to the usage of the charging station.
        /// </summary>
        public IEnumerable<ParkingRestrictions>     ParkingRestrictions     { get; }

        /// <summary>
        /// List of available payment or access methods on site.
        /// </summary>
        public IEnumerable<AuthMethodTypes>         AuthMethods             { get; }

        /// <summary>
        /// Which receptable type is/are present for a power outlet.
        /// </summary>
        public IEnumerable<ConnectorType>           Connectors              { get; }

        /// <summary>
        /// The type of the charge point "AC" or "DC".
        /// </summary>
        public ChargePointTypes                     ChargePointType         { get; }

        /// <summary>
        /// Defines the ratings for the charge point.
        /// </summary>
        public Ratings                              Ratings                 { get; }

        /// <summary>
        /// Language(s) of the user interface or printed on-site instructions.
        /// ISO-639-3 language code.
        /// </summary>
        public IEnumerable<String>                  UserInterfaceLang       { get; }

        /// <summary>
        /// If a reservation of this charge point is possible, this is the maximum
        /// duration the CPO will allow a reservation for in minutes.
        /// Recommended: 30 or 60 minutes.
        /// </summary>
        public TimeSpan                             MaxReservation          { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charge data record information object.
        /// </summary>
        /// <param name="EVSEId">Globally unique identifier.</param>
        /// <param name="LocationId">Identifies a location/pool of EVSEs. Unique within one EVSE Operator.</param>
        /// <param name="Timestamp">Date and time of the latest data update for this ChargePointInfo. When set it must be updated if any value is changed.</param>
        /// <param name="LocationName">Official name; should be unique in the geographical area. [100]</param>
        /// <param name="LocationNameLang">ISO-639-3 language code defining the language of the location name (Alpha3Code).</param>
        /// <param name="Images">Links to images related to the EVSE such as photos or logos.</param>
        /// <param name="RelatedResources">Links to be visited by the user, related to the charge point or charging station.</param>
        /// <param name="ChargePointAddress">Address of the charge point, consisting of housenumber, street, zipcode, city, country.</param>
        /// <param name="ChargePointLocation">Geographical location of the charge point itself (power outlet).</param>
        /// <param name="RelatedLocations">Geographical location of related points relevant to the user.</param>
        /// <param name="TimeZone">One of IANA tzdata's TZ-values representing the time zone of the location.</param>
        /// <param name="OpeningTimes">The times the EVSE is operating and can be used for charging. Must nor be provided if operating hours are unsure/unknown.</param>
        /// <param name="Status">The current status of the charge point.</param>
        /// <param name="ChargePointSchedule">Planned status changes in the future. If a time span matches with the current or displayed date, the corresponding value overwrites "status".</param>
        /// <param name="TelephoneNumber">Service hotline for charging station to be displayed to the EV user. Separators recommended. Must include country code (e.g. +49).</param>
        /// <param name="Location">The general type of the charge point location.</param>
        /// <param name="ParkingSpots">Information regarding a parking spot that can be used to access the EVSE.</param>
        /// <param name="ParkingRestrictions">Restrictions applying to the usage of the charging station.</param>
        /// <param name="AuthMethods">List of available payment or access methods on site.</param>
        /// <param name="Connectors">Which receptable type is/are present for a power outlet.</param>
        /// <param name="ChargePointType">The type of the charge point "AC" or "DC".</param>
        /// <param name="Ratings">Defines the ratings for the charge point.</param>
        /// <param name="UserInterfaceLang">Language(s) of the user interface or printed on-site instructions.</param>
        /// <param name="MaxReservation">If a reservation of this charge point is possible, this is the maximum duration the CPO will allow a reservation for in minutes. Recommended: 30 or 60 minutes.</param>
        public ChargePointInfo(EVSE_Id                             EVSEId,
                               String                              LocationId,
                               DateTime?                           Timestamp,
                               String                              LocationName,
                               String                              LocationNameLang,
                               IEnumerable<EVSEImageURLs>          Images,
                               IEnumerable<RelatedResource>        RelatedResources,
                               Address                             ChargePointAddress,
                               GeoCoordinate                       ChargePointLocation,
                               IEnumerable<ExtendedGeoCoordinate>  RelatedLocations,
                               String                              TimeZone,
                               Hours                               OpeningTimes,
                               ChargePointStatusTypes              Status,
                               IEnumerable<ChargePointSchedule>    ChargePointSchedule,
                               String                              TelephoneNumber,
                               GeneralLocationTypes                Location,
                               IEnumerable<ParkingSpotInfo>        ParkingSpots,
                               IEnumerable<ParkingRestrictions>    ParkingRestrictions,
                               IEnumerable<AuthMethodTypes>        AuthMethods,
                               IEnumerable<ConnectorType>          Connectors,
                               ChargePointTypes                    ChargePointType,
                               Ratings                             Ratings,
                               IEnumerable<String>                 UserInterfaceLang,
                               TimeSpan                            MaxReservation)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),              "The given unique identification of an EVSE must not be null!");

            if (LocationId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(LocationId),          "The given unique identification of a charging location must not be null!");

            if (LocationName.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(LocationName),        "The given location name must not be null!");

            if (LocationNameLang.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(LocationNameLang),    "The given location name language must not be null!");

            if (ChargePointAddress == null)
                throw new ArgumentNullException(nameof(ChargePointAddress),  "The given address must not be null!");

            if (ChargePointLocation == null)
                throw new ArgumentNullException(nameof(ChargePointLocation), "The given charge point geo coordinate must not be null!");

            if (OpeningTimes == null)
                throw new ArgumentNullException(nameof(OpeningTimes),        "The given opening times must not be null!");

            if (AuthMethods == null || AuthMethods.Count() < 1)
                throw new ArgumentNullException(nameof(AuthMethods),         "The given enumeration of authentication methods must have at least one item!");

            if (Connectors == null || Connectors.Count() < 1)
                throw new ArgumentNullException(nameof(Connectors),          "The given enumeration of connectors must have at least one item!");

            #endregion

            this.EVSEId               = EVSEId;
            this.LocationId           = LocationId;
            this.Timestamp            = Timestamp;
            this.LocationName         = LocationName;
            this.LocationNameLang     = LocationNameLang;
            this.Images               = Images;
            this.RelatedResources     = RelatedResources;
            this.ChargePointAddress   = ChargePointAddress;
            this.ChargePointLocation  = ChargePointLocation;
            this.RelatedLocations     = RelatedLocations;
            this.TimeZone             = TimeZone;
            this.OpeningTimes         = OpeningTimes;
            this.Status               = Status;
            this.ChargePointSchedule  = ChargePointSchedule;
            this.TelephoneNumber      = TelephoneNumber;
            this.Location             = Location;
            this.ParkingSpots         = ParkingSpots;
            this.ParkingRestrictions  = ParkingRestrictions;
            this.AuthMethods          = AuthMethods;
            this.Connectors           = Connectors;
            this.ChargePointType      = ChargePointType;
            this.Ratings              = Ratings;
            this.UserInterfaceLang    = UserInterfaceLang;
            this.MaxReservation       = MaxReservation;

        }

        #endregion


    }

}
