/*
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

using System;

using org.GraphDefined.Vanaheimr.Illias;
using System.Xml.Linq;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// The type of connector.
    /// </summary>
    public class ConnectorType
    {

        #region Properties

        /// <summary>
        /// The OCHP connector standard.
        /// </summary>
        public ConnectorStandards  Standard    { get; }

        /// <summary>
        /// The OCHP connector format.
        /// </summary>
        public ConnectorFormats    Format      { get; }

        /// <summary>
        /// References an optional tariff uploaded by the CPO to be used with this connector.
        /// </summary>
        public Tariff_Id?          TariffId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP connector.
        /// </summary>
        /// <param name="Standard">The OCHP connector standard.</param>
        /// <param name="Format">The OCHP connector format.</param>
        /// <param name="TariffId">References an optional tariff uploaded by the CPO to be used with this connector.</param>
        public ConnectorType(ConnectorStandards  Standard,
                             ConnectorFormats    Format,
                             Tariff_Id?          TariffId  = null)
        {

            this.Standard  = Standard;
            this.Format    = Format;
            this.TariffId  = TariffId ?? new Tariff_Id?();

        }

        #endregion


        #region Documentation

        // <ns:connectors>
        //
        //    <ns:connectorStandard>
        //       <ns:ConnectorStandard>?</ns:ConnectorStandard>
        //    </ns:connectorStandard>
        //
        //    <ns:connectorFormat>
        //       <ns:ConnectorFormat>?</ns:ConnectorFormat>
        //    </ns:connectorFormat>
        //
        //    <!--Optional:-->
        //    <ns:tariffId>?</ns:tariffId>
        //
        // </ns:connectors>

        #endregion

        #region (static) Parse(ConnectorTypeXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP connector type.
        /// </summary>
        /// <param name="ConnectorTypeXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ConnectorType Parse(XElement             ConnectorTypeXML,
                                          OnExceptionDelegate  OnException = null)
        {

            ConnectorType _ConnectorType;

            if (TryParse(ConnectorTypeXML, out _ConnectorType, OnException))
                return _ConnectorType;

            return null;

        }

        #endregion

        #region (static) Parse(ConnectorTypeText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP connector type.
        /// </summary>
        /// <param name="ConnectorTypeText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ConnectorType Parse(String               ConnectorTypeText,
                                          OnExceptionDelegate  OnException = null)
        {

            ConnectorType _ConnectorType;

            if (TryParse(ConnectorTypeText, out _ConnectorType, OnException))
                return _ConnectorType;

            return null;

        }

        #endregion

        #region (static) TryParse(ConnectorTypeXML,  out ConnectorType, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP connector type.
        /// </summary>
        /// <param name="ConnectorTypeXML">The XML to parse.</param>
        /// <param name="ConnectorType">The parsed connector type.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             ConnectorTypeXML,
                                       out ConnectorType    ConnectorType,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                ConnectorType = new ConnectorType(

                                    ConnectorTypeXML.MapValueOrFail    (OCHPNS.Default + "connectorStandard",
                                                                        OCHPNS.Default + "ConnectorStandard",
                                                                        XML_IO.AsConnectorStandard),

                                    ConnectorTypeXML.MapValueOrFail    (OCHPNS.Default + "connectorFormat",
                                                                        OCHPNS.Default + "ConnectorFormat",
                                                                        XML_IO.AsConnectorFormat),

                                    ConnectorTypeXML.MapValueOrNullable(OCHPNS.Default + "tariffId",
                                                                        Tariff_Id.Parse)

                                );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, ConnectorTypeXML, e);

                ConnectorType = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ConnectorTypeText, out ConnectorType, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP connector type.
        /// </summary>
        /// <param name="ConnectorTypeText">The text to parse.</param>
        /// <param name="ConnectorType">The parsed connector type.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               ConnectorTypeText,
                                       out ConnectorType    ConnectorType,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ConnectorTypeText).Root,
                             out ConnectorType,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, ConnectorTypeText, e);
            }

            ConnectorType = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">An alternative XML element name [default: "OCHPNS:connector"]</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OCHPNS.Default + "connector",

                   new XElement(OCHPNS.Default + "connectorStandard",
                       new XElement(OCHPNS.Default + "ConnectorStandard",  XML_IO.AsText(Standard))
                   ),

                   new XElement(OCHPNS.Default + "connectorFormat",
                       new XElement(OCHPNS.Default + "ConnectorFormat",    XML_IO.AsText(Format))
                   ),

                   TariffId.HasValue
                       ? new XElement(OCHPNS.Default + "tariffId", TariffId.ToString())
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (ConnectorType1, ConnectorType2)

        /// <summary>
        /// Compares two connectors for equality.
        /// </summary>
        /// <param name="ConnectorType1">A connector.</param>
        /// <param name="ConnectorType2">Another connector.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ConnectorType ConnectorType1, ConnectorType ConnectorType2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ConnectorType1, ConnectorType2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ConnectorType1 == null) || ((Object) ConnectorType2 == null))
                return false;

            return ConnectorType1.Equals(ConnectorType2);

        }

        #endregion

        #region Operator != (ConnectorType1, ConnectorType2)

        /// <summary>
        /// Compares two connectors for inequality.
        /// </summary>
        /// <param name="ConnectorType1">A connector.</param>
        /// <param name="ConnectorType2">Another connector.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ConnectorType ConnectorType1, ConnectorType ConnectorType2)

            => !(ConnectorType1 == ConnectorType2);

        #endregion

        #endregion

        #region IEquatable<ConnectorType> Members

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

            // Check if the given object is a connector.
            var ConnectorType = Object as ConnectorType;
            if ((Object) ConnectorType == null)
                return false;

            return this.Equals(ConnectorType);

        }

        #endregion

        #region Equals(ConnectorType)

        /// <summary>
        /// Compares two connectors for equality.
        /// </summary>
        /// <param name="ConnectorType">An connector to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ConnectorType ConnectorType)
        {

            if ((Object) ConnectorType == null)
                return false;

            return Standard.Equals(ConnectorType.Standard) &&
                   Format.  Equals(ConnectorType.Format)   &&

                   ((!TariffId.HasValue && !ConnectorType.TariffId.HasValue) ||
                     (TariffId.HasValue &&  ConnectorType.TariffId.HasValue && TariffId.Value.Equals(ConnectorType.TariffId.Value)));

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

                return Standard.GetHashCode() * 7 ^
                       Format.  GetHashCode() * 5 ^

                       (TariffId.HasValue
                           ? TariffId.GetHashCode()
                           : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Standard, " / ", Format,

                             TariffId.HasValue
                                 ? " with tariff " + TariffId
                                 : "");

        #endregion

    }

}
