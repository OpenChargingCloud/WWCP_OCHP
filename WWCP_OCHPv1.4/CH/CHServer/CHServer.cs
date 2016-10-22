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
        public new const           String    DefaultHTTPServerName  = "GraphDefined OCHP " + Version.Number + " HTTP/SOAP/XML CH Server API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort    DefaultHTTPServerPort  = new IPPort(2600);

        /// <summary>
        /// The default query timeout.
        /// </summary>
        public new static readonly TimeSpan  DefaultQueryTimeout    = TimeSpan.FromMinutes(1);

        #endregion

        #region Events

        // Shared events...

        #region OnAddServiceEndpoints

        /// <summary>
        /// An event sent whenever an add service endpoints HTTP request was received.
        /// </summary>
        public event RequestLogHandler                     OnAddServiceEndpointsHTTPRequest;

        /// <summary>
        /// An event sent whenever an add service endpoints HTTP response was sent.
        /// </summary>
        public event AccessLogHandler                      OnAddServiceEndpointsHTTPResponse;

        /// <summary>
        /// An event sent whenever an add service endpoints request was received.
        /// </summary>
        public event OnAddServiceEndpointsRequestDelegate  OnAddServiceEndpointsRequest;

        #endregion

        #region OnGetServiceEndpoints

        /// <summary>
        /// An event sent whenever a get service endpoints HTTP request was received.
        /// </summary>
        public event RequestLogHandler                     OnGetServiceEndpointsHTTPRequest;

        /// <summary>
        /// An event sent whenever a get service endpoints HTTP response was sent.
        /// </summary>
        public event AccessLogHandler                      OnGetServiceEndpointsHTTPResponse;

        /// <summary>
        /// An event sent whenever a get service endpoints request was received.
        /// </summary>
        public event OnGetServiceEndpointsRequestDelegate  OnGetServiceEndpointsRequest;

        #endregion


        // CPO events...

        #region OnAddCDRs

        /// <summary>
        /// An event sent whenever an add charge detail records HTTP request was received.
        /// </summary>
        public event RequestLogHandler         OnAddCDRsHTTPRequest;

        /// <summary>
        /// An event sent whenever an add charge detail records HTTP response was sent.
        /// </summary>
        public event AccessLogHandler          OnAddCDRsHTTPResponse;

        /// <summary>
        /// An event sent whenever an add charge detail records request was received.
        /// </summary>
        public event OnAddCDRsRequestDelegate  OnAddCDRsRequest;

        #endregion

        #region OnCheckCDRs

        /// <summary>
        /// An event sent whenever a check charge detail records HTTP request was received.
        /// </summary>
        public event RequestLogHandler           OnCheckCDRsHTTPRequest;

        /// <summary>
        /// An event sent whenever a check charge detail records HTTP response was sent.
        /// </summary>
        public event AccessLogHandler            OnCheckCDRsHTTPResponse;

        /// <summary>
        /// An event sent whenever a check charge detail records request was received.
        /// </summary>
        public event OnCheckCDRsRequestDelegate  OnCheckCDRsRequest;

        #endregion

        #region OnGetRoamingAuthorisationList

        /// <summary>
        /// An event sent whenever a get roaming authorisation list HTTP request was received.
        /// </summary>
        public event RequestLogHandler                             OnGetRoamingAuthorisationListHTTPRequest;

        /// <summary>
        /// An event sent whenever a get roaming authorisation list HTTP response was sent.
        /// </summary>
        public event AccessLogHandler                              OnGetRoamingAuthorisationListHTTPResponse;

        /// <summary>
        /// An event sent whenever a get roaming authorisation list request was received.
        /// </summary>
        public event OnGetRoamingAuthorisationListRequestDelegate  OnGetRoamingAuthorisationListRequest;

        #endregion

        #region OnGetRoamingAuthorisationListUpdates

        /// <summary>
        /// An event sent whenever a get roaming authorisation list updates HTTP request was received.
        /// </summary>
        public event RequestLogHandler                                    OnGetRoamingAuthorisationListUpdatesHTTPRequest;

        /// <summary>
        /// An event sent whenever a get roaming authorisation list updates HTTP response was sent.
        /// </summary>
        public event AccessLogHandler                                     OnGetRoamingAuthorisationListUpdatesHTTPResponse;

        /// <summary>
        /// An event sent whenever a get roaming authorisation list updates request was received.
        /// </summary>
        public event OnGetRoamingAuthorisationListUpdatesRequestDelegate  OnGetRoamingAuthorisationListUpdatesRequest;

        #endregion

        #region OnGetSingleRoamingAuthorisation

        /// <summary>
        /// An event sent whenever a get single roaming authorisation HTTP request was received.
        /// </summary>
        public event RequestLogHandler                               OnGetSingleRoamingAuthorisationHTTPRequest;

        /// <summary>
        /// An event sent whenever a get single roaming authorisation HTTP response was sent.
        /// </summary>
        public event AccessLogHandler                                OnGetSingleRoamingAuthorisationHTTPResponse;

        /// <summary>
        /// An event sent whenever a get single roaming authorisation request was received.
        /// </summary>
        public event OnGetSingleRoamingAuthorisationRequestDelegate  OnGetSingleRoamingAuthorisationRequest;

        #endregion

        #region OnSetChargePointList

        /// <summary>
        /// An event sent whenever a set charge point list HTTP request was received.
        /// </summary>
        public event RequestLogHandler                    OnSetChargePointListHTTPRequest;

        /// <summary>
        /// An event sent whenever a set charge point list HTTP response was sent.
        /// </summary>
        public event AccessLogHandler                     OnSetChargePointListHTTPResponse;

        /// <summary>
        /// An event sent whenever a set charge point list request was received.
        /// </summary>
        public event OnSetChargePointListRequestDelegate  OnSetChargePointListRequest;

        #endregion

        #region OnUpdateChargePointList

        /// <summary>
        /// An event sent whenever a update charge point list HTTP request was received.
        /// </summary>
        public event RequestLogHandler                       OnUpdateChargePointListHTTPRequest;

        /// <summary>
        /// An event sent whenever a update charge point list HTTP response was sent.
        /// </summary>
        public event AccessLogHandler                        OnUpdateChargePointListHTTPResponse;

        /// <summary>
        /// An event sent whenever a update charge point list request was received.
        /// </summary>
        public event OnUpdateChargePointListRequestDelegate  OnUpdateChargePointListRequest;

        #endregion

        #region OnUpdateStatus

        /// <summary>
        /// An event sent whenever an update EVSE and/or parking status HTTP request was received.
        /// </summary>
        public event RequestLogHandler              OnUpdateStatusHTTPRequest;

        /// <summary>
        /// An event sent whenever an update EVSE and/or parking status HTTP response was sent.
        /// </summary>
        public event AccessLogHandler               OnUpdateStatusHTTPResponse;

        /// <summary>
        /// An event sent whenever an update EVSE and/or parking status request was received.
        /// </summary>
        public event OnUpdateStatusRequestDelegate  OnUpdateStatusRequest;

        #endregion

        #region OnUpdateTariffs

        /// <summary>
        /// An event sent whenever an update charging tariffs HTTP request was received.
        /// </summary>
        public event RequestLogHandler               OnUpdateTariffsHTTPRequest;

        /// <summary>
        /// An event sent whenever an update charging tariffs HTTP response was sent.
        /// </summary>
        public event AccessLogHandler                OnUpdateTariffsHTTPResponse;

        /// <summary>
        /// An event sent whenever an update charging tariffs request was received.
        /// </summary>
        public event OnUpdateTariffsRequestDelegate  OnUpdateTariffsRequest;

        #endregion


        // EMP events...

        #region OnConfirmCDRs

        /// <summary>
        /// An event sent whenever a confirm charge detail records HTTP request was received.
        /// </summary>
        public event RequestLogHandler             OnConfirmCDRsHTTPRequest;

        /// <summary>
        /// An event sent whenever a confirm charge detail records HTTP response was sent.
        /// </summary>
        public event AccessLogHandler              OnConfirmCDRsHTTPResponse;

        /// <summary>
        /// An event sent whenever a confirm charge detail records request was received.
        /// </summary>
        public event OnConfirmCDRsRequestDelegate  OnConfirmCDRsRequest;

        #endregion

        #region OnGetChargePointList

        /// <summary>
        /// An event sent whenever a get charge point list HTTP request was received.
        /// </summary>
        public event RequestLogHandler                    OnGetChargePointListHTTPRequest;

        /// <summary>
        /// An event sent whenever a get charge point list HTTP response was sent.
        /// </summary>
        public event AccessLogHandler                     OnGetChargePointListHTTPResponse;

        /// <summary>
        /// An event sent whenever a get charge point list request was received.
        /// </summary>
        public event OnGetChargePointListRequestDelegate  OnGetChargePointListRequest;

        #endregion

        #region OnGetChargePointListUpdates

        /// <summary>
        /// An event sent whenever a get charge point list updates HTTP request was received.
        /// </summary>
        public event RequestLogHandler                           OnGetChargePointListUpdatesHTTPRequest;

        /// <summary>
        /// An event sent whenever a get charge point list updates HTTP response was sent.
        /// </summary>
        public event AccessLogHandler                            OnGetChargePointListUpdatesHTTPResponse;

        /// <summary>
        /// An event sent whenever a get charge point list updates request was received.
        /// </summary>
        public event OnGetChargePointListUpdatesRequestDelegate  OnGetChargePointListUpdatesRequest;

        #endregion

        #region OnGetStatus

        /// <summary>
        /// An event sent whenever a get EVSE and parking status HTTP request was received.
        /// </summary>
        public event RequestLogHandler           OnGetStatusHTTPRequest;

        /// <summary>
        /// An event sent whenever a get EVSE and parking status HTTP response was sent.
        /// </summary>
        public event AccessLogHandler            OnGetStatusHTTPResponse;

        /// <summary>
        /// An event sent whenever a get EVSE and parking status request was received.
        /// </summary>
        public event OnGetStatusRequestDelegate  OnGetStatusRequest;

        #endregion

        #region OnGetTariffUpdates

        /// <summary>
        /// An event sent whenever a get charging tariffs updates HTTP request was received.
        /// </summary>
        public event RequestLogHandler                  OnGetTariffUpdatesHTTPRequest;

        /// <summary>
        /// An event sent whenever a get charging tariffs updates HTTP response was sent.
        /// </summary>
        public event AccessLogHandler                   OnGetTariffUpdatesHTTPResponse;

        /// <summary>
        /// An event sent whenever a get charging tariffs updates request was received.
        /// </summary>
        public event OnGetTariffUpdatesRequestDelegate  OnGetTariffUpdatesRequest;

        #endregion

        #region OnSetRoamingAuthorisationList

        /// <summary>
        /// An event sent whenever a set roaming authorisation infos HTTP request was received.
        /// </summary>
        public event RequestLogHandler                             OnSetRoamingAuthorisationListHTTPRequest;

        /// <summary>
        /// An event sent whenever a set roaming authorisation infos HTTP response was sent.
        /// </summary>
        public event AccessLogHandler                              OnSetRoamingAuthorisationListHTTPResponse;

        /// <summary>
        /// An event sent whenever a set roaming authorisation infos request was received.
        /// </summary>
        public event OnSetRoamingAuthorisationListRequestDelegate  OnSetRoamingAuthorisationListRequest;

        #endregion

        #region OnUpdateRoamingAuthorisationList

        /// <summary>
        /// An event sent whenever an update roaming authorisation infos HTTP request was received.
        /// </summary>
        public event RequestLogHandler                                OnUpdateRoamingAuthorisationListHTTPRequest;

        /// <summary>
        /// An event sent whenever an update roaming authorisation infos HTTP response was sent.
        /// </summary>
        public event AccessLogHandler                                 OnUpdateRoamingAuthorisationListHTTPResponse;

        /// <summary>
        /// An event sent whenever an update roaming authorisation infos request was received.
        /// </summary>
        public event OnUpdateRoamingAuthorisationListRequestDelegate  OnUpdateRoamingAuthorisationListRequest;

        #endregion

        #endregion

        #region Constructor(s)

        #region CHServer(HTTPServerName, TCPPort = null, URIPrefix = "", DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OCHP HTTP/SOAP/XML CH Server API using IPAddress.Any.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public CHServer(String    HTTPServerName  = DefaultHTTPServerName,
                        IPPort    TCPPort         = null,
                        String    URIPrefix       = "",
                        DNSClient DNSClient       = null,
                        Boolean   AutoStart       = false)

            : base(HTTPServerName.IsNotNullOrEmpty() ? HTTPServerName : DefaultHTTPServerName,
                   TCPPort ?? DefaultHTTPServerPort,
                   URIPrefix,
                   HTTPContentType.XMLTEXT_UTF8,
                   DNSClient,
                   AutoStart: false)

        {

            if (AutoStart)
                Start();

        }

        #endregion

        #region CHServer(SOAPServer, URIPrefix = "")

        /// <summary>
        /// Use the given HTTP server for the OCHP HTTP/SOAP/XML CH Server API using IPAddress.Any.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        public CHServer(SOAPServer  SOAPServer,
                         String      URIPrefix  = "")

            : base(SOAPServer,
                   URIPrefix)

        { }

        #endregion

        #endregion


        #region (override) RegisterURITemplates()

        protected override void RegisterURITemplates()
        {

            #region / (HTTPRoot)

            SOAPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         new String[] { "/", URIPrefix + "/" },
                                         HTTPContentType.TEXT_UTF8,
                                         HTTPDelegate: Request => {

                                             return Task.FromResult(
                                                 new HTTPResponseBuilder(Request) {

                                                     HTTPStatusCode  = HTTPStatusCode.BadGateway,
                                                     ContentType     = HTTPContentType.TEXT_UTF8,
                                                     Content         = ("Welcome at " + DefaultHTTPServerName + Environment.NewLine +
                                                                        "This is a HTTP/SOAP/XML endpoint!" + Environment.NewLine + Environment.NewLine +
                                                                        "Defined endpoints: " + Environment.NewLine + Environment.NewLine +
                                                                        SOAPServer.
                                                                            SOAPDispatchers.
                                                                            Select(group => " - " + group.Key + Environment.NewLine +
                                                                                            "   " + group.SelectMany(dispatcher => dispatcher.SOAPDispatches).
                                                                                                          Select    (dispatch   => dispatch.  Description).
                                                                                                          AggregateWith(", ")
                                                                                  ).AggregateWith(Environment.NewLine + Environment.NewLine)
                                                                       ).ToUTF8Bytes(),
                                                     Connection      = "close"

                                                 }.AsImmutable());

                                         },
                                         AllowReplacement: URIReplacement.Allow);

            #endregion


            // Shared messages...

            #region / - AddServiceEndpointsRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "AddServiceEndpointsRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "AddServiceEndpointsRequest").FirstOrDefault(),
                                            async (Request, AddServiceEndpointsXML) => {

                #region Send OnAddServiceEndpointsHTTPRequest event

                try
                {

                    OnAddServiceEndpointsHTTPRequest?.Invoke(DateTime.Now,
                                                             this.SOAPServer,
                                                             Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnAddServiceEndpointsHTTPRequest));
                }

                #endregion


                var _AddServiceEndpointsRequest = AddServiceEndpointsRequest.Parse(AddServiceEndpointsXML);

                AddServiceEndpointsResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnAddServiceEndpointsRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnAddServiceEndpointsRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _AddServiceEndpointsRequest.OperatorEndpoints,
                                           _AddServiceEndpointsRequest.ProviderEndpoints,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = AddServiceEndpointsResponse.Server("Could not process the incoming AddServiceEndpoints request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnAddServiceEndpointsHTTPResponse event

                try
                {

                    OnAddServiceEndpointsHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                              this.SOAPServer,
                                                              Request,
                                                              HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnAddServiceEndpointsHTTPResponse));
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

                #region Send OnGetServiceEndpointsHTTPRequest event

                try
                {

                    OnGetServiceEndpointsHTTPRequest?.Invoke(DateTime.Now,
                                                             this.SOAPServer,
                                                             Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetServiceEndpointsHTTPRequest));
                }

                #endregion


                var _GetServiceEndpointsRequest = GetServiceEndpointsRequest.Parse(GetServiceEndpointsXML);

                GetServiceEndpointsResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetServiceEndpointsRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetServiceEndpointsRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetServiceEndpointsResponse.Server("Could not process the incoming GetServiceEndpoints request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetServiceEndpointsHTTPResponse event

                try
                {

                    OnGetServiceEndpointsHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                              this.SOAPServer,
                                                              Request,
                                                              HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetServiceEndpointsHTTPResponse));
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

                #region Send OnAddCDRsHTTPRequest event

                try
                {

                    OnAddCDRsHTTPRequest?.Invoke(DateTime.Now,
                                                 this.SOAPServer,
                                                 Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnAddCDRsHTTPRequest));
                }

                #endregion


                var _AddCDRsRequest = AddCDRsRequest.Parse(AddCDRsXML);

                AddCDRsResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnAddCDRsRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnAddCDRsRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _AddCDRsRequest.CDRInfos,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = AddCDRsResponse.Server("Could not process the incoming AddCDRs request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnAddCDRsHTTPResponse event

                try
                {

                    OnAddCDRsHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                  this.SOAPServer,
                                                  Request,
                                                  HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnAddCDRsHTTPResponse));
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

                #region Send OnCheckCDRsHTTPRequest event

                try
                {

                    OnCheckCDRsHTTPRequest?.Invoke(DateTime.Now,
                                                 this.SOAPServer,
                                                 Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnCheckCDRsHTTPRequest));
                }

                #endregion


                var _CheckCDRsRequest = CheckCDRsRequest.Parse(CheckCDRsXML);

                CheckCDRsResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnCheckCDRsRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnCheckCDRsRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _CheckCDRsRequest.CDRStatus,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = CheckCDRsResponse.Server("Could not process the incoming CheckCDRs request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnCheckCDRsHTTPResponse event

                try
                {

                    OnCheckCDRsHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                    this.SOAPServer,
                                                    Request,
                                                    HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnCheckCDRsHTTPResponse));
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

                #region Send OnGetRoamingAuthorisationListHTTPRequest event

                try
                {

                    OnGetRoamingAuthorisationListHTTPRequest?.Invoke(DateTime.Now,
                                                                     this.SOAPServer,
                                                                     Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetRoamingAuthorisationListHTTPRequest));
                }

                #endregion


                var _GetRoamingAuthorisationListRequest = GetRoamingAuthorisationListRequest.Parse(GetRoamingAuthorisationListXML);

                GetRoamingAuthorisationListResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetRoamingAuthorisationListRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetRoamingAuthorisationListRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetRoamingAuthorisationListResponse.Server("Could not process the incoming GetRoamingAuthorisationList request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetRoamingAuthorisationListHTTPResponse event

                try
                {

                    OnGetRoamingAuthorisationListHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                      this.SOAPServer,
                                                                      Request,
                                                                      HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetRoamingAuthorisationListHTTPResponse));
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

                #region Send OnGetRoamingAuthorisationListUpdatesHTTPRequest event

                try
                {

                    OnGetRoamingAuthorisationListUpdatesHTTPRequest?.Invoke(DateTime.Now,
                                                                            this.SOAPServer,
                                                                            Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetRoamingAuthorisationListUpdatesHTTPRequest));
                }

                #endregion


                var _GetRoamingAuthorisationListUpdatesRequest = GetRoamingAuthorisationListUpdatesRequest.Parse(GetRoamingAuthorisationListUpdatesXML);

                GetRoamingAuthorisationListUpdatesResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetRoamingAuthorisationListUpdatesRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetRoamingAuthorisationListUpdatesRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _GetRoamingAuthorisationListUpdatesRequest.LastUpdate,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetRoamingAuthorisationListUpdatesResponse.Server("Could not process the incoming GetRoamingAuthorisationListUpdates request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetRoamingAuthorisationListUpdatesHTTPResponse event

                try
                {

                    OnGetRoamingAuthorisationListUpdatesHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                             this.SOAPServer,
                                                                             Request,
                                                                             HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetRoamingAuthorisationListUpdatesHTTPResponse));
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

                #region Send OnGetSingleRoamingAuthorisationHTTPRequest event

                try
                {

                    OnGetSingleRoamingAuthorisationHTTPRequest?.Invoke(DateTime.Now,
                                                                       this.SOAPServer,
                                                                       Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetSingleRoamingAuthorisationHTTPRequest));
                }

                #endregion


                var _GetSingleRoamingAuthorisationRequest = GetSingleRoamingAuthorisationRequest.Parse(GetSingleRoamingAuthorisationXML);

                GetSingleRoamingAuthorisationResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetSingleRoamingAuthorisationRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetSingleRoamingAuthorisationRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _GetSingleRoamingAuthorisationRequest.EMTId,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetSingleRoamingAuthorisationResponse.Server("Could not process the incoming GetSingleRoamingAuthorisation request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetSingleRoamingAuthorisationHTTPResponse event

                try
                {

                    OnGetSingleRoamingAuthorisationHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                        this.SOAPServer,
                                                                        Request,
                                                                        HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetSingleRoamingAuthorisationHTTPResponse));
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

                #region Send OnSetChargePointListHTTPRequest event

                try
                {

                    OnSetChargePointListHTTPRequest?.Invoke(DateTime.Now,
                                                            this.SOAPServer,
                                                            Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnSetChargePointListHTTPRequest));
                }

                #endregion


                var _SetChargePointListRequest = SetChargePointListRequest.Parse(SetChargePointListXML);

                SetChargePointListResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnSetChargePointListRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnSetChargePointListRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _SetChargePointListRequest.ChargePointInfos,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = SetChargePointListResponse.Server("Could not process the incoming SetChargePointList request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnSetChargePointListHTTPResponse event

                try
                {

                    OnSetChargePointListHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                             this.SOAPServer,
                                                             Request,
                                                             HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnSetChargePointListHTTPResponse));
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

                #region Send OnUpdateChargePointListHTTPRequest event

                try
                {

                    OnUpdateChargePointListHTTPRequest?.Invoke(DateTime.Now,
                                                               this.SOAPServer,
                                                               Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateChargePointListHTTPRequest));
                }

                #endregion


                var _UpdateChargePointListRequest = UpdateChargePointListRequest.Parse(UpdateChargePointListXML);

                UpdateChargePointListResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnUpdateChargePointListRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnUpdateChargePointListRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _UpdateChargePointListRequest.ChargePointInfos,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = UpdateChargePointListResponse.Server("Could not process the incoming UpdateChargePointList request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnUpdateChargePointListHTTPResponse event

                try
                {

                    OnUpdateChargePointListHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                this.SOAPServer,
                                                                Request,
                                                                HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateChargePointListHTTPResponse));
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

                #region Send OnUpdateStatusHTTPRequest event

                try
                {

                    OnUpdateStatusHTTPRequest?.Invoke(DateTime.Now,
                                                      this.SOAPServer,
                                                      Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateStatusHTTPRequest));
                }

                #endregion


                var _UpdateStatusRequest = UpdateStatusRequest.Parse(UpdateStatusXML);

                UpdateStatusResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnUpdateStatusRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnUpdateStatusRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _UpdateStatusRequest.EVSEStatus,
                                           _UpdateStatusRequest.ParkingStatus,
                                           _UpdateStatusRequest.DefaultTTL,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = UpdateStatusResponse.Server("Could not process the incoming UpdateStatus request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnUpdateStatusHTTPResponse event

                try
                {

                    OnUpdateStatusHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                       this.SOAPServer,
                                                       Request,
                                                       HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateStatusHTTPResponse));
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

                #region Send OnUpdateTariffsHTTPRequest event

                try
                {

                    OnUpdateTariffsHTTPRequest?.Invoke(DateTime.Now,
                                                       this.SOAPServer,
                                                       Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateTariffsHTTPRequest));
                }

                #endregion


                var _UpdateTariffsRequest = UpdateTariffsRequest.Parse(UpdateTariffsXML);

                UpdateTariffsResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnUpdateTariffsRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnUpdateTariffsRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _UpdateTariffsRequest.TariffInfos,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = UpdateTariffsResponse.Server("Could not process the incoming UpdateTariffs request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnUpdateTariffsHTTPResponse event

                try
                {

                    OnUpdateTariffsHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                        this.SOAPServer,
                                                        Request,
                                                        HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateTariffsHTTPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion


            // EMP messages...

            #region / - ConfirmCDRsRequest

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "ConfirmCDRsRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "ConfirmCDRsRequest").FirstOrDefault(),
                                            async (Request, ConfirmCDRsXML) => {

                #region Send OnConfirmCDRsHTTPRequest event

                try
                {

                    OnConfirmCDRsHTTPRequest?.Invoke(DateTime.Now,
                                                     this.SOAPServer,
                                                     Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnConfirmCDRsHTTPRequest));
                }

                #endregion


                var _ConfirmCDRsRequest = ConfirmCDRsRequest.Parse(ConfirmCDRsXML);

                ConfirmCDRsResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnConfirmCDRsRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnConfirmCDRsRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _ConfirmCDRsRequest.Approved,
                                           _ConfirmCDRsRequest.Declined,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = ConfirmCDRsResponse.Server("Could not process the incoming ConfirmCDRs request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnConfirmCDRsHTTPResponse event

                try
                {

                    OnConfirmCDRsHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                      this.SOAPServer,
                                                      Request,
                                                      HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnConfirmCDRsHTTPResponse));
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

                #region Send OnGetChargePointListHTTPRequest event

                try
                {

                    OnGetChargePointListHTTPRequest?.Invoke(DateTime.Now,
                                                            this.SOAPServer,
                                                            Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetChargePointListHTTPRequest));
                }

                #endregion


                var _GetChargePointListRequest = GetChargePointListRequest.Parse(GetChargePointListXML);

                GetChargePointListResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetChargePointListRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetChargePointListRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetChargePointListResponse.Server("Could not process the incoming GetChargePointList request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetChargePointListHTTPResponse event

                try
                {

                    OnGetChargePointListHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                             this.SOAPServer,
                                                             Request,
                                                             HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetChargePointListHTTPResponse));
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

                #region Send OnGetChargePointListUpdatesHTTPRequest event

                try
                {

                    OnGetChargePointListUpdatesHTTPRequest?.Invoke(DateTime.Now,
                                                                   this.SOAPServer,
                                                                   Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetChargePointListUpdatesHTTPRequest));
                }

                #endregion


                var _GetChargePointListUpdatesRequest = GetChargePointListUpdatesRequest.Parse(GetChargePointListUpdatesXML);

                GetChargePointListUpdatesResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetChargePointListUpdatesRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetChargePointListUpdatesRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _GetChargePointListUpdatesRequest.LastUpdate,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetChargePointListUpdatesResponse.Server("Could not process the incoming GetChargePointListUpdates request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetChargePointListUpdatesHTTPResponse event

                try
                {

                    OnGetChargePointListUpdatesHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                    this.SOAPServer,
                                                                    Request,
                                                                    HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetChargePointListUpdatesHTTPResponse));
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

                #region Send OnGetStatusHTTPRequest event

                try
                {

                    OnGetStatusHTTPRequest?.Invoke(DateTime.Now,
                                                   this.SOAPServer,
                                                   Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetStatusHTTPRequest));
                }

                #endregion


                var _GetStatusRequest = GetStatusRequest.Parse(GetStatusXML);

                GetStatusResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetStatusRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetStatusRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _GetStatusRequest.LastRequest,
                                           _GetStatusRequest.StatusType,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetStatusResponse.Server("Could not process the incoming GetStatus request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetStatusHTTPResponse event

                try
                {

                    OnGetStatusHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                    this.SOAPServer,
                                                    Request,
                                                    HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetStatusHTTPResponse));
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

                #region Send OnGetTariffUpdatesHTTPRequest event

                try
                {

                    OnGetTariffUpdatesHTTPRequest?.Invoke(DateTime.Now,
                                                          this.SOAPServer,
                                                          Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetTariffUpdatesHTTPRequest));
                }

                #endregion


                var _GetTariffUpdatesRequest = GetTariffUpdatesRequest.Parse(GetTariffUpdatesXML);

                GetTariffUpdatesResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnGetTariffUpdatesRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetTariffUpdatesRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _GetTariffUpdatesRequest.LastUpdate,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = GetTariffUpdatesResponse.Server("Could not process the incoming GetTariffUpdates request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnGetTariffUpdatesHTTPResponse event

                try
                {

                    OnGetTariffUpdatesHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                           this.SOAPServer,
                                                           Request,
                                                           HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnGetTariffUpdatesHTTPResponse));
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

                #region Send OnSetRoamingAuthorisationListHTTPRequest event

                try
                {

                    OnSetRoamingAuthorisationListHTTPRequest?.Invoke(DateTime.Now,
                                                                     this.SOAPServer,
                                                                     Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnSetRoamingAuthorisationListHTTPRequest));
                }

                #endregion


                var _SetRoamingAuthorisationListRequest = SetRoamingAuthorisationListRequest.Parse(SetRoamingAuthorisationListXML);

                SetRoamingAuthorisationListResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnSetRoamingAuthorisationListRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnSetRoamingAuthorisationListRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _SetRoamingAuthorisationListRequest.RoamingAuthorisationInfos,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = SetRoamingAuthorisationListResponse.Server("Could not process the incoming SetRoamingAuthorisationList request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnSetRoamingAuthorisationListHTTPResponse event

                try
                {

                    OnSetRoamingAuthorisationListHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                      this.SOAPServer,
                                                                      Request,
                                                                      HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnSetRoamingAuthorisationListHTTPResponse));
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

                #region Send OnUpdateRoamingAuthorisationListHTTPRequest event

                try
                {

                    OnUpdateRoamingAuthorisationListHTTPRequest?.Invoke(DateTime.Now,
                                                                        this.SOAPServer,
                                                                        Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateRoamingAuthorisationListHTTPRequest));
                }

                #endregion


                var _UpdateRoamingAuthorisationListRequest = UpdateRoamingAuthorisationListRequest.Parse(UpdateRoamingAuthorisationListXML);

                UpdateRoamingAuthorisationListResponse response = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnUpdateRoamingAuthorisationListRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnUpdateRoamingAuthorisationListRequestDelegate)
                                          (DateTime.Now,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _UpdateRoamingAuthorisationListRequest.RoamingAuthorisationInfos,
                                           DefaultQueryTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = UpdateRoamingAuthorisationListResponse.Server("Could not process the incoming UpdateRoamingAuthorisationList request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnUpdateRoamingAuthorisationListHTTPResponse event

                try
                {

                    OnUpdateRoamingAuthorisationListHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                         this.SOAPServer,
                                                                         Request,
                                                                         HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(CHServer) + "." + nameof(OnUpdateRoamingAuthorisationListHTTPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion


        }

        #endregion


    }

}
