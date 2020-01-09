/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// The unique identification of an OCHP contract (also knwon as EVCO Id or eMA Id).
    /// </summary>
    public struct Contract_Id : IId,
                                IEquatable <Contract_Id>,
                                IComparable<Contract_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an e-mobility contract identification.
        /// </summary>
        public static readonly Regex ContractId_RegEx  = new Regex("^([A-Za-z]{2}-[A-Za-z0-9]{3})-([A-Za-z0-9]{9})-([A-Za-z0-9])$ |" +
                                                                   "^([A-Za-z]{2}[A-Za-z0-9]{3})([A-Za-z0-9]{9})([A-Za-z0-9])$ |" +
                                                                   "^([A-Za-z]{2}-[A-Za-z0-9]{3})-([A-Za-z0-9]{9})$ |" +
                                                                   "^([A-Za-z]{2}[A-Za-z0-9]{3})([A-Za-z0-9]{9})$",
                                                                   RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The e-mobility provider identification.
        /// </summary>
        public Provider_Id           ProviderId    { get; }

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String                Suffix        { get; }

        /// <summary>
        /// An optional check digit of the e-mobility contract identification.
        /// </summary>
        public Char?                 CheckDigit    { get; }

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length

            => ProviderId.Length +
               1UL +
               (UInt64) Suffix.Length +
               (CheckDigit.HasValue ? 2UL : 0UL);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new e-mobility contract identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="IdSuffix">The suffix of the e-mobility contract identification.</param>
        /// <param name="CheckDigit">An optional check digit of the e-mobility contract identification.</param>
        private Contract_Id(Provider_Id  ProviderId,
                            String       IdSuffix,
                            Char?        CheckDigit = null)
        {

            #region Initial checks

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix),  "The identification suffix must not be null or empty!");

            #endregion

            this.ProviderId  = ProviderId;
            this.Suffix      = IdSuffix;
            this.CheckDigit  = CheckDigit;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a contract identification.
        /// </summary>
        public static Contract_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty() || Text.Trim().IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a contract identification must not be null or empty!");

            #endregion

            var _MatchCollection = ContractId_RegEx.Matches(Text.Trim().ToUpper());

            if (_MatchCollection.Count == 1)
            {

                Provider_Id _ProviderId;

                if (Provider_Id.TryParse(_MatchCollection[0].Groups[1].Value, out _ProviderId))
                    return new Contract_Id(_ProviderId,
                                           _MatchCollection[0].Groups[2].Value,
                                           _MatchCollection[0].Groups[3].Value[0]);

                if (Provider_Id.TryParse(_MatchCollection[0].Groups[4].Value, out _ProviderId))
                    return new Contract_Id(_ProviderId,
                                           _MatchCollection[0].Groups[5].Value,
                                           _MatchCollection[0].Groups[6].Value[0]);

                if (Provider_Id.TryParse(_MatchCollection[0].Groups[7].Value, out _ProviderId))
                    return new Contract_Id(_ProviderId,
                                           _MatchCollection[0].Groups[8].Value);

                if (Provider_Id.TryParse(_MatchCollection[0].Groups[9].Value, out _ProviderId))
                    return new Contract_Id(_ProviderId,
                                           _MatchCollection[0].Groups[10].Value);

            }

            throw new ArgumentException("Illegal contract identification '" + Text + "'!");

        }

        #endregion

        #region Parse(ProviderId, Suffix)

        /// <summary>
        /// Parse the given string as an contract identification.
        /// </summary>
        /// <param name="ProviderId">The unique identification of an e-mobility provider.</param>
        /// <param name="Suffix">The suffix of the e-mobility contract identification.</param>
        public static Contract_Id Parse(Provider_Id  ProviderId,
                                        String       Suffix)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The given electric mobility account identification suffix must not be null or empty!");

            #endregion

            switch (ProviderId.Format)
            {

                case ProviderIdFormats.ISO:
                    return Parse(ProviderId +       Suffix);

                default: // ISO_HYPHEN
                    return Parse(ProviderId + "-" + Suffix);

            }

        }

        #endregion

        #region TryParse(Text, out Contract_Id)

        /// <summary>
        /// Parse the given string as an e-mobility contract identification.
        /// </summary>
        /// <param name="Text">A text representation of an e-mobility contract identification.</param>
        /// <param name="ContractId">The parsed e-mobility contract identification.</param>
        public static Boolean TryParse(String Text, out Contract_Id ContractId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ContractId = default(Contract_Id);
                return false;
            }

            #endregion

            try
            {

                ContractId = default(Contract_Id);

                var _MatchCollection = ContractId_RegEx.Matches(Text.Trim().ToUpper());

                if (_MatchCollection.Count != 1)
                    return false;

                Provider_Id _Provider;

                // New format...
                if (Provider_Id.TryParse(_MatchCollection[0].Groups[1].Value, out _Provider))
                {

                    ContractId = new Contract_Id(_Provider,
                                                 _MatchCollection[0].Groups[2].Value,
                                                 _MatchCollection[0].Groups[3].Value[0]);

                    return true;

                }

                // Old format...
                else if (Provider_Id.TryParse(_MatchCollection[0].Groups[4].Value, out _Provider))
                {

                    ContractId = new Contract_Id(_Provider,
                                                 _MatchCollection[0].Groups[5].Value,
                                                 _MatchCollection[0].Groups[6].Value[0]);

                    return true;

                }

                // Without check digit...
                else if (Provider_Id.TryParse(_MatchCollection[0].Groups[7].Value, out _Provider))
                {

                    ContractId = new Contract_Id(_Provider,
                                                 _MatchCollection[0].Groups[8].Value);

                    return true;

                }

            }
            catch (Exception e)
            { }

            ContractId = default(Contract_Id);
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this EVSE identification.
        /// </summary>
        public Contract_Id Clone

            => new Contract_Id(ProviderId.Clone,
                               Suffix);

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

        #region IComparable<ContractId> Members

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
            if (!(Object is Contract_Id))
                throw new ArgumentException("The given object is not a ContractId!", nameof(Object));

            return CompareTo((Contract_Id) Object);

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

            // If equal: Compare charging operator identifications
            if (_Result == 0)
                _Result = ProviderId.CompareTo(ContractId.ProviderId);

            // If equal: Compare contract identification suffix
            if (_Result == 0)
                _Result = String.Compare(Suffix, ContractId.Suffix, StringComparison.Ordinal);

            // If equal: Compare contract check digit
            if (_Result == 0)
            {

                if (!CheckDigit.HasValue && !ContractId.CheckDigit.HasValue)
                    _Result = 0;

                if ( CheckDigit.HasValue &&  ContractId.CheckDigit.HasValue)
                    _Result = CheckDigit.Value.CompareTo(ContractId.CheckDigit.Value);

            }

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ContractId> Members

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
            if (!(Object is Contract_Id))
                return false;

            return this.Equals((Contract_Id) Object);

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

            return ProviderId.Equals(ContractId.ProviderId) &&
                   Suffix.    Equals(ContractId.Suffix)     &&

                   ((!CheckDigit.HasValue && !ContractId.CheckDigit.HasValue) ||
                     (CheckDigit.HasValue &&  ContractId.CheckDigit.HasValue && CheckDigit.Value.Equals(ContractId.CheckDigit.Value)));

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return ProviderId.GetHashCode() * 7 ^
                       Suffix.    GetHashCode() * 5 ^

                       (CheckDigit.HasValue
                            ? CheckDigit.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {

            switch (ProviderId.Format)
            {

                case ProviderIdFormats.ISO:
                    return String.Concat(ProviderId,
                                         //"C",
                                         Suffix,
                                         CheckDigit.HasValue
                                             ? "" + CheckDigit
                                             : "");

                default: // ISO_HYPHEN
                    return String.Concat(ProviderId, "-",
                                         //"C",
                                         Suffix,
                                         CheckDigit.HasValue
                                             ? "-" + CheckDigit
                                             : "");

            }

        }

        #endregion

    }

}
