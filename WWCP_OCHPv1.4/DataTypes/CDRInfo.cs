/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// Contains all information concerning a charge data record.
    /// </summary>
    public class CDRInfo
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charge data record.
        /// </summary>
        public CDR_Id                  CDRId                { get; }

        /// <summary>
        /// The unique identification of the EVSE of the charging process.
        /// </summary>
        public EVSE_Id                 EVSEId               { get; }

        /// <summary>
        /// Utilized token for this charging session.
        /// </summary>
        public EMT_Id                  EMTId                { get; }

        /// <summary>
        /// Identifies a customer in the electric mobility charging context.
        /// </summary>
        public Contract_Id             ContractId           { get; }

        /// <summary>
        /// References a live authorisation request to the clearing house. 
        /// </summary>
        public LiveAuth_Id             LiveAuthId           { get; }

        /// <summary>
        /// Current status of the CDR. Must be set to "new" by the issuing CMS.
        /// Shall not be changed by any partner but only by the CHS.
        /// </summary>
        public CDRStatus               Status               { get; }

        /// <summary>
        /// Start date and time of the charge session (login with the RFID badge).
        /// Local time of the charge point is used.
        /// </summary>
        public DateTime                StartDateTime        { get; }

        /// <summary>
        /// End date and time of the charge session (log-off with the RFID badge
        /// or physical disconnect).
        /// Must be set in the local time of the charge point.
        /// </summary>
        public DateTime                EndDateTime          { get; }

        /// <summary>
        /// Duration of the charge session, whenever it is more
        /// than the time span between its start- and endtime, e.g.
        /// caused by a tariff granularity of 15 minutes.
        /// </summary>
        public TimeSpan?               Duration             { get; }

        /// <summary>
        /// Optional address information of the charging point.
        /// </summary>
        public Address                 ChargePointAddress   { get; }

        /// <summary>
        /// The type of the charge point "AC" or "DC".
        /// </summary>
        public ChargePointTypes        ChargePointType      { get; }

        /// <summary>
        /// Type of the utilized socket or connector.
        /// </summary>
        public ConnectorType           ConnectorType        { get; }

        /// <summary>
        /// Maximum available power at the socket in kilowatts. Example: "3.7", "11", "22".
        /// </summary>
   //     public Single                  MaxSocketPower       { get; } //Note: Seems to be a bug in the documentation!

        /// <summary>
        /// Written identification number of the physical energy meter, provided by the manufacturer.
        /// </summary>
        public String                  MeterId              { get; }

        /// <summary>
        /// An enumeration of periods per item on the bill.
        /// </summary>
        public IEnumerable<CDRPeriod>  ChargingPeriods      { get; }

        /// <summary>
        /// Total cost for the entire charging process. Should always equal the sum of the individual periodCosts.
        /// </summary>
        public Single?                 TotalCosts           { get; }

        /// <summary>
        /// The displayed and charged currency. Defined in ISO 4217 - Table A.1, alphabetic list.
        /// </summary>
        public Currency                Currency             { get; }


        public Ratings                 Ratings              { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charge data record information object.
        /// </summary>
        /// <param name="CDRId">The unique identification of the charge data record.</param>
        /// <param name="EVSEId">The unique identification of the EVSE of the charging process.</param>
        /// <param name="EMTId">Utilized token for this charging session.</param>
        /// <param name="ContractId">Identifies a customer in the electric mobility charging context.</param>
        /// <param name="Status">Current status of the CDR. Must be set to "new" by the issuing CMS. Shall not be changed by any partner but only by the CHS.</param>
        /// <param name="StartDateTime">Start date and time of the charge session (login with the RFID badge). Local time of the charge point is used.</param>
        /// <param name="EndDateTime">End date and time of the charge session (log-off with the RFID badge or physical disconnect). Must be set in the local time of the charge point.</param>
        /// <param name="Duration">Duration of the charge session, whenever it is more than the time span between its start- and endtime, e.g. caused by a tariff granularity of 15 minutes.</param>
        /// <param name="ChargePointAddress">Optional address information of the charging point.</param>
        /// <param name="ChargePointType">The type of the charge point "AC" or "DC".</param>
        /// <param name="ConnectorType">Type of the utilized socket or connector.</param>
        /// <param name="MeterId">Written identification number of the physical energy meter, provided by the manufacturer.</param>
        /// <param name="ChargingPeriods">An enumeration of periods per item on the bill.</param>
        /// <param name="TotalCosts">Total costs for the entire charging process. Should always equal the sum of the individual periodCosts.</param>
        /// <param name="Currency">The displayed and charged currency. Defined in ISO 4217 - Table A.1, alphabetic list.</param>
        public CDRInfo(CDR_Id                  CDRId,
                       EMT_Id                  EMTId,
                       Contract_Id             ContractId,

                       EVSE_Id                 EVSEId,
                       ChargePointTypes        ChargePointType,
                       ConnectorType           ConnectorType,

                       CDRStatus               Status,
                       DateTime                StartDateTime,
                       DateTime                EndDateTime,
                       IEnumerable<CDRPeriod>  ChargingPeriods,
                       Currency                Currency,

                       Address                 ChargePointAddress,

                       TimeSpan?               Duration            = null,
                       Ratings                 Ratings             = null,
                       String                  MeterId             = null,
                       Single?                 TotalCosts          = null)

        {

            #region Initial checks

            if (ChargePointType == ChargePointTypes.Unknown)
                throw new ArgumentNullException(nameof(ChargePointType),  "The given charge point type information must not be null or empty!");

            if (ConnectorType == null)
                throw new ArgumentNullException(nameof(ConnectorType),    "The given charge point connector type must not be null!");

            if (ChargingPeriods == null || !ChargingPeriods.Any())
                throw new ArgumentNullException(nameof(ChargingPeriods),  "The given enumeration of charge detail record periods must not be null or empty!");

            #endregion

            this.CDRId               = CDRId;
            this.EMTId               = EMTId;
            this.ContractId          = ContractId;
   //         this.LiveAuthId          = LiveAuthId;

            this.EVSEId              = EVSEId;
            this.ChargePointType     = ChargePointType;
            this.ConnectorType       = ConnectorType;

            this.Status              = Status;
            this.StartDateTime       = StartDateTime;
            this.EndDateTime         = EndDateTime;
            this.ChargingPeriods     = ChargingPeriods;
            this.Currency            = Currency;

            this.Duration            = Duration;
            this.ChargePointAddress  = ChargePointAddress;
   //         this.MaxSocketPower      = MaxSocketPower;        //Note: Seems to be a bug in the documentation!
            this.Ratings             = Ratings;
            this.MeterId             = MeterId;
            this.TotalCosts          = TotalCosts;

        }

        #endregion


        #region Documentation

        // <ns:cdrInfoArray>
        //
        //    <ns:CdrId>?</ns:CdrId>
        //    <ns:evseId>?</ns:evseId>
        //
        //    <ns:emtId representation = "plain" >
        //       < ns:instance>?</ns:instance>
        //       <ns:tokenType>?</ns:tokenType>
        //       <!--Optional:-->
        //       <ns:tokenSubType>?</ns:tokenSubType>
        //    </ns:emtId>
        //
        //    <ns:contractId>?</ns:contractId>
        //
        //    <ns:status>
        //       <ns:CdrStatusType>?</ns:CdrStatusType>
        //    </ns:status>
        //
        //    <ns:startDateTime>
        //       <ns:LocalDateTime>?</ns:LocalDateTime>
        //    </ns:startDateTime>
        //
        //    <ns:endDateTime>
        //       <ns:LocalDateTime>?</ns:LocalDateTime>
        //    </ns:endDateTime>
        //
        //    <!--Optional:-->
        //    <ns:duration>?</ns:duration>
        //
        //    <ns:chargePointAddress>
        //       <!--Optional:-->
        //       <ns:houseNumber>?</ns:houseNumber>
        //       <ns:address>?</ns:address>
        //       <ns:city>?</ns:city>
        //       <ns:zipCode>?</ns:zipCode>
        //       <ns:country>?</ns:country>
        //    </ns:chargePointAddress>
        //
        //    <ns:chargePointType>?</ns:chargePointType>
        //
        //    <ns:connectorType>
        //       <ns:connectorStandard>
        //          <ns:ConnectorStandard>?</ns:ConnectorStandard>
        //       </ns:connectorStandard>
        //       <ns:connectorFormat>
        //          <ns:ConnectorFormat>?</ns:ConnectorFormat>
        //       </ns:connectorFormat>
        //       <!--Optional:-->
        //       <ns:tariffId>?</ns:tariffId>
        //    </ns:connectorType>
        //
        //    <!--Optional:-->
        //    <ns:ratings>
        //       <ns:maximumPower>?</ns:maximumPower>
        //       <!--Optional:-->
        //       <ns:guaranteedPower>?</ns:guaranteedPower>
        //       <!--Optional:-->
        //       <ns:nominalVoltage>?</ns:nominalVoltage>
        //    </ns:ratings>
        //
        //    <!--Optional:-->
        //    <ns:meterId>?</ns:meterId>
        //
        //    <!--1 or more repetitions:-->
        //    <ns:chargingPeriods>
        //
        //       <ns:startDateTime>
        //          <ns:LocalDateTime>?</ns:LocalDateTime>
        //       </ns:startDateTime>
        //
        //       <ns:endDateTime>
        //          <ns:LocalDateTime>?</ns:LocalDateTime>
        //       </ns:endDateTime>
        //
        //       <ns:billingItem>
        //          <ns:BillingItemType>?</ns:BillingItemType>
        //       </ns:billingItem>
        //
        //       <ns:billingValue>?</ns:billingValue>
        //       <ns:itemPrice>?</ns:itemPrice>
        //
        //       <!--Optional:-->
        //       <ns:periodCost>?</ns:periodCost>
        //
        //       <!--Optional:-->
        //       <ns:taxrate>?</ns:taxrate>
        //
        //    </ns:chargingPeriods>
        //
        //    <!--Optional:-->
        //    <ns:totalCost>?</ns:totalCost>
        //    <ns:currency>?</ns:currency>
        //
        // </ns:cdrInfoArray>

        #endregion

        #region (static) Parse(CDRInfoXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP charge data record.
        /// </summary>
        /// <param name="CDRInfoXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static CDRInfo Parse(XElement             CDRInfoXML,
                                    OnExceptionDelegate  OnException  = null)
        {

            CDRInfo _CDRInfo;

            if (TryParse(CDRInfoXML, out _CDRInfo, OnException))
                return _CDRInfo;

            return null;

        }

        #endregion

        #region (static) Parse(CDRInfoText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP charge data record
        /// </summary>
        /// <param name="CDRInfoText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static CDRInfo Parse(String               CDRInfoText,
                                    OnExceptionDelegate  OnException  = null)
        {

            CDRInfo _CDRInfo;

            if (TryParse(CDRInfoText, out _CDRInfo, OnException))
                return _CDRInfo;

            return null;

        }

        #endregion

        #region (static) TryParse(CDRInfoXML,  out CDRInfo, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP charge data record
        /// </summary>
        /// <param name="CDRInfoXML">The XML to parse.</param>
        /// <param name="CDRInfo">The parsed charge data record.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             CDRInfoXML,
                                       out CDRInfo          CDRInfo,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                CDRInfo = new CDRInfo(

                              CDRInfoXML.MapValueOrFail       (OCHPNS.Default + "CdrId",
                                                               CDR_Id.Parse),

                              CDRInfoXML.MapValueOrFail       (OCHPNS.Default + "emtId",
                                                               EMT_Id.Parse),

                              CDRInfoXML.MapValueOrFail       (OCHPNS.Default + "contractId",
                                                               Contract_Id.Parse),


                              CDRInfoXML.MapValueOrFail       (OCHPNS.Default + "evseId",
                                                               EVSE_Id.Parse),

                              CDRInfoXML.MapValueOrFail       (OCHPNS.Default + "chargePointType",
                                                               XML_IO.AsChargePointType),

                              CDRInfoXML.MapElementOrFail     (OCHPNS.Default + "connectorType",
                                                               ConnectorType.Parse),


                              CDRInfoXML.MapValueOrFail       (OCHPNS.Default + "status",
                                                               OCHPNS.Default + "CdrStatusType",
                                                               XML_IO.AsCDRStatus),

                              CDRInfoXML.MapValueOrFail       (OCHPNS.Default + "startDateTime",
                                                               OCHPNS.Default + "LocalDateTime",
                                                               DateTime.Parse),

                              CDRInfoXML.MapValueOrFail       (OCHPNS.Default + "endDateTime",
                                                               OCHPNS.Default + "LocalDateTime",
                                                               DateTime.Parse),

                              CDRInfoXML.MapElementsOrFail    (OCHPNS.Default + "chargingPeriods",
                                                               CDRPeriod.Parse),

                              CDRInfoXML.MapValueOrNull       (OCHPNS.Default + "currency",
                                                               Currency.Parse),


                              CDRInfoXML.MapElementOrFail     (OCHPNS.Default + "chargePointAddress",
                                                               Address.Parse),

                              CDRInfoXML.MapValueOrNullable   (OCHPNS.Default + "duration",
                                                               TimeSpan.Parse),

                              CDRInfoXML.MapElement           (OCHPNS.Default + "chargingPeriods",
                                                               Ratings.Parse),

                              CDRInfoXML.ElementValueOrDefault(OCHPNS.Default + "meterId"),

                              CDRInfoXML.MapValueOrNullable   (OCHPNS.Default + "totalCost",
                                                               Single.Parse)

                          );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, CDRInfoXML, e);

                CDRInfo = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(CDRInfoText, out CDRInfo, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP charge data record.
        /// </summary>
        /// <param name="CDRInfoText">The text to parse.</param>
        /// <param name="CDRInfo">The parsed charge data record.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               CDRInfoText,
                                       out CDRInfo          CDRInfo,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(CDRInfoText).Root,
                             out CDRInfo,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, CDRInfoText, e);
            }

            CDRInfo = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">An alternative XML element name [default: "OCHPNS:cdrInfoArray"]</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OCHPNS.Default + "cdrInfoArray",

                   new XElement(OCHPNS.Default + "CdrId",               CDRId.     ToString()),
                   new XElement(OCHPNS.Default + "evseId",              EVSEId.    ToString()),
                   EMTId.ToXML(),
                   new XElement(OCHPNS.Default + "contractId",          ContractId.ToString()),

                   new XElement(OCHPNS.Default + "status",
                       new XElement(OCHPNS.Default + "CdrStatusType",   XML_IO.AsText(Status))
                   ),

                   new XElement(OCHPNS.Default + "startDateTime",
                       new XElement(OCHPNS.Default + "LocalDateTime",   StartDateTime.ToIso8601WithOffset(false))
                   ),

                   new XElement(OCHPNS.Default + "endDateTime",
                       new XElement(OCHPNS.Default + "LocalDateTime",   EndDateTime.ToIso8601WithOffset(false))
                   ),

                   Duration.HasValue
                       ? new XElement(OCHPNS.Default + "duration",      String.Concat(Duration.Value.Hours.  ToString("D3"), ":",
                                                                                      Duration.Value.Minutes.ToString("D2"), ":",
                                                                                      Duration.Value.Seconds.ToString("D2")))
                       : null,

                   ChargePointAddress?.ToXML(),

                   new XElement(OCHPNS.Default + "chargePointType",     XML_IO.AsText(ChargePointType)),

                   ConnectorType.ToXML(OCHPNS.Default + "connectorType"),

                   MeterId.IsNotNullOrEmpty()
                       ? new XElement(OCHPNS.Default + "meterId",       MeterId)
                       : null,

                   ChargingPeriods.Select(period => period.ToXML(OCHPNS.Default + "chargingPeriods")),

                   TotalCosts.HasValue
                       ? new XElement(OCHPNS.Default + "totalCost",     TotalCosts.Value)
                       : null,

                   new XElement(OCHPNS.Default + "currency",            Currency.ISOCode)

               );

        #endregion


        #region Operator overloading

        #region Operator == (CDRInfo1, CDRInfo2)

        /// <summary>
        /// Compares two charge data records for equality.
        /// </summary>
        /// <param name="CDRInfo1">A charge data record.</param>
        /// <param name="CDRInfo2">Another charge data record.</param>
        /// <returns>True if both match; False otherwise.</returns
        public static Boolean operator == (CDRInfo CDRInfo1, CDRInfo CDRInfo2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(CDRInfo1, CDRInfo2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) CDRInfo1 == null) || ((Object) CDRInfo2 == null))
                return false;

            return CDRInfo1.Equals(CDRInfo2);

        }

        #endregion

        #region Operator != (CDRInfo1, CDRInfo2)

        /// <summary>
        /// Compares two charge data records for inequality.
        /// </summary>
        /// <param name="CDRInfo1">A charge data record.</param>
        /// <param name="CDRInfo2">Another charge data record.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (CDRInfo CDRInfo1, CDRInfo CDRInfo2)

            => !(CDRInfo1 == CDRInfo2);

        #endregion

        #endregion

        #region IEquatable<CDRInfo> Members

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

            // Check if the given object is a charge data record.
            var CDRInfo = Object as CDRInfo;
            if ((Object) CDRInfo == null)
                return false;

            return this.Equals(CDRInfo);

        }

        #endregion

        #region Equals(CDRInfo)

        /// <summary>
        /// Compares two charge data records for equality.
        /// </summary>
        /// <param name="CDRInfo">An charge data record to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(CDRInfo CDRInfo)
        {

            if ((Object) CDRInfo == null)
                return false;

            return CDRId.     Equals(CDRInfo.CDRId)  &&
                   EVSEId.    Equals(CDRInfo.EVSEId) &&
                   EMTId.     Equals(CDRInfo.EMTId)  &&
                   ContractId.Equals(CDRInfo.ContractId);

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

                return CDRId.     GetHashCode() * 23 ^
                       EVSEId.    GetHashCode() * 17 ^
                       EMTId.     GetHashCode() * 11 ^
                       ContractId.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(CDRId.     ToString(), " / ",
                             EVSEId.    ToString(), " / ",
                             EMTId.     ToString(), " / ",
                             ContractId.ToString());

        #endregion

    }

}
