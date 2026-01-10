/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHP get charge details record response.
    /// </summary>
    public class GetCDRsResponse : AResponse<GetCDRsRequest,
                                             GetCDRsResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of charge detail records.
        /// </summary>
        public IEnumerable<CDRInfo>  ChargeDetailRecords   { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The get charge details record request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetCDRsResponse OK(GetCDRsRequest  Request,
                                         String          Description = null)

            => new GetCDRsResponse(Request,
                                   Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The get charge details record request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetCDRsResponse Partly(GetCDRsRequest  Request,
                                             String          Description = null)

            => new GetCDRsResponse(Request,
                                   Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The get charge details record request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetCDRsResponse NotAuthorized(GetCDRsRequest  Request,
                                                    String          Description = null)

            => new GetCDRsResponse(Request,
                                   Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The get charge details record request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetCDRsResponse InvalidId(GetCDRsRequest  Request,
                                                String          Description = null)

            => new GetCDRsResponse(Request,
                                   Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The get charge details record request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetCDRsResponse Server(GetCDRsRequest  Request,
                                             String          Description = null)

            => new GetCDRsResponse(Request,
                                   Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The get charge details record request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetCDRsResponse Format(GetCDRsRequest  Request,
                                             String          Description = null)

            => new GetCDRsResponse(Request,
                                   Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP get charge details record response.
        /// </summary>
        /// <param name="Request">The get charge details record request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        public GetCDRsResponse(GetCDRsRequest        Request,
                               Result                Result,
                               IEnumerable<CDRInfo>  ChargeDetailRecords = null)

            : base(Request, Result)

        {

            this.ChargeDetailRecords  = ChargeDetailRecords ?? new CDRInfo[0];

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:GetCDRsResponse>
        //
        //         <ns:result>
        //            <ns:resultCode>
        //               <ns:resultCode>?</ns:resultCode>
        //            </ns:resultCode>
        //            <ns:resultDescription>?</ns:resultDescription>
        //         </ns:result>
        //
        //         <!--Zero or more repetitions:-->
        //         <ns:cdrInfoArray>?</ns:cdrInfoArray>
        //
        //      </ns:GetCDRsResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, GetCDRsResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get charge details record response.
        /// </summary>
        /// <param name="Request">The get charge details record request leading to this response.</param>
        /// <param name="GetCDRsResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static GetCDRsResponse Parse(GetCDRsRequest       Request,
                                            XElement             GetCDRsResponseXML,
                                            OnExceptionDelegate  OnException = null)
        {

            GetCDRsResponse _GetCDRsResponse;

            if (TryParse(Request, GetCDRsResponseXML, out _GetCDRsResponse, OnException))
                return _GetCDRsResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, GetCDRsResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get charge details record response.
        /// </summary>
        /// <param name="Request">The get charge details record request leading to this response.</param>
        /// <param name="GetCDRsResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static GetCDRsResponse Parse(GetCDRsRequest       Request,
                                            String               GetCDRsResponseText,
                                            OnExceptionDelegate  OnException = null)
        {

            GetCDRsResponse _GetCDRsResponse;

            if (TryParse(Request, GetCDRsResponseText, out _GetCDRsResponse, OnException))
                return _GetCDRsResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, GetCDRsResponseXML,  out GetCDRsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get charge details record response.
        /// </summary>
        /// <param name="Request">The get charge details record request leading to this response.</param>
        /// <param name="GetCDRsResponseXML">The XML to parse.</param>
        /// <param name="GetCDRsResponse">The parsed get charge details record response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static Boolean TryParse(GetCDRsRequest       Request,
                                       XElement             GetCDRsResponseXML,
                                       out GetCDRsResponse  GetCDRsResponse,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                GetCDRsResponse = new GetCDRsResponse(

                                      Request,

                                      GetCDRsResponseXML.MapElementOrFail (OCHPNS.Default + "result",
                                                                           Result.Parse,
                                                                           OnException),

                                      GetCDRsResponseXML.MapElements      (OCHPNS.Default + "cdrInfoArray",
                                                                           CDRInfo.Parse,
                                                                           OnException)

                                  );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, GetCDRsResponseXML, e);

                GetCDRsResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, GetCDRsResponseText, out GetCDRsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get charge details record response.
        /// </summary>
        /// <param name="Request">The get charge details record request leading to this response.</param>
        /// <param name="GetCDRsResponseText">The text to parse.</param>
        /// <param name="GetCDRsResponse">The parsed get charge details record response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static Boolean TryParse(GetCDRsRequest       Request,
                                       String               GetCDRsResponseText,
                                       out GetCDRsResponse  GetCDRsResponse,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(GetCDRsResponseText).Root,
                             out GetCDRsResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, GetCDRsResponseText, e);
            }

            GetCDRsResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetCDRsResponse",

                   Result.ToXML(),

                   ChargeDetailRecords.Select(cdr => cdr.ToXML(OCHPNS.Default + "cdrInfoArray"))

               );

        #endregion


        #region Operator overloading

        #region Operator == (GetCDRsResponse1, GetCDRsResponse2)

        /// <summary>
        /// Compares two get charge details record responses for equality.
        /// </summary>
        /// <param name="GetCDRsResponse1">A get charge details record response.</param>
        /// <param name="GetCDRsResponse2">Another get charge details record response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetCDRsResponse GetCDRsResponse1, GetCDRsResponse GetCDRsResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GetCDRsResponse1, GetCDRsResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetCDRsResponse1 is null) || ((Object) GetCDRsResponse2 is null))
                return false;

            return GetCDRsResponse1.Equals(GetCDRsResponse2);

        }

        #endregion

        #region Operator != (GetCDRsResponse1, GetCDRsResponse2)

        /// <summary>
        /// Compares two get charge details record responses for inequality.
        /// </summary>
        /// <param name="GetCDRsResponse1">A get charge details record response.</param>
        /// <param name="GetCDRsResponse2">Another get charge details record response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetCDRsResponse GetCDRsResponse1, GetCDRsResponse GetCDRsResponse2)

            => !(GetCDRsResponse1 == GetCDRsResponse2);

        #endregion

        #endregion

        #region IEquatable<GetCDRsResponse> Members

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

            // Check if the given object is a get charge details record response.
            var GetCDRsResponse = Object as GetCDRsResponse;
            if ((Object) GetCDRsResponse is null)
                return false;

            return this.Equals(GetCDRsResponse);

        }

        #endregion

        #region Equals(GetCDRsResponse)

        /// <summary>
        /// Compares two get charge details record responses for equality.
        /// </summary>
        /// <param name="GetCDRsResponse">A get charge details record response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetCDRsResponse GetCDRsResponse)
        {

            if ((Object) GetCDRsResponse is null)
                return false;

            return this.Result.Equals(GetCDRsResponse.Result);

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

                return ChargeDetailRecords is not null

                           ? Result.GetHashCode() * 11 ^
                             ChargeDetailRecords.SafeSelect(cdr => cdr.GetHashCode()).Aggregate((a, b) => a ^ b)

                           : Result.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Result,
                             ChargeDetailRecords.Any()
                                 ? " " + ChargeDetailRecords.Count() + " charge detail records"
                                 : "");

        #endregion

    }

}
