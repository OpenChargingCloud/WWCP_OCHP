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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// Extention methods for the EMP client interface.
    /// </summary>
    public static class IEMPClientExtentions
    {

        #region SetRoamingAuthorisationList      (RoamingAuthorisationInfos, ...)

        /// <summary>
        /// Create an OCHP SetRoamingAuthorisationList XML/SOAP request.
        /// </summary>
        /// <param name="RoamingAuthorisationInfos">An enumeration of roaming authorisation infos.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public static Task<HTTPResponse<SetRoamingAuthorisationListResponse>>

            SetRoamingAuthorisationList(this IEMPClient                        IEMPClient,
                                        IEnumerable<RoamingAuthorisationInfo>  RoamingAuthorisationInfos,

                                        DateTime?                              Timestamp          = null,
                                        CancellationToken?                     CancellationToken  = null,
                                        EventTracking_Id                       EventTrackingId    = null,
                                        TimeSpan?                              RequestTimeout     = null)


                => IEMPClient.SetRoamingAuthorisationList(new SetRoamingAuthorisationListRequest(RoamingAuthorisationInfos,

                                                                                                 Timestamp,
                                                                                                 CancellationToken,
                                                                                                 EventTrackingId,
                                                                                                 RequestTimeout ?? IEMPClient.RequestTimeout));

        #endregion



    }

}
