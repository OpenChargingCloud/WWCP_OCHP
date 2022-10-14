/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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
    /// An OCHP get charge point list updates request.
    /// </summary>
    public class GetChargePointListUpdatesRequest : ARequest<GetChargePointListUpdatesRequest>
    {

        #region Properties

        /// <summary>
        /// The timestamp of the last charge point list update.
        /// </summary>
        public DateTime  LastUpdate   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP GetChargePointListUpdates XML/SOAP request.
        /// </summary>
        /// <param name="LastUpdate">The timestamp of the last charge point list update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public GetChargePointListUpdatesRequest(DateTime            LastUpdate,

                                                DateTime?           Timestamp           = null,
                                                CancellationToken?  CancellationToken   = null,
                                                EventTracking_Id    EventTrackingId     = null,
                                                TimeSpan?           RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.LastUpdate = LastUpdate;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:OCHP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:GetChargePointListUpdatesRequest>
        //
        //         <ns:lastUpdate>
        //            <ns:DateTime>?</ns:DateTime>
        //         </ns:lastUpdate>
        //
        //      </ns:GetChargePointListUpdatesRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(GetChargePointListUpdatesRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get charge point list updates request.
        /// </summary>
        /// <param name="GetChargePointListUpdatesRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetChargePointListUpdatesRequest Parse(XElement             GetChargePointListUpdatesRequestXML,
                                                             OnExceptionDelegate  OnException = null)
        {

            GetChargePointListUpdatesRequest _GetChargePointListUpdatesRequest;

            if (TryParse(GetChargePointListUpdatesRequestXML, out _GetChargePointListUpdatesRequest, OnException))
                return _GetChargePointListUpdatesRequest;

            return null;

        }

        #endregion

        #region (static) Parse(GetChargePointListUpdatesRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get charge point list updates request.
        /// </summary>
        /// <param name="GetChargePointListUpdatesRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetChargePointListUpdatesRequest Parse(String               GetChargePointListUpdatesRequestText,
                                                             OnExceptionDelegate  OnException = null)
        {

            GetChargePointListUpdatesRequest _GetChargePointListUpdatesRequest;

            if (TryParse(GetChargePointListUpdatesRequestText, out _GetChargePointListUpdatesRequest, OnException))
                return _GetChargePointListUpdatesRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(GetChargePointListUpdatesRequestXML,  out GetChargePointListUpdatesRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get charge point list updates request.
        /// </summary>
        /// <param name="GetChargePointListUpdatesRequestXML">The XML to parse.</param>
        /// <param name="GetChargePointListUpdatesRequest">The parsed get charge point list updates request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                              GetChargePointListUpdatesRequestXML,
                                       out GetChargePointListUpdatesRequest  GetChargePointListUpdatesRequest,
                                       OnExceptionDelegate                   OnException  = null)
        {

            try
            {

                GetChargePointListUpdatesRequest = new GetChargePointListUpdatesRequest(

                                                       GetChargePointListUpdatesRequestXML.MapValueOrFail(OCHPNS.Default + "lastUpdate",
                                                                                                          OCHPNS.Default + "DateTime",
                                                                                                          DateTime.Parse)

                                                   );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, GetChargePointListUpdatesRequestXML, e);

                GetChargePointListUpdatesRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetChargePointListUpdatesRequestText, out GetChargePointListUpdatesRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get charge point list updates request.
        /// </summary>
        /// <param name="GetChargePointListUpdatesRequestText">The text to parse.</param>
        /// <param name="GetChargePointListUpdatesRequest">The parsed get charge point list updates request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                GetChargePointListUpdatesRequestText,
                                       out GetChargePointListUpdatesRequest  GetChargePointListUpdatesRequest,
                                       OnExceptionDelegate                   OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetChargePointListUpdatesRequestText).Root,
                             out GetChargePointListUpdatesRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, GetChargePointListUpdatesRequestText, e);
            }

            GetChargePointListUpdatesRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetChargePointListUpdatesRequest",
                                new XElement(OCHPNS.Default + "lastUpdate",
                                    new XElement(OCHPNS.Default + "DateTime",
                                        LastUpdate.ToIso8601(false)
                           )));

        #endregion


        #region Operator overloading

        #region Operator == (GetChargePointListUpdatesRequest1, GetChargePointListUpdatesRequest2)

        /// <summary>
        /// Compares two get charge point list updates requests for equality.
        /// </summary>
        /// <param name="GetChargePointListUpdatesRequest1">A get charge point list updates request.</param>
        /// <param name="GetChargePointListUpdatesRequest2">Another get charge point list updates request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetChargePointListUpdatesRequest GetChargePointListUpdatesRequest1, GetChargePointListUpdatesRequest GetChargePointListUpdatesRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GetChargePointListUpdatesRequest1, GetChargePointListUpdatesRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetChargePointListUpdatesRequest1 == null) || ((Object) GetChargePointListUpdatesRequest2 == null))
                return false;

            return GetChargePointListUpdatesRequest1.Equals(GetChargePointListUpdatesRequest2);

        }

        #endregion

        #region Operator != (GetChargePointListUpdatesRequest1, GetChargePointListUpdatesRequest2)

        /// <summary>
        /// Compares two get charge point list updates requests for inequality.
        /// </summary>
        /// <param name="GetChargePointListUpdatesRequest1">A get charge point list updates request.</param>
        /// <param name="GetChargePointListUpdatesRequest2">Another get charge point list updates request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetChargePointListUpdatesRequest GetChargePointListUpdatesRequest1, GetChargePointListUpdatesRequest GetChargePointListUpdatesRequest2)

            => !(GetChargePointListUpdatesRequest1 == GetChargePointListUpdatesRequest2);

        #endregion

        #endregion

        #region IEquatable<GetChargePointListUpdatesRequest> Members

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

            // Check if the given object is a get charge point list updates request.
            var GetChargePointListUpdatesRequest = Object as GetChargePointListUpdatesRequest;
            if ((Object) GetChargePointListUpdatesRequest == null)
                return false;

            return this.Equals(GetChargePointListUpdatesRequest);

        }

        #endregion

        #region Equals(GetChargePointListUpdatesRequest)

        /// <summary>
        /// Compares two get charge point list updates requests for equality.
        /// </summary>
        /// <param name="GetChargePointListUpdatesRequest">A get charge point list updates request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetChargePointListUpdatesRequest GetChargePointListUpdatesRequest)
        {

            if ((Object) GetChargePointListUpdatesRequest == null)
                return false;

            return LastUpdate.Equals(GetChargePointListUpdatesRequest.LastUpdate);

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

                return LastUpdate.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => "last update " + LastUpdate.ToIso8601();

        #endregion


    }

}
