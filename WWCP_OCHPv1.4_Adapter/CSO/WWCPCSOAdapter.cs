/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Text.RegularExpressions;

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

using cloud.charging.open.protocols.WWCP;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.CPO
{

    /// <summary>
    /// A WWCP wrapper for the OCHP CPO Roaming client which maps
    /// WWCP data structures onto OCHP data structures and vice versa.
    /// </summary>
    public class WWCPCSOAdapter : AWWCPCSOAdapter<CDRInfo>,
                                  ICSORoamingProvider,
                                  IEquatable<WWCPCSOAdapter>,
                                  IComparable<WWCPCSOAdapter>,
                                  IComparable
    {

        #region Data

        //private        readonly  ISendData                                     _ISendData;

        //private        readonly  ISendStatus                                   _ISendStatus;

        private        readonly  CPO.CustomEVSEIdMapperDelegate                _CustomEVSEIdMapper;

        private        readonly  EVSE2ChargePointInfoDelegate                  _EVSE2ChargePointInfo;

        private        readonly  EVSEStatusUpdate2EVSEStatusDelegate           _EVSEStatusUpdate2EVSEStatus;

        private        readonly  ChargePointInfo2XMLDelegate                   _ChargePointInfo2XML;

        private        readonly  EVSEStatus2XMLDelegate                        _EVSEStatus2XML;

        private        readonly  ChargingStationOperatorNameSelectorDelegate   _OperatorNameSelector;

        private static readonly  Regex                                         pattern = new Regex(@"\s=\s");

        public  static readonly  ChargingStationOperatorNameSelectorDelegate   DefaultOperatorNameSelector = I18N => I18N.FirstText();

                /// <summary>
        /// The default service check interval.
        /// </summary>
        public  readonly static  TimeSpan                                       DefaultServiceCheckEvery       = TimeSpan.FromSeconds(31);

        /// <summary>
        /// The default status check interval.
        /// </summary>
        public  readonly static  TimeSpan                                       DefaultStatusCheckEvery        = TimeSpan.FromSeconds(3);

        /// <summary>
        /// The default EVSE status refresh interval.
        /// </summary>
        public  readonly static  TimeSpan                                       DefaultEVSEStatusRefreshEvery  = TimeSpan.FromHours(12);


        private readonly         Object                                         ServiceCheckLock;
        private readonly         Timer                                          ServiceCheckTimer;
        //private readonly         Object                                         StatusCheckLock;
        //private readonly         Timer                                          StatusCheckTimer;
        //private readonly         Object                                         EVSEStatusRefreshLock;
        private static           SemaphoreSlim                                  EVSEStatusRefreshLock  = new SemaphoreSlim(1,1);
        private readonly         TimeSpan                                       EVSEStatusRefreshEvery;
        private readonly         Timer                                          EVSEStatusRefreshTimer;

        private                  UInt64                                         serviceRunId;

        public readonly static   TimeSpan                                       DefaultRequestTimeout = TimeSpan.FromSeconds(30);


        private readonly         Dictionary<EMT_Id, Contract_Id>                lookup;

        private static readonly  Char[] rs                                      = new Char[] { (Char) 30 };

        #endregion

        #region Properties

        IId IAuthorizeStartStop.AuthId
            => Id;

        IId ISendChargeDetailRecords.SendChargeDetailRecordsId
            => Id;

        /// <summary>
        /// The wrapped CPO roaming object.
        /// </summary>
        public CPORoaming CPORoaming { get; }


        /// <summary>
        /// The CPO client.
        /// </summary>
        public CPOClient CPOClient
            => CPORoaming?.CPOClient;

        ///// <summary>
        ///// The CPO client logger.
        ///// </summary>
        //public CPOClient.Logger ClientLogger
        //    => CPORoaming?.CPOClient?.Logger;


        /// <summary>
        /// The CPO server.
        /// </summary>
        public CPOServer CPOServer
            => CPORoaming?.CPOServer;

        ///// <summary>
        ///// The CPO server logger.
        ///// </summary>
        //public CPOServerLogger ServerLogger
        //    => CPORoaming?.CPOServerLogger;


        #region ServiceCheckEvery

        private UInt32 _ServiceCheckEvery;

        /// <summary>
        /// The service check interval.
        /// </summary>
        public TimeSpan ServiceCheckEvery
        {

            get
            {
                return TimeSpan.FromSeconds(_ServiceCheckEvery);
            }

            set
            {
                _ServiceCheckEvery = (UInt32) value.TotalSeconds;
            }

        }

        #endregion


        public IncludeChargePointDelegate       IncludeChargePoints        { get; }


        #region DisableEVSEStatusRefresh

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisableEVSEStatusRefresh         { get; set; }

        #endregion

        #endregion

        #region Events

        // Client logging...

        #region OnSetChargePointInfosWWCPRequest/-Response

        /// <summary>
        /// An event fired whenever new charge point infos will be send upstream.
        /// </summary>
        public event OnSetChargePointInfosWWCPRequestDelegate      OnSetChargePointInfosWWCPRequest;

        /// <summary>
        /// An event fired whenever new charge point infos had been sent upstream.
        /// </summary>
        public event OnSetChargePointInfosWWCPResponseDelegate     OnSetChargePointInfosWWCPResponse;

        #endregion

        #region OnUpdateChargePointInfosWWCPRequest/-Response

        /// <summary>
        /// An event fired whenever charge point info updates will be send upstream.
        /// </summary>
        public event OnUpdateChargePointInfosWWCPRequestDelegate   OnUpdateChargePointInfosWWCPRequest;

        /// <summary>
        /// An event fired whenever charge point info updates had been sent upstream.
        /// </summary>
        public event OnUpdateChargePointInfosWWCPResponseDelegate  OnUpdateChargePointInfosWWCPResponse;

        #endregion

        #region OnUpdateEVSEStatusWWCPRequest/-Response

        /// <summary>
        /// An event fired whenever EVSE status updates will be send upstream.
        /// </summary>
        public event OnUpdateEVSEStatusWWCPRequestDelegate OnUpdateEVSEStatusWWCPRequest;

        /// <summary>
        /// An event fired whenever EVSE status updates had been sent upstream.
        /// </summary>
        public event OnUpdateEVSEStatusWWCPResponseDelegate OnUpdateEVSEStatusWWCPResponse;

        #endregion


        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate   OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate  OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate   OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate  OnAuthorizeStopResponse;

        #endregion

        #region OnSendCDRRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record was enqueued for later sending upstream.
        /// </summary>
        public event OnSendCDRsRequestDelegate   OnEnqueueSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record will be send upstream.
        /// </summary>
        public event OnSendCDRsRequestDelegate   OnSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record had been sent upstream.
        /// </summary>
        public event OnSendCDRsResponseDelegate  OnSendCDRsResponse;

        #endregion


        #region OnWWCPCPOAdapterException

        public delegate Task OnWWCPCPOAdapterExceptionDelegate(DateTimeOffset  Timestamp,
                                                               WWCPCSOAdapter  Sender,
                                                               Exception       Exception);

        public event OnWWCPCPOAdapterExceptionDelegate OnWWCPCPOAdapterException;

        #endregion


        public delegate void FlushServiceQueuesDelegate(WWCPCSOAdapter Sender, TimeSpan Every);

        public event FlushServiceQueuesDelegate FlushServiceQueuesEvent;


        public delegate void FlushEVSEStatusUpdateQueuesDelegate(WWCPCSOAdapter Sender, TimeSpan Every);

        public event FlushEVSEStatusUpdateQueuesDelegate FlushEVSEStatusUpdateQueuesEvent;


        public delegate void EVSEStatusRefreshEventDelegate(DateTimeOffset Timestamp, WWCPCSOAdapter Sender, String Message);

        public event EVSEStatusRefreshEventDelegate EVSEStatusRefreshEvent;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new WWCP wrapper for the OCHP roaming client for Charging Station Operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The official (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="CPORoaming">A OCHP CPO roaming object to be mapped to WWCP.</param>
        /// <param name="EVSE2ChargePointInfo">A delegate to process a charge point info, e.g. before pushing it to the roaming provider.</param>
        /// <param name="ChargePointInfo2XML">A delegate to process the XML representation of a charge point info, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="ServiceCheckEvery">The service check interval.</param>
        /// <param name="StatusCheckEvery">The status check interval.</param>
        /// <param name="EVSEStatusRefreshEvery">The EVSE status refresh interval.</param>
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableEVSEStatusRefresh">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        public WWCPCSOAdapter(CSORoamingProvider_Id                           Id,
                              I18NString                                      Name,
                              I18NString                                      Description,
                              RoamingNetwork                                  RoamingNetwork,
                              CPORoaming                                      CPORoaming,

                              EVSE2ChargePointInfoDelegate?                   EVSE2ChargePointInfo                = null,
                              EVSEStatusUpdate2EVSEStatusDelegate?            EVSEStatusUpdate2EVSEStatus         = null,
                              ChargePointInfo2XMLDelegate?                    ChargePointInfo2XML                 = null,
                              EVSEStatus2XMLDelegate?                         EVSEStatus2XML                      = null,

                              //WWCP.IncludeChargingStationOperatorIdDelegate?  IncludeChargingStationOperatorIds   = null,
                              //WWCP.IncludeChargingStationOperatorDelegate?    IncludeChargingStationOperators     = null,
                              //WWCP.IncludeChargingPoolIdDelegate?             IncludeChargingPoolIds              = null,
                              //WWCP.IncludeChargingPoolDelegate?               IncludeChargingPools                = null,
                              WWCP.IncludeChargingStationIdDelegate?          IncludeChargingStationIds           = null,
                              WWCP.IncludeChargingStationDelegate?            IncludeChargingStations             = null,
                              WWCP.IncludeEVSEIdDelegate?                     IncludeEVSEIds                      = null,
                              WWCP.IncludeEVSEDelegate?                       IncludeEVSEs                        = null,

                              IncludeChargePointDelegate?                     IncludeChargePoints                 = null,

                              CustomEVSEIdMapperDelegate?                     CustomEVSEIdMapper                  = null,
                              WWCP.ChargeDetailRecordFilterDelegate?          ChargeDetailRecordFilter            = null,

                              TimeSpan?                                       ServiceCheckEvery                   = null,
                              TimeSpan?                                       StatusCheckEvery                    = null,
                              TimeSpan?                                       EVSEStatusRefreshEvery              = null,
                              TimeSpan?                                       CDRCheckEvery                       = null,

                              Boolean                                         DisablePushData                     = false,
                              Boolean                                         DisablePushStatus                   = false,
                              Boolean                                         DisableEVSEStatusRefresh            = false,
                              Boolean                                         DisableAuthentication               = false,
                              Boolean                                         DisableSendChargeDetailRecords      = false,

                              String                                          EllipticCurve                       = "P-256",
                              ECPrivateKeyParameters?                         PrivateKey                          = null,
                              PublicKeyCertificates?                          PublicKeyCertificates               = null,

                              Boolean?                                        IsDevelopment                       = null,
                              IEnumerable<String>?                            DevelopmentServers                  = null,
                              Boolean?                                        DisableLogging                      = false,
                              String?                                         LoggingPath                         = null,
                              String?                                         LoggingContext                      = null,
                              String?                                         LogfileName                         = null,
                              LogfileCreatorDelegate?                         LogfileCreator                      = null,

                              String?                                         ClientsLoggingPath                  = null,
                              String?                                         ClientsLoggingContext               = null,
                              LogfileCreatorDelegate?                         ClientsLogfileCreator               = null,
                              DNSClient?                                      DNSClient                           = null)

            : base(Id,
                   RoamingNetwork,
                   Name,
                   Description,

                   IncludeEVSEIds,
                   IncludeEVSEs,
                   IncludeChargingStationIds,
                   IncludeChargingStations,
                   null,
                   null,
                   null,
                   null,
                   ChargeDetailRecordFilter,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   EVSEStatusRefreshEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   true,
                   DisablePushStatus,
                   DisableEVSEStatusRefresh,
                   true,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,

                   IsDevelopment,
                   DevelopmentServers,
                   DisableLogging,
                   LoggingPath,
                   LoggingContext,
                   LogfileName,
                   LogfileCreator,

                   ClientsLoggingPath,
                   ClientsLoggingContext,
                   ClientsLogfileCreator,
                   DNSClient)

        {

            #region Initial checks

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),        "The given roaming provider name must not be null or empty!");

            if (CPORoaming is null)
                throw new ArgumentNullException(nameof(CPORoaming),  "The given OCHP CPO Roaming object must not be null!");

            #endregion

            this.CPORoaming                           = CPORoaming;
            this._CustomEVSEIdMapper                  = CustomEVSEIdMapper;
            this._EVSE2ChargePointInfo                = EVSE2ChargePointInfo;
            this._EVSEStatusUpdate2EVSEStatus         = EVSEStatusUpdate2EVSEStatus;
            this._ChargePointInfo2XML                 = ChargePointInfo2XML;
            this._EVSEStatus2XML                      = EVSEStatus2XML;

            this.IncludeChargePoints                  = IncludeChargePoints ?? (cp => true);

            this.ServiceCheckLock                     = new Object();
            this._ServiceCheckEvery                   = (UInt32) (ServiceCheckEvery.HasValue
                                                                     ? ServiceCheckEvery.Value. TotalMilliseconds
                                                                     : DefaultServiceCheckEvery.TotalMilliseconds);
            this.ServiceCheckTimer                    = new Timer(ServiceCheck,           null,                           0, _ServiceCheckEvery);

            this.EVSEStatusRefreshEvery               = EVSEStatusRefreshEvery ?? DefaultEVSEStatusRefreshEvery;
            this.EVSEStatusRefreshTimer               = new Timer(EVSEStatusRefresh, null, this.EVSEStatusRefreshEvery, this.EVSEStatusRefreshEvery);

            this.DisableEVSEStatusRefresh             = DisableEVSEStatusRefresh;

            this.lookup                               = [];

            lock (lookup)
            {

                var elements                          = Array.Empty<String>();

                EMT_Id       EMTId       = default;
                Contract_Id  ContractId  = default;

                var path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar.ToString() + "OCHPv1.4";

                if (Directory.Exists(path))
                {

                    foreach (var inputfile in Directory.EnumerateFiles(path,
                                                                       "EMTIds_2_ContractIds_*.log",
                                                                       SearchOption.TopDirectoryOnly))
                    {

                        foreach (var line in File.ReadLines(inputfile))
                        {

                            try
                            {

                                if (!line.StartsWith("#") && !line.StartsWith("//") && !line.IsNullOrEmpty())
                                {

                                    elements = line.Split(rs, StringSplitOptions.None);

                                    if (elements.Length == 3)
                                    {

                                        EMTId       = new EMT_Id(elements[1]?.Trim(),
                                                                 TokenRepresentations.Plain,
                                                                 TokenTypes.RFID,
                                                                 TokenSubTypes.MifareClassic);

                                        ContractId  = Contract_Id.Parse(elements[2]);

                                        if (!lookup.ContainsKey(EMTId))
                                            lookup.Add(EMTId, ContractId);

                                        else
                                        {

                                            if (lookup[EMTId] != ContractId)
                                            {

                                            }

                                        }

                                    }

                                    else if (elements.Length == 5)
                                        lookup.Add(
                                            new EMT_Id(
                                                elements[0]?.Trim() ?? "",
                                                Enum.Parse<TokenRepresentations>(elements[1]?.Trim() ?? "", ignoreCase: true),
                                                Enum.Parse<TokenTypes>          (elements[2]?.Trim() ?? "", ignoreCase: true),
                                                elements[3]?.Trim().IsNotNullOrEmpty() == true
                                                    ? new TokenSubTypes?(Enum.Parse<TokenSubTypes>(elements[3]?.Trim() ?? "", ignoreCase: true))
                                                    : null
                                            ),
                                            Contract_Id.Parse(elements[4])
                                        );

                                }

                            }
                            catch (Exception e)
                            {
                                DebugX.Log("Could not read logfile " + inputfile + @""": " + e.Message);
                            }

                        }

                    }

                }

            }


            // Link events...

            #region OnRemoteReservationStart

            //this.CPORoaming.OnRemoteReservationStart += async (Timestamp,
            //                                                   Sender,
            //                                                   CancellationToken,
            //                                                   EventTrackingId,
            //                                                   EVSEId,
            //                                                   PartnerProductId,
            //                                                   SessionId,
            //                                                   PartnerSessionId,
            //                                                   ProviderId,
            //                                                   EVCOId,
            //                                                   RequestTimeout) => {


            //    #region Request transformation

            //    TimeSpan? Duration = null;

            //    // Analyse the ChargingProductId field and apply the found key/value-pairs
            //    if (PartnerProductId is not null && PartnerProductId.ToString().IsNotNullOrEmpty())
            //    {

            //        var Elements = pattern.Replace(PartnerProductId.ToString(), "=").Split('|').ToArray();

            //        if (Elements.Length > 0)
            //        {

            //            var DurationText = Elements.FirstOrDefault(element => element.StartsWith("D=", StringComparison.InvariantCulture));
            //            if (DurationText is not null)
            //            {

            //                DurationText = DurationText.Substring(2);

            //                if (DurationText.EndsWith("sec", StringComparison.InvariantCulture))
            //                    Duration = TimeSpan.FromSeconds(UInt32.Parse(DurationText.Substring(0, DurationText.Length - 3)));

            //                if (DurationText.EndsWith("min", StringComparison.InvariantCulture))
            //                    Duration = TimeSpan.FromMinutes(UInt32.Parse(DurationText.Substring(0, DurationText.Length - 3)));

            //            }

            //            var PartnerProductText = Elements.FirstOrDefault(element => element.StartsWith("P=", StringComparison.InvariantCulture));
            //            if (PartnerProductText is not null)
            //            {

            //                PartnerProductId = PartnerProduct_Id.Parse(DurationText.Substring(2));

            //            }

            //        }

            //    }

            //    #endregion

            //    var response = await RoamingNetwork.Reserve(EVSEId.ToWWCP(),
            //                                                Duration:           Duration,
            //                                                ReservationId:      SessionId.HasValue ? ChargingReservation_Id.Parse(SessionId.ToString()) : new ChargingReservation_Id?(),
            //                                                ProviderId:         ProviderId.      ToWWCP(),
            //                                                eMAId:              EVCOId.          ToWWCP(),
            //                                                ChargingProductId:  PartnerProductId.ToWWCP(),
            //                                                eMAIds:             new eMobilityAccount_Id[] { EVCOId.Value.ToWWCP() },
            //                                                Timestamp:          Timestamp,
            //                                                CancellationToken:  CancellationToken,
            //                                                EventTrackingId:    EventTrackingId,
            //                                                RequestTimeout:     RequestTimeout).ConfigureAwait(false);

            //    #region Response mapping

            //    if (response is not null)
            //    {
            //        switch (response.Result)
            //        {

            //            case ReservationResultType.Success:
            //                return new Acknowledgement(Session_Id.Parse(response.Reservation.Id.ToString()),
            //                                           StatusCodeDescription: "Ready to charge!");

            //            case ReservationResultType.InvalidCredentials:
            //                return new Acknowledgement(StatusCodes.SessionIsInvalid,
            //                                           "Session is invalid",
            //                                           SessionId: Session_Id.Parse(response.Reservation.Id.ToString()));

            //            case ReservationResultType.Timeout:
            //            case ReservationResultType.CommunicationError:
            //                return new Acknowledgement(StatusCodes.CommunicationToEVSEFailed,
            //                                           "Communication to EVSE failed!");

            //            case ReservationResultType.AlreadyReserved:
            //                return new Acknowledgement(StatusCodes.EVSEAlreadyReserved,
            //                                           "EVSE already reserved!");

            //            case ReservationResultType.AlreadyInUse:
            //                return new Acknowledgement(StatusCodes.EVSEAlreadyInUse_WrongToken,
            //                                           "EVSE is already in use!");

            //            case ReservationResultType.UnknownEVSE:
            //                return new Acknowledgement(StatusCodes.UnknownEVSEID,
            //                                           "Unknown EVSE ID!");

            //            case ReservationResultType.OutOfService:
            //                return new Acknowledgement(StatusCodes.EVSEOutOfService,
            //                                           "EVSE out of service!");

            //        }
            //    }

            //    return new Acknowledgement(StatusCodes.ServiceNotAvailable,
            //                               "Service not available!",
            //                               SessionId: Session_Id.Parse(response.Reservation.Id.ToString()));

            //    #endregion

            //};

            #endregion

            #region OnRemoteReservationStop

            //this.CPORoaming.OnRemoteReservationStop += async (Timestamp,
            //                                                  Sender,
            //                                                  CancellationToken,
            //                                                  EventTrackingId,
            //                                                  EVSEId,
            //                                                  SessionId,
            //                                                  PartnerSessionId,
            //                                                  ProviderId,
            //                                                  RequestTimeout) => {

            //    var response = await RoamingNetwork.CancelReservation(ChargingReservation_Id.Parse(SessionId.ToString()),
            //                                                          ChargingReservationCancellationReason.Deleted,
            //                                                          ProviderId.ToWWCP(),
            //                                                          EVSEId.    ToWWCP(),

            //                                                          Timestamp,
            //                                                          CancellationToken,
            //                                                          EventTrackingId,
            //                                                          RequestTimeout).ConfigureAwait(false);

            //    #region Response mapping

            //    if (response is not null)
            //    {
            //        switch (response.Result)
            //        {

            //            case CancelReservationResultType.Success:
            //                return new Acknowledgement(Session_Id.Parse(response.ReservationId.ToString()),
            //                                           StatusCodeDescription: "Reservation deleted!");

            //            case CancelReservationResultType.UnknownReservationId:
            //                return new Acknowledgement(StatusCodes.SessionIsInvalid,
            //                                           "Session is invalid!",
            //                                           SessionId: SessionId);

            //            case CancelReservationResultType.Offline:
            //            case CancelReservationResultType.Timeout:
            //            case CancelReservationResultType.CommunicationError:
            //                return new Acknowledgement(StatusCodes.CommunicationToEVSEFailed,
            //                                           "Communication to EVSE failed!");

            //            case CancelReservationResultType.UnknownEVSE:
            //                return new Acknowledgement(StatusCodes.UnknownEVSEID,
            //                                           "Unknown EVSE ID!");

            //            case CancelReservationResultType.OutOfService:
            //                return new Acknowledgement(StatusCodes.EVSEOutOfService,
            //                                           "EVSE out of service!");

            //        }
            //    }

            //    return new Acknowledgement(StatusCodes.ServiceNotAvailable,
            //                               "Service not available!",
            //                               SessionId: SessionId);

            //    #endregion

            //};

            #endregion

            #region OnRemoteStart

            //this.CPORoaming.OnRemoteStart += async (Timestamp,
            //                                        Sender,
            //                                        CancellationToken,
            //                                        EventTrackingId,
            //                                        EVSEId,
            //                                        ChargingProductId,
            //                                        SessionId,
            //                                        PartnerSessionId,
            //                                        ProviderId,
            //                                        EVCOId,
            //                                        RequestTimeout) => {

            //    #region Request mapping

            //    ChargingReservation_Id ReservationId = default(ChargingReservation_Id);

            //    if (ChargingProductId is not null && ChargingProductId.ToString().IsNotNullOrEmpty())
            //    {

            //        var Elements = ChargingProductId.ToString().Split('|').ToArray();

            //        if (Elements.Length > 0)
            //        {
            //            var ChargingReservationIdText = Elements.FirstOrDefault(element => element.StartsWith("R=", StringComparison.InvariantCulture));
            //            if (ChargingReservationIdText.IsNotNullOrEmpty())
            //                ReservationId = ChargingReservation_Id.Parse(ChargingReservationIdText.Substring(2));
            //        }

            //    }

            //    #endregion

            //    var response = await RoamingNetwork.RemoteStart(EVSEId.ToWWCP(),
            //                                                    ChargingProductId.ToWWCP(),
            //                                                    ReservationId,
            //                                                    SessionId. ToWWCP().Value,
            //                                                    ProviderId.ToWWCP(),
            //                                                    EVCOId.    ToWWCP(),

            //                                                    Timestamp,
            //                                                    CancellationToken,
            //                                                    EventTrackingId,
            //                                                    RequestTimeout).ConfigureAwait(false);

            //    #region Response mapping

            //    if (response is not null)
            //    {
            //        switch (response.Result)
            //        {

            //            case RemoteStartEVSEResultType.Success:
            //                return new Acknowledgement(response.Session.Id.ToOCHP(),
            //                                           StatusCodeDescription: "Ready to charge!");

            //            case RemoteStartEVSEResultType.InvalidSessionId:
            //                return new Acknowledgement(StatusCodes.SessionIsInvalid,
            //                                           "Session is invalid!",
            //                                           SessionId: SessionId);

            //            case RemoteStartEVSEResultType.InvalidCredentials:
            //                return new Acknowledgement(StatusCodes.NoValidContract,
            //                                           "No valid contract!");

            //            case RemoteStartEVSEResultType.Offline:
            //                return new Acknowledgement(StatusCodes.CommunicationToEVSEFailed,
            //                                           "Communication to EVSE failed!");

            //            case RemoteStartEVSEResultType.Timeout:
            //            case RemoteStartEVSEResultType.CommunicationError:
            //                return new Acknowledgement(StatusCodes.CommunicationToEVSEFailed,
            //                                           "Communication to EVSE failed!");

            //            case RemoteStartEVSEResultType.Reserved:
            //                return new Acknowledgement(StatusCodes.EVSEAlreadyReserved,
            //                                           "EVSE already reserved!");

            //            case RemoteStartEVSEResultType.AlreadyInUse:
            //                return new Acknowledgement(StatusCodes.EVSEAlreadyInUse_WrongToken,
            //                                           "EVSE is already in use!");

            //            case RemoteStartEVSEResultType.UnknownEVSE:
            //                return new Acknowledgement(StatusCodes.UnknownEVSEID,
            //                                           "Unknown EVSE ID!");

            //            case RemoteStartEVSEResultType.OutOfService:
            //                return new Acknowledgement(StatusCodes.EVSEOutOfService,
            //                                           "EVSE out of service!");

            //        }
            //    }

            //    return new Acknowledgement(StatusCodes.ServiceNotAvailable,
            //                               "Service not available!",
            //                               SessionId: SessionId);

            //    #endregion

            //};

            #endregion

            #region OnRemoteStop

            //this.CPORoaming.OnRemoteStop += async (Timestamp,
            //                                       Sender,
            //                                       CancellationToken,
            //                                       EventTrackingId,
            //                                       EVSEId,
            //                                       SessionId,
            //                                       PartnerSessionId,
            //                                       ProviderId,
            //                                       RequestTimeout) => {

            //    var response = await RoamingNetwork.RemoteStop(EVSEId.ToWWCP(),
            //                                                   SessionId. ToWWCP(),
            //                                                   ReservationHandling.Close,
            //                                                   ProviderId.ToWWCP(),
            //                                                   null,

            //                                                   Timestamp,
            //                                                   CancellationToken,
            //                                                   EventTrackingId,
            //                                                   RequestTimeout).ConfigureAwait(false);

            //    #region Response mapping

            //    if (response is not null)
            //    {
            //        switch (response.Result)
            //        {

            //            case RemoteStopEVSEResultType.Success:
            //                return new Acknowledgement(response.SessionId.ToOCHP(),
            //                                           StatusCodeDescription: "Ready to stop charging!");

            //            case RemoteStopEVSEResultType.InvalidSessionId:
            //                return new Acknowledgement(StatusCodes.SessionIsInvalid,
            //                                           "Session is invalid!",
            //                                           SessionId: SessionId);

            //            case RemoteStopEVSEResultType.Offline:
            //            case RemoteStopEVSEResultType.Timeout:
            //            case RemoteStopEVSEResultType.CommunicationError:
            //                return new Acknowledgement(StatusCodes.CommunicationToEVSEFailed,
            //                                           "Communication to EVSE failed!");

            //            case RemoteStopEVSEResultType.UnknownEVSE:
            //                return new Acknowledgement(StatusCodes.UnknownEVSEID,
            //                                           "Unknown EVSE ID!");

            //            case RemoteStopEVSEResultType.OutOfService:
            //                return new Acknowledgement(StatusCodes.EVSEOutOfService,
            //                                           "EVSE out of service!");

            //        }
            //    }

            //    return new Acknowledgement(StatusCodes.ServiceNotAvailable,
            //                               "Service not available!",
            //                               SessionId: SessionId);

            //    #endregion

            //};

            #endregion

        }

        #endregion


        // RN -> External service requests...

        #region SetChargePointInfos/-Status directly...

        #region (private) SetChargePointInfos   (EVSEs, ...)

        /// <summary>
        /// Upload the EVSE data of the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        private async Task<ReplaceEVSEsResult>

            SetChargePointInfos(IEnumerable<IEVSE>  EVSEs,

                                DateTimeOffset?     Timestamp           = null,
                                EventTracking_Id?   EventTrackingId     = null,
                                TimeSpan?           RequestTimeout      = null,
                                CancellationToken   CancellationToken   = default)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            EventTrackingId ??= EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;


            ReplaceEVSEsResult result;

            #endregion

            #region Get effective list of EVSEs/ChargePointInfos to upload

            var warnings          = new List<Warning>();
            var chargePointInfos  = new List<ChargePointInfo>();

            if (EVSEs.IsNeitherNullNorEmpty())
            {
                foreach (var evse in EVSEs)
                {

                    try
                    {

                        if (evse is null)
                            continue;

                        if (IncludeEVSEs  (evse) &&
                            IncludeEVSEIds(evse.Id))
                        {

                            // WWCP EVSE will be added as internal data "WWCP.EVSE"...
                            var chargePointInfo = evse.ToOCHP(_CustomEVSEIdMapper,
                                                              _EVSE2ChargePointInfo);

                            if (chargePointInfo is not null)
                                chargePointInfos.Add(chargePointInfo);

                        }

                    }
                    catch (Exception e)
                    {
                        warnings.Add(Warning.Create(e.Message, evse));
                    }

                }
            }

            #endregion

            #region Send OnSetChargePointInfosWWCPRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnSetChargePointInfosWWCPRequest?.Invoke(startTime,
                                                         Timestamp.Value,
                                                         this,
                                                         Id,
                                                         EventTrackingId,
                                                         RoamingNetwork.Id,
                                                         chargePointInfos.ULongCount(),
                                                         chargePointInfos,
                                                         warnings.Where(warning => warning.IsNeitherNullNorEmpty()),
                                                         RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnSetChargePointInfosWWCPRequest));
            }

            #endregion


            DateTimeOffset endtime;
            TimeSpan       runtime;

            if (chargePointInfos.Count > 0)
            {

                var response = await CPORoaming.SetChargePointList(
                                         chargePointInfos,
                                         IncludeChargePoints,

                                         Timestamp,
                                         EventTrackingId,
                                         RequestTimeout,
                                         CancellationToken
                                     );


                endtime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime = endtime - startTime;

                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content is not null)
                {

                    if (response.Content.Result.ResultCode == ResultCodes.OK)
                        result = ReplaceEVSEsResult.Added(
                                     EVSEs,
                                     Id,
                                     this,
                                     EventTrackingId,
                                     //ChargePointInfos.Select(chargePointInfo => chargePointInfo.GetInternalData("WWCP.EVSE") as EVSE),
                                     response.Content.Result.Description.ToI18NString(),
                                     null,
                                     runtime
                                 );

                    else
                        result = ReplaceEVSEsResult.Error(
                                     EVSEs,
                                     response.Content.Result.Description.ToI18NString(),
                                     EventTrackingId,
                                     Id,
                                     this,
                                     //ChargePointInfos.Select(chargePointInfo => new PushSingleEVSEDataResult(chargePointInfo.GetInternalData("WWCP.EVSE") as EVSE, PushSingleDataResultTypes.Error)),
                                     null,
                                     runtime
                                 );

                }
                else
                {

                    warnings.Add(Warning.Create(response.HTTPStatusCode.ToString()));
                    warnings.Add(Warning.Create(response.HTTPBody is not null
                                                    ? response.HTTPBody.ToUTF8String()
                                                    : "No HTTP body received!"));

                    result = ReplaceEVSEsResult.Error(
                                 EVSEs,
                                 (response.Content?.Result.Description ?? "error").ToI18NString(),
                                 EventTrackingId,
                                 Id,
                                 this,
                                 //ChargePointInfos.Select(chargePointInfo => new PushSingleEVSEDataResult(chargePointInfo.GetInternalData("WWCP.EVSE") as EVSE, PushSingleDataResultTypes.Error)),
                                 warnings,
                                 runtime
                             );

                }

            }

            #region ...or no ChargePointInfos to push...

            else
            {

                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                result   = ReplaceEVSEsResult.NoOperation(
                               EVSEs,
                               Id,
                               this,
                               EventTrackingId,
                               "No ChargePointInfos to push!".ToI18NString(),
                               warnings,
                               org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - startTime
                           );

            }

            #endregion


            #region Send OnSetChargePointInfosWWCPResponse event

            try
            {

                OnSetChargePointInfosWWCPResponse?.Invoke(endtime,
                                                          Timestamp.Value,
                                                          this,
                                                          Id,
                                                          EventTrackingId,
                                                          RoamingNetwork.Id,
                                                          chargePointInfos.ULongCount(),
                                                          chargePointInfos,
                                                          RequestTimeout,
                                                          result,
                                                          runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnSetChargePointInfosWWCPResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region (private) UpdateChargePointInfos(EVSEs, ...)

        /// <summary>
        /// Upload the EVSE data of the given enumeration of EVSEs.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        private async Task<UpdateEVSEsResult>

            UpdateChargePointInfos(IEnumerable<IEVSE>  EVSEs,

                                   DateTimeOffset?     Timestamp           = null,
                                   EventTracking_Id?   EventTrackingId     = null,
                                   TimeSpan?           RequestTimeout      = null,
                                   CancellationToken   CancellationToken   = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;
            RequestTimeout  ??= CPOClient?.RequestTimeout;

            UpdateEVSEsResult result;

            #endregion

            #region Get effective list of EVSEs/ChargePointInfos to upload

            var warnings          = new List<Warning>();
            var chargePointInfos  = new List<ChargePointInfo>();

            if (EVSEs.IsNeitherNullNorEmpty())
            {
                foreach (var evse in EVSEs)
                {

                    try
                    {

                        if (evse is null)
                            continue;

                        if (IncludeEVSEs(evse) && IncludeEVSEIds(evse.Id))
                            // WWCP EVSE will be added as internal data "WWCP.EVSE"...
                            chargePointInfos.Add(evse.ToOCHP(_CustomEVSEIdMapper,
                                                             _EVSE2ChargePointInfo));

                        else
                            DebugX.Log(evse.Id + " was filtered!");

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e.Message);
                        warnings.Add(Warning.Create(e.Message, evse));
                    }

                }
            }

            #endregion

            #region Send OnSetChargePointInfosWWCPRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnUpdateChargePointInfosWWCPRequest?.Invoke(startTime,
                                                            Timestamp.Value,
                                                            this,
                                                            Id,
                                                            EventTrackingId,
                                                            RoamingNetwork.Id,
                                                            chargePointInfos.ULongCount(),
                                                            chargePointInfos,
                                                            warnings.Where(warning => warning.IsNeitherNullNorEmpty()),
                                                            RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnSetChargePointInfosWWCPRequest));
            }

            #endregion


            DateTimeOffset endtime;
            TimeSpan       runtime;

            if (chargePointInfos.Count > 0)
            {

                var response = await CPORoaming.UpdateChargePointList(
                                         chargePointInfos,
                                         IncludeChargePoints,

                                         Timestamp,
                                         EventTrackingId,
                                         RequestTimeout,
                                         CancellationToken
                                     ).
                                     ConfigureAwait(false);

                endtime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime = endtime - startTime;

                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content        is not null)
                {

                    if (response.Content.Result.ResultCode == ResultCodes.OK)
                        result = UpdateEVSEsResult.Success(
                                     EVSEs,
                                     Id,
                                     this,
                                     EventTrackingId,
                                     //ChargePointInfos.Select(chargePointInfo => chargePointInfo.GetInternalData("WWCP.EVSE") as EVSE),
                                     response.Content.Result.Description.ToI18NString(),
                                     null,
                                     runtime
                                 );

                    else
                        result = UpdateEVSEsResult.Error(
                                     EVSEs,
                                     response.Content.Result.Description.ToI18NString(),
                                     EventTrackingId,
                                     Id,
                                     this,
                                     //ChargePointInfos.Select(chargePointInfo => new PushSingleEVSEDataResult(chargePointInfo.GetInternalData("WWCP.EVSE") as EVSE, PushSingleDataResultTypes.Error)),
                                     null,
                                     runtime
                                 );

                }
                else
                {

                    warnings.Add(Warning.Create(response.HTTPStatusCode.ToString()));
                    warnings.Add(Warning.Create(response.HTTPBody is not null
                                                    ? response.HTTPBody.ToUTF8String()
                                                    : "No HTTP body received!"));

                    result = UpdateEVSEsResult.Error(
                             EVSEs,
                             (response.Content?.Result.Description ?? "error").ToI18NString(),
                             EventTrackingId,
                             Id,
                             this,
                             //chargePointInfos.Select(chargePointInfo => new PushSingleEVSEDataResult(chargePointInfo.GetInternalData("WWCP.EVSE") as EVSE, PushSingleDataResultTypes.Error)),
                             warnings,
                             runtime);

                }

            }

            #region ...or no ChargePointInfos to push...

            else
            {

                endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                runtime  = endtime - startTime;
                result   = UpdateEVSEsResult.NoOperation(
                               EVSEs,
                               Id,
                               this,
                               EventTrackingId,
                               "No ChargePointInfos to push!".ToI18NString(),
                               warnings,
                               org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - startTime
                           );

            }

            #endregion


            #region Send OnUpdateChargePointInfosWWCPResponse event

            try
            {

                OnUpdateChargePointInfosWWCPResponse?.Invoke(endtime,
                                                             Timestamp.Value,
                                                             this,
                                                             Id,
                                                             EventTrackingId,
                                                             RoamingNetwork.Id,
                                                             chargePointInfos.ULongCount(),
                                                             chargePointInfos,
                                                             RequestTimeout,
                                                             result,
                                                             runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnUpdateChargePointInfosWWCPResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region (private) UpdateEVSEStatus(EVSEStatusUpdates, ...)

        /// <summary>
        /// Upload the EVSE status of the given lookup of EVSE status types grouped by their Charging Station Operator.
        /// </summary>
        /// <param name="EVSEStatusUpdates">An enumeration of EVSE status updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<PushEVSEStatusResult>

            UpdateEVSEStatus(IEnumerable<EVSEStatusUpdate>  EVSEStatusUpdates,

                             DateTimeOffset?                Timestamp           = null,
                             EventTracking_Id?              EventTrackingId     = null,
                             TimeSpan?                      RequestTimeout      = null,
                             CancellationToken              CancellationToken   = default)

        {

            #region Initial checks

            if (EVSEStatusUpdates is null)
                throw new ArgumentNullException(nameof(EVSEStatusUpdates), "The given enumeration of EVSE status updates must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (EventTrackingId is null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSEStatus/EVSEStatusRecords to upload

            var warnings    = new List<Warning>();

            var evseStatus  = EVSEStatusUpdates.
                                  Where       (evsestatusupdate => IncludeEVSEIds(evsestatusupdate.Id)).
                                  ToLookup    (evsestatusupdate => evsestatusupdate.Id,
                                               evsestatusupdate => evsestatusupdate).
                                  ToDictionary(group            => group.Key,
                                               group            => group.AsEnumerable().OrderByDescending(item => item.NewStatus.Timestamp)).
                                  Select      (evsestatusupdate => {

                                      try
                                      {

                                          var _EVSEId = evsestatusupdate.Key.ToOCHP(_CustomEVSEIdMapper);

                                          if (!_EVSEId.HasValue)
                                              throw new InvalidEVSEIdentificationException(evsestatusupdate.Key.ToString());

                                          // Only push the current status of the latest status update!
                                          return new EVSEStatus(
                                                         _EVSEId.Value,
                                                         evsestatusupdate.Value.First().NewStatus.Value.AsEVSEMajorStatus(),
                                                         evsestatusupdate.Value.First().NewStatus.Value.AsEVSEMinorStatus()
                                                     );

                                      }
                                      catch (Exception e)
                                      {
                                          DebugX.  Log(e.Message);
                                          warnings.Add(Warning.Create(e.Message, evsestatusupdate));
                                      }

                                      return null;

                                  }).
                                  Where(evsestatusrecord => evsestatusrecord is not null).
                                  Cast<EVSEStatus>().
                                  ToArray();

            PushEVSEStatusResult? result  = null;

            #endregion

            #region Send OnUpdateEVSEStatusWWCPRequest event

            var startTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnUpdateEVSEStatusWWCPRequest?.Invoke(startTime,
                                                      Timestamp.Value,
                                                      this,
                                                      Id,
                                                      EventTrackingId,
                                                      RoamingNetwork.Id,
                                                      evseStatus.ULongCount(),
                                                      evseStatus,
                                                      warnings.Where(warning => warning.IsNeitherNullNorEmpty()),
                                                      RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnUpdateEVSEStatusWWCPRequest));
            }

            #endregion


            var response = await CPORoaming.UpdateStatus(
                                     evseStatus,
                                     null,
                                     org.GraphDefined.Vanaheimr.Illias.Timestamp.Now + EVSEStatusRefreshEvery,
                                     null,

                                     Timestamp,
                                     EventTrackingId,
                                     RequestTimeout,
                                     CancellationToken
                                 ).
                                 ConfigureAwait(false);


            var endtime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            var runtime = endtime - startTime;

            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        is not null)
            {

                if (response.Content.Result.ResultCode == ResultCodes.OK)
                    result = PushEVSEStatusResult.Success(Id,
                                                          this,
                                                          response.Content.Result.Description,
                                                          null,
                                                          runtime);

                else
                    result = PushEVSEStatusResult.Error(Id,
                                                        this,
                                                        EVSEStatusUpdates,
                                                        response.Content.Result.Description,
                                                        null,
                                                        runtime);

            }
            else
                result = PushEVSEStatusResult.Error(Id,
                                                    this,
                                                    EVSEStatusUpdates,
                                                    response.HTTPStatusCode.ToString(),
                                                    response.HTTPBody is not null
                                                        ? warnings.AddAndReturnList(I18NString.Create(response.HTTPBody.ToUTF8String()))
                                                        : warnings.AddAndReturnList(I18NString.Create("No HTTP body received!")),
                                                    runtime);


            #region Send OnUpdateEVSEStatusWWCPResponse event

            try
            {

                OnUpdateEVSEStatusWWCPResponse?.Invoke(endtime,
                                                       Timestamp.Value,
                                                       this,
                                                       Id,
                                                       EventTrackingId,
                                                       RoamingNetwork.Id,
                                                       evseStatus.ULongCount(),
                                                       evseStatus,
                                                       RequestTimeout,
                                                       result,
                                                       runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnUpdateEVSEStatusWWCPResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region (Set/Add/Update/Delete) EVSE(s)...

        #region AddEVSE         (EVSE, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given EVSE to the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="EVSE">An EVSE to upload.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public override async Task<AddEVSEResult>

            AddEVSE(IEVSE               EVSE,
                    TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                    DateTimeOffset?     Timestamp           = null,
                    EventTracking_Id?   EventTrackingId     = null,
                    TimeSpan?           RequestTimeout      = null,
                    User_Id?            CurrentUserId       = null,
                    CancellationToken   CancellationToken   = default)

        {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                lock (ServiceCheckLock)
                {

                    if (IncludeEVSEs(EVSE))
                    {

                        evsesToAddQueue.Add(EVSE);

                        ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                    }

                }

                return AddEVSEResult.Enqueued(
                           EVSE,
                           EventTrackingId,
                           Id,
                           this
                       );

            }

            #endregion

            var result = await UpdateChargePointInfos(new IEVSE[] { EVSE },

                                                      Timestamp,
                                                      EventTrackingId,
                                                      RequestTimeout,
                                                      CancellationToken);

            return new AddEVSEResult(
                       EVSE,
                       result.Result,
                       result.EventTrackingId,
                       result.SenderId,
                       result.Sender,
                       null,
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region AddOrUpdateEVSE (EVSE, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the given EVSE as new static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="EVSE">An EVSE to upload.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<AddOrUpdateEVSEResult>

            AddOrUpdateEVSE(IEVSE               EVSE,
                            TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                            DateTimeOffset?     Timestamp           = null,
                            EventTracking_Id?   EventTrackingId     = null,
                            TimeSpan?           RequestTimeout      = null,
                            User_Id?            CurrentUserId       = null,
                            CancellationToken   CancellationToken   = default)

        {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                lock (ServiceCheckLock)
                {

                    if (IncludeEVSEs(EVSE))
                    {

                        evsesToAddQueue.Add(EVSE);

                        ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                    }

                }

                return AddOrUpdateEVSEResult.Enqueued(
                           EVSE,
                           EventTrackingId,
                           Id,
                           this
                       );

            }

            #endregion

            var result = await UpdateChargePointInfos(new IEVSE[] { EVSE },

                                                      Timestamp,
                                                      EventTrackingId,
                                                      RequestTimeout,
                                                      CancellationToken);

            return new AddOrUpdateEVSEResult(
                       EVSE,
                       result.Result,
                       result.EventTrackingId,
                       result.SenderId,
                       result.Sender,
                       null,
                       AddedOrUpdated.Update,
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region UpdateEVSE      (EVSE, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the static data of the given EVSE.
        /// The EVSE can be uploaded as a whole, or just a single property of the EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to update.</param>
        /// <param name="PropertyName">The name of the EVSE property to update.</param>
        /// <param name="OldValue">The old value of the EVSE property to update.</param>
        /// <param name="NewValue">The new value of the EVSE property to update.</param>
        /// <param name="TransmissionType">Whether to send the EVSE update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public override async Task<UpdateEVSEResult>

            UpdateEVSE(IEVSE               EVSE,
                       String              PropertyName,
                       Object?             NewValue,
                       Object?             OldValue,
                       Context?            DataSource,
                       TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                       DateTimeOffset?     Timestamp           = null,
                       EventTracking_Id?   EventTrackingId     = null,
                       TimeSpan?           RequestTimeout      = null,
                       User_Id?            CurrentUserId       = null,
                       CancellationToken   CancellationToken   = default)

        {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                lock (ServiceCheckLock)
                {

                    if (IncludeEVSEs(EVSE))
                    {

                        evsesToUpdateQueue.Add(EVSE);

                        ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                    }

                }

                return UpdateEVSEResult.Enqueued(
                           EVSE,
                           EventTrackingId,
                           Id,
                           this
                       );

            }

            #endregion

            var result = await UpdateChargePointInfos(new IEVSE[] { EVSE },

                                                      Timestamp,
                                                      EventTrackingId,
                                                      RequestTimeout,
                                                      CancellationToken);

            return new UpdateEVSEResult(
                       EVSE,
                       result.Result,
                       result.EventTrackingId,
                       result.SenderId,
                       result.Sender,
                       null,
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion


        #region AddEVSEs        (EVSEs, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs to the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public override async Task<AddEVSEsResult>

            AddEVSEs(IEnumerable<IEVSE>  EVSEs,
                     TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                     DateTimeOffset?     Timestamp           = null,
                     EventTracking_Id?   EventTrackingId     = null,
                     TimeSpan?           RequestTimeout      = null,
                     User_Id?            CurrentUserId       = null,
                     CancellationToken   CancellationToken   = default)

        {

            #region Initial checks

            if (!EVSEs.Any())
                return AddEVSEsResult.NoOperation(
                           EVSEs,
                           Id,
                           this,
                           EventTrackingId
                       );

            #endregion

            var result = await UpdateChargePointInfos(EVSEs,

                                                      Timestamp,
                                                      EventTrackingId,
                                                      RequestTimeout,
                                                      CancellationToken);

            return new AddEVSEsResult(

                       result.Result,

                       result.SuccessfulItems.Select(updateEVSEResult => new AddEVSEResult(
                                                                             updateEVSEResult.EVSE,
                                                                             updateEVSEResult.Result,
                                                                             updateEVSEResult.EventTrackingId,
                                                                             updateEVSEResult.SenderId,
                                                                             updateEVSEResult.Sender,
                                                                             null,
                                                                             updateEVSEResult.Description,
                                                                             updateEVSEResult.Warnings,
                                                                             updateEVSEResult.Runtime
                                                                         )),

                       result.RejectedItems.  Select(updateEVSEResult => new AddEVSEResult(
                                                                             updateEVSEResult.EVSE,
                                                                             updateEVSEResult.Result,
                                                                             updateEVSEResult.EventTrackingId,
                                                                             updateEVSEResult.SenderId,
                                                                             updateEVSEResult.Sender,
                                                                             null,
                                                                             updateEVSEResult.Description,
                                                                             updateEVSEResult.Warnings,
                                                                             updateEVSEResult.Runtime
                                                                         )),

                       result.SenderId,
                       result.Sender,
                       null,
                       result.Description,
                       result.Warnings,
                       result.Runtime

                   );

        }

        #endregion

        #region AddOrUpdateEVSEs(EVSEs, ...)

        /// <summary>
        /// Set the given enumeration of EVSEs as new static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<AddOrUpdateEVSEsResult>

            AddOrUpdateEVSEs(IEnumerable<IEVSE>  EVSEs,
                             TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                             DateTimeOffset?     Timestamp           = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null,
                             User_Id?            CurrentUserId       = null,
                             CancellationToken   CancellationToken   = default)

        {

            #region Initial checks

            if (!EVSEs.Any())
                return AddOrUpdateEVSEsResult.NoOperation(
                           EVSEs,
                           Id,
                           this,
                           EventTrackingId
                       );

            #endregion

            var result = await UpdateChargePointInfos(EVSEs,

                                                      Timestamp,
                                                      EventTrackingId,
                                                      RequestTimeout,
                                                      CancellationToken);

            return new AddOrUpdateEVSEsResult(

                       result.Result,

                       result.SuccessfulItems.Select(updateEVSEResult => new AddOrUpdateEVSEResult(
                                                                             updateEVSEResult.EVSE,
                                                                             updateEVSEResult.Result,
                                                                             updateEVSEResult.EventTrackingId,
                                                                             updateEVSEResult.SenderId,
                                                                             updateEVSEResult.Sender,
                                                                             null,
                                                                             AddedOrUpdated.Update,
                                                                             updateEVSEResult.Description,
                                                                             updateEVSEResult.Warnings,
                                                                             updateEVSEResult.Runtime
                                                                         )),

                       result.RejectedItems.  Select(updateEVSEResult => new AddOrUpdateEVSEResult(
                                                                             updateEVSEResult.EVSE,
                                                                             updateEVSEResult.Result,
                                                                             updateEVSEResult.EventTrackingId,
                                                                             updateEVSEResult.SenderId,
                                                                             updateEVSEResult.Sender,
                                                                             null,
                                                                             AddedOrUpdated.Update,
                                                                             updateEVSEResult.Description,
                                                                             updateEVSEResult.Warnings,
                                                                             updateEVSEResult.Runtime
                                                                         )),

                       result.SenderId,
                       result.Sender,
                       null,
                       result.Description,
                       result.Warnings,
                       result.Runtime

                   );

        }

        #endregion

        #region UpdateEVSEs     (EVSEs, ...)

        /// <summary>
        /// Update the given enumeration of EVSEs within the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public override async Task<UpdateEVSEsResult>

            UpdateEVSEs(IEnumerable<IEVSE>  EVSEs,
                        TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                        DateTimeOffset?     Timestamp           = null,
                        EventTracking_Id?   EventTrackingId     = null,
                        TimeSpan?           RequestTimeout      = null,
                        User_Id?            CurrentUserId       = null,
                        CancellationToken   CancellationToken   = default)

        {

            #region Initial checks

            if (!EVSEs.Any())
                return UpdateEVSEsResult.NoOperation(
                           EVSEs,
                           Id,
                           this,
                           EventTrackingId
                       );

            #endregion

            return await UpdateChargePointInfos(EVSEs,

                                                Timestamp,
                                                EventTrackingId,
                                                RequestTimeout,
                                                CancellationToken);

        }

        #endregion


        #region UpdateStatus    (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of EVSE status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<PushEVSEStatusResult>

            UpdateStatus(IEnumerable<EVSEStatusUpdate>  StatusUpdates,
                         TransmissionTypes              TransmissionType    = TransmissionTypes.Enqueue,

                         DateTimeOffset?                Timestamp           = null,
                         EventTracking_Id?              EventTrackingId     = null,
                         TimeSpan?                      RequestTimeout      = null,
                         User_Id?                       CurrentUserId       = null,
                         CancellationToken              CancellationToken   = default)

        {

            #region Initial checks

            if (!StatusUpdates.Any())
                return PushEVSEStatusResult.NoOperation(
                           Id,
                           this
                       );

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                //                                    Timestamp.Value,
                //                                    this,
                //                                    EventTrackingId,
                //                                    RoamingNetwork.Id,
                //                                    ChargeDetailRecord,
                //                                    RequestTimeout);

                //}
                //catch (Exception e)
                //{
                //    DebugX.LogException(e, nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion


                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        var FilteredUpdates = StatusUpdates.Where(statusupdate => IncludeEVSEIds(statusupdate.Id)).
                                                            ToArray();

                        if (FilteredUpdates.Length > 0)
                        {

                            foreach (var Update in FilteredUpdates)
                            {

                                // Delay the status update until the EVSE data had been uploaded!
                                if (evsesToAddQueue.Any(evse => evse.Id == Update.Id))
                                    evseStatusChangesDelayedQueue.Add(Update);

                                else
                                    evseStatusChangesFastQueue.Add(Update);

                            }

                            FlushEVSEFastStatusTimer.Change(FlushEVSEFastStatusEvery, TimeSpan.FromMilliseconds(-1));

                            return PushEVSEStatusResult.Enqueued(Id, this);

                        }

                        return PushEVSEStatusResult.NoOperation(Id, this);

                    }

                }
                finally
                {
                    if (LockTaken)
                        DataAndStatusLock.Release();
                }

            }

            #endregion


            return await UpdateEVSEStatus(StatusUpdates,

                                          Timestamp,
                                          EventTrackingId,
                                          RequestTimeout,
                                          CancellationToken);

        }

        #endregion

        #endregion

        #endregion


        #region AuthorizeStart(           LocalAuthentication, ChargingLocation = null, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging location.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(LocalAuthentication               LocalAuthentication,
                           ChargingLocation?                 ChargingLocation      = null,
                           ChargingProduct?                  ChargingProduct       = null,   // [maxlength: 100]
                           ChargingSession_Id?               SessionId             = null,
                           ChargingSession_Id?               CPOPartnerSessionId   = null,
                           WWCP.ChargingStationOperator_Id?  OperatorId            = null,

                           DateTimeOffset?                   Timestamp             = null,
                           EventTracking_Id?                 EventTrackingId       = null,
                           TimeSpan?                         RequestTimeout        = null,
                           CancellationToken                 CancellationToken     = default)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (EventTrackingId is null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnAuthorizeStartRequest?.Invoke(StartTime,
                                                Timestamp.Value,
                                                this,
                                                Id.ToString(),
                                                EventTrackingId,
                                                RoamingNetwork.Id,
                                                null,
                                                Id,
                                                OperatorId,
                                                LocalAuthentication,
                                                ChargingLocation,
                                                ChargingProduct,
                                                SessionId,
                                                CPOPartnerSessionId,
                                                new ISendAuthorizeStartStop[0],
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            DateTimeOffset   Endtime;
            TimeSpan         Runtime;
            AuthStartResult  result;

            if (DisableAuthorization)
            {

                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;
                result   = AuthStartResult.AdminDown(Id,
                                                     this,
                                                     SessionId:  SessionId,
                                                     Runtime:    Runtime);

            }

            else
            {

                var EMTId     = new EMT_Id(
                                    LocalAuthentication.AuthToken.ToString(),
                                    TokenRepresentations.Plain,
                                    TokenTypes.RFID
                                );

                var response  = await CPORoaming.GetSingleRoamingAuthorisation(
                                          EMTId,

                                          Timestamp,
                                          EventTrackingId,
                                          RequestTimeout,
                                          CancellationToken
                                      );


                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;

                if (response?.HTTPStatusCode            == HTTPStatusCode.OK &&
                    response?.Content                   is not null          &&
                    response?.Content.Result.ResultCode == ResultCodes.OK)
                {

                    result = AuthStartResult.Authorized(Id,
                                                        this,
                                                        SessionId:      ChargingSession_Id.NewRandom(),
                                                        ProviderId:     response.Content.RoamingAuthorisationInfo is not null
                                                                            ? response.Content.RoamingAuthorisationInfo.ContractId.ProviderId.ToWWCP()
                                                                            : EMobilityProvider_Id.Parse(Country.Germany, "GEF"),
                                                        ContractId:     response.Content.RoamingAuthorisationInfo.ContractId.ToString(),
                                                        PrintedNumber:  response.Content.RoamingAuthorisationInfo.PrintedNumber,
                                                        ExpiryDate:     response.Content.RoamingAuthorisationInfo.ExpiryDate,
                                                        Runtime:        Runtime);

                    lock (lookup)
                    {

                        if (lookup.TryGetValue(EMTId, out Contract_Id ExistingContractId))
                        {

                            if (ExistingContractId != response.Content.RoamingAuthorisationInfo.ContractId)
                            {

                                // Replace

                            }

                        }

                        else
                        {

                            // Add
                            lookup.Add(EMTId, response.Content.RoamingAuthorisationInfo.ContractId);

                            var time = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                            var file = String.Concat(Directory.GetCurrentDirectory(), Path.DirectorySeparatorChar, "OCHPv1.4", Path.DirectorySeparatorChar, "EMTIds_2_ContractIds_", time.ToUniversalTime().Year, "-", time.ToUniversalTime().Month.ToString("D2"), ".log");

                            File.AppendAllText(file, String.Concat("ADD", rs, EMTId.Instance, rs, response.Content.RoamingAuthorisationInfo.ContractId));

                        }

                    }

                }

                else
                {

                    result = AuthStartResult.NotAuthorized(Id,
                                                           this,
                                                           // response.Content.ProviderId.ToWWCP(),
                                                           Runtime: Runtime);

                    lock (lookup)
                    {

                        // Remove

                        lookup.Remove(EMTId);

                    }

                }

            }


            #region Send OnAuthorizeStartResponse event

            try
            {

                OnAuthorizeStartResponse?.Invoke(Endtime,
                                                 Timestamp.Value,
                                                 this,
                                                 Id.ToString(),
                                                 EventTrackingId,
                                                 RoamingNetwork.Id,
                                                 null,
                                                 Id,
                                                 OperatorId,
                                                 LocalAuthentication,
                                                 ChargingLocation,
                                                 ChargingProduct,
                                                 SessionId,
                                                 CPOPartnerSessionId,
                                                 new ISendAuthorizeStartStop[0],
                                                 RequestTimeout,
                                                 result,
                                                 Runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop (SessionId, LocalAuthentication, ChargingLocation = null,                                           OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging location.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingLocation">The charging location.</param>
        /// <param name="CPOPartnerSessionId">An optional session identification of the CPO.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id                SessionId,
                          LocalAuthentication               LocalAuthentication,
                          ChargingLocation?                 ChargingLocation      = null,
                          ChargingSession_Id?               CPOPartnerSessionId   = null,
                          WWCP.ChargingStationOperator_Id?  OperatorId            = null,

                          DateTimeOffset?                   Timestamp             = null,
                          EventTracking_Id?                 EventTrackingId       = null,
                          TimeSpan?                         RequestTimeout        = null,
                          CancellationToken                 CancellationToken     = default)
        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (EventTrackingId is null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnAuthorizeStopRequest?.Invoke(StartTime,
                                                   Timestamp.Value,
                                                   this,
                                                   Id.ToString(),
                                                   EventTrackingId,
                                                   RoamingNetwork.Id,
                                                   null,
                                                   Id,
                                                   OperatorId,
                                                   ChargingLocation,
                                                   SessionId,
                                                   CPOPartnerSessionId,
                                                   LocalAuthentication,
                                                   RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            DateTimeOffset  Endtime;
            TimeSpan        Runtime;
            AuthStopResult  result;

            if (DisableAuthorization)
            {
                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;
                result   = AuthStopResult.AdminDown(Id,
                                                    this,
                                                    SessionId:  SessionId,
                                                    Runtime:    Runtime);
            }

            else
            {

                var response = await CPORoaming.GetSingleRoamingAuthorisation(
                                         new EMT_Id(
                                             LocalAuthentication.AuthToken?.ToString(),
                                             TokenRepresentations.Plain,
                                             TokenTypes.RFID
                                         ),

                                         Timestamp,
                                         EventTrackingId,
                                         RequestTimeout,
                                         CancellationToken
                                     ).ConfigureAwait(false);


                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;

                if (response?.HTTPStatusCode            == HTTPStatusCode.OK &&
                    response?.Content                   is not null &&
                    response?.Content.Result.ResultCode == ResultCodes.OK)
                {

                    result = AuthStopResult.Authorized(Id,
                                                       this,
                                                       SessionId:   ChargingSession_Id.NewRandom(),
                                                       ProviderId:  response.Content.RoamingAuthorisationInfo.ContractId.ProviderId.ToWWCP(),
                                                       Runtime:     Runtime);

                }

                else
                    result = AuthStopResult.NotAuthorized(Id,
                                                          this,
                                                          // response.Content.ProviderId.ToWWCP(),
                                                          Runtime: Runtime);

            }


            #region Send OnAuthorizeStopResponse event

            try
            {

                OnAuthorizeStopResponse?.Invoke(Endtime,
                                                    Timestamp.Value,
                                                    this,
                                                    Id.ToString(),
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    null,
                                                    Id,
                                                    OperatorId,
                                                    ChargingLocation,
                                                    SessionId,
                                                    CPOPartnerSessionId,
                                                    LocalAuthentication,
                                                    RequestTimeout,
                                                    result,
                                                    Runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SendChargeDetailRecord (ChargeDetailRecord,  TransmissionType = Enqueue, ...)

        /// <summary>
        /// Send a charge detail record to an OCHP server.
        /// </summary>
        /// <param name="ChargeDetailRecord">A charge detail record.</param>
        /// <param name="TransmissionType">Whether to send the CDR directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<SendCDRResult>

            SendChargeDetailRecord(ChargeDetailRecord  ChargeDetailRecord,
                                   TransmissionTypes   TransmissionType    = TransmissionTypes.Enqueue,

                                   DateTimeOffset?     Timestamp           = null,
                                   EventTracking_Id?   EventTrackingId     = null,
                                   TimeSpan?           RequestTimeout      = null,
                                   CancellationToken   CancellationToken   = default)

            => (await SendChargeDetailRecords(
                      [ ChargeDetailRecord ],
                      TransmissionType,
                      Timestamp,
                      EventTrackingId,
                      RequestTimeout,
                      CancellationToken)).First();

        #endregion

        #region SendChargeDetailRecords(ChargeDetailRecords, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Send charge detail records to an OCHP server.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="TransmissionType">Whether to send the CDRs directly or enqueue them for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<SendCDRsResult>

            SendChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                                    TransmissionTypes                TransmissionType    = TransmissionTypes.Enqueue,

                                    DateTimeOffset?                  Timestamp           = null,
                                    EventTracking_Id?                EventTrackingId     = null,
                                    TimeSpan?                        RequestTimeout      = null,
                                    CancellationToken                CancellationToken   = default)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (EventTrackingId is null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Filter charge detail records

            var ForwardedCDRs  = new List<ChargeDetailRecord>();
            var FilteredCDRs   = new List<SendCDRResult>();

            foreach (var cdr in ChargeDetailRecords)
            {

                if (ChargeDetailRecordFilter(cdr) == ChargeDetailRecordFilters.forward)
                    ForwardedCDRs.Add(cdr);

                else
                    FilteredCDRs.Add(
                        SendCDRResult.Filtered(
                            org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                            Id,
                            cdr,
                            Warnings: Warnings.Create("This charge detail record was filtered!")
                        )
                    );

            }

            #endregion

            #region Send OnSendCDRsRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnSendCDRsRequest?.Invoke(StartTime,
                                          Timestamp.Value,
                                          this,
                                          Id.ToString(),
                                          EventTrackingId,
                                          RoamingNetwork.Id,
                                          ChargeDetailRecords,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnSendCDRsRequest));
            }

            #endregion


            #region if disabled => 'AdminDown'...

            DateTimeOffset  Endtime;
            TimeSpan        Runtime;
            SendCDRsResult  results;

            if (DisableSendChargeDetailRecords)
            {

                Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                Runtime  = Endtime - StartTime;
                results  = SendCDRsResult.AdminDown(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                    Id,
                                                    this,
                                                    ChargeDetailRecords,
                                                    Runtime: Runtime);

            }

            #endregion

            else
            {

                var invokeTimer  = false;
                var LockTaken    = await FlushChargeDetailRecordsLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        #region if enqueuing is requested...

                        if (TransmissionType == TransmissionTypes.Enqueue)
                        {

                            #region Send OnEnqueueSendCDRRequest event

                            try
                            {

                                OnEnqueueSendCDRsRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                 Timestamp.Value,
                                                                 this,
                                                                 Id.ToString(),
                                                                 EventTrackingId,
                                                                 RoamingNetwork.Id,
                                                                 ChargeDetailRecords,
                                                                 RequestTimeout);

                            }
                            catch (Exception e)
                            {
                                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnSendCDRsRequest));
                            }

                            #endregion

                            var EnquenedCDRsResults = new List<SendCDRResult>();

                            foreach (var chargeDetailRecord in ForwardedCDRs)
                            {

                                try
                                {

                                    chargeDetailRecordsQueue.Add(
                                        chargeDetailRecord.ToOCHP(
                                            ContractIdDelegate:  emtid => lookup[emtid],
                                            CustomEVSEIdMapper:  null
                                        )
                                    );

                                    EnquenedCDRsResults.Add(
                                        SendCDRResult.Enqueued(
                                            org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                            Id,
                                            chargeDetailRecord
                                        )
                                    );

                                }
                                catch (Exception e)
                                {
                                    EnquenedCDRsResults.Add(
                                        SendCDRResult.CouldNotConvertCDRFormat(
                                            org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                            Id,
                                            chargeDetailRecord,
                                            Warnings: Warnings.Create(e.Message)
                                        )
                                    );
                                }

                            }

                            Endtime      = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                            Runtime      = Endtime - StartTime;
                            results      = (!FilteredCDRs.Any())
                                               ? SendCDRsResult.Enqueued(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, Id, this, ForwardedCDRs, I18NString.Create("Enqueued for at least " + FlushChargeDetailRecordsEvery.TotalSeconds + " seconds!"), Runtime: Runtime)
                                               : SendCDRsResult.Mixed   (org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, Id, this, FilteredCDRs.Concat(ForwardedCDRs.Select(cdr => SendCDRResult.Enqueued(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, Id, cdr))), Runtime: Runtime);
                            invokeTimer  = true;

                        }

                        #endregion

                        #region ...or send at once!

                        else
                        {

                            HTTPResponse<AddCDRsResponse> response;

                            try
                            {

                                response = await CPORoaming.AddCDRs(
                                                     ForwardedCDRs.Select(cdr => cdr.ToOCHP(ContractIdDelegate:  emtid => lookup[emtid],
                                                                                            CustomEVSEIdMapper:  null)).ToArray(),

                                                     Timestamp,
                                                     EventTrackingId,
                                                     RequestTimeout,
                                                     CancellationToken
                                                 );

                                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                                    response.Content        is not null)
                                {

                                    var ImplausibleCDRIds = response.Content.ImplausibleCDRs?.ToHashSet() ?? new HashSet<CDR_Id>();
                                    var ImplausibleCDRs   = response.Content.ImplausibleCDRs.SafeAny()
                                                                ? ChargeDetailRecords.Where(cdr => ImplausibleCDRIds.Contains(cdr.SessionId.ToOCHP()))
                                                                : [];

                                    switch (response.Content.Result.ResultCode)
                                    {

                                        case ResultCodes.OK:
                                            if (FilteredCDRs.Count == 0)
                                                results = SendCDRsResult.Success(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, Id, this, ForwardedCDRs);
                                            else
                                                results = SendCDRsResult.Mixed  (org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, Id, this, FilteredCDRs.Concat(ForwardedCDRs.Select(cdr => SendCDRResult.Success(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, Id, cdr))));
                                            break;


                                        case ResultCodes.Partly:
                                            if (FilteredCDRs.Count == 0)
                                                results = SendCDRsResult.Mixed  (
                                                              org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                              Id,
                                                              this,
                                                              ForwardedCDRs.  Select(cdr => SendCDRResult.Success(
                                                                                                org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                                                Id,
                                                                                                cdr
                                                                                            )).Concat(
                                                                                                   ImplausibleCDRs.Select(cdr => SendCDRResult.Error(
                                                                                                                                     org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                                                                                     Id,
                                                                                                                                     cdr,
                                                                                                                                     Warnings: Warnings.Create("Implausible charge detail record!")
                                                                                                                                 ))
                                                                                               )
                                                          );
                                            else
                                                results = SendCDRsResult.Mixed  (
                                                              org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                              Id,
                                                              this,
                                                              FilteredCDRs.Concat(
                                                                  ForwardedCDRs.  Select(cdr => SendCDRResult.Success(
                                                                                                    org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                                                    Id,
                                                                                                    cdr
                                                                                                )).Concat(
                                                                                                       ImplausibleCDRs.Select(cdr => SendCDRResult.Error(
                                                                                                                                         org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                                                                                         Id,
                                                                                                                                         cdr,
                                                                                                                                         Warnings: Warnings.Create("Implausible charge detail record!")
                                                                                                                                     ))
                                                                                                   )
                                                              )
                                                          );
                                            break;


                                        default:
                                            if (FilteredCDRs.Count == 0)
                                                results = SendCDRsResult.Error(
                                                              org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                              Id,
                                                              this,
                                                              ForwardedCDRs,
                                                              Warning.Create(response.Content.Result.ResultCode + " - " + response.Content.Result.Description)
                                                          );

                                            else
                                                results = SendCDRsResult.Mixed(
                                                              org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                              Id,
                                                              this,
                                                              FilteredCDRs.Concat(
                                                                  ForwardedCDRs.Select(cdr => SendCDRResult.Error(
                                                                                                  org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                                                  Id,
                                                                                                  cdr,
                                                                                                  Warnings: Warnings.Create($"{response.Content.Result.ResultCode} - {response.Content.Result.Description}")
                                                                                              ))
                                                              )
                                                          );
                                            break;

                                    }

                                }

                                else
                                    if (FilteredCDRs.Count == 0)
                                        results = SendCDRsResult.Error(
                                                      org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                      Id,
                                                      this,
                                                      ForwardedCDRs,
                                                      Warning.Create(response.HTTPBodyAsUTF8String)
                                                  );

                                    else
                                        results = SendCDRsResult.Mixed(
                                                      org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                      Id,
                                                      this,
                                                      FilteredCDRs.Concat(
                                                          ForwardedCDRs.Select(cdr => SendCDRResult.Error(
                                                                                          org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                                          Id,
                                                                                          cdr,
                                                                                          Warnings: Warnings.Create(response.HTTPBodyAsUTF8String)
                                                                                      ))
                                                      )
                                                  );

                            }
                            catch (Exception e)
                            {
                                if (FilteredCDRs.Count == 0)
                                    results = SendCDRsResult.Error(
                                                  org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                  Id,
                                                  this,
                                                  ForwardedCDRs,
                                                  Warning.Create(e.Message)
                                              );

                                else
                                    results = SendCDRsResult.Mixed(
                                                  org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                  Id,
                                                  this,
                                                  FilteredCDRs.Concat(
                                                      ForwardedCDRs.Select(cdr => SendCDRResult.Error(
                                                                                      org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                                                      Id,
                                                                                      cdr,
                                                                                      Warnings: Warnings.Create(e.Message)
                                                                                  ))
                                                  )
                                              );
                            }


                            Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                            Runtime  = Endtime - StartTime;

                            await RoamingNetwork.ReceiveSendChargeDetailRecordResults(results);

                        }

                        #endregion

                    }

                    #region Could not get the lock for toooo long!

                    else
                    {

                        Endtime  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                        Runtime  = Endtime - StartTime;
                        results  = SendCDRsResult.Timeout(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                          Id,
                                                          this,
                                                          ChargeDetailRecords,
                                                          I18NString.Create("Could not " + (TransmissionType == TransmissionTypes.Enqueue ? "enqueue" : "send") + " charge detail records!"),
                                                          //ChargeDetailRecords.SafeSelect(cdr => new SendCDRResult(cdr, SendCDRResultTypes.Timeout)),
                                                          Runtime: Runtime);

                    }

                    #endregion

                }
                finally
                {
                    if (LockTaken)
                        FlushChargeDetailRecordsLock.Release();
                }

                if (invokeTimer)
                    FlushChargeDetailRecordsTimer.Change(FlushChargeDetailRecordsEvery, TimeSpan.FromMilliseconds(-1));

            }


            #region Send OnSendCDRsResponse event

            try
            {

                OnSendCDRsResponse?.Invoke(Endtime,
                                           Timestamp.Value,
                                           this,
                                           Id.ToString(),
                                           EventTrackingId,
                                           RoamingNetwork.Id,
                                           ChargeDetailRecords,
                                           RequestTimeout,
                                           results,
                                           Runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPCSOAdapter) + "." + nameof(OnSendCDRsResponse));
            }

            #endregion

            return results;

        }

        #endregion


        // -----------------------------------------------------------------------------------------------------


        #region (timer) ServiceCheck(State)

        private void ServiceCheck(Object State)
        {

            if (!DisablePushData)
            {

                try
                {

                    FlushServiceQueues().Wait();

                }
                catch (Exception e)
                {

                    while (e.InnerException is not null)
                        e = e.InnerException;

                    DebugX.Log("A exception occured during ServiceCheck: " + e.Message + Environment.NewLine + e.StackTrace);

                    OnWWCPCPOAdapterException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                      this,
                                                      e);

                }

            }

        }

        public async Task FlushServiceQueues()
        {

            FlushServiceQueuesEvent?.Invoke(this, TimeSpan.FromMilliseconds(_ServiceCheckEvery));

            #region Make a thread local copy of all data

            //ToDo: AsyncLocal is currently not implemented in Mono!
            //var evseDataQueueCopy   = new AsyncLocal<HashSet<EVSE>>();
            //var EVSEStatusQueueCopy = new AsyncLocal<List<EVSEStatusChange>>();

            var evsesToAddQueueCopy                = new ThreadLocal<HashSet<IEVSE>>();
            var evseDataQueueCopy                  = new ThreadLocal<HashSet<IEVSE>>();
            var evseStatusChangesDelayedQueueCopy  = new ThreadLocal<List<EVSEStatusUpdate>>();
            var evsesToRemoveQueueCopy             = new ThreadLocal<HashSet<IEVSE>>();

            if (Monitor.TryEnter(ServiceCheckLock))
            {

                try
                {

                    if (evsesToAddQueue.              Count == 0 &&
                        evsesToUpdateQueue.           Count == 0 &&
                        evseStatusChangesDelayedQueue.Count == 0 &&
                        evsesToRemoveQueue.           Count == 0 &&
                        chargeDetailRecordsQueue.     Count == 0)
                    {
                        return;
                    }

                    serviceRunId++;

                    // Copy 'EVSEs to add', remove originals...
                    evsesToAddQueueCopy.Value                = new HashSet<IEVSE>         (evsesToAddQueue);
                    evsesToAddQueue.Clear();

                    // Copy 'EVSEs to update', remove originals...
                    evseDataQueueCopy.Value                  = new HashSet<IEVSE>         (evsesToUpdateQueue);
                    evsesToUpdateQueue.Clear();

                    // Copy 'EVSE status changes', remove originals...
                    evseStatusChangesDelayedQueueCopy.Value  = new List<EVSEStatusUpdate>(evseStatusChangesDelayedQueue);
                    evseStatusChangesDelayedQueueCopy.Value.AddRange(evsesToAddQueueCopy.Value.SafeSelect(evse => new EVSEStatusUpdate(evse.Id, evse.Status, evse.Status)));
                    evseStatusChangesDelayedQueue.Clear();

                    // Copy 'EVSEs to remove', remove originals...
                    evsesToRemoveQueueCopy.Value             = new HashSet<IEVSE>         (evsesToRemoveQueue);
                    evsesToRemoveQueue.Clear();

                    // Stop the timer. Will be rescheduled by next EVSE data/status change...
                    ServiceCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);

                }
                catch (Exception e)
                {

                    while (e.InnerException is not null)
                        e = e.InnerException;

                    DebugX.LogT(nameof(WWCPCSOAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                }

                finally
                {
                    Monitor.Exit(ServiceCheckLock);
                }

            }

            else
            {

                Console.WriteLine("ServiceCheckLock missed!");
                ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

            }

            #endregion

            // Upload status changes...
            if (evsesToAddQueueCopy.              Value is not null ||
                evseDataQueueCopy.                Value is not null ||
                evseStatusChangesDelayedQueueCopy.Value is not null ||
                evsesToRemoveQueueCopy.           Value is not null)
            {

                // Use the events to evaluate if something went wrong!

                var now                      = Timestamp.Now;
                var eventTrackingId          = EventTracking_Id.New;
                var cancellationTokenSource  = new CancellationTokenSource();

                #region Send new EVSE data

                if (evsesToAddQueueCopy.Value is not null &&
                    evsesToAddQueueCopy.Value.Count > 0)
                {

                    if (serviceRunId == 1)
                        await ReplaceEVSEs(
                                  evsesToAddQueueCopy.Value,
                                  TransmissionTypes.Direct,
                                  now,
                                  eventTrackingId,
                                  null,
                                  null,
                                  cancellationTokenSource.Token
                              );

                    else
                        await AddEVSEs(
                                  evsesToAddQueueCopy.Value,
                                  TransmissionTypes.Direct,
                                  now,
                                  eventTrackingId,
                                  null,
                                  null,
                                  cancellationTokenSource.Token
                              );

                }

                #endregion

                #region Send changed EVSE data

                if (evseDataQueueCopy.Value is not null &&
                    evseDataQueueCopy.Value.Count > 0)
                {

                    // Surpress EVSE data updates for all newly added EVSEs
                    var EVSEsWithoutNewEVSEs = evseDataQueueCopy.Value.
                                                   Where(evse => !evsesToAddQueueCopy.Value.Contains(evse)).
                                                   ToArray();


                    if (EVSEsWithoutNewEVSEs.Length > 0)
                        await UpdateEVSEs(EVSEsWithoutNewEVSEs, EventTrackingId: eventTrackingId);

                }

                #endregion

                #region Send changed EVSE status

                if (evseStatusChangesDelayedQueueCopy.Value is not null &&
                    evseStatusChangesDelayedQueueCopy.Value.Count > 0)
                {
                    await UpdateEVSEStatus(evseStatusChangesDelayedQueueCopy.Value,
                                           EventTrackingId: eventTrackingId);
                }

                #endregion

                //ToDo: Send removed EVSE data!

            }

            return;

        }

        #endregion

        #region (timer) FlushEVSEDataAndStatus()

        protected override Boolean SkipFlushEVSEDataAndStatusQueues()
            => evsesToAddQueue.              Count == 0 &&
               evsesToUpdateQueue.           Count == 0 &&
               evseStatusChangesDelayedQueue.Count == 0 &&
               evsesToRemoveQueue.           Count == 0;

        protected override async Task FlushEVSEDataAndStatusQueues()
        {

            #region Get a copy of all current EVSE data and delayed status

            var evsesToAddQueueCopy                = new HashSet<IEVSE>();
            var EVSEsToUpdateQueueCopy             = new HashSet<IEVSE>();
            var evseStatusChangesDelayedQueueCopy  = new List<EVSEStatusUpdate>();
            var evsesToRemoveQueueCopy             = new HashSet<IEVSE>();
            var EVSEsUpdateLogCopy                 = new Dictionary<IEVSE,            PropertyUpdateInfo[]>();
            var ChargingStationsUpdateLogCopy      = new Dictionary<IChargingStation, PropertyUpdateInfo[]>();
            var ChargingPoolsUpdateLogCopy         = new Dictionary<IChargingPool,    PropertyUpdateInfo[]>();

            await DataAndStatusLock.WaitAsync();

            try
            {

                // Copy 'EVSEs to add', remove originals...
                evsesToAddQueueCopy                      = new HashSet<IEVSE>                (evsesToAddQueue);
                evsesToAddQueue.Clear();

                // Copy 'EVSEs to update', remove originals...
                EVSEsToUpdateQueueCopy                   = new HashSet<IEVSE>                (evsesToUpdateQueue);
                evsesToUpdateQueue.Clear();

                // Copy 'EVSE status changes', remove originals...
                evseStatusChangesDelayedQueueCopy        = new List<EVSEStatusUpdate>       (evseStatusChangesDelayedQueue);
                evseStatusChangesDelayedQueueCopy.AddRange(evsesToAddQueueCopy.SafeSelect(evse => new EVSEStatusUpdate(evse.Id, evse.Status, evse.Status)));
                evseStatusChangesDelayedQueue.Clear();

                // Copy 'EVSEs to remove', remove originals...
                evsesToRemoveQueueCopy                   = new HashSet<IEVSE>                (evsesToRemoveQueue);
                evsesToRemoveQueue.Clear();

                // Copy EVSE property updates
                evsesUpdateLog.           ForEach(_ => EVSEsUpdateLogCopy.           Add(_.Key, _.Value.ToArray()));
                evsesUpdateLog.Clear();

                // Copy charging station property updates
                chargingStationsUpdateLog.ForEach(_ => ChargingStationsUpdateLogCopy.Add(_.Key, _.Value.ToArray()));
                chargingStationsUpdateLog.Clear();

                // Copy charging pool property updates
                chargingPoolsUpdateLog.   ForEach(_ => ChargingPoolsUpdateLogCopy.   Add(_.Key, _.Value.ToArray()));
                chargingPoolsUpdateLog.Clear();


                // Stop the timer. Will be rescheduled by next EVSE data/status change...
                FlushEVSEDataAndStatusTimer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

            }
            finally
            {
                DataAndStatusLock.Release();
            }

            #endregion

            // Use events to check if something went wrong!
            var EventTrackingId = EventTracking_Id.New;

            //Thread.Sleep(30000);

            #region Send new EVSE data

            if (evsesToAddQueueCopy.Count > 0)
            {

                //var EVSEsToAddTask = PushEVSEData(evsesToAddQueueCopy,
                //                                  _FlushEVSEDataRunId == 1
                //                                      ? ActionTypes.fullLoad
                //                                      : ActionTypes.update,
                //                                  EventTrackingId: EventTrackingId);

                //EVSEsToAddTask.Wait();

                //if (EVSEsToAddTask.Result.Warnings.Any())
                //{

                //    SendOnWarnings(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                //                   nameof(WWCPCPOAdapter) + Id,
                //                   "EVSEsToAddTask",
                //                   EVSEsToAddTask.Result.Warnings);

                //}

            }

            #endregion

            #region Send changed EVSE data

            if (EVSEsToUpdateQueueCopy.Count > 0)
            {

                // Surpress EVSE data updates for all newly added EVSEs
                foreach (var _evse in EVSEsToUpdateQueueCopy.Where(evse => evsesToAddQueueCopy.Contains(evse)).ToArray())
                    EVSEsToUpdateQueueCopy.Remove(_evse);

                if (EVSEsToUpdateQueueCopy.Any())
                {

                    //var PushEVSEDataTask = PushEVSEData(EVSEsWithoutNewEVSEs,
                    //                                    ActionTypes.update,
                    //                                    EventTrackingId: EventTrackingId);

                    //PushEVSEDataTask.Wait();

                    //if (PushEVSEDataTask.Result.Warnings.Any())
                    //{

                    //    SendOnWarnings(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                    //                   nameof(WWCPCPOAdapter) + Id,
                    //                   "PushEVSEDataTask",
                    //                   PushEVSEDataTask.Result.Warnings);

                    //}

                }

            }

            #endregion

            #region Send changed EVSE status

            if (!DisableSendStatus &&
                evseStatusChangesDelayedQueueCopy.Count > 0)
            {

                var pushEVSEStatusResult = await UpdateStatus(evseStatusChangesDelayedQueueCopy,
                                                              TransmissionTypes.Direct,

                                                              Timestamp.Now,
                                                              EventTrackingId,
                                                              DefaultRequestTimeout,
                                                              null,
                                                              new CancellationTokenSource().Token).
                                                     ConfigureAwait(false);

                if (pushEVSEStatusResult.Warnings.Any())
                {

                    SendOnWarnings(Timestamp.Now,
                                   nameof(WWCPCSOAdapter) + Id,
                                   "UpdateStatus",
                                   pushEVSEStatusResult.Warnings);

                }

            }

            #endregion

            #region Send removed charging stations

            if (evsesToRemoveQueueCopy.Count > 0)
            {

                var EVSEsToRemove = evsesToRemoveQueueCopy.ToArray();

                if (EVSEsToRemove.Length > 0)
                {

                    //var EVSEsToRemoveTask = PushEVSEData(EVSEsToRemove,
                    //                                     ActionTypes.delete,
                    //                                     EventTrackingId: EventTrackingId);

                    //EVSEsToRemoveTask.Wait();

                    //if (EVSEsToRemoveTask.Result.Warnings.Any())
                    //{

                    //    SendOnWarnings(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                    //                   nameof(WWCPCPOAdapter) + Id,
                    //                   "EVSEsToRemoveTask",
                    //                   EVSEsToRemoveTask.Result.Warnings);

                    //}

                }

            }

            #endregion

        }

        #endregion

        #region (timer) FlushEVSEFastStatus()

        protected override Boolean SkipFlushEVSEFastStatusQueues()
            => evseStatusChangesFastQueue.Count == 0;

        protected override async Task FlushEVSEFastStatusQueues()
        {

            #region Get a copy of all current EVSE data and delayed status

            var EVSEStatusFastQueueCopy = new List<EVSEStatusUpdate>();

            var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

            try
            {

                if (LockTaken)
                {

                    if (evseStatusChangesFastQueue.Count == 0)
                        return;

                    _StatusRunId++;

                    // Copy 'EVSE status changes', remove originals...
                    EVSEStatusFastQueueCopy = new List<EVSEStatusUpdate>(evseStatusChangesFastQueue.Where(evsestatuschange => !evsesToAddQueue.Any(evse => evse.Id == evsestatuschange.Id)));

                    // Add all evse status changes of EVSE *NOT YET UPLOADED* into the delayed queue...
                    var EVSEStatusChangesDelayed = evseStatusChangesFastQueue.Where(evsestatuschange => evsesToAddQueue.Any(evse => evse.Id == evsestatuschange.Id)).ToArray();

                    if (EVSEStatusChangesDelayed.Length > 0)
                        evseStatusChangesDelayedQueue.AddRange(EVSEStatusChangesDelayed);

                    evseStatusChangesFastQueue.Clear();

                    // Stop the timer. Will be rescheduled by next EVSE status change...
                    FlushEVSEFastStatusTimer.Change(TimeSpan.FromMilliseconds(-1), TimeSpan.FromMilliseconds(-1));

                }

            }
            finally
            {
                if (LockTaken)
                    DataAndStatusLock.Release();
            }

            #endregion

            #region Send changed EVSE status

            if (EVSEStatusFastQueueCopy.Count > 0)
            {

                var pushEVSEStatusResult = await UpdateEVSEStatus(EVSEStatusFastQueueCopy,

                                                                  Timestamp.Now,
                                                                  EventTracking_Id.New,
                                                                  DefaultRequestTimeout,
                                                                  new CancellationTokenSource().Token).
                                                     ConfigureAwait(false);

                if (pushEVSEStatusResult.Warnings.Any())
                {

                    SendOnWarnings(Timestamp.Now,
                                   nameof(WWCPCSOAdapter) + Id,
                                   "PushEVSEStatus",
                                   pushEVSEStatusResult.Warnings);

                }

            }

            #endregion

        }

        #endregion

        #region (timer) EVSEStatusRefresh(State)

        private void EVSEStatusRefresh(Object State)
        {

            if (!DisableSendStatus && !DisableEVSEStatusRefresh)
            {

                try
                {

                    RefreshEVSEStatus().Wait();

                }
                catch (Exception e)
                {

                    while (e.InnerException is not null)
                        e = e.InnerException;

                    DebugX.Log("A exception occured during EVSEStatusRefresh: " + e.Message + Environment.NewLine + e.StackTrace);

                }

            }

        }

        private async Task<PushEVSEStatusResult> RefreshEVSEStatus()
        {

            #region Try to acquire the EVSE status refresh lock, or return...

            if (!EVSEStatusRefreshLock.Wait(0))
            {
                DebugX.Log("Could not acquire EVSE status refresh lock!");
                return PushEVSEStatusResult.NoOperation(Id, this);
            }

            #endregion


            EVSEStatusRefreshEvent?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                           this,
                                           "EVSE status refresh, as every " + EVSEStatusRefreshEvery.TotalHours.ToString() + " hours!");

            #region Data

            EVSE_Id?             _EVSEId;
            PushEVSEStatusResult result = null;

            var StartTime                  = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            var Warnings                   = new List<Warning>();
            var AllEVSEStatusRefreshments  = new List<EVSEStatus>();

            #endregion

            try
            {

                #region Fetch EVSE status

                foreach (var evsestatus in RoamingNetwork.EVSEStatus())
                {

                    try
                    {

                        if (IncludeEVSEIds(evsestatus.Id))
                        {

                            _EVSEId = _CustomEVSEIdMapper is not null
                                          ? _CustomEVSEIdMapper(evsestatus.Id)
                                          : evsestatus.Id.ToOCHP();

                            if (_EVSEId.HasValue)
                                AllEVSEStatusRefreshments.Add(new EVSEStatus(_EVSEId.Value,
                                                                             evsestatus.Status.AsEVSEMajorStatus(),
                                                                             evsestatus.Status.AsEVSEMinorStatus()));

                        }

                    }
                    catch (Exception e)
                    {
                        DebugX.  Log(e.Message);
                        Warnings.Add(Warning.Create(e.Message, evsestatus));
                    }

                }

                #endregion

                #region Upload EVSE status

                if (AllEVSEStatusRefreshments.Count > 0)
                {

                    var response = await CPORoaming.
                                             UpdateStatus(AllEVSEStatusRefreshments,
                                                          null,
                                                          // TTL => 2x refresh interval
                                                          org.GraphDefined.Vanaheimr.Illias.Timestamp.Now + EVSEStatusRefreshEvery + EVSEStatusRefreshEvery

                                                         //Timestamp,
                                                         //CancellationToken,
                                                         //EventTrackingId,
                                                         //RequestTimeout
                                                         );


                    var Endtime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
                    var Runtime = Endtime - StartTime;

                    if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                        response.Content        is not null)
                    {

                        if (response.Content.Result.ResultCode == ResultCodes.OK)
                            result = PushEVSEStatusResult.Success(Id,
                                                              this,
                                                              response.Content.Result.Description,
                                                              Warnings,
                                                              Runtime);

                        else
                            result = PushEVSEStatusResult.Error(Id,
                                                            this,
                                                            new EVSEStatusUpdate[0],
                                                            response.Content.Result.Description,
                                                            Warnings,
                                                            Runtime);

                    }
                    else
                        result = PushEVSEStatusResult.Error(Id,
                                                        this,
                                                        new EVSEStatusUpdate[0],
                                                        response.HTTPStatusCode.ToString(),
                                                        response.HTTPBody is not null
                                                            ? Warnings.AddAndReturnList(I18NString.Create(response.HTTPBody.ToUTF8String()))
                                                            : Warnings.AddAndReturnList(I18NString.Create("No HTTP body received!")),
                                                        Runtime);

                }

                #endregion

            }
            catch (Exception e)
            {

                while (e.InnerException is not null)
                    e = e.InnerException;

                DebugX.LogT(nameof(WWCPCSOAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                result = PushEVSEStatusResult.Error(
                             Id,
                             this,
                             [],
                             e.Message,
                             Warnings,
                             Timestamp.Now - StartTime
                         );

            }

            finally
            {
                EVSEStatusRefreshLock.Release();
            }

            return result;

        }

        #endregion

        #region (timer) FlushChargeDetailRecords()

        protected override Boolean SkipFlushChargeDetailRecordsQueues()
            => chargeDetailRecordsQueue.Count == 0;

        protected override async Task FlushChargeDetailRecordsQueues(IEnumerable<CDRInfo> CDRInfos)
        {

            try
            {

                var response = await CPORoaming.AddCDRs(
                                         CDRInfos,

                                         Timestamp.Now,
                                         EventTracking_Id.New,
                                         DefaultRequestTimeout,
                                         new CancellationTokenSource().Token
                                     ).
                                     ConfigureAwait(false);

                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content        is not null)
                {

                    switch (response.Content.Result.ResultCode)
                    {

                        case ResultCodes.OK:
                            {
                                foreach (var CDRInfo in CDRInfos)
                                    await RoamingNetwork.ReceiveSendChargeDetailRecordResult(
                                              SendCDRResult.Success(
                                                  Timestamp.Now,
                                                  Id,
                                                  CDRInfo.GetInternalDataAs<ChargeDetailRecord>(OCHPMapper.WWCP_CDR),
                                                  Runtime: response.Runtime
                                              )
                                          );
                            }
                            break;

                        case ResultCodes.Partly:
                            {

                                var implausibleCDRs = response.Content.ImplausibleCDRs.ToHashSet();

                                foreach (var CDRInfo in CDRInfos)
                                    await RoamingNetwork.ReceiveSendChargeDetailRecordResult(

                                              implausibleCDRs.Contains(CDRInfo.CDRId)

                                                  ? SendCDRResult.Error  (
                                                        Timestamp.Now,
                                                        Id,
                                                        CDRInfo.GetInternalDataAs<ChargeDetailRecord>(OCHPMapper.WWCP_CDR),
                                                        Warnings: Warnings.Create("implausible charge detail record!"),
                                                        Runtime: response.Runtime
                                                    )

                                                  : SendCDRResult.Success(
                                                        Timestamp.Now,
                                                        Id,
                                                        CDRInfo.GetInternalDataAs<ChargeDetailRecord>(OCHPMapper.WWCP_CDR),
                                                        Runtime: response.Runtime
                                                    )

                                          );

                            }
                            break;

                        default:
                            {
                                foreach (var CDRInfo in CDRInfos)
                                    await RoamingNetwork.ReceiveSendChargeDetailRecordResult(
                                              SendCDRResult.Error(
                                                  Timestamp.Now,
                                                  Id,
                                                  CDRInfo.GetInternalDataAs<ChargeDetailRecord>(OCHPMapper.WWCP_CDR),
                                                  Warnings: Warnings.Create($"{response.Content.Result.ResultCode} - {response.Content.Result.Description}"),
                                                  Runtime:  response.Runtime
                                              )
                                          );
                            }
                            break;

                    }

                }

                //ToDo: Re-add to queue if it could not be send...

            }
            catch (Exception e)
            {
                foreach (var CDRInfo in CDRInfos)
                    await RoamingNetwork.ReceiveSendChargeDetailRecordResult(
                              SendCDRResult.Error(
                                  Timestamp.Now,
                                  Id,
                                  CDRInfo.GetInternalDataAs<ChargeDetailRecord>(OCHPMapper.WWCP_CDR),
                                  Warnings: Warnings.Create(e.Message),
                                  Runtime:  TimeSpan.Zero
                              )
                          );
            }

        }

        #endregion


        // -----------------------------------------------------------------------------------------------------


        #region Operator overloading

        #region Operator == (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two WWCPCPOAdapters for equality.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (WWCPCSOAdapter WWCPCPOAdapter1, WWCPCSOAdapter WWCPCPOAdapter2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(WWCPCPOAdapter1, WWCPCPOAdapter2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) WWCPCPOAdapter1 is null) || ((Object) WWCPCPOAdapter2 is null))
                return false;

            return WWCPCPOAdapter1.Equals(WWCPCPOAdapter2);

        }

        #endregion

        #region Operator != (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two WWCPCPOAdapters for inequality.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (WWCPCSOAdapter WWCPCPOAdapter1, WWCPCSOAdapter WWCPCPOAdapter2)

            => !(WWCPCPOAdapter1 == WWCPCPOAdapter2);

        #endregion

        #region Operator <  (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (WWCPCSOAdapter  WWCPCPOAdapter1,
                                          WWCPCSOAdapter  WWCPCPOAdapter2)
        {

            if ((Object) WWCPCPOAdapter1 is null)
                throw new ArgumentNullException(nameof(WWCPCPOAdapter1),  "The given WWCPCPOAdapter must not be null!");

            return WWCPCPOAdapter1.CompareTo(WWCPCPOAdapter2) < 0;

        }

        #endregion

        #region Operator <= (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (WWCPCSOAdapter WWCPCPOAdapter1,
                                           WWCPCSOAdapter WWCPCPOAdapter2)

            => !(WWCPCPOAdapter1 > WWCPCPOAdapter2);

        #endregion

        #region Operator >  (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (WWCPCSOAdapter WWCPCPOAdapter1,
                                          WWCPCSOAdapter WWCPCPOAdapter2)
        {

            if ((Object) WWCPCPOAdapter1 is null)
                throw new ArgumentNullException(nameof(WWCPCPOAdapter1),  "The given WWCPCPOAdapter must not be null!");

            return WWCPCPOAdapter1.CompareTo(WWCPCPOAdapter2) > 0;

        }

        #endregion

        #region Operator >= (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (WWCPCSOAdapter WWCPCPOAdapter1,
                                           WWCPCSOAdapter WWCPCPOAdapter2)

            => !(WWCPCPOAdapter1 < WWCPCPOAdapter2);

        #endregion

        #endregion

        #region IComparable<WWCPCPOAdapter> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object is null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var WWCPCPOAdapter = Object as WWCPCSOAdapter;
            if ((Object) WWCPCPOAdapter is null)
                throw new ArgumentException("The given object is not an WWCPCPOAdapter!", nameof(Object));

            return CompareTo(WWCPCPOAdapter);

        }

        #endregion

        #region CompareTo(WWCPCPOAdapter)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter">An WWCPCPOAdapter object to compare with.</param>
        public Int32 CompareTo(WWCPCSOAdapter WWCPCPOAdapter)
        {

            if ((Object) WWCPCPOAdapter is null)
                throw new ArgumentNullException(nameof(WWCPCPOAdapter), "The given WWCPCPOAdapter must not be null!");

            return Id.CompareTo(WWCPCPOAdapter.Id);

        }

        #endregion

        #endregion

        #region IEquatable<WWCPCPOAdapter> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is null)
                return false;

            var WWCPCPOAdapter = Object as WWCPCSOAdapter;
            if ((Object) WWCPCPOAdapter is null)
                return false;

            return this.Equals(WWCPCPOAdapter);

        }

        #endregion

        #region Equals(WWCPCPOAdapter)

        /// <summary>
        /// Compares two WWCPCPOAdapter for equality.
        /// </summary>
        /// <param name="WWCPCPOAdapter">An WWCPCPOAdapter to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(WWCPCSOAdapter WWCPCPOAdapter)
        {

            if ((Object) WWCPCPOAdapter is null)
                return false;

            return Id.Equals(WWCPCPOAdapter.Id);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Get the hashcode of this object.
        /// </summary>
        public override Int32 GetHashCode()

            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => $"OCHP {Version.Number} CPO Adapter {Id}";

        #endregion


    }

}
