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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// The unique identification of an OCHP token.
    /// </summary>
    public class EMT_Id : IId,
                          IEquatable <EMT_Id>,
                          IComparable<EMT_Id>

    {

        #region Properties

        /// <summary>
        /// The plain value of the token identification.
        /// </summary>
        public String                Instance          { get; }

        /// <summary>
        /// The token instance may be represented by its hash value
        /// (hexadecimal representation of the hash value).
        /// This specifies in which representation the token instance is set.
        /// </summary>
        public TokenRepresentations  Representation    { get; }

        /// <summary>
        /// The type of the supplied instance.
        /// </summary>
        public TokenTypes            Type              { get; }

        /// <summary>
        /// The exact type of the supplied instance.
        /// </summary>
        public TokenSubTypes?        SubType           { get; }


        /// <summary>
        /// Returns the length of the identificator.
        /// </summary>
        public UInt64 Length

            => (UInt64) Instance.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Instance">The plain value of the token identification.</param>
        /// <param name="Representation">The token instance may be represented by its hash value (hexadecimal representation of the hash value). This specifies in which representation the token instance is set.</param>
        /// <param name="Type">The type of the supplied instance.</param>
        /// <param name="SubType">The exact type of the supplied instance.</param>
        private EMT_Id(String                Instance,
                       TokenRepresentations  Representation,
                       TokenTypes            Type,
                       TokenSubTypes?        SubType = null)
        {

            #region Initial checks

            if (Instance.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Instance),  "The given instance value must not be null!");

            #endregion

            this.Instance        = Instance;
            this.Representation  = Representation;
            this.Type            = Type;
            this.SubType         = SubType;

        }

        #endregion


        #region Parse(EMTId)

        /// <summary>
        /// Parse the given string as a contract identification.
        /// </summary>
        //public static EMT_Id Parse(String Text)
        //{

        //    #region Initial checks

        //    if (Text.IsNullOrEmpty())
        //        throw new ArgumentNullException(nameof(Text), "The parameter must not be null or empty!");

        //    #endregion

        //    var _MatchCollection = EMTId_RegEx.Matches(Text.Trim().ToUpper());

        //    if (_MatchCollection.Count != 1)
        //        throw new ArgumentException("Illegal EVSE identification '" + Text + "'!");

        //    ChargingStationOperator_Id __EVSEOperatorId = null;

        //    if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[1].Value, out __EVSEOperatorId))
        //        return new EMT_Id(__EVSEOperatorId,
        //                           _MatchCollection[0].Groups[2].Value);

        //    if (ChargingStationOperator_Id.TryParse(_MatchCollection[0].Groups[3].Value, out __EVSEOperatorId))
        //        return new EMT_Id(__EVSEOperatorId,
        //                           _MatchCollection[0].Groups[4].Value);


        //    throw new ArgumentException("Illegal EVSE identification '" + Text + "'!");

        //}

        #endregion

        #region Parse(OperatorId, IdSuffix)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        //public static EMT_Id Parse(ChargingStationOperator_Id OperatorId, String IdSuffix)
        //{

        //    #region Initial checks

        //    if (OperatorId == null)
        //        throw new ArgumentNullException(nameof(OperatorId),  "The Charging Station Operator identification must not be null or empty!");

        //    if (IdSuffix.IsNullOrEmpty())
        //        throw new ArgumentNullException(nameof(IdSuffix),    "The parameter must not be null or empty!");

        //    #endregion

        //    return EMT_Id.Parse(OperatorId.ToString() + "*" + IdSuffix);

        //}

        #endregion

        #region TryParse(Text, out EMT_Id)

        /// <summary>
        /// Parse the given string as an EVSE identification.
        /// </summary>
        //public static Boolean TryParse(String Text, out EMT_Id EMTId)
        //{

        //    #region Initial checks

        //    if (Text.IsNullOrEmpty())
        //    {
        //        EMTId = null;
        //        return false;
        //    }

        //    #endregion

        //    try
        //    {

        //        EMTId = new EMT_Id(Text);

        //    }
        //    catch (Exception)
        //    { }

        //    EMTId = null;
        //    return false;

        //}

        #endregion

        #region TryParse(OperatorId, IdSuffix, out EMTId)

        ///// <summary>
        ///// Parse the given string as an EVSE identification.
        ///// </summary>
        //public static Boolean TryParse(EVSEOperator_Id OperatorId, String IdSuffix, out EMT_Id EMT_Id)
        //{

        //    try
        //    {
        //        EMT_Id = new EMT_Id(OperatorId, IdSuffix);
        //        return true;
        //    }
        //    catch (Exception e)
        //    { }

        //    EMT_Id = null;
        //    return false;

        //}

        #endregion

        #region Clone

        /// <summary>
        /// Clone this contract identification.
        /// </summary>
        public EMT_Id Clone

            => new EMT_Id(new String(Instance.ToCharArray()),
                          Representation,
                          Type,
                          SubType);

        #endregion


        #region Operator overloading

        #region Operator == (EMTId1, EMTId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMTId1">A token identification.</param>
        /// <param name="EMTId2">Another token identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (EMT_Id EMTId1, EMT_Id EMTId2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EMTId1, EMTId2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EMTId1 == null) || ((Object) EMTId2 == null))
                return false;

            if ((Object) EMTId1 == null)
                throw new ArgumentNullException(nameof(EMTId1),  "The given token identification must not be null!");

            return EMTId1.Equals(EMTId2);

        }

        #endregion

        #region Operator != (EMTId1, EMTId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMTId1">A token identification.</param>
        /// <param name="EMTId2">Another token identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (EMT_Id EMTId1, EMT_Id EMTId2)
            => !(EMTId1 == EMTId2);

        #endregion

        #region Operator <  (EMTId1, EMTId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMTId1">A token identification.</param>
        /// <param name="EMTId2">Another token identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (EMT_Id EMTId1, EMT_Id EMTId2)
        {

            if ((Object) EMTId1 == null)
                throw new ArgumentNullException(nameof(EMTId1),  "The given token identification must not be null!");

            return EMTId1.CompareTo(EMTId2) < 0;

        }

        #endregion

        #region Operator <= (EMTId1, EMTId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMTId1">A token identification.</param>
        /// <param name="EMTId2">Another token identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (EMT_Id EMTId1, EMT_Id EMTId2)
            => !(EMTId1 > EMTId2);

        #endregion

        #region Operator >  (EMTId1, EMTId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMTId1">A token identification.</param>
        /// <param name="EMTId2">Another token identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (EMT_Id EMTId1, EMT_Id EMTId2)
        {

            if ((Object) EMTId1 == null)
                throw new ArgumentNullException(nameof(EMTId1),  "The given token identification must not be null!");

            return EMTId1.CompareTo(EMTId2) > 0;

        }

        #endregion

        #region Operator >= (EMTId1, EMTId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMTId1">A token identification.</param>
        /// <param name="EMTId2">Another token identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (EMT_Id EMTId1, EMT_Id EMTId2)
            => !(EMTId1 < EMTId2);

        #endregion

        #endregion

        #region IComparable<EMT_Id> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            // Check if the given object is a token identification.
            var EMTId = Object as EMT_Id;
            if ((Object) EMTId == null)
                throw new ArgumentException("The given object is not a token identification!", nameof(Object));

            return CompareTo(EMTId);

        }

        #endregion

        #region CompareTo(EMTId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="EMTId">An object to compare with.</param>
        public Int32 CompareTo(EMT_Id EMTId)
        {

            if ((Object) EMTId == null)
                throw new ArgumentNullException(nameof(EMTId),  "The given token identification must not be null!");

            return String.Compare(Instance, EMTId.Instance, StringComparison.Ordinal);

        }

        #endregion

        #endregion

        #region IEquatable<EMT_Id> Members

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

            // Check if the given object is a token identification.
            var EMTId = Object as EMT_Id;
            if ((Object) EMTId == null)
                return false;

            return this.Equals(EMTId);

        }

        #endregion

        #region Equals(EMTId)

        /// <summary>
        /// Compares two token identifications for equality.
        /// </summary>
        /// <param name="EMTId">A token identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EMT_Id EMTId)
        {

            if ((Object) EMTId == null)
                return false;

            return Instance.      Equals(EMTId.Instance)       &&
                   Representation.Equals(EMTId.Representation) &&
                   Type.          Equals(EMTId.Type)           &&
                   SubType.       Equals(EMTId.SubType);

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

                return Instance.      GetHashCode() * 31 ^
                       Representation.GetHashCode() * 23 ^
                       Type.          GetHashCode() * 11 ^

                       (SubType.HasValue
                           ? SubType. GetHashCode()
                           : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()
            => String.Concat(Instance, " - ", Representation, ", ", Type, SubType.HasValue ? ", " + SubType : "");

        #endregion

    }

}
