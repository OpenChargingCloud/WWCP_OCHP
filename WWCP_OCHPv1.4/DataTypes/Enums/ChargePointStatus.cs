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

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// The OCHP overall status of a charge point.
    /// </summary>
    public enum ChargePointStatus
    {

        /// <summary>
        /// No status information available.
        /// </summary>
        Unknown,

        /// <summary>
        /// Charge point is in operation and can be used.
        /// </summary>
        Operative,

        /// <summary>
        /// Charge point cannot be used due to maintenance, greater downtime,
        /// blocking construction works or other access restrictions
        /// (temporarily, will be operative in the future). 
        /// </summary>
        Inoperative,

        /// <summary>
        /// Planned charge point, will be operating soon
        /// </summary>
        Planned,

        /// <summary>
        /// Discontinued charge point, will be deleted soon
        /// </summary>
        Closed

    }

}
