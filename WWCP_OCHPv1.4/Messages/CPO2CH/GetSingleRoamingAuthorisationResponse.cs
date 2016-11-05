/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHP get single roaming authorisation response.
    /// </summary>
    public class GetSingleRoamingAuthorisationResponse : AResponse<GetSingleRoamingAuthorisationRequest,
                                                                   GetSingleRoamingAuthorisationResponse>
    {

        #region Properties

        /// <summary>
        /// The authorisation card info.
        /// </summary>
        public RoamingAuthorisationInfo  RoamingAuthorisationInfo   { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The get single roaming authorisation request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetSingleRoamingAuthorisationResponse OK(GetSingleRoamingAuthorisationRequest  Request,
                                                               String                                Description = null)

            => new GetSingleRoamingAuthorisationResponse(Request,
                                                         Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The get single roaming authorisation request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetSingleRoamingAuthorisationResponse Partly(GetSingleRoamingAuthorisationRequest  Request,
                                                                   String                                Description = null)

            => new GetSingleRoamingAuthorisationResponse(Request,
                                                         Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The get single roaming authorisation request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetSingleRoamingAuthorisationResponse NotAuthorized(GetSingleRoamingAuthorisationRequest  Request,
                                                                          String                                Description = null)

            => new GetSingleRoamingAuthorisationResponse(Request,
                                                         Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The get single roaming authorisation request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetSingleRoamingAuthorisationResponse InvalidId(GetSingleRoamingAuthorisationRequest  Request,
                                                                      String                                Description = null)

            => new GetSingleRoamingAuthorisationResponse(Request,
                                                         Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The get single roaming authorisation request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetSingleRoamingAuthorisationResponse Server(GetSingleRoamingAuthorisationRequest  Request,
                                                                   String                                Description = null)

            => new GetSingleRoamingAuthorisationResponse(Request,
                                                         Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The get single roaming authorisation request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetSingleRoamingAuthorisationResponse Format(GetSingleRoamingAuthorisationRequest  Request,
                                                                   String                                Description = null)

            => new GetSingleRoamingAuthorisationResponse(Request,
                                                         Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP get single roaming authorisation response.
        /// </summary>
        /// <param name="Request">The get single roaming authorisation request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="RoamingAuthorisationInfo">The authorisation card info.</param>
        public GetSingleRoamingAuthorisationResponse(GetSingleRoamingAuthorisationRequest  Request,
                                                     Result                                Result,
                                                     RoamingAuthorisationInfo              RoamingAuthorisationInfo = null)

            : base(Request, Result)

        {

            this.RoamingAuthorisationInfo = RoamingAuthorisationInfo;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:GetSingleRoamingAuthorisationResponse>
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
        //          <!--Optional:-->
        //          <ns:roamingAuthorisationInfo>
        //             ...
        //          <ns:roamingAuthorisationInfo>
        //
        //       </ns:GetSingleRoamingAuthorisationResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, GetSingleRoamingAuthorisationResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get single roaming authorisation response.
        /// </summary>
        /// <param name="Request">The get single roaming authorisation request leading to this response.</param>
        /// <param name="GetSingleRoamingAuthorisationResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetSingleRoamingAuthorisationResponse Parse(GetSingleRoamingAuthorisationRequest  Request,
                                                                  XElement                              GetSingleRoamingAuthorisationResponseXML,
                                                                  OnExceptionDelegate                   OnException = null)
        {

            GetSingleRoamingAuthorisationResponse _GetSingleRoamingAuthorisationResponse;

            if (TryParse(Request, GetSingleRoamingAuthorisationResponseXML, out _GetSingleRoamingAuthorisationResponse, OnException))
                return _GetSingleRoamingAuthorisationResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, GetSingleRoamingAuthorisationResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get single roaming authorisation response.
        /// </summary>
        /// <param name="Request">The get single roaming authorisation request leading to this response.</param>
        /// <param name="GetSingleRoamingAuthorisationResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetSingleRoamingAuthorisationResponse Parse(GetSingleRoamingAuthorisationRequest  Request,
                                                                  String                                GetSingleRoamingAuthorisationResponseText,
                                                                  OnExceptionDelegate                   OnException = null)
        {

            GetSingleRoamingAuthorisationResponse _GetSingleRoamingAuthorisationResponse;

            if (TryParse(Request, GetSingleRoamingAuthorisationResponseText, out _GetSingleRoamingAuthorisationResponse, OnException))
                return _GetSingleRoamingAuthorisationResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, GetSingleRoamingAuthorisationResponseXML,  out GetSingleRoamingAuthorisationResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get single roaming authorisation response.
        /// </summary>
        /// <param name="Request">The get single roaming authorisation request leading to this response.</param>
        /// <param name="GetSingleRoamingAuthorisationResponseXML">The XML to parse.</param>
        /// <param name="GetSingleRoamingAuthorisationResponse">The parsed get single roaming authorisation response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetSingleRoamingAuthorisationRequest       Request,
                                       XElement                                   GetSingleRoamingAuthorisationResponseXML,
                                       out GetSingleRoamingAuthorisationResponse  GetSingleRoamingAuthorisationResponse,
                                       OnExceptionDelegate                        OnException  = null)
        {

            try
            {

                GetSingleRoamingAuthorisationResponse = new GetSingleRoamingAuthorisationResponse(

                                                            Request,

                                                            GetSingleRoamingAuthorisationResponseXML.MapElementOrFail(OCHPNS.Default + "result",
                                                                                                                      Result.Parse,
                                                                                                                      OnException),

                                                            GetSingleRoamingAuthorisationResponseXML.MapElementOrFail(OCHPNS.Default + "roamingAuthorisationInfo",
                                                                                                                      RoamingAuthorisationInfo.Parse,
                                                                                                                      OnException)

                                                        );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, GetSingleRoamingAuthorisationResponseXML, e);

                GetSingleRoamingAuthorisationResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, GetSingleRoamingAuthorisationResponseText, out GetSingleRoamingAuthorisationResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get single roaming authorisation response.
        /// </summary>
        /// <param name="Request">The get single roaming authorisation request leading to this response.</param>
        /// <param name="GetSingleRoamingAuthorisationResponseText">The text to parse.</param>
        /// <param name="GetSingleRoamingAuthorisationResponse">The parsed get single roaming authorisation response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetSingleRoamingAuthorisationRequest       Request,
                                       String                                     GetSingleRoamingAuthorisationResponseText,
                                       out GetSingleRoamingAuthorisationResponse  GetSingleRoamingAuthorisationResponse,
                                       OnExceptionDelegate                        OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(GetSingleRoamingAuthorisationResponseText).Root,
                             out GetSingleRoamingAuthorisationResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, GetSingleRoamingAuthorisationResponseText, e);
            }

            GetSingleRoamingAuthorisationResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetSingleRoamingAuthorisationResponse",

                   new XElement(OCHPNS.Default + "result", Result.ToXML()),

                   RoamingAuthorisationInfo.ToXML()

               );

        #endregion


        #region Operator overloading

        #region Operator == (GetSingleRoamingAuthorisationResponse1, GetSingleRoamingAuthorisationResponse2)

        /// <summary>
        /// Compares two get single roaming authorisation responses for equality.
        /// </summary>
        /// <param name="GetSingleRoamingAuthorisationResponse1">A get single roaming authorisation response.</param>
        /// <param name="GetSingleRoamingAuthorisationResponse2">Another get single roaming authorisation response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetSingleRoamingAuthorisationResponse GetSingleRoamingAuthorisationResponse1, GetSingleRoamingAuthorisationResponse GetSingleRoamingAuthorisationResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetSingleRoamingAuthorisationResponse1, GetSingleRoamingAuthorisationResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetSingleRoamingAuthorisationResponse1 == null) || ((Object) GetSingleRoamingAuthorisationResponse2 == null))
                return false;

            return GetSingleRoamingAuthorisationResponse1.Equals(GetSingleRoamingAuthorisationResponse2);

        }

        #endregion

        #region Operator != (GetSingleRoamingAuthorisationResponse1, GetSingleRoamingAuthorisationResponse2)

        /// <summary>
        /// Compares two get single roaming authorisation responses for inequality.
        /// </summary>
        /// <param name="GetSingleRoamingAuthorisationResponse1">A get single roaming authorisation response.</param>
        /// <param name="GetSingleRoamingAuthorisationResponse2">Another get single roaming authorisation response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetSingleRoamingAuthorisationResponse GetSingleRoamingAuthorisationResponse1, GetSingleRoamingAuthorisationResponse GetSingleRoamingAuthorisationResponse2)

            => !(GetSingleRoamingAuthorisationResponse1 == GetSingleRoamingAuthorisationResponse2);

        #endregion

        #endregion

        #region IEquatable<GetSingleRoamingAuthorisationResponse> Members

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

            // Check if the given object is a get single roaming authorisation response.
            var GetSingleRoamingAuthorisationResponse = Object as GetSingleRoamingAuthorisationResponse;
            if ((Object) GetSingleRoamingAuthorisationResponse == null)
                return false;

            return this.Equals(GetSingleRoamingAuthorisationResponse);

        }

        #endregion

        #region Equals(GetSingleRoamingAuthorisationResponse)

        /// <summary>
        /// Compares two get single roaming authorisation responses for equality.
        /// </summary>
        /// <param name="GetSingleRoamingAuthorisationResponse">A get single roaming authorisation response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetSingleRoamingAuthorisationResponse GetSingleRoamingAuthorisationResponse)
        {

            if ((Object) GetSingleRoamingAuthorisationResponse == null)
                return false;

            return this.Result. Equals(GetSingleRoamingAuthorisationResponse.Result);

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

                return RoamingAuthorisationInfo != null

                           ? Result.                  GetHashCode() * 11 ^
                             RoamingAuthorisationInfo.GetHashCode()

                           : Result.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Result, " / ", RoamingAuthorisationInfo.ToString());

        #endregion

    }

}
