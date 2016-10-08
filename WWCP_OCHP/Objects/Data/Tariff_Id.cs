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
    /// The unique identification of an OCHP tariff.
    /// </summary>
    public class Tariff_Id : IId,
                             IEquatable <Tariff_Id>,
                             IComparable<Tariff_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a tariff identification.
        /// </summary>
        public static readonly Regex TariffId_RegEx  = new Regex(@"^[A-Za-z]{2}\*[A-Za-z0-9]{3}\*[Tt][A-Za-z0-9][A-Za-z0-9\*]{0,9}$ | ^[A-Za-z]{2}[A-Za-z0-9]{3}[Tt][A-Za-z0-9][A-Za-z0-9\*]{0,9}$",
                                                                 RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for parsing an tariff identification suffix.
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

            => OperatorId.Length + 2 + (UInt64) Suffix.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment (EVSE) identification
        /// based on the given string.
        /// </summary>
        private Tariff_Id(ChargingStationOperator_Id   OperatorId,
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


        #region Parse(TariffId)

        /// <summary>
        /// Parse the given string as a tariff identification.
        /// </summary>
        public static Tariff_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The parameter must not be null or empty!");

            #endregion

            var _MatchCollection = TariffId_RegEx.Matches(Text.Trim().ToUpper());

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE identification '" + Text + "'!");

            ChargingStationOperator_Id __EVSEOperatorId = null;

            if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                return new Tariff_Id(__EVSEOperatorId,
                                   _MatchCollection[0].Groups[2].Value);

            if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                return new Tariff_Id(__EVSEOperatorId,
                                   _MatchCollection[0].Groups[4].Value);


            throw new ArgumentException("Illegal EVSE identification '" + Text + "'!");

        }

        #endregion

        #region Parse(OperatorId, IdSuffix)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        public static Tariff_Id Parse(ChargingStationOperator_Id OperatorId, String IdSuffix)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The Charging Station Operator identification must not be null or empty!");

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix),    "The parameter must not be null or empty!");

            #endregion

            return Tariff_Id.Parse(OperatorId.ToString() + "*" + IdSuffix);

        }

        #endregion

        #region TryParse(Text, out Tariff_Id)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        public static Boolean TryParse(String Text, out Tariff_Id TariffId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                TariffId = null;
                return false;
            }

            #endregion

            try
            {

                TariffId = null;

                var _MatchCollection = TariffId_RegEx.Matches(Text.Trim().ToUpper());

                if (_MatchCollection.Count != 1)
                    return false;

                ChargingStationOperator_Id __EVSEOperatorId = null;

                // New format...
                if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                {

                    TariffId = new Tariff_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[2].Value);

                    return true;

                }

                // Old format...
                else if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                {

                    TariffId = new Tariff_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[4].Value);

                    return true;

                }

            }
            catch (Exception e)
            { }

            TariffId = null;
            return false;

        }

        #endregion

        #region TryParse(OperatorId, IdSuffix, out TariffId)

        ///// <summary>
        ///// Parse the given string as an EVSE identification.
        ///// </summary>
        //public static Boolean TryParse(EVSEOperator_Id OperatorId, String IdSuffix, out Tariff_Id Tariff_Id)
        //{

        //    try
        //    {
        //        Tariff_Id = new Tariff_Id(OperatorId, IdSuffix);
        //        return true;
        //    }
        //    catch (Exception e)
        //    { }

        //    Tariff_Id = null;
        //    return false;

        //}

        #endregion

        #region Clone

        /// <summary>
        /// Clone this tariff identification.
        /// </summary>
        public Tariff_Id Clone

            => new Tariff_Id(OperatorId.Clone,
                             new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (TariffId1, TariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TariffId1">A tariff identification.</param>
        /// <param name="TariffId2">Another tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Tariff_Id TariffId1, Tariff_Id TariffId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(TariffId1, TariffId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) TariffId1 == null) || ((Object) TariffId2 == null))
                return false;

            if ((Object) TariffId1 == null)
                throw new ArgumentNullException(nameof(TariffId1),  "The given tariff identification must not be null!");

            return TariffId1.Equals(TariffId2);

        }

        #endregion

        #region Operator != (TariffId1, TariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TariffId1">A tariff identification.</param>
        /// <param name="TariffId2">Another tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Tariff_Id TariffId1, Tariff_Id TariffId2)
            => !(TariffId1 == TariffId2);

        #endregion

        #region Operator <  (TariffId1, TariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TariffId1">A tariff identification.</param>
        /// <param name="TariffId2">Another tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Tariff_Id TariffId1, Tariff_Id TariffId2)
        {

            if ((Object) TariffId1 == null)
                throw new ArgumentNullException(nameof(TariffId1),  "The given tariff identification must not be null!");

            return TariffId1.CompareTo(TariffId2) < 0;

        }

        #endregion

        #region Operator <= (TariffId1, TariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TariffId1">A tariff identification.</param>
        /// <param name="TariffId2">Another tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Tariff_Id TariffId1, Tariff_Id TariffId2)
            => !(TariffId1 > TariffId2);

        #endregion

        #region Operator >  (TariffId1, TariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TariffId1">A tariff identification.</param>
        /// <param name="TariffId2">Another tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Tariff_Id TariffId1, Tariff_Id TariffId2)
        {

            if ((Object) TariffId1 == null)
                throw new ArgumentNullException(nameof(TariffId1),  "The given tariff identification must not be null!");

            return TariffId1.CompareTo(TariffId2) > 0;

        }

        #endregion

        #region Operator >= (TariffId1, TariffId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TariffId1">A tariff identification.</param>
        /// <param name="TariffId2">Another tariff identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Tariff_Id TariffId1, Tariff_Id TariffId2)
            => !(TariffId1 < TariffId2);

        #endregion

        #endregion

        #region IComparable<Tariff_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            // Check if the given object is a tariff identification.
            var TariffId = Object as Tariff_Id;
            if ((Object) TariffId == null)
                throw new ArgumentException("The given object is not a TariffId!", nameof(Object));

            return CompareTo(TariffId);

        }

        #endregion

        #region CompareTo(TariffId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="TariffId">An object to compare with.</param>
        public Int32 CompareTo(Tariff_Id TariffId)
        {

            if ((Object) TariffId == null)
                throw new ArgumentNullException(nameof(TariffId),  "The given tariff identification must not be null!");

            // Compare the length of the tariff identifications
            var _Result = this.Length.CompareTo(TariffId.Length);

            // If equal: Compare OperatorIds
            if (_Result == 0)
                _Result = OperatorId.CompareTo(TariffId.OperatorId);

            // If equal: Compare tariff identification suffix
            if (_Result == 0)
                _Result = String.Compare(Suffix, TariffId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<Tariff_Id> Members

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

            // Check if the given object is a tariff identification.
            var TariffId = Object as Tariff_Id;
            if ((Object) TariffId == null)
                return false;

            return this.Equals(TariffId);

        }

        #endregion

        #region Equals(TariffId)

        /// <summary>
        /// Compares two tariff identifications for equality.
        /// </summary>
        /// <param name="TariffId">A tariff identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Tariff_Id TariffId)
        {

            if ((Object) TariffId == null)
                return false;

            return OperatorId.Equals(TariffId.OperatorId) &&
                   Suffix.    Equals(TariffId.Suffix);

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
            => String.Concat(OperatorId, "*T", Suffix);

        #endregion

    }

}
