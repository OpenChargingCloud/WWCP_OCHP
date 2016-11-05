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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHP get service endpoints request.
    /// </summary>
    public class GetServiceEndpointsRequest : ARequest<GetServiceEndpointsRequest>
    {

        #region Documentation

    // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
    //                   xmlns:OCHP    = "http://ochp.eu/1.4">
    //
    //    <soapenv:Header/>
    //    <soapenv:Body>
    //
    //      <ns:GetServiceEndpointsRequest />
    //
    //    </soapenv:Body>
    // </soapenv:Envelope>

    #endregion

    #region (static) Parse(GetServiceEndpointsRequestXML,  OnException = null)

    /// <summary>
    /// Parse the given XML representation of an OCHP get service endpoints request.
    /// </summary>
    /// <param name="GetServiceEndpointsRequestXML">The XML to parse.</param>
    /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
    public static GetServiceEndpointsRequest Parse(XElement             GetServiceEndpointsRequestXML,
                                                       OnExceptionDelegate  OnException = null)
        {

            GetServiceEndpointsRequest _GetServiceEndpointsRequest;

            if (TryParse(GetServiceEndpointsRequestXML, out _GetServiceEndpointsRequest, OnException))
                return _GetServiceEndpointsRequest;

            return null;

        }

        #endregion

        #region (static) Parse(GetServiceEndpointsRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get service endpoints request.
        /// </summary>
        /// <param name="GetServiceEndpointsRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetServiceEndpointsRequest Parse(String               GetServiceEndpointsRequestText,
                                                       OnExceptionDelegate  OnException = null)
        {

            GetServiceEndpointsRequest _GetServiceEndpointsRequest;

            if (TryParse(GetServiceEndpointsRequestText, out _GetServiceEndpointsRequest, OnException))
                return _GetServiceEndpointsRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(GetServiceEndpointsRequestXML,  out GetServiceEndpointsRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get service endpoints request.
        /// </summary>
        /// <param name="GetServiceEndpointsRequestXML">The XML to parse.</param>
        /// <param name="GetServiceEndpointsRequest">The parsed get service endpoints request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                        GetServiceEndpointsRequestXML,
                                       out GetServiceEndpointsRequest  GetServiceEndpointsRequest,
                                       OnExceptionDelegate             OnException  = null)
        {

            try
            {

                if (GetServiceEndpointsRequestXML.Name != OCHPNS.Default + "GetServiceEndpointsRequest")
                    throw new ArgumentException("Invalid XML tag!", nameof(GetServiceEndpointsRequestXML));

                GetServiceEndpointsRequest = new GetServiceEndpointsRequest();

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, GetServiceEndpointsRequestXML, e);

                GetServiceEndpointsRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetServiceEndpointsRequestText, out GetServiceEndpointsRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get service endpoints request.
        /// </summary>
        /// <param name="GetServiceEndpointsRequestText">The text to parse.</param>
        /// <param name="GetServiceEndpointsRequest">The parsed get service endpoints request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                          GetServiceEndpointsRequestText,
                                       out GetServiceEndpointsRequest  GetServiceEndpointsRequest,
                                       OnExceptionDelegate             OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetServiceEndpointsRequestText).Root,
                             out GetServiceEndpointsRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, GetServiceEndpointsRequestText, e);
            }

            GetServiceEndpointsRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetServiceEndpointsRequest");

        #endregion


        #region Operator overloading

        #region Operator == (GetServiceEndpointsRequest1, GetServiceEndpointsRequest2)

        /// <summary>
        /// Compares two get service endpoints requests for equality.
        /// </summary>
        /// <param name="GetServiceEndpointsRequest1">A get service endpoints request.</param>
        /// <param name="GetServiceEndpointsRequest2">Another get service endpoints request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetServiceEndpointsRequest GetServiceEndpointsRequest1, GetServiceEndpointsRequest GetServiceEndpointsRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetServiceEndpointsRequest1, GetServiceEndpointsRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetServiceEndpointsRequest1 == null) || ((Object) GetServiceEndpointsRequest2 == null))
                return false;

            return GetServiceEndpointsRequest1.Equals(GetServiceEndpointsRequest2);

        }

        #endregion

        #region Operator != (GetServiceEndpointsRequest1, GetServiceEndpointsRequest2)

        /// <summary>
        /// Compares two get service endpoints requests for inequality.
        /// </summary>
        /// <param name="GetServiceEndpointsRequest1">A get service endpoints request.</param>
        /// <param name="GetServiceEndpointsRequest2">Another get service endpoints request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetServiceEndpointsRequest GetServiceEndpointsRequest1, GetServiceEndpointsRequest GetServiceEndpointsRequest2)

            => !(GetServiceEndpointsRequest1 == GetServiceEndpointsRequest2);

        #endregion

        #endregion

        #region IEquatable<GetServiceEndpointsRequest> Members

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

            // Check if the given object is a get service endpoints request.
            var GetServiceEndpointsRequest = Object as GetServiceEndpointsRequest;
            if ((Object) GetServiceEndpointsRequest == null)
                return false;

            return this.Equals(GetServiceEndpointsRequest);

        }

        #endregion

        #region Equals(GetServiceEndpointsRequest)

        /// <summary>
        /// Compares two get service endpoints requests for equality.
        /// </summary>
        /// <param name="GetServiceEndpointsRequest">A get service endpoints request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetServiceEndpointsRequest GetServiceEndpointsRequest)
        {

            if ((Object) GetServiceEndpointsRequest == null)
                return false;

            return Object.ReferenceEquals(this, GetServiceEndpointsRequest);

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

                return base.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => "GetServiceEndpointsRequest";

        #endregion


    }

}
