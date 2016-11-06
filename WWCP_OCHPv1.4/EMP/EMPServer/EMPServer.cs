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
        public new const           String    DefaultHTTPServerName  = "GraphDefined OCHP " + Version.Number + " HTTP/SOAP/XML EMP Server API";

        /// <summary>
        /// The default HTTP/SOAP/XML server TCP port.
        /// </summary>
        public new static readonly IPPort    DefaultHTTPServerPort  = new IPPort(2602);

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

        #region EMPServer(HTTPServerName, TCPPort = null, URIPrefix = "", DNSClient = null, AutoStart = false)

        /// <summary>
        /// Initialize an new HTTP server for the OCHP HTTP/SOAP/XML EMP Server API using IPAddress.Any.
        /// </summary>
        /// <param name="HTTPServerName">An optional identification string for the HTTP server.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public EMPServer(String    HTTPServerName  = DefaultHTTPServerName,
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

        #region EMPServer(SOAPServer, URIPrefix = DefaultURIPrefix)

        /// <summary>
        /// Use the given HTTP server for the OCHP HTTP/SOAP/XML EMP Server API using IPAddress.Any.
        /// </summary>
        /// <param name="SOAPServer">A SOAP server.</param>
        /// <param name="URIPrefix">An optional prefix for the HTTP URIs.</param>
        public EMPServer(SOAPServer  SOAPServer,
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


            #region / - InformProviderMessage

            SOAPServer.RegisterSOAPDelegate(HTTPHostname.Any,
                                            URIPrefix + "/",
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

                    OnInformProviderHTTPRequest?.Invoke(DateTime.Now,
                                                        this.SOAPServer,
                                                        Request);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnInformProviderHTTPRequest));
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
                                          (DateTime.Now,
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
                                           DefaultQueryTimeout)).
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

                var HTTPResponse = new HTTPResponseBuilder(Request) {
                    HTTPStatusCode  = HTTPStatusCode.OK,
                    Server          = SOAPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.XMLTEXT_UTF8,
                    Content         = SOAP.Encapsulation(response.ToXML()).ToUTF8Bytes()
                };

                #endregion


                #region Send OnInformProviderHTTPResponse event

                try
                {

                    OnInformProviderHTTPResponse?.Invoke(HTTPResponse.Timestamp,
                                                         this.SOAPServer,
                                                         Request,
                                                         HTTPResponse);

                }
                catch (Exception e)
                {
                    e.Log(nameof(EMPServer) + "." + nameof(OnInformProviderHTTPResponse));
                }

                #endregion

                return HTTPResponse;

            });

            #endregion

        }

        #endregion


    }

}