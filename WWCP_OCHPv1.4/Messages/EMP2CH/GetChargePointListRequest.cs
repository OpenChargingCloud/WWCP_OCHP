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

namespace cloud.charging.open.protocols.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHP GetChargePointList request.
    /// </summary>
    public class GetChargePointListRequest : ARequest<GetChargePointListRequest>
    {

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP GetChargePointList XML/SOAP request.
        /// </summary>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public GetChargePointListRequest(DateTimeOffset?     Timestamp           = null,
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
        //      <ns:GetChargePointListRequest />
        //
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(GetChargePointListRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get charge point list request.
        /// </summary>
        /// <param name="GetChargePointListRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetChargePointListRequest Parse(XElement             GetChargePointListRequestXML,
                                                      OnExceptionDelegate  OnException = null)
        {

            GetChargePointListRequest _GetChargePointListRequest;

            if (TryParse(GetChargePointListRequestXML, out _GetChargePointListRequest, OnException))
                return _GetChargePointListRequest;

            return null;

        }

        #endregion

        #region (static) Parse(GetChargePointListRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get charge point list request.
        /// </summary>
        /// <param name="GetChargePointListRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetChargePointListRequest Parse(String               GetChargePointListRequestText,
                                                      OnExceptionDelegate  OnException = null)
        {

            GetChargePointListRequest _GetChargePointListRequest;

            if (TryParse(GetChargePointListRequestText, out _GetChargePointListRequest, OnException))
                return _GetChargePointListRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(GetChargePointListRequestXML,  out GetChargePointListRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get charge point list request.
        /// </summary>
        /// <param name="GetChargePointListRequestXML">The XML to parse.</param>
        /// <param name="GetChargePointListRequest">The parsed get charge point list request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                       GetChargePointListRequestXML,
                                       out GetChargePointListRequest  GetChargePointListRequest,
                                       OnExceptionDelegate            OnException  = null)
        {

            try
            {

                if (GetChargePointListRequestXML.Name != OCHPNS.Default + "GetChargePointListRequest")
                    throw new ArgumentException("Invalid XML tag!", nameof(GetChargePointListRequestXML));

                GetChargePointListRequest = new GetChargePointListRequest();

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, GetChargePointListRequestXML, e);

                GetChargePointListRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetChargePointListRequestText, out GetChargePointListRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get charge point list request.
        /// </summary>
        /// <param name="GetChargePointListRequestText">The text to parse.</param>
        /// <param name="GetChargePointListRequest">The parsed get charge point list request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                         GetChargePointListRequestText,
                                       out GetChargePointListRequest  GetChargePointListRequest,
                                       OnExceptionDelegate            OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetChargePointListRequestText).Root,
                             out GetChargePointListRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, GetChargePointListRequestText, e);
            }

            GetChargePointListRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetChargePointListRequest");

        #endregion


        #region Operator overloading

        #region Operator == (GetChargePointListRequest1, GetChargePointListRequest2)

        /// <summary>
        /// Compares two get charge point list requests for equality.
        /// </summary>
        /// <param name="GetChargePointListRequest1">A get charge point list request.</param>
        /// <param name="GetChargePointListRequest2">Another get charge point list request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetChargePointListRequest GetChargePointListRequest1, GetChargePointListRequest GetChargePointListRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GetChargePointListRequest1, GetChargePointListRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetChargePointListRequest1 == null) || ((Object) GetChargePointListRequest2 == null))
                return false;

            return GetChargePointListRequest1.Equals(GetChargePointListRequest2);

        }

        #endregion

        #region Operator != (GetChargePointListRequest1, GetChargePointListRequest2)

        /// <summary>
        /// Compares two get charge point list requests for inequality.
        /// </summary>
        /// <param name="GetChargePointListRequest1">A get charge point list request.</param>
        /// <param name="GetChargePointListRequest2">Another get charge point list request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetChargePointListRequest GetChargePointListRequest1, GetChargePointListRequest GetChargePointListRequest2)

            => !(GetChargePointListRequest1 == GetChargePointListRequest2);

        #endregion

        #endregion

        #region IEquatable<GetChargePointListRequest> Members

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

            // Check if the given object is a get charge point list request.
            var GetChargePointListRequest = Object as GetChargePointListRequest;
            if ((Object) GetChargePointListRequest == null)
                return false;

            return this.Equals(GetChargePointListRequest);

        }

        #endregion

        #region Equals(GetChargePointListRequest)

        /// <summary>
        /// Compares two get charge point list requests for equality.
        /// </summary>
        /// <param name="GetChargePointListRequest">A get charge point list request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetChargePointListRequest GetChargePointListRequest)
        {

            if ((Object) GetChargePointListRequest == null)
                return false;

            return Object.ReferenceEquals(this, GetChargePointListRequest);

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

            => "GetChargePointListRequest";

        #endregion


    }

}
