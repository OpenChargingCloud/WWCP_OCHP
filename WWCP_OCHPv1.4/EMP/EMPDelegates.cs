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

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// A delegate which allows you to modify charge point infos
    /// after receiving them.
    /// </summary>
    /// <param name="ChargePointInfo">An OCHP charge point info.</param>
    /// <param name="EVSE">A WWCP EVSE.</param>
    public delegate EVSE ChargePointInfo2EVSEDelegate(ChargePointInfo  ChargePointInfo,
                                                      EVSE             EVSE);

}
