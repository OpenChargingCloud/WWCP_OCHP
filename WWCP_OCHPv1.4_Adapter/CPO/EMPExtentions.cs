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
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.SOAP;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Extentions methods for the WWCP wrapper for OCHP roaming clients for charging station operators.
    /// </summary>
    public static class EMPExtentions
    {

        #region CreateOCHPv1_4_EMPRoamingProvider(this RoamingNetwork, Id, Name, RemoteHostname, ... , Action = null)

        /// <summary>
        /// Create and register a new electric vehicle roaming provider
        /// using the OCHP protocol and having the given unique electric
        /// vehicle roaming provider identification.
        /// </summary>
        /// 
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// 
        /// <param name="RemoteHostname">The hostname of the remote OCHP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote OCHP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OCHP service.</param>
        /// <param name="URLPrefix">An default URI prefix.</param>
        /// <param name="WSSLoginPassword">The WebService-Security username/password.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// 
        /// <param name="ServerName"> An optional identification string for the HTTP server.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURLPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerURLSuffix">An optional HTTP/SOAP/XML server URI suffix.</param>
        /// <param name="ServerContentType">An optional HTTP content type to use.</param>
        /// <param name="ServerRegisterHTTPRootService">Register HTTP root services for sending a notice to clients connecting via HTML or plain text.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSE2ChargePointInfo">A delegate to process an charge point info, e.g. before pushing it to the roaming provider.</param>
        /// <param name="ChargePointInfo2XML">A delegate to process the XML representation of an charge point info, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// <param name="EVSEStatusRefreshEvery">The EVSE status refresh intervall.</param>
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableEVSEStatusRefresh">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="OCHPConfigurator">An optional delegate to configure the new OCHP roaming provider after its creation.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public static OCHPv1_4.CPO.WWCPEMPAdapter

            CreateOCHPv1_4_EMPRoamingProvider(this RoamingNetwork                                               RoamingNetwork,
                                              EMPRoamingProvider_Id                                             Id,
                                              I18NString                                                        Description,
                                              I18NString                                                        Name,

                                              HTTPHostname                                                      RemoteHostname,
                                              IPPort?                                                           RemoteTCPPort                       = null,
                                              HTTPHostname?                                                     RemoteHTTPVirtualHost               = null,
                                              RemoteCertificateValidationCallback                               RemoteCertificateValidator          = null,
                                              LocalCertificateSelectionCallback                                 ClientCertificateSelector           = null,
                                              HTTPPath?                                                         URLPrefix                           = null,
                                              HTTPPath?                                                         LiveURLPrefix                       = null,
                                              Tuple<String, String>                                             WSSLoginPassword                    = null,
                                              String                                                            HTTPUserAgent                       = OCHPv1_4.CPO.CPOClient.DefaultHTTPUserAgent,
                                              TimeSpan?                                                         RequestTimeout                      = null,
                                              Byte?                                                             MaxNumberOfRetries                  = OCHPv1_4.CPO.CPOClient.DefaultMaxNumberOfRetries,

                                              String                                                            ServerName                          = OCHPv1_4.CPO.CPOServer.DefaultHTTPServerName,
                                              String                                                            ServiceId                           = null,
                                              IPPort?                                                           ServerTCPPort                       = null,
                                              HTTPPath?                                                         ServerURLPrefix                     = null,
                                              HTTPPath?                                                         ServerURLSuffix                     = null,
                                              HTTPContentType                                                   ServerContentType                   = null,
                                              Boolean                                                           ServerRegisterHTTPRootService       = true,
                                              Boolean                                                           ServerAutoStart                     = false,

                                              String                                                            ClientLoggingContext                = OCHPv1_4.CPO.CPOClient.CPOClientLogger.DefaultContext,
                                              String                                                            ServerLoggingContext                = OCHPv1_4.CPO.CPOServerLogger.DefaultContext,
                                              LogfileCreatorDelegate                                            LogFileCreator                      = null,

                                              OCHPv1_4.CPO.EVSE2ChargePointInfoDelegate                         EVSE2ChargePointInfo                = null,
                                              OCHPv1_4.CPO.EVSEStatusUpdate2EVSEStatusDelegate                  EVSEStatusUpdate2EVSEStatus         = null,
                                              OCHPv1_4.CPO.ChargePointInfo2XMLDelegate                          ChargePointInfo2XML                 = null,
                                              OCHPv1_4.CPO.EVSEStatus2XMLDelegate                               EVSEStatus2XML                      = null,

                                              IncludeEVSEIdDelegate                                             IncludeEVSEIds                      = null,
                                              IncludeEVSEDelegate                                               IncludeEVSEs                        = null,
                                              IncludeChargingStationIdDelegate                                  IncludeChargingStationIds           = null,
                                              IncludeChargingStationDelegate                                    IncludeChargingStations             = null,
                                              OCHPv1_4.CPO.IncludeChargePointDelegate                           IncludeChargePoints                 = null,
                                              ChargeDetailRecordFilterDelegate                                  ChargeDetailRecordFilter            = null,
                                              OCHPv1_4.CPO.CustomEVSEIdMapperDelegate                           CustomEVSEIdMapper                  = null,

                                              TimeSpan?                                                         ServiceCheckEvery                   = null,
                                              TimeSpan?                                                         StatusCheckEvery                    = null,
                                              TimeSpan?                                                         EVSEStatusRefreshEvery              = null,
                                              TimeSpan?                                                         CDRCheckEvery                       = null,

                                              Boolean                                                           DisablePushData                     = false,
                                              Boolean                                                           DisablePushStatus                   = false,
                                              Boolean                                                           DisableEVSEStatusRefresh            = false,
                                              Boolean                                                           DisableAuthentication               = false,
                                              Boolean                                                           DisableSendChargeDetailRecords      = false,

                                              Action<OCHPv1_4.CPO.WWCPEMPAdapter>                               OCHPConfigurator                    = null,
                                              Action<IEMPRoamingProvider>                                       Configurator                        = null,
                                              DNSClient                                                         DNSClient                           = null)

        {

            #region Initial checks

            if (RoamingNetwork    == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

            if (Id == null)
                throw new ArgumentNullException(nameof(Id),              "The given unique roaming provider identification must not be null!");

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),            "The given roaming provider name must not be null or empty!");

            if (RemoteHostname    == null)
                throw new ArgumentNullException(nameof(RemoteHostname),  "The given remote hostname must not be null!");

            #endregion

            var NewRoamingProvider = new OCHPv1_4.CPO.WWCPEMPAdapter(Id,
                                                                     Name,
                                                                     Description,
                                                                     RoamingNetwork,

                                                                     RemoteHostname,
                                                                     RemoteTCPPort,
                                                                     RemoteCertificateValidator,
                                                                     ClientCertificateSelector,
                                                                     RemoteHTTPVirtualHost,
                                                                     URLPrefix     ?? OCHPv1_4.CPO.CPOClient.DefaultURLPrefix,
                                                                     LiveURLPrefix ?? OCHPv1_4.CPO.CPOClient.DefaultLiveURLPrefix,
                                                                     WSSLoginPassword,
                                                                     HTTPUserAgent,
                                                                     RequestTimeout,
                                                                     MaxNumberOfRetries,

                                                                     ServerName,
                                                                     ServiceId,
                                                                     ServerTCPPort,
                                                                     ServerURLPrefix ?? OCHPv1_4.CPO.CPOServer.DefaultURLPrefix,
                                                                     ServerURLSuffix ?? OCHPv1_4.CPO.CPOServer.DefaultURLSuffix,
                                                                     ServerContentType,
                                                                     ServerRegisterHTTPRootService,
                                                                     ServerAutoStart,

                                                                     ClientLoggingContext,
                                                                     ServerLoggingContext,
                                                                     LogFileCreator,

                                                                     EVSE2ChargePointInfo,
                                                                     EVSEStatusUpdate2EVSEStatus,
                                                                     ChargePointInfo2XML,
                                                                     EVSEStatus2XML,

                                                                     IncludeEVSEIds,
                                                                     IncludeEVSEs,
                                                                     IncludeChargingStationIds,
                                                                     IncludeChargingStations,
                                                                     IncludeChargePoints,
                                                                     ChargeDetailRecordFilter,
                                                                     CustomEVSEIdMapper,

                                                                     ServiceCheckEvery,
                                                                     StatusCheckEvery,
                                                                     EVSEStatusRefreshEvery,
                                                                     CDRCheckEvery,

                                                                     DisablePushData,
                                                                     DisablePushStatus,
                                                                     DisableEVSEStatusRefresh,
                                                                     DisableAuthentication,
                                                                     DisableSendChargeDetailRecords,

                                                                     DNSClient);


            OCHPConfigurator?.Invoke(NewRoamingProvider);

            return RoamingNetwork.
                       CreateNewRoamingProvider(NewRoamingProvider,
                                                Configurator) as OCHPv1_4.CPO.WWCPEMPAdapter;

        }

        #endregion

        #region CreateOCHPv1_4_EMPRoamingProvider(this RoamingNetwork, Id, Name, SOAPServer, RemoteHostname, ...)

        /// <summary>
        /// Create and register a new electric vehicle roaming provider
        /// using the OCHP protocol and having the given unique electric
        /// vehicle roaming provider identification.
        /// </summary>
        /// 
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The offical (multi-language) name of the roaming provider.</param>
        /// <param name="SOAPServer">An optional identification string for the HTTP server.</param>
        /// <param name="ServerURLPrefix">An optional prefix for the HTTP URIs.</param>
        /// 
        /// <param name="RemoteHostname">The hostname of the remote OCHP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote OCHP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OCHP service.</param>
        /// <param name="URLPrefix">An default URI prefix.</param>
        /// <param name="ServerURLSuffix">An optional HTTP/SOAP/XML server URI suffix.</param>
        /// <param name="WSSLoginPassword">The WebService-Security username/password.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogFileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSE2ChargePointInfo">A delegate to process an charge point info, e.g. before pushing it to the roaming provider.</param>
        /// <param name="ChargePointInfo2XML">A delegate to process the XML representation of an charge point info, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// <param name="EVSEStatusRefreshEvery">The EVSE status refresh intervall.</param>
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableEVSEStatusRefresh">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="OCHPConfigurator">An optional delegate to configure the new OCHP roaming provider after its creation.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public static OCHPv1_4.CPO.WWCPEMPAdapter

            CreateOCHPv1_4_EMPRoamingProvider(this RoamingNetwork                                RoamingNetwork,
                                              EMPRoamingProvider_Id                              Id,
                                              I18NString                                         Name,
                                              I18NString                                         Description,
                                              SOAPServer                                         SOAPServer,

                                              HTTPHostname                                       RemoteHostname,
                                              IPPort?                                            RemoteTCPPort                       = null,
                                              RemoteCertificateValidationCallback                RemoteCertificateValidator          = null,
                                              LocalCertificateSelectionCallback                  ClientCertificateSelector           = null,
                                              HTTPHostname?                                      RemoteHTTPVirtualHost               = null,
                                              HTTPPath?                                          URLPrefix                           = null,
                                              HTTPPath?                                          LiveURLPrefix                       = null,
                                              Tuple<String, String>                              WSSLoginPassword                    = null,
                                              String                                             HTTPUserAgent                       = OCHPv1_4.CPO.CPOClient.DefaultHTTPUserAgent,
                                              TimeSpan?                                          RequestTimeout                      = null,
                                              Byte?                                              MaxNumberOfRetries                  = OCHPv1_4.CPO.CPOClient.DefaultMaxNumberOfRetries,

                                              String                                             ServiceId                           = null,
                                              HTTPPath?                                          ServerURLPrefix                     = null,
                                              HTTPPath?                                          ServerURLSuffix                     = null,

                                              String                                             ClientLoggingContext                = OCHPv1_4.CPO.CPOClient.CPOClientLogger.DefaultContext,
                                              String                                             ServerLoggingContext                = OCHPv1_4.CPO.CPOServerLogger.DefaultContext,
                                              LogfileCreatorDelegate                             LogFileCreator                      = null,

                                              OCHPv1_4.CPO.CustomEVSEIdMapperDelegate            CustomEVSEIdMapper                  = null,
                                              OCHPv1_4.CPO.EVSE2ChargePointInfoDelegate          EVSE2ChargePointInfo                = null,
                                              OCHPv1_4.CPO.EVSEStatusUpdate2EVSEStatusDelegate   EVSEStatusUpdate2EVSEStatus         = null,
                                              OCHPv1_4.CPO.ChargePointInfo2XMLDelegate           ChargePointInfo2XML                 = null,
                                              OCHPv1_4.CPO.EVSEStatus2XMLDelegate                EVSEStatus2XML                      = null,

                                              IncludeEVSEIdDelegate                              IncludeEVSEIds                      = null,
                                              IncludeEVSEDelegate                                IncludeEVSEs                        = null,
                                              IncludeChargingStationIdDelegate                   IncludeChargingStationIds           = null,
                                              IncludeChargingStationDelegate                     IncludeChargingStations             = null,
                                              OCHPv1_4.CPO.IncludeChargePointDelegate            IncludeChargePoints                 = null,
                                              ChargeDetailRecordFilterDelegate                   ChargeDetailRecordFilter            = null,

                                              TimeSpan?                                          ServiceCheckEvery                   = null,
                                              TimeSpan?                                          StatusCheckEvery                    = null,
                                              TimeSpan?                                          EVSEStatusRefreshEvery              = null,
                                              TimeSpan?                                          CDRCheckEvery                       = null,

                                              Boolean                                            DisablePushData                     = false,
                                              Boolean                                            DisablePushStatus                   = false,
                                              Boolean                                            DisableEVSEStatusRefresh            = false,
                                              Boolean                                            DisableAuthentication               = false,
                                              Boolean                                            DisableSendChargeDetailRecords      = false,

                                              Action<OCHPv1_4.CPO.WWCPEMPAdapter>                OCHPConfigurator                    = null,
                                              Action<IEMPRoamingProvider>                        Configurator                        = null,
                                              DNSClient                                          DNSClient                           = null)

        {

            #region Initial checks

            if (SOAPServer == null)
                throw new ArgumentNullException(nameof(SOAPServer),      "The given SOAP/HTTP server must not be null!");


            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

            if (Id == null)
                throw new ArgumentNullException(nameof(Id),              "The given unique roaming provider identification must not be null!");

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),            "The given roaming provider name must not be null or empty!");

            if (RemoteHostname == null)
                throw new ArgumentNullException(nameof(RemoteHostname),  "The given remote hostname must not be null!");

            #endregion

            var NewRoamingProvider = new OCHPv1_4.CPO.WWCPEMPAdapter(Id,
                                                                     Name,
                                                                     Description,
                                                                     RoamingNetwork,

                                                                     new OCHPv1_4.CPO.CPOClient(Id.ToString(),
                                                                                                RemoteHostname,
                                                                                                RemoteTCPPort,
                                                                                                RemoteCertificateValidator,
                                                                                                ClientCertificateSelector,
                                                                                                RemoteHTTPVirtualHost ?? RemoteHostname,
                                                                                                URLPrefix             ?? OCHPv1_4.CPO.CPOClient.DefaultURLPrefix,
                                                                                                LiveURLPrefix         ?? OCHPv1_4.CPO.CPOClient.DefaultLiveURLPrefix,
                                                                                                WSSLoginPassword,
                                                                                                HTTPUserAgent,
                                                                                                RequestTimeout,
                                                                                                MaxNumberOfRetries,
                                                                                                DNSClient,
                                                                                                ClientLoggingContext,
                                                                                                LogFileCreator),

                                                                     new OCHPv1_4.CPO.CPOServer(SOAPServer,
                                                                                                ServiceId,
                                                                                                ServerURLPrefix ?? OCHPv1_4.CPO.CPOServer.DefaultURLPrefix,
                                                                                                ServerURLSuffix ?? OCHPv1_4.CPO.CPOServer.DefaultURLSuffix),

                                                                     ServerLoggingContext,
                                                                     LogFileCreator,

                                                                     EVSE2ChargePointInfo,
                                                                     EVSEStatusUpdate2EVSEStatus,
                                                                     ChargePointInfo2XML,
                                                                     EVSEStatus2XML,

                                                                     IncludeEVSEIds,
                                                                     IncludeEVSEs,
                                                                     IncludeChargingStationIds,
                                                                     IncludeChargingStations,
                                                                     IncludeChargePoints,
                                                                     ChargeDetailRecordFilter,
                                                                     CustomEVSEIdMapper,

                                                                     ServiceCheckEvery,
                                                                     StatusCheckEvery,
                                                                     EVSEStatusRefreshEvery,
                                                                     CDRCheckEvery,

                                                                     DisablePushData,
                                                                     DisablePushStatus,
                                                                     DisableEVSEStatusRefresh,
                                                                     DisableAuthentication,
                                                                     DisableSendChargeDetailRecords);

            OCHPConfigurator?.Invoke(NewRoamingProvider);

            return RoamingNetwork.
                       CreateNewRoamingProvider(NewRoamingProvider,
                                                Configurator) as OCHPv1_4.CPO.WWCPEMPAdapter;

        }

        #endregion

    }

}
