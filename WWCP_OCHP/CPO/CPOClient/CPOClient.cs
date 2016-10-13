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
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// An OCHP CPO Client.
    /// </summary>
    public partial class CPOClient : ASOAPClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const           String  DefaultHTTPUserAgent  = "GraphDefined OCHP " + Version.Number + " CPO Client";

        /// <summary>
        /// The default remote TCP port to connect to.
        /// </summary>
        public new static readonly IPPort  DefaultRemotePort     = IPPort.Parse(443);

        #endregion

        #region Properties

        /// <summary>
        /// The attached OCHP CPO client (HTTP/SOAP client) logger.
        /// </summary>
        public CPOClientLogger                              Logger                       { get; }

        public RoamingNetwork                               RoamingNetwork               { get; }

        public ChargingStationOperatorNameSelectorDelegate  DefaultOperatorNameSelector  { get; }

        #endregion

        #region Events

        #region OnSetChargePointListRequest/-Response

        /// <summary>
        /// An event fired whenever a request uploading charge points will be send.
        /// </summary>
        public event OnSetChargePointListRequestDelegate   OnSetChargePointListRequest;

        /// <summary>
        /// An event fired whenever a SOAP request uploading charge points will be send.
        /// </summary>
        public event ClientRequestLogHandler               OnSetChargePointListSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a upload charge points SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler              OnSetChargePointListSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a upload charge points request had been received.
        /// </summary>
        public event OnSetChargePointListResponseDelegate  OnSetChargePointListResponse;

        #endregion

        #region OnUpdateChargePointListRequest/-Response

        /// <summary>
        /// An event fired whenever a request uploading charge point updates will be send.
        /// </summary>
        public event OnUpdateChargePointListRequestDelegate   OnUpdateChargePointListRequest;

        /// <summary>
        /// An event fired whenever a SOAP request uploading charge point updates will be send.
        /// </summary>
        public event ClientRequestLogHandler                  OnUpdateChargePointListSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a upload charge point updates SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                 OnUpdateChargePointListSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a upload charge point updates request had been received.
        /// </summary>
        public event OnUpdateChargePointListResponseDelegate  OnUpdateChargePointListResponse;

        #endregion

        #region OnUpdateStatusRequest/-Response

        /// <summary>
        /// An event fired whenever evse and parking status will be send.
        /// </summary>
        public event OnUpdateStatusRequestDelegate   OnUpdateStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for evse and parking status will be send.
        /// </summary>
        public event ClientRequestLogHandler         OnUpdateStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an evse and parking status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler        OnUpdateStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an evse and parking status request had been received.
        /// </summary>
        public event OnUpdateStatusResponseDelegate  OnUpdateStatusResponse;

        #endregion


        #region OnGetSingleRoamingAuthorisationRequest/-Response

        /// <summary>
        /// An event fired whenever a request authenticating an e-mobility token will be send.
        /// </summary>
        public event OnGetSingleRoamingAuthorisationRequestDelegate   OnGetSingleRoamingAuthorisationRequest;

        /// <summary>
        /// An event fired whenever a SOAP request authenticating an e-mobility token will be send.
        /// </summary>
        public event ClientRequestLogHandler                          OnGetSingleRoamingAuthorisationSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a authenticate an e-mobility token SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                         OnGetSingleRoamingAuthorisationSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a authenticate an e-mobility token request had been received.
        /// </summary>
        public event OnGetSingleRoamingAuthorisationResponseDelegate  OnGetSingleRoamingAuthorisationResponse;

        #endregion

        #region OnGetRoamingAuthorisationListRequest/-Response

        /// <summary>
        /// An event fired whenever a request for the current roaming authorisation list will be send.
        /// </summary>
        public event OnGetRoamingAuthorisationListRequestDelegate   OnGetRoamingAuthorisationListRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for the current roaming authorisation list will be send.
        /// </summary>
        public event ClientRequestLogHandler                        OnGetRoamingAuthorisationListSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a SOAP request for the current roaming authorisation list had been received.
        /// </summary>
        public event ClientResponseLogHandler                       OnGetRoamingAuthorisationListSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a request for the current roaming authorisation list had been received.
        /// </summary>
        public event OnGetRoamingAuthorisationListResponseDelegate  OnGetRoamingAuthorisationListResponse;

        #endregion

        #region OnGetRoamingAuthorisationListUpdatesRequest/-Response

        /// <summary>
        /// An event fired whenever a request for updates of a roaming authorisation list will be send.
        /// </summary>
        public event OnGetRoamingAuthorisationListUpdatesRequestDelegate   OnGetRoamingAuthorisationListUpdatesRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for updates of a roaming authorisation list will be send.
        /// </summary>
        public event ClientRequestLogHandler                               OnGetRoamingAuthorisationListUpdatesSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a roaming authorisation list update SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                              OnGetRoamingAuthorisationListUpdatesSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a roaming authorisation list update request had been received.
        /// </summary>
        public event OnGetRoamingAuthorisationListUpdatesResponseDelegate  OnGetRoamingAuthorisationListUpdatesResponse;

        #endregion


        #region OnAddCDRsRequest/-Response

        /// <summary>
        /// An event fired whenever a request for adding charge detail records will be send.
        /// </summary>
        public event OnAddCDRsRequestDelegate   OnAddCDRsRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for adding charge detail records will be send.
        /// </summary>
        public event ClientRequestLogHandler    OnAddCDRsSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an add charge detail records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler   OnAddCDRsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an add charge detail records request had been received.
        /// </summary>
        public event OnAddCDRsResponseDelegate  OnAddCDRsResponse;

        #endregion

        #endregion

        #region Constructor(s)

        #region CPOClient(ClientId, Hostname, ..., LoggingContext = CPOClientLogger.DefaultContext, ...)

        /// <summary>
        /// Create a new OCHP CPO Client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The hostname of the remote OCHP service.</param>
        /// <param name="RemotePort">An optional TCP port of the remote OCHP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the remote OCHP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="LoggingContext">An optional context for logging client methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CPOClient(String                               ClientId,
                         String                               Hostname,
                         IPPort                               RemotePort                  = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                         X509Certificate                      ClientCert                  = null,
                         String                               HTTPVirtualHost             = null,
                         String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                         TimeSpan?                            QueryTimeout                = null,
                         DNSClient                            DNSClient                   = null,
                         String                               LoggingContext              = CPOClientLogger.DefaultContext,
                         Func<String, String, String>         LogFileCreator              = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCert,
                   HTTPVirtualHost,
                   HTTPUserAgent,
                   QueryTimeout,
                   DNSClient)

        {

            #region Initial checks

            if (ClientId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Logger),    "The given client identification must not be null or empty!");

            if (Hostname.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Hostname),  "The given hostname must not be null or empty!");

            #endregion

            this.Logger                       = new CPOClientLogger(this,
                                                                    LoggingContext,
                                                                    LogFileCreator);

            this.DefaultOperatorNameSelector  = I18N => I18N.FirstText;

        }

        #endregion

        #region CPOClient(ClientId, Logger, Hostname, ...)

        /// <summary>
        /// Create a new OCHP CPO Client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The hostname of the remote OCHP service.</param>
        /// <param name="RemotePort">An optional TCP port of the remote OCHP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual hostname of the remote OCHP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPOClient(String                               ClientId,
                         CPOClientLogger                      Logger,
                         String                               Hostname,
                         IPPort                               RemotePort                  = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                         X509Certificate                      ClientCert                  = null,
                         String                               HTTPVirtualHost             = null,
                         String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                         TimeSpan?                            QueryTimeout                = null,
                         DNSClient                            DNSClient                   = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCert,
                   HTTPVirtualHost,
                   HTTPUserAgent,
                   QueryTimeout,
                   DNSClient)

        {

            #region Initial checks

            if (ClientId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Logger),    "The given client identification must not be null or empty!");

            if (Logger == null)
                throw new ArgumentNullException(nameof(Logger),    "The given mobile client logger must not be null!");

            if (Hostname.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Hostname),  "The given hostname must not be null or empty!");

            #endregion

            this.Logger                       = Logger;
            this.DefaultOperatorNameSelector  = I18N => I18N.FirstText;

        }

        #endregion

        #endregion


        #region SetChargePointList(ChargePointInfos, ...)

        /// <summary>
        /// Upload the given enumeration of charge point infos.
        /// </summary>
        /// <param name="ChargePointInfos">An enumeration of charge point infos.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<SetChargePointListResponse>>

            SetChargePointList(IEnumerable<ChargePointInfo>  ChargePointInfos,

                               DateTime?                     Timestamp          = null,
                               CancellationToken?            CancellationToken  = null,
                               EventTracking_Id              EventTrackingId    = null,
                               TimeSpan?                     RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargePointInfos == null)
                throw new ArgumentNullException(nameof(ChargePointInfos),  "The given enumeration of charge point infos must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Get effective number of charge point infos to upload

            var NumberOfChargePoints = ChargePointInfos.Count();

            HTTPResponse<SetChargePointListResponse> result = null;

            #endregion

            #region Send OnSetChargePointListRequest event

            try
            {

                OnSetChargePointListRequest?.Invoke(DateTime.Now,
                                                    Timestamp.Value,
                                                    this,
                                                    ClientId,
                                                    EventTrackingId,
                                                    ChargePointInfos,
                                                    (UInt32) NumberOfChargePoints,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSetChargePointListRequest));
            }

            #endregion


            if (NumberOfChargePoints > 0)
            {

                using (var _OCHPClient = new SOAPClient(Hostname,
                                                        RemotePort,
                                                        HTTPVirtualHost,
                                                        "/service/ochp/v1.4",
                                                        RemoteCertificateValidator,
                                                        ClientCert,
                                                        UserAgent,
                                                        DNSClient))
                {

                    result = await _OCHPClient.Query(CPOClientXMLMethods.SetChargePointListRequestXML(ChargePointInfos),
                                                     "SetChargePointListRequest",
                                                     RequestLogDelegate:   OnSetChargePointListSOAPRequest,
                                                     ResponseLogDelegate:  OnSetChargePointListSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(SetChargePointListResponse.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<SetChargePointListResponse>(httpresponse,
                                                                                                             new SetChargePointListResponse(
                                                                                                                 Result.Format(
                                                                                                                     "Invalid SOAP => " +
                                                                                                                     httpresponse.HTTPBody.ToUTF8String()
                                                                                                                 )
                                                                                                             ),
                                                                                                             IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);

                                                         return new HTTPResponse<SetChargePointListResponse>(httpresponse,
                                                                                                             new SetChargePointListResponse(
                                                                                                                 Result.Server(
                                                                                                                      httpresponse.HTTPStatusCode.ToString() +
                                                                                                                      " => " +
                                                                                                                      httpresponse.HTTPBody.      ToUTF8String()
                                                                                                                 )
                                                                                                             ),
                                                                                                             IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<SetChargePointListResponse>.ExceptionThrown(new SetChargePointListResponse(
                                                                                                                             Result.Format(exception.Message +
                                                                                                                                           " => " +
                                                                                                                                           exception.StackTrace)),
                                                                                                                         exception);

                                                     }

                                                     #endregion

                                                    );

                }

            }


            if (result == null)
                result = HTTPResponse<SetChargePointListResponse>.OK(new SetChargePointListResponse(Result.OK("Nothing to upload!")));


            #region Send OnSetChargePointListResponse event

            try
            {

                OnSetChargePointListResponse?.Invoke(DateTime.Now,
                                                     Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     EventTrackingId,
                                                     ChargePointInfos,
                                                     (UInt32) NumberOfChargePoints,
                                                     RequestTimeout,
                                                     result.Content,
                                                     DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSetChargePointListResponse));
            }

            #endregion


            return result;

        }

        #endregion

        #region UpdateChargePointList(ChargePointInfos, ...)

        /// <summary>
        /// Upload the given enumeration of charge point info updates.
        /// </summary>
        /// <param name="ChargePointInfos">An enumeration of charge point info updates.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<UpdateChargePointListResponse>>

            UpdateChargePointList(IEnumerable<ChargePointInfo>  ChargePointInfos,

                                  DateTime?                     Timestamp          = null,
                                  CancellationToken?            CancellationToken  = null,
                                  EventTracking_Id              EventTrackingId    = null,
                                  TimeSpan?                     RequestTimeout     = null)

        {

            #region Initial checks

            if (ChargePointInfos == null)
                throw new ArgumentNullException(nameof(ChargePointInfos),  "The given enumeration of charge point info updates must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;

            #endregion

            #region Get effective number of charge point infos to upload

            var NumberOfChargePoints = ChargePointInfos.Count();

            HTTPResponse<UpdateChargePointListResponse> result = null;

            #endregion

            #region Send OnUpdateChargePointListRequest event

            try
            {

                OnUpdateChargePointListRequest?.Invoke(DateTime.Now,
                                                       Timestamp.Value,
                                                       this,
                                                       ClientId,
                                                       EventTrackingId,
                                                       ChargePointInfos,
                                                       (UInt32) NumberOfChargePoints,
                                                       RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnUpdateChargePointListRequest));
            }

            #endregion


            if (NumberOfChargePoints > 0)
            {

                using (var _OCHPClient = new SOAPClient(Hostname,
                                                        RemotePort,
                                                        HTTPVirtualHost,
                                                        "/service/ochp/v1.4",
                                                        RemoteCertificateValidator,
                                                        ClientCert,
                                                        UserAgent,
                                                        DNSClient))
                {

                    result = await _OCHPClient.Query(CPOClientXMLMethods.UpdateChargePointListRequestXML(ChargePointInfos),
                                                     "UpdateChargePointListRequest",
                                                     RequestLogDelegate:   OnUpdateChargePointListSOAPRequest,
                                                     ResponseLogDelegate:  OnUpdateChargePointListSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(UpdateChargePointListResponse.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<UpdateChargePointListResponse>(httpresponse,
                                                                                                             new UpdateChargePointListResponse(
                                                                                                                 Result.Format(
                                                                                                                     "Invalid SOAP => " +
                                                                                                                     httpresponse.HTTPBody.ToUTF8String()
                                                                                                                 )
                                                                                                             ),
                                                                                                             IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);

                                                         return new HTTPResponse<UpdateChargePointListResponse>(httpresponse,
                                                                                                             new UpdateChargePointListResponse(
                                                                                                                 Result.Server(
                                                                                                                      httpresponse.HTTPStatusCode.ToString() +
                                                                                                                      " => " +
                                                                                                                      httpresponse.HTTPBody.      ToUTF8String()
                                                                                                                 )
                                                                                                             ),
                                                                                                             IsFault: true);

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<UpdateChargePointListResponse>.ExceptionThrown(new UpdateChargePointListResponse(
                                                                                                                                Result.Format(exception.Message +
                                                                                                                                              " => " +
                                                                                                                                              exception.StackTrace)),
                                                                                                                            exception);

                                                     }

                                                     #endregion

                                                    );

                }

            }


            if (result == null)
                result = HTTPResponse<UpdateChargePointListResponse>.OK(new UpdateChargePointListResponse(Result.OK("Nothing to upload!")));


            #region Send OnUpdateChargePointListResponse event

            try
            {

                OnUpdateChargePointListResponse?.Invoke(DateTime.Now,
                                                        Timestamp.Value,
                                                        this,
                                                        ClientId,
                                                        EventTrackingId,
                                                        ChargePointInfos,
                                                        (UInt32) NumberOfChargePoints,
                                                        RequestTimeout,
                                                        result.Content,
                                                        DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnUpdateChargePointListResponse));
            }

            #endregion


            return result;

        }

        #endregion

        #region AddCDRsRequest(EVSEStatus, ParkingStatus = null, DefaultTTL = null, ...)

        /// <summary>
        /// Upload the given enumeration of charge detail records.
        /// </summary>
        /// <param name="EVSEStatus">An enumeration of EVSE status.</param>
        /// <param name="ParkingStatus">An enumeration of parking status.</param>
        /// <param name="DefaultTTL">The default time to live for these status.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<UpdateStatusResponse>>

            UpdateStatusRequest(IEnumerable<EVSEStatus>     EVSEStatus         = null,
                                IEnumerable<ParkingStatus>  ParkingStatus      = null,
                                DateTime?                   DefaultTTL         = null,

                                DateTime?                   Timestamp          = null,
                                CancellationToken?          CancellationToken  = null,
                                EventTracking_Id            EventTrackingId    = null,
                                TimeSpan?                   RequestTimeout     = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            HTTPResponse<UpdateStatusResponse> result = null;

            #endregion

            #region Send OnUpdateStatusRequest event

            try
            {

                OnUpdateStatusRequest?.Invoke(DateTime.Now,
                                              Timestamp.Value,
                                              this,
                                              ClientId,
                                              EventTrackingId,
                                              EVSEStatus,
                                              ParkingStatus,
                                              DefaultTTL,
                                              RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnUpdateStatusRequest));
            }

            #endregion


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    "/service/ochp/v1.4",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(CPOClientXMLMethods.UpdateStatusXML(EVSEStatus,
                                                                                     ParkingStatus,
                                                                                     DefaultTTL),
                                                 "UpdateStatusRequest",
                                                 RequestLogDelegate:   OnAddCDRsSOAPRequest,
                                                 ResponseLogDelegate:  OnAddCDRsSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(UpdateStatusResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<UpdateStatusResponse>(httpresponse,
                                                                                                   new UpdateStatusResponse(
                                                                                                       Result.Format(
                                                                                                           "Invalid SOAP => " +
                                                                                                           httpresponse.HTTPBody.ToUTF8String()
                                                                                                       )
                                                                                                   ),
                                                                                                   IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, this, httpresponse);

                                                     return new HTTPResponse<UpdateStatusResponse>(httpresponse,
                                                                                                   new UpdateStatusResponse(
                                                                                                       Result.Server(
                                                                                                            httpresponse.HTTPStatusCode.ToString() +
                                                                                                            " => " +
                                                                                                            httpresponse.HTTPBody.      ToUTF8String()
                                                                                                       )
                                                                                                   ),
                                                                                                   IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<UpdateStatusResponse>.ExceptionThrown(new UpdateStatusResponse(
                                                                                                                   Result.Format(exception.Message +
                                                                                                                                 " => " +
                                                                                                                                 exception.StackTrace)),
                                                                                                               exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<UpdateStatusResponse>.OK(new UpdateStatusResponse(Result.OK("Nothing to upload!")));


            #region Send OnAddCDRsResponse event

            try
            {

                OnUpdateStatusResponse?.Invoke(DateTime.Now,
                                               Timestamp.Value,
                                               this,
                                               ClientId,
                                               EventTrackingId,
                                               EVSEStatus,
                                               ParkingStatus,
                                               DefaultTTL,
                                               RequestTimeout,
                                               result.Content,
                                               DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnUpdateStatusResponse));
            }

            #endregion


            return result;

        }

        #endregion


        #region GetSingleRoamingAuthorisation(EMTId, ...)

        /// <summary>
        /// Authenticate the given e-mobility token.
        /// </summary>
        /// <param name="EMTId">An e-mobility token.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<GetSingleRoamingAuthorisationResponse>>

            GetSingleRoamingAuthorisation(EMT_Id              EMTId,

                                          DateTime?           Timestamp          = null,
                                          CancellationToken?  CancellationToken  = null,
                                          EventTracking_Id    EventTrackingId    = null,
                                          TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (EMTId == null)
                throw new ArgumentNullException(nameof(EMTId),  "The given e-mobility token must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            HTTPResponse<GetSingleRoamingAuthorisationResponse> result = null;

            #endregion

            #region Send OnGetSingleRoamingAuthorisationRequest event

            try
            {

                OnGetSingleRoamingAuthorisationRequest?.Invoke(DateTime.Now,
                                                               Timestamp.Value,
                                                               this,
                                                               ClientId,
                                                               EventTrackingId,
                                                               EMTId,
                                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnGetSingleRoamingAuthorisationRequest));
            }

            #endregion


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    "/service/ochp/v1.4",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(CPOClientXMLMethods.GetSingleRoamingAuthorisationXML(EMTId),
                                                 "GetSingleRoamingAuthorisationRequest",
                                                 RequestLogDelegate:   OnGetSingleRoamingAuthorisationSOAPRequest,
                                                 ResponseLogDelegate:  OnGetSingleRoamingAuthorisationSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(GetSingleRoamingAuthorisationResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<GetSingleRoamingAuthorisationResponse>(httpresponse,
                                                                                                                    new GetSingleRoamingAuthorisationResponse(
                                                                                                                        Result.Format(
                                                                                                                            "Invalid SOAP => " +
                                                                                                                            httpresponse.HTTPBody.ToUTF8String()
                                                                                                                        )
                                                                                                                    ),
                                                                                                                    IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, this, httpresponse);

                                                     return new HTTPResponse<GetSingleRoamingAuthorisationResponse>(httpresponse,
                                                                                                                    new GetSingleRoamingAuthorisationResponse(
                                                                                                                        Result.Server(
                                                                                                                             httpresponse.HTTPStatusCode.ToString() +
                                                                                                                             " => " +
                                                                                                                             httpresponse.HTTPBody.      ToUTF8String()
                                                                                                                        )
                                                                                                                    ),
                                                                                                                    IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<GetSingleRoamingAuthorisationResponse>.ExceptionThrown(new GetSingleRoamingAuthorisationResponse(
                                                                                                                                    Result.Format(exception.Message +
                                                                                                                                                  " => " +
                                                                                                                                                  exception.StackTrace)),
                                                                                                                                exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<GetSingleRoamingAuthorisationResponse>.OK(new GetSingleRoamingAuthorisationResponse(Result.OK("Nothing to upload!")));


            #region Send OnGetSingleRoamingAuthorisationResponse event

            try
            {

                OnGetSingleRoamingAuthorisationResponse?.Invoke(DateTime.Now,
                                                                Timestamp.Value,
                                                                this,
                                                                ClientId,
                                                                EventTrackingId,
                                                                EMTId,
                                                                RequestTimeout,
                                                                result.Content,
                                                                DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnGetSingleRoamingAuthorisationResponse));
            }

            #endregion


            return result;

        }

        #endregion

        #region GetRoamingAuthorisationListRequest(...)

        /// <summary>
        /// Get the entire current version of the roaming authorisation list.
        /// </summary>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<GetRoamingAuthorisationListResponse>>

            GetRoamingAuthorisationListRequest(DateTime?           Timestamp          = null,
                                               CancellationToken?  CancellationToken  = null,
                                               EventTracking_Id    EventTrackingId    = null,
                                               TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            HTTPResponse<GetRoamingAuthorisationListResponse> result = null;

            #endregion

            #region Send OnGetRoamingAuthorisationListRequest event

            try
            {

                OnGetRoamingAuthorisationListRequest?.Invoke(DateTime.Now,
                                                             Timestamp.Value,
                                                             this,
                                                             ClientId,
                                                             EventTrackingId,
                                                             RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnGetRoamingAuthorisationListRequest));
            }

            #endregion


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    "/service/ochp/v1.4",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(CPOClientXMLMethods.GetRoamingAuthorisationListXML(),
                                                 "GetRoamingAuthorisationListRequest",
                                                 RequestLogDelegate:   OnGetRoamingAuthorisationListSOAPRequest,
                                                 ResponseLogDelegate:  OnGetRoamingAuthorisationListSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(GetRoamingAuthorisationListResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<GetRoamingAuthorisationListResponse>(httpresponse,
                                                                                                                  new GetRoamingAuthorisationListResponse(
                                                                                                                      Result.Format(
                                                                                                                          "Invalid SOAP => " +
                                                                                                                          httpresponse.HTTPBody.ToUTF8String()
                                                                                                                      )
                                                                                                                  ),
                                                                                                                  IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, this, httpresponse);

                                                     return new HTTPResponse<GetRoamingAuthorisationListResponse>(httpresponse,
                                                                                                                  new GetRoamingAuthorisationListResponse(
                                                                                                                      Result.Server(
                                                                                                                           httpresponse.HTTPStatusCode.ToString() +
                                                                                                                           " => " +
                                                                                                                           httpresponse.HTTPBody.      ToUTF8String()
                                                                                                                      )
                                                                                                                  ),
                                                                                                                  IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<GetRoamingAuthorisationListResponse>.ExceptionThrown(new GetRoamingAuthorisationListResponse(
                                                                                                                                  Result.Format(exception.Message +
                                                                                                                                                " => " +
                                                                                                                                                exception.StackTrace)),
                                                                                                                              exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<GetRoamingAuthorisationListResponse>.OK(new GetRoamingAuthorisationListResponse(Result.OK("Nothing to upload!")));


            #region Send OnGetRoamingAuthorisationListResponse event

            try
            {

                OnGetRoamingAuthorisationListResponse?.Invoke(DateTime.Now,
                                                              Timestamp.Value,
                                                              this,
                                                              ClientId,
                                                              EventTrackingId,
                                                              RequestTimeout,
                                                              result.Content,
                                                              DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnGetRoamingAuthorisationListResponse));
            }

            #endregion


            return result;

        }

        #endregion

        #region GetRoamingAuthorisationListUpdatesRequest(...)

        /// <summary>
        /// Get the entire current version of the roaming authorisation list.
        /// </summary>
        /// <param name="LastUpdate">The timestamp of the last roaming authorisation list update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>>

            GetRoamingAuthorisationListUpdatesRequest(DateTime            LastUpdate,

                                                      DateTime?           Timestamp          = null,
                                                      CancellationToken?  CancellationToken  = null,
                                                      EventTracking_Id    EventTrackingId    = null,
                                                      TimeSpan?           RequestTimeout     = null)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            HTTPResponse<GetRoamingAuthorisationListUpdatesResponse> result = null;

            #endregion

            #region Send OnGetRoamingAuthorisationListUpdatesRequest event

            try
            {

                OnGetRoamingAuthorisationListUpdatesRequest?.Invoke(DateTime.Now,
                                                                    Timestamp.Value,
                                                                    this,
                                                                    ClientId,
                                                                    EventTrackingId,
                                                                    LastUpdate,
                                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnGetRoamingAuthorisationListUpdatesRequest));
            }

            #endregion


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    "/service/ochp/v1.4",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(CPOClientXMLMethods.GetRoamingAuthorisationListUpdatesXML(LastUpdate),
                                                 "GetRoamingAuthorisationListUpdatesRequest",
                                                 RequestLogDelegate:   OnGetRoamingAuthorisationListUpdatesSOAPRequest,
                                                 ResponseLogDelegate:  OnGetRoamingAuthorisationListUpdatesSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(GetRoamingAuthorisationListUpdatesResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>(httpresponse,
                                                                                                                  new GetRoamingAuthorisationListUpdatesResponse(
                                                                                                                      Result.Format(
                                                                                                                          "Invalid SOAP => " +
                                                                                                                          httpresponse.HTTPBody.ToUTF8String()
                                                                                                                      )
                                                                                                                  ),
                                                                                                                  IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, this, httpresponse);

                                                     return new HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>(httpresponse,
                                                                                                                  new GetRoamingAuthorisationListUpdatesResponse(
                                                                                                                      Result.Server(
                                                                                                                           httpresponse.HTTPStatusCode.ToString() +
                                                                                                                           " => " +
                                                                                                                           httpresponse.HTTPBody.      ToUTF8String()
                                                                                                                      )
                                                                                                                  ),
                                                                                                                  IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>.ExceptionThrown(new GetRoamingAuthorisationListUpdatesResponse(
                                                                                                                                  Result.Format(exception.Message +
                                                                                                                                                " => " +
                                                                                                                                                exception.StackTrace)),
                                                                                                                              exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>.OK(new GetRoamingAuthorisationListUpdatesResponse(Result.OK("Nothing to upload!")));


            #region Send OnGetRoamingAuthorisationListUpdatesResponse event

            try
            {

                OnGetRoamingAuthorisationListUpdatesResponse?.Invoke(DateTime.Now,
                                                                     Timestamp.Value,
                                                                     this,
                                                                     ClientId,
                                                                     EventTrackingId,
                                                                     LastUpdate,
                                                                     RequestTimeout,
                                                                     result.Content,
                                                                     DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnGetRoamingAuthorisationListUpdatesResponse));
            }

            #endregion


            return result;

        }

        #endregion


        #region AddCDRsRequest(...)

        /// <summary>
        /// Upload the given enumeration of charge detail records.
        /// </summary>
        /// <param name="CDRInfos">An enumeration of charge detail records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<AddCDRsResponse>>

            AddCDRsRequest(IEnumerable<CDRInfo>  CDRInfos,

                           DateTime?             Timestamp          = null,
                           CancellationToken?    CancellationToken  = null,
                           EventTracking_Id      EventTrackingId    = null,
                           TimeSpan?             RequestTimeout     = null)

        {

            #region Initial checks

            if (CDRInfos == null || !CDRInfos.Any())
                throw new ArgumentNullException(nameof(CDRInfos),  "The given enumeration of charge detail records must not be null or empty!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            HTTPResponse<AddCDRsResponse> result = null;

            #endregion

            #region Send OnAddCDRsRequest event

            try
            {

                OnAddCDRsRequest?.Invoke(DateTime.Now,
                                         Timestamp.Value,
                                         this,
                                         ClientId,
                                         EventTrackingId,
                                         CDRInfos,
                                         RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAddCDRsRequest));
            }

            #endregion


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    "/service/ochp/v1.4",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(CPOClientXMLMethods.AddCDRsXML(CDRInfos),
                                                 "AddCDRsRequest",
                                                 RequestLogDelegate:   OnAddCDRsSOAPRequest,
                                                 ResponseLogDelegate:  OnAddCDRsSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(AddCDRsResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<AddCDRsResponse>(httpresponse,
                                                                                              new AddCDRsResponse(
                                                                                                  Result.Format(
                                                                                                      "Invalid SOAP => " +
                                                                                                      httpresponse.HTTPBody.ToUTF8String()
                                                                                                  )
                                                                                              ),
                                                                                              IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, this, httpresponse);

                                                     return new HTTPResponse<AddCDRsResponse>(httpresponse,
                                                                                              new AddCDRsResponse(
                                                                                                  Result.Server(
                                                                                                       httpresponse.HTTPStatusCode.ToString() +
                                                                                                       " => " +
                                                                                                       httpresponse.HTTPBody.      ToUTF8String()
                                                                                                  )
                                                                                              ),
                                                                                              IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<AddCDRsResponse>.ExceptionThrown(new AddCDRsResponse(
                                                                                                              Result.Format(exception.Message +
                                                                                                                            " => " +
                                                                                                                            exception.StackTrace)),
                                                                                                          exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<AddCDRsResponse>.OK(new AddCDRsResponse(Result.OK("Nothing to upload!")));


            #region Send OnAddCDRsResponse event

            try
            {

                OnAddCDRsResponse?.Invoke(DateTime.Now,
                                          Timestamp.Value,
                                          this,
                                          ClientId,
                                          EventTrackingId,
                                          CDRInfos,
                                          RequestTimeout,
                                          result.Content,
                                          DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAddCDRsResponse));
            }

            #endregion


            return result;

        }

        #endregion


    }

}
