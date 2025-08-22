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
using System.Xml.Linq;
using System.Threading;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHP get roaming authorisation list request.
    /// </summary>
    public class GetRoamingAuthorisationListRequest : ARequest<GetRoamingAuthorisationListRequest>
    {

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP GetRoamingAuthorisationList XML/SOAP request.
        /// </summary>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public GetRoamingAuthorisationListRequest(DateTimeOffset?     Timestamp           = null,
                                                  EventTracking_Id?   EventTrackingId     = null,
                                                  TimeSpan?           RequestTimeout      = null,
                                                  CancellationToken   CancellationToken   = default)

            : base(Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        { }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:OCHP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //
        //      <ns:GetRoamingAuthorisationListRequest />
        //
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(GetRoamingAuthorisationListRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get roaming authorisation list request.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetRoamingAuthorisationListRequest Parse(XElement             GetRoamingAuthorisationListRequestXML,
                                                               OnExceptionDelegate  OnException = null)
        {

            GetRoamingAuthorisationListRequest _GetRoamingAuthorisationListRequest;

            if (TryParse(GetRoamingAuthorisationListRequestXML, out _GetRoamingAuthorisationListRequest, OnException))
                return _GetRoamingAuthorisationListRequest;

            return null;

        }

        #endregion

        #region (static) Parse(GetRoamingAuthorisationListRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get roaming authorisation list request.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetRoamingAuthorisationListRequest Parse(String               GetRoamingAuthorisationListRequestText,
                                                               OnExceptionDelegate  OnException = null)
        {

            GetRoamingAuthorisationListRequest _GetRoamingAuthorisationListRequest;

            if (TryParse(GetRoamingAuthorisationListRequestText, out _GetRoamingAuthorisationListRequest, OnException))
                return _GetRoamingAuthorisationListRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(GetRoamingAuthorisationListRequestXML,  out GetRoamingAuthorisationListRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get roaming authorisation list request.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListRequestXML">The XML to parse.</param>
        /// <param name="GetRoamingAuthorisationListRequest">The parsed get roaming authorisation list request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                GetRoamingAuthorisationListRequestXML,
                                       out GetRoamingAuthorisationListRequest  GetRoamingAuthorisationListRequest,
                                       OnExceptionDelegate                     OnException  = null)
        {

            try
            {

                if (GetRoamingAuthorisationListRequestXML.Name != OCHPNS.Default + "GetRoamingAuthorisationListRequest")
                    throw new ArgumentException("Invalid XML tag!", nameof(GetRoamingAuthorisationListRequestXML));

                GetRoamingAuthorisationListRequest = new GetRoamingAuthorisationListRequest();

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, GetRoamingAuthorisationListRequestXML, e);

                GetRoamingAuthorisationListRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetRoamingAuthorisationListRequestText, out GetRoamingAuthorisationListRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get roaming authorisation list request.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListRequestText">The text to parse.</param>
        /// <param name="GetRoamingAuthorisationListRequest">The parsed get roaming authorisation list request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                  GetRoamingAuthorisationListRequestText,
                                       out GetRoamingAuthorisationListRequest  GetRoamingAuthorisationListRequest,
                                       OnExceptionDelegate                     OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetRoamingAuthorisationListRequestText).Root,
                             out GetRoamingAuthorisationListRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, GetRoamingAuthorisationListRequestText, e);
            }

            GetRoamingAuthorisationListRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetRoamingAuthorisationListRequest");

        #endregion


        #region Operator overloading

        #region Operator == (GetRoamingAuthorisationListRequest1, GetRoamingAuthorisationListRequest2)

        /// <summary>
        /// Compares two get roaming authorisation list requests for equality.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListRequest1">A get roaming authorisation list request.</param>
        /// <param name="GetRoamingAuthorisationListRequest2">Another get roaming authorisation list request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetRoamingAuthorisationListRequest GetRoamingAuthorisationListRequest1, GetRoamingAuthorisationListRequest GetRoamingAuthorisationListRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GetRoamingAuthorisationListRequest1, GetRoamingAuthorisationListRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetRoamingAuthorisationListRequest1 == null) || ((Object) GetRoamingAuthorisationListRequest2 == null))
                return false;

            return GetRoamingAuthorisationListRequest1.Equals(GetRoamingAuthorisationListRequest2);

        }

        #endregion

        #region Operator != (GetRoamingAuthorisationListRequest1, GetRoamingAuthorisationListRequest2)

        /// <summary>
        /// Compares two get roaming authorisation list requests for inequality.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListRequest1">A get roaming authorisation list request.</param>
        /// <param name="GetRoamingAuthorisationListRequest2">Another get roaming authorisation list request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetRoamingAuthorisationListRequest GetRoamingAuthorisationListRequest1, GetRoamingAuthorisationListRequest GetRoamingAuthorisationListRequest2)

            => !(GetRoamingAuthorisationListRequest1 == GetRoamingAuthorisationListRequest2);

        #endregion

        #endregion

        #region IEquatable<GetRoamingAuthorisationListRequest> Members

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

            // Check if the given object is a get roaming authorisation list request.
            var GetRoamingAuthorisationListRequest = Object as GetRoamingAuthorisationListRequest;
            if ((Object) GetRoamingAuthorisationListRequest == null)
                return false;

            return this.Equals(GetRoamingAuthorisationListRequest);

        }

        #endregion

        #region Equals(GetRoamingAuthorisationListRequest)

        /// <summary>
        /// Compares two get roaming authorisation list requests for equality.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListRequest">A get roaming authorisation list request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetRoamingAuthorisationListRequest GetRoamingAuthorisationListRequest)
        {

            if ((Object) GetRoamingAuthorisationListRequest == null)
                return false;

            return Object.ReferenceEquals(this, GetRoamingAuthorisationListRequest);

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => "GetRoamingAuthorisationListRequest";

        #endregion


    }

}
