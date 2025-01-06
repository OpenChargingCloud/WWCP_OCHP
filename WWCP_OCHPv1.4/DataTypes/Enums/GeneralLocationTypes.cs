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
    /// The current status of an OCHP parking space.
    /// </summary>
    public enum GeneralLocationTypes
    {

        /// <summary>
        /// Unknown parking location type is not known by the operator.
        /// </summary>
        Unknown,

        /// <summary>
        /// Parking in public space.
        /// </summary>
        OnStreet,

        /// <summary>
        /// Multistorey car park.
        /// </summary>
        ParkingGarage,

        /// <summary>
        /// Multistorey car park, mainly underground.
        /// </summary>
        UndergroundGarage,

        /// <summary>
        /// A cleared area that is intended for parking vehicles, i.e.at super markets, bars, etc.
        /// </summary>
        ParkingLot,

        /// <summary>
        /// Located in private or corporate grounds, may not be accessible to the public.
        /// </summary>
        Private,

        /// <summary>
        /// none of the given possibilities.
        /// </summary>
        Other

    }

}
