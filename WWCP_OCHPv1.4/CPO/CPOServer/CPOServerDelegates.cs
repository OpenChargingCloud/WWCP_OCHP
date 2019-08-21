/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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

using org.GraphDefined.WWCP.OCHPv1_4.EMP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    // OCHPdirect

    #region OnSelectEVSE

    /// <summary>
    /// Initiate a select EVSE request at the given EVSE and for the given e-mobility contract.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSEId">The unique identification of the selected EVSE.</param>
    /// <param name="ContractId">The unique identification of an e-mobility contract.</param>
    /// <param name="ReserveUntil">An optional timestamp till when then given EVSE should be reserved.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task<SelectEVSEResponse>

        OnSelectEVSEDelegate(DateTime             Timestamp,
                             CPOServer            Sender,
                             CancellationToken    CancellationToken,
                             EventTracking_Id     EventTrackingId,
                             EVSE_Id              EVSEId,
                             Contract_Id          ContractId,
                             DateTime?            ReserveUntil,
                             TimeSpan?            RequestTimeout  = null);

    #endregion

    #region OnControlEVSE

    /// <summary>
    /// Initiate a control EVSE request for the given charging process session identification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="DirectId">The unique session identification of the direct charging process to be controlled.</param>
    /// <param name="Operation">The operation to be performed for the selected charge point.</param>
    /// <param name="MaxPower">Maximum authorised power in kW.</param>
    /// <param name="MaxCurrent">Maximum authorised current in A.</param>
    /// <param name="OnePhase">Marks an AC-charging session to be limited to one-phase charging.</param>
    /// <param name="MaxEnergy">Maximum authorised energy in kWh.</param>
    /// <param name="MinEnergy">Minimum required energy in kWh.</param>
    /// <param name="Departure">Scheduled time of departure.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task<ControlEVSEResponse>

        OnControlEVSEDelegate(DateTime             Timestamp,
                              CPOServer            Sender,
                              CancellationToken    CancellationToken,
                              EventTracking_Id     EventTrackingId,
                              Direct_Id            DirectId,
                              DirectOperations     Operation,
                              Single?              MaxPower,
                              Single?              MaxCurrent,
                              Boolean?             OnePhase,
                              Single?              MaxEnergy,
                              Single?              MinEnergy,
                              DateTime?            Departure,
                              TimeSpan?            RequestTimeout  = null);

    #endregion

    #region OnReleaseEVSE

    /// <summary>
    /// Initiate a release EVSE request for the given direct charging session identification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="DirectId">The session id referencing the direct charging process to be released.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task<ReleaseEVSEResponse>

        OnReleaseEVSEDelegate(DateTime             Timestamp,
                              CPOServer            Sender,
                              CancellationToken    CancellationToken,
                              EventTracking_Id     EventTrackingId,
                              Direct_Id            DirectId,
                              TimeSpan?            RequestTimeout  = null);

    #endregion

    #region OnGetEVSEStatus

    /// <summary>
    /// Initiate a get EVSE status request for the given enumeration of EVSE identifications.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSEIds">An enumeration of EVSE identifications.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task<GetEVSEStatusResponse>

        OnGetEVSEStatusDelegate(DateTime                Timestamp,
                                CPOServer               Sender,
                                CancellationToken       CancellationToken,
                                EventTracking_Id        EventTrackingId,
                                IEnumerable<EVSE_Id>    EVSEIds,
                                TimeSpan?               RequestTimeout  = null);

    #endregion

    #region OnReportDiscrepancy

    /// <summary>
    /// Initiate a report discrepancy request for the given EVSE identification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// <param name="EVSEId">The EVSE identification affected by this report.</param>
    /// <param name="Report">Textual or generated report of the discrepancy.</param>
    /// <param name="RequestTimeout">An optional timeout for this request.</param>
    public delegate Task<ReportDiscrepancyResponse>

        OnReportDiscrepancyDelegate(DateTime             Timestamp,
                                    CPOServer            Sender,
                                    CancellationToken    CancellationToken,
                                    EventTracking_Id     EventTrackingId,
                                    EVSE_Id              EVSEId,
                                    String               Report,
                                    TimeSpan?            RequestTimeout  = null);

    #endregion

}
