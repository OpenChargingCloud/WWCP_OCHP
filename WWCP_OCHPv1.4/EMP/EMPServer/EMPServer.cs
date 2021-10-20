/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHP EMP HTTP/SOAP/XML server.
    /// </summary>
    public class EMPServer : ASOAPServer
    {

        #region Data

        /// <summary>
        /// The default HTTP/SOAP/XML server name.
        /// </summary>
        public new const           String           DefaultHTTPServerName  = "GraphDefined OCHP " + Version.Number + " HTTP/SOAP/XML EMP API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort           DefaultHTTPServerPort  = IPPort.Parse(2602);

        /// <summary>
        /// The default HTTP/SOAP/XML server URI prefix.
        /// </summary>
        public new static readonly HTTPPath          DefaultURLPrefix       = HTTPPath.Parse("/");

        /// <summary>
        /// The default HTTP/SOAP/XML server URI suffix.
        /// </summary>
        public     static readonly HTTPPath          DefaultURLSuffix       = HTTPPath.Parse("/OCHP");

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
        public HTTPPath URLSuffix           { get; }

        #endregion

        #region Events

        #region OnInformProvider

        /// <summary>
        /// An event sent whenever an inform provider SOAP request was received.
        /// </summary>
        public event RequestLogHandler         OnInformProviderHTTPRequest;

        /// <summary>
        /// An event sent whenever an inform provider SOAP response was sent.
        /// </summary>
        public event AccessLogHandler          OnInformProviderHTTPResponse;

        /// <summary>
        /// An event sent whenever an inform provider request was received.
        /// </summary>
        public event OnInformProviderDelegate  OnInformProviderRequest;

        #endregion

        #endregion

        #region Constructor(s)

        #region EMPServer(HTTPServerName, ServiceName = null, TCPPort = default, URLPrefix = default, URLSuffix = default, ContentType = default, DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OCHP HTTP/SOAP/XML EMP Server API.
        /// </summary>
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServiceName">An optional identification for this SOAP service.</param>
        /// <param name="URLPrefix">An optional prefix for the HTTP URLs.</param>
        /// <param name="URLSuffix">An optional HTTP/SOAP/XML server URI suffix.</param>
        /// <param name="ContentType">An optional HTTP content type to use.</param>
        /// <param name="RegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="ServerLoggingPath">The logging path.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and logfile name.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public EMPServer(String                  ServerName                = DefaultHTTPServerName,
                         IPPort?                 TCPPort                   = null,
                         String                  ServiceName               = null,
                         HTTPPath?               URLPrefix                 = null,
                         HTTPPath?               URLSuffix                 = null,
                         HTTPContentType         ContentType               = null,
                         Boolean                 RegisterHTTPRootService   = true,
                         String                  ServerLoggingPath         = null,
                         String                  ServerLoggingContext      = EMPServerLogger.DefaultContext,
                         LogfileCreatorDelegate  LogfileCreator            = null,
                         DNSClient               DNSClient                 = null,
                         Boolean                 AutoStart                 = false)

            : base(ServerName.IsNotNullOrEmpty() ? ServerName : DefaultHTTPServerName,
                   TCPPort     ?? DefaultHTTPServerPort,
                   ServiceName ?? "OCHP " + Version.Number + " " + nameof(EMPServer),
                   URLPrefix   ?? DefaultURLPrefix,
                   ContentType ?? DefaultContentType,
                   RegisterHTTPRootService,
                   DNSClient,
                   AutoStart: false)

        {

            this.ServiceId    = ServiceName ?? "OCHP " + Version.Number + " " + nameof(EMPServer);
            this.URLSuffix    = URLSuffix   ?? DefaultURLSuffix;
            this.HTTPLogger   = new EMPServerLogger(this,
                                                    ServerLoggingPath,
                                                    ServerLoggingContext ?? CPOServerLogger.DefaultContext,
                                                    LogfileCreator);

            RegisterURITemplates();

            if (AutoStart)
                Start();

        }

        #endregion

        #region EMPServer(SOAPServer, ServiceName = null, URLPrefix = default, URLSuffix = default)

        /// <summary>
        /// Use the given HTTP server for the OCHP HTTP/SOAP/XML EMP Server API.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="ServiceName">An optional identification for this SOAP service.</param>
        /// <param name="URLPrefix">An optional prefix for the HTTP URLs.</param>
        /// <param name="URLSuffix">An optional HTTP/SOAP/XML server URI suffix.</param>
        public EMPServer(SOAPServer  SOAPServer,
                         String      ServiceName   = null,
                         HTTPPath?   URLPrefix     = null,
                         HTTPPath?   URLSuffix     = null)

            : base(SOAPServer,
                   URLPrefix ?? DefaultURLPrefix)

        {

            this.ServiceId  = ServiceName ?? "OCHP " + Version.Number + " " + nameof(EMPServer);
            this.URLSuffix  = URLSuffix   ?? DefaultURLSuffix;

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

            #region / - InformProviderMessage

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URLPrefix + URLSuffix,
                                            "InformProviderMessage",
                                            XML => XML.Descendants(OCHPNS.Default + "InformProviderMessage").FirstOrDefault(),
                                            async (Request, InformProviderMessageXML) => {

                #region Documentation

                // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
                //                   xmlns:ns      = "http://ochp.eu/1.4">
                //
                //    <soapenv:Header/>
                //    <soapenv:Body>
                //       <ns:InformProviderMessage>
                //
                //          <ns:message>
                //             <ns:message>?</ns:message>
                //          </ns:message>
                //
                //          <ns:evseId>?</ns:evseId>
                //          <ns:contractId>?</ns:contractId>
                //          <ns:directId>?</ns:directId>
                //
                //          <!--Optional:-->
                //          <ns:ttl>
                //             <ns:DateTime>?</ns:DateTime>
                //          </ns:ttl>
                //
                //          <!--Optional:-->
                //          <ns:stateOfCharge>?</ns:stateOfCharge>
                //
                //          <!--Optional:-->
                //          <ns:maxPower>?</ns:maxPower>
                //
                //          <!--Optional:-->
                //          <ns:maxCurrent>?</ns:maxCurrent>
                //
                //          <!--Optional:-->
                //          <ns:onePhase>?</ns:onePhase>
                //
                //          <!--Optional:-->
                //          <ns:maxEnergy>?</ns:maxEnergy>
                //
                //          <!--Optional:-->
                //          <ns:minEnergy>?</ns:minEnergy>
                //
                //          <!--Optional:-->
                //          <ns:departure>
                //             <ns:DateTime>?</ns:DateTime>
                //          </ns:departure>
                //
                //          <!--Optional:-->
                //          <ns:currentPower>?</ns:currentPower>
                //
                //          <!--Optional:-->
                //          <ns:chargedEnergy>?</ns:chargedEnergy>
                //
                //          <!--Optional:-->
                //          <ns:meterReading>
                //             <ns:meterValue>?</ns:meterValue>
                //             <ns:meterTime>
                //                <ns:LocalDateTime>?</ns:LocalDateTime>
                //             </ns:meterTime>
                //          </ns:meterReading>
                //
                //          <!--Zero or more repetitions:-->
                //          <ns:chargingPeriods>
                //
                //             <ns:startDateTime>
                //                <ns:LocalDateTime>?</ns:LocalDateTime>
                //             </ns:startDateTime>
                //
                //             <ns:endDateTime>
                //                <ns:LocalDateTime>?</ns:LocalDateTime>
                //             </ns:endDateTime>
                //
                //             <ns:billingItem>
                //                <ns:BillingItemType>?</ns:BillingItemType>
                //             </ns:billingItem>
                //
                //             <ns:billingValue>?</ns:billingValue>
                //             <ns:itemPrice>?</ns:itemPrice>
                //
                //             <!--Optional:-->
                //             <ns:periodCost>?</ns:periodCost>
                //
                //             <!--Optional:-->
                //             <ns:taxrate>?</ns:taxrate>
                //
                //          </ns:chargingPeriods>
                //
                //          <!--Optional:-->
                //          <ns:currentCost>?</ns:currentCost>
                //
                //          <!--Optional:-->
                //          <ns:currency>?</ns:currency>
                //
                //       </ns:InformProviderMessage>
                //    </soapenv:Body>
                // </soapenv:Envelope>

                #endregion

                #region Send OnInformProviderHTTPRequest event

                try
                {

                    OnInformProviderHTTPRequest?.Invoke(DateTime.UtcNow,
                                                        this.SOAPServer.HTTPServer,
                                                        Request);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, nameof(EMPServer) + "." + nameof(OnInformProviderHTTPRequest));
                }

                #endregion


                var _InformProviderRequest = InformProviderRequest.Parse(InformProviderMessageXML);

                InformProviderResponse response            = null;


                #region Call async subscribers

                if (response == null)
                {

                    var results = OnInformProviderRequest?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnInformProviderDelegate)
                                          (DateTime.UtcNow,
                                           this,
                                           Request.CancellationToken,
                                           Request.EventTrackingId,
                                           _InformProviderRequest.DirectMessage,
                                           _InformProviderRequest.EVSEId,
                                           _InformProviderRequest.ContractId,
                                           _InformProviderRequest.DirectId,

                                           _InformProviderRequest.SessionTimeoutAt,
                                           _InformProviderRequest.StateOfCharge,
                                           _InformProviderRequest.MaxPower,
                                           _InformProviderRequest.MaxCurrent,
                                           _InformProviderRequest.OnePhase,
                                           _InformProviderRequest.MaxEnergy,
                                           _InformProviderRequest.MinEnergy,
                                           _InformProviderRequest.Departure,
                                           _InformProviderRequest.CurrentPower,
                                           _InformProviderRequest.ChargedEnergy,
                                           _InformProviderRequest.MeterReading,
                                           _InformProviderRequest.ChargingPeriods,
                                           _InformProviderRequest.CurrentCost,
                                           _InformProviderRequest.Currency,
                                           DefaultRequestTimeout)).
                                      ToArray();

                    if (results.Length > 0)
                    {

                        await Task.WhenAll(results);

                        response = results.FirstOrDefault()?.Result;

                    }

                    if (results.Length == 0 || response == null)
                        response = InformProviderResponse.Server(_InformProviderRequest, "Could not process the incoming InformProvider request!");

                }

                #endregion

                #region Create SOAPResponse

                var HTTPResponse = new HTTPResponse.Builder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.HTTPServer.DefaultServerName,
                    Date            = DateTime.UtcNow,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnInformProviderHTTPResponse event

                try
                {

                    OnInformProviderHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                         this.SOAPServer.HTTPServer,
                                                         Request,
                                                         HTTPResponse);

                }
                catch (Exception e)
                {
                    DebugX.LogException(e, nameof(EMPServer) + "." + nameof(OnInformProviderHTTPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

        }

        #endregion


    }

}
