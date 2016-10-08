/*
 * Copyright (c) 2014-2016 GraphDefined GmbH <achim.friedland@graphdefined.com>
 * This file is part of WWCP Core <https://github.com/OpenChargingCloud/WWCP_Core>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// The unique identification of an OCHP contract (also knwon as EVCO Id or eMA Id).
    /// </summary>
    public class Contract_Id : IId,
                               IEquatable <Contract_Id>,
                               IComparable<Contract_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a contract identification.
        /// </summary>
        public static readonly Regex ContractId_RegEx  = new Regex(@"^[A-Za-z]{2}-[A-Za-z0-9]{3}-[A-Za-z0-9]{9}-[A-Za-z0-9]$ |" +
                                                                   @"^[A-Za-z]{2}[A-Za-z0-9]{3}[A-Za-z0-9]{9}[A-Za-z0-9]$ |" +
                                                                   @"^[A-Za-z]{2}-[A-Za-z0-9]{3}-[A-Za-z0-9]{9}$ |" +
                                                                   @"^[A-Za-z]{2}[A-Za-z0-9]{3}[A-Za-z0-9]{9}$",
                                                                   RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for parsing an contract identification suffix.
        /// </summary>
        public static readonly Regex IdSuffix_RegEx  = new Regex(@"^[Tt][A-Za-z0-9][A-Za-z0-9\*]{0,9}$",
                                                                 RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The internal identification.
        /// </summary>
        public ChargingStationOperator_Id  OperatorId   { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String                      Suffix       { get; }

        /// <summary>
        /// Returns the length of the identificator.
        /// </summary>
        public UInt64 Length

            => OperatorId.Length + 1 + (UInt64) Suffix.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment (EVSE) identification
        /// based on the given string.
        /// </summary>
        private Contract_Id(ChargingStationOperator_Id   OperatorId,
                          String            IdSuffix)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The parameter must not be null!");

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix),    "The parameter must not be null or empty!");

            #endregion

            var _MatchCollection = IdSuffix_RegEx.Matches(IdSuffix.Trim());

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE identification '" + OperatorId.ToString() + "' with suffix '" + IdSuffix + "'!");

            this.OperatorId  = OperatorId;
            this.Suffix      = _MatchCollection[0].Value;

        }

        #endregion


        #region Parse(ContractId)

        /// <summary>
        /// Parse the given string as a contract identification.
        /// </summary>
        public static Contract_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The parameter must not be null or empty!");

            #endregion

            var _MatchCollection = ContractId_RegEx.Matches(Text.Trim().ToUpper());

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE identification '" + Text + "'!");

            ChargingStationOperator_Id __EVSEOperatorId = null;

            if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                return new Contract_Id(__EVSEOperatorId,
                                   _MatchCollection[0].Groups[2].Value);

            if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                return new Contract_Id(__EVSEOperatorId,
                                   _MatchCollection[0].Groups[4].Value);


            throw new ArgumentException("Illegal EVSE identification '" + Text + "'!");

        }

        #endregion

        #region Parse(OperatorId, IdSuffix)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        public static Contract_Id Parse(ChargingStationOperator_Id OperatorId, String IdSuffix)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The Charging Station Operator identification must not be null or empty!");

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix),    "The parameter must not be null or empty!");

            #endregion

            return Contract_Id.Parse(OperatorId.ToString() + "*" + IdSuffix);

        }

        #endregion

        #region TryParse(Text, out Contract_Id)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        public static Boolean TryParse(String Text, out Contract_Id ContractId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ContractId = null;
                return false;
            }

            #endregion

            try
            {

                ContractId = null;

                var _MatchCollection = ContractId_RegEx.Matches(Text.Trim().ToUpper());

                if (_MatchCollection.Count != 1)
                    return false;

                ChargingStationOperator_Id __EVSEOperatorId = null;

                // New format...
                if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                {

                    ContractId = new Contract_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[2].Value);

                    return true;

                }

                // Old format...
                else if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                {

                    ContractId = new Contract_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[4].Value);

                    return true;

                }

            }
            catch (Exception e)
            { }

            ContractId = null;
            return false;

        }

        #endregion

        #region TryParse(OperatorId, IdSuffix, out ContractId)

        ///// <summary>
        ///// Parse the given string as an EVSE identification.
        ///// </summary>
        //public static Boolean TryParse(EVSEOperator_Id OperatorId, String IdSuffix, out Contract_Id Contract_Id)
        //{

        //    try
        //    {
        //        Contract_Id = new Contract_Id(OperatorId, IdSuffix);
        //        return true;
        //    }
        //    catch (Exception e)
        //    { }

        //    Contract_Id = null;
        //    return false;

        //}

        #endregion

        #region Clone

        /// <summary>
        /// Clone this contract identification.
        /// </summary>
        public Contract_Id Clone

            => new Contract_Id(OperatorId.Clone,
                             new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (ContractId1, ContractId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ContractId1">A contract identification.</param>
        /// <param name="ContractId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Contract_Id ContractId1, Contract_Id ContractId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ContractId1, ContractId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ContractId1 == null) || ((Object) ContractId2 == null))
                return false;

            if ((Object) ContractId1 == null)
                throw new ArgumentNullException(nameof(ContractId1),  "The given contract identification must not be null!");

            return ContractId1.Equals(ContractId2);

        }

        #endregion

        #region Operator != (ContractId1, ContractId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ContractId1">A contract identification.</param>
        /// <param name="ContractId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Contract_Id ContractId1, Contract_Id ContractId2)
            => !(ContractId1 == ContractId2);

        #endregion

        #region Operator <  (ContractId1, ContractId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ContractId1">A contract identification.</param>
        /// <param name="ContractId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Contract_Id ContractId1, Contract_Id ContractId2)
        {

            if ((Object) ContractId1 == null)
                throw new ArgumentNullException(nameof(ContractId1),  "The given contract identification must not be null!");

            return ContractId1.CompareTo(ContractId2) < 0;

        }

        #endregion

        #region Operator <= (ContractId1, ContractId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ContractId1">A contract identification.</param>
        /// <param name="ContractId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Contract_Id ContractId1, Contract_Id ContractId2)
            => !(ContractId1 > ContractId2);

        #endregion

        #region Operator >  (ContractId1, ContractId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ContractId1">A contract identification.</param>
        /// <param name="ContractId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Contract_Id ContractId1, Contract_Id ContractId2)
        {

            if ((Object) ContractId1 == null)
                throw new ArgumentNullException(nameof(ContractId1),  "The given contract identification must not be null!");

            return ContractId1.CompareTo(ContractId2) > 0;

        }

        #endregion

        #region Operator >= (ContractId1, ContractId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ContractId1">A contract identification.</param>
        /// <param name="ContractId2">Another contract identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Contract_Id ContractId1, Contract_Id ContractId2)
            => !(ContractId1 < ContractId2);

        #endregion

        #endregion

        #region IComparable<Contract_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            // Check if the given object is a contract identification.
            var ContractId = Object as Contract_Id;
            if ((Object) ContractId == null)
                throw new ArgumentException("The given object is not a ContractId!", nameof(Object));

            return CompareTo(ContractId);

        }

        #endregion

        #region CompareTo(ContractId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ContractId">An object to compare with.</param>
        public Int32 CompareTo(Contract_Id ContractId)
        {

            if ((Object) ContractId == null)
                throw new ArgumentNullException(nameof(ContractId),  "The given contract identification must not be null!");

            // Compare the length of the contract identifications
            var _Result = this.Length.CompareTo(ContractId.Length);

            // If equal: Compare OperatorIds
            if (_Result == 0)
                _Result = OperatorId.CompareTo(ContractId.OperatorId);

            // If equal: Compare contract identification suffix
            if (_Result == 0)
                _Result = String.Compare(Suffix, ContractId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<Contract_Id> Members

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

            // Check if the given object is a contract identification.
            var ContractId = Object as Contract_Id;
            if ((Object) ContractId == null)
                return false;

            return this.Equals(ContractId);

        }

        #endregion

        #region Equals(ContractId)

        /// <summary>
        /// Compares two contract identifications for equality.
        /// </summary>
        /// <param name="ContractId">A contract identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Contract_Id ContractId)
        {

            if ((Object) ContractId == null)
                return false;

            return OperatorId.Equals(ContractId.OperatorId) &&
                   Suffix.    Equals(ContractId.Suffix);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => OperatorId.GetHashCode() ^ Suffix.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat(OperatorId, "-", Suffix);

        #endregion

    }

}
