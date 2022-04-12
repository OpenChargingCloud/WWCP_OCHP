/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

using org.GraphDefined.WWCP.OCHPv1_4.CPO;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.WebAPI
{

    /// <summary>
    /// OCHP+ HTTP API extention methods.
    /// </summary>
    public static class ExtensionMethods
    {

        #region ParseRoamingNetwork(this HTTPRequest, HTTPServer, out RoamingNetwork, out HTTPResponse)

        /// <summary>
        /// Parse the given HTTP request and return the roaming network
        /// for the given HTTP hostname and HTTP query parameter
        /// or an HTTP error response.
        /// </summary>
        /// <param name="HTTPRequest">A HTTP request.</param>
        /// <param name="HTTPServer">A HTTP server.</param>
        /// <param name="RoamingNetwork">The roaming network.</param>
        /// <param name="HTTPResponse">A HTTP error response.</param>
        /// <returns>True, when roaming network was found; false else.</returns>
        public static Boolean ParseRoamingNetwork(this HTTPRequest                             HTTPRequest,
                                                  HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer,
                                                  out RoamingNetwork                           RoamingNetwork,
                                                  out HTTPResponse                             HTTPResponse)
        {

            if (HTTPServer == null)
                Console.WriteLine("HTTPServer == null!");

            #region Initial checks

            if (HTTPRequest == null)
                throw new ArgumentNullException("HTTPRequest",  "The given HTTP request must not be null!");

            if (HTTPServer == null)
                throw new ArgumentNullException("HTTPServer",   "The given HTTP server must not be null!");

            #endregion

            RoamingNetwork_Id RoamingNetworkId;
                              RoamingNetwork    = null;
                              HTTPResponse      = null;

            if (HTTPRequest.ParsedURLParameters.Length < 1)
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                };

                return false;

            }

            if (!RoamingNetwork_Id.TryParse(HTTPRequest.ParsedURLParameters[0], out RoamingNetworkId))
            {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.BadRequest,
                    Server          = HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Invalid RoamingNetworkId!"" }".ToUTF8Bytes()
                };

                return false;

            }

            RoamingNetwork  = HTTPServer.
                                  GetAllTenants(HTTPRequest.Host).
                                  FirstOrDefault(roamingnetwork => roamingnetwork.Id == RoamingNetworkId);

            if (RoamingNetwork == null) {

                HTTPResponse = new HTTPResponse.Builder(HTTPRequest) {
                    HTTPStatusCode  = HTTPStatusCode.NotFound,
                    Server          = HTTPServer.DefaultServerName,
                    Date            = DateTime.Now,
                    ContentType     = HTTPContentType.JSON_UTF8,
                    Content         = @"{ ""description"": ""Unknown RoamingNetworkId!"" }".ToUTF8Bytes()
                };

                return false;

            }

            return true;

        }

        #endregion

    }


    public static class OCHPMapper
    {

        public static ChargePointInfo AsChargePointInfo(EVSE                          EVSE,
                                                        EVSE2ChargePointInfoDelegate  EVSE2EVSEDataRecord)
        {
            return null;
        }

    }


    /// <summary>
    /// A HTTP API providing OCHP+ data structures.
    /// </summary>
    public class OCHPWebAPI
    {

        #region Data

        /// <summary>
        /// The default HTTP URI prefix.
        /// </summary>
        public static readonly HTTPPath                      DefaultURLPathPrefix       = HTTPPath.Parse("/ext/OCHPPlus");

        /// <summary>
        /// The default HTTP realm, if HTTP Basic Authentication is used.
        /// </summary>
        public const           String                        DefaultHTTPRealm           = "Open Charging Cloud OCHPPlus WebAPI";

        //ToDo: http://www.iana.org/form/media-types

        /// <summary>
        /// The HTTP content type for serving OCHP+ XML data.
        /// </summary>
        public static readonly HTTPContentType               OCHPPlusXMLContentType     = new HTTPContentType("application", "vnd.OCHPPlus+xml", "utf-8", null, null);

        /// <summary>
        /// The HTTP content type for serving OCHP+ HTML data.
        /// </summary>
        public static readonly HTTPContentType               OCHPPlusHTMLContentType    = new HTTPContentType("application", "vnd.OCHPPlus+html", "utf-8", null, null);


        private readonly       XMLNamespacesDelegate         XMLNamespaces;
        private readonly       EVSE2ChargePointInfoDelegate  EVSE2ChargePointInfo;
        private readonly       ChargePointInfo2XMLDelegate   ChargePointInfo2XML;
        private readonly       EVSEStatus2XMLDelegate        EVSEStatusRecord2XML;
        private readonly       XMLPostProcessingDelegate     XMLPostProcessing;

        public static readonly HTTPEventSource_Id            DebugLogId                 = HTTPEventSource_Id.Parse("OCHPDebugLog");

        #endregion

        #region Properties

        /// <summary>
        /// The HTTP server for serving the OCHP+ WebAPI.
        /// </summary>
        public HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer          { get; }

        /// <summary>
        /// The HTTP URI prefix.
        /// </summary>
        public HTTPPath                                      URLPathPrefix      { get; }

        /// <summary>
        /// The HTTP realm, if HTTP Basic Authentication is used.
        /// </summary>
        public String                                        HTTPRealm          { get; }

        /// <summary>
        /// An enumeration of logins for an optional HTTP Basic Authentication.
        /// </summary>
        public IEnumerable<KeyValuePair<String, String>>     HTTPLogins         { get; }


        /// <summary>
        /// Send debug information via HTTP Server Sent Events.
        /// </summary>
        public HTTPEventSource<JObject>                      DebugLog           { get; }


        /// <summary>
        /// The DNS client to use.
        /// </summary>
        public DNSClient                                     DNSClient          { get; }


        private readonly List<WWCPEMPAdapter> _CPOAdapters;

        public IEnumerable<WWCPEMPAdapter> CPOAdapters
            => _CPOAdapters;

        #endregion

        #region Events

        #region Generic HTTP/SOAP server logging

        /// <summary>
        /// An event called whenever a HTTP request came in.
        /// </summary>
        public HTTPRequestLogEvent   RequestLog    = new HTTPRequestLogEvent();

        /// <summary>
        /// An event called whenever a HTTP request could successfully be processed.
        /// </summary>
        public HTTPResponseLogEvent  ResponseLog   = new HTTPResponseLogEvent();

        /// <summary>
        /// An event called whenever a HTTP request resulted in an error.
        /// </summary>
        public HTTPErrorLogEvent     ErrorLog      = new HTTPErrorLogEvent();

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Attach the OCHP+ WebAPI to the given HTTP server.
        /// </summary>
        /// <param name="HTTPServer">A HTTP server.</param>
        /// <param name="URLPathPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="HTTPRealm">The HTTP realm, if HTTP Basic Authentication is used.</param>
        /// <param name="HTTPLogins">An enumeration of logins for an optional HTTP Basic Authentication.</param>
        /// 
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        /// <param name="EVSE2ChargePointInfo">An optional delegate to process an EVSE data record before converting it to XML.</param>
        /// <param name="ChargePointInfo2XML">An optional delegate to process an EVSE data record XML before sending it somewhere.</param>
        /// <param name="EVSEStatus2XML">An optional delegate to process an EVSE status record XML before sending it somewhere.</param>
        /// <param name="XMLPostProcessing">An optional delegate to process the XML after its final creation.</param>
        public OCHPWebAPI(HTTPServer<RoamingNetworks, RoamingNetwork>  HTTPServer,
                          HTTPPath?                                    URLPathPrefix         = null,
                          String                                       HTTPRealm             = DefaultHTTPRealm,
                          IEnumerable<KeyValuePair<String, String>>    HTTPLogins            = null,

                          XMLNamespacesDelegate                        XMLNamespaces         = null,
                          EVSE2ChargePointInfoDelegate                 EVSE2ChargePointInfo  = null,
                          ChargePointInfo2XMLDelegate                  ChargePointInfo2XML   = null,
                          EVSEStatus2XMLDelegate                       EVSEStatus2XML        = null,
                          XMLPostProcessingDelegate                    XMLPostProcessing     = null)
        {

            this.HTTPServer            = HTTPServer    ?? throw new ArgumentNullException(nameof(HTTPServer), "The given HTTP server must not be null!");
            this.URLPathPrefix         = URLPathPrefix ?? DefaultURLPathPrefix;
            this.HTTPRealm             = HTTPRealm.IsNotNullOrEmpty() ? HTTPRealm : DefaultHTTPRealm;
            this.HTTPLogins            = HTTPLogins    ?? new KeyValuePair<String, String>[0];
            this.DNSClient             = HTTPServer.DNSClient;

            this.XMLNamespaces         = XMLNamespaces;
            this.EVSE2ChargePointInfo  = EVSE2ChargePointInfo;
            this.ChargePointInfo2XML   = ChargePointInfo2XML;
            this.EVSEStatusRecord2XML  = EVSEStatus2XML;
            this.XMLPostProcessing     = XMLPostProcessing;

            this._CPOAdapters          = new List<WWCPEMPAdapter>();

            // Link HTTP events...
            HTTPServer.RequestLog   += (HTTPProcessor, ServerTimestamp, Request)                                 => RequestLog. WhenAll(HTTPProcessor, ServerTimestamp, Request);
            HTTPServer.ResponseLog  += (HTTPProcessor, ServerTimestamp, Request, Response)                       => ResponseLog.WhenAll(HTTPProcessor, ServerTimestamp, Request, Response);
            HTTPServer.ErrorLog     += (HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException) => ErrorLog.   WhenAll(HTTPProcessor, ServerTimestamp, Request, Response, Error, LastException);

            var LogfilePrefix          = "HTTPSSEs" + Path.DirectorySeparatorChar;

            this.DebugLog              = HTTPServer.AddJSONEventSource(EventIdentification:      DebugLogId,
                                                                       URLTemplate:              this.URLPathPrefix + "/DebugLog",
                                                                       MaxNumberOfCachedEvents:  10000,
                                                                       RetryIntervall:           TimeSpan.FromSeconds(5),
                                                                       EnableLogging:            true,
                                                                       LogfilePrefix:            LogfilePrefix);

            RegisterURITemplates();

        }

        #endregion


        #region (private) RegisterURITemplates()

        private void RegisterURITemplates()
        {

            #region / (HTTPRoot)

            HTTPServer.RegisterResourcesFolder(HTTPHostname.Any,
                                               URLPathPrefix + "/",
                                               "org.GraphDefined.WWCP.OCHPv2_1.WebAPI.HTTPRoot",
                                               DefaultFilename: "index.html");

            #endregion


            #region GET     ~/ChargePoints

            #region EVSEsDelegate

            HTTPDelegate EVSEsDelegate = Request => {

                #region Check HTTP Basic Authentication

                if (Request.Authorization == null ||
                    !HTTPLogins.Any(kvp => kvp.Key   == (Request.Authorization as HTTPBasicAuthentication)?.Username &&
                                           kvp.Value == (Request.Authorization as HTTPBasicAuthentication)?.Password))
                {

                    return Task.FromResult(
                        new HTTPResponse.Builder(Request) {
                            HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                            WWWAuthenticate  = @"Basic realm=""" + HTTPRealm + @"""",
                            Server           = HTTPServer.DefaultServerName,
                            Date             = DateTime.Now,
                            Connection       = "close"
                        }.AsImmutable);

                }

                #endregion

                #region Check parameters

                HTTPResponse   _HTTPResponse;
                RoamingNetwork _RoamingNetwork;

                if (!Request.ParseRoamingNetwork(HTTPServer, out _RoamingNetwork, out _HTTPResponse))
                    return Task.FromResult(_HTTPResponse);

                #endregion

                var skip = Request.QueryString.GetUInt32("skip");
                var take = Request.QueryString.GetUInt32("take");

                //ToDo: Getting the expected total is very expensive!
                var _ExpectedCount = _RoamingNetwork.EVSEs.ULongCount();

                return Task.FromResult(
                    new HTTPResponse.Builder(Request) {
                        HTTPStatusCode                = HTTPStatusCode.OK,
                        Server                        = HTTPServer.DefaultServerName,
                        Date                          = DateTime.Now,
                        AccessControlAllowOrigin      = "*",
                        AccessControlAllowMethods     = "GET",
                        AccessControlAllowHeaders     = "Content-Type, Authorization",
                        ETag                          = "1",
                        ContentType                   = Request.Accept.FirstOrDefault()?.ContentType,
                        Content                       = _RoamingNetwork.EVSEs.
                                                            OrderBy(evse => evse.Id).
                                                            Skip   (skip).
                                                            Take   (take).
                                                            Select (evse => {

                                                                        try
                                                                        {
                                                                            return OCHPMapper.AsChargePointInfo(evse, EVSE2ChargePointInfo);
                                                                        }
#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
                                                                        catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
                                                                        { }

                                                                        return null;

                                                                    }).
                                                                    Where(evsedatarecord => evsedatarecord != null).
                                                                    ToXML(_RoamingNetwork, XMLNamespaces, ChargePointInfo2XML, XMLPostProcessing).
                                                                    ToUTF8Bytes(),
                        X_ExpectedTotalNumberOfItems  = _ExpectedCount
                    }.AsImmutable);

            };

            #endregion

            // ------------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/json" http://127.0.0.1:3004/RNs/Prod/ext/OCHPPlus/ChargePoints
            // ------------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URLPathPrefix + "/RNs/{RoamingNetworkId}" + URLPathPrefix + "/ChargePoints",
                                         HTTPContentType.XML_UTF8,
                                         HTTPDelegate: EVSEsDelegate);

            // -----------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/vnd.OCHPPlus+xml" http://127.0.0.1:3004/RNs/Prod/ChargePoints
            // -----------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URLPathPrefix + "/RNs/{RoamingNetworkId}/ChargePoints",
                                         OCHPPlusXMLContentType,
                                         HTTPDelegate: EVSEsDelegate);

            #endregion

            #region GET     ~/EVSEStatus

            #region XMLDelegate

            HTTPDelegate EVSEStatusXMLDelegate = Request => {

                #region Check HTTP Basic Authentication

                if (Request.Authorization == null ||
                    !HTTPLogins.Any(kvp => kvp.Key   == (Request.Authorization as HTTPBasicAuthentication)?.Username &&
                                           kvp.Value == (Request.Authorization as HTTPBasicAuthentication)?.Password))
                {

                    return Task.FromResult(
                        new HTTPResponse.Builder(Request) {
                            HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                            WWWAuthenticate  = @"Basic realm=""" + HTTPRealm + @"""",
                            Server           = HTTPServer.DefaultServerName,
                            Date             = DateTime.Now,
                            Connection       = "close"
                        }.AsImmutable);

                }

                #endregion

                #region Check parameters

                HTTPResponse   _HTTPResponse;
                RoamingNetwork _RoamingNetwork;

                if (!Request.ParseRoamingNetwork(HTTPServer, out _RoamingNetwork, out _HTTPResponse))
                    return Task.FromResult(_HTTPResponse);

                #endregion

                #region Check Query String parameters

                var skip          = Request.QueryString.GetUInt32("skip");
                var take          = Request.QueryString.GetUInt32("take");

                var majorStatus   = Request.QueryString.TryParseEnum<EVSEMajorStatusTypes>("majorstatus");
                var minorStatus   = Request.QueryString.TryParseEnum<EVSEMinorStatusTypes>("minorstatus");
                var WWCPStatus    = majorStatus.HasValue && minorStatus.HasValue
                                        ? new WWCP.EVSEStatusTypes?(OCHPv1_4.OCHPMapper.AsWWCPEVSEStatus(majorStatus.Value, minorStatus.Value))
                                        : new WWCP.EVSEStatusTypes?();

                var statusFilter  = WWCPStatus.HasValue
                                        ? Request.QueryString.CreateEnumFilter<EVSE, EVSEMajorStatusTypes>("majorstatus",
                                                                                                           (evse, status) => evse.Status == WWCPStatus.Value)
                                        : evse => true;

                var matchFilter   = Request.QueryString.CreateStringFilter<EVSE>  ("match",
                                                                                   (evse, match) => evse.Id.ToString().Contains(match) ||
                                                                                                    evse.ChargingStation.Name.FirstText().Contains(match));

                var sinceFilter   = Request.QueryString.CreateDateTimeFilter<EVSE>("since",
                                                                                   (evse, timestamp) => evse.Status.Timestamp >= timestamp);

                #endregion

                //ToDo: Getting the expected total is very expensive!
                var _ExpectedCount = _RoamingNetwork.EVSEStatus().ULongCount();

                return Task.FromResult(
                    new HTTPResponse.Builder(Request) {
                        HTTPStatusCode                = HTTPStatusCode.OK,
                        Server                        = HTTPServer.DefaultServerName,
                        Date                          = DateTime.Now,
                        AccessControlAllowOrigin      = "*",
                        AccessControlAllowMethods     = "GET",
                        AccessControlAllowHeaders     = "Content-Type, Authorization",
                        ETag                          = "1",
                        ContentType                   = Request.Accept.FirstOrDefault()?.ContentType,
                        Content                       = _RoamingNetwork.EVSEs.
                                                            Where  (statusFilter).
                                                            Where  (matchFilter).
                                                            Where  (sinceFilter).
                                                            OrderBy(evse => evse.Id).
                                                            Skip   (skip).
                                                            Take   (take).
                                                            ToXML  (_RoamingNetwork,
                                                                    XMLNamespaces,
                                                                    EVSEStatusRecord2XML,
                                                                    XMLPostProcessing).
                                                            ToUTF8Bytes(),
                        X_ExpectedTotalNumberOfItems  = _ExpectedCount
                    }.AsImmutable);

            };

            #endregion

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/xml" http://127.0.0.1:3004/RNs/Prod/ext/OCHPPlus/EVSEStatus
            // ---------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URLPathPrefix + "/RNs/{RoamingNetworkId}" + URLPathPrefix + "/EVSEStatus",
                                         HTTPContentType.XML_UTF8,
                                         HTTPDelegate: EVSEStatusXMLDelegate);

            // ---------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/vnd.OCHPPlus+xml" http://127.0.0.1:3004/RNs/Prod/EVSEStatus
            // ---------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEStatus",
                                         OCHPPlusXMLContentType,
                                         HTTPDelegate: EVSEStatusXMLDelegate);


            #region HTMLDelegate

            Func<EVSE, String> EVSEStatusColorSelector = evse =>
            {
                switch (evse.Status.Value.AsEVSEMinorStatus())
                {

                    case EVSEMinorStatusTypes.Available:
                        return @"style=""background-color: rgba(170, 232, 170, 0.55);""";

                    case EVSEMinorStatusTypes.Reserved:
                        return @"style=""background-color: rgba(170, 170, 232, 0.55);""";

                    case EVSEMinorStatusTypes.Charging:
                        return @"style=""background-color: rgba(232, 170, 170, 0.55);""";

                    case EVSEMinorStatusTypes.OutOfOrder:
                        return @"style=""background-color: rgba(170, 232, 232, 0.55);""";

                    case EVSEMinorStatusTypes.Blocked:
                        return @"style=""background-color: rgba(232, 232, 170, 0.55);""";

                    default:
                        return @"style=""background-color: rgba(100, 100, 100, 0.55);""";

                }
            };


            HTTPDelegate EVSEStatusHTMLDelegate = Request => {

                #region Check HTTP Basic Authentication

                if (Request.Authorization == null ||
                    !HTTPLogins.Any(kvp => kvp.Key   == (Request.Authorization as HTTPBasicAuthentication)?.Username &&
                                           kvp.Value == (Request.Authorization as HTTPBasicAuthentication)?.Password))
                {

                    return Task.FromResult(
                        new HTTPResponse.Builder(Request) {
                            HTTPStatusCode   = HTTPStatusCode.Unauthorized,
                            WWWAuthenticate  = @"Basic realm=""" + HTTPRealm + @"""",
                            Server           = HTTPServer.DefaultServerName,
                            Date             = DateTime.Now,
                            Connection       = "close"
                        }.AsImmutable);

                }

                #endregion

                #region Check parameters

                HTTPResponse    _HTTPResponse;
                RoamingNetwork  _RoamingNetwork;

                if (!Request.ParseRoamingNetwork(HTTPServer, out _RoamingNetwork, out _HTTPResponse))
                    return Task.FromResult(_HTTPResponse);

                #endregion

                #region Check Query String parameters

                var skip          = Request.QueryString.GetUInt32("skip");
                var take          = Request.QueryString.GetUInt32("take");

                var majorStatus   = Request.QueryString.TryParseEnum<EVSEMajorStatusTypes>("majorstatus");
                var minorStatus   = Request.QueryString.TryParseEnum<EVSEMinorStatusTypes>("minorstatus");
                var WWCPStatus    = majorStatus.HasValue && minorStatus.HasValue
                                        ? new WWCP.EVSEStatusTypes?(OCHPv1_4.OCHPMapper.AsWWCPEVSEStatus(majorStatus.Value, minorStatus.Value))
                                        : new WWCP.EVSEStatusTypes?();

                var statusFilter  = WWCPStatus.HasValue
                                        ? Request.QueryString.CreateEnumFilter<EVSE, EVSEMajorStatusTypes>("majorstatus",
                                                                                                           (evse, status) => evse.Status == WWCPStatus.Value)
                                        : evse => true;

                var matchFilter   = Request.QueryString.CreateStringFilter<EVSE>  ("match",
                                                                                   (evse, match) => evse.Id.ToString().Contains(match) ||
                                                                                                    evse.ChargingStation.Name.FirstText().Contains(match));

                var sinceFilter   = Request.QueryString.CreateDateTimeFilter<EVSE>("since",
                                                                                   (evse, timestamp) => evse.Status.Timestamp >= timestamp);

                #endregion

                //ToDo: Getting the expected total is very expensive!
                var _ExpectedCount = _RoamingNetwork.EVSEStatus().ULongCount();

                return Task.FromResult(
                    new HTTPResponse.Builder(Request) {
                        HTTPStatusCode                = HTTPStatusCode.OK,
                        Server                        = HTTPServer.DefaultServerName,
                        Date                          = DateTime.Now,
                        AccessControlAllowOrigin      = "*",
                        AccessControlAllowMethods     = "GET",
                        AccessControlAllowHeaders     = "Content-Type, Authorization",
                        ETag                          = "1",
                        ContentType                   = Request.Accept.FirstOrDefault()?.ContentType,
                        Content                       = String.Concat(@"<html>", Environment.NewLine,
                                                                      Environment.NewLine,

                                                                      @"  <head>", Environment.NewLine,
                                                                      @"    <link href=""" + URLPathPrefix + @"/styles.css"" type=""text/css"" rel=""stylesheet"" />", Environment.NewLine,
                                                                      @"  </head>", Environment.NewLine,
                                                                      Environment.NewLine,

                                                                      @"  <script>", Environment.NewLine,
                                                                      @"    function refresh() {", Environment.NewLine,
                                                                      @"        setTimeout(function () {", Environment.NewLine,
                                                                      @"            location.reload()", Environment.NewLine,
                                                                      @"        }, 10000);", Environment.NewLine,
                                                                      @"    }", Environment.NewLine,
                                                                      @"  </script>", Environment.NewLine,
                                                                      Environment.NewLine,

                                                                      @"  <body onLoad=""refresh()"">", Environment.NewLine,
                                                                      @"    <div class=""evsestatuslist"">", Environment.NewLine,
                                                                      Environment.NewLine,

                                                                      _RoamingNetwork.EVSEs.
                                                                          Where  (statusFilter).
                                                                          Where  (matchFilter).
                                                                          Where  (sinceFilter).
                                                                          OrderBy(evse => evse.Id).
                                                                          Skip   (skip).
                                                                          Take   (take).
                                                                          Select (evse => String.Concat(@"      <div class=""evsestatus"" " + EVSEStatusColorSelector(evse) + ">", Environment.NewLine,
                                                                                                        @"        <div class=""id"">",            evse.Id.ToString(),                    @"</div>", Environment.NewLine,
                                                                                                        @"        <div class=""description"">",   evse.ChargingStation.Name.FirstText(), @"</div>", Environment.NewLine,
                                                                                                        @"        <div class=""statuslist"">", Environment.NewLine,
                                                                                                        @"          <div class=""timestampedstatus"">", Environment.NewLine,
                                                                                                        @"            <div class=""timestamp"">",   evse.Status.Timestamp.ToString("dd.MM.yyyy HH:mm:ss"), @"</div>", Environment.NewLine,
                                                                                                        @"            <div class=""majorstatus"">", evse.Status.Value.AsEVSEMajorStatus().ToString(),      @"</div>", Environment.NewLine,
                                                                                                        @"            <div class=""minorstatus"">", evse.Status.Value.AsEVSEMinorStatus().ToString(),      @"</div>", Environment.NewLine,
                                                                                                        @"          </div>", Environment.NewLine,
                                                                                                        @"        </div>", Environment.NewLine,
                                                                                                        @"      </div>", Environment.NewLine
                                                                                                       )).
                                                                          AggregateWith(Environment.NewLine),
                                                                      Environment.NewLine,

                                                                      @"    </div>", Environment.NewLine,
                                                                      @"  </body>", Environment.NewLine,
                                                                      @"</html>", Environment.NewLine).
                                                               ToUTF8Bytes(),
                        X_ExpectedTotalNumberOfItems  = _ExpectedCount
                   }.AsImmutable);

            };

            #endregion

            // ---------------------------------------------------------------------------------------
            // curl -v -H "Accept: text/html" http://127.0.0.1:3004/RNs/Prod/ext/OCHPPlus/EVSEStatus
            // ---------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URLPathPrefix + "/RNs/{RoamingNetworkId}" + URLPathPrefix + "/EVSEStatus",
                                         HTTPContentType.HTML_UTF8,
                                         HTTPDelegate: EVSEStatusHTMLDelegate);

            // ----------------------------------------------------------------------------------------------
            // curl -v -H "Accept: application/vnd.OCHPPlus+html" http://127.0.0.1:3004/RNs/Prod/EVSEStatus
            // ----------------------------------------------------------------------------------------------
            HTTPServer.AddMethodCallback(HTTPHostname.Any,
                                         HTTPMethod.GET,
                                         URLPathPrefix + "/RNs/{RoamingNetworkId}/EVSEStatus",
                                         OCHPPlusHTMLContentType,
                                         HTTPDelegate: EVSEStatusHTMLDelegate);

            #endregion

        }

        #endregion


        public void Add(WWCPEMPAdapter CPOAdapter)
        {

            _CPOAdapters.Add(CPOAdapter);

            #region OnGetSingleRoamingAuthorisationRequest/-Response

            CPOAdapter.CPOClient.OnGetSingleRoamingAuthorisationRequest += async (LogTimestamp,
                                                                                  RequestTimestamp,
                                                                                  Sender,
                                                                                  SenderId,
                                                                                  EventTrackingId,
                                                                                  EMTId,
                                                                                  RequestTimeout) => await DebugLog.SubmitEvent("GetSingleRoamingAuthorisationRequest",
                                                                                                                                JSONObject.Create(
                                                                                                                                    new JProperty("timestamp",                   RequestTimestamp.    ToIso8601()),
                                                                                                                                    new JProperty("eventTrackingId",             EventTrackingId.     ToString()),
                                                                                                                                    //new JProperty("roamingNetworkId",            RoamingNetworkId.    ToString()),
                                                                                                                                    //EMPRoamingProviderId.HasValue
                                                                                                                                    //    ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                                                                                                                                    //    : null,
                                                                                                                                    //CSORoamingProviderId.HasValue
                                                                                                                                    //    ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                                                                                                                                    //    : null,
                                                                                                                                    new JProperty("EMTId",                       EMTId.               ToJSON()),
                                                                                                                                    new JProperty("requestTimeout",              Math.Round(RequestTimeout.TotalSeconds, 0))
                                                                                                                                ));

            CPOAdapter.CPOClient.OnGetSingleRoamingAuthorisationResponse += async (LogTimestamp,
                                                                                   RequestTimestamp,
                                                                                   Sender,
                                                                                   SenderId,
                                                                                   EventTrackingId,
                                                                                   EMTId,
                                                                                   RequestTimeout,
                                                                                   Result,
                                                                                   Runtime) => await DebugLog.SubmitEvent("GetSingleRoamingAuthorisationResponse",
                                                                                                                          JSONObject.Create(
                                                                                                                              new JProperty("timestamp",                   RequestTimestamp.    ToIso8601()),
                                                                                                                              new JProperty("eventTrackingId",             EventTrackingId.     ToString()),
                                                                                                                              //new JProperty("roamingNetworkId",            RoamingNetwork.Id.   ToString()),
                                                                                                                              //EMPRoamingProviderId.HasValue
                                                                                                                              //    ? new JProperty("EMPRoamingProviderId",  EMPRoamingProviderId.ToString())
                                                                                                                              //    : null,
                                                                                                                              //CSORoamingProviderId.HasValue
                                                                                                                              //    ? new JProperty("CSORoamingProviderId",  CSORoamingProviderId.ToString())
                                                                                                                              //    : null,
                                                                                                                              new JProperty("EMTId",                       EMTId.               ToJSON()),
                                                                                                                              new JProperty("requestTimeout",              Math.Round(RequestTimeout.TotalSeconds, 0)),
                                                                                                                              new JProperty("result",                      Result.              ToJSON()),
                                                                                                                              new JProperty("runtime",                     Math.Round(Runtime.TotalMilliseconds,   0))
                                                                                                                          ));

            #endregion

        }

    }

}
