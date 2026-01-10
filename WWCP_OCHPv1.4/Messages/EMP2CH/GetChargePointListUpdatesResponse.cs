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
    /// An OCHP get charge point list updates response.
    /// </summary>
    public class GetChargePointListUpdatesResponse : AResponse<GetChargePointListUpdatesRequest,
                                                               GetChargePointListUpdatesResponse>
    {

        #region Properties

        /// <summary>
        ///  An enumeration of charge points.
        /// </summary>
        public IEnumerable<ChargePointInfo>  ChargePoints    { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The get charge point list updates request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListUpdatesResponse OK(GetChargePointListUpdatesRequest  Request,
                                                           String                            Description = null)

            => new GetChargePointListUpdatesResponse(Request,
                                                     Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The get charge point list updates request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListUpdatesResponse Partly(GetChargePointListUpdatesRequest  Request,
                                                               String                            Description = null)

            => new GetChargePointListUpdatesResponse(Request,
                                                     Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The get charge point list updates request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListUpdatesResponse NotAuthorized(GetChargePointListUpdatesRequest  Request,
                                                                      String                            Description = null)

            => new GetChargePointListUpdatesResponse(Request,
                                                     Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The get charge point list updates request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListUpdatesResponse InvalidId(GetChargePointListUpdatesRequest  Request,
                                                                  String                            Description = null)

            => new GetChargePointListUpdatesResponse(Request,
                                                     Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The get charge point list updates request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListUpdatesResponse Server(GetChargePointListUpdatesRequest  Request,
                                                               String                            Description = null)

            => new GetChargePointListUpdatesResponse(Request,
                                                     Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The get charge point list updates request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListUpdatesResponse Format(GetChargePointListUpdatesRequest  Request,
                                                               String                            Description = null)

            => new GetChargePointListUpdatesResponse(Request,
                                                     Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP get charge point list response.
        /// </summary>
        /// <param name="Request">The get charge point list updates request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="ChargePoints">An enumeration of charge points.</param>
        public GetChargePointListUpdatesResponse(GetChargePointListUpdatesRequest  Request,
                                                 Result                            Result,
                                                 IEnumerable<ChargePointInfo>      ChargePoints = null)

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
        //       <ns:GetChargePointListUpdatesResponse>
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
        //       </ns:GetChargePointListUpdatesResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, GetChargePointListUpdatesResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get charge point list response.
        /// </summary>
        /// <param name="Request">The get charge point list updates request leading to this response.</param>
        /// <param name="GetChargePointListUpdatesResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static GetChargePointListUpdatesResponse Parse(GetChargePointListUpdatesRequest  Request,
                                                              XElement                          GetChargePointListUpdatesResponseXML,
                                                              OnExceptionDelegate               OnException = null)
        {

            GetChargePointListUpdatesResponse _GetChargePointListUpdatesResponse;

            if (TryParse(Request, GetChargePointListUpdatesResponseXML, out _GetChargePointListUpdatesResponse, OnException))
                return _GetChargePointListUpdatesResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, GetChargePointListUpdatesResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get charge point list response.
        /// </summary>
        /// <param name="Request">The get charge point list updates request leading to this response.</param>
        /// <param name="GetChargePointListUpdatesResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static GetChargePointListUpdatesResponse Parse(GetChargePointListUpdatesRequest  Request,
                                                              String                            GetChargePointListUpdatesResponseText,
                                                              OnExceptionDelegate               OnException = null)
        {

            GetChargePointListUpdatesResponse _GetChargePointListUpdatesResponse;

            if (TryParse(Request, GetChargePointListUpdatesResponseText, out _GetChargePointListUpdatesResponse, OnException))
                return _GetChargePointListUpdatesResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, GetChargePointListUpdatesResponseXML,  out GetChargePointListUpdatesResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get charge point list response.
        /// </summary>
        /// <param name="Request">The get charge point list updates request leading to this response.</param>
        /// <param name="GetChargePointListUpdatesResponseXML">The XML to parse.</param>
        /// <param name="GetChargePointListUpdatesResponse">The parsed get charge point list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static Boolean TryParse(GetChargePointListUpdatesRequest       Request,
                                       XElement                               GetChargePointListUpdatesResponseXML,
                                       out GetChargePointListUpdatesResponse  GetChargePointListUpdatesResponse,
                                       OnExceptionDelegate                    OnException  = null)
        {

            try
            {

                GetChargePointListUpdatesResponse = new GetChargePointListUpdatesResponse(

                                                        Request,

                                                        GetChargePointListUpdatesResponseXML.MapElementOrFail (OCHPNS.Default + "result",
                                                                                                               Result.Parse,
                                                                                                               OnException),

                                                        GetChargePointListUpdatesResponseXML.MapElementsOrFail(OCHPNS.Default + "chargePointInfoArray",
                                                                                                               ChargePointInfo.Parse,
                                                                                                               OnException)

                                                    );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, GetChargePointListUpdatesResponseXML, e);

                GetChargePointListUpdatesResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, GetChargePointListUpdatesResponseText, out GetChargePointListUpdatesResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get charge point list response.
        /// </summary>
        /// <param name="Request">The get charge point list updates request leading to this response.</param>
        /// <param name="GetChargePointListUpdatesResponseText">The text to parse.</param>
        /// <param name="GetChargePointListUpdatesResponse">The parsed get charge point list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static Boolean TryParse(GetChargePointListUpdatesRequest       Request,
                                       String                                 GetChargePointListUpdatesResponseText,
                                       out GetChargePointListUpdatesResponse  GetChargePointListUpdatesResponse,
                                       OnExceptionDelegate                    OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(GetChargePointListUpdatesResponseText).Root,
                             out GetChargePointListUpdatesResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, GetChargePointListUpdatesResponseText, e);
            }

            GetChargePointListUpdatesResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetChargePointListUpdatesResponse",

                   Result.ToXML(),

                   ChargePoints.SafeSelect(chargepoint => chargepoint.ToXML(OCHPNS.Default + "chargePointInfoArray"))

               );

        #endregion


        #region Operator overloading

        #region Operator == (GetChargePointListUpdatesResponse1, GetChargePointListUpdatesResponse2)

        /// <summary>
        /// Compares two get charge point list update responses for equality.
        /// </summary>
        /// <param name="GetChargePointListUpdatesResponse1">A get charge point list update response.</param>
        /// <param name="GetChargePointListUpdatesResponse2">Another get charge point list update response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetChargePointListUpdatesResponse GetChargePointListUpdatesResponse1, GetChargePointListUpdatesResponse GetChargePointListUpdatesResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GetChargePointListUpdatesResponse1, GetChargePointListUpdatesResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetChargePointListUpdatesResponse1 is null) || ((Object) GetChargePointListUpdatesResponse2 is null))
                return false;

            return GetChargePointListUpdatesResponse1.Equals(GetChargePointListUpdatesResponse2);

        }

        #endregion

        #region Operator != (GetChargePointListUpdatesResponse1, GetChargePointListUpdatesResponse2)

        /// <summary>
        /// Compares two get charge point list update responses for inequality.
        /// </summary>
        /// <param name="GetChargePointListUpdatesResponse1">A get charge point list update response.</param>
        /// <param name="GetChargePointListUpdatesResponse2">Another get charge point list update response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetChargePointListUpdatesResponse GetChargePointListUpdatesResponse1, GetChargePointListUpdatesResponse GetChargePointListUpdatesResponse2)

            => !(GetChargePointListUpdatesResponse1 == GetChargePointListUpdatesResponse2);

        #endregion

        #endregion

        #region IEquatable<GetChargePointListUpdatesResponse> Members

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

            // Check if the given object is a get charge point list update response.
            var GetChargePointListUpdatesResponse = Object as GetChargePointListUpdatesResponse;
            if ((Object) GetChargePointListUpdatesResponse is null)
                return false;

            return this.Equals(GetChargePointListUpdatesResponse);

        }

        #endregion

        #region Equals(GetChargePointListUpdatesResponse)

        /// <summary>
        /// Compares two get charge point list update responses for equality.
        /// </summary>
        /// <param name="GetChargePointListUpdatesResponse">A get charge point list update response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetChargePointListUpdatesResponse GetChargePointListUpdatesResponse)
        {

            if ((Object) GetChargePointListUpdatesResponse is null)
                return false;

            return this.Result. Equals(GetChargePointListUpdatesResponse.Result);

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

                return ChargePoints is not null

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
