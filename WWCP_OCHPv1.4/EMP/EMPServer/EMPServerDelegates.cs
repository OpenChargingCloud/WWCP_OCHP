/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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

using org.GraphDefined.WWCP.OCHPv1_4.CPO;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    // OCHPdirect

    #region OnInformProvider

    /// <summary>
    /// Initiate an AuthorizeStart for the given AuthToken at the given EVSE.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="CancellationToken">A token to cancel this task.</param>
    /// <param name="EventTrackingId">An unique event tracking identification for correlating this request with other events.</param>
    /// 
    /// <param name="DirectMessage">The operation that triggered the operator to send this message.</param>
    /// <param name="EVSEId">The uqniue EVSE identification of the charge point which is used for this charging process.</param>
    /// <param name="ContractId">The current contract identification using the charge point.</param>
    /// <param name="DirectId">The session identification of the direct charging process.</param>
    /// 
    /// <param name="SessionTimeoutAt">On success the timeout for this session.</param>
    /// <param name="StateOfCharge">Current state of charge of the connected EV in percent.</param>
    /// <param name="MaxPower">Maximum authorised power in kW.</param>
    /// <param name="MaxCurrent">Maximum authorised current in A.</param>
    /// <param name="OnePhase">Marks an AC-charging session to be limited to one-phase charging.</param>
    /// <param name="MaxEnergy">Maximum authorised energy in kWh.</param>
    /// <param name="MinEnergy">Minimum required energy in kWh.</param>
    /// <param name="Departure">Scheduled time of departure.</param>
    /// <param name="CurrentPower">The currently supplied power limit in kWs (in case of load management).</param>
    /// <param name="ChargedEnergy">The overall amount of energy in kWhs transferred during this charging process.</param>
    /// <param name="MeterReading">The current meter value as displayed on the meter with corresponding timestamp to enable displaying it to the user.</param>
    /// <param name="ChargingPeriods">Can be used to transfer billing information to the provider in near real time.</param>
    /// <param name="CurrentCost">The total cost of the charging process that will be billed by the operator up to this point.</param>
    /// <param name="Currency">The displayed and charged currency. Defined in ISO 4217 - Table A.1, alphabetic list.</param>
    /// 
    /// <param name="QueryTimeout">An optional timeout for this request.</param>
    public delegate Task<InformProviderResponse>

        OnInformProviderDelegate(DateTime                 Timestamp,
                                 EMPServer                Sender,
                                 CancellationToken        CancellationToken,
                                 EventTracking_Id         EventTrackingId,

                                 DirectMessages           DirectMessage,
                                 EVSE_Id                  EVSEId,
                                 Contract_Id              ContractId,
                                 Direct_Id                DirectId,

                                 DateTime?                SessionTimeoutAt,
                                 Single?                  StateOfCharge,
                                 Single?                  MaxPower,
                                 Single?                  MaxCurrent,
                                 Boolean?                 OnePhase,
                                 Single?                  MaxEnergy,
                                 Single?                  MinEnergy,
                                 DateTime?                Departure,
                                 Single?                  CurrentPower,
                                 Single?                  ChargedEnergy,
                                 Timestamped<Single>?     MeterReading,
                                 IEnumerable<CDRPeriod>   ChargingPeriods,
                                 Single?                  CurrentCost,
                                 Currency                 Currency,

                                 TimeSpan?                QueryTimeout      = null);

    #endregion

}
