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
    /// An OCHP get roaming authorisation list response.
    /// </summary>
    public class GetRoamingAuthorisationListResponse : AResponse
    {

        #region Properties

        /// <summary>
        /// An enumeration of authorisation card infos.
        /// </summary>
        public IEnumerable<RoamingAuthorisationInfo>  RoamingAuthorisationInfos   { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetRoamingAuthorisationListResponse OK(String Description = null)

            => new GetRoamingAuthorisationListResponse(Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetRoamingAuthorisationListResponse Partly(String Description = null)

            => new GetRoamingAuthorisationListResponse(Result.Unknown(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetRoamingAuthorisationListResponse NotAuthorized(String Description = null)

            => new GetRoamingAuthorisationListResponse(Result.Unknown(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetRoamingAuthorisationListResponse InvalidId(String Description = null)

            => new GetRoamingAuthorisationListResponse(Result.Unknown(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetRoamingAuthorisationListResponse Server(String Description = null)

            => new GetRoamingAuthorisationListResponse(Result.Unknown(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetRoamingAuthorisationListResponse Format(String Description = null)

            => new GetRoamingAuthorisationListResponse(Result.Unknown(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP get roaming authorisation list response.
        /// </summary>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="RoamingAuthorisationInfos">An enumeration of authorisation card infos.</param>
        public GetRoamingAuthorisationListResponse(Result                                 Result,
                                                   IEnumerable<RoamingAuthorisationInfo>  RoamingAuthorisationInfos = null)

            : base(Result)

        {

            this.RoamingAuthorisationInfos  = RoamingAuthorisationInfos;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:GetRoamingAuthorisationListResponse>
        //
        //          <ns:result>
        //             <ns:resultCode>
        //                <ns:resultCode>?</ns:resultCode>
        //             </ns:resultCode>
        //             <ns:resultDescription>?</ns:resultDescription>
        //          </ns:result>
        //
        //          <!--Zero or more repetitions:-->
        //          <ns:roamingAuthorisationInfoArray>
        //
        //             <ns:EmtId representation="plain">
        //                < ns:instance>?</ns:instance>
        //                <ns:tokenType>?</ns:tokenType>
        //                <!--Optional:-->
        //                <ns:tokenSubType>?</ns:tokenSubType>
        //             </ns:EmtId>
        //
        //             <ns:contractId>?</ns:contractId>
        //
        //             <!--Optional:-->
        //             <ns:printedNumber>?</ns:printedNumber>
        //
        //             <ns:expiryDate>
        //                <ns:DateTime>?</ns:DateTime>
        //             </ns:expiryDate>
        //
        //          </ns:roamingAuthorisationInfoArray>
        //
        //       </ns:GetRoamingAuthorisationListResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(GetRoamingAuthorisationListResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP roaming authorisation list response.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetRoamingAuthorisationListResponse Parse(XElement             GetRoamingAuthorisationListResponseXML,
                                                                OnExceptionDelegate  OnException = null)
        {

            GetRoamingAuthorisationListResponse _GetRoamingAuthorisationListResponse;

            if (TryParse(GetRoamingAuthorisationListResponseXML, out _GetRoamingAuthorisationListResponse, OnException))
                return _GetRoamingAuthorisationListResponse;

            return null;

        }

        #endregion

        #region (static) Parse(GetRoamingAuthorisationListResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP roaming authorisation list response.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetRoamingAuthorisationListResponse Parse(String               GetRoamingAuthorisationListResponseText,
                                                                OnExceptionDelegate  OnException = null)
        {

            GetRoamingAuthorisationListResponse _GetRoamingAuthorisationListResponse;

            if (TryParse(GetRoamingAuthorisationListResponseText, out _GetRoamingAuthorisationListResponse, OnException))
                return _GetRoamingAuthorisationListResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(GetRoamingAuthorisationListResponseXML,  out GetRoamingAuthorisationListResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP roaming authorisation list response.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListResponseXML">The XML to parse.</param>
        /// <param name="GetRoamingAuthorisationListResponse">The parsed roaming authorisation list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                 GetRoamingAuthorisationListResponseXML,
                                       out GetRoamingAuthorisationListResponse  GetRoamingAuthorisationListResponse,
                                       OnExceptionDelegate                      OnException  = null)
        {

            try
            {

                GetRoamingAuthorisationListResponse = new GetRoamingAuthorisationListResponse(

                                                          GetRoamingAuthorisationListResponseXML.MapElementOrFail(OCHPNS.Default + "result",
                                                                                                                  Result.Parse,
                                                                                                                  OnException),

                                                          GetRoamingAuthorisationListResponseXML.MapElements     (OCHPNS.Default + "roamingAuthorisationInfoArray",
                                                                                                                  RoamingAuthorisationInfo.Parse,
                                                                                                                  OnException)

                                                      );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, GetRoamingAuthorisationListResponseXML, e);

                GetRoamingAuthorisationListResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetRoamingAuthorisationListResponseText, out GetRoamingAuthorisationListResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP roaming authorisation list response.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListResponseText">The text to parse.</param>
        /// <param name="GetRoamingAuthorisationListResponse">The parsed roaming authorisation list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                   GetRoamingAuthorisationListResponseText,
                                       out GetRoamingAuthorisationListResponse  GetRoamingAuthorisationListResponse,
                                       OnExceptionDelegate                      OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetRoamingAuthorisationListResponseText).Root,
                             out GetRoamingAuthorisationListResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, GetRoamingAuthorisationListResponseText, e);
            }

            GetRoamingAuthorisationListResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetRoamingAuthorisationListResponse",

                   new XElement(OCHPNS.Default + "result", Result.ToXML()),

                   RoamingAuthorisationInfos.Select(info => info.ToXML(OCHPNS.Default + "roamingAuthorisationInfoArray"))

               );

        #endregion


        #region Operator overloading

        #region Operator == (GetRoamingAuthorisationListResponse1, GetRoamingAuthorisationListResponse2)

        /// <summary>
        /// Compares two roaming authorisation lists for equality.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListResponse1">A roaming authorisation list response.</param>
        /// <param name="GetRoamingAuthorisationListResponse2">Another roaming authorisation list response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetRoamingAuthorisationListResponse GetRoamingAuthorisationListResponse1, GetRoamingAuthorisationListResponse GetRoamingAuthorisationListResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetRoamingAuthorisationListResponse1, GetRoamingAuthorisationListResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetRoamingAuthorisationListResponse1 == null) || ((Object) GetRoamingAuthorisationListResponse2 == null))
                return false;

            return GetRoamingAuthorisationListResponse1.Equals(GetRoamingAuthorisationListResponse2);

        }

        #endregion

        #region Operator != (GetRoamingAuthorisationListResponse1, GetRoamingAuthorisationListResponse2)

        /// <summary>
        /// Compares two roaming authorisation lists for inequality.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListResponse1">A roaming authorisation list response.</param>
        /// <param name="GetRoamingAuthorisationListResponse2">Another roaming authorisation list response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetRoamingAuthorisationListResponse GetRoamingAuthorisationListResponse1, GetRoamingAuthorisationListResponse GetRoamingAuthorisationListResponse2)

            => !(GetRoamingAuthorisationListResponse1 == GetRoamingAuthorisationListResponse2);

        #endregion

        #endregion

        #region IEquatable<GetRoamingAuthorisationListResponse> Members

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

            // Check if the given object is a roaming authorisation list response.
            var GetRoamingAuthorisationListResponse = Object as GetRoamingAuthorisationListResponse;
            if ((Object) GetRoamingAuthorisationListResponse == null)
                return false;

            return this.Equals(GetRoamingAuthorisationListResponse);

        }

        #endregion

        #region Equals(GetRoamingAuthorisationListResponse)

        /// <summary>
        /// Compares two roaming authorisation lists for equality.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListResponse">A roaming authorisation list to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(GetRoamingAuthorisationListResponse GetRoamingAuthorisationListResponse)
        {

            if ((Object) GetRoamingAuthorisationListResponse == null)
                return false;

            return this.Result. Equals(GetRoamingAuthorisationListResponse.Result);

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

                return RoamingAuthorisationInfos != null

                           ? Result.GetHashCode() * 11 ^
                             RoamingAuthorisationInfos.SafeSelect(info => info.GetHashCode()).Aggregate((a, b) => a ^ b)

                           : Result.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Result,
                             RoamingAuthorisationInfos.Any()
                                 ? " " + RoamingAuthorisationInfos.Count() + " roaming authorisation infos"
                                 : "");

        #endregion

    }

}
