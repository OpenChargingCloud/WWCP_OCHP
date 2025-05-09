﻿/*
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
using System.Text;
using System.Xml.Linq;
using System.Security.Cryptography;

using SOAPNS = org.GraphDefined.Vanaheimr.Hermod.SOAP.v1_1;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// OCHP SOAP helpers.
    /// </summary>
    public static class SOAP
    {

        /// <summary>
        /// Encapsulate the given XML within a XML SOAP frame.
        /// </summary>
        /// <param name="SOAPBody">The internal XML for the SOAP body.</param>
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        public static XElement Encapsulation(XElement                                     SOAPBody,
                                             org.GraphDefined.Vanaheimr.Hermod.SOAP.XMLNamespacesDelegate  XMLNamespaces = null)
        {

            #region Initial checks

            if (SOAPBody == null)
                throw new ArgumentNullException(nameof(SOAPBody),  "The given XML must not be null!");

            if (XMLNamespaces == null)
                XMLNamespaces = xml => xml;

            #endregion

            return XMLNamespaces(
                new XElement(SOAPNS.NS.SOAPEnvelope + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "SOAP",  SOAPNS.NS.SOAPEnvelope.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "OCHP",  OCHPNS.Default.        NamespaceName),

                    new XElement(SOAPNS.NS.SOAPEnvelope + "Header"),
                    new XElement(SOAPNS.NS.SOAPEnvelope + "Body",  SOAPBody)
                )
            );

        }

        #region Encapsulation(WSSUsername, WSSPassword, SOAPBody, XMLNamespaces = null)

        /// <summary>
        /// Encapsulate the given XML within a XML SOAP frame.
        /// </summary>
        /// <param name="WSSUsername">The webservice-security username.</param>
        /// <param name="WSSPassword">The webservice-security password.</param>
        /// <param name="SOAPBody">The internal XML for the SOAP body.</param>
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        public static XElement Encapsulation(String                                       WSSUsername,
                                             String                                       WSSPassword,
                                             XElement                                     SOAPBody,
                                             org.GraphDefined.Vanaheimr.Hermod.SOAP.XMLNamespacesDelegate  XMLNamespaces = null)
        {

            #region Initial checks

            if (WSSUsername == null)
                throw new ArgumentNullException(nameof(WSSUsername),  "The given XML must not be null!");

            if (WSSPassword == null)
                throw new ArgumentNullException(nameof(WSSPassword),  "The given XML must not be null!");

            if (SOAPBody == null)
                throw new ArgumentNullException(nameof(SOAPBody),     "The given XML must not be null!");

            if (XMLNamespaces == null)
                XMLNamespaces = xml => xml;

            #endregion

            #region Namespaces and data

            XNamespace WSSE       = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            XNamespace WSU        = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

            var Nonce             = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);
            var CreatedTimestamp  = Timestamp.Now.ToISO8601();
            var HashedPassword    = Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(Nonce + CreatedTimestamp + WSSPassword)));

            #endregion


            return XMLNamespaces(
                new XElement(SOAPNS.NS.SOAPEnvelope + "Envelope",

                    new XAttribute(XNamespace.Xmlns + "SOAP",  SOAPNS.NS.SOAPEnvelope.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "OCHP",  OCHPNS.Default.             NamespaceName),

                    new XElement(SOAPNS.NS.SOAPEnvelope + "Header",

                        new XElement(WSSE + "Security",
                            new XAttribute(SOAPNS.NS.SOAPEnvelope + "mustUnderstand", 1),
                            new XAttribute(XNamespace.Xmlns + "WSSE", WSSE.NamespaceName),
                            new XAttribute(XNamespace.Xmlns + "WSU",  WSU. NamespaceName),

                            new XElement(WSSE + "UsernameToken",
                                         new XAttribute(WSU + "Id", "UsernameToken-" + Nonce),

                                new XElement(WSSE + "Username",
                                             WSSUsername),

                                new XElement(WSSE + "Password",
                                             new XAttribute("Type",         "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"),
                                             WSSPassword),

                                new XElement(WSSE + "Nonce",
                                             new XAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"),
                                             Convert.ToBase64String(Encoding.UTF8.GetBytes(Nonce))),

                                new XElement(WSU + "Created",
                                             CreatedTimestamp)

                            )

                        )
                    ),

                    new XElement(SOAPNS.NS.SOAPEnvelope + "Body", SOAPBody)

                )
            );

        }

        #endregion

    }

}
