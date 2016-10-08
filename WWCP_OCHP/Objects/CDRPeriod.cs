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
    /// Specifies one time and billing period in an OCHP charge detail record.
    /// </summary>
    public class CDRPeriod
    {

        #region Properties

        /// <summary>
        /// Starting time of period. Must be equal or later than startDateTime of the CDRInfo.
        /// </summary>
        public DateTime          Start           { get; }

        /// <summary>
        /// Ending time of the period. Must be equal or earlier than endDateTime of the CDRInfo.
        /// </summary>
        public DateTime          End             { get; }

        /// <summary>
        /// Defines what the EVSP is charged for during this period.
        /// </summary>
        public BillingItemTypes  BillingItem     { get; }

        /// <summary>
        /// The value the EVSP is charged for. The unit of the value depends on the billingItem.
        /// </summary>
        public Single            BillingValue    { get; }

        /// <summary>
        /// Price per unit of the billingItem in the given currency.
        /// </summary>
        public Single            ItemPrice       { get; }

        /// <summary>
        /// The cost of the period in the given currency.
        /// </summary>
        public Single?           PeriodCost      { get; }

        /// <summary>
        /// Tax rate in percent that is to be paid for charging processes in the country of origin.
        /// </summary>
        public Single?           TaxRate         { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new time and billing period of an OCHP charge detail record.
        /// </summary>
        /// <param name="Start">Starting time of period. Must be equal or later than startDateTime of the CDRInfo.</param>
        /// <param name="End">Ending time of the period. Must be equal or earlier than endDateTime of the CDRInfo.</param>
        /// <param name="BillingItem">Defines what the EVSP is charged for during this period.</param>
        /// <param name="BillingValue">The value the EVSP is charged for. The unit of the value depends on the billingItem.</param>
        /// <param name="ItemPrice">Price per unit of the billingItem in the given currency.</param>
        /// <param name="PeriodCost">The cost of the period in the given currency.</param>
        /// <param name="TaxRate">Tax rate in percent that is to be paid for charging processes in the country of origin.</param>
        public CDRPeriod(DateTime          Start,
                         DateTime          End,
                         BillingItemTypes  BillingItem,
                         Single            BillingValue,
                         Single            ItemPrice,
                         Single?           PeriodCost,
                         Single?           TaxRate)

        {

            this.Start         = Start;
            this.End           = End;
            this.BillingItem   = BillingItem;
            this.BillingValue  = BillingValue;
            this.ItemPrice     = ItemPrice;
            this.PeriodCost    = PeriodCost;
            this.TaxRate       = TaxRate;

        }

        #endregion


    }

}
