/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
using System.Threading;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHP set charge point list request.
    /// </summary>
    public class SetChargePointListRequest : ARequest<SetChargePointListRequest>
    {

        #region Properties

        /// <summary>
        /// An enumeration of charge point infos.
        /// </summary>
        public IEnumerable<ChargePointInfo>  ChargePointInfos   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP SetChargePointList XML/SOAP request.
        /// </summary>
        /// <param name="ChargePointInfos">An enumeration of charge point infos.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public SetChargePointListRequest(IEnumerable<ChargePointInfo>  ChargePointInfos,

                                         DateTime?                     Timestamp           = null,
                                         CancellationToken?            CancellationToken   = null,
                                         EventTracking_Id              EventTrackingId     = null,
                                         TimeSpan?                     RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            #region Initial checks

            if (ChargePointInfos == null)
                throw new ArgumentNullException(nameof(ChargePointInfos),  "The given enumeration of charge point infos must not be null!");

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
        //       <OCHP:SetChargePointListRequest>
        //
        //          <!--1 or more repetitions / We allow also 0:-->
        //          <OCHP:chargePointInfoArray>
        //             ...
        //          </OCHP:chargePointInfoArray>
        //
        //       </OCHP:SetChargePointListRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(SetChargePointListRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP set charge point list request.
        /// </summary>
        /// <param name="SetChargePointListRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetChargePointListRequest Parse(XElement             SetChargePointListRequestXML,
                                                      OnExceptionDelegate  OnException = null)
        {

            SetChargePointListRequest _SetChargePointListRequest;

            if (TryParse(SetChargePointListRequestXML, out _SetChargePointListRequest, OnException))
                return _SetChargePointListRequest;

            return null;

        }

        #endregion

        #region (static) Parse(SetChargePointListRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP set charge point list request.
        /// </summary>
        /// <param name="SetChargePointListRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetChargePointListRequest Parse(String               SetChargePointListRequestText,
                                                      OnExceptionDelegate  OnException = null)
        {

            SetChargePointListRequest _SetChargePointListRequest;

            if (TryParse(SetChargePointListRequestText, out _SetChargePointListRequest, OnException))
                return _SetChargePointListRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(SetChargePointListRequestXML,  out SetChargePointListRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP set charge point list request.
        /// </summary>
        /// <param name="SetChargePointListRequestXML">The XML to parse.</param>
        /// <param name="SetChargePointListRequest">The parsed set charge point list request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                       SetChargePointListRequestXML,
                                       out SetChargePointListRequest  SetChargePointListRequest,
                                       OnExceptionDelegate            OnException  = null)
        {

            try
            {

                SetChargePointListRequest = new SetChargePointListRequest(

                                                SetChargePointListRequestXML.MapElements(OCHPNS.Default + "chargePointInfoArray",
                                                                                         ChargePointInfo.Parse,
                                                                                         OnException)

                                            );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, SetChargePointListRequestXML, e);

                SetChargePointListRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SetChargePointListRequestText, out SetChargePointListRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP set charge point list request.
        /// </summary>
        /// <param name="SetChargePointListRequestText">The text to parse.</param>
        /// <param name="SetChargePointListRequest">The parsed set charge point list request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                         SetChargePointListRequestText,
                                       out SetChargePointListRequest  SetChargePointListRequest,
                                       OnExceptionDelegate            OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SetChargePointListRequestText).Root,
                             out SetChargePointListRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, SetChargePointListRequestText, e);
            }

            SetChargePointListRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "SetChargePointListRequest",

                                ChargePointInfos.Any()
                                    ? ChargePointInfos.SafeSelect(chargepointinfo => chargepointinfo.ToXML())
                                    : null

                           );

        #endregion


        #region Operator overloading

        #region Operator == (SetChargePointListRequest1, SetChargePointListRequest2)

        /// <summary>
        /// Compares two set charge point list requests for equality.
        /// </summary>
        /// <param name="SetChargePointListRequest1">A set charge point list request.</param>
        /// <param name="SetChargePointListRequest2">Another set charge point list request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetChargePointListRequest SetChargePointListRequest1, SetChargePointListRequest SetChargePointListRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SetChargePointListRequest1, SetChargePointListRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SetChargePointListRequest1 == null) || ((Object) SetChargePointListRequest2 == null))
                return false;

            return SetChargePointListRequest1.Equals(SetChargePointListRequest2);

        }

        #endregion

        #region Operator != (SetChargePointListRequest1, SetChargePointListRequest2)

        /// <summary>
        /// Compares two set charge point list requests for inequality.
        /// </summary>
        /// <param name="SetChargePointListRequest1">A set charge point list request.</param>
        /// <param name="SetChargePointListRequest2">Another set charge point list request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetChargePointListRequest SetChargePointListRequest1, SetChargePointListRequest SetChargePointListRequest2)

            => !(SetChargePointListRequest1 == SetChargePointListRequest2);

        #endregion

        #endregion

        #region IEquatable<SetChargePointListRequest> Members

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

            // Check if the given object is a set charge point list request.
            var SetChargePointListRequest = Object as SetChargePointListRequest;
            if ((Object) SetChargePointListRequest == null)
                return false;

            return this.Equals(SetChargePointListRequest);

        }

        #endregion

        #region Equals(SetChargePointListRequest)

        /// <summary>
        /// Compares two set charge point list requests for equality.
        /// </summary>
        /// <param name="SetChargePointListRequest">A set charge point list request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetChargePointListRequest SetChargePointListRequest)
        {

            if ((Object) SetChargePointListRequest == null)
                return false;

            return ChargePointInfos.Count().Equals(SetChargePointListRequest.ChargePointInfos.Count());

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
