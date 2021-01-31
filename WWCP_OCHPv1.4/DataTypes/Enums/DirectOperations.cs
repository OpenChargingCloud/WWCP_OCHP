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
    /// Operations to control an OCHP-direct charging process.
    /// </summary>
    public enum DirectOperations
    {

        /// <summary>
        /// Unknown operation.
        /// </summary>
        Unknown,

        /// <summary>
        /// Initiate the start. Operator should allow the user to plug in.
        /// </summary>
        Start,

        /// <summary>
        /// Change the parameters of the charging process.
        /// </summary>
        Change,

        /// <summary>
        /// End the charging process. Operator should allow the user to plug out.
        /// </summary>
        End

    }

}
