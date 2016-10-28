﻿/*
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

using org.GraphDefined.WWCP.OCHPv1_4.EMP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHP CPO HTTP/SOAP/XML Server API.
    /// </summary>
    public class CPOServer : ASOAPServer
    {

        #region Data

        /// <summary>
        /// The default HTTP/SOAP/XML server name.
        /// </summary>
        public new const           String    DefaultHTTPServerName  = "GraphDefined OCHP " + Version.Number + " HTTP/SOAP/XML CPO Server API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort    DefaultHTTPServerPort  = new IPPort(2601);

        /// <summary>
        /// The default HTTP/SOAP/XML server URI prefix.
        /// </summary>
        public new const           String    DefaultURIPrefix       = "";

        /// <summary>
        /// The default query timeout.
        /// </summary>
        public new static readonly TimeSpan  DefaultQueryTimeout    = TimeSpan.FromMinutes(1);

        #endregion

        #region Events

        #region OnSelectEVSERequest

        /// <summary>
        /// An event sent whenever a select EVSE SOAP request was received.
        /// </summary>
        public event RequestLogHandler     OnSelectEVSESOAPRequest;

        /// <summary>
        /// An event sent whenever a select EVSE SOAP response was sent.
        /// </summary>
        public event AccessLogHandler      OnSelectEVSESOAPResponse;

        /// <summary>
        /// An event sent whenever a select EVSE request was received.
        /// </summary>
        public event OnSelectEVSEDelegate  OnSelectEVSERequest;

        #endregion

        #region OnControlEVSERequest

        /// <summary>
        /// An event sent whenever a control EVSE SOAP request was received.
        /// </summary>
        public event RequestLogHandler      OnControlEVSESOAPRequest;

        /// <summary>
        /// An event sent whenever a control EVSE SOAP response was sent.
        /// </summary>
        public event AccessLogHandler       OnControlEVSESOAPResponse;

        /// <summary>
        /// An event sent whenever a control EVSE request was received.
        /// </summary>
        public event OnControlEVSEDelegate  OnControlEVSERequest;

        #endregion

        #region OnReleaseEVSERequest

        /// <summary>
        /// An event sent whenever a release EVSE SOAP request was received.
        /// </summary>
        public event RequestLogHandler      OnReleaseEVSESOAPRequest;

        /// <summary>
        /// An event sent whenever a release EVSE SOAP response was sent.
        /// </summary>
        public event AccessLogHandler       OnReleaseEVSESOAPResponse;

        /// <summary>
        /// An event sent whenever a release EVSE request was received.
        /// </summary>
        public event OnReleaseEVSEDelegate  OnReleaseEVSERequest;

        #endregion

        #region OnGetEVSEStatusRequest

        /// <summary>
        /// An event sent whenever a get EVSE status SOAP request was received.
        /// </summary>
        public event RequestLogHandler        OnGetEVSEStatusSOAPRequest;

        /// <summary>
        /// An event sent whenever a get EVSE status SOAP response was sent.
        /// </summary>
        public event AccessLogHandler         OnGetEVSEStatusSOAPResponse;

        /// <summary>
        /// An event sent whenever a get EVSE status request was received.
        /// </summary>
        public event OnGetEVSEStatusDelegate  OnGetEVSEStatusRequest;

        #endregion

        #region OnReportDiscrepancyRequest

        /// <summary>
        /// An event sent whenever a report discrepancy SOAP request was received.
        /// </summary>
        public event RequestLogHandler            OnReportDiscrepancySOAPRequest;

        /// <summary>
        /// An event sent whenever a report discrepancy SOAP response was sent.
        /// </summary>
        public event AccessLogHandler             OnReportDiscrepancySOAPResponse;

        /// <summary>
        /// An event sent whenever a report discrepancy request was received.
        /// </summary>
        public event OnReportDiscrepancyDelegate  OnReportDiscrepancyRequest;

        #endregion

        #endregion

        #region Constructor(s)

        #region CPOServer(HTTPServerName, TCPPort = null, URIPrefix = DefaultURIPrefix, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OCHP HTTP/SOAP/XML CPO Server API using IPAddress.Any.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Whether to start the server immediately or not.</param>
        public CPOServer(String    HTTPServerName  = DefaultHTTPServerName,
                         IPPort    TCPPort         = null,
                         String    URIPrefix       = DefaultURIPrefix,
                         DNSClient DNSClient       = null,
                         Boolean   AutoStart       = false)

            : base(HTTPServerName.IsNotNullOrEmpty() ? HTTPServerName : DefaultHTTPServerName,
                   TCPPort ?? DefaultHTTPServerPort,
                   URIPrefix.     IsNotNullOrEmpty() ? URIPrefix      : DefaultURIPrefix,
                   HTTPContentType.XMLTEXT_UTF8,
                   DNSClient,
                   AutoStart: false)

        {

            if (AutoStart)
                Start();

        }

        #endregion

        #region CPOServer(SOAPServer, URIPrefix = DefaultURIPrefix)

        /// <summary>
        /// Use the given HTTP server for the OCHP HTTP/SOAP/XML CPO Server API using IPAddress.Any.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        public CPOServer(SOAPServer  SOAPServer,
                         String      URIPrefix  = DefaultURIPrefix)

            : base(SOAPServer,
                   URIPrefix.IsNotNullOrEmpty() ? URIPrefix : DefaultURIPrefix)

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


            #region / - SelectEVSE

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "SelectEvseRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "SelectEvseRequest").FirstOrDefault(),
                                            async (Request, SelectEVSEXML) => {

                    #region Send OnSelectEVSESOAPRequest event

                    try
                    {

                        OnSelectEVSESOAPRequest?.Invoke(DateTime.Now,
                                                        this.SOAPServer,
                                                        Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnSelectEVSESOAPRequest));
                    }

                    #endregion


                    var _SelectEVSERequest = SelectEVSERequest.Parse(SelectEVSEXML);

                    SelectEVSEResponse response            = null;


                    #region Call async subscribers

                    if (response == null)
                    {

                        var results = OnSelectEVSERequest?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnSelectEVSEDelegate)
                                              (DateTime.Now,
                                               this,
                                               Request.CancellationToken,
                                               Request.EventTrackingId,
                                               _SelectEVSERequest.EVSEId,
                                               _SelectEVSERequest.ContractId,
                                               _SelectEVSERequest.ReserveUntil,
                                               DefaultQueryTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = SelectEVSEResponse.Server("Could not process the incoming SelectEVSE request!");

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


                    #region Send OnSelectEVSESOAPResponse event

                    try
                    {

                        OnSelectEVSESOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                         this.SOAPServer,
                                                         Request,
                                                         HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnSelectEVSESOAPResponse));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion

            #region / - ControlEVSE

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "ControlEvseRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "ControlEvseRequest").FirstOrDefault(),
                                            async (Request, ControlEVSEXML) => {

                    #region Send OnControlEVSESOAPRequest event

                    try
                    {

                        OnControlEVSESOAPRequest?.Invoke(DateTime.Now,
                                                         this.SOAPServer,
                                                         Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnControlEVSESOAPRequest));
                    }

                    #endregion


                    var _ControlEVSERequest = ControlEVSERequest.Parse(ControlEVSEXML);

                    ControlEVSEResponse response = null;


                    #region Call async subscribers

                    if (response == null)
                    {

                        var results = OnControlEVSERequest?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnControlEVSEDelegate)
                                              (DateTime.Now,
                                               this,
                                               Request.CancellationToken,
                                               Request.EventTrackingId,
                                               _ControlEVSERequest.DirectId,
                                               _ControlEVSERequest.Operation,
                                               _ControlEVSERequest.MaxPower,
                                               _ControlEVSERequest.MaxCurrent,
                                               _ControlEVSERequest.OnePhase,
                                               _ControlEVSERequest.MaxEnergy,
                                               _ControlEVSERequest.MinEnergy,
                                               _ControlEVSERequest.Departure,
                                               DefaultQueryTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = ControlEVSEResponse.Server("Could not process the incoming ControlEVSE request!");

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


                    #region Send OnControlEVSESOAPResponse event

                    try
                    {

                        OnControlEVSESOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                          this.SOAPServer,
                                                          Request,
                                                          HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnControlEVSESOAPResponse));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion

            #region / - ReleaseEVSE

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "ReleaseEvseRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "ReleaseEvseRequest").FirstOrDefault(),
                                            async (Request, ReleaseEVSEXML) => {

                    #region Send OnReleaseEVSESOAPRequest event

                    try
                    {

                        OnReleaseEVSESOAPRequest?.Invoke(DateTime.Now,
                                                         this.SOAPServer,
                                                         Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnReleaseEVSESOAPRequest));
                    }

                    #endregion


                    var _ReleaseEVSERequest = ReleaseEVSERequest.Parse(ReleaseEVSEXML);

                    ReleaseEVSEResponse response = null;


                    #region Call async subscribers

                    if (response == null)
                    {

                        var results = OnReleaseEVSERequest?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnReleaseEVSEDelegate)
                                              (DateTime.Now,
                                               this,
                                               Request.CancellationToken,
                                               Request.EventTrackingId,
                                               _ReleaseEVSERequest.DirectId,
                                               DefaultQueryTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = ReleaseEVSEResponse.Server("Could not process the incoming ReleaseEVSE request!");

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


                    #region Send OnReleaseEVSESOAPResponse event

                    try
                    {

                        OnReleaseEVSESOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                          this.SOAPServer,
                                                          Request,
                                                          HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnReleaseEVSESOAPResponse));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion

            #region / - GetEVSEStatus

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "DirectEvseStatusRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "DirectEvseStatusRequest").FirstOrDefault(),
                                            async (Request, GetEVSEStatusXML) => {

                    #region Send OnGetEVSEStatusSOAPRequest event

                    try
                    {

                        OnGetEVSEStatusSOAPRequest?.Invoke(DateTime.Now,
                                                           this.SOAPServer,
                                                           Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnGetEVSEStatusSOAPRequest));
                    }

                    #endregion


                    var _GetEVSEStatusRequest = GetEVSEStatusRequest.Parse(GetEVSEStatusXML);

                    GetEVSEStatusResponse response = null;


                    #region Call async subscribers

                    if (response == null)
                    {

                        var results = OnGetEVSEStatusRequest?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnGetEVSEStatusDelegate)
                                              (DateTime.Now,
                                               this,
                                               Request.CancellationToken,
                                               Request.EventTrackingId,
                                               _GetEVSEStatusRequest.EVSEIds,
                                               DefaultQueryTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = GetEVSEStatusResponse.Server("Could not process the incoming GetEVSEStatus request!");

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


                    #region Send OnGetEVSEStatusSOAPResponse event

                    try
                    {

                        OnGetEVSEStatusSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                            this.SOAPServer,
                                                            Request,
                                                            HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnGetEVSEStatusSOAPResponse));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion

            #region / - ReportDiscrepancy

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "ReportDiscrepancyRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "ReportDiscrepancyRequest").FirstOrDefault(),
                                            async (Request, ReportDiscrepancyXML) => {

                    #region Send OnReportDiscrepancySOAPRequest event

                    try
                    {

                        OnReportDiscrepancySOAPRequest?.Invoke(DateTime.Now,
                                                               this.SOAPServer,
                                                               Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnReportDiscrepancySOAPRequest));
                    }

                    #endregion


                    var _ReportDiscrepancyRequest = ReportDiscrepancyRequest.Parse(ReportDiscrepancyXML);

                    ReportDiscrepancyResponse response = null;


                    #region Call async subscribers

                    if (response == null)
                    {

                        var results = OnReportDiscrepancyRequest?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnReportDiscrepancyDelegate)
                                              (DateTime.Now,
                                               this,
                                               Request.CancellationToken,
                                               Request.EventTrackingId,
                                               _ReportDiscrepancyRequest.EVSEId,
                                               _ReportDiscrepancyRequest.Report,
                                               DefaultQueryTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = ReportDiscrepancyResponse.Server("Could not process the incoming ReportDiscrepancy request!");

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


                    #region Send OnReportDiscrepancySOAPResponse event

                    try
                    {

                        OnReportDiscrepancySOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                this.SOAPServer,
                                                                Request,
                                                                HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnReportDiscrepancySOAPResponse));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion


        }

        #endregion


    }

}
