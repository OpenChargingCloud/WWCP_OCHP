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

using Org.BouncyCastle.Crypto.Parameters;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.Logging;

using cloud.charging.open.protocols.OCHPv1_4.CPO;

#endregion

namespace cloud.charging.open.protocols.WWCP
{

    /// <summary>
    /// Extensions methods for the WWCP wrapper for OCHP roaming clients for charging station operators.
    /// </summary>
    public static class CPOExtensions
    {

        /// <summary>
        /// Create and register a new electric vehicle roaming provider
        /// using the OCHP protocol and having the given unique electric
        /// vehicle roaming provider identification.
        /// </summary>
        /// 
        /// <param name="RoamingNetwork">A WWCP roaming network.</param>
        /// <param name="Id">The unique identification of the roaming provider.</param>
        /// <param name="Name">The official (multi-language) name of the roaming provider.</param>
        /// 
        /// <param name="EVSE2ChargePointInfo">A delegate to process a charge point info, e.g. before pushing it to the roaming provider.</param>
        /// <param name="ChargePointInfo2XML">A delegate to process the XML representation of a charge point info, e.g. before pushing it to the roaming provider.</param>
        /// 
        /// <param name="IncludeEVSEs">Only include the EVSEs matching the given delegate.</param>
        /// <param name="ServiceCheckEvery">The service check interval.</param>
        /// <param name="StatusCheckEvery">The status check interval.</param>
        /// <param name="EVSEStatusRefreshEvery">The EVSE status refresh interval.</param>
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
        public static WWCPCSOAdapter

            CreateOCHPv1_4_EMPRoamingProvider(this RoamingNetwork                                 RoamingNetwork,
                                              CSORoamingProvider_Id                               Id,
                                              I18NString                                          Name,
                                              I18NString                                          Description,
                                              CPORoaming                                          CPORoaming,

                                              OCHPv1_4.CPO.EVSE2ChargePointInfoDelegate?          EVSE2ChargePointInfo                = null,
                                              OCHPv1_4.CPO.EVSEStatusUpdate2EVSEStatusDelegate?   EVSEStatusUpdate2EVSEStatus         = null,
                                              OCHPv1_4.CPO.ChargePointInfo2XMLDelegate?           ChargePointInfo2XML                 = null,
                                              OCHPv1_4.CPO.EVSEStatus2XMLDelegate?                EVSEStatus2XML                      = null,

                                              IncludeEVSEIdDelegate?                              IncludeEVSEIds                      = null,
                                              IncludeEVSEDelegate?                                IncludeEVSEs                        = null,
                                              IncludeChargingStationIdDelegate?                   IncludeChargingStationIds           = null,
                                              IncludeChargingStationDelegate?                     IncludeChargingStations             = null,

                                              OCHPv1_4.CPO.IncludeChargePointDelegate?            IncludeChargePoints                 = null,

                                              OCHPv1_4.CPO.CustomEVSEIdMapperDelegate?            CustomEVSEIdMapper                  = null,
                                              ChargeDetailRecordFilterDelegate?                   ChargeDetailRecordFilter            = null,

                                              TimeSpan?                                           ServiceCheckEvery                   = null,
                                              TimeSpan?                                           StatusCheckEvery                    = null,
                                              TimeSpan?                                           EVSEStatusRefreshEvery              = null,
                                              TimeSpan?                                           CDRCheckEvery                       = null,

                                              Boolean                                             DisablePushData                     = false,
                                              Boolean                                             DisablePushStatus                   = false,
                                              Boolean                                             DisableEVSEStatusRefresh            = false,
                                              Boolean                                             DisableAuthentication               = false,
                                              Boolean                                             DisableSendChargeDetailRecords      = false,

                                              String                                              EllipticCurve                       = "P-256",
                                              ECPrivateKeyParameters?                             PrivateKey                          = null,
                                              PublicKeyCertificates?                              PublicKeyCertificates               = null,

                                              Boolean?                                            IsDevelopment                       = null,
                                              IEnumerable<String>?                                DevelopmentServers                  = null,
                                              Boolean?                                            DisableLogging                      = false,
                                              String?                                             LoggingPath                         = null,
                                              String?                                             LoggingContext                      = null,
                                              String?                                             LogfileName                         = null,
                                              LogfileCreatorDelegate?                             LogfileCreator                      = null,

                                              String?                                             ClientsLoggingPath                  = null,
                                              String?                                             ClientsLoggingContext               = null,
                                              LogfileCreatorDelegate?                             ClientsLogfileCreator               = null,
                                              DNSClient?                                          DNSClient                           = null,

                                              Action<OCHPv1_4.CPO.WWCPCSOAdapter>?                OCHPConfigurator                    = null,
                                              Action<ICSORoamingProvider>?                        Configurator                        = null)

        {

            #region Initial checks

            if (RoamingNetwork is null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),            "The given roaming provider name must not be null or empty!");

            if (CPORoaming is null)
                throw new ArgumentNullException(nameof(CPORoaming),      "The given CPO roaming must not be null!");

            #endregion

            var newRoamingProvider = new WWCPCSOAdapter(

                                         Id,
                                         Name,
                                         Description,
                                         RoamingNetwork,
                                         CPORoaming,

                                         EVSE2ChargePointInfo,
                                         EVSEStatusUpdate2EVSEStatus,
                                         ChargePointInfo2XML,
                                         EVSEStatus2XML,

                                         IncludeChargingStationIds,
                                         IncludeChargingStations,
                                         IncludeEVSEIds,
                                         IncludeEVSEs,
                                         IncludeChargePoints,

                                         CustomEVSEIdMapper,
                                         ChargeDetailRecordFilter,

                                         ServiceCheckEvery,
                                         StatusCheckEvery,
                                         EVSEStatusRefreshEvery,
                                         CDRCheckEvery,

                                         DisablePushData,
                                         DisablePushStatus,
                                         DisableEVSEStatusRefresh,
                                         DisableAuthentication,
                                         DisableSendChargeDetailRecords,

                                         EllipticCurve,
                                         PrivateKey,
                                         PublicKeyCertificates,

                                         IsDevelopment,
                                         DevelopmentServers,
                                         DisableLogging,
                                         LoggingPath,
                                         LoggingContext,
                                         LogfileName,
                                         LogfileCreator,

                                         ClientsLoggingPath,
                                         ClientsLoggingContext,
                                         ClientsLogfileCreator,
                                         DNSClient

                                     );

            OCHPConfigurator?.Invoke(newRoamingProvider);

            return RoamingNetwork.
                       CreateCSORoamingProvider(newRoamingProvider,
                                                Configurator) as WWCPCSOAdapter;

        }

    }

}
