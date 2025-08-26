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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.CH
{

    /// <summary>
    /// An OCHP CH server logger.
    /// </summary>
    public class CHServerLogger : HTTPServerLoggerX
    {

        #region Data

        /// <summary>
        /// The default context for this logger.
        /// </summary>
        public const String DefaultContext = "OCHP_CHServer";

        #endregion

        #region Properties

        /// <summary>
        /// The linked OCHP CH server.
        /// </summary>
        public CHServer CHServer { get; }

        #endregion

        #region Constructor(s)

        #region CHServerLogger(CHServer, Context = DefaultContext, LogfileCreator = null)

        /// <summary>
        /// Create a new OCHP CH server logger using the default logging delegates.
        /// </summary>
        /// <param name="CHServer">A OCHP CH server.</param>
        /// <param name="LoggingPath">The logging path.</param>
        /// <param name="Context">A context of this API.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CHServerLogger(CHServer                 CHServer,
                              String                   LoggingPath,
                              String                   Context         = DefaultContext,
                              LogfileCreatorDelegate?  LogfileCreator  = null)

            : this(CHServer,
                   LoggingPath,
                   Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                   null,
                   null,
                   null,
                   null,
                   LogfileCreator: LogfileCreator)

        { }

        #endregion

        #region CHServerLogger(CHServer, Context, ... Logging delegates ...)

        /// <summary>
        /// Create a new OCHP CH server logger using the given logging delegates.
        /// </summary>
        /// <param name="CHServer">A OCHP CH server.</param>
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
        /// <param name="LogHTTPRequest_toHTTPSSE">A delegate to log incoming HTTP requests to a HTTP server sent events source.</param>
        /// <param name="LogHTTPResponse_toHTTPSSE">A delegate to log HTTP requests/responses to a HTTP server sent events source.</param>
        /// 
        /// <param name="LogHTTPError_toConsole">A delegate to log HTTP errors to console.</param>
        /// <param name="LogHTTPError_toDisc">A delegate to log HTTP errors to disc.</param>
        /// <param name="LogHTTPError_toNetwork">A delegate to log HTTP errors to a network target.</param>
        /// <param name="LogHTTPError_toHTTPSSE">A delegate to log HTTP errors to a HTTP server sent events source.</param>
        /// 
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CHServerLogger(CHServer                     CHServer,
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

            : base(CHServer.SOAPServer.HTTPServer,
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

            if (CHServer is null)
                throw new ArgumentNullException(nameof(CHServer), "The given CH server must not be null!");

            this.CHServer = CHServer;

            #endregion


            // Shared event logs...

            #region AddServiceEndpoints

            RegisterEvent2("AddServiceEndpointsRequest",
                          handler => CHServer.OnAddServiceEndpointsSOAPRequest += handler,
                          handler => CHServer.OnAddServiceEndpointsSOAPRequest -= handler,
                          "AddServiceEndpoints", "OCHPdirect", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("AddServiceEndpointsResponse",
                          handler => CHServer.OnAddServiceEndpointsSOAPResponse += handler,
                          handler => CHServer.OnAddServiceEndpointsSOAPResponse -= handler,
                          "AddServiceEndpoints", "OCHPdirect", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region GetServiceEndpoints

            RegisterEvent2("GetServiceEndpointsRequest",
                          handler => CHServer.OnGetServiceEndpointsSOAPRequest += handler,
                          handler => CHServer.OnGetServiceEndpointsSOAPRequest -= handler,
                          "GetServiceEndpoints", "OCHPdirect", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("GetServiceEndpointsResponse",
                          handler => CHServer.OnGetServiceEndpointsSOAPResponse += handler,
                          handler => CHServer.OnGetServiceEndpointsSOAPResponse -= handler,
                          "GetServiceEndpoints", "OCHPdirect", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion


            // CPO event logs...

            #region SetChargePointList

            RegisterEvent2("SetChargePointListRequest",
                          handler => CHServer.OnSetChargePointListSOAPRequest += handler,
                          handler => CHServer.OnSetChargePointListSOAPRequest -= handler,
                          "SetChargePointList", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("SetChargePointListResponse",
                          handler => CHServer.OnSetChargePointListSOAPResponse += handler,
                          handler => CHServer.OnSetChargePointListSOAPResponse -= handler,
                          "SetChargePointList", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region UpdateChargePointList

            RegisterEvent2("UpdateChargePointListRequest",
                          handler => CHServer.OnUpdateChargePointListSOAPRequest += handler,
                          handler => CHServer.OnUpdateChargePointListSOAPRequest -= handler,
                          "UpdateChargePointList", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("UpdateChargePointListResponse",
                          handler => CHServer.OnUpdateChargePointListSOAPResponse += handler,
                          handler => CHServer.OnUpdateChargePointListSOAPResponse -= handler,
                          "UpdateChargePointList", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region UpdateStatusRequest

            RegisterEvent2("UpdateStatusRequest",
                          handler => CHServer.OnUpdateStatusSOAPRequest += handler,
                          handler => CHServer.OnUpdateStatusSOAPRequest -= handler,
                          "UpdateStatus", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("UpdateStatusResponse",
                          handler => CHServer.OnUpdateStatusSOAPResponse += handler,
                          handler => CHServer.OnUpdateStatusSOAPResponse -= handler,
                          "UpdateStatus", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region GetSingleRoamingAuthorisation

            RegisterEvent2("GetSingleRoamingAuthorisationRequest",
                          handler => CHServer.OnGetSingleRoamingAuthorisationSOAPRequest += handler,
                          handler => CHServer.OnGetSingleRoamingAuthorisationSOAPRequest -= handler,
                          "GetSingleRoamingAuthorisation", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("GetSingleRoamingAuthorisationResponse",
                          handler => CHServer.OnGetSingleRoamingAuthorisationSOAPResponse += handler,
                          handler => CHServer.OnGetSingleRoamingAuthorisationSOAPResponse -= handler,
                          "GetSingleRoamingAuthorisation", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region GetRoamingAuthorisationList

            RegisterEvent2("GetRoamingAuthorisationListRequest",
                          handler => CHServer.OnGetRoamingAuthorisationListSOAPRequest += handler,
                          handler => CHServer.OnGetRoamingAuthorisationListSOAPRequest -= handler,
                          "GetRoamingAuthorisationList", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("GetRoamingAuthorisationListResponse",
                          handler => CHServer.OnGetRoamingAuthorisationListSOAPResponse += handler,
                          handler => CHServer.OnGetRoamingAuthorisationListSOAPResponse -= handler,
                          "GetRoamingAuthorisationList", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region GetRoamingAuthorisationListUpdates

            RegisterEvent2("GetRoamingAuthorisationListUpdatesRequest",
                          handler => CHServer.OnGetRoamingAuthorisationListUpdatesSOAPRequest += handler,
                          handler => CHServer.OnGetRoamingAuthorisationListUpdatesSOAPRequest -= handler,
                          "GetRoamingAuthorisationListUpdates", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("GetRoamingAuthorisationListUpdatesResponse",
                          handler => CHServer.OnGetRoamingAuthorisationListUpdatesSOAPResponse += handler,
                          handler => CHServer.OnGetRoamingAuthorisationListUpdatesSOAPResponse -= handler,
                          "GetRoamingAuthorisationListUpdates", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region AddCDRs

            RegisterEvent2("AddCDRsRequest",
                          handler => CHServer.OnAddCDRsSOAPRequest += handler,
                          handler => CHServer.OnAddCDRsSOAPRequest -= handler,
                          "AddCDRs", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("AddCDRsResponse",
                          handler => CHServer.OnAddCDRsSOAPResponse += handler,
                          handler => CHServer.OnAddCDRsSOAPResponse -= handler,
                          "AddCDRs", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region CheckCDRs

            RegisterEvent2("CheckCDRsRequest",
                          handler => CHServer.OnCheckCDRsSOAPRequest += handler,
                          handler => CHServer.OnCheckCDRsSOAPRequest -= handler,
                          "CheckCDRs", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("CheckCDRsResponse",
                          handler => CHServer.OnCheckCDRsSOAPResponse += handler,
                          handler => CHServer.OnCheckCDRsSOAPResponse -= handler,
                          "CheckCDRs", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region UpdateTariffs

            RegisterEvent2("UpdateTariffsRequest",
                          handler => CHServer.OnUpdateTariffsSOAPRequest += handler,
                          handler => CHServer.OnUpdateTariffsSOAPRequest -= handler,
                          "UpdateTariffs", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("UpdateTariffsResponse",
                          handler => CHServer.OnUpdateTariffsSOAPResponse += handler,
                          handler => CHServer.OnUpdateTariffsSOAPResponse -= handler,
                          "UpdateTariffs", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion


            // EMP event logs...

            #region GetChargePointList

            RegisterEvent2("GetChargePointListRequest",
                          handler => CHServer.OnGetChargePointListSOAPRequest += handler,
                          handler => CHServer.OnGetChargePointListSOAPRequest -= handler,
                          "GetChargePointList", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("GetChargePointListResponse",
                          handler => CHServer.OnGetChargePointListSOAPResponse += handler,
                          handler => CHServer.OnGetChargePointListSOAPResponse -= handler,
                          "GetChargePointList", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region GetChargePointListUpdates

            RegisterEvent2("GetChargePointListUpdatesRequest",
                          handler => CHServer.OnGetChargePointListUpdatesSOAPRequest += handler,
                          handler => CHServer.OnGetChargePointListUpdatesSOAPRequest -= handler,
                          "GetChargePointListUpdates", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("GetChargePointListUpdatesResponse",
                          handler => CHServer.OnGetChargePointListUpdatesSOAPResponse += handler,
                          handler => CHServer.OnGetChargePointListUpdatesSOAPResponse -= handler,
                          "GetChargePointListUpdates", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region GetStatus

            RegisterEvent2("GetStatusRequest",
                          handler => CHServer.OnGetStatusSOAPRequest += handler,
                          handler => CHServer.OnGetStatusSOAPRequest -= handler,
                          "GetStatus", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("GetStatusResponse",
                          handler => CHServer.OnGetStatusSOAPResponse += handler,
                          handler => CHServer.OnGetStatusSOAPResponse -= handler,
                          "GetStatus", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region SetRoamingAuthorisationList

            RegisterEvent2("SetRoamingAuthorisationListRequest",
                          handler => CHServer.OnSetRoamingAuthorisationListSOAPRequest += handler,
                          handler => CHServer.OnSetRoamingAuthorisationListSOAPRequest -= handler,
                          "SetRoamingAuthorisationList", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("SetRoamingAuthorisationListResponse",
                          handler => CHServer.OnSetRoamingAuthorisationListSOAPResponse += handler,
                          handler => CHServer.OnSetRoamingAuthorisationListSOAPResponse -= handler,
                          "SetRoamingAuthorisationList", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region UpdateRoamingAuthorisationList

            RegisterEvent2("UpdateRoamingAuthorisationListRequest",
                          handler => CHServer.OnUpdateRoamingAuthorisationListSOAPRequest += handler,
                          handler => CHServer.OnUpdateRoamingAuthorisationListSOAPRequest -= handler,
                          "UpdateRoamingAuthorisationList", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("UpdateRoamingAuthorisationListResponse",
                          handler => CHServer.OnUpdateRoamingAuthorisationListSOAPResponse += handler,
                          handler => CHServer.OnUpdateRoamingAuthorisationListSOAPResponse -= handler,
                          "UpdateRoamingAuthorisationList", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region GetCDRs

            RegisterEvent2("GetCDRsRequest",
                          handler => CHServer.OnGetCDRsSOAPRequest += handler,
                          handler => CHServer.OnGetCDRsSOAPRequest -= handler,
                          "GetCDRs", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("GetCDRsResponse",
                          handler => CHServer.OnGetCDRsSOAPResponse += handler,
                          handler => CHServer.OnGetCDRsSOAPResponse -= handler,
                          "GetCDRs", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region ConfirmCDRs

            RegisterEvent2("ConfirmCDRsRequest",
                          handler => CHServer.OnConfirmCDRsSOAPRequest += handler,
                          handler => CHServer.OnConfirmCDRsSOAPRequest -= handler,
                          "ConfirmCDRs", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("ConfirmCDRsResponse",
                          handler => CHServer.OnConfirmCDRsSOAPResponse += handler,
                          handler => CHServer.OnConfirmCDRsSOAPResponse -= handler,
                          "ConfirmCDRs", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion

            #region GetTariffUpdates

            RegisterEvent2("GetTariffUpdatesRequest",
                          handler => CHServer.OnGetTariffUpdatesSOAPRequest += handler,
                          handler => CHServer.OnGetTariffUpdatesSOAPRequest -= handler,
                          "GetTariffUpdates", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            RegisterEvent2("GetTariffUpdatesResponse",
                          handler => CHServer.OnGetTariffUpdatesSOAPResponse += handler,
                          handler => CHServer.OnGetTariffUpdatesSOAPResponse -= handler,
                          "GetTariffUpdates", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTargetX(this).
                RegisterDefaultDiscLogTargetX(this);

            #endregion


        }

        #endregion

        #endregion

    }

}
