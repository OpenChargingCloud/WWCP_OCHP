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

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

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

}
