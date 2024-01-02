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

namespace cloud.charging.open.protocols.OCHPv1_4.EMP
{

    /// <summary>
    /// The OCHP EMP client.
    /// </summary>
    public partial class EMPClient : ASOAPClient,
                                     IEMPClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const           String    DefaultHTTPUserAgent      = "GraphDefined OCHP " + Version.Number + " EMP Client";

        /// <summary>
        /// The default remote TCP port to connect to.
        /// </summary>
        public new static readonly IPPort    DefaultRemotePort         = IPPort.HTTPS;

        /// <summary>
        /// The default URL path prefix.
        /// </summary>
        public new static readonly HTTPPath  DefaultURLPathPrefix      = HTTPPath.Parse("/service/ochp/v1.4/");

        /// <summary>
        /// The default Live URI prefix.
        /// </summary>
        public     static readonly HTTPPath  DefaultLiveURLPathPrefix  = HTTPPath.Parse("/live/ochp/v1.4");

        private EndpointInfos _EndpointInfos;

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

        #region Custom request/response mappers

        // OCHP

        #region CustomGetChargePointList(SOAP)RequestMapper

        #region CustomGetChargePointListRequestMapper

        private Func<GetChargePointListRequest, GetChargePointListRequest> _CustomGetChargePointListRequestMapper = _ => _;

        public Func<GetChargePointListRequest, GetChargePointListRequest> CustomGetChargePointListRequestMapper
        {

            get
            {
                return _CustomGetChargePointListRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetChargePointListRequestMapper = value;
            }

        }

        #endregion

        #region CustomGetChargePointListSOAPRequestMapper

        private Func<GetChargePointListRequest, XElement, XElement> _CustomGetChargePointListSOAPRequestMapper = (request, xml) => xml;

