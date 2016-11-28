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
using org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHP EMP client.
    /// </summary>
    public partial class EMPClient : ASOAPClient
    {

        /// <summary>
        /// An OCHP EMP client (HTTP/SOAP client) logger.
        /// </summary>
        public class EMPClientLogger : HTTPLogger
        {

            #region Data

            /// <summary>
            /// The default context for this logger.
            /// </summary>
            public const String DefaultContext = "OCHP_EMPClient";

            #endregion

            #region Properties

            /// <summary>
            /// The attached OCHP EMP client.
            /// </summary>
            public EMPClient EMPClient { get; }

            #endregion

            #region Constructor(s)

            #region EMPClientLogger(EMPClient, Context = DefaultContext, LogFileCreator = null)

            /// <summary>
            /// Create a new OCHP EMP client logger using the default logging delegates.
            /// </summary>
            /// <param name="EMPClient">A OCHP EMP client.</param>
            /// <param name="Context">A context of this API.</param>
            /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
            public EMPClientLogger(EMPClient                     EMPClient,
                                   String                        Context         = DefaultContext,
                                   Func<String, String, String>  LogFileCreator  = null)

                : this(EMPClient,
                       Context.IsNotNullOrEmpty() ? Context : DefaultContext,
                       null,
                       null,
                       null,
                       null,

                       LogFileCreator: LogFileCreator)

            { }

            #endregion

            #region EMPClientLogger(EMPClient, Context, ... Logging delegates ...)

            /// <summary>
            /// Create a new OCHP EMP client logger using the given logging delegates.
            /// </summary>
            /// <param name="EMPClient">A OCHP EMP client.</param>
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
            /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
            public EMPClientLogger(EMPClient                     EMPClient,
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

                : base(Context.IsNotNullOrEmpty() ? Context : DefaultContext,

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

                if (EMPClient == null)
                    throw new ArgumentNullException(nameof(EMPClient), "The given EMP client must not be null!");

                this.EMPClient = EMPClient;

                #endregion


                // OCHP

                #region GetChargePointList

                RegisterEvent("GetChargePointListRequest",
                              handler => EMPClient.OnGetChargePointListSOAPRequest += handler,
                              handler => EMPClient.OnGetChargePointListSOAPRequest -= handler,
                              "GetChargePointList", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetChargePointListResponse",
                              handler => EMPClient.OnGetChargePointListSOAPResponse += handler,
                              handler => EMPClient.OnGetChargePointListSOAPResponse -= handler,
                              "GetChargePointList", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetChargePointListUpdates

                RegisterEvent("GetChargePointListUpdatesRequest",
                              handler => EMPClient.OnGetChargePointListUpdatesSOAPRequest += handler,
                              handler => EMPClient.OnGetChargePointListUpdatesSOAPRequest -= handler,
                              "GetChargePointListUpdates", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetChargePointListUpdatesResponse",
                              handler => EMPClient.OnGetChargePointListUpdatesSOAPResponse += handler,
                              handler => EMPClient.OnGetChargePointListUpdatesSOAPResponse -= handler,
                              "GetChargePointListUpdates", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetStatus

                RegisterEvent("GetStatusRequest",
                              handler => EMPClient.OnGetStatusSOAPRequest += handler,
                              handler => EMPClient.OnGetStatusSOAPRequest -= handler,
                              "GetStatus", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetStatusResponse",
                              handler => EMPClient.OnGetStatusSOAPResponse += handler,
                              handler => EMPClient.OnGetStatusSOAPResponse -= handler,
                              "GetStatus", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region SetRoamingAuthorisationList

                RegisterEvent("SetRoamingAuthorisationListRequest",
                              handler => EMPClient.OnSetRoamingAuthorisationListSOAPRequest += handler,
                              handler => EMPClient.OnSetRoamingAuthorisationListSOAPRequest -= handler,
                              "SetRoamingAuthorisationList", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SetRoamingAuthorisationListResponse",
                              handler => EMPClient.OnSetRoamingAuthorisationListSOAPResponse += handler,
                              handler => EMPClient.OnSetRoamingAuthorisationListSOAPResponse -= handler,
                              "SetRoamingAuthorisationList", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region UpdateRoamingAuthorisationList

                RegisterEvent("UpdateRoamingAuthorisationListRequest",
                              handler => EMPClient.OnUpdateRoamingAuthorisationListSOAPRequest += handler,
                              handler => EMPClient.OnUpdateRoamingAuthorisationListSOAPRequest -= handler,
                              "UpdateRoamingAuthorisationList", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("UpdateRoamingAuthorisationListResponse",
                              handler => EMPClient.OnUpdateRoamingAuthorisationListSOAPResponse += handler,
                              handler => EMPClient.OnUpdateRoamingAuthorisationListSOAPResponse -= handler,
                              "UpdateRoamingAuthorisationList", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetCDRs

                RegisterEvent("GetCDRsRequest",
                              handler => EMPClient.OnGetCDRsSOAPRequest += handler,
                              handler => EMPClient.OnGetCDRsSOAPRequest -= handler,
                              "GetCDRs", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetCDRsResponse",
                              handler => EMPClient.OnGetCDRsSOAPResponse += handler,
                              handler => EMPClient.OnGetCDRsSOAPResponse -= handler,
                              "GetCDRs", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region ConfirmCDRs

                RegisterEvent("ConfirmCDRsRequest",
                              handler => EMPClient.OnConfirmCDRsSOAPRequest += handler,
                              handler => EMPClient.OnConfirmCDRsSOAPRequest -= handler,
                              "ConfirmCDRs", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ConfirmCDRsResponse",
                              handler => EMPClient.OnConfirmCDRsSOAPResponse += handler,
                              handler => EMPClient.OnConfirmCDRsSOAPResponse -= handler,
                              "ConfirmCDRs", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetTariffUpdates

                RegisterEvent("GetTariffUpdatesRequest",
                              handler => EMPClient.OnGetTariffUpdatesSOAPRequest += handler,
                              handler => EMPClient.OnGetTariffUpdatesSOAPRequest -= handler,
                              "GetTariffUpdates", "OCHP", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetTariffUpdatesResponse",
                              handler => EMPClient.OnGetTariffUpdatesSOAPResponse += handler,
                              handler => EMPClient.OnGetTariffUpdatesSOAPResponse -= handler,
                              "GetTariffUpdates", "OCHP", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion


                // OCHPdirect

                #region AddServiceEndpoints

                RegisterEvent("AddServiceEndpointsRequest",
                              handler => EMPClient.OnAddServiceEndpointsSOAPRequest += handler,
                              handler => EMPClient.OnAddServiceEndpointsSOAPRequest -= handler,
                              "AddServiceEndpoints", "OCHPdirect", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("AddServiceEndpointsResponse",
                              handler => EMPClient.OnAddServiceEndpointsSOAPResponse += handler,
                              handler => EMPClient.OnAddServiceEndpointsSOAPResponse -= handler,
                              "AddServiceEndpoints", "OCHPdirect", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetServiceEndpoints

                RegisterEvent("GetServiceEndpointsRequest",
                              handler => EMPClient.OnGetServiceEndpointsSOAPRequest += handler,
                              handler => EMPClient.OnGetServiceEndpointsSOAPRequest -= handler,
                              "GetServiceEndpoints", "OCHPdirect", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetServiceEndpointsResponse",
                              handler => EMPClient.OnGetServiceEndpointsSOAPResponse += handler,
                              handler => EMPClient.OnGetServiceEndpointsSOAPResponse -= handler,
                              "GetServiceEndpoints", "OCHPdirect", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region SelectEVSE

                RegisterEvent("SelectEVSERequest",
                              handler => EMPClient.OnSelectEVSESOAPRequest += handler,
                              handler => EMPClient.OnSelectEVSESOAPRequest -= handler,
                              "SelectEVSE", "OCHPdirect", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("SelectEVSEResponse",
                              handler => EMPClient.OnSelectEVSESOAPResponse += handler,
                              handler => EMPClient.OnSelectEVSESOAPResponse -= handler,
                              "SelectEVSE", "OCHPdirect", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region ControlEVSE

                RegisterEvent("ControlEVSERequest",
                              handler => EMPClient.OnControlEVSESOAPRequest += handler,
                              handler => EMPClient.OnControlEVSESOAPRequest -= handler,
                              "ControlEVSE", "OCHPdirect", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ControlEVSEResponse",
                              handler => EMPClient.OnControlEVSESOAPResponse += handler,
                              handler => EMPClient.OnControlEVSESOAPResponse -= handler,
                              "ControlEVSE", "OCHPdirect", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region ReleaseEVSE

                RegisterEvent("ReleaseEVSERequest",
                              handler => EMPClient.OnReleaseEVSESOAPRequest += handler,
                              handler => EMPClient.OnReleaseEVSESOAPRequest -= handler,
                              "ReleaseEVSE", "OCHPdirect", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ReleaseEVSEResponse",
                              handler => EMPClient.OnReleaseEVSESOAPResponse += handler,
                              handler => EMPClient.OnReleaseEVSESOAPResponse -= handler,
                              "ReleaseEVSE", "OCHPdirect", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetEVSEStatus

                RegisterEvent("GetEVSEStatusRequest",
                              handler => EMPClient.OnGetEVSEStatusSOAPRequest += handler,
                              handler => EMPClient.OnGetEVSEStatusSOAPRequest -= handler,
                              "GetEVSEStatus", "OCHPdirect", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetEVSEStatusResponse",
                              handler => EMPClient.OnGetEVSEStatusSOAPResponse += handler,
                              handler => EMPClient.OnGetEVSEStatusSOAPResponse -= handler,
                              "GetEVSEStatus", "OCHPdirect", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetEVSEStatus

                RegisterEvent("ReportDiscrepancyRequest",
                              handler => EMPClient.OnReportDiscrepancySOAPRequest += handler,
                              handler => EMPClient.OnReportDiscrepancySOAPRequest -= handler,
                              "ReportDiscrepancy", "OCHPdirect", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("ReportDiscrepancyResponse",
                              handler => EMPClient.OnReportDiscrepancySOAPResponse += handler,
                              handler => EMPClient.OnReportDiscrepancySOAPResponse -= handler,
                              "ReportDiscrepancy", "OCHPdirect", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion

                #region GetInformProvider

                RegisterEvent("GetInformProviderRequest",
                              handler => EMPClient.OnGetInformProviderSOAPRequest += handler,
                              handler => EMPClient.OnGetInformProviderSOAPRequest -= handler,
                              "GetInformProvider", "OCHPdirect", "Requests", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                RegisterEvent("GetInformProviderResponse",
                              handler => EMPClient.OnGetInformProviderSOAPResponse += handler,
                              handler => EMPClient.OnGetInformProviderSOAPResponse -= handler,
                              "GetInformProvider", "OCHPdirect", "Responses", "All").
                    RegisterDefaultConsoleLogTarget(this).
                    RegisterDefaultDiscLogTarget(this);

                #endregion


            }

            #endregion

            #endregion

        }

    }

}
