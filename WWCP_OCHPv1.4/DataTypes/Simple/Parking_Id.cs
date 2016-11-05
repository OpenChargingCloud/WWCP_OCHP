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
    /// The unique identification of an OCHP parking spot.
    /// </summary>
    public struct Parking_Id : IId,
                               IEquatable <Parking_Id>,
                               IComparable<Parking_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a parking identification.
        /// </summary>
        public static readonly Regex ParkingId_RegEx  = new Regex(@"^([A-Za-z]{2}\*[A-Za-z0-9]{3})\*[Pp]([A-Za-z0-9][A-Za-z0-9\*]{0,30})$ | ^([A-Za-z]{2}[A-Za-z0-9]{3})[Pp]([A-Za-z0-9][A-Za-z0-9\*]{0,30})$",
                                                                  RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for parsing a parking identification suffix.
        /// </summary>
        public static readonly Regex IdSuffix_RegEx   = new Regex(@"^[A-Za-z0-9][A-Za-z0-9\*]{0,30}$",
                                                                  RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The charging station operator identification.
        /// </summary>
        public ChargingStationOperator_Id  OperatorId   { get; }

        /// <summary>
        /// The suffix of the parking spot identification.
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
        /// Generate a new parking spot identification based on the given
        /// charging station operator and identification suffix.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="IdSuffix">The suffix of the parking spot identification.</param>
        private Parking_Id(ChargingStationOperator_Id  OperatorId,
                           String                      IdSuffix)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The charging station operator identification must not be null!");

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix),    "The parking identification suffix must not be null or empty!");

            #endregion

            if (!IdSuffix_RegEx.IsMatch(IdSuffix))
                throw new ArgumentException("Illegal parking spot identification '" + OperatorId + "' with suffix '" + IdSuffix + "'!");

            this.OperatorId  = OperatorId;
            this.Suffix      = IdSuffix;

        }

        #endregion


        #region Parse(Text)

        /// <summary>
        /// Parse the given string as a parking spot identification.
        /// </summary>
        /// <param name="Text">A text representation of a parking spot identification.</param>
        public static Parking_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a parking identification must not be null or empty!");

            #endregion

            var MatchCollection = ParkingId_RegEx.Matches(Text.Trim());

            if (MatchCollection.Count != 1)
                throw new ArgumentException("Illegal parking identification '" + Text + "'!");

            ChargingStationOperator_Id ParkingOperatorId = null;

            if (ChargingStationOperator_Id.TryParse(MatchCollection[0].Groups[1].Value, out ParkingOperatorId))
                return new Parking_Id(ParkingOperatorId,
                                      MatchCollection[0].Groups[2].Value);

            if (ChargingStationOperator_Id.TryParse(MatchCollection[0].Groups[3].Value, out ParkingOperatorId))
                return new Parking_Id(ParkingOperatorId,
                                      MatchCollection[0].Groups[4].Value);


            throw new ArgumentException("Illegal parking identification '" + Text + "'!");

        }

        #endregion

        #region Parse(OperatorId, IdSuffix)

        /// <summary>
        /// Parse the given string as a parking spot identification.
        /// </summary>
        /// <param name="OperatorId">The unique identification of a charging station operator.</param>
        /// <param name="IdSuffix">The suffix of the parking spot identification.</param>
        public static Parking_Id Parse(ChargingStationOperator_Id  OperatorId,
                                       String                      IdSuffix)

            => new Parking_Id(OperatorId,
                              IdSuffix);

        #endregion

        #region TryParse(Text, out ParkingId)

        /// <summary>
        /// Parse the given string as a parking spot identification.
        /// </summary>
        /// <param name="Text">A text representation of a parking spot identification.</param>
        /// <param name="ParkingId">The parsed parking spot identification.</param>
        public static Boolean TryParse(String Text, out Parking_Id ParkingId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ParkingId = default(Parking_Id);
                return false;
            }

            #endregion

            try
            {

                ParkingId = default(Parking_Id);

                var MatchCollection = ParkingId_RegEx.Matches(Text);

                if (MatchCollection.Count != 1)
                    return false;

                ChargingStationOperator_Id ParkingOperatorId = null;

                // New format...
                if (ChargingStationOperator_Id.TryParse(MatchCollection[0].Groups[1].Value, out ParkingOperatorId))
                {

                    ParkingId = new Parking_Id(ParkingOperatorId,
                                               MatchCollection[0].Groups[2].Value);

                    return true;

                }

                // Old format...
                else if (ChargingStationOperator_Id.TryParse(MatchCollection[0].Groups[3].Value, out ParkingOperatorId))
                {

                    ParkingId = new Parking_Id(ParkingOperatorId,
                                               MatchCollection[0].Groups[4].Value);

                    return true;

                }

            }
            catch (Exception e)
            { }

            ParkingId = default(Parking_Id);
            return false;

        }

        #endregion


        #region Operator overloading

        #region Operator == (ParkingId1, ParkingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingId1">A parking spot identification.</param>
        /// <param name="ParkingId2">Another parking spot identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Parking_Id ParkingId1, Parking_Id ParkingId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ParkingId1, ParkingId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ParkingId1 == null) || ((Object) ParkingId2 == null))
                return false;

            if ((Object) ParkingId1 == null)
                throw new ArgumentNullException(nameof(ParkingId1),  "The given parking spot identification must not be null!");

            return ParkingId1.Equals(ParkingId2);

        }

        #endregion

        #region Operator != (ParkingId1, ParkingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingId1">A parking spot identification.</param>
        /// <param name="ParkingId2">Another parking spot identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Parking_Id ParkingId1, Parking_Id ParkingId2)
            => !(ParkingId1 == ParkingId2);

        #endregion

        #region Operator <  (ParkingId1, ParkingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingId1">A parking spot identification.</param>
        /// <param name="ParkingId2">Another parking spot identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Parking_Id ParkingId1, Parking_Id ParkingId2)
        {

            if ((Object) ParkingId1 == null)
                throw new ArgumentNullException(nameof(ParkingId1),  "The given parking spot identification must not be null!");

            return ParkingId1.CompareTo(ParkingId2) < 0;

        }

        #endregion

        #region Operator <= (ParkingId1, ParkingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingId1">A parking spot identification.</param>
        /// <param name="ParkingId2">Another parking spot identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Parking_Id ParkingId1, Parking_Id ParkingId2)
            => !(ParkingId1 > ParkingId2);

        #endregion

        #region Operator >  (ParkingId1, ParkingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingId1">A parking spot identification.</param>
        /// <param name="ParkingId2">Another parking spot identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Parking_Id ParkingId1, Parking_Id ParkingId2)
        {

            if ((Object) ParkingId1 == null)
                throw new ArgumentNullException(nameof(ParkingId1),  "The given parking spot identification must not be null!");

            return ParkingId1.CompareTo(ParkingId2) > 0;

        }

        #endregion

        #region Operator >= (ParkingId1, ParkingId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingId1">A parking spot identification.</param>
        /// <param name="ParkingId2">Another parking spot identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Parking_Id ParkingId1, Parking_Id ParkingId2)
            => !(ParkingId1 < ParkingId2);

        #endregion

        #endregion

        #region IComparable<ParkingId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            // Check if the given object is a parking spot identification.
            if (!(Object is Parking_Id))
                throw new ArgumentException("The given object is not a parking spot identification!", nameof(Object));

            return CompareTo((Parking_Id) Object);

        }

        #endregion

        #region CompareTo(ParkingId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ParkingId">An object to compare with.</param>
        public Int32 CompareTo(Parking_Id ParkingId)
        {

            if ((Object) ParkingId == null)
                throw new ArgumentNullException(nameof(ParkingId),  "The given parking spot identification must not be null!");

            // Compare the length of the parking spot identifications
            var _Result = this.Length.CompareTo(ParkingId.Length);

            // If equal: Compare OperatorIds
            if (_Result == 0)
                _Result = OperatorId.CompareTo(ParkingId.OperatorId);

            // If equal: Compare parking spot identification suffix
            if (_Result == 0)
                _Result = String.Compare(Suffix, ParkingId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<ParkingId> Members

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

            // Check if the given object is a parking spot identification.
            if (!(Object is Parking_Id))
                return false;

            return this.Equals((Parking_Id) Object);

        }

        #endregion

        #region Equals(ParkingId)

        /// <summary>
        /// Compares two parking spot identifications for equality.
        /// </summary>
        /// <param name="ParkingId">A parking spot identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Parking_Id ParkingId)
        {

            if ((Object) ParkingId == null)
                return false;

            return OperatorId.Equals(ParkingId.OperatorId) &&
                   Suffix.    Equals(ParkingId.Suffix);

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
            => String.Concat(OperatorId, "*P", Suffix);

        #endregion

    }

}
