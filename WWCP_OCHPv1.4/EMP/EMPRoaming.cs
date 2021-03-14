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
using System.Xml.Linq;
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// An OICP roaming client for EMPs.
    /// </summary>
    public class EMPRoaming : IEMPClient
    {

        #region Properties

        /// <summary>
        /// The EMP client part.
        /// </summary>
        public EMPClient        EMPClient           { get; }

        #region IEMPClient

        /// <summary>
        /// The remote URL of the OICP HTTP endpoint to connect to.
        /// </summary>
        URL IHTTPClient.RemoteURL
            => EMPClient.RemoteURL;

        /// <summary>
        /// The virtual HTTP hostname to connect to.
        /// </summary>
        HTTPHostname? IHTTPClient.VirtualHostname
            => EMPClient.VirtualHostname;

        /// <summary>
        /// An optional description of this CPO client.
        /// </summary>
        String IHTTPClient.Description
        {

            get
            {
                return EMPClient.Description;
            }

            set
            {
                EMPClient.Description = value;
            }

        }

        /// <summary>
        /// The remote SSL/TLS certificate validator.
        /// </summary>
        RemoteCertificateValidationCallback IHTTPClient.RemoteCertificateValidator
            => EMPClient.RemoteCertificateValidator;

        /// <summary>
        /// The SSL/TLS client certificate to use of HTTP authentication.
        /// </summary>
        X509Certificate IHTTPClient.ClientCert
            => EMPClient.ClientCert;

        /// <summary>
        /// The HTTP user agent identification.
        /// </summary>
        String IHTTPClient.HTTPUserAgent
            => EMPClient.HTTPUserAgent;

        /// <summary>
        /// The timeout for upstream requests.
        /// </summary>
        TimeSpan IHTTPClient.RequestTimeout
        {

            get
            {
                return EMPClient.RequestTimeout;
            }

            set
            {
                EMPClient.RequestTimeout = value;
            }

        }

        /// <summary>
        /// The delay between transmission retries.
        /// </summary>
        TransmissionRetryDelayDelegate IHTTPClient.TransmissionRetryDelay
            => EMPClient.TransmissionRetryDelay;

        /// <summary>
        /// The maximum number of retries when communicationg with the remote OICP service.
        /// </summary>
        UInt16 IHTTPClient.MaxNumberOfRetries
            => EMPClient.MaxNumberOfRetries;

        /// <summary>
        /// Make use of HTTP pipelining.
        /// </summary>
        Boolean IHTTPClient.UseHTTPPipelining
            => EMPClient.UseHTTPPipelining;

        /// <summary>
        /// The CPO client (HTTP client) logger.
        /// </summary>
        HTTPClientLogger IHTTPClient.HTTPLogger
        {

            get
            {
                return EMPClient.HTTPLogger;
            }

            set
            {
                EMPClient.HTTPLogger = value;
            }

        }

        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        DNSClient IHTTPClient.DNSClient
            => EMPClient.DNSClient;

        #endregion

        /// <summary>
        /// The EMP server.
        /// </summary>
        public EMPServer        EMPServer           { get; }


        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        public DNSClient DNSClient
            => EMPServer?.DNSClient;

        #endregion

        #region Events

        // OCHP

        #region OnGetChargePointListRequest/-Response

        /// <summary>
        /// An event fired whenever a request to download the charge points list will be send.
        /// </summary>
        public event OnGetChargePointListRequestDelegate   OnGetChargePointListRequest
        {

            add
            {
                EMPClient.OnGetChargePointListRequest += value;
            }

            remove
            {
                EMPClient.OnGetChargePointListRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request to download the charge points list will be send.
        /// </summary>
        public event ClientRequestLogHandler               OnGetChargePointListSOAPRequest
        {

            add
            {
                EMPClient.OnGetChargePointListSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnGetChargePointListSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a charge points list download SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler              OnGetChargePointListSOAPResponse
        {

            add
            {
                EMPClient.OnGetChargePointListSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnGetChargePointListSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a charge points list download request had been received.
        /// </summary>
        public event OnGetChargePointListResponseDelegate  OnGetChargePointListResponse
        {

            add
            {
                EMPClient.OnGetChargePointListResponse += value;
            }

            remove
            {
                EMPClient.OnGetChargePointListResponse -= value;
            }

        }

        #endregion

        #region OnGetChargePointListUpdatesRequest/-Response

        /// <summary>
        /// An event fired whenever a request to download a charge points list update will be send.
        /// </summary>
        public event OnGetChargePointListUpdatesRequestDelegate   OnGetChargePointListUpdatesRequest
        {

            add
            {
                EMPClient.OnGetChargePointListUpdatesRequest += value;
            }

            remove
            {
                EMPClient.OnGetChargePointListUpdatesRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request to download a charge points list update will be send.
        /// </summary>
        public event ClientRequestLogHandler                      OnGetChargePointListUpdatesSOAPRequest
        {

            add
            {
                EMPClient.OnGetChargePointListUpdatesSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnGetChargePointListUpdatesSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a charge points list update download SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler                     OnGetChargePointListUpdatesSOAPResponse
        {

            add
            {
                EMPClient.OnGetChargePointListUpdatesSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnGetChargePointListUpdatesSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response to a charge points list update download request had been received.
        /// </summary>
        public event OnGetChargePointListUpdatesResponseDelegate  OnGetChargePointListUpdatesResponse
        {

            add
            {
                EMPClient.OnGetChargePointListUpdatesResponse += value;
            }

            remove
            {
                EMPClient.OnGetChargePointListUpdatesResponse -= value;
            }

        }

        #endregion

        #region OnGetStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request for evse or parking status will be send.
        /// </summary>
        public event OnGetStatusRequestDelegate   OnGetStatusRequest
        {

            add
            {
                EMPClient.OnGetStatusRequest += value;
            }

            remove
            {
                EMPClient.OnGetStatusRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request for evse or parking status will be send.
        /// </summary>
        public event ClientRequestLogHandler      OnGetStatusSOAPRequest
        {

            add
            {
                EMPClient.OnGetStatusSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnGetStatusSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for a evse or parking status SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler     OnGetStatusSOAPResponse
        {

            add
            {
                EMPClient.OnGetStatusSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnGetStatusSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for a evse or parking status request had been received.
        /// </summary>
        public event OnGetStatusResponseDelegate  OnGetStatusResponse
        {

            add
            {
                EMPClient.OnGetStatusResponse += value;
            }

            remove
            {
                EMPClient.OnGetStatusResponse -= value;
            }

        }

        #endregion


        #region OnSetRoamingAuthorisationListRequest/-Response

        /// <summary>
        /// An event fired whenever a roaming authorisation list will be send.
        /// </summary>
        public event OnSetRoamingAuthorisationListRequestDelegate   OnSetRoamingAuthorisationListRequest
        {

            add
            {
                EMPClient.OnSetRoamingAuthorisationListRequest += value;
            }

            remove
            {
                EMPClient.OnSetRoamingAuthorisationListRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request for uploading a roaming authorisation list will be send.
        /// </summary>
        public event ClientRequestLogHandler                        OnSetRoamingAuthorisationListSOAPRequest
        {

            add
            {
                EMPClient.OnSetRoamingAuthorisationListSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnSetRoamingAuthorisationListSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for uploading a roaming authorisation list had been received.
        /// </summary>
        public event ClientResponseLogHandler                       OnSetRoamingAuthorisationListSOAPResponse
        {

            add
            {
                EMPClient.OnSetRoamingAuthorisationListSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnSetRoamingAuthorisationListSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for uploading a roaming authorisation list had been received.
        /// </summary>
        public event OnSetRoamingAuthorisationListResponseDelegate  OnSetRoamingAuthorisationListResponse
        {

            add
            {
                EMPClient.OnSetRoamingAuthorisationListResponse += value;
            }

            remove
            {
                EMPClient.OnSetRoamingAuthorisationListResponse -= value;
            }

        }

        #endregion

        #region OnUpdateRoamingAuthorisationListRequest/-Response

        /// <summary>
        /// An event fired whenever a roaming authorisation list update will be send.
        /// </summary>
        public event OnUpdateRoamingAuthorisationListRequestDelegate   OnUpdateRoamingAuthorisationListRequest
        {

            add
            {
                EMPClient.OnUpdateRoamingAuthorisationListRequest += value;
            }

            remove
            {
                EMPClient.OnUpdateRoamingAuthorisationListRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request for uploading a roaming authorisation list update will be send.
        /// </summary>
        public event ClientRequestLogHandler                           OnUpdateRoamingAuthorisationListSOAPRequest
        {

            add
            {
                EMPClient.OnUpdateRoamingAuthorisationListSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnUpdateRoamingAuthorisationListSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for uploading a roaming authorisation list update had been received.
        /// </summary>
        public event ClientResponseLogHandler                          OnUpdateRoamingAuthorisationListSOAPResponse
        {

            add
            {
                EMPClient.OnUpdateRoamingAuthorisationListSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnUpdateRoamingAuthorisationListSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for uploading a roaming authorisation list update had been received.
        /// </summary>
        public event OnUpdateRoamingAuthorisationListResponseDelegate  OnUpdateRoamingAuthorisationListResponse
        {

            add
            {
                EMPClient.OnUpdateRoamingAuthorisationListResponse += value;
            }

            remove
            {
                EMPClient.OnUpdateRoamingAuthorisationListResponse -= value;
            }

        }

        #endregion


        #region OnGetCDRsRequest/-Response

        /// <summary>
        /// An event fired whenever a request for charge detail records will be send.
        /// </summary>
        public event OnGetCDRsRequestDelegate   OnGetCDRsRequest
        {

            add
            {
                EMPClient.OnGetCDRsRequest += value;
            }

            remove
            {
                EMPClient.OnGetCDRsRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request for charge detail records will be send.
        /// </summary>
        public event ClientRequestLogHandler    OnGetCDRsSOAPRequest
        {

            add
            {
                EMPClient.OnGetCDRsSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnGetCDRsSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for a charge detail records SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler   OnGetCDRsSOAPResponse
        {

            add
            {
                EMPClient.OnGetCDRsSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnGetCDRsSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for a charge detail records request had been received.
        /// </summary>
        public event OnGetCDRsResponseDelegate  OnGetCDRsResponse
        {

            add
            {
                EMPClient.OnGetCDRsResponse += value;
            }

            remove
            {
                EMPClient.OnGetCDRsResponse -= value;
            }

        }

        #endregion

        #region OnConfirmCDRsRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record confirmation request will be send.
        /// </summary>
        public event OnConfirmCDRsRequestDelegate   OnConfirmCDRsRequest
        {

            add
            {
                EMPClient.OnConfirmCDRsRequest += value;
            }

            remove
            {
                EMPClient.OnConfirmCDRsRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a charge detail record confirmation SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler        OnConfirmCDRsSOAPRequest
        {

            add
            {
                EMPClient.OnConfirmCDRsSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnConfirmCDRsSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for a charge detail record confirmation SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler       OnConfirmCDRsSOAPResponse
        {

            add
            {
                EMPClient.OnConfirmCDRsSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnConfirmCDRsSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for a charge detail record confirmation request had been received.
        /// </summary>
        public event OnConfirmCDRsResponseDelegate  OnConfirmCDRsResponse
        {

            add
            {
                EMPClient.OnConfirmCDRsResponse += value;
            }

            remove
            {
                EMPClient.OnConfirmCDRsResponse -= value;
            }

        }

        #endregion


        #region OnGetTariffUpdatesRequest/-Response

        /// <summary>
        /// An event fired whenever a request for tariff infos will be send.
        /// </summary>
        public event OnGetTariffUpdatesRequestDelegate   OnGetTariffUpdatesRequest
        {

            add
            {
                EMPClient.OnGetTariffUpdatesRequest += value;
            }

            remove
            {
                EMPClient.OnGetTariffUpdatesRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request for tariff infos will be send.
        /// </summary>
        public event ClientRequestLogHandler             OnGetTariffUpdatesSOAPRequest
        {

            add
            {
                EMPClient.OnGetTariffUpdatesSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnGetTariffUpdatesSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for a tariff infos SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler            OnGetTariffUpdatesSOAPResponse
        {

            add
            {
                EMPClient.OnGetTariffUpdatesSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnGetTariffUpdatesSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for a tariff infos request had been received.
        /// </summary>
        public event OnGetTariffUpdatesResponseDelegate  OnGetTariffUpdatesResponse
        {

            add
            {
                EMPClient.OnGetTariffUpdatesResponse += value;
            }

            remove
            {
                EMPClient.OnGetTariffUpdatesResponse -= value;
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
                EMPClient.OnAddServiceEndpointsRequest += value;
            }

            remove
            {
                EMPClient.OnAddServiceEndpointsRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request to add service endpoints will be send.
        /// </summary>
        public event ClientRequestLogHandler                OnAddServiceEndpointsSOAPRequest
        {

            add
            {
                EMPClient.OnAddServiceEndpointsSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnAddServiceEndpointsSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to add service endpoints had been received.
        /// </summary>
        public event ClientResponseLogHandler               OnAddServiceEndpointsSOAPResponse
        {

            add
            {
                EMPClient.OnAddServiceEndpointsSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnAddServiceEndpointsSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for request to add service endpoints had been received.
        /// </summary>
        public event OnAddServiceEndpointsResponseDelegate  OnAddServiceEndpointsResponse
        {

            add
            {
                EMPClient.OnAddServiceEndpointsResponse += value;
            }

            remove
            {
                EMPClient.OnAddServiceEndpointsResponse -= value;
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
                EMPClient.OnGetServiceEndpointsRequest += value;
            }

            remove
            {
                EMPClient.OnGetServiceEndpointsRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request to get service endpoints will be send.
        /// </summary>
        public event ClientRequestLogHandler                OnGetServiceEndpointsSOAPRequest
        {

            add
            {
                EMPClient.OnGetServiceEndpointsSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnGetServiceEndpointsSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to get service endpoints had been received.
        /// </summary>
        public event ClientResponseLogHandler               OnGetServiceEndpointsSOAPResponse
        {

            add
            {
                EMPClient.OnGetServiceEndpointsSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnGetServiceEndpointsSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for request to get service endpoints had been received.
        /// </summary>
        public event OnGetServiceEndpointsResponseDelegate  OnGetServiceEndpointsResponse
        {

            add
            {
                EMPClient.OnGetServiceEndpointsResponse += value;
            }

            remove
            {
                EMPClient.OnGetServiceEndpointsResponse -= value;
            }

        }

        #endregion


        #region OnSelectEVSERequest/-Response

        /// <summary>
        /// An event fired whenever a request to select an EVSE will be send.
        /// </summary>
        public event OnSelectEVSERequestDelegate   OnSelectEVSERequest
        {

            add
            {
                EMPClient.OnSelectEVSERequest += value;
            }

            remove
            {
                EMPClient.OnSelectEVSERequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request to select an EVSE will be send.
        /// </summary>
        public event ClientRequestLogHandler       OnSelectEVSESOAPRequest
        {

            add
            {
                EMPClient.OnSelectEVSESOAPRequest += value;
            }

            remove
            {
                EMPClient.OnSelectEVSESOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to select an EVSE had been received.
        /// </summary>
        public event ClientResponseLogHandler      OnSelectEVSESOAPResponse
        {

            add
            {
                EMPClient.OnSelectEVSESOAPResponse += value;
            }

            remove
            {
                EMPClient.OnSelectEVSESOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for request to select an EVSE had been received.
        /// </summary>
        public event OnSelectEVSEResponseDelegate  OnSelectEVSEResponse
        {

            add
            {
                EMPClient.OnSelectEVSEResponse += value;
            }

            remove
            {
                EMPClient.OnSelectEVSEResponse -= value;
            }

        }

        #endregion

        #region OnControlEVSERequest/-Response

        /// <summary>
        /// An event fired whenever a request to control a direct charging process will be send.
        /// </summary>
        public event OnControlEVSERequestDelegate   OnControlEVSERequest
        {

            add
            {
                EMPClient.OnControlEVSERequest += value;
            }

            remove
            {
                EMPClient.OnControlEVSERequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request to control a direct charging process will be send.
        /// </summary>
        public event ClientRequestLogHandler        OnControlEVSESOAPRequest
        {

            add
            {
                EMPClient.OnControlEVSESOAPRequest += value;
            }

            remove
            {
                EMPClient.OnControlEVSESOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to control a direct charging process had been received.
        /// </summary>
        public event ClientResponseLogHandler       OnControlEVSESOAPResponse
        {

            add
            {
                EMPClient.OnControlEVSESOAPResponse += value;
            }

            remove
            {
                EMPClient.OnControlEVSESOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for request to control a direct charging process had been received.
        /// </summary>
        public event OnControlEVSEResponseDelegate  OnControlEVSEResponse
        {

            add
            {
                EMPClient.OnControlEVSEResponse += value;
            }

            remove
            {
                EMPClient.OnControlEVSEResponse -= value;
            }

        }

        #endregion

        #region OnReleaseEVSERequest/-Response

        /// <summary>
        /// An event fired whenever a request to release an EVSE will be send.
        /// </summary>
        public event OnReleaseEVSERequestDelegate   OnReleaseEVSERequest
        {

            add
            {
                EMPClient.OnReleaseEVSERequest += value;
            }

            remove
            {
                EMPClient.OnReleaseEVSERequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request to release an EVSE will be send.
        /// </summary>
        public event ClientRequestLogHandler        OnReleaseEVSESOAPRequest
        {

            add
            {
                EMPClient.OnReleaseEVSESOAPRequest += value;
            }

            remove
            {
                EMPClient.OnReleaseEVSESOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to release an EVSE had been received.
        /// </summary>
        public event ClientResponseLogHandler       OnReleaseEVSESOAPResponse
        {

            add
            {
                EMPClient.OnReleaseEVSESOAPResponse += value;
            }

            remove
            {
                EMPClient.OnReleaseEVSESOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for request to release an EVSE had been received.
        /// </summary>
        public event OnReleaseEVSEResponseDelegate  OnReleaseEVSEResponse
        {

            add
            {
                EMPClient.OnReleaseEVSEResponse += value;
            }

            remove
            {
                EMPClient.OnReleaseEVSEResponse -= value;
            }

        }

        #endregion

        #region OnGetEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request for EVSE status will be send.
        /// </summary>
        public event OnGetEVSEStatusRequestDelegate   OnGetEVSEStatusRequest
        {

            add
            {
                EMPClient.OnGetEVSEStatusRequest += value;
            }

            remove
            {
                EMPClient.OnGetEVSEStatusRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP request for EVSE status will be send.
        /// </summary>
        public event ClientRequestLogHandler          OnGetEVSEStatusSOAPRequest
        {

            add
            {
                EMPClient.OnGetEVSEStatusSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnGetEVSEStatusSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request for EVSE status had been received.
        /// </summary>
        public event ClientResponseLogHandler         OnGetEVSEStatusSOAPResponse
        {

            add
            {
                EMPClient.OnGetEVSEStatusSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnGetEVSEStatusSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for an EVSE status request had been received.
        /// </summary>
        public event OnGetEVSEStatusResponseDelegate  OnGetEVSEStatusResponse
        {

            add
            {
                EMPClient.OnGetEVSEStatusResponse += value;
            }

            remove
            {
                EMPClient.OnGetEVSEStatusResponse -= value;
            }

        }

        #endregion


        #region OnReportDiscrepancyRequest/-Response

        /// <summary>
        /// An event fired whenever a report discrepancy request will be send.
        /// </summary>
        public event OnReportDiscrepancyRequestDelegate   OnReportDiscrepancyRequest
        {

            add
            {
                EMPClient.OnReportDiscrepancyRequest += value;
            }

            remove
            {
                EMPClient.OnReportDiscrepancyRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a report discrepancy SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler              OnReportDiscrepancySOAPRequest
        {

            add
            {
                EMPClient.OnReportDiscrepancySOAPRequest += value;
            }

            remove
            {
                EMPClient.OnReportDiscrepancySOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for a report discrepancy SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler             OnReportDiscrepancySOAPResponse
        {

            add
            {
                EMPClient.OnReportDiscrepancySOAPResponse += value;
            }

            remove
            {
                EMPClient.OnReportDiscrepancySOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for a report discrepancy request had been received.
        /// </summary>
        public event OnReportDiscrepancyResponseDelegate  OnReportDiscrepancyResponse
        {

            add
            {
                EMPClient.OnReportDiscrepancyResponse += value;
            }

            remove
            {
                EMPClient.OnReportDiscrepancyResponse -= value;
            }

        }

        #endregion

        #region OnGetInformProviderRequest/-Response

        /// <summary>
        /// An event fired whenever a get inform provider request will be send.
        /// </summary>
        public event OnGetInformProviderRequestDelegate   OnGetInformProviderRequest
        {

            add
            {
                EMPClient.OnGetInformProviderRequest += value;
            }

            remove
            {
                EMPClient.OnGetInformProviderRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a get inform provider SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler              OnGetInformProviderSOAPRequest
        {

            add
            {
                EMPClient.OnGetInformProviderSOAPRequest += value;
            }

            remove
            {
                EMPClient.OnGetInformProviderSOAPRequest -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a SOAP response for a get inform provider SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler             OnGetInformProviderSOAPResponse
        {

            add
            {
                EMPClient.OnGetInformProviderSOAPResponse += value;
            }

            remove
            {
                EMPClient.OnGetInformProviderSOAPResponse -= value;
            }

        }

        /// <summary>
        /// An event fired whenever a response for a get inform provider request had been received.
        /// </summary>
        public event OnGetInformProviderResponseDelegate  OnGetInformProviderResponse
        {

            add
            {
                EMPClient.OnGetInformProviderResponse += value;
            }

            remove
            {
                EMPClient.OnGetInformProviderResponse -= value;
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

        #region Custom request/response mappers


        #endregion

        #region Constructor(s)

        #region EMPRoaming(EMPClient, EMPServer)

        /// <summary>
        /// Create a new OICP roaming client for EMPs.
        /// </summary>
        /// <param name="EMPClient">A EMP client.</param>
        /// <param name="EMPServer">A EMP sever.</param>
        public EMPRoaming(EMPClient  EMPClient,
                          EMPServer  EMPServer)
        {

            this.EMPClient  = EMPClient ?? throw new ArgumentNullException(nameof(EMPClient), "The given EMPClient must not be null!");
            this.EMPServer  = EMPServer ?? throw new ArgumentNullException(nameof(EMPServer), "The given EMPServer must not be null!");

            // Link HTTP events...
            EMPServer.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            EMPServer.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            EMPServer.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

        }

        #endregion

        #region EMPRoaming(ClientId, RemoteHostname, RemoteTCPPort = null, RemoteHTTPVirtualHost = null, ... )

        /// <summary>
        /// Create a new OICP roaming client for EMPs.
        /// </summary>
        /// <param name="RemoteURL">The remote URL of the OICP HTTP endpoint to connect to.</param>
        /// <param name="VirtualHostname">An optional HTTP virtual hostname.</param>
        /// <param name="Description">An optional description of this CPO client.</param>
        /// <param name="RemoteCertificateValidator">The remote SSL/TLS certificate validator.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="ClientCert">The SSL/TLS client certificate to use of HTTP authentication.</param>
        /// <param name="HTTPUserAgent">The HTTP user agent identification.</param>
        /// <param name="URLPathPrefix">An optional default URL path prefix.</param>
        /// <param name="WSSLoginPassword">The WebService-Security username/password.</param>
        /// <param name="RequestTimeout">An optional request timeout.</param>
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The maximum number of transmission retries for HTTP request.</param>
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ClientLogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerServiceName">An optional identification for this SOAP service.</param>
        /// <param name="ServerURLPrefix">An optional prefix for the HTTP URLs.</param>
        /// <param name="ServerURLSuffix">An optional HTTP/SOAP/XML server URI suffix.</param>
        /// <param name="ServerContentType">An optional HTTP content type to use.</param>
        /// <param name="ServerRegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="ServerLogfileCreator">A delegate to create a log file from the given context and logfile name.</param>
        /// <param name="ServerAutoStart">Start the server immediately.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public EMPRoaming(URL                                  RemoteURL,
                          HTTPHostname?                        VirtualHostname                 = null,
                          String                               Description                     = null,
                          RemoteCertificateValidationCallback  RemoteCertificateValidator      = null,
                          LocalCertificateSelectionCallback    ClientCertificateSelector       = null,
                          X509Certificate                      ClientCert                      = null,
                          String                               HTTPUserAgent                   = EMPClient.DefaultHTTPUserAgent,
                          HTTPPath?                            URLPathPrefix                   = null,
                          HTTPPath?                            LiveURLPathPrefix               = null,
                          Tuple<String, String>                WSSLoginPassword                = null,
                          TimeSpan?                            RequestTimeout                  = null,
                          TransmissionRetryDelayDelegate       TransmissionRetryDelay          = null,
                          UInt16?                              MaxNumberOfRetries              = EMPClient.DefaultMaxNumberOfRetries,
                          String                               ClientLoggingContext            = EMPClient.EMPClientLogger.DefaultContext,
                          LogfileCreatorDelegate               ClientLogfileCreator            = null,

                          String                               ServerName                      = EMPServer.DefaultHTTPServerName,
                          IPPort?                              ServerTCPPort                   = null,
                          String                               ServerServiceName               = null,
                          HTTPPath?                            ServerURLPrefix                 = null,
                          HTTPPath?                            ServerURLSuffix                 = null,
                          HTTPContentType                      ServerContentType               = null,
                          Boolean                              ServerRegisterHTTPRootService   = true,
                          String                               ServerLoggingContext            = EMPServerLogger.DefaultContext,
                          LogfileCreatorDelegate               ServerLogfileCreator            = null,
                          Boolean                              ServerAutoStart                 = false,

                          DNSClient                            DNSClient                       = null)

            : this(new EMPClient(RemoteURL,
                                 VirtualHostname,
                                 Description,
                                 RemoteCertificateValidator,
                                 ClientCertificateSelector,
                                 ClientCert,
                                 HTTPUserAgent,
                                 URLPathPrefix,
                                 LiveURLPathPrefix,
                                 WSSLoginPassword,
                                 RequestTimeout,
                                 TransmissionRetryDelay,
                                 MaxNumberOfRetries,
                                 ClientLoggingContext,
                                 ClientLogfileCreator,
                                 DNSClient),

                   new EMPServer(ServerName,
                                 ServerTCPPort,
                                 ServerServiceName,
                                 ServerURLPrefix ?? EMPServer.DefaultURLPrefix,
                                 ServerURLSuffix ?? EMPServer.DefaultURLSuffix,
                                 ServerContentType,
                                 ServerRegisterHTTPRootService,
                                 ServerLoggingContext,
                                 ServerLogfileCreator,
                                 DNSClient,
                                 false))

        {

            if (ServerAutoStart)
                Start();

        }

        #endregion

        #endregion


        // OCHP

        #region GetChargePointList       (Request)

        /// <summary>
        /// Download the current list of charge points.
        /// </summary>
        /// <param name="Request">A GetChargePointList request.</param>
        public Task<HTTPResponse<GetChargePointListResponse>>

            GetChargePointList(GetChargePointListRequest Request)

                => EMPClient.GetChargePointList(Request);

        #endregion

        #region GetChargePointListUpdates(Request)

        /// <summary>
        /// Download all charge point list updates since the given date.
        /// </summary>
        /// <param name="Request">A GetChargePointListUpdates request.</param>
        public Task<HTTPResponse<GetChargePointListUpdatesResponse>>

            GetChargePointListUpdates(GetChargePointListUpdatesRequest Request)

                => EMPClient.GetChargePointListUpdates(Request);

        #endregion

        #region GetStatus                (Request)

        /// <summary>
        /// Download the current list of charge point status filtered by
        /// an optional last request timestamp or their status type.
        /// </summary>
        /// <param name="Request">A GetStatus request.</param>
        public Task<HTTPResponse<GetStatusResponse>>

            GetStatus(GetStatusRequest Request)

                => EMPClient.GetStatus(Request);

        #endregion

        #region GetChargePointListUpdates(Request)

        /// <summary>
        /// Download an update of the current tariff list since the given date.
        /// </summary>
        /// <param name="Request">A GetTariffUpdates request.</param>
        public Task<HTTPResponse<GetTariffUpdatesResponse>>

            GetTariffUpdates(GetTariffUpdatesRequest Request)

                => EMPClient.GetTariffUpdates(Request);

        #endregion


        #region SetRoamingAuthorisationList   (Request)

        /// <summary>
        /// Upload the entire roaming authorisation list.
        /// </summary>
        /// <param name="Request">A SetRoamingAuthorisationList request.</param>
        public Task<HTTPResponse<SetRoamingAuthorisationListResponse>>

            SetRoamingAuthorisationList(SetRoamingAuthorisationListRequest Request)

                => EMPClient.SetRoamingAuthorisationList(Request);

        #endregion

        #region UpdateRoamingAuthorisationList(Request)

        /// <summary>
        /// Send a roaming authorisation list update.
        /// </summary>
        /// <param name="Request">An UpdateRoamingAuthorisationList request.</param>
        public Task<HTTPResponse<UpdateRoamingAuthorisationListResponse>>

            UpdateRoamingAuthorisationList(UpdateRoamingAuthorisationListRequest Request)

                => EMPClient.UpdateRoamingAuthorisationList(Request);

        #endregion


        #region GetCDRsRequest    (Request)

        /// <summary>
        /// Download charge detail records having the given optional status.
        /// </summary>
        /// <param name="Request">A GetCDRs request.</param>
        public Task<HTTPResponse<GetCDRsResponse>>

            GetCDRs(GetCDRsRequest Request)

                => EMPClient.GetCDRs(Request);

        #endregion

        #region ConfirmCDRsRequest(Request)

        /// <summary>
        /// Approve or decline charge detail records.
        /// </summary>
        /// <param name="Request">A ConfirmCDRs request.</param>
        public Task<HTTPResponse<ConfirmCDRsResponse>>

            ConfirmCDRs(ConfirmCDRsRequest Request)

                => EMPClient.ConfirmCDRs(Request);

        #endregion


        // OCHP direct

        #region AddServiceEndpoints(Request)

        /// <summary>
        /// Upload the given enumeration of OCHPdirect provider endpoints.
        /// </summary>
        /// <param name="Request">An AddServiceEndpoints request.</param>
        public Task<HTTPResponse<AddServiceEndpointsResponse>>

            AddServiceEndpoints(AddServiceEndpointsRequest Request)

                => EMPClient.AddServiceEndpoints(Request);

        #endregion

        #region GetServiceEndpoints(Request)

        /// <summary>
        /// Download OCHPdirect provider endpoints.
        /// </summary>
        /// <param name="Request">A GetServiceEndpoints request.</param>
        public Task<HTTPResponse<GetServiceEndpointsResponse>>

            GetServiceEndpoints(GetServiceEndpointsRequest Request)

                => EMPClient.GetServiceEndpoints(Request);

        #endregion


        #region Start()

        public void Start()
        {
            EMPServer.Start();
        }

        #endregion

        #region Shutdown(Message = null, Wait = true)

        public void Shutdown(String Message = null, Boolean Wait = true)
        {
            EMPServer.Shutdown(Message, Wait);
        }

        #endregion

        public void Dispose()
        { }

    }

}
