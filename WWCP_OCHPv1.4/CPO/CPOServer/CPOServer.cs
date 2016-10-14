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

namespace org.GraphDefined.WWCP.OCHPv1_4
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

        #region OnRemoteReservationStart

        /// <summary>
        /// An event sent whenever a remote reservation start command was received.
        /// </summary>
        public event RequestLogHandler                 OnLogRemoteReservationStart;

        /// <summary>
        /// An event sent whenever a remote reservation start response was sent.
        /// </summary>
        public event AccessLogHandler                  OnLogRemoteReservationStarted;

        /// <summary>
        /// An event sent whenever a remote reservation start command was received.
        /// </summary>
        public event OnRemoteReservationStartDelegate  OnRemoteReservationStart;

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


        }

        #endregion


    }

}