        public Func<GetChargePointListRequest, XElement, XElement> CustomGetChargePointListSOAPRequestMapper
        {

            get
            {
                return _CustomGetChargePointListSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetChargePointListSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<GetChargePointListResponse> CustomGetChargePointListParser { get; set; }

        #endregion

        #region CustomGetChargePointListUpdates(SOAP)RequestMapper

        #region CustomGetChargePointListUpdatesRequestMapper

        private Func<GetChargePointListUpdatesRequest, GetChargePointListUpdatesRequest> _CustomGetChargePointListUpdatesRequestMapper = _ => _;

        public Func<GetChargePointListUpdatesRequest, GetChargePointListUpdatesRequest> CustomGetChargePointListUpdatesRequestMapper
        {

            get
            {
                return _CustomGetChargePointListUpdatesRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetChargePointListUpdatesRequestMapper = value;
            }

        }

        #endregion

        #region CustomGetChargePointListUpdatesSOAPRequestMapper

        private Func<GetChargePointListUpdatesRequest, XElement, XElement> _CustomGetChargePointListUpdatesSOAPRequestMapper = (request, xml) => xml;

        public Func<GetChargePointListUpdatesRequest, XElement, XElement> CustomGetChargePointListUpdatesSOAPRequestMapper
        {

            get
            {
                return _CustomGetChargePointListUpdatesSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetChargePointListUpdatesSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<GetChargePointListUpdatesResponse> CustomGetChargePointListUpdatesParser { get; set; }

        #endregion

        #region CustomGetStatus(SOAP)RequestMapper

        #region CustomGetStatusRequestMapper

        private Func<GetStatusRequest, GetStatusRequest> _CustomGetStatusRequestMapper = _ => _;

        public Func<GetStatusRequest, GetStatusRequest> CustomGetStatusRequestMapper
        {

            get
            {
                return _CustomGetStatusRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetStatusRequestMapper = value;
            }

        }

        #endregion

        #region CustomGetStatusSOAPRequestMapper

        private Func<GetStatusRequest, XElement, XElement> _CustomGetStatusSOAPRequestMapper = (request, xml) => xml;

        public Func<GetStatusRequest, XElement, XElement> CustomGetStatusSOAPRequestMapper
        {

            get
            {
                return _CustomGetStatusSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetStatusSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<GetStatusResponse> CustomGetStatusParser { get; set; }

        #endregion

        #region CustomGetTariffUpdates(SOAP)RequestMapper

        #region CustomGetTariffUpdatesRequestMapper

        private Func<GetTariffUpdatesRequest, GetTariffUpdatesRequest> _CustomGetTariffUpdatesRequestMapper = _ => _;

        public Func<GetTariffUpdatesRequest, GetTariffUpdatesRequest> CustomGetTariffUpdatesRequestMapper
        {

            get
            {
                return _CustomGetTariffUpdatesRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetTariffUpdatesRequestMapper = value;
            }

        }

        #endregion

        #region CustomGetTariffUpdatesSOAPRequestMapper

        private Func<GetTariffUpdatesRequest, XElement, XElement> _CustomGetTariffUpdatesSOAPRequestMapper = (request, xml) => xml;

        public Func<GetTariffUpdatesRequest, XElement, XElement> CustomGetTariffUpdatesSOAPRequestMapper
        {

            get
            {
                return _CustomGetTariffUpdatesSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetTariffUpdatesSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<GetTariffUpdatesResponse> CustomGetTariffUpdatesParser { get; set; }

        #endregion


        #region CustomSetRoamingAuthorisationList(SOAP)RequestMapper

        #region CustomSetRoamingAuthorisationListRequestMapper

        private Func<SetRoamingAuthorisationListRequest, SetRoamingAuthorisationListRequest> _CustomSetRoamingAuthorisationListRequestMapper = _ => _;

        public Func<SetRoamingAuthorisationListRequest, SetRoamingAuthorisationListRequest> CustomSetRoamingAuthorisationListRequestMapper
        {

            get
            {
                return _CustomSetRoamingAuthorisationListRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetRoamingAuthorisationListRequestMapper = value;
            }

        }

        #endregion

        #region CustomSetRoamingAuthorisationListSOAPRequestMapper

        private Func<SetRoamingAuthorisationListRequest, XElement, XElement> _CustomSetRoamingAuthorisationListSOAPRequestMapper = (request, xml) => xml;

        public Func<SetRoamingAuthorisationListRequest, XElement, XElement> CustomSetRoamingAuthorisationListSOAPRequestMapper
        {

            get
            {
                return _CustomSetRoamingAuthorisationListSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomSetRoamingAuthorisationListSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<SetRoamingAuthorisationListResponse> CustomSetRoamingAuthorisationListParser { get; set; }

        #endregion

        #region CustomUpdateRoamingAuthorisationList(SOAP)RequestMapper

        #region CustomUpdateRoamingAuthorisationListRequestMapper

        private Func<UpdateRoamingAuthorisationListRequest, UpdateRoamingAuthorisationListRequest> _CustomUpdateRoamingAuthorisationListRequestMapper = _ => _;

        public Func<UpdateRoamingAuthorisationListRequest, UpdateRoamingAuthorisationListRequest> CustomUpdateRoamingAuthorisationListRequestMapper
        {

            get
            {
                return _CustomUpdateRoamingAuthorisationListRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomUpdateRoamingAuthorisationListRequestMapper = value;
            }

        }

        #endregion

        #region CustomUpdateRoamingAuthorisationListSOAPRequestMapper

        private Func<UpdateRoamingAuthorisationListRequest, XElement, XElement> _CustomUpdateRoamingAuthorisationListSOAPRequestMapper = (request, xml) => xml;

        public Func<UpdateRoamingAuthorisationListRequest, XElement, XElement> CustomUpdateRoamingAuthorisationListSOAPRequestMapper
        {

            get
            {
                return _CustomUpdateRoamingAuthorisationListSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomUpdateRoamingAuthorisationListSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<UpdateRoamingAuthorisationListResponse> CustomUpdateRoamingAuthorisationListParser { get; set; }

        #endregion


        #region CustomGetCDRs(SOAP)RequestMapper

        #region CustomGetCDRsRequestMapper

        private Func<GetCDRsRequest, GetCDRsRequest> _CustomGetCDRsRequestMapper = _ => _;

        public Func<GetCDRsRequest, GetCDRsRequest> CustomGetCDRsRequestMapper
        {

            get
            {
                return _CustomGetCDRsRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetCDRsRequestMapper = value;
            }

        }

        #endregion

        #region CustomGetCDRsSOAPRequestMapper

        private Func<GetCDRsRequest, XElement, XElement> _CustomGetCDRsSOAPRequestMapper = (request, xml) => xml;

        public Func<GetCDRsRequest, XElement, XElement> CustomGetCDRsSOAPRequestMapper
        {

            get
            {
                return _CustomGetCDRsSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomGetCDRsSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<GetCDRsResponse> CustomGetCDRsParser { get; set; }

        #endregion

        #region CustomConfirmCDRs(SOAP)RequestMapper

        #region CustomConfirmCDRsRequestMapper

        private Func<ConfirmCDRsRequest, ConfirmCDRsRequest> _CustomConfirmCDRsRequestMapper = _ => _;

        public Func<ConfirmCDRsRequest, ConfirmCDRsRequest> CustomConfirmCDRsRequestMapper
        {

            get
            {
                return _CustomConfirmCDRsRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomConfirmCDRsRequestMapper = value;
            }

        }

        #endregion

        #region CustomConfirmCDRsSOAPRequestMapper

        private Func<ConfirmCDRsRequest, XElement, XElement> _CustomConfirmCDRsSOAPRequestMapper = (request, xml) => xml;

        public Func<ConfirmCDRsRequest, XElement, XElement> CustomConfirmCDRsSOAPRequestMapper
        {

            get
            {
                return _CustomConfirmCDRsSOAPRequestMapper;
            }

            set
            {
                if (value != null)
                    _CustomConfirmCDRsSOAPRequestMapper = value;
            }

        }

        #endregion

        public CustomXMLParserDelegate<ConfirmCDRsResponse> CustomConfirmCDRsParser { get; set; }

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

        #region OnConfirmCDRsRequest/-Response

        /// <summary>
        /// An event fired whenever a charge detail record confirmation request will be send.
        /// </summary>
        public event OnConfirmCDRsRequestDelegate   OnConfirmCDRsRequest;

        /// <summary>
        /// An event fired whenever a charge detail record confirmation SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler        OnConfirmCDRsSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a charge detail record confirmation SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler       OnConfirmCDRsSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for a charge detail record confirmation request had been received.
        /// </summary>
        public event OnConfirmCDRsResponseDelegate  OnConfirmCDRsResponse;

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


        #region OnSelectEVSERequest/-Response

        /// <summary>
        /// An event fired whenever a request to select an EVSE will be send.
        /// </summary>
        public event OnSelectEVSERequestDelegate   OnSelectEVSERequest;

        /// <summary>
        /// An event fired whenever a SOAP request to select an EVSE will be send.
        /// </summary>
        public event ClientRequestLogHandler       OnSelectEVSESOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to select an EVSE had been received.
        /// </summary>
        public event ClientResponseLogHandler      OnSelectEVSESOAPResponse;

        /// <summary>
        /// An event fired whenever a response for request to select an EVSE had been received.
        /// </summary>
        public event OnSelectEVSEResponseDelegate  OnSelectEVSEResponse;

        #endregion

        #region OnControlEVSERequest/-Response

        /// <summary>
        /// An event fired whenever a request to control a direct charging process will be send.
        /// </summary>
        public event OnControlEVSERequestDelegate   OnControlEVSERequest;

        /// <summary>
        /// An event fired whenever a SOAP request to control a direct charging process will be send.
        /// </summary>
        public event ClientRequestLogHandler        OnControlEVSESOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to control a direct charging process had been received.
        /// </summary>
        public event ClientResponseLogHandler       OnControlEVSESOAPResponse;

        /// <summary>
        /// An event fired whenever a response for request to control a direct charging process had been received.
        /// </summary>
        public event OnControlEVSEResponseDelegate  OnControlEVSEResponse;

        #endregion

        #region OnReleaseEVSERequest/-Response

        /// <summary>
        /// An event fired whenever a request to release an EVSE will be send.
        /// </summary>
        public event OnReleaseEVSERequestDelegate   OnReleaseEVSERequest;

        /// <summary>
        /// An event fired whenever a SOAP request to release an EVSE will be send.
        /// </summary>
        public event ClientRequestLogHandler        OnReleaseEVSESOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request to release an EVSE had been received.
        /// </summary>
        public event ClientResponseLogHandler       OnReleaseEVSESOAPResponse;

        /// <summary>
        /// An event fired whenever a response for request to release an EVSE had been received.
        /// </summary>
        public event OnReleaseEVSEResponseDelegate  OnReleaseEVSEResponse;

        #endregion

        #region OnGetEVSEStatusRequest/-Response

        /// <summary>
        /// An event fired whenever a request for EVSE status will be send.
        /// </summary>
        public event OnGetEVSEStatusRequestDelegate   OnGetEVSEStatusRequest;

        /// <summary>
        /// An event fired whenever a SOAP request for EVSE status will be send.
        /// </summary>
        public event ClientRequestLogHandler          OnGetEVSEStatusSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a SOAP request for EVSE status had been received.
        /// </summary>
        public event ClientResponseLogHandler         OnGetEVSEStatusSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for an EVSE status request had been received.
        /// </summary>
        public event OnGetEVSEStatusResponseDelegate  OnGetEVSEStatusResponse;

        #endregion


        #region OnReportDiscrepancyRequest/-Response

        /// <summary>
        /// An event fired whenever a report discrepancy request will be send.
        /// </summary>
        public event OnReportDiscrepancyRequestDelegate   OnReportDiscrepancyRequest;

        /// <summary>
        /// An event fired whenever a report discrepancy SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler              OnReportDiscrepancySOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a report discrepancy SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler             OnReportDiscrepancySOAPResponse;

        /// <summary>
        /// An event fired whenever a response for a report discrepancy request had been received.
        /// </summary>
        public event OnReportDiscrepancyResponseDelegate  OnReportDiscrepancyResponse;

        #endregion

        #region OnGetInformProviderRequest/-Response

        /// <summary>
        /// An event fired whenever a get inform provider request will be send.
        /// </summary>
        public event OnGetInformProviderRequestDelegate   OnGetInformProviderRequest;

        /// <summary>
        /// An event fired whenever a get inform provider SOAP request will be send.
        /// </summary>
        public event ClientRequestLogHandler              OnGetInformProviderSOAPRequest;

        /// <summary>
        /// An event fired whenever a SOAP response for a get inform provider SOAP request had been received.
        /// </summary>
        public event ClientResponseLogHandler             OnGetInformProviderSOAPResponse;

        /// <summary>
        /// An event fired whenever a response for a get inform provider request had been received.
        /// </summary>
        public event OnGetInformProviderResponseDelegate  OnGetInformProviderResponse;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP EMP client.
        /// </summary>
        /// <param name="RemoteURL">The remote URL of the OICP HTTP endpoint to connect to.</param>
        /// <param name="VirtualHostname">An optional HTTP virtual hostname.</param>
        /// <param name="Description">An optional description of this CPO client.</param>
        /// <param name="RemoteCertificateValidator">The remote TLS certificate validator.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
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
        public EMPClient(URL                                  RemoteURL,
                         HTTPHostname?                        VirtualHostname              = null,
                         String?                              Description                  = null,
                         Boolean?                             PreferIPv4                   = null,
                         RemoteCertificateValidationHandler?  RemoteCertificateValidator   = null,
                         LocalCertificateSelectionHandler?    ClientCertificateSelector    = null,
                         X509Certificate?                     ClientCert                   = null,
                         SslProtocols?                        TLSProtocol                  = null,
                         String?                              HTTPUserAgent                = DefaultHTTPUserAgent,
                         IHTTPAuthentication?                 HTTPAuthentication           = null,
                         HTTPPath?                            URLPathPrefix                = null,
                         HTTPPath?                            LiveURLPathPrefix            = null,
                         Tuple<String, String>?               WSSLoginPassword             = null,
                         HTTPContentType?                     HTTPContentType              = null,
                         TimeSpan?                            RequestTimeout               = null,
                         TransmissionRetryDelayDelegate?      TransmissionRetryDelay       = null,
                         UInt16?                              MaxNumberOfRetries           = null,
                         UInt32?                              InternalBufferSize           = null,
                         Boolean?                             DisableLogging               = false,
                         String?                              LoggingPath                  = null,
                         String?                              LoggingContext               = Logger.DefaultContext,
                         LogfileCreatorDelegate?              LogfileCreator               = null,
                         DNSClient?                           DNSClient                    = null)

            : base(RemoteURL,
                   VirtualHostname,
                   Description,
                   PreferIPv4,
                   RemoteCertificateValidator,
                   ClientCertificateSelector,
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
            this._EndpointInfos     = new EndpointInfos();

            base.HTTPLogger         = this.DisableLogging == false
                                          ? new Logger(this,
                                                       LoggingPath,
                                                       LoggingContext,
                                                       LogfileCreator)
                                          : null;

        }

        #endregion


        // OCHP

        #region GetChargePointList       (Request)

        /// <summary>
        /// Download the current list of charge points.
        /// </summary>
        /// <param name="Request">A GetChargePointList request.</param>
        public async Task<HTTPResponse<GetChargePointListResponse>>

            GetChargePointList(GetChargePointListRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given GetChargePointList request must not be null!");

            Request = _CustomGetChargePointListRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped GetChargePointList request must not be null!");


            Byte                                     TransmissionRetry  = 0;
            HTTPResponse<GetChargePointListResponse> result             = null;

            #endregion

            #region Send OnGetChargePointListRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnGetChargePointListRequest != null)
                    await Task.WhenAll(OnGetChargePointListRequest.GetInvocationList().
                                       Cast<OnGetChargePointListRequestDelegate>().
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
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetChargePointListRequest));
            }

            #endregion


            do
            {

                using (var _OCHPClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        ClientCertificateSelector:   ClientCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await _OCHPClient.Query(_CustomGetChargePointListSOAPRequestMapper(Request,
                                                                                     SOAP.Encapsulation(
                                                                                         WSSLoginPassword.Item1,
                                                                                         WSSLoginPassword.Item2,
                                                                                         Request.ToXML()
                                                                                    )),
                                                     "http://ochp.eu/1.4/GetChargePointList",
                                                     RequestLogDelegate:   OnGetChargePointListSOAPRequest,
                                                     ResponseLogDelegate:  OnGetChargePointListSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              GetChargePointListResponse.Parse(request,
                                                                                                                                    xml,
                                                                                                                                    //CustomAuthorizeRemoteReservationStartParser,
                                                                                                                                    //CustomStatusCodeParser,
                                                                                                                                    onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return HTTPResponse<GetChargePointListResponse>.IsFault(
                                                                    httpresponse,
                                                                    new GetChargePointListResponse(
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

                                                         return HTTPResponse<GetChargePointListResponse>.IsFault(
                                                                    httpresponse,
                                                                    new GetChargePointListResponse(
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

                                                         return HTTPResponse<GetChargePointListResponse>.ExceptionThrown(

                                                                    new GetChargePointListResponse(
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
                    result = HTTPResponse<GetChargePointListResponse>.ClientError(
                                 new GetChargePointListResponse(
                                     Request,
                                     Result.OK("Nothing to upload!")
                                     //StatusCodes.SystemError,
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnGetChargePointListResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnGetChargePointListResponse != null)
                    await Task.WhenAll(OnGetChargePointListResponse.GetInvocationList().
                                       Cast<OnGetChargePointListResponseDelegate>().
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
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetChargePointListResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region GetChargePointListUpdates(Request)

        /// <summary>
        /// Download all charge point list updates since the given date.
        /// </summary>
        /// <param name="Request">A GetChargePointListUpdates request.</param>
        public async Task<HTTPResponse<GetChargePointListUpdatesResponse>>

            GetChargePointListUpdates(GetChargePointListUpdatesRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given GetChargePointListUpdates request must not be null!");

            Request = _CustomGetChargePointListUpdatesRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped GetChargePointListUpdates request must not be null!");


            Byte                                            TransmissionRetry  = 0;
            HTTPResponse<GetChargePointListUpdatesResponse> result             = null;

            #endregion

            #region Send OnGetChargePointListUpdatesRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnGetChargePointListUpdatesRequest != null)
                    await Task.WhenAll(OnGetChargePointListUpdatesRequest.GetInvocationList().
                                       Cast<OnGetChargePointListUpdatesRequestDelegate>().
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
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetChargePointListUpdatesRequest));
            }

            #endregion


            do
            {

                using (var _OCHPClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        ClientCertificateSelector:   ClientCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await _OCHPClient.Query(_CustomGetChargePointListUpdatesSOAPRequestMapper(Request,
                                                                                                       SOAP.Encapsulation(
                                                                                                           WSSLoginPassword.Item1,
                                                                                                           WSSLoginPassword.Item2,
                                                                                                           Request.ToXML()
                                                                                                      )),
                                                     "http://ochp.eu/1.4/GetChargePointListUpdates",
                                                     RequestLogDelegate:   OnGetChargePointListUpdatesSOAPRequest,
                                                     ResponseLogDelegate:  OnGetChargePointListUpdatesSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              GetChargePointListUpdatesResponse.Parse(request,
                                                                                                                                    xml,
                                                                                                                                    //CustomAuthorizeRemoteReservationStartParser,
                                                                                                                                    //CustomStatusCodeParser,
                                                                                                                                    onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return HTTPResponse<GetChargePointListUpdatesResponse>.IsFault(
                                                                    httpresponse,
                                                                    new GetChargePointListUpdatesResponse(
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

                                                         return HTTPResponse<GetChargePointListUpdatesResponse>.IsFault(
                                                                    httpresponse,
                                                                    new GetChargePointListUpdatesResponse(
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

                                                         return HTTPResponse<GetChargePointListUpdatesResponse>.ExceptionThrown(

                                                                    new GetChargePointListUpdatesResponse(
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
                    result = HTTPResponse<GetChargePointListUpdatesResponse>.ClientError(
                                 new GetChargePointListUpdatesResponse(
                                     Request,
                                     Result.OK("Nothing to upload!")
                                     //StatusCodes.SystemError,
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnGetChargePointListUpdatesResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnGetChargePointListUpdatesResponse != null)
                    await Task.WhenAll(OnGetChargePointListUpdatesResponse.GetInvocationList().
                                       Cast<OnGetChargePointListUpdatesResponseDelegate>().
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
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetChargePointListUpdatesResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region GetStatus                (Request)

        /// <summary>
        /// Download the current list of charge point status filtered by
        /// an optional last request timestamp or their status type.
        /// </summary>
        /// <param name="Request">A GetStatus request.</param>
        public async Task<HTTPResponse<GetStatusResponse>>

            GetStatus(GetStatusRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given GetStatus request must not be null!");

            Request = _CustomGetStatusRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped GetStatus request must not be null!");


            Byte                                     TransmissionRetry  = 0;
            HTTPResponse<GetStatusResponse> result             = null;

            #endregion

            #region Send OnGetStatusRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnGetStatusRequest != null)
                    await Task.WhenAll(OnGetStatusRequest.GetInvocationList().
                                       Cast<OnGetStatusRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.LastRequest,
                                                     Request.StatusType,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetStatusRequest));
            }

            #endregion


            do
            {

                using (var _OCHPClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        ClientCertificateSelector:   ClientCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await _OCHPClient.Query(_CustomGetStatusSOAPRequestMapper(Request,
                                                                                       SOAP.Encapsulation(
                                                                                           WSSLoginPassword.Item1,
                                                                                           WSSLoginPassword.Item2,
                                                                                           Request.ToXML()
                                                                                      )),
                                                     "http://ochp.e-clearing.net/service/GetStatus",
                                                     RequestLogDelegate:   OnGetStatusSOAPRequest,
                                                     ResponseLogDelegate:  OnGetStatusSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              GetStatusResponse.Parse(request,
                                                                                                                                    xml,
                                                                                                                                    //CustomAuthorizeRemoteReservationStartParser,
                                                                                                                                    //CustomStatusCodeParser,
                                                                                                                                    onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return HTTPResponse<GetStatusResponse>.IsFault(
                                                                    httpresponse,
                                                                    new GetStatusResponse(
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

                                                         return HTTPResponse<GetStatusResponse>.IsFault(
                                                                    httpresponse,
                                                                    new GetStatusResponse(
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

                                                         return HTTPResponse<GetStatusResponse>.ExceptionThrown(

                                                                    new GetStatusResponse(
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
                    result = HTTPResponse<GetStatusResponse>.ClientError(
                                 new GetStatusResponse(
                                     Request,
                                     Result.OK("Nothing to upload!")
                                     //StatusCodes.SystemError,
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnGetStatusResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnGetStatusResponse != null)
                    await Task.WhenAll(OnGetStatusResponse.GetInvocationList().
                                       Cast<OnGetStatusResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.LastRequest,
                                                     Request.StatusType,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetStatusResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region GetTariffUpdates         (Request)

        /// <summary>
        /// Download an update of the current tariff list since the given date.
        /// </summary>
        /// <param name="Request">A GetTariffUpdates request.</param>
        public async Task<HTTPResponse<GetTariffUpdatesResponse>>

            GetTariffUpdates(GetTariffUpdatesRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given GetTariffUpdates request must not be null!");

            Request = _CustomGetTariffUpdatesRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped GetTariffUpdates request must not be null!");


            Byte                                   TransmissionRetry  = 0;
            HTTPResponse<GetTariffUpdatesResponse> result             = null;

            #endregion

            #region Send OnGetTariffUpdatesRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnGetTariffUpdatesRequest != null)
                    await Task.WhenAll(OnGetTariffUpdatesRequest.GetInvocationList().
                                       Cast<OnGetTariffUpdatesRequestDelegate>().
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
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetTariffUpdatesRequest));
            }

            #endregion


            do
            {

                using (var _OCHPClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        ClientCertificateSelector:   ClientCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await _OCHPClient.Query(_CustomGetTariffUpdatesSOAPRequestMapper(Request,
                                                                                              SOAP.Encapsulation(
                                                                                                  WSSLoginPassword.Item1,
                                                                                                  WSSLoginPassword.Item2,
                                                                                                  Request.ToXML()
                                                                                             )),
                                                     "http://ochp.eu/1.4/GetTariffUpdates",
                                                     RequestLogDelegate:   OnGetTariffUpdatesSOAPRequest,
                                                     ResponseLogDelegate:  OnGetTariffUpdatesSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              GetTariffUpdatesResponse.Parse(request,
                                                                                                                                    xml,
                                                                                                                                    //CustomAuthorizeRemoteReservationStartParser,
                                                                                                                                    //CustomStatusCodeParser,
                                                                                                                                    onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return HTTPResponse<GetTariffUpdatesResponse>.IsFault(
                                                                    httpresponse,
                                                                    new GetTariffUpdatesResponse(
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

                                                         return HTTPResponse<GetTariffUpdatesResponse>.IsFault(
                                                                    httpresponse,
                                                                    new GetTariffUpdatesResponse(
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

                                                         return HTTPResponse<GetTariffUpdatesResponse>.ExceptionThrown(

                                                                    new GetTariffUpdatesResponse(
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
                    result = HTTPResponse<GetTariffUpdatesResponse>.ClientError(
                                 new GetTariffUpdatesResponse(
                                     Request,
                                     Result.OK("Nothing to upload!")
                                     //StatusCodes.SystemError,
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnGetTariffUpdatesResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnGetTariffUpdatesResponse != null)
                    await Task.WhenAll(OnGetTariffUpdatesResponse.GetInvocationList().
                                       Cast<OnGetTariffUpdatesResponseDelegate>().
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
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetTariffUpdatesResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SetRoamingAuthorisationList   (Request)

        /// <summary>
        /// Upload the entire roaming authorisation list.
        /// </summary>
        /// <param name="Request">An SetRoamingAuthorisationList request.</param>
        public async Task<HTTPResponse<SetRoamingAuthorisationListResponse>>

            SetRoamingAuthorisationList(SetRoamingAuthorisationListRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given SetRoamingAuthorisationList request must not be null!");

            Request = _CustomSetRoamingAuthorisationListRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped SetRoamingAuthorisationList request must not be null!");


            Byte                                              TransmissionRetry  = 0;
            HTTPResponse<SetRoamingAuthorisationListResponse> result             = null;

            #endregion

            #region Send OnSetRoamingAuthorisationListRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnSetRoamingAuthorisationListRequest != null)
                    await Task.WhenAll(OnSetRoamingAuthorisationListRequest.GetInvocationList().
                                       Cast<OnSetRoamingAuthorisationListRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.RoamingAuthorisationInfos,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnSetRoamingAuthorisationListRequest));
            }

            #endregion


            #region No charge point infos to upload?

            if (!Request.RoamingAuthorisationInfos.Any())
            {

                result = HTTPResponse<SetRoamingAuthorisationListResponse>.OK(
                             new SetRoamingAuthorisationListResponse(Request,
                                                                     Result.NoOperation("No roaming authorisation infos to upload!"))
                         );

            }

            #endregion

            else do
            {

                using (var _OCHPClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        ClientCertificateSelector:   ClientCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await _OCHPClient.Query(_CustomSetRoamingAuthorisationListSOAPRequestMapper(Request,
                                                                                                         SOAP.Encapsulation(
                                                                                                             WSSLoginPassword.Item1,
                                                                                                             WSSLoginPassword.Item2,
                                                                                                             Request.ToXML()
                                                                                                        )),
                                                     "http://ochp.eu/1.4/SetRoamingAuthorisationList",
                                                     RequestLogDelegate:   OnSetRoamingAuthorisationListSOAPRequest,
                                                     ResponseLogDelegate:  OnSetRoamingAuthorisationListSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              SetRoamingAuthorisationListResponse.Parse(request,
                                                                                                                                                        xml,
                                                                                                                                                        //CustomAuthorizeRemoteReservationStartParser,
                                                                                                                                                        //CustomStatusCodeParser,
                                                                                                                                                        onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return HTTPResponse<SetRoamingAuthorisationListResponse>.IsFault(
                                                                    httpresponse,
                                                                    new SetRoamingAuthorisationListResponse(
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

                                                         return HTTPResponse<SetRoamingAuthorisationListResponse>.IsFault(
                                                                    httpresponse,
                                                                    new SetRoamingAuthorisationListResponse(
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

                                                         return HTTPResponse<SetRoamingAuthorisationListResponse>.ExceptionThrown(

                                                                    new SetRoamingAuthorisationListResponse(
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

                //if (result == null)
                //    result = HTTPResponse<SetRoamingAuthorisationListResponse>.OK(new SetRoamingAuthorisationListResponse(Request, Result.OK("Nothing to upload!")));

                if (result == null)
                    result = HTTPResponse<SetRoamingAuthorisationListResponse>.ClientError(
                                 new SetRoamingAuthorisationListResponse(
                                     Request,
                                     Result.OK("Nothing to upload!")
                                     //StatusCodes.SystemError,
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnGetRoamingAuthorisationListResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnSetRoamingAuthorisationListResponse != null)
                    await Task.WhenAll(OnSetRoamingAuthorisationListResponse.GetInvocationList().
                                       Cast<OnSetRoamingAuthorisationListResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.RoamingAuthorisationInfos,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnSetRoamingAuthorisationListResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region UpdateRoamingAuthorisationList(Request)

        /// <summary>
        /// Send a roaming authorisation list update.
        /// </summary>
        /// <param name="Request">An UpdateRoamingAuthorisationList request.</param>
        public async Task<HTTPResponse<UpdateRoamingAuthorisationListResponse>>

            UpdateRoamingAuthorisationList(UpdateRoamingAuthorisationListRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given UpdateRoamingAuthorisationList request must not be null!");

            Request = _CustomUpdateRoamingAuthorisationListRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped UpdateRoamingAuthorisationList request must not be null!");


            Byte                                                 TransmissionRetry  = 0;
            HTTPResponse<UpdateRoamingAuthorisationListResponse> result             = null;

            #endregion

            #region Send OnUpdateRoamingAuthorisationListRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnUpdateRoamingAuthorisationListRequest != null)
                    await Task.WhenAll(OnUpdateRoamingAuthorisationListRequest.GetInvocationList().
                                       Cast<OnUpdateRoamingAuthorisationListRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.RoamingAuthorisationInfos,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnUpdateRoamingAuthorisationListRequest));
            }

            #endregion


            #region No charge point infos to upload?

            if (!Request.RoamingAuthorisationInfos.Any())
            {

                result = HTTPResponse<UpdateRoamingAuthorisationListResponse>.OK(
                             new UpdateRoamingAuthorisationListResponse(Request,
                                                                     Result.NoOperation("No roaming authorisation infos to upload!"))
                         );

            }

            #endregion

            else do
            {

                using (var _OCHPClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        ClientCertificateSelector:   ClientCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await _OCHPClient.Query(_CustomUpdateRoamingAuthorisationListSOAPRequestMapper(Request,
                                                                                                         SOAP.Encapsulation(
                                                                                                             WSSLoginPassword.Item1,
                                                                                                             WSSLoginPassword.Item2,
                                                                                                             Request.ToXML()
                                                                                                        )),
                                                     "http://ochp.eu/1.4/UpdateRoamingAuthorisationList",
                                                     RequestLogDelegate:   OnUpdateRoamingAuthorisationListSOAPRequest,
                                                     ResponseLogDelegate:  OnUpdateRoamingAuthorisationListSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              UpdateRoamingAuthorisationListResponse.Parse(request,
                                                                                                                                                        xml,
                                                                                                                                                        //CustomAuthorizeRemoteReservationStartParser,
                                                                                                                                                        //CustomStatusCodeParser,
                                                                                                                                                        onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return HTTPResponse<UpdateRoamingAuthorisationListResponse>.IsFault(
                                                                    httpresponse,
                                                                    new UpdateRoamingAuthorisationListResponse(
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

                                                         return HTTPResponse<UpdateRoamingAuthorisationListResponse>.IsFault(
                                                                    httpresponse,
                                                                    new UpdateRoamingAuthorisationListResponse(
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

                                                         return HTTPResponse<UpdateRoamingAuthorisationListResponse>.ExceptionThrown(

                                                                    new UpdateRoamingAuthorisationListResponse(
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

                //if (result == null)
                //    result = HTTPResponse<UpdateRoamingAuthorisationListResponse>.OK(new UpdateRoamingAuthorisationListResponse(Request, Result.OK("Nothing to upload!")));

                if (result == null)
                    result = HTTPResponse<UpdateRoamingAuthorisationListResponse>.ClientError(
                                 new UpdateRoamingAuthorisationListResponse(
                                     Request,
                                     Result.OK("Nothing to upload!")
                                     //StatusCodes.SystemError,
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnGetRoamingAuthorisationListResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnUpdateRoamingAuthorisationListResponse != null)
                    await Task.WhenAll(OnUpdateRoamingAuthorisationListResponse.GetInvocationList().
                                       Cast<OnUpdateRoamingAuthorisationListResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.RoamingAuthorisationInfos,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnUpdateRoamingAuthorisationListResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region GetCDRsRequest    (Request)

        /// <summary>
        /// Download charge detail records having the given optional status.
        /// </summary>
        /// <param name="Request">An GetCDRs request.</param>
        public async Task<HTTPResponse<GetCDRsResponse>>

            GetCDRs(GetCDRsRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given GetCDRs request must not be null!");

            Request = _CustomGetCDRsRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped GetCDRs request must not be null!");


            Byte                          TransmissionRetry  = 0;
            HTTPResponse<GetCDRsResponse> result             = null;

            #endregion

            #region Send OnGetCDRsRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnGetCDRsRequest != null)
                    await Task.WhenAll(OnGetCDRsRequest.GetInvocationList().
                                       Cast<OnGetCDRsRequestDelegate>().
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
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetCDRsRequest));
            }

            #endregion


            do
            {

                using (var _OCHPClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        ClientCertificateSelector:   ClientCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await _OCHPClient.Query(_CustomGetCDRsSOAPRequestMapper(Request,
                                                                                     SOAP.Encapsulation(
                                                                                         WSSLoginPassword.Item1,
                                                                                         WSSLoginPassword.Item2,
                                                                                         Request.ToXML()
                                                                                    )),
                                                     "http://ochp.eu/1.4/GetCDRs",
                                                     RequestLogDelegate:   OnGetCDRsSOAPRequest,
                                                     ResponseLogDelegate:  OnGetCDRsSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              GetCDRsResponse.Parse(request,
                                                                                                                                    xml,
                                                                                                                                    //CustomAuthorizeRemoteReservationStartParser,
                                                                                                                                    //CustomStatusCodeParser,
                                                                                                                                    onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return HTTPResponse<GetCDRsResponse>.IsFault(
                                                                    httpresponse,
                                                                    new GetCDRsResponse(
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

                                                         return HTTPResponse<GetCDRsResponse>.IsFault(
                                                                    httpresponse,
                                                                    new GetCDRsResponse(
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

                                                         return HTTPResponse<GetCDRsResponse>.ExceptionThrown(

                                                                    new GetCDRsResponse(
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
                    result = HTTPResponse<GetCDRsResponse>.ClientError(
                                 new GetCDRsResponse(
                                     Request,
                                     Result.OK("Nothing to upload!")
                                     //StatusCodes.SystemError,
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnGetCDRsResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnGetCDRsResponse != null)
                    await Task.WhenAll(OnGetCDRsResponse.GetInvocationList().
                                       Cast<OnGetCDRsResponseDelegate>().
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
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetCDRsResponse));
            }

            #endregion

            return result;

        }

        #endregion

        #region ConfirmCDRsRequest(Request)

        /// <summary>
        /// Approve or decline charge detail records.
        /// </summary>
        /// <param name="Request">An ConfirmCDRs request.</param>
        public async Task<HTTPResponse<ConfirmCDRsResponse>>

            ConfirmCDRs(ConfirmCDRsRequest Request)

        {

            #region Initial checks

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The given ConfirmCDRs request must not be null!");

            Request = _CustomConfirmCDRsRequestMapper(Request);

            if (Request == null)
                throw new ArgumentNullException(nameof(Request), "The mapped ConfirmCDRs request must not be null!");


            Byte                              TransmissionRetry  = 0;
            HTTPResponse<ConfirmCDRsResponse> result             = null;

            #endregion

            #region Send OnConfirmCDRsRequest event

            var StartTime = Timestamp.Now;

            try
            {

                if (OnConfirmCDRsRequest != null)
                    await Task.WhenAll(OnConfirmCDRsRequest.GetInvocationList().
                                       Cast<OnConfirmCDRsRequestDelegate>().
                                       Select(e => e(StartTime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.Approved,
                                                     Request.Declined,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnConfirmCDRsRequest));
            }

            #endregion


            #region No charge point infos to upload?

            if (!Request.Approved.Any() && !Request.Declined.Any())
            {

                result = HTTPResponse<ConfirmCDRsResponse>.OK(
                             new ConfirmCDRsResponse(Request,
                                                     Result.NoOperation("No CDRs to confirm!"))
                         );

            }

            #endregion

            else do
            {

                using (var _OCHPClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        ClientCertificateSelector:   ClientCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await _OCHPClient.Query(_CustomConfirmCDRsSOAPRequestMapper(Request,
                                                                                     SOAP.Encapsulation(
                                                                                         WSSLoginPassword.Item1,
                                                                                         WSSLoginPassword.Item2,
                                                                                         Request.ToXML()
                                                                                    )),
                                                     "http://ochp.eu/1.4/ConfirmCDRs",
                                                     RequestLogDelegate:   OnConfirmCDRsSOAPRequest,
                                                     ResponseLogDelegate:  OnConfirmCDRsSOAPResponse,
                                                     CancellationToken:    Request.CancellationToken,
                                                     EventTrackingId:      Request.EventTrackingId,
                                                     RequestTimeout:       Request.RequestTimeout ?? RequestTimeout,
                                                     NumberOfRetry:        TransmissionRetry,

                                                     #region OnSuccess

                                                     OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request,
                                                                                                          (request, xml, onexception) =>
                                                                                                              ConfirmCDRsResponse.Parse(request,
                                                                                                                                        xml,
                                                                                                                                        //CustomAuthorizeRemoteReservationStartParser,
                                                                                                                                        //CustomStatusCodeParser,
                                                                                                                                        onexception)),

                                                     #endregion

                                                     #region OnSOAPFault

                                                     OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                         SendSOAPError(timestamp, soapclient, httpresponse.Content);

                                                         return HTTPResponse<ConfirmCDRsResponse>.IsFault(
                                                                    httpresponse,
                                                                    new ConfirmCDRsResponse(
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

                                                         return HTTPResponse<ConfirmCDRsResponse>.IsFault(
                                                                    httpresponse,
                                                                    new ConfirmCDRsResponse(
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

                                                         return HTTPResponse<ConfirmCDRsResponse>.ExceptionThrown(

                                                                    new ConfirmCDRsResponse(
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
                    result = HTTPResponse<ConfirmCDRsResponse>.ClientError(
                                 new ConfirmCDRsResponse(
                                     Request,
                                     Result.OK("Nothing to upload!")
                                     //StatusCodes.SystemError,
                                     //"HTTP request failed!"
                                 )
                             );

            }
            while (result.HTTPStatusCode == HTTPStatusCode.RequestTimeout &&
                   TransmissionRetry++ < MaxNumberOfRetries);


            #region Send OnConfirmCDRsResponse event

            var Endtime = Timestamp.Now;

            try
            {

                if (OnConfirmCDRsResponse != null)
                    await Task.WhenAll(OnConfirmCDRsResponse.GetInvocationList().
                                       Cast<OnConfirmCDRsResponseDelegate>().
                                       Select(e => e(Endtime,
                                                     Request.Timestamp.Value,
                                                     this,
                                                     Description,
                                                     Request.EventTrackingId,
                                                     Request.Approved,
                                                     Request.Declined,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnConfirmCDRsResponse));
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
                                                     Request.ProviderEndpoints,
                                                     Request.RequestTimeout ?? RequestTimeout))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnAddServiceEndpointsRequest));
            }

            #endregion


            #region No provider endpoints to upload?

            if (!Request.ProviderEndpoints.Any())
            {

                result = HTTPResponse<AddServiceEndpointsResponse>.OK(
                             new AddServiceEndpointsResponse(Request,
                                                             Result.NoOperation("No provider service endpoints infos to upload!"))
                         );

            }

            #endregion

            else do
            {

                using (var _OCHPClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        ClientCertificateSelector:   ClientCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await _OCHPClient.Query(_CustomAddServiceEndpointsSOAPRequestMapper(Request,
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
                                                     Request.ProviderEndpoints,
                                                     Request.RequestTimeout ?? RequestTimeout,
                                                     result.Content,
                                                     Endtime - StartTime))).
                                       ConfigureAwait(false);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnAddServiceEndpointsResponse));
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


            Byte                          TransmissionRetry  = 0;
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
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetServiceEndpointsRequest));
            }

            #endregion


            do
            {

                using (var _OCHPClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                        VirtualHostname:             VirtualHostname,
                                                        RemoteCertificateValidator:  RemoteCertificateValidator,
                                                        ClientCertificateSelector:   ClientCertificateSelector,
                                                        HTTPUserAgent:               HTTPUserAgent,
                                                        RequestTimeout:              RequestTimeout,
                                                        DNSClient:                   DNSClient))
                {

                    result = await _OCHPClient.Query(_CustomGetServiceEndpointsSOAPRequestMapper(Request,
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
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetServiceEndpointsResponse));
            }

            #endregion

            return result;

        }

        #endregion


        #region SelectEVSE(...)

        /// <summary>
        /// Select an EVSE and create a new charging session.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ContractId">The unique identification of an e-mobility contract.</param>
        /// <param name="ReserveUntil">An optional timestamp till when then given EVSE should be reserved.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<SelectEVSEResponse>>

            SelectEVSE(EVSE_Id             EVSEId,
                       Contract_Id         ContractId,
                       DateTime?           ReserveUntil        = null,

                       DateTime?           Timestamp           = null,
                       EventTracking_Id?   EventTrackingId     = null,
                       TimeSpan?           RequestTimeout      = null,
                       CancellationToken   CancellationToken   = default)

        {

            #region Initial checks

            if (ReserveUntil.HasValue && ReserveUntil.Value <= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)
                throw new ArgumentException("The given reservation end time must be after than the current time!");

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;
            RequestTimeout  ??= this.RequestTimeout;

            HTTPResponse<SelectEVSEResponse>? result = null;

            #endregion

            #region Send OnSelectEVSERequest event

            try
            {

                OnSelectEVSERequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                            Timestamp.Value,
                                            this,
                                            Description,
                                            EventTrackingId,
                                            EVSEId,
                                            ContractId,
                                            ReserveUntil,
                                            RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnSelectEVSERequest));
            }

            #endregion


            var ep      = _EndpointInfos.Get(EVSEId);
            var Request = new SelectEVSERequest(EVSEId,
                                                ContractId,
                                                ReserveUntil);


            using (var _OCHPClient = new SOAPClient(ep.First().URL,
                                                    VirtualHostname:             VirtualHostname,
                                                    RemoteCertificateValidator:  RemoteCertificateValidator,
                                                    ClientCertificateSelector:   ClientCertificateSelector,
                                                    HTTPUserAgent:               HTTPUserAgent,
                                                    RequestTimeout:              RequestTimeout,
                                                    DNSClient:                   DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(Request.ToXML()),
                                                 "SelectEVSERequest",
                                                 RequestLogDelegate:   OnSelectEVSESOAPRequest,
                                                 ResponseLogDelegate:  OnSelectEVSESOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 RequestTimeout:       RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, SelectEVSEResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return HTTPResponse<SelectEVSEResponse>.IsFault(
                                                                httpresponse,
                                                                new SelectEVSEResponse(
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

                                                     return HTTPResponse<SelectEVSEResponse>.IsFault(
                                                                httpresponse,
                                                                new SelectEVSEResponse(
                                                                    Request,
                                                                    Result.Server(
                                                                         httpresponse.HTTPStatusCode.ToString() +
                                                                         " => " +
                                                                         httpresponse.HTTPBody.      ToUTF8String()
                                                                    )
                                                                )
                                                            );

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<SelectEVSEResponse>.ExceptionThrown(new SelectEVSEResponse(
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
                result = HTTPResponse<SelectEVSEResponse>.OK(new SelectEVSEResponse(Request, Result.OK("Nothing to upload!")));


            _EndpointInfos.Add(result.Content.DirectId, ep);

            #region Send OnSelectEVSEResponse event

            try
            {

                OnSelectEVSEResponse?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                             Timestamp.Value,
                                             this,
                                             Description,
                                             EventTrackingId,
                                             EVSEId,
                                             ContractId,
                                             ReserveUntil,
                                             RequestTimeout,
                                             result.Content,
                                             org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnSelectEVSEResponse));
            }

            #endregion


            return result;

        }

        #endregion

        #region ControlEVSE(...)

        /// <summary>
        /// Control a direct charging process.
        /// </summary>
        /// <param name="DirectId">The unique session identification of the direct charging process to be controlled.</param>
        /// <param name="Operation">The operation to be performed for the selected charge point.</param>
        /// <param name="MaxPower">Maximum authorised power in kW.</param>
        /// <param name="MaxCurrent">Maximum authorised current in A.</param>
        /// <param name="OnePhase">Marks an AC-charging session to be limited to one-phase charging.</param>
        /// <param name="MaxEnergy">Maximum authorised energy in kWh.</param>
        /// <param name="MinEnergy">Minimum required energy in kWh.</param>
        /// <param name="Departure">Scheduled time of departure.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<ControlEVSEResponse>>

            ControlEVSE(Direct_Id           DirectId,
                        DirectOperations    Operation,
                        Single?             MaxPower            = null,
                        Single?             MaxCurrent          = null,
                        Boolean?            OnePhase            = null,
                        Single?             MaxEnergy           = null,
                        Single?             MinEnergy           = null,
                        DateTime?           Departure           = null,

                        DateTime?           Timestamp           = null,
                        EventTracking_Id?   EventTrackingId     = null,
                        TimeSpan?           RequestTimeout      = null,
                        CancellationToken   CancellationToken   = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;
            RequestTimeout  ??= this.RequestTimeout;

            HTTPResponse<ControlEVSEResponse>? result = null;

            #endregion

            #region Send OnControlEVSERequest event

            try
            {

                OnControlEVSERequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                             Timestamp.Value,
                                             this,
                                             Description,
                                             EventTrackingId,
                                             DirectId,
                                             Operation,
                                             MaxPower,
                                             MaxCurrent,
                                             OnePhase,
                                             MaxEnergy,
                                             MinEnergy,
                                             Departure,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnSelectEVSERequest));
            }

            #endregion


            var ep      = _EndpointInfos.Get(DirectId);
            var Request = new ControlEVSERequest(DirectId,
                                                 Operation,
                                                 MaxPower,
                                                 MaxCurrent,
                                                 OnePhase,
                                                 MaxEnergy,
                                                 MinEnergy,
                                                 Departure);


            using (var _OCHPClient = new SOAPClient(ep.First().URL,
                                                    VirtualHostname:             VirtualHostname,
                                                    RemoteCertificateValidator:  RemoteCertificateValidator,
                                                    ClientCertificateSelector:   ClientCertificateSelector,
                                                    HTTPUserAgent:               HTTPUserAgent,
                                                    RequestTimeout:              RequestTimeout,
                                                    DNSClient:                   DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(Request.ToXML()),
                                                 "ControlEVSERequest",
                                                 RequestLogDelegate:   OnControlEVSESOAPRequest,
                                                 ResponseLogDelegate:  OnControlEVSESOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 RequestTimeout:       RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, ControlEVSEResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return HTTPResponse<ControlEVSEResponse>.IsFault(
                                                                httpresponse,
                                                                new ControlEVSEResponse(
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

                                                     return HTTPResponse<ControlEVSEResponse>.IsFault(
                                                                httpresponse,
                                                                new ControlEVSEResponse(
                                                                    Request,
                                                                    Result.Server(
                                                                         httpresponse.HTTPStatusCode.ToString() +
                                                                         " => " +
                                                                         httpresponse.HTTPBody.      ToUTF8String()
                                                                    )
                                                                )
                                                            );

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<ControlEVSEResponse>.ExceptionThrown(new ControlEVSEResponse(
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
                result = HTTPResponse<ControlEVSEResponse>.OK(new ControlEVSEResponse(Request, Result.OK("Nothing to upload!")));


            #region Send OnControlEVSEResponse event

            try
            {

                OnControlEVSEResponse?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                              Timestamp.Value,
                                              this,
                                              Description,
                                              EventTrackingId,
                                              DirectId,
                                              Operation,
                                              MaxPower,
                                              MaxCurrent,
                                              OnePhase,
                                              MaxEnergy,
                                              MinEnergy,
                                              Departure,
                                              RequestTimeout,
                                              result.Content,
                                              org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnControlEVSEResponse));
            }

            #endregion


            return result;

        }

        #endregion

        #region ReleaseEVSE(...)

        /// <summary>
        /// Release an EVSE and stop a direct charging process.
        /// </summary>
        /// <param name="DirectId">The session identification of the direct charging process to be released.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<ReleaseEVSEResponse>>

            ReleaseEVSE(Direct_Id          DirectId,

                        DateTime?          Timestamp           = null,
                        EventTracking_Id?  EventTrackingId     = null,
                        TimeSpan?          RequestTimeout      = null,
                        CancellationToken  CancellationToken   = default)

        {

            #region Initial checks

            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;
            RequestTimeout  ??= this.RequestTimeout;

            HTTPResponse<ReleaseEVSEResponse>? result = null;

            #endregion

            #region Send OnReleaseEVSERequest event

            try
            {

                OnReleaseEVSERequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                             Timestamp.Value,
                                             this,
                                             Description,
                                             EventTrackingId,
                                             DirectId,
                                             RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnReleaseEVSERequest));
            }

            #endregion


            var ep      = _EndpointInfos.Get(DirectId);
            var Request = new ReleaseEVSERequest(DirectId);


            using (var _OCHPClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                    VirtualHostname:             VirtualHostname,
                                                    RemoteCertificateValidator:  RemoteCertificateValidator,
                                                    ClientCertificateSelector:   ClientCertificateSelector,
                                                    HTTPUserAgent:               HTTPUserAgent,
                                                    RequestTimeout:              RequestTimeout,
                                                    DNSClient:                   DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(Request.ToXML()),
                                                 "ReleaseEVSERequest",
                                                 RequestLogDelegate:   OnReleaseEVSESOAPRequest,
                                                 ResponseLogDelegate:  OnReleaseEVSESOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 RequestTimeout:       RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, ReleaseEVSEResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return HTTPResponse<ReleaseEVSEResponse>.IsFault(
                                                                httpresponse,
                                                                new ReleaseEVSEResponse(
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

                                                     return HTTPResponse<ReleaseEVSEResponse>.IsFault(
                                                                httpresponse,
                                                                new ReleaseEVSEResponse(
                                                                    Request,
                                                                    Result.Server(
                                                                         httpresponse.HTTPStatusCode.ToString() +
                                                                         " => " +
                                                                         httpresponse.HTTPBody.      ToUTF8String()
                                                                    )
                                                                )
                                                            );

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<ReleaseEVSEResponse>.ExceptionThrown(new ReleaseEVSEResponse(
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
                result = HTTPResponse<ReleaseEVSEResponse>.OK(new ReleaseEVSEResponse(Request, Result.OK("Nothing to upload!")));


            _EndpointInfos.Delete(DirectId);


            #region Send OnReleaseEVSEResponse event

            try
            {

                OnReleaseEVSEResponse?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                              Timestamp.Value,
                                              this,
                                              Description,
                                              EventTrackingId,
                                              DirectId,
                                              RequestTimeout,
                                              result.Content,
                                              org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnReleaseEVSEResponse));
            }

            #endregion


            return result;

        }

        #endregion

        #region GetEVSEStatus(...)

        /// <summary>
        /// Get the status of the given EVSEs directly from the charge point operator.
        /// </summary>
        /// <param name="EVSEIds">An enumeration of EVSE identifications.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<GetEVSEStatusResponse>>

            GetEVSEStatus(IEnumerable<EVSE_Id>  EVSEIds,

                          DateTime?             Timestamp           = null,
                          EventTracking_Id?     EventTrackingId     = null,
                          TimeSpan?             RequestTimeout      = null,
                          CancellationToken     CancellationToken   = default)

        {

            #region Initial checks

            if (!EVSEIds.Any())
                throw new ArgumentNullException(nameof(EVSEIds),  "The given enumeration of EVSE identifications must not be empty!");


            Timestamp       ??= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            EventTrackingId ??= EventTracking_Id.New;
            RequestTimeout  ??= this.RequestTimeout;

            HTTPResponse<GetEVSEStatusResponse>? result = null;

            #endregion

            #region Send OnGetEVSEStatusRequest event

            try
            {

                OnGetEVSEStatusRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                               Timestamp.Value,
                                               this,
                                               Description,
                                               EventTrackingId,
                                               EVSEIds,
                                               RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetEVSEStatusRequest));
            }

            #endregion


            var Request = new GetEVSEStatusRequest(EVSEIds);

            using (var _OCHPClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                    VirtualHostname:             VirtualHostname,
                                                    RemoteCertificateValidator:  RemoteCertificateValidator,
                                                    ClientCertificateSelector:   ClientCertificateSelector,
                                                    HTTPUserAgent:               HTTPUserAgent,
                                                    RequestTimeout:              RequestTimeout,
                                                    DNSClient:                   DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(Request.ToXML()),
                                                 "DirectEvseStatusRequest",
                                                 RequestLogDelegate:   OnGetEVSEStatusSOAPRequest,
                                                 ResponseLogDelegate:  OnGetEVSEStatusSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 RequestTimeout:       RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, GetEVSEStatusResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return HTTPResponse<GetEVSEStatusResponse>.IsFault(
                                                                httpresponse,
                                                                new GetEVSEStatusResponse(
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

                                                     return HTTPResponse<GetEVSEStatusResponse>.IsFault(
                                                                httpresponse,
                                                                new GetEVSEStatusResponse(
                                                                    Request,
                                                                    Result.Server(
                                                                         httpresponse.HTTPStatusCode.ToString() +
                                                                         " => " +
                                                                         httpresponse.HTTPBody.      ToUTF8String()
                                                                    )
                                                                )
                                                            );

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<GetEVSEStatusResponse>.ExceptionThrown(new GetEVSEStatusResponse(
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
                result = HTTPResponse<GetEVSEStatusResponse>.OK(new GetEVSEStatusResponse(Request, Result.OK("Nothing to upload!")));


            #region Send OnGetEVSEStatusResponse event

            try
            {

                OnGetEVSEStatusResponse?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                Timestamp.Value,
                                                this,
                                                Description,
                                                EventTrackingId,
                                                EVSEIds,
                                                RequestTimeout,
                                                result.Content,
                                                org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetEVSEStatusResponse));
            }

            #endregion


            return result;

        }

        #endregion


        #region ReportDiscrepancy(...)

        /// <summary>
        /// Report a discrepancy or any issue concerning the data, compatibility or status
        /// of an EVSE to the charge point operator.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification affected by this report.</param>
        /// <param name="Report">Textual or generated report of the discrepancy.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<ReportDiscrepancyResponse>>

            ReportDiscrepancy(EVSE_Id            EVSEId,
                              String             Report,

                              DateTime?          Timestamp           = null,
                              EventTracking_Id?  EventTrackingId     = null,
                              TimeSpan?          RequestTimeout      = null,
                              CancellationToken  CancellationToken   = default)

        {

            #region Initial checks

            if (Report == null || Report.Trim().IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Report),  "The given EVSE report must not be null or empty!");


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            EventTrackingId ??= EventTracking_Id.New;
            RequestTimeout  ??= this.RequestTimeout;

            HTTPResponse<ReportDiscrepancyResponse>? result = null;

            #endregion

            #region Send OnReportDiscrepancyRequest event

            try
            {

                OnReportDiscrepancyRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                   Timestamp.Value,
                                                   this,
                                                   Description,
                                                   EventTrackingId,
                                                   EVSEId,
                                                   Report,
                                                   RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnReportDiscrepancyRequest));
            }

            #endregion


            var Request = new ReportDiscrepancyRequest(EVSEId,
                                                       Report);


            using (var _OCHPClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                    VirtualHostname:             VirtualHostname,
                                                    RemoteCertificateValidator:  RemoteCertificateValidator,
                                                    ClientCertificateSelector:   ClientCertificateSelector,
                                                    HTTPUserAgent:               HTTPUserAgent,
                                                    RequestTimeout:              RequestTimeout,
                                                    DNSClient:                   DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(Request.ToXML()),
                                                 "ReportDiscrepancyRequest",
                                                 RequestLogDelegate:   OnReportDiscrepancySOAPRequest,
                                                 ResponseLogDelegate:  OnReportDiscrepancySOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 RequestTimeout:       RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, ReportDiscrepancyResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return HTTPResponse<ReportDiscrepancyResponse>.IsFault(
                                                                httpresponse,
                                                                new ReportDiscrepancyResponse(
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

                                                     return HTTPResponse<ReportDiscrepancyResponse>.IsFault(
                                                                httpresponse,
                                                                new ReportDiscrepancyResponse(
                                                                    Request,
                                                                    Result.Server(
                                                                         httpresponse.HTTPStatusCode.ToString() +
                                                                         " => " +
                                                                         httpresponse.HTTPBody.      ToUTF8String()
                                                                    )
                                                                )
                                                            );

                                                 },

                                                 #endregion

                                                 #region OnException

                                                 OnException: (timestamp, sender, exception) => {

                                                     SendException(timestamp, sender, exception);

                                                     return HTTPResponse<ReportDiscrepancyResponse>.ExceptionThrown(new ReportDiscrepancyResponse(
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
                result = HTTPResponse<ReportDiscrepancyResponse>.OK(new ReportDiscrepancyResponse(Request, Result.OK("Nothing to upload!")));


            #region Send OnReportDiscrepancyResponse event

            try
            {

                OnReportDiscrepancyResponse?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                    Timestamp.Value,
                                                    this,
                                                    Description,
                                                    EventTrackingId,
                                                    EVSEId,
                                                    Report,
                                                    RequestTimeout,
                                                    result.Content,
                                                    org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnReportDiscrepancyResponse));
            }

            #endregion


            return result;

        }

        #endregion

        #region GetInformProvider(...)

        /// <summary>
        /// Request an inform provider message.
        /// </summary>
        /// <param name="DirectId">The session identification of the direct charging process.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public async Task<HTTPResponse<GetInformProviderResponse>>

            GetInformProvider(Direct_Id           DirectId,

                              DateTime?           Timestamp           = null,
                              EventTracking_Id?   EventTrackingId     = null,
                              TimeSpan?           RequestTimeout      = null,
                              CancellationToken   CancellationToken   = default)

        {

            #region Initial checks

            if (DirectId == null)
                throw new ArgumentNullException(nameof(DirectId),  "The given direct charging session identification must not be null!");


            if (!Timestamp.HasValue)
                Timestamp = org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;

            if (EventTrackingId == null)
                EventTrackingId = EventTracking_Id.New;

            if (!RequestTimeout.HasValue)
                RequestTimeout = this.RequestTimeout;


            HTTPResponse<GetInformProviderResponse> result = null;

            #endregion

            #region Send OnGetInformProviderRequest event

            try
            {

                OnGetInformProviderRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                   Timestamp.Value,
                                                   this,
                                                   Description,
                                                   EventTrackingId,
                                                   DirectId,
                                                   RequestTimeout);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetInformProviderRequest));
            }

            #endregion


            var ep      = _EndpointInfos.Get(DirectId);
            var Request = new GetInformProviderRequest(DirectId);


            using (var _OCHPClient = new SOAPClient(RemoteURL:                   RemoteURL,
                                                    VirtualHostname:             VirtualHostname,
                                                    RemoteCertificateValidator:  RemoteCertificateValidator,
                                                    ClientCertificateSelector:   ClientCertificateSelector,
                                                    HTTPUserAgent:               HTTPUserAgent,
                                                    RequestTimeout:              RequestTimeout,
                                                    DNSClient:                   DNSClient))
            {

                result = await _OCHPClient.Query(SOAP.Encapsulation(Request.ToXML()),
                                                 "InformProviderRequest",
                                                 RequestLogDelegate:   OnGetInformProviderSOAPRequest,
                                                 ResponseLogDelegate:  OnGetInformProviderSOAPResponse,
                                                 CancellationToken:    CancellationToken,
                                                 EventTrackingId:      EventTrackingId,
                                                 RequestTimeout:       RequestTimeout,

                                                 #region OnSuccess

                                                 OnSuccess: XMLResponse => XMLResponse.ConvertContent(Request, GetInformProviderResponse.Parse),

                                                 #endregion

                                                 #region OnSOAPFault

                                                 OnSOAPFault: (timestamp, soapclient, httpresponse) => {

                                                     SendSOAPError(timestamp, this, httpresponse.Content);

                                                     return HTTPResponse<GetInformProviderResponse>.IsFault(
                                                                httpresponse,
                                                                GetInformProviderResponse.Format(
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

                                                     return HTTPResponse<GetInformProviderResponse>.IsFault(
                                                                httpresponse,
                                                                GetInformProviderResponse.Server(
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

                                                     return HTTPResponse<GetInformProviderResponse>.ExceptionThrown(GetInformProviderResponse.Format(
                                                                                                                        Request,
                                                                                                                        exception.Message +
                                                                                                                        " => " +
                                                                                                                        exception.StackTrace),
                                                                                                                    exception);

                                                 }

                                                 #endregion

                                                );

            }

            if (result == null)
                result = HTTPResponse<GetInformProviderResponse>.OK(GetInformProviderResponse.OK(Request, "Nothing to upload!"));


            _EndpointInfos.Delete(DirectId);


            #region Send OnGetInformProviderResponse event

            try
            {

                OnGetInformProviderResponse?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                    Timestamp.Value,
                                                    this,
                                                    Description,
                                                    EventTrackingId,
                                                    DirectId,
                                                    RequestTimeout,
                                                    result.Content,
                                                    org.GraphDefined.Vanaheimr.Illias.Timestamp.Now - Timestamp.Value);

            }
            catch (Exception e)
            {
                DebugX.LogException(e, nameof(EMPClient) + "." + nameof(OnGetInformProviderResponse));
            }

            #endregion


            return result;

        }

        #endregion


    }

}
