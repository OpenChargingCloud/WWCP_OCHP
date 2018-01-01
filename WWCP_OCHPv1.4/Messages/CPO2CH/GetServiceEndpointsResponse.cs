/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHP get service endpoints response.
    /// </summary>
    public class GetServiceEndpointsResponse : AResponse<GetServiceEndpointsRequest,
                                                         GetServiceEndpointsResponse>
    {

        #region Properties

        /// <summary>
        ///  An enumeration of provider service endpoints.
        /// </summary>
        public IEnumerable<ProviderEndpoint>  ServiceEndpoints   { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The get service endpoints request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetServiceEndpointsResponse OK(GetServiceEndpointsRequest  Request,
                                                     String                      Description = null)

            => new GetServiceEndpointsResponse(Request,
                                               Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The get service endpoints request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetServiceEndpointsResponse Partly(GetServiceEndpointsRequest  Request,
                                                         String                      Description = null)

            => new GetServiceEndpointsResponse(Request,
                                               Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The get service endpoints request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetServiceEndpointsResponse NotAuthorized(GetServiceEndpointsRequest  Request,
                                                                String                      Description = null)

            => new GetServiceEndpointsResponse(Request,
                                               Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The get service endpoints request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetServiceEndpointsResponse InvalidId(GetServiceEndpointsRequest  Request,
                                                            String                      Description = null)

            => new GetServiceEndpointsResponse(Request,
                                               Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The get service endpoints request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetServiceEndpointsResponse Server(GetServiceEndpointsRequest  Request,
                                                         String                      Description = null)

            => new GetServiceEndpointsResponse(Request,
                                               Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The get service endpoints request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetServiceEndpointsResponse Format(GetServiceEndpointsRequest  Request,
                                                         String                      Description = null)

            => new GetServiceEndpointsResponse(Request,
                                               Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP get service endpoints response.
        /// </summary>
        /// <param name="Request">The get service endpoints request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="ServiceEndpoints">An enumeration of operator service endpoints.</param>
        public GetServiceEndpointsResponse(GetServiceEndpointsRequest     Request,
                                           Result                         Result,
                                           IEnumerable<ProviderEndpoint>  ServiceEndpoints = null)

            : base(Request, Result)

        {

            this.ServiceEndpoints  = ServiceEndpoints ?? new ProviderEndpoint[0];

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:GetServiceEndpointsResponse>
        //
        //          <ns:result>
        //
        //             <ns:resultCode>
        //                <ns:resultCode>?</ns:resultCode>
        //             </ns:resultCode>
        //
        //             <ns:resultDescription>?</ns:resultDescription>
        //
        //          </ns:result>
        //
        //          <!--Zero or more repetitions:-->
        //          <ns:providerEndpointArray>
        //             <ns:url>?</ns:url>
        //             <ns:namespaceUrl>?</ns:namespaceUrl>
        //             <ns:accessToken>?</ns:accessToken>
        //             <ns:validDate>?</ns:validDate>
        //             <!--1 or more repetitions:-->
        //             <ns:whitelist>?</ns:whitelist>
        //             <!--Zero or more repetitions:-->
        //             <ns:blacklist>?</ns:blacklist>
        //          </ns:providerEndpointArray>
        //
        //          <!--Zero or more repetitions:-->
        //          <ns:operatorEndpointArray>
        //             <ns:url>?</ns:url>
        //             <ns:namespaceUrl>?</ns:namespaceUrl>
        //             <ns:accessToken>?</ns:accessToken>
        //             <ns:validDate>?</ns:validDate>
        //             <!--1 or more repetitions:-->
        //             <ns:whitelist>?</ns:whitelist>
        //             <!--Zero or more repetitions:-->
        //             <ns:blacklist>?</ns:blacklist>
        //          </ns:operatorEndpointArray>
        //
        //       </ns:GetServiceEndpointsResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, GetServiceEndpointsResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get service endpoints response.
        /// </summary>
        /// <param name="Request">The get service endpoints request leading to this response.</param>
        /// <param name="GetServiceEndpointsResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetServiceEndpointsResponse Parse(GetServiceEndpointsRequest  Request,
                                                        XElement                    GetServiceEndpointsResponseXML,
                                                        OnExceptionDelegate         OnException = null)
        {

            GetServiceEndpointsResponse _GetServiceEndpointsResponse;

            if (TryParse(Request, GetServiceEndpointsResponseXML, out _GetServiceEndpointsResponse, OnException))
                return _GetServiceEndpointsResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, GetServiceEndpointsResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get service endpoints response.
        /// </summary>
        /// <param name="Request">The get service endpoints request leading to this response.</param>
        /// <param name="GetServiceEndpointsResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetServiceEndpointsResponse Parse(GetServiceEndpointsRequest  Request,
                                                        String                      GetServiceEndpointsResponseText,
                                                        OnExceptionDelegate         OnException = null)
        {

            GetServiceEndpointsResponse _GetServiceEndpointsResponse;

            if (TryParse(Request, GetServiceEndpointsResponseText, out _GetServiceEndpointsResponse, OnException))
                return _GetServiceEndpointsResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, GetServiceEndpointsResponseXML,  out GetServiceEndpointsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get service endpoints response.
        /// </summary>
        /// <param name="Request">The get service endpoints request leading to this response.</param>
        /// <param name="GetServiceEndpointsResponseXML">The XML to parse.</param>
        /// <param name="GetServiceEndpointsResponse">The parsed get service endpoints response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetServiceEndpointsRequest       Request,
                                       XElement                         GetServiceEndpointsResponseXML,
                                       out GetServiceEndpointsResponse  GetServiceEndpointsResponse,
                                       OnExceptionDelegate              OnException  = null)
        {

            try
            {

                GetServiceEndpointsResponse = new GetServiceEndpointsResponse(

                                                  Request,

                                                  GetServiceEndpointsResponseXML.MapElementOrFail (OCHPNS.Default + "result",
                                                                                                   Result.Parse,
                                                                                                   OnException),

                                                  GetServiceEndpointsResponseXML.MapElementsOrFail(OCHPNS.Default + "operatorEndpointArray",
                                                                                                   ProviderEndpoint.Parse,
                                                                                                   OnException)

                                              );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, GetServiceEndpointsResponseXML, e);

                GetServiceEndpointsResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, GetServiceEndpointsResponseText, out GetServiceEndpointsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get service endpoints response.
        /// </summary>
        /// <param name="Request">The get service endpoints request leading to this response.</param>
        /// <param name="GetServiceEndpointsResponseText">The text to parse.</param>
        /// <param name="GetServiceEndpointsResponse">The parsed get service endpoints response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetServiceEndpointsRequest       Request,
                                       String                           GetServiceEndpointsResponseText,
                                       out GetServiceEndpointsResponse  GetServiceEndpointsResponse,
                                       OnExceptionDelegate              OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(GetServiceEndpointsResponseText).Root,
                             out GetServiceEndpointsResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, GetServiceEndpointsResponseText, e);
            }

            GetServiceEndpointsResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetServiceEndpointsResponse",

                   Result.ToXML(),

                   ServiceEndpoints.SafeSelect(endpoint => endpoint.ToXML(OCHPNS.Default + "operatorEndpointArray"))

               );

        #endregion


        #region Operator overloading

        #region Operator == (GetServiceEndpointsResponse1, GetServiceEndpointsResponse2)

        /// <summary>
        /// Compares two get service endpoints responses for equality.
        /// </summary>
        /// <param name="GetServiceEndpointsResponse1">A get service endpoints response.</param>
        /// <param name="GetServiceEndpointsResponse2">Another get service endpoints response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetServiceEndpointsResponse GetServiceEndpointsResponse1, GetServiceEndpointsResponse GetServiceEndpointsResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetServiceEndpointsResponse1, GetServiceEndpointsResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetServiceEndpointsResponse1 == null) || ((Object) GetServiceEndpointsResponse2 == null))
                return false;

            return GetServiceEndpointsResponse1.Equals(GetServiceEndpointsResponse2);

        }

        #endregion

        #region Operator != (GetServiceEndpointsResponse1, GetServiceEndpointsResponse2)

        /// <summary>
        /// Compares two get service endpoints responses for inequality.
        /// </summary>
        /// <param name="GetServiceEndpointsResponse1">A get service endpoints response.</param>
        /// <param name="GetServiceEndpointsResponse2">Another get service endpoints response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetServiceEndpointsResponse GetServiceEndpointsResponse1, GetServiceEndpointsResponse GetServiceEndpointsResponse2)

            => !(GetServiceEndpointsResponse1 == GetServiceEndpointsResponse2);

        #endregion

        #endregion

        #region IEquatable<GetServiceEndpointsResponse> Members

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

            // Check if the given object is a get service endpoints response.
            var GetServiceEndpointsResponse = Object as GetServiceEndpointsResponse;
            if ((Object) GetServiceEndpointsResponse == null)
                return false;

            return this.Equals(GetServiceEndpointsResponse);

        }

        #endregion

        #region Equals(GetServiceEndpointsResponse)

        /// <summary>
        /// Compares two get service endpoints responses for equality.
        /// </summary>
        /// <param name="GetServiceEndpointsResponse">A get service endpoints response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetServiceEndpointsResponse GetServiceEndpointsResponse)
        {

            if ((Object) GetServiceEndpointsResponse == null)
                return false;

            return this.Result. Equals(GetServiceEndpointsResponse.Result);

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

                return ServiceEndpoints != null

                           ? Result.GetHashCode() * 11 ^
                             ServiceEndpoints.SafeSelect(endpoint => endpoint.GetHashCode()).Aggregate((a, b) => a ^ b)

                           : Result.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Result,
                             ServiceEndpoints.Any()
                                 ? " " + ServiceEndpoints.Count() + " service endpoints"
                                 : "");

        #endregion

    }

}
