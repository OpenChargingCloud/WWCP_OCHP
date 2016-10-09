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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// An OCHP price component.
    /// </summary>
    public class PriceComponent
    {

        #region Properties

        /// <summary>
        /// What dimension is part of this tariff element.
        /// </summary>
        public BillingItemTypes  BillingItem    { get; }

        /// <summary>
        /// Price per unit of the billing item in the given currency.
        /// </summary>
        public Single            ItemPrice      { get; }

        /// <summary>
        /// Minimum amount to be billed (billing will happen in stepSize increments).
        /// In case the billingItem is a one time payment, stepSize is to be set to 1.
        /// </summary>
        public UInt16            StepSize       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP price component.
        /// </summary>
        /// <param name="BillingItem">What dimension is part of this tariff element.</param>
        /// <param name="ItemPrice">Price per unit of the billing item in the given currency.</param>
        /// <param name="StepSize">Minimum amount to be billed (billing will happen in stepSize increments). In case the billingItem is a one time payment, stepSize is to be set to 1.</param>
        public PriceComponent(BillingItemTypes  BillingItem,
                              Single            ItemPrice,
                              UInt16            StepSize)
        {

            this.BillingItem  = BillingItem;
            this.ItemPrice    = ItemPrice;
            this.StepSize     = StepSize;

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(BillingItem, " for ", ItemPrice, ", step size ", StepSize);

        #endregion

    }

}
