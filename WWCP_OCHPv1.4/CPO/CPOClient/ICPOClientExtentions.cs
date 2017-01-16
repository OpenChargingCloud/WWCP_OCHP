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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    /// <summary>
    /// Extention methods for the OCHP CPO client interface.
    /// </summary>
    public static class ICPOClientExtentions
    {


        #region UpdateStatus(EVSEStatus = null, ParkingStatus = null, DefaultTTL = null, IncludeEVSEIds = null, ...)

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
        public static async Task<HTTPResponse<UpdateStatusResponse>>

            UpdateStatus(this ICPOClient             ICPOClient,
                         IEnumerable<EVSEStatus>     EVSEStatus         = null,
                         IEnumerable<ParkingStatus>  ParkingStatus      = null,
                         DateTime?                   DefaultTTL         = null,
                         IncludeEVSEIdsDelegate      IncludeEVSEIds     = null,

                         DateTime?                   Timestamp          = null,
                         CancellationToken?          CancellationToken  = null,
                         EventTracking_Id            EventTrackingId    = null,
                         TimeSpan?                   RequestTimeout     = null)


                => await ICPOClient.UpdateStatus(
                             new UpdateStatusRequest(
                                 EVSEStatus.   Where(evsestatus => IncludeEVSEIds != null ? IncludeEVSEIds(evsestatus.EVSEId) : true),
                                 ParkingStatus,
                                 DefaultTTL,

                                 Timestamp,
                                 CancellationToken,
                                 EventTrackingId,
                                 RequestTimeout.HasValue ? RequestTimeout.Value : ICPOClient.RequestTimeout
                             )
                         );

        #endregion

        #region GetSingleRoamingAuthorisation(EMTId, ...)

        /// <summary>
        /// Authenticate the given e-mobility token.
        /// </summary>
        /// <param name="EMTId">An e-mobility token.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static async Task<HTTPResponse<GetSingleRoamingAuthorisationResponse>>

            GetSingleRoamingAuthorisation(this ICPOClient     ICPOClient,
                                          EMT_Id              EMTId,

                                          DateTime?           Timestamp          = null,
                                          CancellationToken?  CancellationToken  = null,
                                          EventTracking_Id    EventTrackingId    = null,
                                          TimeSpan?           RequestTimeout     = null)


                => await ICPOClient.GetSingleRoamingAuthorisation(
                             new GetSingleRoamingAuthorisationRequest(
                                 EMTId,

                                 Timestamp,
                                 CancellationToken,
                                 EventTrackingId,
                                 RequestTimeout.HasValue ? RequestTimeout.Value : ICPOClient.RequestTimeout
                             )
                         );

        #endregion


        #region AddCDRs(CDRInfos, ...)

        /// <summary>
        /// Upload the given enumeration of charge detail records.
        /// </summary>
        /// <param name="CDRInfos">An enumeration of charge detail records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static async Task<HTTPResponse<AddCDRsResponse>>

            AddCDRs(this ICPOClient       ICPOClient,
                    IEnumerable<CDRInfo>  CDRInfos,

                    DateTime?             Timestamp          = null,
                    CancellationToken?    CancellationToken  = null,
                    EventTracking_Id      EventTrackingId    = null,
                    TimeSpan?             RequestTimeout     = null)

                => await ICPOClient.AddCDRs(
                             new AddCDRsRequest(
                                 CDRInfos,

                                 Timestamp,
                                 CancellationToken,
                                 EventTrackingId,
                                 RequestTimeout.HasValue ? RequestTimeout.Value : ICPOClient.RequestTimeout
                             )
                         );

        #endregion




    }

}
