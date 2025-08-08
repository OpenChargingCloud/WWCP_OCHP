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

using org.GraphDefined.Vanaheimr.Illias;

using cloud.charging.open.protocols.OCHPv1_4.CPO;
using cloud.charging.open.protocols.OCHPv1_4.EMP;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.CH
{

    // Shared event delegates...

    #region OnAddServiceEndpoints

    /// <summary>
    /// Add service endpoints.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="OperatorEndpoints">An enumeration of operator endpoints.</param>
    /// <param name="ProviderEndpoints">An enumeration of provider endpoints.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<AddServiceEndpointsResponse>

        OnAddServiceEndpointsDelegate(DateTimeOffset                 Timestamp,
                                      CHServer                       Sender,
                                      CancellationToken              CancellationToken,
                                      EventTracking_Id               EventTrackingId,

                                      IEnumerable<OperatorEndpoint>  OperatorEndpoints,
                                      IEnumerable<ProviderEndpoint>  ProviderEndpoints,

                                      TimeSpan?                      QueryTimeout = null);

    #endregion

    #region GetServiceEndpoints

    /// <summary>
    /// Get service endpoints.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<GetServiceEndpointsResponse>

        OnGetServiceEndpointsDelegate(DateTimeOffset      Timestamp,
                                      CHServer            Sender,
                                      CancellationToken   CancellationToken,
                                      EventTracking_Id    EventTrackingId,
                                      TimeSpan?           QueryTimeout  = null);

    #endregion


    // CPO event delegates...

    #region OnAddCDRs

    /// <summary>
    /// Add charge detail records.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="CDRInfos">An enumeration of charge detail records.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<AddCDRsResponse>

        OnAddCDRsDelegate(DateTimeOffset         Timestamp,
                          CHServer               Sender,
                          CancellationToken      CancellationToken,
                          EventTracking_Id       EventTrackingId,

                          IEnumerable<CDRInfo>   CDRInfos,

                          TimeSpan?              QueryTimeout = null);

    #endregion

    #region OnCheckCDRs

    /// <summary>
    /// Check charge detail records.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="CDRStatus">The status of the requested charge detail records.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<CheckCDRsResponse>

        OnCheckCDRsDelegate(DateTimeOffset      Timestamp,
                            CHServer            Sender,
                            CancellationToken   CancellationToken,
                            EventTracking_Id    EventTrackingId,

                            CDRStatus?          CDRStatus     = null,

                            TimeSpan?           QueryTimeout  = null);

    #endregion

    #region OnGetRoamingAuthorisationList

    /// <summary>
    /// Get the roaming authorisation list.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<GetRoamingAuthorisationListResponse>

        OnGetRoamingAuthorisationListDelegate(DateTimeOffset      Timestamp,
                                              CHServer            Sender,
                                              CancellationToken   CancellationToken,
                                              EventTracking_Id    EventTrackingId,
                                              TimeSpan?           QueryTimeout  = null);

    #endregion

    #region OnGetRoamingAuthorisationListUpdates

    /// <summary>
    /// Get updates of the roaming authorisation list for the given timestamp.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="LastUpdate">The timestamp of the last roaming authorisation list update.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<GetRoamingAuthorisationListUpdatesResponse>

        OnGetRoamingAuthorisationListUpdatesDelegate(DateTimeOffset      Timestamp,
                                                     CHServer            Sender,
                                                     CancellationToken   CancellationToken,
                                                     EventTracking_Id    EventTrackingId,

                                                     DateTime            LastUpdate,

                                                     TimeSpan?           QueryTimeout  = null);

    #endregion

    #region GetSingleRoamingAuthorisation

    /// <summary>
    /// Get single roaming authorisation.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="EMTId">An e-mobility token.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<GetSingleRoamingAuthorisationResponse>

        OnGetSingleRoamingAuthorisationDelegate(DateTimeOffset      Timestamp,
                                                CHServer            Sender,
                                                CancellationToken   CancellationToken,
                                                EventTracking_Id    EventTrackingId,

                                                EMT_Id              EMTId,

                                                TimeSpan?           QueryTimeout  = null);

    #endregion

    #region SetChargePointList

    /// <summary>
    /// Set the list of charge points.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="ChargePointInfos">An enumeration of charge point infos.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<SetChargePointListResponse>

        OnSetChargePointListDelegate(DateTimeOffset                 Timestamp,
                                     CHServer                       Sender,
                                     CancellationToken              CancellationToken,
                                     EventTracking_Id               EventTrackingId,

                                     IEnumerable<ChargePointInfo>   ChargePointInfos,

                                     TimeSpan?                      QueryTimeout  = null);

    #endregion

    #region UpdateChargePointList

    /// <summary>
    /// Update the list of charge points.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="ChargePointInfos">An enumeration of charge point infos.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<UpdateChargePointListResponse>

        OnUpdateChargePointListDelegate(DateTimeOffset                 Timestamp,
                                        CHServer                       Sender,
                                        CancellationToken              CancellationToken,
                                        EventTracking_Id               EventTrackingId,

                                        IEnumerable<ChargePointInfo>   ChargePointInfos,

                                        TimeSpan?                      QueryTimeout  = null);

    #endregion

    #region UpdateStatus

    /// <summary>
    /// Update EVSE and/or parking status.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="EVSEStatus">An enumeration of EVSE status.</param>
    /// <param name="ParkingStatus">An enumeration of parking status.</param>
    /// <param name="DefaultTTL">The default time to live for these status.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<UpdateStatusResponse>

        OnUpdateStatusDelegate(DateTimeOffset               Timestamp,
                               CHServer                     Sender,
                               CancellationToken            CancellationToken,
                               EventTracking_Id             EventTrackingId,

                               IEnumerable<EVSEStatus>      EVSEStatus,
                               IEnumerable<ParkingStatus>   ParkingStatus,
                               DateTimeOffset?              DefaultTTL,

                               TimeSpan?                    QueryTimeout  = null);

    #endregion

    #region UpdateTariffs

    /// <summary>
    /// Update the list of charging tariffs.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="TariffInfos">An enumeration of tariff infos.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<UpdateTariffsResponse>

        OnUpdateTariffsDelegate(DateTimeOffset            Timestamp,
                                CHServer                  Sender,
                                CancellationToken         CancellationToken,
                                EventTracking_Id          EventTrackingId,

                                IEnumerable<TariffInfo>   TariffInfos,

                                TimeSpan?                 QueryTimeout  = null);

    #endregion


    // EMP event delegates...

    #region OnGetCDRs

    /// <summary>
    /// Get charge detail records.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="CDRStatus">The optional status of the requested charge detail records.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<GetCDRsResponse>

        OnGetCDRsDelegate(DateTimeOffset      Timestamp,
                          CHServer            Sender,
                          CancellationToken   CancellationToken,
                          EventTracking_Id    EventTrackingId,

                          CDRStatus?          CDRStatus = null,

                          TimeSpan?           QueryTimeout  = null);

    #endregion

    #region OnConfirmCDRs

    /// <summary>
    /// Confirm charge detail records.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="Approved">An enumeration of approved charge detail records.</param>
    /// <param name="Declined">An enumeration of declined charge detail records.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<ConfirmCDRsResponse>

        OnConfirmCDRsDelegate(DateTimeOffset             Timestamp,
                              CHServer                   Sender,
                              CancellationToken          CancellationToken,
                              EventTracking_Id           EventTrackingId,

                              IEnumerable<EVSECDRPair>   Approved,
                              IEnumerable<EVSECDRPair>   Declined,

                              TimeSpan?                  QueryTimeout  = null);

    #endregion

    #region GetChargePointList

    /// <summary>
    /// Get the list of charge points.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<GetChargePointListResponse>

        OnGetChargePointListDelegate(DateTimeOffset      Timestamp,
                                     CHServer            Sender,
                                     CancellationToken   CancellationToken,
                                     EventTracking_Id    EventTrackingId,
                                     TimeSpan?           QueryTimeout  = null);

    #endregion

    #region OnGetChargePointListUpdates

    /// <summary>
    /// Get updates of the charge point list for the given timestamp.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="LastUpdate">The timestamp of the last charge point list update.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<GetChargePointListUpdatesResponse>

        OnGetChargePointListUpdatesDelegate(DateTimeOffset      Timestamp,
                                            CHServer            Sender,
                                            CancellationToken   CancellationToken,
                                            EventTracking_Id    EventTrackingId,

                                            DateTimeOffset      LastUpdate,

                                            TimeSpan?           QueryTimeout  = null);

    #endregion

    #region GetStatus

    /// <summary>
    /// Get EVSE and parking status.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="LastRequest">Only return status data newer than the given timestamp.</param>
    /// <param name="StatusType">A status type filter.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<GetStatusResponse>

        OnGetStatusDelegate(DateTimeOffset      Timestamp,
                            CHServer            Sender,
                            CancellationToken   CancellationToken,
                            EventTracking_Id    EventTrackingId,

                            DateTimeOffset?     LastRequest,
                            StatusTypes?        StatusType,

                            TimeSpan?           QueryTimeout  = null);

    #endregion

    #region OnGetTariffUpdates

    /// <summary>
    /// Get charging tariffs updates for the given timestamp.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="LastUpdate">An optional timestamp of the last charging tariffs update.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<GetTariffUpdatesResponse>

        OnGetTariffUpdatesDelegate(DateTimeOffset      Timestamp,
                                   CHServer            Sender,
                                   CancellationToken   CancellationToken,
                                   EventTracking_Id    EventTrackingId,

                                   DateTimeOffset?     LastUpdate,

                                   TimeSpan?           QueryTimeout  = null);

    #endregion

    #region SetRoamingAuthorisationList

    /// <summary>
    /// Set the list of roaming authorisation infos.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="RoamingAuthorisationInfos">An enumeration of roaming authorisation infos.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<SetRoamingAuthorisationListResponse>

        OnSetRoamingAuthorisationListDelegate(DateTimeOffset                          Timestamp,
                                              CHServer                                Sender,
                                              CancellationToken                       CancellationToken,
                                              EventTracking_Id                        EventTrackingId,

                                              IEnumerable<RoamingAuthorisationInfo>   RoamingAuthorisationInfos,

                                              TimeSpan?                               QueryTimeout  = null);

    #endregion

    #region UpdateRoamingAuthorisationList

    /// <summary>
    /// Update the list of roaming authorisation infos.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="RoamingAuthorisationInfos">An enumeration of roaming authorisation infos.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<UpdateRoamingAuthorisationListResponse>

        OnUpdateRoamingAuthorisationListDelegate(DateTimeOffset                          Timestamp,
                                                 CHServer                                Sender,
                                                 CancellationToken                       CancellationToken,
                                                 EventTracking_Id                        EventTrackingId,

                                                 IEnumerable<RoamingAuthorisationInfo>   RoamingAuthorisationInfos,

                                                 TimeSpan?                               QueryTimeout  = null);

    #endregion


}
