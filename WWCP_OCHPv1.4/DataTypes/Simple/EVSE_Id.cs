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

using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// The unique identification of an OCHP charge point (EVSE).
    /// </summary>
    public readonly struct EVSE_Id : IId,
                                     IEquatable<EVSE_Id>,
                                     IComparable<EVSE_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charge point identification.
        /// </summary>
        public static readonly Regex EVSEId_RegEx = new Regex(@"^([A-Za-z]{2}\*[A-Za-z0-9]{3})\*[Ee]([A-Za-z0-9][A-Za-z0-9\*]{0,30})$ |" +
                                                              @"^([A-Za-z]{2}[A-Za-z0-9]{3})[Ee]([A-Za-z0-9][A-Za-z0-9\*]{0,30})$",
                                                              RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The charging station operator identification.
        /// </summary>
        public Operator_Id  OperatorId   { get; }

        /// <summary>
        /// The suffix of the charge point identification.
        /// </summary>
        public String       Suffix       { get; }

        /// <summary>
        /// The detected format of the charge point identification.
        /// </summary>
        public OperatorIdFormats Format
            => OperatorId.Format;

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

            => OperatorId.Length + 2 + (UInt64) Suffix.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a new Electric Vehicle Supply Equipment (EVSE) identification
        /// based on the given charging station operator and identification suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charge point identification.</param>
        private EVSE_Id(Operator_Id  OperatorId,
                        String       Suffix)
        {

            #region Initial checks

            if (Suffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Suffix), "The charge point identification suffix must not be null or empty!");

            #endregion

            this.OperatorId  = OperatorId;
            this.Suffix      = Suffix;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a charge point identification.
        /// </summary>
        /// <param name="Text">A text representation of a charge point identification.</param>
        public static EVSE_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text),  "The given text representation of a charge point identification must not be null or empty!");

            #endregion

            var MatchCollection = EVSEId_RegEx.Matches(Text);

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal charge point identification '" + Text + "'!",
                                            nameof(Text));

            Operator_Id _OperatorId;

            if (Operator_Id.TryParse(MatchCollection[0].Groups[1].Value, out _OperatorId))
                return new EVSE_Id(_OperatorId,
                                   MatchCollection[0].Groups[2].Value);

            if (Operator_Id.TryParse(MatchCollection[0].Groups[3].Value, out _OperatorId))
                return new EVSE_Id(_OperatorId,
                                   MatchCollection[0].Groups[4].Value);


            throw new ArgumentException("Illegal charge point identification '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region Parse(OperatorId, Suffix)

        /// <summary>
        /// Parse the given string as a charge point identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="Suffix">The suffix of the charge point identification.</param>
        public static EVSE_Id Parse(Operator_Id  OperatorId,
                                    String       Suffix)
        {

            switch (OperatorId.Format)
            {

                case OperatorIdFormats.ISO:
                    return Parse(OperatorId + "E" + Suffix);

                default: // ISO_STAR
                    return Parse(OperatorId + "*E" + Suffix);

            }

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Parse the given string as a charge point identification.
        /// </summary>
        /// <param name="Text">A text representation of a charge point identification.</param>
        public static EVSE_Id? TryParse(String Text)
        {

            EVSE_Id _EVSEId;

            if (TryParse(Text, out _EVSEId))
                return _EVSEId;

            return new EVSE_Id?();

        }

        #endregion

        #region TryParse(Text, out EVSEId)

        /// <summary>
        /// Parse the given string as a charge point identification.
        /// </summary>
        /// <param name="Text">A text representation of a charge point identification.</param>
        /// <param name="EVSEId">The parsed charge point identification.</param>
        public static Boolean TryParse(String Text, out EVSE_Id EVSEId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                EVSEId = default(EVSE_Id);
                return false;
            }

            #endregion

            try
            {

                EVSEId = default(EVSE_Id);

                var _MatchCollection = EVSEId_RegEx.Matches(Text.Trim().ToUpper());

                if (_MatchCollection.Count != 1)
                    return false;

                Operator_Id __EVSEOperatorId;

                // New format...
                if (Operator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                {

                    EVSEId = new EVSE_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[2].Value);

                    return true;

                }

                // Old format...
                if (Operator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                {

                    EVSEId = new EVSE_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[4].Value);

                    return true;

                }

            }
#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception e)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            EVSEId = default(EVSE_Id);
            return false;

        }

        #endregion

        #region Clone()

        /// <summary>
        /// Clone this charge point identification.
        /// </summary>
        public EVSE_Id Clone()

            => new (
                   OperatorId.Clone(),
                   Suffix.    CloneString()
               );

        #endregion


        #region Operator overloading

        #region Operator == (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A charge point identification.</param>
        /// <param name="EVSEId2">Another charge point identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EVSEId1, EVSEId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEId1 == null) || ((Object) EVSEId2 == null))
                return false;

            if ((Object) EVSEId1 == null)
                throw new ArgumentNullException(nameof(EVSEId1),  "The given charge point identification must not be null!");

            return EVSEId1.Equals(EVSEId2);

        }

        #endregion

        #region Operator != (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A charge point identification.</param>
        /// <param name="EVSEId2">Another charge point identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => !(EVSEId1 == EVSEId2);

        #endregion

        #region Operator <  (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A charge point identification.</param>
        /// <param name="EVSEId2">Another charge point identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
        {

            if ((Object) EVSEId1 == null)
                throw new ArgumentNullException(nameof(EVSEId1),  "The given charge point identification must not be null!");

            return EVSEId1.CompareTo(EVSEId2) < 0;

        }

        #endregion

        #region Operator <= (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A charge point identification.</param>
        /// <param name="EVSEId2">Another charge point identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => !(EVSEId1 > EVSEId2);

        #endregion

        #region Operator >  (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A charge point identification.</param>
        /// <param name="EVSEId2">Another charge point identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
        {

            if ((Object) EVSEId1 == null)
                throw new ArgumentNullException(nameof(EVSEId1),  "The given charge point identification must not be null!");

            return EVSEId1.CompareTo(EVSEId2) > 0;

        }

        #endregion

        #region Operator >= (EVSEId1, EVSEId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId1">A charge point identification.</param>
        /// <param name="EVSEId2">Another charge point identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EVSE_Id EVSEId1, EVSE_Id EVSEId2)
            => !(EVSEId1 < EVSEId2);

        #endregion

        #endregion

        #region IComparable<EVSEId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            if (!(Object is EVSE_Id))
                throw new ArgumentException("The given object is not an EVSEId!", nameof(Object));

            return CompareTo((EVSE_Id) Object);

        }

        #endregion

        #region CompareTo(EVSEId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EVSEId">An object to compare with.</param>
        public Int32 CompareTo(EVSE_Id EVSEId)
        {

            if ((Object) EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),  "The given charge point identification must not be null!");

            // Compare the length of the charge point identifications
            var _Result = Length.CompareTo(EVSEId.Length);

            // If equal: Compare charging operator identifications
            if (_Result == 0)
                _Result = OperatorId.CompareTo(EVSEId.OperatorId);

            // If equal: Compare charge point identification suffix
            if (_Result == 0)
                _Result = String.Compare(Suffix, EVSEId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<EVSEId> Members

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

            if (!(Object is EVSE_Id))
                return false;

            return Equals((EVSE_Id) Object);

        }

        #endregion

        #region Equals(EVSEId)

        /// <summary>
        /// Compares two charge point identifications for equality.
        /// </summary>
        /// <param name="EVSEId">A charge point identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSE_Id EVSEId)
        {

            if ((Object) EVSEId == null)
                return false;

            return OperatorId.Equals(EVSEId.OperatorId) &&
                   Suffix.    Equals(EVSEId.Suffix);

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
        {

            switch (Format)
            {

                case OperatorIdFormats.ISO:
                    return String.Concat(OperatorId, "E", Suffix);

                default: // ISO_STAR
                    return String.Concat(OperatorId, "*E", Suffix);

            }

        }

        #endregion

    }

}
