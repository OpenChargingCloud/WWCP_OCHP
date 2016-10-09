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
using System.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// An OCHP individual tariff.
    /// </summary>
    public class IndividualTariff
    {

        #region Properties

        /// <summary>
        /// Contains information about the pricing structure of the tariff element.
        /// </summary>
        public TariffElement        TariffElement   { get; }

        /// <summary>
        /// Identifies a recipient EMSP according to EMSP-ID without separators.
        /// If not provided, tariff element is considered the default tariff for
        /// this tariffId. Should never be returned by the CHS (i.e. only part
        /// of upload, not download).
        /// </summary>
        public IEnumerable<String>  Recipients      { get; }

        /// <summary>
        /// Contains information about the pricing structure of the tariff element.
        /// </summary>
        public String               Currency        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP individual tariff.
        /// </summary>
        /// <param name="TariffElement">Contains information about the pricing structure of the tariff element.</param>
        /// <param name="Recipients">Identifies a recipient EMSP according to EMSP-ID without separators. If not provided, tariff element is considered the default tariff for this tariffId. Should never be returned by the CHS (i.e. only part of upload, not download).</param>
        /// <param name="Currency">Contains information about the pricing structure of the tariff element.</param>
        public IndividualTariff(TariffElement        TariffElement,
                                IEnumerable<String>  Recipients,
                                String               Currency)
        {

            this.TariffElement  = TariffElement;
            this.Recipients     = Recipients;
            this.Currency       = Currency;

        }

        #endregion


        #region Operator overloading

        #region Operator == (IndividualTariff1, IndividualTariff2)

        /// <summary>
        /// Compares two individual tariffs for equality.
        /// </summary>
        /// <param name="IndividualTariff1">A individual tariff.</param>
        /// <param name="IndividualTariff2">Another individual tariff.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (IndividualTariff IndividualTariff1, IndividualTariff IndividualTariff2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(IndividualTariff1, IndividualTariff2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) IndividualTariff1 == null) || ((Object) IndividualTariff2 == null))
                return false;

            return IndividualTariff1.Equals(IndividualTariff2);

        }

        #endregion

        #region Operator != (IndividualTariff1, IndividualTariff2)

        /// <summary>
        /// Compares two individual tariffs for inequality.
        /// </summary>
        /// <param name="IndividualTariff1">A individual tariff.</param>
        /// <param name="IndividualTariff2">Another individual tariff.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (IndividualTariff IndividualTariff1, IndividualTariff IndividualTariff2)

            => !(IndividualTariff1 == IndividualTariff2);

        #endregion

        #endregion

        #region IEquatable<IndividualTariff> Members

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

            // Check if the given object is a individual tariff.
            var IndividualTariff = Object as IndividualTariff;
            if ((Object) IndividualTariff == null)
                return false;

            return this.Equals(IndividualTariff);

        }

        #endregion

        #region Equals(IndividualTariff)

        /// <summary>
        /// Compares two individual tariffs for equality.
        /// </summary>
        /// <param name="IndividualTariff">An individual tariff to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(IndividualTariff IndividualTariff)
        {

            if ((Object) IndividualTariff == null)
                return false;

            return this.TariffElement.     Equals(IndividualTariff.TariffElement) &&
                   this.Recipients.Count().Equals(IndividualTariff.Recipients.Count()) &&
                   this.Currency.          Equals(IndividualTariff.Currency);

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

                return TariffElement.GetHashCode() * 17 ^
                       Recipients.   GetHashCode() * 11 ^
                       Currency.     GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(TariffElement, " ", Currency, " for ", Recipients.AggregateWith(", "));

        #endregion

    }

}
