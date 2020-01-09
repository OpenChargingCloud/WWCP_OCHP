/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHP get charge point list response.
    /// </summary>
    public class GetChargePointListResponse : AResponse<GetChargePointListRequest,
                                                        GetChargePointListResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of charge points.
        /// </summary>
        public IEnumerable<ChargePointInfo>  ChargePoints    { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The get charge point list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListResponse OK(GetChargePointListRequest  Request,
                                                    String                     Description = null)

            => new GetChargePointListResponse(Request,
                                              Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The get charge point list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListResponse Partly(GetChargePointListRequest  Request,
                                                        String                     Description = null)

            => new GetChargePointListResponse(Request,
                                              Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The get charge point list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListResponse NotAuthorized(GetChargePointListRequest  Request,
                                                               String                     Description = null)

            => new GetChargePointListResponse(Request,
                                              Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The get charge point list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListResponse InvalidId(GetChargePointListRequest  Request,
                                                           String                     Description = null)

            => new GetChargePointListResponse(Request,
                                              Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The get charge point list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListResponse Server(GetChargePointListRequest  Request,
                                                        String                     Description = null)

            => new GetChargePointListResponse(Request,
                                              Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The get charge point list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListResponse Format(GetChargePointListRequest  Request,
                                                        String                     Description = null)

            => new GetChargePointListResponse(Request,
                                              Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP get charge point list response.
        /// </summary>
        /// <param name="Request">The get charge point list request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="ChargePoints">An enumeration of charge points.</param>
        public GetChargePointListResponse(GetChargePointListRequest     Request,
                                          Result                        Result,
                                          IEnumerable<ChargePointInfo>  ChargePoints = null)

            : base(Request, Result)

        {

            this.ChargePoints  = ChargePoints ?? new ChargePointInfo[0];

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:GetChargePointListResponse>
        //
        //          <ns:result>
        //
        //             <ns:resultCode>
        //                <ns:resultCode>?</ns:resultCode>
        //             </ns:resultCode>
        //
        //             <ns:resultDescription>?</ns:resultDescription>
        //
        //          </ns:result>
        //
        //          <!--Zero or more repetitions:-->
        //          <ns:chargePointInfoArray>
        //             ...
        //          <ns:chargePointInfoArray>
        //
        //       </ns:GetChargePointListResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, GetChargePointListResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get charge point list response.
        /// </summary>
        /// <param name="Request">The get charge point list request leading to this response.</param>
        /// <param name="GetChargePointListResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetChargePointListResponse Parse(GetChargePointListRequest  Request,
                                                       XElement                   GetChargePointListResponseXML,
                                                       OnExceptionDelegate        OnException = null)
        {

            GetChargePointListResponse _GetChargePointListResponse;

            if (TryParse(Request, GetChargePointListResponseXML, out _GetChargePointListResponse, OnException))
                return _GetChargePointListResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, GetChargePointListResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get charge point list response.
        /// </summary>
        /// <param name="Request">The get charge point list request leading to this response.</param>
        /// <param name="GetChargePointListResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetChargePointListResponse Parse(GetChargePointListRequest  Request,
                                                       String                     GetChargePointListResponseText,
                                                       OnExceptionDelegate        OnException = null)
        {

            GetChargePointListResponse _GetChargePointListResponse;

            if (TryParse(Request, GetChargePointListResponseText, out _GetChargePointListResponse, OnException))
                return _GetChargePointListResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, GetChargePointListResponseXML,  out GetChargePointListResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get charge point list response.
        /// </summary>
        /// <param name="Request">The get charge point list request leading to this response.</param>
        /// <param name="GetChargePointListResponseXML">The XML to parse.</param>
        /// <param name="GetChargePointListResponse">The parsed get charge point list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetChargePointListRequest       Request,
                                       XElement                        GetChargePointListResponseXML,
                                       out GetChargePointListResponse  GetChargePointListResponse,
                                       OnExceptionDelegate             OnException  = null)
        {

            try
            {

                GetChargePointListResponse = new GetChargePointListResponse(

                                                 Request,

                                                 GetChargePointListResponseXML.MapElementOrFail(OCHPNS.Default + "result",
                                                                                                Result.Parse,
                                                                                                OnException),

                                                 GetChargePointListResponseXML.MapElements     (OCHPNS.Default + "chargePointInfoArray",
                                                                                                ChargePointInfo.Parse,
                                                                                                OnException)

                                             );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, GetChargePointListResponseXML, e);

                GetChargePointListResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, GetChargePointListResponseText, out GetChargePointListResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get charge point list response.
        /// </summary>
        /// <param name="Request">The get charge point list request leading to this response.</param>
        /// <param name="GetChargePointListResponseText">The text to parse.</param>
        /// <param name="GetChargePointListResponse">The parsed get charge point list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetChargePointListRequest       Request,
                                       String                          GetChargePointListResponseText,
                                       out GetChargePointListResponse  GetChargePointListResponse,
                                       OnExceptionDelegate             OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(GetChargePointListResponseText).Root,
                             out GetChargePointListResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, GetChargePointListResponseText, e);
            }

            GetChargePointListResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetChargePointListResponse",

                   Result.ToXML(),

                   ChargePoints.Any()
                       ? ChargePoints.SafeSelect(chargepoint => chargepoint.ToXML(OCHPNS.Default + "chargePointInfoArray"))
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (GetChargePointListResponse1, GetChargePointListResponse2)

        /// <summary>
        /// Compares two get charge point list responses for equality.
        /// </summary>
        /// <param name="GetChargePointListResponse1">A get charge point list response.</param>
        /// <param name="GetChargePointListResponse2">Another get charge point list response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetChargePointListResponse GetChargePointListResponse1, GetChargePointListResponse GetChargePointListResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetChargePointListResponse1, GetChargePointListResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetChargePointListResponse1 == null) || ((Object) GetChargePointListResponse2 == null))
                return false;

            return GetChargePointListResponse1.Equals(GetChargePointListResponse2);

        }

        #endregion

        #region Operator != (GetChargePointListResponse1, GetChargePointListResponse2)

        /// <summary>
        /// Compares two get charge point list responses for inequality.
        /// </summary>
        /// <param name="GetChargePointListResponse1">A get charge point list response.</param>
        /// <param name="GetChargePointListResponse2">Another get charge point list response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetChargePointListResponse GetChargePointListResponse1, GetChargePointListResponse GetChargePointListResponse2)

            => !(GetChargePointListResponse1 == GetChargePointListResponse2);

        #endregion

        #endregion

        #region IEquatable<GetChargePointListResponse> Members

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

            // Check if the given object is a get charge point list response.
            var GetChargePointListResponse = Object as GetChargePointListResponse;
            if ((Object) GetChargePointListResponse == null)
                return false;

            return this.Equals(GetChargePointListResponse);

        }

        #endregion

        #region Equals(GetChargePointListResponse)

        /// <summary>
        /// Compares two get charge point list responses for equality.
        /// </summary>
        /// <param name="GetChargePointListResponse">A get charge point list response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetChargePointListResponse GetChargePointListResponse)
        {

            if ((Object) GetChargePointListResponse == null)
                return false;

            return this.Result. Equals(GetChargePointListResponse.Result);

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

                return ChargePoints != null

                           ? Result.GetHashCode() * 11 ^
                             ChargePoints.SafeSelect(chargepoint => chargepoint.GetHashCode()).Aggregate((a, b) => a ^ b)

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
                             ChargePoints.Any()
                                 ? " " + ChargePoints.Count() + " charge points"
                                 : "");

        #endregion

    }

}
