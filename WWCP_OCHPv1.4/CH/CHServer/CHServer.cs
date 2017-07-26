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
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

using org.GraphDefined.WWCP.OCHPv1_4.CPO;
using org.GraphDefined.WWCP.OCHPv1_4.EMP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CH
{

    /// <summary>
    /// An OCHP CH HTTP/SOAP/XML server.
    /// </summary>
    public class CHServer : ASOAPServer
    {

        #region Data

        /// <summary>
        /// The default HTTP/SOAP/XML server name.
        /// </summary>
        public new const           String           DefaultHTTPServerName  = "GraphDefined OCHP " + Version.Number + " HTTP/SOAP/XML Clearing House API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort           DefaultHTTPServerPort  = new IPPort(2600);

        /// <summary>
        /// The default HTTP/SOAP/XML server URI prefix.
        /// </summary>
        public new const           String           DefaultURIPrefix       = "";

        /// <summary>
        /// The default HTTP/SOAP/XML content type.
        /// </summary>
        public new static readonly HTTPContentType  DefaultContentType     = HTTPContentType.XMLTEXT_UTF8;

        /// <summary>
        /// The default request timeout.
        /// </summary>
        public new static readonly TimeSpan         DefaultRequestTimeout  = TimeSpan.FromMinutes(1);

        #endregion

        #region Events

        // Shared events...

        #region OnAddServiceEndpoints

        /// <summary>
        /// An event sent whenever an add service endpoints SOAP request was received.
        /// </summary>
        public event RequestLogHandler              OnAddServiceEndpointsSOAPRequest;

        /// <summary>
        /// An event sent whenever an add service endpoints SOAP response was sent.
        /// </summary>
        public event AccessLogHandler               OnAddServiceEndpointsSOAPResponse;

        /// <summary>
        /// An event sent whenever an add service endpoints request was received.
        /// </summary>
        public event OnAddServiceEndpointsDelegate  OnAddServiceEndpointsRequest;

        #endregion

        #region OnGetServiceEndpoints

        /// <summary>
        /// An event sent whenever a get service endpoints SOAP request was received.
        /// </summary>
        public event RequestLogHandler              OnGetServiceEndpointsSOAPRequest;

        /// <summary>
        /// An event sent whenever a get service endpoints SOAP response was sent.
        /// </summary>
        public event AccessLogHandler               OnGetServiceEndpointsSOAPResponse;

        /// <summary>
        /// An event sent whenever a get service endpoints request was received.
        /// </summary>
        public event OnGetServiceEndpointsDelegate  OnGetServiceEndpointsRequest;

        #endregion


        // CPO events...

        #region OnAddCDRs

        /// <summary>
        /// An event sent whenever an add charge detail records SOAP request was received.
        /// </summary>
        public event RequestLogHandler  OnAddCDRsSOAPRequest;

        /// <summary>
        /// An event sent whenever an add charge detail records SOAP response was sent.
        /// </summary>
        public event AccessLogHandler   OnAddCDRsSOAPResponse;

        /// <summary>
        /// An event sent whenever an add charge detail records request was received.
        /// </summary>
        public event OnAddCDRsDelegate  OnAddCDRsRequest;

        #endregion

        #region OnCheckCDRs

        /// <summary>
        /// An event sent whenever a check charge detail records SOAP request was received.
        /// </summary>
        public event RequestLogHandler    OnCheckCDRsSOAPRequest;

        /// <summary>
        /// An event sent whenever a check charge detail records SOAP response was sent.
        /// </summary>
        public event AccessLogHandler     OnCheckCDRsSOAPResponse;

        /// <summary>
        /// An event sent whenever a check charge detail records request was received.
        /// </summary>
        public event OnCheckCDRsDelegate  OnCheckCDRsRequest;

        #endregion

        #region OnGetRoamingAuthorisationList

        /// <summary>
        /// An event sent whenever a get roaming authorisation list SOAP request was received.
        /// </summary>
        public event RequestLogHandler                      OnGetRoamingAuthorisationListSOAPRequest;

        /// <summary>
        /// An event sent whenever a get roaming authorisation list SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                       OnGetRoamingAuthorisationListSOAPResponse;

        /// <summary>
        /// An event sent whenever a get roaming authorisation list request was received.
        /// </summary>
        public event OnGetRoamingAuthorisationListDelegate  OnGetRoamingAuthorisationListRequest;

        #endregion

        #region OnGetRoamingAuthorisationListUpdates

        /// <summary>
        /// An event sent whenever a get roaming authorisation list updates SOAP request was received.
        /// </summary>
        public event RequestLogHandler                             OnGetRoamingAuthorisationListUpdatesSOAPRequest;

        /// <summary>
        /// An event sent whenever a get roaming authorisation list updates SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                              OnGetRoamingAuthorisationListUpdatesSOAPResponse;

        /// <summary>
        /// An event sent whenever a get roaming authorisation list updates request was received.
        /// </summary>
        public event OnGetRoamingAuthorisationListUpdatesDelegate  OnGetRoamingAuthorisationListUpdatesRequest;

        #endregion

        #region OnGetSingleRoamingAuthorisation

        /// <summary>
        /// An event sent whenever a get single roaming authorisation SOAP request was received.
        /// </summary>
        public event RequestLogHandler                        OnGetSingleRoamingAuthorisationSOAPRequest;

        /// <summary>
        /// An event sent whenever a get single roaming authorisation SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                         OnGetSingleRoamingAuthorisationSOAPResponse;

        /// <summary>
        /// An event sent whenever a get single roaming authorisation request was received.
        /// </summary>
        public event OnGetSingleRoamingAuthorisationDelegate  OnGetSingleRoamingAuthorisationRequest;

        #endregion

        #region OnSetChargePointList

        /// <summary>
        /// An event sent whenever a set charge point list SOAP request was received.
        /// </summary>
        public event RequestLogHandler             OnSetChargePointListSOAPRequest;

        /// <summary>
        /// An event sent whenever a set charge point list SOAP response was sent.
        /// </summary>
        public event AccessLogHandler              OnSetChargePointListSOAPResponse;

        /// <summary>
        /// An event sent whenever a set charge point list request was received.
        /// </summary>
        public event OnSetChargePointListDelegate  OnSetChargePointListRequest;

        #endregion

        #region OnUpdateChargePointList

        /// <summary>
        /// An event sent whenever a update charge point list SOAP request was received.
        /// </summary>
        public event RequestLogHandler                OnUpdateChargePointListSOAPRequest;

        /// <summary>
        /// An event sent whenever a update charge point list SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                 OnUpdateChargePointListSOAPResponse;

        /// <summary>
        /// An event sent whenever a update charge point list request was received.
        /// </summary>
        public event OnUpdateChargePointListDelegate  OnUpdateChargePointListRequest;

        #endregion

        #region OnUpdateStatus

        /// <summary>
        /// An event sent whenever an update EVSE and/or parking status SOAP request was received.
        /// </summary>
        public event RequestLogHandler       OnUpdateStatusSOAPRequest;

        /// <summary>
        /// An event sent whenever an update EVSE and/or parking status SOAP response was sent.
        /// </summary>
        public event AccessLogHandler        OnUpdateStatusSOAPResponse;

        /// <summary>
        /// An event sent whenever an update EVSE and/or parking status request was received.
        /// </summary>
        public event OnUpdateStatusDelegate  OnUpdateStatusRequest;

        #endregion

        #region OnUpdateTariffs

        /// <summary>
        /// An event sent whenever an update charging tariffs SOAP request was received.
        /// </summary>
        public event RequestLogHandler        OnUpdateTariffsSOAPRequest;

        /// <summary>
        /// An event sent whenever an update charging tariffs SOAP response was sent.
        /// </summary>
        public event AccessLogHandler         OnUpdateTariffsSOAPResponse;

        /// <summary>
        /// An event sent whenever an update charging tariffs request was received.
        /// </summary>
        public event OnUpdateTariffsDelegate  OnUpdateTariffsRequest;

        #endregion


        // EMP events...

        #region OnGetCDRs

        /// <summary>
        /// An event sent whenever a get charge detail records SOAP request was received.
        /// </summary>
        public event RequestLogHandler  OnGetCDRsSOAPRequest;

        /// <summary>
        /// An event sent whenever a get charge detail records SOAP response was sent.
        /// </summary>
        public event AccessLogHandler   OnGetCDRsSOAPResponse;

        /// <summary>
        /// An event sent whenever a get charge detail records request was received.
        /// </summary>
        public event OnGetCDRsDelegate  OnGetCDRsRequest;

        #endregion

        #region OnConfirmCDRs

        /// <summary>
        /// An event sent whenever a confirm charge detail records SOAP request was received.
        /// </summary>
        public event RequestLogHandler      OnConfirmCDRsSOAPRequest;

        /// <summary>
        /// An event sent whenever a confirm charge detail records SOAP response was sent.
        /// </summary>
        public event AccessLogHandler       OnConfirmCDRsSOAPResponse;

        /// <summary>
        /// An event sent whenever a confirm charge detail records request was received.
        /// </summary>
        public event OnConfirmCDRsDelegate  OnConfirmCDRsRequest;

        #endregion

        #region OnGetChargePointList

        /// <summary>
        /// An event sent whenever a get charge point list SOAP request was received.
        /// </summary>
        public event RequestLogHandler             OnGetChargePointListSOAPRequest;

        /// <summary>
        /// An event sent whenever a get charge point list SOAP response was sent.
        /// </summary>
        public event AccessLogHandler              OnGetChargePointListSOAPResponse;

        /// <summary>
        /// An event sent whenever a get charge point list request was received.
        /// </summary>
        public event OnGetChargePointListDelegate  OnGetChargePointListRequest;

        #endregion

        #region OnGetChargePointListUpdates

        /// <summary>
        /// An event sent whenever a get charge point list updates SOAP request was received.
        /// </summary>
        public event RequestLogHandler                    OnGetChargePointListUpdatesSOAPRequest;

        /// <summary>
        /// An event sent whenever a get charge point list updates SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                     OnGetChargePointListUpdatesSOAPResponse;

        /// <summary>
        /// An event sent whenever a get charge point list updates request was received.
        /// </summary>
        public event OnGetChargePointListUpdatesDelegate  OnGetChargePointListUpdatesRequest;

        #endregion

        #region OnGetStatus

        /// <summary>
        /// An event sent whenever a get EVSE and parking status SOAP request was received.
        /// </summary>
        public event RequestLogHandler    OnGetStatusSOAPRequest;

        /// <summary>
        /// An event sent whenever a get EVSE and parking status SOAP response was sent.
        /// </summary>
        public event AccessLogHandler     OnGetStatusSOAPResponse;

        /// <summary>
        /// An event sent whenever a get EVSE and parking status request was received.
        /// </summary>
        public event OnGetStatusDelegate  OnGetStatusRequest;

        #endregion

        #region OnGetTariffUpdates

        /// <summary>
        /// An event sent whenever a get charging tariffs updates SOAP request was received.
        /// </summary>
        public event RequestLogHandler           OnGetTariffUpdatesSOAPRequest;

        /// <summary>
        /// An event sent whenever a get charging tariffs updates SOAP response was sent.
        /// </summary>
        public event AccessLogHandler            OnGetTariffUpdatesSOAPResponse;

        /// <summary>
        /// An event sent whenever a get charging tariffs updates request was received.
        /// </summary>
        public event OnGetTariffUpdatesDelegate  OnGetTariffUpdatesRequest;

        #endregion

        #region OnSetRoamingAuthorisationList

        /// <summary>
        /// An event sent whenever a set roaming authorisation infos SOAP request was received.
        /// </summary>
        public event RequestLogHandler                      OnSetRoamingAuthorisationListSOAPRequest;

        /// <summary>
        /// An event sent whenever a set roaming authorisation infos SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                       OnSetRoamingAuthorisationListSOAPResponse;

        /// <summary>
        /// An event sent whenever a set roaming authorisation infos request was received.
        /// </summary>
        public event OnSetRoamingAuthorisationListDelegate  OnSetRoamingAuthorisationListRequest;

        #endregion

        #region OnUpdateRoamingAuthorisationList

        /// <summary>
        /// An event sent whenever an update roaming authorisation infos SOAP request was received.
        /// </summary>
        public event RequestLogHandler                         OnUpdateRoamingAuthorisationListSOAPRequest;

        /// <summary>
        /// An event sent whenever an update roaming authorisation infos SOAP response was sent.
        /// </summary>
        public event AccessLogHandler                          OnUpdateRoamingAuthorisationListSOAPResponse;

        /// <summary>
        /// An event sent whenever an update roaming authorisation infos request was received.
        /// </summary>
        public event OnUpdateRoamingAuthorisationListDelegate  OnUpdateRoamingAuthorisationListRequest;

        #endregion

        #endregion

        #region Constructor(s)

        #region CHServer(HTTPServerName, TCPPort = default, URIPrefix = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OCHP HTTP/SOAP/XML CH Server API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public CHServer(String          HTTPServerName           = DefaultHTTPServerName,
                        IPPort          TCPPort                  = null,
                        String          URIPrefix                = DefaultURIPrefix,
                        HTTPContentType ContentType              = null,
                        Boolean         RegisterHTTPRootService  = true,
                        DNSClient       DNSClient                = null,
                        Boolean         AutoStart                = false)

            : base(HTTPServerName.IsNotNullOrEmpty() ? HTTPServerName : DefaultHTTPServerName,
                   TCPPort     ?? DefaultHTTPServerPort,
                   URIPrefix   ?? DefaultURIPrefix,
                   ContentType ?? DefaultContentType,
                   RegisterHTTPRootService,
                   DNSClient,
                   AutoStart: false)

        {

            RegisterURITemplates();

            if (AutoStart)
                Start();

        }

        #endregion

        #region CHServer(SOAPServer, URIPrefix = default)

        /// <summary>
        /// Use the given HTTP server for the OCHP HTTP/SOAP/XML CH Server API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        public CHServer(SOAPServer  SOAPServer,
                        String      URIPrefix  = DefaultURIPrefix)

            : base(SOAPServer,
                   URIPrefix ?? DefaultURIPrefix)

        {

            RegisterURITemplates();

        }

        #endregion

        #endregion


        #region RegisterURITemplates()

        /// <summary>
        /// Register all URI templates for this SOAP API.
        /// </summary>
        protected void RegisterURITemplates()
        {

            // Shared messages...

            #region / - AddServiceEndpointsRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "AddServiceEndpointsRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "AddServiceEndpointsRequest").FirstOrDefault(),
                                            async (Request, AddServiceEndpointsXML) => {

                #region Send OnAddServiceEndpointsSOAPRequest event

                try
                {

                    OnAddServiceEndpointsSOAPRequest?.Invoke(DateTime.UtcNow,
                                                             this.SOAPServer,
                                                             Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnAddServiceEndpointsSOAPRequest));
                }

                #endregion


                var _AddServiceEndpointsRequest = AddServiceEndpointsRequest.Parse(AddServiceEndpointsXML);

                AddServiceEndpointsResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnAddServiceEndpointsRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnAddServiceEndpointsDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _AddServiceEndpointsRequest.OperatorEndpoints,
                                           _AddServiceEndpointsRequest.ProviderEndpoints,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = AddServiceEndpointsResponse.Server(_AddServiceEndpointsRequest, "Could not process the incoming AddServiceEndpoints request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnAddServiceEndpointsHTTPResponse event

                try
                {

                    OnAddServiceEndpointsSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                              this.SOAPServer,
                                                              Request,
                                                              HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnAddServiceEndpointsSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - GetServiceEndpointsRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "GetServiceEndpointsRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "GetServiceEndpointsRequest").FirstOrDefault(),
                                            async (Request, GetServiceEndpointsXML) => {

                #region Send OnGetServiceEndpointsSOAPRequest event

                try
                {

                    OnGetServiceEndpointsSOAPRequest?.Invoke(DateTime.UtcNow,
                                                             this.SOAPServer,
                                                             Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetServiceEndpointsSOAPRequest));
                }

                #endregion


                var _GetServiceEndpointsRequest = GetServiceEndpointsRequest.Parse(GetServiceEndpointsXML);

                GetServiceEndpointsResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetServiceEndpointsRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetServiceEndpointsDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetServiceEndpointsResponse.Server(_GetServiceEndpointsRequest, "Could not process the incoming GetServiceEndpoints request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetServiceEndpointsHTTPResponse event

                try
                {

                    OnGetServiceEndpointsSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                              this.SOAPServer,
                                                              Request,
                                                              HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetServiceEndpointsSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion


            // CPO messages...

            #region / - AddCDRsRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "AddCDRsRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "AddCDRsRequest").FirstOrDefault(),
                                            async (Request, AddCDRsXML) => {

                #region Send OnAddCDRsSOAPRequest event

                try
                {

                    OnAddCDRsSOAPRequest?.Invoke(DateTime.UtcNow,
                                                 this.SOAPServer,
                                                 Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnAddCDRsSOAPRequest));
                }

                #endregion


                var _AddCDRsRequest = AddCDRsRequest.Parse(AddCDRsXML);

                AddCDRsResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnAddCDRsRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnAddCDRsDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _AddCDRsRequest.CDRInfos,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = AddCDRsResponse.Server(_AddCDRsRequest, "Could not process the incoming AddCDRs request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnAddCDRsHTTPResponse event

                try
                {

                    OnAddCDRsSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                  this.SOAPServer,
                                                  Request,
                                                  HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnAddCDRsSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - CheckCDRsRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "CheckCDRsRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "CheckCDRsRequest").FirstOrDefault(),
                                            async (Request, CheckCDRsXML) => {

                #region Send OnCheckCDRsSOAPRequest event

                try
                {

                    OnCheckCDRsSOAPRequest?.Invoke(DateTime.UtcNow,
                                                 this.SOAPServer,
                                                 Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnCheckCDRsSOAPRequest));
                }

                #endregion


                var _CheckCDRsRequest = CheckCDRsRequest.Parse(CheckCDRsXML);

                CheckCDRsResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnCheckCDRsRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnCheckCDRsDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _CheckCDRsRequest.CDRStatus,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = CheckCDRsResponse.Server(_CheckCDRsRequest, "Could not process the incoming CheckCDRs request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnCheckCDRsHTTPResponse event

                try
                {

                    OnCheckCDRsSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                    this.SOAPServer,
                                                    Request,
                                                    HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnCheckCDRsSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - GetRoamingAuthorisationListRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "GetRoamingAuthorisationListRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "GetRoamingAuthorisationListRequest").FirstOrDefault(),
                                            async (Request, GetRoamingAuthorisationListXML) => {

                #region Send OnGetRoamingAuthorisationListSOAPRequest event

                try
                {

                    OnGetRoamingAuthorisationListSOAPRequest?.Invoke(DateTime.UtcNow,
                                                                     this.SOAPServer,
                                                                     Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetRoamingAuthorisationListSOAPRequest));
                }

                #endregion


                var _GetRoamingAuthorisationListRequest = GetRoamingAuthorisationListRequest.Parse(GetRoamingAuthorisationListXML);

                GetRoamingAuthorisationListResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetRoamingAuthorisationListRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetRoamingAuthorisationListDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetRoamingAuthorisationListResponse.Server(_GetRoamingAuthorisationListRequest, "Could not process the incoming GetRoamingAuthorisationList request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetRoamingAuthorisationListHTTPResponse event

                try
                {

                    OnGetRoamingAuthorisationListSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                      this.SOAPServer,
                                                                      Request,
                                                                      HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetRoamingAuthorisationListSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - GetRoamingAuthorisationListUpdatesRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "GetRoamingAuthorisationListUpdatesRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "GetRoamingAuthorisationListUpdatesRequest").FirstOrDefault(),
                                            async (Request, GetRoamingAuthorisationListUpdatesXML) => {

                #region Send OnGetRoamingAuthorisationListUpdatesSOAPRequest event

                try
                {

                    OnGetRoamingAuthorisationListUpdatesSOAPRequest?.Invoke(DateTime.UtcNow,
                                                                            this.SOAPServer,
                                                                            Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetRoamingAuthorisationListUpdatesSOAPRequest));
                }

                #endregion


                var _GetRoamingAuthorisationListUpdatesRequest = GetRoamingAuthorisationListUpdatesRequest.Parse(GetRoamingAuthorisationListUpdatesXML);

                GetRoamingAuthorisationListUpdatesResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetRoamingAuthorisationListUpdatesRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetRoamingAuthorisationListUpdatesDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _GetRoamingAuthorisationListUpdatesRequest.LastUpdate,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetRoamingAuthorisationListUpdatesResponse.Server(_GetRoamingAuthorisationListUpdatesRequest, "Could not process the incoming GetRoamingAuthorisationListUpdates request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetRoamingAuthorisationListUpdatesHTTPResponse event

                try
                {

                    OnGetRoamingAuthorisationListUpdatesSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                             this.SOAPServer,
                                                                             Request,
                                                                             HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetRoamingAuthorisationListUpdatesSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - GetSingleRoamingAuthorisationRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "GetSingleRoamingAuthorisationRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "GetSingleRoamingAuthorisationRequest").FirstOrDefault(),
                                            async (Request, GetSingleRoamingAuthorisationXML) => {

                #region Send OnGetSingleRoamingAuthorisationSOAPRequest event

                try
                {

                    OnGetSingleRoamingAuthorisationSOAPRequest?.Invoke(DateTime.UtcNow,
                                                                       this.SOAPServer,
                                                                       Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetSingleRoamingAuthorisationSOAPRequest));
                }

                #endregion


                var _GetSingleRoamingAuthorisationRequest = GetSingleRoamingAuthorisationRequest.Parse(GetSingleRoamingAuthorisationXML);

                GetSingleRoamingAuthorisationResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetSingleRoamingAuthorisationRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetSingleRoamingAuthorisationDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _GetSingleRoamingAuthorisationRequest.EMTId,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results != null && results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (response == null || results.Length == 0)
                        response = GetSingleRoamingAuthorisationResponse.Server(_GetSingleRoamingAuthorisationRequest, "Could not process the incoming GetSingleRoamingAuthorisation request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetSingleRoamingAuthorisationHTTPResponse event

                try
                {

                    OnGetSingleRoamingAuthorisationSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                        this.SOAPServer,
                                                                        Request,
                                                                        HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetSingleRoamingAuthorisationSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - SetChargePointListRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "SetChargePointListRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "SetChargePointListRequest").FirstOrDefault(),
                                            async (Request, SetChargePointListXML) => {

                #region Send OnSetChargePointListSOAPRequest event

                try
                {

                    OnSetChargePointListSOAPRequest?.Invoke(DateTime.UtcNow,
                                                            this.SOAPServer,
                                                            Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnSetChargePointListSOAPRequest));
                }

                #endregion


                var _SetChargePointListRequest = SetChargePointListRequest.Parse(SetChargePointListXML);

                SetChargePointListResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnSetChargePointListRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnSetChargePointListDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _SetChargePointListRequest.ChargePointInfos,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = SetChargePointListResponse.Server(_SetChargePointListRequest, "Could not process the incoming SetChargePointList request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnSetChargePointListHTTPResponse event

                try
                {

                    OnSetChargePointListSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                             this.SOAPServer,
                                                             Request,
                                                             HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnSetChargePointListSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - UpdateChargePointListRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "UpdateChargePointListRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "UpdateChargePointListRequest").FirstOrDefault(),
                                            async (Request, UpdateChargePointListXML) => {

                #region Send OnUpdateChargePointListSOAPRequest event

                try
                {

                    OnUpdateChargePointListSOAPRequest?.Invoke(DateTime.UtcNow,
                                                               this.SOAPServer,
                                                               Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateChargePointListSOAPRequest));
                }

                #endregion


                var _UpdateChargePointListRequest = UpdateChargePointListRequest.Parse(UpdateChargePointListXML);

                UpdateChargePointListResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnUpdateChargePointListRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnUpdateChargePointListDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _UpdateChargePointListRequest.ChargePointInfos,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = UpdateChargePointListResponse.Server(_UpdateChargePointListRequest, "Could not process the incoming UpdateChargePointList request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnUpdateChargePointListHTTPResponse event

                try
                {

                    OnUpdateChargePointListSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                this.SOAPServer,
                                                                Request,
                                                                HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateChargePointListSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - UpdateStatusRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "UpdateStatusRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "UpdateStatusRequest").FirstOrDefault(),
                                            async (Request, UpdateStatusXML) => {

                #region Send OnUpdateStatusSOAPRequest event

                try
                {

                    OnUpdateStatusSOAPRequest?.Invoke(DateTime.UtcNow,
                                                      this.SOAPServer,
                                                      Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateStatusSOAPRequest));
                }

                #endregion


                var _UpdateStatusRequest = UpdateStatusRequest.Parse(UpdateStatusXML);

                UpdateStatusResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnUpdateStatusRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnUpdateStatusDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _UpdateStatusRequest.EVSEStatus,
                                           _UpdateStatusRequest.ParkingStatus,
                                           _UpdateStatusRequest.DefaultTTL,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results != null && results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (response == null || results.Length == 0)
                        response = UpdateStatusResponse.Server(_UpdateStatusRequest, "Could not process the incoming UpdateStatus request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnUpdateStatusHTTPResponse event

                try
                {

                    OnUpdateStatusSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                       this.SOAPServer,
                                                       Request,
                                                       HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateStatusSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - UpdateTariffsRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "UpdateTariffsRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "UpdateTariffsRequest").FirstOrDefault(),
                                            async (Request, UpdateTariffsXML) => {

                #region Send OnUpdateTariffsSOAPRequest event

                try
                {

                    OnUpdateTariffsSOAPRequest?.Invoke(DateTime.UtcNow,
                                                       this.SOAPServer,
                                                       Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateTariffsSOAPRequest));
                }

                #endregion


                var _UpdateTariffsRequest = UpdateTariffsRequest.Parse(UpdateTariffsXML);

                UpdateTariffsResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnUpdateTariffsRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnUpdateTariffsDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _UpdateTariffsRequest.TariffInfos,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = UpdateTariffsResponse.Server(_UpdateTariffsRequest, "Could not process the incoming UpdateTariffs request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnUpdateTariffsHTTPResponse event

                try
                {

                    OnUpdateTariffsSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                        this.SOAPServer,
                                                        Request,
                                                        HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateTariffsSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion


            // EMP messages...

            #region / - GetCDRsRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "GetCDRsRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "GetCDRsRequest").FirstOrDefault(),
                                            async (Request, GetCDRsXML) => {

                #region Send OnGetCDRsSOAPRequest event

                try
                {

                    OnGetCDRsSOAPRequest?.Invoke(DateTime.UtcNow,
                                                 this.SOAPServer,
                                                 Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetCDRsSOAPRequest));
                }

                #endregion


                var _GetCDRsRequest = GetCDRsRequest.Parse(GetCDRsXML);

                GetCDRsResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetCDRsRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetCDRsDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _GetCDRsRequest.CDRStatus,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetCDRsResponse.Server(_GetCDRsRequest, "Could not process the incoming GetCDRs request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetCDRsHTTPResponse event

                try
                {

                    OnGetCDRsSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                  this.SOAPServer,
                                                  Request,
                                                  HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetCDRsSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - ConfirmCDRsRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "ConfirmCDRsRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "ConfirmCDRsRequest").FirstOrDefault(),
                                            async (Request, ConfirmCDRsXML) => {

                #region Send OnConfirmCDRsSOAPRequest event

                try
                {

                    OnConfirmCDRsSOAPRequest?.Invoke(DateTime.UtcNow,
                                                     this.SOAPServer,
                                                     Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnConfirmCDRsSOAPRequest));
                }

                #endregion


                var _ConfirmCDRsRequest = ConfirmCDRsRequest.Parse(ConfirmCDRsXML);

                ConfirmCDRsResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnConfirmCDRsRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnConfirmCDRsDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _ConfirmCDRsRequest.Approved,
                                           _ConfirmCDRsRequest.Declined,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = ConfirmCDRsResponse.Server(_ConfirmCDRsRequest, "Could not process the incoming ConfirmCDRs request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnConfirmCDRsHTTPResponse event

                try
                {

                    OnConfirmCDRsSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                      this.SOAPServer,
                                                      Request,
                                                      HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnConfirmCDRsSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - GetChargePointListRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "GetChargePointListRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "GetChargePointListRequest").FirstOrDefault(),
                                            async (Request, GetChargePointListXML) => {

                #region Send OnGetChargePointListSOAPRequest event

                try
                {

                    OnGetChargePointListSOAPRequest?.Invoke(DateTime.UtcNow,
                                                            this.SOAPServer,
                                                            Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetChargePointListSOAPRequest));
                }

                #endregion


                var _GetChargePointListRequest = GetChargePointListRequest.Parse(GetChargePointListXML);

                GetChargePointListResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetChargePointListRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetChargePointListDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetChargePointListResponse.Server(_GetChargePointListRequest, "Could not process the incoming GetChargePointList request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetChargePointListHTTPResponse event

                try
                {

                    OnGetChargePointListSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                             this.SOAPServer,
                                                             Request,
                                                             HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetChargePointListSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - GetChargePointListUpdatesRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "GetChargePointListUpdatesRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "GetChargePointListUpdatesRequest").FirstOrDefault(),
                                            async (Request, GetChargePointListUpdatesXML) => {

                #region Send OnGetChargePointListUpdatesSOAPRequest event

                try
                {

                    OnGetChargePointListUpdatesSOAPRequest?.Invoke(DateTime.UtcNow,
                                                                   this.SOAPServer,
                                                                   Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetChargePointListUpdatesSOAPRequest));
                }

                #endregion


                var _GetChargePointListUpdatesRequest = GetChargePointListUpdatesRequest.Parse(GetChargePointListUpdatesXML);

                GetChargePointListUpdatesResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetChargePointListUpdatesRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetChargePointListUpdatesDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _GetChargePointListUpdatesRequest.LastUpdate,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetChargePointListUpdatesResponse.Server(_GetChargePointListUpdatesRequest, "Could not process the incoming GetChargePointListUpdates request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetChargePointListUpdatesHTTPResponse event

                try
                {

                    OnGetChargePointListUpdatesSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                    this.SOAPServer,
                                                                    Request,
                                                                    HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetChargePointListUpdatesSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - GetStatusRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "GetStatusRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "GetStatusRequest").FirstOrDefault(),
                                            async (Request, GetStatusXML) => {

                #region Send OnGetStatusSOAPRequest event

                try
                {

                    OnGetStatusSOAPRequest?.Invoke(DateTime.UtcNow,
                                                   this.SOAPServer,
                                                   Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetStatusSOAPRequest));
                }

                #endregion


                var _GetStatusRequest = GetStatusRequest.Parse(GetStatusXML);

                GetStatusResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetStatusRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetStatusDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _GetStatusRequest.LastRequest,
                                           _GetStatusRequest.StatusType,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetStatusResponse.Server(_GetStatusRequest, "Could not process the incoming GetStatus request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetStatusHTTPResponse event

                try
                {

                    OnGetStatusSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                    this.SOAPServer,
                                                    Request,
                                                    HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetStatusSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - GetTariffUpdatesRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "GetTariffUpdatesRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "GetTariffUpdatesRequest").FirstOrDefault(),
                                            async (Request, GetTariffUpdatesXML) => {

                #region Send OnGetTariffUpdatesSOAPRequest event

                try
                {

                    OnGetTariffUpdatesSOAPRequest?.Invoke(DateTime.UtcNow,
                                                          this.SOAPServer,
                                                          Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetTariffUpdatesSOAPRequest));
                }

                #endregion


                var _GetTariffUpdatesRequest = GetTariffUpdatesRequest.Parse(GetTariffUpdatesXML);

                GetTariffUpdatesResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetTariffUpdatesRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetTariffUpdatesDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _GetTariffUpdatesRequest.LastUpdate,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetTariffUpdatesResponse.Server(_GetTariffUpdatesRequest, "Could not process the incoming GetTariffUpdates request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetTariffUpdatesHTTPResponse event

                try
                {

                    OnGetTariffUpdatesSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                           this.SOAPServer,
                                                           Request,
                                                           HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetTariffUpdatesSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - SetRoamingAuthorisationListRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "SetRoamingAuthorisationListRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "SetRoamingAuthorisationListRequest").FirstOrDefault(),
                                            async (Request, SetRoamingAuthorisationListXML) => {

                #region Send OnSetRoamingAuthorisationListSOAPRequest event

                try
                {

                    OnSetRoamingAuthorisationListSOAPRequest?.Invoke(DateTime.UtcNow,
                                                                     this.SOAPServer,
                                                                     Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnSetRoamingAuthorisationListSOAPRequest));
                }

                #endregion


                var _SetRoamingAuthorisationListRequest = SetRoamingAuthorisationListRequest.Parse(SetRoamingAuthorisationListXML);

                SetRoamingAuthorisationListResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnSetRoamingAuthorisationListRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnSetRoamingAuthorisationListDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _SetRoamingAuthorisationListRequest.RoamingAuthorisationInfos,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = SetRoamingAuthorisationListResponse.Server(_SetRoamingAuthorisationListRequest, "Could not process the incoming SetRoamingAuthorisationList request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnSetRoamingAuthorisationListHTTPResponse event

                try
                {

                    OnSetRoamingAuthorisationListSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                      this.SOAPServer,
                                                                      Request,
                                                                      HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnSetRoamingAuthorisationListSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

            #region / - UpdateRoamingAuthorisationListRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "UpdateRoamingAuthorisationListRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "UpdateRoamingAuthorisationListRequest").FirstOrDefault(),
                                            async (Request, UpdateRoamingAuthorisationListXML) => {

                #region Send OnUpdateRoamingAuthorisationListSOAPRequest event

                try
                {

                    OnUpdateRoamingAuthorisationListSOAPRequest?.Invoke(DateTime.UtcNow,
                                                                        this.SOAPServer,
                                                                        Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateRoamingAuthorisationListSOAPRequest));
                }

                #endregion


                var _UpdateRoamingAuthorisationListRequest = UpdateRoamingAuthorisationListRequest.Parse(UpdateRoamingAuthorisationListXML);

                UpdateRoamingAuthorisationListResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnUpdateRoamingAuthorisationListRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnUpdateRoamingAuthorisationListDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _UpdateRoamingAuthorisationListRequest.RoamingAuthorisationInfos,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = UpdateRoamingAuthorisationListResponse.Server(_UpdateRoamingAuthorisationListRequest, "Could not process the incoming UpdateRoamingAuthorisationList request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnUpdateRoamingAuthorisationListHTTPResponse event

                try
                {

                    OnUpdateRoamingAuthorisationListSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                         this.SOAPServer,
                                                                         Request,
                                                                         HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateRoamingAuthorisationListSOAPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion


        }

        #endregion


    }

}
