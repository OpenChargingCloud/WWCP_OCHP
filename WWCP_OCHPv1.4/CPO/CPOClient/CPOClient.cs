/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using System.Xml.Linq;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP.v1_1;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.CPO
{

    /// <summary>
    /// The OCHP CPO Client.
    /// </summary>
    public partial class CPOClient : ASOAPClient,
                                     ICPOClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const           String    DefaultHTTPUserAgent  = "GraphDefined OCHP " + Version.Number + " CPO Client";

        /// <summary>
        /// The default remote TCP port to connect to.
        /// </summary>
        public new static readonly IPPort    DefaultRemotePort     = IPPort.Parse(443);

        /// <summary>
        /// The default URI prefix.
        /// </summary>
        public static readonly     HTTPPath  DefaultURLPrefix      = HTTPPath.Parse("/service/ochp/v1.4/");

        /// <summary>
        /// The default Live URI prefix.
        /// </summary>
        public static readonly     HTTPPath  DefaultLiveURLPathPrefix  = HTTPPath.Parse("/live/ochp/v1.4");

        #endregion

        #region Properties

        /// <summary>
        /// The default Live URL path prefix.
        /// </summary>
        public HTTPPath  LiveURLPathPrefix    { get; }


        /// <summary>
        /// The attached HTTP client logger.
        /// </summary>
        public new Logger HTTPLogger
        {
            get
            {
                return base.HTTPLogger as Logger;
            }
            set
            {
                base.HTTPLogger = value;
            }
        }

        #endregion

        #region Custom request mappers

        // OCHP

        #region CustomSetChargePointListRequestMapper

        #region CustomSetChargePointListRequestMapper

        private Func<SetChargePointListRequest, SetChargePointListRequest> _CustomSetChargePointListRequestMapper = _ => _;

        public Func<SetChargePointListRequest, SetChargePointListRequest> CustomSetChargePointListRequestMapper
        {

            get
            {
                return _CustomSetChargePointListRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetChargePointListRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetChargePointListSOAPRequestMapper

        private Func<SetChargePointListRequest, XElement, XElement> _CustomSetChargePointListSOAPRequestMapper = (request, xml) => xml;

        public Func<SetChargePointListRequest, XElement, XElement> CustomSetChargePointListSOAPRequestMapper
        {

            get
            {
                return _CustomSetChargePointListSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetChargePointListSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetChargePointListRequest> CustomSetChargePointListParser   { get; set; }

        #endregion

        #region CustomUpdateChargePointListRequestMapper

        #region CustomUpdateChargePointListRequestMapper

        private Func<UpdateChargePointListRequest, UpdateChargePointListRequest> _CustomUpdateChargePointListRequestMapper = _ => _;

        public Func<UpdateChargePointListRequest, UpdateChargePointListRequest> CustomUpdateChargePointListRequestMapper
        {

            get
            {
                return _CustomUpdateChargePointListRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomUpdateChargePointListRequestMapper = value;
            }

        }

        #endregion

        #region CustomUpdateChargePointListSOAPRequestMapper

        private Func<UpdateChargePointListRequest, XElement, XElement> _CustomUpdateChargePointListSOAPRequestMapper = (request, xml) => xml;

        public Func<UpdateChargePointListRequest, XElement, XElement> CustomUpdateChargePointListSOAPRequestMapper
        {

            get
            {
                return _CustomUpdateChargePointListSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomUpdateChargePointListSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<UpdateChargePointListRequest> CustomUpdateChargePointListParser   { get; set; }

        #endregion

        #region CustomUpdateStatusRequestMapper

        #region CustomUpdateStatusRequestMapper

        private Func<UpdateStatusRequest, UpdateStatusRequest> _CustomUpdateStatusRequestMapper = _ => _;

        public Func<UpdateStatusRequest, UpdateStatusRequest> CustomUpdateStatusRequestMapper
        {

            get
            {
                return _CustomUpdateStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomUpdateStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomUpdateStatusSOAPRequestMapper

        private Func<UpdateStatusRequest, XElement, XElement> _CustomUpdateStatusSOAPRequestMapper = (request, xml) => xml;

        public Func<UpdateStatusRequest, XElement, XElement> CustomUpdateStatusSOAPRequestMapper
        {

            get
            {
                return _CustomUpdateStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomUpdateStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<UpdateStatusRequest> CustomUpdateStatusParser { get; set; }

        #endregion

        #region CustomUpdateTariffsRequestMapper

        #region CustomUpdateTariffsRequestMapper

        private Func<UpdateTariffsRequest, UpdateTariffsRequest> _CustomUpdateTariffsRequestMapper = _ => _;

        public Func<UpdateTariffsRequest, UpdateTariffsRequest> CustomUpdateTariffsRequestMapper
        {

            get
            {
                return _CustomUpdateTariffsRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomUpdateTariffsRequestMapper = value;
            }

        }

        #endregion

        #region CustomUpdateTariffsSOAPRequestMapper

        private Func<UpdateTariffsRequest, XElement, XElement> _CustomUpdateTariffsSOAPRequestMapper = (request, xml) => xml;

        public Func<UpdateTariffsRequest, XElement, XElement> CustomUpdateTariffsSOAPRequestMapper
        {

            get
            {
                return _CustomUpdateTariffsSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomUpdateTariffsSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<UpdateTariffsRequest> CustomUpdateTariffsParser { get; set; }

        #endregion


        #region CustomGetSingleRoamingAuthorisationMapper

        #region CustomGetSingleRoamingAuthorisationRequestMapper

        private Func<GetSingleRoamingAuthorisationRequest, GetSingleRoamingAuthorisationRequest> _CustomGetSingleRoamingAuthorisationRequestMapper = _ => _;

        public Func<GetSingleRoamingAuthorisationRequest, GetSingleRoamingAuthorisationRequest> CustomGetSingleRoamingAuthorisationRequestMapper
        {

            get
            {
                return _CustomGetSingleRoamingAuthorisationRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetSingleRoamingAuthorisationRequestMapper = value;
            }

        }

        #endregion

        #region CustomGetSingleRoamingAuthorisationSOAPRequestMapper

        private Func<GetSingleRoamingAuthorisationRequest, XElement, XElement> _CustomGetSingleRoamingAuthorisationSOAPRequestMapper = (request, xml) => xml;

        public Func<GetSingleRoamingAuthorisationRequest, XElement, XElement> CustomGetSingleRoamingAuthorisationSOAPRequestMapper
        {

            get
            {
                return _CustomGetSingleRoamingAuthorisationSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetSingleRoamingAuthorisationSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<GetSingleRoamingAuthorisationResponse, GetSingleRoamingAuthorisationResponse.Builder> CustomGetSingleRoamingAuthorisationResponseMapper { get; set; }

        #endregion

        #region CustomGetRoamingAuthorisationListMapper

        #region CustomGetRoamingAuthorisationListRequestMapper

        private Func<GetRoamingAuthorisationListRequest, GetRoamingAuthorisationListRequest> _CustomGetRoamingAuthorisationListRequestMapper = _ => _;

        public Func<GetRoamingAuthorisationListRequest, GetRoamingAuthorisationListRequest> CustomGetRoamingAuthorisationListRequestMapper
        {

            get
            {
                return _CustomGetRoamingAuthorisationListRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetRoamingAuthorisationListRequestMapper = value;
            }

        }

        #endregion

        #region CustomGetRoamingAuthorisationListSOAPRequestMapper

        private Func<GetRoamingAuthorisationListRequest, XElement, XElement> _CustomGetRoamingAuthorisationListSOAPRequestMapper = (request, xml) => xml;

        public Func<GetRoamingAuthorisationListRequest, XElement, XElement> CustomGetRoamingAuthorisationListSOAPRequestMapper
        {

            get
            {
                return _CustomGetRoamingAuthorisationListSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetRoamingAuthorisationListSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<GetRoamingAuthorisationListResponse, GetRoamingAuthorisationListResponse.Builder> CustomGetRoamingAuthorisationListResponseMapper { get; set; }

        #endregion

        #region CustomGetRoamingAuthorisationListUpdatesMapper

        #region CustomGetRoamingAuthorisationListUpdatesRequestMapper

        private Func<GetRoamingAuthorisationListUpdatesRequest, GetRoamingAuthorisationListUpdatesRequest> _CustomGetRoamingAuthorisationListUpdatesRequestMapper = _ => _;

        public Func<GetRoamingAuthorisationListUpdatesRequest, GetRoamingAuthorisationListUpdatesRequest> CustomGetRoamingAuthorisationListUpdatesRequestMapper
        {

            get
            {
                return _CustomGetRoamingAuthorisationListUpdatesRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetRoamingAuthorisationListUpdatesRequestMapper = value;
            }

        }

        #endregion

        #region CustomGetRoamingAuthorisationListUpdatesSOAPRequestMapper

        private Func<GetRoamingAuthorisationListUpdatesRequest, XElement, XElement> _CustomGetRoamingAuthorisationListUpdatesSOAPRequestMapper = (request, xml) => xml;

        public Func<GetRoamingAuthorisationListUpdatesRequest, XElement, XElement> CustomGetRoamingAuthorisationListUpdatesSOAPRequestMapper
        {

            get
            {
                return _CustomGetRoamingAuthorisationListUpdatesSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetRoamingAuthorisationListUpdatesSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<GetRoamingAuthorisationListUpdatesResponse, GetRoamingAuthorisationListUpdatesResponse.Builder> CustomGetRoamingAuthorisationListUpdatesResponseMapper { get; set; }

        #endregion


        #region CustomAddCDRsMapper

        #region CustomAddCDRsRequestMapper

        private Func<AddCDRsRequest, AddCDRsRequest> _CustomAddCDRsRequestMapper = _ => _;

        public Func<AddCDRsRequest, AddCDRsRequest> CustomAddCDRsRequestMapper
        {

            get
            {
                return _CustomAddCDRsRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAddCDRsRequestMapper = value;
            }

        }

        #endregion

        #region CustomAddCDRsSOAPRequestMapper

        private Func<AddCDRsRequest, XElement, XElement> _CustomAddCDRsSOAPRequestMapper = (request, xml) => xml;

        public Func<AddCDRsRequest, XElement, XElement> CustomAddCDRsSOAPRequestMapper
        {

            get
            {
                return _CustomAddCDRsSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAddCDRsSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<AddCDRsResponse, AddCDRsResponse.Builder> CustomAddCDRsResponseMapper { get; set; }

        #endregion

        #region CustomCheckCDRsMapper

        #region CustomCheckCDRsRequestMapper

        private Func<CheckCDRsRequest, CheckCDRsRequest> _CustomCheckCDRsRequestMapper = _ => _;

        public Func<CheckCDRsRequest, CheckCDRsRequest> CustomCheckCDRsRequestMapper
        {

            get
            {
                return _CustomCheckCDRsRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomCheckCDRsRequestMapper = value;
            }

        }

        #endregion

        #region CustomCheckCDRsSOAPRequestMapper

        private Func<CheckCDRsRequest, XElement, XElement> _CustomCheckCDRsSOAPRequestMapper = (request, xml) => xml;

        public Func<CheckCDRsRequest, XElement, XElement> CustomCheckCDRsSOAPRequestMapper
        {

            get
            {
                return _CustomCheckCDRsSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomCheckCDRsSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomMapperDelegate<CheckCDRsResponse, CheckCDRsResponse.Builder> CustomCheckCDRsResponseMapper { get; set; }

        #endregion


        // OCHP direct

        #region CustomAddServiceEndpoints(SOAP)RequestMapper

        #region CustomAddServiceEndpointsRequestMapper

        private Func<AddServiceEndpointsRequest, AddServiceEndpointsRequest> _CustomAddServiceEndpointsRequestMapper = _ => _;

        public Func<AddServiceEndpointsRequest, AddServiceEndpointsRequest> CustomAddServiceEndpointsRequestMapper
        {

            get
            {
                return _CustomAddServiceEndpointsRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAddServiceEndpointsRequestMapper = value;
            }

        }

        #endregion

        #region CustomAddServiceEndpointsSOAPRequestMapper

        private Func<AddServiceEndpointsRequest, XElement, XElement> _CustomAddServiceEndpointsSOAPRequestMapper = (request, xml) => xml;

        public Func<AddServiceEndpointsRequest, XElement, XElement> CustomAddServiceEndpointsSOAPRequestMapper
        {

            get
            {
                return _CustomAddServiceEndpointsSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomAddServiceEndpointsSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<AddServiceEndpointsResponse> CustomAddServiceEndpointsParser { get; set; }

        #endregion

        #region CustomGetServiceEndpoints(SOAP)RequestMapper

        #region CustomGetServiceEndpointsRequestMapper

        private Func<GetServiceEndpointsRequest, GetServiceEndpointsRequest> _CustomGetServiceEndpointsRequestMapper = _ => _;

        public Func<GetServiceEndpointsRequest, GetServiceEndpointsRequest> CustomGetServiceEndpointsRequestMapper
        {

            get
            {
                return _CustomGetServiceEndpointsRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetServiceEndpointsRequestMapper = value;
            }

        }

        #endregion

        #region CustomGetServiceEndpointsSOAPRequestMapper

        private Func<GetServiceEndpointsRequest, XElement, XElement> _CustomGetServiceEndpointsSOAPRequestMapper = (request, xml) => xml;

        public Func<GetServiceEndpointsRequest, XElement, XElement> CustomGetServiceEndpointsSOAPRequestMapper
        {

            get
            {
                return _CustomGetServiceEndpointsSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetServiceEndpointsSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<GetServiceEndpointsResponse> CustomGetServiceEndpointsParser { get; set; }

        #endregion

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

        /// <summary>
        /// Create a new OCHP CPO Client.
        /// </summary>
        /// <param name="RemoteURL">The remote URL of the OICP HTTP endpoint to connect to.</param>
        /// <param name="VirtualHostname">An optional HTTP virtual hostname.</param>
        /// <param name="Description">An optional description of this CPO client.</param>
        /// <param name="RemoteCertificateValidator">The remote TLS certificate validator.</param>
        /// <param name="LocalCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use of HTTP authentication.</param>
        /// <param name="HTTPUserAgent">The HTTP user agent identification.</param>
        /// <param name="URLPathPrefix">An optional default URL path prefix.</param>
        /// <param name="WSSLoginPassword">The WebService-Security username/password.</param>
        /// <param name="HTTPContentType">The HTTP content type to use.</param>
        /// <param name="RequestTimeout">An optional request timeout.</param>
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The maximum number of transmission retries for HTTP request.</param>
        /// <param name="DisableLogging">Disable all logging.</param>
        /// <param name="LoggingPath">The logging path.</param>
        /// <param name="LoggingContext">An optional context for logging client methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// <param name="DNSClient">The DNS client to use.</param>
        public CPOClient(URL                                                        RemoteURL,
                         HTTPHostname?                                              VirtualHostname              = null,
                         String?                                                    Description                  = null,
                         Boolean?                                                   PreferIPv4                   = null,
                         RemoteTLSServerCertificateValidationHandler<IHTTPClient>?  RemoteCertificateValidator   = null,
                         LocalCertificateSelectionHandler?                          LocalCertificateSelector    = null,
                         X509Certificate?                                           ClientCert                   = null,
                         SslProtocols?                                              TLSProtocol                  = null,
                         String                                                     HTTPUserAgent                = DefaultHTTPUserAgent,
                         IHTTPAuthentication?                                       HTTPAuthentication           = null,
                         HTTPPath?                                                  URLPathPrefix                = null,
                         HTTPPath?                                                  LiveURLPathPrefix            = null,
                         Tuple<String, String>?                                     WSSLoginPassword             = null,
                         HTTPContentType?                                           HTTPContentType              = null,
                         TimeSpan?                                                  RequestTimeout               = null,
                         TransmissionRetryDelayDelegate?                            TransmissionRetryDelay       = null,
                         UInt16?                                                    MaxNumberOfRetries           = null,
                         UInt32?                                                    InternalBufferSize           = null,
                         Boolean?                                                   DisableLogging               = false,
                         String?                                                    LoggingPath                  = null,
                         String                                                     LoggingContext               = Logger.DefaultContext,
                         LogfileCreatorDelegate?                                    LogfileCreator               = null,
                         DNSClient?                                                 DNSClient                    = null)

            : base(RemoteURL,
                   VirtualHostname,
                   Description,
                   PreferIPv4,
                   RemoteCertificateValidator,
                   LocalCertificateSelector,
                   ClientCert,
                   TLSProtocol,
                   HTTPUserAgent,
                   HTTPAuthentication,
                   URLPathPrefix ?? DefaultURLPathPrefix,
                   WSSLoginPassword,
                   HTTPContentType,
                   RequestTimeout,
                   TransmissionRetryDelay,
                   MaxNumberOfRetries,
                   InternalBufferSize,
                   false,
                   DisableLogging,
                   null,
                   DNSClient)

        {

            this.LiveURLPathPrefix  = LiveURLPathPrefix ?? DefaultLiveURLPathPrefix;

            base.HTTPLogger         = this.DisableLogging == false
                                          ? new Logger(
                                                this,
                                                LoggingPath,
                                                LoggingContext,
                                                LogfileCreator
                                            )
                                          : null;

        }

        #endregion


        // OCHP

        #region SetChargePointList   (Request)

        /// <summary>
        /// Upload the given enumeration of charge points.
        /// </summary>
        /// <param name="Request">A SetChargePointList request.</param>
        public async Task<HTTPResponse<SetChargePointListResponse>>

            SetChargePointList(SetChargePointListRequest  Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given SetChargePointList request must not be null!");

            Request = _CustomSetChargePointListRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped SetChargePointList request must not be null!");


            Byte                                     TransmissionRetry  = 0;
            HTTPResponse<SetChargePointListResponse> result             = null;

            #endregion

            #region Send OnSetChargePointListRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnSetChargePointListRequest != null)
                    await Task.WhenAll(OnSetChargePointListRequest.GetInvocationList().
                                       Cast<OnSetChargePointListRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.ChargePointInfos,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnSetChargePointListRequest));
            }

            #endregion


            #region No charge point infos to upload?

            if (!Request.ChargePointInfos.Any())
            {

                result = HTTPResponse<SetChargePointListResponse>.OK(
                             new SetChargePointListResponse(Request,
                                                            Result.NoOperation("No chargepoint infos to upload!"))
                         );

            }

            #endregion

            else do
            {

                using (var ochpClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                       VirtualHostname:             VirtualHostname,
                                                       RemoteCertificateValidator:  RemoteCertificateValidator,
                                                       LocalCertificateSelector:    LocalCertificateSelector,
                                                       HTTPUserAgent:               HTTPUserAgent,
                                                       RequestTimeout:              RequestTimeout,
                                                       DNSClient:                   DNSClient))
                {

                    result = await ochpClient.Query(_CustomSetChargePointListSOAPRequestMapper(Request,
                                                                                                SOAP.Encapsulation(
                                                                                                    WSSLoginPassword.Item1,
                                                                                                    WSSLoginPassword.Item2,
                                                                                                    Request.ToXML()
                                                                                                )),
                                                     "http://ochp.eu/1.4/SetChargepointList",
                                                     RequestLogDelegate:   OnSetChargePointListSOAPRequest,
                                                     ResponseLogDelegate:  OnSetChargePointListSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, SetChargePointListResponse.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return HTTPResponse<SetChargePointListResponse>.IsFault(
                                                                    httpresponse,
                                                                    new SetChargePointListResponse(
                                                                        Request,
                                                                        Result.Format(
                                                                            "Invalid SOAP => " +
                                                                            httpresponse.HTTPBody.ToUTF8String()
                                                                        )
                                                                    )
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);

                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                                                         {

                                                             return HTTPResponse<SetChargePointListResponse>.IsFault(
                                                                 httpresponse,
                                                                 new SetChargePointListResponse(
                                                                     Request,
                                                                     Result.Server(
                                                                          httpresponse.HTTPStatusCode +
                                                                          " => " +
                                                                          httpresponse.HTTPBody.ToUTF8String()
                                                                     )
                                                                 ));

                                                         }

                                                         return HTTPResponse<SetChargePointListResponse>.IsFault(
                                                             httpresponse,
                                                             new SetChargePointListResponse(
                                                                 Request,
                                                                 Result.Server(
                                                                      httpresponse.HTTPStatusCode +
                                                                      " => " +
                                                                      httpresponse.HTTPBody.ToUTF8String()
                                                                 )
                                                             )
                                                         );

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<SetChargePointListResponse>.ExceptionThrown(

                                                             new SetChargePointListResponse(
                                                                 Request,
                                                                 Result.Format(exception.Message +
                                                                               " => " +
                                                                               exception.StackTrace)
                                                             ),

                                                             Exception: exception

                                                         );

                                                     }

                                                     #endregion

                                                    );

                }

                if (result == null)
                    result = HTTPResponse<SetChargePointListResponse>.ClientError(
                                 new SetChargePointListResponse(Request,
                                                                Result.Client("HTTP request failed!"))
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnSetChargePointListResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnSetChargePointListResponse != null)
                    await Task.WhenAll(OnSetChargePointListResponse.GetInvocationList().
                                       Cast<OnSetChargePointListResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.ChargePointInfos,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnSetChargePointListResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region UpdateChargePointList(Request)

        /// <summary>
        /// Update the given enumeration of charge points.
        /// </summary>
        /// <param name="Request">A UpdateChargePointList request.</param>
        public async Task<HTTPResponse<UpdateChargePointListResponse>>

            UpdateChargePointList(UpdateChargePointListRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given UpdateChargePointList request must not be null!");

            Request = _CustomUpdateChargePointListRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped UpdateChargePointList request must not be null!");


            Byte                                        TransmissionRetry  = 0;
            HTTPResponse<UpdateChargePointListResponse> result             = null;

            #endregion

            #region Send OnUpdateChargePointListRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnUpdateChargePointListRequest != null)
                    await Task.WhenAll(OnUpdateChargePointListRequest.GetInvocationList().
                                       Cast<OnUpdateChargePointListRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.ChargePointInfos,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnUpdateChargePointListRequest));
            }

            #endregion


            #region No charge point infos to update?

            if (!Request.ChargePointInfos.Any())
            {

                result = HTTPResponse<UpdateChargePointListResponse>.OK(
                             new UpdateChargePointListResponse(Request,
                                                               Result.NoOperation("No chargepoints info to update!"))
                         );

            }

            #endregion

            else do
            {

                using (var ochpClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        LocalCertificateSelector:   LocalCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await ochpClient.Query(SOAP.Encapsulation(WSSLoginPassword.Item1, WSSLoginPassword.Item2, Request.ToXML()),
                                                     "http://ochp.eu/1.4/UpdateChargePointList",
                                                     RequestLogDelegate:   OnUpdateChargePointListSOAPRequest,
                                                     ResponseLogDelegate:  OnUpdateChargePointListSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, UpdateChargePointListResponse.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return HTTPResponse<UpdateChargePointListResponse>.IsFault(
                                                                    httpresponse,
                                                                    new UpdateChargePointListResponse(
                                                                        Request,
                                                                        Result.Format(
                                                                            "Invalid SOAP => " +
                                                                            httpresponse.HTTPBody.ToUTF8String()
                                                                        )
                                                                    )
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);

                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                                                         {

                                                             return HTTPResponse<UpdateChargePointListResponse>.IsFault(
                                                                 httpresponse,
                                                                 new UpdateChargePointListResponse(
                                                                     Request,
                                                                     Result.Server(
                                                                          httpresponse.HTTPStatusCode +
                                                                          " => " +
                                                                          httpresponse.HTTPBody.ToUTF8String()
                                                                     )
                                                                 )
                                                             );

                                                         }

                                                         return HTTPResponse<UpdateChargePointListResponse>.IsFault(
                                                             httpresponse,
                                                             new UpdateChargePointListResponse(
                                                                 Request,
                                                                 Result.Server(
                                                                      httpresponse.HTTPStatusCode +
                                                                      " => " +
                                                                      httpresponse.HTTPBody.ToUTF8String()
                                                                 )
                                                             )
                                                         );

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<UpdateChargePointListResponse>.ExceptionThrown(

                                                             new UpdateChargePointListResponse(
                                                                 Request,
                                                                 Result.Format(exception.Message +
                                                                               " => " +
                                                                               exception.StackTrace)
                                                             ),

                                                             Exception: exception

                                                         );

                                                     }

                                                     #endregion

                                                    );

                }

                if (result == null)
                    result = HTTPResponse<UpdateChargePointListResponse>.ClientError(
                                 new UpdateChargePointListResponse(Request,
                                                                   Result.Client("HTTP request failed!"))
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnUpdateChargePointListResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnUpdateChargePointListResponse != null)
                    await Task.WhenAll(OnUpdateChargePointListResponse.GetInvocationList().
                                       Cast<OnUpdateChargePointListResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.ChargePointInfos,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnUpdateChargePointListResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region UpdateStatus         (Request)

        /// <summary>
        /// Upload the given enumeration of EVSE and/or parking status.
        /// </summary>
        /// <param name="Request"></param>
        public async Task<HTTPResponse<UpdateStatusResponse>>

            UpdateStatus(UpdateStatusRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given UpdateStatus request must not be null!");

            Request = _CustomUpdateStatusRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped UpdateStatus request must not be null!");


            Byte                                        TransmissionRetry  = 0;
            HTTPResponse<UpdateStatusResponse> result             = null;

            #endregion

            #region Send OnUpdateStatusRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnUpdateStatusRequest != null)
                    await Task.WhenAll(OnUpdateStatusRequest.GetInvocationList().
                                       Cast<OnUpdateStatusRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.EVSEStatus.   ULongCount(),
                                                     Request.EVSEStatus,
                                                     Request.ParkingStatus.ULongCount(),
                                                     Request.ParkingStatus,
                                                     Request.DefaultTTL,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnUpdateStatusRequest));
            }

            #endregion


            #region No charge point status to update?

            if (!Request.EVSEStatus.   Any() &&
                !Request.ParkingStatus.Any())
            {

                result = HTTPResponse<UpdateStatusResponse>.OK(
                             new UpdateStatusResponse(Request,
                                                      Result.NoOperation("No status to update!"))
                         );

            }

            #endregion

            else do
            {

                using (var ochpClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        LocalCertificateSelector:   LocalCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await ochpClient.Query(_CustomUpdateStatusSOAPRequestMapper(Request,
                                                                                          SOAP.Encapsulation(
                                                                                              WSSLoginPassword.Item1,
                                                                                              WSSLoginPassword.Item2,
                                                                                              Request.ToXML()
                                                                                          )),
                                                     "http://ochp.e-clearing.net/service/UpdateStatus",
                                                     RequestLogDelegate:   OnUpdateStatusSOAPRequest,
                                                     ResponseLogDelegate:  OnUpdateStatusSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, UpdateStatusResponse.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return HTTPResponse<UpdateStatusResponse>.IsFault(
                                                                    httpresponse,
                                                                    new UpdateStatusResponse(
                                                                        Request,
                                                                        Result.Format(
                                                                            "Invalid SOAP => " +
                                                                            httpresponse.HTTPBody.ToUTF8String()
                                                                        )
                                                                    )
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);

                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                                                         {

                                                             return HTTPResponse<UpdateStatusResponse>.IsFault(
                                                                 httpresponse,
                                                                 new UpdateStatusResponse(
                                                                     Request,
                                                                     Result.Server(
                                                                          httpresponse.HTTPStatusCode +
                                                                          " => " +
                                                                          httpresponse.HTTPBody.ToUTF8String()
                                                                     )
                                                                 )
                                                             );

                                                         }

                                                         return HTTPResponse<UpdateStatusResponse>.IsFault(
                                                             httpresponse,
                                                             new UpdateStatusResponse(
                                                                 Request,
                                                                 Result.Server(
                                                                      httpresponse.HTTPStatusCode +
                                                                      " => " +
                                                                      httpresponse.HTTPBody.ToUTF8String()
                                                                 )
                                                             )
                                                         );

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<UpdateStatusResponse>.ExceptionThrown(

                                                             new UpdateStatusResponse(
                                                                 Request,
                                                                 Result.Format(exception.Message +
                                                                               " => " +
                                                                               exception.StackTrace)
                                                             ),

                                                             Exception: exception

                                                         );

                                                     }

                                                     #endregion

                                                    );

                }

                if (result == null)
                    result = HTTPResponse<UpdateStatusResponse>.ClientError(
                                 new UpdateStatusResponse(Request,
                                                          Result.Client("HTTP request failed!"))
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
               TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnUpdateStatusResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnUpdateStatusResponse != null)
                    await Task.WhenAll(OnUpdateStatusResponse.GetInvocationList().
                                       Cast<OnUpdateStatusResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.EVSEStatus.ULongCount(),
                                                     Request.EVSEStatus,
                                                     Request.ParkingStatus.ULongCount(),
                                                     Request.ParkingStatus,
                                                     Request.DefaultTTL,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnUpdateStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region UpdateTariffs        (Request)

        /// <summary>
        /// Upload the given enumeration of tariff infos.
        /// </summary>
        /// <param name="Request">A UpdateTariffs request.</param>
        public async Task<HTTPResponse<UpdateTariffsResponse>>

            UpdateTariffs(UpdateTariffsRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given UpdateTariffs request must not be null!");

            Request = _CustomUpdateTariffsRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped UpdateTariffs request must not be null!");


            Byte                                TransmissionRetry  = 0;
            HTTPResponse<UpdateTariffsResponse> result             = null;

            #endregion

            #region Send OnUpdateTariffsRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnUpdateTariffsRequest != null)
                    await Task.WhenAll(OnUpdateTariffsRequest.GetInvocationList().
                                       Cast<OnUpdateTariffsRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.TariffInfos,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnUpdateTariffsRequest));
            }

            #endregion


            #region No tariff infos to update?

            if (!Request.TariffInfos.Any())
            {

                result = HTTPResponse<UpdateTariffsResponse>.OK(
                             new UpdateTariffsResponse(Request,
                                                               Result.NoOperation("No tariff info to update!"))
                         );

            }

            #endregion

            else do
            {

                using (var ochpClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        LocalCertificateSelector:   LocalCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await ochpClient.Query(SOAP.Encapsulation(WSSLoginPassword.Item1, WSSLoginPassword.Item2, Request.ToXML()),
                                                     "http://ochp.eu/1.4/UpdateTariffs",
                                                     RequestLogDelegate:   OnUpdateTariffsSOAPRequest,
                                                     ResponseLogDelegate:  OnUpdateTariffsSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, UpdateTariffsResponse.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return HTTPResponse<UpdateTariffsResponse>.IsFault(
                                                                    httpresponse,
                                                                    new UpdateTariffsResponse(
                                                                        Request,
                                                                        Result.Format(
                                                                            "Invalid SOAP => " +
                                                                            httpresponse.HTTPBody.ToUTF8String()
                                                                        )
                                                                    )
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);

                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                                                         {

                                                             return HTTPResponse<UpdateTariffsResponse>.IsFault(
                                                                 httpresponse,
                                                                 new UpdateTariffsResponse(
                                                                     Request,
                                                                     Result.Server(
                                                                          httpresponse.HTTPStatusCode +
                                                                          " => " +
                                                                          httpresponse.HTTPBody.ToUTF8String()
                                                                     )
                                                                 )
                                                             );

                                                         }

                                                         return HTTPResponse<UpdateTariffsResponse>.IsFault(
                                                             httpresponse,
                                                             new UpdateTariffsResponse(
                                                                 Request,
                                                                 Result.Server(
                                                                      httpresponse.HTTPStatusCode +
                                                                      " => " +
                                                                      httpresponse.HTTPBody.ToUTF8String()
                                                                 )
                                                             )
                                                         );

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<UpdateTariffsResponse>.ExceptionThrown(

                                                             new UpdateTariffsResponse(
                                                                 Request,
                                                                 Result.Format(exception.Message +
                                                                               " => " +
                                                                               exception.StackTrace)
                                                             ),

                                                             Exception: exception

                                                         );

                                                     }

                                                     #endregion

                                                    );

                }

                if (result == null)
                    result = HTTPResponse<UpdateTariffsResponse>.ClientError(
                                 new UpdateTariffsResponse(Request,
                                                                   Result.Client("HTTP request failed!"))
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnUpdateTariffsResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnUpdateTariffsResponse != null)
                    await Task.WhenAll(OnUpdateTariffsResponse.GetInvocationList().
                                       Cast<OnUpdateTariffsResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.TariffInfos,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnUpdateTariffsResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region GetSingleRoamingAuthorisation     (Request)

        /// <summary>
        /// Authenticate the given token.
        /// </summary>
        /// <param name="Request">A GetSingleRoamingAuthorisation request.</param>
        public async Task<HTTPResponse<GetSingleRoamingAuthorisationResponse>>

            GetSingleRoamingAuthorisation(GetSingleRoamingAuthorisationRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given GetSingleRoamingAuthorisation request must not be null!");

            Request = _CustomGetSingleRoamingAuthorisationRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped GetSingleRoamingAuthorisation request must not be null!");


            Byte                                                TransmissionRetry  = 0;
            HTTPResponse<GetSingleRoamingAuthorisationResponse> result             = null;

            #endregion

            #region Send OnGetSingleRoamingAuthorisationRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnGetSingleRoamingAuthorisationRequest != null)
                    await Task.WhenAll(OnGetSingleRoamingAuthorisationRequest.GetInvocationList().
                                       Cast<OnGetSingleRoamingAuthorisationRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.EMTId,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnUpdateStatusRequest));
            }

            #endregion


            do
            {

                using (var ochpClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        LocalCertificateSelector:   LocalCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await ochpClient.Query(_CustomGetSingleRoamingAuthorisationSOAPRequestMapper(Request,
                                                                                                           SOAP.Encapsulation(
                                                                                                               WSSLoginPassword.Item1,
                                                                                                               WSSLoginPassword.Item2,
                                                                                                               Request.ToXML()
                                                                                                           )),
                                                     "http://ochp.eu/1.4/GetSingleRoamingAuthorisation",
                                                     RequestLogDelegate:   OnGetSingleRoamingAuthorisationSOAPRequest,
                                                     ResponseLogDelegate:  OnGetSingleRoamingAuthorisationSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, GetSingleRoamingAuthorisationResponse.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return HTTPResponse<GetSingleRoamingAuthorisationResponse>.IsFault(
                                                                    httpresponse,
                                                                    new GetSingleRoamingAuthorisationResponse(
                                                                        Request,
                                                                        Result.Format(
                                                                            "Invalid SOAP => " +
                                                                            httpresponse.HTTPBody.ToUTF8String()
                                                                        )
                                                                    )
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);

                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                                                         {

                                                             return HTTPResponse<GetSingleRoamingAuthorisationResponse>.IsFault(
                                                                 httpresponse,
                                                                 new GetSingleRoamingAuthorisationResponse(
                                                                     Request,
                                                                     Result.Server(
                                                                          httpresponse.HTTPStatusCode +
                                                                          " => " +
                                                                          httpresponse.HTTPBody.ToUTF8String()
                                                                     )
                                                                 )
                                                             );

                                                         }

                                                         return HTTPResponse<GetSingleRoamingAuthorisationResponse>.IsFault(
                                                             httpresponse,
                                                             new GetSingleRoamingAuthorisationResponse(
                                                                 Request,
                                                                 Result.Server(
                                                                      httpresponse.HTTPStatusCode +
                                                                      " => " +
                                                                      httpresponse.HTTPBody.ToUTF8String()
                                                                 )
                                                             )
                                                         );

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<GetSingleRoamingAuthorisationResponse>.ExceptionThrown(

                                                             new GetSingleRoamingAuthorisationResponse(
                                                                 Request,
                                                                 Result.Format(exception.Message +
                                                                               " => " +
                                                                               exception.StackTrace)
                                                             ),

                                                             Exception: exception

                                                         );

                                                     }

                                                     #endregion

                                                    );

                }

                if (result == null)
                    result = HTTPResponse<GetSingleRoamingAuthorisationResponse>.ClientError(
                                 new GetSingleRoamingAuthorisationResponse(Request,
                                                                           Result.Client("HTTP request failed!"))
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
               TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnGetSingleRoamingAuthorisationResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnGetSingleRoamingAuthorisationResponse != null)
                    await Task.WhenAll(OnGetSingleRoamingAuthorisationResponse.GetInvocationList().
                                       Cast<OnGetSingleRoamingAuthorisationResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.EMTId,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnGetSingleRoamingAuthorisationResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region GetRoamingAuthorisationList       (Request)

        /// <summary>
        /// Get the entire current version of the roaming authorisation list.
        /// </summary>
        /// <param name="Request">A GetRoamingAuthorisationList request.</param>
        public async Task<HTTPResponse<GetRoamingAuthorisationListResponse>>

            GetRoamingAuthorisationList(GetRoamingAuthorisationListRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given GetRoamingAuthorisationList request must not be null!");

            Request = _CustomGetRoamingAuthorisationListRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped GetRoamingAuthorisationList request must not be null!");


            Byte                                                TransmissionRetry  = 0;
            HTTPResponse<GetRoamingAuthorisationListResponse> result             = null;

            #endregion

            #region Send OnGetRoamingAuthorisationListRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnGetRoamingAuthorisationListRequest != null)
                    await Task.WhenAll(OnGetRoamingAuthorisationListRequest.GetInvocationList().
                                       Cast<OnGetRoamingAuthorisationListRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnUpdateStatusRequest));
            }

            #endregion


            do
            {

                using (var ochpClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        LocalCertificateSelector:   LocalCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await ochpClient.Query(_CustomGetRoamingAuthorisationListSOAPRequestMapper(Request,
                                                                                                         SOAP.Encapsulation(
                                                                                                             WSSLoginPassword.Item1,
                                                                                                             WSSLoginPassword.Item2,
                                                                                                             Request.ToXML()
                                                                                                         )),
                                                     "http://ochp.eu/1.4/GetRoamingAuthorisationList",
                                                     RequestLogDelegate:   OnGetRoamingAuthorisationListSOAPRequest,
                                                     ResponseLogDelegate:  OnGetRoamingAuthorisationListSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, GetRoamingAuthorisationListResponse.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return HTTPResponse<GetRoamingAuthorisationListResponse>.IsFault(
                                                                    httpresponse,
                                                                    new GetRoamingAuthorisationListResponse(
                                                                        Request,
                                                                        Result.Format(
                                                                            "Invalid SOAP => " +
                                                                            httpresponse.HTTPBody.ToUTF8String()
                                                                        )
                                                                    )
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);

                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                                                         {

                                                             return HTTPResponse<GetRoamingAuthorisationListResponse>.IsFault(
                                                                 httpresponse,
                                                                 new GetRoamingAuthorisationListResponse(
                                                                     Request,
                                                                     Result.Server(
                                                                          httpresponse.HTTPStatusCode +
                                                                          " => " +
                                                                          httpresponse.HTTPBody.ToUTF8String()
                                                                     )
                                                                 )
                                                             );

                                                         }

                                                         return HTTPResponse<GetRoamingAuthorisationListResponse>.IsFault(
                                                             httpresponse,
                                                             new GetRoamingAuthorisationListResponse(
                                                                 Request,
                                                                 Result.Server(
                                                                      httpresponse.HTTPStatusCode +
                                                                      " => " +
                                                                      httpresponse.HTTPBody.ToUTF8String()
                                                                 )
                                                             )
                                                         );

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<GetRoamingAuthorisationListResponse>.ExceptionThrown(

                                                             new GetRoamingAuthorisationListResponse(
                                                                 Request,
                                                                 Result.Format(exception.Message +
                                                                               " => " +
                                                                               exception.StackTrace)
                                                             ),

                                                             Exception: exception

                                                         );

                                                     }

                                                     #endregion

                                                    );

                }

                if (result == null)
                    result = HTTPResponse<GetRoamingAuthorisationListResponse>.ClientError(
                                 new GetRoamingAuthorisationListResponse(Request,
                                                                           Result.Client("HTTP request failed!"))
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
               TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnGetRoamingAuthorisationListResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnGetRoamingAuthorisationListResponse != null)
                    await Task.WhenAll(OnGetRoamingAuthorisationListResponse.GetInvocationList().
                                       Cast<OnGetRoamingAuthorisationListResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnGetRoamingAuthorisationListResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region GetRoamingAuthorisationListUpdates(Request)

        /// <summary>
        /// Get the entire current version of the roaming authorisation list.
        /// </summary>
        /// <param name="Request">A GetRoamingAuthorisationListUpdates request.</param>
        public async Task<HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>>

            GetRoamingAuthorisationListUpdates(GetRoamingAuthorisationListUpdatesRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given GetRoamingAuthorisationListUpdates request must not be null!");

            Request = _CustomGetRoamingAuthorisationListUpdatesRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped GetRoamingAuthorisationListUpdates request must not be null!");


            Byte                                                     TransmissionRetry  = 0;
            HTTPResponse<GetRoamingAuthorisationListUpdatesResponse> result             = null;

            #endregion

            #region Send OnGetRoamingAuthorisationListUpdatesRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnGetRoamingAuthorisationListUpdatesRequest != null)
                    await Task.WhenAll(OnGetRoamingAuthorisationListUpdatesRequest.GetInvocationList().
                                       Cast<OnGetRoamingAuthorisationListUpdatesRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.LastUpdate,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnUpdateStatusRequest));
            }

            #endregion


            do
            {

                using (var ochpClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        LocalCertificateSelector:   LocalCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await ochpClient.Query(_CustomGetRoamingAuthorisationListUpdatesSOAPRequestMapper(Request,
                                                                                                                SOAP.Encapsulation(
                                                                                                                    WSSLoginPassword.Item1,
                                                                                                                    WSSLoginPassword.Item2,
                                                                                                                    Request.ToXML()
                                                                                                                )),
                                                     "http://ochp.eu/1.4/GetRoamingAuthorisationListUpdates",
                                                     RequestLogDelegate:   OnGetRoamingAuthorisationListUpdatesSOAPRequest,
                                                     ResponseLogDelegate:  OnGetRoamingAuthorisationListUpdatesSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, GetRoamingAuthorisationListUpdatesResponse.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>.IsFault(
                                                                    httpresponse,
                                                                    new GetRoamingAuthorisationListUpdatesResponse(
                                                                        Request,
                                                                        Result.Format(
                                                                            "Invalid SOAP => " +
                                                                            httpresponse.HTTPBody.ToUTF8String()
                                                                        )
                                                                    )
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);

                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                                                         {

                                                             return HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>.IsFault(
                                                                 httpresponse,
                                                                 new GetRoamingAuthorisationListUpdatesResponse(
                                                                     Request,
                                                                     Result.Server(
                                                                          httpresponse.HTTPStatusCode +
                                                                          " => " +
                                                                          httpresponse.HTTPBody.ToUTF8String()
                                                                     )
                                                                 )
                                                             );

                                                         }

                                                         return HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>.IsFault(
                                                             httpresponse,
                                                             new GetRoamingAuthorisationListUpdatesResponse(
                                                                 Request,
                                                                 Result.Server(
                                                                      httpresponse.HTTPStatusCode +
                                                                      " => " +
                                                                      httpresponse.HTTPBody.ToUTF8String()
                                                                 )
                                                             )
                                                         );

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>.ExceptionThrown(

                                                             new GetRoamingAuthorisationListUpdatesResponse(
                                                                 Request,
                                                                 Result.Format(exception.Message +
                                                                               " => " +
                                                                               exception.StackTrace)
                                                             ),

                                                             Exception: exception

                                                         );

                                                     }

                                                     #endregion

                                                    );

                }

                if (result == null)
                    result = HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>.ClientError(
                                 new GetRoamingAuthorisationListUpdatesResponse(Request,
                                                                           Result.Client("HTTP request failed!"))
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
               TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnGetRoamingAuthorisationListUpdatesResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnGetRoamingAuthorisationListUpdatesResponse != null)
                    await Task.WhenAll(OnGetRoamingAuthorisationListUpdatesResponse.GetInvocationList().
                                       Cast<OnGetRoamingAuthorisationListUpdatesResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.LastUpdate,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnGetRoamingAuthorisationListUpdatesResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region AddCDRs  (Request)

        /// <summary>
        /// Upload the given enumeration of charge detail records.
        /// </summary>
        /// <param name="Request">An AddCDRs request.</param>
        public async Task<HTTPResponse<AddCDRsResponse>>

            AddCDRs(AddCDRsRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given AddCDRs request must not be null!");

            Request = _CustomAddCDRsRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped AddCDRs request must not be null!");


            Byte                                                TransmissionRetry  = 0;
            HTTPResponse<AddCDRsResponse> result             = null;

            #endregion

            #region Send OnAddCDRsRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnAddCDRsRequest != null)
                    await Task.WhenAll(OnAddCDRsRequest.GetInvocationList().
                                       Cast<OnAddCDRsRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.CDRInfos,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnUpdateStatusRequest));
            }

            #endregion


            #region No CDR infos to upload?

            if (!Request.CDRInfos.Any())
            {

                result = HTTPResponse<AddCDRsResponse>.OK(
                             new AddCDRsResponse(Request,
                                                 Result.NoOperation("No CDR infos to upload!"))
                         );

            }

            #endregion

            else do
            {

                using (var ochpClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        LocalCertificateSelector:   LocalCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await ochpClient.Query(_CustomAddCDRsSOAPRequestMapper(Request,
                                                                                     SOAP.Encapsulation(
                                                                                         WSSLoginPassword.Item1,
                                                                                         WSSLoginPassword.Item2,
                                                                                         Request.ToXML()
                                                                                     )),
                                                     "http://ochp.eu/1.4/AddCDRs",
                                                     RequestLogDelegate:   OnAddCDRsSOAPRequest,
                                                     ResponseLogDelegate:  OnAddCDRsSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, AddCDRsResponse.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return HTTPResponse<AddCDRsResponse>.IsFault(
                                                                    httpresponse,
                                                                    new AddCDRsResponse(
                                                                        Request,
                                                                        Result.Format(
                                                                            "Invalid SOAP => " +
                                                                            httpresponse.HTTPBody.ToUTF8String()
                                                                        )
                                                                    )
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);

                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                                                         {

                                                             return HTTPResponse<AddCDRsResponse>.IsFault(
                                                                 httpresponse,
                                                                 new AddCDRsResponse(
                                                                     Request,
                                                                     Result.Server(
                                                                          httpresponse.HTTPStatusCode +
                                                                          " => " +
                                                                          httpresponse.HTTPBody.ToUTF8String()
                                                                     )
                                                                 ));

                                                         }

                                                         return HTTPResponse<AddCDRsResponse>.IsFault(
                                                             httpresponse,
                                                             new AddCDRsResponse(
                                                                 Request,
                                                                 Result.Server(
                                                                      httpresponse.HTTPStatusCode +
                                                                      " => " +
                                                                      httpresponse.HTTPBody.ToUTF8String()
                                                                 )
                                                             )
                                                         );

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<AddCDRsResponse>.ExceptionThrown(

                                                             new AddCDRsResponse(
                                                                 Request,
                                                                 Result.Format(exception.Message +
                                                                               " => " +
                                                                               exception.StackTrace)
                                                             ),

                                                             Exception: exception

                                                         );

                                                     }

                                                     #endregion

                                                    );

                }

                if (result == null)
                    result = HTTPResponse<AddCDRsResponse>.ClientError(
                                 new AddCDRsResponse(Request,
                                                                           Result.Client("HTTP request failed!"))
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
               TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnAddCDRsResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnAddCDRsResponse != null)
                    await Task.WhenAll(OnAddCDRsResponse.GetInvocationList().
                                       Cast<OnAddCDRsResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.CDRInfos,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnAddCDRsResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region CheckCDRs(Request)

        /// <summary>
        /// Check charge detail records having the given optional status.
        /// </summary>
        /// <param name="Request">A CheckCDRs request.</param>
        public async Task<HTTPResponse<CheckCDRsResponse>>

            CheckCDRs(CheckCDRsRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given CheckCDRs request must not be null!");

            Request = _CustomCheckCDRsRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped CheckCDRs request must not be null!");


            Byte                            TransmissionRetry  = 0;
            HTTPResponse<CheckCDRsResponse> result             = null;

            #endregion

            #region Send OnCheckCDRsRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnCheckCDRsRequest != null)
                    await Task.WhenAll(OnCheckCDRsRequest.GetInvocationList().
                                       Cast<OnCheckCDRsRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.CDRStatus,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnUpdateStatusRequest));
            }

            #endregion


            do
            {

                using (var ochpClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        LocalCertificateSelector:   LocalCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await ochpClient.Query(_CustomCheckCDRsSOAPRequestMapper(Request,
                                                                                       SOAP.Encapsulation(
                                                                                           WSSLoginPassword.Item1,
                                                                                           WSSLoginPassword.Item2,
                                                                                           Request.ToXML()
                                                                                       )),
                                                     "http://ochp.eu/1.4/CheckCDRs",
                                                     RequestLogDelegate:   OnCheckCDRsSOAPRequest,
                                                     ResponseLogDelegate:  OnCheckCDRsSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, CheckCDRsResponse.Parse),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, this, httpresponse.Content);

                                                         return HTTPResponse<CheckCDRsResponse>.IsFault(
                                                                    httpresponse,
                                                                    new CheckCDRsResponse(
                                                                        Request,
                                                                        Result.Format(
                                                                            "Invalid SOAP => " +
                                                                            httpresponse.HTTPBody.ToUTF8String()
                                                                        )
                                                                    )
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, this, httpresponse);

                                                         if (httpresponse.HTTPStatusCode == HTTPStatusCode.ServiceUnavailable ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Unauthorized       ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.Forbidden          ||
                                                             httpresponse.HTTPStatusCode == HTTPStatusCode.NotFound)
                                                         {

                                                             return HTTPResponse<CheckCDRsResponse>.IsFault(
                                                                 httpresponse,
                                                                 new CheckCDRsResponse(
                                                                     Request,
                                                                     Result.Server(
                                                                          httpresponse.HTTPStatusCode +
                                                                          " => " +
                                                                          httpresponse.HTTPBody.ToUTF8String()
                                                                     )
                                                                 )
                                                              );

                                                         }

                                                         return HTTPResponse<CheckCDRsResponse>.IsFault(
                                                             httpresponse,
                                                             new CheckCDRsResponse(
                                                                 Request,
                                                                 Result.Server(
                                                                      httpresponse.HTTPStatusCode +
                                                                      " => " +
                                                                      httpresponse.HTTPBody.ToUTF8String()
                                                                 )
                                                             )
                                                         );

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<CheckCDRsResponse>.ExceptionThrown(

                                                             new CheckCDRsResponse(
                                                                 Request,
                                                                 Result.Format(exception.Message +
                                                                               " => " +
                                                                               exception.StackTrace)
                                                             ),

                                                             Exception: exception

                                                         );

                                                     }

                                                     #endregion

                                                    );

                }

                if (result == null)
                    result = HTTPResponse<CheckCDRsResponse>.ClientError(
                                 new CheckCDRsResponse(Request,
                                                                           Result.Client("HTTP request failed!"))
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
               TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnCheckCDRsResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnCheckCDRsResponse != null)
                    await Task.WhenAll(OnCheckCDRsResponse.GetInvocationList().
                                       Cast<OnCheckCDRsResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.CDRStatus,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnCheckCDRsResponse));
            }

            #endregion

            return result;

        }

        #endregion


        // OCHPdirect

        #region AddServiceEndpoints(Request)

        /// <summary>
        /// Upload the given enumeration of OCHPdirect provider endpoints.
        /// </summary>
        /// <param name="Request">An GetCDRs request.</param>
        public async Task<HTTPResponse<AddServiceEndpointsResponse>>

            AddServiceEndpoints(AddServiceEndpointsRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given AddServiceEndpoints request must not be null!");

            Request = _CustomAddServiceEndpointsRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped AddServiceEndpoints request must not be null!");


            Byte                                      TransmissionRetry  = 0;
            HTTPResponse<AddServiceEndpointsResponse> result             = null;

            #endregion

            #region Send OnAddServiceEndpointsRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnAddServiceEndpointsRequest != null)
                    await Task.WhenAll(OnAddServiceEndpointsRequest.GetInvocationList().
                                       Cast<OnAddServiceEndpointsRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.OperatorEndpoints,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnAddServiceEndpointsRequest));
            }

            #endregion


            #region No provider endpoints to upload?

            if (!Request.OperatorEndpoints.Any())
            {

                result = HTTPResponse<AddServiceEndpointsResponse>.OK(
                             new AddServiceEndpointsResponse(Request,
                                                             Result.NoOperation("No provider service endpoints infos to upload!"))
                         );

            }

            #endregion

            else do
            {

                using (var ochpClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        LocalCertificateSelector:   LocalCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await ochpClient.Query(_CustomAddServiceEndpointsSOAPRequestMapper(Request,
                                                                                     SOAP.Encapsulation(
                                                                                         WSSLoginPassword.Item1,
                                                                                         WSSLoginPassword.Item2,
                                                                                         Request.ToXML()
                                                                                    )),
                                                     "http://ochp.eu/1.4/AddServiceEndpoints",
                                                     RequestLogDelegate:   OnAddServiceEndpointsSOAPRequest,
                                                     ResponseLogDelegate:  OnAddServiceEndpointsSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              AddServiceEndpointsResponse.Parse(request,
                                                                                                                                    xml,
                                                                                                                                    //CustomAuthorizeRemoteReservationStartParser,
                                                                                                                                    //CustomStatusCodeParser,
                                                                                                                                    onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return HTTPResponse<AddServiceEndpointsResponse>.IsFault(
                                                                    httpresponse,
                                                                    new AddServiceEndpointsResponse(
                                                                        Request,
                                                                        Result.Format(
                                                                            "Invalid SOAP => " +
                                                                            httpresponse.HTTPBody.ToUTF8String()
                                                                        )
                                                                    )
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

                                                         return HTTPResponse<AddServiceEndpointsResponse>.IsFault(
                                                                    httpresponse,
                                                                    new AddServiceEndpointsResponse(
                                                                        Request,
                                                                        Result.Server(
                                                                             httpresponse.HTTPStatusCode +
                                                                             " => " +
                                                                             httpresponse.HTTPBody.ToUTF8String()
                                                                        )
                                                                    )
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<AddServiceEndpointsResponse>.ExceptionThrown(

                                                                    new AddServiceEndpointsResponse(
                                                                        Request,
                                                                        Result.Format(exception.Message +
                                                                                      " => " +
                                                                                      exception.StackTrace)
                                                                    ),

                                                                    Exception: exception

                                                                );

                                                     }

                                                     #endregion

                                                    ).ConfigureAwait(false);

                }

                if (result == null)
                    result = HTTPResponse<AddServiceEndpointsResponse>.ClientError(
                                 new AddServiceEndpointsResponse(
                                     Request,
                                     Result.OK("Nothing to upload!")
                                     //StatusCodes.SystemError,
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnAddServiceEndpointsResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnAddServiceEndpointsResponse != null)
                    await Task.WhenAll(OnAddServiceEndpointsResponse.GetInvocationList().
                                       Cast<OnAddServiceEndpointsResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.OperatorEndpoints,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnAddServiceEndpointsResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region GetServiceEndpoints(Request)

        /// <summary>
        /// Download OCHPdirect provider endpoints.
        /// </summary>
        /// <param name="Request">An GetServiceEndpoints request.</param>
        public async Task<HTTPResponse<GetServiceEndpointsResponse>>

            GetServiceEndpoints(GetServiceEndpointsRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given GetServiceEndpoints request must not be null!");

            Request = _CustomGetServiceEndpointsRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped GetServiceEndpoints request must not be null!");


            Byte                                      TransmissionRetry  = 0;
            HTTPResponse<GetServiceEndpointsResponse> result             = null;

            #endregion

            #region Send OnGetServiceEndpointsRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnGetServiceEndpointsRequest != null)
                    await Task.WhenAll(OnGetServiceEndpointsRequest.GetInvocationList().
                                       Cast<OnGetServiceEndpointsRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnGetServiceEndpointsRequest));
            }

            #endregion


            do
            {

                using (var ochpClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        LocalCertificateSelector:   LocalCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await ochpClient.Query(_CustomGetServiceEndpointsSOAPRequestMapper(Request,
                                                                                     SOAP.Encapsulation(
                                                                                         WSSLoginPassword.Item1,
                                                                                         WSSLoginPassword.Item2,
                                                                                         Request.ToXML()
                                                                                    )),
                                                     "http://ochp.eu/1.4/GetServiceEndpoints",
                                                     RequestLogDelegate:   OnGetServiceEndpointsSOAPRequest,
                                                     ResponseLogDelegate:  OnGetServiceEndpointsSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              GetServiceEndpointsResponse.Parse(request,
                                                                                                                                    xml,
                                                                                                                                    //CustomAuthorizeRemoteReservationStartParser,
                                                                                                                                    //CustomStatusCodeParser,
                                                                                                                                    onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return HTTPResponse<GetServiceEndpointsResponse>.IsFault(
                                                                    httpresponse,
                                                                    new GetServiceEndpointsResponse(
                                                                        Request,
                                                                        Result.Format(
                                                                            "Invalid SOAP => " +
                                                                            httpresponse.HTTPBody.ToUTF8String()
                                                                        )
                                                                    )
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnHTTPError

                                                     OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                         SendHTTPError(timestamp, soapclient, httpresponse);

                                                         return HTTPResponse<GetServiceEndpointsResponse>.IsFault(
                                                                    httpresponse,
                                                                    new GetServiceEndpointsResponse(
                                                                        Request,
                                                                        Result.Server(
                                                                             httpresponse.HTTPStatusCode +
                                                                             " => " +
                                                                             httpresponse.HTTPBody.ToUTF8String()
                                                                        )
                                                                    )
                                                                );

                                                     },

                                                     #endregion

                                                     #region OnException

                                                     OnException: (timestamp, sender, exception) => {

                                                         SendException(timestamp, sender, exception);

                                                         return HTTPResponse<GetServiceEndpointsResponse>.ExceptionThrown(

                                                                    new GetServiceEndpointsResponse(
                                                                        Request,
                                                                        Result.Format(exception.Message +
                                                                                      " => " +
                                                                                      exception.StackTrace)
                                                                    ),

                                                                    Exception: exception

                                                                );

                                                     }

                                                     #endregion

                                                    ).ConfigureAwait(false);

                }

                if (result == null)
                    result = HTTPResponse<GetServiceEndpointsResponse>.ClientError(
                                 new GetServiceEndpointsResponse(
                                     Request,
                                     Result.OK("Nothing to upload!")
                                     //StatusCodes.SystemError,
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnGetServiceEndpointsResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnGetServiceEndpointsResponse != null)
                    await Task.WhenAll(OnGetServiceEndpointsResponse.GetInvocationList().
                                       Cast<OnGetServiceEndpointsResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnGetServiceEndpointsResponse));
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

            InformProvider(DirectMessages           DirectMessage,
                           EVSE_Id                  EVSEId,
                           Contract_Id              ContractId,
                           Direct_Id                DirectId,

                           DateTime?                SessionTimeoutAt    = null,
                           Single?                  StateOfCharge       = null,
                           Single?                  MaxPower            = null,
                           Single?                  MaxCurrent          = null,
                           Boolean?                 OnePhase            = null,
                           Single?                  MaxEnergy           = null,
                           Single?                  MinEnergy           = null,
                           DateTime?                Departure           = null,
                           Single?                  CurrentPower        = null,
                           Single?                  ChargedEnergy       = null,
                           Timestamped<Single>?     MeterReading        = null,
                           IEnumerable<CDRPeriod>?  ChargingPeriods     = null,
                           Single?                  CurrentCost         = null,
                           Currency?                Currency            = null,

                           DateTime?                Timestamp           = null,
                           EventTracking_Id?        EventTrackingId     = null,
                           TimeSpan?                RequestTimeout      = null,
                           CancellationToken        CancellationToken   = default)

        {

            #region Initial checks

            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            EventTrackingId ??= EventTracking_Id.New;
            RequestTimeout  ??= this.RequestTimeout;


            HTTPResponse<InformProviderResponse>? result = null;

            #endregion

            #region Send OnInformProviderSOAPRequest event

            var StartTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnInformProviderRequest?.Invoke(StartTime,
                                                Timestamp.Value,
                                                this,
                                                Description,
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

                                                RequestTimeout ?? DefaultRequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnInformProviderSOAPRequest));
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


            using (var ochpClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                    VirtualHostname:             VirtualHostname,
                                                    RemoteCertificateValidator:  RemoteCertificateValidator,
                                                    LocalCertificateSelector:   LocalCertificateSelector,
                                                    HTTPUserAgent:               HTTPUserAgent,
                                                    RequestTimeout:              RequestTimeout,
                                                    DNSClient:                   DNSClient))
            {

                result = await ochpClient.Query(SOAP.Encapsulation(WSSLoginPassword.Item1, WSSLoginPassword.Item2, Request.ToXML()),
                                                 "InformProviderMessage",
                                                 RequestLogDelegate:   OnInformProviderSOAPRequest,
                                                 ResponseLogDelegate:  OnInformProviderSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 RequestTimeout:       RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, InformProviderResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return HTTPResponse<InformProviderResponse>.IsFault(
                                                                httpresponse,
                                                                InformProviderResponse.Format(
                                                                   Request,
                                                                   "Invalid SOAP => " +
                                                                   httpresponse.HTTPBody.ToUTF8String()
                                                                )
                                                            );

                                                 },

                                                 #endregion

                                                 #region OnHTTPError

                                                 OnHTTPError: (timestamp, soapclient, httpresponse) => {

                                                     SendHTTPError(timestamp, this, httpresponse);

                                                     return HTTPResponse<InformProviderResponse>.IsFault(
                                                                httpresponse,
                                                                InformProviderResponse.Server(
                                                                    Request,
                                                                    httpresponse.HTTPStatusCode.ToString() +
                                                                    " => " +
                                                                    httpresponse.HTTPBody.      ToUTF8String()
                                                                )
                                                            );

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

            #region Handle HTTP client errors...

            if (result == null)
                result = HTTPResponse<InformProviderResponse>.ClientError(
                             new InformProviderResponse(Request,
                                                        Result.Client("HTTP request failed!"))
                         );

            #endregion


            #region Send OnInformProviderResponse event

            var EndTime = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            try
            {

                OnInformProviderResponse?.Invoke(EndTime,
                                                 Timestamp.Value,
                                                 this,
                                                 Description,
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

                                                 RequestTimeout ?? DefaultRequestTimeout,
                                                 result.Content,
                                                 EndTime - StartTime);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(CPOClient) + "." + nameof(OnInformProviderResponse));
            }

            #endregion

            return result;

        }

        #endregion


    }

}
