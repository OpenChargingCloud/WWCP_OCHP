/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// Specifies the representation of the token to allow hashed token values.
    /// </summary>
    public enum TokenRepresentations
    {

        /// <summary>
        /// The token instance is represented in plain text. (default)
        /// </summary>
        Plain,

        /// <summary>
        /// The token instance is represented in its 160bit SHA1 hash in 40 hexadecimal digits.
        /// </summary>
        SHA160,

        /// <summary>
        /// The token instance is represented in its 256bit SHA2 hash in 64 hexadecimal digits.
        /// </summary>
        SHA256

    }

}
