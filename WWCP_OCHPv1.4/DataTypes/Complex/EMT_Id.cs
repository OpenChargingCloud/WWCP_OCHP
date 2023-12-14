/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Hermod.JSON;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// The unique identification of an OCHP token.
    /// </summary>
    public readonly struct EMT_Id : IId,
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
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => Instance.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => Instance.IsNotNullOrEmpty();

        /// <summary>
        /// Returns the length of the identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) Instance.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new unique identification of an OCHP token.
        /// </summary>
        /// <param name="Instance">The plain value of the token identification.</param>
        /// <param name="Representation">The token instance may be represented by its hash value (hexadecimal representation of the hash value). This specifies in which representation the token instance is set.</param>
        /// <param name="Type">The type of the supplied instance.</param>
        /// <param name="SubType">The exact type of the supplied instance.</param>
        public EMT_Id(String                Instance,
                      TokenRepresentations  Representation,
                      TokenTypes            Type,
                      TokenSubTypes?        SubType = null)
        {

            #region Initial checks

            if (Instance?.Trim().IsNullOrEmpty() == true)
                throw new ArgumentNullException(nameof(Instance),  "The given instance value must not be null!");

            #endregion

            this.Instance        = Instance;
            this.Representation  = Representation;
            this.Type            = Type;
            this.SubType         = SubType ?? new TokenSubTypes?();

        }

        #endregion


        #region Documentation

        // <ns:emtId representation="plain">
        //
        //    <ns:instance>?</ns:instance>
        //    <ns:tokenType>?</ns:tokenType>
        //
        //    <!--Optional:-->
        //    <ns:tokenSubType>?</ns:tokenSubType>
        //
        // </ns:emtId>

        #endregion

        #region (static) Parse(EMTIdXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP token identification.
        /// </summary>
        /// <param name="EMTIdXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EMT_Id Parse(XElement             EMTIdXML,
                                   OnExceptionDelegate  OnException = null)

            => TryParse(EMTIdXML, out EMT_Id _EMTId, OnException)
                   ? _EMTId
                   : default(EMT_Id);

        #endregion

        #region (static) Parse(EMTIdText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP token identification.
        /// </summary>
        /// <param name="EMTIdText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EMT_Id Parse(String               EMTIdText,
                                   OnExceptionDelegate  OnException = null)

            => TryParse(EMTIdText, out EMT_Id _EMTId, OnException)
                   ? _EMTId
                   : default(EMT_Id);

        #endregion

        #region (static) TryParse(EMTIdXML,             OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP token identification.
        /// </summary>
        /// <param name="EMTIdXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EMT_Id? TryParse(XElement             EMTIdXML,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                return new EMT_Id(

                           EMTIdXML.ElementValueOrFail     (OCHPNS.Default + "instance"),

                           EMTIdXML.MapAttributeValueOrFail(OCHPNS.Default + "representation",
                                                            XML_IO.AsTokenRepresentation),

                           EMTIdXML.MapValueOrFail         (OCHPNS.Default + "tokenType",
                                                            XML_IO.AsTokenType),

                           EMTIdXML.MapValueOrNullable     (OCHPNS.Default + "tokenSubType",
                                                            XML_IO.AsTokenSubType)

                       );

            }
            catch (Exception e)
            {
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, EMTIdXML, e);
                return new EMT_Id?();
            }

        }

        #endregion

        #region (static) TryParse(EMTIdXML,  out EMTId, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP token identification.
        /// </summary>
        /// <param name="EMTIdXML">The XML to parse.</param>
        /// <param name="EMTId">The parsed token identification.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             EMTIdXML,
                                       out EMT_Id           EMTId,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                EMTId = new EMT_Id(

                            EMTIdXML.ElementValueOrFail     (OCHPNS.Default + "instance"),

                            EMTIdXML.MapAttributeValueOrFail(OCHPNS.Default + "representation",
                                                             XML_IO.AsTokenRepresentation),

                            EMTIdXML.MapValueOrFail         (OCHPNS.Default + "tokenType",
                                                             XML_IO.AsTokenType),

                            EMTIdXML.MapValueOrNullable     (OCHPNS.Default + "tokenSubType",
                                                             XML_IO.AsTokenSubType)

                        );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, EMTIdXML, e);

                EMTId = default(EMT_Id);
                return false;

            }

        }

        #endregion

        #region (static) TryParse(EMTIdText,            OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP token identification.
        /// </summary>
        /// <param name="EMTIdText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EMT_Id? TryParse(String               EMTIdText,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(EMTIdText).Root,
                             out EMT_Id EMTId,
                             OnException))
                {
                    return EMTId;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, EMTIdText, e);
            }

            return new EMT_Id?();

        }

        #endregion

        #region (static) TryParse(EMTIdText, out EMTId, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP token identification.
        /// </summary>
        /// <param name="EMTIdText">The text to parse.</param>
        /// <param name="EMTId">The parsed token identification.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               EMTIdText,
                                       out EMT_Id           EMTId,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(EMTIdText).Root,
                             out EMTId,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, EMTIdText, e);
            }

            EMTId = default(EMT_Id);
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">An alternative XML element name [default: "OCHPNS:emtId"]</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OCHPNS.Default + "emtId",

                   new XAttribute("representation",                     XML_IO.AsText(Representation)),

                   new XElement  (OCHPNS.Default + "instance",          Instance),
                   new XElement  (OCHPNS.Default + "tokenType",         XML_IO.AsText(Type)),

                   SubType.HasValue
                       ? new XElement(OCHPNS.Default + "tokenSubType",  XML_IO.AsText(SubType.Value))
                       : null

               );

        #endregion


        #region ToJSON(CustomEMTIdSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomEMTIdSerializer">A delegate to customize the serialization of EMT identification.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<EMT_Id> CustomEMTIdSerializer = null)
        {

            var JSON = JSONObject.Create(

                   new JProperty("representation",      XML_IO.AsText(Representation)),

                   new JProperty("instance",            Instance),
                   new JProperty("tokenType",           XML_IO.AsText(Type)),

                   SubType.HasValue
                       ? new JProperty("tokenSubType",  XML_IO.AsText(SubType.Value))
                       : null

               );

            return CustomEMTIdSerializer is not null
                       ? CustomEMTIdSerializer(this, JSON)
                       : JSON;

        }

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
            if (ReferenceEquals(EMTId1, EMTId2))
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

        #region IComparable<EMTId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)
        {

            if (Object == null)
                throw new ArgumentNullException(nameof(Object),  "The given object must not be null!");

            if (!(Object is EMT_Id))
                throw new ArgumentException("The given object is not a token identification!", nameof(Object));

            return CompareTo((EMT_Id) Object);

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

        #region IEquatable<EMTId> Members

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

            if (!(Object is EMT_Id))
                return false;

            return this.Equals((EMT_Id) Object);

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

                   ((!SubType.HasValue && !EMTId.SubType.HasValue) ||
                     (SubType.HasValue &&  EMTId.SubType.HasValue && SubType.Value.Equals(EMTId.SubType.Value)));

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Instance,       " - ",
                             Representation, ", ",
                             Type,

                             SubType.HasValue
                                 ? ", " + SubType
                                 : "");

        #endregion

    }

}
