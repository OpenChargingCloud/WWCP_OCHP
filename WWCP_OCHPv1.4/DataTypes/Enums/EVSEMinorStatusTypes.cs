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

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// The current minor dynamic status of an OCHP Electric Vehicle Supply Equipment.
    /// </summary>
    public enum EVSEMinorStatusTypes
    {

        /// <summary>
        /// The EVSE minor status is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The EVSE is able to start a new charging process.
        /// </summary>
        Available,

        /// <summary>
        /// The EVSE is able to start a new charging process for limited duration
        /// as a future reservation is present. ttl to be set on the start of the
        /// reservation when in future or to the end of the reservation else.
        /// </summary>
        Reserved,

        /// <summary>
        /// The EVSE is in use. TTL to be set on the expected end of the charging process.
        /// </summary>
        Charging,

        /// <summary>
        /// The EVSE not accessible because of a physical barrier, i.e. a car.
        /// </summary>
        Blocked,

        /// <summary>
        /// The EVSE is currently out of order. TTL to be set to the expected re-enabling.
        /// </summary>
        OutOfOrder

    }

}
