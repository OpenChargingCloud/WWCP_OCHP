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
    /// An OCHP check charge detail records request.
    /// </summary>
    public class CheckCDRsRequest : ARequest<CheckCDRsRequest>
    {

        #region Properties

    /// <summary>
    /// The status of the requested charge detail records.
    /// </summary>
    public CDRStatus?  CDRStatus   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP CheckCDRs XML/SOAP request.
        /// </summary>
        /// <param name="CDRStatus">The status of the requested charge detail records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public CheckCDRsRequest(CDRStatus?         CDRStatus           = null,

                                DateTimeOffset?    Timestamp           = null,
                                EventTracking_Id?  EventTrackingId     = null,
                                TimeSpan?          RequestTimeout      = null,
                                CancellationToken  CancellationToken   = default)

            : base(Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.CDRStatus  = CDRStatus ?? new CDRStatus?();

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:OCHP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <OCHP:CheckCDRsRequest>
        //
        //         <!--Optional:-->
        //         <OCHP:cdrStatus>
        //            <OCHP:CdrStatusType>?</OCHP:CdrStatusType>
        //         </OCHP:cdrStatus>
        //
        //      </OCHP:CheckCDRsRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(CheckCDRsRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP check charge detail records request.
        /// </summary>
        /// <param name="CheckCDRsRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static CheckCDRsRequest Parse(XElement             CheckCDRsRequestXML,
                                             OnExceptionDelegate  OnException = null)
        {

            CheckCDRsRequest _CheckCDRsRequest;

            if (TryParse(CheckCDRsRequestXML, out _CheckCDRsRequest, OnException))
                return _CheckCDRsRequest;

            return null;

        }

        #endregion

        #region (static) Parse(CheckCDRsRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP check charge detail records request.
        /// </summary>
        /// <param name="CheckCDRsRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static CheckCDRsRequest Parse(String               CheckCDRsRequestText,
                                             OnExceptionDelegate  OnException = null)
        {

            CheckCDRsRequest _CheckCDRsRequest;

            if (TryParse(CheckCDRsRequestText, out _CheckCDRsRequest, OnException))
                return _CheckCDRsRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(CheckCDRsRequestXML,  out CheckCDRsRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP check charge detail records request.
        /// </summary>
        /// <param name="CheckCDRsRequestXML">The XML to parse.</param>
        /// <param name="CheckCDRsRequest">The parsed check charge detail records request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement              CheckCDRsRequestXML,
                                       out CheckCDRsRequest  CheckCDRsRequest,
                                       OnExceptionDelegate   OnException  = null)
        {

            try
            {

                CheckCDRsRequest = new CheckCDRsRequest(

                                       CheckCDRsRequestXML.MapEnumValues(OCHPNS.Default + "cdrStatus",
                                                                         OCHPNS.Default + "CdrStatusType",
                                                                         XML_IO.AsCDRStatus)

                                   );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, CheckCDRsRequestXML, e);

                CheckCDRsRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(CheckCDRsRequestText, out CheckCDRsRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP check charge detail records request.
        /// </summary>
        /// <param name="CheckCDRsRequestText">The text to parse.</param>
        /// <param name="CheckCDRsRequest">The parsed check charge detail records request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                CheckCDRsRequestText,
                                       out CheckCDRsRequest  CheckCDRsRequest,
                                       OnExceptionDelegate   OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(CheckCDRsRequestText).Root,
                             out CheckCDRsRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, CheckCDRsRequestText, e);
            }

            CheckCDRsRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "CheckCDRsRequest",

                                CDRStatus.HasValue
                                    ? new XElement(OCHPNS.Default + "cdrStatus",
                                          new XElement(OCHPNS.Default + "CdrStatusType", XML_IO.AsText(CDRStatus.Value))
                                      )
                                    : null

                           );

        #endregion


        #region Operator overloading

        #region Operator == (CheckCDRsRequest1, CheckCDRsRequest2)

        /// <summary>
        /// Compares two check charge detail records requests for equality.
        /// </summary>
        /// <param name="CheckCDRsRequest1">A check charge detail records request.</param>
        /// <param name="CheckCDRsRequest2">Another check charge detail records request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (CheckCDRsRequest CheckCDRsRequest1, CheckCDRsRequest CheckCDRsRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(CheckCDRsRequest1, CheckCDRsRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) CheckCDRsRequest1 == null) || ((Object) CheckCDRsRequest2 == null))
                return false;

            return CheckCDRsRequest1.Equals(CheckCDRsRequest2);

        }

        #endregion

        #region Operator != (CheckCDRsRequest1, CheckCDRsRequest2)

        /// <summary>
        /// Compares two check charge detail records requests for inequality.
        /// </summary>
        /// <param name="CheckCDRsRequest1">A check charge detail records request.</param>
        /// <param name="CheckCDRsRequest2">Another check charge detail records request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (CheckCDRsRequest CheckCDRsRequest1, CheckCDRsRequest CheckCDRsRequest2)

            => !(CheckCDRsRequest1 == CheckCDRsRequest2);

        #endregion

        #endregion

        #region IEquatable<CheckCDRsRequest> Members

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

            // Check if the given object is a check charge detail records request.
            var CheckCDRsRequest = Object as CheckCDRsRequest;
            if ((Object) CheckCDRsRequest == null)
                return false;

            return this.Equals(CheckCDRsRequest);

        }

        #endregion

        #region Equals(CheckCDRsRequest)

        /// <summary>
        /// Compares two check charge detail records requests for equality.
        /// </summary>
        /// <param name="CheckCDRsRequest">A check charge detail records request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(CheckCDRsRequest CheckCDRsRequest)
        {

            if ((Object) CheckCDRsRequest == null)
                return false;

            return (CDRStatus == null && CheckCDRsRequest.CDRStatus == null) ||
                   (CDRStatus != null && CheckCDRsRequest.CDRStatus != null && CDRStatus.Value.Equals(CheckCDRsRequest.CDRStatus.Value));

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

                return CDRStatus.HasValue
                           ? CDRStatus.GetHashCode()
                           : GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => CDRStatus.HasValue
                   ? CDRStatus.Value.ToString()
                   : "no CDR status filter";

        #endregion


    }

}
