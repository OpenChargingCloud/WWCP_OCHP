/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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
    public struct CDR_Id : IId,
                           IEquatable <CDR_Id>,
                           IComparable<CDR_Id>

    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a charge point identification.
        /// </summary>
        public static readonly Regex CDRId_RegEx  = new Regex("^[0-9A-Z]{1,36}$",  //([A-Z]{2}[A-Z0-9]{3})([A-Z0-9][A-Z0-9]{0,30})$",
                                                              RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The suffix of the identification.
        /// </summary>
        public String  Id   { get; }

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) Id.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Generate a charge point identification based on the given string.
        /// </summary>
        private CDR_Id(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text),  "The parameter must not be null or empty!");

            #endregion

            var _MatchCollection = CDRId_RegEx.Matches(Text.Trim());

            if (_MatchCollection.Count != 1)
                throw new ArgumentException("Illegal charge detail record identification '" + Text + "'!");

            this.Id = _MatchCollection[0].Value;

        }

        #endregion


        #region Parse(CDRId)

        /// <summary>
        /// Parse the given string as a charge detail record identification.
        /// </summary>
        public static CDR_Id Parse(String Text)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty() || Text.Trim().IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a CDR identification must not be null or empty!");

            #endregion

            var _MatchCollection = CDRId_RegEx.Matches(Text.Trim().ToUpper());

            if (_MatchCollection.Count == 1)
                return new CDR_Id(_MatchCollection[0].Groups[0].Value);

            throw new ArgumentException("Illegal text representation of a charge detail record identification '" + Text + "'!");

        }

        #endregion

        #region TryParse(Text)

        /// <summary>
        /// Try to parse the given text representation of a charge detail record identification.
        /// </summary>
        /// <param name="Text">A text representation of a charge detail record identification.</param>
        public static CDR_Id? TryParse(String Text)
        {

            if (TryParse(Text, out CDR_Id _CDRId))
                return _CDRId;

            return new CDR_Id?();

        }

        #endregion

        #region TryParse(Text, out CDR_Id)

        /// <summary>
        /// Parse the given string as a charge detail record identification.
        /// </summary>
        public static Boolean TryParse(String Text, out CDR_Id CDRId)
        {

            #region Initial checks

            if (Text.IsNullOrEmpty() || Text.Trim().IsNullOrEmpty())
            {
                CDRId = default(CDR_Id);
                return false;
            }

            #endregion

            try
            {

                var MatchCollection = CDRId_RegEx.Matches(Text.Trim().ToUpper());

                if (MatchCollection.Count == 1)
                {

                    CDRId = new CDR_Id(MatchCollection[0].Groups[0].Value);
                    return true;

                }

            }
#pragma warning disable RCS1075  // Avoid empty catch clause that catches System.Exception.
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception)
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
#pragma warning restore RCS1075  // Avoid empty catch clause that catches System.Exception.
            { }

            CDRId = default(CDR_Id);
            return false;

        }

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

            if (!(Object is CDR_Id))
                throw new ArgumentException("The given object is not a CDRId!", nameof(Object));

            return CompareTo((CDR_Id) Object);

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

            return String.Compare(Id, CDRId.Id, StringComparison.Ordinal);

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

            if (!(Object is CDR_Id))
                return false;

            return this.Equals((CDR_Id) Object);

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

            return Id.Equals(CDRId.Id);

        }

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
            => Id.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => Id;

        #endregion

    }

}
