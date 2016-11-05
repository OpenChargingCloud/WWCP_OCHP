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
    /// The unique identification of an OCHP charge detail record.
    /// </summary>
    public class CDR_Id : IId,
                          IEquatable <CDR_Id>,
                          IComparable<CDR_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charge point identification.
        /// </summary>
        public static readonly Regex CDRId_RegEx    = new Regex(@"^[A-Za-z]{2}\*[A-Za-z0-9]{3}\*[Ee][A-Za-z0-9][A-Za-z0-9\*]{0,30}$ |" +
                                                                 @"^[A-Za-z]{2}[A-Za-z0-9]{3}[Ee][A-Za-z0-9][A-Za-z0-9\*]{0,30}$",
                                                                 RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for parsing an charge point identification suffix.
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
        /// Generate a charge point identification based on the given string.
        /// </summary>
        private CDR_Id(ChargingStationOperator_Id  OperatorId,
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
                throw new ArgumentException("Illegal charge point identification '" + OperatorId + "' with suffix '" + IdSuffix + "'!");

            this.OperatorId  = OperatorId;
            this.Suffix      = _MatchCollection[0].Value;

        }

        #endregion


        #region Parse(CDRId)

        /// <summary>
        /// Parse the given string as a charge point identification.
        /// </summary>
        public static CDR_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The parameter must not be null or empty!");

            #endregion

            var _MatchCollection = CDRId_RegEx.Matches(Text.Trim().ToUpper());

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal EVSE identification '" + Text + "'!");

            ChargingStationOperator_Id __EVSEOperatorId = null;

            if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                return new CDR_Id(__EVSEOperatorId,
                                   _MatchCollection[0].Groups[2].Value);

            if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                return new CDR_Id(__EVSEOperatorId,
                                   _MatchCollection[0].Groups[4].Value);


            throw new ArgumentException("Illegal EVSE identification '" + Text + "'!");

        }

        #endregion

        #region Parse(OperatorId, IdSuffix)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        public static CDR_Id Parse(ChargingStationOperator_Id OperatorId, String IdSuffix)
        {

            #region Initial checks

            if (OperatorId == null)
                throw new ArgumentNullException(nameof(OperatorId),  "The Charging Station Operator identification must not be null or empty!");

            if (IdSuffix.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(IdSuffix),    "The parameter must not be null or empty!");

            #endregion

            return CDR_Id.Parse(OperatorId.ToString() + "*" + IdSuffix);

        }

        #endregion

        #region TryParse(Text, out CDR_Id)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        public static Boolean TryParse(String Text, out CDR_Id CDRId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
            {
                CDRId = null;
                return false;
            }

            #endregion

            try
            {

                CDRId = null;

                var _MatchCollection = CDRId_RegEx.Matches(Text.Trim().ToUpper());

                if (_MatchCollection.Count != 1)
                    return false;

                ChargingStationOperator_Id __EVSEOperatorId = null;

                // New format...
                if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
                {

                    CDRId = new CDR_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[2].Value);

                    return true;

                }

                // Old format...
                else if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
                {

                    CDRId = new CDR_Id(__EVSEOperatorId,
                                         _MatchCollection[0].Groups[4].Value);

                    return true;

                }

            }
            catch (Exception e)
            { }

            CDRId = null;
            return false;

        }

        #endregion

        #region TryParse(OperatorId, IdSuffix, out CDRId)

        ///// <summary>
        ///// Parse the given string as an EVSE identification.
        ///// </summary>
        //public static Boolean TryParse(EVSEOperator_Id OperatorId, String IdSuffix, out CDR_Id CDR_Id)
        //{

        //    try
        //    {
        //        CDR_Id = new CDR_Id(OperatorId, IdSuffix);
        //        return true;
        //    }
        //    catch (Exception e)
        //    { }

        //    CDR_Id = null;
        //    return false;

        //}

        #endregion

        #region Clone

        /// <summary>
        /// Clone this charge point identification.
        /// </summary>
        public CDR_Id Clone

            => new CDR_Id(OperatorId.Clone,
                             new String(Suffix.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (CDRId1, CDRId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CDRId1">A charge point identification.</param>
        /// <param name="CDRId2">Another charge point identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (CDR_Id CDRId1, CDR_Id CDRId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(CDRId1, CDRId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) CDRId1 == null) || ((Object) CDRId2 == null))
                return false;

            if ((Object) CDRId1 == null)
                throw new ArgumentNullException(nameof(CDRId1),  "The given charge point identification must not be null!");

            return CDRId1.Equals(CDRId2);

        }

        #endregion

        #region Operator != (CDRId1, CDRId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CDRId1">A charge point identification.</param>
        /// <param name="CDRId2">Another charge point identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (CDR_Id CDRId1, CDR_Id CDRId2)
            => !(CDRId1 == CDRId2);

        #endregion

        #region Operator <  (CDRId1, CDRId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CDRId1">A charge point identification.</param>
        /// <param name="CDRId2">Another charge point identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (CDR_Id CDRId1, CDR_Id CDRId2)
        {

            if ((Object) CDRId1 == null)
                throw new ArgumentNullException(nameof(CDRId1),  "The given charge point identification must not be null!");

            return CDRId1.CompareTo(CDRId2) < 0;

        }

        #endregion

        #region Operator <= (CDRId1, CDRId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CDRId1">A charge point identification.</param>
        /// <param name="CDRId2">Another charge point identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (CDR_Id CDRId1, CDR_Id CDRId2)
            => !(CDRId1 > CDRId2);

        #endregion

        #region Operator >  (CDRId1, CDRId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CDRId1">A charge point identification.</param>
        /// <param name="CDRId2">Another charge point identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (CDR_Id CDRId1, CDR_Id CDRId2)
        {

            if ((Object) CDRId1 == null)
                throw new ArgumentNullException(nameof(CDRId1),  "The given charge point identification must not be null!");

            return CDRId1.CompareTo(CDRId2) > 0;

        }

        #endregion

        #region Operator >= (CDRId1, CDRId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CDRId1">A charge point identification.</param>
        /// <param name="CDRId2">Another charge point identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (CDR_Id CDRId1, CDR_Id CDRId2)
            => !(CDRId1 < CDRId2);

        #endregion

        #endregion

        #region IComparable<CDRId> Members

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
            var CDRId = Object as CDR_Id;
            if ((Object) CDRId == null)
                throw new ArgumentException("The given object is not a CDRId!", nameof(Object));

            return CompareTo(CDRId);

        }

        #endregion

        #region CompareTo(CDRId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CDRId">An object to compare with.</param>
        public Int32 CompareTo(CDR_Id CDRId)
        {

            if ((Object) CDRId == null)
                throw new ArgumentNullException(nameof(CDRId),  "The given charge point identification must not be null!");

            // Compare the length of the charge point identifications
            var _Result = this.Length.CompareTo(CDRId.Length);

            // If equal: Compare OperatorIds
            if (_Result == 0)
                _Result = OperatorId.CompareTo(CDRId.OperatorId);

            // If equal: Compare charge point identification suffix
            if (_Result == 0)
                _Result = String.Compare(Suffix, CDRId.Suffix, StringComparison.Ordinal);

            return _Result;

        }

        #endregion

        #endregion

        #region IEquatable<CDRId> Members

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
            var CDRId = Object as CDR_Id;
            if ((Object) CDRId == null)
                return false;

            return this.Equals(CDRId);

        }

        #endregion

        #region Equals(CDRId)

        /// <summary>
        /// Compares two charge point identifications for equality.
        /// </summary>
        /// <param name="CDRId">A charge point identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(CDR_Id CDRId)
        {

            if ((Object) CDRId == null)
                return false;

            return OperatorId.Equals(CDRId.OperatorId) &&
                   Suffix.    Equals(CDRId.Suffix);

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
            => String.Concat(OperatorId, "*E", Suffix);

        #endregion

    }

}
