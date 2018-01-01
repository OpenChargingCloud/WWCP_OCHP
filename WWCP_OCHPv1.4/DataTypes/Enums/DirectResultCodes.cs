/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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
    /// Result and error codes for the class DirectResult as return value for method calls.
    /// </summary>
    public enum DirectResultCodes
    {

        /// <summary>
        /// Unknown OCHPdirect result code.
        /// </summary>
        Unknown,

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        OK,

        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        Partly,

        /// <summary>
        /// Given EVSEId is not known to the operator.
        /// </summary>
        NotFound,

        /// <summary>
        /// Given EVSEId does not support OCHPdirect.
        /// </summary>
        NotSupported,

        /// <summary>
        /// The DirectId is not valid or has expired.
        /// </summary>
        InvalidId,

        /// <summary>
        /// Internal server error.
        /// </summary>
        Server

    }

}
