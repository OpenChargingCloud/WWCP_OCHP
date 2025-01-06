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

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.EMP
{

    /// <summary>
    /// Extension methods for the EMP client interface.
    /// </summary>
    public static class IEMPClientExtensions
    {

        // OCHP

        #region GetChargePointList       ()

        /// <summary>
        /// Download the current charge point list.
        /// </summary>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<GetChargePointListResponse>>

            GetChargePointList(this IEMPClient     IEMPClient,

                               DateTime?           Timestamp           = null,
                               EventTracking_Id?   EventTrackingId     = null,
                               TimeSpan?           RequestTimeout      = null,
                               CancellationToken   CancellationToken   = default)

                => IEMPClient.GetChargePointList(
                       new GetChargePointListRequest(
                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? IEMPClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion

        #region GetChargePointListUpdates(LastUpdate, ...)

        /// <summary>
        /// Download all charge point list updates since the given date.
        /// </summary>
        /// <param name="LastUpdate">The timestamp of the last call to this method.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<GetChargePointListUpdatesResponse>>

            GetChargePointListUpdates(this IEMPClient     IEMPClient,
                                      DateTime            LastUpdate,

                                      DateTime?           Timestamp           = null,
                                      EventTracking_Id?   EventTrackingId     = null,
                                      TimeSpan?           RequestTimeout      = null,
                                      CancellationToken   CancellationToken   = default)

                => IEMPClient.GetChargePointListUpdates(
                       new GetChargePointListUpdatesRequest(
                           LastUpdate,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? IEMPClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion

        #region GetStatus                (LastRequest = null, StatusType = null)

        /// <summary>
        /// Download the current list of charge point status filtered by
        /// an optional last request timestamp or their status type.
        /// </summary>
        /// <param name="LastRequest">Only return status data newer than the given timestamp.</param>
        /// <param name="StatusType">A status type filter.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<GetStatusResponse>>

            GetStatus(this IEMPClient     IEMPClient,
                      DateTime?           LastRequest         = null,
                      StatusTypes?        StatusType          = null,

                      DateTime?           Timestamp           = null,
                      EventTracking_Id?   EventTrackingId     = null,
                      TimeSpan?           RequestTimeout      = null,
                      CancellationToken   CancellationToken   = default)


                => IEMPClient.GetStatus(
                       new GetStatusRequest(
                           LastRequest,
                           StatusType,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? IEMPClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion

        #region GetTariffUpdates         (LastUpdate  = null, ...)

        /// <summary>
        /// Download an update of the current tariff list since the given date.
        /// </summary>
        /// <param name="LastUpdate">The timestamp of the last call to this method.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<GetTariffUpdatesResponse>>

            GetTariffUpdates(this IEMPClient     IEMPClient,
                             DateTime?           LastUpdate          = null,

                             DateTime?           Timestamp           = null,
                             EventTracking_Id?   EventTrackingId     = null,
                             TimeSpan?           RequestTimeout      = null,
                             CancellationToken   CancellationToken   = default)

                => IEMPClient.GetTariffUpdates(
                       new GetTariffUpdatesRequest(
                           LastUpdate,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? IEMPClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion


        #region SetRoamingAuthorisationList   (RoamingAuthorisationInfo, ...)

        /// <summary>
        /// Create an OCHP SetRoamingAuthorisationList XML/SOAP request.
        /// </summary>
        /// <param name="RoamingAuthorisationInfo">A roaming authorisation info.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<SetRoamingAuthorisationListResponse>>

            SetRoamingAuthorisationList(this IEMPClient           IEMPClient,
                                        RoamingAuthorisationInfo  RoamingAuthorisationInfo,

                                        DateTime?                 Timestamp           = null,
                                        EventTracking_Id?         EventTrackingId     = null,
                                        TimeSpan?                 RequestTimeout      = null,
                                        CancellationToken         CancellationToken   = default)


                => IEMPClient.SetRoamingAuthorisationList(
                       new SetRoamingAuthorisationListRequest(
                           [ RoamingAuthorisationInfo ],

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? IEMPClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion

        #region SetRoamingAuthorisationList   (RoamingAuthorisationInfos, ...)

        /// <summary>
        /// Create an OCHP SetRoamingAuthorisationList XML/SOAP request.
        /// </summary>
        /// <param name="RoamingAuthorisationInfos">An enumeration of roaming authorisation infos.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<SetRoamingAuthorisationListResponse>>

            SetRoamingAuthorisationList(this IEMPClient                        IEMPClient,
                                        IEnumerable<RoamingAuthorisationInfo>  RoamingAuthorisationInfos,

                                        DateTime?                              Timestamp           = null,
                                        EventTracking_Id?                      EventTrackingId     = null,
                                        TimeSpan?                              RequestTimeout      = null,
                                        CancellationToken                      CancellationToken   = default)


                => IEMPClient.SetRoamingAuthorisationList(
                       new SetRoamingAuthorisationListRequest(
                           RoamingAuthorisationInfos,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? IEMPClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion

        #region UpdateRoamingAuthorisationList(RoamingAuthorisationInfo, ...)

        /// <summary>
        /// Create an OCHP UpdateRoamingAuthorisationList XML/SOAP request.
        /// </summary>
        /// <param name="RoamingAuthorisationInfo">A roaming authorisation info.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<UpdateRoamingAuthorisationListResponse>>

            UpdateRoamingAuthorisationList(this IEMPClient           IEMPClient,
                                           RoamingAuthorisationInfo  RoamingAuthorisationInfo,

                                           DateTime?                 Timestamp           = null,
                                           EventTracking_Id?         EventTrackingId     = null,
                                           TimeSpan?                 RequestTimeout      = null,
                                           CancellationToken         CancellationToken   = default)


                => IEMPClient.UpdateRoamingAuthorisationList(
                       new UpdateRoamingAuthorisationListRequest(
                           [ RoamingAuthorisationInfo ],

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? IEMPClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion

        #region UpdateRoamingAuthorisationList(RoamingAuthorisationInfos, ...)

        /// <summary>
        /// Create an OCHP UpdateRoamingAuthorisationList XML/SOAP request.
        /// </summary>
        /// <param name="RoamingAuthorisationInfos">An enumeration of roaming authorisation infos.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<UpdateRoamingAuthorisationListResponse>>

            UpdateRoamingAuthorisationList(this IEMPClient                        IEMPClient,
                                           IEnumerable<RoamingAuthorisationInfo>  RoamingAuthorisationInfos,

                                           DateTime?                              Timestamp           = null,
                                           EventTracking_Id?                      EventTrackingId     = null,
                                           TimeSpan?                              RequestTimeout      = null,
                                           CancellationToken                      CancellationToken   = default)


                => IEMPClient.UpdateRoamingAuthorisationList(
                       new UpdateRoamingAuthorisationListRequest(
                           RoamingAuthorisationInfos,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? IEMPClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion


        #region GetCDRsRequest    (CDRStatus = null, ...)

        /// <summary>
        /// Download charge detail records having the given optional status.
        /// </summary>
        /// <param name="CDRStatus">The status of the requested charge detail records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<GetCDRsResponse>>

            GetCDRs(this IEMPClient     IEMPClient,
                    CDRStatus?          CDRStatus           = null,

                    DateTime?           Timestamp           = null,
                    EventTracking_Id?   EventTrackingId     = null,
                    TimeSpan?           RequestTimeout      = null,
                    CancellationToken   CancellationToken   = default)


                => IEMPClient.GetCDRs(
                       new GetCDRsRequest(
                           CDRStatus,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? IEMPClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion

        #region ConfirmCDRsRequest(Approved = null, Declined = null, ...)

        /// <summary>
        /// Approve or decline charge detail records.
        /// </summary>
        /// <param name="Approved">An enumeration of approved charge detail records.</param>
        /// <param name="Declined">An enumeration of declined charge detail records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<ConfirmCDRsResponse>>

            ConfirmCDRs(this IEMPClient            IEMPClient,
                        IEnumerable<EVSECDRPair>?  Approved            = null,
                        IEnumerable<EVSECDRPair>?  Declined            = null,

                        DateTime?                  Timestamp           = null,
                        EventTracking_Id?          EventTrackingId     = null,
                        TimeSpan?                  RequestTimeout      = null,
                        CancellationToken          CancellationToken   = default)


                => IEMPClient.ConfirmCDRs(
                       new ConfirmCDRsRequest(
                           Approved,
                           Declined,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? IEMPClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion


        // OCHP direct

        #region AddServiceEndpoints(ProviderEndpoints, ...)

        /// <summary>
        /// Upload the given enumeration of OCHPdirect provider endpoints.
        /// </summary>
        /// <param name="ProviderEndpoints">An enumeration of provider endpoints.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<AddServiceEndpointsResponse>>

            AddServiceEndpoints(this IEMPClient                IEMPClient,
                                IEnumerable<ProviderEndpoint>  ProviderEndpoints,

                                DateTime?                      Timestamp           = null,
                                EventTracking_Id?              EventTrackingId     = null,
                                TimeSpan?                      RequestTimeout      = null,
                                CancellationToken              CancellationToken   = default)


                => IEMPClient.AddServiceEndpoints(
                       new AddServiceEndpointsRequest(
                           ProviderEndpoints,

                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? IEMPClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion

        #region GetServiceEndpoints(...)

        /// <summary>
        /// Download OCHPdirect provider endpoints.
        /// </summary>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<HTTPResponse<GetServiceEndpointsResponse>>

            GetServiceEndpoints(this IEMPClient     IEMPClient,
                                DateTime?           Timestamp           = null,
                                EventTracking_Id?   EventTrackingId     = null,
                                TimeSpan?           RequestTimeout      = null,
                                CancellationToken   CancellationToken   = default)


                => IEMPClient.GetServiceEndpoints(
                       new GetServiceEndpointsRequest(
                           Timestamp,
                           EventTrackingId,
                           RequestTimeout ?? IEMPClient.RequestTimeout,
                           CancellationToken
                       )
                   );

        #endregion


    }

}
