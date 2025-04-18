﻿/*
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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHP get roaming authorisation list update response.
    /// </summary>
    public class GetRoamingAuthorisationListUpdatesResponse : AResponse<GetRoamingAuthorisationListUpdatesRequest,
                                                                        GetRoamingAuthorisationListUpdatesResponse>
    {

        #region Properties

        /// <summary>
        /// The authorisation card info updates.
        /// </summary>
        public IEnumerable<RoamingAuthorisationInfo>  RoamingAuthorisationInfos   { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The get roaming authorisation list update request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetRoamingAuthorisationListUpdatesResponse OK(GetRoamingAuthorisationListUpdatesRequest  Request,
                                                                    String                                     Description = null)

            => new GetRoamingAuthorisationListUpdatesResponse(Request,
                                                              Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The get roaming authorisation list update request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetRoamingAuthorisationListUpdatesResponse Partly(GetRoamingAuthorisationListUpdatesRequest  Request,
                                                                        String                                     Description = null)

            => new GetRoamingAuthorisationListUpdatesResponse(Request,
                                                              Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The get roaming authorisation list update request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetRoamingAuthorisationListUpdatesResponse NotAuthorized(GetRoamingAuthorisationListUpdatesRequest  Request,
                                                                               String                                     Description = null)

            => new GetRoamingAuthorisationListUpdatesResponse(Request,
                                                              Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The get roaming authorisation list update request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetRoamingAuthorisationListUpdatesResponse InvalidId(GetRoamingAuthorisationListUpdatesRequest  Request,
                                                                           String                                     Description = null)

            => new GetRoamingAuthorisationListUpdatesResponse(Request,
                                                              Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The get roaming authorisation list update request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetRoamingAuthorisationListUpdatesResponse Server(GetRoamingAuthorisationListUpdatesRequest  Request,
                                                                        String                                     Description = null)

            => new GetRoamingAuthorisationListUpdatesResponse(Request,
                                                              Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The get roaming authorisation list update request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetRoamingAuthorisationListUpdatesResponse Format(GetRoamingAuthorisationListUpdatesRequest  Request,
                                                                        String                                     Description = null)

            => new GetRoamingAuthorisationListUpdatesResponse(Request,
                                                              Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP get roaming authorisation list update response.
        /// </summary>
        /// <param name="Request">The get roaming authorisation list update request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="RoamingAuthorisationInfos">An enumeration of authorisation card info updates.</param>
        public GetRoamingAuthorisationListUpdatesResponse(GetRoamingAuthorisationListUpdatesRequest  Request,
                                                          Result                                     Result,
                                                          IEnumerable<RoamingAuthorisationInfo>      RoamingAuthorisationInfos = null)

            : base(Request, Result)

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
        //       <ns:GetRoamingAuthorisationListUpdatesResponse>
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
        //       </ns:GetRoamingAuthorisationListUpdatesResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, GetRoamingAuthorisationListUpdatesResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get roaming authorisation list update response.
        /// </summary>
        /// <param name="Request">The get roaming authorisation list update request leading to this response.</param>
        /// <param name="GetRoamingAuthorisationListUpdatesResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetRoamingAuthorisationListUpdatesResponse Parse(GetRoamingAuthorisationListUpdatesRequest  Request,
                                                                       XElement                                   GetRoamingAuthorisationListUpdatesResponseXML,
                                                                       OnExceptionDelegate                        OnException = null)
        {

            GetRoamingAuthorisationListUpdatesResponse _GetRoamingAuthorisationListUpdatesResponse;

            if (TryParse(Request, GetRoamingAuthorisationListUpdatesResponseXML, out _GetRoamingAuthorisationListUpdatesResponse, OnException))
                return _GetRoamingAuthorisationListUpdatesResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, GetRoamingAuthorisationListUpdatesResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get roaming authorisation list update response.
        /// </summary>
        /// <param name="Request">The get roaming authorisation list update request leading to this response.</param>
        /// <param name="GetRoamingAuthorisationListUpdatesResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetRoamingAuthorisationListUpdatesResponse Parse(GetRoamingAuthorisationListUpdatesRequest  Request,
                                                                       String                                     GetRoamingAuthorisationListUpdatesResponseText,
                                                                       OnExceptionDelegate                        OnException = null)
        {

            GetRoamingAuthorisationListUpdatesResponse _GetRoamingAuthorisationListUpdatesResponse;

            if (TryParse(Request, GetRoamingAuthorisationListUpdatesResponseText, out _GetRoamingAuthorisationListUpdatesResponse, OnException))
                return _GetRoamingAuthorisationListUpdatesResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, GetRoamingAuthorisationListUpdatesResponseXML,  out GetRoamingAuthorisationListUpdatesResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get roaming authorisation list update response.
        /// </summary>
        /// <param name="Request">The get roaming authorisation list update request leading to this response.</param>
        /// <param name="GetRoamingAuthorisationListUpdatesResponseXML">The XML to parse.</param>
        /// <param name="GetRoamingAuthorisationListUpdatesResponse">The parsed get roaming authorisation list update response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetRoamingAuthorisationListUpdatesRequest       Request,
                                       XElement                                        GetRoamingAuthorisationListUpdatesResponseXML,
                                       out GetRoamingAuthorisationListUpdatesResponse  GetRoamingAuthorisationListUpdatesResponse,
                                       OnExceptionDelegate                             OnException  = null)
        {

            try
            {

                GetRoamingAuthorisationListUpdatesResponse = new GetRoamingAuthorisationListUpdatesResponse(

                                                                 Request,

                                                                 GetRoamingAuthorisationListUpdatesResponseXML.MapElementOrFail(OCHPNS.Default + "result",
                                                                                                                                Result.Parse,
                                                                                                                                OnException),

                                                                 GetRoamingAuthorisationListUpdatesResponseXML.MapElements     (OCHPNS.Default + "roamingAuthorisationInfoArray",
                                                                                                                                RoamingAuthorisationInfo.Parse,
                                                                                                                                OnException)

                                                             );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, GetRoamingAuthorisationListUpdatesResponseXML, e);

                GetRoamingAuthorisationListUpdatesResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, GetRoamingAuthorisationListUpdatesResponseText, out GetRoamingAuthorisationListUpdatesResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get roaming authorisation list update response.
        /// </summary>
        /// <param name="Request">The get roaming authorisation list update request leading to this response.</param>
        /// <param name="GetRoamingAuthorisationListUpdatesResponseText">The text to parse.</param>
        /// <param name="GetRoamingAuthorisationListUpdatesResponse">The parsed get roaming authorisation list update response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetRoamingAuthorisationListUpdatesRequest       Request,
                                       String                                          GetRoamingAuthorisationListUpdatesResponseText,
                                       out GetRoamingAuthorisationListUpdatesResponse  GetRoamingAuthorisationListUpdatesResponse,
                                       OnExceptionDelegate                             OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(GetRoamingAuthorisationListUpdatesResponseText).Root,
                             out GetRoamingAuthorisationListUpdatesResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, GetRoamingAuthorisationListUpdatesResponseText, e);
            }

            GetRoamingAuthorisationListUpdatesResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetRoamingAuthorisationListUpdatesResponse",

                   Result.ToXML(),

                   RoamingAuthorisationInfos.Select(info => info.ToXML(OCHPNS.Default + "roamingAuthorisationInfoArray"))

               );

        #endregion


        #region Operator overloading

        #region Operator == (GetRoamingAuthorisationListUpdatesResponse1, GetRoamingAuthorisationListUpdatesResponse2)

        /// <summary>
        /// Compares two roaming authorisation list updates for equality.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListUpdatesResponse1">A get roaming authorisation list update response.</param>
        /// <param name="GetRoamingAuthorisationListUpdatesResponse2">Another get roaming authorisation list update response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetRoamingAuthorisationListUpdatesResponse GetRoamingAuthorisationListUpdatesResponse1, GetRoamingAuthorisationListUpdatesResponse GetRoamingAuthorisationListUpdatesResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GetRoamingAuthorisationListUpdatesResponse1, GetRoamingAuthorisationListUpdatesResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetRoamingAuthorisationListUpdatesResponse1 == null) || ((Object) GetRoamingAuthorisationListUpdatesResponse2 == null))
                return false;

            return GetRoamingAuthorisationListUpdatesResponse1.Equals(GetRoamingAuthorisationListUpdatesResponse2);

        }

        #endregion

        #region Operator != (GetRoamingAuthorisationListUpdatesResponse1, GetRoamingAuthorisationListUpdatesResponse2)

        /// <summary>
        /// Compares two roaming authorisation list updates for inequality.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListUpdatesResponse1">A get roaming authorisation list update response.</param>
        /// <param name="GetRoamingAuthorisationListUpdatesResponse2">Another get roaming authorisation list update response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetRoamingAuthorisationListUpdatesResponse GetRoamingAuthorisationListUpdatesResponse1, GetRoamingAuthorisationListUpdatesResponse GetRoamingAuthorisationListUpdatesResponse2)

            => !(GetRoamingAuthorisationListUpdatesResponse1 == GetRoamingAuthorisationListUpdatesResponse2);

        #endregion

        #endregion

        #region IEquatable<GetRoamingAuthorisationListUpdatesResponse> Members

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

            // Check if the given object is a get roaming authorisation list update response.
            var GetRoamingAuthorisationListUpdatesResponse = Object as GetRoamingAuthorisationListUpdatesResponse;
            if ((Object) GetRoamingAuthorisationListUpdatesResponse == null)
                return false;

            return this.Equals(GetRoamingAuthorisationListUpdatesResponse);

        }

        #endregion

        #region Equals(GetRoamingAuthorisationListUpdatesResponse)

        /// <summary>
        /// Compares two roaming authorisation list updates for equality.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListUpdatesResponse">A roaming authorisation list update to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetRoamingAuthorisationListUpdatesResponse GetRoamingAuthorisationListUpdatesResponse)
        {

            if ((Object) GetRoamingAuthorisationListUpdatesResponse == null)
                return false;

            return this.Result. Equals(GetRoamingAuthorisationListUpdatesResponse.Result);

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Result,
                             RoamingAuthorisationInfos.Any()
                                 ? " " + RoamingAuthorisationInfos.Count() + " roaming authorisation infos"
                                 : "");

        #endregion




        public class Builder : ABuilder
        {

            #region Properties

            /// <summary>
            /// An enumeration of authorisation card infos.
            /// </summary>
            public IEnumerable<RoamingAuthorisationInfo> RoamingAuthorisationInfos { get; set; }

            #endregion

            public Builder(GetRoamingAuthorisationListUpdatesResponse GetRoamingAuthorisationListUpdatesResponse = null)
            {

                if (GetRoamingAuthorisationListUpdatesResponse != null)
                {

                    this.RoamingAuthorisationInfos = GetRoamingAuthorisationListUpdatesResponse.RoamingAuthorisationInfos;

                    if (GetRoamingAuthorisationListUpdatesResponse.CustomData != null)
                        foreach (var item in GetRoamingAuthorisationListUpdatesResponse.CustomData)
                            CustomData.Add(item.Key, item.Value);

                }

            }


            //public Acknowledgement<T> ToImmutable()

            //    => new Acknowledgement<T>(Request,
            //                              Result,
            //                              StatusCode,
            //                              SessionId,
            //                              PartnerSessionId,
            //                              CustomData);

        }

    }

}
