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

        public IPPort RemotePort
            => EMPClient?.RemotePort;

        public RemoteCertificateValidationCallback RemoteCertificateValidator
            => EMPClient?.RemoteCertificateValidator;

        /// <summary>
        /// The EMP server part.
        /// </summary>
        public EMPServer        EMPServer           { get; }

        /// <summary>
        /// The EMP server logger.
        /// </summary>
        public EMPServerLogger  EMPServerLogger     { get; }

        /// <summary>
        /// The default request timeout for this client.
        /// </summary>
        public TimeSpan?        RequestTimeout      { get; }


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


        // Generic HTTP/SOAP server logging

        #region RequestLog

        /// <summary>
        /// An event called whenever a request came in.
        /// </summary>
        public event RequestLogHandler RequestLog
        {

            add
            {
                EMPServer.RequestLog += value;
            }

            remove
            {
                EMPServer.RequestLog -= value;
            }

        }

        #endregion

        #region AccessLog

        /// <summary>
        /// An event called whenever a request could successfully be processed.
        /// </summary>
        public event AccessLogHandler AccessLog
        {

            add
            {
                EMPServer.AccessLog += value;
            }

            remove
            {
                EMPServer.AccessLog -= value;
            }

        }

        #endregion

        #region ErrorLog

        /// <summary>
        /// An event called whenever a request resulted in an error.
        /// </summary>
        public event ErrorLogHandler ErrorLog
        {

            add
            {
                EMPServer.ErrorLog += value;
            }

            remove
            {
                EMPServer.ErrorLog -= value;
            }

        }

        #endregion

        #endregion

        #region Custom request mappers


        #endregion

        #region Constructor(s)

        #region EMPRoaming(EMPClient, EMPServer, ServerLoggingContext = EMPServerLogger.DefaultContext, LogfileCreator = null)

        /// <summary>
        /// Create a new OICP roaming client for EMPs.
        /// </summary>
        /// <param name="EMPClient">A EMP client.</param>
        /// <param name="EMPServer">A EMP sever.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public EMPRoaming(EMPClient               EMPClient,
                          EMPServer               EMPServer,
                          String                  ServerLoggingContext  = EMPServerLogger.DefaultContext,
                          LogfileCreatorDelegate  LogfileCreator        = null)
        {

            this.EMPClient        = EMPClient;
            this.EMPServer        = EMPServer;
            this.EMPServerLogger  = new EMPServerLogger(EMPServer,
                                                        ServerLoggingContext,
                                                        LogfileCreator);

        }

        #endregion

        #region EMPRoaming(ClientId, RemoteHostname, RemoteTCPPort = null, RemoteHTTPVirtualHost = null, ... )

        /// <summary>
        /// Create a new OICP roaming client for EMPs.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="RemoteHostname">The hostname of the remote OICP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote OICP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OICP service.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerContentType">An optional HTTP content type to use.</param>
        /// <param name="ServerRegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public EMPRoaming(String                               ClientId,
                          String                               RemoteHostname,
                          IPPort                               RemoteTCPPort                   = null,
                          RemoteCertificateValidationCallback  RemoteCertificateValidator      = null,
                          LocalCertificateSelectionCallback    LocalCertificateSelector        = null,
                          X509Certificate                      ClientCert                      = null,
                          String                               RemoteHTTPVirtualHost           = null,
                          String                               URIPrefix                       = EMPClient.DefaultURIPrefix,
                          Tuple<String, String>                WSSLoginPassword                = null,
                          String                               HTTPUserAgent                   = EMPClient.DefaultHTTPUserAgent,
                          TimeSpan?                            RequestTimeout                  = null,
                          Byte?                                MaxNumberOfRetries              = EMPClient.DefaultMaxNumberOfRetries,

                          String                               ServerName                      = EMPServer.DefaultHTTPServerName,
                          String                               ServiceId                       = null,
                          IPPort                               ServerTCPPort                   = null,
                          String                               ServerURIPrefix                 = EMPServer.DefaultURIPrefix,
                          HTTPContentType                      ServerContentType               = null,
                          Boolean                              ServerRegisterHTTPRootService   = true,
                          Boolean                              ServerAutoStart                 = false,

                          String                               ClientLoggingContext            = EMPClient.EMPClientLogger.DefaultContext,
                          String                               ServerLoggingContext            = EMPServerLogger.DefaultContext,
                          LogfileCreatorDelegate               LogfileCreator                  = null,

                          DNSClient                            DNSClient                       = null)

            : this(new EMPClient(ClientId,
                                 RemoteHostname,
                                 RemoteTCPPort,
                                 RemoteCertificateValidator,
                                 LocalCertificateSelector,
                                 ClientCert,
                                 RemoteHTTPVirtualHost,
                                 URIPrefix,
                                 WSSLoginPassword,
                                 HTTPUserAgent,
                                 RequestTimeout,
                                 MaxNumberOfRetries,
                                 DNSClient,
                                 ClientLoggingContext,
                                 LogfileCreator),

                   new EMPServer(ServerName,
                                 ServiceId,
                                 ServerTCPPort,
                                 ServerURIPrefix,
                                 ServerContentType,
                                 ServerRegisterHTTPRootService,
                                 DNSClient,
                                 false),

                   ServerLoggingContext,
                   LogfileCreator)

        {

            if (ServerAutoStart)
                Start();

        }

        #endregion

        #endregion


        #region SetRoamingAuthorisationList      (Request)

        /// <summary>
        /// Create a new task querying EVSE data from the OICP server.
        /// The request might either have none, 'SearchCenter + DistanceKM' or 'LastCall' parameters.
        /// Because of limitations at Hubject the SearchCenter and LastCall parameters can not be used at the same time!
        /// </summary>
        /// <param name="Request">An PullEVSEData request.</param>
        public Task<HTTPResponse<SetRoamingAuthorisationListResponse>>

            SetRoamingAuthorisationList(SetRoamingAuthorisationListRequest Request)

                => EMPClient.SetRoamingAuthorisationList(Request);

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
