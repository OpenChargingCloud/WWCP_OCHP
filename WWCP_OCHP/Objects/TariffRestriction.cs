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

#region Usings

using System;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// Specifies the status of an OCHP tariff restriction.
    /// </summary>
    public class TariffRestriction
    {

        #region Properties

        /// <summary>
        /// Regular hours that this tariff element should be valid for (maximum of 14 entries).
        /// If always valid (24/7), don't set (as this is a tariff restriction).
        /// </summary>
        public RegularHours  RegularHours   { get; }

        /// <summary>
        /// Start date, for example: 2015-12-24, valid from this day (midnight, i.e. including this day).
        /// </summary>
        public HourMin?      StartDate      { get; }

        /// <summary>
        /// End date, for example: 2015-12-27, valid until this day (midnight, i.e. excluding this day).
        /// </summary>
        public HourMin?      EndDate        { get; }

        /// <summary>
        /// Minimum used energy in kWh, for example 20.0, valid from this amount of energy is used.
        /// </summary>
        public Single?       MinEnergy      { get; }

        /// <summary>
        /// Maximum used energy in kWh, for example 50.0, valid until this amount of energy is used.
        /// </summary>
        public Single?       MaxEnergy      { get; }

        /// <summary>
        /// Minimum power in kW, for example 0.0, valid from this charging speed.
        /// </summary>
        public Single?       MinPower       { get; }

        /// <summary>
        /// Maximum power in kW, for example 20.0, valid up to this charging speed.
        /// </summary>
        public Single?       MaxPower       { get; }

        /// <summary>
        /// Minimum duration, valid for the given amount of time.
        /// </summary>
        public TimeSpan?     MinDuration    { get; }

        /// <summary>
        /// Maximum duration, valid for the given amount of time.
        /// </summary>
        public TimeSpan?     MaxDuration    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP parking tariff restriction.
        /// </summary>
        /// <param name="RegularHours">Regular hours that this tariff element should be valid for (maximum of 14 entries). If always valid (24/7), don't set (as this is a tariff restriction).</param>
        /// <param name="StartDate">Start date, for example: 2015-12-24, valid from this day (midnight, i.e. including this day).</param>
        /// <param name="EndDate">End date, for example: 2015-12-27, valid until this day (midnight, i.e. excluding this day).</param>
        /// <param name="MinEnergy">Minimum used energy in kWh, for example 20.0, valid from this amount of energy is used.</param>
        /// <param name="MaxEnergy">Maximum used energy in kWh, for example 50.0, valid until this amount of energy is used.</param>
        /// <param name="MinPower">Minimum power in kW, for example 0.0, valid from this charging speed.</param>
        /// <param name="MaxPower">Maximum power in kW, for example 20.0, valid up to this charging speed.</param>
        /// <param name="MinDuration">Minimum duration, valid for the given amount of time.</param>
        /// <param name="MaxDuration">Maximum duration, valid for the given amount of time.</param>
        public TariffRestriction(RegularHours  RegularHours,
                                 HourMin?      StartDate,
                                 HourMin?      EndDate,
                                 Single?       MinEnergy,
                                 Single?       MaxEnergy,
                                 Single?       MinPower,
                                 Single?       MaxPower,
                                 TimeSpan?     MinDuration,
                                 TimeSpan?     MaxDuration)
        {

            this.RegularHours  = RegularHours;
            this.StartDate     = StartDate;
            this.EndDate       = EndDate;
            this.MinEnergy     = MinEnergy;
            this.MaxEnergy     = MaxEnergy;
            this.MinPower      = MinPower;
            this.MaxPower      = MaxPower;
            this.MinDuration   = MinDuration;
            this.MaxDuration   = MaxDuration;

        }

        #endregion


    }

}
