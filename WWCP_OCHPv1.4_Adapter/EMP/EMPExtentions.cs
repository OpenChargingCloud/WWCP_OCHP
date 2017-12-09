﻿/*
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
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Extentions methods for the WWCP wrapper for OCHP roaming clients for e-mobility providers/EMPs.
    /// </summary>
    public static class EMPExtentions
    {

        #region CreateOCHPv1_4_EMPRoamingProvider(this RoamingNetwork, Id, Name, RemoteHostname, ...)

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
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// 
        /// <param name="ServerName">An optional identification string for the HTTP server.</param>
        /// <param name="ServerTCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// <param name="ServerAutoStart">Whether to start the server immediately or not.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record after receiving it from the roaming provider.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// 
        /// <param name="OCHPConfigurator">An optional delegate to configure the new OCHP roaming provider after its creation.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        public static OCHPv1_4.EMP.WWCPEMPAdapter

            CreateOCHPv1_4_EMPRoamingProvider(this RoamingNetwork                       RoamingNetwork,
                                              EMPRoamingProvider_Id                     Id,
                                              I18NString                                Name,

                                              String                                    RemoteHostname,
                                              IPPort                                    RemoteTCPPort                     = null,
                                              RemoteCertificateValidationCallback       RemoteCertificateValidator        = null,
                                              LocalCertificateSelectionCallback         LocalCertificateSelector          = null,
                                              X509Certificate                           ClientCert                        = null,
                                              String                                    RemoteHTTPVirtualHost             = null,
                                              String                                    URIPrefix                         = OCHPv1_4.EMP.EMPClient.DefaultURIPrefix,
                                              Tuple<String, String>                     WSSLoginPassword                  = null,
                                              //String                                    EVSEDataURI                       = OCHPv1_4.EMP.EMPClient.DefaultEVSEDataURI,
                                              //String                                    EVSEStatusURI                     = OCHPv1_4.EMP.EMPClient.DefaultEVSEStatusURI,
                                              //String                                    AuthenticationDataURI             = OCHPv1_4.EMP.EMPClient.DefaultAuthenticationDataURI,
                                              //String                                    ReservationURI                    = OCHPv1_4.EMP.EMPClient.DefaultReservationURI,
                                              //String                                    AuthorizationURI                  = OCHPv1_4.EMP.EMPClient.DefaultAuthorizationURI,
                                              OCHPv1_4.Provider_Id?                     DefaultProviderId                 = null,

                                              String                                    HTTPUserAgent                     = OCHPv1_4.EMP.EMPClient.DefaultHTTPUserAgent,
                                              TimeSpan?                                 RequestTimeout                    = null,
                                              Byte?                                     MaxNumberOfRetries                = OCHPv1_4.EMP.EMPClient.DefaultMaxNumberOfRetries,

                                              String                                    ServerName                        = OCHPv1_4.EMP.EMPServer.DefaultHTTPServerName,
                                              String                                    ServiceId                         = null,
                                              IPPort                                    ServerTCPPort                     = null,
                                              String                                    ServerURIPrefix                   = OCHPv1_4.EMP.EMPServer.DefaultURIPrefix,
                                              //String                                    ServerAuthorizationURI            = OCHPv1_4.EMP.EMPServer.DefaultAuthorizationURI,
                                              HTTPContentType                           ServerContentType                 = null,
                                              Boolean                                   ServerRegisterHTTPRootService     = true,
                                              Boolean                                   ServerAutoStart                   = false,

                                              String                                    ClientLoggingContext              = OCHPv1_4.EMP.EMPClient.EMPClientLogger.DefaultContext,
                                              String                                    ServerLoggingContext              = OCHPv1_4.EMP.EMPServerLogger.DefaultContext,
                                              LogfileCreatorDelegate                    LogfileCreator                    = null,

                                              //OCHPv1_4.EMP.EVSEDataRecord2EVSEDelegate  EVSEDataRecord2EVSE               = null,

                                              //OCHPv1_4.EVSEOperatorFilterDelegate       EVSEOperatorFilter                = null,

                                              TimeSpan?                                 PullDataServiceEvery              = null,
                                              Boolean                                   DisablePullData                   = false,
                                              TimeSpan?                                 PullDataServiceRequestTimeout     = null,

                                              TimeSpan?                                 PullStatusServiceEvery            = null,
                                              Boolean                                   DisablePullStatus                 = false,
                                              TimeSpan?                                 PullStatusServiceRequestTimeout   = null,

                                              eMobilityProvider                         DefaultProvider                   = null,
                                              GeoCoordinate?                            DefaultSearchCenter               = null,
                                              UInt64?                                   DefaultDistanceKM                 = null,

                                              DNSClient                                 DNSClient                         = null,

                                              Action<OCHPv1_4.EMP.WWCPEMPAdapter>       OCHPConfigurator                  = null,
                                              Action<IEMPRoamingProvider>               Configurator                      = null)

        {

            #region Initial checks

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

            if (Id == null)
                throw new ArgumentNullException(nameof(Id),              "The given unique roaming provider identification must not be null!");

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),            "The given roaming provider name must not be null or empty!");

            if (RemoteHostname == null)
                throw new ArgumentNullException(nameof(RemoteHostname),  "The given remote hostname must not be null!");

            #endregion

            var NewRoamingProvider = new OCHPv1_4.EMP.WWCPEMPAdapter(Id,
                                                                     Name,
                                                                     RoamingNetwork,

                                                                     RemoteHostname,
                                                                     RemoteTCPPort,
                                                                     RemoteCertificateValidator,
                                                                     LocalCertificateSelector,
                                                                     ClientCert,
                                                                     RemoteHTTPVirtualHost,
                                                                     URIPrefix,
                                                                     WSSLoginPassword,
                                                                     //EVSEDataURI,
                                                                     //EVSEStatusURI,
                                                                     //AuthenticationDataURI,
                                                                     //ReservationURI,
                                                                     //AuthorizationURI,
                                                                     //DefaultProviderId,

                                                                     HTTPUserAgent,
                                                                     RequestTimeout,
                                                                     MaxNumberOfRetries,

                                                                     ServerName,
                                                                     ServiceId,
                                                                     ServerTCPPort,
                                                                     ServerURIPrefix,
                                                                     //ServerAuthorizationURI,
                                                                     ServerContentType,
                                                                     ServerRegisterHTTPRootService,
                                                                     ServerAutoStart,

                                                                     ClientLoggingContext,
                                                                     ServerLoggingContext,
                                                                     LogfileCreator,

                                                                     //EVSEDataRecord2EVSE,

                                                                     //EVSEOperatorFilter,

                                                                     PullDataServiceEvery,
                                                                     DisablePullData,
                                                                     PullDataServiceRequestTimeout,

                                                                     PullStatusServiceEvery,
                                                                     DisablePullStatus,
                                                                     PullStatusServiceRequestTimeout,

                                                                     DefaultProvider,
                                                                     DefaultSearchCenter,
                                                                     DefaultDistanceKM,

                                                                     DNSClient);


            OCHPConfigurator?.Invoke(NewRoamingProvider);

            return RoamingNetwork.
                       CreateNewRoamingProvider(NewRoamingProvider,
                                                Configurator) as OCHPv1_4.EMP.WWCPEMPAdapter;

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
        /// <param name="ServerURIPrefix">An optional prefix for the HTTP URIs.</param>
        /// 
        /// <param name="RemoteHostname">The hostname of the remote OCHP service.</param>
        /// <param name="RemoteTCPPort">An optional TCP port of the remote OCHP service.</param>
        /// <param name="RemoteHTTPVirtualHost">An optional HTTP virtual hostname of the remote OCHP service.</param>
        /// <param name="RemoteCertificateValidator">A delegate to verify the remote TLS certificate.</param>
        /// <param name="HTTPUserAgent">An optional HTTP user agent identification string for this HTTP client.</param>
        /// <param name="RequestTimeout">An optional timeout for upstream queries.</param>
        /// <param name="MaxNumberOfRetries">The default number of maximum transmission retries.</param>
        /// 
        /// <param name="ClientLoggingContext">An optional context for logging client methods.</param>
        /// <param name="ServerLoggingContext">An optional context for logging server methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// 
        /// <param name="EVSEDataRecord2EVSE">A delegate to process an EVSE data record after receiving it from the roaming provider.</param>
        /// 
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// 
        /// <param name="OCHPConfigurator">An optional delegate to configure the new OCHP roaming provider after its creation.</param>
        /// <param name="Configurator">An optional delegate to configure the new roaming provider after its creation.</param>
        public static OCHPv1_4.EMP.WWCPEMPAdapter

            CreateOCHPv1_4_EMPRoamingProvider(this RoamingNetwork                       RoamingNetwork,
                                              EMPRoamingProvider_Id                     Id,
                                              I18NString                                Name,
                                              SOAPServer                                SOAPServer,

                                              String                                    RemoteHostname,
                                              IPPort                                    RemoteTCPPort                     = null,
                                              RemoteCertificateValidationCallback       RemoteCertificateValidator        = null,
                                              LocalCertificateSelectionCallback         LocalCertificateSelector          = null,
                                              X509Certificate                           ClientCert                        = null,
                                              String                                    RemoteHTTPVirtualHost             = null,
                                              String                                    URIPrefix                         = OCHPv1_4.EMP.EMPClient.DefaultURIPrefix,
                                              Tuple<String, String>                     WSSLoginPassword                  = null,
                                              //String                                    EVSEDataURI                       = OCHPv1_4.EMP.EMPClient.DefaultEVSEDataURI,
                                              //String                                    EVSEStatusURI                     = OCHPv1_4.EMP.EMPClient.DefaultEVSEStatusURI,
                                              //String                                    AuthenticationDataURI             = OCHPv1_4.EMP.EMPClient.DefaultAuthenticationDataURI,
                                              //String                                    ReservationURI                    = OCHPv1_4.EMP.EMPClient.DefaultReservationURI,
                                              //String                                    AuthorizationURI                  = OCHPv1_4.EMP.EMPClient.DefaultAuthorizationURI,
                                              //OCHPv1_4.Provider_Id?                     DefaultProviderId                 = null,

                                              String                                    HTTPUserAgent                     = OCHPv1_4.EMP.EMPClient.DefaultHTTPUserAgent,
                                              TimeSpan?                                 RequestTimeout                    = null,
                                              Byte?                                     MaxNumberOfRetries                = OCHPv1_4.EMP.EMPClient.DefaultMaxNumberOfRetries,

                                              String                                    ServiceId                         = null,
                                              String                                    ServerURIPrefix                   = null,
                                              //String                                    ServerAuthorizationURI            = OCHPv1_4.EMP.EMPServer.DefaultAuthorizationURI,

                                              String                                    ClientLoggingContext              = OCHPv1_4.EMP.EMPClient.EMPClientLogger.DefaultContext,
                                              String                                    ServerLoggingContext              = OCHPv1_4.EMP.EMPServerLogger.DefaultContext,
                                              LogfileCreatorDelegate                    LogfileCreator                    = null,

                                              //OCHPv1_4.EMP.EVSEDataRecord2EVSEDelegate  EVSEDataRecord2EVSE               = null,

                                              //OCHPv1_4.EVSEOperatorFilterDelegate       EVSEOperatorFilter                = null,

                                              TimeSpan?                                 PullDataServiceEvery              = null,
                                              Boolean                                   DisablePullData                   = false,
                                              TimeSpan?                                 PullDataServiceRequestTimeout     = null,

                                              TimeSpan?                                 PullStatusServiceEvery            = null,
                                              Boolean                                   DisablePullStatus                 = false,
                                              TimeSpan?                                 PullStatusServiceRequestTimeout   = null,

                                              eMobilityProvider                         DefaultProvider                   = null,
                                              GeoCoordinate?                            DefaultSearchCenter               = null,
                                              UInt64?                                   DefaultDistanceKM                 = null,

                                              DNSClient                                 DNSClient                         = null,

                                              Action<OCHPv1_4.EMP.WWCPEMPAdapter>       OCHPConfigurator                  = null,
                                              Action<IEMPRoamingProvider>               Configurator                      = null)

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

            var NewRoamingProvider = new OCHPv1_4.EMP.WWCPEMPAdapter(Id,
                                                                     Name,
                                                                     RoamingNetwork,

                                                                     new OCHPv1_4.EMP.EMPClient(Id.ToString(),
                                                                                                RemoteHostname,
                                                                                                RemoteTCPPort,
                                                                                                RemoteCertificateValidator,
                                                                                                LocalCertificateSelector,
                                                                                                ClientCert,
                                                                                                RemoteHTTPVirtualHost,
                                                                                                URIPrefix,
                                                                                                WSSLoginPassword,
                                                                                                HTTPUserAgent,
                                                                                                RequestTimeout,
                                                                                                MaxNumberOfRetries,
                                                                                                DNSClient,
                                                                                                ClientLoggingContext,
                                                                                                LogfileCreator),

                                                                     new OCHPv1_4.EMP.EMPServer(SOAPServer,
                                                                                                ServiceId,
                                                                                                ServerURIPrefix),

                                                                     ServerLoggingContext,
                                                                     LogfileCreator,

                                                                     //EVSEDataRecord2EVSE,

                                                                     //EVSEOperatorFilter,

                                                                     PullDataServiceEvery,
                                                                     DisablePullData,
                                                                     PullDataServiceRequestTimeout,

                                                                     PullStatusServiceEvery,
                                                                     DisablePullStatus,
                                                                     PullStatusServiceRequestTimeout,

                                                                     DefaultProvider,
                                                                     DefaultSearchCenter,
                                                                     DefaultDistanceKM);


            OCHPConfigurator?.Invoke(NewRoamingProvider);

            return RoamingNetwork.
                       CreateNewRoamingProvider(NewRoamingProvider,
                                                Configurator) as OCHPv1_4.EMP.WWCPEMPAdapter;

        }

        #endregion

    }

}