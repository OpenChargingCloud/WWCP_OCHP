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
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    // OCHP

    /// <summary>
    /// A delegate for filtering charge points.
    /// </summary>
    /// <param name="ChargePointInfo">A charge point.</param>
    public delegate Boolean IncludeChargePointsDelegate(ChargePointInfo  ChargePointInfo);


    #region On(Set|Update)ChargePointList

    /// <summary>
    /// A delegate called whenever a charge point list will be send upstream.
    /// </summary>
    public delegate Task OnSetChargePointListRequestDelegate    (DateTime                        LogTimestamp,
                                                                 DateTime                        RequestTimestamp,
                                                                 CPOClient                       Sender,
                                                                 String                          SenderId,
                                                                 EventTracking_Id                EventTrackingId,
                                                                 IEnumerable<ChargePointInfo>    ChargePointInfos,
                                                                 UInt32                          NumberOfChargePoints,
                                                                 TimeSpan?                       RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response on a set charge point list was received.
    /// </summary>
    public delegate Task OnSetChargePointListResponseDelegate   (DateTime                        LogTimestamp,
                                                                 DateTime                        RequestTimestamp,
                                                                 CPOClient                       Sender,
                                                                 String                          SenderId,
                                                                 EventTracking_Id                EventTrackingId,
                                                                 IEnumerable<ChargePointInfo>    ChargePointInfos,
                                                                 UInt32                          NumberOfChargePoints,
                                                                 TimeSpan?                       RequestTimeout,
                                                                 SetChargePointListResponse      Result,
                                                                 TimeSpan                        Duration);


    /// <summary>
    /// A delegate called whenever a charge point list update will be send upstream.
    /// </summary>
    public delegate Task OnUpdateChargePointListRequestDelegate (DateTime                        LogTimestamp,
                                                                 DateTime                        RequestTimestamp,
                                                                 CPOClient                       Sender,
                                                                 String                          SenderId,
                                                                 EventTracking_Id                EventTrackingId,
                                                                 IEnumerable<ChargePointInfo>    ChargePointInfos,
                                                                 UInt32                          NumberOfChargePoints,
                                                                 TimeSpan?                       RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response on an update charge point list was received.
    /// </summary>
    public delegate Task OnUpdateChargePointListResponseDelegate(DateTime                        LogTimestamp,
                                                                 DateTime                        RequestTimestamp,
                                                                 CPOClient                       Sender,
                                                                 String                          SenderId,
                                                                 EventTracking_Id                EventTrackingId,
                                                                 IEnumerable<ChargePointInfo>    ChargePointInfos,
                                                                 UInt32                          NumberOfChargePoints,
                                                                 TimeSpan?                       RequestTimeout,
                                                                 UpdateChargePointListResponse   Result,
                                                                 TimeSpan                        Duration);

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
                                                        IEnumerable<EVSEStatus>      EVSEStatus,
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
                                                        IEnumerable<EVSEStatus>      EVSEStatus,
                                                        IEnumerable<ParkingStatus>   ParkingStatus,
                                                        DateTime?                    DefaultTTL,
                                                        TimeSpan?                    RequestTimeout,
                                                        UpdateStatusResponse         Result,
                                                        TimeSpan                     Duration);

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
                                                                         TimeSpan                                Duration);

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
                                                                       TimeSpan                              Duration);


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
                                                                              TimeSpan                                    Duration);

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
                                                     TimeSpan               Duration);



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
                                                     TimeSpan               Duration);

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
                                                         TimeSpan                     Duration);

    #endregion


    // OCHPdirect

    #region OnAddServiceEndpoints

    /// <summary>
    /// A delegate called whenever an add service endpoints request will be send upstream.
    /// </summary>
    public delegate Task OnAddServiceEndpointsRequestDelegate (DateTime                        LogTimestamp,
                                                               DateTime                        RequestTimestamp,
                                                               CPOClient                       Sender,
                                                               String                          SenderId,
                                                               EventTracking_Id                EventTrackingId,
                                                               IEnumerable<OperatorEndpoint>   OperatorEndpoints,
                                                               TimeSpan?                       RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending an add service endpoints request upstream had been received.
    /// </summary>
    public delegate Task OnAddServiceEndpointsResponseDelegate(DateTime                        LogTimestamp,
                                                               DateTime                        RequestTimestamp,
                                                               CPOClient                       Sender,
                                                               String                          SenderId,
                                                               EventTracking_Id                EventTrackingId,
                                                               IEnumerable<OperatorEndpoint>   OperatorEndpoints,
                                                               TimeSpan?                       RequestTimeout,
                                                               AddServiceEndpointsResponse     Result,
                                                               TimeSpan                        Duration);

    #endregion

    #region OnGetServiceEndpoints

    /// <summary>
    /// A delegate called whenever a get service endpoints request will be send upstream.
    /// </summary>
    public delegate Task OnGetServiceEndpointsRequestDelegate (DateTime                      LogTimestamp,
                                                               DateTime                      RequestTimestamp,
                                                               CPOClient                     Sender,
                                                               String                        SenderId,
                                                               EventTracking_Id              EventTrackingId,
                                                               TimeSpan?                     RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a get service endpoints request upstream had been received.
    /// </summary>
    public delegate Task OnGetServiceEndpointsResponseDelegate(DateTime                      LogTimestamp,
                                                               DateTime                      RequestTimestamp,
                                                               CPOClient                     Sender,
                                                               String                        SenderId,
                                                               EventTracking_Id              EventTrackingId,
                                                               TimeSpan?                     RequestTimeout,
                                                               GetServiceEndpointsResponse   Result,
                                                               TimeSpan                      Duration);

    #endregion


}
