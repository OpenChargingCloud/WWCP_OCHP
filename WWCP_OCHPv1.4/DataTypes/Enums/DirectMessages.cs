/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// Messages to inform a provider about an OCHP-direct charging process.
    /// </summary>
    public enum DirectMessages
    {

        /// <summary>
        /// Unknown message type.
        /// </summary>
        Unknown,

        /// <summary>
        /// A OCHPdirect charging process has been started.
        /// </summary>
        Start,

        /// <summary>
        /// The parameters of the charging process were changed.
        /// </summary>
        Change,

        /// <summary>
        /// A informative update is available, e.g. updated consumed energy value.
        /// </summary>
        Info,

        /// <summary>
        /// The charging process has ended, the connector was unplugged.
        /// </summary>
        End,

        /// <summary>
        /// The session is finished and the CDR was sent out.
        /// </summary>
        Finish

    }

}
