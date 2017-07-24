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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// An OCHPdirect provider endpoint.
    /// </summary>
    public class ProviderEndpoint : ADirectEndpoint
    {

        #region Data

        /// <summary>
        /// The regular expression for parsing contract Ids.
        /// </summary>
        public static readonly Regex ContractIdPattern_RegEx = new Regex(@"^[A-Za-z]{2}[A-Za-z0-9]{3}[Cc][A-Za-z0-9]{0,8}%?$",
                                                                         RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// An enumeration of patterns that match all Contract Ids the endpoint is responsible for.
        /// </summary>
        public IEnumerable<String>  WhiteList   { get; }

        /// <summary>
        /// An optional enumeration of patterns that match Contract Ids the endpoint is not
        /// responsible for, but are matched by the whitelist.
        /// </summary>
        public IEnumerable<String>  BlackList   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHPdirect proivder endpoint.
        /// </summary>
        /// <param name="URL">The endpoint address.</param>
        /// <param name="NamespaceURL">The WSDL namespace definition.</param>
        /// <param name="AccessToken">The secret token to access this endpoint.</param>
        /// <param name="ValidDate">The date on which this endpoint/token combination is valid.</param>
        /// <param name="WhiteList">An enumeration of patterns that match all Contract Ids the endpoint is responsible for.</param>
        /// <param name="BlackList">An optional enumeration of patterns that match Contract Ids the endpoint is not responsible for, but are matched by the whitelist.</param>
        public ProviderEndpoint(String               URL,
                                String               NamespaceURL,
                                String               AccessToken,
                                String               ValidDate,
                                IEnumerable<String>  WhiteList,
                                IEnumerable<String>  BlackList  = null)

            : base(URL,
                   NamespaceURL,
                   AccessToken,
                   ValidDate)

        {

            #region Initial checks

            if (!WhiteList.IsNeitherNullNorEmpty())
                throw new ArgumentNullException(nameof(WhiteList),  "The whitelist of ContractIds must not be null or empty!");

            #endregion

            this.WhiteList  = WhiteList;
            this.BlackList  = BlackList;

        }

        #endregion


        #region Documentation

        // <ns:providerEndpointArray>
        //
        //    <ns:url>?</ns:url>
        //    <ns:namespaceUrl>?</ns:namespaceUrl>
        //    <ns:accessToken>?</ns:accessToken>
        //    <ns:validDate>?</ns:validDate>
        //
        //    <!--1 or more repetitions:-->
        //    <ns:whitelist>?</ns:whitelist>
        //
        //    <!--Zero or more repetitions:-->
        //    <ns:blacklist>?</ns:blacklist>
        //
        // </ns:providerEndpointArray>

        #endregion

        #region (static) Parse(ProviderEndpointXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of OCHP provider endpoint.
        /// </summary>
        /// <param name="ProviderEndpointXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ProviderEndpoint Parse(XElement             ProviderEndpointXML,
                                             OnExceptionDelegate  OnException = null)
        {

            ProviderEndpoint _ProviderEndpoint;

            if (TryParse(ProviderEndpointXML, out _ProviderEndpoint, OnException))
                return _ProviderEndpoint;

            return null;

        }

        #endregion

        #region (static) Parse(ProviderEndpointText, OnException = null)

        /// <summary>
        /// Parse the given text representation of OCHP provider endpoint.
        /// </summary>
        /// <param name="ProviderEndpointText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ProviderEndpoint Parse(String               ProviderEndpointText,
                                             OnExceptionDelegate  OnException = null)
        {

            ProviderEndpoint _ProviderEndpoint;

            if (TryParse(ProviderEndpointText, out _ProviderEndpoint, OnException))
                return _ProviderEndpoint;

            return null;

        }

        #endregion

        #region (static) TryParse(ProviderEndpointXML,  out ProviderEndpoint, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of OCHP provider endpoint.
        /// </summary>
        /// <param name="ProviderEndpointXML">The XML to parse.</param>
        /// <param name="ProviderEndpoint">The parsed provider endpoint.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement              ProviderEndpointXML,
                                       out ProviderEndpoint  ProviderEndpoint,
                                       OnExceptionDelegate   OnException  = null)
        {

            try
            {

                ProviderEndpoint = new ProviderEndpoint(

                                       ProviderEndpointXML.ElementValueOrFail(OCHPNS.Default + "url"),
                                       ProviderEndpointXML.ElementValueOrFail(OCHPNS.Default + "namespaceUrl"),
                                       ProviderEndpointXML.ElementValueOrFail(OCHPNS.Default + "accesstoken"),
                                       ProviderEndpointXML.ElementValueOrFail(OCHPNS.Default + "validDate"),

                                       ProviderEndpointXML.MapValuesOrFail   (OCHPNS.Default + "whitelist",
                                                                              s => s),

                                       ProviderEndpointXML.MapValuesOrFail   (OCHPNS.Default + "blacklist",
                                                                              s => s)

                                   );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, ProviderEndpointXML, e);

                ProviderEndpoint = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ProviderEndpointText, out ProviderEndpoint, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of OCHP provider endpoint.
        /// </summary>
        /// <param name="ProviderEndpointText">The text to parse.</param>
        /// <param name="ProviderEndpoint">The parsed provider endpoint.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                ProviderEndpointText,
                                       out ProviderEndpoint  ProviderEndpoint,
                                       OnExceptionDelegate   OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ProviderEndpointText).Root,
                             out ProviderEndpoint,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, ProviderEndpointText, e);
            }

            ProviderEndpoint = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">An alternative XML element name [default: "OCHPNS:providerEndpoint"]</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OCHPNS.Default + "providerEndpoint",

                   new XElement(OCHPNS.Default + "url",           URL),
                   new XElement(OCHPNS.Default + "namespaceUrl",  NamespaceURL),
                   new XElement(OCHPNS.Default + "accesstoken",   AccessToken),
                   new XElement(OCHPNS.Default + "validDate",     ValidDate),

                   WhiteList.      Select(item => new XElement(OCHPNS.Default + "whitelist",  item)),

                   BlackList.IsNeitherNullNorEmpty()
                       ? BlackList.Select(item => new XElement(OCHPNS.Default + "blacklist",  item))
                       : null

               );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(base.ToString(),
                             " having ",
                             WhiteList.IsNeitherNullNorEmpty()
                                 ? WhiteList.Count() + " whitelist entries"
                                 : "",
                             BlackList.IsNeitherNullNorEmpty()
                                 ? " and " + BlackList.Count() + " blacklist entries"
                                 : "");

        #endregion

    }

}
