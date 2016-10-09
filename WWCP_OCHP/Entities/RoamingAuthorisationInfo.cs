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
    /// OCHP whitelisted authorisation card info.
    /// </summary>
    public class RoamingAuthorisationInfo
    {

        #region Properties

        /// <summary>
        /// Electrical vehicle contract identifier.
        /// </summary>
        public EMT_Id       EMTId           { get; }

        /// <summary>
        /// EMA-Id the token belongs to.
        /// </summary>
        public Contract_Id  ContractId      { get; }

        /// <summary>
        /// Tokens may be used until the date of expiry is reached. To be handled
        /// by the partners systems. Expired roaming authorisations may be erased
        /// locally by each partner's system.
        /// </summary>
        public DateTime     ExpiryDate      { get; }

        /// <summary>
        /// Might be used for manual authorisation.
        /// </summary>
        public String       PrintedNumber   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP whitelisted authorisation card info.
        /// </summary>
        /// <param name="EMTId">Electrical vehicle contract identifier.</param>
        /// <param name="ContractId">EMA-Id the token belongs to.</param>
        /// <param name="ExpiryDate">Tokens may be used until the date of expiry is reached. To be handled by the partners systems. Expired roaming authorisations may be erased locally by each partner's system.</param>
        /// <param name="PrintedNumber">Might be used for manual authorisation.</param>
        public RoamingAuthorisationInfo(EMT_Id       EMTId,
                                        Contract_Id  ContractId,
                                        DateTime     ExpiryDate,
                                        String       PrintedNumber = null)
        {

            this.EMTId          = EMTId;
            this.ContractId     = ContractId;
            this.ExpiryDate     = ExpiryDate;
            this.PrintedNumber  = PrintedNumber;

        }

        #endregion


        #region Operator overloading

        #region Operator == (RoamingAuthorisationInfo1, RoamingAuthorisationInfo2)

        /// <summary>
        /// Compares two roaming authorisation infos for equality.
        /// </summary>
        /// <param name="RoamingAuthorisationInfo1">A roaming authorisation info.</param>
        /// <param name="RoamingAuthorisationInfo2">Another roaming authorisation info.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RoamingAuthorisationInfo RoamingAuthorisationInfo1, RoamingAuthorisationInfo RoamingAuthorisationInfo2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(RoamingAuthorisationInfo1, RoamingAuthorisationInfo2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RoamingAuthorisationInfo1 == null) || ((Object) RoamingAuthorisationInfo2 == null))
                return false;

            return RoamingAuthorisationInfo1.Equals(RoamingAuthorisationInfo2);

        }

        #endregion

        #region Operator != (RoamingAuthorisationInfo1, RoamingAuthorisationInfo2)

        /// <summary>
        /// Compares two roaming authorisation infos for inequality.
        /// </summary>
        /// <param name="RoamingAuthorisationInfo1">A roaming authorisation info.</param>
        /// <param name="RoamingAuthorisationInfo2">Another roaming authorisation info.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RoamingAuthorisationInfo RoamingAuthorisationInfo1, RoamingAuthorisationInfo RoamingAuthorisationInfo2)

            => !(RoamingAuthorisationInfo1 == RoamingAuthorisationInfo2);

        #endregion

        #endregion

        #region IEquatable<RoamingAuthorisationInfo> Members

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

            // Check if the given object is a roaming authorisation info.
            var RoamingAuthorisationInfo = Object as RoamingAuthorisationInfo;
            if ((Object) RoamingAuthorisationInfo == null)
                return false;

            return this.Equals(RoamingAuthorisationInfo);

        }

        #endregion

        #region Equals(RoamingAuthorisationInfo)

        /// <summary>
        /// Compares two roaming authorisation infos for equality.
        /// </summary>
        /// <param name="RoamingAuthorisationInfo">An roaming authorisation info to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingAuthorisationInfo RoamingAuthorisationInfo)
        {

            if ((Object) RoamingAuthorisationInfo == null)
                return false;

            return this.EMTId.     Equals(RoamingAuthorisationInfo.EMTId) &&
                   this.ContractId.Equals(RoamingAuthorisationInfo.ContractId) &&
                   this.ExpiryDate.Equals(RoamingAuthorisationInfo.ExpiryDate);

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

                return EMTId.     GetHashCode() * 11 ^
                       ContractId.GetHashCode() *  7 ^
                       ExpiryDate.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EMTId, " / ", ContractId, PrintedNumber.IsNotNullOrEmpty() ? " (" + PrintedNumber + ")" : "", " expires ", ExpiryDate.ToIso8601());

        #endregion

    }

}
