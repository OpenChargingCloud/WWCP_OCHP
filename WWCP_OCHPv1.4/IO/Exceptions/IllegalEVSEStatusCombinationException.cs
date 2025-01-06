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

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// An exception thrown whenever an illegal combination of EVSE major and minor status occured.
    /// </summary>
    public class IllegalEVSEStatusCombinationException : OCHPException
    {

        /// <summary>
        /// Create a new illegal EVSE major and minor status combination exception.
        /// </summary>
        /// <param name="EVSEId">An EVSE identification.</param>
        /// <param name="MajorStatus">An EVSE major status.</param>
        /// <param name="MinorStatus">An EVSE minor status.</param>
        public IllegalEVSEStatusCombinationException(EVSE_Id               EVSEId,
                                                     EVSEMajorStatusTypes  MajorStatus,
                                                     EVSEMinorStatusTypes  MinorStatus)

            : base("Illegal combination of major '" + MajorStatus + "' and minor '" + MinorStatus + "' EVSE status for EVSE '" + EVSEId + "'!")

        { }

    }

}
