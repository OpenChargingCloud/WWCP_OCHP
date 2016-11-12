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
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// The unique identification of an OCHP charge point.
    /// </summary>
    public struct EVSE_Id : IId,
                            IEquatable <EVSE_Id>,
                            IComparable<EVSE_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charge point identification.
        /// </summary>
        public static readonly Regex EVSEId_RegEx    = new Regex(@"^([A-Za-z]{2}\*[A-Za-z0-9]{3})\*[Ee]([A-Za-z0-9][A-Za-z0-9\*]{0,30})$ |" +
                                                                 @"^([A-Za-z]{2}[A-Za-z0-9]{3})[Ee]([A-Za-z0-9][A-Za-z0-9\*]{0,30})$",
                                                                 RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for parsing an charge point identification suffix.
        /// </summary>
        public static readonly Regex IdSuffix_RegEx  = new Regex(@"^[A-Za-z0-9][A-Za-z0-9\*]{0,30}$",
                                                                 RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The charging station operator identification.
        /// </summary>
        public ChargingStationOperator_Id  OperatorId   { get; }

        /// <summary>
        /// The suffix of the EVSE identification.
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
        /// Generate a new EVSE identification based on the given
        /// charging station operator and identification suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="IdSuffix">The suffix of the EVSE identification.</param>
        private EVSE_Id(ChargingStationOperator_Id  OperatorId,
                        String                      IdSuffix)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The given charging station operator identification must not be null!");

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix),    "The identification suffix must not be null or empty!");

            #endregion

            if (!IdSuffix_RegEx.IsMatch(IdSuffix))
                throw new ArgumentException("Illegal EVSE identification '" + OperatorId + "' with suffix '" + IdSuffix + "'!");

            this.OperatorId  = OperatorId;
            this.Suffix      = IdSuffix;

        }

        #endregion


        #region Parse(EVSEId)

        /// <summary>
        /// Parse the given string as a charge point identification.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE identification.</param>
        public static EVSE_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of an EVSE identification must not be null or empty!");

            #endregion

            var MatchCollection = EVSEId_RegEx.Matches(Text.Trim());

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE identification '" + Text + "'!");

            ChargingStationOperator_Id _OperatorId;

            if (ChargingStationOperator_Id.TryParse(MatchCollection[0].Groups[1].Value, out _OperatorId))
                return new EVSE_Id(_OperatorId,
                                   MatchCollection[0].Groups[2].Value);

            if (ChargingStationOperator_Id.TryParse(MatchCollection[0].Groups[3].Value, out _OperatorId))
                return new EVSE_Id(_OperatorId,
                                   MatchCollection[0].Groups[4].Value);


            throw new ArgumentException("Illegal EVSE identification '" + Text + "'!");

        }

        #endregion

        #region Parse(OperatorId, IdSuffix)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="IdSuffix">The suffix of the EVSE identification.</param>
        public static EVSE_Id Parse(ChargingStationOperator_Id  OperatorId,
                                    String                      IdSuffix)

            => new EVSE_Id(OperatorId,
                           IdSuffix);

        #endregion

        #region TryParse(Text, out EVSEId)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        /// <param name="Text">A text representation of an EVSE identification.</param>
        /// <param name="EVSEId">The parsed EVSE identification.</param>
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

                ChargingStationOperator_Id __EVSEOperatorId;

                // New format...
                if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                {

                    EVSEId = new EVSE_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[2].Value);

                    return true;

                }

                // Old format...
                else if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                {

                    EVSEId = new EVSE_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[4].Value);

                    return true;

                }

            }
            catch (Exception e)
            { }

            EVSEId = default(EVSE_Id);
            return false;

        }

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
            if (Object.ReferenceEquals(EVSEId1, EVSEId2))
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

            // Check if the given object is a charge point identification.
            if (!(Object is EVSE_Id))
                throw new ArgumentException("The given object is not a EVSEId!", nameof(Object));

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
            var _Result = this.Length.CompareTo(EVSEId.Length);

            // If equal: Compare OperatorIds
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

            // Check if the given object is a charge point identification.
            if (!(Object is EVSE_Id))
                return false;

            return this.Equals((EVSE_Id) Object);

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat(OperatorId, "*E", Suffix);

        #endregion

    }

}
