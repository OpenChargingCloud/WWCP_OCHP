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
using System.Threading;
using System.Net.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHP CPO Client.
    /// </summary>
    public class CPORoaming : ICPOClient
    {

        #region Properties

        /// <summary>
        /// The CPO client.
        /// </summary>
        public CPOClient        CPOClient         { get; }

        /// <summary>
        /// The CPO server.
        /// </summary>
        public CPOServer        CPOServer         { get; }

        /// <summary>
        /// The CPO server logger.
        /// </summary>
        public CPOServerLogger  CPOServerLogger   { get; }

        /// <summary>
        /// The DNS client defines which DNS servers to use.
        /// </summary>
        public DNSClient DNSClient
            => CPOServer.DNSClient;

        /// <summary>
        /// The default request timeout for this client.
        /// </summary>
        public TimeSpan?        RequestTimeout    { get; }

        #endregion

        #region Events

        // CPOClient logging methods


        // CPOServer methods


        // Generic HTTP/SOAP server logging

        #region RequestLog

        /// <summary>
        /// An event called whenever a request came in.
        /// </summary>
        public event RequestLogHandler RequestLog
        {

            add
            {
                CPOServer.RequestLog += value;
            }

            remove
            {
                CPOServer.RequestLog -= value;
            }

        }

        #endregion

        #region AccessLog

        /// <summary>
        /// An event called whenever a request could successfully be processed.
        /// </summary>
        public event AccessLogHandler AccessLog
        {

            add
            {
                CPOServer.AccessLog += value;
            }

            remove
            {
                CPOServer.AccessLog -= value;
            }

        }

        #endregion

        #region ErrorLog

        /// <summary>
        /// An event called whenever a request resulted in an error.
        /// </summary>
        public event ErrorLogHandler ErrorLog
        {

            add
            {
                CPOServer.ErrorLog += value;
            }

            remove
            {
                CPOServer.ErrorLog -= value;
            }

        }

        #endregion

        #endregion

        #region Custom request mappers

        #endregion

        #region Constructor(s)

        #region CPORoaming(CPOClient, CPOServer, ServerLoggingContext = CPOServerLogger.DefaultContext, LogFileCreator = null)

        /// <summary>
        /// Create a new OCHP roaming client for CPOs.
        /// </summary>
        /// <param name="CPOClient">A CPO client.</param>
        /// <param name="CPOServer">A CPO sever.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        public CPORoaming(CPOClient               CPOClient,
                          CPOServer               CPOServer,
                          String                  ServerLoggingContext  = CPOServerLogger.DefaultContext,
                          LogfileCreatorDelegate  LogFileCreator        = null)
        {

            this.CPOClient        = CPOClient;
            this.CPOServer        = CPOServer;

            this.CPOServerLogger  = new CPOServerLogger(CPOServer,
                                                        ServerLoggingContext,
                                                        LogFileCreator);

        }

        #endregion

        #region CPORoaming(ClientId, RemoteHostname, RemoteTCPPort = null, RemoteHTTPVirtualHost = null, ... )

        /// <summary>
        /// Create a new OCHP roaming client for CPOs.
        /// </summary>
        /// <param name="ClientId">A unqiue identification of this client.</param>
        /// <param name="RemoteHostname">The hostname of the remote OCHP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote OCHP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OCHP service.</param>
        /// <param name="URIPrefix">An default URI prefix.</param>
        /// <param name="WSSLoginPassword">The WebService-Security username/password.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerURISuffix">An optional HTTP/SOAP/XML server URI suffix.</param>
        /// <param name="ServerContentType">An optional HTTP content type to use.</param>
        /// <param name="ServerRegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public CPORoaming(String                               ClientId,
                          String                               RemoteHostname,
                          IPPort                               RemoteTCPPort                   = null,
                          RemoteCertificateValidationCallback  RemoteCertificateValidator      = null,
                          X509Certificate                      ClientCert                      = null,
                          String                               RemoteHTTPVirtualHost           = null,
                          String                               URIPrefix                       = CPOClient.DefaultURIPrefix,
                          Tuple<String, String>                WSSLoginPassword                = null,
                          String                               HTTPUserAgent                   = CPOClient.DefaultHTTPUserAgent,
                          TimeSpan?                            RequestTimeout                  = null,

                          String                               ServerName                      = CPOServer.DefaultHTTPServerName,
                          IPPort                               ServerTCPPort                   = null,
                          String                               ServerURIPrefix                 = CPOServer.DefaultURIPrefix,
                          String                               ServerURISuffix                 = CPOServer.DefaultURISuffix,
                          HTTPContentType                      ServerContentType               = null,
                          Boolean                              ServerRegisterHTTPRootService   = true,
                          Boolean                              ServerAutoStart                 = false,

                          String                               ClientLoggingContext            = CPOClient.CPOClientLogger.DefaultContext,
                          String                               ServerLoggingContext            = CPOServerLogger.DefaultContext,
                          LogfileCreatorDelegate               LogFileCreator                  = null,

                          DNSClient                            DNSClient                       = null)

            : this(new CPOClient(ClientId,
                                 RemoteHostname,
                                 RemoteTCPPort,
                                 RemoteCertificateValidator,
                                 ClientCert,
                                 RemoteHTTPVirtualHost,
                                 URIPrefix,
                                 WSSLoginPassword,
                                 HTTPUserAgent,
                                 RequestTimeout,
                                 DNSClient,
                                 ClientLoggingContext,
                                 LogFileCreator),

                   new CPOServer(ServerName,
                                 ServerTCPPort,
                                 ServerURIPrefix,
                                 ServerURISuffix,
                                 ServerContentType,
                                 ServerRegisterHTTPRootService,
                                 DNSClient,
                                 false),

                   ServerLoggingContext,
                   LogFileCreator)

        {

            if (ServerAutoStart)
                Start();

        }

        #endregion

        #endregion


        // OCHP

        #region SetChargePointList   (ChargePointInfos, IncludeChargePoints = null, ...)

        /// <summary>
        /// Upload the given enumeration of charge points.
        /// </summary>
        /// <param name="ChargePointInfos">An enumeration of charge points.</param>
        /// <param name="IncludeChargePoints">An optional delegate for filtering charge points before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<HTTPResponse<SetChargePointListResponse>>

            SetChargePointList(IEnumerable<ChargePointInfo>  ChargePointInfos,
                               IncludeChargePointsDelegate   IncludeChargePoints  = null,

                               DateTime?                     Timestamp            = null,
                               CancellationToken?            CancellationToken    = null,
                               EventTracking_Id              EventTrackingId      = null,
                               TimeSpan?                     RequestTimeout       = null)


                => CPOClient.SetChargePointList(ChargePointInfos,
                                                IncludeChargePoints,

                                                Timestamp,
                                                CancellationToken,
                                                EventTrackingId,
                                                RequestTimeout);

        #endregion

        #region UpdateChargePointList(ChargePointInfos, IncludeChargePoints = null, ...)

        /// <summary>
        /// Upload the given enumeration of updated charge points.
        /// </summary>
        /// <param name="ChargePointInfos">An enumeration of updated charge points.</param>
        /// <param name="IncludeChargePoints">An optional delegate for filtering charge points before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<HTTPResponse<UpdateChargePointListResponse>>

            UpdateChargePointList(IEnumerable<ChargePointInfo>  ChargePointInfos,
                                  IncludeChargePointsDelegate   IncludeChargePoints  = null,

                                  DateTime?                     Timestamp            = null,
                                  CancellationToken?            CancellationToken    = null,
                                  EventTracking_Id              EventTrackingId      = null,
                                  TimeSpan?                     RequestTimeout       = null)


                => CPOClient.UpdateChargePointList(ChargePointInfos,
                                                   IncludeChargePoints,

                                                   Timestamp,
                                                   CancellationToken,
                                                   EventTrackingId,
                                                   RequestTimeout);

        #endregion

        #region UpdateStatus(EVSEStatus = null, ParkingStatus = null, DefaultTTL = null, IncludeEVSEIds = null, ...)

        /// <summary>
        /// Upload the given enumeration of EVSE and/or parking status.
        /// </summary>
        /// <param name="Request">A UpdateStatus request.</param>
        public Task<HTTPResponse<UpdateStatusResponse>>

            UpdateStatus(UpdateStatusRequest Request)


                => CPOClient.UpdateStatus(Request);


        #endregion


        #region GetSingleRoamingAuthorisation(EMTId, ...)

        /// <summary>
        /// Create a new OCHP GetSingleRoamingAuthorisation request.
        /// </summary>
        /// <param name="Request">A GetSingleRoamingAuthorisation request.</param>
        public Task<HTTPResponse<GetSingleRoamingAuthorisationResponse>>

            GetSingleRoamingAuthorisation(GetSingleRoamingAuthorisationRequest Request)


                => CPOClient.GetSingleRoamingAuthorisation(Request);


        #endregion

        #region GetRoamingAuthorisationList(...)

        /// <summary>
        /// Get the entire current version of the roaming authorisation list.
        /// </summary>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<HTTPResponse<GetRoamingAuthorisationListResponse>>

            GetRoamingAuthorisationList(DateTime?           Timestamp          = null,
                                        CancellationToken?  CancellationToken  = null,
                                        EventTracking_Id    EventTrackingId    = null,
                                        TimeSpan?           RequestTimeout     = null)


                => CPOClient.GetRoamingAuthorisationList(Timestamp,
                                                         CancellationToken,
                                                         EventTrackingId,
                                                         RequestTimeout);

        #endregion

        #region GetRoamingAuthorisationListUpdates(LastUpdate, ...)

        /// <summary>
        /// Get the entire current version of the roaming authorisation list.
        /// </summary>
        /// <param name="LastUpdate">The timestamp of the last roaming authorisation list update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>>

            GetRoamingAuthorisationListUpdates(DateTime            LastUpdate,

                                               DateTime?           Timestamp          = null,
                                               CancellationToken?  CancellationToken  = null,
                                               EventTracking_Id    EventTrackingId    = null,
                                               TimeSpan?           RequestTimeout     = null)


                => CPOClient.GetRoamingAuthorisationListUpdates(LastUpdate,

                                                                Timestamp,
                                                                CancellationToken,
                                                                EventTrackingId,
                                                                RequestTimeout);

        #endregion


        #region AddCDRs(CDRInfos, ...)

        /// <summary>
        /// Upload the given enumeration of charge detail records.
        /// </summary>
        /// <param name="Request">A AddCDRs request.</param>
        public Task<HTTPResponse<AddCDRsResponse>>

            AddCDRs(AddCDRsRequest Request)


                => CPOClient.AddCDRs(Request);


        #endregion

        #region CheckCDRs(CDRStatus = null, ...)

        /// <summary>
        /// Check charge detail records having the given optional status.
        /// </summary>
        /// <param name="CDRStatus">The status of the requested charge detail records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<HTTPResponse<CheckCDRsResponse>>

            CheckCDRs(CDRStatus?            CDRStatus          = null,

                      DateTime?             Timestamp          = null,
                      CancellationToken?    CancellationToken  = null,
                      EventTracking_Id      EventTrackingId    = null,
                      TimeSpan?             RequestTimeout     = null)


                => CPOClient.CheckCDRs(CDRStatus,

                                       Timestamp,
                                       CancellationToken,
                                       EventTrackingId,
                                       RequestTimeout);

        #endregion


        #region UpdateTariffs(TariffInfos, ...)

        /// <summary>
        /// Upload the given enumeration of tariff infos.
        /// </summary>
        /// <param name="TariffInfos">An enumeration of tariff infos.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<HTTPResponse<UpdateTariffsResponse>>

            UpdateTariffs(IEnumerable<TariffInfo>  TariffInfos,

                          DateTime?                Timestamp          = null,
                          CancellationToken?       CancellationToken  = null,
                          EventTracking_Id         EventTrackingId    = null,
                          TimeSpan?                RequestTimeout     = null)


                => CPOClient.UpdateTariffs(TariffInfos,

                                           Timestamp,
                                           CancellationToken,
                                           EventTrackingId,
                                           RequestTimeout);

        #endregion


        // OCHPdirect

        #region AddServiceEndpoints(OperatorEndpoints, ...)

        /// <summary>
        /// Upload the given enumeration of OCHPdirect provider endpoints.
        /// </summary>
        /// <param name="OperatorEndpoints">An enumeration of provider endpoints.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<HTTPResponse<AddServiceEndpointsResponse>>

            AddServiceEndpoints(IEnumerable<OperatorEndpoint>  OperatorEndpoints,

                                DateTime?                      Timestamp          = null,
                                CancellationToken?             CancellationToken  = null,
                                EventTracking_Id               EventTrackingId    = null,
                                TimeSpan?                      RequestTimeout     = null)


                => CPOClient.AddServiceEndpoints(OperatorEndpoints,

                                                 Timestamp,
                                                 CancellationToken,
                                                 EventTrackingId,
                                                 RequestTimeout);

        #endregion

        #region GetServiceEndpoints(...)

        /// <summary>
        /// Download OCHPdirect provider endpoints.
        /// </summary>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<HTTPResponse<GetServiceEndpointsResponse>>

            GetServiceEndpoints(DateTime?           Timestamp          = null,
                                CancellationToken?  CancellationToken  = null,
                                EventTracking_Id    EventTrackingId    = null,
                                TimeSpan?           RequestTimeout     = null)


                => CPOClient.GetServiceEndpoints(Timestamp,
                                                 CancellationToken,
                                                 EventTrackingId,
                                                 RequestTimeout);

        #endregion


        #region InformProvider(...)

        /// <summary>
        /// Send an inform provider OCHPdirect message.
        /// </summary>
        /// <param name="DirectMessage">The operation that triggered the operator to send this message.</param>
        /// <param name="EVSEId">The uqniue EVSE identification of the charge point which is used for this charging process.</param>
        /// <param name="ContractId">The current contract identification using the charge point.</param>
        /// <param name="DirectId">The session identification of the direct charging process.</param>
        /// 
        /// <param name="SessionTimeoutAt">On success the timeout for this session.</param>
        /// <param name="StateOfCharge">Current state of charge of the connected EV in percent.</param>
        /// <param name="MaxPower">Maximum authorised power in kW.</param>
        /// <param name="MaxCurrent">Maximum authorised current in A.</param>
        /// <param name="OnePhase">Marks an AC-charging session to be limited to one-phase charging.</param>
        /// <param name="MaxEnergy">Maximum authorised energy in kWh.</param>
        /// <param name="MinEnergy">Minimum required energy in kWh.</param>
        /// <param name="Departure">Scheduled time of departure.</param>
        /// <param name="CurrentPower">The currently supplied power limit in kWs (in case of load management).</param>
        /// <param name="ChargedEnergy">The overall amount of energy in kWhs transferred during this charging process.</param>
        /// <param name="MeterReading">The current meter value as displayed on the meter with corresponding timestamp to enable displaying it to the user.</param>
        /// <param name="ChargingPeriods">Can be used to transfer billing information to the provider in near real time.</param>
        /// <param name="CurrentCost">The total cost of the charging process that will be billed by the operator up to this point.</param>
        /// <param name="Currency">The displayed and charged currency. Defined in ISO 4217 - Table A.1, alphabetic list.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public Task<HTTPResponse<InformProviderResponse>>

            InformProvider(DirectMessages          DirectMessage,
                           EVSE_Id                 EVSEId,
                           Contract_Id             ContractId,
                           Direct_Id               DirectId,

                           DateTime?               SessionTimeoutAt   = null,
                           Single?                 StateOfCharge      = null,
                           Single?                 MaxPower           = null,
                           Single?                 MaxCurrent         = null,
                           Boolean?                OnePhase           = null,
                           Single?                 MaxEnergy          = null,
                           Single?                 MinEnergy          = null,
                           DateTime?               Departure          = null,
                           Single?                 CurrentPower       = null,
                           Single?                 ChargedEnergy      = null,
                           Timestamped<Single>?    MeterReading       = null,
                           IEnumerable<CDRPeriod>  ChargingPeriods    = null,
                           Single?                 CurrentCost        = null,
                           Currency                Currency           = null,

                           DateTime?               Timestamp          = null,
                           CancellationToken?      CancellationToken  = null,
                           EventTracking_Id        EventTrackingId    = null,
                           TimeSpan?               RequestTimeout     = null)


                => CPOClient.InformProvider(DirectMessage,
                                            EVSEId,
                                            ContractId,
                                            DirectId,

                                            SessionTimeoutAt,
                                            StateOfCharge,
                                            MaxPower,
                                            MaxCurrent,
                                            OnePhase,
                                            MaxEnergy,
                                            MinEnergy,
                                            Departure,
                                            CurrentPower,
                                            ChargedEnergy,
                                            MeterReading,
                                            ChargingPeriods,
                                            CurrentCost,
                                            Currency,

                                            Timestamp,
                                            CancellationToken,
                                            EventTrackingId,
                                            RequestTimeout);

        #endregion



        #region Start()

        public void Start()
        {
            CPOServer.Start();
        }

        #endregion

        #region Shutdown(Message = null, Wait = true)

        public void Shutdown(String Message = null, Boolean Wait = true)
        {
            CPOServer.Shutdown(Message, Wait);
        }

        #endregion


    }

}
