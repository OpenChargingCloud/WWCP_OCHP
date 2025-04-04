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

using cloud.charging.open.protocols.WWCP;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.EMP
{

    ///// <summary>
    ///// A delegate which allows you to modify the conversion from EVSE data records to WWCP EVSEs.
    ///// </summary>
    ///// <param name="EVSEDataRecord">An OCHP EVSE data record.</param>
    ///// <param name="EVSE">A WWCP EVSE.</param>
    //public delegate EVSE                     EVSEDataRecord2EVSEDelegate                      (EVSEDataRecord           EVSEDataRecord,
    //                                                                                           EVSE                     EVSE);

    ///// <summary>
    ///// A delegate which allows you to modify the conversion from EVSE status records to WWCP EVSE status updates.
    ///// </summary>
    ///// <param name="EVSEStatusRecord">An OCHP EVSE status record.</param>
    ///// <param name="EVSEStatusUpdate">A WWCP EVSE status update.</param>
    //public delegate EVSEStatusUpdate         EVSEStatusRecord2EVSEStatusUpdateDelegate        (EVSEStatusRecord         EVSEStatusRecord,
    //                                                                                           EVSEStatusUpdate         EVSEStatusUpdate);

    ///// <summary>
    ///// A delegate which allows you to modify the conversion from charge detail records to WWCP charge detail records.
    ///// </summary>
    ///// <param name="EVSEStatusRecord">An OCHP charge detail record.</param>
    ///// <param name="EVSEStatus">A WWCP charge detail record.</param>
    //public delegate WWCP.ChargeDetailRecord  ChargeDetailRecord2WWCPChargeDetailRecordDelegate(ChargeDetailRecord       ChargeDetailRecord,
    //                                                                                           WWCP.ChargeDetailRecord  WWCPChargeDetailRecord);

    /// <summary>
    /// A delegate which allows you to modify charge point infos
    /// after receiving them.
    /// </summary>
    /// <param name="ChargePointInfo">An OCHP charge point info.</param>
    /// <param name="EVSE">A WWCP EVSE.</param>
    public delegate EVSE ChargePointInfo2EVSEDelegate(ChargePointInfo  ChargePointInfo,
                                                      EVSE             EVSE);

}
