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
using System.Threading;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

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

        OnSelectEVSERequestDelegate(DateTime             Timestamp,
                                    CPOServer            Sender,
                                    CancellationToken    CancellationToken,
                                    EventTracking_Id     EventTrackingId,
                                    EVSE_Id              EVSEId,
                                    Contract_Id          ContractId,
                                    DateTime?            ReserveUntil,
                                    TimeSpan?            RequestTimeout  = null);


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

        OnReleaseEVSERequestDelegate(DateTime            Timestamp,
                                     CPOServer           Sender,
                                     CancellationToken   CancellationToken,
                                     EventTracking_Id    EventTrackingId,
                                     Direct_Id           DirectId,
                                     TimeSpan?           RequestTimeout  = null);


}
