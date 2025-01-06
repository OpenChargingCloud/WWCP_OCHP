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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHP set set roaming authorisation list response response.
    /// </summary>
    public class SetRoamingAuthorisationListResponse : AResponse<SetRoamingAuthorisationListRequest,
                                                                 SetRoamingAuthorisationListResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of refused authorisation card infos.
        /// </summary>
        public IEnumerable<RoamingAuthorisationInfo>  RefusedRoamingAuthorisationInfos   { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The set roaming authorisation list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static SetRoamingAuthorisationListResponse OK(SetRoamingAuthorisationListRequest  Request,
                                                             String                              Description = null)

            => new SetRoamingAuthorisationListResponse(Request,
                                                       Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The set roaming authorisation list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static SetRoamingAuthorisationListResponse Partly(SetRoamingAuthorisationListRequest  Request,
                                                                 String                              Description = null)

            => new SetRoamingAuthorisationListResponse(Request,
                                                       Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The set roaming authorisation list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static SetRoamingAuthorisationListResponse NotAuthorized(SetRoamingAuthorisationListRequest  Request,
                                                                        String                              Description = null)

            => new SetRoamingAuthorisationListResponse(Request,
                                                       Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The set roaming authorisation list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static SetRoamingAuthorisationListResponse InvalidId(SetRoamingAuthorisationListRequest  Request,
                                                                    String                              Description = null)

            => new SetRoamingAuthorisationListResponse(Request,
                                                       Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The set roaming authorisation list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static SetRoamingAuthorisationListResponse Server(SetRoamingAuthorisationListRequest  Request,
                                                                 String                              Description = null)

            => new SetRoamingAuthorisationListResponse(Request,
                                                       Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The set roaming authorisation list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static SetRoamingAuthorisationListResponse Format(SetRoamingAuthorisationListRequest  Request,
                                                                 String                              Description = null)

            => new SetRoamingAuthorisationListResponse(Request,
                                                       Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP set set roaming authorisation list response response.
        /// </summary>
        /// <param name="Request">The set roaming authorisation list request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="RefusedRoamingAuthorisationInfos">An enumeration of authorisation card infos.</param>
        public SetRoamingAuthorisationListResponse(SetRoamingAuthorisationListRequest     Request,
                                                   Result                                 Result,
                                                   IEnumerable<RoamingAuthorisationInfo>  RefusedRoamingAuthorisationInfos = null)

            : base(Request, Result)

        {

            this.RefusedRoamingAuthorisationInfos = RefusedRoamingAuthorisationInfos;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:SetRoamingAuthorisationListResponse>
        //
        //          <ns:result>
        //             <ns:resultCode>
        //                <ns:resultCode>?</ns:resultCode>
        //             </ns:resultCode>
        //             <ns:resultDescription>?</ns:resultDescription>
        //          </ns:result>
        //
        //          <!--Zero or more repetitions:-->
        //          <ns:refusedRoamingAuthorisationInfo>
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
        //          </ns:refusedRoamingAuthorisationInfo>
        //
        //       </ns:SetRoamingAuthorisationListResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, SetRoamingAuthorisationListResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP set roaming authorisation list response.
        /// </summary>
        /// <param name="Request">The set roaming authorisation list request leading to this response.</param>
        /// <param name="SetRoamingAuthorisationListResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetRoamingAuthorisationListResponse Parse(SetRoamingAuthorisationListRequest  Request,
                                                                XElement                            SetRoamingAuthorisationListResponseXML,
                                                                OnExceptionDelegate                 OnException = null)
        {

            if (TryParse(Request, SetRoamingAuthorisationListResponseXML, out SetRoamingAuthorisationListResponse _SetRoamingAuthorisationListResponse, OnException))
                return _SetRoamingAuthorisationListResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, SetRoamingAuthorisationListResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP set roaming authorisation list response.
        /// </summary>
        /// <param name="Request">The set roaming authorisation list request leading to this response.</param>
        /// <param name="SetRoamingAuthorisationListResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetRoamingAuthorisationListResponse Parse(SetRoamingAuthorisationListRequest  Request,
                                                                String                              SetRoamingAuthorisationListResponseText,
                                                                OnExceptionDelegate                 OnException = null)
        {

            if (TryParse(Request, SetRoamingAuthorisationListResponseText, out SetRoamingAuthorisationListResponse _SetRoamingAuthorisationListResponse, OnException))
                return _SetRoamingAuthorisationListResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, SetRoamingAuthorisationListResponseXML,  out SetRoamingAuthorisationListResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP set roaming authorisation list response.
        /// </summary>
        /// <param name="Request">The set roaming authorisation list request leading to this response.</param>
        /// <param name="SetRoamingAuthorisationListResponseXML">The XML to parse.</param>
        /// <param name="SetRoamingAuthorisationListResponse">The parsed set roaming authorisation list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetRoamingAuthorisationListRequest       Request,
                                       XElement                                 SetRoamingAuthorisationListResponseXML,
                                       out SetRoamingAuthorisationListResponse  SetRoamingAuthorisationListResponse,
                                       OnExceptionDelegate                      OnException  = null)
        {

            try
            {

                SetRoamingAuthorisationListResponse = new SetRoamingAuthorisationListResponse(

                                                          Request,

                                                          SetRoamingAuthorisationListResponseXML.MapElementOrFail(OCHPNS.Default + "result",
                                                                                                                  Result.Parse,
                                                                                                                  OnException),

                                                          SetRoamingAuthorisationListResponseXML.MapElements     (OCHPNS.Default + "refusedRoamingAuthorisationInfo",
                                                                                                                  RoamingAuthorisationInfo.Parse,
                                                                                                                  OnException)

                                                      );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SetRoamingAuthorisationListResponseXML, e);

                SetRoamingAuthorisationListResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, SetRoamingAuthorisationListResponseText, out SetRoamingAuthorisationListResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP set roaming authorisation list response.
        /// </summary>
        /// <param name="Request">The set roaming authorisation list request leading to this response.</param>
        /// <param name="SetRoamingAuthorisationListResponseText">The text to parse.</param>
        /// <param name="SetRoamingAuthorisationListResponse">The parsed set roaming authorisation list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SetRoamingAuthorisationListRequest       Request,
                                       String                                   SetRoamingAuthorisationListResponseText,
                                       out SetRoamingAuthorisationListResponse  SetRoamingAuthorisationListResponse,
                                       OnExceptionDelegate                      OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(SetRoamingAuthorisationListResponseText).Root,
                             out SetRoamingAuthorisationListResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SetRoamingAuthorisationListResponseText, e);
            }

            SetRoamingAuthorisationListResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "SetRoamingAuthorisationListResponse",

                   Result.ToXML(),

                   RefusedRoamingAuthorisationInfos.Select(info => info.ToXML(OCHPNS.Default + "refusedRoamingAuthorisationInfo"))

               );

        #endregion


        #region Operator overloading

        #region Operator == (SetRoamingAuthorisationListResponse1, SetRoamingAuthorisationListResponse2)

        /// <summary>
        /// Compares two set roaming authorisation list responses for equality.
        /// </summary>
        /// <param name="SetRoamingAuthorisationListResponse1">A set roaming authorisation list response.</param>
        /// <param name="SetRoamingAuthorisationListResponse2">Another set roaming authorisation list response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetRoamingAuthorisationListResponse SetRoamingAuthorisationListResponse1, SetRoamingAuthorisationListResponse SetRoamingAuthorisationListResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetRoamingAuthorisationListResponse1, SetRoamingAuthorisationListResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SetRoamingAuthorisationListResponse1 == null) || ((Object) SetRoamingAuthorisationListResponse2 == null))
                return false;

            return SetRoamingAuthorisationListResponse1.Equals(SetRoamingAuthorisationListResponse2);

        }

        #endregion

        #region Operator != (SetRoamingAuthorisationListResponse1, SetRoamingAuthorisationListResponse2)

        /// <summary>
        /// Compares two set roaming authorisation list responses for inequality.
        /// </summary>
        /// <param name="SetRoamingAuthorisationListResponse1">A set roaming authorisation list response.</param>
        /// <param name="SetRoamingAuthorisationListResponse2">Another set roaming authorisation list response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetRoamingAuthorisationListResponse SetRoamingAuthorisationListResponse1, SetRoamingAuthorisationListResponse SetRoamingAuthorisationListResponse2)

            => !(SetRoamingAuthorisationListResponse1 == SetRoamingAuthorisationListResponse2);

        #endregion

        #endregion

        #region IEquatable<SetRoamingAuthorisationListResponse> Members

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

            // Check if the given object is a set roaming authorisation list response.
            var SetRoamingAuthorisationListResponse = Object as SetRoamingAuthorisationListResponse;
            if ((Object) SetRoamingAuthorisationListResponse == null)
                return false;

            return this.Equals(SetRoamingAuthorisationListResponse);

        }

        #endregion

        #region Equals(SetRoamingAuthorisationListResponse)

        /// <summary>
        /// Compares two set roaming authorisation list responses for equality.
        /// </summary>
        /// <param name="SetRoamingAuthorisationListResponse">A set roaming authorisation list response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetRoamingAuthorisationListResponse SetRoamingAuthorisationListResponse)
        {

            if ((Object) SetRoamingAuthorisationListResponse == null)
                return false;

            return this.Result. Equals(SetRoamingAuthorisationListResponse.Result);

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

                return RefusedRoamingAuthorisationInfos != null

                           ? Result.GetHashCode() * 11 ^
                             RefusedRoamingAuthorisationInfos.SafeSelect(info => info.GetHashCode()).Aggregate((a, b) => a ^ b)

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
                             RefusedRoamingAuthorisationInfos.Any()
                                 ? " " + RefusedRoamingAuthorisationInfos.Count() + " refused roaming authorisation infos"
                                 : "");

        #endregion

    }

}
