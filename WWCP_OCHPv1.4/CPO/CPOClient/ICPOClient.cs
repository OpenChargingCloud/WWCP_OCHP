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

using System;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    /// <summary>
    /// The common interface of all OCHP CPO Clients.
    /// </summary>
    public interface ICPOClient : IHTTPClient
    {

        #region Properties

        /// <summary>
        /// The default request timeout for this client.
        /// </summary>
        TimeSpan? RequestTimeout { get; }

        #endregion

        #region Events

        // OCHP

        #region OnSetChargePointListRequest/-Response

        /// <summary>
        /// An event fired whenever a request uploading charge points will be send.
        /// </summary>
        event OnSetChargePointListRequestDelegate   OnSetChargePointListRequest;

        /// <summary>
        /// An event fired whenever a SOAP request uploading charge points will be send.
        /// </summary>
        event ClientRequestLogHandler               OnSetChargePointListSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a upload charge points SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler              OnSetChargePointListSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a upload charge points request had been received.
        /// </summary>
        event OnSetChargePointListResponseDelegate  OnSetChargePointListResponse;

        #endregion

        #region OnUpdateChargePointListRequest/-Response

        /// <summary>
        /// An event fired whenever a request uploading charge point updates will be send.
        /// </summary>
        event OnUpdateChargePointListRequestDelegate   OnUpdateChargePointListRequest;

        /// <summary>
        /// An event fired whenever a SOAP request uploading charge point updates will be send.
        /// </summary>
        event ClientRequestLogHandler                  OnUpdateChargePointListSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a upload charge point updates SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                 OnUpdateChargePointListSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a upload charge point updates request had been received.
        /// </summary>
        event OnUpdateChargePointListResponseDelegate  OnUpdateChargePointListResponse;

        #endregion

        #region OnUpdateStatusRequest/-Response

        /// <summary>
        /// An event fired whenever evse and parking status will be send.
        /// </summary>
        event OnUpdateStatusRequestDelegate   OnUpdateStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for evse and parking status will be send.
        /// </summary>
        event ClientRequestLogHandler         OnUpdateStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an evse and parking status SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler        OnUpdateStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an evse and parking status request had been received.
        /// </summary>
        event OnUpdateStatusResponseDelegate  OnUpdateStatusResponse;

        #endregion


        #region OnGetSingleRoamingAuthorisationRequest/-Response

        /// <summary>
        /// An event fired whenever a request authenticating an e-mobility token will be send.
        /// </summary>
        event OnGetSingleRoamingAuthorisationRequestDelegate   OnGetSingleRoamingAuthorisationRequest;

        /// <summary>
        /// An event fired whenever a SOAP request authenticating an e-mobility token will be send.
        /// </summary>
        event ClientRequestLogHandler                          OnGetSingleRoamingAuthorisationSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a authenticate an e-mobility token SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                         OnGetSingleRoamingAuthorisationSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a authenticate an e-mobility token request had been received.
        /// </summary>
        event OnGetSingleRoamingAuthorisationResponseDelegate  OnGetSingleRoamingAuthorisationResponse;

        #endregion

        #region OnGetRoamingAuthorisationListRequest/-Response

        /// <summary>
        /// An event fired whenever a request for the current roaming authorisation list will be send.
        /// </summary>
        event OnGetRoamingAuthorisationListRequestDelegate   OnGetRoamingAuthorisationListRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for the current roaming authorisation list will be send.
        /// </summary>
        event ClientRequestLogHandler                        OnGetRoamingAuthorisationListSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a SOAP request for the current roaming authorisation list had been received.
        /// </summary>
        event ClientResponseLogHandler                       OnGetRoamingAuthorisationListSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a request for the current roaming authorisation list had been received.
        /// </summary>
        event OnGetRoamingAuthorisationListResponseDelegate  OnGetRoamingAuthorisationListResponse;

        #endregion

        #region OnGetRoamingAuthorisationListUpdatesRequest/-Response

        /// <summary>
        /// An event fired whenever a request for updates of a roaming authorisation list will be send.
        /// </summary>
        event OnGetRoamingAuthorisationListUpdatesRequestDelegate   OnGetRoamingAuthorisationListUpdatesRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for updates of a roaming authorisation list will be send.
        /// </summary>
        event ClientRequestLogHandler                               OnGetRoamingAuthorisationListUpdatesSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a roaming authorisation list update SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler                              OnGetRoamingAuthorisationListUpdatesSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a roaming authorisation list update request had been received.
        /// </summary>
        event OnGetRoamingAuthorisationListUpdatesResponseDelegate  OnGetRoamingAuthorisationListUpdatesResponse;

        #endregion


        #region OnAddCDRsRequest/-Response

        /// <summary>
        /// An event fired whenever a request for adding charge detail records will be send.
        /// </summary>
        event OnAddCDRsRequestDelegate   OnAddCDRsRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for adding charge detail records will be send.
        /// </summary>
        event ClientRequestLogHandler    OnAddCDRsSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to an add charge detail records SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler   OnAddCDRsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to an add charge detail records request had been received.
        /// </summary>
        event OnAddCDRsResponseDelegate  OnAddCDRsResponse;

        #endregion

        #region OnCheckCDRsRequest/-Response

        /// <summary>
        /// An event fired whenever a request for checking charge detail records will be send.
        /// </summary>
        event OnCheckCDRsRequestDelegate   OnCheckCDRsRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for checking charge detail records will be send.
        /// </summary>
        event ClientRequestLogHandler      OnCheckCDRsSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to a check charge detail records SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler     OnCheckCDRsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to a check charge detail records request had been received.
        /// </summary>
        event OnCheckCDRsResponseDelegate  OnCheckCDRsResponse;

        #endregion


        #region OnUpdateTariffsRequest/-Response

        /// <summary>
        /// An event fired whenever tariff updates will be send.
        /// </summary>
        event OnUpdateTariffsRequestDelegate   OnUpdateTariffsRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for updating tariffs will be send.
        /// </summary>
        event ClientRequestLogHandler          OnUpdateTariffsSOAPRequest;

        /// <summary>
        /// An event fired whenever a response to update tariffs SOAP request had been received.
        /// </summary>
        event ClientResponseLogHandler         OnUpdateTariffsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response to update tariffs request had been received.
        /// </summary>
        event OnUpdateTariffsResponseDelegate  OnUpdateTariffsResponse;

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


        #region OnInformProviderRequest/-Response

        /// <summary>
        /// An event fired whenever an inform provider message will be send.
        /// </summary>
        event OnInformProviderRequestDelegate   OnInformProviderRequest;

        /// <summary>
        /// An event fired whenever an inform provider SOAP message will be send.
        /// </summary>
        event ClientRequestLogHandler           OnInformProviderSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for an inform provider SOAP message had been received.
        /// </summary>
        event ClientResponseLogHandler          OnInformProviderSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for an inform provider message had been received.
        /// </summary>
        event OnInformProviderResponseDelegate  OnInformProviderResponse;

        #endregion

        #endregion

        Task<HTTPResponse<SetChargePointListResponse>>

            SetChargePointList(SetChargePointListRequest Request);


        Task<HTTPResponse<UpdateChargePointListResponse>>

            UpdateChargePointList(UpdateChargePointListRequest Request);


        Task<HTTPResponse<UpdateStatusResponse>>

            UpdateStatus(UpdateStatusRequest Request);


        Task<HTTPResponse<GetSingleRoamingAuthorisationResponse>>

            GetSingleRoamingAuthorisation(GetSingleRoamingAuthorisationRequest Request);


        Task<HTTPResponse<AddCDRsResponse>>

            AddCDRs(AddCDRsRequest Request);

    }

}
