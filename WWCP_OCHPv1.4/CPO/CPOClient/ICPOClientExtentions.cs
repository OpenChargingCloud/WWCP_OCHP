/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.CPO
{

    /// <summary>
    /// Extension methods for the OCHP CPO client interface.
    /// </summary>
    public static class ICPOClientExtensions
    {

        #region SetChargePointList   (ChargePointInfos, IncludeChargePoints = null, ...)

        /// <summary>
        /// Upload the given enumeration of charge points.
        /// </summary>
        /// <param name="ChargePointInfos">An enumeration of charge points.</param>
        /// <param name="IncludeChargePoints">An optional delegate for filtering charge points before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<SetChargePointListResponse>>

            SetChargePointList(this ICPOClient               ICPOClient,
                               IEnumerable<ChargePointInfo>  ChargePointInfos,
                               IncludeChargePointDelegate?   IncludeChargePoints   = null,

                               DateTime?                     Timestamp             = null,
                               EventTracking_Id?             EventTrackingId       = null,
                               TimeSpan?                     RequestTimeout        = null,
                               CancellationToken             CancellationToken     = default)


                => ICPOClient.SetChargePointList(
                       new SetChargePointListRequest(
                           IncludeChargePoints is null
                               ? ChargePointInfos
                               : ChargePointInfos.Where(cpo => IncludeChargePoints(cpo)),

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? ICPOClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion

        #region UpdateChargePointList(ChargePointInfos, IncludeChargePoints = null, ...)

        /// <summary>
        /// Update the given enumeration of charge points.
        /// </summary>
        /// <param name="ChargePointInfos">An enumeration of charge points.</param>
        /// <param name="IncludeChargePoints">An optional delegate for filtering charge points before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<UpdateChargePointListResponse>>

            UpdateChargePointList(this ICPOClient               ICPOClient,
                                  IEnumerable<ChargePointInfo>  ChargePointInfos,
                                  IncludeChargePointDelegate?   IncludeChargePoints   = null,

                                  DateTime?                     Timestamp             = null,
                                  EventTracking_Id?             EventTrackingId       = null,
                                  TimeSpan?                     RequestTimeout        = null,
                                  CancellationToken             CancellationToken     = default)


                => ICPOClient.UpdateChargePointList(
                       new UpdateChargePointListRequest(
                           IncludeChargePoints is null
                               ? ChargePointInfos
                               : ChargePointInfos.Where(cpo => IncludeChargePoints(cpo)),

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? ICPOClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion

        #region UpdateStatus         (EVSEStatus = null, ParkingStatus = null, DefaultTTL = null, IncludeEVSEIds = null, ...)

        /// <summary>
        /// Upload the given enumeration of EVSE and/or parking status.
        /// </summary>
        /// <param name="EVSEStatus">An optional enumeration of EVSE status.</param>
        /// <param name="ParkingStatus">An optional enumeration of parking status.</param>
        /// <param name="DefaultTTL">The default time to live for these status.</param>
        /// <param name="IncludeEVSEIds">An optional delegate for filtering EVSE status based on their EVSE identification before pushing them to the server.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<UpdateStatusResponse>>

            UpdateStatus(this ICPOClient              ICPOClient,
                         IEnumerable<EVSEStatus>?     EVSEStatus          = null,
                         IEnumerable<ParkingStatus>?  ParkingStatus       = null,
                         DateTime?                    DefaultTTL          = null,
                         IncludeEVSEIdsDelegate?      IncludeEVSEIds      = null,

                         DateTime?                    Timestamp           = null,
                         EventTracking_Id?            EventTrackingId     = null,
                         TimeSpan?                    RequestTimeout      = null,
                         CancellationToken            CancellationToken   = default)


                => ICPOClient.UpdateStatus(
                       new UpdateStatusRequest(
                           IncludeEVSEIds is null
                               ? EVSEStatus
                               : EVSEStatus?.Where(evseStatus => IncludeEVSEIds(evseStatus.EVSEId)),
                           ParkingStatus,
                           DefaultTTL,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? ICPOClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion

        #region UpdateTariffs        (TariffInfos, ...)

        /// <summary>
        /// Upload the given enumeration of tariff infos.
        /// </summary>
        /// <param name="TariffInfos">An enumeration of tariff infos.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<UpdateTariffsResponse>>

            UpdateTariffs(this ICPOClient          ICPOClient,
                          IEnumerable<TariffInfo>  TariffInfos,

                          DateTime?                Timestamp          = null,
                          EventTracking_Id?        EventTrackingId    = null,
                          TimeSpan?                RequestTimeout     = null,
                          CancellationToken        CancellationToken  = default)


                => ICPOClient.UpdateTariffs(
                       new UpdateTariffsRequest(
                           TariffInfos,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion


        #region GetSingleRoamingAuthorisation     (EMTId, ...)

        /// <summary>
        /// Authenticate the given e-mobility token.
        /// </summary>
        /// <param name="EMTId">An e-mobility token.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<GetSingleRoamingAuthorisationResponse>>

            GetSingleRoamingAuthorisation(this ICPOClient    ICPOClient,
                                          EMT_Id             EMTId,

                                          DateTime?          Timestamp           = null,
                                          EventTracking_Id?  EventTrackingId     = null,
                                          TimeSpan?          RequestTimeout      = null,
                                          CancellationToken  CancellationToken   = default)


                => ICPOClient.GetSingleRoamingAuthorisation(
                       new GetSingleRoamingAuthorisationRequest(
                           EMTId,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? ICPOClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion

        #region GetRoamingAuthorisationList       (...)

        /// <summary>
        /// Get the entire current version of the roaming authorisation list.
        /// </summary>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<GetRoamingAuthorisationListResponse>>

            GetRoamingAuthorisationList(this ICPOClient     ICPOClient,

                                        DateTime?           Timestamp           = null,
                                        EventTracking_Id?   EventTrackingId     = null,
                                        TimeSpan?           RequestTimeout      = null,
                                        CancellationToken   CancellationToken   = default)


                => ICPOClient.GetRoamingAuthorisationList(
                       new GetRoamingAuthorisationListRequest(
                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? ICPOClient.RequestTimeout,
                           CancellationToken
                       )
                   );

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
        public static Task<HTTPResponse<GetRoamingAuthorisationListUpdatesResponse>>

            GetRoamingAuthorisationListUpdates(this ICPOClient     ICPOClient,
                                               DateTime            LastUpdate,

                                               DateTime?           Timestamp           = null,
                                               EventTracking_Id?   EventTrackingId     = null,
                                               TimeSpan?           RequestTimeout      = null,
                                               CancellationToken   CancellationToken   = default)


                => ICPOClient.GetRoamingAuthorisationListUpdates(
                       new GetRoamingAuthorisationListUpdatesRequest(
                           LastUpdate,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? ICPOClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion


        #region AddCDR   (CDRInfo, ...)

        /// <summary>
        /// Upload the given charge detail record.
        /// </summary>
        /// <param name="CDRInfo">A charge detail record.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<AddCDRsResponse>>

            AddCDR(this ICPOClient     ICPOClient,
                   CDRInfo             CDRInfo,

                   DateTime?           Timestamp           = null,
                   EventTracking_Id?   EventTrackingId     = null,
                   TimeSpan?           RequestTimeout      = null,
                   CancellationToken   CancellationToken   = default)


                => ICPOClient.AddCDRs(
                       new AddCDRsRequest(
                           [ CDRInfo ],

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? ICPOClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion

        #region AddCDRs  (CDRInfos, ...)

        /// <summary>
        /// Upload the given enumeration of charge detail records.
        /// </summary>
        /// <param name="CDRInfos">An enumeration of charge detail records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<AddCDRsResponse>>

            AddCDRs(this ICPOClient       ICPOClient,
                    IEnumerable<CDRInfo>  CDRInfos,

                    DateTime?             Timestamp           = null,
                    EventTracking_Id?     EventTrackingId     = null,
                    TimeSpan?             RequestTimeout      = null,
                    CancellationToken     CancellationToken   = default)


                => ICPOClient.AddCDRs(
                       new AddCDRsRequest(
                           CDRInfos,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? ICPOClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion

        #region CheckCDRs(CDRStatus = null, ...)

        /// <summary>
        /// Check charge detail records having the given optional status.
        /// </summary>
        /// <param name="CDRStatus">The status of the requested charge detail records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<CheckCDRsResponse>>

            CheckCDRs(this ICPOClient    ICPOClient,
                      CDRStatus?         CDRStatus           = null,

                      DateTime?          Timestamp           = null,
                      EventTracking_Id?  EventTrackingId     = null,
                      TimeSpan?          RequestTimeout      = null,
                      CancellationToken  CancellationToken   = default)


                => ICPOClient.CheckCDRs(
                       new CheckCDRsRequest(
                           CDRStatus,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? ICPOClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion


        // OCHP direct

        #region AddServiceEndpoints(OperatorEndpoints, ...)

        /// <summary>
        /// Upload the given enumeration of OCHPdirect operator service endpoints.
        /// </summary>
        /// <param name="OperatorEndpoints">An enumeration of operator service endpoints.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<AddServiceEndpointsResponse>>

            AddServiceEndpoints(this ICPOClient                ICPOClient,
                                IEnumerable<OperatorEndpoint>  OperatorEndpoints,

                                DateTime?                      Timestamp           = null,
                                EventTracking_Id?              EventTrackingId     = null,
                                TimeSpan?                      RequestTimeout      = null,
                                CancellationToken              CancellationToken   = default)


                => ICPOClient.AddServiceEndpoints(
                       new AddServiceEndpointsRequest(
                           OperatorEndpoints,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout  ?? ICPOClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion

        #region GetServiceEndpoints(...)

        /// <summary>
        /// Download OCHPdirect provider endpoints.
        /// </summary>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<GetServiceEndpointsResponse>>

            GetServiceEndpoints(this ICPOClient     ICPOClient,
                                DateTime?           Timestamp           = null,
                                EventTracking_Id?   EventTrackingId     = null,
                                TimeSpan?           RequestTimeout      = null,
                                CancellationToken   CancellationToken   = default)


                => ICPOClient.GetServiceEndpoints(
                       new GetServiceEndpointsRequest(
                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? ICPOClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion


    }

}
