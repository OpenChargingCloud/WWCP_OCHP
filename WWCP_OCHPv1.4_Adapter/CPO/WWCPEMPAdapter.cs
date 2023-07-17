/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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
    public class WWCPEMPAdapter : AWWCPEMPAdapter<CDRInfo>,
                                  IEMPRoamingProvider,
                                  IEquatable<WWCPEMPAdapter>,
                                  IComparable<WWCPEMPAdapter>,
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
        /// The default service check intervall.
        /// </summary>
        public  readonly static  TimeSpan                                       DefaultServiceCheckEvery       = TimeSpan.FromSeconds(31);

        /// <summary>
        /// The default status check intervall.
        /// </summary>
        public  readonly static  TimeSpan                                       DefaultStatusCheckEvery        = TimeSpan.FromSeconds(3);

        /// <summary>
        /// The default EVSE status refresh intervall.
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


        private readonly         Dictionary<EMT_Id, Contract_Id>                _Lookup;

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
        /// The service check intervall.
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

        public delegate Task OnWWCPCPOAdapterExceptionDelegate(DateTime        Timestamp,
                                                               WWCPEMPAdapter  Sender,
                                                               Exception       Exception);

        public event OnWWCPCPOAdapterExceptionDelegate OnWWCPCPOAdapterException;

        #endregion


        public delegate void FlushServiceQueuesDelegate(WWCPEMPAdapter Sender, TimeSpan Every);

        public event FlushServiceQueuesDelegate FlushServiceQueuesEvent;


        public delegate void FlushEVSEStatusUpdateQueuesDelegate(WWCPEMPAdapter Sender, TimeSpan Every);

        public event FlushEVSEStatusUpdateQueuesDelegate FlushEVSEStatusUpdateQueuesEvent;


        public delegate void EVSEStatusRefreshEventDelegate(DateTime Timestamp, WWCPEMPAdapter Sender, String Message);

        public event EVSEStatusRefreshEventDelegate EVSEStatusRefreshEvent;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new WWCP wrapper for the OCHP roaming client for Charging Station Operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="CPORoaming">A OCHP CPO roaming object to be mapped to WWCP.</param>
        /// <param name="EVSE2ChargePointInfo">A delegate to process a charge point info, e.g. before pushing it to the roaming provider.</param>
        /// <param name="ChargePointInfo2XML">A delegate to process the XML representation of a charge point info, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// <param name="EVSEStatusRefreshEvery">The EVSE status refresh intervall.</param>
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableEVSEStatusRefresh">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        public WWCPEMPAdapter(EMPRoamingProvider_Id                           Id,
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

            if (CPORoaming == null)
                throw new ArgumentNullException(nameof(CPORoaming),  "The given OCHP CPO Roaming object must not be null!");

            #endregion

            //this.Name                                 = Name;
            //this._ISendData                           = this as ISendData;
            //this._ISendStatus                         = this as ISendStatus;

            this.CPORoaming                           = CPORoaming;
            this._CustomEVSEIdMapper                  = CustomEVSEIdMapper;
            this._EVSE2ChargePointInfo                = EVSE2ChargePointInfo;
            this._EVSEStatusUpdate2EVSEStatus         = EVSEStatusUpdate2EVSEStatus;
            this._ChargePointInfo2XML                 = ChargePointInfo2XML;
            this._EVSEStatus2XML                      = EVSEStatus2XML;

            //this._IncludeEVSEIds                      = IncludeEVSEIds ?? (evseid => true);
            //this.IncludeEVSEs                        = IncludeEVSEs   ?? (evse   => true);
            this.IncludeChargePoints                  = IncludeChargePoints ?? (cp => true);

            this.ServiceCheckLock                     = new Object();
            this._ServiceCheckEvery                   = (UInt32) (ServiceCheckEvery.HasValue
                                                                     ? ServiceCheckEvery.Value. TotalMilliseconds
                                                                     : DefaultServiceCheckEvery.TotalMilliseconds);
            this.ServiceCheckTimer                    = new Timer(ServiceCheck,           null,                           0, _ServiceCheckEvery);

            //this.StatusCheckLock                      = new Object();
            //this._FlushEVSEStatusUpdatesEvery                    = (UInt32) (StatusCheckEvery.HasValue
            //                                                         ? StatusCheckEvery.Value.  TotalMilliseconds
            //                                                         : DefaultStatusCheckEvery. TotalMilliseconds);
            //this.StatusCheckTimer                     = new Timer(FlushEVSEStatusUpdates, null,                           0, _FlushEVSEStatusUpdatesEvery);

            this.EVSEStatusRefreshEvery               = EVSEStatusRefreshEvery ?? DefaultEVSEStatusRefreshEvery;
            this.EVSEStatusRefreshTimer               = new Timer(EVSEStatusRefresh, null, this.EVSEStatusRefreshEvery, this.EVSEStatusRefreshEvery);

            //this.DisablePushData                      = DisablePushData;
            //this.DisablePushStatus                    = DisablePushStatus;
            this.DisableEVSEStatusRefresh             = DisableEVSEStatusRefresh;
            //this.DisableAuthentication                = DisableAuthentication;
            //this.DisableSendChargeDetailRecords       = DisableSendChargeDetailRecords;

            //this.evsesToAddQueue                      = new HashSet<EVSE>();
            //this.evsesToUpdateQueue                   = new HashSet<EVSE>();
            //this.evseStatusChangesFastQueue           = new List<EVSEStatusUpdate>();
            //this.evseStatusChangesDelayedQueue        = new List<EVSEStatusUpdate>();
            //this.evsesToRemoveQueue                   = new HashSet<EVSE>();

            this._Lookup                              = new Dictionary<EMT_Id, Contract_Id>();

            lock (_Lookup)
            {

                var elements                          = new String[0];

                Directory.CreateDirectory(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar.ToString() + "OCHPv1.4");

                EMT_Id       EMTId       = default;
                Contract_Id  ContractId  = default;

                foreach (var inputfile in Directory.EnumerateFiles(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar.ToString() + "OCHPv1.4",
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

                                    if (!_Lookup.ContainsKey(EMTId))
                                        _Lookup.Add(EMTId, ContractId);

                                    else
                                    {

                                        if (_Lookup[EMTId] != ContractId)
                                        {

                                        }

                                    }

                                }

                                else if (elements.Length == 5)
                                    _Lookup.Add(new EMT_Id(elements[0]?.Trim(),
                                                           (TokenRepresentations) Enum.Parse(typeof(TokenRepresentations), elements[1]?.Trim(), ignoreCase: true),
                                                           (TokenTypes)           Enum.Parse(typeof(TokenTypes),           elements[2]?.Trim(), ignoreCase: true),
                                                           elements[3]?.Trim().IsNotNullOrEmpty() == true
                                                               ? new TokenSubTypes?((TokenSubTypes) Enum.Parse(typeof(TokenSubTypes), elements[3]?.Trim(), ignoreCase: true))
                                                               : null),
                                                Contract_Id.Parse(elements[4]));

                            }

                        }
                        catch (Exception e)
                        {
                            DebugX.Log("Could not read logfile " + inputfile + @""": " + e.Message);
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
            //    if (PartnerProductId != null && PartnerProductId.ToString().IsNotNullOrEmpty())
            //    {

            //        var Elements = pattern.Replace(PartnerProductId.ToString(), "=").Split('|').ToArray();

            //        if (Elements.Length > 0)
            //        {

            //            var DurationText = Elements.FirstOrDefault(element => element.StartsWith("D=", StringComparison.InvariantCulture));
            //            if (DurationText != null)
            //            {

            //                DurationText = DurationText.Substring(2);

            //                if (DurationText.EndsWith("sec", StringComparison.InvariantCulture))
            //                    Duration = TimeSpan.FromSeconds(UInt32.Parse(DurationText.Substring(0, DurationText.Length - 3)));

            //                if (DurationText.EndsWith("min", StringComparison.InvariantCulture))
            //                    Duration = TimeSpan.FromMinutes(UInt32.Parse(DurationText.Substring(0, DurationText.Length - 3)));

            //            }

            //            var PartnerProductText = Elements.FirstOrDefault(element => element.StartsWith("P=", StringComparison.InvariantCulture));
            //            if (PartnerProductText != null)
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

            //    if (response != null)
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

            //    if (response != null)
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

            //    if (ChargingProductId != null && ChargingProductId.ToString().IsNotNullOrEmpty())
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

            //    if (response != null)
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

            //    if (response != null)
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
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        private async Task<PushEVSEDataResult>

            SetChargePointInfos(IEnumerable<IEVSE>  EVSEs,

                                DateTime?           Timestamp           = null,
                                CancellationToken   CancellationToken   = default,
                                EventTracking_Id?   EventTrackingId     = null,
                                TimeSpan?           RequestTimeout      = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            EventTrackingId ??= EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;


            PushEVSEDataResult result;

            #endregion

            #region Get effective list of EVSEs/ChargePointInfos to upload

            var Warnings          = new List<Warning>();
            var ChargePointInfos  = new List<ChargePointInfo>();

            if (EVSEs.IsNeitherNullNorEmpty())
            {
                foreach (var evse in EVSEs)
                {

                    try
                    {

                        if (evse == null)
                            continue;

                        if (IncludeEVSEs(evse) && IncludeEVSEIds(evse.Id))
                            // WWCP EVSE will be added as internal data "WWCP.EVSE"...
                            ChargePointInfos.Add(evse.ToOCHP(_CustomEVSEIdMapper,
                                                             _EVSE2ChargePointInfo));

                        else
                            DebugX.Log(evse.Id + " was filtered!");

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e.Message);
                        Warnings.Add(Warning.Create(I18NString.Create(Languages.en, e.Message), evse));
                    }

                }
            }

            #endregion

            #region Send OnSetChargePointInfosWWCPRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnSetChargePointInfosWWCPRequest?.Invoke(StartTime,
                                                          Timestamp.Value,
                                                          this,
                                                          Id,
                                                          EventTrackingId,
                                                          RoamingNetwork.Id,
                                                          ChargePointInfos.ULongCount(),
                                                          ChargePointInfos,
                                                          Warnings.Where(warning => warning.IsNeitherNullNorEmpty()),
                                                          RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnSetChargePointInfosWWCPRequest));
            }

            #endregion


            DateTime Endtime;
            TimeSpan Runtime;

            if (ChargePointInfos.Count > 0)
            {

                var response = await CPORoaming.
                                         SetChargePointList(ChargePointInfos,
                                                            IncludeChargePoints,

                                                            Timestamp,
                                                            CancellationToken,
                                                            EventTrackingId,
                                                            RequestTimeout);


                Endtime = DateTime.UtcNow;
                Runtime = Endtime - StartTime;

                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content != null)
                {

                    if (response.Content.Result.ResultCode == ResultCodes.OK)
                        result = PushEVSEDataResult.Success(Id,
                                                            this,
                                                            ChargePointInfos.Select(chargePointInfo => chargePointInfo.GetInternalData("WWCP.EVSE") as EVSE),
                                                            response.Content.Result.Description,
                                                            null,
                                                            Runtime);

                    else
                        result = PushEVSEDataResult.Error(Id,
                                                          this,
                                                          ChargePointInfos.Select(chargePointInfo => new PushSingleEVSEDataResult(chargePointInfo.GetInternalData("WWCP.EVSE") as EVSE, PushSingleDataResultTypes.Error)),
                                                          response.Content.Result.Description,
                                                          null,
                                                          Runtime);

                }
                else
                    result = PushEVSEDataResult.Error(Id,
                                                      this,
                                                      ChargePointInfos.Select(chargePointInfo => new PushSingleEVSEDataResult(chargePointInfo.GetInternalData("WWCP.EVSE") as EVSE, PushSingleDataResultTypes.Error)),
                                                      response.HTTPStatusCode.ToString(),
                                                      response.HTTPBody != null
                                                          ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.HTTPBody.ToUTF8String()))
                                                          : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
                                                      Runtime);

            }

            #region ...or no ChargePointInfos to push...

            else
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = PushEVSEDataResult.NoOperation(Id,
                                                          this,
                                                          EVSEs,
                                                          "No ChargePointInfos to push!",
                                                          Warnings,
                                                          DateTime.UtcNow - StartTime);

            }

            #endregion


            #region Send OnSetChargePointInfosWWCPResponse event

            try
            {

                OnSetChargePointInfosWWCPResponse?.Invoke(Endtime,
                                                          Timestamp.Value,
                                                          this,
                                                          Id,
                                                          EventTrackingId,
                                                          RoamingNetwork.Id,
                                                          ChargePointInfos.ULongCount(),
                                                          ChargePointInfos,
                                                          RequestTimeout,
                                                          result,
                                                          Runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnSetChargePointInfosWWCPResponse));
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
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        private async Task<PushEVSEDataResult>

            UpdateChargePointInfos(IEnumerable<IEVSE>  EVSEs,

                                   DateTime?           Timestamp           = null,
                                   CancellationToken   CancellationToken   = default,
                                   EventTracking_Id?   EventTrackingId     = null,
                                   TimeSpan?           RequestTimeout      = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;


            PushEVSEDataResult result;

            #endregion

            #region Get effective list of EVSEs/ChargePointInfos to upload

            var Warnings          = new List<Warning>();
            var ChargePointInfos  = new List<ChargePointInfo>();

            if (EVSEs.IsNeitherNullNorEmpty())
            {
                foreach (var evse in EVSEs)
                {

                    try
                    {

                        if (evse == null)
                            continue;

                        if (IncludeEVSEs(evse) && IncludeEVSEIds(evse.Id))
                            // WWCP EVSE will be added as internal data "WWCP.EVSE"...
                            ChargePointInfos.Add(evse.ToOCHP(_CustomEVSEIdMapper,
                                                             _EVSE2ChargePointInfo));

                        else
                            DebugX.Log(evse.Id + " was filtered!");

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e.Message);
                        Warnings.Add(Warning.Create(I18NString.Create(Languages.en, e.Message), evse));
                    }

                }
            }

            #endregion

            #region Send OnSetChargePointInfosWWCPRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnUpdateChargePointInfosWWCPRequest?.Invoke(StartTime,
                                                            Timestamp.Value,
                                                            this,
                                                            Id,
                                                            EventTrackingId,
                                                            RoamingNetwork.Id,
                                                            ChargePointInfos.ULongCount(),
                                                            ChargePointInfos,
                                                            Warnings.Where(warning => warning.IsNeitherNullNorEmpty()),
                                                            RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnSetChargePointInfosWWCPRequest));
            }

            #endregion


            DateTime Endtime;
            TimeSpan Runtime;

            if (ChargePointInfos.Count > 0)
            {

                var response = await CPORoaming.
                                         UpdateChargePointList(ChargePointInfos,
                                                               IncludeChargePoints,

                                                               Timestamp,
                                                               CancellationToken,
                                                               EventTrackingId,
                                                               RequestTimeout).
                                         ConfigureAwait(false);

                Endtime = DateTime.UtcNow;
                Runtime = Endtime - StartTime;

                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content        != null)
                {

                    if (response.Content.Result.ResultCode == ResultCodes.OK)
                        result = PushEVSEDataResult.Success(Id,
                                                            this,
                                                            ChargePointInfos.Select(chargePointInfo => chargePointInfo.GetInternalData("WWCP.EVSE") as EVSE),
                                                            response.Content.Result.Description,
                                                            null,
                                                            Runtime);

                    else
                        result = PushEVSEDataResult.Error(Id,
                                                          this,
                                                          ChargePointInfos.Select(chargePointInfo => new PushSingleEVSEDataResult(chargePointInfo.GetInternalData("WWCP.EVSE") as EVSE, PushSingleDataResultTypes.Error)),
                                                          response.Content.Result.Description,
                                                          null,
                                                          Runtime);

                }
                else
                    result = PushEVSEDataResult.Error(Id,
                                                      this,
                                                      ChargePointInfos.Select(chargePointInfo => new PushSingleEVSEDataResult(chargePointInfo.GetInternalData("WWCP.EVSE") as EVSE, PushSingleDataResultTypes.Error)),
                                                      response.HTTPStatusCode.ToString(),
                                                      response.HTTPBody != null
                                                          ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.HTTPBody.ToUTF8String()))
                                                          : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
                                                      Runtime);

            }

            #region ...or no ChargePointInfos to push...

            else
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = PushEVSEDataResult.NoOperation(Id,
                                                          this,
                                                          EVSEs,
                                                          "No ChargePointInfos to push!",
                                                          Warnings,
                                                          DateTime.UtcNow - StartTime);

            }

            #endregion


            #region Send OnUpdateChargePointInfosWWCPResponse event

            try
            {

                OnUpdateChargePointInfosWWCPResponse?.Invoke(Endtime,
                                                             Timestamp.Value,
                                                             this,
                                                             Id,
                                                             EventTrackingId,
                                                             RoamingNetwork.Id,
                                                             ChargePointInfos.ULongCount(),
                                                             ChargePointInfos,
                                                             RequestTimeout,
                                                             result,
                                                             Runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnUpdateChargePointInfosWWCPResponse));
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
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<PushEVSEStatusResult>

            UpdateEVSEStatus(IEnumerable<EVSEStatusUpdate>  EVSEStatusUpdates,

                             DateTime?                      Timestamp           = null,
                             CancellationToken              CancellationToken   = default,
                             EventTracking_Id               EventTrackingId     = null,
                             TimeSpan?                      RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEStatusUpdates == null)
                throw new ArgumentNullException(nameof(EVSEStatusUpdates), "The given enumeration of EVSE status updates must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSEStatus/EVSEStatusRecords to upload

            var Warnings = new List<Warning>();

            var _EVSEStatus = EVSEStatusUpdates.
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
                                          Warnings.Add(Warning.Create(I18NString.Create(Languages.en, e.Message), evsestatusupdate));
                                      }

                                      return null;

                                  }).
                                  Where(evsestatusrecord => evsestatusrecord != null).
                                  ToArray();

            PushEVSEStatusResult result = null;

            #endregion

            #region Send OnUpdateEVSEStatusWWCPRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnUpdateEVSEStatusWWCPRequest?.Invoke(StartTime,
                                                      Timestamp.Value,
                                                      this,
                                                      Id,
                                                      EventTrackingId,
                                                      RoamingNetwork.Id,
                                                      _EVSEStatus.ULongCount(),
                                                      _EVSEStatus,
                                                      Warnings.Where(warning => warning.IsNeitherNullNorEmpty()),
                                                      RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnUpdateEVSEStatusWWCPRequest));
            }

            #endregion


            var response = await CPORoaming.
                                     UpdateStatus(_EVSEStatus,
                                                  null,
                                                  DateTime.UtcNow + EVSEStatusRefreshEvery,
                                                  null,

                                                  Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  RequestTimeout).
                                     ConfigureAwait(false);


            var Endtime = DateTime.UtcNow;
            var Runtime = Endtime - StartTime;

            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null)
            {

                if (response.Content.Result.ResultCode == ResultCodes.OK)
                    result = PushEVSEStatusResult.Success(Id,
                                                      this,
                                                      response.Content.Result.Description,
                                                      null,
                                                      Runtime);

                else
                    result = PushEVSEStatusResult.Error(Id,
                                                    this,
                                                    EVSEStatusUpdates,
                                                    response.Content.Result.Description,
                                                    null,
                                                    Runtime);

            }
            else
                result = PushEVSEStatusResult.Error(Id,
                                                this,
                                                EVSEStatusUpdates,
                                                response.HTTPStatusCode.ToString(),
                                                response.HTTPBody != null
                                                    ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.HTTPBody.ToUTF8String()))
                                                    : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
                                                Runtime);


            #region Send OnUpdateEVSEStatusWWCPResponse event

            try
            {

                OnUpdateEVSEStatusWWCPResponse?.Invoke(Endtime,
                                                       Timestamp.Value,
                                                       this,
                                                       Id,
                                                       EventTrackingId,
                                                       RoamingNetwork.Id,
                                                       _EVSEStatus.ULongCount(),
                                                       _EVSEStatus,
                                                       RequestTimeout,
                                                       result,
                                                       Runtime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnUpdateEVSEStatusWWCPResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region (Set/Add/Update/Delete) EVSE(s)...

        #region SetStaticData   (EVSE, TransmissionType = Enqueue, ...)

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
        Task<PushEVSEDataResult>

            ISendPOIData.SetStaticData(IEVSE               EVSE,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken   CancellationToken,
                                       EventTracking_Id?   EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
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

                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this, new IEVSE[] { EVSE }));

            }

            #endregion

            return SetChargePointInfos(new IEVSE[] { EVSE },

                                       Timestamp,
                                       CancellationToken,
                                       EventTrackingId,
                                       RequestTimeout);

        }

        #endregion

        #region AddStaticData   (EVSE, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the given EVSE to the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="EVSE">An EVSE to upload.</param>
        /// <param name="TransmissionType">Whether to send the EVSE directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendPOIData.AddStaticData(IEVSE               EVSE,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken   CancellationToken,
                                       EventTracking_Id?   EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
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

                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this, new IEVSE[] { EVSE }));

            }

            #endregion

            return UpdateChargePointInfos(new IEVSE[] { EVSE },

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(EVSE, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

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
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendPOIData.UpdateStaticData(IEVSE               EVSE,
                                          String              PropertyName,
                                          Object?             NewValue,
                                          Object?             OldValue,
                                          Context?            DataSource,
                                          TransmissionTypes   TransmissionType,

                                          DateTime?           Timestamp,
                                          CancellationToken   CancellationToken,
                                          EventTracking_Id?   EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
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

                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this, new IEVSE[] { EVSE }));

            }

            #endregion

            return UpdateChargePointInfos(new IEVSE[] { EVSE },

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(EVSE, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the static data of the given EVSE.
        /// </summary>
        /// <param name="EVSE">An EVSE to delete.</param>
        /// <param name="TransmissionType">Whether to send the EVSE deletion directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendPOIData.DeleteStaticData(IEVSE               EVSE,
                                          TransmissionTypes   TransmissionType,

                                          DateTime?           Timestamp,
                                          CancellationToken   CancellationToken,
                                          EventTracking_Id?   EventTrackingId,
                                          TimeSpan?           RequestTimeout)

                => Task.FromResult(PushEVSEDataResult.NoOperation(Id, this, new IEVSE[] { EVSE }));

        #endregion


        #region SetStaticData   (EVSEs, ...)

        /// <summary>
        /// Set the given enumeration of EVSEs as new static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendPOIData.SetStaticData(IEnumerable<IEVSE>  EVSEs,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken   CancellationToken,
                                       EventTracking_Id?   EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (!EVSEs.Any())
                Task.FromResult(PushEVSEDataResult.NoOperation(Id, this, EVSEs));

            #endregion

            return SetChargePointInfos(EVSEs,

                                       Timestamp,
                                       CancellationToken,
                                       EventTrackingId,
                                       RequestTimeout);

        }

        #endregion

        #region AddStaticData   (EVSEs, ...)

        /// <summary>
        /// Add the given enumeration of EVSEs to the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendPOIData.AddStaticData(IEnumerable<IEVSE>  EVSEs,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken   CancellationToken,
                                       EventTracking_Id?   EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (!EVSEs.Any())
                Task.FromResult(PushEVSEDataResult.NoOperation(Id, this, EVSEs));

            #endregion

            return UpdateChargePointInfos(EVSEs,

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);

        }

        #endregion

        #region UpdateStaticData(EVSEs, ...)

        /// <summary>
        /// Update the given enumeration of EVSEs within the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendPOIData.UpdateStaticData(IEnumerable<IEVSE>  EVSEs,
                                          TransmissionTypes   TransmissionType,

                                          DateTime?           Timestamp,
                                          CancellationToken   CancellationToken,
                                          EventTracking_Id?   EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (!EVSEs.Any())
                Task.FromResult(PushEVSEDataResult.NoOperation(Id, this, EVSEs));

            #endregion

            return UpdateChargePointInfos(EVSEs,

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);

        }

        #endregion

        #region DeleteStaticData(EVSEs, ...)

        /// <summary>
        /// Delete the given enumeration of EVSEs from the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendPOIData.DeleteStaticData(IEnumerable<IEVSE>  EVSEs,
                                          TransmissionTypes   TransmissionType,

                                          DateTime?           Timestamp,
                                          CancellationToken   CancellationToken,
                                          EventTracking_Id?   EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (!EVSEs.Any())
                Task.FromResult(PushEVSEDataResult.NoOperation(Id, this, EVSEs));

            #endregion

            return UpdateChargePointInfos(EVSEs,

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);

        }

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of EVSE admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<EVSEAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                   TransmissionType,

                                               DateTime?                           Timestamp,
                                               CancellationToken                   CancellationToken,
                                               EventTracking_Id?                   EventTrackingId,
                                               TimeSpan?                           RequestTimeout)


                => Task.FromResult(PushEVSEAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of EVSE status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of EVSE status updates.</param>
        /// <param name="TransmissionType">Whether to send the EVSE status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<PushEVSEStatusResult>

            UpdateStatus(IEnumerable<EVSEStatusUpdate>  StatusUpdates,
                         TransmissionTypes              TransmissionType,

                         DateTime?                      Timestamp,
                         CancellationToken              CancellationToken,
                         EventTracking_Id?              EventTrackingId,
                         TimeSpan?                      RequestTimeout)

        {

            #region Initial checks

            if (StatusUpdates == null || !StatusUpdates.Any())
                return PushEVSEStatusResult.NoOperation(Id, this);

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
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
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);

        }

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station(s)...

        #region SetStaticData   (ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station as new static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingStationDataResult>

            ISendPOIData.SetStaticData(IChargingStation    ChargingStation,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken   CancellationToken,
                                       EventTracking_Id?   EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
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

                    foreach (var evse in ChargingStation)
                    {

                        if (IncludeEVSEs(evse))
                        {

                            evsesToAddQueue.Add(evse);

                            ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                        }

                    }

                }

                return PushChargingStationDataResult.Enqueued(Id, this, new IChargingStation[] { ChargingStation });

            }

            #endregion

            var result = await SetChargePointInfos(ChargingStation.EVSEs,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout).

                                                   ConfigureAwait(false);

            return new WWCP.PushChargingStationDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region AddStaticData   (ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station to the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingStationDataResult>

            ISendPOIData.AddStaticData(IChargingStation    ChargingStation,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken   CancellationToken,
                                       EventTracking_Id?   EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
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

                    foreach (var evse in ChargingStation)
                    {

                        if (IncludeEVSEs(evse))
                        {

                            evsesToAddQueue.Add(evse);

                            ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                        }

                    }

                }

                return PushChargingStationDataResult.Enqueued(Id, this, null);

            }

            #endregion

            var result = await UpdateChargePointInfos(ChargingStation.EVSEs,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

            return new WWCP.PushChargingStationDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region UpdateStaticData(ChargingStation, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station within the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="PropertyName">The name of the charging station property to update.</param>
        /// <param name="OldValue">The old value of the charging station property to update.</param>
        /// <param name="NewValue">The new value of the charging station property to update.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingStationDataResult>

            ISendPOIData.UpdateStaticData(IChargingStation    ChargingStation,
                                          String?             PropertyName,
                                          Object?             NewValue,
                                          Object?             OldValue,
                                          Context?            DataSource,
                                          TransmissionTypes   TransmissionType,

                                          DateTime?           Timestamp,
                                          CancellationToken   CancellationToken,
                                          EventTracking_Id?   EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
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

                    foreach (var evse in ChargingStation)
                    {

                        if (IncludeEVSEs(evse))
                        {

                            evsesToUpdateQueue.Add(evse);

                            ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                        }

                    }

                }

                return PushChargingStationDataResult.Enqueued(Id, this, new IChargingStation[] { ChargingStation });

            }

            #endregion

            var result = await SetChargePointInfos(ChargingStation,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout).

                                                   ConfigureAwait(false);

            return new WWCP.PushChargingStationDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region DeleteStaticData(ChargingStation, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station from the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingStationDataResult>

            ISendPOIData.DeleteStaticData(IChargingStation    ChargingStation,
                                          TransmissionTypes   TransmissionType,

                                          DateTime?           Timestamp,
                                          CancellationToken   CancellationToken,
                                          EventTracking_Id?   EventTrackingId,
                                          TimeSpan?           RequestTimeout)

           {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
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

                    foreach (var evse in ChargingStation)
                    {

                        if (IncludeEVSEs(evse))
                        {

                            evsesToAddQueue.Add(evse);

                            ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                        }

                    }

                }

                return PushChargingStationDataResult.Enqueued(Id, this, null);

            }

            #endregion

            var result = await UpdateChargePointInfos(ChargingStation.EVSEs,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

            return new WWCP.PushChargingStationDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion


        #region SetStaticData   (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging stations as new static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingStationDataResult>

            ISendPOIData.SetStaticData(IEnumerable<IChargingStation>  ChargingStations,
                                       TransmissionTypes              TransmissionType,

                                       DateTime?                      Timestamp,
                                       CancellationToken              CancellationToken,
                                       EventTracking_Id?              EventTrackingId,
                                       TimeSpan?                      RequestTimeout)

        {

            #region Initial checks

            if (!ChargingStations.Any())
                return PushChargingStationDataResult.NoOperation(Id, this, ChargingStations);

            #endregion

            var result = await SetChargePointInfos(ChargingStations.SafeSelectMany(station => station.EVSEs),

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout);

            return new WWCP.PushChargingStationDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region AddStaticData   (ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging stations to the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingStationDataResult>

            ISendPOIData.AddStaticData(IEnumerable<IChargingStation>  ChargingStations,
                                       TransmissionTypes              TransmissionType,


                                       DateTime?                      Timestamp,
                                       CancellationToken              CancellationToken,
                                       EventTracking_Id?              EventTrackingId,
                                       TimeSpan?                      RequestTimeout)

        {

            #region Initial checks

            if (!ChargingStations.Any())
                return PushChargingStationDataResult.NoOperation(Id, this, ChargingStations);

            #endregion

            var result = await UpdateChargePointInfos(ChargingStations.SafeSelectMany(station => station.EVSEs),

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

            return new WWCP.PushChargingStationDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region UpdateStaticData(ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging stations within the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingStationDataResult>

            ISendPOIData.UpdateStaticData(IEnumerable<IChargingStation>  ChargingStations,
                                          TransmissionTypes              TransmissionType,

                                          DateTime?                      Timestamp,
                                          CancellationToken              CancellationToken,
                                          EventTracking_Id?              EventTrackingId,
                                          TimeSpan?                      RequestTimeout)

        {

            #region Initial checks

            if (!ChargingStations.Any())
                return PushChargingStationDataResult.NoOperation(Id, this, ChargingStations);

            #endregion

            var result = await UpdateChargePointInfos(ChargingStations.SafeSelectMany(station => station.EVSEs),

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

            return new WWCP.PushChargingStationDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region DeleteStaticData(ChargingStations, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging stations from the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// <param name="TransmissionType">Whether to send the charging station update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingStationDataResult>

            ISendPOIData.DeleteStaticData(IEnumerable<IChargingStation>  ChargingStations,
                                          TransmissionTypes              TransmissionType,

                                          DateTime?                      Timestamp,
                                          CancellationToken              CancellationToken,
                                          EventTracking_Id?              EventTrackingId,
                                          TimeSpan?                      RequestTimeout)

        {

            #region Initial checks

            if (!ChargingStations.Any())
                return PushChargingStationDataResult.NoOperation(Id, this, ChargingStations);

            #endregion

            var result = await UpdateChargePointInfos(ChargingStations.SafeSelectMany(station => station.EVSEs),

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

            return new WWCP.PushChargingStationDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingStationDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging station admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                              TransmissionType,

                                               DateTime?                                      Timestamp,
                                               CancellationToken                              CancellationToken,
                                               EventTracking_Id?                              EventTrackingId,
                                               TimeSpan?                                      RequestTimeout)


                => Task.FromResult(PushChargingStationAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging station status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingStationStatusUpdate>  StatusUpdates,

                                     TransmissionTypes                         TransmissionType,
                                     DateTime?                                 Timestamp,
                                     CancellationToken                         CancellationToken,
                                     EventTracking_Id?                         EventTrackingId,
                                     TimeSpan?                                 RequestTimeout)


                => Task.FromResult(PushChargingStationStatusResult.NoOperation(Id, this));

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging pool(s)...

        #region SetStaticData   (ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given charging pool as new static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingPoolDataResult>

            ISendPOIData.SetStaticData(IChargingPool       ChargingPool,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken   CancellationToken,
                                       EventTracking_Id?   EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
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

                    foreach (var evse in ChargingPool.EVSEs)
                    {

                        if (IncludeEVSEs(evse))
                        {

                            evsesToAddQueue.Add(evse);

                            ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                        }

                    }

                }

                return PushChargingPoolDataResult.Enqueued(Id, this, new IChargingPool[] { ChargingPool });

            }

            #endregion

            var result = await SetChargePointInfos(ChargingPool.EVSEs,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout).

                                                   ConfigureAwait(false);

            return new WWCP.PushChargingPoolDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region AddStaticData   (ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given charging pool to the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingPoolDataResult>

            ISendPOIData.AddStaticData(IChargingPool       ChargingPool,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken   CancellationToken,
                                       EventTracking_Id?   EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
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

                    foreach (var evse in ChargingPool.EVSEs)
                    {

                        if (IncludeEVSEs(evse))
                        {

                            evsesToAddQueue.Add(evse);

                            ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                        }

                    }

                }

                return PushChargingPoolDataResult.Enqueued(Id, this, new IChargingPool[] { ChargingPool });

            }

            #endregion

            var result = await UpdateChargePointInfos(ChargingPool.EVSEs,

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

            return new WWCP.PushChargingPoolDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region UpdateStaticData(ChargingPool, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given charging pool within the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="PropertyName">The name of the charging pool property to update.</param>
        /// <param name="OldValue">The old value of the charging pool property to update.</param>
        /// <param name="NewValue">The new value of the charging pool property to update.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingPoolDataResult>

            ISendPOIData.UpdateStaticData(IChargingPool       ChargingPool,
                                          String?             PropertyName,
                                          Object?             NewValue,
                                          Object?             OldValue,
                                          Context?            DataSource,
                                          TransmissionTypes   TransmissionType,

                                          DateTime?           Timestamp,
                                          CancellationToken   CancellationToken,
                                          EventTracking_Id?   EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
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

                    foreach (var evse in ChargingPool.EVSEs)
                    {

                        if (IncludeEVSEs(evse))
                        {

                            evsesToUpdateQueue.Add(evse);

                            ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                        }

                    }

                }

                return PushChargingPoolDataResult.Enqueued(Id, this, new IChargingPool[] { ChargingPool });

            }

            #endregion

            var result = await SetChargePointInfos(ChargingPool.EVSEs,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout).

                                                   ConfigureAwait(false);

            return new WWCP.PushChargingPoolDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region DeleteStaticData(ChargingPool, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging pool from the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingPoolDataResult>

            ISendPOIData.DeleteStaticData(IChargingPool       ChargingPool,
                                          TransmissionTypes   TransmissionType,

                                          DateTime?           Timestamp,
                                          CancellationToken   CancellationToken,
                                          EventTracking_Id?   EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.UtcNow,
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

                    foreach (var evse in ChargingPool.EVSEs)
                    {

                        if (IncludeEVSEs(evse))
                        {

                            evsesToUpdateQueue.Add(evse);

                            ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                        }

                    }

                }

                return PushChargingPoolDataResult.Enqueued(Id, this, new IChargingPool[] { ChargingPool });

            }

            #endregion

            var result = await SetChargePointInfos(ChargingPool.EVSEs,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout).

                                                   ConfigureAwait(false);

            return new WWCP.PushChargingPoolDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion


        #region SetStaticData   (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging pools as new static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingPoolDataResult>

            ISendPOIData.SetStaticData(IEnumerable<IChargingPool>  ChargingPools,
                                       TransmissionTypes           TransmissionType,

                                       DateTime?                   Timestamp,
                                       CancellationToken           CancellationToken,
                                       EventTracking_Id?           EventTrackingId,
                                       TimeSpan?                   RequestTimeout)

        {

            #region Initial checks

            if (!ChargingPools.Any())
                return PushChargingPoolDataResult.NoOperation(Id, this, ChargingPools);

            #endregion

            var result = await SetChargePointInfos(ChargingPools.SafeSelectMany(pool => pool.EVSEs),

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout);

            return new WWCP.PushChargingPoolDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region AddStaticData   (ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging pools to the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingPoolDataResult>

            ISendPOIData.AddStaticData(IEnumerable<IChargingPool>  ChargingPools,
                                       TransmissionTypes           TransmissionType,

                                       DateTime?                   Timestamp,
                                       CancellationToken           CancellationToken,
                                       EventTracking_Id?           EventTrackingId,
                                       TimeSpan?                   RequestTimeout)

        {

            #region Initial checks

            if (!ChargingPools.Any())
                return PushChargingPoolDataResult.NoOperation(Id, this, ChargingPools);

            #endregion

            var result = await UpdateChargePointInfos(ChargingPools.SafeSelectMany(pool => pool.EVSEs),

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

            return new WWCP.PushChargingPoolDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region UpdateStaticData(ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging pools within the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingPoolDataResult>

            ISendPOIData.UpdateStaticData(IEnumerable<IChargingPool>  ChargingPools,
                                          TransmissionTypes           TransmissionType,

                                          DateTime?                   Timestamp,
                                          CancellationToken           CancellationToken,
                                          EventTracking_Id?           EventTrackingId,
                                          TimeSpan?                   RequestTimeout)

        {

            #region Initial checks

            if (!ChargingPools.Any())
                return PushChargingPoolDataResult.NoOperation(Id, this, ChargingPools);

            #endregion

            var result = await UpdateChargePointInfos(ChargingPools.SafeSelectMany(pool => pool.EVSEs),

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

            return new WWCP.PushChargingPoolDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion

        #region DeleteStaticData(ChargingPools, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging pools from the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// <param name="TransmissionType">Whether to send the charging pool update directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushChargingPoolDataResult>

            ISendPOIData.DeleteStaticData(IEnumerable<IChargingPool>  ChargingPools,
                                          TransmissionTypes           TransmissionType,

                                          DateTime?                   Timestamp,
                                          CancellationToken           CancellationToken,
                                          EventTracking_Id?           EventTrackingId,
                                          TimeSpan?                   RequestTimeout)

        {

            #region Initial checks

            if (!ChargingPools.Any())
                return PushChargingPoolDataResult.NoOperation(Id, this, ChargingPools);

            #endregion

            var result = await UpdateChargePointInfos(ChargingPools.SafeSelectMany(pool => pool.EVSEs),

                                                      Timestamp,
                                                      CancellationToken,
                                                      EventTrackingId,
                                                      RequestTimeout);

            return new WWCP.PushChargingPoolDataResult(
                       result.AuthId,
                       this,
                       result.Result,
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       Array.Empty<WWCP.PushSingleChargingPoolDataResult>(),
                       result.Description,
                       result.Warnings,
                       result.Runtime
                   );

        }

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pool admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging pool admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingPoolAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                           TransmissionType,

                                               DateTime?                                   Timestamp,
                                               CancellationToken                           CancellationToken,
                                               EventTracking_Id?                           EventTrackingId,
                                               TimeSpan?                                   RequestTimeout)


                => Task.FromResult(PushChargingPoolAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging pool status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging pool status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging pool status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingPoolStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingPoolStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                      TransmissionType,

                                     DateTime?                              Timestamp,
                                     CancellationToken                      CancellationToken,
                                     EventTracking_Id?                      EventTrackingId,
                                     TimeSpan?                              RequestTimeout)


                => Task.FromResult(PushChargingPoolStatusResult.NoOperation(Id, this));

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station operator(s)...

        #region SetStaticData   (ChargingStationOperator, ...)

        /// <summary>
        /// Set the EVSE data of the given charging station operator as new static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendPOIData.SetStaticData(IChargingStationOperator  ChargingStationOperator,
                                       TransmissionTypes         TransmissionType,

                                       DateTime?                 Timestamp,
                                       CancellationToken         CancellationToken,
                                       EventTracking_Id?         EventTrackingId,
                                       TimeSpan?                 RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return await SetChargePointInfos(ChargingStationOperator.EVSEs,

                                             Timestamp,
                                             CancellationToken,
                                             EventTrackingId,
                                             RequestTimeout).

                                             ConfigureAwait(false);

        }

        #endregion

        #region AddStaticData   (ChargingStationOperator, ...)

        /// <summary>
        /// Add the EVSE data of the given charging station operator to the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendPOIData.AddStaticData(IChargingStationOperator  ChargingStationOperator,
                                       TransmissionTypes         TransmissionType,

                                       DateTime?                 Timestamp,
                                       CancellationToken         CancellationToken,
                                       EventTracking_Id          EventTrackingId,
                                       TimeSpan?                 RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return await UpdateChargePointInfos(ChargingStationOperator.EVSEs,

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

        }

        #endregion

        #region UpdateStaticData(ChargingStationOperator, ...)

        /// <summary>
        /// Update the EVSE data of the given charging station operator within the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendPOIData.UpdateStaticData(IChargingStationOperator  ChargingStationOperator,
                                          String?                   PropertyName,
                                          Object?                   NewValue,
                                          Object?                   OldValue,
                                          Context?                  DataSource,
                                          TransmissionTypes         TransmissionType,

                                          DateTime?                 Timestamp,
                                          CancellationToken         CancellationToken,
                                          EventTracking_Id          EventTrackingId,
                                          TimeSpan?                 RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperator == null)
                throw new ArgumentNullException(nameof(ChargingStationOperator), "The given charging station operator must not be null!");

            #endregion

            return await UpdateChargePointInfos(ChargingStationOperator.EVSEs,

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

        }

        #endregion

        #region DeleteStaticData(ChargingStationOperator, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station operator from the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStationOperator">A charging station operator.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendPOIData.DeleteStaticData(IChargingStationOperator  ChargingStationOperator,
                                          TransmissionTypes         TransmissionType,

                                          DateTime?                 Timestamp,
                                          CancellationToken         CancellationToken,
                                          EventTracking_Id          EventTrackingId,
                                          TimeSpan?                 RequestTimeout)

                => Task.FromResult(PushEVSEDataResult.NoOperation(Id, this, null));

        #endregion


        #region SetStaticData   (ChargingStationOperators, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging station operators as new static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendPOIData.SetStaticData(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                       TransmissionTypes                      TransmissionType,

                                       DateTime?                              Timestamp,
                                       CancellationToken                      CancellationToken,
                                       EventTracking_Id                       EventTrackingId,
                                       TimeSpan?                              RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return await SetChargePointInfos(ChargingStationOperators.SafeSelectMany(stationoperator => stationoperator.EVSEs),

                                             Timestamp,
                                             CancellationToken,
                                             EventTrackingId,
                                             RequestTimeout).

                                             ConfigureAwait(false);

        }

        #endregion

        #region AddStaticData   (ChargingStationOperators, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging station operators to the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendPOIData.AddStaticData(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                       TransmissionTypes                      TransmissionType,

                                       DateTime?                              Timestamp,
                                       CancellationToken                      CancellationToken,
                                       EventTracking_Id                       EventTrackingId,
                                       TimeSpan?                              RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return await UpdateChargePointInfos(ChargingStationOperators.SafeSelectMany(stationoperator => stationoperator.EVSEs),

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

        }

        #endregion

        #region UpdateStaticData(ChargingStationOperators, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging station operators within the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendPOIData.UpdateStaticData(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                          TransmissionTypes                      TransmissionType,

                                          DateTime?                              Timestamp,
                                          CancellationToken                      CancellationToken,
                                          EventTracking_Id                       EventTrackingId,
                                          TimeSpan?                              RequestTimeout)

        {

            #region Initial checks

            if (ChargingStationOperators == null)
                throw new ArgumentNullException(nameof(ChargingStationOperators), "The given enumeration of charging station operators must not be null!");

            #endregion

            return await UpdateChargePointInfos(ChargingStationOperators.SafeSelectMany(stationoperator => stationoperator.EVSEs),

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

        }

        #endregion

        #region DeleteStaticData(ChargingStationOperators, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging station operators from the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStationOperators">An enumeration of charging station operators.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendPOIData.DeleteStaticData(IEnumerable<IChargingStationOperator>  ChargingStationOperators,
                                          TransmissionTypes                      TransmissionType,

                                          DateTime?                              Timestamp,
                                          CancellationToken                      CancellationToken,
                                          EventTracking_Id                       EventTrackingId,
                                          TimeSpan?                              RequestTimeout)

                => Task.FromResult(PushEVSEDataResult.NoOperation(Id, this, null));

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station operator admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of charging station operator admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationOperatorAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                                      TransmissionType,

                                               DateTime?                                              Timestamp,
                                               CancellationToken                                      CancellationToken,
                                               EventTracking_Id?                                      EventTrackingId,
                                               TimeSpan?                                              RequestTimeout)


                => Task.FromResult(PushChargingStationOperatorAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of charging station operator status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of charging station operator status updates.</param>
        /// <param name="TransmissionType">Whether to send the charging station operator status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushChargingStationOperatorStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                                 TransmissionType,

                                     DateTime?                                         Timestamp,
                                     CancellationToken                                 CancellationToken,
                                     EventTracking_Id?                                 EventTrackingId,
                                     TimeSpan?                                         RequestTimeout)


                => Task.FromResult(PushChargingStationOperatorStatusResult.NoOperation(Id, this));

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Roaming network...

        #region SetStaticData   (RoamingNetwork, ...)

        /// <summary>
        /// Set the EVSE data of the given roaming network as new static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendPOIData.SetStaticData(IRoamingNetwork     RoamingNetwork,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken   CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return await SetChargePointInfos(RoamingNetwork.EVSEs,

                                             Timestamp,
                                             CancellationToken,
                                             EventTrackingId,
                                             RequestTimeout).

                                             ConfigureAwait(false);

        }

        #endregion

        #region AddStaticData   (RoamingNetwork, ...)

        /// <summary>
        /// Add the EVSE data of the given roaming network to the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendPOIData.AddStaticData(IRoamingNetwork     RoamingNetwork,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken   CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return await UpdateChargePointInfos(RoamingNetwork.EVSEs,

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

        }

        #endregion

        #region UpdateStaticData(RoamingNetwork, ...)

        /// <summary>
        /// Update the EVSE data of the given roaming network within the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<PushEVSEDataResult>

            ISendPOIData.UpdateStaticData(IRoamingNetwork     RoamingNetwork,
                                          String              PropertyName,
                                          Object?             NewValue,
                                          Object?             OldValue,
                                          Context?            DataSource,
                                          TransmissionTypes   TransmissionType,

                                          DateTime?           Timestamp,
                                          CancellationToken   CancellationToken,
                                          EventTracking_Id    EventTrackingId,
                                          TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork), "The given roaming network must not be null!");

            #endregion

            return await UpdateChargePointInfos(RoamingNetwork.EVSEs,

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

        }

        #endregion

        #region DeleteStaticData(RoamingNetwork, ...)

        /// <summary>
        /// Delete the EVSE data of the given roaming network from the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="RoamingNetwork">A roaming network to upload.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushEVSEDataResult>

            ISendPOIData.DeleteStaticData(IRoamingNetwork     RoamingNetwork,
                                          TransmissionTypes   TransmissionType,

                                          DateTime?           Timestamp,
                                          CancellationToken   CancellationToken,
                                          EventTracking_Id    EventTrackingId,
                                          TimeSpan?           RequestTimeout)

                => Task.FromResult(PushEVSEDataResult.NoOperation(Id, this, null));

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of roaming network admin status updates.
        /// </summary>
        /// <param name="AdminStatusUpdates">An enumeration of roaming network admin status updates.</param>
        /// <param name="TransmissionType">Whether to send the roaming network admin status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushRoamingNetworkAdminStatusResult>

            ISendAdminStatus.UpdateAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate>  AdminStatusUpdates,
                                               TransmissionTypes                             TransmissionType,

                                               DateTime?                                     Timestamp,
                                               CancellationToken                             CancellationToken,
                                               EventTracking_Id?                             EventTrackingId,
                                               TimeSpan?                                     RequestTimeout)


                => Task.FromResult(PushRoamingNetworkAdminStatusResult.NoOperation(Id, this));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueue, ...)

        /// <summary>
        /// Update the given enumeration of roaming network status updates.
        /// </summary>
        /// <param name="StatusUpdates">An enumeration of roaming network status updates.</param>
        /// <param name="TransmissionType">Whether to send the roaming network status updates directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<PushRoamingNetworkStatusResult>

            ISendStatus.UpdateStatus(IEnumerable<RoamingNetworkStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                        TransmissionType,

                                     DateTime?                                Timestamp,
                                     CancellationToken                        CancellationToken,
                                     EventTracking_Id?                        EventTrackingId,
                                     TimeSpan?                                RequestTimeout)


                => Task.FromResult(PushRoamingNetworkStatusResult.NoOperation(Id, this));

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

                           DateTime?                         Timestamp             = null,
                           EventTracking_Id?                 EventTrackingId       = null,
                           TimeSpan?                         RequestTimeout        = null,
                           CancellationToken                 CancellationToken     = default)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var StartTime = DateTime.UtcNow;

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
                DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            DateTime         Endtime;
            TimeSpan         Runtime;
            AuthStartResult  result;

            if (DisableAuthentication)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStartResult.AdminDown(Id,
                                                     this,
                                                     SessionId,
                                                     Runtime: Runtime);

            }

            else
            {

                var EMTId     = new EMT_Id(
                                    LocalAuthentication.AuthToken.ToString(),
                                    TokenRepresentations.Plain,
                                    TokenTypes.RFID
                                );

                var response  = await CPORoaming.GetSingleRoamingAuthorisation(EMTId,

                                                                               Timestamp,
                                                                               CancellationToken,
                                                                               EventTrackingId,
                                                                               RequestTimeout);


                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if (response?.HTTPStatusCode            == HTTPStatusCode.OK &&
                    response?.Content                   != null              &&
                    response?.Content.Result.ResultCode == ResultCodes.OK)
                {

                    result = AuthStartResult.Authorized(Id,
                                                        this,
                                                        ChargingSession_Id.NewRandom,
                                                        ProviderId:     response.Content.RoamingAuthorisationInfo != null
                                                                            ? response.Content.RoamingAuthorisationInfo.ContractId.ProviderId.ToWWCP()
                                                                            : EMobilityProvider_Id.Parse(Country.Germany, "GEF"),
                                                        ContractId:     response.Content.RoamingAuthorisationInfo.ContractId.ToString(),
                                                        PrintedNumber:  response.Content.RoamingAuthorisationInfo.PrintedNumber,
                                                        ExpiryDate:     response.Content.RoamingAuthorisationInfo.ExpiryDate,
                                                        Runtime:        Runtime);

                    lock (_Lookup)
                    {

                        if (_Lookup.TryGetValue(EMTId, out Contract_Id ExistingContractId))
                        {

                            if (ExistingContractId != response.Content.RoamingAuthorisationInfo.ContractId)
                            {

                                // Replace

                            }

                        }

                        else
                        {

                            // Add
                            _Lookup.Add(EMTId, response.Content.RoamingAuthorisationInfo.ContractId);

                            var time = DateTime.UtcNow;
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

                    lock (_Lookup)
                    {

                        // Remove

                        _Lookup.Remove(EMTId);

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
                DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStartResponse));
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

                          DateTime?                         Timestamp             = null,
                          EventTracking_Id?                 EventTrackingId       = null,
                          TimeSpan?                         RequestTimeout        = null,
                          CancellationToken                 CancellationToken     = default)
        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var StartTime = DateTime.UtcNow;

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
                DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            DateTime        Endtime;
            TimeSpan        Runtime;
            AuthStopResult  result;

            if (DisableAuthentication)
            {
                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStopResult.AdminDown(Id,
                                                    this,
                                                    SessionId,
                                                    Runtime: Runtime);
            }

            else
            {

                var response = await CPORoaming.GetSingleRoamingAuthorisation(new EMT_Id(
                                                                                  LocalAuthentication.AuthToken.ToString(),
                                                                                  TokenRepresentations.Plain,
                                                                                  TokenTypes.RFID
                                                                              ),

                                                                              Timestamp,
                                                                              CancellationToken,
                                                                              EventTrackingId,
                                                                              RequestTimeout).ConfigureAwait(false);


                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if (response?.HTTPStatusCode            == HTTPStatusCode.OK &&
                    response?.Content                   != null              &&
                    response?.Content.Result.ResultCode == ResultCodes.OK)
                {

                    result = AuthStopResult.Authorized(Id,
                                                       this,
                                                       ChargingSession_Id.NewRandom,
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
                DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SendChargeDetailRecords(ChargeDetailRecords, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Send a charge detail record to an OCHP server.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="TransmissionType">Whether to send the CDR directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public async Task<SendCDRsResult>

            SendChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                                    TransmissionTypes                TransmissionType    = TransmissionTypes.Enqueue,

                                    DateTime?                        Timestamp           = null,
                                    EventTracking_Id?                EventTrackingId     = null,
                                    TimeSpan?                        RequestTimeout      = null,
                                    CancellationToken                CancellationToken   = default)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (EventTrackingId == null)
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
                    FilteredCDRs.Add(SendCDRResult.Filtered(DateTime.UtcNow,
                                                            cdr,
                                                            Warning: Warning.Create(I18NString.Create(Languages.en, "This charge detail record was filtered!"))));

            }

            #endregion

            #region Send OnSendCDRsRequest event

            var StartTime = DateTime.UtcNow;

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
                DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRsRequest));
            }

            #endregion


            #region if disabled => 'AdminDown'...

            DateTime        Endtime;
            TimeSpan        Runtime;
            SendCDRsResult  results;

            if (DisableSendChargeDetailRecords)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                results  = SendCDRsResult.AdminDown(DateTime.UtcNow,
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

                                OnEnqueueSendCDRsRequest?.Invoke(DateTime.UtcNow,
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
                                DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRsRequest));
                            }

                            #endregion

                            var EnquenedCDRsResults = new List<SendCDRResult>();

                            foreach (var chargeDetailRecord in ForwardedCDRs)
                            {

                                try
                                {

                                    chargeDetailRecordsQueue.Add(chargeDetailRecord.ToOCHP(ContractIdDelegate: emtid => _Lookup[emtid],
                                                                                           CustomEVSEIdMapper: null));
                                    EnquenedCDRsResults.Add(SendCDRResult.Enqueued(DateTime.UtcNow,
                                                                                   chargeDetailRecord));

                                }
                                catch (Exception e)
                                {
                                    EnquenedCDRsResults.Add(SendCDRResult.CouldNotConvertCDRFormat(DateTime.UtcNow,
                                                                                                   chargeDetailRecord,
                                                                                                   Warning: Warning.Create(I18NString.Create(Languages.en, e.Message))));
                                }

                            }

                            Endtime      = DateTime.UtcNow;
                            Runtime      = Endtime - StartTime;
                            results      = (!FilteredCDRs.Any())
                                               ? SendCDRsResult.Enqueued(DateTime.UtcNow, Id, this, ForwardedCDRs, I18NString.Create(Languages.en, "Enqueued for at least " + FlushChargeDetailRecordsEvery.TotalSeconds + " seconds!"), Runtime: Runtime)
                                               : SendCDRsResult.Mixed   (DateTime.UtcNow, Id, this, FilteredCDRs.Concat(ForwardedCDRs.Select(cdr => SendCDRResult.Enqueued(DateTime.UtcNow, cdr))), Runtime: Runtime);
                            invokeTimer  = true;

                        }

                        #endregion

                        #region ...or send at once!

                        else
                        {

                            HTTPResponse<AddCDRsResponse> response;

                            try
                            {

                                response = await CPORoaming.AddCDRs(ForwardedCDRs.Select(cdr => cdr.ToOCHP(ContractIdDelegate:  emtid => _Lookup[emtid],
                                                                                                           CustomEVSEIdMapper:  null)).ToArray(),

                                                                    Timestamp,
                                                                    CancellationToken,
                                                                    EventTrackingId,
                                                                    RequestTimeout);

                                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                                    response.Content        != null)
                                {

                                    var ImplausibleCDRIds = response.Content.ImplausibleCDRs?.ToHashSet() ?? new HashSet<CDR_Id>();
                                    var ImplausibleCDRs   = response.Content.ImplausibleCDRs.SafeAny()
                                                                ? ChargeDetailRecords.Where(cdr => ImplausibleCDRIds.Contains(cdr.SessionId.ToOCHP()))
                                                                : new ChargeDetailRecord[0];

                                    switch (response.Content.Result.ResultCode)
                                    {

                                        case ResultCodes.OK:
                                            if (!FilteredCDRs.Any())
                                                results = SendCDRsResult.Success(DateTime.UtcNow, Id, this, ForwardedCDRs);
                                            else
                                                results = SendCDRsResult.Mixed  (DateTime.UtcNow, Id, this, FilteredCDRs.Concat(ForwardedCDRs.Select(cdr => SendCDRResult.Success(DateTime.UtcNow, cdr))));
                                            break;

                                        case ResultCodes.Partly:
                                            if (!FilteredCDRs.Any())
                                                results = SendCDRsResult.Mixed  (DateTime.UtcNow, Id, this, ForwardedCDRs.  Select(cdr => SendCDRResult.Success(DateTime.UtcNow, cdr)).Concat(
                                                                                                            ImplausibleCDRs.Select(cdr => SendCDRResult.Error  (DateTime.UtcNow,
                                                                                                                                                                cdr,
                                                                                                                                                                Warning: Warning.Create(I18NString.Create(Languages.en, "Implausible charge detail record!"))))
                                                                                            ));
                                            else
                                                results = SendCDRsResult.Mixed  (DateTime.UtcNow, Id, this, FilteredCDRs.Concat(
                                                                                                                ForwardedCDRs.  Select(cdr => SendCDRResult.Success(DateTime.UtcNow, cdr)).Concat(
                                                                                                                ImplausibleCDRs.Select(cdr => SendCDRResult.Error(DateTime.UtcNow,
                                                                                                                                                                  cdr,
                                                                                                                                                                  Warning: Warning.Create(I18NString.Create(Languages.en, "Implausible charge detail record!"))))
                                                                                            )));
                                            break;

                                        default:
                                            if (!FilteredCDRs.Any())
                                                results = SendCDRsResult.Error(DateTime.UtcNow, Id, this, ForwardedCDRs, Warning.Create(I18NString.Create(Languages.en, response.Content.Result.ResultCode + " - " + response.Content.Result.Description)));
                                            else
                                                results = SendCDRsResult.Mixed(DateTime.UtcNow, Id, this, FilteredCDRs.Concat(ForwardedCDRs.Select(cdr => SendCDRResult.Error(DateTime.UtcNow,
                                                                                                                                                                              cdr,
                                                                                                                                                                              Warning.Create(I18NString.Create(Languages.en, response.Content.Result.ResultCode + " - " + response.Content.Result.Description))))));
                                            break;

                                    }

                                }

                                else
                                    if (!FilteredCDRs.Any())
                                        results = SendCDRsResult.Error(DateTime.UtcNow, Id, this, ForwardedCDRs, Warning.Create(I18NString.Create(Languages.en, response.HTTPBodyAsUTF8String)));
                                    else
                                        results = SendCDRsResult.Mixed(DateTime.UtcNow, Id, this, FilteredCDRs.
                                                                                     Concat(ForwardedCDRs.Select(cdr => SendCDRResult.Error(DateTime.UtcNow, cdr, Warning.Create(I18NString.Create(Languages.en, response.HTTPBodyAsUTF8String))))));

                            }
                            catch (Exception e)
                            {
                                if (!FilteredCDRs.Any())
                                    results = SendCDRsResult.Error(DateTime.UtcNow, Id, this, ForwardedCDRs, Warning.Create(I18NString.Create(Languages.en, e.Message)));
                                else
                                    results = SendCDRsResult.Mixed(DateTime.UtcNow, Id, this, FilteredCDRs.
                                                                                 Concat(ForwardedCDRs.Select(cdr => SendCDRResult.Error(DateTime.UtcNow, cdr, Warning.Create(I18NString.Create(Languages.en, e.Message))))));
                            }


                            Endtime  = DateTime.UtcNow;
                            Runtime  = Endtime - StartTime;

                            foreach (var result in results)
                                RoamingNetwork.SessionsStore.CDRForwarded(result.ChargeDetailRecord.SessionId, result);

                        }

                        #endregion

                    }

                    #region Could not get the lock for toooo long!

                    else
                    {

                        Endtime  = DateTime.UtcNow;
                        Runtime  = Endtime - StartTime;
                        results  = SendCDRsResult.Timeout(DateTime.UtcNow,
                                                          Id,
                                                          this,
                                                          ChargeDetailRecords,
                                                          I18NString.Create(Languages.en, "Could not " + (TransmissionType == TransmissionTypes.Enqueue ? "enqueue" : "send") + " charge detail records!"),
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
                DebugX.LogException(e, nameof(WWCPEMPAdapter) + "." + nameof(OnSendCDRsResponse));
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

                    while (e.InnerException != null)
                        e = e.InnerException;

                    DebugX.Log("A exception occured during ServiceCheck: " + e.Message + Environment.NewLine + e.StackTrace);

                    OnWWCPCPOAdapterException?.Invoke(DateTime.UtcNow,
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

                    while (e.InnerException != null)
                        e = e.InnerException;

                    DebugX.LogT(nameof(WWCPEMPAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

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
            if (evsesToAddQueueCopy.              Value != null ||
                evseDataQueueCopy.                Value != null ||
                evseStatusChangesDelayedQueueCopy.Value != null ||
                evsesToRemoveQueueCopy.           Value != null)
            {

                // Use the events to evaluate if something went wrong!

                var EventTrackingId = EventTracking_Id.New;

                #region Send new EVSE data

                if (evsesToAddQueueCopy.Value.Count > 0)
                {

                    var EVSEsToAddTask = serviceRunId == 1
                                             ? (this as ISendPOIData).SetStaticData(evsesToAddQueueCopy.Value, EventTrackingId: EventTrackingId)
                                             : (this as ISendPOIData).AddStaticData(evsesToAddQueueCopy.Value, EventTrackingId: EventTrackingId);

                    EVSEsToAddTask.Wait();

                }

                #endregion

                #region Send changed EVSE data

                if (evseDataQueueCopy.Value.Count > 0)
                {

                    // Surpress EVSE data updates for all newly added EVSEs
                    var EVSEsWithoutNewEVSEs = evseDataQueueCopy.Value.
                                                   Where(evse => !evsesToAddQueueCopy.Value.Contains(evse)).
                                                   ToArray();


                    if (EVSEsWithoutNewEVSEs.Length > 0)
                    {

                        var SetChargePointInfosTask = (this as ISendPOIData).UpdateStaticData(EVSEsWithoutNewEVSEs, EventTrackingId: EventTrackingId);

                        SetChargePointInfosTask.Wait();

                    }

                }

                #endregion

                #region Send changed EVSE status

                if (evseStatusChangesDelayedQueueCopy.Value.Count > 0)
                {

                    var UpdateEVSEStatusTask = UpdateEVSEStatus(evseStatusChangesDelayedQueueCopy.Value,
                                                                EventTrackingId: EventTrackingId);

                    UpdateEVSEStatusTask.Wait();

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

                //    SendOnWarnings(DateTime.UtcNow,
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

                    //    SendOnWarnings(DateTime.UtcNow,
                    //                   nameof(WWCPCPOAdapter) + Id,
                    //                   "PushEVSEDataTask",
                    //                   PushEVSEDataTask.Result.Warnings);

                    //}

                }

            }

            #endregion

            #region Send changed EVSE status

            if (!DisablePushStatus &&
                evseStatusChangesDelayedQueueCopy.Count > 0)
            {

                var pushEVSEStatusResult = await UpdateStatus(evseStatusChangesDelayedQueueCopy,
                                                              TransmissionTypes.Direct,

                                                              DateTime.UtcNow,
                                                              new CancellationTokenSource().Token,
                                                              EventTrackingId,
                                                              DefaultRequestTimeout).
                                                     ConfigureAwait(false);

                if (pushEVSEStatusResult.Warnings.Any())
                {

                    SendOnWarnings(DateTime.UtcNow,
                                   nameof(WWCPEMPAdapter) + Id,
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

                    //    SendOnWarnings(DateTime.UtcNow,
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

                                                                  DateTime.UtcNow,
                                                                  new CancellationTokenSource().Token,
                                                                  EventTracking_Id.New,
                                                                  DefaultRequestTimeout).
                                                     ConfigureAwait(false);

                if (pushEVSEStatusResult.Warnings.Any())
                {

                    SendOnWarnings(DateTime.UtcNow,
                                   nameof(WWCPEMPAdapter) + Id,
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

            if (!DisablePushStatus && !DisableEVSEStatusRefresh)
            {

                try
                {

                    RefreshEVSEStatus().Wait();

                }
                catch (Exception e)
                {

                    while (e.InnerException != null)
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


            EVSEStatusRefreshEvent?.Invoke(DateTime.UtcNow,
                                           this,
                                           "EVSE status refresh, as every " + EVSEStatusRefreshEvery.TotalHours.ToString() + " hours!");

            #region Data

            EVSE_Id?             _EVSEId;
            PushEVSEStatusResult result = null;

            var StartTime                  = DateTime.UtcNow;
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

                            _EVSEId = _CustomEVSEIdMapper != null
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
                        Warnings.Add(Warning.Create(I18NString.Create(Languages.en, e.Message), evsestatus));
                    }

                }

                #endregion

                #region Upload EVSE status

                if (AllEVSEStatusRefreshments.Count > 0)
                {

                    var response = await CPORoaming.
                                             UpdateStatus(AllEVSEStatusRefreshments,
                                                          null,
                                                          // TTL => 2x refresh intervall
                                                          DateTime.UtcNow + EVSEStatusRefreshEvery + EVSEStatusRefreshEvery

                                                         //Timestamp,
                                                         //CancellationToken,
                                                         //EventTrackingId,
                                                         //RequestTimeout
                                                         );


                    var Endtime = DateTime.UtcNow;
                    var Runtime = Endtime - StartTime;

                    if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                        response.Content        != null)
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
                                                        response.HTTPBody != null
                                                            ? Warnings.AddAndReturnList(I18NString.Create(Languages.en, response.HTTPBody.ToUTF8String()))
                                                            : Warnings.AddAndReturnList(I18NString.Create(Languages.en, "No HTTP body received!")),
                                                        Runtime);

                }

                #endregion

            }
            catch (Exception e)
            {

                while (e.InnerException != null)
                    e = e.InnerException;

                DebugX.LogT(nameof(WWCPEMPAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

                result = PushEVSEStatusResult.Error(Id,
                                                this,
                                                new EVSEStatusUpdate[0],
                                                e.Message,
                                                Warnings,
                                                DateTime.UtcNow - StartTime);

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

                var response = await CPORoaming.AddCDRs(CDRInfos,

                                                        DateTime.UtcNow,
                                                        new CancellationTokenSource().Token,
                                                        EventTracking_Id.New,
                                                        DefaultRequestTimeout).
                                                ConfigureAwait(false);

                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content        != null)
                {

                    switch (response.Content.Result.ResultCode)
                    {

                        case ResultCodes.OK:
                            {
                                foreach (var CDRInfo in CDRInfos)
                                    RoamingNetwork.SessionsStore.CDRForwarded(CDRInfo.CDRId.ToWWCP(),
                                                                              SendCDRResult.Success(DateTime.UtcNow,
                                                                                                    CDRInfo.GetInternalDataAs<ChargeDetailRecord>(OCHPMapper.WWCP_CDR),
                                                                                                    Runtime: response.Runtime));
                            }
                            break;

                        case ResultCodes.Partly:
                            {

                                var implausibleCDRs = response.Content.ImplausibleCDRs.ToHashSet();

                                foreach (var CDRInfo in CDRInfos)
                                    RoamingNetwork.SessionsStore.CDRForwarded(CDRInfo.CDRId.ToWWCP(),
                                                                              implausibleCDRs.Contains(CDRInfo.CDRId)
                                                                                  ? SendCDRResult.Error  (DateTime.UtcNow,
                                                                                                          CDRInfo.GetInternalDataAs<ChargeDetailRecord>(OCHPMapper.WWCP_CDR),
                                                                                                          Warning.Create(I18NString.Create(Languages.en, "implausible charge detail record!")),
                                                                                                          Runtime: response.Runtime)
                                                                                  : SendCDRResult.Success(DateTime.UtcNow,
                                                                                                          CDRInfo.GetInternalDataAs<ChargeDetailRecord>(OCHPMapper.WWCP_CDR),
                                                                                                          Runtime: response.Runtime));

                            }
                            break;

                        default:
                            {
                                foreach (var CDRInfo in CDRInfos)
                                    RoamingNetwork.SessionsStore.CDRForwarded(CDRInfo.CDRId.ToWWCP(),
                                                                              SendCDRResult.Error(DateTime.UtcNow,
                                                                                                  CDRInfo.GetInternalDataAs<ChargeDetailRecord>(OCHPMapper.WWCP_CDR),
                                                                                                  Warning.Create(I18NString.Create(Languages.en, response.Content.Result.ResultCode.ToString() + " - " + response.Content.Result.Description)),
                                                                                                  Runtime: response.Runtime));
                            }
                            break;

                    }

                }

                //ToDo: Re-add to queue if it could not be send...

            }
            catch (Exception e)
            {
                foreach (var CDRInfo in CDRInfos)
                    RoamingNetwork.SessionsStore.CDRForwarded(CDRInfo.CDRId.ToWWCP(),
                                                              SendCDRResult.Error(DateTime.UtcNow,
                                                                                  CDRInfo.GetInternalDataAs<ChargeDetailRecord>(OCHPMapper.WWCP_CDR),
                                                                                  Warning.Create(I18NString.Create(Languages.en, e.Message)),
                                                                                  Runtime: TimeSpan.Zero));
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
        public static Boolean operator == (WWCPEMPAdapter WWCPCPOAdapter1, WWCPEMPAdapter WWCPCPOAdapter2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(WWCPCPOAdapter1, WWCPCPOAdapter2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) WWCPCPOAdapter1 == null) || ((Object) WWCPCPOAdapter2 == null))
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
        public static Boolean operator != (WWCPEMPAdapter WWCPCPOAdapter1, WWCPEMPAdapter WWCPCPOAdapter2)

            => !(WWCPCPOAdapter1 == WWCPCPOAdapter2);

        #endregion

        #region Operator <  (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (WWCPEMPAdapter  WWCPCPOAdapter1,
                                          WWCPEMPAdapter  WWCPCPOAdapter2)
        {

            if ((Object) WWCPCPOAdapter1 == null)
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
        public static Boolean operator <= (WWCPEMPAdapter WWCPCPOAdapter1,
                                           WWCPEMPAdapter WWCPCPOAdapter2)

            => !(WWCPCPOAdapter1 > WWCPCPOAdapter2);

        #endregion

        #region Operator >  (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (WWCPEMPAdapter WWCPCPOAdapter1,
                                          WWCPEMPAdapter WWCPCPOAdapter2)
        {

            if ((Object) WWCPCPOAdapter1 == null)
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
        public static Boolean operator >= (WWCPEMPAdapter WWCPCPOAdapter1,
                                           WWCPEMPAdapter WWCPCPOAdapter2)

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

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var WWCPCPOAdapter = Object as WWCPEMPAdapter;
            if ((Object) WWCPCPOAdapter == null)
                throw new ArgumentException("The given object is not an WWCPCPOAdapter!", nameof(Object));

            return CompareTo(WWCPCPOAdapter);

        }

        #endregion

        #region CompareTo(WWCPCPOAdapter)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter">An WWCPCPOAdapter object to compare with.</param>
        public Int32 CompareTo(WWCPEMPAdapter WWCPCPOAdapter)
        {

            if ((Object) WWCPCPOAdapter == null)
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

            if (Object == null)
                return false;

            var WWCPCPOAdapter = Object as WWCPEMPAdapter;
            if ((Object) WWCPCPOAdapter == null)
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
        public Boolean Equals(WWCPEMPAdapter WWCPCPOAdapter)
        {

            if ((Object) WWCPCPOAdapter == null)
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

            => "OCHP " + Version.Number + " CPO Adapter " + Id;

        #endregion


    }

}
