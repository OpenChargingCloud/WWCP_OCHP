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

using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using System.Net.Mime;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHP CPO Client.
    /// </summary>
    public class CPORoaming : ICPOClient
    {

        #region Properties

        /// <summary>
        /// The CPO client.
        /// </summary>
        public CPOClient             CPOClient         { get; }

        #region ICPOClient

        /// <summary>
        /// The remote URL of the OICP HTTP endpoint to connect to.
        /// </summary>
        URL                                  IHTTPClient.RemoteURL
            => CPOClient.RemoteURL;

        /// <summary>
        /// The virtual HTTP hostname to connect to.
        /// </summary>
        HTTPHostname?                        IHTTPClient.VirtualHostname
            => CPOClient.VirtualHostname;

        /// <summary>
        /// An optional description of this CPO client.
        /// </summary>
        I18NString                           IHTTPClient.Description
        {

            get
            {
                return CPOClient.Description;
            }

            //set
            //{
            //    CPOClient.Description = value;
            //}

        }

        /// <summary>
        /// The remote TLS certificate validator.
        /// </summary>
        RemoteTLSServerCertificateValidationHandler<IHTTPClient>?  IHTTPClient.RemoteCertificateValidator
            => CPOClient.RemoteCertificateValidator;

        /// <summary>
        /// The TLS client certificate to use of HTTP authentication.
        /// </summary>
        X509Certificate?                     IHTTPClient.ClientCert
            => CPOClient.ClientCert;

        /// <summary>
        /// The TLS protocol to use.
        /// </summary>
        SslProtocols                         IHTTPClient.TLSProtocols
            => CPOClient.TLSProtocols;

        /// <summary>
        /// Prefer IPv4 instead of IPv6.
        /// </summary>
        Boolean                              IHTTPClient.PreferIPv4
            => CPOClient.PreferIPv4;

        /// <summary>
        /// The optional HTTP connection type.
        /// </summary>
        ConnectionType?                      IHTTPClient.Connection
            => CPOClient.Connection;

        /// <summary>
        /// The optional HTTP content type.
        /// </summary>
        HTTPContentType?                     IHTTPClient.ContentType
            => CPOClient.ContentType;

        /// <summary>
        /// The optional HTTP accept header.
        /// </summary>
        AcceptTypes?                         IHTTPClient.Accept
            => CPOClient.Accept;

        /// <summary>
        /// The optional HTTP authentication to use.
        /// </summary>
        IHTTPAuthentication?                 IHTTPClient.Authentication
            => CPOClient.Authentication;

        /// <summary>
        /// The HTTP user agent identification.
        /// </summary>
        String                               IHTTPClient.HTTPUserAgent
            => CPOClient.HTTPUserAgent;

        /// <summary>
        /// The timeout for upstream requests.
        /// </summary>
        TimeSpan                             IHTTPClient.RequestTimeout
        {

            get
            {
                return CPOClient.RequestTimeout;
            }

            set
            {
                CPOClient.RequestTimeout = value;
            }

        }

        /// <summary>
        /// The delay between transmission retries.
        /// </summary>
        TransmissionRetryDelayDelegate       IHTTPClient.TransmissionRetryDelay
            => CPOClient.TransmissionRetryDelay;

        /// <summary>
        /// The maximum number of retries when communicating with the remote OICP service.
        /// </summary>
        UInt16                               IHTTPClient.MaxNumberOfRetries
            => CPOClient.MaxNumberOfRetries;

        /// <summary>
        /// Make use of HTTP pipelining.
        /// </summary>
        Boolean                              IHTTPClient.UseHTTPPipelining
            => CPOClient.UseHTTPPipelining;

        /// <summary>
        /// The CPO client (HTTP client) logger.
        /// </summary>
        HTTPClientLogger                     IHTTPClient.HTTPLogger
            => CPOClient.HTTPLogger;

        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        IDNSClient                           IHTTPClient.DNSClient
            => CPOClient.DNSClient;

        Boolean                              IHTTPClient.Connected
            => CPOClient.Connected;

        IIPAddress?                          IHTTPClient.RemoteIPAddress
            => CPOClient.RemoteIPAddress;

        UInt64                               IHTTPClient.KeepAliveMessageCount
            => CPOClient.KeepAliveMessageCount;

        #endregion

        /// <summary>
        /// The CPO server.
        /// </summary>
        public CPOServer             CPOServer           { get; }



        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        public IDNSClient? DNSClient
            => CPOServer?.DNSClient;

        #endregion

        #region Events

        // OCHP

        #region OnSetChargePointListRequest/-Response

        /// <summary>
        /// An event fired whenever a request uploading charge points will be send.
        /// </summary>
        public event OnSetChargePointListRequestDelegate   OnSetChargePointListRequest
        {

            add
            {
                CPOClient.OnSetChargePointListRequest += value;
            }

            remove
            {
                CPOClient.OnSetChargePointListRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request uploading charge points will be send.
        /// </summary>
        public event ClientRequestLogHandler               OnSetChargePointListSOAPRequest
        {

            add
            {
                CPOClient.OnSetChargePointListSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnSetChargePointListSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a upload charge points SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler              OnSetChargePointListSOAPResponse
        {

            add
            {
                CPOClient.OnSetChargePointListSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnSetChargePointListSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a upload charge points request had been received.
        /// </summary>
        public event OnSetChargePointListResponseDelegate  OnSetChargePointListResponse
        {

            add
            {
                CPOClient.OnSetChargePointListResponse += value;
            }

            remove
            {
                CPOClient.OnSetChargePointListResponse -= value;
            }

        }

        #endregion

        #region OnUpdateChargePointListRequest/-Response

        /// <summary>
        /// An event fired whenever a request uploading charge point updates will be send.
        /// </summary>
        public event OnUpdateChargePointListRequestDelegate   OnUpdateChargePointListRequest
        {

            add
            {
                CPOClient.OnUpdateChargePointListRequest += value;
            }

            remove
            {
                CPOClient.OnUpdateChargePointListRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request uploading charge point updates will be send.
        /// </summary>
        public event ClientRequestLogHandler                  OnUpdateChargePointListSOAPRequest
        {

            add
            {
                CPOClient.OnUpdateChargePointListSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnUpdateChargePointListSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a upload charge point updates SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                 OnUpdateChargePointListSOAPResponse
        {

            add
            {
                CPOClient.OnUpdateChargePointListSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnUpdateChargePointListSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a upload charge point updates request had been received.
        /// </summary>
        public event OnUpdateChargePointListResponseDelegate  OnUpdateChargePointListResponse
        {

            add
            {
                CPOClient.OnUpdateChargePointListResponse += value;
            }

            remove
            {
                CPOClient.OnUpdateChargePointListResponse -= value;
            }

        }

        #endregion

        #region OnUpdateStatusRequest/-Response

        /// <summary>
        /// An event fired whenever evse and parking status will be send.
        /// </summary>
        public event OnUpdateStatusRequestDelegate   OnUpdateStatusRequest
        {

            add
            {
                CPOClient.OnUpdateStatusRequest += value;
            }

            remove
            {
                CPOClient.OnUpdateStatusRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request for evse and parking status will be send.
        /// </summary>
        public event ClientRequestLogHandler         OnUpdateStatusSOAPRequest
        {

            add
            {
                CPOClient.OnUpdateStatusSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnUpdateStatusSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an evse and parking status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler        OnUpdateStatusSOAPResponse
        {

            add
            {
                CPOClient.OnUpdateStatusSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnUpdateStatusSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an evse and parking status request had been received.
        /// </summary>
        public event OnUpdateStatusResponseDelegate  OnUpdateStatusResponse
        {

            add
            {
                CPOClient.OnUpdateStatusResponse += value;
            }

            remove
            {
                CPOClient.OnUpdateStatusResponse -= value;
            }

        }

        #endregion


        #region OnGetSingleRoamingAuthorisationRequest/-Response

        /// <summary>
        /// An event fired whenever a request authenticating an e-mobility token will be send.
        /// </summary>
        public event OnGetSingleRoamingAuthorisationRequestDelegate   OnGetSingleRoamingAuthorisationRequest
        {

            add
            {
                CPOClient.OnGetSingleRoamingAuthorisationRequest += value;
            }

            remove
            {
                CPOClient.OnGetSingleRoamingAuthorisationRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request authenticating an e-mobility token will be send.
        /// </summary>
        public event ClientRequestLogHandler                          OnGetSingleRoamingAuthorisationSOAPRequest
        {

            add
            {
                CPOClient.OnGetSingleRoamingAuthorisationSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnGetSingleRoamingAuthorisationSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a authenticate an e-mobility token SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                         OnGetSingleRoamingAuthorisationSOAPResponse
        {

            add
            {
                CPOClient.OnGetSingleRoamingAuthorisationSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnGetSingleRoamingAuthorisationSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a authenticate an e-mobility token request had been received.
        /// </summary>
        public event OnGetSingleRoamingAuthorisationResponseDelegate  OnGetSingleRoamingAuthorisationResponse
        {

            add
            {
                CPOClient.OnGetSingleRoamingAuthorisationResponse += value;
            }

            remove
            {
                CPOClient.OnGetSingleRoamingAuthorisationResponse -= value;
            }

        }

        #endregion

        #region OnGetRoamingAuthorisationListRequest/-Response

        /// <summary>
        /// An event fired whenever a request for the current roaming authorisation list will be send.
        /// </summary>
        public event OnGetRoamingAuthorisationListRequestDelegate   OnGetRoamingAuthorisationListRequest
        {

            add
            {
                CPOClient.OnGetRoamingAuthorisationListRequest += value;
            }

            remove
            {
                CPOClient.OnGetRoamingAuthorisationListRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request for the current roaming authorisation list will be send.
        /// </summary>
        public event ClientRequestLogHandler                        OnGetRoamingAuthorisationListSOAPRequest
        {

            add
            {
                CPOClient.OnGetRoamingAuthorisationListSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnGetRoamingAuthorisationListSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a SOAP request for the current roaming authorisation list had been received.
        /// </summary>
        public event ClientResponseLogHandler                       OnGetRoamingAuthorisationListSOAPResponse
        {

            add
            {
                CPOClient.OnGetRoamingAuthorisationListSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnGetRoamingAuthorisationListSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a request for the current roaming authorisation list had been received.
        /// </summary>
        public event OnGetRoamingAuthorisationListResponseDelegate  OnGetRoamingAuthorisationListResponse
        {

            add
            {
                CPOClient.OnGetRoamingAuthorisationListResponse += value;
            }

            remove
            {
                CPOClient.OnGetRoamingAuthorisationListResponse -= value;
            }

        }

        #endregion

        #region OnGetRoamingAuthorisationListUpdatesRequest/-Response

        /// <summary>
        /// An event fired whenever a request for updates of a roaming authorisation list will be send.
        /// </summary>
        public event OnGetRoamingAuthorisationListUpdatesRequestDelegate   OnGetRoamingAuthorisationListUpdatesRequest
        {

            add
            {
                CPOClient.OnGetRoamingAuthorisationListUpdatesRequest += value;
            }

            remove
            {
                CPOClient.OnGetRoamingAuthorisationListUpdatesRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request for updates of a roaming authorisation list will be send.
        /// </summary>
        public event ClientRequestLogHandler                               OnGetRoamingAuthorisationListUpdatesSOAPRequest
        {

            add
            {
                CPOClient.OnGetRoamingAuthorisationListUpdatesSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnGetRoamingAuthorisationListUpdatesSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a roaming authorisation list update SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                              OnGetRoamingAuthorisationListUpdatesSOAPResponse
        {

            add
            {
                CPOClient.OnGetRoamingAuthorisationListUpdatesSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnGetRoamingAuthorisationListUpdatesSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a roaming authorisation list update request had been received.
        /// </summary>
        public event OnGetRoamingAuthorisationListUpdatesResponseDelegate  OnGetRoamingAuthorisationListUpdatesResponse
        {

            add
            {
                CPOClient.OnGetRoamingAuthorisationListUpdatesResponse += value;
            }

            remove
            {
                CPOClient.OnGetRoamingAuthorisationListUpdatesResponse -= value;
            }

        }

        #endregion


        #region OnAddCDRsRequest/-Response

        /// <summary>
        /// An event fired whenever a request for adding charge detail records will be send.
        /// </summary>
        public event OnAddCDRsRequestDelegate   OnAddCDRsRequest
        {

            add
            {
                CPOClient.OnAddCDRsRequest += value;
            }

            remove
            {
                CPOClient.OnAddCDRsRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request for adding charge detail records will be send.
        /// </summary>
        public event ClientRequestLogHandler    OnAddCDRsSOAPRequest
        {

            add
            {
                CPOClient.OnAddCDRsSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnAddCDRsSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an add charge detail records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler   OnAddCDRsSOAPResponse
        {

            add
            {
                CPOClient.OnAddCDRsSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnAddCDRsSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to an add charge detail records request had been received.
        /// </summary>
        public event OnAddCDRsResponseDelegate  OnAddCDRsResponse
        {

            add
            {
                CPOClient.OnAddCDRsResponse += value;
            }

            remove
            {
                CPOClient.OnAddCDRsResponse -= value;
            }

        }

        #endregion

        #region OnCheckCDRsRequest/-Response

        /// <summary>
        /// An event fired whenever a request for checking charge detail records will be send.
        /// </summary>
        public event OnCheckCDRsRequestDelegate   OnCheckCDRsRequest
        {

            add
            {
                CPOClient.OnCheckCDRsRequest += value;
            }

            remove
            {
                CPOClient.OnCheckCDRsRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request for checking charge detail records will be send.
        /// </summary>
        public event ClientRequestLogHandler      OnCheckCDRsSOAPRequest
        {

            add
            {
                CPOClient.OnCheckCDRsSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnCheckCDRsSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a check charge detail records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler     OnCheckCDRsSOAPResponse
        {

            add
            {
                CPOClient.OnCheckCDRsSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnCheckCDRsSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a check charge detail records request had been received.
        /// </summary>
        public event OnCheckCDRsResponseDelegate  OnCheckCDRsResponse
        {

            add
            {
                CPOClient.OnCheckCDRsResponse += value;
            }

            remove
            {
                CPOClient.OnCheckCDRsResponse -= value;
            }

        }

        #endregion


        #region OnUpdateTariffsRequest/-Response

        /// <summary>
        /// An event fired whenever tariff updates will be send.
        /// </summary>
        public event OnUpdateTariffsRequestDelegate   OnUpdateTariffsRequest
        {

            add
            {
                CPOClient.OnUpdateTariffsRequest += value;
            }

            remove
            {
                CPOClient.OnUpdateTariffsRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request for updating tariffs will be send.
        /// </summary>
        public event ClientRequestLogHandler          OnUpdateTariffsSOAPRequest
        {

            add
            {
                CPOClient.OnUpdateTariffsSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnUpdateTariffsSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to update tariffs SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler         OnUpdateTariffsSOAPResponse
        {

            add
            {
                CPOClient.OnUpdateTariffsSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnUpdateTariffsSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to update tariffs request had been received.
        /// </summary>
        public event OnUpdateTariffsResponseDelegate  OnUpdateTariffsResponse
        {

            add
            {
                CPOClient.OnUpdateTariffsResponse += value;
            }

            remove
            {
                CPOClient.OnUpdateTariffsResponse -= value;
            }

        }

        #endregion


        // OCHPdirect

        #region OnAddServiceEndpointsRequest/-Response

        /// <summary>
        /// An event fired whenever a request to add service endpoints will be send.
        /// </summary>
        public event OnAddServiceEndpointsRequestDelegate   OnAddServiceEndpointsRequest
        {

            add
            {
                CPOClient.OnAddServiceEndpointsRequest += value;
            }

            remove
            {
                CPOClient.OnAddServiceEndpointsRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request to add service endpoints will be send.
        /// </summary>
        public event ClientRequestLogHandler                OnAddServiceEndpointsSOAPRequest
        {

            add
            {
                CPOClient.OnAddServiceEndpointsSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnAddServiceEndpointsSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to add service endpoints had been received.
        /// </summary>
        public event ClientResponseLogHandler               OnAddServiceEndpointsSOAPResponse
        {

            add
            {
                CPOClient.OnAddServiceEndpointsSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnAddServiceEndpointsSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for request to add service endpoints had been received.
        /// </summary>
        public event OnAddServiceEndpointsResponseDelegate  OnAddServiceEndpointsResponse
        {

            add
            {
                CPOClient.OnAddServiceEndpointsResponse += value;
            }

            remove
            {
                CPOClient.OnAddServiceEndpointsResponse -= value;
            }

        }

        #endregion

        #region OnGetServiceEndpointsRequest/-Response

        /// <summary>
        /// An event fired whenever a request to get service endpoints will be send.
        /// </summary>
        public event OnGetServiceEndpointsRequestDelegate   OnGetServiceEndpointsRequest
        {

            add
            {
                CPOClient.OnGetServiceEndpointsRequest += value;
            }

            remove
            {
                CPOClient.OnGetServiceEndpointsRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request to get service endpoints will be send.
        /// </summary>
        public event ClientRequestLogHandler                OnGetServiceEndpointsSOAPRequest
        {

            add
            {
                CPOClient.OnGetServiceEndpointsSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnGetServiceEndpointsSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to get service endpoints had been received.
        /// </summary>
        public event ClientResponseLogHandler               OnGetServiceEndpointsSOAPResponse
        {

            add
            {
                CPOClient.OnGetServiceEndpointsSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnGetServiceEndpointsSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for request to get service endpoints had been received.
        /// </summary>
        public event OnGetServiceEndpointsResponseDelegate  OnGetServiceEndpointsResponse
        {

            add
            {
                CPOClient.OnGetServiceEndpointsResponse += value;
            }

            remove
            {
                CPOClient.OnGetServiceEndpointsResponse -= value;
            }

        }

        #endregion


        #region OnInformProviderRequest/-Response

        /// <summary>
        /// An event fired whenever an inform provider message will be send.
        /// </summary>
        public event OnInformProviderRequestDelegate   OnInformProviderRequest
        {

            add
            {
                CPOClient.OnInformProviderRequest += value;
            }

            remove
            {
                CPOClient.OnInformProviderRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever an inform provider SOAP message will be send.
        /// </summary>
        public event ClientRequestLogHandler           OnInformProviderSOAPRequest
        {

            add
            {
                CPOClient.OnInformProviderSOAPRequest += value;
            }

            remove
            {
                CPOClient.OnInformProviderSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for an inform provider SOAP message had been received.
        /// </summary>
        public event ClientResponseLogHandler          OnInformProviderSOAPResponse
        {

            add
            {
                CPOClient.OnInformProviderSOAPResponse += value;
            }

            remove
            {
                CPOClient.OnInformProviderSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for an inform provider message had been received.
        /// </summary>
        public event OnInformProviderResponseDelegate  OnInformProviderResponse
        {

            add
            {
                CPOClient.OnInformProviderResponse += value;
            }

            remove
            {
                CPOClient.OnInformProviderResponse -= value;
            }

        }

        #endregion


        #region Generic HTTP/SOAP server logging

        /// <summary>
        /// An event called whenever a HTTP request came in.
        /// </summary>
        public HTTPRequestLogEvent   RequestLog    = new HTTPRequestLogEvent();

        /// <summary>
        /// An event called whenever a HTTP request could successfully be processed.
        /// </summary>
        public HTTPResponseLogEvent  ResponseLog   = new HTTPResponseLogEvent();

        /// <summary>
        /// An event called whenever a HTTP request resulted in an error.
        /// </summary>
        public HTTPErrorLogEvent     ErrorLog      = new HTTPErrorLogEvent();

        #endregion

        #endregion

        #region Custom request mappers

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP roaming client for CPOs.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// <param name="CPOServer">A CPO sever.</param>
        public CPORoaming(CPOClient  CPOClient,
                          CPOServer  CPOServer)
        {

            this.CPOClient  = CPOClient ?? throw new ArgumentNullException(nameof(CPOClient), "The given CPOClient must not be null!");
            this.CPOServer  = CPOServer ?? throw new ArgumentNullException(nameof(CPOServer), "The given CPOServer must not be null!");

            // Link HTTP events...
            CPOServer.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            CPOServer.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            CPOServer.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

        }

        #endregion


        // OCHP

        #region SetChargePointList   (Request)

        /// <summary>
        /// Upload the given enumeration of charge points.
        /// </summary>
        /// <param name="Request">A SetChargePointList request.</param>
        public Task<HTTPResponse<SetChargePointListResponse>>

            SetChargePointList(SetChargePointListRequest Request)

                => CPOClient.SetChargePointList(Request);

        #endregion

        #region UpdateChargePointList(Request)

        /// <summary>
        /// Update the given enumeration of charge points.
        /// </summary>
        /// <param name="Request">An UpdateChargePointList request.</param>
        public Task<HTTPResponse<UpdateChargePointListResponse>>

            UpdateChargePointList(UpdateChargePointListRequest Request)

                => CPOClient.UpdateChargePointList(Request);

        #endregion

        #region UpdateStatus         (Request)

        /// <summary>
        /// Upload the given enumeration of EVSE and/or parking status.
        /// </summary>
        /// <param name="Request">An UpdateStatus request.</param>
        public Task<HTTPResponse<UpdateStatusResponse>>

            UpdateStatus(UpdateStatusRequest Request)

                => CPOClient.UpdateStatus(Request);

        #endregion

        #region UpdateTariffs        (Request)

        /// <summary>
        /// Upload the given enumeration of tariff infos.
        /// </summary>
        /// <param name="Request">An UpdateTariffs request.</param>
        public Task<HTTPResponse<UpdateTariffsResponse>>

            UpdateTariffs(UpdateTariffsRequest Request)

                => CPOClient.UpdateTariffs(Request);

        #endregion


        #region GetSingleRoamingAuthorisation     (Request)

        /// <summary>
        /// Create a new OCHP GetSingleRoamingAuthorisation request.
        /// </summary>
        /// <param name="Request">A GetSingleRoamingAuthorisation request.</param>
        public Task<HTTPResponse<GetSingleRoamingAuthorisationResponse>>

            GetSingleRoamingAuthorisation(GetSingleRoamingAuthorisationRequest Request)

                => CPOClient.GetSingleRoamingAuthorisation(Request);

        #endregion

        #region GetRoamingAuthorisationList       (Request)

        /// <summary>
        /// Get the entire current version of the roaming authorisation list.
        /// </summary>
        /// <param name="Request">A GetRoamingAuthorisationList request.</param>
        public Task<HTTPResponse<GetRoamingAuthorisationListResponse>>

            GetRoamingAuthorisationList(GetRoamingAuthorisationListRequest Request)

                => CPOClient.GetRoamingAuthorisationList(Request);

        #endregion

        #region GetRoamingAuthorisationListUpdates(Request)

        /// <summary>
        /// Get the entire current version of the roaming authorisation list.
        /// </summary>
        /// <param name="Request">A GetRoamingAuthorisationListUpdates request.</param>
        public Task<HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>>

            GetRoamingAuthorisationListUpdates(GetRoamingAuthorisationListUpdatesRequest Request)

                => CPOClient.GetRoamingAuthorisationListUpdates(Request);

        #endregion


        #region AddCDRs  (Request)

        /// <summary>
        /// Upload the given enumeration of charge detail records.
        /// </summary>
        /// <param name="Request">A AddCDRs request.</param>
        public Task<HTTPResponse<AddCDRsResponse>>

            AddCDRs(AddCDRsRequest Request)

                => CPOClient.AddCDRs(Request);

        #endregion

        #region CheckCDRs(Request)

        /// <summary>
        /// Check charge detail records having the given optional status.
        /// </summary>
        /// <param name="Request">A CheckCDRs request.</param>
        public Task<HTTPResponse<CheckCDRsResponse>>

            CheckCDRs(CheckCDRsRequest Request)

                => CPOClient.CheckCDRs(Request);

        #endregion


        // OCHPdirect

        #region AddServiceEndpoints(Request)

        /// <summary>
        /// Upload the given enumeration of OCHPdirect provider endpoints.
        /// </summary>
        /// <param name="Request">An AddServiceEndpoints request.</param>
        public Task<HTTPResponse<AddServiceEndpointsResponse>>

            AddServiceEndpoints(AddServiceEndpointsRequest Request)

                => CPOClient.AddServiceEndpoints(Request);

        #endregion

        #region GetServiceEndpoints(Request)

        /// <summary>
        /// Download OCHPdirect provider endpoints.
        /// </summary>
        /// <param name="Request">A GetServiceEndpoints request.</param>
        public Task<HTTPResponse<GetServiceEndpointsResponse>>

            GetServiceEndpoints(GetServiceEndpointsRequest Request)

                => CPOClient.GetServiceEndpoints(Request);

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
        public Task<HTTPResponse<InformProviderResponse>>

            InformProvider(DirectMessages           DirectMessage,
                           EVSE_Id                  EVSEId,
                           Contract_Id              ContractId,
                           Direct_Id                DirectId,

                           DateTimeOffset?          SessionTimeoutAt    = null,
                           Single?                  StateOfCharge       = null,
                           Single?                  MaxPower            = null,
                           Single?                  MaxCurrent          = null,
                           Boolean?                 OnePhase            = null,
                           Single?                  MaxEnergy           = null,
                           Single?                  MinEnergy           = null,
                           DateTimeOffset?          Departure           = null,
                           Single?                  CurrentPower        = null,
                           Single?                  ChargedEnergy       = null,
                           Timestamped<Single>?     MeterReading        = null,
                           IEnumerable<CDRPeriod>?  ChargingPeriods     = null,
                           Single?                  CurrentCost         = null,
                           Currency?                Currency            = null,

                           DateTimeOffset?          Timestamp           = null,
                           EventTracking_Id?        EventTrackingId     = null,
                           TimeSpan?                RequestTimeout      = null,
                           CancellationToken        CancellationToken   = default)


                => CPOClient.InformProvider(DirectMessage,
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

                                            Timestamp,
                                            EventTrackingId,
                                            RequestTimeout,
                                            CancellationToken);

        #endregion



        #region Start    (EventTrackingId = null)

        ///// <summary>
        ///// Start this HTTP API.
        ///// </summary>
        ///// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        //public async Task<Boolean> Start(EventTracking_Id? EventTrackingId = null)
        //{

        //    var result = await CPOServer.Start(
        //                           EventTrackingId ?? EventTracking_Id.New
        //                       );

        //    //SendStarted(this, CurrentTimestamp);

        //    return result;

        //}

        #endregion

        #region Shutdown (EventTrackingId = null, Message = null, Wait = true)

        ///// <summary>
        ///// Shutdown this HTTP API.
        ///// </summary>
        ///// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
        ///// <param name="Message">An optional shutdown message.</param>
        ///// <param name="Wait">Whether to wait for the shutdown to complete.</param>
        //public async virtual Task<Boolean> Shutdown(EventTracking_Id?  EventTrackingId   = null,
        //                                            String?            Message           = null,
        //                                            Boolean            Wait              = true)
        //{

        //    var result = await CPOServer.Shutdown(
        //                           EventTrackingId ?? EventTracking_Id.New,
        //                           Message,
        //                           Wait
        //                       );

        //    //SendShutdown(this, CurrentTimestamp);

        //    return result;

        //}

        #endregion

        #region Dispose()

        public virtual void Dispose()
        {
            CPOServer.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion

    }

}
