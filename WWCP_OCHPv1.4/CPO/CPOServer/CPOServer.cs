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
        public new static readonly IPPort    DefaultHTTPServerPort  = new IPPort(2600);

        /// <summary>
        /// The default query timeout.
        /// </summary>
        public new static readonly TimeSpan  DefaultQueryTimeout    = TimeSpan.FromMinutes(1);

        #endregion

        #region Events

        #region OnSelectEVSERequest

        /// <summary>
        /// An event sent whenever a select EVSE HTTP request was received.
        /// </summary>
        public event RequestLogHandler            OnSelectEVSEHTTPRequest;

        /// <summary>
        /// An event sent whenever a select EVSE HTTP response was sent.
        /// </summary>
        public event AccessLogHandler             OnSelectEVSEHTTPResponse;

        /// <summary>
        /// An event sent whenever a select EVSE request was received.
        /// </summary>
        public event OnSelectEVSERequestDelegate  OnSelectEVSERequest;

        #endregion

        #region OnControlEVSERequest

        /// <summary>
        /// An event sent whenever a control EVSE HTTP request was received.
        /// </summary>
        public event RequestLogHandler             OnControlEVSEHTTPRequest;

        /// <summary>
        /// An event sent whenever a control EVSE HTTP response was sent.
        /// </summary>
        public event AccessLogHandler              OnControlEVSEHTTPResponse;

        /// <summary>
        /// An event sent whenever a control EVSE request was received.
        /// </summary>
        public event OnControlEVSERequestDelegate  OnControlEVSERequest;

        #endregion

        #region OnReleaseEVSERequest

        /// <summary>
        /// An event sent whenever a release EVSE HTTP request was received.
        /// </summary>
        public event RequestLogHandler             OnReleaseEVSEHTTPRequest;

        /// <summary>
        /// An event sent whenever a release EVSE HTTP response was sent.
        /// </summary>
        public event AccessLogHandler              OnReleaseEVSEHTTPResponse;

        /// <summary>
        /// An event sent whenever a release EVSE request was received.
        /// </summary>
        public event OnReleaseEVSERequestDelegate  OnReleaseEVSERequest;

        #endregion

        #region OnGetEVSEStatusRequest

        /// <summary>
        /// An event sent whenever a get EVSE status HTTP request was received.
        /// </summary>
        public event RequestLogHandler               OnGetEVSEStatusHTTPRequest;

        /// <summary>
        /// An event sent whenever a get EVSE status HTTP response was sent.
        /// </summary>
        public event AccessLogHandler                OnGetEVSEStatusHTTPResponse;

        /// <summary>
        /// An event sent whenever a get EVSE status request was received.
        /// </summary>
        public event OnGetEVSEStatusRequestDelegate  OnGetEVSEStatusRequest;

        #endregion

        #region OnReportDiscrepancyRequest

        /// <summary>
        /// An event sent whenever a report discrepancy HTTP request was received.
        /// </summary>
        public event RequestLogHandler                   OnReportDiscrepancyHTTPRequest;

        /// <summary>
        /// An event sent whenever a report discrepancy HTTP response was sent.
        /// </summary>
        public event AccessLogHandler                    OnReportDiscrepancyHTTPResponse;

        /// <summary>
        /// An event sent whenever a report discrepancy request was received.
        /// </summary>
        public event OnReportDiscrepancyRequestDelegate  OnReportDiscrepancyRequest;

        #endregion

        #endregion

        #region Constructor(s)

        #region CPOServer(HTTPServerName, TCPPort = null, URIPrefix = "", DNSClient = null, AutoStart = false)

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
                         String    URIPrefix       = "",
                         DNSClient DNSClient       = null,
                         Boolean   AutoStart       = false)

            : base(HTTPServerName.IsNotNullOrEmpty() ? HTTPServerName : DefaultHTTPServerName,
                   TCPPort != null                   ? TCPPort        : DefaultHTTPServerPort,
                   URIPrefix,
                   HTTPContentType.XMLTEXT_UTF8,
                   DNSClient,
                   AutoStart: false)

        {

            if (AutoStart)
                Start();

        }

        #endregion

        #region CPOServer(SOAPServer, URIPrefix = "")

        /// <summary>
        /// Use the given HTTP server for the OCHP HTTP/SOAP/XML CPO Server API using IPAddress.Any.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        public CPOServer(SOAPServer  SOAPServer,
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


            #region / - SelectEVSE

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
                                            "SelectEvseRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "SelectEvseRequest").FirstOrDefault(),
                                            async (Request, SelectEVSEXML) => {


                    #region Documentation

                    // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                    //                   xmlns:OCHP    = "http://ochp.eu/1.4">
                    //
                    //    <soapenv:Header/>
                    //    <soapenv:Body>
                    //       <OCHP:SelectEvseRequest>
                    //
                    //          <OCHP:evseId>?</OCHP:evseId>
                    //          <OCHP:contractId>?</OCHP:contractId>
                    //
                    //          <!--Optional:-->
                    //          <OCHP:reserveUntil>
                    //             <OCHP:DateTime>?</OCHP:DateTime>
                    //          </OCHP:reserveUntil>
                    //
                    //       </OCHP:SelectEvseRequest>
                    //    </soapenv:Body>
                    // </soapenv:Envelope>

                    #endregion

                    #region Send OnSelectEVSEHTTPRequest event

                    try
                    {

                        OnSelectEVSEHTTPRequest?.Invoke(DateTime.Now,
                                                        this.SOAPServer,
                                                        Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnSelectEVSEHTTPRequest));
                    }

                    #endregion


                    var _SelectEVSERequest = OCHPv1_4.SelectEVSERequest.Parse(SelectEVSEXML);

                    SelectEVSEResponse response            = null;


                    #region Call async subscribers

                    if (response == null)
                    {

                        var results = OnSelectEVSERequest?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnSelectEVSERequestDelegate)
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


                    #region Send OnSelectEVSEHTTPResponse event

                    try
                    {

                        OnSelectEVSEHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                         this.SOAPServer,
                                                         Request,
                                                         HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnSelectEVSEHTTPResponse));
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


                    #region Documentation

                    // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                    //                   xmlns:OCHP    = "http://ochp.eu/1.4">
                    //
                    //    <soapenv:Header/>
                    //    <soapenv:Body>
                    //       <OCHP:ControlEvseRequest>
                    //
                    //          <OCHP:directId>?</OCHP:directId>
                    //
                    //          <OCHP:operation>
                    //             <OCHP:operation>?</OCHP:operation>
                    //          </OCHP:operation>
                    //
                    //          <!--Optional:-->
                    //          <OCHP:maxPower>?</OCHP:maxPower>
                    //
                    //          <!--Optional:-->
                    //          <OCHP:maxCurrent>?</OCHP:maxCurrent>
                    //
                    //          <!--Optional:-->
                    //          <OCHP:onePhase>?</OCHP:onePhase>
                    //
                    //          <!--Optional:-->
                    //          <OCHP:maxEnergy>?</OCHP:maxEnergy>
                    //
                    //          <!--Optional:-->
                    //          <OCHP:minEnergy>?</OCHP:minEnergy>
                    //
                    //          <!--Optional:-->
                    //          <OCHP:departure>
                    //             <OCHP:DateTime>?</OCHP:DateTime>
                    //          </OCHP:departure>
                    //
                    //       </OCHP:ControlEvseRequest>
                    //    </soapenv:Body>
                    // </soapenv:Envelope>

                    #endregion

                    #region Send OnControlEVSEHTTPRequest event

                    try
                    {

                        OnControlEVSEHTTPRequest?.Invoke(DateTime.Now,
                                                         this.SOAPServer,
                                                         Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnControlEVSEHTTPRequest));
                    }

                    #endregion


                    var _ControlEVSERequest = OCHPv1_4.ControlEVSERequest.Parse(ControlEVSEXML);

                    ControlEVSEResponse response = null;


                    #region Call async subscribers

                    if (response == null)
                    {

                        var results = OnControlEVSERequest?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnControlEVSERequestDelegate)
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


                    #region Send OnControlEVSEHTTPResponse event

                    try
                    {

                        OnControlEVSEHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                          this.SOAPServer,
                                                          Request,
                                                          HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnControlEVSEHTTPResponse));
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


                    #region Documentation

                    // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                    //                   xmlns:OCHP    = "http://ochp.eu/1.4">
                    //
                    //    <soapenv:Header/>
                    //    <soapenv:Body>
                    //       <OCHP:ReleaseEvseRequest>
                    //
                    //          <ns:directId>?</ns:directId>
                    //
                    //       </OCHP:ReleaseEvseRequest>
                    //    </soapenv:Body>
                    // </soapenv:Envelope>

                    #endregion

                    #region Send OnReleaseEVSEHTTPRequest event

                    try
                    {

                        OnReleaseEVSEHTTPRequest?.Invoke(DateTime.Now,
                                                         this.SOAPServer,
                                                         Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnReleaseEVSEHTTPRequest));
                    }

                    #endregion


                    var _ReleaseEVSERequest = OCHPv1_4.ReleaseEVSERequest.Parse(ReleaseEVSEXML);

                    ReleaseEVSEResponse response = null;


                    #region Call async subscribers

                    if (response == null)
                    {

                        var results = OnReleaseEVSERequest?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnReleaseEVSERequestDelegate)
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


                    #region Send OnReleaseEVSEHTTPResponse event

                    try
                    {

                        OnReleaseEVSEHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                          this.SOAPServer,
                                                          Request,
                                                          HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnReleaseEVSEHTTPResponse));
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


                    #region Documentation

                    // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                    //                   xmlns:OCHP    = "http://ochp.eu/1.4">
                    //
                    //    <soapenv:Header/>
                    //    <soapenv:Body>
                    //       <OCHP:DirectEvseStatusRequest>
                    //
                    //          <!--1 or more repetitions:-->
                    //          <ns:requestedEvseId>?</ns:requestedEvseId>
                    //
                    //       </OCHP:DirectEvseStatusRequest>
                    //    </soapenv:Body>
                    // </soapenv:Envelope>

                    #endregion

                    #region Send OnGetEVSEStatusHTTPRequest event

                    try
                    {

                        OnGetEVSEStatusHTTPRequest?.Invoke(DateTime.Now,
                                                           this.SOAPServer,
                                                           Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnGetEVSEStatusHTTPRequest));
                    }

                    #endregion


                    var _GetEVSEStatusRequest = OCHPv1_4.GetEVSEStatusRequest.Parse(GetEVSEStatusXML);

                    GetEVSEStatusResponse response = null;


                    #region Call async subscribers

                    if (response == null)
                    {

                        var results = OnGetEVSEStatusRequest?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnGetEVSEStatusRequestDelegate)
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


                    #region Send OnGetEVSEStatusHTTPResponse event

                    try
                    {

                        OnGetEVSEStatusHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                            this.SOAPServer,
                                                            Request,
                                                            HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnGetEVSEStatusHTTPResponse));
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


                    #region Documentation

                    // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                    //                   xmlns:OCHP    = "http://ochp.eu/1.4">
                    //
                    //    <soapenv:Header/>
                    //    <soapenv:Body>
                    //      <OCHP:ReportDiscrepancyRequest>
                    //
                    //         <OCHP:evseId>?</OCHP:evseId>
                    //         <OCHP:report>?</OCHP:report>
                    //
                    //      </OCHP:ReportDiscrepancyRequest>
                    //    </soapenv:Body>
                    // </soapenv:Envelope>

                    #endregion

                    #region Send OnReportDiscrepancyHTTPRequest event

                    try
                    {

                        OnReportDiscrepancyHTTPRequest?.Invoke(DateTime.Now,
                                                               this.SOAPServer,
                                                               Request);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnReportDiscrepancyHTTPRequest));
                    }

                    #endregion


                    var _ReportDiscrepancyRequest = OCHPv1_4.ReportDiscrepancyRequest.Parse(ReportDiscrepancyXML);

                    ReportDiscrepancyResponse response = null;


                    #region Call async subscribers

                    if (response == null)
                    {

                        var results = OnReportDiscrepancyRequest?.
                                          GetInvocationList()?.
                                          SafeSelect(subscriber => (subscriber as OnReportDiscrepancyRequestDelegate)
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


                    #region Send OnReportDiscrepancyHTTPResponse event

                    try
                    {

                        OnReportDiscrepancyHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                this.SOAPServer,
                                                                Request,
                                                                HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        e.Log(nameof(CPOServer) + "." + nameof(OnReportDiscrepancyHTTPResponse));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion


        }

        #endregion


    }

}
