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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using System.Text.RegularExpressions;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// An OCHP address.
    /// </summary>
    public class Address : IEquatable<Address>
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing a house number.
        /// </summary>
        public static readonly Regex HouseNumber_RegEx  = new Regex(@"^[A-Z0-9 \-]{1,6}$",  RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// The regular expression for parsing a ZIP code.
        /// </summary>
        public static readonly Regex ZIPCode_RegEx      = new Regex(@"^[A-Z0-9 \-]{1,10}$", RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The house number.
        /// </summary>
        public String   HouseNumber   { get; }

        /// <summary>
        /// The address/street.
        /// </summary>
        public String   Street        { get; }

        /// <summary>
        /// The name of the city.
        /// </summary>
        public String   City          { get; }

        /// <summary>
        /// The postal code.
        /// </summary>
        public String   ZIPCode       { get; }

        /// <summary>
        /// The country.
        /// </summary>
        public Country  Country       { get; }

        #endregion

        #region Constructor(s)

        #region Address(Street, City, ZIPCode, Country)

        /// <summary>
        /// Create a new OCHP address.
        /// </summary>
        /// <param name="Street">The address/street.</param>
        /// <param name="City">The name of the city.</param>
        /// <param name="ZIPCode">The postal code</param>
        /// <param name="Country">The country.</param>
        public Address(String   Street,
                       String   City,
                       String   ZIPCode,
                       Country  Country)

            : this("",
                   Street,
                   City,
                   ZIPCode,
                   Country)

        { }

        #endregion

        #region Address(HouseNumber, Street, City, ZIPCode, Country)

        /// <summary>
        /// Create a new OCHP address.
        /// </summary>
        /// <param name="HouseNumber">The house number.</param>
        /// <param name="Street">The address/street.</param>
        /// <param name="City">The name of the city.</param>
        /// <param name="ZIPCode">The postal code</param>
        /// <param name="Country">The country.</param>
        public Address(String   HouseNumber,
                       String   Street,
                       String   City,
                       String   ZIPCode,
                       Country  Country)
        {

            this.HouseNumber  = HouseNumber;
            this.Street       = Street;
            this.City         = City;
            this.ZIPCode      = ZIPCode;
            this.Country      = Country;

        }

        #endregion

        #endregion


        #region Documentation

        // <ns:chargePointAddress xmlns:ns="http://ochp.eu/1.4">
        //
        //    <!--Optional:-->
        //    <ns:houseNumber>?</ns:houseNumber>
        //
        //    <ns:address>?</ns:address>
        //    <ns:city>?</ns:city>
        //    <ns:zipCode>?</ns:zipCode>
        //    <ns:country>?</ns:country>
        //
        // </ns:chargePointAddress>

        #endregion

        #region (static) Parse(AddressXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP address.
        /// </summary>
        /// <param name="AddressXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Address Parse(XElement             AddressXML,
                                    OnExceptionDelegate  OnException = null)
        {

            Address _Address;

            if (TryParse(AddressXML, out _Address, OnException))
                return _Address;

            return null;

        }

        #endregion

        #region (static) Parse(AddressText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP address.
        /// </summary>
        /// <param name="AddressText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Address Parse(String               AddressText,
                                    OnExceptionDelegate  OnException = null)
        {

            Address _Address;

            if (TryParse(AddressText, out _Address, OnException))
                return _Address;

            return null;

        }

        #endregion

        #region (static) TryParse(AddressXML,  out Address, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP address.
        /// </summary>
        /// <param name="AddressXML">The XML to parse.</param>
        /// <param name="Address">The parsed address.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             AddressXML,
                                       out Address          Address,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                Address = new Address(

                              AddressXML.ElementValueOrDefault(OCHPNS.Default + "houseNumber", "").Trim(),

                              AddressXML.ElementValueOrFail   (OCHPNS.Default + "address"        ).Trim(),
                              AddressXML.ElementValueOrFail   (OCHPNS.Default + "city"           ).Trim(),
                              AddressXML.ElementValueOrFail   (OCHPNS.Default + "zipCode"        ).Trim(),

                              AddressXML.MapValueOrFail       (OCHPNS.Default + "country",
                                                               value => Country.ParseAlpha3Code(value.Trim()))

                          );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, AddressXML, e);

                Address = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(AddressText, out Address, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP address.
        /// </summary>
        /// <param name="AddressText">The text to parse.</param>
        /// <param name="Address">The parsed address.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               AddressText,
                                       out Address          Address,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(AddressText).Root,
                             out Address,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, AddressText, e);
            }

            Address = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "chargePointAddress",

                   HouseNumber.IsNotNullOrEmpty()
                       ? new XElement(OCHPNS.Default + "houseNumber", HouseNumber)
                       : null,

                   new XElement(OCHPNS.Default + "address",  Street),
                   new XElement(OCHPNS.Default + "city",     City),
                   new XElement(OCHPNS.Default + "zipCode",  ZIPCode),
                   new XElement(OCHPNS.Default + "country",  Country.Alpha3Code)

               );

        #endregion


        #region Operator overloading

        #region Operator == (Address1, Address2)

        /// <summary>
        /// Compares two addresses for equality.
        /// </summary>
        /// <param name="Address1">An address.</param>
        /// <param name="Address2">Another address.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Address Address1, Address Address2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(Address1, Address2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) Address1 == null) || ((Object) Address2 == null))
                return false;

            return Address1.Equals(Address2);

        }

        #endregion

        #region Operator != (Address1, Address2)

        /// <summary>
        /// Compares two addresses for inequality.
        /// </summary>
        /// <param name="Address1">An address.</param>
        /// <param name="Address2">Another address.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Address Address1, Address Address2)

            => !(Address1 == Address2);

        #endregion

        #endregion

        #region IEquatable<Address> Members

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

            // Check if the given object is an address.
            var Address = Object as Address;
            if ((Object) Address == null)
                return false;

            return this.Equals(Address);

        }

        #endregion

        #region Equals(Address)

        /// <summary>
        /// Compares two addresses for equality.
        /// </summary>
        /// <param name="Address">An address to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(Address Address)
        {

            if ((Object) Address == null)
                return false;

            return Street.      Equals(Address.Street)      &&
                   HouseNumber. Equals(Address.HouseNumber) &&
                   City.        Equals(Address.City)        &&
                   ZIPCode.     Equals(Address.ZIPCode)     &&
                   Country.     Equals(Address.Country);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return HouseNumber. GetHashCode() * 31 ^
                       Street.      GetHashCode() * 23 ^
                       City.        GetHashCode() * 17 ^
                       ZIPCode.     GetHashCode() * 11 ^
                       Country.     GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Street, " ", HouseNumber, ", ",
                             ZIPCode,    " ", City,         ", ",
                             Country.CountryName.FirstText());

        #endregion

    }

}
