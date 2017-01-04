/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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
    /// An OCHP get charge detail records request.
    /// </summary>
    public class GetCDRsRequest : ARequest<GetCDRsRequest>
    {

        #region Properties

        /// <summary>
        /// The optional status of the requested charge detail records.
        /// </summary>
        public CDRStatus?  CDRStatus   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP GetCDRsRequest XML/SOAP request.
        /// </summary>
        /// <param name="CDRStatus">The optional status of the requested charge detail records.</param>
        public GetCDRsRequest(CDRStatus? CDRStatus = null)
        {

            this.CDRStatus = CDRStatus ?? new CDRStatus?();

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:OCHP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <OCHP:GetCDRsRequest>
        //
        //         <!--Optional:-->
        //         <OCHP:cdrStatus>
        //            <OCHP:CdrStatusType>?</OCHP:CdrStatusType>
        //         </OCHP:cdrStatus>
        //
        //      </OCHP:GetCDRsRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(GetCDRsRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get charge detail records request.
        /// </summary>
        /// <param name="GetCDRsRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetCDRsRequest Parse(XElement             GetCDRsRequestXML,
                                           OnExceptionDelegate  OnException = null)
        {

            GetCDRsRequest _GetCDRsRequest;

            if (TryParse(GetCDRsRequestXML, out _GetCDRsRequest, OnException))
                return _GetCDRsRequest;

            return null;

        }

        #endregion

        #region (static) Parse(GetCDRsRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get charge detail records request.
        /// </summary>
        /// <param name="GetCDRsRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetCDRsRequest Parse(String               GetCDRsRequestText,
                                           OnExceptionDelegate  OnException = null)
        {

            GetCDRsRequest _GetCDRsRequest;

            if (TryParse(GetCDRsRequestText, out _GetCDRsRequest, OnException))
                return _GetCDRsRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(GetCDRsRequestXML,  out GetCDRsRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get charge detail records request.
        /// </summary>
        /// <param name="GetCDRsRequestXML">The XML to parse.</param>
        /// <param name="GetCDRsRequest">The parsed get charge detail records request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             GetCDRsRequestXML,
                                       out GetCDRsRequest   GetCDRsRequest,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                GetCDRsRequest = new GetCDRsRequest(

                                     GetCDRsRequestXML.MapValueOrNullable(OCHPNS.Default + "cdrStatus",
                                                                          OCHPNS.Default + "CdrStatusType",
                                                                          XML_IO.AsCDRStatus)

                                 );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, GetCDRsRequestXML, e);

                GetCDRsRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetCDRsRequestText, out GetCDRsRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get charge detail records request.
        /// </summary>
        /// <param name="GetCDRsRequestText">The text to parse.</param>
        /// <param name="GetCDRsRequest">The parsed get charge detail records request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               GetCDRsRequestText,
                                       out GetCDRsRequest   GetCDRsRequest,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetCDRsRequestText).Root,
                             out GetCDRsRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, GetCDRsRequestText, e);
            }

            GetCDRsRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetCDRsRequest",

                                CDRStatus.HasValue
                                    ? new XElement(OCHPNS.Default + "cdrStatus",
                                          new XElement(OCHPNS.Default + "CdrStatusType",
                                              XML_IO.AsText(CDRStatus.Value)
                                          )
                                      )
                                    : null

                           );

        #endregion


        #region Operator overloading

        #region Operator == (GetCDRsRequest1, GetCDRsRequest2)

        /// <summary>
        /// Compares two get charge detail records requests for equality.
        /// </summary>
        /// <param name="GetCDRsRequest1">A get charge detail records request.</param>
        /// <param name="GetCDRsRequest2">Another get charge detail records request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetCDRsRequest GetCDRsRequest1, GetCDRsRequest GetCDRsRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetCDRsRequest1, GetCDRsRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetCDRsRequest1 == null) || ((Object) GetCDRsRequest2 == null))
                return false;

            return GetCDRsRequest1.Equals(GetCDRsRequest2);

        }

        #endregion

        #region Operator != (GetCDRsRequest1, GetCDRsRequest2)

        /// <summary>
        /// Compares two get charge detail records requests for inequality.
        /// </summary>
        /// <param name="GetCDRsRequest1">A get charge detail records request.</param>
        /// <param name="GetCDRsRequest2">Another get charge detail records request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetCDRsRequest GetCDRsRequest1, GetCDRsRequest GetCDRsRequest2)

            => !(GetCDRsRequest1 == GetCDRsRequest2);

        #endregion

        #endregion

        #region IEquatable<GetCDRsRequest> Members

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

            // Check if the given object is a get charge detail records request.
            var GetCDRsRequest = Object as GetCDRsRequest;
            if ((Object) GetCDRsRequest == null)
                return false;

            return this.Equals(GetCDRsRequest);

        }

        #endregion

        #region Equals(GetCDRsRequest)

        /// <summary>
        /// Compares two get charge detail records requests for equality.
        /// </summary>
        /// <param name="GetCDRsRequest">A get charge detail records request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetCDRsRequest GetCDRsRequest)
        {

            if ((Object) GetCDRsRequest == null)
                return false;

            return (!CDRStatus.HasValue && !GetCDRsRequest.CDRStatus.HasValue) ||
                    (CDRStatus.HasValue &&  GetCDRsRequest.CDRStatus.HasValue && CDRStatus.Value.Equals(GetCDRsRequest.CDRStatus.Value));

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
                           : base.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => CDRStatus.HasValue
                   ? "charge detail record status filter: " + CDRStatus.Value
                   : "";

        #endregion


    }

}
