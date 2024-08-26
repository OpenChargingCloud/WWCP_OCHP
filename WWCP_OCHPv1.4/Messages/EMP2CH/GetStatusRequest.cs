/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// An OCHP get status request.
    /// </summary>
    public class GetStatusRequest : ARequest<GetStatusRequest>
    {

        #region Properties

        /// <summary>
        /// Only return status data newer than the given timestamp.
        /// </summary>
        public DateTime?     LastRequest   { get; }

        /// <summary>
        /// A status type filter.
        /// </summary>
        public StatusTypes?  StatusType    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP GetStatusRequest XML/SOAP request.
        /// </summary>
        /// <param name="LastRequest">Only return status data newer than the given timestamp.</param>
        /// <param name="StatusType">A status type filter.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public GetStatusRequest(DateTime?           LastRequest         = null,
                                StatusTypes?        StatusType          = null,

                                DateTime?           Timestamp           = null,
                                EventTracking_Id?   EventTrackingId     = null,
                                TimeSpan?           RequestTimeout      = null,
                                CancellationToken   CancellationToken   = default)

            : base(Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.LastRequest  = LastRequest;
            this.StatusType   = StatusType;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:OCHP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <OCHP:GetStatusRequest>
        //
        //         <!--Optional:-->
        //         <OCHP:startDateTime>
        //            <ns:DateTime>?</ns:DateTime>
        //         </OCHP:startDateTime>
        //
        //         <!--Optional:-->
        //         <OCHP:statusType>?</OCHP:statusType>
        //
        //      </OCHP:GetStatusRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(GetStatusRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get status request.
        /// </summary>
        /// <param name="GetStatusRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetStatusRequest Parse(XElement             GetStatusRequestXML,
                                             OnExceptionDelegate  OnException = null)
        {

            GetStatusRequest _GetStatusRequest;

            if (TryParse(GetStatusRequestXML, out _GetStatusRequest, OnException))
                return _GetStatusRequest;

            return null;

        }

        #endregion

        #region (static) Parse(GetStatusRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get status request.
        /// </summary>
        /// <param name="GetStatusRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetStatusRequest Parse(String               GetStatusRequestText,
                                             OnExceptionDelegate  OnException = null)
        {

            GetStatusRequest _GetStatusRequest;

            if (TryParse(GetStatusRequestText, out _GetStatusRequest, OnException))
                return _GetStatusRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(GetStatusRequestXML,  out GetStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get status request.
        /// </summary>
        /// <param name="GetStatusRequestXML">The XML to parse.</param>
        /// <param name="GetStatusRequest">The parsed get status request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement              GetStatusRequestXML,
                                       out GetStatusRequest  GetStatusRequest,
                                       OnExceptionDelegate   OnException  = null)
        {

            try
            {

                GetStatusRequest = new GetStatusRequest(

                                       GetStatusRequestXML.MapValueOrNullable(OCHPNS.Default + "startDateTime",
                                                                              OCHPNS.Default + "DateTime",
                                                                              DateTime.Parse),

                                       GetStatusRequestXML.MapEnumValues     (OCHPNS.Default + "statusType",
                                                                              XML_IO.AsStatusType)

                                   );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, GetStatusRequestXML, e);

                GetStatusRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetStatusRequestText, out GetStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get status request.
        /// </summary>
        /// <param name="GetStatusRequestText">The text to parse.</param>
        /// <param name="GetStatusRequest">The parsed get status request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                GetStatusRequestText,
                                       out GetStatusRequest  GetStatusRequest,
                                       OnExceptionDelegate   OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetStatusRequestText).Root,
                             out GetStatusRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, GetStatusRequestText, e);
            }

            GetStatusRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetStatusRequest",

                                LastRequest.HasValue
                                    ? new XElement(OCHPNS.Default + "startDateTime",
                                          new XElement(OCHPNS.Default + "DateTime",
                                              LastRequest.Value.ToIso8601(false)))
                                    : null,

                                StatusType.HasValue
                                    ? new XElement(OCHPNS.Default + "statusType", StatusType.Value.ToString().ToLower())
                                    : null

                           );

        #endregion


        #region Operator overloading

        #region Operator == (GetStatusRequest1, GetStatusRequest2)

        /// <summary>
        /// Compares two get status requests for equality.
        /// </summary>
        /// <param name="GetStatusRequest1">A get status request.</param>
        /// <param name="GetStatusRequest2">Another get status request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetStatusRequest GetStatusRequest1, GetStatusRequest GetStatusRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GetStatusRequest1, GetStatusRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetStatusRequest1 == null) || ((Object) GetStatusRequest2 == null))
                return false;

            return GetStatusRequest1.Equals(GetStatusRequest2);

        }

        #endregion

        #region Operator != (GetStatusRequest1, GetStatusRequest2)

        /// <summary>
        /// Compares two get status requests for inequality.
        /// </summary>
        /// <param name="GetStatusRequest1">A get status request.</param>
        /// <param name="GetStatusRequest2">Another get status request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetStatusRequest GetStatusRequest1, GetStatusRequest GetStatusRequest2)

            => !(GetStatusRequest1 == GetStatusRequest2);

        #endregion

        #endregion

        #region IEquatable<GetStatusRequest> Members

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

            // Check if the given object is a get status request.
            var GetStatusRequest = Object as GetStatusRequest;
            if ((Object) GetStatusRequest == null)
                return false;

            return this.Equals(GetStatusRequest);

        }

        #endregion

        #region Equals(GetStatusRequest)

        /// <summary>
        /// Compares two get status requests for equality.
        /// </summary>
        /// <param name="GetStatusRequest">A get status request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetStatusRequest GetStatusRequest)
        {

            if ((Object) GetStatusRequest == null)
                return false;

            return ((!LastRequest.HasValue && !GetStatusRequest.LastRequest.HasValue) ||
                     (LastRequest.HasValue &&  GetStatusRequest.LastRequest.HasValue && LastRequest.Value.Equals(GetStatusRequest.LastRequest.Value))) &&

                   ((!StatusType.HasValue  && !GetStatusRequest.StatusType. HasValue) ||
                     (StatusType.HasValue  &&  GetStatusRequest.StatusType. HasValue && StatusType. Value.Equals(GetStatusRequest.StatusType. Value)));

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

                return (LastRequest.HasValue
                            ? LastRequest.GetHashCode()
                            : 0) ^

                       (StatusType.HasValue
                            ? StatusType.GetHashCode()
                            : 0) ^

                       (!LastRequest.HasValue && !StatusType.HasValue
                            ? base.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(LastRequest.HasValue
                                 ? "last request: " + LastRequest.Value.ToIso8601()
                                 : "",

                             LastRequest.HasValue && StatusType.HasValue
                                 ? ", "
                                 : "",

                             StatusType.HasValue
                                 ? "status type: " + StatusType.Value.ToString()
                                 : "");

        #endregion


    }

}
