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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CH
{

    /// <summary>
    /// An OCHP CH server logger.
    /// </summary>
    public class CHServerLogger : HTTPLogger
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

        #region CHServerLogger(CHServer, Context = DefaultContext, LogFileCreator = null)

        /// <summary>
        /// Create a new OCHP CH server logger using the default logging delegates.
        /// </summary>
        /// <param name="CHServer">A OCHP CH server.</param>
        /// <param name="Context">A context of this API.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CHServerLogger(CHServer                     CHServer,
                               String                        Context         = DefaultContext,
                               Func<String, String, String>  LogFileCreator  = null)

            : this(CHServer,
                   Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                   null,
                   null,
                   null,
                   null,
                   LogFileCreator: LogFileCreator)

        { }

        #endregion

        #region CHServerLogger(CHServer, Context, ... Logging delegates ...)

        /// <summary>
        /// Create a new OCHP CH server logger using the given logging delegates.
        /// </summary>
        /// <param name="CHServer">A OCHP CH server.</param>
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
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CHServerLogger(CHServer                     CHServer,
                               String                        Context,

                               HTTPRequestLoggerDelegate     LogHTTPRequest_toConsole,
                               HTTPResponseLoggerDelegate    LogHTTPResponse_toConsole,
                               HTTPRequestLoggerDelegate     LogHTTPRequest_toDisc,
                               HTTPResponseLoggerDelegate    LogHTTPResponse_toDisc,

                               HTTPRequestLoggerDelegate     LogHTTPRequest_toNetwork   = null,
                               HTTPResponseLoggerDelegate    LogHTTPResponse_toNetwork  = null,
                               HTTPRequestLoggerDelegate     LogHTTPRequest_toHTTPSSE   = null,
                               HTTPResponseLoggerDelegate    LogHTTPResponse_toHTTPSSE  = null,

                               HTTPResponseLoggerDelegate    LogHTTPError_toConsole     = null,
                               HTTPResponseLoggerDelegate    LogHTTPError_toDisc        = null,
                               HTTPResponseLoggerDelegate    LogHTTPError_toNetwork     = null,
                               HTTPResponseLoggerDelegate    LogHTTPError_toHTTPSSE     = null,

                               Func<String, String, String>  LogFileCreator             = null)

            : base(CHServer.SOAPServer,
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

                   LogFileCreator)

        {

            #region Initial checks

            if (CHServer == null)
                throw new ArgumentNullException(nameof(CHServer), "The given CH server must not be null!");

            this.CHServer = CHServer;

            #endregion


            // Shared event logs...

            #region AddServiceEndpoints

            RegisterEvent("AddServiceEndpointsRequest",
                          handler => CHServer.OnAddServiceEndpointsSOAPRequest += handler,
                          handler => CHServer.OnAddServiceEndpointsSOAPRequest -= handler,
                          "AddServiceEndpoints", "OCHPdirect", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("AddServiceEndpointsResponse",
                          handler => CHServer.OnAddServiceEndpointsSOAPResponse += handler,
                          handler => CHServer.OnAddServiceEndpointsSOAPResponse -= handler,
                          "AddServiceEndpoints", "OCHPdirect", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region GetServiceEndpoints

            RegisterEvent("GetServiceEndpointsRequest",
                          handler => CHServer.OnGetServiceEndpointsSOAPRequest += handler,
                          handler => CHServer.OnGetServiceEndpointsSOAPRequest -= handler,
                          "GetServiceEndpoints", "OCHPdirect", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("GetServiceEndpointsResponse",
                          handler => CHServer.OnGetServiceEndpointsSOAPResponse += handler,
                          handler => CHServer.OnGetServiceEndpointsSOAPResponse -= handler,
                          "GetServiceEndpoints", "OCHPdirect", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion


            // CPO event logs...

            #region SetChargePointList

            RegisterEvent("SetChargePointListRequest",
                          handler => CHServer.OnSetChargePointListSOAPRequest += handler,
                          handler => CHServer.OnSetChargePointListSOAPRequest -= handler,
                          "SetChargePointList", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("SetChargePointListResponse",
                          handler => CHServer.OnSetChargePointListSOAPResponse += handler,
                          handler => CHServer.OnSetChargePointListSOAPResponse -= handler,
                          "SetChargePointList", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region UpdateChargePointList

            RegisterEvent("UpdateChargePointListRequest",
                          handler => CHServer.OnUpdateChargePointListSOAPRequest += handler,
                          handler => CHServer.OnUpdateChargePointListSOAPRequest -= handler,
                          "UpdateChargePointList", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("UpdateChargePointListResponse",
                          handler => CHServer.OnUpdateChargePointListSOAPResponse += handler,
                          handler => CHServer.OnUpdateChargePointListSOAPResponse -= handler,
                          "UpdateChargePointList", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region UpdateStatusRequest

            RegisterEvent("UpdateStatusRequest",
                          handler => CHServer.OnUpdateStatusSOAPRequest += handler,
                          handler => CHServer.OnUpdateStatusSOAPRequest -= handler,
                          "UpdateStatus", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("UpdateStatusResponse",
                          handler => CHServer.OnUpdateStatusSOAPResponse += handler,
                          handler => CHServer.OnUpdateStatusSOAPResponse -= handler,
                          "UpdateStatus", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region GetSingleRoamingAuthorisation

            RegisterEvent("GetSingleRoamingAuthorisationRequest",
                          handler => CHServer.OnGetSingleRoamingAuthorisationSOAPRequest += handler,
                          handler => CHServer.OnGetSingleRoamingAuthorisationSOAPRequest -= handler,
                          "GetSingleRoamingAuthorisation", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("GetSingleRoamingAuthorisationResponse",
                          handler => CHServer.OnGetSingleRoamingAuthorisationSOAPResponse += handler,
                          handler => CHServer.OnGetSingleRoamingAuthorisationSOAPResponse -= handler,
                          "GetSingleRoamingAuthorisation", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region GetRoamingAuthorisationList

            RegisterEvent("GetRoamingAuthorisationListRequest",
                          handler => CHServer.OnGetRoamingAuthorisationListSOAPRequest += handler,
                          handler => CHServer.OnGetRoamingAuthorisationListSOAPRequest -= handler,
                          "GetRoamingAuthorisationList", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("GetRoamingAuthorisationListResponse",
                          handler => CHServer.OnGetRoamingAuthorisationListSOAPResponse += handler,
                          handler => CHServer.OnGetRoamingAuthorisationListSOAPResponse -= handler,
                          "GetRoamingAuthorisationList", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region GetRoamingAuthorisationListUpdates

            RegisterEvent("GetRoamingAuthorisationListUpdatesRequest",
                          handler => CHServer.OnGetRoamingAuthorisationListUpdatesSOAPRequest += handler,
                          handler => CHServer.OnGetRoamingAuthorisationListUpdatesSOAPRequest -= handler,
                          "GetRoamingAuthorisationListUpdates", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("GetRoamingAuthorisationListUpdatesResponse",
                          handler => CHServer.OnGetRoamingAuthorisationListUpdatesSOAPResponse += handler,
                          handler => CHServer.OnGetRoamingAuthorisationListUpdatesSOAPResponse -= handler,
                          "GetRoamingAuthorisationListUpdates", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region AddCDRs

            RegisterEvent("AddCDRsRequest",
                          handler => CHServer.OnAddCDRsSOAPRequest += handler,
                          handler => CHServer.OnAddCDRsSOAPRequest -= handler,
                          "AddCDRs", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("AddCDRsResponse",
                          handler => CHServer.OnAddCDRsSOAPResponse += handler,
                          handler => CHServer.OnAddCDRsSOAPResponse -= handler,
                          "AddCDRs", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region CheckCDRs

            RegisterEvent("CheckCDRsRequest",
                          handler => CHServer.OnCheckCDRsSOAPRequest += handler,
                          handler => CHServer.OnCheckCDRsSOAPRequest -= handler,
                          "CheckCDRs", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("CheckCDRsResponse",
                          handler => CHServer.OnCheckCDRsSOAPResponse += handler,
                          handler => CHServer.OnCheckCDRsSOAPResponse -= handler,
                          "CheckCDRs", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region UpdateTariffs

            RegisterEvent("UpdateTariffsRequest",
                          handler => CHServer.OnUpdateTariffsSOAPRequest += handler,
                          handler => CHServer.OnUpdateTariffsSOAPRequest -= handler,
                          "UpdateTariffs", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("UpdateTariffsResponse",
                          handler => CHServer.OnUpdateTariffsSOAPResponse += handler,
                          handler => CHServer.OnUpdateTariffsSOAPResponse -= handler,
                          "UpdateTariffs", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion


            // EMP event logs...

            #region GetChargePointList

            RegisterEvent("GetChargePointListRequest",
                          handler => CHServer.OnGetChargePointListSOAPRequest += handler,
                          handler => CHServer.OnGetChargePointListSOAPRequest -= handler,
                          "GetChargePointList", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("GetChargePointListResponse",
                          handler => CHServer.OnGetChargePointListSOAPResponse += handler,
                          handler => CHServer.OnGetChargePointListSOAPResponse -= handler,
                          "GetChargePointList", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region GetChargePointListUpdates

            RegisterEvent("GetChargePointListUpdatesRequest",
                          handler => CHServer.OnGetChargePointListUpdatesSOAPRequest += handler,
                          handler => CHServer.OnGetChargePointListUpdatesSOAPRequest -= handler,
                          "GetChargePointListUpdates", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("GetChargePointListUpdatesResponse",
                          handler => CHServer.OnGetChargePointListUpdatesSOAPResponse += handler,
                          handler => CHServer.OnGetChargePointListUpdatesSOAPResponse -= handler,
                          "GetChargePointListUpdates", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region GetStatus

            RegisterEvent("GetStatusRequest",
                          handler => CHServer.OnGetStatusSOAPRequest += handler,
                          handler => CHServer.OnGetStatusSOAPRequest -= handler,
                          "GetStatus", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("GetStatusResponse",
                          handler => CHServer.OnGetStatusSOAPResponse += handler,
                          handler => CHServer.OnGetStatusSOAPResponse -= handler,
                          "GetStatus", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region SetRoamingAuthorisationList

            RegisterEvent("SetRoamingAuthorisationListRequest",
                          handler => CHServer.OnSetRoamingAuthorisationListSOAPRequest += handler,
                          handler => CHServer.OnSetRoamingAuthorisationListSOAPRequest -= handler,
                          "SetRoamingAuthorisationList", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("SetRoamingAuthorisationListResponse",
                          handler => CHServer.OnSetRoamingAuthorisationListSOAPResponse += handler,
                          handler => CHServer.OnSetRoamingAuthorisationListSOAPResponse -= handler,
                          "SetRoamingAuthorisationList", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region UpdateRoamingAuthorisationList

            RegisterEvent("UpdateRoamingAuthorisationListRequest",
                          handler => CHServer.OnUpdateRoamingAuthorisationListSOAPRequest += handler,
                          handler => CHServer.OnUpdateRoamingAuthorisationListSOAPRequest -= handler,
                          "UpdateRoamingAuthorisationList", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("UpdateRoamingAuthorisationListResponse",
                          handler => CHServer.OnUpdateRoamingAuthorisationListSOAPResponse += handler,
                          handler => CHServer.OnUpdateRoamingAuthorisationListSOAPResponse -= handler,
                          "UpdateRoamingAuthorisationList", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region GetCDRs

            RegisterEvent("GetCDRsRequest",
                          handler => CHServer.OnGetCDRsSOAPRequest += handler,
                          handler => CHServer.OnGetCDRsSOAPRequest -= handler,
                          "GetCDRs", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("GetCDRsResponse",
                          handler => CHServer.OnGetCDRsSOAPResponse += handler,
                          handler => CHServer.OnGetCDRsSOAPResponse -= handler,
                          "GetCDRs", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region ConfirmCDRs

            RegisterEvent("ConfirmCDRsRequest",
                          handler => CHServer.OnConfirmCDRsSOAPRequest += handler,
                          handler => CHServer.OnConfirmCDRsSOAPRequest -= handler,
                          "ConfirmCDRs", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("ConfirmCDRsResponse",
                          handler => CHServer.OnConfirmCDRsSOAPResponse += handler,
                          handler => CHServer.OnConfirmCDRsSOAPResponse -= handler,
                          "ConfirmCDRs", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion

            #region GetTariffUpdates

            RegisterEvent("GetTariffUpdatesRequest",
                          handler => CHServer.OnGetTariffUpdatesSOAPRequest += handler,
                          handler => CHServer.OnGetTariffUpdatesSOAPRequest -= handler,
                          "GetTariffUpdates", "OCHP", "Requests", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            RegisterEvent("GetTariffUpdatesResponse",
                          handler => CHServer.OnGetTariffUpdatesSOAPResponse += handler,
                          handler => CHServer.OnGetTariffUpdatesSOAPResponse -= handler,
                          "GetTariffUpdates", "OCHP", "Responses", "All").
                RegisterDefaultConsoleLogTarget(this).
                RegisterDefaultDiscLogTarget(this);

            #endregion


        }

        #endregion

        #endregion

    }

}
