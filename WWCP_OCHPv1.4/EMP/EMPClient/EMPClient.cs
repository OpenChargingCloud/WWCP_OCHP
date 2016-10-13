﻿/*
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
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// An OCHP EMP client.
    /// </summary>
    public partial class EMPClient : ASOAPClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const           String  DefaultHTTPUserAgent  = "GraphDefined OCHP " + Version.Number + " EMP Client";

        /// <summary>
        /// The default remote TCP port to connect to.
        /// </summary>
        public new static readonly IPPort  DefaultRemotePort     = IPPort.Parse(443);

        #endregion

        #region Properties

        /// <summary>
        /// The attached OCHP EMP client (HTTP/SOAP client) logger.
        /// </summary>
        public EMPClientLogger Logger { get; }

        #endregion

        #region Events

        #region OnGetChargePointListRequest/-Response

        /// <summary>
        /// An event fired whenever a request to download the charge points list will be send.
        /// </summary>
        public event OnGetChargePointListRequestDelegate   OnGetChargePointListRequest;

        /// <summary>
        /// An event fired whenever a SOAP request to download the charge points list will be send.
        /// </summary>
        public event ClientRequestLogHandler               OnGetChargePointListSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a charge points list download SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler              OnGetChargePointListSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a charge points list download request had been received.
        /// </summary>
        public event OnGetChargePointListResponseDelegate  OnGetChargePointListResponse;

        #endregion

        #region OnGetChargePointListUpdatesRequest/-Response

        /// <summary>
        /// An event fired whenever a request to download a charge points list update will be send.
        /// </summary>
        public event OnGetChargePointListUpdatesRequestDelegate   OnGetChargePointListUpdatesRequest;

        /// <summary>
        /// An event fired whenever a SOAP request to download a charge points list update will be send.
        /// </summary>
        public event ClientRequestLogHandler                      OnGetChargePointListUpdatesSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a charge points list update download SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                     OnGetChargePointListUpdatesSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a charge points list update download request had been received.
        /// </summary>
        public event OnGetChargePointListUpdatesResponseDelegate  OnGetChargePointListUpdatesResponse;

        #endregion

        #region OnGetStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request for evse or parking status will be send.
        /// </summary>
        public event OnGetStatusRequestDelegate   OnGetStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for evse or parking status will be send.
        /// </summary>
        public event ClientRequestLogHandler      OnGetStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a evse or parking status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler     OnGetStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for a evse or parking status request had been received.
        /// </summary>
        public event OnGetStatusResponseDelegate  OnGetStatusResponse;

        #endregion


        #region OnSetRoamingAuthorisationListRequest/-Response

        /// <summary>
        /// An event fired whenever a roaming authorisation list will be send.
        /// </summary>
        public event OnSetRoamingAuthorisationListRequestDelegate   OnSetRoamingAuthorisationListRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for uploading a roaming authorisation list will be send.
        /// </summary>
        public event ClientRequestLogHandler                        OnSetRoamingAuthorisationListSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for uploading a roaming authorisation list had been received.
        /// </summary>
        public event ClientResponseLogHandler                       OnSetRoamingAuthorisationListSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for uploading a roaming authorisation list had been received.
        /// </summary>
        public event OnSetRoamingAuthorisationListResponseDelegate  OnSetRoamingAuthorisationListResponse;

        #endregion

        #region OnUpdateRoamingAuthorisationListRequest/-Response

        /// <summary>
        /// An event fired whenever a roaming authorisation list update will be send.
        /// </summary>
        public event OnUpdateRoamingAuthorisationListRequestDelegate   OnUpdateRoamingAuthorisationListRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for uploading a roaming authorisation list update will be send.
        /// </summary>
        public event ClientRequestLogHandler                           OnUpdateRoamingAuthorisationListSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for uploading a roaming authorisation list update had been received.
        /// </summary>
        public event ClientResponseLogHandler                          OnUpdateRoamingAuthorisationListSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for uploading a roaming authorisation list update had been received.
        /// </summary>
        public event OnUpdateRoamingAuthorisationListResponseDelegate  OnUpdateRoamingAuthorisationListResponse;

        #endregion


        #region OnGetCDRsRequest/-Response

        /// <summary>
        /// An event fired whenever a request for charge detail records will be send.
        /// </summary>
        public event OnGetCDRsRequestDelegate   OnGetCDRsRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for charge detail records will be send.
        /// </summary>
        public event ClientRequestLogHandler    OnGetCDRsSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a charge detail records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler   OnGetCDRsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for a charge detail records request had been received.
        /// </summary>
        public event OnGetCDRsResponseDelegate  OnGetCDRsResponse;

        #endregion


        #region OnGetTariffUpdatesRequest/-Response

        /// <summary>
        /// An event fired whenever a request for tariff infos will be send.
        /// </summary>
        public event OnGetTariffUpdatesRequestDelegate   OnGetTariffUpdatesRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for tariff infos will be send.
        /// </summary>
        public event ClientRequestLogHandler             OnGetTariffUpdatesSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a tariff infos SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler            OnGetTariffUpdatesSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for a tariff infos request had been received.
        /// </summary>
        public event OnGetTariffUpdatesResponseDelegate  OnGetTariffUpdatesResponse;

        #endregion

        #endregion

        #region Constructor(s)

        #region EMPClient(ClientId, Hostname, ..., LoggingContext = EMPClientLogger.DefaultContext, ...)

        /// <summary>
        /// Create a new OCHP EMP client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The OCHP hostname to connect to.</param>
        /// <param name="RemotePort">An optional OCHP TCP port to connect to.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent to use.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        /// <param name="LoggingContext">An optional context for logging client methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public EMPClient(String                               ClientId,
                         String                               Hostname,
                         IPPort                               RemotePort                  = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                         X509Certificate                      ClientCert                  = null,
                         String                               HTTPVirtualHost             = null,
                         String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                         TimeSpan?                            RequestTimeout              = null,
                         DNSClient                            DNSClient                   = null,
                         String                               LoggingContext              = EMPClientLogger.DefaultContext,
                         Func<String, String, String>         LogFileCreator              = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCert,
                   HTTPVirtualHost,
                   HTTPUserAgent,
                   RequestTimeout,
                   DNSClient)

        {

            #region Initial checks

            if (ClientId.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Logger),    "The given client identification must not be null or empty!");

            if (Hostname.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Hostname),  "The given hostname must not be null or empty!");

            #endregion

            this.Logger = new EMPClientLogger(this,
                                              LoggingContext,
                                              LogFileCreator);

        }

        #endregion

        #region EMPClient(ClientId, Logger, Hostname, ...)

        /// <summary>
        /// Create a new OCHP EMP client.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="Hostname">The OCHP hostname to connect to.</param>
        /// <param name="RemotePort">An optional OCHP TCP port to connect to.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="HTTPVirtualHost">An optional HTTP virtual host name to use.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent to use.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="DNSClient">An optional DNS client.</param>
        public EMPClient(String                               ClientId,
                         EMPClientLogger                      Logger,
                         String                               Hostname,
                         IPPort                               RemotePort                  = null,
                         RemoteCertificateValidationCallback  RemoteCertificateValidator  = null,
                         X509Certificate                      ClientCert                  = null,
                         String                               HTTPVirtualHost             = null,
                         String                               HTTPUserAgent               = DefaultHTTPUserAgent,
                         TimeSpan?                            RequestTimeout              = null,
                         DNSClient                            DNSClient                   = null)

            : base(ClientId,
                   Hostname,
                   RemotePort ?? DefaultRemotePort,
                   RemoteCertificateValidator,
                   ClientCert,
                   HTTPVirtualHost,
                   HTTPUserAgent,
                   RequestTimeout,
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


        #region GetChargePointList(...)

        /// <summary>
        /// Download the current charge point list.
        /// </summary>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<GetChargePointListResponse>>

            GetChargePointList(DateTime?           Timestamp          = null,
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


            HTTPResponse<GetChargePointListResponse> result = null;

            #endregion

            #region Send OnGetChargePointListRequest event

            try
            {

                OnGetChargePointListRequest?.Invoke(DateTime.Now,
                                                    Timestamp.Value,
                                                    this,
                                                    ClientId,
                                                    EventTrackingId,
                                                    RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnGetChargePointListRequest));
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

                result = await _OCHPClient.Query(EMPClientXMLMethods.GetChargePointListXML(),
                                                 "GetChargePointList",
                                                 RequestLogDelegate:   OnGetChargePointListSOAPRequest,
                                                 ResponseLogDelegate:  OnGetChargePointListSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(GetChargePointListResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<GetChargePointListResponse>(httpresponse,
                                                                                                         new GetChargePointListResponse(
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

                                                     return new HTTPResponse<GetChargePointListResponse>(httpresponse,
                                                                                                         new GetChargePointListResponse(
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

                                                     return HTTPResponse<GetChargePointListResponse>.ExceptionThrown(new GetChargePointListResponse(
                                                                                                                         Result.Format(exception.Message +
                                                                                                                                       " => " +
                                                                                                                                       exception.StackTrace)),
                                                                                                                     exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<GetChargePointListResponse>.OK(new GetChargePointListResponse(Result.OK("Nothing to upload!")));


            #region Send OnGetChargePointListResponse event

            try
            {

                OnGetChargePointListResponse?.Invoke(DateTime.Now,
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
                e.Log(nameof(EMPClient) + "." + nameof(OnGetChargePointListResponse));
            }

            #endregion

            return result;


        }

        #endregion

        #region GetChargePointListUpdates(LastUpdate, ...)

        /// <summary>
        /// Download an update of the current charge point list since the given date.
        /// </summary>
        /// <param name="LastUpdate">The timestamp of the last call to this method.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<GetChargePointListUpdatesResponse>>

            GetChargePointListUpdates(DateTime            LastUpdate,

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


            HTTPResponse<GetChargePointListUpdatesResponse> result = null;

            #endregion

            #region Send OnGetChargePointListUpdatesRequest event

            try
            {

                OnGetChargePointListUpdatesRequest?.Invoke(DateTime.Now,
                                                           Timestamp.Value,
                                                           this,
                                                           ClientId,
                                                           EventTrackingId,
                                                           LastUpdate,
                                                           RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnGetChargePointListUpdatesRequest));
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

                result = await _OCHPClient.Query(EMPClientXMLMethods.GetChargePointListUpdatesXML(LastUpdate),
                                                 "GetChargePointListUpdates",
                                                 RequestLogDelegate:   OnGetChargePointListUpdatesSOAPRequest,
                                                 ResponseLogDelegate:  OnGetChargePointListUpdatesSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(GetChargePointListUpdatesResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<GetChargePointListUpdatesResponse>(httpresponse,
                                                                                                                new GetChargePointListUpdatesResponse(
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

                                                     return new HTTPResponse<GetChargePointListUpdatesResponse>(httpresponse,
                                                                                                                new GetChargePointListUpdatesResponse(
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

                                                     return HTTPResponse<GetChargePointListUpdatesResponse>.ExceptionThrown(new GetChargePointListUpdatesResponse(
                                                                                                                                Result.Format(exception.Message +
                                                                                                                                              " => " +
                                                                                                                                              exception.StackTrace)),
                                                                                                                            exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<GetChargePointListUpdatesResponse>.OK(new GetChargePointListUpdatesResponse(Result.OK("Nothing to upload!")));


            #region Send OnGetChargePointListUpdatesResponse event

            try
            {

                OnGetChargePointListUpdatesResponse?.Invoke(DateTime.Now,
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
                e.Log(nameof(EMPClient) + "." + nameof(OnGetChargePointListUpdatesResponse));
            }

            #endregion


            return result;

        }

        #endregion

        #region GetStatusRequest(...)

        /// <summary>
        /// Download charge detail records having the given optional status.
        /// </summary>
        /// <param name="LastRequest">Only return status data newer than the given timestamp.</param>
        /// <param name="StatusType">A status type filter.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<GetStatusResponse>>

            GetStatusRequest(DateTime?             LastRequest        = null,
                             StatusTypes?          StatusType         = null,

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


            HTTPResponse<GetStatusResponse> result = null;

            #endregion

            #region Send OnGetStatusRequest event

            try
            {

                OnGetStatusRequest?.Invoke(DateTime.Now,
                                           Timestamp.Value,
                                           this,
                                           ClientId,
                                           EventTrackingId,
                                           LastRequest,
                                           StatusType,
                                           RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnGetStatusRequest));
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

                result = await _OCHPClient.Query(EMPClientXMLMethods.GetStatusXML(LastRequest,
                                                                                  StatusType),
                                                 "GetStatusRequest",
                                                 RequestLogDelegate:   OnGetStatusSOAPRequest,
                                                 ResponseLogDelegate:  OnGetStatusSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(GetStatusResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<GetStatusResponse>(httpresponse,
                                                                                                new GetStatusResponse(
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

                                                     return new HTTPResponse<GetStatusResponse>(httpresponse,
                                                                                                new GetStatusResponse(
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

                                                     return HTTPResponse<GetStatusResponse>.ExceptionThrown(new GetStatusResponse(
                                                                                                                Result.Format(exception.Message +
                                                                                                                              " => " +
                                                                                                                              exception.StackTrace)),
                                                                                                            exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<GetStatusResponse>.OK(new GetStatusResponse(Result.OK("Nothing to upload!")));


            #region Send OnGetStatusResponse event

            try
            {

                OnGetStatusResponse?.Invoke(DateTime.Now,
                                            Timestamp.Value,
                                            this,
                                            ClientId,
                                            EventTrackingId,
                                            LastRequest,
                                            StatusType,
                                            RequestTimeout,
                                            result.Content,
                                            DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnGetStatusResponse));
            }

            #endregion


            return result;

        }

        #endregion


        #region SetRoamingAuthorisationList(RoamingAuthorisationInfos, ...)

        /// <summary>
        /// Upload the entire roaming authorisation list.
        /// </summary>
        /// <param name="RoamingAuthorisationInfos">An enumeration of roaming authorisation infos.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<SetRoamingAuthorisationListResponse>>

            SetRoamingAuthorisationList(IEnumerable<RoamingAuthorisationInfo>  RoamingAuthorisationInfos,

                                        DateTime?                              Timestamp          = null,
                                        CancellationToken?                     CancellationToken  = null,
                                        EventTracking_Id                       EventTrackingId    = null,
                                        TimeSpan?                              RequestTimeout     = null)

        {

            #region Initial checks

            if (RoamingAuthorisationInfos == null)
                throw new ArgumentNullException(nameof(RoamingAuthorisationInfos),  "The given enumeration of roaming authorisation infos must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            HTTPResponse<SetRoamingAuthorisationListResponse> result = null;

            #endregion

            #region Send OnSetRoamingAuthorisationListRequest event

            try
            {

                OnSetRoamingAuthorisationListRequest?.Invoke(DateTime.Now,
                                                             Timestamp.Value,
                                                             this,
                                                             ClientId,
                                                             EventTrackingId,
                                                             RoamingAuthorisationInfos,
                                                             RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSetRoamingAuthorisationListRequest));
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

                result = await _OCHPClient.Query(EMPClientXMLMethods.SetRoamingAuthorisationListXML(RoamingAuthorisationInfos),
                                                 "SetRoamingAuthorisationListRequest",
                                                 RequestLogDelegate:   OnSetRoamingAuthorisationListSOAPRequest,
                                                 ResponseLogDelegate:  OnSetRoamingAuthorisationListSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(SetRoamingAuthorisationListResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<SetRoamingAuthorisationListResponse>(httpresponse,
                                                                                                                  new SetRoamingAuthorisationListResponse(
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

                                                     return new HTTPResponse<SetRoamingAuthorisationListResponse>(httpresponse,
                                                                                                                  new SetRoamingAuthorisationListResponse(
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

                                                     return HTTPResponse<SetRoamingAuthorisationListResponse>.ExceptionThrown(new SetRoamingAuthorisationListResponse(
                                                                                                                                  Result.Format(exception.Message +
                                                                                                                                                " => " +
                                                                                                                                                exception.StackTrace)),
                                                                                                                              exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<SetRoamingAuthorisationListResponse>.OK(new SetRoamingAuthorisationListResponse(Result.OK("Nothing to upload!")));


            #region Send OnGetRoamingAuthorisationListResponse event

            try
            {

                OnSetRoamingAuthorisationListResponse?.Invoke(DateTime.Now,
                                                              Timestamp.Value,
                                                              this,
                                                              ClientId,
                                                              EventTrackingId,
                                                              RoamingAuthorisationInfos,
                                                              RequestTimeout,
                                                              result.Content,
                                                              DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnSetRoamingAuthorisationListResponse));
            }

            #endregion


            return result;

        }

        #endregion

        #region UpdateRoamingAuthorisationList(RoamingAuthorisationInfos, ...)

        /// <summary>
        /// Send a roaming authorisation list update.
        /// </summary>
        /// <param name="RoamingAuthorisationInfos">An enumeration of roaming authorisation infos.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<UpdateRoamingAuthorisationListResponse>>

            UpdateRoamingAuthorisationList(IEnumerable<RoamingAuthorisationInfo>  RoamingAuthorisationInfos,

                                           DateTime?                              Timestamp          = null,
                                           CancellationToken?                     CancellationToken  = null,
                                           EventTracking_Id                       EventTrackingId    = null,
                                           TimeSpan?                              RequestTimeout     = null)

        {

            #region Initial checks

            if (RoamingAuthorisationInfos == null)
                throw new ArgumentNullException(nameof(RoamingAuthorisationInfos),  "The given enumeration of roaming authorisation infos must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = DateTime.Now;

            if (!CancellationToken.HasValue)
                CancellationToken = new CancellationTokenSource().Token;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            HTTPResponse<UpdateRoamingAuthorisationListResponse> result = null;

            #endregion

            #region Send OnSetRoamingAuthorisationListRequest event

            try
            {

                OnUpdateRoamingAuthorisationListRequest?.Invoke(DateTime.Now,
                                                                Timestamp.Value,
                                                                this,
                                                                ClientId,
                                                                EventTrackingId,
                                                                RoamingAuthorisationInfos,
                                                                RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnUpdateRoamingAuthorisationListRequest));
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

                result = await _OCHPClient.Query(EMPClientXMLMethods.UpdateRoamingAuthorisationListXML(RoamingAuthorisationInfos),
                                                 "UpdateRoamingAuthorisationListRequest",
                                                 RequestLogDelegate:   OnUpdateRoamingAuthorisationListSOAPRequest,
                                                 ResponseLogDelegate:  OnUpdateRoamingAuthorisationListSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(UpdateRoamingAuthorisationListResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<UpdateRoamingAuthorisationListResponse>(httpresponse,
                                                                                                                     new UpdateRoamingAuthorisationListResponse(
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

                                                     return new HTTPResponse<UpdateRoamingAuthorisationListResponse>(httpresponse,
                                                                                                                     new UpdateRoamingAuthorisationListResponse(
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

                                                     return HTTPResponse<UpdateRoamingAuthorisationListResponse>.ExceptionThrown(new UpdateRoamingAuthorisationListResponse(
                                                                                                                                     Result.Format(exception.Message +
                                                                                                                                                   " => " +
                                                                                                                                                   exception.StackTrace)),
                                                                                                                                 exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<UpdateRoamingAuthorisationListResponse>.OK(new UpdateRoamingAuthorisationListResponse(Result.OK("Nothing to upload!")));


            #region Send OnUpdateRoamingAuthorisationListResponse event

            try
            {

                OnUpdateRoamingAuthorisationListResponse?.Invoke(DateTime.Now,
                                                                 Timestamp.Value,
                                                                 this,
                                                                 ClientId,
                                                                 EventTrackingId,
                                                                 RoamingAuthorisationInfos,
                                                                 RequestTimeout,
                                                                 result.Content,
                                                                 DateTime.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnUpdateRoamingAuthorisationListResponse));
            }

            #endregion


            return result;

        }

        #endregion


        #region AddCDRsRequest(CDRStatus = null, ...)

        /// <summary>
        /// Download charge detail records having the given optional status.
        /// </summary>
        /// <param name="CDRStatus">The status of the requested charge detail records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<GetCDRsResponse>>

            AddCDRsRequest(CDRStatus?            CDRStatus          = null,

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


            HTTPResponse<GetCDRsResponse> result = null;

            #endregion

            #region Send OnGetCDRsRequest event

            try
            {

                OnGetCDRsRequest?.Invoke(DateTime.Now,
                                         Timestamp.Value,
                                         this,
                                         ClientId,
                                         EventTrackingId,
                                         CDRStatus,
                                         RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(CPOClient) + "." + nameof(OnGetCDRsRequest));
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

                result = await _OCHPClient.Query(EMPClientXMLMethods.GetCDRsXML(CDRStatus),
                                                 "AddCDRsRequest",
                                                 RequestLogDelegate:   OnGetCDRsSOAPRequest,
                                                 ResponseLogDelegate:  OnGetCDRsSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(GetCDRsResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<GetCDRsResponse>(httpresponse,
                                                                                              new GetCDRsResponse(
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

                                                     return new HTTPResponse<GetCDRsResponse>(httpresponse,
                                                                                              new GetCDRsResponse(
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

                                                     return HTTPResponse<GetCDRsResponse>.ExceptionThrown(new GetCDRsResponse(
                                                                                                              Result.Format(exception.Message +
                                                                                                                            " => " +
                                                                                                                            exception.StackTrace)),
                                                                                                          exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<GetCDRsResponse>.OK(new GetCDRsResponse(Result.OK("Nothing to upload!")));


            #region Send OnAddCDRsResponse event

            try
            {

                OnGetCDRsResponse?.Invoke(DateTime.Now,
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
                e.Log(nameof(CPOClient) + "." + nameof(OnGetCDRsResponse));
            }

            #endregion


            return result;

        }

        #endregion


        #region GetTariffUpdates(LastUpdate, ...)

        /// <summary>
        /// Download an update of the current tariff list since the given date.
        /// </summary>
        /// <param name="LastUpdate">The timestamp of the last call to this method.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<GetTariffUpdatesResponse>>

            GetTariffUpdates(DateTime            LastUpdate,

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


            HTTPResponse<GetTariffUpdatesResponse> result = null;

            #endregion

            #region Send OnGetTariffUpdatesRequest event

            try
            {

                OnGetTariffUpdatesRequest?.Invoke(DateTime.Now,
                                                  Timestamp.Value,
                                                  this,
                                                  ClientId,
                                                  EventTrackingId,
                                                  LastUpdate,
                                                  RequestTimeout);

            }
            catch (Exception e)
            {
                e.Log(nameof(EMPClient) + "." + nameof(OnGetTariffUpdatesRequest));
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

                result = await _OCHPClient.Query(EMPClientXMLMethods.GetChargePointListUpdatesXML(LastUpdate),
                                                 "GetTariffUpdates",
                                                 RequestLogDelegate:   OnGetTariffUpdatesSOAPRequest,
                                                 ResponseLogDelegate:  OnGetTariffUpdatesSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 QueryTimeout:         RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(GetTariffUpdatesResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return new HTTPResponse<GetTariffUpdatesResponse>(httpresponse,
                                                                                                       new GetTariffUpdatesResponse(
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

                                                     return new HTTPResponse<GetTariffUpdatesResponse>(httpresponse,
                                                                                                       new GetTariffUpdatesResponse(
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

                                                     return HTTPResponse<GetTariffUpdatesResponse>.ExceptionThrown(new GetTariffUpdatesResponse(
                                                                                                                       Result.Format(exception.Message +
                                                                                                                                     " => " +
                                                                                                                                     exception.StackTrace)),
                                                                                                                   exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<GetTariffUpdatesResponse>.OK(new GetTariffUpdatesResponse(Result.OK("Nothing to upload!")));


            #region Send OnGetTariffUpdatesResponse event

            try
            {

                OnGetTariffUpdatesResponse?.Invoke(DateTime.Now,
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
                e.Log(nameof(EMPClient) + "." + nameof(OnGetTariffUpdatesResponse));
            }

            #endregion


            return result;

        }

        #endregion


    }

}