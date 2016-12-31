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
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

using SOAPNS = org.GraphDefined.Vanaheimr.Hermod.SOAP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
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
        public static XElement Encapsulation(XElement                      SOAPBody,
                                             SOAPNS.XMLNamespacesDelegate  XMLNamespaces = null)
        {

            #region Initial checks

            if (SOAPBody == null)
                throw new ArgumentNullException(nameof(SOAPBody),  "The given XML must not be null!");

            if (XMLNamespaces == null)
                XMLNamespaces = xml => xml;

            #endregion

            return XMLNamespaces(
                new XElement(SOAPNS.NS.SOAPEnvelope_v1_1 + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "SOAP",  SOAPNS.NS.SOAPEnvelope_v1_1.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "OCHP",  OCHPNS.Default.             NamespaceName),

                    new XElement(SOAPNS.NS.SOAPEnvelope_v1_1 + "Header"),
                    new XElement(SOAPNS.NS.SOAPEnvelope_v1_1 + "Body",  SOAPBody)
                )
            );

        }

        /// <summary>
        /// Encapsulate the given XML within a XML SOAP frame.
        /// </summary>
        /// <param name="SOAPBody">The internal XML for the SOAP body.</param>
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        public static XElement Encapsulation(String                        Username,
                                             String                        Password,
                                             XElement                      SOAPBody,
                                             SOAPNS.XMLNamespacesDelegate  XMLNamespaces = null)
        {

            #region Initial checks

            if (Username == null)
                throw new ArgumentNullException(nameof(Username),  "The given XML must not be null!");

            if (Password == null)
                throw new ArgumentNullException(nameof(Password),  "The given XML must not be null!");

            if (SOAPBody == null)
                throw new ArgumentNullException(nameof(SOAPBody),  "The given XML must not be null!");

            if (XMLNamespaces == null)
                XMLNamespaces = xml => xml;

            #endregion


            var WSSE = XNamespace.Get("http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            var WSU  = XNamespace.Get("http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");


            var created         = DateTime.Now.ToUniversalTime();
            var nonce           = getNonce();
            var nonceToSend     = Convert.ToBase64String(Encoding.UTF8.GetBytes(nonce));
            var createdStr      = created.ToString("yyyy-MM-ddTHH:mm:ssZ");
            var passwordToSend  = GetSHA1String(nonce + createdStr + Password);


            return XMLNamespaces(
                new XElement(SOAPNS.NS.SOAPEnvelope_v1_1 + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "SOAP",  SOAPNS.NS.SOAPEnvelope_v1_1.NamespaceName),
                    new XAttribute(XNamespace.Xmlns + "OCHP",  OCHPNS.Default.             NamespaceName),

                    new XElement(SOAPNS.NS.SOAPEnvelope_v1_1 + "Header",
                        new XElement(WSSE + "Security",
                            new XAttribute(SOAPNS.NS.SOAPEnvelope_v1_1 + "mustUnderstand", 1),
                            new XElement(WSSE + "UsernameToken", new XAttribute(WSU + "id", "UsernameToken-27777511"),
                                new XElement(WSSE + "Username", Username),
                                new XElement(WSSE + "Password",
                                    new XAttribute("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"),
                                    Password),
                                new XElement(WSSE + "Nonce",
                                    new XAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"),
                                    nonceToSend),
                                new XElement(WSU + "Created",
                                    createdStr)
                            )
                        )),

                    new XElement(SOAPNS.NS.SOAPEnvelope_v1_1 + "Body", SOAPBody)

                )
            );

        }


        private static string getNonce()
            => Guid.NewGuid().ToString();

        private static string GetSHA1String(string phrase)
        {

            var hashedDataBytes = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(phrase));
            var test            = Convert.ToString(hashedDataBytes);

            return Convert.ToBase64String(hashedDataBytes);

        }

    }

}
