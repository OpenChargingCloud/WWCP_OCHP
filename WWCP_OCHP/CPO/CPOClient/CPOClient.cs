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

                               DateTime?                     Timestamp             = null,
                               CancellationToken?            CancellationToken     = null,
                               EventTracking_Id              EventTrackingId       = null,
                               TimeSpan?                     RequestTimeout        = null)

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

            #region Send OnPushEVSEDataRequest event

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

                                                         return HTTPResponse<SetChargePointListResponse>.ExceptionThrown(
                                                                                                             new SetChargePointListResponse(
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



    }

}
