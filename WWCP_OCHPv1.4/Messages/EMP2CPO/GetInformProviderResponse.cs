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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHPdirect get inform provider response.
    /// </summary>
    public class GetInformProviderResponse : AResponse
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

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetInformProviderResponse OK(String Description = null)

            => new GetInformProviderResponse(Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetInformProviderResponse Partly(String Description = null)

            => new GetInformProviderResponse(Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetInformProviderResponse NotAuthorized(String Description = null)

            => new GetInformProviderResponse(Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetInformProviderResponse InvalidId(String Description = null)

            => new GetInformProviderResponse(Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetInformProviderResponse Server(String Description = null)

            => new GetInformProviderResponse(Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetInformProviderResponse Format(String Description = null)

            => new GetInformProviderResponse(Result.Format(Description));

        #endregion

        #region Constructor(s)

        private GetInformProviderResponse(Result Result)
            : base(Result)
        { }

        /// <summary>
        /// Create a new OCHPdirect get inform provider response.
        /// </summary>
        /// <param name="Result">A generic OHCP result.</param>
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
        public GetInformProviderResponse(Result                  Result,
                                         DirectMessages          DirectMessage,
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

            : base(Result)

        {

            #region Initial checks

            if (EVSEId     == null)
                throw new ArgumentNullException(nameof(EVSEId),      "The given identification of an EVSE must not be null!");

            if (ContractId == null)
                throw new ArgumentNullException(nameof(ContractId),  "The given identification of an e-mobility contract must not be null!");

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
        //          <ns:result>
        //             <ns:resultCode>
        //                <ns:resultCode>?</ns:resultCode>
        //             </ns:resultCode>
        //             <ns:resultDescription>?</ns:resultDescription>
        //          </ns:result>
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

        #region (static) Parse(GetInformProviderResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHPdirect get inform provider response.
        /// </summary>
        /// <param name="GetInformProviderResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetInformProviderResponse Parse(XElement             GetInformProviderResponseXML,
                                                      OnExceptionDelegate  OnException = null)
        {

            GetInformProviderResponse _GetInformProviderResponse;

            if (TryParse(GetInformProviderResponseXML, out _GetInformProviderResponse, OnException))
                return _GetInformProviderResponse;

            return null;

        }

        #endregion

        #region (static) Parse(GetInformProviderResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHPdirect get inform provider response.
        /// </summary>
        /// <param name="GetInformProviderResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetInformProviderResponse Parse(String               GetInformProviderResponseText,
                                                      OnExceptionDelegate  OnException = null)
        {

            GetInformProviderResponse _GetInformProviderResponse;

            if (TryParse(GetInformProviderResponseText, out _GetInformProviderResponse, OnException))
                return _GetInformProviderResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(GetInformProviderResponseXML,  out GetInformProviderResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHPdirect get inform provider response.
        /// </summary>
        /// <param name="GetInformProviderResponseXML">The XML to parse.</param>
        /// <param name="GetInformProviderResponse">The parsed get inform provider response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                       GetInformProviderResponseXML,
                                       out GetInformProviderResponse  GetInformProviderResponse,
                                       OnExceptionDelegate            OnException  = null)
        {

            try
            {

                GetInformProviderResponse = new GetInformProviderResponse(

                                                GetInformProviderResponseXML.MapElementOrFail  (OCHPNS.Default + "result",
                                                                                                Result.Parse,
                                                                                                OnException),

                                                GetInformProviderResponseXML.MapEnumValues     (OCHPNS.Default + "message",
                                                                                                OCHPNS.Default + "message",
                                                                                                s => (DirectMessages) Enum.Parse(typeof(DirectMessages), s)),

                                                GetInformProviderResponseXML.MapValueOrFail    (OCHPNS.Default + "evseId",
                                                                                                EVSE_Id.Parse),

                                                GetInformProviderResponseXML.MapValueOrFail    (OCHPNS.Default + "contractId",
                                                                                                Contract_Id.Parse),

                                                GetInformProviderResponseXML.MapValueOrFail    (OCHPNS.Default + "directId",
                                                                                                Direct_Id.Parse),

                                                GetInformProviderResponseXML.MapValueOrNullable(OCHPNS.Default + "ttl",
                                                                                                OCHPNS.Default + "DateTime",
                                                                                                DateTime.Parse),

                                                GetInformProviderResponseXML.MapValueOrNullable(OCHPNS.Default + "stateOfCharge",
                                                                                                Single.Parse),

                                                GetInformProviderResponseXML.MapValueOrNullable(OCHPNS.Default + "maxPower",
                                                                                                Single.Parse),

                                                GetInformProviderResponseXML.MapValueOrNullable(OCHPNS.Default + "maxCurrent",
                                                                                                Single.Parse),

                                                GetInformProviderResponseXML.MapNullableBoolean(OCHPNS.Default + "onePhase"),

                                                GetInformProviderResponseXML.MapValueOrNullable(OCHPNS.Default + "maxEnergy",
                                                                                                Single.Parse),

                                                GetInformProviderResponseXML.MapValueOrNullable(OCHPNS.Default + "minEnergy",
                                                                                                Single.Parse),

                                                GetInformProviderResponseXML.MapValueOrNullable(OCHPNS.Default + "departure",
                                                                                                OCHPNS.Default + "DateTime",
                                                                                                DateTime.Parse),

                                                GetInformProviderResponseXML.MapValueOrNullable(OCHPNS.Default + "currentPower",
                                                                                                Single.Parse),

                                                GetInformProviderResponseXML.MapValueOrNullable(OCHPNS.Default + "chargedEnergy",
                                                                                                Single.Parse),

                                                GetInformProviderResponseXML.MapElement        (OCHPNS.Default + "meterReading",
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

                                                GetInformProviderResponseXML.MapElements       (OCHPNS.Default + "chargingPeriods",
                                                                                                CDRPeriod.Parse,
                                                                                                OnException),

                                                GetInformProviderResponseXML.MapValueOrNullable(OCHPNS.Default + "currentCost",
                                                                                                Single.Parse),

                                                GetInformProviderResponseXML.MapValueOrNull    (OCHPNS.Default + "currency",
                                                                                                Currency.ParseString)

                                            );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, GetInformProviderResponseXML, e);

                GetInformProviderResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetInformProviderResponseText, out GetInformProviderResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHPdirect get inform provider response.
        /// </summary>
        /// <param name="GetInformProviderResponseText">The text to parse.</param>
        /// <param name="GetInformProviderResponse">The parsed get inform provider response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                         GetInformProviderResponseText,
                                       out GetInformProviderResponse  GetInformProviderResponse,
                                       OnExceptionDelegate            OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetInformProviderResponseText).Root,
                             out GetInformProviderResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, GetInformProviderResponseText, e);
            }

            GetInformProviderResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetInformProviderResponse",

                   new XElement(OCHPNS.Default + "result",                  Result.    ToXML()),

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

        #region Operator == (GetInformProviderResponse1, GetInformProviderResponse2)

        /// <summary>
        /// Compares two get inform provider responses for equality.
        /// </summary>
        /// <param name="GetInformProviderResponse1">A get inform provider response.</param>
        /// <param name="GetInformProviderResponse2">Another get inform provider response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetInformProviderResponse GetInformProviderResponse1, GetInformProviderResponse GetInformProviderResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetInformProviderResponse1, GetInformProviderResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetInformProviderResponse1 == null) || ((Object) GetInformProviderResponse2 == null))
                return false;

            return GetInformProviderResponse1.Equals(GetInformProviderResponse2);

        }

        #endregion

        #region Operator != (GetInformProviderResponse1, GetInformProviderResponse2)

        /// <summary>
        /// Compares two get inform provider responses for inequality.
        /// </summary>
        /// <param name="GetInformProviderResponse1">A get inform provider response.</param>
        /// <param name="GetInformProviderResponse2">Another get inform provider response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetInformProviderResponse GetInformProviderResponse1, GetInformProviderResponse GetInformProviderResponse2)

            => !(GetInformProviderResponse1 == GetInformProviderResponse2);

        #endregion

        #endregion

        #region IEquatable<GetInformProviderResponse> Members

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

            // Check if the given object is a get inform provider response.
            var GetInformProviderResponse = Object as GetInformProviderResponse;
            if ((Object) GetInformProviderResponse == null)
                return false;

            return this.Equals(GetInformProviderResponse);

        }

        #endregion

        #region Equals(GetInformProviderResponse)

        /// <summary>
        /// Compares two get inform provider responses for equality.
        /// </summary>
        /// <param name="GetInformProviderResponse">A get inform provider response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(GetInformProviderResponse GetInformProviderResponse)
        {

            if ((Object) GetInformProviderResponse == null)
                return false;

            return Result.       Equals(GetInformProviderResponse.Result)        &&
                   DirectMessage.Equals(GetInformProviderResponse.DirectMessage) &&
                   EVSEId.       Equals(GetInformProviderResponse.EVSEId)        &&
                   ContractId.   Equals(GetInformProviderResponse.ContractId)    &&
                   DirectId.     Equals(GetInformProviderResponse.DirectId)      &&

                   (SessionTimeoutAt.HasValue
                       ? SessionTimeoutAt.Equals(GetInformProviderResponse.SessionTimeoutAt)
                       : true) &&

                   (StateOfCharge.HasValue
                       ? StateOfCharge.   Equals(GetInformProviderResponse.StateOfCharge)
                       : true) &&

                   (MaxPower.HasValue
                       ? MaxPower.        Equals(GetInformProviderResponse.MaxPower)
                       : true) &&

                   (MaxCurrent.HasValue
                       ? MaxCurrent.      Equals(GetInformProviderResponse.MaxCurrent)
                       : true) &&

                   (OnePhase.HasValue
                       ? OnePhase.        Equals(GetInformProviderResponse.OnePhase)
                       : true) &&

                   (MaxEnergy.HasValue
                       ? MaxEnergy.       Equals(GetInformProviderResponse.MaxEnergy)
                       : true) &&

                   (MinEnergy.HasValue
                       ? MinEnergy.       Equals(GetInformProviderResponse.MinEnergy)
                       : true) &&

                   (Departure.HasValue
                       ? MaxCurrent.      Equals(GetInformProviderResponse.Departure)
                       : true) &&

                   (CurrentPower.HasValue
                       ? CurrentPower.    Equals(GetInformProviderResponse.CurrentPower)
                       : true) &&

                   (ChargedEnergy.HasValue
                       ? ChargedEnergy.   Equals(GetInformProviderResponse.ChargedEnergy)
                       : true) &&

                   (MeterReading.HasValue
                       ? MeterReading.    Equals(GetInformProviderResponse.MeterReading)
                       : true) &&

                   //(ChargingPeriods.Any()
                   //    ? ChargingPeriods
                   //    : true) &&

                   (CurrentCost.HasValue
                       ? CurrentCost.     Equals(GetInformProviderResponse.CurrentCost)
                       : true) &&

                   Currency.Equals(GetInformProviderResponse.Currency);

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

                return Result.       GetHashCode() * 79 ^
                       DirectMessage.GetHashCode() * 73 ^
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

            => String.Concat(Result,
                             " / ",        DirectMessage,
                             " for ",      DirectId,
                             " at ",       EVSEId,
                             " contract ", ContractId);

        #endregion

    }

}
