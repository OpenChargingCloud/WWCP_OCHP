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

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.EMP
{

    // OCHP

    #region OnGetChargePointList(Updates)

    /// <summary>
    /// A delegate called whenever a request for the list of charge points will be send upstream.
    /// </summary>
    public delegate Task OnGetChargePointListRequestDelegate        (DateTimeOffset                      LogTimestamp,
                                                                     DateTimeOffset                      RequestTimestamp,
                                                                     EMPClient                           Sender,
                                                                     //String                              SenderId,
                                                                     EventTracking_Id                    EventTrackingId,
                                                                     TimeSpan?                           RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response on a request for the list of charge points was received.
    /// </summary>
    public delegate Task OnGetChargePointListResponseDelegate       (DateTimeOffset                      LogTimestamp,
                                                                     DateTimeOffset                      RequestTimestamp,
                                                                     EMPClient                           Sender,
                                                                     //String                              SenderId,
                                                                     EventTracking_Id                    EventTrackingId,
                                                                     TimeSpan?                           RequestTimeout,
                                                                     GetChargePointListResponse          Result,
                                                                     TimeSpan                            Duration);


    /// <summary>
    /// A delegate called whenever a request for a list of charge point updates will be send upstream.
    /// </summary>
    public delegate Task OnGetChargePointListUpdatesRequestDelegate (DateTimeOffset                      LogTimestamp,
                                                                     DateTimeOffset                      RequestTimestamp,
                                                                     EMPClient                           Sender,
                                                                     //String                              SenderId,
                                                                     EventTracking_Id                    EventTrackingId,
                                                                     DateTimeOffset                      LastUpdate,
                                                                     TimeSpan?                           RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response on a request for a list of charge point updates was received.
    /// </summary>
    public delegate Task OnGetChargePointListUpdatesResponseDelegate(DateTimeOffset                      LogTimestamp,
                                                                     DateTimeOffset                      RequestTimestamp,
                                                                     EMPClient                           Sender,
                                                                     //String                              SenderId,
                                                                     EventTracking_Id                    EventTrackingId,
                                                                     DateTimeOffset                      LastUpdate,
                                                                     TimeSpan?                           RequestTimeout,
                                                                     GetChargePointListUpdatesResponse   Result,
                                                                     TimeSpan                            Duration);

    #endregion

    #region OnGetStatus

    /// <summary>
    /// A delegate called whenever a get status request will be send upstream.
    /// </summary>
    public delegate Task OnGetStatusRequestDelegate (DateTimeOffset         LogTimestamp,
                                                     DateTimeOffset         RequestTimestamp,
                                                     EMPClient              Sender,
                                                     //String                 SenderId,
                                                     EventTracking_Id       EventTrackingId,
                                                     DateTimeOffset?        LastRequest,
                                                     StatusTypes?           StatusType,
                                                     TimeSpan?              RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a get status request upstream had been received.
    /// </summary>
    public delegate Task OnGetStatusResponseDelegate(DateTimeOffset         LogTimestamp,
                                                     DateTimeOffset         RequestTimestamp,
                                                     EMPClient              Sender,
                                                     //String                 SenderId,
                                                     EventTracking_Id       EventTrackingId,
                                                     DateTimeOffset?        LastRequest,
                                                     StatusTypes?           StatusType,
                                                     TimeSpan?              RequestTimeout,
                                                     GetStatusResponse      Result,
                                                     TimeSpan               Duration);

    #endregion

    #region OnGetTariffUpdates

    /// <summary>
    /// A delegate called whenever a get tariff updates request will be send upstream.
    /// </summary>
    public delegate Task OnGetTariffUpdatesRequestDelegate (DateTimeOffset             LogTimestamp,
                                                            DateTimeOffset             RequestTimestamp,
                                                            EMPClient                  Sender,
                                                            //String                     SenderId,
                                                            EventTracking_Id           EventTrackingId,
                                                            DateTimeOffset?            LastRequest,
                                                            TimeSpan?                  RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a get tariff updates request upstream had been received.
    /// </summary>
    public delegate Task OnGetTariffUpdatesResponseDelegate(DateTimeOffset             LogTimestamp,
                                                            DateTimeOffset             RequestTimestamp,
                                                            EMPClient                  Sender,
                                                            //String                     SenderId,
                                                            EventTracking_Id           EventTrackingId,
                                                            DateTimeOffset?            LastRequest,
                                                            TimeSpan?                  RequestTimeout,
                                                            GetTariffUpdatesResponse   Result,
                                                            TimeSpan                   Duration);

    #endregion

    #region On(Set|Update)RoamingAuthorisationList

    /// <summary>
    /// A delegate called whenever a roaming authorisation list will be send upstream.
    /// </summary>
    public delegate Task OnSetRoamingAuthorisationListRequestDelegate    (DateTimeOffset                           LogTimestamp,
                                                                          DateTimeOffset                           RequestTimestamp,
                                                                          EMPClient                                Sender,
                                                                          //String                                   SenderId,
                                                                          EventTracking_Id                         EventTrackingId,
                                                                          IEnumerable<RoamingAuthorisationInfo>    RoamingAuthorisationInfos,
                                                                          TimeSpan?                                RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response on sending a roaming authorisation list had been received.
    /// </summary>
    public delegate Task OnSetRoamingAuthorisationListResponseDelegate   (DateTimeOffset                           LogTimestamp,
                                                                          DateTimeOffset                           RequestTimestamp,
                                                                          EMPClient                                Sender,
                                                                          //String                                   SenderId,
                                                                          EventTracking_Id                         EventTrackingId,
                                                                          IEnumerable<RoamingAuthorisationInfo>    RoamingAuthorisationInfos,
                                                                          TimeSpan?                                RequestTimeout,
                                                                          SetRoamingAuthorisationListResponse      Result,
                                                                          TimeSpan                                 Duration);


    /// <summary>
    /// A delegate called whenever an update for a roaming authorisation list will be send upstream.
    /// </summary>
    public delegate Task OnUpdateRoamingAuthorisationListRequestDelegate (DateTimeOffset                           LogTimestamp,
                                                                          DateTimeOffset                           RequestTimestamp,
                                                                          EMPClient                                Sender,
                                                                          //String                                   SenderId,
                                                                          EventTracking_Id                         EventTrackingId,
                                                                          IEnumerable<RoamingAuthorisationInfo>    RoamingAuthorisationInfos,
                                                                          TimeSpan?                                RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response on a roaming authorisation list update had been received.
    /// </summary>
    public delegate Task OnUpdateRoamingAuthorisationListResponseDelegate(DateTimeOffset                           LogTimestamp,
                                                                          DateTimeOffset                           RequestTimestamp,
                                                                          EMPClient                                Sender,
                                                                          //String                                   SenderId,
                                                                          EventTracking_Id                         EventTrackingId,
                                                                          IEnumerable<RoamingAuthorisationInfo>    RoamingAuthorisationInfos,
                                                                          TimeSpan?                                RequestTimeout,
                                                                          UpdateRoamingAuthorisationListResponse   Result,
                                                                          TimeSpan                                 Duration);

    #endregion

    #region On(Get|Confirm)CDRs

    /// <summary>
    /// A delegate called whenever a get charge detail records request will be send upstream.
    /// </summary>
    public delegate Task OnGetCDRsRequestDelegate     (DateTimeOffset         LogTimestamp,
                                                       DateTimeOffset         RequestTimestamp,
                                                       EMPClient              Sender,
                                                       //String                 SenderId,
                                                       EventTracking_Id       EventTrackingId,
                                                       CDRStatus?             CDRStatus,
                                                       TimeSpan?              RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a get charge detail records request upstream had been received.
    /// </summary>
    public delegate Task OnGetCDRsResponseDelegate    (DateTimeOffset         LogTimestamp,
                                                       DateTimeOffset         RequestTimestamp,
                                                       EMPClient              Sender,
                                                       //String                 SenderId,
                                                       EventTracking_Id       EventTrackingId,
                                                       CDRStatus?             CDRStatus,
                                                       TimeSpan?              RequestTimeout,
                                                       GetCDRsResponse        Result,
                                                       TimeSpan               Duration);


    /// <summary>
    /// A delegate called whenever a confirm charge detail records request will be send upstream.
    /// </summary>
    public delegate Task OnConfirmCDRsRequestDelegate (DateTimeOffset             LogTimestamp,
                                                       DateTimeOffset             RequestTimestamp,
                                                       EMPClient                  Sender,
                                                       //String                     SenderId,
                                                       EventTracking_Id           EventTrackingId,
                                                       IEnumerable<EVSECDRPair>   Approved,
                                                       IEnumerable<EVSECDRPair>   Declined,
                                                       TimeSpan?                  RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a confirm charge detail records request upstream had been received.
    /// </summary>
    public delegate Task OnConfirmCDRsResponseDelegate(DateTimeOffset             LogTimestamp,
                                                       DateTimeOffset             RequestTimestamp,
                                                       EMPClient                  Sender,
                                                       //String                     SenderId,
                                                       EventTracking_Id           EventTrackingId,
                                                       IEnumerable<EVSECDRPair>   Approved,
                                                       IEnumerable<EVSECDRPair>   Declined,
                                                       TimeSpan?                  RequestTimeout,
                                                       ConfirmCDRsResponse        Result,
                                                       TimeSpan                   Duration);

    #endregion


    // OCHPdirect

    #region On(Add|Get)ServiceEndpoints

    /// <summary>
    /// A delegate called whenever an add service endpoints request will be send upstream.
    /// </summary>
    public delegate Task OnAddServiceEndpointsRequestDelegate (DateTimeOffset                  LogTimestamp,
                                                               DateTimeOffset                  RequestTimestamp,
                                                               EMPClient                       Sender,
                                                               //String                          SenderId,
                                                               EventTracking_Id                EventTrackingId,
                                                               IEnumerable<ProviderEndpoint>   ProviderEndpoints,
                                                               TimeSpan?                       RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending an add service endpoints request upstream had been received.
    /// </summary>
    public delegate Task OnAddServiceEndpointsResponseDelegate(//DateTimeOffset                  LogTimestamp,
                                                               DateTimeOffset                  RequestTimestamp,
                                                               EMPClient                       Sender,
                                                               //String                          SenderId,
                                                               EventTracking_Id                EventTrackingId,
                                                               IEnumerable<ProviderEndpoint>   ProviderEndpoints,
                                                               TimeSpan?                       RequestTimeout,
                                                               AddServiceEndpointsResponse     Result,
                                                               TimeSpan                        Duration);



    /// <summary>
    /// A delegate called whenever a get service endpoints request will be send upstream.
    /// </summary>
    public delegate Task OnGetServiceEndpointsRequestDelegate (DateTimeOffset                LogTimestamp,
                                                               DateTimeOffset                RequestTimestamp,
                                                               EMPClient                     Sender,
                                                               //String                        SenderId,
                                                               EventTracking_Id              EventTrackingId,
                                                               TimeSpan?                     RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a get service endpoints request upstream had been received.
    /// </summary>
    public delegate Task OnGetServiceEndpointsResponseDelegate(//DateTimeOffset                LogTimestamp,
                                                               DateTimeOffset                RequestTimestamp,
                                                               EMPClient                     Sender,
                                                               //String                        SenderId,
                                                               EventTracking_Id              EventTrackingId,
                                                               TimeSpan?                     RequestTimeout,
                                                               GetServiceEndpointsResponse   Result,
                                                               TimeSpan                      Duration);

    #endregion

    #region On(Select|Control|Release)EVSE

    /// <summary>
    /// A delegate called whenever a select EVSE request will be send to a charge point operator.
    /// </summary>
    public delegate Task OnSelectEVSERequestDelegate (DateTimeOffset         LogTimestamp,
                                                      DateTimeOffset         RequestTimestamp,
                                                      EMPClient              Sender,
                                                      //String                 SenderId,
                                                      EventTracking_Id       EventTrackingId,
                                                      EVSE_Id                EVSEId,
                                                      Contract_Id            ContractId,
                                                      DateTimeOffset?        ReserveUntil,
                                                      TimeSpan?              RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a select EVSE request to a charge point operator had been received.
    /// </summary>
    public delegate Task OnSelectEVSEResponseDelegate(DateTimeOffset         LogTimestamp,
                                                      DateTimeOffset         RequestTimestamp,
                                                      EMPClient              Sender,
                                                      //String                 SenderId,
                                                      EventTracking_Id       EventTrackingId,
                                                      EVSE_Id                EVSEId,
                                                      Contract_Id            ContractId,
                                                      DateTimeOffset?        ReserveUntil,
                                                      TimeSpan?              RequestTimeout,
                                                      SelectEVSEResponse     Result,
                                                      TimeSpan               Duration);



    /// <summary>
    /// A delegate called whenever a control EVSE request will be send to a charge point operator.
    /// </summary>
    public delegate Task OnControlEVSERequestDelegate (DateTimeOffset        LogTimestamp,
                                                       DateTimeOffset        RequestTimestamp,
                                                       EMPClient             Sender,
                                                       //String                SenderId,
                                                       EventTracking_Id      EventTrackingId,
                                                       Direct_Id             DirectId,
                                                       DirectOperations      Operation,
                                                       Single?               MaxPower,
                                                       Single?               MaxCurrent,
                                                       Boolean?              OnePhase,
                                                       Single?               MaxEnergy,
                                                       Single?               MinEnergy,
                                                       DateTimeOffset?       Departure,
                                                       TimeSpan?             RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a control EVSE request to a charge point operator had been received.
    /// </summary>
    public delegate Task OnControlEVSEResponseDelegate(DateTimeOffset        LogTimestamp,
                                                       DateTimeOffset        RequestTimestamp,
                                                       EMPClient             Sender,
                                                       //String                SenderId,
                                                       EventTracking_Id      EventTrackingId,
                                                       Direct_Id             DirectId,
                                                       DirectOperations      Operation,
                                                       Single?               MaxPower,
                                                       Single?               MaxCurrent,
                                                       Boolean?              OnePhase,
                                                       Single?               MaxEnergy,
                                                       Single?               MinEnergy,
                                                       DateTimeOffset?       Departure,
                                                       TimeSpan?             RequestTimeout,
                                                       ControlEVSEResponse   Result,
                                                       TimeSpan              Duration);



    /// <summary>
    /// A delegate called whenever a release EVSE request will be send to a charge point operator.
    /// </summary>
    public delegate Task OnReleaseEVSERequestDelegate (DateTimeOffset        LogTimestamp,
                                                       DateTimeOffset        RequestTimestamp,
                                                       EMPClient             Sender,
                                                       //String                SenderId,
                                                       EventTracking_Id      EventTrackingId,
                                                       Direct_Id             DirectId,
                                                       TimeSpan?             RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a release EVSE request to a charge point operator had been received.
    /// </summary>
    public delegate Task OnReleaseEVSEResponseDelegate(DateTimeOffset        LogTimestamp,
                                                       DateTimeOffset        RequestTimestamp,
                                                       EMPClient             Sender,
                                                       //String                SenderId,
                                                       EventTracking_Id      EventTrackingId,
                                                       Direct_Id             DirectId,
                                                       TimeSpan?             RequestTimeout,
                                                       ReleaseEVSEResponse   Result,
                                                       TimeSpan              Duration);

    #endregion

    #region OnDirectEVSEStatus

    /// <summary>
    /// A delegate called whenever an EVSE status request will be send to a charge point operator.
    /// </summary>
    public delegate Task OnGetEVSEStatusRequestDelegate (DateTimeOffset          LogTimestamp,
                                                         DateTimeOffset          RequestTimestamp,
                                                         EMPClient               Sender,
                                                         //String                  SenderId,
                                                         EventTracking_Id        EventTrackingId,
                                                         IEnumerable<EVSE_Id>    EVSEs,
                                                         TimeSpan?               RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending an EVSE status request to a charge point operator had been received.
    /// </summary>
    public delegate Task OnGetEVSEStatusResponseDelegate(DateTimeOffset          LogTimestamp,
                                                         DateTimeOffset          RequestTimestamp,
                                                         EMPClient               Sender,
                                                         //String                  SenderId,
                                                         EventTracking_Id        EventTrackingId,
                                                         IEnumerable<EVSE_Id>    EVSEs,
                                                         TimeSpan?               RequestTimeout,
                                                         GetEVSEStatusResponse   Result,
                                                         TimeSpan                Duration);

    #endregion

    #region OnReportDiscrepancy

    /// <summary>
    /// A delegate called whenever a report discrepancy request will be send to a charge point operator.
    /// </summary>
    public delegate Task OnReportDiscrepancyRequestDelegate (DateTimeOffset              LogTimestamp,
                                                             DateTimeOffset              RequestTimestamp,
                                                             EMPClient                   Sender,
                                                             //String                      SenderId,
                                                             EventTracking_Id            EventTrackingId,
                                                             EVSE_Id                     EVSEId,
                                                             String                      Report,
                                                             TimeSpan?                   RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a report discrepancy request to a charge point operator had been received.
    /// </summary>
    public delegate Task OnReportDiscrepancyResponseDelegate(DateTimeOffset              LogTimestamp,
                                                             DateTimeOffset              RequestTimestamp,
                                                             EMPClient                   Sender,
                                                             //String                      SenderId,
                                                             EventTracking_Id            EventTrackingId,
                                                             EVSE_Id                     EVSEId,
                                                             String                      Report,
                                                             TimeSpan?                   RequestTimeout,
                                                             ReportDiscrepancyResponse   Result,
                                                             TimeSpan                    Duration);

    #endregion

    #region GetInformProvider

    /// <summary>
    /// A delegate called whenever a get inform provider request will be send to a charge point operator.
    /// </summary>
    public delegate Task OnGetInformProviderRequestDelegate (DateTimeOffset              LogTimestamp,
                                                             DateTimeOffset              RequestTimestamp,
                                                             EMPClient                   Sender,
                                                             //String                      SenderId,
                                                             EventTracking_Id            EventTrackingId,
                                                             Direct_Id                   DirectId,
                                                             TimeSpan?                   RequestTimeout);

    /// <summary>
    /// A delegate called whenever a response after sending a get inform provider request to a charge point operator had been received.
    /// </summary>
    public delegate Task OnGetInformProviderResponseDelegate(DateTimeOffset              LogTimestamp,
                                                             DateTimeOffset              RequestTimestamp,
                                                             EMPClient                   Sender,
                                                             //String                      SenderId,
                                                             EventTracking_Id            EventTrackingId,
                                                             Direct_Id                   DirectId,
                                                             TimeSpan?                   RequestTimeout,
                                                             GetInformProviderResponse   Result,
                                                             TimeSpan                    Duration);

    #endregion

}
