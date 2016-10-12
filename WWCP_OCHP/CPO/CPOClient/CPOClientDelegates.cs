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

namespace org.GraphDefined.WWCP.OCHPv1_4
{

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


}
