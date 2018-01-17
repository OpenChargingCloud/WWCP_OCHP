/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    /// <summary>
    /// A delegate which allows you to customize the mapping between WWCP EVSE identifications
    /// and OCHP EVSE identifications.
    /// </summary>
    /// <param name="EVSEId">A WWCP EVSE identification.</param>
    public delegate EVSE_Id          CustomEVSEIdMapperDelegate         (WWCP.EVSE_Id      EVSEId);


    /// <summary>
    /// A delegate which allows you to modify OCHP charge point infos before sending them upstream.
    /// </summary>
    /// <param name="EVSE">A WWCP EVSE.</param>
    /// <param name="ChargePointInfo">An OCHP charge point info.</param>
    public delegate ChargePointInfo  EVSE2ChargePointInfoDelegate       (EVSE              EVSE,
                                                                         ChargePointInfo   ChargePointInfo);


    /// <summary>
    /// A delegate which allows you to modify OCHP EVSE status before sending them upstream.
    /// </summary>
    /// <param name="EVSEStatusUpdate">A WWCP EVSE status update.</param>
    /// <param name="EVSEStatus">An OCHP EVSE status.</param>
    public delegate EVSEStatus       EVSEStatusUpdate2EVSEStatusDelegate(EVSEStatusUpdate  EVSEStatusUpdate,
                                                                         EVSEStatus        EVSEStatus);



    /// <summary>
    /// A delegate called whenever new charge point infos will be send upstream.
    /// </summary>
    public delegate void OnSetChargePointInfosWWCPRequestDelegate (DateTime                         LogTimestamp,
                                                                   DateTime                         RequestTimestamp,
                                                                   Object                           Sender,
                                                                   CSORoamingProvider_Id            SenderId,
                                                                   EventTracking_Id                 EventTrackingId,
                                                                   RoamingNetwork_Id                RoamingNetworkId,
                                                                   UInt64                           NumberOfChargePointInfos,
                                                                   IEnumerable<ChargePointInfo>     ChargePointInfos,
                                                                   IEnumerable<Warning>             Warnings,
                                                                   TimeSpan?                        RequestTimeout);


    /// <summary>
    /// A delegate called whenever new charge point infos had been send upstream.
    /// </summary>
    public delegate void OnSetChargePointInfosWWCPResponseDelegate(DateTime                         LogTimestamp,
                                                                   DateTime                         RequestTimestamp,
                                                                   Object                           Sender,
                                                                   CSORoamingProvider_Id            SenderId,
                                                                   EventTracking_Id                 EventTrackingId,
                                                                   RoamingNetwork_Id                RoamingNetworkId,
                                                                   UInt64                           NumberOfChargePointInfos,
                                                                   IEnumerable<ChargePointInfo>     ChargePointInfos,
                                                                   TimeSpan?                        RequestTimeout,
                                                                   PushEVSEDataResult               Result,
                                                                   TimeSpan                         Runtime);


    /// <summary>
    /// A delegate called whenever charge point info updates will be send upstream.
    /// </summary>
    public delegate void OnUpdateChargePointInfosWWCPRequestDelegate (DateTime                         LogTimestamp,
                                                                      DateTime                         RequestTimestamp,
                                                                      Object                           Sender,
                                                                      CSORoamingProvider_Id            SenderId,
                                                                      EventTracking_Id                 EventTrackingId,
                                                                      RoamingNetwork_Id                RoamingNetworkId,
                                                                      UInt64                           NumberOfChargePointInfos,
                                                                      IEnumerable<ChargePointInfo>     ChargePointInfos,
                                                                      IEnumerable<Warning>             Warnings,
                                                                      TimeSpan?                        RequestTimeout);


    /// <summary>
    /// A delegate called whenever charge point info updates had been send upstream.
    /// </summary>
    public delegate void OnUpdateChargePointInfosWWCPResponseDelegate(DateTime                         LogTimestamp,
                                                                      DateTime                         RequestTimestamp,
                                                                      Object                           Sender,
                                                                      CSORoamingProvider_Id            SenderId,
                                                                      EventTracking_Id                 EventTrackingId,
                                                                      RoamingNetwork_Id                RoamingNetworkId,
                                                                      UInt64                           NumberOfChargePointInfos,
                                                                      IEnumerable<ChargePointInfo>     ChargePointInfos,
                                                                      TimeSpan?                        RequestTimeout,
                                                                      PushEVSEDataResult               Result,
                                                                      TimeSpan                         Runtime);


    /// <summary>
    /// A delegate called whenever EVSE status updates will be send upstream.
    /// </summary>
    public delegate void OnUpdateEVSEStatusWWCPRequestDelegate (DateTime                         LogTimestamp,
                                                                DateTime                         RequestTimestamp,
                                                                Object                           Sender,
                                                                CSORoamingProvider_Id            SenderId,
                                                                EventTracking_Id                 EventTrackingId,
                                                                RoamingNetwork_Id                RoamingNetworkId,
                                                                UInt64                           NumberOfChargePointInfos,
                                                                IEnumerable<EVSEStatus>          ChargePointInfos,
                                                                IEnumerable<Warning>             Warnings,
                                                                TimeSpan?                        RequestTimeout);


    /// <summary>
    /// A delegate called whenever EVSE status updates had been send upstream.
    /// </summary>
    public delegate void OnUpdateEVSEStatusWWCPResponseDelegate(DateTime                         LogTimestamp,
                                                                DateTime                         RequestTimestamp,
                                                                Object                           Sender,
                                                                CSORoamingProvider_Id            SenderId,
                                                                EventTracking_Id                 EventTrackingId,
                                                                RoamingNetwork_Id                RoamingNetworkId,
                                                                UInt64                           NumberOfChargePointInfos,
                                                                IEnumerable<EVSEStatus>          ChargePointInfos,
                                                                TimeSpan?                        RequestTimeout,
                                                                PushEVSEStatusResult             Result,
                                                                TimeSpan                         Runtime);

}
