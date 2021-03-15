/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// A WWCP wrapper for the OCHP EMP roaming client which maps
    /// WWCP data structures onto OCHP data structures and vice versa.
    /// </summary>
    public class WWCPCPOAdapter : ACryptoEMobilityEntity<CSORoamingProvider_Id>,
                                  ICSORoamingProvider,
                                  ISendAuthenticationData
    {

        #region Data

        //private readonly EVSEDataRecord2EVSEDelegate _EVSEDataRecord2EVSE;

        /// <summary>
        ///  The default reservation time.
        /// </summary>
        public static readonly TimeSpan DefaultReservationTime = TimeSpan.FromMinutes(15);


        private readonly        Object         PullDataServiceLock;
        private readonly        Timer          PullDataServiceTimer;

        /// <summary>
        /// The default status check intervall.
        /// </summary>
        public  readonly static TimeSpan       DefaultPullDataServiceEvery              = TimeSpan.FromMinutes(15);

        public  readonly static TimeSpan       DefaultPullDataServiceRequestTimeout     = TimeSpan.FromMinutes(30);


        private readonly        Object         PullStatusServiceLock;
        private readonly        Timer          PullStatusServiceTimer;

        /// <summary>
        /// The default status check intervall.
        /// </summary>
        public  readonly static TimeSpan       DefaultPullStatusServiceEvery            = TimeSpan.FromMinutes(1);

        public  readonly static TimeSpan       DefaultPullStatusServiceRequestTimeout   = TimeSpan.FromMinutes(3);

        #endregion

        #region Properties

        /// <summary>
        /// The wrapped EMP roaming object.
        /// </summary>
        public EMPRoaming    EMPRoaming           { get; }

        /// <summary>
        /// An optional default e-mobility provider identification.
        /// </summary>
        public Provider_Id?  DefaultProviderId    { get; }

        //public EVSEOperatorFilterDelegate EVSEOperatorFilter;


        #region OnWWCPEMPAdapterException

        public delegate Task OnWWCPEMPAdapterExceptionDelegate(DateTime        Timestamp,
                                                               WWCPCPOAdapter  Sender,
                                                               Exception       Exception);

        public event OnWWCPEMPAdapterExceptionDelegate OnWWCPEMPAdapterException;

        #endregion

        #region PullDataService

        public Boolean  DisablePullPOIData { get; set; }

        private UInt32 _PullDataServiceEvery;

        public TimeSpan PullDataServiceRequestTimeout { get; }

        /// <summary>
        /// The 'Pull Status' service intervall.
        /// </summary>
        public TimeSpan PullDataServiceEvery
        {

            get
            {
                return TimeSpan.FromSeconds(_PullDataServiceEvery);
            }

            set
            {
                _PullDataServiceEvery = (UInt32) value.TotalSeconds;
            }

        }

        public DateTime? TimestampOfLastPullDataRun { get; }

        private static SemaphoreSlim PullEVSEDataLock = new SemaphoreSlim(1, 1);

        public delegate void PullEVSEDataDelegate(DateTime Timestamp, WWCPCPOAdapter Sender, TimeSpan Every);

        public event PullEVSEDataDelegate FlushServiceQueuesEvent;

        #endregion

        #region PullStatusService

        public Boolean  DisablePullStatus { get; set; }

        private UInt32 _PullStatusServiceEvery;

        public TimeSpan PullStatusServiceRequestTimeout { get; }

        /// <summary>
        /// The 'Pull Status' service intervall.
        /// </summary>
        public TimeSpan PullStatusServiceEvery
        {

            get
            {
                return TimeSpan.FromSeconds(_PullStatusServiceEvery);
            }

            set
            {
                _PullStatusServiceEvery = (UInt32) value.TotalSeconds;
            }

        }

        #endregion

        public GeoCoordinate?  DefaultSearchCenter    { get; }
        public UInt64?         DefaultDistanceKM      { get; }

        public Func<EVSEStatusReport, ChargingStationStatusTypes>  EVSEStatusAggregationDelegate { get; }

        public IEnumerable<ChargingReservation>                    ChargingReservations  => throw new NotImplementedException();

        public IEnumerable<ChargingSession>                        ChargingSessions      => throw new NotImplementedException();

        #endregion

        #region Events

        // Client methods (logging)

        #region OnPullEVSEDataRequest/-Response (OCHP event!)

        ///// <summary>
        ///// An event sent whenever a 'pull EVSE data' request will be send.
        ///// </summary>
        //public event OnPullEVSEDataRequestHandler        OnPullEVSEDataRequest;

        ///// <summary>
        ///// An event sent whenever a response for a 'pull EVSE data' request had been received.
        ///// </summary>
        //public event OnPullEVSEDataResponseHandler       OnPullEVSEDataResponse;

        #endregion

        #region OnPullEVSEStatusRequest/-Response (OCHP event!)

        ///// <summary>
        ///// An event sent whenever a 'pull EVSE status' request will be send.
        ///// </summary>
        //public event OnPullEVSEStatusRequestHandler      OnPullEVSEStatusRequest;

        ///// <summary>
        ///// An event sent whenever a response for a 'pull EVSE status' request had been received.
        ///// </summary>
        //public event OnPullEVSEStatusResponseHandler     OnPullEVSEStatusResponse;

        #endregion


        #region OnReserveEVSERequest/-Response

        /// <summary>
        /// An event sent whenever a reserve EVSE command will be send.
        /// </summary>
        public event OnReserveRequestDelegate         OnReserveEVSERequest;

        /// <summary>
        /// An event sent whenever a reserve EVSE command was sent.
        /// </summary>
        public event OnReserveResponseDelegate        OnReserveEVSEResponse;

        #endregion

        #region OnCancelReservationRequest/-Response

        /// <summary>
        /// An event sent whenever a cancel reservation command will be send.
        /// </summary>
        public event OnCancelReservationRequestDelegate   OnCancelReservationRequest;

        /// <summary>
        /// An event sent whenever a cancel reservation command was sent.
        /// </summary>
        public event OnCancelReservationResponseDelegate  OnCancelReservationResponse;

        #endregion

        #region OnRemoteStart/StopRequest/-Response

        /// <summary>
        /// An event fired whenever a remote start command was received.
        /// </summary>
        public event OnRemoteStartRequestDelegate     OnRemoteStartRequest;

        /// <summary>
        /// An event fired whenever a remote start command completed.
        /// </summary>
        public event OnRemoteStartResponseDelegate    OnRemoteStartResponse;


        /// <summary>
        /// An event fired whenever a remote stop command was received.
        /// </summary>
        public event OnRemoteStopRequestDelegate      OnRemoteStopRequest;

        /// <summary>
        /// An event fired whenever a remote stop command completed.
        /// </summary>
        public event OnRemoteStopResponseDelegate     OnRemoteStopResponse;

        #endregion


        #region OnGetChargeDetailRecordsRequest/-Response

        ///// <summary>
        ///// An event sent whenever a 'get charge detail records' request will be send.
        ///// </summary>
        //public event OnGetCDRsRequestDelegate    OnGetChargeDetailRecordsRequest;

        ///// <summary>
        ///// An event sent whenever a response to a 'get charge detail records' request was received.
        ///// </summary>
        //public event OnGetCDRsResponseDelegate   OnGetChargeDetailRecordsResponse;

        #endregion


        // Server methods (logging)

        #region OnAuthorizeStartRequest/-Response

        /// <summary>
        /// An event sent whenever an 'authorize start' request was received.
        /// </summary>
        public event WWCP.OnAuthorizeStartRequestDelegate   OnAuthorizeStartRequest;

        /// <summary>
        /// An event sent whenever a response to an 'authorize start' request was sent.
        /// </summary>
        public event WWCP.OnAuthorizeStartResponseDelegate  OnAuthorizeStartResponse;

        #endregion

        #region OnAuthorizeEVSEStartRequest/-Response

        /// <summary>
        /// An event sent whenever an 'authorize EVSE start' request was received.
        /// </summary>
        public event OnAuthorizeStartRequestDelegate   OnAuthorizeEVSEStartRequest;

        /// <summary>
        /// An event sent whenever a response to an 'authorize EVSE start' request was sent.
        /// </summary>
        public event OnAuthorizeStartResponseDelegate  OnAuthorizeEVSEStartResponse;

        #endregion

        #region OnAuthorizeStopRequest/-Response

        /// <summary>
        /// An event sent whenever an 'authorize stop' request was received.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate   OnAuthorizeStopRequest;

        /// <summary>
        /// An event sent whenever a response to an 'authorize stop' request was sent.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate  OnAuthorizeStopResponse;

        #endregion

        #region OnAuthorizeEVSEStopRequest/-Response

        /// <summary>
        /// An event sent whenever an 'authorize EVSE stop' request was received.
        /// </summary>
        public event OnAuthorizeStopRequestDelegate   OnAuthorizeEVSEStopRequest;

        /// <summary>
        /// An event sent whenever a response to an 'authorize EVSE stop' request was sent.
        /// </summary>
        public event OnAuthorizeStopResponseDelegate  OnAuthorizeEVSEStopResponse;

        #endregion

        #region OnChargeDetailRecordRequest/-Response

        /// <summary>
        /// An event sent whenever a 'charge detail record' was received.
        /// </summary>
        public event OnSendCDRsRequestDelegate   OnChargeDetailRecordRequest;

        /// <summary>
        /// An event sent whenever a response to a 'charge detail record' was sent.
        /// </summary>
        public event OnSendCDRsResponseDelegate  OnChargeDetailRecordResponse;
        public event WWCP.OnGetCDRsRequestDelegate OnGetChargeDetailRecordsRequest;
        public event WWCP.OnGetCDRsResponseDelegate OnGetChargeDetailRecordsResponse;
        public event OnReserveRequestDelegate OnReserveRequest;
        public event OnReserveResponseDelegate OnReserveResponse;
        public event OnNewReservationDelegate OnNewReservation;
        public event OnReservationCanceledDelegate OnReservationCanceled;
        public event OnNewChargingSessionDelegate OnNewChargingSession;
        public event OnNewChargeDetailRecordDelegate OnNewChargeDetailRecord;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new WWCP wrapper for the OCHP EMP Roaming client for e-mobility providers/EMPs.
        /// </summary>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// 
        /// <param name="EMPRoaming">A OCHP EMP roaming object to be mapped to WWCP.</param>
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record after receiving it from the roaming provider.</param>
        public WWCPCPOAdapter(CSORoamingProvider_Id        Id,
                              I18NString                   Name,
                              RoamingNetwork               RoamingNetwork,
                              EMPRoaming                   EMPRoaming,
                              //EVSEDataRecord2EVSEDelegate  EVSEDataRecord2EVSE               = null,

                              //EVSEOperatorFilterDelegate   EVSEOperatorFilter                = null,

                              TimeSpan?                    PullDataServiceEvery              = null,
                              Boolean                      DisablePullData                   = false,
                              TimeSpan?                    PullDataServiceRequestTimeout     = null,

                              TimeSpan?                    PullStatusServiceEvery            = null,
                              Boolean                      DisablePullStatus                 = false,
                              TimeSpan?                    PullStatusServiceRequestTimeout   = null,

                              eMobilityProvider            DefaultProvider                   = null,
                              GeoCoordinate?               DefaultSearchCenter               = null,
                              UInt64?                      DefaultDistanceKM                 = null)

            : base(Id,
                   Name,
                   RoamingNetwork)

        {

            #region Initial checks

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),        "The given roaming provider name must not be null or empty!");

            if (EMPRoaming == null)
                throw new ArgumentNullException(nameof(EMPRoaming),  "The given OCHP EMP Roaming object must not be null!");

            #endregion

            this.EMPRoaming                       = EMPRoaming;
            //this._EVSEDataRecord2EVSE             = EVSEDataRecord2EVSE;

            //this.EVSEOperatorFilter               = EVSEOperatorFilter ?? ((name, id) => true);

            this._PullDataServiceEvery            = (UInt32) (PullDataServiceEvery.HasValue
                                                                  ? PullDataServiceEvery.Value.  TotalMilliseconds
                                                                  : DefaultPullDataServiceEvery. TotalMilliseconds);
            this.PullDataServiceRequestTimeout    = PullDataServiceRequestTimeout ?? DefaultPullDataServiceRequestTimeout;
            this.PullDataServiceLock              = new Object();
            this.PullDataServiceTimer             = new Timer(PullDataService, null, 5000, _PullDataServiceEvery);
            this.DisablePullPOIData                  = DisablePullData;


            this._PullStatusServiceEvery          = (UInt32) (PullStatusServiceEvery.HasValue
                                                                  ? PullStatusServiceEvery.Value.  TotalMilliseconds
                                                                  : DefaultPullStatusServiceEvery. TotalMilliseconds);
            this.PullStatusServiceRequestTimeout  = PullStatusServiceRequestTimeout ?? DefaultPullStatusServiceRequestTimeout;
            this.PullStatusServiceLock            = new Object();
            this.PullStatusServiceTimer           = new Timer(PullStatusService, null, 150000, _PullStatusServiceEvery);
            this.DisablePullStatus                = DisablePullStatus;

            this.DefaultProviderId                = DefaultProvider != null
                                                        ? new Provider_Id?(DefaultProvider.Id.ToOCHP())
                                                        : null;
            this.DefaultSearchCenter              = DefaultSearchCenter;
            this.DefaultDistanceKM                = DefaultDistanceKM;


            // Link events...

            #region OnAuthorizeStart

            //this.EMPRoaming.OnAuthorizeStart += async (Timestamp,
            //                                           Sender,
            //                                           Request) => {

            //    #region Map parameter values

            //    var OperatorId      = Request.OperatorId.    ToWWCP();
            //    var Identification  = Request.Identification.ToWWCP();
            //    var EVSEId          = Request.EVSEId?.       ToWWCP();
            //    var ProductId       = Request.PartnerProductId.HasValue
            //                              ? new ChargingProduct(Request.PartnerProductId.Value.ToWWCP())
            //                              : null;
            //    var SessionId       = Request.SessionId.     ToWWCP();

            //    #endregion

            //    if (EVSEId.HasValue)
            //    {

            //        #region Send OnAuthorizeEVSEStartRequest event

            //        var StartTime = DateTime.UtcNow;

            //        try
            //        {

            //            OnAuthorizeEVSEStartRequest?.Invoke(StartTime,
            //                                                Timestamp,
            //                                                this,
            //                                                Id.ToString(),
            //                                                Request.EventTrackingId,
            //                                                RoamingNetwork.Id,
            //                                                OperatorId,
            //                                                Identification,
            //                                                EVSEId.Value,
            //                                                ProductId,
            //                                                SessionId,
            //                                                new ISendAuthorizeStartStop[0],
            //                                                Request.RequestTimeout);

            //        }
            //        catch (Exception e)
            //        {
            //            e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeEVSEStartRequest));
            //        }

            //        #endregion

            //        var response = await RoamingNetwork.AuthorizeStart(Identification,
            //                                                           EVSEId.Value,
            //                                                           ProductId,
            //                                                           SessionId,
            //                                                           OperatorId,

            //                                                           Timestamp,
            //                                                           Request.CancellationToken,
            //                                                           Request.EventTrackingId,
            //                                                           Request.RequestTimeout);

            //        #region Send OnAuthorizeEVSEStartResponse event

            //        var EndTime = DateTime.UtcNow;

            //        try
            //        {

            //            OnAuthorizeEVSEStartResponse?.Invoke(EndTime,
            //                                                 Timestamp,
            //                                                 this,
            //                                                 Id.ToString(),
            //                                                 Request.EventTrackingId,
            //                                                 RoamingNetwork.Id,
            //                                                 OperatorId,
            //                                                 Identification,
            //                                                 EVSEId.Value,
            //                                                 ProductId,
            //                                                 SessionId,
            //                                                 new ISendAuthorizeStartStop[0],
            //                                                 Request.RequestTimeout,
            //                                                 response,
            //                                                 EndTime - StartTime);

            //        }
            //        catch (Exception e)
            //        {
            //            e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeEVSEStartResponse));
            //        }

            //        #endregion

            //        #region Map response

            //        if (response != null)
            //        {
            //            switch (response.Result)
            //            {

            //                case AuthStartEVSEResultType.Authorized:
            //                    return CPO.AuthorizationStart.Authorized(Request,
            //                                                             response.SessionId. HasValue ? response.SessionId. Value.ToOCHP() : default(Session_Id?),
            //                                                             default(PartnerSession_Id?),
            //                                                             DefaultProviderId,//    response.ProviderId.HasValue ? response.ProviderId.Value.ToOCHP() : default(Provider_Id?),
            //                                                             "Ready to charge!",
            //                                                             null,
            //                                                             response.ListOfAuthStopTokens.
            //                                                                 SafeSelect(token => OCHPv1_4.Identification.FromUID(token.ToOCHP()))
            //                                                            );

            //                case AuthStartEVSEResultType.NotAuthorized:
            //                    return CPO.AuthorizationStart.NotAuthorized(Request,
            //                                                                StatusCodes.RFIDAuthenticationfailed_InvalidUID,
            //                                                                "RFID Authentication failed - invalid UID");

            //                case AuthStartEVSEResultType.InvalidSessionId:
            //                    return CPO.AuthorizationStart.SessionIsInvalid(Request,
            //                                                                   SessionId:         Request.SessionId,
            //                                                                   PartnerSessionId:  Request.PartnerSessionId);

            //                case AuthStartEVSEResultType.CommunicationTimeout:
            //                    return CPO.AuthorizationStart.CommunicationToEVSEFailed(Request);

            //                case AuthStartEVSEResultType.StartChargingTimeout:
            //                    return CPO.AuthorizationStart.NoEVConnectedToEVSE(Request);

            //                case AuthStartEVSEResultType.Reserved:
            //                    return CPO.AuthorizationStart.EVSEAlreadyReserved(Request);

            //                case AuthStartEVSEResultType.UnknownEVSE:
            //                    return CPO.AuthorizationStart.UnknownEVSEID(Request);

            //                case AuthStartEVSEResultType.OutOfService:
            //                    return CPO.AuthorizationStart.EVSEOutOfService(Request);

            //            }
            //        }

            //        #endregion

            //        return CPO.AuthorizationStart.ServiceNotAvailable(
            //                   Request,
            //                   SessionId:  response?.SessionId. ToOCHP() ?? Request.SessionId,
            //                   ProviderId: response?.ProviderId.ToOCHP()
            //               );

            //    }

            //    else
            //    {

            //        #region Send OnAuthorizeStartRequest event

            //        var StartTime = DateTime.UtcNow;

            //        try
            //        {

            //            OnAuthorizeStartRequest?.Invoke(StartTime,
            //                                            Timestamp,
            //                                            this,
            //                                            Id.ToString(),
            //                                            Request.EventTrackingId,
            //                                            RoamingNetwork.Id,
            //                                            OperatorId,
            //                                            Identification,
            //                                            ProductId,
            //                                            SessionId,
            //                                            Request.RequestTimeout);

            //        }
            //        catch (Exception e)
            //        {
            //            e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStartRequest));
            //        }

            //        #endregion

            //        var response = await RoamingNetwork.AuthorizeStart(Identification,
            //                                                           ProductId,
            //                                                           SessionId,
            //                                                           OperatorId,

            //                                                           Timestamp,
            //                                                           Request.CancellationToken,
            //                                                           Request.EventTrackingId,
            //                                                           Request.RequestTimeout);


            //        #region Send OnAuthorizeStartResponse event

            //        var EndTime = DateTime.UtcNow;

            //        try
            //        {

            //            OnAuthorizeStartResponse?.Invoke(EndTime,
            //                                             Timestamp,
            //                                             this,
            //                                             Id.ToString(),
            //                                             Request.EventTrackingId,
            //                                             RoamingNetwork.Id,
            //                                             OperatorId,
            //                                             Identification,
            //                                             ProductId,
            //                                             SessionId,
            //                                             Request.RequestTimeout,
            //                                             response,
            //                                             EndTime - StartTime);

            //        }
            //        catch (Exception e)
            //        {
            //            e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStartResponse));
            //        }

            //        #endregion

            //        #region Map response

            //        if (response != null)
            //        {
            //            switch (response.Result)
            //            {

            //                case AuthStartResultType.Authorized:
            //                    return CPO.AuthorizationStart.Authorized(Request,
            //                                                             response.SessionId. HasValue ? response.SessionId. Value.ToOCHP() : default(Session_Id?),
            //                                                             default(PartnerSession_Id?),
            //                                                             DefaultProviderId,//    response.ProviderId.HasValue ? response.ProviderId.Value.ToOCHP() : default(Provider_Id?),
            //                                                             "Ready to charge!",
            //                                                             null,
            //                                                             response.ListOfAuthStopTokens.
            //                                                                 SafeSelect(token => OCHPv1_4.Identification.FromUID(token.ToOCHP()))
            //                                                            );

            //                case AuthStartResultType.NotAuthorized:
            //                    if (Request.Identification.RFIDId != null)
            //                        return CPO.AuthorizationStart.NotAuthorized(Request,
            //                                                                    StatusCodes.RFIDAuthenticationfailed_InvalidUID,
            //                                                                    "RFID Authentication failed - Invalid UID!");

            //                    if (Request.Identification.QRCodeIdentification != null)
            //                        return CPO.AuthorizationStart.NotAuthorized(Request,
            //                                                                    StatusCodes.QRCodeAuthenticationFailed_InvalidCredentials,
            //                                                                    "QR-Code Authentication failed - Invalid credentials!");

            //                    return CPO.AuthorizationStart.NotAuthorized(Request,
            //                                                                StatusCodes.NoPositiveAuthenticationResponse,
            //                                                                "No positive authentication response!");


            //                case AuthStartResultType.InvalidSessionId:
            //                    return CPO.AuthorizationStart.SessionIsInvalid(Request,
            //                                                                   SessionId:         Request.SessionId,
            //                                                                   PartnerSessionId:  Request.PartnerSessionId);

            //                case AuthStartResultType.CommunicationTimeout:
            //                    return CPO.AuthorizationStart.CommunicationToEVSEFailed(Request);

            //                case AuthStartResultType.StartChargingTimeout:
            //                    return CPO.AuthorizationStart.NoEVConnectedToEVSE(Request);

            //                case AuthStartResultType.Reserved:
            //                    return CPO.AuthorizationStart.EVSEAlreadyReserved(Request);

            //                case AuthStartResultType.OutOfService:
            //                    return CPO.AuthorizationStart.EVSEOutOfService(Request);

            //            }
            //        }

            //        #endregion

            //        return CPO.AuthorizationStart.ServiceNotAvailable(
            //               Request,
            //               SessionId:  response?.SessionId. ToOCHP() ?? Request.SessionId,
            //               ProviderId: response?.ProviderId.ToOCHP()
            //           );

            //    }

            //};

            #endregion

            #region OnAuthorizeStop

            //this.EMPRoaming.OnAuthorizeStop += async (Timestamp,
            //                                          Sender,
            //                                          Request) => {

            //    #region Map parameter values

            //    var SessionId            = Request.SessionId.     ToWWCP();
            //    var AuthIdentification   = Request.Identification.ToWWCP();
            //    var EVSEId               = Request.EVSEId?.       ToWWCP();
            //    var OperatorId           = Request.OperatorId.    ToWWCP();

            //    #endregion

            //    if (EVSEId.HasValue)
            //    {

            //        #region Send OnAuthorizeEVSEStopRequest event

            //        var StartTime = DateTime.UtcNow;

            //        try
            //        {

            //            OnAuthorizeEVSEStopRequest?.Invoke(StartTime,
            //                                               Timestamp,
            //                                               this,
            //                                               Id.ToString(),
            //                                               Request.EventTrackingId,
            //                                               RoamingNetwork.Id,
            //                                               OperatorId,
            //                                               EVSEId.Value,
            //                                               SessionId,
            //                                               AuthIdentification,
            //                                               Request.RequestTimeout);

            //        }
            //        catch (Exception e)
            //        {
            //            e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeEVSEStopRequest));
            //        }

            //        #endregion

            //        var response = await RoamingNetwork.AuthorizeStop(SessionId,
            //                                                          AuthIdentification,
            //                                                          EVSEId.Value,
            //                                                          OperatorId,

            //                                                          Request.Timestamp,
            //                                                          Request.CancellationToken,
            //                                                          Request.EventTrackingId,
            //                                                          Request.RequestTimeout);


            //        #region Send OnAuthorizeEVSEStopResponse event

            //        var EndTime = DateTime.UtcNow;

            //        try
            //        {

            //            OnAuthorizeEVSEStopResponse?.Invoke(EndTime,
            //                                                Timestamp,
            //                                                this,
            //                                                Id.ToString(),
            //                                                Request.EventTrackingId,
            //                                                RoamingNetwork.Id,
            //                                                OperatorId,
            //                                                EVSEId.Value,
            //                                                SessionId,
            //                                                AuthIdentification,
            //                                                Request.RequestTimeout,
            //                                                response,
            //                                                EndTime - StartTime);

            //        }
            //        catch (Exception e)
            //        {
            //            e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeEVSEStopResponse));
            //        }

            //        #endregion

            //        #region Map response

            //        if (response != null)
            //        {
            //            switch (response.Result)
            //            {

            //                case AuthStopEVSEResultType.Authorized:
            //                    return CPO.AuthorizationStop.Authorized(
            //                               Request,
            //                               response.SessionId. ToOCHP(),
            //                               null,
            //                               response.ProviderId.ToOCHP(),
            //                               "Ready to stop charging!"
            //                           );

            //                case AuthStopEVSEResultType.InvalidSessionId:
            //                    return CPO.AuthorizationStop.SessionIsInvalid(Request);

            //                case AuthStopEVSEResultType.CommunicationTimeout:
            //                    return CPO.AuthorizationStop.CommunicationToEVSEFailed(Request);

            //                case AuthStopEVSEResultType.StopChargingTimeout:
            //                    return CPO.AuthorizationStop.NoEVConnectedToEVSE(Request);

            //                case AuthStopEVSEResultType.UnknownEVSE:
            //                    return CPO.AuthorizationStop.UnknownEVSEID(Request);

            //                case AuthStopEVSEResultType.OutOfService:
            //                    return CPO.AuthorizationStop.EVSEOutOfService(Request);

            //            }
            //        }

            //        #endregion

            //        return CPO.AuthorizationStop.ServiceNotAvailable(
            //                   Request,
            //                   SessionId:  response?.SessionId. ToOCHP() ?? Request.SessionId,
            //                   ProviderId: response?.ProviderId.ToOCHP()
            //               );

            //    }

            //    else
            //    {

            //        #region Send OnAuthorizeStopRequest event

            //        var StartTime = DateTime.UtcNow;

            //        try
            //        {

            //            OnAuthorizeStopRequest?.Invoke(StartTime,
            //                                           Timestamp,
            //                                           this,
            //                                           Id.ToString(),
            //                                           Request.EventTrackingId,
            //                                           RoamingNetwork.Id,
            //                                           OperatorId,
            //                                           SessionId,
            //                                           AuthIdentification,
            //                                           Request.RequestTimeout);

            //        }
            //        catch (Exception e)
            //        {
            //            e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStopRequest));
            //        }

            //        #endregion

            //        var response = await RoamingNetwork.AuthorizeStop(SessionId,
            //                                                          AuthIdentification,
            //                                                          OperatorId,

            //                                                          Request.Timestamp,
            //                                                          Request.CancellationToken,
            //                                                          Request.EventTrackingId,
            //                                                          Request.RequestTimeout);


            //        #region Send OnAuthorizeStopResponse event

            //        var EndTime = DateTime.UtcNow;

            //        try
            //        {

            //            OnAuthorizeStopResponse?.Invoke(EndTime,
            //                                            Timestamp,
            //                                            this,
            //                                            Id.ToString(),
            //                                            Request.EventTrackingId,
            //                                            RoamingNetwork.Id,
            //                                            OperatorId,
            //                                            SessionId,
            //                                            AuthIdentification,
            //                                            Request.RequestTimeout,
            //                                            response,
            //                                            EndTime - StartTime);

            //        }
            //        catch (Exception e)
            //        {
            //            e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeStopResponse));
            //        }

            //        #endregion

            //        #region Map response

            //        if (response != null)
            //        {
            //            switch (response.Result)
            //            {

            //                case AuthStopResultType.Authorized:
            //                    return CPO.AuthorizationStop.Authorized(
            //                               Request,
            //                               response.SessionId. ToOCHP(),
            //                               null,
            //                               response.ProviderId.ToOCHP(),
            //                               "Ready to stop charging!"
            //                           );

            //                case AuthStopResultType.InvalidSessionId:
            //                    return CPO.AuthorizationStop.SessionIsInvalid(Request);

            //                case AuthStopResultType.CommunicationTimeout:
            //                    return CPO.AuthorizationStop.CommunicationToEVSEFailed(Request);

            //                case AuthStopResultType.StopChargingTimeout:
            //                    return CPO.AuthorizationStop.NoEVConnectedToEVSE(Request);

            //                case AuthStopResultType.OutOfService:
            //                    return CPO.AuthorizationStop.EVSEOutOfService(Request);

            //            }
            //        }

            //        #endregion

            //        return CPO.AuthorizationStop.ServiceNotAvailable(
            //                   Request,
            //                   SessionId:  response?.SessionId. ToOCHP() ?? Request.SessionId,
            //                   ProviderId: response?.ProviderId.ToOCHP()
            //               );

            //    }

            //};

            #endregion

            #region OnChargeDetailRecord

            //this.EMPRoaming.OnChargeDetailRecord += async (Timestamp,
            //                                               Sender,
            //                                               ChargeDetailRecordRequest) => {

            //    #region Map parameter values

            //    var CDRs = new WWCP.ChargeDetailRecord[] { ChargeDetailRecordRequest.ChargeDetailRecord.ToWWCP() };

            //    #endregion

            //    #region Send OnChargeDetailRecordRequest event

            //    var StartTime = DateTime.UtcNow;

            //    try
            //    {

            //        OnChargeDetailRecordRequest?.Invoke(StartTime,
            //                                            Timestamp,
            //                                            this,
            //                                            Id.ToString(),
            //                                            ChargeDetailRecordRequest.EventTrackingId,
            //                                            RoamingNetwork.Id,
            //                                            CDRs,
            //                                            ChargeDetailRecordRequest.RequestTimeout);

            //    }
            //    catch (Exception e)
            //    {
            //        e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnAuthorizeEVSEStopRequest));
            //    }

            //    #endregion


            //    var response = await RoamingNetwork.SendChargeDetailRecords(CDRs,
            //                                                                TransmissionTypes.Direct,

            //                                                                ChargeDetailRecordRequest.Timestamp,
            //                                                                ChargeDetailRecordRequest.CancellationToken,
            //                                                                ChargeDetailRecordRequest.EventTrackingId,
            //                                                                ChargeDetailRecordRequest.RequestTimeout);


            //    #region Send OnChargeDetailRecordResponse event

            //    var EndTime = DateTime.UtcNow;

            //    try
            //    {

            //        OnChargeDetailRecordResponse?.Invoke(EndTime,
            //                                             Timestamp,
            //                                             this,
            //                                             Id.ToString(),
            //                                             ChargeDetailRecordRequest.EventTrackingId,
            //                                             RoamingNetwork.Id,
            //                                             CDRs,
            //                                             ChargeDetailRecordRequest.RequestTimeout,
            //                                             response,
            //                                             EndTime - StartTime);

            //    }
            //    catch (Exception e)
            //    {
            //        e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnChargeDetailRecordResponse));
            //    }

            //    #endregion

            //    #region Map response

            //    if (response != null)
            //    {

            //        if (response.Result == SendCDRsResultTypes.Success)
            //            return Acknowledgement<CPO.SendChargeDetailRecordRequest>.Success(
            //                       ChargeDetailRecordRequest,
            //                       ChargeDetailRecordRequest.ChargeDetailRecord.SessionId,
            //                       ChargeDetailRecordRequest.ChargeDetailRecord.PartnerSessionId,
            //                       "Charge detail record forwarded!"
            //                   );

            //        var FailedCDR = response.RejectedChargeDetailRecords.FirstOrDefault();

            //        if (FailedCDR != null)
            //        {
            //            switch (FailedCDR.Result)
            //            {

            //                //case SendCDRResultTypes.NotForwared:
            //                //    return Acknowledgement<CPO.SendChargeDetailRecordRequest>.SystemError(
            //                //               ChargeDetailRecordRequest,
            //                //               "Communication to EVSE failed!",
            //                //               SessionId:         ChargeDetailRecordRequest.ChargeDetailRecord.SessionId,
            //                //               PartnerSessionId:  ChargeDetailRecordRequest.ChargeDetailRecord.PartnerSessionId
            //                //           );

            //                case SendCDRResultTypes.InvalidSessionId:
            //                    return Acknowledgement<CPO.SendChargeDetailRecordRequest>.SessionIsInvalid(
            //                               ChargeDetailRecordRequest,
            //                               SessionId:         ChargeDetailRecordRequest.ChargeDetailRecord.SessionId,
            //                               PartnerSessionId:  ChargeDetailRecordRequest.ChargeDetailRecord.PartnerSessionId
            //                           );

            //                case SendCDRResultTypes.UnknownEVSE:
            //                    return Acknowledgement<CPO.SendChargeDetailRecordRequest>.UnknownEVSEID(
            //                               ChargeDetailRecordRequest,
            //                               SessionId:         ChargeDetailRecordRequest.ChargeDetailRecord.SessionId,
            //                               PartnerSessionId:  ChargeDetailRecordRequest.ChargeDetailRecord.PartnerSessionId
            //                           );

            //                case SendCDRResultTypes.Error:
            //                    return Acknowledgement<CPO.SendChargeDetailRecordRequest>.DataError(
            //                               ChargeDetailRecordRequest,
            //                               SessionId:         ChargeDetailRecordRequest.ChargeDetailRecord.SessionId,
            //                               PartnerSessionId:  ChargeDetailRecordRequest.ChargeDetailRecord.PartnerSessionId
            //                           );

            //            }
            //        }

            //    }

            //    #endregion

            //    return Acknowledgement<CPO.SendChargeDetailRecordRequest>.ServiceNotAvailable(
            //               ChargeDetailRecordRequest,
            //               SessionId: ChargeDetailRecordRequest.ChargeDetailRecord.SessionId
            //           );

            //};

            #endregion

        }

        #endregion



        // Outgoing EMPClient requests...

        #region GetChargeDetailRecords(From, To = null, ProviderId = null, ...)

        /// <summary>
        /// Download all charge detail records from the OCHP server.
        /// </summary>
        /// <param name="From">The starting time.</param>
        /// <param name="To">An optional end time. [default: current time].</param>
        /// <param name="ProviderId">An optional unique identification of e-mobility service provider.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<IEnumerable<WWCP.ChargeDetailRecord>>

            GetChargeDetailRecords(DateTime               From,
                                   DateTime?              To                  = null,
                                   eMobilityProvider_Id?  ProviderId          = null,

                                   DateTime?              Timestamp           = null,
                                   CancellationToken?     CancellationToken   = null,
                                   EventTracking_Id       EventTrackingId     = null,
                                   TimeSpan?              RequestTimeout      = null)

        {

            return null;

            //#region Initial checks

            //if (!To.HasValue)
            //    To = DateTime.UtcNow;


            //if (!Timestamp.HasValue)
            //    Timestamp = DateTime.UtcNow;

            //if (!CancellationToken.HasValue)
            //    CancellationToken = new CancellationTokenSource().Token;

            //if (EventTrackingId == null)
            //    EventTrackingId = EventTracking_Id.New;

            //if (!RequestTimeout.HasValue)
            //    RequestTimeout = EMPClient?.RequestTimeout;


            //IEnumerable<WWCP.ChargeDetailRecord> result = null;

            //#endregion

            //#region Send OnGetChargeDetailRecordsRequest event

            //var StartTime = DateTime.UtcNow;

            //try
            //{

            //    OnGetChargeDetailRecordsRequest?.Invoke(StartTime,
            //                                            Timestamp.Value,
            //                                            this,
            //                                            Id.ToString(),
            //                                            EventTrackingId,
            //                                            RoamingNetwork.Id,
            //                                            From,
            //                                            To,
            //                                            ProviderId,
            //                                            RequestTimeout);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnGetChargeDetailRecordsRequest));
            //}

            //#endregion


            //var response = await EMPRoaming.GetChargeDetailRecords(ProviderId.HasValue
            //                                                           ? ProviderId.Value.ToOCHP()
            //                                                           : DefaultProviderId.Value,
            //                                                       From,
            //                                                       To.Value,

            //                                                       Timestamp,
            //                                                       CancellationToken,
            //                                                       EventTrackingId,
            //                                                       RequestTimeout).
            //                                ConfigureAwait(false);

            //if (response.HTTPStatusCode == HTTPStatusCode.OK &&
            //    response.Content        != null)
            //{

            //    var Warnings = new List<String>();

            //    result = response.Content.
            //                 ChargeDetailRecords.
            //                 SafeSelect(cdr => {

            //                                       try
            //                                       {

            //                                           return cdr.ToWWCP();

            //                                       }
            //                                       catch (Exception e)
            //                                       {
            //                                           Warnings.Add("Error during import of charge detail record: " + e.Message);
            //                                           return null;
            //                                       }

            //                                   }).
            //                 SafeWhere(cdr => cdr != null);

            //}

            //else
            //    result = new WWCP.ChargeDetailRecord[0];


            //#region Send OnGetChargeDetailRecordsResponse event

            //var EndTime = DateTime.UtcNow;

            //try
            //{

            //    OnGetChargeDetailRecordsResponse?.Invoke(StartTime,
            //                                             Timestamp.Value,
            //                                             this,
            //                                             Id.ToString(),
            //                                             EventTrackingId,
            //                                             RoamingNetwork.Id,
            //                                             From,
            //                                             To,
            //                                             ProviderId,
            //                                             RequestTimeout,
            //                                             result,
            //                                             EndTime - StartTime);

            //}
            //catch (Exception e)
            //{
            //    e.Log(nameof(WWCPEMPAdapter) + "." + nameof(OnGetChargeDetailRecordsResponse));
            //}

            //#endregion

            //return result;

        }

        #endregion

        public Task<POIDataPull<EVSE>> PullEVSEData(DateTime? LastCall = null, GeoCoordinate? SearchCenter = null, float DistanceKM = 0, eMobilityProvider_Id? ProviderId = null, IEnumerable<ChargingStationOperator_Id> OperatorIdFilter = null, IEnumerable<Country> CountryCodeFilter = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<StatusPull<WWCP.EVSEStatus>> PullEVSEStatus(DateTime? LastCall = null, GeoCoordinate? SearchCenter = null, float DistanceKM = 0, EVSEStatusTypes? EVSEStatusFilter = null, eMobilityProvider_Id? ProviderId = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<ReservationResult> Reserve(WWCP.EVSE_Id EVSEId, DateTime? StartTime = null, TimeSpan? Duration = null, ChargingReservation_Id? ReservationId = null, eMobilityProvider_Id? ProviderId = null, RemoteAuthentication Identification = null, ChargingProduct ChargingProduct = null, IEnumerable<Auth_Token> AuthTokens = null, IEnumerable<eMobilityAccount_Id> eMAIds = null, IEnumerable<uint> PINs = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<CancelReservationResult> CancelReservation(ChargingReservation_Id ReservationId, ChargingReservationCancellationReason Reason, eMobilityProvider_Id? ProviderId = null, WWCP.EVSE_Id? EVSEId = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<RemoteStopResult> RemoteStop(ChargingSession_Id SessionId, ReservationHandling? ReservationHandling = null, eMobilityProvider_Id? ProviderId = null, RemoteAuthentication eMAId = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public bool TryGetChargingReservationById(ChargingReservation_Id ReservationId, out ChargingReservation ChargingReservation)
        {
            throw new NotImplementedException();
        }

        public Task<ReservationResult> Reserve(ChargingLocation ChargingLocation, ChargingReservationLevel ReservationLevel = ChargingReservationLevel.EVSE, DateTime? StartTime = null, TimeSpan? Duration = null, ChargingReservation_Id? ReservationId = null, eMobilityProvider_Id? ProviderId = null, RemoteAuthentication RemoteAuthentication = null, ChargingProduct ChargingProduct = null, IEnumerable<Auth_Token> AuthTokens = null, IEnumerable<eMobilityAccount_Id> eMAIds = null, IEnumerable<uint> PINs = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public Task<CancelReservationResult> CancelReservation(ChargingReservation_Id ReservationId, ChargingReservationCancellationReason Reason, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }

        public bool TryGetChargingSessionById(ChargingSession_Id ChargingSessionId, out ChargingSession ChargingSession)
        {
            throw new NotImplementedException();
        }

        public Task<RemoteStartResult> RemoteStart(ChargingLocation ChargingLocation, ChargingProduct ChargingProduct = null, ChargingReservation_Id? ReservationId = null, ChargingSession_Id? SessionId = null, eMobilityProvider_Id? ProviderId = null, RemoteAuthentication RemoteAuthentication = null, DateTime? Timestamp = null, CancellationToken? CancellationToken = null, EventTracking_Id EventTrackingId = null, TimeSpan? RequestTimeout = null)
        {
            throw new NotImplementedException();
        }




        // -----------------------------------------------------------------------------------------------------

        #region (timer) PullDataService(State)

        private void PullDataService(Object State)
        {

            if (!DisablePullPOIData)
            {

                try
                {

                    PullData().Wait();

                }
                catch (Exception e)
                {

                    while (e.InnerException != null)
                        e = e.InnerException;

                    DebugX.Log("A exception occured during PullDataService: " + e.Message + Environment.NewLine + e.StackTrace);

                    OnWWCPEMPAdapterException?.Invoke(DateTime.Now,
                                                      this,
                                                      e);

                }

            }

        }

        public async Task PullData()
        {

            //var LockTaken = await PullEVSEDataLock.WaitAsync(0);

            //if (LockTaken)
            //{

            //    Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            //    var StartTime = DateTime.UtcNow;
            //    DebugX.LogT("[" + Id + "] 'Pull data service' started at " + StartTime.ToIso8601());

            //    try
            //    {

            //        var TimestampBeforeLastPullDataRun = DateTime.UtcNow;

            //        var PullEVSEData  = await EMPRoaming.PullEVSEData(DefaultProviderId.Value,
            //                                                          DefaultSearchCenter,
            //                                                          DefaultDistanceKM ?? 0,
            //                                                          TimestampOfLastPullDataRun,

            //                                                          CancellationToken:  new CancellationTokenSource().Token,
            //                                                          EventTrackingId:    EventTracking_Id.New,
            //                                                          RequestTimeout:     PullDataServiceRequestTimeout).

            //                                             ConfigureAwait(false);

            //        //var PullEVSEData = new {
            //        //    Content = PullEVSEDataResponse.Parse(null,
            //        //                                         XDocument.Parse(File.ReadAllText(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar +
            //        //                                                                          "PullEvseDataResponse_2017-07-19_PROD.xml", Encoding.UTF8)).Root)
            //        //};

            //        var DownloadTime = DateTime.UtcNow;

            //        TimestampOfLastPullDataRun = TimestampBeforeLastPullDataRun;

            //        #region Everything is ok!

            //        if (PullEVSEData                    != null     &&
            //            PullEVSEData.Content            != null     &&
            //            PullEVSEData.Content.StatusCode == null)
            //            //PullEVSEData.Content.StatusCode != null     &&
            //            //PullEVSEData.Content.StatusCode.HasResult() &&
            //            //PullEVSEData.Content.StatusCode.Value.Code == StatusCodes.Success)
            //        {

            //            // This will parse all nested data structures!
            //            var OperatorEVSEData = PullEVSEData?.Content?.EVSEData?.OperatorEVSEData?.ToArray();

            //            if (OperatorEVSEData?.Length > 0)
            //            {

            //                DebugX.Log(String.Concat("Imported data from ", OperatorEVSEData.Length, " OCHP EVSE operators"));

            //                ChargingStationOperator      WWCPChargingStationOperator     = null;
            //                ChargingStationOperator_Id?  WWCPChargingStationOperatorId   = null;
            //                EVSEDataRecord[]             CurrentEVSEDataRecords          = null;

            //                UInt64                       IllegalOperatorsIds             = 0;
            //                UInt64                       OperatorsSkipped                = 0;
            //                UInt64                       TotalEVSEsCreated               = 0;
            //                UInt64                       TotalEVSEsUpdated               = 0;
            //                UInt64                       TotalEVSEsSkipped               = 0;

            //                CPInfoList                   _CPInfoList;
            //                WWCP.EVSE_Id?  CurrentEVSEId;
            //                UInt64         EVSEsSkipped;

            //                foreach (var CurrentOperatorEVSEData in OperatorEVSEData.OrderBy(evseoperator => evseoperator.OperatorName))
            //                {

            //                    if (EVSEOperatorFilter(CurrentOperatorEVSEData.OperatorName,
            //                                           CurrentOperatorEVSEData.OperatorId))
            //                    {

            //                        WWCPChargingStationOperatorId = CurrentOperatorEVSEData.OperatorId.ToWWCP();

            //                        if (WWCPChargingStationOperatorId.HasValue)
            //                        {

            //                            #region Get WWCP charging station operator...

            //                            if (!RoamingNetwork.TryGetChargingStationOperatorById(WWCPChargingStationOperatorId, out WWCPChargingStationOperator))
            //                            {

            //                                DebugX.Log(String.Concat("Creating OCHP EVSE operator '", CurrentOperatorEVSEData.OperatorName,
            //                                                     "' (", CurrentOperatorEVSEData.OperatorId.ToString(),
            //                                                     " => ", WWCPChargingStationOperatorId, ")"));

            //                                WWCPChargingStationOperator = RoamingNetwork.CreateChargingStationOperator(WWCPChargingStationOperatorId.Value,
            //                                                                                                           I18NString.Create(Languages.unknown,
            //                                                                                                                             CurrentOperatorEVSEData.OperatorName));

            //                            }

            //                            #endregion

            //                            #region ...or create a new one!

            //                            else
            //                            {

            //                                DebugX.Log(String.Concat("Updating OCHP EVSE operator '", CurrentOperatorEVSEData.OperatorName,
            //                                                         "' (", CurrentOperatorEVSEData.OperatorId.ToString(),
            //                                                         " => ", WWCPChargingStationOperatorId, ")"));

            //                                // Update name (via events)!
            //                                WWCPChargingStationOperator.Name = I18NString.Create(Languages.unknown,
            //                                                                                     CurrentOperatorEVSEData.OperatorName);

            //                            }

            //                            #endregion


            //                            #region Generate a list of all charging pools/stations/EVSEs

            //                            CurrentEVSEId           = null;
            //                            EVSEsSkipped            = 0;
            //                            _CPInfoList             = new CPInfoList(WWCPChargingStationOperator.Id);
            //                            CurrentEVSEDataRecords  = CurrentOperatorEVSEData.EVSEDataRecords.ToArray();

            //                            foreach (var CurrentEVSEDataRecord in CurrentEVSEDataRecords)
            //                            {

            //                                CurrentEVSEId = CurrentEVSEDataRecord.Id.ToWWCP();

            //                                if (CurrentEVSEId.HasValue)
            //                                {

            //                                    try
            //                                    {
            //                                                                      // Generate a stable charging pool identification
            //                                        _CPInfoList.AddOrUpdateCPInfo(WWCP.ChargingPool_Id.Generate(CurrentEVSEDataRecord.Id.OperatorId.ToWWCP().Value,
            //                                                                                                    CurrentEVSEDataRecord.Address.      ToWWCP(),
            //                                                                                                    CurrentEVSEDataRecord.GeoCoordinate),
            //                                                                      CurrentEVSEDataRecord.Address,
            //                                                                      CurrentEVSEDataRecord.GeoCoordinate,
            //                                                                      CurrentEVSEDataRecord.ChargingStationId.ToString(),
            //                                                                      CurrentEVSEDataRecord.Id);

            //                                    } catch (Exception e)
            //                                    {
            //                                        DebugX.Log("WWCPEMPAdapter PullEVSEData failed: " + e.Message);
            //                                        EVSEsSkipped++;
            //                                    }

            //                                }

            //                                else
            //                                    // Invalid WWCP EVSE identification
            //                                    EVSEsSkipped++;

            //                            }

            //                            var EVSEIdLookup = _CPInfoList.VerifyUniquenessOfChargingStationIds();

            //                            DebugX.Log(String.Concat(_CPInfoList.                                                               Count(), " pools, ",
            //                                                     _CPInfoList.SelectMany(_ => _.ChargingStations).                           Count(), " stations, ",
            //                                                     _CPInfoList.SelectMany(_ => _.ChargingStations).SelectMany(_ => _.EVSEIds).Count(), " EVSEs imported. ",
            //                                                     EVSEsSkipped, " EVSEs skipped."));

            //                            #endregion

            //                            #region Data

            //                            ChargingPool     _ChargingPool                  = null;
            //                            UInt64           ChargingPoolsCreated           = 0;
            //                            UInt64           ChargingPoolsUpdated           = 0;
            //                            Languages        LocationLanguage               = Languages.unknown;
            //                            Languages        LocalChargingStationLanguage   = Languages.unknown;

            //                            ChargingStation  _ChargingStation               = null;
            //                            UInt64           ChargingStationsCreated        = 0;
            //                            UInt64           ChargingStationsUpdated        = 0;

            //                            EVSEInfo         EVSEInfo                       = null;
            //                            EVSE             _EVSE                          = null;
            //                            UInt64           EVSEsCreated                   = 0;
            //                            UInt64           EVSEsUpdated                   = 0;

            //                            #endregion


            //                            //foreach (var poolinfo in _CPInfoList.ChargingPools)
            //                            //{

            //                            //    try
            //                            //    {

            //                            //        foreach (var stationinfo in poolinfo)
            //                            //        {

            //                            //            try
            //                            //            {

            //                            //                foreach (var evseid in stationinfo)
            //                            //                {

            //                            //                    try
            //                            //                    {


            //                            //                    }
            //                            //                    catch (Exception e)
            //                            //                    { }

            //                            //                }

            //                            //            }
            //                            //            catch (Exception e)
            //                            //            { }

            //                            //        }

            //                            //    }
            //                            //    catch (Exception e)
            //                            //    { }

            //                            //}

            //                            foreach (var CurrentEVSEDataRecord in CurrentEVSEDataRecords)
            //                            {

            //                                CurrentEVSEId = CurrentEVSEDataRecord.Id.ToWWCP();

            //                                if (CurrentEVSEId.HasValue && EVSEIdLookup.Contains(CurrentEVSEDataRecord.Id))
            //                                {

            //                                    try
            //                                    {

            //                                        EVSEInfo = EVSEIdLookup[CurrentEVSEDataRecord.Id];

            //                                        #region Set LocationLanguage

            //                                        switch (EVSEInfo.PoolAddress.Country.Alpha2Code.ToLower())
            //                                        {

            //                                            case "de": LocationLanguage = Languages.deu; break;
            //                                            case "fr": LocationLanguage = Languages.fra; break;
            //                                            case "dk": LocationLanguage = Languages.dk; break;
            //                                            case "no": LocationLanguage = Languages.no; break;
            //                                            case "fi": LocationLanguage = Languages.fin; break;
            //                                            case "se": LocationLanguage = Languages.swe; break;

            //                                            case "sk": LocationLanguage = Languages.sk; break;
            //                                            case "it": LocationLanguage = Languages.ita; break;
            //                                            case "us": LocationLanguage = Languages.en; break;
            //                                            case "nl": LocationLanguage = Languages.nld; break;
            //                                            case "at": LocationLanguage = Languages.deu; break;
            //                                            case "ru": LocationLanguage = Languages.ru; break;
            //                                            case "il": LocationLanguage = Languages.heb; break;

            //                                            case "be":
            //                                            case "ch":
            //                                            case "al":
            //                                            default:   LocationLanguage = Languages.unknown; break;

            //                                        }

            //                                        if (EVSEInfo.PoolAddress.Country == Country.Germany)
            //                                            LocalChargingStationLanguage = Languages.deu;

            //                                        else if (EVSEInfo.PoolAddress.Country == Country.Denmark)
            //                                            LocalChargingStationLanguage = Languages.dk;

            //                                        else if (EVSEInfo.PoolAddress.Country == Country.France)
            //                                            LocalChargingStationLanguage = Languages.fra;

            //                                        else
            //                                            LocalChargingStationLanguage = Languages.unknown;

            //                                        #endregion

            //                                        #region Guess the language of the 'ChargingStationName' by '_Address.Country'

            //                                        //_ChargingStationName = new I18NString();

            //                                        //if (LocalChargingStationName.IsNotNullOrEmpty())
            //                                        //    _ChargingStationName.Add(LocalChargingStationLanguage,
            //                                        //                             LocalChargingStationName);

            //                                        //if (EnChargingStationName.IsNotNullOrEmpty())
            //                                        //    _ChargingStationName.Add(Languages.en,
            //                                        //                             EnChargingStationName);

            //                                        #endregion


            //                                        #region Update matching charging pool...

            //                                        if (WWCPChargingStationOperator.TryGetChargingPoolbyId(EVSEInfo.PoolId, out _ChargingPool))
            //                                        {

            //                                            // External update via events!
            //                                            _ChargingPool.Description           = CurrentEVSEDataRecord.AdditionalInfo;
            //                                            _ChargingPool.LocationLanguage      = LocationLanguage;
            //                                            _ChargingPool.EntranceLocation      = CurrentEVSEDataRecord.GeoChargingPointEntrance;
            //                                            _ChargingPool.OpeningTimes          = CurrentEVSEDataRecord.OpeningTime != null ? OpeningTimes.Parse(CurrentEVSEDataRecord.OpeningTime) : null;
            //                                            _ChargingPool.AuthenticationModes   = new ReactiveSet<WWCP.AuthenticationModes>(CurrentEVSEDataRecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OCHPMapper.AsWWCPAuthenticationMode(mode)));
            //                                            _ChargingPool.PaymentOptions        = new ReactiveSet<WWCP.PaymentOptions>     (CurrentEVSEDataRecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OCHPMapper.AsWWCPPaymentOption(option)));
            //                                            _ChargingPool.Accessibility         = CurrentEVSEDataRecord.Accessibility.ToWWCP();
            //                                            _ChargingPool.HotlinePhoneNumber    = I18NString.Create(Languages.unknown, CurrentEVSEDataRecord.HotlinePhoneNumber);

            //                                            ChargingPoolsUpdated++;

            //                                        }

            //                                        #endregion

            //                                        #region  ...or create a new one!

            //                                        else
            //                                        {

            //                                            // An operator might have multiple suboperator ids!
            //                                            if (!WWCPChargingStationOperator.Ids.Contains(EVSEInfo.PoolId.OperatorId))
            //                                                WWCPChargingStationOperator.AddId(EVSEInfo.PoolId.OperatorId);

            //                                            _ChargingPool = WWCPChargingStationOperator.CreateChargingPool(

            //                                                                EVSEInfo.PoolId,

            //                                                                Configurator: pool => {

            //                                                                    pool.DataSource                  = Id.ToString();
            //                                                                    pool.Description                 = CurrentEVSEDataRecord.AdditionalInfo;
            //                                                                    pool.Address                     = CurrentEVSEDataRecord.Address.ToWWCP();
            //                                                                    pool.GeoLocation                 = CurrentEVSEDataRecord.GeoCoordinate;
            //                                                                    pool.LocationLanguage            = LocationLanguage;
            //                                                                    pool.EntranceLocation            = CurrentEVSEDataRecord.GeoChargingPointEntrance;
            //                                                                    pool.OpeningTimes                = CurrentEVSEDataRecord.OpeningTime != null ? OpeningTimes.Parse(CurrentEVSEDataRecord.OpeningTime) : null;
            //                                                                    pool.AuthenticationModes         = new ReactiveSet<WWCP.AuthenticationModes>(CurrentEVSEDataRecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OCHPMapper.AsWWCPAuthenticationMode(mode)));
            //                                                                    pool.PaymentOptions              = new ReactiveSet<WWCP.PaymentOptions>     (CurrentEVSEDataRecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OCHPMapper.AsWWCPPaymentOption(option)));
            //                                                                    pool.Accessibility               = CurrentEVSEDataRecord.Accessibility.ToWWCP();
            //                                                                    pool.HotlinePhoneNumber          = I18NString.Create(Languages.unknown, CurrentEVSEDataRecord.HotlinePhoneNumber);
            //                                                                    //pool.StatusAggregationDelegate   = ChargingStationStatusAggregationDelegate;

            //                                                                    ChargingPoolsCreated++;

            //                                                                });

            //                                        }

            //                                        #endregion


            //                                        #region Update matching charging station...

            //                                        if (_ChargingPool.TryGetChargingStationbyId(EVSEInfo.StationId, out _ChargingStation))
            //                                        {

            //                                            // Update via events!
            //                                            _ChargingStation.Name                       = CurrentEVSEDataRecord.ChargingStationName;
            //                                            _ChargingStation.HubjectStationId           = CurrentEVSEDataRecord.ChargingStationId.ToString();
            //                                            _ChargingStation.Description                = CurrentEVSEDataRecord.AdditionalInfo;
            //                                            _ChargingStation.AuthenticationModes        = new ReactiveSet<WWCP.AuthenticationModes>(CurrentEVSEDataRecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OCHPMapper.AsWWCPAuthenticationMode(mode)));
            //                                            _ChargingStation.PaymentOptions             = new ReactiveSet<WWCP.PaymentOptions>     (CurrentEVSEDataRecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OCHPMapper.AsWWCPPaymentOption(option)));
            //                                            _ChargingStation.Accessibility              = CurrentEVSEDataRecord.Accessibility.ToWWCP();
            //                                            _ChargingStation.HotlinePhoneNumber         = I18NString.Create(Languages.unknown, CurrentEVSEDataRecord.HotlinePhoneNumber);
            //                                            _ChargingStation.IsHubjectCompatible        = CurrentEVSEDataRecord.IsHubjectCompatible;
            //                                            _ChargingStation.DynamicInfoAvailable       = CurrentEVSEDataRecord.DynamicInfoAvailable;
            //                                            _ChargingStation.StatusAggregationDelegate  = EVSEStatusAggregationDelegate;

            //                                            ChargingStationsUpdated++;

            //                                        }

            //                                        #endregion

            //                                        #region ...or create a new one!

            //                                        else
            //                                            _ChargingStation = _ChargingPool.CreateChargingStation(

            //                                                                    EVSEInfo.StationId,

            //                                                                    Configurator: station => {

            //                                                                        station.DataSource                 = Id.ToString();
            //                                                                        station.Name                       = CurrentEVSEDataRecord.ChargingStationName;
            //                                                                        station.HubjectStationId           = CurrentEVSEDataRecord.ChargingStationId.ToString();
            //                                                                        station.Description                = CurrentEVSEDataRecord.AdditionalInfo;
            //                                                                        station.AuthenticationModes        = new ReactiveSet<WWCP.AuthenticationModes>(CurrentEVSEDataRecord.AuthenticationModes.ToEnumeration().SafeSelect(mode   => OCHPMapper.AsWWCPAuthenticationMode(mode)));
            //                                                                        station.PaymentOptions             = new ReactiveSet<WWCP.PaymentOptions>     (CurrentEVSEDataRecord.PaymentOptions.     ToEnumeration().SafeSelect(option => OCHPMapper.AsWWCPPaymentOption(option)));
            //                                                                        station.Accessibility              = CurrentEVSEDataRecord.Accessibility.ToWWCP();
            //                                                                        station.HotlinePhoneNumber         = I18NString.Create(Languages.unknown, CurrentEVSEDataRecord.HotlinePhoneNumber);
            //                                                                        station.IsHubjectCompatible        = CurrentEVSEDataRecord.IsHubjectCompatible;
            //                                                                        station.DynamicInfoAvailable       = CurrentEVSEDataRecord.DynamicInfoAvailable;
            //                                                                        station.StatusAggregationDelegate  = EVSEStatusAggregationDelegate;

            //                                                                        // photo_uri => "place_photo"

            //                                                                        ChargingStationsCreated++;

            //                                                                    }

            //                                                   );

            //                                        #endregion


            //                                        #region Update matching EVSE...

            //                                        if (_ChargingStation.TryGetEVSEbyId(CurrentEVSEDataRecord.Id.ToWWCP().Value, out _EVSE))
            //                                        {

            //                                            // Update via events!
            //                                            _EVSE.Description     = CurrentEVSEDataRecord.AdditionalInfo;
            //                                            _EVSE.ChargingModes   = CurrentEVSEDataRecord.ChargingModes.AsWWCPChargingMode();
            //                                            OCHPMapper.ApplyChargingFacilities(_EVSE, CurrentEVSEDataRecord.ChargingFacilities);
            //                                            _EVSE.MaxCapacity     = CurrentEVSEDataRecord.MaxCapacity;
            //                                            _EVSE.SocketOutlets   = new ReactiveSet<SocketOutlet>(CurrentEVSEDataRecord.Plugs.ToEnumeration().SafeSelect(Plug => new SocketOutlet(Plug.AsWWCPPlugTypes())));

            //                                            EVSEsUpdated++;

            //                                        }

            //                                        #endregion

            //                                        #region ...or create a new one!

            //                                        else
            //                                            _ChargingStation.CreateEVSE(CurrentEVSEDataRecord.Id.ToWWCP().Value,

            //                                                                        Configurator: evse => {

            //                                                                            evse.DataSource      = Id.ToString();
            //                                                                            evse.Description     = CurrentEVSEDataRecord.AdditionalInfo;
            //                                                                            evse.ChargingModes   = CurrentEVSEDataRecord.ChargingModes.AsWWCPChargingMode();
            //                                                                            OCHPMapper.ApplyChargingFacilities(evse, CurrentEVSEDataRecord.ChargingFacilities);
            //                                                                            evse.MaxCapacity     = CurrentEVSEDataRecord.MaxCapacity;
            //                                                                            evse.SocketOutlets   = new ReactiveSet<SocketOutlet>(CurrentEVSEDataRecord.Plugs.ToEnumeration().SafeSelect(Plug => new SocketOutlet(Plug.AsWWCPPlugTypes())));

            //                                                                            EVSEsCreated++;

            //                                                                        }
            //                                                                       );

            //                                        #endregion


            //                                    }
            //                                    catch (Exception e)
            //                                    {
            //                                        DebugX.Log(e.Message);
            //                                    }

            //                                }

            //                            }

            //                            DebugX.Log(EVSEsCreated + " EVSE created, " + EVSEsUpdated + " EVSEs updated, " + EVSEsSkipped + " EVSEs skipped");

            //                            TotalEVSEsCreated += EVSEsCreated;
            //                            TotalEVSEsUpdated += EVSEsUpdated;
            //                            TotalEVSEsSkipped += EVSEsSkipped;

            //                        }

            //                        #region Illegal charging station operator identification...

            //                        else
            //                        {
            //                            DebugX.Log("Illegal charging station operator identification: '" + CurrentOperatorEVSEData.OperatorId.ToString() + "'!");
            //                            IllegalOperatorsIds++;
            //                            TotalEVSEsSkipped += (UInt64) CurrentOperatorEVSEData.EVSEDataRecords.LongCount();
            //                        }

            //                        #endregion

            //                    }

            //                    #region EVSE operator is filtered...

            //                    else
            //                    {
            //                        DebugX.Log("Skipping EVSE operator " + CurrentOperatorEVSEData.OperatorName + " (" + CurrentOperatorEVSEData.OperatorId.ToString() + ") with " + CurrentOperatorEVSEData.EVSEDataRecords.Count() + " EVSE data records");
            //                        OperatorsSkipped++;
            //                        TotalEVSEsSkipped += (UInt64) CurrentOperatorEVSEData.EVSEDataRecords.LongCount();
            //                    }

            //                    #endregion

            //                }

            //                if (IllegalOperatorsIds > 0)
            //                    DebugX.Log(IllegalOperatorsIds + " illegal EVSE operator identifications");

            //                if (OperatorsSkipped > 0)
            //                    DebugX.Log(OperatorsSkipped    + " EVSE operators skipped");

            //                if (TotalEVSEsCreated > 0)
            //                    DebugX.Log(TotalEVSEsCreated   + " EVSEs created");

            //                if (TotalEVSEsUpdated > 0)
            //                    DebugX.Log(TotalEVSEsUpdated   + " EVSEs updated");

            //                if (TotalEVSEsSkipped > 0)
            //                    DebugX.Log(TotalEVSEsSkipped   + " EVSEs skipped");

            //            }

            //        }

            //        #endregion

            //        #region HTTP status is not 200 - OK

            //            //else if (PullEVSEDataTask.Result.HTTPStatusCode != HTTPStatusCode.OK)
            //            //{
            //            //
            //            //    DebugX.Log("Importing EVSE data records failed: " +
            //            //               PullEVSEDataTask.Result.HTTPStatusCode.ToString() +
            //            //
            //            //               PullEVSEDataTask.Result.HTTPBody != null
            //            //                   ? Environment.NewLine + PullEVSEDataTask.Result.HTTPBody.ToUTF8String()
            //            //                   : "");
            //            //
            //            //}

            //            #endregion

            //        #region OCHP StatusCode is not 'Success'

            //            //else if (PullEVSEDataTask.Result.Content.StatusCode != null &&
            //            //        !PullEVSEDataTask.Result.Content.StatusCode.HasResult)
            //            //{
            //            //
            //            //    DebugX.Log("Importing EVSE data records failed: " +
            //            //               PullEVSEDataTask.Result.Content.StatusCode.Code.ToString() +
            //            //
            //            //               (PullEVSEDataTask.Result.Content.StatusCode.Description.IsNotNullOrEmpty()
            //            //                    ? ", " + PullEVSEDataTask.Result.Content.StatusCode.Description
            //            //                    : "") +
            //            //
            //            //               (PullEVSEDataTask.Result.Content.StatusCode.AdditionalInfo.IsNotNullOrEmpty()
            //            //                    ? ", " + PullEVSEDataTask.Result.Content.StatusCode.AdditionalInfo
            //            //                    : ""));
            //            //
            //            //}

            //            #endregion

            //        #region Something unexpected happend!

            //            //else
            //            //{
            //            //    DebugX.Log("Importing EVSE data records failed unexpectedly!");
            //            //}

            //            #endregion


            //        var EndTime = DateTime.UtcNow;
            //        DebugX.LogT("[" + Id + "] 'Pull data service' finished after " + (EndTime - StartTime).TotalSeconds + " seconds (" + (DownloadTime - StartTime).TotalSeconds + "/" + (EndTime - DownloadTime).TotalSeconds + ")");

            //    }
            //    catch (Exception e)
            //    {

            //        while (e.InnerException != null)
            //            e = e.InnerException;

            //        DebugX.LogT(nameof(WWCPEMPAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

            //    }

            //    finally
            //    {
            //        if (LockTaken)
            //            PullEVSEDataLock.Release();
            //    }

            //}

        }

        #endregion

        #region (timer) PullStatusService(State)

        private void PullStatusService(Object State)
        {

            if (!DisablePullStatus)
            {

                PullStatus().Wait();

                //ToDo: Handle errors!

            }

        }

        public async Task PullStatus()
        {

            //DebugX.LogT("[" + Id + "] 'Pull status service', as every " + _PullStatusServiceEvery + "ms!");

            //if (Monitor.TryEnter(PullStatusServiceLock,
            //                     TimeSpan.FromSeconds(5)))
            //{

            //    Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;

            //    var StartTime = DateTime.UtcNow;
            //    DebugX.LogT("[" + Id + "] 'Pull status service' started at " + StartTime.ToIso8601());

            //    try
            //    {

            //        var PullEVSEStatusTask  = EMPRoaming.PullEVSEStatus(DefaultProviderId.Value,
            //                                                            DefaultSearchCenter,
            //                                                            DefaultDistanceKM.HasValue ? DefaultDistanceKM.Value : 0,

            //                                                            CancellationToken:  new CancellationTokenSource().Token,
            //                                                            EventTrackingId:    EventTracking_Id.New,
            //                                                            RequestTimeout:     PullStatusServiceRequestTimeout);

            //        PullEVSEStatusTask.Wait();

            //        var DownloadTime = DateTime.UtcNow;

            //        #region Everything is ok!

            //        if (PullEVSEStatusTask.Result                    != null  &&
            //            PullEVSEStatusTask.Result.Content            != null  &&
            //            PullEVSEStatusTask.Result.Content.StatusCode.HasValue &&
            //            PullEVSEStatusTask.Result.Content.StatusCode.Value.Code == StatusCodes.Success)
            //        {

            //            var OperatorEVSEStatus = PullEVSEStatusTask.Result.Content.OperatorEVSEStatus;

            //            if (OperatorEVSEStatus != null && OperatorEVSEStatus.Any())
            //            {

            //                DebugX.Log("Imported " + OperatorEVSEStatus.Count() + " OperatorEVSEStatus!");
            //                DebugX.Log("Imported " + OperatorEVSEStatus.SelectMany(status => status.EVSEStatusRecords).Count() + " EVSEStatusRecords!");

            //                ChargingStationOperator      WWCPChargingStationOperator     = null;
            //                ChargingStationOperator_Id?  WWCPChargingStationOperatorId   = null;
            //                UInt64                       IllegalOperatorsIds             = 0;
            //                UInt64                       OperatorsSkipped                = 0;
            //                UInt64                       TotalEVSEsUpdated               = 0;
            //                UInt64                       TotalEVSEsSkipped               = 0;

            //                foreach (var CurrentOperatorEVSEStatus in OperatorEVSEStatus.OrderBy(evseoperator => evseoperator.OperatorName))
            //                {

            //                    if (EVSEOperatorFilter(CurrentOperatorEVSEStatus.OperatorName,
            //                                           CurrentOperatorEVSEStatus.OperatorId))
            //                    {

            //                        DebugX.Log("Importing EVSE operator " + CurrentOperatorEVSEStatus.OperatorName + " (" + CurrentOperatorEVSEStatus.OperatorId.ToString() + ") with " + CurrentOperatorEVSEStatus.EVSEStatusRecords.Count() + " EVSE status records");

            //                        WWCPChargingStationOperatorId = CurrentOperatorEVSEStatus.OperatorId.ToWWCP();

            //                        if (WWCPChargingStationOperatorId.HasValue)
            //                        {

            //                            if (!RoamingNetwork.TryGetChargingStationOperatorById(WWCPChargingStationOperatorId, out WWCPChargingStationOperator))
            //                                WWCPChargingStationOperator = RoamingNetwork.CreateChargingStationOperator(WWCPChargingStationOperatorId.Value,
            //                                                                                                           I18NString.Create(Languages.unknown, CurrentOperatorEVSEStatus.OperatorName));

            //                            else
            //                                // Update name (via events)!
            //                                WWCPChargingStationOperator.Name = I18NString.Create(Languages.unknown, CurrentOperatorEVSEStatus.OperatorName);

            //                            WWCP.EVSE     CurrentEVSE    = null;
            //                            WWCP.EVSE_Id? CurrentEVSEId  = null;
            //                            UInt64        EVSEsUpdated   = 0;
            //                            UInt64        EVSEsSkipped   = 0;

            //                            foreach (var CurrentEVSEDataRecord in CurrentOperatorEVSEStatus.EVSEStatusRecords)
            //                            {

            //                                CurrentEVSEId = CurrentEVSEDataRecord.Id.ToWWCP();

            //                                if (CurrentEVSEId.HasValue &&
            //                                    WWCPChargingStationOperator.TryGetEVSEbyId(CurrentEVSEId, out CurrentEVSE))
            //                                {
            //                                    CurrentEVSE.Status = CurrentEVSEDataRecord.Status.AsWWCPEVSEStatus();
            //                                    EVSEsUpdated++;
            //                                }

            //                                else
            //                                    EVSEsSkipped++;

            //                            }

            //                            DebugX.Log(EVSEsUpdated + " EVSE status updated, " + EVSEsSkipped + " EVSEs skipped");

            //                            TotalEVSEsUpdated += EVSEsUpdated;
            //                            TotalEVSEsSkipped += EVSEsSkipped;

            //                        }

            //                        #region Illegal charging station operator identification...

            //                        else
            //                        {
            //                            DebugX.Log("Illegal charging station operator identification: '" + CurrentOperatorEVSEStatus.OperatorId.ToString() + "'!");
            //                            IllegalOperatorsIds++;
            //                            TotalEVSEsSkipped += (UInt64) CurrentOperatorEVSEStatus.EVSEStatusRecords.LongCount();
            //                        }

            //                        #endregion

            //                    }

            //                    #region EVSE operator is filtered...

            //                    else
            //                    {
            //                        DebugX.Log("Skipping EVSE operator " + CurrentOperatorEVSEStatus.OperatorName + " (" + CurrentOperatorEVSEStatus.OperatorId.ToString() + ") with " + CurrentOperatorEVSEStatus.EVSEStatusRecords.Count() + " EVSE status records");
            //                        OperatorsSkipped++;
            //                        TotalEVSEsSkipped += (UInt64) CurrentOperatorEVSEStatus.EVSEStatusRecords.LongCount();
            //                    }

            //                    #endregion

            //                }

            //                if (IllegalOperatorsIds > 0)
            //                    DebugX.Log(OperatorsSkipped + " illegal EVSE operator identifications");

            //                if (OperatorsSkipped > 0)
            //                    DebugX.Log(OperatorsSkipped + " EVSE operators skipped");

            //                if (TotalEVSEsUpdated > 0)
            //                    DebugX.Log(TotalEVSEsUpdated + " EVSEs updated");

            //                if (TotalEVSEsSkipped > 0)
            //                    DebugX.Log(TotalEVSEsSkipped + " EVSEs skipped");

            //            }

            //        }

            //        #endregion

            //        #region HTTP status is not 200 - OK

            //        else if (PullEVSEStatusTask.Result.HTTPStatusCode != HTTPStatusCode.OK)
            //        {

            //            DebugX.Log("Importing EVSE status records failed: " +
            //                       PullEVSEStatusTask.Result.HTTPStatusCode.ToString() +

            //                       PullEVSEStatusTask.Result.HTTPBody != null
            //                           ? Environment.NewLine + PullEVSEStatusTask.Result.HTTPBody.ToUTF8String()
            //                           : "");

            //        }

            //        #endregion

            //        #region OCHP StatusCode is not 'Success'

            //        else if (PullEVSEStatusTask.Result.Content.StatusCode.HasValue &&
            //                !PullEVSEStatusTask.Result.Content.StatusCode.Value.HasResult)
            //        {

            //            DebugX.Log("Importing EVSE status records failed: " +
            //                       PullEVSEStatusTask.Result.Content.StatusCode.Value.Code.ToString() +

            //                       (PullEVSEStatusTask.Result.Content.StatusCode.Value.Description.IsNotNullOrEmpty()
            //                            ? ", " + PullEVSEStatusTask.Result.Content.StatusCode.Value.Description
            //                            : "") +

            //                       (PullEVSEStatusTask.Result.Content.StatusCode.Value.AdditionalInfo.IsNotNullOrEmpty()
            //                            ? ", " + PullEVSEStatusTask.Result.Content.StatusCode.Value.AdditionalInfo
            //                            : ""));

            //        }

            //        #endregion

            //        #region Something unexpected happend!

            //        else
            //        {
            //            DebugX.Log("Importing EVSE status records failed unexpectedly!");
            //        }

            //        #endregion


            //        var EndTime = DateTime.UtcNow;

            //        DebugX.LogT("[" + Id + "] 'Pull status service' finished after " + (EndTime - StartTime).TotalSeconds + " seconds (" + (DownloadTime - StartTime).TotalSeconds + "/" + (EndTime - DownloadTime).TotalSeconds + ")");

            //    }
            //    catch (Exception e)
            //    {

            //        while (e.InnerException != null)
            //            e = e.InnerException;

            //        DebugX.LogT(nameof(WWCPEMPAdapter) + " '" + Id + "' led to an exception: " + e.Message + Environment.NewLine + e.StackTrace);

            //    }

            //    finally
            //    {
            //        Monitor.Exit(PullStatusServiceLock);
            //    }

            //}

            //else
            //    Console.WriteLine("PullStatusServiceLock missed!");

            //return;

        }

        #endregion

        // Pull CDRs!

        // -----------------------------------------------------------------------------------------------------


        #region Operator overloading

        #region Operator == (WWCPEMPAdapter1, WWCPEMPAdapter2)

        /// <summary>
        /// Compares two WWCPEMPAdapters for equality.
        /// </summary>
        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (WWCPCPOAdapter WWCPEMPAdapter1, WWCPCPOAdapter WWCPEMPAdapter2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(WWCPEMPAdapter1, WWCPEMPAdapter2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) WWCPEMPAdapter1 == null) || ((Object) WWCPEMPAdapter2 == null))
                return false;

            return WWCPEMPAdapter1.Equals(WWCPEMPAdapter2);

        }

        #endregion

        #region Operator != (WWCPEMPAdapter1, WWCPEMPAdapter2)

        /// <summary>
        /// Compares two WWCPEMPAdapters for inequality.
        /// </summary>
        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (WWCPCPOAdapter WWCPEMPAdapter1, WWCPCPOAdapter WWCPEMPAdapter2)

            => !(WWCPEMPAdapter1 == WWCPEMPAdapter2);

        #endregion

        #region Operator <  (WWCPEMPAdapter1, WWCPEMPAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (WWCPCPOAdapter  WWCPEMPAdapter1,
                                          WWCPCPOAdapter  WWCPEMPAdapter2)
        {

            if ((Object) WWCPEMPAdapter1 == null)
                throw new ArgumentNullException(nameof(WWCPEMPAdapter1),  "The given WWCPEMPAdapter must not be null!");

            return WWCPEMPAdapter1.CompareTo(WWCPEMPAdapter2) < 0;

        }

        #endregion

        #region Operator <= (WWCPEMPAdapter1, WWCPEMPAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (WWCPCPOAdapter WWCPEMPAdapter1,
                                           WWCPCPOAdapter WWCPEMPAdapter2)

            => !(WWCPEMPAdapter1 > WWCPEMPAdapter2);

        #endregion

        #region Operator >  (WWCPEMPAdapter1, WWCPEMPAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (WWCPCPOAdapter WWCPEMPAdapter1,
                                          WWCPCPOAdapter WWCPEMPAdapter2)
        {

            if ((Object) WWCPEMPAdapter1 == null)
                throw new ArgumentNullException(nameof(WWCPEMPAdapter1),  "The given WWCPEMPAdapter must not be null!");

            return WWCPEMPAdapter1.CompareTo(WWCPEMPAdapter2) > 0;

        }

        #endregion

        #region Operator >= (WWCPEMPAdapter1, WWCPEMPAdapter2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPEMPAdapter1">A WWCPEMPAdapter.</param>
        /// <param name="WWCPEMPAdapter2">Another WWCPEMPAdapter.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (WWCPCPOAdapter WWCPEMPAdapter1,
                                           WWCPCPOAdapter WWCPEMPAdapter2)

            => !(WWCPEMPAdapter1 < WWCPEMPAdapter2);

        #endregion

        #endregion

        #region IComparable<WWCPEMPAdapter> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public override Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            var WWCPEMPAdapter = Object as WWCPCPOAdapter;
            if ((Object) WWCPEMPAdapter == null)
                throw new ArgumentException("The given object is not an WWCPEMPAdapter!", nameof(Object));

            return CompareTo(WWCPEMPAdapter);

        }

        #endregion

        #region CompareTo(WWCPEMPAdapter)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="WWCPEMPAdapter">An WWCPEMPAdapter object to compare with.</param>
        public Int32 CompareTo(WWCPCPOAdapter WWCPEMPAdapter)
        {

            if ((Object) WWCPEMPAdapter == null)
                throw new ArgumentNullException(nameof(WWCPEMPAdapter), "The given WWCPEMPAdapter must not be null!");

            return Id.CompareTo(WWCPEMPAdapter.Id);

        }

        #endregion

        #endregion

        #region IEquatable<WWCPEMPAdapter> Members

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

            var WWCPEMPAdapter = Object as WWCPCPOAdapter;
            if ((Object) WWCPEMPAdapter == null)
                return false;

            return Equals(WWCPEMPAdapter);

        }

        #endregion

        #region Equals(WWCPEMPAdapter)

        /// <summary>
        /// Compares two WWCPEMPAdapter for equality.
        /// </summary>
        /// <param name="WWCPEMPAdapter">An WWCPEMPAdapter to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(WWCPCPOAdapter WWCPEMPAdapter)
        {

            if ((Object) WWCPEMPAdapter == null)
                return false;

            return Id.Equals(WWCPEMPAdapter.Id);

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

            => "OCHP" + Version.Number + " EMP Adapter " + Id;

        #endregion


    }

}
