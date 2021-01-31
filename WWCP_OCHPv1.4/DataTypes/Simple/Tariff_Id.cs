/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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
    /// The unique identification of an OCHP tariff.
    /// </summary>
    public struct Tariff_Id : IId,
                              IEquatable <Tariff_Id>,
                              IComparable<Tariff_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a tariff identification.
        /// </summary>
        public static readonly Regex TariffId_RegEx  = new Regex(@"^([A-Za-z]{2}\*[A-Za-z0-9]{3})\*[Tt]([A-Za-z0-9][A-Za-z0-9\*]{0,9})$ |" +
                                                                 @"^([A-Za-z]{2}[A-Za-z0-9]{3})[Tt]([A-Za-z0-9][A-Za-z0-9\*]{0,9})$",
                                                                 RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for parsing an tariff identification suffix.
        /// </summary>
        public static readonly Regex IdSuffix_RegEx  = new Regex(@"^[A-Za-z0-9][A-Za-z0-9\*]{0,9}$",
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
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => OperatorId.Length + 2 + (UInt64) Suffix.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new charging tariff identification
        /// based on the given string.
        /// </summary>
        private Tariff_Id(ChargingStationOperator_Id  OperatorId,
                          String                      IdSuffix)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The given charging station operator identification must not be null!");

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix),    "The identification suffix must not be null or empty!");

            #endregion

            if (!IdSuffix_RegEx.IsMatch(IdSuffix))
                throw new ArgumentException("Illegal charging tariff identification '" + OperatorId + "' with suffix '" + IdSuffix + "'!");

            this.OperatorId  = OperatorId;
            this.Suffix      = IdSuffix;

        }

        #endregion


        #region Parse(EVSEId)

        /// <summary>
        /// Parse the given string as a charge point identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging tariff identification.</param>
        public static Tariff_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging tariff identification must not be null or empty!");

            #endregion

            var _MatchCollection = TariffId_RegEx.Matches(Text.Trim());

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal charging tariff identification '" + Text + "'!");

            ChargingStationOperator_Id _OperatorId;

            if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out _OperatorId))
                return new Tariff_Id(_OperatorId,
                                     _MatchCollection[0].Groups[2].Value);

            if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out _OperatorId))
                return new Tariff_Id(_OperatorId,
                                     _MatchCollection[0].Groups[4].Value);


            throw new ArgumentException("Illegal charging tariff identification '" + Text + "'!");

        }

        #endregion

        #region Parse(OperatorId, IdSuffix)

        /// <summary>
        /// Parse the given string as a charging tariff identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="IdSuffix">The suffix of the charging tariff identification.</param>
        public static Tariff_Id Parse(ChargingStationOperator_Id  OperatorId,
                                      String                      IdSuffix)

            => new Tariff_Id(OperatorId,
                             IdSuffix);

        #endregion

        #region TryParse(Text, out TariffId)

        /// <summary>
        /// Parse the given string as a charging tariff identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging tariff identification.</param>
        /// <param name="TariffId">The parsed charging tariff identification.</param>
        public static Boolean TryParse(String Text, out Tariff_Id TariffId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                TariffId = default(Tariff_Id);
                return false;
            }

            #endregion

            try
            {

                TariffId = default(Tariff_Id);

                var _MatchCollection = TariffId_RegEx.Matches(Text.Trim().ToUpper());

                if (_MatchCollection.Count != 1)
                    return false;

                ChargingStationOperator_Id _OperatorId;

                // New format...
                if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out _OperatorId))
                {

                    TariffId = new Tariff_Id(_OperatorId,
                                             _MatchCollection[0].Groups[2].Value);

                    return true;

                }

                // Old format...
                else if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out _OperatorId))
                {

                    TariffId = new Tariff_Id(_OperatorId,
                                             _MatchCollection[0].Groups[4].Value);

                    return true;

                }

            }
            catch (Exception e)
            { }

            TariffId = default(Tariff_Id);
            return false;

        }

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
            if (ReferenceEquals(TariffId1, TariffId2))
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

            if (!(Object  is Tariff_Id))
                throw new ArgumentException("The given object is not a charging tariff identification!", nameof(Object));

            return CompareTo((Tariff_Id) Object);

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

            // If equal: Compare charging operator identifications
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

            if (!(Object is Tariff_Id))
                return false;

            return this.Equals((Tariff_Id) Object);

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
            => OperatorId.GetHashCode() ^
               Suffix.    GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat(OperatorId, "*T", Suffix);

        #endregion

    }

}
