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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    // OCHP

    #region On(Set|Update)ChargePointList

    /// <summary>
    /// A delegate called whenever a charge point list will be send upstream.
    /// </summary>
    public delegate Task OnSetChargePointListRequestDelegate    (DateTime                        LogTimestamp,
                                                                 DateTime                        RequestTimestamp,
                                                                 CPOClient                       Sender,
                                                                 String                          SenderId,
                                                                 EventTracking_Id                EventTrackingId,
                                                                 UInt64                          NumberOfChargePoints,
                                                                 IEnumerable<ChargePointInfo>    ChargePointInfos,
                                                                 TimeSpan?                       RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response on a set charge point list was received.
    /// </summary>
    public delegate Task OnSetChargePointListResponseDelegate   (DateTime                        LogTimestamp,
                                                                 DateTime                        RequestTimestamp,
                                                                 CPOClient                       Sender,
                                                                 String                          SenderId,
                                                                 EventTracking_Id                EventTrackingId,
                                                                 UInt64                          NumberOfChargePoints,
                                                                 IEnumerable<ChargePointInfo>    ChargePointInfos,
                                                                 TimeSpan?                       RequestTimeout,
                                                                 SetChargePointListResponse      Result,
                                                                 TimeSpan                        Runtime);


    /// <summary>
    /// A delegate called whenever a charge point list update will be send upstream.
    /// </summary>
    public delegate Task OnUpdateChargePointListRequestDelegate (DateTime                        LogTimestamp,
                                                                 DateTime                        RequestTimestamp,
                                                                 CPOClient                       Sender,
                                                                 String                          SenderId,
                                                                 EventTracking_Id                EventTrackingId,
                                                                 UInt64                          NumberOfChargePoints,
                                                                 IEnumerable<ChargePointInfo>    ChargePointInfos,
                                                                 TimeSpan?                       RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response on an update charge point list was received.
    /// </summary>
    public delegate Task OnUpdateChargePointListResponseDelegate(DateTime                        LogTimestamp,
                                                                 DateTime                        RequestTimestamp,
                                                                 CPOClient                       Sender,
                                                                 String                          SenderId,
                                                                 EventTracking_Id                EventTrackingId,
                                                                 UInt64                          NumberOfChargePoints,
                                                                 IEnumerable<ChargePointInfo>    ChargePointInfos,
                                                                 TimeSpan?                       RequestTimeout,
                                                                 UpdateChargePointListResponse   Result,
                                                                 TimeSpan                        Runtime);

    #endregion

    #region OnUpdateStatus

    /// <summary>
    /// A delegate called whenever evse and parking status will be send upstream.
    /// </summary>
    public delegate Task OnUpdateStatusRequestDelegate (DateTime                     LogTimestamp,
                                                        DateTime                     RequestTimestamp,
                                                        CPOClient                    Sender,
                                                        String                       SenderId,
                                                        EventTracking_Id             EventTrackingId,
                                                        UInt64                       NumberOfEVSEStatus,
                                                        IEnumerable<EVSEStatus>      EVSEStatus,
                                                        UInt64                       NumberOfParkingStatus,
                                                        IEnumerable<ParkingStatus>   ParkingStatus,
                                                        DateTime?                    DefaultTTL,
                                                        TimeSpan?                    RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending evse and parking status upstream had been received.
    /// </summary>
    public delegate Task OnUpdateStatusResponseDelegate(DateTime                     LogTimestamp,
                                                        DateTime                     RequestTimestamp,
                                                        CPOClient                    Sender,
                                                        String                       SenderId,
                                                        EventTracking_Id             EventTrackingId,
                                                        UInt64                       NumberOfEVSEStatus,
                                                        IEnumerable<EVSEStatus>      EVSEStatus,
                                                        UInt64                       NumberOfParkingStatus,
                                                        IEnumerable<ParkingStatus>   ParkingStatus,
                                                        DateTime?                    DefaultTTL,
                                                        TimeSpan?                    RequestTimeout,
                                                        UpdateStatusResponse         Result,
                                                        TimeSpan                     Runtime);

    #endregion


    #region OnGetSingleRoamingAuthorisation

    /// <summary>
    /// A delegate called whenever an e-mobility token authentication will be send upstream.
    /// </summary>
    public delegate Task OnGetSingleRoamingAuthorisationRequestDelegate (DateTime                                LogTimestamp,
                                                                         DateTime                                RequestTimestamp,
                                                                         CPOClient                               Sender,
                                                                         String                                  SenderId,
                                                                         EventTracking_Id                        EventTrackingId,
                                                                         EMT_Id                                  EMTId,
                                                                         TimeSpan?                               RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response on an e-mobility token authentication was received.
    /// </summary>
    public delegate Task OnGetSingleRoamingAuthorisationResponseDelegate(DateTime                                LogTimestamp,
                                                                         DateTime                                RequestTimestamp,
                                                                         CPOClient                               Sender,
                                                                         String                                  SenderId,
                                                                         EventTracking_Id                        EventTrackingId,
                                                                         EMT_Id                                  EMTId,
                                                                         TimeSpan?                               RequestTimeout,
                                                                         GetSingleRoamingAuthorisationResponse   Result,
                                                                         TimeSpan                                Runtime);

    #endregion

    #region OnGetRoamingAuthorisationList(Updates)

    /// <summary>
    /// A delegate called whenever a request for the current roaming authorisation list will be send upstream.
    /// </summary>
    public delegate Task OnGetRoamingAuthorisationListRequestDelegate (DateTime                              LogTimestamp,
                                                                       DateTime                              RequestTimestamp,
                                                                       CPOClient                             Sender,
                                                                       String                                SenderId,
                                                                       EventTracking_Id                      EventTrackingId,
                                                                       TimeSpan?                             RequestTimeout);

    /// <summary>
    /// A delegate called whenever the current roaming authorisation list had been received.
    /// </summary>
    public delegate Task OnGetRoamingAuthorisationListResponseDelegate(DateTime                              LogTimestamp,
                                                                       DateTime                              RequestTimestamp,
                                                                       CPOClient                             Sender,
                                                                       String                                SenderId,
                                                                       EventTracking_Id                      EventTrackingId,
                                                                       TimeSpan?                             RequestTimeout,
                                                                       GetRoamingAuthorisationListResponse   Result,
                                                                       TimeSpan                              Runtime);


    /// <summary>
    /// A delegate called whenever a request for updates for the roaming authorisation list will be send upstream.
    /// </summary>
    public delegate Task OnGetRoamingAuthorisationListUpdatesRequestDelegate (DateTime                                    LogTimestamp,
                                                                              DateTime                                    RequestTimestamp,
                                                                              CPOClient                                   Sender,
                                                                              String                                      SenderId,
                                                                              EventTracking_Id                            EventTrackingId,
                                                                              DateTime                                    LastUpdate,
                                                                              TimeSpan?                                   RequestTimeout);

    /// <summary>
    /// A delegate called whenever updates of the roaming authorisation list had been received.
    /// </summary>
    public delegate Task OnGetRoamingAuthorisationListUpdatesResponseDelegate(DateTime                                    LogTimestamp,
                                                                              DateTime                                    RequestTimestamp,
                                                                              CPOClient                                   Sender,
                                                                              String                                      SenderId,
                                                                              EventTracking_Id                            EventTrackingId,
                                                                              DateTime                                    LastUpdate,
                                                                              TimeSpan?                                   RequestTimeout,
                                                                              GetRoamingAuthorisationListUpdatesResponse  Result,
                                                                              TimeSpan                                    Runtime);

    #endregion


    #region On(Add|Check)CDRs

    /// <summary>
    /// A delegate called whenever charge detail records will be send upstream.
    /// </summary>
    public delegate Task OnAddCDRsRequestDelegate   (DateTime               LogTimestamp,
                                                     DateTime               RequestTimestamp,
                                                     CPOClient              Sender,
                                                     String                 SenderId,
                                                     EventTracking_Id       EventTrackingId,
                                                     IEnumerable<CDRInfo>   CDRInfos,
                                                     TimeSpan?              RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending charge detail records upstream had been received.
    /// </summary>
    public delegate Task OnAddCDRsResponseDelegate  (DateTime               LogTimestamp,
                                                     DateTime               RequestTimestamp,
                                                     CPOClient              Sender,
                                                     String                 SenderId,
                                                     EventTracking_Id       EventTrackingId,
                                                     IEnumerable<CDRInfo>   CDRInfos,
                                                     TimeSpan?              RequestTimeout,
                                                     AddCDRsResponse        Result,
                                                     TimeSpan               Runtime);



    /// <summary>
    /// A delegate called whenever a check charge detail records request will be send upstream.
    /// </summary>
    public delegate Task OnCheckCDRsRequestDelegate (DateTime               LogTimestamp,
                                                     DateTime               RequestTimestamp,
                                                     CPOClient              Sender,
                                                     String                 SenderId,
                                                     EventTracking_Id       EventTrackingId,
                                                     CDRStatus?             CDRStatus,
                                                     TimeSpan?              RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a check charge detail records request upstream had been received.
    /// </summary>
    public delegate Task OnCheckCDRsResponseDelegate(DateTime               LogTimestamp,
                                                     DateTime               RequestTimestamp,
                                                     CPOClient              Sender,
                                                     String                 SenderId,
                                                     EventTracking_Id       EventTrackingId,
                                                     CDRStatus?             CDRStatus,
                                                     TimeSpan?              RequestTimeout,
                                                     CheckCDRsResponse      Result,
                                                     TimeSpan               Runtime);

    #endregion


    #region OnUpdateTariffs

    /// <summary>
    /// A delegate called whenever tariff infos will be send upstream.
    /// </summary>
    public delegate Task OnUpdateTariffsRequestDelegate (DateTime                     LogTimestamp,
                                                         DateTime                     RequestTimestamp,
                                                         CPOClient                    Sender,
                                                         String                       SenderId,
                                                         EventTracking_Id             EventTrackingId,
                                                         IEnumerable<TariffInfo>      TariffInfos,
                                                         TimeSpan?                    RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending tariff infos upstream had been received.
    /// </summary>
    public delegate Task OnUpdateTariffsResponseDelegate(DateTime                     LogTimestamp,
                                                         DateTime                     RequestTimestamp,
                                                         CPOClient                    Sender,
                                                         String                       SenderId,
                                                         EventTracking_Id             EventTrackingId,
                                                         IEnumerable<TariffInfo>      TariffInfos,
                                                         TimeSpan?                    RequestTimeout,
                                                         UpdateTariffsResponse        Result,
                                                         TimeSpan                     Runtime);

    #endregion


    // OCHPdirect

    #region OnAddServiceEndpoints

    /// <summary>
    /// A delegate called whenever an add service endpoints request will be send upstream.
    /// </summary>
    public delegate Task OnAddServiceEndpointsRequestDelegate (DateTime                        LogTimestamp,
                                                               DateTime                        RequestTimestamp,
                                                               IOCHPClient               Sender,
                                                               String                          SenderId,
                                                               EventTracking_Id                EventTrackingId,
                                                               IEnumerable<OperatorEndpoint>   OperatorEndpoints,
                                                               TimeSpan?                       RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending an add service endpoints request upstream had been received.
    /// </summary>
    public delegate Task OnAddServiceEndpointsResponseDelegate(DateTime                        LogTimestamp,
                                                               DateTime                        RequestTimestamp,
                                                               IOCHPClient               Sender,
                                                               String                          SenderId,
                                                               EventTracking_Id                EventTrackingId,
                                                               IEnumerable<OperatorEndpoint>   OperatorEndpoints,
                                                               TimeSpan?                       RequestTimeout,
                                                               AddServiceEndpointsResponse     Result,
                                                               TimeSpan                        Runtime);

    #endregion

    #region OnGetServiceEndpoints

    /// <summary>
    /// A delegate called whenever a get service endpoints request will be send upstream.
    /// </summary>
    public delegate Task OnGetServiceEndpointsRequestDelegate (DateTime                      LogTimestamp,
                                                               DateTime                      RequestTimestamp,
                                                               IOCHPClient             Sender,
                                                               String                        SenderId,
                                                               EventTracking_Id              EventTrackingId,
                                                               TimeSpan?                     RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a get service endpoints request upstream had been received.
    /// </summary>
    public delegate Task OnGetServiceEndpointsResponseDelegate(DateTime                      LogTimestamp,
                                                               DateTime                      RequestTimestamp,
                                                               IOCHPClient             Sender,
                                                               String                        SenderId,
                                                               EventTracking_Id              EventTrackingId,
                                                               TimeSpan?                     RequestTimeout,
                                                               GetServiceEndpointsResponse   Result,
                                                               TimeSpan                      Runtime);

    #endregion


    #region OnInformProvider

    /// <summary>
    /// A delegate called whenever an inform provider message will be send to an e-mobility provider.
    /// </summary>
    public delegate Task OnInformProviderRequestDelegate (DateTime                 LogTimestamp,
                                                          DateTime                 RequestTimestamp,
                                                          CPOClient                Sender,
                                                          String                   SenderId,
                                                          EventTracking_Id         EventTrackingId,

                                                          DirectMessages           DirectMessage,
                                                          EVSE_Id                  EVSEId,
                                                          Contract_Id              ContractId,
                                                          Direct_Id                DirectId,

                                                          DateTime?                SessionTimeoutAt,
                                                          Single?                  StateOfCharge,
                                                          Single?                  MaxPower,
                                                          Single?                  MaxCurrent,
                                                          Boolean?                 OnePhase,
                                                          Single?                  MaxEnergy,
                                                          Single?                  MinEnergy,
                                                          DateTime?                Departure,
                                                          Single?                  CurrentPower,
                                                          Single?                  ChargedEnergy,
                                                          Timestamped<Single>?     MeterReading,
                                                          IEnumerable<CDRPeriod>   ChargingPeriods,
                                                          Single?                  CurrentCost,
                                                          Currency                 Currency,

                                                          TimeSpan?                RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending an inform provider message to an e-mobility provider had been received.
    /// </summary>
    public delegate Task OnInformProviderResponseDelegate(DateTime                 LogTimestamp,
                                                          DateTime                 RequestTimestamp,
                                                          CPOClient                Sender,
                                                          String                   SenderId,
                                                          EventTracking_Id         EventTrackingId,

                                                          DirectMessages           DirectMessage,
                                                          EVSE_Id                  EVSEId,
                                                          Contract_Id              ContractId,
                                                          Direct_Id                DirectId,

                                                          DateTime?                SessionTimeoutAt,
                                                          Single?                  StateOfCharge,
                                                          Single?                  MaxPower,
                                                          Single?                  MaxCurrent,
                                                          Boolean?                 OnePhase,
                                                          Single?                  MaxEnergy,
                                                          Single?                  MinEnergy,
                                                          DateTime?                Departure,
                                                          Single?                  CurrentPower,
                                                          Single?                  ChargedEnergy,
                                                          Timestamped<Single>?     MeterReading,
                                                          IEnumerable<CDRPeriod>   ChargingPeriods,
                                                          Single?                  CurrentCost,
                                                          Currency                 Currency,

                                                          TimeSpan?                RequestTimeout,
                                                          InformProviderResponse   Result,
                                                          TimeSpan                 Runtime);

    #endregion


}
