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
    public static class CPOExtentions
    {

        #region CreateOCHPv1_4_CPORoamingProvider(this RoamingNetwork, Id, Name, RemoteHostname, ... , Action = null)

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
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OCHP service.</param>
        /// <param name="URIPrefix">An default URI prefix.</param>
        /// <param name="WSSLoginPassword">The WebService-Security username/password.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
        /// 
        /// <param name="ServerName"> An optional identification string for the HTTP server.</param>
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
        /// <param name="EVSE2ChargePointInfo">A delegate to process an charge point info, e.g. before pushing it to the roaming provider.</param>
        /// <param name="ChargePointInfo2XML">A delegate to process the XML representation of an charge point info, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="ServiceCheckEvery">The service check intervall.</param>
        /// <param name="StatusCheckEvery">The status check intervall.</param>
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="OCHPConfigurator">An optional delegate to configure the new OCHP roaming provider after its creation.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public static ICSORoamingProvider

            CreateOCHPv1_4_CPORoamingProvider(this RoamingNetwork                                     RoamingNetwork,
                                              CSORoamingProvider_Id                                   Id,
                                              I18NString                                              Name,

                                              String                                                  RemoteHostname,
                                              IPPort                                                  RemoteTCPPort                       = null,
                                              String                                                  RemoteHTTPVirtualHost               = null,
                                              RemoteCertificateValidationCallback                     RemoteCertificateValidator          = null,
                                              X509Certificate                                         ClientCert                          = null,
                                              String                                                  URIPrefix                           = OCHPv1_4.CPO.CPOClient.DefaultURIPrefix,
                                              Tuple<String, String>                                   WSSLoginPassword                    = null,
                                              String                                                  HTTPUserAgent                       = OCHPv1_4.CPO.CPOClient.DefaultHTTPUserAgent,
                                              TimeSpan?                                               QueryTimeout                        = null,

                                              String                                                  ServerName                          = OCHPv1_4.CPO.CPOServer.DefaultHTTPServerName,
                                              IPPort                                                  ServerTCPPort                       = null,
                                              String                                                  ServerURIPrefix                     = OCHPv1_4.CPO.CPOServer.DefaultURIPrefix,
                                              String                                                  ServerURISuffix                     = OCHPv1_4.CPO.CPOServer.DefaultURISuffix,
                                              HTTPContentType                                         ServerContentType                   = null,
                                              Boolean                                                 ServerRegisterHTTPRootService       = true,
                                              Boolean                                                 ServerAutoStart                     = false,

                                              String                                                  ClientLoggingContext                = OCHPv1_4.CPO.CPOClient.CPOClientLogger.DefaultContext,
                                              String                                                  ServerLoggingContext                = OCHPv1_4.CPO.CPOServerLogger.DefaultContext,
                                              LogfileCreatorDelegate                                  LogFileCreator                      = null,

                                              OCHPv1_4.CPO.CustomEVSEIdMapperDelegate                 CustomEVSEIdMapper                  = null,
                                              OCHPv1_4.CPO.EVSE2ChargePointInfoDelegate               EVSE2ChargePointInfo                = null,
                                              OCHPv1_4.CPO.EVSEStatusUpdate2EVSEStatusDelegate        EVSEStatusUpdate2EVSEStatus         = null,
                                              OCHPv1_4.CPO.ChargePointInfo2XMLDelegate                ChargePointInfo2XML                 = null,
                                              OCHPv1_4.CPO.EVSEStatus2XMLDelegate                     EVSEStatus2XML                      = null,

                                              IncludeEVSEDelegate                                     IncludeEVSEs                        = null,
                                              TimeSpan?                                               ServiceCheckEvery                   = null,
                                              TimeSpan?                                               StatusCheckEvery                    = null,

                                              Boolean                                                 DisablePushData                     = false,
                                              Boolean                                                 DisablePushStatus                   = false,
                                              Boolean                                                 DisableAuthentication               = false,
                                              Boolean                                                 DisableSendChargeDetailRecords      = false,

                                              Action<OCHPv1_4.CPO.WWCPCPOAdapter>                     OCHPConfigurator                    = null,
                                              Action<ICSORoamingProvider>                             Configurator                        = null,
                                              DNSClient                                               DNSClient                           = null)

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

            var NewRoamingProvider = new OCHPv1_4.CPO.WWCPCPOAdapter(Id,
                                                                     Name,
                                                                     RoamingNetwork,

                                                                     RemoteHostname,
                                                                     RemoteTCPPort,
                                                                     RemoteCertificateValidator,
                                                                     ClientCert,
                                                                     RemoteHTTPVirtualHost,
                                                                     URIPrefix,
                                                                     WSSLoginPassword,
                                                                     HTTPUserAgent,
                                                                     QueryTimeout,

                                                                     ServerName,
                                                                     ServerTCPPort,
                                                                     ServerURIPrefix,
                                                                     ServerURISuffix,
                                                                     ServerContentType,
                                                                     ServerRegisterHTTPRootService,
                                                                     ServerAutoStart,

                                                                     ClientLoggingContext,
                                                                     ServerLoggingContext,
                                                                     LogFileCreator,

                                                                     CustomEVSEIdMapper,
                                                                     EVSE2ChargePointInfo,
                                                                     EVSEStatusUpdate2EVSEStatus,
                                                                     ChargePointInfo2XML,
                                                                     EVSEStatus2XML,

                                                                     IncludeEVSEs,
                                                                     ServiceCheckEvery,
                                                                     StatusCheckEvery,

                                                                     DisablePushData,
                                                                     DisablePushStatus,
                                                                     DisableAuthentication,
                                                                     DisableSendChargeDetailRecords,

                                                                     DNSClient);


            OCHPConfigurator?.Invoke(NewRoamingProvider);

            return RoamingNetwork.
                       CreateNewRoamingProvider(NewRoamingProvider,
                                                Configurator);

        }

        #endregion

        #region CreateOCHPv1_4_CPORoamingProvider(this RoamingNetwork, Id, Name, SOAPServer, RemoteHostname, ...)

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
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// 
        /// <param name="RemoteHostname">The hostname of the remote OCHP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote OCHP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="ClientCert">The TLS client certificate to use.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OCHP service.</param>
        /// <param name="URIPrefix">An default URI prefix.</param>
        /// <param name="ServerURISuffix">An optional HTTP/SOAP/XML server URI suffix.</param>
        /// <param name="WSSLoginPassword">The WebService-Security username/password.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="QueryTimeout">An optional timeout for upstream queries.</param>
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
        /// 
        /// <param name="DisablePushData">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisablePushStatus">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableAuthentication">This service can be disabled, e.g. for debugging reasons.</param>
        /// <param name="DisableSendChargeDetailRecords">This service can be disabled, e.g. for debugging reasons.</param>
        /// 
        /// <param name="OCHPConfigurator">An optional delegate to configure the new OCHP roaming provider after its creation.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        public static ICSORoamingProvider

            CreateOCHPv1_4_CPORoamingProvider(this RoamingNetwork                                     RoamingNetwork,
                                              CSORoamingProvider_Id                                   Id,
                                              I18NString                                              Name,
                                              SOAPServer                                              SOAPServer,

                                              String                                                  RemoteHostname,
                                              IPPort                                                  RemoteTCPPort                       = null,
                                              RemoteCertificateValidationCallback                     RemoteCertificateValidator          = null,
                                              X509Certificate                                         ClientCert                          = null,
                                              String                                                  RemoteHTTPVirtualHost               = null,
                                              String                                                  URIPrefix                           = OCHPv1_4.CPO.CPOClient.DefaultURIPrefix,
                                              Tuple<String, String>                                   WSSLoginPassword                    = null,
                                              String                                                  HTTPUserAgent                       = OCHPv1_4.CPO.CPOClient.DefaultHTTPUserAgent,
                                              TimeSpan?                                               QueryTimeout                        = null,

                                              String                                                  ServerURIPrefix                     = OCHPv1_4.CPO.CPOServer.DefaultURIPrefix,
                                              String                                                  ServerURISuffix                     = OCHPv1_4.CPO.CPOServer.DefaultURISuffix,

                                              String                                                  ClientLoggingContext                = OCHPv1_4.CPO.CPOClient.CPOClientLogger.DefaultContext,
                                              String                                                  ServerLoggingContext                = OCHPv1_4.CPO.CPOServerLogger.DefaultContext,
                                              LogfileCreatorDelegate                                  LogFileCreator                      = null,

                                              OCHPv1_4.CPO.CustomEVSEIdMapperDelegate                 CustomEVSEIdMapper                  = null,
                                              OCHPv1_4.CPO.EVSE2ChargePointInfoDelegate               EVSE2ChargePointInfo                = null,
                                              OCHPv1_4.CPO.EVSEStatusUpdate2EVSEStatusDelegate        EVSEStatusUpdate2EVSEStatus         = null,
                                              OCHPv1_4.CPO.ChargePointInfo2XMLDelegate                ChargePointInfo2XML                 = null,
                                              OCHPv1_4.CPO.EVSEStatus2XMLDelegate                     EVSEStatus2XML                      = null,

                                              IncludeEVSEDelegate                                     IncludeEVSEs                        = null,
                                              TimeSpan?                                               ServiceCheckEvery                   = null,
                                              TimeSpan?                                               StatusCheckEvery                    = null,

                                              Boolean                                                 DisablePushData                     = false,
                                              Boolean                                                 DisablePushStatus                   = false,
                                              Boolean                                                 DisableAuthentication               = false,
                                              Boolean                                                 DisableSendChargeDetailRecords      = false,

                                              Action<OCHPv1_4.CPO.WWCPCPOAdapter>                     OCHPConfigurator                    = null,
                                              Action<ICSORoamingProvider>                             Configurator                        = null,
                                              DNSClient                                               DNSClient                           = null)

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

            var NewRoamingProvider = new OCHPv1_4.CPO.WWCPCPOAdapter(Id,
                                                                     Name,
                                                                     RoamingNetwork,

                                                                     new OCHPv1_4.CPO.CPOClient(Id.ToString(),
                                                                                                RemoteHostname,
                                                                                                RemoteTCPPort,
                                                                                                RemoteCertificateValidator,
                                                                                                ClientCert,
                                                                                                RemoteHTTPVirtualHost,
                                                                                                URIPrefix,
                                                                                                WSSLoginPassword,
                                                                                                HTTPUserAgent,
                                                                                                QueryTimeout,
                                                                                                DNSClient,
                                                                                                ClientLoggingContext,
                                                                                                LogFileCreator),

                                                                     new OCHPv1_4.CPO.CPOServer(SOAPServer,
                                                                                                ServerURIPrefix,
                                                                                                ServerURISuffix),

                                                                     ServerLoggingContext,
                                                                     LogFileCreator,

                                                                     CustomEVSEIdMapper,
                                                                     EVSE2ChargePointInfo,
                                                                     EVSEStatusUpdate2EVSEStatus,
                                                                     ChargePointInfo2XML,
                                                                     EVSEStatus2XML,

                                                                     IncludeEVSEs,
                                                                     ServiceCheckEvery,
                                                                     StatusCheckEvery,

                                                                     DisablePushData,
                                                                     DisablePushStatus,
                                                                     DisableAuthentication,
                                                                     DisableSendChargeDetailRecords);

            OCHPConfigurator?.Invoke(NewRoamingProvider);

            return RoamingNetwork.
                       CreateNewRoamingProvider(NewRoamingProvider,
                                                Configurator);

        }

        #endregion

    }

}
