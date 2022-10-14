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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.CPO
{

    /// <summary>
    /// The OCHP CPO client.
    /// </summary>
    public partial class CPOClient : ASOAPClient
    {

        /// <summary>
        /// The OCHP CPO HTTP/SOAP client logger.
        /// </summary>
        public class Logger : HTTPClientLogger
        {

            #region Data

            /// <summary>
            /// The default context for this logger.
            /// </summary>
            public const String DefaultContext = "OCHP_CPOClient";

            #endregion

            #region Properties

            /// <summary>
            /// The attached OCHP CPO client.
            /// </summary>
            public ICPOClient  CPOClient   { get; }

            #endregion

            #region Constructor(s)

            #region Logger(CPOClient, Context = DefaultContext, LogfileCreator = null)

            /// <summary>
            /// Create a new OCHP CPO client logger using the default logging delegates.
            /// </summary>
            /// <param name="CPOClient">A OCHP CPO client.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public Logger(CPOClient                CPOClient,
                          String                   LoggingPath,
                          String                   Context          = DefaultContext,
                          LogfileCreatorDelegate?  LogfileCreator   = null)

                : this(CPOClient,
                       LoggingPath,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                       null,
                       null,
                       null,
                       null,

                       LogfileCreator: LogfileCreator)

            { }

            #endregion

            #region Logger(CPOClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new OCHP CPO client logger using the given logging delegates.
            /// </summary>
            /// <param name="CPOClient">A OCHP CPO client.</param>
            /// <param name="LoggingPath">The logging path.</param>
            /// <param name="Context">A context of this API.</param>
            /// 
            /// <param name="LogHTTPRequest_toConsole">A delegate to log incoming HTTP requests to console.</param>
            /// <param name="LogHTTPResponse_toConsole">A delegate to log HTTP requests/responses to console.</param>
            /// <param name="LogHTTPRequest_toDisc">A delegate to log incoming HTTP requests to disc.</param>
            /// <param name="LogHTTPResponse_toDisc">A delegate to log HTTP requests/responses to disc.</param>
            /// 
            /// <param name="LogHTTPRequest_toNetwork">A delegate to log incoming HTTP requests to a network target.</param>
            /// <param name="LogHTTPResponse_toNetwork">A delegate to log HTTP requests/responses to a network target.</param>
            /// <param name="LogHTTPRequest_toHTTPSSE">A delegate to log incoming HTTP requests to a HTTP client sent events source.</param>
            /// <param name="LogHTTPResponse_toHTTPSSE">A delegate to log HTTP requests/responses to a HTTP client sent events source.</param>
            /// 
            /// <param name="LogHTTPError_toConsole">A delegate to log HTTP errors to console.</param>
            /// <param name="LogHTTPError_toDisc">A delegate to log HTTP errors to disc.</param>
            /// <param name="LogHTTPError_toNetwork">A delegate to log HTTP errors to a network target.</param>
            /// <param name="LogHTTPError_toHTTPSSE">A delegate to log HTTP errors to a HTTP client sent events source.</param>
            /// 
            /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
            public Logger(ICPOClient                   CPOClient,
                          String                       LoggingPath,
                          String                       Context,

                          HTTPRequestLoggerDelegate?   LogHTTPRequest_toConsole    = null,
                          HTTPResponseLoggerDelegate?  LogHTTPResponse_toConsole   = null,
                          HTTPRequestLoggerDelegate?   LogHTTPRequest_toDisc       = null,
                          HTTPResponseLoggerDelegate?  LogHTTPResponse_toDisc      = null,

                          HTTPRequestLoggerDelegate?   LogHTTPRequest_toNetwork    = null,
                          HTTPResponseLoggerDelegate?  LogHTTPResponse_toNetwork   = null,
                          HTTPRequestLoggerDelegate?   LogHTTPRequest_toHTTPSSE    = null,
                          HTTPResponseLoggerDelegate?  LogHTTPResponse_toHTTPSSE   = null,

                          HTTPResponseLoggerDelegate?  LogHTTPError_toConsole      = null,
                          HTTPResponseLoggerDelegate?  LogHTTPError_toDisc         = null,
                          HTTPResponseLoggerDelegate?  LogHTTPError_toNetwork      = null,
                          HTTPResponseLoggerDelegate?  LogHTTPError_toHTTPSSE      = null,

                          LogfileCreatorDelegate?      LogfileCreator              = null)

                : base(CPOClient,
                       LoggingPath,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,

                       LogHTTPRequest_toConsole,
                       LogHTTPResponse_toConsole,
                       LogHTTPRequest_toDisc,
                       LogHTTPResponse_toDisc,

                       LogHTTPRequest_toNetwork,
                       LogHTTPResponse_toNetwork,
                       LogHTTPRequest_toHTTPSSE,
                       LogHTTPResponse_toHTTPSSE,

                       LogHTTPError_toConsole,
                       LogHTTPError_toDisc,
                       LogHTTPError_toNetwork,
                       LogHTTPError_toHTTPSSE,

                       LogfileCreator)

            {

                #region Initial checks

                this.CPOClient = CPOClient ?? throw new ArgumentNullException(nameof(CPOClient), "The given CPO client must not be null!");

                #endregion


                // OCHP

                #region SetChargePointList

                RegisterEvent("SetChargePointListRequest",
                              handler => CPOClient.OnSetChargePointListSOAPRequest += handler,
                              handler => CPOClient.OnSetChargePointListSOAPRequest -= handler,
                              "SetChargePointList", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SetChargePointListResponse",
                              handler => CPOClient.OnSetChargePointListSOAPResponse += handler,
                              handler => CPOClient.OnSetChargePointListSOAPResponse -= handler,
                              "SetChargePointList", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region UpdateChargePointList

                RegisterEvent("UpdateChargePointListRequest",
                              handler => CPOClient.OnUpdateChargePointListSOAPRequest += handler,
                              handler => CPOClient.OnUpdateChargePointListSOAPRequest -= handler,
                              "UpdateChargePointList", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("UpdateChargePointListResponse",
                              handler => CPOClient.OnUpdateChargePointListSOAPResponse += handler,
                              handler => CPOClient.OnUpdateChargePointListSOAPResponse -= handler,
                              "UpdateChargePointList", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region UpdateStatusRequest

                RegisterEvent("UpdateStatusRequest",
                              handler => CPOClient.OnUpdateStatusSOAPRequest += handler,
                              handler => CPOClient.OnUpdateStatusSOAPRequest -= handler,
                              "UpdateStatus", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("UpdateStatusResponse",
                              handler => CPOClient.OnUpdateStatusSOAPResponse += handler,
                              handler => CPOClient.OnUpdateStatusSOAPResponse -= handler,
                              "UpdateStatus", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetSingleRoamingAuthorisation

                RegisterEvent("GetSingleRoamingAuthorisationRequest",
                              handler => CPOClient.OnGetSingleRoamingAuthorisationSOAPRequest += handler,
                              handler => CPOClient.OnGetSingleRoamingAuthorisationSOAPRequest -= handler,
                              "GetSingleRoamingAuthorisation", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetSingleRoamingAuthorisationResponse",
                              handler => CPOClient.OnGetSingleRoamingAuthorisationSOAPResponse += handler,
                              handler => CPOClient.OnGetSingleRoamingAuthorisationSOAPResponse -= handler,
                              "GetSingleRoamingAuthorisation", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetRoamingAuthorisationList

                RegisterEvent("GetRoamingAuthorisationListRequest",
                              handler => CPOClient.OnGetRoamingAuthorisationListSOAPRequest += handler,
                              handler => CPOClient.OnGetRoamingAuthorisationListSOAPRequest -= handler,
                              "GetRoamingAuthorisationList", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetRoamingAuthorisationListResponse",
                              handler => CPOClient.OnGetRoamingAuthorisationListSOAPResponse += handler,
                              handler => CPOClient.OnGetRoamingAuthorisationListSOAPResponse -= handler,
                              "GetRoamingAuthorisationList", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetRoamingAuthorisationListUpdates

                RegisterEvent("GetRoamingAuthorisationListUpdatesRequest",
                              handler => CPOClient.OnGetRoamingAuthorisationListUpdatesSOAPRequest += handler,
                              handler => CPOClient.OnGetRoamingAuthorisationListUpdatesSOAPRequest -= handler,
                              "GetRoamingAuthorisationListUpdates", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetRoamingAuthorisationListUpdatesResponse",
                              handler => CPOClient.OnGetRoamingAuthorisationListUpdatesSOAPResponse += handler,
                              handler => CPOClient.OnGetRoamingAuthorisationListUpdatesSOAPResponse -= handler,
                              "GetRoamingAuthorisationListUpdates", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region AddCDRs

                RegisterEvent("AddCDRsRequest",
                              handler => CPOClient.OnAddCDRsSOAPRequest += handler,
                              handler => CPOClient.OnAddCDRsSOAPRequest -= handler,
                              "AddCDRs", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AddCDRsResponse",
                              handler => CPOClient.OnAddCDRsSOAPResponse += handler,
                              handler => CPOClient.OnAddCDRsSOAPResponse -= handler,
                              "AddCDRs", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region CheckCDRs

                RegisterEvent("CheckCDRsRequest",
                              handler => CPOClient.OnCheckCDRsSOAPRequest += handler,
                              handler => CPOClient.OnCheckCDRsSOAPRequest -= handler,
                              "CheckCDRs", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("CheckCDRsResponse",
                              handler => CPOClient.OnCheckCDRsSOAPResponse += handler,
                              handler => CPOClient.OnCheckCDRsSOAPResponse -= handler,
                              "CheckCDRs", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region UpdateTariffs

                RegisterEvent("UpdateTariffsRequest",
                              handler => CPOClient.OnUpdateTariffsSOAPRequest += handler,
                              handler => CPOClient.OnUpdateTariffsSOAPRequest -= handler,
                              "UpdateTariffs", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("UpdateTariffsResponse",
                              handler => CPOClient.OnUpdateTariffsSOAPResponse += handler,
                              handler => CPOClient.OnUpdateTariffsSOAPResponse -= handler,
                              "UpdateTariffs", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion


                // OCHPdirect

                #region AddServiceEndpoints

                RegisterEvent("AddServiceEndpointsRequest",
                              handler => CPOClient.OnAddServiceEndpointsSOAPRequest += handler,
                              handler => CPOClient.OnAddServiceEndpointsSOAPRequest -= handler,
                              "AddServiceEndpoints", "OCHPdirect", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AddServiceEndpointsResponse",
                              handler => CPOClient.OnAddServiceEndpointsSOAPResponse += handler,
                              handler => CPOClient.OnAddServiceEndpointsSOAPResponse -= handler,
                              "AddServiceEndpoints", "OCHPdirect", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetServiceEndpoints

                RegisterEvent("GetServiceEndpointsRequest",
                              handler => CPOClient.OnGetServiceEndpointsSOAPRequest += handler,
                              handler => CPOClient.OnGetServiceEndpointsSOAPRequest -= handler,
                              "GetServiceEndpoints", "OCHPdirect", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetServiceEndpointsResponse",
                              handler => CPOClient.OnGetServiceEndpointsSOAPResponse += handler,
                              handler => CPOClient.OnGetServiceEndpointsSOAPResponse -= handler,
                              "GetServiceEndpoints", "OCHPdirect", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region InformProvider

                RegisterEvent("InformProviderRequest",
                              handler => CPOClient.OnInformProviderSOAPRequest += handler,
                              handler => CPOClient.OnInformProviderSOAPRequest -= handler,
                              "InformProvider", "OCHPdirect", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("InformProviderResponse",
                              handler => CPOClient.OnInformProviderSOAPResponse += handler,
                              handler => CPOClient.OnInformProviderSOAPResponse -= handler,
                              "InformProvider", "OCHPdirect", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion


            }

            #endregion

            #endregion

        }

     }

}
