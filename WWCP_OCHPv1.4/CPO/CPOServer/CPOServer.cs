/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

using cloud.charging.open.protocols.OCHPv1_4.EMP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.CPO
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
        public new const           String           DefaultHTTPServerName  = "GraphDefined OCHP " + Version.Number + " HTTP/SOAP/XML CPO API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort           DefaultHTTPServerPort  = IPPort.Parse(2601);

        /// <summary>
        /// The default HTTP/SOAP/XML server URI prefix.
        /// </summary>
        public new static readonly HTTPPath         DefaultURLPrefix       = HTTPPath.Parse("/");

        /// <summary>
        /// The default HTTP/SOAP/XML server URI suffix.
        /// </summary>
        public     static readonly HTTPPath         DefaultURLSuffix       = HTTPPath.Parse("/OCHP");

        /// <summary>
        /// The default HTTP/SOAP/XML content type.
        /// </summary>
        public new static readonly HTTPContentType  DefaultContentType     = HTTPContentType.Text.XML_UTF8;

        /// <summary>
        /// The default request timeout.
        /// </summary>
        public new static readonly TimeSpan         DefaultRequestTimeout  = TimeSpan.FromMinutes(1);

        #endregion

        #region Properties

        /// <summary>
        /// The identification of this HTTP/SOAP service.
        /// </summary>
        public String   ServiceName           { get; }

        /// <summary>
        /// The HTTP/SOAP/XML server URI suffix.
        /// </summary>
        public HTTPPath  URLSuffix           { get; }

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

        #region CPOServer(HTTPServerName, ServiceName = null, TCPPort = default, URLPrefix = default, URLSuffix = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OCHP HTTP/SOAP/XML CPO Server API.
        /// </summary>
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServiceName">An optional identification for this SOAP service.</param>
        /// <param name="URLPrefix">An optional prefix for the HTTP URLs.</param>
        /// <param name="URLSuffix">An optional HTTP/SOAP/XML server URI suffix.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="LoggingPath">The logging path.</param>
        /// <param name="LoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and logfile name.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public CPOServer(String                  ServerName                = DefaultHTTPServerName,
                         IPPort?                 TCPPort                   = null,
                         String                  ServiceName               = null,
                         HTTPPath?               URLPrefix                 = null,
                         HTTPPath?               URLSuffix                 = null,
                         HTTPContentType         ContentType               = null,
                         Boolean                 RegisterHTTPRootService   = true,
                         String                  LoggingPath               = null,
                         String                  LoggingContext            = CPOServerLogger.DefaultContext,
                         LogfileCreatorDelegate  LogfileCreator            = null,
                         DNSClient               DNSClient                 = null,
                         Boolean                 AutoStart                 = false)

            : base(ServerName.IsNotNullOrEmpty() ? ServerName : DefaultHTTPServerName,
                   TCPPort     ?? DefaultHTTPServerPort,
                   ServiceName ?? "OCHP " + Version.Number + " " + nameof(CPOServer),
                   URLPrefix   ?? DefaultURLPrefix,
                   ContentType ?? DefaultContentType,
                   RegisterHTTPRootService,
                   DNSClient,
                   AutoStart: false)

        {

            this.ServiceName  = ServiceName ?? "OCHP " + Version.Number + " " + nameof(CPOServer);
            this.URLSuffix    = URLSuffix   ?? DefaultURLSuffix;
            this.HTTPLogger   = new CPOServerLogger(this,
                                                    LoggingPath,
                                                    LoggingContext ?? CPOServerLogger.DefaultContext,
                                                    LogfileCreator);

            RegisterURITemplates();

            if (AutoStart)
                Start();

        }

        #endregion

        #region CPOServer(SOAPServer, ServiceName = null, URLPrefix = default, URLSuffix = default)

        /// <summary>
        /// Use the given HTTP server for the OCHP HTTP/SOAP/XML CPO Server API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="ServiceName">An optional identification for this SOAP service.</param>
        /// <param name="URLPrefix">An optional prefix for the HTTP URLs.</param>
        /// <param name="URLSuffix">An optional HTTP/SOAP/XML server URI suffix.</param>
        public CPOServer(SOAPServer  SOAPServer,
                         String      ServiceName   = null,
                         HTTPPath?   URLPrefix     = null,
                         HTTPPath?   URLSuffix     = null)

            : base(SOAPServer,
                   URLPrefix ?? DefaultURLPrefix)

        {

            this.ServiceName  = ServiceName ?? "OCHP " + Version.Number + " " + nameof(CPOServer);
            this.URLSuffix    = URLSuffix   ?? DefaultURLSuffix;

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

            #region / - SelectEVSE

            SOAPServer.RegisterSOAPDelegate(null,
                                            HTTPHostname.Any,
                                            URLPrefix + URLSuffix,
                                            "SelectEvseRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "SelectEvseRequest").FirstOrDefault(),
                                            async (Request, SelectEVSEXML) => {

                    #region Send OnSelectEVSESOAPRequest event

                    try
                    {

                        OnSelectEVSESOAPRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                        this.SOAPServer.HTTPServer,
                                                        Request);

                    }
                    catch (Exception e)
                    {
                        DebugX.LogException(e, nameof(CPOServer) + "." + nameof(OnSelectEVSESOAPRequest));
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
                                              (Timestamp.Now,
                                               this,
                                               Request.CancellationToken,
                                               Request.EventTrackingId,
                                               _SelectEVSERequest.EVSEId,
                                               _SelectEVSERequest.ContractId,
                                               _SelectEVSERequest.ReserveUntil,
                                               DefaultRequestTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = SelectEVSEResponse.Server(_SelectEVSERequest, "Could not process the incoming SelectEVSE request!");

                    }

                    #endregion

                    #region Create SOAPResponse

                    var HTTPResponse = new HTTPResponse.Builder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.HTTPServer.DefaultServerName,
                        Date            = Timestamp.Now,
                        ContentType     = HTTPContentType.Text.XML_UTF8,
                        Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                    };

                    #endregion


                    #region Send OnSelectEVSESOAPResponse event

                    try
                    {

                        OnSelectEVSESOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                         this.SOAPServer.HTTPServer,
                                                         Request,
                                                         HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        DebugX.LogException(e, nameof(CPOServer) + "." + nameof(OnSelectEVSESOAPResponse));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion

            #region / - ControlEVSE

            SOAPServer.RegisterSOAPDelegate(null,
                                            HTTPHostname.Any,
                                            URLPrefix + URLSuffix,
                                            "ControlEvseRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "ControlEvseRequest").FirstOrDefault(),
                                            async (Request, ControlEVSEXML) => {

                    #region Send OnControlEVSESOAPRequest event

                    try
                    {

                        OnControlEVSESOAPRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                         this.SOAPServer.HTTPServer,
                                                         Request);

                    }
                    catch (Exception e)
                    {
                        DebugX.LogException(e, nameof(CPOServer) + "." + nameof(OnControlEVSESOAPRequest));
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
                                              (Timestamp.Now,
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
                                               DefaultRequestTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = ControlEVSEResponse.Server(_ControlEVSERequest, "Could not process the incoming ControlEVSE request!");

                    }

                    #endregion

                    #region Create SOAPResponse

                    var HTTPResponse = new HTTPResponse.Builder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.HTTPServer.DefaultServerName,
                        Date            = Timestamp.Now,
                        ContentType     = HTTPContentType.Text.XML_UTF8,
                        Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                    };

                    #endregion


                    #region Send OnControlEVSESOAPResponse event

                    try
                    {

                        OnControlEVSESOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                          this.SOAPServer.HTTPServer,
                                                          Request,
                                                          HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        DebugX.LogException(e, nameof(CPOServer) + "." + nameof(OnControlEVSESOAPResponse));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion

            #region / - ReleaseEVSE

            SOAPServer.RegisterSOAPDelegate(null,
                                            HTTPHostname.Any,
                                            URLPrefix + URLSuffix,
                                            "ReleaseEvseRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "ReleaseEvseRequest").FirstOrDefault(),
                                            async (Request, ReleaseEVSEXML) => {

                    #region Send OnReleaseEVSESOAPRequest event

                    try
                    {

                        OnReleaseEVSESOAPRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                         this.SOAPServer.HTTPServer,
                                                         Request);

                    }
                    catch (Exception e)
                    {
                        DebugX.LogException(e, nameof(CPOServer) + "." + nameof(OnReleaseEVSESOAPRequest));
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
                                              (Timestamp.Now,
                                               this,
                                               Request.CancellationToken,
                                               Request.EventTrackingId,
                                               _ReleaseEVSERequest.DirectId,
                                               DefaultRequestTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = ReleaseEVSEResponse.Server(_ReleaseEVSERequest, "Could not process the incoming ReleaseEVSE request!");

                    }

                    #endregion

                    #region Create SOAPResponse

                    var HTTPResponse = new HTTPResponse.Builder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.HTTPServer.DefaultServerName,
                        Date            = Timestamp.Now,
                        ContentType     = HTTPContentType.Text.XML_UTF8,
                        Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                    };

                    #endregion


                    #region Send OnReleaseEVSESOAPResponse event

                    try
                    {

                        OnReleaseEVSESOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                          this.SOAPServer.HTTPServer,
                                                          Request,
                                                          HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        DebugX.LogException(e, nameof(CPOServer) + "." + nameof(OnReleaseEVSESOAPResponse));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion

            #region / - GetEVSEStatus

            SOAPServer.RegisterSOAPDelegate(null,
                                            HTTPHostname.Any,
                                            URLPrefix + URLSuffix,
                                            "DirectEvseStatusRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "DirectEvseStatusRequest").FirstOrDefault(),
                                            async (Request, GetEVSEStatusXML) => {

                    #region Send OnGetEVSEStatusSOAPRequest event

                    try
                    {

                        OnGetEVSEStatusSOAPRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                           this.SOAPServer.HTTPServer,
                                                           Request);

                    }
                    catch (Exception e)
                    {
                        DebugX.LogException(e, nameof(CPOServer) + "." + nameof(OnGetEVSEStatusSOAPRequest));
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
                                              (Timestamp.Now,
                                               this,
                                               Request.CancellationToken,
                                               Request.EventTrackingId,
                                               _GetEVSEStatusRequest.EVSEIds,
                                               DefaultRequestTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = GetEVSEStatusResponse.Server(_GetEVSEStatusRequest, "Could not process the incoming GetEVSEStatus request!");

                    }

                    #endregion

                    #region Create SOAPResponse

                    var HTTPResponse = new HTTPResponse.Builder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.HTTPServer.DefaultServerName,
                        Date            = Timestamp.Now,
                        ContentType     = HTTPContentType.Text.XML_UTF8,
                        Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                    };

                    #endregion


                    #region Send OnGetEVSEStatusSOAPResponse event

                    try
                    {

                        OnGetEVSEStatusSOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                            this.SOAPServer.HTTPServer,
                                                            Request,
                                                            HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        DebugX.LogException(e, nameof(CPOServer) + "." + nameof(OnGetEVSEStatusSOAPResponse));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion

            #region / - ReportDiscrepancy

            SOAPServer.RegisterSOAPDelegate(null,
                                            HTTPHostname.Any,
                                            URLPrefix + URLSuffix,
                                            "ReportDiscrepancyRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "ReportDiscrepancyRequest").FirstOrDefault(),
                                            async (Request, ReportDiscrepancyXML) => {

                    #region Send OnReportDiscrepancySOAPRequest event

                    try
                    {

                        OnReportDiscrepancySOAPRequest?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now,
                                                               this.SOAPServer.HTTPServer,
                                                               Request);

                    }
                    catch (Exception e)
                    {
                        DebugX.LogException(e, nameof(CPOServer) + "." + nameof(OnReportDiscrepancySOAPRequest));
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
                                              (Timestamp.Now,
                                               this,
                                               Request.CancellationToken,
                                               Request.EventTrackingId,
                                               _ReportDiscrepancyRequest.EVSEId,
                                               _ReportDiscrepancyRequest.Report,
                                               DefaultRequestTimeout)).
                                          ToArray();

                        if (results.Length > 0)
                        {

                            await Task.WhenAll(results);

                            response = results.FirstOrDefault()?.Result;

                        }

                        if (results.Length == 0 || response == null)
                            response = ReportDiscrepancyResponse.Server(_ReportDiscrepancyRequest, "Could not process the incoming ReportDiscrepancy request!");

                    }

                    #endregion

                    #region Create SOAPResponse

                    var HTTPResponse = new HTTPResponse.Builder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.HTTPServer.DefaultServerName,
                        Date            = Timestamp.Now,
                        ContentType     = HTTPContentType.Text.XML_UTF8,
                        Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                    };

                    #endregion


                    #region Send OnReportDiscrepancySOAPResponse event

                    try
                    {

                        OnReportDiscrepancySOAPResponse?.Invoke(HTTPResponse.Timestamp,
                                                                this.SOAPServer.HTTPServer,
                                                                Request,
                                                                HTTPResponse);

                    }
                    catch (Exception e)
                    {
                        DebugX.LogException(e, nameof(CPOServer) + "." + nameof(OnReportDiscrepancySOAPResponse));
                    }

                    #endregion

                    return HTTPResponse;

            });

            #endregion

        }

        #endregion


    }

}
