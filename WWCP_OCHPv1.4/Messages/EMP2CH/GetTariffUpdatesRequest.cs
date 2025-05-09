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
    /// An OCHP get tariff updates request.
    /// </summary>
    public class GetTariffUpdatesRequest : ARequest<GetTariffUpdatesRequest>
    {

        #region Properties

        /// <summary>
        /// An optional timestamp of the last tariff update.
        /// </summary>
        public DateTime?  LastUpdate   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP GetTariffUpdatesRequest XML/SOAP request.
        /// </summary>
        /// <param name="LastUpdate">An optional timestamp of the last tariff update.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public GetTariffUpdatesRequest(DateTime?           LastUpdate          = null,

                                       DateTime?           Timestamp           = null,
                                       EventTracking_Id?   EventTrackingId     = null,
                                       TimeSpan?           RequestTimeout      = null,
                                       CancellationToken   CancellationToken   = default)

            : base(Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

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
        //      <ns:GetTariffUpdatesRequest>
        //
        //         <!--Optional:-->
        //         <ns:lastUpdate>
        //            <ns:DateTime>?</ns:DateTime>
        //         </ns:lastUpdate>
        //
        //      </ns:GetTariffUpdatesRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(GetTariffUpdatesRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get tariff updates request.
        /// </summary>
        /// <param name="GetTariffUpdatesRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetTariffUpdatesRequest Parse(XElement             GetTariffUpdatesRequestXML,
                                                    OnExceptionDelegate  OnException = null)
        {

            GetTariffUpdatesRequest _GetTariffUpdatesRequest;

            if (TryParse(GetTariffUpdatesRequestXML, out _GetTariffUpdatesRequest, OnException))
                return _GetTariffUpdatesRequest;

            return null;

        }

        #endregion

        #region (static) Parse(GetTariffUpdatesRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get tariff updates request.
        /// </summary>
        /// <param name="GetTariffUpdatesRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetTariffUpdatesRequest Parse(String               GetTariffUpdatesRequestText,
                                                    OnExceptionDelegate  OnException = null)
        {

            GetTariffUpdatesRequest _GetTariffUpdatesRequest;

            if (TryParse(GetTariffUpdatesRequestText, out _GetTariffUpdatesRequest, OnException))
                return _GetTariffUpdatesRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(GetTariffUpdatesRequestXML,  out GetTariffUpdatesRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get tariff updates request.
        /// </summary>
        /// <param name="GetTariffUpdatesRequestXML">The XML to parse.</param>
        /// <param name="GetTariffUpdatesRequest">The parsed get tariff updates request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                     GetTariffUpdatesRequestXML,
                                       out GetTariffUpdatesRequest  GetTariffUpdatesRequest,
                                       OnExceptionDelegate          OnException  = null)
        {

            try
            {

                GetTariffUpdatesRequest = new GetTariffUpdatesRequest(

                                              GetTariffUpdatesRequestXML.MapValueOrNullable(OCHPNS.Default + "lastUpdate",
                                                                                            OCHPNS.Default + "DateTime",
                                                                                            DateTime.Parse)

                                          );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, GetTariffUpdatesRequestXML, e);

                GetTariffUpdatesRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetTariffUpdatesRequestText, out GetTariffUpdatesRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get tariff updates request.
        /// </summary>
        /// <param name="GetTariffUpdatesRequestText">The text to parse.</param>
        /// <param name="GetTariffUpdatesRequest">The parsed get tariff updates request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                       GetTariffUpdatesRequestText,
                                       out GetTariffUpdatesRequest  GetTariffUpdatesRequest,
                                       OnExceptionDelegate          OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetTariffUpdatesRequestText).Root,
                             out GetTariffUpdatesRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, GetTariffUpdatesRequestText, e);
            }

            GetTariffUpdatesRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetTariffUpdatesRequest",

                                LastUpdate.HasValue

                                    ? new XElement(OCHPNS.Default + "lastUpdate",
                                          new XElement(OCHPNS.Default + "DateTime",
                                              LastUpdate.Value.ToISO8601(false)
                                          )
                                      )

                                    : null

                           );

        #endregion


        #region Operator overloading

        #region Operator == (GetTariffUpdatesRequest1, GetTariffUpdatesRequest2)

        /// <summary>
        /// Compares two get tariff updates requests for equality.
        /// </summary>
        /// <param name="GetTariffUpdatesRequest1">A get tariff updates request.</param>
        /// <param name="GetTariffUpdatesRequest2">Another get tariff updates request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetTariffUpdatesRequest GetTariffUpdatesRequest1, GetTariffUpdatesRequest GetTariffUpdatesRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GetTariffUpdatesRequest1, GetTariffUpdatesRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetTariffUpdatesRequest1 == null) || ((Object) GetTariffUpdatesRequest2 == null))
                return false;

            return GetTariffUpdatesRequest1.Equals(GetTariffUpdatesRequest2);

        }

        #endregion

        #region Operator != (GetTariffUpdatesRequest1, GetTariffUpdatesRequest2)

        /// <summary>
        /// Compares two get tariff updates requests for inequality.
        /// </summary>
        /// <param name="GetTariffUpdatesRequest1">A get tariff updates request.</param>
        /// <param name="GetTariffUpdatesRequest2">Another get tariff updates request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetTariffUpdatesRequest GetTariffUpdatesRequest1, GetTariffUpdatesRequest GetTariffUpdatesRequest2)

            => !(GetTariffUpdatesRequest1 == GetTariffUpdatesRequest2);

        #endregion

        #endregion

        #region IEquatable<GetTariffUpdatesRequest> Members

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

            // Check if the given object is a get tariff updates request.
            var GetTariffUpdatesRequest = Object as GetTariffUpdatesRequest;
            if ((Object) GetTariffUpdatesRequest == null)
                return false;

            return this.Equals(GetTariffUpdatesRequest);

        }

        #endregion

        #region Equals(GetTariffUpdatesRequest)

        /// <summary>
        /// Compares two get tariff updates requests for equality.
        /// </summary>
        /// <param name="GetTariffUpdatesRequest">A get tariff updates request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetTariffUpdatesRequest GetTariffUpdatesRequest)
        {

            if ((Object) GetTariffUpdatesRequest == null)
                return false;

            return (!LastUpdate.HasValue && !GetTariffUpdatesRequest.LastUpdate.HasValue) ||
                    (LastUpdate.HasValue &&  GetTariffUpdatesRequest.LastUpdate.HasValue && LastUpdate.Value.Equals(GetTariffUpdatesRequest.LastUpdate.Value));

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

                return LastUpdate.HasValue
                           ? LastUpdate.GetHashCode()
                           : base.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => LastUpdate.HasValue
                   ? "last update " + LastUpdate.Value.ToISO8601()
                   : "GetTariffUpdatesRequest";

        #endregion


    }

}
