﻿/*
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
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    /// <summary>
    /// A WWCP wrapper for the OCHP CPO Roaming client which maps
    /// WWCP data structures onto OCHP data structures and vice versa.
    /// </summary>
    public class WWCPCPOAdapter : ABaseEMobilityEntity<CSORoamingProvider_Id>,
                                  ICSORoamingProvider,
                                  IEquatable <WWCPCPOAdapter>,
                                  IComparable<WWCPCPOAdapter>,
                                  IComparable
    {

        #region Data

        private        readonly  ISendData                               _ISendData;

        private        readonly  ISendStatus                             _ISendStatus;

        private        readonly  CustomEVSEIdMapperDelegate                    _CustomEVSEIdMapper;

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
        public  readonly static TimeSpan                                       DefaultServiceCheckEvery       = TimeSpan.FromSeconds(31);

        /// <summary>
        /// The default status check intervall.
        /// </summary>
        public  readonly static TimeSpan                                       DefaultStatusCheckEvery        = TimeSpan.FromSeconds(3);

        /// <summary>
        /// The default EVSE status refresh intervall.
        /// </summary>
        public  readonly static TimeSpan                                       DefaultEVSEStatusRefreshEvery  = TimeSpan.FromHours(12);


        private readonly        Object                                         ServiceCheckLock;
        private readonly        Timer                                          ServiceCheckTimer;
        private readonly        Object                                         StatusCheckLock;
        private readonly        Timer                                          StatusCheckTimer;
        //private readonly        Object                                         EVSEStatusRefreshLock;
        private static          SemaphoreSlim                                  EVSEStatusRefreshLock  = new SemaphoreSlim(1,1);
        private readonly        TimeSpan                                       EVSEStatusRefreshEvery;
        private readonly        Timer                                          EVSEStatusRefreshTimer;

        private readonly        HashSet<EVSE>                                  EVSEsToAddQueue;
        private readonly        HashSet<EVSE>                                  EVSEsToUpdateQueue;
        private readonly        List<EVSEStatusUpdate>                         EVSEStatusChangesFastQueue;
        private readonly        List<EVSEStatusUpdate>                         EVSEStatusChangesDelayedQueue;
        private readonly        HashSet<EVSE>                                  EVSEsToRemoveQueue;
        private readonly        List<ChargeDetailRecord>                       ChargeDetailRecordQueue;

        private                 UInt64                                         _ServiceRunId;
        private                 UInt64                                         _StatusRunId;
        private                 IncludeEVSEDelegate                            _IncludeEVSEs;

        

        public readonly static TimeSpan DefaultRequestTimeout = TimeSpan.FromSeconds(30);

        #endregion

        #region Properties

        IId ISendAuthorizeStartStop.AuthId
            => Id;

        IId ISend2RemoteChargeDetailRecords.Id
            => Id;

        IEnumerable<IId> ISend2RemoteChargeDetailRecords.Ids
            => Ids.Cast<IId>();

        #region Name

        /// <summary>
        /// The offical (multi-language) name of the roaming provider.
        /// </summary>
        [Mandatory]
        public I18NString Name { get; }

        #endregion

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


        /// <summary>
        /// The attached DNS server.
        /// </summary>
        public DNSClient DNSClient
            => CPORoaming.DNSClient;


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

        #region FlushEVSEStatusUpdatesEvery

        private UInt32 _FlushEVSEStatusUpdatesEvery;

        /// <summary>
        /// The flush EVSE status updates intervall.
        /// </summary>
        public TimeSpan FlushEVSEStatusUpdatesEvery
        {

            get
            {
                return TimeSpan.FromSeconds(_FlushEVSEStatusUpdatesEvery);
            }

            set
            {
                _FlushEVSEStatusUpdatesEvery = (UInt32) value.TotalSeconds;
            }

        }

        #endregion


        public IncludeChargePointsDelegate  IncludeChargePoints   { get; set; }

        public IncludeEVSEIdsDelegate       IncludeEVSEIds        { get; set; }


        #region DisablePushData

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisablePushData                  { get; set; }

        #endregion

        #region DisablePushStatus

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisablePushStatus                { get; set; }

        #endregion

        #region DisableAuthentication

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisableAuthentication            { get; set; }

        #endregion

        #region DisableSendChargeDetailRecords

        /// <summary>
        /// This service can be disabled, e.g. for debugging reasons.
        /// </summary>
        public Boolean  DisableSendChargeDetailRecords   { get; set; }

        #endregion

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
        public WWCPCPOAdapter(CSORoamingProvider_Id                        Id,
                              I18NString                                   Name,
                              RoamingNetwork                               RoamingNetwork,

                              CPORoaming                                   CPORoaming,
                              CustomEVSEIdMapperDelegate                   CustomEVSEIdMapper               = null,
                              EVSE2ChargePointInfoDelegate                 EVSE2ChargePointInfo             = null,
                              EVSEStatusUpdate2EVSEStatusDelegate          EVSEStatusUpdate2EVSEStatus      = null,
                              ChargePointInfo2XMLDelegate                  ChargePointInfo2XML              = null,
                              EVSEStatus2XMLDelegate                       EVSEStatus2XML                   = null,

                              IncludeEVSEDelegate                          IncludeEVSEs                     = null,
                              TimeSpan?                                    ServiceCheckEvery                = null,
                              TimeSpan?                                    StatusCheckEvery                 = null,
                              TimeSpan?                                    EVSEStatusRefreshEvery           = null,

                              Boolean                                      DisablePushData                  = false,
                              Boolean                                      DisablePushStatus                = false,
                              Boolean                                      DisableEVSEStatusRefresh         = false,
                              Boolean                                      DisableAuthentication            = false,
                              Boolean                                      DisableSendChargeDetailRecords   = false)

            : base(Id,
                   RoamingNetwork)

        {

            #region Initial checks

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),        "The given roaming provider name must not be null or empty!");

            if (CPORoaming == null)
                throw new ArgumentNullException(nameof(CPORoaming),  "The given OCHP CPO Roaming object must not be null!");

            #endregion

            this.Name = Name;
            this._ISendData                           = this as ISendData;
            this._ISendStatus                         = this as ISendStatus;

            this.CPORoaming                           = CPORoaming;
            this._CustomEVSEIdMapper                  = CustomEVSEIdMapper;
            this._EVSE2ChargePointInfo                = EVSE2ChargePointInfo;
            this._EVSEStatusUpdate2EVSEStatus         = EVSEStatusUpdate2EVSEStatus;
            this._ChargePointInfo2XML                 = ChargePointInfo2XML;
            this._EVSEStatus2XML                      = EVSEStatus2XML;

            this._IncludeEVSEs                        = IncludeEVSEs;

            this.ServiceCheckLock                     = new Object();
            this._ServiceCheckEvery                   = (UInt32) (ServiceCheckEvery.HasValue
                                                                     ? ServiceCheckEvery.Value. TotalMilliseconds
                                                                     : DefaultServiceCheckEvery.TotalMilliseconds);
            this.ServiceCheckTimer                    = new Timer(ServiceCheck,      null,                           0, _ServiceCheckEvery);

            this.StatusCheckLock                      = new Object();
            this._FlushEVSEStatusUpdatesEvery                    = (UInt32) (StatusCheckEvery.HasValue
                                                                     ? StatusCheckEvery.Value.  TotalMilliseconds
                                                                     : DefaultStatusCheckEvery. TotalMilliseconds);
            this.StatusCheckTimer                     = new Timer(FlushEVSEStatusUpdates,       null,                           0, _FlushEVSEStatusUpdatesEvery);

            this.EVSEStatusRefreshEvery               = EVSEStatusRefreshEvery ?? DefaultEVSEStatusRefreshEvery;
            this.EVSEStatusRefreshTimer               = new Timer(EVSEStatusRefresh, null, this.EVSEStatusRefreshEvery, this.EVSEStatusRefreshEvery);

            this.DisablePushData                      = DisablePushData;
            this.DisablePushStatus                    = DisablePushStatus;
            this.DisableEVSEStatusRefresh             = DisableEVSEStatusRefresh;
            this.DisableAuthentication                = DisableAuthentication;
            this.DisableSendChargeDetailRecords       = DisableSendChargeDetailRecords;

            this.EVSEsToAddQueue                      = new HashSet<EVSE>();
            this.EVSEsToUpdateQueue                   = new HashSet<EVSE>();
            this.EVSEStatusChangesFastQueue           = new List<EVSEStatusUpdate>();
            this.EVSEStatusChangesDelayedQueue        = new List<EVSEStatusUpdate>();
            this.EVSEsToRemoveQueue                   = new HashSet<EVSE>();
            this.ChargeDetailRecordQueue              = new List<ChargeDetailRecord>();


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
        public WWCPCPOAdapter(CSORoamingProvider_Id                        Id,
                              I18NString                                   Name,
                              RoamingNetwork                               RoamingNetwork,

                              CPOClient                                    CPOClient,
                              CPOServer                                    CPOServer,
                              String                                       ServerLoggingContext             = CPOServerLogger.DefaultContext,
                              LogfileCreatorDelegate                       LogFileCreator                   = null,

                              CustomEVSEIdMapperDelegate                   CustomEVSEIdMapper               = null,
                              EVSE2ChargePointInfoDelegate                 EVSE2ChargePointInfo             = null,
                              EVSEStatusUpdate2EVSEStatusDelegate          EVSEStatusUpdate2EVSEStatus      = null,
                              ChargePointInfo2XMLDelegate                  ChargePointInfo2XML              = null,
                              EVSEStatus2XMLDelegate                       EVSEStatus2XML                   = null,

                              IncludeEVSEDelegate                          IncludeEVSEs                     = null,
                              TimeSpan?                                    ServiceCheckEvery                = null,
                              TimeSpan?                                    StatusCheckEvery                 = null,
                              TimeSpan?                                    EVSEStatusRefreshEvery           = null,

                              Boolean                                      DisablePushData                  = false,
                              Boolean                                      DisablePushStatus                = false,
                              Boolean                                      DisableEVSEStatusRefresh         = false,
                              Boolean                                      DisableAuthentication            = false,
                              Boolean                                      DisableSendChargeDetailRecords   = false)

            : this(Id,
                   Name,
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

                   IncludeEVSEs,
                   ServiceCheckEvery,
                   StatusCheckEvery,
                   EVSEStatusRefreshEvery,

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
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OCHP service.</param>
        /// <param name="URIPrefix">An default URI prefix.</param>
        /// <param name="WSSLoginPassword">The WebService-Security username/password.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
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
        public WWCPCPOAdapter(CSORoamingProvider_Id                        Id,
                              I18NString                                   Name,
                              RoamingNetwork                               RoamingNetwork,

                              String                                       RemoteHostname,
                              IPPort                                       RemoteTCPPort                       = null,
                              RemoteCertificateValidationCallback          RemoteCertificateValidator          = null,
                              X509Certificate                              ClientCert                          = null,
                              String                                       RemoteHTTPVirtualHost               = null,
                              String                                       URIPrefix                           = CPOClient.DefaultURIPrefix,
                              Tuple<String, String>                        WSSLoginPassword                    = null,
                              String                                       HTTPUserAgent                       = CPOClient.DefaultHTTPUserAgent,
                              TimeSpan?                                    RequestTimeout                      = null,

                              String                                       ServerName                          = CPOServer.DefaultHTTPServerName,
                              IPPort                                       ServerTCPPort                       = null,
                              String                                       ServerURIPrefix                     = CPOServer.DefaultURIPrefix,
                              String                                       ServerURISuffix                     = CPOServer.DefaultURISuffix,
                              HTTPContentType                              ServerContentType                   = null,
                              Boolean                                      ServerRegisterHTTPRootService       = true,
                              Boolean                                      ServerAutoStart                     = false,

                              String                                       ClientLoggingContext                = CPOClient.CPOClientLogger.DefaultContext,
                              String                                       ServerLoggingContext                = CPOServerLogger.DefaultContext,
                              LogfileCreatorDelegate                       LogFileCreator                      = null,

                              CustomEVSEIdMapperDelegate                   CustomEVSEIdMapper                  = null,
                              EVSE2ChargePointInfoDelegate                 EVSE2ChargePointInfo                = null,
                              EVSEStatusUpdate2EVSEStatusDelegate          EVSEStatusUpdate2EVSEStatus         = null,
                              ChargePointInfo2XMLDelegate                  ChargePointInfo2XML                 = null,
                              EVSEStatus2XMLDelegate                       EVSEStatus2XML                      = null,

                              IncludeEVSEDelegate                          IncludeEVSEs                        = null,
                              TimeSpan?                                    ServiceCheckEvery                   = null,
                              TimeSpan?                                    StatusCheckEvery                    = null,
                              TimeSpan?                                    EVSEStatusRefreshEvery              = null,

                              Boolean                                      DisablePushData                     = false,
                              Boolean                                      DisablePushStatus                   = false,
                              Boolean                                      DisableEVSEStatusRefresh            = false,
                              Boolean                                      DisableAuthentication               = false,
                              Boolean                                      DisableSendChargeDetailRecords      = false,

                              DNSClient                                    DNSClient                           = null)

            : this(Id,
                   Name,
                   RoamingNetwork,

                   new CPORoaming(Id.ToString(),
                                  RemoteHostname,
                                  RemoteTCPPort,
                                  RemoteCertificateValidator,
                                  ClientCert,
                                  RemoteHTTPVirtualHost,
                                  URIPrefix,
                                  WSSLoginPassword,
                                  HTTPUserAgent,
                                  RequestTimeout,

                                  ServerName,
                                  ServerTCPPort,
                                  ServerURIPrefix,
                                  ServerURISuffix,
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

                   IncludeEVSEs,
                   ServiceCheckEvery,
                   StatusCheckEvery,
                   EVSEStatusRefreshEvery,

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
        private async Task<Acknowledgement>

            SetChargePointInfos(IEnumerable<EVSE>   EVSEs,

                                DateTime?           Timestamp           = null,
                                CancellationToken?  CancellationToken   = null,
                                EventTracking_Id    EventTrackingId     = null,
                                TimeSpan?           RequestTimeout      = null)

        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs), "The given enumeration of EVSEs must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSE status to upload

            var Warnings = new List<String>();

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
                                                  Warnings.Add(e.Message);
                                              }

                                              return null;

                                          }).
                                          Where(chargepointinfo => chargepointinfo != null).
                                          ToArray();

            Acknowledgement result;

            #endregion

            #region Send OnSetChargePointInfosWWCPRequest event

            var StartTime = DateTime.Now;

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


            var response = await CPORoaming.
                                     SetChargePointList(_ChargePointInfos,
                                                        IncludeChargePoints,
                                                        //IncludeEVSEIds,

                                                        Timestamp,
                                                        CancellationToken,
                                                        EventTrackingId,
                                                        RequestTimeout).
                                     ConfigureAwait(false);


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null)
            {

                if (response.Content.Result.ResultCode == ResultCodes.OK)
                    result = new Acknowledgement(ResultType.True,
                                                      response.Content.Result.Description,
                                                      null,
                                                      Runtime);

                else
                    result = new Acknowledgement(ResultType.False,
                                                      response.Content.Result.Description,
                                                      null,
                                                      Runtime);

            }
            else
                result = new Acknowledgement(ResultType.False,
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
                                                          Warnings.Where(warning => warning.IsNotNullOrEmpty()),
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
        private async Task<Acknowledgement>

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
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSE status to upload

            var Warnings = new List<String>();

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
                                                  Warnings.Add(e.Message);
                                              }

                                              return null;

                                          }).
                                          Where(chargepointinfo => chargepointinfo != null).
                                          ToArray();

            Acknowledgement result;

            #endregion

            #region Send OnSetChargePointInfosWWCPRequest event

            var StartTime = DateTime.Now;

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


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null)
            {

                if (response.Content.Result.ResultCode == ResultCodes.OK)
                    result = new Acknowledgement(ResultType.True,
                                                      response.Content.Result.Description,
                                                      null,
                                                      Runtime);

                else
                    result = new Acknowledgement(ResultType.False,
                                                      response.Content.Result.Description,
                                                      null,
                                                      Runtime);

            }
            else
                result = new Acknowledgement(ResultType.False,
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
                                                             Warnings.Where(warning => warning.IsNotNullOrEmpty()),
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
        public async Task<Acknowledgement>

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
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Get effective number of EVSE status to upload

            EVSE_Id EVSEId;

            var Warnings       = new List<String>();
            var AllEVSEStatus  = new Dictionary<EVSE_Id, EVSEStatus>();

            foreach (var evsestatusupdate in EVSEStatusUpdates.OrderByDescending(sup => sup.NewStatus.Timestamp))
            {

                try
                {

                    EVSEId = _CustomEVSEIdMapper != null
                                 ? _CustomEVSEIdMapper(evsestatusupdate.EVSE.Id)
                                 : evsestatusupdate.EVSE.Id.ToOCHP();

                    if (IncludeEVSEIds(EVSEId) &&
                        !AllEVSEStatus.ContainsKey(EVSEId))
                    {

                        AllEVSEStatus.Add(EVSEId, new EVSEStatus(EVSEId,
                                                                 evsestatusupdate.NewStatus.Value.AsEVSEMajorStatus(),
                                                                 evsestatusupdate.NewStatus.Value.AsEVSEMinorStatus()));

                    }

                }
                catch (Exception e)
                {
                    DebugX.  Log(e.Message);
                    Warnings.Add(e.Message);
                }

            }

            //var _EVSEStatus = EVSEStatusUpdates.
            //                      Where       (evsestatusupdate => _IncludeEVSEs(evsestatusupdate.EVSE)).
            //                      ToLookup    (evsestatusupdate => evsestatusupdate.EVSE.Id,
            //                                   evsestatusupdate => evsestatusupdate).
            //                      ToDictionary(group            => group.Key,
            //                                   group            => group.AsEnumerable().OrderByDescending(item => item.NewStatus.Timestamp)).
            //                      Select      (evsestatusupdate => {

            //                          try
            //                          {

            //                              // Only push the current major/minor status of the latest status update!
            //                              return new EVSEStatus?(
            //                                         new EVSEStatus(
            //                                             _CustomEVSEIdMapper != null
            //                                                 ? _CustomEVSEIdMapper(evsestatusupdate.Key)
            //                                                 : evsestatusupdate.Key.ToOCHP(),
            //                                             evsestatusupdate.Value.First().NewStatus.Value.AsEVSEMajorStatus(),
            //                                             evsestatusupdate.Value.First().NewStatus.Value.AsEVSEMinorStatus()
            //                                         )
            //                                     );

            //                          }
            //                          catch (Exception e)
            //                          {
            //                              DebugX.  Log(e.Message);
            //                              Warnings.Add(e.Message);
            //                          }

            //                          return null;

            //                      }).
            //                      Where (evsestatus => evsestatus != null).
            //                      Select(evsestatus => evsestatus.Value).
            //                      ToArray();

            Acknowledgement result = null;

            #endregion

            #region Send OnUpdateEVSEStatusWWCPRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnUpdateEVSEStatusWWCPRequest?.Invoke(StartTime,
                                                      Timestamp.Value,
                                                      this,
                                                      Id,
                                                      EventTrackingId,
                                                      RoamingNetwork.Id,
                                                      AllEVSEStatus.ULongCount(),
                                                      AllEVSEStatus.Values,
                                                      Warnings.Where(warning => warning.IsNotNullOrEmpty()),
                                                      RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnUpdateEVSEStatusWWCPRequest));
            }

            #endregion


            var response = await CPORoaming.
                                     UpdateStatus(AllEVSEStatus.Values,
                                                  null,
                                                  DateTime.UtcNow + EVSEStatusRefreshEvery,
                                                  null,

                                                  Timestamp,
                                                  CancellationToken,
                                                  EventTrackingId,
                                                  RequestTimeout).
                                     ConfigureAwait(false);


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

            if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                response.Content        != null)
            {

                if (response.Content.Result.ResultCode == ResultCodes.OK)
                    result = new Acknowledgement(ResultType.True,
                                                 response.Content.Result.Description,
                                                 null,
                                                 Runtime);

                else
                    result = new Acknowledgement(ResultType.False,
                                                 response.Content.Result.Description,
                                                 null,
                                                 Runtime);

            }
            else
                result = new Acknowledgement(ResultType.False,
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
                                                       AllEVSEStatus.ULongCount(),
                                                       AllEVSEStatus.Values,
                                                       Warnings.Where(warning => warning.IsNotNullOrEmpty()),
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

        #region SetStaticData   (EVSE, TransmissionType = Enqueued, ...)

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
        async Task<Acknowledgement>

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

            if (TransmissionType == TransmissionTypes.Enqueued)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
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

                return new Acknowledgement(ResultType.Enqueued);

            }

            #endregion

            return await SetChargePointInfos(new EVSE[] { EVSE },

                                             Timestamp,
                                             CancellationToken,
                                             EventTrackingId,
                                             RequestTimeout).
 
                                             ConfigureAwait(false);

        }

        #endregion

        #region AddStaticData   (EVSE, TransmissionType = Enqueued, ...)

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
        async Task<Acknowledgement>

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

            if (TransmissionType == TransmissionTypes.Enqueued)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
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

                return new Acknowledgement(ResultType.Enqueued);

            }

            #endregion

            return await UpdateChargePointInfos(new EVSE[] { EVSE },

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

        }

        #endregion

        #region UpdateStaticData(EVSE, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueued, ...)

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
        async Task<Acknowledgement>

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

            if (TransmissionType == TransmissionTypes.Enqueued)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
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

                return new Acknowledgement(ResultType.Enqueued);

            }

            #endregion

            return await UpdateChargePointInfos(new EVSE[] { EVSE },

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

        }

        #endregion

        #region DeleteStaticData(EVSE, TransmissionType = Enqueued, ...)

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
        Task<Acknowledgement>

            ISendData.DeleteStaticData(EVSE                EVSE,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

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
        async Task<Acknowledgement>

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

            return await SetChargePointInfos(EVSEs,

                                             Timestamp,
                                             CancellationToken,
                                             EventTrackingId,
                                             RequestTimeout).

                                             ConfigureAwait(false);

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
        async Task<Acknowledgement>

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

            return await UpdateChargePointInfos(EVSEs,

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

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
        async Task<Acknowledgement>

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

            return await UpdateChargePointInfos(EVSEs,

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

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
        Task<Acknowledgement>

            ISendData.DeleteStaticData(IEnumerable<EVSE>   EVSEs,
                                       TransmissionTypes   TransmissionType,

                                       DateTime?           Timestamp,
                                       CancellationToken?  CancellationToken,
                                       EventTracking_Id    EventTrackingId,
                                       TimeSpan?           RequestTimeout)

                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueued, ...)

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
        Task<Acknowledgement>

            ISendStatus.UpdateAdminStatus(IEnumerable<EVSEAdminStatusUpdate>  AdminStatusUpdates,
                                          TransmissionTypes                   TransmissionType,

                                          DateTime?                           Timestamp,
                                          CancellationToken?                  CancellationToken,
                                          EventTracking_Id                    EventTrackingId,
                                          TimeSpan?                           RequestTimeout)


                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueued, ...)

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
        async Task<Acknowledgement>

            ISendStatus.UpdateStatus(IEnumerable<EVSEStatusUpdate>  StatusUpdates,
                                     TransmissionTypes              TransmissionType,

                                     DateTime?                      Timestamp,
                                     CancellationToken?             CancellationToken,
                                     EventTracking_Id               EventTrackingId,
                                     TimeSpan?                      RequestTimeout)

        {

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueued)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
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

                    //if (_IncludeEVSEs == null ||
                    //   (_IncludeEVSEs != null && _IncludeEVSEs(EVSE)))
                    //{

                        EVSEStatusChangesFastQueue.AddRange(StatusUpdates);
                        StatusCheckTimer.Change(_FlushEVSEStatusUpdatesEvery, Timeout.Infinite);

                    //}

                }

                return new Acknowledgement(ResultType.True);

            }

            #endregion


            return await UpdateEVSEStatus(StatusUpdates,

                                          Timestamp,
                                          CancellationToken,
                                          EventTrackingId,
                                          RequestTimeout).

                                          ConfigureAwait(false);

        }

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging station(s)...

        #region SetStaticData   (ChargingStation, TransmissionType = Enqueued, ...)

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
        async Task<Acknowledgement>

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

            if (TransmissionType == TransmissionTypes.Enqueued)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
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

                return new Acknowledgement(ResultType.Enqueued);

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

        #region AddStaticData   (ChargingStation, TransmissionType = Enqueued, ...)

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
        async Task<Acknowledgement>

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

            if (TransmissionType == TransmissionTypes.Enqueued)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
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

                return new Acknowledgement(ResultType.Enqueued);

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

        #region UpdateStaticData(ChargingStation, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueued, ...)

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
        async Task<Acknowledgement>

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

            if (TransmissionType == TransmissionTypes.Enqueued)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
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

                return new Acknowledgement(ResultType.Enqueued);

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

        #region DeleteStaticData(ChargingStation, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging station from the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStation">A charging station.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            ISendData.DeleteStaticData(ChargingStation     ChargingStation,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

        #endregion


        #region SetStaticData   (ChargingStations, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging stations as new static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<Acknowledgement>

            ISendData.SetStaticData(IEnumerable<ChargingStation>  ChargingStations,

                                          DateTime?                     Timestamp,
                                          CancellationToken?            CancellationToken,
                                          EventTracking_Id              EventTrackingId,
                                          TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return await SetChargePointInfos(ChargingStations.SafeSelectMany(station => station.EVSEs),

                                             Timestamp,
                                             CancellationToken,
                                             EventTrackingId,
                                             RequestTimeout).

                                             ConfigureAwait(false);

        }

        #endregion

        #region AddStaticData   (ChargingStations, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging stations to the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<Acknowledgement>

            ISendData.AddStaticData(IEnumerable<ChargingStation>  ChargingStations,

                                          DateTime?                     Timestamp,
                                          CancellationToken?            CancellationToken,
                                          EventTracking_Id              EventTrackingId,
                                          TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return await UpdateChargePointInfos(ChargingStations.SafeSelectMany(station => station.EVSEs),

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

        }

        #endregion

        #region UpdateStaticData(ChargingStations, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging stations within the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<Acknowledgement>

            ISendData.UpdateStaticData(IEnumerable<ChargingStation>  ChargingStations,

                                             DateTime?                     Timestamp,
                                             CancellationToken?            CancellationToken,
                                             EventTracking_Id              EventTrackingId,
                                             TimeSpan?                     RequestTimeout)

        {

            #region Initial checks

            if (ChargingStations == null)
                throw new ArgumentNullException(nameof(ChargingStations), "The given enumeration of charging stations must not be null!");

            #endregion

            return await UpdateChargePointInfos(ChargingStations.SafeSelectMany(station => station.EVSEs),

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

        }

        #endregion

        #region DeleteStaticData(ChargingStations, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging stations from the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingStations">An enumeration of charging stations.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            ISendData.DeleteStaticData(IEnumerable<ChargingStation>  ChargingStations,

                                             DateTime?                     Timestamp,
                                             CancellationToken?            CancellationToken,
                                             EventTracking_Id              EventTrackingId,
                                             TimeSpan?                     RequestTimeout)

                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueued, ...)

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
        Task<Acknowledgement>

            ISendStatus.UpdateAdminStatus(IEnumerable<ChargingStationAdminStatusUpdate>  AdminStatusUpdates,
                                          TransmissionTypes                              TransmissionType,

                                          DateTime?                                      Timestamp,
                                          CancellationToken?                             CancellationToken,
                                          EventTracking_Id                               EventTrackingId,
                                          TimeSpan?                                      RequestTimeout)


                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueued, ...)

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
        Task<Acknowledgement>

            ISendStatus.UpdateStatus(IEnumerable<ChargingStationStatusUpdate>  StatusUpdates,

                                     TransmissionTypes                         TransmissionType,
                                     DateTime?                                 Timestamp,
                                     CancellationToken?                        CancellationToken,
                                     EventTracking_Id                          EventTrackingId,
                                     TimeSpan?                                 RequestTimeout)


                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

        #endregion

        #endregion

        #region (Set/Add/Update/Delete) Charging pool(s)...

        #region SetStaticData   (ChargingPool, TransmissionType = Enqueued, ...)

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
        async Task<Acknowledgement>

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

            if (TransmissionType == TransmissionTypes.Enqueued)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
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

                return new Acknowledgement(ResultType.Enqueued);

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

        #region AddStaticData   (ChargingPool, TransmissionType = Enqueued, ...)

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
        async Task<Acknowledgement>

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

            if (TransmissionType == TransmissionTypes.Enqueued)
            {

                #region Send OnEnqueueSendCDRRequest event

                //try
                //{

                //    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
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

                return new Acknowledgement(ResultType.Enqueued);

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

        #region UpdateStaticData(ChargingPool, PropertyName = null, OldValue = null, NewValue = null, TransmissionType = Enqueued, ...)

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
        async Task<Acknowledgement>

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

                if (TransmissionType == TransmissionTypes.Enqueued)
                {

                    #region Send OnEnqueueSendCDRRequest event

                    //try
                    //{

                    //    OnEnqueueSendCDRRequest?.Invoke(DateTime.Now,
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

                    return new Acknowledgement(ResultType.Enqueued);

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

        #region DeleteStaticData(ChargingPool, ...)

        /// <summary>
        /// Delete the EVSE data of the given charging pool from the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingPool">A charging pool.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            ISendData.DeleteStaticData(ChargingPool        ChargingPool,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

        #endregion


        #region SetStaticData   (ChargingPools, ...)

        /// <summary>
        /// Set the EVSE data of the given enumeration of charging pools as new static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<Acknowledgement>

            ISendData.SetStaticData(IEnumerable<ChargingPool>  ChargingPools,

                                          DateTime?                  Timestamp,
                                          CancellationToken?         CancellationToken,
                                          EventTracking_Id           EventTrackingId,
                                          TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return await SetChargePointInfos(ChargingPools.SafeSelectMany(pool => pool.EVSEs),

                                             Timestamp,
                                             CancellationToken,
                                             EventTrackingId,
                                             RequestTimeout).

                                             ConfigureAwait(false);

        }

        #endregion

        #region AddStaticData   (ChargingPools, ...)

        /// <summary>
        /// Add the EVSE data of the given enumeration of charging pools to the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<Acknowledgement>

            ISendData.AddStaticData(IEnumerable<ChargingPool>  ChargingPools,

                                          DateTime?                  Timestamp,
                                          CancellationToken?         CancellationToken,
                                          EventTracking_Id           EventTrackingId,
                                          TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return await UpdateChargePointInfos(ChargingPools.SafeSelectMany(pool => pool.EVSEs),

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

        }

        #endregion

        #region UpdateStaticData(ChargingPools, ...)

        /// <summary>
        /// Update the EVSE data of the given enumeration of charging pools within the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        async Task<Acknowledgement>

            ISendData.UpdateStaticData(IEnumerable<ChargingPool>  ChargingPools,

                                             DateTime?                  Timestamp,
                                             CancellationToken?         CancellationToken,
                                             EventTracking_Id           EventTrackingId,
                                             TimeSpan?                  RequestTimeout)

        {

            #region Initial checks

            if (ChargingPools == null)
                throw new ArgumentNullException(nameof(ChargingPools), "The given enumeration of charging pools must not be null!");

            #endregion

            return await UpdateChargePointInfos(ChargingPools.SafeSelectMany(pool => pool.EVSEs),

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout).

                                                ConfigureAwait(false);

        }

        #endregion

        #region DeleteStaticData(ChargingPools, ...)

        /// <summary>
        /// Delete the EVSE data of the given enumeration of charging pools from the static EVSE data at the OCHP server.
        /// </summary>
        /// <param name="ChargingPools">An enumeration of charging pools.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        Task<Acknowledgement>

            ISendData.DeleteStaticData(IEnumerable<ChargingPool>  ChargingPools,

                                             DateTime?                  Timestamp,
                                             CancellationToken?         CancellationToken,
                                             EventTracking_Id           EventTrackingId,
                                             TimeSpan?                  RequestTimeout)

                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueued, ...)

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
        Task<Acknowledgement>

            ISendStatus.UpdateAdminStatus(IEnumerable<ChargingPoolAdminStatusUpdate>  AdminStatusUpdates,
                                          TransmissionTypes                           TransmissionType,

                                          DateTime?                                   Timestamp,
                                          CancellationToken?                          CancellationToken,
                                          EventTracking_Id                            EventTrackingId,
                                          TimeSpan?                                   RequestTimeout)


                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueued, ...)

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
        Task<Acknowledgement>

            ISendStatus.UpdateStatus(IEnumerable<ChargingPoolStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                      TransmissionType,

                                     DateTime?                              Timestamp,
                                     CancellationToken?                     CancellationToken,
                                     EventTracking_Id                       EventTrackingId,
                                     TimeSpan?                              RequestTimeout)


                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

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
        async Task<Acknowledgement>

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
        async Task<Acknowledgement>

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
        async Task<Acknowledgement>

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
        Task<Acknowledgement>

            ISendData.DeleteStaticData(ChargingStationOperator  ChargingStationOperator,

                                             DateTime?                Timestamp,
                                             CancellationToken?       CancellationToken,
                                             EventTracking_Id         EventTrackingId,
                                             TimeSpan?                RequestTimeout)

                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

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
        async Task<Acknowledgement>

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
        async Task<Acknowledgement>

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
        async Task<Acknowledgement>

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
        Task<Acknowledgement>

            ISendData.DeleteStaticData(IEnumerable<ChargingStationOperator>  ChargingStationOperators,

                                             DateTime?                             Timestamp,
                                             CancellationToken?                    CancellationToken,
                                             EventTracking_Id                      EventTrackingId,
                                             TimeSpan?                             RequestTimeout)

                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueued, ...)

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
        Task<Acknowledgement>

            ISendStatus.UpdateAdminStatus(IEnumerable<ChargingStationOperatorAdminStatusUpdate>  AdminStatusUpdates,
                                          TransmissionTypes                                      TransmissionType,

                                          DateTime?                                              Timestamp,
                                          CancellationToken?                                     CancellationToken,
                                          EventTracking_Id                                       EventTrackingId,
                                          TimeSpan?                                              RequestTimeout)


                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueued, ...)

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
        Task<Acknowledgement>

            ISendStatus.UpdateStatus(IEnumerable<ChargingStationOperatorStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                                 TransmissionType,

                                     DateTime?                                         Timestamp,
                                     CancellationToken?                                CancellationToken,
                                     EventTracking_Id                                  EventTrackingId,
                                     TimeSpan?                                         RequestTimeout)


                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

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
        async Task<Acknowledgement>

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
        async Task<Acknowledgement>

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
        async Task<Acknowledgement>

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
        Task<Acknowledgement>

            ISendData.DeleteStaticData(RoamingNetwork      RoamingNetwork,

                                             DateTime?           Timestamp,
                                             CancellationToken?  CancellationToken,
                                             EventTracking_Id    EventTrackingId,
                                             TimeSpan?           RequestTimeout)

                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

        #endregion


        #region UpdateAdminStatus(AdminStatusUpdates, TransmissionType = Enqueued, ...)

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
        Task<Acknowledgement>

            ISendStatus.UpdateAdminStatus(IEnumerable<RoamingNetworkAdminStatusUpdate>  AdminStatusUpdates,
                                          TransmissionTypes                             TransmissionType,

                                          DateTime?                                     Timestamp,
                                          CancellationToken?                            CancellationToken,
                                          EventTracking_Id                              EventTrackingId,
                                          TimeSpan?                                     RequestTimeout)


                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

        #endregion

        #region UpdateStatus     (StatusUpdates,      TransmissionType = Enqueued, ...)

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
        Task<Acknowledgement>

            ISendStatus.UpdateStatus(IEnumerable<RoamingNetworkStatusUpdate>  StatusUpdates,
                                     TransmissionTypes                        TransmissionType,

                                     DateTime?                                Timestamp,
                                     CancellationToken?                       CancellationToken,
                                     EventTracking_Id                         EventTrackingId,
                                     TimeSpan?                                RequestTimeout)


                => Task.FromResult(new Acknowledgement(ResultType.NoOperation));

        #endregion

        #endregion

        #endregion

        #region AuthorizeStart/-Stop  directly...

        #region AuthorizeStart(AuthIdentification,                    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingProduct">An optional charging product.</param>
        /// <param name="SessionId">An optional session identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStartResult>

            AuthorizeStart(AuthIdentification           AuthIdentification,
                           ChargingProduct              ChargingProduct     = null,
                           ChargingSession_Id?          SessionId           = null,
                           ChargingStationOperator_Id?  OperatorId          = null,

                           DateTime?                    Timestamp           = null,
                           CancellationToken?           CancellationToken   = null,
                           EventTracking_Id             EventTrackingId     = null,
                           TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification),   "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeStartRequest?.Invoke(StartTime,
                                                Timestamp.Value,
                                                this,
                                                Id.ToString(),
                                                EventTrackingId,
                                                RoamingNetwork.Id,
                                                OperatorId,
                                                AuthIdentification,
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
                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;
                result   = AuthStartResult.OutOfService(Id,
                                                        this,
                                                        SessionId,
                                                        Runtime);
            }

            else
            {

                var response = await CPORoaming.GetSingleRoamingAuthorisation(new EMT_Id(
                                                                                  AuthIdentification.AuthToken.ToString(),
                                                                                  TokenRepresentations.Plain,
                                                                                  TokenTypes.RFID
                                                                              ),

                                                                              Timestamp,
                                                                              CancellationToken,
                                                                              EventTrackingId,
                                                                              RequestTimeout).ConfigureAwait(false);


                Endtime  = DateTime.Now;
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
                                                 AuthIdentification,
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

        #region AuthorizeStart(AuthIdentification, EVSEId,            ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given EVSE.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
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

            AuthorizeStart(AuthIdentification           AuthIdentification,
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

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification),  "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeEVSEStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeEVSEStartRequest?.Invoke(StartTime,
                                                    Timestamp.Value,
                                                    this,
                                                    Id.ToString(),
                                                    EventTrackingId,
                                                    RoamingNetwork.Id,
                                                    OperatorId,
                                                    AuthIdentification,
                                                    EVSEId,
                                                    ChargingProduct,
                                                    SessionId,
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
                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;
                result   = AuthStartEVSEResult.OutOfService(Id,
                                                            this,
                                                            SessionId,
                                                            Runtime);
            }

            else
            {

                var response = await CPORoaming.GetSingleRoamingAuthorisation(new EMT_Id(
                                                                                  AuthIdentification.AuthToken.ToString(),
                                                                                  TokenRepresentations.Plain,
                                                                                  TokenTypes.RFID
                                                                              ),

                                                                              Timestamp,
                                                                              CancellationToken,
                                                                              EventTrackingId,
                                                                              RequestTimeout).ConfigureAwait(false);


                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode            == HTTPStatusCode.OK &&
                    response.Content                   != null              &&
                    response.Content.Result.ResultCode == ResultCodes.OK)
                {

                    result = AuthStartEVSEResult.Authorized(Id,
                                                            this,
                                                            ChargingSession_Id.New,
                                                            ProviderId: response.Content.RoamingAuthorisationInfo != null
                                                                            ? response.Content.RoamingAuthorisationInfo.ContractId.ProviderId.ToWWCP()
                                                                            : eMobilityProvider_Id.Parse(Country.Germany, "GEF"),
                                                            Runtime:    Runtime);

                }

                else
                    result = AuthStartEVSEResult.NotAuthorized(Id,
                                                               this,
                                                               // response.Content.ProviderId.ToWWCP(),
                                                               Runtime: Runtime);

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
                                                     AuthIdentification,
                                                     EVSEId,
                                                     ChargingProduct,
                                                     SessionId,
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

        #region AuthorizeStart(AuthIdentification, ChargingStationId, ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging station.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
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

            AuthorizeStart(AuthIdentification           AuthIdentification,
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

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingStationStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeChargingStationStartRequest?.Invoke(StartTime,
                                                               Timestamp.Value,
                                                               this,
                                                               Id.ToString(),
                                                               EventTrackingId,
                                                               RoamingNetwork.Id,
                                                               OperatorId,
                                                               AuthIdentification,
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
                                                                              AuthIdentification.AuthToken.ToString(),
                                                                              TokenRepresentations.Plain,
                                                                              TokenTypes.RFID
                                                                          ),

                                                                          Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          RequestTimeout).ConfigureAwait(false);


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

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
                                                                AuthIdentification,
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

        #region AuthorizeStart(AuthIdentification, ChargingPoolId,    ChargingProduct = null, SessionId = null, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize start request at the given charging pool.
        /// </summary>
        /// <param name="AuthIdentification">An user identification.</param>
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

            AuthorizeStart(AuthIdentification           AuthIdentification,
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

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingPoolStartRequest event

            var StartTime = DateTime.Now;

            try
            {

                OnAuthorizeChargingPoolStartRequest?.Invoke(StartTime,
                                                            Timestamp.Value,
                                                            this,
                                                            Id.ToString(),
                                                            EventTrackingId,
                                                            RoamingNetwork.Id,
                                                            OperatorId,
                                                            AuthIdentification,
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
                                                                              AuthIdentification.AuthToken.ToString(),
                                                                              TokenRepresentations.Plain,
                                                                              TokenTypes.RFID
                                                                          ),

                                                                          Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          RequestTimeout).ConfigureAwait(false);


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

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
                                                             AuthIdentification,
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

        #region AuthorizeStop(SessionId, AuthIdentification,                    OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification),  "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeStopRequest event

            var StartTime = DateTime.Now;

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
                                               AuthIdentification,
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
                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;
                result   = AuthStopResult.OutOfService(Id, SessionId, Runtime);
            }

            else
            {

                var response = await CPORoaming.GetSingleRoamingAuthorisation(new EMT_Id(
                                                                                  AuthIdentification.AuthToken.ToString(),
                                                                                  TokenRepresentations.Plain,
                                                                                  TokenTypes.RFID
                                                                              ),

                                                                              Timestamp,
                                                                              CancellationToken,
                                                                              EventTrackingId,
                                                                              RequestTimeout).ConfigureAwait(false);


                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode            == HTTPStatusCode.OK &&
                    response.Content                   != null              &&
                    response.Content.Result.ResultCode == ResultCodes.OK)
                {

                    result = AuthStopResult.Authorized(Id,
                                                       ChargingSession_Id.New,
                                                       ProviderId:  response.Content.RoamingAuthorisationInfo.ContractId.ProviderId.ToWWCP(),
                                                       Runtime:     Runtime);

                }

                else
                    result = AuthStopResult.NotAuthorized(Id,
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
                                                AuthIdentification,
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

        #region AuthorizeStop(SessionId, AuthIdentification, EVSEId,            OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given EVSE.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopEVSEResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
                          WWCP.EVSE_Id                 EVSEId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)
        {

            #region Initial checks

            if (AuthIdentification  == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeEVSEStopRequest event

            var StartTime = DateTime.Now;

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
                                                   AuthIdentification,
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
                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;
                result   = AuthStopEVSEResult.OutOfService(Id, SessionId, Runtime);
            }

            else
            {

                var response = await CPORoaming.GetSingleRoamingAuthorisation(new EMT_Id(
                                                                                  AuthIdentification.AuthToken.ToString(),
                                                                                  TokenRepresentations.Plain,
                                                                                  TokenTypes.RFID
                                                                              ),

                                                                              Timestamp,
                                                                              CancellationToken,
                                                                              EventTrackingId,
                                                                              RequestTimeout).ConfigureAwait(false);


                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode            == HTTPStatusCode.OK &&
                    response.Content                   != null              &&
                    response.Content.Result.ResultCode == ResultCodes.OK)
                {

                    result = AuthStopEVSEResult.Authorized(Id,
                                                           ChargingSession_Id.New,
                                                           ProviderId:  response.Content.RoamingAuthorisationInfo.ContractId.ProviderId.ToWWCP(),
                                                           Runtime:     Runtime);

                }

                else
                    result = AuthStopEVSEResult.NotAuthorized(Id,
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
                                                    AuthIdentification,
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

        #region AuthorizeStop(SessionId, AuthIdentification, ChargingStationId, OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging station.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingStationId">The unique identification of a charging station.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingStationResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
                          ChargingStation_Id           ChargingStationId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingStationStopRequest event

            var StartTime = DateTime.Now;

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
                                                              AuthIdentification,
                                                              RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingStationStopRequest));
            }

            #endregion


            var response = await CPORoaming.GetSingleRoamingAuthorisation(new EMT_Id(
                                                                              AuthIdentification.AuthToken.ToString(),
                                                                              TokenRepresentations.Plain,
                                                                              TokenTypes.RFID
                                                                          ),

                                                                          Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          RequestTimeout).ConfigureAwait(false);


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

            AuthStopChargingStationResult result = null;

            if (response.HTTPStatusCode            == HTTPStatusCode.OK &&
                response.Content                   != null              &&
                response.Content.Result.ResultCode == ResultCodes.OK)
            {

                result = AuthStopChargingStationResult.Authorized(Id,
                                                                  ChargingSession_Id.New,
                                                                  ProviderId:  response.Content.RoamingAuthorisationInfo.ContractId.ProviderId.ToWWCP(),
                                                                  Runtime:     Runtime);

            }

            else
                result = AuthStopChargingStationResult.NotAuthorized(Id,
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
                                                               AuthIdentification,
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

        #region AuthorizeStop(SessionId, AuthIdentification, ChargingPoolId,    OperatorId = null, ...)

        /// <summary>
        /// Create an authorize stop request at the given charging pool.
        /// </summary>
        /// <param name="SessionId">The session identification from the AuthorizeStart request.</param>
        /// <param name="AuthIdentification">An user identification.</param>
        /// <param name="ChargingPoolId">The unique identification of a charging pool.</param>
        /// <param name="OperatorId">An optional charging station operator identification.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<AuthStopChargingPoolResult>

            AuthorizeStop(ChargingSession_Id           SessionId,
                          AuthIdentification           AuthIdentification,
                          ChargingPool_Id              ChargingPoolId,
                          ChargingStationOperator_Id?  OperatorId          = null,

                          DateTime?                    Timestamp           = null,
                          CancellationToken?           CancellationToken   = null,
                          EventTracking_Id             EventTrackingId     = null,
                          TimeSpan?                    RequestTimeout      = null)

        {

            #region Initial checks

            if (AuthIdentification == null)
                throw new ArgumentNullException(nameof(AuthIdentification), "The given authentication token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Send OnAuthorizeChargingPoolStopRequest event

            var StartTime = DateTime.Now;

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
                                                           AuthIdentification,
                                                           RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnAuthorizeChargingPoolStopRequest));
            }

            #endregion


            var response = await CPORoaming.GetSingleRoamingAuthorisation(new EMT_Id(
                                                                              AuthIdentification.AuthToken.ToString(),
                                                                              TokenRepresentations.Plain,
                                                                              TokenTypes.RFID
                                                                          ),

                                                                          Timestamp,
                                                                          CancellationToken,
                                                                          EventTrackingId,
                                                                          RequestTimeout).ConfigureAwait(false);


            var Endtime = DateTime.Now;
            var Runtime = Endtime - StartTime;

            AuthStopChargingPoolResult result = null;

            if (response.HTTPStatusCode            == HTTPStatusCode.OK &&
                response.Content                   != null              &&
                response.Content.Result.ResultCode == ResultCodes.OK)
            {

                result = AuthStopChargingPoolResult.Authorized(Id,
                                                               ChargingSession_Id.New,
                                                               ProviderId:  response.Content.RoamingAuthorisationInfo.ContractId.ProviderId.ToWWCP(),
                                                               Runtime:     Runtime);

            }

            else
                result = AuthStopChargingPoolResult.NotAuthorized(Id,
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
                                                            AuthIdentification,
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

        #region SendChargeDetailRecords(ChargeDetailRecords, TransmissionType = Enqueued, ...)

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
                                    TransmissionTypes                TransmissionType    = TransmissionTypes.Enqueued,

                                    DateTime?                        Timestamp           = null,
                                    CancellationToken?               CancellationToken   = null,
                                    EventTracking_Id                 EventTrackingId     = null,
                                    TimeSpan?                        RequestTimeout      = null)

        {

            #region Initial checks

            if (ChargeDetailRecords == null)
                throw new ArgumentNullException(nameof(ChargeDetailRecords),  "The given enumeration of charge detail records must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = CPOClient?.RequestTimeout;

            #endregion

            #region Enqueue, if requested...

            if (TransmissionType == TransmissionTypes.Enqueued)
            {

                #region Send OnEnqueueSendCDRsRequest event

                try
                {

                    OnEnqueueSendCDRsRequest?.Invoke(DateTime.Now,
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
                    e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsRequest));
                }

                #endregion

                lock (ServiceCheckLock)
                {

                    ChargeDetailRecordQueue.AddRange(ChargeDetailRecords);

                    ServiceCheckTimer.Change(_ServiceCheckEvery, Timeout.Infinite);

                }

                return SendCDRsResult.Enqueued(Id);

            }

            #endregion

            #region Send OnSendCDRsRequest event

            var StartTime = DateTime.Now;

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
                e.Log(nameof(WWCPCPOAdapter) + "." + nameof(OnSendCDRsRequest));
            }

            #endregion


            DateTime        Endtime;
            TimeSpan        Runtime;
            SendCDRsResult  result;

            if (DisableSendChargeDetailRecords)
            {
                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;
                result   = SendCDRsResult.OutOfService(Id, Runtime: Runtime);
            }

            else
            {

                var response = await CPORoaming.AddCDRs(ChargeDetailRecords.Select(cdr => cdr.ToOCHP()).ToArray(),

                                                        Timestamp,
                                                        CancellationToken,
                                                        EventTrackingId,
                                                        RequestTimeout).ConfigureAwait(false);


                Endtime  = DateTime.Now;
                Runtime  = Endtime - StartTime;

                if (response.HTTPStatusCode == HTTPStatusCode.OK &&
                    response.Content        != null)
                {

                    switch (response.Content.Result.ResultCode)
                    {

                        case ResultCodes.OK:
                            result = SendCDRsResult.Forwarded(Id);
                            break;

                        case ResultCodes.Partly:
                            result = SendCDRsResult.Partly(Id,
                                                           response.Content.ImplausibleCDRs.Select(cdr => cdr.ToWWCP()));
                            break;

                        default:
                            result = SendCDRsResult.Error(Id,
                                                          response.Content.ImplausibleCDRs.Select(cdr => cdr.ToWWCP()));
                            break;

                    }

                }

                else
                    result = SendCDRsResult.NotForwared(Id,
                                                        ChargeDetailRecords,
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
                                           ChargeDetailRecords,
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

                    OnWWCPCPOAdapterException?.Invoke(DateTime.Now,
                                                      this,
                                                      e);

                }

            }

        }

        public async Task FlushServiceQueues()
        {

            DebugX.Log("ServiceCheck, as every " + _ServiceCheckEvery + "ms!");

            #region Make a thread local copy of all data

            //ToDo: AsyncLocal is currently not implemented in Mono!
            //var EVSEDataQueueCopy   = new AsyncLocal<HashSet<EVSE>>();
            //var EVSEStatusQueueCopy = new AsyncLocal<List<EVSEStatusChange>>();

            var EVSEsToAddQueueCopy          = new ThreadLocal<HashSet<EVSE>>();
            var EVSEDataQueueCopy            = new ThreadLocal<HashSet<EVSE>>();
            var EVSEStatusQueueCopy          = new ThreadLocal<List<EVSEStatusUpdate>>();
            var EVSEsToRemoveQueueCopy       = new ThreadLocal<HashSet<EVSE>>();
            var ChargeDetailRecordQueueCopy  = new ThreadLocal<List<WWCP.ChargeDetailRecord>>();

            if (Monitor.TryEnter(ServiceCheckLock))
            {

                try
                {

                    if (EVSEsToAddQueue.               Count == 0 &&
                        EVSEsToUpdateQueue.          Count == 0 &&
                        EVSEStatusChangesDelayedQueue. Count == 0 &&
                        EVSEsToRemoveQueue.            Count == 0 &&
                        ChargeDetailRecordQueue.       Count == 0)
                    {
                        return;
                    }

                    _ServiceRunId++;

                    // Copy 'EVSEs to add', remove originals...
                    EVSEsToAddQueueCopy.Value          = new HashSet<EVSE>           (EVSEsToAddQueue);
                    EVSEsToAddQueue.Clear();

                    // Copy 'EVSEs to update', remove originals...
                    EVSEDataQueueCopy.Value            = new HashSet<EVSE>           (EVSEsToUpdateQueue);
                    EVSEsToUpdateQueue.Clear();

                    // Copy 'EVSE status changes', remove originals...
                    EVSEStatusQueueCopy.Value          = new List<EVSEStatusUpdate>  (EVSEStatusChangesDelayedQueue);
                    EVSEStatusChangesDelayedQueue.Clear();

                    // Copy 'EVSEs to remove', remove originals...
                    EVSEsToRemoveQueueCopy.Value       = new HashSet<EVSE>           (EVSEsToRemoveQueue);
                    EVSEsToRemoveQueue.Clear();

                    // Copy 'EVSEs to remove', remove originals...
                    ChargeDetailRecordQueueCopy.Value  = new List<WWCP.ChargeDetailRecord>(ChargeDetailRecordQueue);
                    ChargeDetailRecordQueue.Clear();

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
            if (EVSEsToAddQueueCopy.        Value != null ||
                EVSEDataQueueCopy.          Value != null ||
                EVSEStatusQueueCopy.        Value != null ||
                EVSEsToRemoveQueueCopy.     Value != null ||
                ChargeDetailRecordQueueCopy.Value != null)
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

                if (EVSEStatusQueueCopy.Value.Count > 0)
                {

                    var UpdateEVSEStatusTask = UpdateEVSEStatus(EVSEStatusQueueCopy.Value,
                                                                EventTrackingId: EventTrackingId);

                    UpdateEVSEStatusTask.Wait();

                }

                #endregion

                #region Send charge detail records

                if (ChargeDetailRecordQueueCopy.Value.Count > 0)
                {

                    var SendCDRResults   = await SendChargeDetailRecords(ChargeDetailRecordQueueCopy.Value,
                                                                         TransmissionTypes.Direct,
                                                                         DateTime.Now,
                                                                         new CancellationTokenSource().Token,
                                                                         EventTrackingId,
                                                                         DefaultRequestTimeout).ConfigureAwait(false);

                    //ToDo: Send results events...
                    //ToDo: Read to queue if it could not be sent...

                }

                #endregion

                //ToDo: Send removed EVSE data!

            }

            return;

        }

        #endregion

        #region (timer) FlushEVSEStatusUpdates(State)

        private void FlushEVSEStatusUpdates(Object State)
        {

            if (!DisablePushStatus)
            {

                FlushEVSEStatusUpdateQueues().Wait();

                //ToDo: Handle errors!

            }

        }

        public async Task FlushEVSEStatusUpdateQueues()
        {

            DebugX.Log("Flush EVSE status updates, as every " + _FlushEVSEStatusUpdatesEvery + "ms!");

            #region Make a thread local copy of all data

            //ToDo: AsyncLocal is currently not implemented in Mono!
            //var EVSEStatusQueueCopy = new AsyncLocal<List<EVSEStatusChange>>();

            var EVSEStatusFastQueueCopy = new ThreadLocal<List<EVSEStatusUpdate>>();

            if (Monitor.TryEnter(ServiceCheckLock,
                                 TimeSpan.FromMinutes(5)))
            {

                try
                {

                    if (EVSEStatusChangesFastQueue.Count == 0)
                        return;

                    _StatusRunId++;

                    // Copy 'EVSE status changes', remove originals...
                    EVSEStatusFastQueueCopy.Value = new List<EVSEStatusUpdate>(EVSEStatusChangesFastQueue.Where(evsestatuschange => !EVSEsToAddQueue.Any(evse => evse == evsestatuschange.EVSE)));

                    // Add all evse status changes of EVSE *NOT YET UPLOADED* into the delayed queue...
                    EVSEStatusChangesDelayedQueue.AddRange(EVSEStatusChangesFastQueue.Where(evsestatuschange => EVSEsToAddQueue.Any(evse => evse == evsestatuschange.EVSE)));

                    EVSEStatusChangesFastQueue.Clear();

                    // Stop the timer. Will be rescheduled by next EVSE status change...
                    StatusCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);

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

                Console.WriteLine("StatusCheckLock missed!");
                StatusCheckTimer.Change(_FlushEVSEStatusUpdatesEvery, Timeout.Infinite);

            }

            #endregion

            // Upload status changes...
            if (EVSEStatusFastQueueCopy.Value != null)
            {

                var EventTrackingId = EventTracking_Id.New;

                // Use the events to evaluate if something went wrong!

                #region Send changed EVSE status

                if (EVSEStatusFastQueueCopy.Value.Count > 0)
                {

                    var UpdateEVSEStatusTask = UpdateEVSEStatus(EVSEStatusFastQueueCopy.Value,
                                                                EventTrackingId: EventTrackingId);

                    UpdateEVSEStatusTask.Wait();

                }

                #endregion

            }

            return;

        }

        #endregion

        #region (timer) EVSEStatusRefresh(State)

        private void EVSEStatusRefresh(Object State)
        {

            if (!DisablePushStatus && !DisableEVSEStatusRefresh)
            {

                RefreshEVSEStatus().Wait();

                //ToDo: Handle errors!

            }

        }

        private async Task<Acknowledgement> RefreshEVSEStatus()
        {

            #region Try to acquire the EVSE status refresh lock, or return...

            if (!EVSEStatusRefreshLock.Wait(0))
            {
                DebugX.Log("Could not acquire EVSE status refresh lock!");
                return new Acknowledgement(ResultType.NoOperation);
            }

            #endregion


            EVSEStatusRefreshEvent?.Invoke(DateTime.UtcNow,
                                           this,
                                           "EVSE status refresh, as every " + EVSEStatusRefreshEvery.TotalHours.ToString() + " hours!");

            #region Data

            EVSE_Id         EVSEId;
            Acknowledgement result = null;

            var StartTime                  = DateTime.UtcNow;
            var Warnings                   = new List<String>();
            var AllEVSEStatusRefreshments  = new List<EVSEStatus>();

            #endregion

            try
            {

                #region Fetch EVSE status

                foreach (var EVSEStatusHistory in RoamingNetwork.EVSEStatus(1))
                {

                    try
                    {

                        EVSEId = _CustomEVSEIdMapper != null
                                     ? _CustomEVSEIdMapper(EVSEStatusHistory.Key)
                                     : EVSEStatusHistory.Key.ToOCHP();

                        if (IncludeEVSEIds(EVSEId))
                            AllEVSEStatusRefreshments.Add(new EVSEStatus(EVSEId,
                                                                        EVSEStatusHistory.Value.First().Value.AsEVSEMajorStatus(),
                                                                        EVSEStatusHistory.Value.First().Value.AsEVSEMinorStatus()));

                    }
                    catch (Exception e)
                    {
                        DebugX.  Log(e.Message);
                        Warnings.Add(e.Message);
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
                            result = new Acknowledgement(ResultType.True,
                                                         response.Content.Result.Description,
                                                         Warnings,
                                                         Runtime);

                        else
                            result = new Acknowledgement(ResultType.False,
                                                         response.Content.Result.Description,
                                                         Warnings,
                                                         Runtime);

                    }
                    else
                        result = new Acknowledgement(ResultType.False,
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

                result = new Acknowledgement(ResultType.False,
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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => "OCHP " + Version.Number + " CPO Adapter " + Id;

        #endregion


    }

}
