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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHPdirect inform provider request.
    /// </summary>
    public class InformProviderRequest : ARequest<InformProviderRequest>
    {

        #region Properties

        /// <summary>
        /// The operation that triggered the operator to send this message.
        /// </summary>
        public DirectMessages          DirectMessage       { get; }

        /// <summary>
        /// The uqniue EVSE identification of the charge point which is used for this charging process.
        /// </summary>
        public EVSE_Id                 EVSEId              { get; }

        /// <summary>
        /// The current contract identification using the charge point.
        /// </summary>
        public Contract_Id             ContractId          { get; }

        /// <summary>
        /// The session identification of the direct charging process.
        /// </summary>
        public Direct_Id               DirectId            { get; }

        /// <summary>
        /// On success the timeout for this session.
        /// </summary>
        public DateTime?               SessionTimeoutAt    { get; }

        /// <summary>
        /// Current state of charge of the connected EV in percent.
        /// </summary>
        public Single?                 StateOfCharge       { get; }

        /// <summary>
        /// Maximum authorised power in kW.
        /// </summary>
        public Single?                 MaxPower            { get; }

        /// <summary>
        /// Maximum authorised current in A.
        /// </summary>
        public Single?                 MaxCurrent          { get; }

        /// <summary>
        /// Marks an AC-charging session to be limited to one-phase charging.
        /// </summary>
        public Boolean?                OnePhase            { get; }

        /// <summary>
        /// Maximum authorised energy in kWh.
        /// </summary>
        public Single?                 MaxEnergy           { get; }

        /// <summary>
        /// Minimum required energy in kWh.
        /// </summary>
        public Single?                 MinEnergy           { get; }

        /// <summary>
        /// Scheduled time of departure.
        /// </summary>
        public DateTime?               Departure           { get; }

        /// <summary>
        /// The currently supplied power limit in kWs (in case of load management).
        /// </summary>
        public Single?                 CurrentPower        { get; }

        /// <summary>
        /// The overall amount of energy in kWhs transferred during this charging process.
        /// </summary>
        public Single?                 ChargedEnergy       { get; }

        /// <summary>
        /// The current meter value as displayed on the meter with corresponding timestamp to enable displaying it to the user.
        /// </summary>
        public Timestamped<Single>?    MeterReading        { get; }

        /// <summary>
        /// Can be used to transfer billing information to the provider in near real time.
        /// </summary>
        public IEnumerable<CDRPeriod>  ChargingPeriods     { get; }

        /// <summary>
        /// The total cost of the charging process that will be billed by the operator up to this point.
        /// </summary>
        public Single?                 CurrentCost         { get; }

        /// <summary>
        /// The displayed and charged currency. Defined in ISO 4217 - Table A.1, alphabetic list.
        /// </summary>
        public Currency                Currency            { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHPdirect InformProvider XML/SOAP request.
        /// </summary>
        /// <param name="DirectMessage">The operation that triggered the operator to send this message.</param>
        /// <param name="EVSEId">The uqniue EVSE identification of the charge point which is used for this charging process.</param>
        /// <param name="ContractId">The current contract identification using the charge point.</param>
        /// <param name="DirectId">The session identification of the direct charging process.</param>
        /// 
        /// <param name="SessionTimeoutAt">On success the timeout for this session.</param>
        /// <param name="StateOfCharge">Current state of charge of the connected EV in percent.</param>
        /// <param name="MaxPower">Maximum authorised power in kW.</param>
        /// <param name="MaxCurrent">Maximum authorised current in A.</param>
        /// <param name="OnePhase">Marks an AC-charging session to be limited to one-phase charging.</param>
        /// <param name="MaxEnergy">Maximum authorised energy in kWh.</param>
        /// <param name="MinEnergy">Minimum required energy in kWh.</param>
        /// <param name="Departure">Scheduled time of departure.</param>
        /// <param name="CurrentPower">The currently supplied power limit in kWs (in case of load management).</param>
        /// <param name="ChargedEnergy">The overall amount of energy in kWhs transferred during this charging process.</param>
        /// <param name="MeterReading">The current meter value as displayed on the meter with corresponding timestamp to enable displaying it to the user.</param>
        /// <param name="ChargingPeriods">Can be used to transfer billing information to the provider in near real time.</param>
        /// <param name="CurrentCost">The total cost of the charging process that will be billed by the operator up to this point.</param>
        /// <param name="Currency">The displayed and charged currency. Defined in ISO 4217 - Table A.1, alphabetic list.</param>
        public InformProviderRequest(DirectMessages          DirectMessage,
                                     EVSE_Id                 EVSEId,
                                     Contract_Id             ContractId,
                                     Direct_Id               DirectId,

                                     DateTime?               SessionTimeoutAt  = null,
                                     Single?                 StateOfCharge     = null,
                                     Single?                 MaxPower          = null,
                                     Single?                 MaxCurrent        = null,
                                     Boolean?                OnePhase          = null,
                                     Single?                 MaxEnergy         = null,
                                     Single?                 MinEnergy         = null,
                                     DateTime?               Departure         = null,
                                     Single?                 CurrentPower      = null,
                                     Single?                 ChargedEnergy     = null,
                                     Timestamped<Single>?    MeterReading      = null,
                                     IEnumerable<CDRPeriod>  ChargingPeriods   = null,
                                     Single?                 CurrentCost       = null,
                                     Currency                Currency          = null)
        {

            #region Initial checks

            if (DirectId   == null)
                throw new ArgumentNullException(nameof(DirectId),    "The given identification of an direct charging process must not be null!");

            #endregion

            this.DirectMessage     = DirectMessage;
            this.EVSEId            = EVSEId;
            this.ContractId        = ContractId;
            this.DirectId          = DirectId;

            this.SessionTimeoutAt  = SessionTimeoutAt ?? new DateTime?();
            this.StateOfCharge     = StateOfCharge    ?? new Single?();
            this.MaxPower          = MaxPower         ?? new Single?();
            this.MaxCurrent        = MaxCurrent       ?? new Single?();
            this.OnePhase          = OnePhase         ?? new Boolean?();
            this.MaxEnergy         = MaxEnergy        ?? new Single?();
            this.MinEnergy         = MinEnergy        ?? new Single?();
            this.Departure         = Departure        ?? new DateTime?();
            this.CurrentPower      = CurrentPower     ?? new Single?();
            this.ChargedEnergy     = ChargedEnergy    ?? new Single?();
            this.MeterReading      = MeterReading     ?? new Timestamped<Single>?();
            this.ChargingPeriods   = ChargingPeriods;
            this.CurrentCost       = CurrentCost      ?? new Single?();
            this.Currency          = Currency;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:InformProviderMessage>
        //
        //          <ns:message>
        //             <ns:message>?</ns:message>
        //          </ns:message>
        //
        //          <ns:evseId>?</ns:evseId>
        //          <ns:contractId>?</ns:contractId>
        //          <ns:directId>?</ns:directId>
        //
        //          <!--Optional:-->
        //          <ns:ttl>
        //             <ns:DateTime>?</ns:DateTime>
        //          </ns:ttl>
        //
        //          <!--Optional:-->
        //          <ns:stateOfCharge>?</ns:stateOfCharge>
        //
        //          <!--Optional:-->
        //          <ns:maxPower>?</ns:maxPower>
        //
        //          <!--Optional:-->
        //          <ns:maxCurrent>?</ns:maxCurrent>
        //
        //          <!--Optional:-->
        //          <ns:onePhase>?</ns:onePhase>
        //
        //          <!--Optional:-->
        //          <ns:maxEnergy>?</ns:maxEnergy>
        //
        //          <!--Optional:-->
        //          <ns:minEnergy>?</ns:minEnergy>
        //
        //          <!--Optional:-->
        //          <ns:departure>
        //             <ns:DateTime>?</ns:DateTime>
        //          </ns:departure>
        //
        //          <!--Optional:-->
        //          <ns:currentPower>?</ns:currentPower>
        //
        //          <!--Optional:-->
        //          <ns:chargedEnergy>?</ns:chargedEnergy>
        //
        //          <!--Optional:-->
        //          <ns:meterReading>
        //             <ns:meterValue>?</ns:meterValue>
        //             <ns:meterTime>
        //                <ns:LocalDateTime>?</ns:LocalDateTime>
        //             </ns:meterTime>
        //          </ns:meterReading>
        //
        //          <!--Zero or more repetitions:-->
        //          <ns:chargingPeriods>
        //
        //             <ns:startDateTime>
        //                <ns:LocalDateTime>?</ns:LocalDateTime>
        //             </ns:startDateTime>
        //
        //             <ns:endDateTime>
        //                <ns:LocalDateTime>?</ns:LocalDateTime>
        //             </ns:endDateTime>
        //
        //             <ns:billingItem>
        //                <ns:BillingItemType>?</ns:BillingItemType>
        //             </ns:billingItem>
        //
        //             <ns:billingValue>?</ns:billingValue>
        //             <ns:itemPrice>?</ns:itemPrice>
        //
        //             <!--Optional:-->
        //             <ns:periodCost>?</ns:periodCost>
        //
        //             <!--Optional:-->
        //             <ns:taxrate>?</ns:taxrate>
        //
        //          </ns:chargingPeriods>
        //
        //          <!--Optional:-->
        //          <ns:currentCost>?</ns:currentCost>
        //
        //          <!--Optional:-->
        //          <ns:currency>?</ns:currency>
        //
        //       </ns:InformProviderMessage>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(InformProviderRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHPdirect inform provider request.
        /// </summary>
        /// <param name="InformProviderRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static InformProviderRequest Parse(XElement             InformProviderRequestXML,
                                                  OnExceptionDelegate  OnException = null)
        {

            InformProviderRequest _InformProviderRequest;

            if (TryParse(InformProviderRequestXML, out _InformProviderRequest, OnException))
                return _InformProviderRequest;

            return null;

        }

        #endregion

        #region (static) Parse(InformProviderRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHPdirect inform provider request.
        /// </summary>
        /// <param name="InformProviderRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static InformProviderRequest Parse(String               InformProviderRequestText,
                                                  OnExceptionDelegate  OnException = null)
        {

            InformProviderRequest _InformProviderRequest;

            if (TryParse(InformProviderRequestText, out _InformProviderRequest, OnException))
                return _InformProviderRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(InformProviderRequestXML,  out InformProviderRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHPdirect inform provider request.
        /// </summary>
        /// <param name="InformProviderRequestXML">The XML to parse.</param>
        /// <param name="InformProviderRequest">The parsed inform provider request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                   InformProviderRequestXML,
                                       out InformProviderRequest  InformProviderRequest,
                                       OnExceptionDelegate        OnException  = null)
        {

            try
            {

                InformProviderRequest = new InformProviderRequest(

                                            InformProviderRequestXML.MapEnumValues     (OCHPNS.Default + "message",
                                                                                        OCHPNS.Default + "message",
                                                                                        s => (DirectMessages) Enum.Parse(typeof(DirectMessages), s)),

                                            InformProviderRequestXML.MapValueOrFail    (OCHPNS.Default + "evseId",
                                                                                        EVSE_Id.Parse),

                                            InformProviderRequestXML.MapValueOrFail    (OCHPNS.Default + "contractId",
                                                                                        Contract_Id.Parse),

                                            InformProviderRequestXML.MapValueOrFail    (OCHPNS.Default + "directId",
                                                                                        Direct_Id.Parse),

                                            InformProviderRequestXML.MapValueOrNullable(OCHPNS.Default + "ttl",
                                                                                        OCHPNS.Default + "DateTime",
                                                                                        DateTime.Parse),

                                            InformProviderRequestXML.MapValueOrNullable(OCHPNS.Default + "stateOfCharge",
                                                                                        Single.Parse),

                                            InformProviderRequestXML.MapValueOrNullable(OCHPNS.Default + "maxPower",
                                                                                        Single.Parse),

                                            InformProviderRequestXML.MapValueOrNullable(OCHPNS.Default + "maxCurrent",
                                                                                        Single.Parse),

                                            InformProviderRequestXML.MapNullableBoolean(OCHPNS.Default + "onePhase"),

                                            InformProviderRequestXML.MapValueOrNullable(OCHPNS.Default + "maxEnergy",
                                                                                        Single.Parse),

                                            InformProviderRequestXML.MapValueOrNullable(OCHPNS.Default + "minEnergy",
                                                                                        Single.Parse),

                                            InformProviderRequestXML.MapValueOrNullable(OCHPNS.Default + "departure",
                                                                                        OCHPNS.Default + "DateTime",
                                                                                        DateTime.Parse),

                                            InformProviderRequestXML.MapValueOrNullable(OCHPNS.Default + "currentPower",
                                                                                        Single.Parse),

                                            InformProviderRequestXML.MapValueOrNullable(OCHPNS.Default + "chargedEnergy",
                                                                                        Single.Parse),

                                            InformProviderRequestXML.MapElement        (OCHPNS.Default + "meterReading",
                                                                                        meterreading => {

                                                                                            var Value     = meterreading.MapValueOrNullable(OCHPNS.Default + "meterValue",
                                                                                                                                            Single.Parse);

                                                                                            if (!Value.HasValue)
                                                                                                return new Timestamped<Single>?();

                                                                                            var Timestamp = meterreading.MapValueOrNullable(OCHPNS.Default + "meterTime",
                                                                                                                                            OCHPNS.Default + "LocalDateTime",
                                                                                                                                            DateTime.Parse);

                                                                                            if (!Timestamp.HasValue)
                                                                                                return new Timestamped<Single>?();

                                                                                            return new Timestamped<Single>?(new Timestamped<Single>(Timestamp.Value, Value.Value));

                                                                                        }),

                                            InformProviderRequestXML.MapElements       (OCHPNS.Default + "chargingPeriods",
                                                                                        CDRPeriod.Parse,
                                                                                        OnException),

                                            InformProviderRequestXML.MapValueOrNullable(OCHPNS.Default + "currentCost",
                                                                                        Single.Parse),

                                            InformProviderRequestXML.MapValueOrNull    (OCHPNS.Default + "currency",
                                                                                        Currency.ParseString)

                                        );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, InformProviderRequestXML, e);

                InformProviderRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(InformProviderRequestText, out InformProviderRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHPdirect inform provider request.
        /// </summary>
        /// <param name="InformProviderRequestText">The text to parse.</param>
        /// <param name="InformProviderRequest">The parsed inform provider request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                        InformProviderRequestText,
                                       out InformProviderRequest  InformProviderRequest,
                                       OnExceptionDelegate           OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(InformProviderRequestText).Root,
                             out InformProviderRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, InformProviderRequestText, e);
            }

            InformProviderRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "InformProviderMessage",

                   new XElement(OCHPNS.Default + "evseId",                  EVSEId.    ToString()),
                   new XElement(OCHPNS.Default + "contractId",              ContractId.ToString()),
                   new XElement(OCHPNS.Default + "directId",                DirectId.  ToString()),

                   SessionTimeoutAt.HasValue
                       ? new XElement(OCHPNS.Default + "ttl",
                             new XElement(OCHPNS.Default + "DateTime",      SessionTimeoutAt.Value.ToIso8601())
                         )
                       : null,

                   StateOfCharge.HasValue
                       ? new XElement(OCHPNS.Default + "stateOfCharge",     StateOfCharge.Value)
                       : null,

                   MaxPower.HasValue
                       ? new XElement(OCHPNS.Default + "maxPower",          MaxPower.Value)
                       : null,

                   MaxEnergy.HasValue
                       ? new XElement(OCHPNS.Default + "maxEnergy",         MaxEnergy.Value)
                       : null,

                   MinEnergy.HasValue
                       ? new XElement(OCHPNS.Default + "minEnergy",         MinEnergy.Value)
                       : null,

                   Departure.HasValue
                       ? new XElement(OCHPNS.Default + "departure",
                             new XElement(OCHPNS.Default + "DateTime",      SessionTimeoutAt.Value.ToIso8601())
                         )
                       : null,

                   CurrentPower.HasValue
                       ? new XElement(OCHPNS.Default + "currentPower",      CurrentPower.Value)
                       : null,

                   ChargedEnergy.HasValue
                       ? new XElement(OCHPNS.Default + "chargedEnergy",     ChargedEnergy.Value)
                       : null,

                   MeterReading.HasValue
                       ? new XElement(OCHPNS.Default + "meterReading",
                             new XElement(OCHPNS.Default + "meterValue", MeterReading.Value.Value),
                             new XElement(OCHPNS.Default + "meterTime",
                                 new XElement(OCHPNS.Default + "LocalDateTime", MeterReading.Value.Timestamp.ToIso8601())
                             )
                         )
                       : null,

                   ChargingPeriods != null && ChargingPeriods.Any()
                       ? new XElement(OCHPNS.Default + "chargingPeriods",   ChargingPeriods.Select(persiod => persiod.ToXML()))
                       : null,

                   CurrentCost.HasValue
                       ? new XElement(OCHPNS.Default + "currentCost",       CurrentCost.Value)
                       : null,

                   Currency != null
                       ? new XElement(OCHPNS.Default + "currency",          Currency.ISOCode)
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (InformProviderRequest1, InformProviderRequest2)

        /// <summary>
        /// Compares two inform provider requests for equality.
        /// </summary>
        /// <param name="InformProviderRequest1">A inform provider request.</param>
        /// <param name="InformProviderRequest2">Another inform provider request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (InformProviderRequest InformProviderRequest1, InformProviderRequest InformProviderRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(InformProviderRequest1, InformProviderRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) InformProviderRequest1 == null) || ((Object) InformProviderRequest2 == null))
                return false;

            return InformProviderRequest1.Equals(InformProviderRequest2);

        }

        #endregion

        #region Operator != (InformProviderRequest1, InformProviderRequest2)

        /// <summary>
        /// Compares two inform provider requests for inequality.
        /// </summary>
        /// <param name="InformProviderRequest1">A inform provider request.</param>
        /// <param name="InformProviderRequest2">Another inform provider request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (InformProviderRequest InformProviderRequest1, InformProviderRequest InformProviderRequest2)

            => !(InformProviderRequest1 == InformProviderRequest2);

        #endregion

        #endregion

        #region IEquatable<InformProviderRequest> Members

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

            // Check if the given object is a inform provider request.
            var InformProviderRequest = Object as InformProviderRequest;
            if ((Object) InformProviderRequest == null)
                return false;

            return this.Equals(InformProviderRequest);

        }

        #endregion

        #region Equals(InformProviderRequest)

        /// <summary>
        /// Compares two inform provider requests for equality.
        /// </summary>
        /// <param name="InformProviderRequest">A inform provider request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(InformProviderRequest InformProviderRequest)
        {

            if ((Object) InformProviderRequest == null)
                return false;

            return DirectMessage.Equals(InformProviderRequest.DirectMessage) &&
                   EVSEId.       Equals(InformProviderRequest.EVSEId)        &&
                   ContractId.   Equals(InformProviderRequest.ContractId)    &&
                   DirectId.     Equals(InformProviderRequest.DirectId)      &&

                   (SessionTimeoutAt.HasValue
                       ? SessionTimeoutAt.Equals(InformProviderRequest.SessionTimeoutAt)
                       : true) &&

                   (StateOfCharge.HasValue
                       ? StateOfCharge.   Equals(InformProviderRequest.StateOfCharge)
                       : true) &&

                   (MaxPower.HasValue
                       ? MaxPower.        Equals(InformProviderRequest.MaxPower)
                       : true) &&

                   (MaxCurrent.HasValue
                       ? MaxCurrent.      Equals(InformProviderRequest.MaxCurrent)
                       : true) &&

                   (OnePhase.HasValue
                       ? OnePhase.        Equals(InformProviderRequest.OnePhase)
                       : true) &&

                   (MaxEnergy.HasValue
                       ? MaxEnergy.       Equals(InformProviderRequest.MaxEnergy)
                       : true) &&

                   (MinEnergy.HasValue
                       ? MinEnergy.       Equals(InformProviderRequest.MinEnergy)
                       : true) &&

                   (Departure.HasValue
                       ? MaxCurrent.      Equals(InformProviderRequest.Departure)
                       : true) &&

                   (CurrentPower.HasValue
                       ? CurrentPower.    Equals(InformProviderRequest.CurrentPower)
                       : true) &&

                   (ChargedEnergy.HasValue
                       ? ChargedEnergy.   Equals(InformProviderRequest.ChargedEnergy)
                       : true) &&

                   (MeterReading.HasValue
                       ? MeterReading.    Equals(InformProviderRequest.MeterReading)
                       : true) &&

                   //(ChargingPeriods.Any()
                   //    ? ChargingPeriods
                   //    : true) &&

                   (CurrentCost.HasValue
                       ? CurrentCost.     Equals(InformProviderRequest.CurrentCost)
                       : true) &&

                   Currency.Equals(InformProviderRequest.Currency);

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

                return DirectMessage.GetHashCode() * 73 ^
                       EVSEId.       GetHashCode() * 71 ^
                       ContractId.   GetHashCode() * 67 ^
                       DirectId.     GetHashCode() * 61 ^

                       (SessionTimeoutAt.HasValue
                            ? SessionTimeoutAt.GetHashCode() * 59
                            : 0) ^

                       (StateOfCharge.HasValue
                            ? StateOfCharge.   GetHashCode() * 53
                            : 0) ^

                       (MaxPower.HasValue
                            ? MaxPower.        GetHashCode() * 47
                            : 0) ^

                       (MaxCurrent.HasValue
                            ? MaxCurrent.      GetHashCode() * 43
                            : 0) ^

                       (OnePhase.HasValue
                            ? OnePhase.        GetHashCode() * 41
                            : 0) ^

                       (MaxEnergy.HasValue
                            ? MaxEnergy.       GetHashCode() * 37
                            : 0) ^

                       (MinEnergy.HasValue
                            ? MinEnergy.       GetHashCode() * 31
                            : 0) ^

                       (Departure.HasValue
                            ? Departure.       GetHashCode() * 29
                            : 0) ^

                       (CurrentPower.HasValue
                            ? CurrentPower.    GetHashCode() * 23
                            : 0) ^

                       (ChargedEnergy.HasValue
                            ? ChargedEnergy.   GetHashCode() * 19
                            : 0) ^

                       (MeterReading.HasValue
                            ? MeterReading.    GetHashCode() * 17
                            : 0) ^

                       //(ChargingPeriods.HasValue
                       //     ? ChargingPeriods.GetHashCode() * 13
                       //     : 0) ^

                       (CurrentCost.HasValue
                            ? CurrentCost.     GetHashCode() * 11
                            : 0) ^

                       (Currency != null
                            ? Currency.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(DirectMessage,
                             " for ", DirectId,
                             " at ", EVSEId,
                             " contract ", ContractId);

        #endregion


    }

}
