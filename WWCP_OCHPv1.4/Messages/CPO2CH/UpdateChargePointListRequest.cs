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

using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.CPO
{

    /// <summary>
    /// An UpdateChargePointList request.
    /// </summary>
    public class UpdateChargePointListRequest : ARequest<UpdateChargePointListRequest>
    {

        #region Properties

        /// <summary>
        /// An enumeration of charge point infos.
        /// </summary>
        public IEnumerable<ChargePointInfo>  ChargePointInfos   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP UpdateChargePointList XML/SOAP request.
        /// </summary>
        /// <param name="ChargePointInfos">An enumeration of charge point infos.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public UpdateChargePointListRequest(IEnumerable<ChargePointInfo>  ChargePointInfos,

                                            DateTimeOffset?               Timestamp           = null,
                                            EventTracking_Id?             EventTrackingId     = null,
                                            TimeSpan?                     RequestTimeout      = null,
                                            CancellationToken             CancellationToken   = default)

            : base(Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            #region Initial checks

            if (ChargePointInfos.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(ChargePointInfos),  "The given enumeration of charge point infos must not be null or empty!");

            #endregion

            this.ChargePointInfos = ChargePointInfos;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:OCHP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <OCHP:UpdateChargePointListRequest>
        //
        //          <!--1 or more repetitions:-->
        //          <OCHP:chargePointInfoArray>
        //             ...
        //          </OCHP:chargePointInfoArray>
        //
        //       </OCHP:UpdateChargePointListRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(UpdateChargePointListRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP update charge point list request.
        /// </summary>
        /// <param name="UpdateChargePointListRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateChargePointListRequest Parse(XElement             UpdateChargePointListRequestXML,
                                                         OnExceptionDelegate  OnException = null)
        {

            UpdateChargePointListRequest _UpdateChargePointListRequest;

            if (TryParse(UpdateChargePointListRequestXML, out _UpdateChargePointListRequest, OnException))
                return _UpdateChargePointListRequest;

            return null;

        }

        #endregion

        #region (static) Parse(UpdateChargePointListRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP update charge point list request.
        /// </summary>
        /// <param name="UpdateChargePointListRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateChargePointListRequest Parse(String               UpdateChargePointListRequestText,
                                                         OnExceptionDelegate  OnException = null)
        {

            UpdateChargePointListRequest _UpdateChargePointListRequest;

            if (TryParse(UpdateChargePointListRequestText, out _UpdateChargePointListRequest, OnException))
                return _UpdateChargePointListRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(UpdateChargePointListRequestXML,  out UpdateChargePointListRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP update charge point list request.
        /// </summary>
        /// <param name="UpdateChargePointListRequestXML">The XML to parse.</param>
        /// <param name="UpdateChargePointListRequest">The parsed update charge point list request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                          UpdateChargePointListRequestXML,
                                       out UpdateChargePointListRequest  UpdateChargePointListRequest,
                                       OnExceptionDelegate               OnException  = null)
        {

            try
            {

                UpdateChargePointListRequest = new UpdateChargePointListRequest(

                                                   UpdateChargePointListRequestXML.MapElementsOrFail(OCHPNS.Default + "chargePointInfoArray",
                                                                                                     ChargePointInfo.Parse,
                                                                                                     OnException)

                                               );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, UpdateChargePointListRequestXML, e);

                UpdateChargePointListRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(UpdateChargePointListRequestText, out UpdateChargePointListRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP update charge point list request.
        /// </summary>
        /// <param name="UpdateChargePointListRequestText">The text to parse.</param>
        /// <param name="UpdateChargePointListRequest">The parsed update charge point list request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                         UpdateChargePointListRequestText,
                                       out UpdateChargePointListRequest  UpdateChargePointListRequest,
                                       OnExceptionDelegate            OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(UpdateChargePointListRequestText).Root,
                             out UpdateChargePointListRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, UpdateChargePointListRequestText, e);
            }

            UpdateChargePointListRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "UpdateChargePointListRequest",

                                ChargePointInfos.Select(chargepointinfo => chargepointinfo.ToXML()).
                                                 ToArray()

                           );

        #endregion


        #region Operator overloading

        #region Operator == (UpdateChargePointListRequest1, UpdateChargePointListRequest2)

        /// <summary>
        /// Compares two update charge point list requests for equality.
        /// </summary>
        /// <param name="UpdateChargePointListRequest1">A update charge point list request.</param>
        /// <param name="UpdateChargePointListRequest2">Another update charge point list request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (UpdateChargePointListRequest UpdateChargePointListRequest1, UpdateChargePointListRequest UpdateChargePointListRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(UpdateChargePointListRequest1, UpdateChargePointListRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) UpdateChargePointListRequest1 is null) || ((Object) UpdateChargePointListRequest2 is null))
                return false;

            return UpdateChargePointListRequest1.Equals(UpdateChargePointListRequest2);

        }

        #endregion

        #region Operator != (UpdateChargePointListRequest1, UpdateChargePointListRequest2)

        /// <summary>
        /// Compares two update charge point list requests for inequality.
        /// </summary>
        /// <param name="UpdateChargePointListRequest1">A update charge point list request.</param>
        /// <param name="UpdateChargePointListRequest2">Another update charge point list request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (UpdateChargePointListRequest UpdateChargePointListRequest1, UpdateChargePointListRequest UpdateChargePointListRequest2)

            => !(UpdateChargePointListRequest1 == UpdateChargePointListRequest2);

        #endregion

        #endregion

        #region IEquatable<UpdateChargePointListRequest> Members

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

            // Check if the given object is a update charge point list request.
            var UpdateChargePointListRequest = Object as UpdateChargePointListRequest;
            if ((Object) UpdateChargePointListRequest is null)
                return false;

            return this.Equals(UpdateChargePointListRequest);

        }

        #endregion

        #region Equals(UpdateChargePointListRequest)

        /// <summary>
        /// Compares two update charge point list requests for equality.
        /// </summary>
        /// <param name="UpdateChargePointListRequest">A update charge point list request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(UpdateChargePointListRequest UpdateChargePointListRequest)
        {

            if ((Object) UpdateChargePointListRequest is null)
                return false;

            return ChargePointInfos.Count().Equals(UpdateChargePointListRequest.ChargePointInfos.Count());

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

                return ChargePointInfos.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(ChargePointInfos.Count(), " charge point info(s)");

        #endregion


    }

}
