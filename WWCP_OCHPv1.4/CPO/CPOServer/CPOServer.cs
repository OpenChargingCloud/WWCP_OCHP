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
        public new const           String           DefaultHTTPServerName  = "GraphDefined OCHP " + Version.Number + " HTTP/SOAP/XML CPO API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort           DefaultHTTPServerPort  = new IPPort(2601);

        /// <summary>
        /// The default HTTP/SOAP/XML server URI prefix.
        /// </summary>
        public new const           String           DefaultURIPrefix       = "";

        /// <summary>
        /// The default HTTP/SOAP/XML server URI suffix.
        /// </summary>
        public     const           String           DefaultURISuffix       = "/OCHP";

        /// <summary>
        /// The default HTTP/SOAP/XML content type.
        /// </summary>
        public new static readonly HTTPContentType  DefaultContentType     = HTTPContentType.XMLTEXT_UTF8;

        /// <summary>
        /// The default request timeout.
        /// </summary>
        public new static readonly TimeSpan         DefaultRequestTimeout  = TimeSpan.FromMinutes(1);

        #endregion

        #region Properties

        /// <summary>
        /// The identification of this HTTP/SOAP service.
        /// </summary>
        public String  ServiceId           { get; }

        /// <summary>
        /// The HTTP/SOAP/XML server URI suffix.
        /// </summary>
        public String  URISuffix           { get; }

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

        #region CPOServer(HTTPServerName, ServiceId = null, TCPPort = default, URIPrefix = default, URISuffix = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OCHP HTTP/SOAP/XML CPO Server API.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="URISuffix">An optional HTTP/SOAP/XML server URI suffix.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public CPOServer(String          HTTPServerName           = DefaultHTTPServerName,
                         String          ServiceId                = null,
                         IPPort          TCPPort                  = null,
                         String          URIPrefix                = DefaultURIPrefix,
                         String          URISuffix                = DefaultURISuffix,
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

            #region Initial checks

            URISuffix = URISuffix != null && URISuffix.Trim().IsNotNullOrEmpty()
                            ? URISuffix.Trim()
                            : DefaultURISuffix;

            if (URISuffix.Length > 0 && !URISuffix.StartsWith("/", StringComparison.Ordinal))
                URISuffix = "/" + URISuffix;

            while (URISuffix.EndsWith("/", StringComparison.Ordinal))
                URISuffix = URISuffix.Substring(0, URISuffix.Length - 1);

            #endregion

            this.ServiceId  = ServiceId ?? nameof(CPOServer);
            this.URISuffix  = URISuffix;

            RegisterURITemplates();

            if (AutoStart)
                Start();

        }

        #endregion

        #region CPOServer(SOAPServer, ServiceId = null, URIPrefix = default, URISuffix = default)

        /// <summary>
        /// Use the given HTTP server for the OCHP HTTP/SOAP/XML CPO Server API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="ServiceId">An optional identification for this SOAP service.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="URISuffix">An optional HTTP/SOAP/XML server URI suffix.</param>
        public CPOServer(SOAPServer  SOAPServer,
                         String      ServiceId   = null,
                         String      URIPrefix   = DefaultURIPrefix,
                         String      URISuffix   = DefaultURISuffix)

            : base(SOAPServer,
                   URIPrefix != null && URIPrefix.Trim().IsNotNullOrEmpty()
                                 ? URIPrefix.Trim()
                                 : DefaultURIPrefix)

        {

            #region Initial checks

            URISuffix = URISuffix != null && URISuffix.Trim().IsNotNullOrEmpty()
                            ? URISuffix.Trim()
                            : DefaultURISuffix;

            if (URISuffix.Length > 0 && !URISuffix.StartsWith("/", StringComparison.Ordinal))
                URISuffix = "/" + URISuffix;

            while (URISuffix.EndsWith("/", StringComparison.Ordinal))
                URISuffix = URISuffix.Substring(0, URISuffix.Length - 1);

            #endregion

            this.ServiceId  = ServiceId ?? nameof(EMPServer);
            this.URISuffix  = URISuffix;

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

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + URISuffix,
                                            "SelectEvseRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "SelectEvseRequest").FirstOrDefault(),
                                            async (Request, SelectEVSEXML) => {

                    #region Send OnSelectEVSESOAPRequest event

                    try
                    {

                        OnSelectEVSESOAPRequest?.Invoke(DateTime.UtcNow,
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
                                              (DateTime.UtcNow,
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

                    var HTTPResponse = new HTTPResponseBuilder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.DefaultServerName,
                        Date            = DateTime.UtcNow,
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
                                            URIPrefix + URISuffix,
                                            "ControlEvseRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "ControlEvseRequest").FirstOrDefault(),
                                            async (Request, ControlEVSEXML) => {

                    #region Send OnControlEVSESOAPRequest event

                    try
                    {

                        OnControlEVSESOAPRequest?.Invoke(DateTime.UtcNow,
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
                                              (DateTime.UtcNow,
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

                    var HTTPResponse = new HTTPResponseBuilder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.DefaultServerName,
                        Date            = DateTime.UtcNow,
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
                                            URIPrefix + URISuffix,
                                            "ReleaseEvseRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "ReleaseEvseRequest").FirstOrDefault(),
                                            async (Request, ReleaseEVSEXML) => {

                    #region Send OnReleaseEVSESOAPRequest event

                    try
                    {

                        OnReleaseEVSESOAPRequest?.Invoke(DateTime.UtcNow,
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
                                              (DateTime.UtcNow,
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

                    var HTTPResponse = new HTTPResponseBuilder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.DefaultServerName,
                        Date            = DateTime.UtcNow,
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
                                            URIPrefix + URISuffix,
                                            "DirectEvseStatusRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "DirectEvseStatusRequest").FirstOrDefault(),
                                            async (Request, GetEVSEStatusXML) => {

                    #region Send OnGetEVSEStatusSOAPRequest event

                    try
                    {

                        OnGetEVSEStatusSOAPRequest?.Invoke(DateTime.UtcNow,
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
                                              (DateTime.UtcNow,
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

                    var HTTPResponse = new HTTPResponseBuilder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.DefaultServerName,
                        Date            = DateTime.UtcNow,
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
                                            URIPrefix + URISuffix,
                                            "ReportDiscrepancyRequest",
                                            XML => XML.Descendants(OCHPNS.Default + "ReportDiscrepancyRequest").FirstOrDefault(),
                                            async (Request, ReportDiscrepancyXML) => {

                    #region Send OnReportDiscrepancySOAPRequest event

                    try
                    {

                        OnReportDiscrepancySOAPRequest?.Invoke(DateTime.UtcNow,
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
                                              (DateTime.UtcNow,
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

                    var HTTPResponse = new HTTPResponseBuilder(Request) {
                        HTTPStatusCode  = HTTPStatusCode.OK,
                        Server          = SOAPServer.DefaultServerName,
                        Date            = DateTime.UtcNow,
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
