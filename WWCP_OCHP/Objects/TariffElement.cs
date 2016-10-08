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

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// An OCHP tariff element.
    /// </summary>
    public class TariffElement
    {

        #region Properties

        /// <summary>
        /// Contains information about the pricing structure of the tariff element.
        /// </summary>
        public PriceComponent     PriceComponent      { get; }

        /// <summary>
        /// Contains information about when to apply the defined priceComponent / tariffElement.
        /// </summary>
        public TariffRestriction  TariffRestriction   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP tariff element.
        /// </summary>
        /// <param name="PriceComponent">Contains information about the pricing structure of the tariff element.</param>
        /// <param name="TariffRestriction">Contains information about when to apply the defined priceComponent / tariffElement.</param>
        public TariffElement(PriceComponent     PriceComponent,
                             TariffRestriction  TariffRestriction)
        {

            this.PriceComponent     = PriceComponent;
            this.TariffRestriction  = TariffRestriction;

        }

        #endregion


        #region Operator overloading

        #region Operator == (TariffElement1, TariffElement2)

        /// <summary>
        /// Compares two tariff elements for equality.
        /// </summary>
        /// <param name="TariffElement1">A tariff element.</param>
        /// <param name="TariffElement2">Another tariff element.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (TariffElement TariffElement1, TariffElement TariffElement2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(TariffElement1, TariffElement2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) TariffElement1 == null) || ((Object) TariffElement2 == null))
                return false;

            return TariffElement1.Equals(TariffElement2);

        }

        #endregion

        #region Operator != (TariffElement1, TariffElement2)

        /// <summary>
        /// Compares two tariff elements for inequality.
        /// </summary>
        /// <param name="TariffElement1">A tariff element.</param>
        /// <param name="TariffElement2">Another tariff element.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (TariffElement TariffElement1, TariffElement TariffElement2)

            => !(TariffElement1 == TariffElement2);

        #endregion

        #endregion

        #region IEquatable<TariffElement> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is a tariff element.
            var TariffElement = Object as TariffElement;
            if ((Object) TariffElement == null)
                return false;

            return this.Equals(TariffElement);

        }

        #endregion

        #region Equals(TariffElement)

        /// <summary>
        /// Compares two tariff elements for equality.
        /// </summary>
        /// <param name="TariffElement">An tariff element to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(TariffElement TariffElement)
        {

            if ((Object) TariffElement == null)
                return false;

            return this.PriceComponent.   Equals(TariffElement.PriceComponent) &&
                   this.TariffRestriction.Equals(TariffElement.TariffRestriction);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return PriceComponent.   GetHashCode() * 11 ^
                       TariffRestriction.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(PriceComponent, ", ", TariffRestriction);

        #endregion

    }

}
