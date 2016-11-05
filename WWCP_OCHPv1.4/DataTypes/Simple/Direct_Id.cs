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
    /// The unique identification of an OCHPdirect charging process.
    /// </summary>
    public class Direct_Id : IId,
                             IEquatable <Direct_Id>,
                             IComparable<Direct_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing an OCHPdirect charging process identification.
        /// </summary>
        public static readonly Regex DirectId_RegEx  = new Regex(@"^[A-Za-z]{2}\*[A-Za-z0-9]{3}\*[Ee][A-Za-z0-9][A-Za-z0-9\*]{0,30}$ |" +
                                                                 @"^[A-Za-z]{2}[A-Za-z0-9]{3}[Ee][A-Za-z0-9][A-Za-z0-9\*]{0,30}$",
                                                                 RegexOptions.IgnorePatternWhitespace);

        // [A-Z0-9\-]{1,255}

        /// <summary>
        /// The regular expression for parsing a charging process identification suffix.
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
        /// Generate a charging process identification based on the given string.
        /// </summary>
        private Direct_Id(ChargingStationOperator_Id  OperatorId,
                          String                      IdSuffix)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The parameter must not be null!");

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix),    "The parameter must not be null or empty!");

            #endregion

            var _MatchCollection = IdSuffix_RegEx.Matches(IdSuffix.Trim());

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal charging process identification '" + OperatorId + "' with suffix '" + IdSuffix + "'!");

            this.OperatorId  = OperatorId;
            this.Suffix      = _MatchCollection[0].Value;

        }

        #endregion


        #region Parse(DirectId)

        /// <summary>
        /// Parse the given string as a charging process identification.
        /// </summary>
        public static Direct_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The parameter must not be null or empty!");

            #endregion

            var _MatchCollection = DirectId_RegEx.Matches(Text.Trim().ToUpper());

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal charging process identification '" + Text + "'!");

            ChargingStationOperator_Id __EVSEOperatorId = null;

            if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                return new Direct_Id(__EVSEOperatorId,
                                     _MatchCollection[0].Groups[2].Value);

            if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                return new Direct_Id(__EVSEOperatorId,
                                     _MatchCollection[0].Groups[4].Value);


            throw new ArgumentException("Illegal charging process identification '" + Text + "'!");

        }

        #endregion

        #region Parse(OperatorId, IdSuffix)

        /// <summary>
        /// Parse the given string as a charging process identification.
        /// </summary>
        public static Direct_Id Parse(ChargingStationOperator_Id OperatorId, String IdSuffix)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The charging station operator identification must not be null or empty!");

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix),    "The suffix must not be null or empty!");

            #endregion

            return Direct_Id.Parse(OperatorId + "*" + IdSuffix);

        }

        #endregion

        #region TryParse(Text, out Direct_Id)

        /// <summary>
        /// Parse the given string as a charging process identification.
        /// </summary>
        public static Boolean TryParse(String Text, out Direct_Id DirectId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                DirectId = null;
                return false;
            }

            #endregion

            try
            {

                DirectId = null;

                var _MatchCollection = DirectId_RegEx.Matches(Text.Trim().ToUpper());

                if (_MatchCollection.Count != 1)
                    return false;

                ChargingStationOperator_Id __EVSEOperatorId = null;

                // New format...
                if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                {

                    DirectId = new Direct_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[2].Value);

                    return true;

                }

                // Old format...
                else if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                {

                    DirectId = new Direct_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[4].Value);

                    return true;

                }

            }
            catch (Exception e)
            { }

            DirectId = null;
            return false;

        }

        #endregion

        #region TryParse(OperatorId, IdSuffix, out DirectId)

        ///// <summary>
        ///// Parse the given string as an EVSE identification.
        ///// </summary>
        //public static Boolean TryParse(EVSEOperator_Id OperatorId, String IdSuffix, out Direct_Id Direct_Id)
        //{

        //    try
        //    {
        //        Direct_Id = new Direct_Id(OperatorId, IdSuffix);
        //        return true;
        //    }
        //    catch (Exception e)
        //    { }

        //    Direct_Id = null;
        //    return false;

        //}

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charging process identification.
        /// </summary>
        public Direct_Id Clone

            => new Direct_Id(OperatorId.Clone,
                             new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (DirectId1, DirectId2)

        /// <summary>
        /// Compares two charging process identifications for equality.
        /// </summary>
        /// <param name="DirectId1">A charging process identification.</param>
        /// <param name="DirectId2">Another charging process identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Direct_Id DirectId1, Direct_Id DirectId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(DirectId1, DirectId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) DirectId1 == null) || ((Object) DirectId2 == null))
                return false;

            if ((Object) DirectId1 == null)
                throw new ArgumentNullException(nameof(DirectId1),  "The given charging process identification must not be null!");

            return DirectId1.Equals(DirectId2);

        }

        #endregion

        #region Operator != (DirectId1, DirectId2)

        /// <summary>
        /// Compares two charging process identifications for inequality.
        /// </summary>
        /// <param name="DirectId1">A charging process identification.</param>
        /// <param name="DirectId2">Another charging process identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Direct_Id DirectId1, Direct_Id DirectId2)
            => !(DirectId1 == DirectId2);

        #endregion

        #region Operator <  (DirectId1, DirectId2)

        /// <summary>
        /// Compares two charging process identifications.
        /// </summary>
        /// <param name="DirectId1">A charging process identification.</param>
        /// <param name="DirectId2">Another charging process identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (Direct_Id DirectId1, Direct_Id DirectId2)
        {

            if ((Object) DirectId1 == null)
                throw new ArgumentNullException(nameof(DirectId1),  "The given charging process identification must not be null!");

            return DirectId1.CompareTo(DirectId2) < 0;

        }

        #endregion

        #region Operator <= (DirectId1, DirectId2)

        /// <summary>
        /// Compares two charging process identifications.
        /// </summary>
        /// <param name="DirectId1">A charging process identification.</param>
        /// <param name="DirectId2">Another charging process identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (Direct_Id DirectId1, Direct_Id DirectId2)
            => !(DirectId1 > DirectId2);

        #endregion

        #region Operator >  (DirectId1, DirectId2)

        /// <summary>
        /// Compares two charging process identifications.
        /// </summary>
        /// <param name="DirectId1">A charging process identification.</param>
        /// <param name="DirectId2">Another charging process identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (Direct_Id DirectId1, Direct_Id DirectId2)
        {

            if ((Object) DirectId1 == null)
                throw new ArgumentNullException(nameof(DirectId1),  "The given charging process identification must not be null!");

            return DirectId1.CompareTo(DirectId2) > 0;

        }

        #endregion

        #region Operator >= (DirectId1, DirectId2)

        /// <summary>
        /// Compares two charging process identifications.
        /// </summary>
        /// <param name="DirectId1">A charging process identification.</param>
        /// <param name="DirectId2">Another charging process identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (Direct_Id DirectId1, Direct_Id DirectId2)
            => !(DirectId1 < DirectId2);

        #endregion

        #endregion

        #region IComparable<Direct_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two charging process identifications.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            // Check if the given object is a charging process identification.
            var DirectId = Object as Direct_Id;
            if ((Object) DirectId == null)
                throw new ArgumentException("The given object is not a charging process identification!", nameof(Object));

            return CompareTo(DirectId);

        }

        #endregion

        #region CompareTo(DirectId)

        /// <summary>
        /// Compares two charging process identifications.
        /// </summary>
        /// <param name="DirectId">An object to compare with.</param>
        public Int32 CompareTo(Direct_Id DirectId)
        {

            if ((Object) DirectId == null)
                throw new ArgumentNullException(nameof(DirectId),  "The given charging process identification must not be null!");

            // Compare the length of the charging process identifications
            var _Result = this.Length.CompareTo(DirectId.Length);

            // If equal: Compare OperatorIds
            if (_Result == 0)
                _Result = OperatorId.CompareTo(DirectId.OperatorId);

            // If equal: Compare charging process identification suffix
            if (_Result == 0)
                _Result = String.Compare(Suffix, DirectId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<Direct_Id> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two charging process identifications.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is a charging process identification.
            var DirectId = Object as Direct_Id;
            if ((Object) DirectId == null)
                return false;

            return this.Equals(DirectId);

        }

        #endregion

        #region Equals(DirectId)

        /// <summary>
        /// Compares two charging process identifications for equality.
        /// </summary>
        /// <param name="DirectId">A charging process identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Direct_Id DirectId)
        {

            if ((Object) DirectId == null)
                return false;

            return OperatorId.Equals(DirectId.OperatorId) &&
                   Suffix.    Equals(DirectId.Suffix);

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

                return OperatorId.GetHashCode() * 11 ^
                       Suffix.    GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat(OperatorId, "*I", Suffix);

        #endregion

    }

}
