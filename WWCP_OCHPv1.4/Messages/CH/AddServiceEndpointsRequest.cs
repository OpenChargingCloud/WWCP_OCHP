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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.CH
{

    /// <summary>
    /// An OCHP add service endpoints request.
    /// </summary>
    public class AddServiceEndpointsRequest : ARequest<AddServiceEndpointsRequest>
    {

        #region Properties

        /// <summary>
        /// An enumeration of operator endpoints.
        /// </summary>
        public IEnumerable<OperatorEndpoint>  OperatorEndpoints   { get; }

        /// <summary>
        /// An enumeration of provider endpoints.
        /// </summary>
        public IEnumerable<ProviderEndpoint>  ProviderEndpoints   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP AddServiceEndpoints XML/SOAP request.
        /// </summary>
        /// <param name="OperatorEndpoints">An enumeration of operator endpoints.</param>
        /// <param name="ProviderEndpoints">An enumeration of provider endpoints.</param>
        public AddServiceEndpointsRequest(IEnumerable<OperatorEndpoint>  OperatorEndpoints = null,
                                          IEnumerable<ProviderEndpoint>  ProviderEndpoints = null)
        {

            #region Initial checks

            if (OperatorEndpoints.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(OperatorEndpoints),  "The given enumeration of operator endpoints must not be null or empty!");

            if (ProviderEndpoints.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(ProviderEndpoints),  "The given enumeration of provider endpoints must not be null or empty!");

            #endregion

            this.OperatorEndpoints = OperatorEndpoints ?? new OperatorEndpoint[0];
            this.ProviderEndpoints = ProviderEndpoints ?? new ProviderEndpoint[0];

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:OCHP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <OCHP:AddServiceEndpointsRequest>
        //
        //          <!--Zero or more repetitions:-->
        //          <OCHP:providerEndpointArray>
        //             <OCHP:url>?</ns:url>
        //             <OCHP:namespaceUrl>?</ns:namespaceUrl>
        //             <OCHP:accessToken>?</ns:accessToken>
        //             <OCHP:validDate>?</ns:validDate>
        //             <!--1 or more repetitions:-->
        //             <OCHP:whitelist>?</ns:whitelist>
        //             <!--Zero or more repetitions:-->
        //             <OCHP:blacklist>?</ns:blacklist>
        //          </ns:providerEndpointArray>
        //
        //          <!--Zero or more repetitions:-->
        //          <OCHP:operatorEndpointArray>
        //             <OCHP:url>?</ns:url>
        //             <OCHP:namespaceUrl>?</ns:namespaceUrl>
        //             <OCHP:accessToken>?</ns:accessToken>
        //             <OCHP:validDate>?</ns:validDate>
        //             <!--1 or more repetitions:-->
        //             <OCHP:whitelist>?</ns:whitelist>
        //             <!--Zero or more repetitions:-->
        //             <OCHP:blacklist>?</ns:blacklist>
        //          </ns:operatorEndpointArray>
        //
        //       </ns:AddServiceEndpointsRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(AddServiceEndpointsRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP add service endpoints request.
        /// </summary>
        /// <param name="AddServiceEndpointsRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AddServiceEndpointsRequest Parse(XElement             AddServiceEndpointsRequestXML,
                                                       OnExceptionDelegate  OnException = null)
        {

            AddServiceEndpointsRequest _AddServiceEndpointsRequest;

            if (TryParse(AddServiceEndpointsRequestXML, out _AddServiceEndpointsRequest, OnException))
                return _AddServiceEndpointsRequest;

            return null;

        }

        #endregion

        #region (static) Parse(AddServiceEndpointsRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP add service endpoints request.
        /// </summary>
        /// <param name="AddServiceEndpointsRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AddServiceEndpointsRequest Parse(String               AddServiceEndpointsRequestText,
                                                       OnExceptionDelegate  OnException = null)
        {

            AddServiceEndpointsRequest _AddServiceEndpointsRequest;

            if (TryParse(AddServiceEndpointsRequestText, out _AddServiceEndpointsRequest, OnException))
                return _AddServiceEndpointsRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(AddServiceEndpointsRequestXML,  out AddServiceEndpointsRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP add service endpoints request.
        /// </summary>
        /// <param name="AddServiceEndpointsRequestXML">The XML to parse.</param>
        /// <param name="AddServiceEndpointsRequest">The parsed add service endpoints request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                        AddServiceEndpointsRequestXML,
                                       out AddServiceEndpointsRequest  AddServiceEndpointsRequest,
                                       OnExceptionDelegate             OnException  = null)
        {

            try
            {

                AddServiceEndpointsRequest = new AddServiceEndpointsRequest(

                                                 AddServiceEndpointsRequestXML.MapElements(OCHPNS.Default + "operatorEndpointArray",
                                                                                           OperatorEndpoint.Parse,
                                                                                           OnException),

                                                 AddServiceEndpointsRequestXML.MapElements(OCHPNS.Default + "providerEndpointArray",
                                                                                           ProviderEndpoint.Parse,
                                                                                           OnException)

                                             );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, AddServiceEndpointsRequestXML, e);

                AddServiceEndpointsRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(AddServiceEndpointsRequestText, out AddServiceEndpointsRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP add service endpoints request.
        /// </summary>
        /// <param name="AddServiceEndpointsRequestText">The text to parse.</param>
        /// <param name="AddServiceEndpointsRequest">The parsed add service endpoints request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                          AddServiceEndpointsRequestText,
                                       out AddServiceEndpointsRequest  AddServiceEndpointsRequest,
                                       OnExceptionDelegate             OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(AddServiceEndpointsRequestText).Root,
                             out AddServiceEndpointsRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, AddServiceEndpointsRequestText, e);
            }

            AddServiceEndpointsRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "AddServiceEndpointsRequest",

                                OperatorEndpoints.Select(endpoints => endpoints.ToXML(OCHPNS.Default + "operatorEndpointArray")).
                                                  ToArray(),

                                ProviderEndpoints.Select(endpoints => endpoints.ToXML(OCHPNS.Default + "providerEndpointArray")).
                                                  ToArray()

                           );

        #endregion


        #region Operator overloading

        #region Operator == (AddServiceEndpointsRequest1, AddServiceEndpointsRequest2)

        /// <summary>
        /// Compares two add service endpoints requests for equality.
        /// </summary>
        /// <param name="AddServiceEndpointsRequest1">A add service endpoints request.</param>
        /// <param name="AddServiceEndpointsRequest2">Another add service endpoints request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AddServiceEndpointsRequest AddServiceEndpointsRequest1, AddServiceEndpointsRequest AddServiceEndpointsRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(AddServiceEndpointsRequest1, AddServiceEndpointsRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AddServiceEndpointsRequest1 is null) || ((Object) AddServiceEndpointsRequest2 is null))
                return false;

            return AddServiceEndpointsRequest1.Equals(AddServiceEndpointsRequest2);

        }

        #endregion

        #region Operator != (AddServiceEndpointsRequest1, AddServiceEndpointsRequest2)

        /// <summary>
        /// Compares two add service endpoints requests for inequality.
        /// </summary>
        /// <param name="AddServiceEndpointsRequest1">A add service endpoints request.</param>
        /// <param name="AddServiceEndpointsRequest2">Another add service endpoints request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AddServiceEndpointsRequest AddServiceEndpointsRequest1, AddServiceEndpointsRequest AddServiceEndpointsRequest2)

            => !(AddServiceEndpointsRequest1 == AddServiceEndpointsRequest2);

        #endregion

        #endregion

        #region IEquatable<AddServiceEndpointsRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is null)
                return false;

            // Check if the given object is a add service endpoints request.
            var AddServiceEndpointsRequest = Object as AddServiceEndpointsRequest;
            if ((Object) AddServiceEndpointsRequest is null)
                return false;

            return this.Equals(AddServiceEndpointsRequest);

        }

        #endregion

        #region Equals(AddServiceEndpointsRequest)

        /// <summary>
        /// Compares two add service endpoints requests for equality.
        /// </summary>
        /// <param name="AddServiceEndpointsRequest">A add service endpoints request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AddServiceEndpointsRequest AddServiceEndpointsRequest)
        {

            if ((Object) AddServiceEndpointsRequest is null)
                return false;

            return OperatorEndpoints.Count().Equals(AddServiceEndpointsRequest.OperatorEndpoints.Count()) &&
                   ProviderEndpoints.Count().Equals(AddServiceEndpointsRequest.ProviderEndpoints.Count());

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

                return OperatorEndpoints.GetHashCode() * 11 ^
                       ProviderEndpoints.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(OperatorEndpoints.Any()
                                 ? OperatorEndpoints.Count() + " operator endpoint(s)"
                                 : "",

                             ProviderEndpoints.Any()
                                 ? ProviderEndpoints.Count() + " provider endpoint(s)"
                                 : "");

        #endregion


    }

}
