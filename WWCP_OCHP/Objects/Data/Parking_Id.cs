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
    /// The unique identification of an OCHP parking spot.
    /// </summary>
    public class Parking_Id : IId,
                              IEquatable <Parking_Id>,
                              IComparable<Parking_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a parking identification.
        /// </summary>
        public static readonly Regex ParkingId_RegEx  = new Regex(@"^([A-Za-z]{2}\*[A-Za-z0-9]{3}\*[Pp][A-Za-z0-9][A-Za-z0-9\*]{0,30})$ | ^([A-Za-z]{2}[A-Za-z0-9]{3}[Pp][A-Za-z0-9][A-Za-z0-9\*]{0,30})$",
                                                                  RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for parsing a parking identification suffix.
        /// </summary>
        public static readonly Regex IdSuffix_RegEx   = new Regex(@"^[Pp][A-Za-z0-9][A-Za-z0-9\*]{0,30}$",
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
        /// Generate a new parking spot identification based on the given string.
        /// </summary>
        private Parking_Id(ChargingStationOperator_Id   OperatorId,
                           String                       IdSuffix)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The parameter must not be null!");

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix),    "The parameter must not be null or empty!");

            #endregion

            var _MatchCollection = IdSuffix_RegEx.Matches(IdSuffix.Trim());

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE identification '" + OperatorId + "' with suffix '" + IdSuffix + "'!");

            this.OperatorId  = OperatorId;
            this.Suffix      = _MatchCollection[0].Value;

        }

        #endregion


        #region Parse(ParkingId)

        /// <summary>
        /// Parse the given string as a parking spot identification.
        /// </summary>
        public static Parking_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text must not be null or empty!");

            #endregion

            var _MatchCollection = ParkingId_RegEx.Matches(Text.Trim());

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE identification '" + Text + "'!");

            ChargingStationOperator_Id __EVSEOperatorId = null;

            if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                return new Parking_Id(__EVSEOperatorId,
                                   _MatchCollection[0].Groups[2].Value);

            if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                return new Parking_Id(__EVSEOperatorId,
                                   _MatchCollection[0].Groups[4].Value);


            throw new ArgumentException("Illegal EVSE identification '" + Text + "'!");

        }

        #endregion

        #region Parse(OperatorId, IdSuffix)

        /// <summary>
        /// Parse the given string as a parking spot identification.
        /// </summary>
        public static Parking_Id Parse(ChargingStationOperator_Id OperatorId, String IdSuffix)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The Charging Station Operator identification must not be null or empty!");

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix),    "The parameter must not be null or empty!");

            #endregion

            return Parking_Id.Parse(OperatorId + "*" + IdSuffix);

        }

        #endregion

        #region TryParse(Text, out Parking_Id)

        /// <summary>
        /// Parse the given string as a parking spot identification.
        /// </summary>
        public static Boolean TryParse(String Text, out Parking_Id ParkingId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                ParkingId = null;
                return false;
            }

            #endregion

            try
            {

                ParkingId = null;

                var _MatchCollection = ParkingId_RegEx.Matches(Text.Trim());

                if (_MatchCollection.Count != 1)
                    return false;

                ChargingStationOperator_Id __EVSEOperatorId = null;

                // New format...
                if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                {

                    ParkingId = new Parking_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[2].Value);

                    return true;

                }

                // Old format...
                else if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                {

                    ParkingId = new Parking_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[4].Value);

                    return true;

                }

            }
            catch (Exception e)
            { }

            ParkingId = null;
            return false;

        }

        #endregion

        #region TryParse(OperatorId, IdSuffix, out ParkingId)

        ///// <summary>
        ///// Parse the given string as an EVSE identification.
        ///// </summary>
        //public static Boolean TryParse(EVSEOperator_Id OperatorId, String IdSuffix, out Parking_Id Parking_Id)
        //{

        //    try
        //    {
        //        Parking_Id = new Parking_Id(OperatorId, IdSuffix);
        //        return true;
        //    }
        //    catch (Exception e)
        //    { }

        //    Parking_Id = null;
        //    return false;

        //}

        #endregion

        #region Clone

        /// <summary>
        /// Clone this parking spot identification.
        /// </summary>
        public Parking_Id Clone

            => new Parking_Id(OperatorId.Clone,
                              new String(Suffix.ToCharArray()));

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

        #region IComparable<Parking_Id> Members

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
            var ParkingId = Object as Parking_Id;
            if ((Object) ParkingId == null)
                throw new ArgumentException("The given object is not a ParkingId!", nameof(Object));

            return CompareTo(ParkingId);

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

        #region IEquatable<Parking_Id> Members

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
            var ParkingId = Object as Parking_Id;
            if ((Object) ParkingId == null)
                return false;

            return this.Equals(ParkingId);

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
            => OperatorId.GetHashCode() ^ Suffix.GetHashCode();

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
