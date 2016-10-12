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
        public static readonly Regex LocationId_RegEx       = new Regex(@"^[A-Z0-9 ]{1,15}$",
                                                                        RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for parsing an optional service hotline telephone number.
        /// </summary>
        public static readonly Regex TelephoneNumber_RegEx  = new Regex(@"^[\+]{0,1}[1-9]{0,1}[0-9 \-]{1,17}$",
                                                                        RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// Globally unique identifier.
        /// </summary>
        public EVSE_Id                               EVSEId                  { get; }

        /// <summary>
        /// Identifies a location/pool of EVSEs. Unique within one EVSE Operator.
        /// </summary>
        public String                                LocationId              { get; }

        /// <summary>
        /// Date and time of the latest data update for this ChargePointInfo.
        /// When set it must be updated if any value is changed.
        /// </summary>
        public DateTime?                             Timestamp               { get; }

        /// <summary>
        /// Official name; should be unique in the geographical area. [100]
        /// </summary>
        public String                                LocationName            { get; }

        /// <summary>
        /// ISO-639-3 language code defining the language of the location name (Alpha3Code).
        /// </summary>
        public String                                LocationNameLang        { get; }

        /// <summary>
        /// Links to images related to the EVSE such as photos or logos.
        /// </summary>
        public IEnumerable<EVSEImageURL>             Images                  { get; }

        /// <summary>
        /// Links to be visited by the user, related to the charge point or charging station.
        /// </summary>
        public IEnumerable<RelatedResource>          RelatedResources        { get; }

        /// <summary>
        /// Address of the charge point, consisting of housenumber, street, zipcode, city, country.
        /// </summary>
        public Address                               ChargePointAddress      { get; }

        /// <summary>
        /// Geographical location of the charge point itself (power outlet).
        /// </summary>
        public GeoCoordinate                         ChargePointLocation     { get; }

        /// <summary>
        /// Geographical location of related points relevant to the user.
        /// </summary>
        public IEnumerable<ExtendedGeoCoordinate>    RelatedLocations        { get; }

        /// <summary>
        /// One of IANA tzdata's TZ-values representing the time zone of the location.
        /// http://www.iana.org/time-zones
        /// </summary>
        /// <example>"Europe/Oslo", "Europe/Zurich"</example>
        public String                                TimeZone                { get; }

        /// <summary>
        /// The times the EVSE is operating and can be used for charging.
        /// Must nor be provided if operating hours are unsure/unknown.
        /// </summary>
        public Hours                                 OpeningTimes            { get; }

        /// <summary>
        /// The current status of the charge point.
        /// </summary>
        public ChargePointStatus?                    Status                  { get; }

        /// <summary>
        /// Planned status changes in the future. If a time span matches with
        /// the current or displayed date, the corresponding value overwrites
        /// "status".
        /// </summary>
        public IEnumerable<ChargePointSchedule>      ChargePointSchedule     { get; }

        /// <summary>
        /// Service hotline for charging station to be displayed to the EV user.
        /// Separators recommended. Must include country code (e.g. +49).
        /// </summary>
        public String                                TelephoneNumber         { get; }

        /// <summary>
        /// The general type of the charge point location.
        /// </summary>
        public GeneralLocationTypes                  Location                { get; }

        /// <summary>
        /// Information regarding a parking spot that can be used to access the EVSE.
        /// </summary>
        public IEnumerable<ParkingSpotInfo>          ParkingSpots            { get; }

        /// <summary>
        /// Restrictions applying to the usage of the charging station.
        /// </summary>
        public RestrictionTypes?                     Restrictions            { get; }

        /// <summary>
        /// List of available payment or access methods on site.
        /// </summary>
        public AuthMethodTypes                       AuthMethods             { get; }

        /// <summary>
        /// Which receptable type is/are present for a power outlet.
        /// </summary>
        public IEnumerable<ConnectorType>            Connectors              { get; }

        /// <summary>
        /// The type of the charge point "AC" or "DC".
        /// </summary>
        public ChargePointTypes                      ChargePointType         { get; }

        /// <summary>
        /// Defines the ratings for the charge point.
        /// </summary>
        public Ratings                               Ratings                 { get; }

        /// <summary>
        /// Language(s) of the user interface or printed on-site instructions.
        /// ISO-639-3 language code.
        /// </summary>
        public IEnumerable<String>                   UserInterfaceLang       { get; }

        /// <summary>
        /// If a reservation of this charge point is possible, this is the maximum
        /// duration the CPO will allow a reservation for in minutes.
        /// Recommended: 30 or 60 minutes.
        /// </summary>
        public TimeSpan?                             MaxReservation          { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charge data record information object.
        /// </summary>
        /// <param name="EVSEId">Globally unique identifier.</param>
        /// <param name="LocationId">Identifies a location/pool of EVSEs. Unique within one EVSE Operator.</param>
        /// <param name="LocationName">Official name; should be unique in the geographical area. [100]</param>
        /// <param name="LocationNameLang">ISO-639-3 language code defining the language of the location name (Alpha3Code).</param>
        /// <param name="ChargePointAddress">Address of the charge point, consisting of housenumber, street, zipcode, city, country.</param>
        /// <param name="ChargePointLocation">Geographical location of the charge point itself (power outlet).</param>
        /// <param name="Location">The general type of the charge point location.</param>
        /// <param name="AuthMethods">List of available payment or access methods on site.</param>
        /// <param name="Connectors">Which receptable type is/are present for a power outlet.</param>
        /// <param name="ChargePointType">The type of the charge point "AC" or "DC".</param>
        /// 
        /// <param name="Timestamp">Date and time of the latest data update for this ChargePointInfo. When set it must be updated if any value is changed.</param>
        /// <param name="Images">Links to images related to the EVSE such as photos or logos.</param>
        /// <param name="RelatedResources">Links to be visited by the user, related to the charge point or charging station.</param>
        /// <param name="RelatedLocations">Geographical location of related points relevant to the user.</param>
        /// <param name="TimeZone">One of IANA tzdata's TZ-values representing the time zone of the location.</param>
        /// <param name="OpeningTimes">The times the EVSE is operating and can be used for charging. Must nor be provided if operating hours are unsure/unknown.</param>
        /// <param name="Status">The current status of the charge point.</param>
        /// <param name="ChargePointSchedule">Planned status changes in the future. If a time span matches with the current or displayed date, the corresponding value overwrites "status".</param>
        /// <param name="TelephoneNumber">Service hotline for charging station to be displayed to the EV user. Separators recommended. Must include country code (e.g. +49).</param>
        /// <param name="ParkingSpots">Information regarding a parking spot that can be used to access the EVSE.</param>
        /// <param name="Restrictions">Restrictions applying to the usage of the charging station.</param>
        /// <param name="Ratings">Defines the ratings for the charge point.</param>
        /// <param name="UserInterfaceLang">Language(s) of the user interface or printed on-site instructions.</param>
        /// <param name="MaxReservation">If a reservation of this charge point is possible, this is the maximum duration the CPO will allow a reservation for in minutes. Recommended: 30 or 60 minutes.</param>
        public ChargePointInfo(EVSE_Id                             EVSEId,
                               String                              LocationId,
                               String                              LocationName,
                               String                              LocationNameLang,
                               Address                             ChargePointAddress,
                               GeoCoordinate                       ChargePointLocation,
                               GeneralLocationTypes                Location,
                               AuthMethodTypes                     AuthMethods,
                               IEnumerable<ConnectorType>          Connectors,
                               ChargePointTypes                    ChargePointType,

                               DateTime?                           Timestamp            = null,
                               IEnumerable<EVSEImageURL>           Images               = null,
                               IEnumerable<RelatedResource>        RelatedResources     = null,
                               IEnumerable<ExtendedGeoCoordinate>  RelatedLocations     = null,
                               String                              TimeZone             = null,
                               Hours                               OpeningTimes         = null,
                               ChargePointStatus?                  Status               = ChargePointStatus.Unknown,
                               IEnumerable<ChargePointSchedule>    ChargePointSchedule  = null,
                               String                              TelephoneNumber      = null,
                               IEnumerable<ParkingSpotInfo>        ParkingSpots         = null,
                               RestrictionTypes?                   Restrictions         = RestrictionTypes.Unknown,
                               Ratings                             Ratings              = null,
                               IEnumerable<String>                 UserInterfaceLang    = null,
                               TimeSpan?                           MaxReservation       = null)

        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),              "The given unique identification of an EVSE must not be null!");

            if (LocationId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(LocationId),          "The given unique identification of a charging location must not be null!");

            if (!LocationId_RegEx.IsMatch(LocationId))
                throw new ArgumentException("The given unique identification of a charging location is invalid!", nameof(LocationId));

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

            if (Location == GeneralLocationTypes.Unknown)
                throw new ArgumentNullException(nameof(Location),            "The given general location type must have at least one item!");

            if (AuthMethods == AuthMethodTypes.Unknown)
                throw new ArgumentNullException(nameof(AuthMethods),         "The given authentication methods must have at least one item!");

            if (Connectors == null || Connectors.Count() < 1)
                throw new ArgumentNullException(nameof(Connectors),          "The given enumeration of connectors must have at least one item!");

            if (ChargePointType == ChargePointTypes.Unknown)
                throw new ArgumentNullException(nameof(ChargePointType),     "The given charge point type is invalid!");


            if (TelephoneNumber.IsNotNullOrEmpty() && !TelephoneNumber_RegEx.IsMatch(LocationId))
                throw new ArgumentException("The given service hotline telephone number is invalid!", nameof(LocationId));

            #endregion

            this.EVSEId               = EVSEId;
            this.LocationId           = LocationId;
            this.LocationName         = LocationName;
            this.LocationNameLang     = LocationNameLang;
            this.ChargePointAddress   = ChargePointAddress;
            this.ChargePointLocation  = ChargePointLocation;
            this.Location             = Location;
            this.AuthMethods          = AuthMethods;
            this.Connectors           = Connectors;
            this.ChargePointType      = ChargePointType;

            this.Timestamp            = Timestamp            ?? new DateTime?();
            this.Images               = Images               ?? new EVSEImageURL[0];
            this.RelatedResources     = RelatedResources     ?? new RelatedResource[0];
            this.RelatedLocations     = RelatedLocations     ?? new ExtendedGeoCoordinate[0];
            this.TimeZone             = TimeZone.Trim();
            this.OpeningTimes         = OpeningTimes         ?? new Hours(ClosedCharging: true);
            this.Status               = Status               ?? new ChargePointStatus?();
            this.ChargePointSchedule  = ChargePointSchedule  ?? new ChargePointSchedule[0];
            this.TelephoneNumber      = TelephoneNumber.Trim();
            this.ParkingSpots         = ParkingSpots         ?? new ParkingSpotInfo[0];
            this.Restrictions         = Restrictions         ?? new RestrictionTypes?();
            this.Ratings              = Ratings;
            this.UserInterfaceLang    = UserInterfaceLang    ?? new String[0];
            this.MaxReservation       = MaxReservation       ?? new TimeSpan?();

        }

        #endregion


        #region AllRestrictions

        /// <summary>
        /// Return an enumeration of all (parking) restrictions.
        /// </summary>
        public IEnumerable<String> AllRestrictions

            => Restrictions.HasValue

                   ? Enum.GetValues(typeof(RestrictionTypes)).
                          Cast<RestrictionTypes>().
                          Where (restriction => Restrictions.Value.HasFlag(restriction)).
                          Select(restriction => ObjectMapper.AsText(restriction))

                   : new String[0];

        #endregion

        #region AllAuthMethods

        /// <summary>
        /// Return an enumeration of all authentication methods.
        /// </summary>
        public IEnumerable<String> AllAuthMethods

            => Enum.GetValues(typeof(AuthMethodTypes)).
                    Cast<AuthMethodTypes>().
                    Where (method => method.HasFlag(method)).
                    Select(method => ObjectMapper.AsText(method));

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:SetChargePointListRequest>
        //          <!--1 or more repetitions:-->
        //          <ns:chargePointInfoArray>
        //
        //             <ns:evseId>?</ns:evseId>
        //             <ns:locationId>?</ns:locationId>
        //
        //             <!--Optional:-->
        //             <ns:timestamp>
        //                <ns:DateTime>?</ns:DateTime>
        //             </ns:timestamp>
        //
        //             <ns:locationName>?</ns:locationName>
        //             <ns:locationNameLang>?</ns:locationNameLang>
        //
        //             <!--Zero or more repetitions:-->
        //             <ns:images>
        //
        //                <ns:uri>?</ns:uri>
        //
        //                <!--Optional:-->
        //                <ns:thumbUri>?</ns:thumbUri>
        //                <ns:class>?</ns:class>
        //                <ns:type>?</ns:type>
        //
        //                <!--Optional:-->
        //                <ns:width>?</ns:width>
        //
        //                <!--Optional:-->
        //                <ns:height>?</ns:height>
        //
        //             </ns:images>
        //
        //             <!--Zero or more repetitions:-->
        //             <ns:relatedResource>
        //                <ns:uri>?</ns:uri>
        //                <ns:class>?</ns:class>
        //             </ns:relatedResource>
        //
        //             <ns:chargePointAddress>
        //
        //                <!--Optional:-->
        //                <ns:houseNumber>?</ns:houseNumber>
        //
        //                <ns:address>?</ns:address>
        //                <ns:city>?</ns:city>
        //                <ns:zipCode>?</ns:zipCode>
        //                <ns:country>?</ns:country>
        //
        //             </ns:chargePointAddress>
        //
        //             <ns:chargePointLocation lat = "?" lon="?"/>
        //
        //             <!--Zero or more repetitions:-->
        //             <ns:relatedLocation lat = "?" lon="?" name="?" type="?"/>
        //
        //             <!--Optional:-->
        //             <ns:timeZone>?</ns:timeZone>
        //
        //             <!--Optional:-->
        //             <ns:openingTimes>
        //
        //                <!--You have a CHOICE of the next 2 items at this level-->
        //
        //                <!--1 to 7 repetitions:-->
        //                <ns:regularHours weekday = "?" periodBegin="?" periodEnd="?"/>
        //                <ns:twentyfourseven>?</ns:twentyfourseven>
        //
        //                <!--Zero or more repetitions:-->
        //                <ns:exceptionalOpenings>
        //                   <ns:periodBegin>
        //                      <ns:DateTime>?</ns:DateTime>
        //                   </ns:periodBegin>
        //                   <ns:periodEnd>
        //                      <ns:DateTime>?</ns:DateTime>
        //                   </ns:periodEnd>
        //                </ns:exceptionalOpenings>
        //
        //                <!--Zero or more repetitions:-->
        //                <ns:exceptionalClosings>
        //                   <ns:periodBegin>
        //                      <ns:DateTime>?</ns:DateTime>
        //                   </ns:periodBegin>
        //                   <ns:periodEnd>
        //                      <ns:DateTime>?</ns:DateTime>
        //                   </ns:periodEnd>
        //                </ns:exceptionalClosings>
        //
        //                <ns:closedCharging>?</ns:closedCharging>
        //
        //             </ns:openingTimes>
        //
        //             <!--Optional:-->
        //             <ns:status>
        //                <ns:ChargePointStatusType>?</ns:ChargePointStatusType>
        //             </ns:status>
        //
        //             <!--Zero or more repetitions:-->
        //             <ns:statusSchedule>
        //
        //                <ns:startDate>
        //                   <ns:DateTime>?</ns:DateTime>
        //                </ns:startDate>
        //
        //                <!--Optional:-->
        //                <ns:endDate>
        //                   <ns:DateTime>?</ns:DateTime>
        //                </ns:endDate>
        //
        //                <ns:status>
        //                   <ns:ChargePointStatusType>?</ns:ChargePointStatusType>
        //                </ns:status>
        //
        //             </ns:statusSchedule>
        //
        //             <!--Optional:-->
        //             <ns:telephoneNumber>?</ns:telephoneNumber>
        //
        //             <ns:location>
        //                <ns:GeneralLocationType>?</ns:GeneralLocationType>
        //             </ns:location>
        //
        //             <!--Zero or more repetitions:-->
        //             <ns:parkingSpot>
        //
        //                <ns:parkingId>?</ns:parkingId>
        //
        //                <!--Zero or more repetitions:-->
        //                <ns:restriction>
        //                   <ns:RestrictionType>?</ns:RestrictionType>
        //                </ns:restriction>
        //
        //                <!--Optional:-->
        //                <ns:floorLevel>?</ns:floorLevel>
        //
        //                <!--Optional:-->
        //                <ns:parkingSpotNumber>?</ns:parkingSpotNumber>
        //
        //             </ns:parkingSpot>
        //
        //             <!--Zero or more repetitions:-->
        //             <ns:restriction>
        //                <ns:RestrictionType>?</ns:RestrictionType>
        //             </ns:restriction>
        //
        //             <!--1 or more repetitions:-->
        //             <ns:authMethods>
        //                <ns:AuthMethodType>?</ns:AuthMethodType>
        //             </ns:authMethods>
        //
        //             <!--1 or more repetitions:-->
        //             <ns:connectors>
        //
        //                <ns:connectorStandard>
        //                   <ns:ConnectorStandard>?</ns:ConnectorStandard>
        //                </ns:connectorStandard>
        //
        //                <ns:connectorFormat>
        //                   <ns:ConnectorFormat>?</ns:ConnectorFormat>
        //                </ns:connectorFormat>
        //
        //                <!--Optional:-->
        //                <ns:tariffId>?</ns:tariffId>
        //
        //             </ns:connectors>
        //
        //             <ns:chargePointType>?</ns:chargePointType>
        //
        //             <!--Optional:-->
        //             <ns:ratings>
        //
        //                <ns:maximumPower>?</ns:maximumPower>
        //
        //                <!--Optional:-->
        //                <ns:guaranteedPower>?</ns:guaranteedPower>
        //
        //                <!--Optional:-->
        //                <ns:nominalVoltage>?</ns:nominalVoltage>
        //
        //             </ns:ratings>
        //
        //             <!--Zero or more repetitions:-->
        //             <ns:userInterfaceLang>?</ns:userInterfaceLang>
        //
        //             <!--Optional:-->
        //             <ns:maxReservation>?</ns:maxReservation>
        //
        //          </ns:chargePointInfoArray>
        //       </ns:SetChargePointListRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(ChargePointInfoXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of static OCHP charge point data.
        /// </summary>
        /// <param name="ChargePointInfoXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ChargePointInfo Parse(XElement             ChargePointInfoXML,
                                            OnExceptionDelegate  OnException  = null)
        {

            ChargePointInfo _ChargePointInfo;

            if (TryParse(ChargePointInfoXML, out _ChargePointInfo, OnException))
                return _ChargePointInfo;

            return null;

        }

        #endregion

        #region (static) Parse(ChargePointInfoText, OnException = null)

        /// <summary>
        /// Parse the given text representation of static OCHP charge point data.
        /// </summary>
        /// <param name="ChargePointInfoText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ChargePointInfo Parse(String               ChargePointInfoText,
                                            OnExceptionDelegate  OnException  = null)
        {

            ChargePointInfo _ChargePointInfo;

            if (TryParse(ChargePointInfoText, out _ChargePointInfo, OnException))
                return _ChargePointInfo;

            return null;

        }

        #endregion

        #region (static) TryParse(ChargePointInfoXML,  out ChargePointInfo, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of static OCHP charge point data.
        /// </summary>
        /// <param name="ChargePointInfoXML">The XML to parse.</param>
        /// <param name="ChargePointInfo">The parsed static charge point data.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             ChargePointInfoXML,
                                       out ChargePointInfo  ChargePointInfo,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                ChargePointInfo = new ChargePointInfo(

                                      ChargePointInfoXML.MapValueOrFail       (OCHPNS.Default + "evseId",
                                                                               EVSE_Id.Parse,
                                                                               "The XML element 'evseId' is invalid or missing!"),

                                      ChargePointInfoXML.ElementValueOrFail   (OCHPNS.Default + "locationId"),

                                      ChargePointInfoXML.ElementValueOrFail   (OCHPNS.Default + "locationName"),
                                      ChargePointInfoXML.ElementValueOrFail   (OCHPNS.Default + "locationNameLang"),

                                      ChargePointInfoXML.MapElementOrFail     (OCHPNS.Default + "chargePointAddress",
                                                                               "The XML element 'chargePointAddress' is invalid or missing!",
                                                                               Address.Parse,
                                                                               OnException),

                                      ChargePointInfoXML.MapElementOrFail     (OCHPNS.Default + "chargePointLocation",
                                                                               "The XML element 'chargePointLocation' is invalid or missing!",
                                                                               ObjectMapper.ParseGeoCoordinate,
                                                                               OnException),

                                      ChargePointInfoXML.MapValueOrFail       (OCHPNS.Default + "location",
                                                                               OCHPNS.Default + "GeneralLocationType",
                                                                               ObjectMapper.AsGeneralLocationType,
                                                                               "The XML element 'chargePointAddress' is invalid or missing!"),

                                      ChargePointInfoXML.MapEnumValuesOrFail  (OCHPNS.Default + "authMethods",
                                                                               "The XML element 'authMethods' is invalid or missing!",
                                                                               OCHPNS.Default + "AuthMethodType",
                                                                               ObjectMapper.AsAuthMethodType),

                                      ChargePointInfoXML.MapElementsOrFail    (OCHPNS.Default + "connectors",
                                                                               "The XML element 'connectors' is invalid or missing!",
                                                                               ConnectorType.Parse,
                                                                               OnException),

                                      ChargePointInfoXML.MapValueOrFail       (OCHPNS.Default + "chargePointType",
                                                                               "The XML element 'chargePointType' is invalid or missing!",
                                                                               ObjectMapper.AsChargePointType),



                                      ChargePointInfoXML.MapValueOrNullable   (OCHPNS.Default + "timestamp",
                                                                               OCHPNS.Default + "DateTime",
                                                                               DateTime.Parse),

                                      ChargePointInfoXML.MapElements          (OCHPNS.Default + "images",
                                                                               EVSEImageURL.Parse,
                                                                               OnException),

                                      ChargePointInfoXML.MapElements          (OCHPNS.Default + "relatedResource",
                                                                               RelatedResource.Parse,
                                                                               OnException),

                                      ChargePointInfoXML.MapElements          (OCHPNS.Default + "relatedLocation",
                                                                               ExtendedGeoCoordinate.Parse,
                                                                               OnException),

                                      ChargePointInfoXML.ElementValueOrDefault(OCHPNS.Default + "timeZone"),

                                      ChargePointInfoXML.MapElement           (OCHPNS.Default + "openingTimes",
                                                                               Hours.Parse),

                                      ChargePointInfoXML.MapValueOrNullable   (OCHPNS.Default + "status",
                                                                               OCHPNS.Default + "ChargePointStatusType",
                                                                               ObjectMapper.AsChargePointStatus),

                                      ChargePointInfoXML.MapElements          (OCHPNS.Default + "statusSchedule",
                                                                               OCHPv1_4.ChargePointSchedule.Parse,
                                                                               OnException),

                                      ChargePointInfoXML.ElementValueOrDefault(OCHPNS.Default + "telephoneNumber"),

                                      ChargePointInfoXML.MapElements          (OCHPNS.Default + "parkingSpot",
                                                                               ParkingSpotInfo.Parse,
                                                                               OnException),

                                      ChargePointInfoXML.MapValueOrNullable   (OCHPNS.Default + "restriction",
                                                                               OCHPNS.Default + "RestrictionTypes",
                                                                               ObjectMapper.AsRestrictionType),

                                      ChargePointInfoXML.MapElement           (OCHPNS.Default + "ratings",
                                                                               Ratings.Parse),

                                      ChargePointInfoXML.ElementValues        (OCHPNS.Default + "relatedLocation"),

                                      ChargePointInfoXML.MapValueOrNullable   (OCHPNS.Default + "maxReservation",
                                                                               value => TimeSpan.FromSeconds(Double.Parse(value)))

                                  );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, ChargePointInfoXML, e);

                ChargePointInfo = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ChargePointInfoText, out ChargePointInfo, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of static OCHP charge point data.
        /// </summary>
        /// <param name="ChargePointInfoText">The text to parse.</param>
        /// <param name="ChargePointInfo">The parsed static charge point data.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               ChargePointInfoText,
                                       out ChargePointInfo  ChargePointInfo,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ChargePointInfoText).Root,
                             out ChargePointInfo,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, ChargePointInfoText, e);
            }

            ChargePointInfo = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">An alternative XML element name [default: "OCHPNS:chargePointInfoArray"]</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OCHPNS.Default + "chargePointInfoArray",

                   new XElement(OCHPNS.Default + "evseId", EVSEId.ToString()),
                   new XElement(OCHPNS.Default + "locationId", LocationId),

                   Timestamp.HasValue
                       ? new XElement(OCHPNS.Default + "timestamp",
                             new XElement(OCHPNS.Default + "DateTime", Timestamp.Value)
                         )
                       : null,

                   new XElement(OCHPNS.Default + "locationName", LocationName),
                   new XElement(OCHPNS.Default + "locationNameLang", LocationNameLang),

                   Images != null
                       ? Images.SafeSelect(image => image.ToXML())
                       : null,

                   RelatedResources != null
                       ? RelatedResources.SafeSelect(resource => resource.ToXML())
                       : null,

                   ChargePointAddress.ToXML(),

                   new XElement(OCHPNS.Default + "chargePointLocation",
                       new XAttribute(OCHPNS.Default + "lat", ChargePointLocation.Latitude.Value),
                       new XAttribute(OCHPNS.Default + "lon", ChargePointLocation.Longitude.Value)
                   ),

                   RelatedLocations != null
                       ? RelatedLocations.SafeSelect(location => location.ToXML())
                       : null,

                   TimeZone.IsNotNullOrEmpty()
                       ? new XElement(OCHPNS.Default + "timeZone", TimeZone)
                       : null,

                   OpeningTimes.ToXML(),

                   Status.HasValue
                       ? new XElement(OCHPNS.Default + "status",
                             new XElement(OCHPNS.Default + "status", TimeZone)
                         )
                       : null,

                   ChargePointSchedule != null
                       ? ChargePointSchedule.SafeSelect(schedule => "")
                       : null,

                   TelephoneNumber.IsNotNullOrEmpty()
                       ? new XElement(OCHPNS.Default + "telephoneNumber", TelephoneNumber)
                       : null,

                   new XElement(OCHPNS.Default + "location",
                       new XElement(OCHPNS.Default + "GeneralLocationType", ObjectMapper.AsText(Location))
                   ),

                   ParkingSpots != null
                       ? ParkingSpots.SafeSelect(parkingspot => parkingspot.ToXML())
                       : null,

                   Restrictions.HasValue
                       ? AllRestrictions.Select(restriction => new XElement(OCHPNS.Default + "restriction",
                                                                   new XElement(OCHPNS.Default + "RestrictionType", restriction)
                                                               ))
                       : null,

                   AuthMethods != AuthMethodTypes.Unknown
                       ? AllAuthMethods.Select(method => new XElement(OCHPNS.Default + "authMethods",
                                                                  new XElement(OCHPNS.Default + "AuthMethodType", method)
                                                              ))
                       : null,

                   Connectors.SafeSelect(connector => connector.ToXML()),

                   new XElement(OCHPNS.Default + "chargePointType", ObjectMapper.AsText(ChargePointType)),

                   Ratings != null
                       ? Ratings.ToXML()
                       : null,

                   UserInterfaceLang != null
                       ? UserInterfaceLang.Select(language => new XElement(OCHPNS.Default + "userInterfaceLang", language))
                       : null,

                   MaxReservation.HasValue
                       ? new XElement(OCHPNS.Default + "maxReservation", MaxReservation.Value.TotalMinutes)
                       : null

               );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => EVSEId.ToString();

        #endregion

    }

}
