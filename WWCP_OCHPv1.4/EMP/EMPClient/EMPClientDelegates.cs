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
using org.GraphDefined.Vanaheimr.Aegir;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    #region OnGetChargePointList(Updates)

    /// <summary>
    /// A delegate called whenever a request for the list of charge points will be send upstream.
    /// </summary>
    public delegate Task OnGetChargePointListRequestDelegate        (DateTime                            LogTimestamp,
                                                                     DateTime                            RequestTimestamp,
                                                                     EMPClient                           Sender,
                                                                     String                              SenderId,
                                                                     EventTracking_Id                    EventTrackingId,
                                                                     TimeSpan?                           RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response on a request for the list of charge points was received.
    /// </summary>
    public delegate Task OnGetChargePointListResponseDelegate       (DateTime                            LogTimestamp,
                                                                     DateTime                            RequestTimestamp,
                                                                     EMPClient                           Sender,
                                                                     String                              SenderId,
                                                                     EventTracking_Id                    EventTrackingId,
                                                                     TimeSpan?                           RequestTimeout,
                                                                     GetChargePointListResponse          Result,
                                                                     TimeSpan                            Duration);


    /// <summary>
    /// A delegate called whenever a request for a list of charge point updates will be send upstream.
    /// </summary>
    public delegate Task OnGetChargePointListUpdatesRequestDelegate (DateTime                            LogTimestamp,
                                                                     DateTime                            RequestTimestamp,
                                                                     EMPClient                           Sender,
                                                                     String                              SenderId,
                                                                     EventTracking_Id                    EventTrackingId,
                                                                     DateTime                            LastUpdate,
                                                                     TimeSpan?                           RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response on a request for a list of charge point updates was received.
    /// </summary>
    public delegate Task OnGetChargePointListUpdatesResponseDelegate(DateTime                            LogTimestamp,
                                                                     DateTime                            RequestTimestamp,
                                                                     EMPClient                           Sender,
                                                                     String                              SenderId,
                                                                     EventTracking_Id                    EventTrackingId,
                                                                     DateTime                            LastUpdate,
                                                                     TimeSpan?                           RequestTimeout,
                                                                     GetChargePointListUpdatesResponse   Result,
                                                                     TimeSpan                            Duration);

    #endregion

    #region OnGetStatus

    /// <summary>
    /// A delegate called whenever a get status request will be send upstream.
    /// </summary>
    public delegate Task OnGetStatusRequestDelegate (DateTime               LogTimestamp,
                                                     DateTime               RequestTimestamp,
                                                     EMPClient              Sender,
                                                     String                 SenderId,
                                                     EventTracking_Id       EventTrackingId,
                                                     DateTime?              LastRequest,
                                                     StatusTypes?           StatusType,
                                                     TimeSpan?              RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a get status request upstream had been received.
    /// </summary>
    public delegate Task OnGetStatusResponseDelegate(DateTime               LogTimestamp,
                                                     DateTime               RequestTimestamp,
                                                     EMPClient              Sender,
                                                     String                 SenderId,
                                                     EventTracking_Id       EventTrackingId,
                                                     DateTime?              LastRequest,
                                                     StatusTypes?           StatusType,
                                                     TimeSpan?              RequestTimeout,
                                                     GetStatusResponse      Result,
                                                     TimeSpan               Duration);

    #endregion

    #region On(Set|Update)RoamingAuthorisationList

    /// <summary>
    /// A delegate called whenever a roaming authorisation list will be send upstream.
    /// </summary>
    public delegate Task OnSetRoamingAuthorisationListRequestDelegate    (DateTime                                 LogTimestamp,
                                                                          DateTime                                 RequestTimestamp,
                                                                          EMPClient                                Sender,
                                                                          String                                   SenderId,
                                                                          EventTracking_Id                         EventTrackingId,
                                                                          IEnumerable<RoamingAuthorisationInfo>    RoamingAuthorisationInfos,
                                                                          TimeSpan?                                RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response on sending a roaming authorisation list had been received.
    /// </summary>
    public delegate Task OnSetRoamingAuthorisationListResponseDelegate   (DateTime                                 LogTimestamp,
                                                                          DateTime                                 RequestTimestamp,
                                                                          EMPClient                                Sender,
                                                                          String                                   SenderId,
                                                                          EventTracking_Id                         EventTrackingId,
                                                                          IEnumerable<RoamingAuthorisationInfo>    RoamingAuthorisationInfos,
                                                                          TimeSpan?                                RequestTimeout,
                                                                          SetRoamingAuthorisationListResponse      Result,
                                                                          TimeSpan                                 Duration);


    /// <summary>
    /// A delegate called whenever an update for a roaming authorisation list will be send upstream.
    /// </summary>
    public delegate Task OnUpdateRoamingAuthorisationListRequestDelegate (DateTime                                 LogTimestamp,
                                                                          DateTime                                 RequestTimestamp,
                                                                          EMPClient                                Sender,
                                                                          String                                   SenderId,
                                                                          EventTracking_Id                         EventTrackingId,
                                                                          IEnumerable<RoamingAuthorisationInfo>    RoamingAuthorisationInfos,
                                                                          TimeSpan?                                RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response on a roaming authorisation list update had been received.
    /// </summary>
    public delegate Task OnUpdateRoamingAuthorisationListResponseDelegate(DateTime                                 LogTimestamp,
                                                                          DateTime                                 RequestTimestamp,
                                                                          EMPClient                                Sender,
                                                                          String                                   SenderId,
                                                                          EventTracking_Id                         EventTrackingId,
                                                                          IEnumerable<RoamingAuthorisationInfo>    RoamingAuthorisationInfos,
                                                                          TimeSpan?                                RequestTimeout,
                                                                          UpdateRoamingAuthorisationListResponse   Result,
                                                                          TimeSpan                                 Duration);

    #endregion

    #region OnGetCDRs

    /// <summary>
    /// A delegate called whenever a get charge detail records request will be send upstream.
    /// </summary>
    public delegate Task OnGetCDRsRequestDelegate (DateTime               LogTimestamp,
                                                   DateTime               RequestTimestamp,
                                                   EMPClient              Sender,
                                                   String                 SenderId,
                                                   EventTracking_Id       EventTrackingId,
                                                   CDRStatus?             CDRStatus,
                                                   TimeSpan?              RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a get charge detail records request upstream had been received.
    /// </summary>
    public delegate Task OnGetCDRsResponseDelegate(DateTime               LogTimestamp,
                                                   DateTime               RequestTimestamp,
                                                   EMPClient              Sender,
                                                   String                 SenderId,
                                                   EventTracking_Id       EventTrackingId,
                                                   CDRStatus?             CDRStatus,
                                                   TimeSpan?              RequestTimeout,
                                                   GetCDRsResponse        Result,
                                                   TimeSpan               Duration);

    #endregion

    #region OnGetTariffUpdates

    /// <summary>
    /// A delegate called whenever a get tariff updates request will be send upstream.
    /// </summary>
    public delegate Task OnGetTariffUpdatesRequestDelegate (DateTime                   LogTimestamp,
                                                            DateTime                   RequestTimestamp,
                                                            EMPClient                  Sender,
                                                            String                     SenderId,
                                                            EventTracking_Id           EventTrackingId,
                                                            DateTime?                  LastRequest,
                                                            TimeSpan?                  RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a get tariff updates request upstream had been received.
    /// </summary>
    public delegate Task OnGetTariffUpdatesResponseDelegate(DateTime                   LogTimestamp,
                                                            DateTime                   RequestTimestamp,
                                                            EMPClient                  Sender,
                                                            String                     SenderId,
                                                            EventTracking_Id           EventTrackingId,
                                                            DateTime?                  LastRequest,
                                                            TimeSpan?                  RequestTimeout,
                                                            GetTariffUpdatesResponse   Result,
                                                            TimeSpan                   Duration);

    #endregion

}
