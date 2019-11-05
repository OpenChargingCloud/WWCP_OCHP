/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
using System.IO;
using System.Linq;
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{


    public enum ChargeDetailRecordFilters
    {
        drop,
        forward
    }

    /// <summary>
    /// A WWCP wrapper for the OCHP CPO Roaming client which maps
    /// WWCP data structures onto OCHP data structures and vice versa.
    /// </summary>
    public class WWCPCPOAdapter : AWWCPCSOAdapter<CDRInfo>,
                                  ICSORoamingProvider,
                                  IEquatable<WWCPCPOAdapter>,
                                  IComparable<WWCPCPOAdapter>,
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

        private                  UInt64                                         _ServiceRunId;
        //private                  UInt64                                         _StatusRunId;
        private                  IncludeEVSEDelegate                            _IncludeEVSEs;
        private                  IncludeEVSEIdDelegate                          _IncludeEVSEIds;

        public readonly static   TimeSpan                                       DefaultRequestTimeout = TimeSpan.FromSeconds(30);


        private readonly         Dictionary<EMT_Id, Contract_Id>                _Lookup;

        private static readonly  Char[] rs                                      = new Char[] { (Char) 30 };

        #endregion

        #region Properties

        IId ISendAuthorizeStartStop.AuthId
            => Id;

        IId ISendChargeDetailRecords.Id
            => Id;

        IEnumerable<IId> ISendChargeDetailRecords.Ids
            => Ids.Cast<IId>();

        /// <summary>
        /// The wrapped CPO roaming object.
        /// </summary>
        public CPORoaming CPORoaming { get; }


        /// <summary>
        /// The CPO client.
        /// </summary>
        public CPOClient CPOClient
            => CPORoaming?.CPOClient;

        /// <summary>
        /// The CPO client logger.
        /// </summary>
        public CPOClient.CPOClientLogger ClientLogger
            => CPORoaming?.CPOClient?.Logger;


        /// <summary>
        /// The CPO server.
        /// </summary>
        public CPOServer CPOServer
            => CPORoaming?.CPOServer;

        /// <summary>
        /// The CPO server logger.
        /// </summary>
        public CPOServerLogger ServerLogger
            => CPORoaming?.CPOServerLogger;


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
                _ServiceCheckEvery = (UInt32)value.TotalSeconds;
            }

        }

        #endregion


        public IncludeChargePointsDelegate  IncludeChargePoints   { get; set; }

        public Func<ChargeDetailRecord, ChargeDetailRecordFilters> ChargeDetailRecordFilter { get; set; }


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
        public event OnAuthorizeStartRequestDelegate                  OnAuthorizeStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate                 OnAuthorizeStartResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStartRequestDelegate              OnAuthorizeEVSEStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStartResponseDelegate             OnAuthorizeEVSEStartResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStartRequestDelegate   OnAuthorizeChargingStationStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStartResponseDelegate  OnAuthorizeChargingStationStartResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified for charging at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStartRequestDelegate      OnAuthorizeChargingPoolStartRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified for charging at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStartResponseDelegate     OnAuthorizeChargingPoolStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate                  OnAuthorizeStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate                 OnAuthorizeStopResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStopRequestDelegate              OnAuthorizeEVSEStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given EVSE.
        /// </summary>
        public event OnAuthorizeEVSEStopResponseDelegate             OnAuthorizeEVSEStopResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStopRequestDelegate   OnAuthorizeChargingStationStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging station.
        /// </summary>
        public event OnAuthorizeChargingStationStopResponseDelegate  OnAuthorizeChargingStationStopResponse;


        /// <summary>
        /// An event fired whenever an authentication token will be verified to stop a charging process at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStopRequestDelegate      OnAuthorizeChargingPoolStopRequest;

        /// <summary>
        /// An event fired whenever an authentication token had been verified to stop a charging process at the given charging pool.
        /// </summary>
        public event OnAuthorizeChargingPoolStopResponseDelegate     OnAuthorizeChargingPoolStopResponse;

        #endregion

        #region OnSendCDRRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record was enqueued for later sending upstream.
        /// </summary>
        public event OnSendCDRRequestDelegate   OnEnqueueSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record will be send upstream.
        /// </summary>
        public event OnSendCDRRequestDelegate   OnSendCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record had been sent upstream.
        /// </summary>
        public event OnSendCDRResponseDelegate  OnSendCDRsResponse;

        #endregion


        #region OnWWCPCPOAdapterException

        public delegate Task OnWWCPCPOAdapterExceptionDelegate(DateTime        Timestamp,
                                                               WWCPCPOAdapter  Sender,
                                                               Exception       Exception);

        public event OnWWCPCPOAdapterExceptionDelegate OnWWCPCPOAdapterException;

        #endregion


        public delegate void FlushServiceQueuesDelegate(WWCPCPOAdapter Sender, TimeSpan Every);

        public event FlushServiceQueuesDelegate FlushServiceQueuesEvent;


        public delegate void FlushEVSEStatusUpdateQueuesDelegate(WWCPCPOAdapter Sender, TimeSpan Every);

        public event FlushEVSEStatusUpdateQueuesDelegate FlushEVSEStatusUpdateQueuesEvent;


        public delegate void EVSEStatusRefreshEventDelegate(DateTime Timestamp, WWCPCPOAdapter Sender, String Message);

        public event EVSEStatusRefreshEventDelegate EVSEStatusRefreshEvent;

        #endregion

        #region Constructor(s)

        #region WWCPCPOAdapter(Id, Name, RoamingNetwork, CPORoaming, EVSE2ChargePointInfo = null)

        /// <summary>
        /// Create a new WWCP wrapper for the OCHP roaming client for Charging Station Operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="CPORoaming">A OCHP CPO roaming object to be mapped to WWCP.</param>
        /// <param name="EVSE2ChargePointInfo">A delegate to process an charge point info, e.g. before pushing it to the roaming provider.</param>
        /// <param name="ChargePointInfo2XML">A delegate to process the XML representation of an charge point info, e.g. before pushing it to the roaming provider.</param>
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
        public WWCPCPOAdapter(CSORoamingProvider_Id                                Id,
                              I18NString                                           Name,
                              I18NString                                           Description,
                              RoamingNetwork                                       RoamingNetwork,

                              CPORoaming                                           CPORoaming,
                              CustomEVSEIdMapperDelegate                           CustomEVSEIdMapper               = null,
                              EVSE2ChargePointInfoDelegate                         EVSE2ChargePointInfo             = null,
                              EVSEStatusUpdate2EVSEStatusDelegate                  EVSEStatusUpdate2EVSEStatus      = null,
                              ChargePointInfo2XMLDelegate                          ChargePointInfo2XML              = null,
                              EVSEStatus2XMLDelegate                               EVSEStatus2XML                   = null,

                              IncludeEVSEIdDelegate                                IncludeEVSEIds                   = null,
                              IncludeEVSEDelegate                                  IncludeEVSEs                     = null,
                              Func<ChargeDetailRecord, ChargeDetailRecordFilters>  ChargeDetailRecordFilter         = null,

                              TimeSpan?                                            ServiceCheckEvery                = null,
                              TimeSpan?                                            StatusCheckEvery                 = null,
                              TimeSpan?                                            EVSEStatusRefreshEvery           = null,
                              TimeSpan?                                            CDRCheckEvery                    = null,

                              Boolean                                              DisablePushData                  = false,
                              Boolean                                              DisablePushStatus                = false,
                              Boolean                                              DisableEVSEStatusRefresh         = false,
                              Boolean                                              DisableAuthentication            = false,
                              Boolean                                              DisableSendChargeDetailRecords   = false,

                              String                                               EllipticCurve                    = "P-256",
                              ECPrivateKeyParameters                               PrivateKey                       = null,
                              PublicKeyCertificates                                PublicKeyCertificates            = null,
                              DNSClient                                            DNSClient                        = null)

            : base(Id,
                   Name,
                   Description,
                   RoamingNetwork,

                   IncludeEVSEIds,
                   IncludeEVSEs,
                   //CustomEVSEIdMapper,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushStatus,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords,

                   EllipticCurve,
                   PrivateKey,
                   PublicKeyCertificates,

                   DNSClient ?? CPORoaming?.DNSClient)

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
            //this._IncludeEVSEs                        = IncludeEVSEs   ?? (evse   => true);

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

            //this.EVSEsToAddQueue                      = new HashSet<EVSE>();
            //this.EVSEsToUpdateQueue                   = new HashSet<EVSE>();
            //this.EVSEStatusChangesFastQueue           = new List<EVSEStatusUpdate>();
            //this.EVSEStatusChangesDelayedQueue        = new List<EVSEStatusUpdate>();
            //this.EVSEsToRemoveQueue                   = new HashSet<EVSE>();

            this.ChargeDetailRecordFilter             = ChargeDetailRecordFilter ?? (cdr => ChargeDetailRecordFilters.forward);

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

        #region WWCPCPOAdapter(Id, Name, RoamingNetwork, CPOClient, CPOServer, ChargePointInfoProcessing = null)

        /// <summary>
        /// Create a new WWCP wrapper for the OCHP roaming client for Charging Station Operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="CPOClient">An OCHP CPO client.</param>
        /// <param name="CPOServer">An OCHP CPO sever.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSE2ChargePointInfo">A delegate to process an charge point info, e.g. before pushing it to the roaming provider.</param>
        /// <param name="ChargePointInfo2XML">A delegate to process the XML representation of an charge point info, e.g. before pushing it to the roaming provider.</param>
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
        public WWCPCPOAdapter(CSORoamingProvider_Id                                Id,
                              I18NString                                           Name,
                              I18NString                                           Description,
                              RoamingNetwork                                       RoamingNetwork,

                              CPOClient                                            CPOClient,
                              CPOServer                                            CPOServer,
                              String                                               ServerLoggingContext             = CPOServerLogger.DefaultContext,
                              LogfileCreatorDelegate                               LogFileCreator                   = null,

                              CustomEVSEIdMapperDelegate                           CustomEVSEIdMapper               = null,
                              EVSE2ChargePointInfoDelegate                         EVSE2ChargePointInfo             = null,
                              EVSEStatusUpdate2EVSEStatusDelegate                  EVSEStatusUpdate2EVSEStatus      = null,
                              ChargePointInfo2XMLDelegate                          ChargePointInfo2XML              = null,
                              EVSEStatus2XMLDelegate                               EVSEStatus2XML                   = null,

                              IncludeEVSEIdDelegate                                IncludeEVSEIds                   = null,
                              IncludeEVSEDelegate                                  IncludeEVSEs                     = null,
                              Func<ChargeDetailRecord, ChargeDetailRecordFilters>  ChargeDetailRecordFilter         = null,

                              TimeSpan?                                            ServiceCheckEvery                = null,
                              TimeSpan?                                            StatusCheckEvery                 = null,
                              TimeSpan?                                            EVSEStatusRefreshEvery           = null,
                              TimeSpan?                                            CDRCheckEvery                    = null,

                              Boolean                                              DisablePushData                  = false,
                              Boolean                                              DisablePushStatus                = false,
                              Boolean                                              DisableEVSEStatusRefresh         = false,
                              Boolean                                              DisableAuthentication            = false,
                              Boolean                                              DisableSendChargeDetailRecords   = false)

            : this(Id,
                   Name,
                   Description,
                   RoamingNetwork,

                   new CPORoaming(CPOClient,
                                  CPOServer,
                                  ServerLoggingContext,
                                  LogFileCreator),

                   CustomEVSEIdMapper,
                   EVSE2ChargePointInfo,
                   EVSEStatusUpdate2EVSEStatus,
                   ChargePointInfo2XML,
                   EVSEStatus2XML,

                   IncludeEVSEIds,
                   IncludeEVSEs,
                   ChargeDetailRecordFilter,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   EVSEStatusRefreshEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushStatus,
                   DisableEVSEStatusRefresh,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords)

        { }

        #endregion

        #region WWCPCPOAdapter(Id, Name, RoamingNetwork, RemoteHostName, ...)

        /// <summary>
        /// Create a new WWCP wrapper for the OCHP roaming client for Charging Station Operators/CPOs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="RemoteHostname">The hostname of the remote OCHP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote OCHP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OCHP service.</param>
        /// <param name="URIPrefix">An default URI prefix.</param>
        /// <param name="WSSLoginPassword">The WebService-Security username/password.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerURISuffix">An optional HTTP/SOAP/XML server URI suffix.</param>
        /// <param name="ServerContentType">An optional HTTP content type to use.</param>
        /// <param name="ServerRegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSE2ChargePointInfo">A delegate to process an charge point info, e.g. before pushing it to the roaming provider.</param>
        /// <param name="ChargePointInfo2XML">A delegate to process the XML representation of an charge point info, e.g. before pushing it to the roaming provider.</param>
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
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public WWCPCPOAdapter(CSORoamingProvider_Id                                Id,
                              I18NString                                           Name,
                              I18NString                                           Description,
                              RoamingNetwork                                       RoamingNetwork,

                              HTTPHostname                                         RemoteHostname,
                              IPPort?                                              RemoteTCPPort                       = null,
                              RemoteCertificateValidationCallback                  RemoteCertificateValidator          = null,
                              LocalCertificateSelectionCallback                    ClientCertificateSelector           = null,
                              HTTPHostname?                                        RemoteHTTPVirtualHost               = null,
                              HTTPPath?                                            URIPrefix                           = null,
                              HTTPPath?                                            LiveURIPrefix                       = null,
                              Tuple<String, String>                                WSSLoginPassword                    = null,
                              String                                               HTTPUserAgent                       = CPOClient.DefaultHTTPUserAgent,
                              TimeSpan?                                            RequestTimeout                      = null,
                              Byte?                                                MaxNumberOfRetries                  = CPOClient.DefaultMaxNumberOfRetries,

                              String                                               ServerName                          = CPOServer.DefaultHTTPServerName,
                              String                                               ServiceId                           = null,
                              IPPort?                                              ServerTCPPort                       = null,
                              HTTPPath?                                            ServerURIPrefix                     = null,
                              HTTPPath?                                            ServerURISuffix                     = null,
                              HTTPContentType                                      ServerContentType                   = null,
                              Boolean                                              ServerRegisterHTTPRootService       = true,
                              Boolean                                              ServerAutoStart                     = false,

                              String                                               ClientLoggingContext                = CPOClient.CPOClientLogger.DefaultContext,
                              String                                               ServerLoggingContext                = CPOServerLogger.DefaultContext,
                              LogfileCreatorDelegate                               LogFileCreator                      = null,

                              CustomEVSEIdMapperDelegate                           CustomEVSEIdMapper                  = null,
                              EVSE2ChargePointInfoDelegate                         EVSE2ChargePointInfo                = null,
                              EVSEStatusUpdate2EVSEStatusDelegate                  EVSEStatusUpdate2EVSEStatus         = null,
                              ChargePointInfo2XMLDelegate                          ChargePointInfo2XML                 = null,
                              EVSEStatus2XMLDelegate                               EVSEStatus2XML                      = null,

                              IncludeEVSEIdDelegate                                IncludeEVSEIds                      = null,
                              IncludeEVSEDelegate                                  IncludeEVSEs                        = null,
                              Func<ChargeDetailRecord, ChargeDetailRecordFilters>  ChargeDetailRecordFilter            = null,

                              TimeSpan?                                            ServiceCheckEvery                   = null,
                              TimeSpan?                                            StatusCheckEvery                    = null,
                              TimeSpan?                                            EVSEStatusRefreshEvery              = null,
                              TimeSpan?                                            CDRCheckEvery                       = null,

                              Boolean                                              DisablePushData                     = false,
                              Boolean                                              DisablePushStatus                   = false,
                              Boolean                                              DisableEVSEStatusRefresh            = false,
                              Boolean                                              DisableAuthentication               = false,
                              Boolean                                              DisableSendChargeDetailRecords      = false,

                              DNSClient                                            DNSClient                           = null)

            : this(Id,
                   Name,
                   Description,
                   RoamingNetwork,

                   new CPORoaming(Id.ToString(),
                                  RemoteHostname,
                                  RemoteTCPPort,
                                  RemoteCertificateValidator,
                                  ClientCertificateSelector,
                                  RemoteHTTPVirtualHost,
                                  URIPrefix     ?? CPOClient.DefaultURIPrefix,
                                  LiveURIPrefix ?? CPOClient.DefaultLiveURIPrefix,
                                  WSSLoginPassword,
                                  HTTPUserAgent,
                                  RequestTimeout,
                                  MaxNumberOfRetries,

                                  ServerName,
                                  ServiceId,
                                  ServerTCPPort,
                                  ServerURIPrefix ?? CPOServer.DefaultURIPrefix,
                                  ServerURISuffix ?? CPOServer.DefaultURISuffix,
                                  ServerContentType,
                                  ServerRegisterHTTPRootService,
                                  false,

                                  ClientLoggingContext,
                                  ServerLoggingContext,
                                  LogFileCreator,

                                  DNSClient),

                   CustomEVSEIdMapper,
                   EVSE2ChargePointInfo,
                   EVSEStatusUpdate2EVSEStatus,
                   ChargePointInfo2XML,
                   EVSEStatus2XML,

                   IncludeEVSEIds,
                   IncludeEVSEs,
                   ChargeDetailRecordFilter,

                   ServiceCheckEvery,
                   StatusCheckEvery,
                   EVSEStatusRefreshEvery,
                   CDRCheckEvery,

                   DisablePushData,
                   DisablePushStatus,
                   DisableEVSEStatusRefresh,
                   DisableAuthentication,
                   DisableSendChargeDetailRecords)

        {

            if (ServerAutoStart)
                CPOServer.Start();

        }

        #endregion

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

            SetChargePointInfos(IEnumerable<EVSE>   EVSEs,

                                DateTime?           Timestamp           = null,
                                CancellationToken?  CancellationToken   = null,
                                EventTracking_Id    EventTrackingId     = null,
                                TimeSpan?           RequestTimeout      = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSE status to upload

            var Warnings = new List<Warning>();

            var _ChargePointInfos = EVSEs == null || !EVSEs.Any()
                                        ? new ChargePointInfo[0]
                                        : EVSEs.Where (evse => evse != null && _IncludeEVSEs(evse)).
                                              Select(evse => {

                                                  try
                                                  {

                                                      return evse.ToOCHP(_CustomEVSEIdMapper,
                                                                         _EVSE2ChargePointInfo);

                                                  }
                                                  catch (Exception e)
                                                  {
                                                      DebugX.  Log(e.Message);
                                                      Warnings.Add(Warning.Create(e.Message, evse));
                                                  }

                                                  return null;

                                              }).
                                              Where(chargepointinfo => chargepointinfo != null).
                                              ToArray();


            HTTPResponse<SetChargePointListResponse>  response  = null;
            PushEVSEDataResult                        result    = null;

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
                                                          _ChargePointInfos.ULongCount(),
                                                          _ChargePointInfos,
                                                          Warnings.Where(warning => warning.IsNotNullOrEmpty()),
                                                          RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSetChargePointInfosWWCPRequest));
            }

            #endregion


            DateTime Endtime;
            TimeSpan Runtime;

            if (_ChargePointInfos.Length == 0)
            {

                Endtime   = DateTime.UtcNow;
                Runtime   = Endtime - StartTime;
             //   response  = 

            }

            else
            {

                response = await CPORoaming.
                                     SetChargePointList(_ChargePointInfos,
                                                        IncludeChargePoints,

                                                        Timestamp,
                                                        CancellationToken,
                                                        EventTrackingId,
                                                        RequestTimeout);

            }


                Endtime = DateTime.UtcNow;
                Runtime = Endtime - StartTime;

                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content        != null)
                {

                    if (response.Content.Result.ResultCode == ResultCodes.OK)
                        result = PushEVSEDataResult.Success(Id,
                                                            this,
                                                            response.Content.Result.Description,
                                                            null,
                                                            Runtime);

                    else
                        result = PushEVSEDataResult.Error(Id,
                                                          this,
                                                          EVSEs,
                                                          response.Content.Result.Description,
                                                          null,
                                                          Runtime);

                }
                else
                    result = PushEVSEDataResult.Error(Id,
                                                      this,
                                                      EVSEs,
                                                      response.HTTPStatusCode.ToString(),
                                                      response.HTTPBody != null
                                                          ? Warnings.AddAndReturnList(response.HTTPBody.ToUTF8String())
                                                          : Warnings.AddAndReturnList("No HTTP body received!"),
                                                      Runtime);


            #region Send OnSetChargePointInfosWWCPResponse event

            try
            {

                OnSetChargePointInfosWWCPResponse?.Invoke(Endtime,
                                                          Timestamp.Value,
                                                          this,
                                                          Id,
                                                          EventTrackingId,
                                                          RoamingNetwork.Id,
                                                          _ChargePointInfos.ULongCount(),
                                                          _ChargePointInfos,
                                                          RequestTimeout,
                                                          result,
                                                          Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSetChargePointInfosWWCPResponse));
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

            UpdateChargePointInfos(IEnumerable<EVSE>   EVSEs,

                                   DateTime?           Timestamp           = null,
                                   CancellationToken?  CancellationToken   = null,
                                   EventTracking_Id    EventTrackingId     = null,
                                   TimeSpan?           RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSE status to upload

            var Warnings = new List<Warning>();

            var _ChargePointInfos = EVSEs.Where (evse => evse != null && _IncludeEVSEs(evse)).
                                          Select(evse => {

                                              try
                                              {

                                                  return evse.ToOCHP(_CustomEVSEIdMapper,
                                                                     _EVSE2ChargePointInfo);

                                              }
                                              catch (Exception e)
                                              {
                                                  DebugX.  Log(e.Message);
                                                  Warnings.Add(Warning.Create(e.Message, evse));
                                              }

                                              return null;

                                          }).
                                          Where(chargepointinfo => chargepointinfo != null).
                                          ToArray();

            PushEVSEDataResult result;

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
                                                            _ChargePointInfos.ULongCount(),
                                                            _ChargePointInfos,
                                                            Warnings.Where(warning => warning.IsNotNullOrEmpty()),
                                                            RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSetChargePointInfosWWCPRequest));
            }

            #endregion


            var response = await CPORoaming.
                                     UpdateChargePointList(_ChargePointInfos,
                                                           IncludeChargePoints,

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
                    result = PushEVSEDataResult.Success(Id,
                                                    this,
                                                    response.Content.Result.Description,
                                                    null,
                                                    Runtime);

                else
                    result = PushEVSEDataResult.Error(Id,
                                                  this,
                                                  EVSEs,
                                                  response.Content.Result.Description,
                                                  null,
                                                  Runtime);

            }
            else
                result = PushEVSEDataResult.Error(Id,
                                              this,
                                              EVSEs,
                                              response.HTTPStatusCode.ToString(),
                                              response.HTTPBody != null
                                                  ? Warnings.AddAndReturnList(response.HTTPBody.ToUTF8String())
                                                  : Warnings.AddAndReturnList("No HTTP body received!"),
                                              Runtime);


            #region Send OnUpdateChargePointInfosWWCPResponse event

            try
            {

                OnUpdateChargePointInfosWWCPResponse?.Invoke(Endtime,
                                                             Timestamp.Value,
                                                             this,
                                                             Id,
                                                             EventTrackingId,
                                                             RoamingNetwork.Id,
                                                             _ChargePointInfos.ULongCount(),
                                                             _ChargePointInfos,
                                                             RequestTimeout,
                                                             result,
                                                             Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnUpdateChargePointInfosWWCPResponse));
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
                             CancellationToken?             CancellationToken   = null,
                             EventTracking_Id               EventTrackingId     = null,
                             TimeSpan?                      RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEStatusUpdates == null)
                throw new ArgumentNullException(nameof(EVSEStatusUpdates), "The given enumeration of EVSE status updates must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSEStatus/EVSEStatusRecords to upload

            var Warnings = new List<Warning>();

            var _EVSEStatus = EVSEStatusUpdates.
                                  Where       (evsestatusupdate => IncludeEVSEs  (evsestatusupdate.EVSE) &&
                                                                   IncludeEVSEIds(evsestatusupdate.EVSE.Id)).
                                  ToLookup    (evsestatusupdate => evsestatusupdate.EVSE.Id,
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
                                          Warnings.Add(Warning.Create(e.Message, evsestatusupdate));
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
                                                      Warnings.Where(warning => warning.IsNotNullOrEmpty()),
                                                      RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnUpdateEVSEStatusWWCPRequest));
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
                                                    ? Warnings.AddAndReturnList(response.HTTPBody.ToUTF8String())
                                                    : Warnings.AddAndReturnList("No HTTP body received!"),
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
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnUpdateEVSEStatusWWCPResponse));
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

            ISendData.SetStaticData(EVSE                EVSE,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

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
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                lock (ServiceCheckLock)
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEsToAddQueue.Add(EVSE);

                        ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                    }

                }

                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

            }

            #endregion

            return SetChargePointInfos(new EVSE[] { EVSE },

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

            ISendData.AddStaticData(EVSE                EVSE,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

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
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                lock (ServiceCheckLock)
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEsToAddQueue.Add(EVSE);

                        ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                    }

                }

                Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

            }

            #endregion

            return UpdateChargePointInfos(new EVSE[] { EVSE },

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

            ISendData.UpdateStaticData(EVSE                EVSE,
                                       String              PropertyName,
                                       Object              OldValue,
                                       Object              NewValue,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSE == null)
                throw new ArgumentNullException(nameof(EVSE), "The given EVSE must not be null!");

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
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                lock (ServiceCheckLock)
                {

                    if (_IncludeEVSEs == null ||
                       (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    {

                        EVSEsToUpdateQueue.Add(EVSE);

                        ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                    }

                }

                return Task.FromResult(PushEVSEDataResult.Enqueued(Id, this));

            }

            #endregion

            return UpdateChargePointInfos(new EVSE[] { EVSE },

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

            ISendData.DeleteStaticData(EVSE                EVSE,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

                => Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

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

            ISendData.SetStaticData(IEnumerable<EVSE>   EVSEs,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

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

            ISendData.AddStaticData(IEnumerable<EVSE>   EVSEs,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

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

            ISendData.UpdateStaticData(IEnumerable<EVSE>   EVSEs,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");

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

            ISendData.DeleteStaticData(IEnumerable<EVSE>   EVSEs,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

                => Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

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
                                               CancellationToken?                  CancellationToken,
                                               EventTracking_Id                    EventTrackingId,
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
                         CancellationToken?             CancellationToken,
                         EventTracking_Id               EventTrackingId,
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
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion


                var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

                try
                {

                    if (LockTaken)
                    {

                        var FilteredUpdates = StatusUpdates.Where(statusupdate => _IncludeEVSEs  (statusupdate.EVSE) &&
                                                                                  _IncludeEVSEIds(statusupdate.EVSE.Id)).
                                                            ToArray();

                        if (FilteredUpdates.Length > 0)
                        {

                            foreach (var Update in FilteredUpdates)
                            {

                                // Delay the status update until the EVSE data had been uploaded!
                                if (EVSEsToAddQueue.Any(evse => evse == Update.EVSE))
                                    EVSEStatusChangesDelayedQueue.Add(Update);

                                else
                                    EVSEStatusChangesFastQueue.Add(Update);

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
        async Task<PushEVSEDataResult>

            ISendData.SetStaticData(ChargingStation     ChargingStation,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

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
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                lock (ServiceCheckLock)
                {

                    foreach (var evse in ChargingStation)
                    {

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                        }

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await SetChargePointInfos(ChargingStation.EVSEs,

                                             Timestamp,
                                             CancellationToken,
                                             EventTrackingId,
                                             RequestTimeout).

                                             ConfigureAwait(false);

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
        async Task<PushEVSEDataResult>

            ISendData.AddStaticData(ChargingStation     ChargingStation,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

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
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                lock (ServiceCheckLock)
                {

                    foreach (var evse in ChargingStation)
                    {

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                        }

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await UpdateChargePointInfos(ChargingStation.EVSEs,

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

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
        async Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(ChargingStation     ChargingStation,
                                       String              PropertyName,
                                       Object              OldValue,
                                       Object              NewValue,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingStation == null)
                throw new ArgumentNullException(nameof(ChargingStation), "The given charging station must not be null!");

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
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                lock (ServiceCheckLock)
                {

                    foreach (var evse in ChargingStation)
                    {

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToUpdateQueue.Add(evse);

                            ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                        }

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await SetChargePointInfos(ChargingStation,

                                             Timestamp,
                                             CancellationToken,
                                             EventTrackingId,
                                             RequestTimeout).

                                             ConfigureAwait(false);

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
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(ChargingStation     ChargingStation,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

                => Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

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
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                    TransmissionTypes             TransmissionType,

                                    DateTime?                     Timestamp,
                                    CancellationToken?            CancellationToken,
                                    EventTracking_Id              EventTrackingId,
                                    TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return SetChargePointInfos(ChargingStations.SafeSelectMany(station => station.EVSEs),

                                       Timestamp,
                                       CancellationToken,
                                       EventTrackingId,
                                       RequestTimeout);

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
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                    TransmissionTypes             TransmissionType,


                                    DateTime?                     Timestamp,
                                    CancellationToken?            CancellationToken,
                                    EventTracking_Id              EventTrackingId,
                                    TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return UpdateChargePointInfos(ChargingStations.SafeSelectMany(station => station.EVSEs),

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);

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
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                       TransmissionTypes             TransmissionType,

                                       DateTime?                     Timestamp,
                                       CancellationToken?            CancellationToken,
                                       EventTracking_Id              EventTrackingId,
                                       TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return UpdateChargePointInfos(ChargingStations.SafeSelectMany(station => station.EVSEs),

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);

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
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(IEnumerable<ChargingStation>  ChargingStations,
                                       TransmissionTypes             TransmissionType,

                                       DateTime?                     Timestamp,
                                       CancellationToken?            CancellationToken,
                                       EventTracking_Id              EventTrackingId,
                                       TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

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
                                               CancellationToken?                             CancellationToken,
                                               EventTracking_Id                               EventTrackingId,
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
                                     CancellationToken?                        CancellationToken,
                                     EventTracking_Id                          EventTrackingId,
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
        async Task<PushEVSEDataResult>

            ISendData.SetStaticData(ChargingPool        ChargingPool,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

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
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                lock (ServiceCheckLock)
                {

                    foreach (var evse in ChargingPool.EVSEs)
                    {

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                        }

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await SetChargePointInfos(ChargingPool.EVSEs,

                                             Timestamp,
                                             CancellationToken,
                                             EventTrackingId,
                                             RequestTimeout).

                                             ConfigureAwait(false);

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
        async Task<PushEVSEDataResult>

            ISendData.AddStaticData(ChargingPool        ChargingPool,
                                    TransmissionTypes   TransmissionType,

                                    DateTime?           Timestamp,
                                    CancellationToken?  CancellationToken,
                                    EventTracking_Id    EventTrackingId,
                                    TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

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
                //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                //}

                #endregion

                lock (ServiceCheckLock)
                {

                    foreach (var evse in ChargingPool.EVSEs)
                    {

                        if (_IncludeEVSEs == null ||
                           (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                        {

                            EVSEsToAddQueue.Add(evse);

                            ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                        }

                    }

                }

                return PushEVSEDataResult.Enqueued(Id, this);

            }

            #endregion

            return await UpdateChargePointInfos(ChargingPool.EVSEs,

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

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
        async Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(ChargingPool        ChargingPool,
                                             String              PropertyName,
                                             Object              OldValue,
                                             Object              NewValue,
                                             TransmissionTypes   TransmissionType,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            {

                #region Initial checks

                if (ChargingPool == null)
                    throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

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
                    //    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRRequest));
                    //}

                    #endregion

                    lock (ServiceCheckLock)
                    {

                        foreach (var evse in ChargingPool.EVSEs)
                        {

                            if (_IncludeEVSEs == null ||
                               (_IncludeEVSEs != null && _IncludeEVSEs(evse)))
                            {

                                EVSEsToUpdateQueue.Add(evse);

                                ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                            }

                        }

                    }

                    return PushEVSEDataResult.Enqueued(Id, this);

                }

                #endregion

                return await SetChargePointInfos(ChargingPool.EVSEs,

                                                 Timestamp,
                                                 CancellationToken,
                                                 EventTrackingId,
                                                 RequestTimeout).

                                                 ConfigureAwait(false);

            }

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
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(ChargingPool        ChargingPool,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

        {

            #region Initial checks

            if (ChargingPool == null)
                throw new ArgumentNullException(nameof(ChargingPool), "The given charging pool must not be null!");

            #endregion

            return UpdateChargePointInfos(ChargingPool.EVSEs,   // Mark as deleted?

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);

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
        Task<PushEVSEDataResult>

            ISendData.SetStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                    TransmissionTypes          TransmissionType,

                                    DateTime?                  Timestamp,
                                    CancellationToken?         CancellationToken,
                                    EventTracking_Id           EventTrackingId,
                                    TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return SetChargePointInfos(ChargingPools.SafeSelectMany(pool => pool.EVSEs),

                                       Timestamp,
                                       CancellationToken,
                                       EventTrackingId,
                                       RequestTimeout);

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
        Task<PushEVSEDataResult>

            ISendData.AddStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                    TransmissionTypes          TransmissionType,

                                    DateTime?                  Timestamp,
                                    CancellationToken?         CancellationToken,
                                    EventTracking_Id           EventTrackingId,
                                    TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return UpdateChargePointInfos(ChargingPools.SafeSelectMany(pool => pool.EVSEs),

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);

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
        Task<PushEVSEDataResult>

            ISendData.UpdateStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                       TransmissionTypes          TransmissionType,

                                       DateTime?                  Timestamp,
                                       CancellationToken?         CancellationToken,
                                       EventTracking_Id           EventTrackingId,
                                       TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return UpdateChargePointInfos(ChargingPools.SafeSelectMany(pool => pool.EVSEs),

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);

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
        Task<PushEVSEDataResult>

            ISendData.DeleteStaticData(IEnumerable<ChargingPool>  ChargingPools,
                                       TransmissionTypes          TransmissionType,

                                       DateTime?                  Timestamp,
                                       CancellationToken?         CancellationToken,
                                       EventTracking_Id           EventTrackingId,
                                       TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return UpdateChargePointInfos(ChargingPools.SafeSelectMany(pool => pool.EVSEs),

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout);

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
                                               CancellationToken?                          CancellationToken,
                                               EventTracking_Id                            EventTrackingId,
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
                                     CancellationToken?                     CancellationToken,
                                     EventTracking_Id                       EventTrackingId,
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

            ISendData.SetStaticData(ChargingStationOperator  ChargingStationOperator,

                                          DateTime?                Timestamp,
                                          CancellationToken?       CancellationToken,
                                          EventTracking_Id         EventTrackingId,
                                          TimeSpan?                RequestTimeout)

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

            ISendData.AddStaticData(ChargingStationOperator  ChargingStationOperator,

                                          DateTime?                Timestamp,
                                          CancellationToken?       CancellationToken,
                                          EventTracking_Id         EventTrackingId,
                                          TimeSpan?                RequestTimeout)

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

            ISendData.UpdateStaticData(ChargingStationOperator  ChargingStationOperator,

                                             DateTime?                Timestamp,
                                             CancellationToken?       CancellationToken,
                                             EventTracking_Id         EventTrackingId,
                                             TimeSpan?                RequestTimeout)

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

            ISendData.DeleteStaticData(ChargingStationOperator  ChargingStationOperator,

                                             DateTime?                Timestamp,
                                             CancellationToken?       CancellationToken,
                                             EventTracking_Id         EventTrackingId,
                                             TimeSpan?                RequestTimeout)

                => Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

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

            ISendData.SetStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                          DateTime?                             Timestamp,
                                          CancellationToken?                    CancellationToken,
                                          EventTracking_Id                      EventTrackingId,
                                          TimeSpan?                             RequestTimeout)

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

            ISendData.AddStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                          DateTime?                             Timestamp,
                                          CancellationToken?                    CancellationToken,
                                          EventTracking_Id                      EventTrackingId,
                                          TimeSpan?                             RequestTimeout)

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

            ISendData.UpdateStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                             DateTime?                             Timestamp,
                                             CancellationToken?                    CancellationToken,
                                             EventTracking_Id                      EventTrackingId,
                                             TimeSpan?                             RequestTimeout)

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

            ISendData.DeleteStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                             DateTime?                             Timestamp,
                                             CancellationToken?                    CancellationToken,
                                             EventTracking_Id                      EventTrackingId,
                                             TimeSpan?                             RequestTimeout)

                => Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

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
                                               CancellationToken?                                     CancellationToken,
                                               EventTracking_Id                                       EventTrackingId,
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
                                     CancellationToken?                                CancellationToken,
                                     EventTracking_Id                                  EventTrackingId,
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

            ISendData.SetStaticData(RoamingNetwork      RoamingNetwork,

                                          DateTime?           Timestamp,
                                          CancellationToken?  CancellationToken,
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

            ISendData.AddStaticData(RoamingNetwork      RoamingNetwork,

                                          DateTime?           Timestamp,
                                          CancellationToken?  CancellationToken,
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

            ISendData.UpdateStaticData(RoamingNetwork      RoamingNetwork,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
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

            ISendData.DeleteStaticData(RoamingNetwork      RoamingNetwork,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

                => Task.FromResult(PushEVSEDataResult.NoOperation(Id, this));

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
                                               CancellationToken?                            CancellationToken,
                                               EventTracking_Id                              EventTrackingId,
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
                                     CancellationToken?                       CancellationToken,
                                     EventTracking_Id                         EventTrackingId,
                                     TimeSpan?                                RequestTimeout)


                => Task.FromResult(PushRoamingNetworkStatusResult.NoOperation(Id, this));

        #endregion

        #endregion

        #endregion

        #region AuthorizeStart/-Stop  directly...

        #region AuthorizeStart(LocalAuthentication,                    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           ChargingProduct              ChargingProduct     = null,
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (LocalAuthentication == null)
                throw new ArgumentNullException(nameof(LocalAuthentication),   "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

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
                                                OperatorId,
                                                LocalAuthentication,
                                                ChargingProduct,
                                                SessionId,
                                                RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStartRequest));
            }

            #endregion


            DateTime         Endtime;
            TimeSpan         Runtime;
            AuthStartResult  result;

            if (DisableAuthentication)
            {
                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStartResult.OutOfService(Id,
                                                        this,
                                                        SessionId,
                                                        Runtime);
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

                if (response.HTTPStatusCode            == HTTPStatusCode.OK &&
                    response.Content                   != null              &&
                    response.Content.Result.ResultCode == ResultCodes.OK)
                {

                    result = AuthStartResult.Authorized(Id,
                                                        this,
                                                        ChargingSession_Id.New,
                                                        ProviderId: response.Content.RoamingAuthorisationInfo.ContractId.ProviderId.ToWWCP(),
                                                        Runtime:    Runtime);

                }

                else
                    result = AuthStartResult.NotAuthorized(Id,
                                                           this,
                                                           // response.Content.ProviderId.ToWWCP(),
                                                           Runtime: Runtime);

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
                                                 OperatorId,
                                                 LocalAuthentication,
                                                 ChargingProduct,
                                                 SessionId,
                                                 RequestTimeout,
                                                 result,
                                                 Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(LocalAuthentication, EVSEId,            ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartEVSEResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           WWCP.EVSE_Id                 EVSEId,
                           ChargingProduct              ChargingProduct     = null,   // [maxlength: 100]
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (LocalAuthentication == null)
                throw new ArgumentNullException(nameof(LocalAuthentication),  "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeEVSEStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeEVSEStartRequest?.Invoke(StartTime,
                                                    Timestamp.Value,
                                                    this,
                                                    Id.ToString(),
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    OperatorId,
                                                    LocalAuthentication,
                                                    EVSEId,
                                                    ChargingProduct,
                                                    SessionId,
                                                    new ISendAuthorizeStartStop[0],
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeEVSEStartRequest));
            }

            #endregion


            DateTime             Endtime;
            TimeSpan             Runtime;
            AuthStartEVSEResult  result;

            if (DisableAuthentication)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStartEVSEResult.OutOfService(Id,
                                                            this,
                                                            SessionId,
                                                            Runtime);

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

                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content != null &&
                    response.Content.Result.ResultCode == ResultCodes.OK)
                {

                    result = AuthStartEVSEResult.Authorized(Id,
                                                            this,
                                                            ChargingSession_Id.New,
                                                            ProviderId:     response.Content.RoamingAuthorisationInfo != null
                                                                                ? response.Content.RoamingAuthorisationInfo.ContractId.ProviderId.ToWWCP()
                                                                                : eMobilityProvider_Id.Parse(Country.Germany, "GEF"),
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

                    result = AuthStartEVSEResult.NotAuthorized(Id,
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


            #region Send OnAuthorizeEVSEStartResponse event

            try
            {

                OnAuthorizeEVSEStartResponse?.Invoke(Endtime,
                                                     Timestamp.Value,
                                                     this,
                                                     Id.ToString(),
                                                     EventTrackingId,
                                                     RoamingNetwork.Id,
                                                     OperatorId,
                                                     LocalAuthentication,
                                                     EVSEId,
                                                     ChargingProduct,
                                                     SessionId,
                                                     new ISendAuthorizeStartStop[0],
                                                     RequestTimeout,
                                                     result,
                                                     Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeEVSEStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(LocalAuthentication, ChargingStationId, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging station.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingStationId">The unique identification charging station.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartChargingStationResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           ChargingStation_Id           ChargingStationId,
                           ChargingProduct              ChargingProduct     = null,   // [maxlength: 100]
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (LocalAuthentication == null)
                throw new ArgumentNullException(nameof(LocalAuthentication), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingStationStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingStationStartRequest?.Invoke(StartTime,
                                                               Timestamp.Value,
                                                               this,
                                                               Id.ToString(),
                                                               EventTrackingId,
                                                               RoamingNetwork.Id,
                                                               OperatorId,
                                                               LocalAuthentication,
                                                               ChargingStationId,
                                                               ChargingProduct,
                                                               SessionId,
                                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingStationStartRequest));
            }

            #endregion


            var response = await CPORoaming.GetSingleRoamingAuthorisation(new EMT_Id(
                                                                              LocalAuthentication.AuthToken.ToString(),
                                                                              TokenRepresentations.Plain,
                                                                              TokenTypes.RFID
                                                                          ),

                                                                          Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          RequestTimeout).ConfigureAwait(false);


            var Endtime  = DateTime.UtcNow;
            var Runtime  = Endtime - StartTime;

            AuthStartChargingStationResult result = null;

            if (response.HTTPStatusCode            == HTTPStatusCode.OK &&
                response.Content                   != null              &&
                response.Content.Result.ResultCode == ResultCodes.OK)
            {

                result = AuthStartChargingStationResult.Authorized(Id,
                                                                   this,
                                                                   ChargingSession_Id.New,
                                                                   ProviderId:  response.Content.RoamingAuthorisationInfo.ContractId.ProviderId.ToWWCP(),
                                                                   Runtime:     Runtime);

            }

            else
                result = AuthStartChargingStationResult.NotAuthorized(Id,
                                                                      this,
                                                                      // response.Content.ProviderId.ToWWCP(),
                                                                      // response.Content.StatusCode.Description,
                                                                      // response.Content.StatusCode.AdditionalInfo,
                                                                      Runtime: Runtime);


            #region Send OnAuthorizeChargingStationStartResponse event

            try
            {

                OnAuthorizeChargingStationStartResponse?.Invoke(Endtime,
                                                                Timestamp.Value,
                                                                this,
                                                                Id.ToString(),
                                                                EventTrackingId,
                                                                RoamingNetwork.Id,
                                                                OperatorId,
                                                                LocalAuthentication,
                                                                ChargingStationId,
                                                                ChargingProduct,
                                                                SessionId,
                                                                RequestTimeout,
                                                                result,
                                                                Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingStationStartResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStart(LocalAuthentication, ChargingPoolId,    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging pool.
        /// </summary>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingPoolId">The unique identification charging pool.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartChargingPoolResult>

            AuthorizeStart(LocalAuthentication          LocalAuthentication,
                           ChargingPool_Id              ChargingPoolId,
                           ChargingProduct              ChargingProduct     = null,   // [maxlength: 100]
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (LocalAuthentication == null)
                throw new ArgumentNullException(nameof(LocalAuthentication), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingPoolStartRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingPoolStartRequest?.Invoke(StartTime,
                                                            Timestamp.Value,
                                                            this,
                                                            Id.ToString(),
                                                            EventTrackingId,
                                                            RoamingNetwork.Id,
                                                            OperatorId,
                                                            LocalAuthentication,
                                                            ChargingPoolId,
                                                            ChargingProduct,
                                                            SessionId,
                                                            RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingPoolStartRequest));
            }

            #endregion


            var response = await CPORoaming.GetSingleRoamingAuthorisation(new EMT_Id(
                                                                              LocalAuthentication.AuthToken.ToString(),
                                                                              TokenRepresentations.Plain,
                                                                              TokenTypes.RFID
                                                                          ),

                                                                          Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          RequestTimeout).ConfigureAwait(false);


            var Endtime  = DateTime.UtcNow;
            var Runtime  = Endtime - StartTime;

            AuthStartChargingPoolResult result = null;

            if (response.HTTPStatusCode            == HTTPStatusCode.OK &&
                response.Content                   != null              &&
                response.Content.Result.ResultCode == ResultCodes.OK)
            {

                result = AuthStartChargingPoolResult.Authorized(Id,
                                                                this,
                                                                ChargingSession_Id.New,
                                                                ProviderId:  response.Content.RoamingAuthorisationInfo.ContractId.ProviderId.ToWWCP(),
                                                                Runtime:     Runtime);

            }

            else
                result = AuthStartChargingPoolResult.NotAuthorized(Id,
                                                                   this,
                                                                   // response.Content.ProviderId.ToWWCP(),
                                                                   // response.Content.StatusCode.Description,
                                                                   // response.Content.StatusCode.AdditionalInfo,
                                                                   Runtime: Runtime);


            #region Send OnAuthorizeChargingPoolStartResponse event

            try
            {

                OnAuthorizeChargingPoolStartResponse?.Invoke(Endtime,
                                                             Timestamp.Value,
                                                             this,
                                                             Id.ToString(),
                                                             EventTrackingId,
                                                             RoamingNetwork.Id,
                                                             OperatorId,
                                                             LocalAuthentication,
                                                             ChargingPoolId,
                                                             ChargingProduct,
                                                             SessionId,
                                                             RequestTimeout,
                                                             result,
                                                             Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingPoolStartResponse));
            }

            #endregion

            return result;

        }

        #endregion


        // UID => Not everybody can stop any session, but maybe another
        //        UID than the UID which started the session!
        //        (e.g. car sharing)

        #region AuthorizeStop(SessionId, LocalAuthentication,                    OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (LocalAuthentication == null)
                throw new ArgumentNullException(nameof(LocalAuthentication),  "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

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
                                               OperatorId,
                                               SessionId,
                                               LocalAuthentication,
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStopRequest));
            }

            #endregion


            DateTime        Endtime;
            TimeSpan        Runtime;
            AuthStopResult  result;

            if (DisableAuthentication)
            {
                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStopResult.OutOfService(Id,
                                                       this,
                                                       SessionId,
                                                       Runtime);
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

                if (response.HTTPStatusCode            == HTTPStatusCode.OK &&
                    response.Content                   != null              &&
                    response.Content.Result.ResultCode == ResultCodes.OK)
                {

                    result = AuthStopResult.Authorized(Id,
                                                       this,
                                                       ChargingSession_Id.New,
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
                                                OperatorId,
                                                SessionId,
                                                LocalAuthentication,
                                                RequestTimeout,
                                                result,
                                                Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, LocalAuthentication, EVSEId,            OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given EVSE.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopEVSEResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          WWCP.EVSE_Id                 EVSEId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (LocalAuthentication  == null)
                throw new ArgumentNullException(nameof(LocalAuthentication), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeEVSEStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeEVSEStopRequest?.Invoke(StartTime,
                                                   Timestamp.Value,
                                                   this,
                                                   Id.ToString(),
                                                   EventTrackingId,
                                                   RoamingNetwork.Id,
                                                   OperatorId,
                                                   EVSEId,
                                                   SessionId,
                                                   LocalAuthentication,
                                                   RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeEVSEStopRequest));
            }

            #endregion


            DateTime            Endtime;
            TimeSpan            Runtime;
            AuthStopEVSEResult  result;

            if (DisableAuthentication)
            {
                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = AuthStopEVSEResult.OutOfService(Id,
                                                           this,
                                                           SessionId,
                                                           Runtime);
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

                if (response.HTTPStatusCode            == HTTPStatusCode.OK &&
                    response.Content                   != null              &&
                    response.Content.Result.ResultCode == ResultCodes.OK)
                {

                    result = AuthStopEVSEResult.Authorized(Id,
                                                           this,
                                                           ChargingSession_Id.New,
                                                           ProviderId:  response.Content.RoamingAuthorisationInfo.ContractId.ProviderId.ToWWCP(),
                                                           Runtime:     Runtime);

                }

                else
                    result = AuthStopEVSEResult.NotAuthorized(Id,
                                                              this,
                                                              // response.Content.ProviderId.ToWWCP(),
                                                              Runtime: Runtime);

            }


            #region Send OnAuthorizeEVSEStopResponse event

            try
            {

                OnAuthorizeEVSEStopResponse?.Invoke(Endtime,
                                                    Timestamp.Value,
                                                    this,
                                                    Id.ToString(),
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    OperatorId,
                                                    EVSEId,
                                                    SessionId,
                                                    LocalAuthentication,
                                                    RequestTimeout,
                                                    result,
                                                    Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeEVSEStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, LocalAuthentication, ChargingStationId, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging station.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingStationResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          ChargingStation_Id           ChargingStationId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (LocalAuthentication == null)
                throw new ArgumentNullException(nameof(LocalAuthentication), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingStationStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingStationStopRequest?.Invoke(StartTime,
                                                              Timestamp.Value,
                                                              this,
                                                              Id.ToString(),
                                                              EventTrackingId,
                                                              RoamingNetwork.Id,
                                                              OperatorId,
                                                              ChargingStationId,
                                                              SessionId,
                                                              LocalAuthentication,
                                                              RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingStationStopRequest));
            }

            #endregion


            var response = await CPORoaming.GetSingleRoamingAuthorisation(new EMT_Id(
                                                                              LocalAuthentication.AuthToken.ToString(),
                                                                              TokenRepresentations.Plain,
                                                                              TokenTypes.RFID
                                                                          ),

                                                                          Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          RequestTimeout).ConfigureAwait(false);


            var Endtime  = DateTime.UtcNow;
            var Runtime  = Endtime - StartTime;

            AuthStopChargingStationResult result = null;

            if (response.HTTPStatusCode            == HTTPStatusCode.OK &&
                response.Content                   != null              &&
                response.Content.Result.ResultCode == ResultCodes.OK)
            {

                result = AuthStopChargingStationResult.Authorized(Id,
                                                                  this,
                                                                  ChargingSession_Id.New,
                                                                  ProviderId:  response.Content.RoamingAuthorisationInfo.ContractId.ProviderId.ToWWCP(),
                                                                  Runtime:     Runtime);

            }

            else
                result = AuthStopChargingStationResult.NotAuthorized(Id,
                                                                     this,
                                                                     // response.Content.ProviderId.ToWWCP(),
                                                                     // response.Content.StatusCode.Description,
                                                                     // response.Content.StatusCode.AdditionalInfo,
                                                                     Runtime: Runtime);


            #region Send OnAuthorizeChargingStationStopResponse event

            try
            {

                OnAuthorizeChargingStationStopResponse?.Invoke(Endtime,
                                                               Timestamp.Value,
                                                               this,
                                                               Id.ToString(),
                                                               EventTrackingId,
                                                               RoamingNetwork.Id,
                                                               OperatorId,
                                                               ChargingStationId,
                                                               SessionId,
                                                               LocalAuthentication,
                                                               RequestTimeout,
                                                               result,
                                                               Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingStationStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region AuthorizeStop(SessionId, LocalAuthentication, ChargingPoolId,    OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging pool.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="LocalAuthentication">An user identification.</param>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingPoolResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          LocalAuthentication          LocalAuthentication,
                          ChargingPool_Id              ChargingPoolId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (LocalAuthentication == null)
                throw new ArgumentNullException(nameof(LocalAuthentication), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingPoolStopRequest event

            var StartTime = DateTime.UtcNow;

            try
            {

                OnAuthorizeChargingPoolStopRequest?.Invoke(StartTime,
                                                           Timestamp.Value,
                                                           this,
                                                           Id.ToString(),
                                                           EventTrackingId,
                                                           RoamingNetwork.Id,
                                                           OperatorId,
                                                           ChargingPoolId,
                                                           SessionId,
                                                           LocalAuthentication,
                                                           RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingPoolStopRequest));
            }

            #endregion


            var response = await CPORoaming.GetSingleRoamingAuthorisation(new EMT_Id(
                                                                              LocalAuthentication.AuthToken.ToString(),
                                                                              TokenRepresentations.Plain,
                                                                              TokenTypes.RFID
                                                                          ),

                                                                          Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          RequestTimeout).ConfigureAwait(false);


            var Endtime  = DateTime.UtcNow;
            var Runtime  = Endtime - StartTime;

            AuthStopChargingPoolResult result = null;

            if (response.HTTPStatusCode            == HTTPStatusCode.OK &&
                response.Content                   != null              &&
                response.Content.Result.ResultCode == ResultCodes.OK)
            {

                result = AuthStopChargingPoolResult.Authorized(Id,
                                                               this,
                                                               ChargingSession_Id.New,
                                                               ProviderId:  response.Content.RoamingAuthorisationInfo.ContractId.ProviderId.ToWWCP(),
                                                               Runtime:     Runtime);

            }

            else
                result = AuthStopChargingPoolResult.NotAuthorized(Id,
                                                                  this,
                                                                  // response.Content.ProviderId.ToWWCP(),
                                                                  // response.Content.StatusCode.Description,
                                                                  // response.Content.StatusCode.AdditionalInfo,
                                                                  Runtime: Runtime);


            #region Send OnAuthorizeChargingPoolStopResponse event

            try
            {

                OnAuthorizeChargingPoolStopResponse?.Invoke(Endtime,
                                                            Timestamp.Value,
                                                            this,
                                                            Id.ToString(),
                                                            EventTrackingId,
                                                            RoamingNetwork.Id,
                                                            OperatorId,
                                                            ChargingPoolId,
                                                            SessionId,
                                                            LocalAuthentication,
                                                            RequestTimeout,
                                                            result,
                                                            Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingPoolStopResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #endregion

        #region SendChargeDetailRecords(ChargeDetailRecords, TransmissionType = Enqueue, ...)

        /// <summary>
        /// Send a charge detail record to an OCHP server.
        /// </summary>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        /// <param name="TransmissionType">Whether to send the CDR directly or enqueue it for a while.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<SendCDRsResult>

            SendChargeDetailRecords(IEnumerable<ChargeDetailRecord>  ChargeDetailRecords,
                                    TransmissionTypes                TransmissionType    = TransmissionTypes.Enqueue,

                                    DateTime?                        Timestamp           = null,
                                    CancellationToken?               CancellationToken   = null,
                                    EventTracking_Id                 EventTrackingId     = null,
                                    TimeSpan?                        RequestTimeout      = null)

        {

            #region Initial checks

            if (ChargeDetailRecords == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecords),  "The given enumeration of charge detail records must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.UtcNow;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            var ForwardedChargeDetailRecords = new List<ChargeDetailRecord>();
            var DroppedChargeDetailRecords   = new List<ChargeDetailRecord>();

            foreach (var cdr in ChargeDetailRecords)
            {

                if (ChargeDetailRecordFilter(cdr) == ChargeDetailRecordFilters.forward)
                    ForwardedChargeDetailRecords.Add(cdr);

                else
                    DroppedChargeDetailRecords.Add(cdr);

            }



            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueue)
            {

                #region Send OnEnqueueSendCDRsRequest event

                try
                {

                    OnEnqueueSendCDRsRequest?.Invoke(DateTime.UtcNow,
                                                     Timestamp.Value,
                                                     this,
                                                     Id.ToString(),
                                                     EventTrackingId,
                                                     RoamingNetwork.Id,
                                                     DroppedChargeDetailRecords,
                                                     ForwardedChargeDetailRecords,
                                                     RequestTimeout);

                }
                catch (Exception e)
                {
                    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsRequest));
                }

                #endregion

                lock (ServiceCheckLock)
                {

                    ChargeDetailRecordsQueue.AddRange(ForwardedChargeDetailRecords.Select(cdr => cdr.ToOCHP(ContractIdDelegate: emtid => _Lookup[emtid],
                                                                                                            CustomEVSEIdMapper: null)));

                    ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                }

                return SendCDRsResult.Enqueued(Id, this, ForwardedChargeDetailRecords);

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
                                          DroppedChargeDetailRecords,
                                          ForwardedChargeDetailRecords,
                                          RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsRequest));
            }

            #endregion


            DateTime        Endtime;
            TimeSpan        Runtime;
            SendCDRsResult  result;

            if (DisableSendChargeDetailRecords)
            {

                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;
                result   = SendCDRsResult.OutOfService(Id,
                                                       this,
                                                       ForwardedChargeDetailRecords,
                                                       Runtime: Runtime);

            }

            else
            {

                var response = await CPORoaming.AddCDRs(ForwardedChargeDetailRecords.Select(cdr => cdr.ToOCHP(ContractIdDelegate:  emtid => _Lookup[emtid],
                                                                                                              CustomEVSEIdMapper:  null)).ToArray(),

                                                        Timestamp,
                                                        CancellationToken,
                                                        EventTrackingId,
                                                        RequestTimeout).ConfigureAwait(false);


                Endtime  = DateTime.UtcNow;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content        != null)
                {

                    var CDRIdHash = response.Content.ImplausibleCDRs.Any()
                                        ? new HashSet<ChargingSession_Id>(response.Content.ImplausibleCDRs.Select(cdrid => cdrid.ToWWCP()))
                                        : new HashSet<ChargingSession_Id>();

                    switch (response.Content.Result.ResultCode)
                    {

                        case ResultCodes.OK:
                            result = SendCDRsResult.Success(Id, this, ForwardedChargeDetailRecords);
                            break;

                        case ResultCodes.Partly:
                            result = SendCDRsResult.Error(Id,
                                                          this,
                                                          ForwardedChargeDetailRecords.Where(cdr => CDRIdHash.Contains(cdr.SessionId)));
                            break;

                        default:
                            result = SendCDRsResult.Error(Id,
                                                          this,
                                                          ForwardedChargeDetailRecords.Where(cdr => CDRIdHash.Contains(cdr.SessionId)));
                            break;

                    }

                }

                else
                    result = SendCDRsResult.Error(Id,
                                                  this,
                                                  ForwardedChargeDetailRecords,
                                                  response?.Content?.Result.Description);

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
                                           DroppedChargeDetailRecords,
                                           ForwardedChargeDetailRecords,
                                           RequestTimeout,
                                           result,
                                           Runtime);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsResponse));
            }

            #endregion

            return result;

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
            //var EVSEDataQueueCopy   = new AsyncLocal<HashSet<EVSE>>();
            //var EVSEStatusQueueCopy = new AsyncLocal<List<EVSEStatusChange>>();

            var EVSEsToAddQueueCopy                = new ThreadLocal<HashSet<EVSE>>();
            var EVSEDataQueueCopy                  = new ThreadLocal<HashSet<EVSE>>();
            var EVSEStatusChangesDelayedQueueCopy  = new ThreadLocal<List<EVSEStatusUpdate>>();
            var EVSEsToRemoveQueueCopy             = new ThreadLocal<HashSet<EVSE>>();

            if (Monitor.TryEnter(ServiceCheckLock))
            {

                try
                {

                    if (EVSEsToAddQueue.              Count == 0 &&
                        EVSEsToUpdateQueue.           Count == 0 &&
                        EVSEStatusChangesDelayedQueue.Count == 0 &&
                        EVSEsToRemoveQueue.           Count == 0 &&
                        ChargeDetailRecordsQueue.     Count == 0)
                    {
                        return;
                    }

                    _ServiceRunId++;

                    // Copy 'EVSEs to add', remove originals...
                    EVSEsToAddQueueCopy.Value                = new HashSet<EVSE>         (EVSEsToAddQueue);
                    EVSEsToAddQueue.Clear();

                    // Copy 'EVSEs to update', remove originals...
                    EVSEDataQueueCopy.Value                  = new HashSet<EVSE>         (EVSEsToUpdateQueue);
                    EVSEsToUpdateQueue.Clear();

                    // Copy 'EVSE status changes', remove originals...
                    EVSEStatusChangesDelayedQueueCopy.Value  = new List<EVSEStatusUpdate>(EVSEStatusChangesDelayedQueue);
                    EVSEStatusChangesDelayedQueueCopy.Value.AddRange(EVSEsToAddQueueCopy.Value.SafeSelect(evse => new EVSEStatusUpdate(evse, evse.Status, evse.Status)));
                    EVSEStatusChangesDelayedQueue.Clear();

                    // Copy 'EVSEs to remove', remove originals...
                    EVSEsToRemoveQueueCopy.Value             = new HashSet<EVSE>         (EVSEsToRemoveQueue);
                    EVSEsToRemoveQueue.Clear();

                    // Stop the timer. Will be rescheduled by next EVSE data/status change...
                    ServiceCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);

                }
                catch (Exception e)
                {

                    while (e.InnerException != null)
                        e = e.InnerException;

                    DebugX.LogT(nameof(WWCPCPOAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

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
            if (EVSEsToAddQueueCopy.              Value != null ||
                EVSEDataQueueCopy.                Value != null ||
                EVSEStatusChangesDelayedQueueCopy.Value != null ||
                EVSEsToRemoveQueueCopy.           Value != null)
            {

                // Use the events to evaluate if something went wrong!

                var EventTrackingId = EventTracking_Id.New;

                #region Send new EVSE data

                if (EVSEsToAddQueueCopy.Value.Count > 0)
                {

                    var EVSEsToAddTask = _ServiceRunId == 1
                                             ? (this as ISendData).SetStaticData(EVSEsToAddQueueCopy.Value, EventTrackingId: EventTrackingId)
                                             : (this as ISendData).AddStaticData(EVSEsToAddQueueCopy.Value, EventTrackingId: EventTrackingId);

                    EVSEsToAddTask.Wait();

                }

                #endregion

                #region Send changed EVSE data

                if (EVSEDataQueueCopy.Value.Count > 0)
                {

                    // Surpress EVSE data updates for all newly added EVSEs
                    var EVSEsWithoutNewEVSEs = EVSEDataQueueCopy.Value.
                                                   Where(evse => !EVSEsToAddQueueCopy.Value.Contains(evse)).
                                                   ToArray();


                    if (EVSEsWithoutNewEVSEs.Length > 0)
                    {

                        var SetChargePointInfosTask = (this as ISendData).UpdateStaticData(EVSEsWithoutNewEVSEs, EventTrackingId: EventTrackingId);

                        SetChargePointInfosTask.Wait();

                    }

                }

                #endregion

                #region Send changed EVSE status

                if (EVSEStatusChangesDelayedQueueCopy.Value.Count > 0)
                {

                    var UpdateEVSEStatusTask = UpdateEVSEStatus(EVSEStatusChangesDelayedQueueCopy.Value,
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
            => EVSEsToAddQueue.              Count == 0 &&
               EVSEsToUpdateQueue.           Count == 0 &&
               EVSEStatusChangesDelayedQueue.Count == 0 &&
               EVSEsToRemoveQueue.           Count == 0;

        protected override async Task FlushEVSEDataAndStatusQueues()
        {

            #region Get a copy of all current EVSE data and delayed status

            var EVSEsToAddQueueCopy                = new HashSet<EVSE>();
            var EVSEsToUpdateQueueCopy             = new HashSet<EVSE>();
            var EVSEStatusChangesDelayedQueueCopy  = new List<EVSEStatusUpdate>();
            var EVSEsToRemoveQueueCopy             = new HashSet<EVSE>();
            var EVSEsUpdateLogCopy                 = new Dictionary<EVSE,            PropertyUpdateInfos[]>();
            var ChargingStationsUpdateLogCopy      = new Dictionary<ChargingStation, PropertyUpdateInfos[]>();
            var ChargingPoolsUpdateLogCopy         = new Dictionary<ChargingPool,    PropertyUpdateInfos[]>();

            await DataAndStatusLock.WaitAsync();

            try
            {

                // Copy 'EVSEs to add', remove originals...
                EVSEsToAddQueueCopy                      = new HashSet<EVSE>                (EVSEsToAddQueue);
                EVSEsToAddQueue.Clear();

                // Copy 'EVSEs to update', remove originals...
                EVSEsToUpdateQueueCopy                   = new HashSet<EVSE>                (EVSEsToUpdateQueue);
                EVSEsToUpdateQueue.Clear();

                // Copy 'EVSE status changes', remove originals...
                EVSEStatusChangesDelayedQueueCopy        = new List<EVSEStatusUpdate>       (EVSEStatusChangesDelayedQueue);
                EVSEStatusChangesDelayedQueueCopy.AddRange(EVSEsToAddQueueCopy.SafeSelect(evse => new EVSEStatusUpdate(evse, evse.Status, evse.Status)));
                EVSEStatusChangesDelayedQueue.Clear();

                // Copy 'EVSEs to remove', remove originals...
                EVSEsToRemoveQueueCopy                   = new HashSet<EVSE>                (EVSEsToRemoveQueue);
                EVSEsToRemoveQueue.Clear();

                // Copy EVSE property updates
                EVSEsUpdateLog.           ForEach(_ => EVSEsUpdateLogCopy.           Add(_.Key, _.Value.ToArray()));
                EVSEsUpdateLog.Clear();

                // Copy charging station property updates
                ChargingStationsUpdateLog.ForEach(_ => ChargingStationsUpdateLogCopy.Add(_.Key, _.Value.ToArray()));
                ChargingStationsUpdateLog.Clear();

                // Copy charging pool property updates
                ChargingPoolsUpdateLog.   ForEach(_ => ChargingPoolsUpdateLogCopy.   Add(_.Key, _.Value.ToArray()));
                ChargingPoolsUpdateLog.Clear();


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

            if (EVSEsToAddQueueCopy.Count > 0)
            {

                //var EVSEsToAddTask = PushEVSEData(EVSEsToAddQueueCopy,
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
                var EVSEsWithoutNewEVSEs = EVSEsToUpdateQueueCopy.
                                               Where(evse => !EVSEsToAddQueueCopy.Contains(evse)).
                                               ToArray();


                if (EVSEsWithoutNewEVSEs.Length > 0)
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
                EVSEStatusChangesDelayedQueueCopy.Count > 0)
            {

                var pushEVSEStatusResult = await UpdateStatus(EVSEStatusChangesDelayedQueueCopy,
                                                              TransmissionTypes.Direct,

                                                              DateTime.UtcNow,
                                                              new CancellationTokenSource().Token,
                                                              EventTrackingId,
                                                              DefaultRequestTimeout).
                                                     ConfigureAwait(false);

                if (pushEVSEStatusResult.Warnings.Any())
                {

                    SendOnWarnings(DateTime.UtcNow,
                                   nameof(WWCPCPOAdapter) + Id,
                                   "UpdateStatus",
                                   pushEVSEStatusResult.Warnings);

                }

            }

            #endregion

            #region Send removed charging stations

            if (EVSEsToRemoveQueueCopy.Count > 0)
            {

                var EVSEsToRemove = EVSEsToRemoveQueueCopy.ToArray();

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
            => EVSEStatusChangesFastQueue.Count == 0;

        protected override async Task FlushEVSEFastStatusQueues()
        {

            #region Get a copy of all current EVSE data and delayed status

            var EVSEStatusFastQueueCopy = new List<EVSEStatusUpdate>();

            var LockTaken = await DataAndStatusLock.WaitAsync(MaxLockWaitingTime);

            try
            {

                if (LockTaken)
                {

                    if (EVSEStatusChangesFastQueue.Count == 0)
                        return;

                    _StatusRunId++;

                    // Copy 'EVSE status changes', remove originals...
                    EVSEStatusFastQueueCopy = new List<EVSEStatusUpdate>(EVSEStatusChangesFastQueue.Where(evsestatuschange => !EVSEsToAddQueue.Any(evse => evse == evsestatuschange.EVSE)));

                    // Add all evse status changes of EVSE *NOT YET UPLOADED* into the delayed queue...
                    var EVSEStatusChangesDelayed = EVSEStatusChangesFastQueue.Where(evsestatuschange => EVSEsToAddQueue.Any(evse => evse == evsestatuschange.EVSE)).ToArray();

                    if (EVSEStatusChangesDelayed.Length > 0)
                        EVSEStatusChangesDelayedQueue.AddRange(EVSEStatusChangesDelayed);

                    EVSEStatusChangesFastQueue.Clear();

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
                                   nameof(WWCPCPOAdapter) + Id,
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
                                                                             evsestatus.Status.Value.AsEVSEMajorStatus(),
                                                                             evsestatus.Status.Value.AsEVSEMinorStatus()));

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
                                                            ? Warnings.AddAndReturnList(response.HTTPBody.ToUTF8String())
                                                            : Warnings.AddAndReturnList("No HTTP body received!"),
                                                        Runtime);

                }

                #endregion

            }
            catch (Exception e)
            {

                while (e.InnerException != null)
                    e = e.InnerException;

                DebugX.LogT(nameof(WWCPCPOAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

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
            => ChargeDetailRecordsQueue.Count == 0;

        protected override async Task FlushChargeDetailRecordsQueues()
        {

            var ChargeDetailRecordsQueueCopy = ChargeDetailRecordsQueue.ToArray();

            if (ChargeDetailRecordsQueueCopy.Length > 0)
            {

                var sendCDRsResult = await CPORoaming.AddCDRs(ChargeDetailRecordsQueueCopy,

                                                              DateTime.UtcNow,
                                                              new CancellationTokenSource().Token,
                                                              EventTracking_Id.New,
                                                              DefaultRequestTimeout).
                                                      ConfigureAwait(false);

                if (sendCDRsResult.Content.ImplausibleCDRs.Any())
                {

                    SendOnWarnings(DateTime.UtcNow,
                                   nameof(WWCPCPOAdapter) + Id,
                                   "SendChargeDetailRecords",
                                   sendCDRsResult.Content.ImplausibleCDRs.
                                                          Select(cdr => Warning.Create(cdr.ToString())));

                }

            }

            //ToDo: Send FlushChargeDetailRecordsQueues result event...
            //ToDo: Re-add to queue if it could not be send...

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
        public static Boolean operator == (WWCPCPOAdapter WWCPCPOAdapter1, WWCPCPOAdapter WWCPCPOAdapter2)
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
        public static Boolean operator != (WWCPCPOAdapter WWCPCPOAdapter1, WWCPCPOAdapter WWCPCPOAdapter2)

            => !(WWCPCPOAdapter1 == WWCPCPOAdapter2);

        #endregion

        #region Operator <  (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (WWCPCPOAdapter  WWCPCPOAdapter1,
                                          WWCPCPOAdapter  WWCPCPOAdapter2)
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
        public static Boolean operator <= (WWCPCPOAdapter WWCPCPOAdapter1,
                                           WWCPCPOAdapter WWCPCPOAdapter2)

            => !(WWCPCPOAdapter1 > WWCPCPOAdapter2);

        #endregion

        #region Operator >  (WWCPCPOAdapter1, WWCPCPOAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPCPOAdapter1">A WWCPCPOAdapter.</param>
        /// <param name="WWCPCPOAdapter2">Another WWCPCPOAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (WWCPCPOAdapter WWCPCPOAdapter1,
                                          WWCPCPOAdapter WWCPCPOAdapter2)
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
        public static Boolean operator >= (WWCPCPOAdapter WWCPCPOAdapter1,
                                           WWCPCPOAdapter WWCPCPOAdapter2)

            => !(WWCPCPOAdapter1 < WWCPCPOAdapter2);

        #endregion

        #endregion

        #region IComparable<WWCPCPOAdapter> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var WWCPCPOAdapter = Object as WWCPCPOAdapter;
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
        public Int32 CompareTo(WWCPCPOAdapter WWCPCPOAdapter)
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

            var WWCPCPOAdapter = Object as WWCPCPOAdapter;
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
        public Boolean Equals(WWCPCPOAdapter WWCPCPOAdapter)
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
