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
    /// The type of connector.
    /// </summary>
    public class ConnectorType
    {

        #region Properties

        /// <summary>
        /// The connector standard.
        /// </summary>
        public ConnectorStandards  Standard    { get; }

        /// <summary>
        /// The connector format.
        /// </summary>
        public ConnectorFormats    Format      { get; }

        /// <summary>
        /// References an optional tariff uploaded by the CPO to be used with this connector.
        /// </summary>
        public Tariff_Id           TariffId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP connector.
        /// </summary>
        /// <param name="Standard">The connector standard.</param>
        /// <param name="Format">The connector format.</param>
        /// <param name="TariffId">References an optional tariff uploaded by the CPO to be used with this connector.</param>
        public ConnectorType(ConnectorStandards  Standard,
                             ConnectorFormats    Format,
                             Tariff_Id           TariffId  = null)
        {

            this.Standard  = Standard;
            this.Format    = Format;
            this.TariffId  = TariffId;

        }

        #endregion


        #region Operator overloading

        #region Operator == (ConnectorType1, ConnectorType2)

        /// <summary>
        /// Compares two connectors for equality.
        /// </summary>
        /// <param name="ConnectorType1">A connector.</param>
        /// <param name="ConnectorType2">Another connector.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ConnectorType ConnectorType1, ConnectorType ConnectorType2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ConnectorType1, ConnectorType2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ConnectorType1 == null) || ((Object) ConnectorType2 == null))
                return false;

            return ConnectorType1.Equals(ConnectorType2);

        }

        #endregion

        #region Operator != (ConnectorType1, ConnectorType2)

        /// <summary>
        /// Compares two connectors for inequality.
        /// </summary>
        /// <param name="ConnectorType1">A connector.</param>
        /// <param name="ConnectorType2">Another connector.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ConnectorType ConnectorType1, ConnectorType ConnectorType2)

            => !(ConnectorType1 == ConnectorType2);

        #endregion

        #endregion

        #region IEquatable<ConnectorType> Members

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

            // Check if the given object is a connector.
            var ConnectorType = Object as ConnectorType;
            if ((Object) ConnectorType == null)
                return false;

            return this.Equals(ConnectorType);

        }

        #endregion

        #region Equals(ConnectorType)

        /// <summary>
        /// Compares two connectors for equality.
        /// </summary>
        /// <param name="ConnectorType">An connector to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ConnectorType ConnectorType)
        {

            if ((Object) ConnectorType == null)
                return false;

            return this.Standard.Equals(ConnectorType.Standard) &&
                   this.Format.  Equals(ConnectorType.Format);

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

                return Standard.GetHashCode() * 11 ^
                       Format.  GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Standard, " / ", Format, TariffId != null ? " with tariff " + TariffId : "");

        #endregion

    }

}
