﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// The different formats of charging station operator identifications.
    /// </summary>
    public enum OperatorIdFormats
    {

        /// <summary>
        /// The new ISO format.
        /// </summary>
        ISO,

        /// <summary>
        /// The new ISO format with a '*' as separator.
        /// </summary>
        ISO_STAR

    }


    /// <summary>
    /// The unique identification of an OCHP charging station operator.
    /// </summary>
    public readonly struct Operator_Id : IId,
                                         IEquatable<Operator_Id>,
                                         IComparable<Operator_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an OCHP charging station operator identification.
        /// </summary>
        public static readonly Regex  OperatorId_RegEx  = new Regex(@"^([A-Z]{2})(\*?)([A-Z0-9]{3})$",
                                                                    RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The country code.
        /// </summary>
        public Country            CountryCode   { get; }

        /// <summary>
        /// The identificator suffix.
        /// </summary>
        public String             Suffix        { get; }

        /// <summary>
        /// The format of the charging station operator identification.
        /// </summary>
        public OperatorIdFormats  Format        { get; }

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Suffix.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => Suffix.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
        {
            get
            {
                return Format switch {
                    OperatorIdFormats.ISO_STAR  => (UInt64) (CountryCode.Alpha2Code.Length + 1 + Suffix.Length),
                    // ISO
                    _                           => (UInt64) (CountryCode.Alpha2Code.Length +     Suffix.Length),
                };
            }
        }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charging station operator identification.
        /// </summary>
        /// <param name="CountryCode">The country code.</param>
        /// <param name="Suffix">The suffix of the charging station operator identification.</param>
        /// <param name="Format">The format of the charging station operator identification.</param>
        private Operator_Id(Country            CountryCode,
                            String             Suffix,
                            OperatorIdFormats  Format = OperatorIdFormats.ISO)
        {

            this.CountryCode  = CountryCode;
            this.Suffix       = Suffix;
            this.Format       = Format;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given text representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator identification.</param>
        public static Operator_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty() || Text.Trim().IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a charging station operator identification must not be null or empty!");

            #endregion

            var MatchCollection = OperatorId_RegEx.Matches(Text.Trim().ToUpper());

            if (MatchCollection.Count == 1 &&
                Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out Country _CountryCode))
            {

                return new Operator_Id(_CountryCode,
                                       MatchCollection[0].Groups[3].Value,
                                       MatchCollection[0].Groups[2].Value == "*" ? OperatorIdFormats.ISO_STAR : OperatorIdFormats.ISO);

            }

            throw new ArgumentException("Illegal text representation of a charging station operator identification: '{Text}'!", nameof(Text));

        }

        #endregion

        #region Parse(CountryCode, Suffix, IdFormat = IdFormatType.ISO)

        /// <summary>
        /// Parse the given string as an charging station operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an charging station operator identification.</param>
        /// <param name="IdFormat">The format of the charging station operator identification [old|new].</param>
        public static Operator_Id Parse(Country            CountryCode,
                                        String             Suffix,
                                        OperatorIdFormats  IdFormat = OperatorIdFormats.ISO)
        {

            #region Initial checks

            if (CountryCode == null)
                throw new ArgumentNullException(nameof(CountryCode),  "The given country must not be null!");

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix),       "The given charging station operator identification suffix must not be null or empty!");

            #endregion

            switch (IdFormat)
            {

                case OperatorIdFormats.ISO:
                    return Parse(CountryCode.Alpha2Code +       Suffix);

                default:
                    return Parse(CountryCode.Alpha2Code + "*" + Suffix);

            }

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given text representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator identification.</param>
        public static Operator_Id? TryParse(String Text)
        {

            if (TryParse(Text, out Operator_Id _OperatorId))
                return _OperatorId;

            return new Operator_Id?();

        }

        #endregion

        #region TryParse(Text, out OperatorId)

        /// <summary>
        /// Try to parse the given text representation of a charging station operator identification.
        /// </summary>
        /// <param name="Text">A text representation of a charging station operator identification.</param>
        /// <param name="OperatorId">The parsed charging station operator identification.</param>
        public static Boolean TryParse(String           Text,
                                       out Operator_Id  OperatorId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                OperatorId = default(Operator_Id);
                return false;
            }

            #endregion

            try
            {

                var MatchCollection = OperatorId_RegEx.Matches(Text.Trim().ToUpper());

                if (MatchCollection.Count == 1 &&
                    Country.TryParseAlpha2Code(MatchCollection[0].Groups[1].Value, out Country _CountryCode))
                {

                    OperatorId = new Operator_Id(_CountryCode,
                                                 MatchCollection[0].Groups[3].Value,
                                                 MatchCollection[0].Groups[2].Value == "*" ? OperatorIdFormats.ISO_STAR : OperatorIdFormats.ISO);

                    return true;

                }

            }

