/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

#region Usings

using System;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// OCHP (parking) restrictions.
    /// </summary>
    [Flags]
    public enum RestrictionTypes
    {

        /// <summary>
        /// The (parking) restriction is unknown.
        /// </summary>
        Unknown         = 0,

        /// <summary>
        /// Reserved parking spot for electric vehicles.
        /// </summary>
        EVOnly          = 1,

        /// <summary>
        /// Parking allowed only while plugged in (and charging).
        /// </summary>
        Plugged         = 2,

        /// <summary>
        /// Reserved parking spot for disabled people with valid ID.
        /// </summary>
        Disabled        = 4,

        /// <summary>
        /// Parking or charging for costumer/guests only, for example in case of a hotel or shop.
        /// </summary>
        Customers       = 8,

        /// <summary>
        /// Parking spot only suitable for (electric) motorcycles, scooters or bicycles.
        /// </summary>
        Motorcycles     = 16,

        /// <summary>
        /// Charging / parking only for carsharing vehicles.
        /// </summary>
        CarSharing      = 32

    }

}
