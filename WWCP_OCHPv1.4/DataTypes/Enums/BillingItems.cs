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
    /// The billing items for charging periods and tariffs.
    /// </summary>
    public enum BillingItems
    {

        /// <summary>
        /// Unknown billing item.
        /// </summary>
        Unknown,

        /// <summary>
        /// Price for the time of parking. The billingValue represents the time in hours.
        /// </summary>
        ParkingTime,

        /// <summary>
        /// Price for the time of EVSE usage. The billingValue represents the time in hours.
        /// </summary>
        UsageTime,

        /// <summary>
        /// Price for the consumed energy. The billingValue represents the energy in kilowatt-hours.
        /// </summary>
        Energy,

        /// <summary>
        /// Price for the used power level. The billingValue represents the maximum power in kilowatts.
        /// </summary>
        Power,

        /// <summary>
        /// General service fee per charging process. The billingValue represents a multiplier and thus has to be set to "1.0".
        /// </summary>
        ServiceFee,

        /// <summary>
        /// One time fee for a reservation of the EVSE. The billingValue represents a multiplier and thus has to be set to "1.0".
        /// </summary>
        Reservation,

        /// <summary>
        /// Price for the duration of a reservation. The billingValue represents the time in hours.
        /// </summary>
        ReservationTime

    }

}
