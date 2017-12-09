/*
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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using System.Threading.Tasks;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// The common interface of all OCHP EMP clients.
    /// </summary>
    public interface IEMPClient : IHTTPClient
    {

        #region Events

        // OCHP

        #region OnGetChargePointListRequest/-Response

        /// <summary>
        /// An event fired whenever a request to download the charge points list will be send.
        /// </summary>
        event OnGetChargePointListRequestDelegate   OnGetChargePointListRequest;

        /// <summary>
        /// An event fired whenever a SOAP request to download the charge points list will be send.
        /// </summary>
        event ClientRequestLogHandler               OnGetChargePointListSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a charge points list download SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler              OnGetChargePointListSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a charge points list download request had been received.
        /// </summary>
        event OnGetChargePointListResponseDelegate  OnGetChargePointListResponse;

        #endregion

        #region OnGetChargePointListUpdatesRequest/-Response

        /// <summary>
        /// An event fired whenever a request to download a charge points list update will be send.
        /// </summary>
        event OnGetChargePointListUpdatesRequestDelegate   OnGetChargePointListUpdatesRequest;

        /// <summary>
        /// An event fired whenever a SOAP request to download a charge points list update will be send.
        /// </summary>
        event ClientRequestLogHandler                      OnGetChargePointListUpdatesSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a charge points list update download SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                     OnGetChargePointListUpdatesSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a charge points list update download request had been received.
        /// </summary>
        event OnGetChargePointListUpdatesResponseDelegate  OnGetChargePointListUpdatesResponse;

        #endregion

        #region OnGetStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request for evse or parking status will be send.
        /// </summary>
        event OnGetStatusRequestDelegate   OnGetStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for evse or parking status will be send.
        /// </summary>
        event ClientRequestLogHandler      OnGetStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a evse or parking status SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler     OnGetStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for a evse or parking status request had been received.
        /// </summary>
        event OnGetStatusResponseDelegate  OnGetStatusResponse;

        #endregion


        #region OnSetRoamingAuthorisationListRequest/-Response

        /// <summary>
        /// An event fired whenever a roaming authorisation list will be send.
        /// </summary>
        event OnSetRoamingAuthorisationListRequestDelegate   OnSetRoamingAuthorisationListRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for uploading a roaming authorisation list will be send.
        /// </summary>
        event ClientRequestLogHandler                        OnSetRoamingAuthorisationListSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for uploading a roaming authorisation list had been received.
        /// </summary>
        event ClientResponseLogHandler                       OnSetRoamingAuthorisationListSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for uploading a roaming authorisation list had been received.
        /// </summary>
        event OnSetRoamingAuthorisationListResponseDelegate  OnSetRoamingAuthorisationListResponse;

        #endregion

        #region OnUpdateRoamingAuthorisationListRequest/-Response

        /// <summary>
        /// An event fired whenever a roaming authorisation list update will be send.
        /// </summary>
        event OnUpdateRoamingAuthorisationListRequestDelegate   OnUpdateRoamingAuthorisationListRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for uploading a roaming authorisation list update will be send.
        /// </summary>
        event ClientRequestLogHandler                           OnUpdateRoamingAuthorisationListSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for uploading a roaming authorisation list update had been received.
        /// </summary>
        event ClientResponseLogHandler                          OnUpdateRoamingAuthorisationListSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for uploading a roaming authorisation list update had been received.
        /// </summary>
        event OnUpdateRoamingAuthorisationListResponseDelegate  OnUpdateRoamingAuthorisationListResponse;

        #endregion


        #region OnGetCDRsRequest/-Response

        /// <summary>
        /// An event fired whenever a request for charge detail records will be send.
        /// </summary>
        event OnGetCDRsRequestDelegate   OnGetCDRsRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for charge detail records will be send.
        /// </summary>
        event ClientRequestLogHandler    OnGetCDRsSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a charge detail records SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler   OnGetCDRsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for a charge detail records request had been received.
        /// </summary>
        event OnGetCDRsResponseDelegate  OnGetCDRsResponse;

        #endregion

        #region OnConfirmCDRsRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record confirmation request will be send.
        /// </summary>
        event OnConfirmCDRsRequestDelegate   OnConfirmCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record confirmation SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler        OnConfirmCDRsSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a charge detail record confirmation SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler       OnConfirmCDRsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for a charge detail record confirmation request had been received.
        /// </summary>
        event OnConfirmCDRsResponseDelegate  OnConfirmCDRsResponse;

        #endregion


        #region OnGetTariffUpdatesRequest/-Response

        /// <summary>
        /// An event fired whenever a request for tariff infos will be send.
        /// </summary>
        event OnGetTariffUpdatesRequestDelegate   OnGetTariffUpdatesRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for tariff infos will be send.
        /// </summary>
        event ClientRequestLogHandler             OnGetTariffUpdatesSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a tariff infos SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler            OnGetTariffUpdatesSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for a tariff infos request had been received.
        /// </summary>
        event OnGetTariffUpdatesResponseDelegate  OnGetTariffUpdatesResponse;

        #endregion


        // OCHPdirect

        #region OnAddServiceEndpointsRequest/-Response

        /// <summary>
        /// An event fired whenever a request to add service endpoints will be send.
        /// </summary>
        event OnAddServiceEndpointsRequestDelegate   OnAddServiceEndpointsRequest;

        /// <summary>
        /// An event fired whenever a SOAP request to add service endpoints will be send.
        /// </summary>
        event ClientRequestLogHandler                OnAddServiceEndpointsSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to add service endpoints had been received.
        /// </summary>
        event ClientResponseLogHandler               OnAddServiceEndpointsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for request to add service endpoints had been received.
        /// </summary>
        event OnAddServiceEndpointsResponseDelegate  OnAddServiceEndpointsResponse;

        #endregion

        #region OnGetServiceEndpointsRequest/-Response

        /// <summary>
        /// An event fired whenever a request to get service endpoints will be send.
        /// </summary>
        event OnGetServiceEndpointsRequestDelegate   OnGetServiceEndpointsRequest;

        /// <summary>
        /// An event fired whenever a SOAP request to get service endpoints will be send.
        /// </summary>
        event ClientRequestLogHandler                OnGetServiceEndpointsSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to get service endpoints had been received.
        /// </summary>
        event ClientResponseLogHandler               OnGetServiceEndpointsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for request to get service endpoints had been received.
        /// </summary>
        event OnGetServiceEndpointsResponseDelegate  OnGetServiceEndpointsResponse;

        #endregion


        #region OnSelectEVSERequest/-Response

        /// <summary>
        /// An event fired whenever a request to select an EVSE will be send.
        /// </summary>
        event OnSelectEVSERequestDelegate   OnSelectEVSERequest;

        /// <summary>
        /// An event fired whenever a SOAP request to select an EVSE will be send.
        /// </summary>
        event ClientRequestLogHandler       OnSelectEVSESOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to select an EVSE had been received.
        /// </summary>
        event ClientResponseLogHandler      OnSelectEVSESOAPResponse;

        /// <summary>
        /// An event fired whenever a response for request to select an EVSE had been received.
        /// </summary>
        event OnSelectEVSEResponseDelegate  OnSelectEVSEResponse;

        #endregion

        #region OnControlEVSERequest/-Response

        /// <summary>
        /// An event fired whenever a request to control a direct charging process will be send.
        /// </summary>
        event OnControlEVSERequestDelegate   OnControlEVSERequest;

        /// <summary>
        /// An event fired whenever a SOAP request to control a direct charging process will be send.
        /// </summary>
        event ClientRequestLogHandler        OnControlEVSESOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to control a direct charging process had been received.
        /// </summary>
        event ClientResponseLogHandler       OnControlEVSESOAPResponse;

        /// <summary>
        /// An event fired whenever a response for request to control a direct charging process had been received.
        /// </summary>
        event OnControlEVSEResponseDelegate  OnControlEVSEResponse;

        #endregion

        #region OnReleaseEVSERequest/-Response

        /// <summary>
        /// An event fired whenever a request to release an EVSE will be send.
        /// </summary>
        event OnReleaseEVSERequestDelegate   OnReleaseEVSERequest;

        /// <summary>
        /// An event fired whenever a SOAP request to release an EVSE will be send.
        /// </summary>
        event ClientRequestLogHandler        OnReleaseEVSESOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to release an EVSE had been received.
        /// </summary>
        event ClientResponseLogHandler       OnReleaseEVSESOAPResponse;

        /// <summary>
        /// An event fired whenever a response for request to release an EVSE had been received.
        /// </summary>
        event OnReleaseEVSEResponseDelegate  OnReleaseEVSEResponse;

        #endregion

        #region OnGetEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request for EVSE status will be send.
        /// </summary>
        event OnGetEVSEStatusRequestDelegate   OnGetEVSEStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for EVSE status will be send.
        /// </summary>
        event ClientRequestLogHandler          OnGetEVSEStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request for EVSE status had been received.
        /// </summary>
        event ClientResponseLogHandler         OnGetEVSEStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for an EVSE status request had been received.
        /// </summary>
        event OnGetEVSEStatusResponseDelegate  OnGetEVSEStatusResponse;

        #endregion


        #region OnReportDiscrepancyRequest/-Response

        /// <summary>
        /// An event fired whenever a report discrepancy request will be send.
        /// </summary>
        event OnReportDiscrepancyRequestDelegate   OnReportDiscrepancyRequest;

        /// <summary>
        /// An event fired whenever a report discrepancy SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler              OnReportDiscrepancySOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a report discrepancy SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler             OnReportDiscrepancySOAPResponse;

        /// <summary>
        /// An event fired whenever a response for a report discrepancy request had been received.
        /// </summary>
        event OnReportDiscrepancyResponseDelegate  OnReportDiscrepancyResponse;

        #endregion

        #region OnGetInformProviderRequest/-Response

        /// <summary>
        /// An event fired whenever a get inform provider request will be send.
        /// </summary>
        event OnGetInformProviderRequestDelegate   OnGetInformProviderRequest;

        /// <summary>
        /// An event fired whenever a get inform provider SOAP request will be send.
        /// </summary>
        event ClientRequestLogHandler              OnGetInformProviderSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a get inform provider SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler             OnGetInformProviderSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for a get inform provider request had been received.
        /// </summary>
        event OnGetInformProviderResponseDelegate  OnGetInformProviderResponse;

        #endregion

        #endregion


        Task<HTTPResponse<SetRoamingAuthorisationListResponse>>
            SetRoamingAuthorisationList(SetRoamingAuthorisationListRequest Request);

    }

}