#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            OperatorId = default(Operator_Id);
            return false;

        }

        #endregion

        #region TryParse(CountryCode, Suffix, IdFormat = OperatorIdFormats.ISO_STAR)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility operator identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility operator identification.</param>
        public static Operator_Id? TryParse(Country            CountryCode,
                                            String             Suffix,
                                            OperatorIdFormats  IdFormat = OperatorIdFormats.ISO_STAR)
        {

            if (TryParse(CountryCode, Suffix, out Operator_Id _OperatorId, IdFormat))
                return _OperatorId;

            return new Operator_Id?();

        }

        #endregion

        #region TryParse(CountryCode, Suffix, out OperatorId, IdFormat = OperatorIdFormats.ISO_STAR)

        /// <summary>
        /// Try to parse the given text representation of an e-mobility operator identification.
        /// </summary>
        /// <param name="CountryCode">A country code.</param>
        /// <param name="Suffix">The suffix of an e-mobility operator identification.</param>
        /// <param name="OperatorId">The parsed e-mobility operator identification.</param>
        /// <param name="IdFormat">The optional format of the e-mobility operator identification.</param>
        public static Boolean TryParse(Country            CountryCode,
                                       String             Suffix,
                                       out Operator_Id    OperatorId,
                                       OperatorIdFormats  IdFormat = OperatorIdFormats.ISO_STAR)
        {

            #region Initial checks

            if (CountryCode == null || Suffix.IsNullOrEmpty() || Suffix.Trim().IsNullOrEmpty())
            {
                OperatorId = default(Operator_Id);
                return false;
            }

            #endregion

            switch (IdFormat)
            {

                case OperatorIdFormats.ISO:
                    return TryParse(CountryCode.Alpha2Code +       Suffix,
                                    out OperatorId);

                default: // ISO_STAR:
                    return TryParse(CountryCode.Alpha2Code + "*" + Suffix,
                                    out OperatorId);

            }

        }

        #endregion

        #region ChangeFormat(NewFormat)

        /// <summary>
        /// Return a new charging station operator identification in the given format.
        /// </summary>
        /// <param name="NewFormat">The new charging station operator identification format.</param>
        public Operator_Id ChangeFormat(OperatorIdFormats NewFormat)

            => new Operator_Id(CountryCode,
                               Suffix,
                               NewFormat);

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this charging station operator identification.
        /// </summary>
        public Operator_Id Clone()

            => new (
                   CountryCode.Clone(),
                   Suffix.     CloneString(),
                   Format
               );

        #endregion


        #region Operator overloading

        #region Operator == (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Operator_Id OperatorId1, Operator_Id OperatorId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(OperatorId1, OperatorId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) OperatorId1 == null) || ((Object) OperatorId2 == null))
                return false;

            return OperatorId1.Equals(OperatorId2);

        }

        #endregion

        #region Operator != (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Operator_Id OperatorId1, Operator_Id OperatorId2)
            => !(OperatorId1 == OperatorId2);

        #endregion

        #region Operator <  (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Operator_Id OperatorId1, Operator_Id OperatorId2)
        {

            if ((Object) OperatorId1 == null)
                throw new ArgumentNullException(nameof(OperatorId1), "The given OperatorId1 must not be null!");

            return OperatorId1.CompareTo(OperatorId2) < 0;

        }

        #endregion

        #region Operator <= (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Operator_Id OperatorId1, Operator_Id OperatorId2)
            => !(OperatorId1 > OperatorId2);

        #endregion

        #region Operator >  (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Operator_Id OperatorId1, Operator_Id OperatorId2)
        {

            if ((Object) OperatorId1 == null)
                throw new ArgumentNullException(nameof(OperatorId1), "The given OperatorId1 must not be null!");

            return OperatorId1.CompareTo(OperatorId2) > 0;

        }

        #endregion

        #region Operator >= (OperatorId1, OperatorId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId1">An charging station operator identification.</param>
        /// <param name="OperatorId2">Another charging station operator identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Operator_Id OperatorId1, Operator_Id OperatorId2)
            => !(OperatorId1 < OperatorId2);

        #endregion

        #endregion

        #region IComparable<OperatorId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object), "The given object must not be null!");

            if (!(Object is Operator_Id))
                throw new ArgumentException("The given object is not a charging station operator identification!", nameof(Object));

            return CompareTo((Operator_Id) Object);

        }

        #endregion

        #region CompareTo(OperatorId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="OperatorId">An object to compare with.</param>
        public Int32 CompareTo(Operator_Id OperatorId)
        {

            if ((Object) OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId), "The given charging station operator identification must not be null!");

            // Compare the length of the OperatorIds
            var _Result = this.Length.CompareTo(OperatorId.Length);

            // If equal: Compare country codes
            if (_Result == 0)
                _Result = CountryCode.CompareTo(OperatorId.CountryCode);

            // If equal: Compare provider ids
            if (_Result == 0)
                _Result = String.Compare(Suffix, OperatorId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<OperatorId> Members

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

            if (!(Object is Operator_Id))
                return false;

            return Equals((Operator_Id) Object);

        }

        #endregion

        #region Equals(OperatorId)

        /// <summary>
        /// Compares two OperatorIds for equality.
        /// </summary>
        /// <param name="OperatorId">A OperatorId to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Operator_Id OperatorId)
        {

            if ((Object) OperatorId == null)
                return false;

            return CountryCode.Equals(OperatorId.CountryCode) &&
                   Suffix.     Equals(OperatorId.Suffix);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => CountryCode.GetHashCode() ^
               Suffix.     GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()
        {

            switch (Format)
            {

                case OperatorIdFormats.ISO_STAR:
                    return CountryCode.Alpha2Code + "*" + Suffix;

                default: // ISO
                    return CountryCode.Alpha2Code       + Suffix;

            }

        }

        #endregion

        #region ToString(Format)

        /// <summary>
        /// Return the identification in the given format.
        /// </summary>
        /// <param name="Format">The format of the identification.</param>
        public String ToString(OperatorIdFormats Format)
        {

            switch (Format)
            {

                case OperatorIdFormats.ISO:
                    return String.Concat(CountryCode.Alpha2Code,
                                         Suffix);

                case OperatorIdFormats.ISO_STAR:
                    return String.Concat(CountryCode.Alpha2Code,
                                         "*",
                                         Suffix);

                default: // DIN
                    return String.Concat("+",
                                         CountryCode.TelefonCode,
                                         "*",
                                         Suffix);

            }

        }

        #endregion

    }

}
