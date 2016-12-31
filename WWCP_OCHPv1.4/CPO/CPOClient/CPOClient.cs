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

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
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

        /// <summary>
        /// The default URI prefix.
        /// </summary>
        public const               String  DefaultURIPrefix      = "/service/ochp/v1.4/";

        #endregion

        #region Properties

        /// <summary>
        /// The attached OCHP CPO client (HTTP/SOAP client) logger.
        /// </summary>
        public CPOClientLogger  Logger           { get; }

        public RoamingNetwork   RoamingNetwork   { get; }

        #endregion

        #region Events

        // OCHP

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

        #region OnCheckCDRsRequest/-Response

        /// <summary>
        /// An event fired whenever a request for checking charge detail records will be send.
        /// </summary>
        public event OnCheckCDRsRequestDelegate   OnCheckCDRsRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for checking charge detail records will be send.
        /// </summary>
        public event ClientRequestLogHandler      OnCheckCDRsSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a check charge detail records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler     OnCheckCDRsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a check charge detail records request had been received.
        /// </summary>
        public event OnCheckCDRsResponseDelegate  OnCheckCDRsResponse;

        #endregion


        #region OnUpdateTariffsRequest/-Response

        /// <summary>
        /// An event fired whenever tariff updates will be send.
        /// </summary>
        public event OnUpdateTariffsRequestDelegate   OnUpdateTariffsRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for updating tariffs will be send.
        /// </summary>
        public event ClientRequestLogHandler          OnUpdateTariffsSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to update tariffs SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler         OnUpdateTariffsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to update tariffs request had been received.
        /// </summary>
        public event OnUpdateTariffsResponseDelegate  OnUpdateTariffsResponse;

        #endregion


        // OCHPdirect

        #region OnAddServiceEndpointsRequest/-Response

        /// <summary>
        /// An event fired whenever a request to add service endpoints will be send.
        /// </summary>
        public event OnAddServiceEndpointsRequestDelegate   OnAddServiceEndpointsRequest;

        /// <summary>
        /// An event fired whenever a SOAP request to add service endpoints will be send.
        /// </summary>
        public event ClientRequestLogHandler                OnAddServiceEndpointsSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to add service endpoints had been received.
        /// </summary>
        public event ClientResponseLogHandler               OnAddServiceEndpointsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for request to add service endpoints had been received.
        /// </summary>
        public event OnAddServiceEndpointsResponseDelegate  OnAddServiceEndpointsResponse;

        #endregion

        #region OnGetServiceEndpointsRequest/-Response

        /// <summary>
        /// An event fired whenever a request to get service endpoints will be send.
        /// </summary>
        public event OnGetServiceEndpointsRequestDelegate   OnGetServiceEndpointsRequest;

        /// <summary>
        /// An event fired whenever a SOAP request to get service endpoints will be send.
        /// </summary>
        public event ClientRequestLogHandler                OnGetServiceEndpointsSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to get service endpoints had been received.
        /// </summary>
        public event ClientResponseLogHandler               OnGetServiceEndpointsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for request to get service endpoints had been received.
        /// </summary>
        public event OnGetServiceEndpointsResponseDelegate  OnGetServiceEndpointsResponse;

        #endregion


        #region OnInformProviderRequest/-Response

        /// <summary>
        /// An event fired whenever an inform provider message will be send.
        /// </summary>
        public event OnInformProviderRequestDelegate   OnInformProviderRequest;

        /// <summary>
        /// An event fired whenever an inform provider SOAP message will be send.
        /// </summary>
        public event ClientRequestLogHandler           OnInformProviderSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for an inform provider SOAP message had been received.
        /// </summary>
        public event ClientResponseLogHandler          OnInformProviderSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for an inform provider message had been received.
        /// </summary>
        public event OnInformProviderResponseDelegate  OnInformProviderResponse;

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
                         String                               URIPrefix                   = DefaultURIPrefix,
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
                   URIPrefix.Trim().IsNotNullOrEmpty() ? URIPrefix : DefaultURIPrefix,
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

            this.Logger  = new CPOClientLogger(this,
                                               LoggingContext,
                                               LogFileCreator);

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
                         String                               URIPrefix                   = DefaultURIPrefix,
                         String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                         TimeSpan?                            QueryTimeout                = null,
                         DNSClient                            DNSClient                   = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCert,
                   HTTPVirtualHost,
                   URIPrefix.Trim().IsNotNullOrEmpty() ? URIPrefix : DefaultURIPrefix,
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

            this.Logger = Logger;

        }

        #endregion

        #endregion


        // OCHP

        #region SetChargePointList   (ChargePointInfos, IncludeChargePoints = null, ...)

        /// <summary>
        /// Upload the given enumeration of charge points.
        /// </summary>
        /// <param name="ChargePointInfos">An enumeration of charge points.</param>
        /// <param name="IncludeChargePoints">An optional delegate for filtering charge points before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<SetChargePointListResponse>>

            SetChargePointList(IEnumerable<ChargePointInfo>  ChargePointInfos,
                               IncludeChargePointsDelegate   IncludeChargePoints  = null,

                               DateTime?                     Timestamp            = null,
                               CancellationToken?            CancellationToken    = null,
                               EventTracking_Id              EventTrackingId      = null,
                               TimeSpan?                     RequestTimeout       = null)

        {

            #region Initial checks

            if (ChargePointInfos == null)
                throw new ArgumentNullException(nameof(ChargePointInfos),  "The given enumeration of charge point infos must not be null!");

            if (IncludeChargePoints == null)
                IncludeChargePoints = chargepoint => true;


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


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
                                                    (UInt32) ChargePointInfos.Count(),
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSetChargePointListRequest));
            }

            #endregion


            var Request = new SetChargePointListRequest(ChargePointInfos.Where(chargepoint => IncludeChargePoints(chargepoint)));


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    DefaultURIPrefix,
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(LoginPassword.Item1, LoginPassword.Item2, Request.ToXML()),
                                                 "http://ochp.eu/1.4/SetChargepointList",
                                                 RequestLogDelegate:   OnSetChargePointListSOAPRequest,
                                                 ResponseLogDelegate:  OnSetChargePointListSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, SetChargePointListResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<SetChargePointListResponse>(httpresponse,
                                                                                                         new SetChargePointListResponse(
                                                                                                             Request,
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
                                                                                                             Request,
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
                                                                                                                         Request,
                                                                                                                         Result.Format(exception.Message +
                                                                                                                                       " => " +
                                                                                                                                       exception.StackTrace)),
                                                                                                                     exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<SetChargePointListResponse>.OK(new SetChargePointListResponse(Request, Result.OK("Nothing to upload!")));


            #region Send OnSetChargePointListResponse event

            try
            {

                OnSetChargePointListResponse?.Invoke(DateTime.Now,
                                                     Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     EventTrackingId,
                                                     ChargePointInfos,
                                                     (UInt32) ChargePointInfos.Count(),
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

        #region UpdateChargePointList(ChargePointInfos, IncludeChargePoints = null, ...)

        /// <summary>
        /// Upload the given enumeration of updated charge points.
        /// </summary>
        /// <param name="ChargePointInfos">An enumeration of updated charge points.</param>
        /// <param name="IncludeChargePoints">An optional delegate for filtering charge points before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<UpdateChargePointListResponse>>

            UpdateChargePointList(IEnumerable<ChargePointInfo>  ChargePointInfos,
                                  IncludeChargePointsDelegate   IncludeChargePoints  = null,

                                  DateTime?                     Timestamp            = null,
                                  CancellationToken?            CancellationToken    = null,
                                  EventTracking_Id              EventTrackingId      = null,
                                  TimeSpan?                     RequestTimeout       = null)

        {

            #region Initial checks

            if (ChargePointInfos == null)
                throw new ArgumentNullException(nameof(ChargePointInfos),  "The given enumeration of charge point info updates must not be null!");

            if (IncludeChargePoints == null)
                IncludeChargePoints = chargepoint => true;


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

            var NumberOfChargePoints = ChargePointInfos.Count(chargepoint => IncludeChargePoints(chargepoint));

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


            var Request = new UpdateChargePointListRequest(ChargePointInfos.Where(chargepoint => IncludeChargePoints(chargepoint)));


            if (NumberOfChargePoints > 0)
            {

                using (var _OCHPClient = new SOAPClient(Hostname,
                                                        RemotePort,
                                                        HTTPVirtualHost,
                                                        DefaultURIPrefix,
                                                        RemoteCertificateValidator,
                                                        ClientCert,
                                                        UserAgent,
                                                        DNSClient))
                {

                    result = await _OCHPClient.Query(SOAP.Encapsulation(LoginPassword.Item1, LoginPassword.Item2, Request.ToXML()),
                                                     "http://ochp.eu/1.4/UpdateChargePointList",
                                                     RequestLogDelegate:   OnUpdateChargePointListSOAPRequest,
                                                     ResponseLogDelegate:  OnUpdateChargePointListSOAPResponse,
                                                     CancellationToken:    CancellationToken,
                                                     EventTrackingId:      EventTrackingId,
                                                     QueryTimeout:         RequestTimeout,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, UpdateChargePointListResponse.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return new HTTPResponse<UpdateChargePointListResponse>(httpresponse,
                                                                                                             new UpdateChargePointListResponse(
                                                                                                                 Request,
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
                                                                                                                 Request,
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
                                                                                                                                Request,
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
                result = HTTPResponse<UpdateChargePointListResponse>.OK(new UpdateChargePointListResponse(Request, Result.OK("Nothing to upload!")));


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

        #region UpdateStatus(EVSEStatus = null, ParkingStatus = null, DefaultTTL = null, ...)

        /// <summary>
        /// Upload the given enumeration of EVSE and/or parking status.
        /// </summary>
        /// <param name="EVSEStatus">An optional enumeration of EVSE status.</param>
        /// <param name="ParkingStatus">An optional enumeration of parking status.</param>
        /// <param name="DefaultTTL">The default time to live for these status.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<UpdateStatusResponse>>

            UpdateStatus(IEnumerable<EVSEStatus>     EVSEStatus         = null,
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


            var Request = new UpdateStatusRequest(EVSEStatus,
                                                  ParkingStatus,
                                                  DefaultTTL);


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    "/live/ochp/v1.4",
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(LoginPassword.Item1, LoginPassword.Item2, Request.ToXML()),
                                                 "http://ochp.e-clearing.net/service/UpdateStatus",
                                                 RequestLogDelegate:   OnUpdateStatusSOAPRequest,
                                                 ResponseLogDelegate:  OnUpdateStatusSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, UpdateStatusResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<UpdateStatusResponse>(httpresponse,
                                                                                                   new UpdateStatusResponse(
                                                                                                       Request,
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
                                                                                                       Request,
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
                                                                                                                   Request,
                                                                                                                   Result.Format(exception.Message +
                                                                                                                                 " => " +
                                                                                                                                 exception.StackTrace)),
                                                                                                               exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<UpdateStatusResponse>.OK(new UpdateStatusResponse(Request, Result.OK("Nothing to upload!")));


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


            var Request = new GetSingleRoamingAuthorisationRequest(EMTId);


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    DefaultURIPrefix,
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(LoginPassword.Item1, LoginPassword.Item2, Request.ToXML()),
                                                 "http://ochp.eu/1.4/GetSingleRoamingAuthorisation",
                                                 RequestLogDelegate:   OnGetSingleRoamingAuthorisationSOAPRequest,
                                                 ResponseLogDelegate:  OnGetSingleRoamingAuthorisationSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, GetSingleRoamingAuthorisationResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<GetSingleRoamingAuthorisationResponse>(httpresponse,
                                                                                                                    new GetSingleRoamingAuthorisationResponse(
                                                                                                                        Request,
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
                                                                                                                        Request,
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
                                                                                                                                    Request,
                                                                                                                                    Result.Format(exception.Message +
                                                                                                                                                  " => " +
                                                                                                                                                  exception.StackTrace)),
                                                                                                                                exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<GetSingleRoamingAuthorisationResponse>.OK(new GetSingleRoamingAuthorisationResponse(Request, Result.OK("Nothing to upload!")));


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

        #region GetRoamingAuthorisationList(...)

        /// <summary>
        /// Get the entire current version of the roaming authorisation list.
        /// </summary>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<GetRoamingAuthorisationListResponse>>

            GetRoamingAuthorisationList(DateTime?           Timestamp          = null,
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


            var Request = new GetRoamingAuthorisationListRequest();


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    DefaultURIPrefix,
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(Request.ToXML()),
                                                 "GetRoamingAuthorisationListRequest",
                                                 RequestLogDelegate:   OnGetRoamingAuthorisationListSOAPRequest,
                                                 ResponseLogDelegate:  OnGetRoamingAuthorisationListSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, GetRoamingAuthorisationListResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<GetRoamingAuthorisationListResponse>(httpresponse,
                                                                                                                  new GetRoamingAuthorisationListResponse(
                                                                                                                      Request,
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
                                                                                                                      Request,
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
                                                                                                                                  Request,
                                                                                                                                  Result.Format(exception.Message +
                                                                                                                                                " => " +
                                                                                                                                                exception.StackTrace)),
                                                                                                                              exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<GetRoamingAuthorisationListResponse>.OK(new GetRoamingAuthorisationListResponse(Request, Result.OK("Nothing to upload!")));


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

        #region GetRoamingAuthorisationListUpdates(LastUpdate, ...)

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

            GetRoamingAuthorisationListUpdates(DateTime            LastUpdate,

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


            var Request = new GetRoamingAuthorisationListUpdatesRequest(LastUpdate);


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    DefaultURIPrefix,
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(Request.ToXML()),
                                                 "GetRoamingAuthorisationListUpdatesRequest",
                                                 RequestLogDelegate:   OnGetRoamingAuthorisationListUpdatesSOAPRequest,
                                                 ResponseLogDelegate:  OnGetRoamingAuthorisationListUpdatesSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, GetRoamingAuthorisationListUpdatesResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>(httpresponse,
                                                                                                                  new GetRoamingAuthorisationListUpdatesResponse(
                                                                                                                      Request,
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
                                                                                                                      Request,
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
                                                                                                                                         Request,
                                                                                                                                         Result.Format(exception.Message +
                                                                                                                                                       " => " +
                                                                                                                                                       exception.StackTrace)),
                                                                                                                                     exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>.OK(new GetRoamingAuthorisationListUpdatesResponse(Request, Result.OK("Nothing to upload!")));


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


        #region AddCDRs(CDRInfos, ...)

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

            AddCDRs(IEnumerable<CDRInfo>  CDRInfos,

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


            var Request = new AddCDRsRequest(CDRInfos);


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    DefaultURIPrefix,
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(Request.ToXML()),
                                                 "AddCDRsRequest",
                                                 RequestLogDelegate:   OnAddCDRsSOAPRequest,
                                                 ResponseLogDelegate:  OnAddCDRsSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, AddCDRsResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<AddCDRsResponse>(httpresponse,
                                                                                              new AddCDRsResponse(
                                                                                                  Request,
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
                                                                                                  Request,
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
                                                                                                              Request,
                                                                                                              Result.Format(exception.Message +
                                                                                                                            " => " +
                                                                                                                            exception.StackTrace)),
                                                                                                          exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<AddCDRsResponse>.OK(new AddCDRsResponse(Request, Result.OK("Nothing to upload!")));


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

        #region CheckCDRs(CDRStatus = null, ...)

        /// <summary>
        /// Check charge detail records having the given optional status.
        /// </summary>
        /// <param name="CDRStatus">The status of the requested charge detail records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<CheckCDRsResponse>>

            CheckCDRs(CDRStatus?            CDRStatus          = null,

                      DateTime?             Timestamp          = null,
                      CancellationToken?    CancellationToken  = null,
                      EventTracking_Id      EventTrackingId    = null,
                      TimeSpan?             RequestTimeout     = null)

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


            HTTPResponse<CheckCDRsResponse> result = null;

            #endregion

            #region Send OnCheckCDRsRequest event

            try
            {

                OnCheckCDRsRequest?.Invoke(DateTime.Now,
                                           Timestamp.Value,
                                           this,
                                           ClientId,
                                           EventTrackingId,
                                           CDRStatus,
                                           RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnCheckCDRsRequest));
            }

            #endregion


            var Request = new CheckCDRsRequest(CDRStatus);


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    DefaultURIPrefix,
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(Request.ToXML()),
                                                 "CheckCDRsRequest",
                                                 RequestLogDelegate:   OnCheckCDRsSOAPRequest,
                                                 ResponseLogDelegate:  OnCheckCDRsSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, CheckCDRsResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<CheckCDRsResponse>(httpresponse,
                                                                                                new CheckCDRsResponse(
                                                                                                    Request,
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

                                                     return new HTTPResponse<CheckCDRsResponse>(httpresponse,
                                                                                                new CheckCDRsResponse(
                                                                                                    Request,
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

                                                     return HTTPResponse<CheckCDRsResponse>.ExceptionThrown(new CheckCDRsResponse(
                                                                                                                Request,
                                                                                                                Result.Format(exception.Message +
                                                                                                                              " => " +
                                                                                                                              exception.StackTrace)),
                                                                                                            exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<CheckCDRsResponse>.OK(new CheckCDRsResponse(Request, Result.OK("Nothing to upload!")));


            #region Send OnCheckCDRsResponse event

            try
            {

                OnCheckCDRsResponse?.Invoke(DateTime.Now,
                                            Timestamp.Value,
                                            this,
                                            ClientId,
                                            EventTrackingId,
                                            CDRStatus,
                                            RequestTimeout,
                                            result.Content,
                                            DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnCheckCDRsResponse));
            }

            #endregion


            return result;

        }

        #endregion


        #region UpdateTariffs(TariffInfos, ...)

        /// <summary>
        /// Upload the given enumeration of tariff infos.
        /// </summary>
        /// <param name="TariffInfos">An enumeration of tariff infos.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<UpdateTariffsResponse>>

            UpdateTariffs(IEnumerable<TariffInfo>  TariffInfos,

                          DateTime?                Timestamp          = null,
                          CancellationToken?       CancellationToken  = null,
                          EventTracking_Id         EventTrackingId    = null,
                          TimeSpan?                RequestTimeout     = null)

        {

            #region Initial checks

            if (TariffInfos == null || !TariffInfos.Any())
                throw new ArgumentNullException(nameof(TariffInfos),  "The given enumeration of tariff infos must not be null or empty!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            HTTPResponse<UpdateTariffsResponse> result = null;

            #endregion

            #region Send OnUpdateTariffsRequest event

            try
            {

                OnUpdateTariffsRequest?.Invoke(DateTime.Now,
                                               Timestamp.Value,
                                               this,
                                               ClientId,
                                               EventTrackingId,
                                               TariffInfos,
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnUpdateTariffsRequest));
            }

            #endregion


            var Request = new UpdateTariffsRequest(TariffInfos);


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    DefaultURIPrefix,
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(Request.ToXML()),
                                                 "UpdateTariffsRequest",
                                                 RequestLogDelegate:   OnUpdateTariffsSOAPRequest,
                                                 ResponseLogDelegate:  OnUpdateTariffsSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, UpdateTariffsResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<UpdateTariffsResponse>(httpresponse,
                                                                                                    new UpdateTariffsResponse(
                                                                                                        Request,
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

                                                     return new HTTPResponse<UpdateTariffsResponse>(httpresponse,
                                                                                                    new UpdateTariffsResponse(
                                                                                                        Request,
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

                                                     return HTTPResponse<UpdateTariffsResponse>.ExceptionThrown(new UpdateTariffsResponse(
                                                                                                                    Request,
                                                                                                                    Result.Format(exception.Message +
                                                                                                                                  " => " +
                                                                                                                                  exception.StackTrace)),
                                                                                                                exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<UpdateTariffsResponse>.OK(new UpdateTariffsResponse(Request, Result.OK("Nothing to upload!")));


            #region Send OnUpdateTariffsResponse event

            try
            {

                OnUpdateTariffsResponse?.Invoke(DateTime.Now,
                                                Timestamp.Value,
                                                this,
                                                ClientId,
                                                EventTrackingId,
                                                TariffInfos,
                                                RequestTimeout,
                                                result.Content,
                                                DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnUpdateTariffsResponse));
            }

            #endregion


            return result;

        }

        #endregion


        // OCHPdirect

        #region AddServiceEndpoints(OperatorEndpoints, ...)

        /// <summary>
        /// Upload the given enumeration of OCHPdirect provider endpoints.
        /// </summary>
        /// <param name="OperatorEndpoints">An enumeration of provider endpoints.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<AddServiceEndpointsResponse>>

            AddServiceEndpoints(IEnumerable<OperatorEndpoint>  OperatorEndpoints,

                                DateTime?                      Timestamp          = null,
                                CancellationToken?             CancellationToken  = null,
                                EventTracking_Id               EventTrackingId    = null,
                                TimeSpan?                      RequestTimeout     = null)

        {

            #region Initial checks

            if (OperatorEndpoints == null)
                throw new ArgumentNullException(nameof(OperatorEndpoints),  "The given enumeration of operator endpoints must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            HTTPResponse<AddServiceEndpointsResponse> result = null;

            #endregion

            #region Send OnAddServiceEndpointsRequest event

            try
            {

                OnAddServiceEndpointsRequest?.Invoke(DateTime.Now,
                                                     Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     EventTrackingId,
                                                     OperatorEndpoints,
                                                     RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAddServiceEndpointsRequest));
            }

            #endregion


            var Request = new AddServiceEndpointsRequest(OperatorEndpoints);


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    DefaultURIPrefix,
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(Request.ToXML()),
                                                 "AddServiceEndpointsRequest",
                                                 RequestLogDelegate:   OnAddServiceEndpointsSOAPRequest,
                                                 ResponseLogDelegate:  OnAddServiceEndpointsSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, AddServiceEndpointsResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<AddServiceEndpointsResponse>(httpresponse,
                                                                                                          new AddServiceEndpointsResponse(
                                                                                                              Request,
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

                                                     return new HTTPResponse<AddServiceEndpointsResponse>(httpresponse,
                                                                                                          new AddServiceEndpointsResponse(
                                                                                                              Request,
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

                                                     return HTTPResponse<AddServiceEndpointsResponse>.ExceptionThrown(new AddServiceEndpointsResponse(
                                                                                                                          Request,
                                                                                                                          Result.Format(exception.Message +
                                                                                                                                        " => " +
                                                                                                                                        exception.StackTrace)),
                                                                                                                      exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<AddServiceEndpointsResponse>.OK(new AddServiceEndpointsResponse(Request, Result.OK("Nothing to upload!")));


            #region Send OnAddServiceEndpointsResponse event

            try
            {

                OnAddServiceEndpointsResponse?.Invoke(DateTime.Now,
                                                      Timestamp.Value,
                                                      this,
                                                      ClientId,
                                                      EventTrackingId,
                                                      OperatorEndpoints,
                                                      RequestTimeout,
                                                      result.Content,
                                                      DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnAddServiceEndpointsResponse));
            }

            #endregion


            return result;

        }

        #endregion

        #region GetServiceEndpoints(...)

        /// <summary>
        /// Download OCHPdirect provider endpoints.
        /// </summary>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<GetServiceEndpointsResponse>>

            GetServiceEndpoints(DateTime?           Timestamp          = null,
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


            HTTPResponse<GetServiceEndpointsResponse> result = null;

            #endregion

            #region Send OnGetServiceEndpointsRequest event

            try
            {

                OnGetServiceEndpointsRequest?.Invoke(DateTime.Now,
                                                     Timestamp.Value,
                                                     this,
                                                     ClientId,
                                                     EventTrackingId,
                                                     RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnGetServiceEndpointsRequest));
            }

            #endregion


            var Request = new GetServiceEndpointsRequest();


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    DefaultURIPrefix,
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(Request.ToXML()),
                                                 "GetServiceEndpointsRequest",
                                                 RequestLogDelegate:   OnGetServiceEndpointsSOAPRequest,
                                                 ResponseLogDelegate:  OnGetServiceEndpointsSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, GetServiceEndpointsResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<GetServiceEndpointsResponse>(httpresponse,
                                                                                                          new GetServiceEndpointsResponse(
                                                                                                              Request,
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

                                                     return new HTTPResponse<GetServiceEndpointsResponse>(httpresponse,
                                                                                                          new GetServiceEndpointsResponse(
                                                                                                              Request,
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

                                                     return HTTPResponse<GetServiceEndpointsResponse>.ExceptionThrown(new GetServiceEndpointsResponse(
                                                                                                                          Request,
                                                                                                                          Result.Format(exception.Message +
                                                                                                                                        " => " +
                                                                                                                                        exception.StackTrace)),
                                                                                                                      exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<GetServiceEndpointsResponse>.OK(new GetServiceEndpointsResponse(Request, Result.OK("Nothing to upload!")));


            #region Send OnGetServiceEndpointsResponse event

            try
            {

                OnGetServiceEndpointsResponse?.Invoke(DateTime.Now,
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
                e.Log(nameof(CPOClient) + "." + nameof(OnGetServiceEndpointsResponse));
            }

            #endregion


            return result;

        }

        #endregion


        #region InformProvider(...)

        /// <summary>
        /// Send an inform provider OCHPdirect message.
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
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<InformProviderResponse>>

            InformProvider(DirectMessages          DirectMessage,
                           EVSE_Id                 EVSEId,
                           Contract_Id             ContractId,
                           Direct_Id               DirectId,

                           DateTime?               SessionTimeoutAt   = null,
                           Single?                 StateOfCharge      = null,
                           Single?                 MaxPower           = null,
                           Single?                 MaxCurrent         = null,
                           Boolean?                OnePhase           = null,
                           Single?                 MaxEnergy          = null,
                           Single?                 MinEnergy          = null,
                           DateTime?               Departure          = null,
                           Single?                 CurrentPower       = null,
                           Single?                 ChargedEnergy      = null,
                           Timestamped<Single>?    MeterReading       = null,
                           IEnumerable<CDRPeriod>  ChargingPeriods    = null,
                           Single?                 CurrentCost        = null,
                           Currency                Currency           = null,

                           DateTime?               Timestamp          = null,
                           CancellationToken?      CancellationToken  = null,
                           EventTracking_Id        EventTrackingId    = null,
                           TimeSpan?               RequestTimeout     = null)

        {

            #region Initial checks

            if (ContractId == null)
                throw new ArgumentNullException(nameof(ContractId),  "The given identification of an e-mobility contract must not be null!");

            if (DirectId   == null)
                throw new ArgumentNullException(nameof(DirectId),    "The given identification of an direct charging process must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            HTTPResponse<InformProviderResponse> result = null;

            #endregion

            #region Send OnInformProviderSOAPRequest event

            try
            {

                OnInformProviderRequest?.Invoke(DateTime.Now,
                                                Timestamp.Value,
                                                this,
                                                ClientId,
                                                EventTrackingId,

                                                DirectMessage,
                                                EVSEId,
                                                ContractId,
                                                DirectId,

                                                SessionTimeoutAt,
                                                StateOfCharge,
                                                MaxPower,
                                                MaxCurrent,
                                                OnePhase,
                                                MaxEnergy,
                                                MinEnergy,
                                                Departure,
                                                CurrentPower,
                                                ChargedEnergy,
                                                MeterReading,
                                                ChargingPeriods,
                                                CurrentCost,
                                                Currency,

                                                RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnInformProviderSOAPRequest));
            }

            #endregion


            var Request = new InformProviderRequest(DirectMessage,
                                                    EVSEId,
                                                    ContractId,
                                                    DirectId,

                                                    SessionTimeoutAt,
                                                    StateOfCharge,
                                                    MaxPower,
                                                    MaxCurrent,
                                                    OnePhase,
                                                    MaxEnergy,
                                                    MinEnergy,
                                                    Departure,
                                                    CurrentPower,
                                                    ChargedEnergy,
                                                    MeterReading,
                                                    ChargingPeriods,
                                                    CurrentCost,
                                                    Currency);


            using (var _OCHPClient = new SOAPClient(Hostname,
                                                    RemotePort,
                                                    HTTPVirtualHost,
                                                    DefaultURIPrefix,
                                                    RemoteCertificateValidator,
                                                    ClientCert,
                                                    UserAgent,
                                                    DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(Request.ToXML()),
                                                 "InformProviderMessage",
                                                 RequestLogDelegate:   OnInformProviderSOAPRequest,
                                                 ResponseLogDelegate:  OnInformProviderSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, InformProviderResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<InformProviderResponse>(httpresponse,
                                                                                                     InformProviderResponse.Format(
                                                                                                        Request,
                                                                                                        "Invalid SOAP => " +
                                                                                                        httpresponse.HTTPBody.ToUTF8String()
                                                                                                     ),
                                                                                                     IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, this, httpresponse);

                                                     return new HTTPResponse<InformProviderResponse>(httpresponse,
                                                                                                     InformProviderResponse.Server(
                                                                                                         Request,
                                                                                                         httpresponse.HTTPStatusCode.ToString() +
                                                                                                         " => " +
                                                                                                         httpresponse.HTTPBody.      ToUTF8String()
                                                                                                     ),
                                                                                                     IsFault: true);

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<InformProviderResponse>.ExceptionThrown(InformProviderResponse.Format(
                                                                                                                     Request,
                                                                                                                     exception.Message +
                                                                                                                     " => " +
                                                                                                                     exception.StackTrace
                                                                                                                 ),
                                                                                                                 exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<InformProviderResponse>.OK(InformProviderResponse.OK(Request, "Nothing to upload!"));


            #region Send OnInformProviderResponse event

            try
            {

                OnInformProviderResponse?.Invoke(DateTime.Now,
                                                 Timestamp.Value,
                                                 this,
                                                 ClientId,
                                                 EventTrackingId,

                                                 DirectMessage,
                                                 EVSEId,
                                                 ContractId,
                                                 DirectId,

                                                 SessionTimeoutAt,
                                                 StateOfCharge,
                                                 MaxPower,
                                                 MaxCurrent,
                                                 OnePhase,
                                                 MaxEnergy,
                                                 MinEnergy,
                                                 Departure,
                                                 CurrentPower,
                                                 ChargedEnergy,
                                                 MeterReading,
                                                 ChargingPeriods,
                                                 CurrentCost,
                                                 Currency,

                                                 RequestTimeout,
                                                 result.Content,
                                                 DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnInformProviderResponse));
            }

            #endregion


            return result;

        }

        #endregion


    }

}
