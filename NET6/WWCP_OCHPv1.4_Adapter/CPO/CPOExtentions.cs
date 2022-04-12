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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

using org.GraphDefined.WWCP.OCHPv1_4.CPO;

#endregion

namespace org.GraphDefined.WWCP
{

    /// <summary>
    /// Extensions methods for the WWCP wrapper for OCHP roaming clients for charging station operators.
    /// </summary>
    public static class EMPExtensions
    {

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
        public static WWCPEMPAdapter

            CreateOCHPv1_4_EMPRoamingProvider(this RoamingNetwork                                RoamingNetwork,
                                              EMPRoamingProvider_Id                              Id,
                                              I18NString                                         Name,
                                              I18NString                                         Description,
                                              CPORoaming                                         CPORoaming,

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

            if (RoamingNetwork == null)
                throw new ArgumentNullException(nameof(RoamingNetwork),  "The given roaming network must not be null!");

            if (Id == null)
                throw new ArgumentNullException(nameof(Id),              "The given unique roaming provider identification must not be null!");

            if (Name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Name),            "The given roaming provider name must not be null or empty!");

            if (CPORoaming is null)
                throw new ArgumentNullException(nameof(CPORoaming),      "The given CPO roaming must not be null!");

            #endregion

            var newRoamingProvider = new WWCPEMPAdapter(Id,
                                                        Name,
                                                        Description,
                                                        RoamingNetwork,
                                                        CPORoaming,

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

            OCHPConfigurator?.Invoke(newRoamingProvider);

            return RoamingNetwork.
                       CreateNewRoamingProvider(newRoamingProvider,
                                                Configurator) as WWCPEMPAdapter;

        }

    }

}
