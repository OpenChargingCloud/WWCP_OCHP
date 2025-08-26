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
    /// An OCHP update roaming authorisation list response.
    /// </summary>
    public class UpdateRoamingAuthorisationListResponse : AResponse<UpdateRoamingAuthorisationListRequest,
                                                                    UpdateRoamingAuthorisationListResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of refused authorisation card info updates.
        /// </summary>
        public IEnumerable<RoamingAuthorisationInfo>  RefusedRoamingAuthorisationInfos   { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The update roaming authorisation list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateRoamingAuthorisationListResponse OK(UpdateRoamingAuthorisationListRequest  Request,
                                                                String                                 Description = null)

            => new UpdateRoamingAuthorisationListResponse(Request,
                                                          Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The update roaming authorisation list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateRoamingAuthorisationListResponse Partly(UpdateRoamingAuthorisationListRequest  Request,
                                                                    String                                 Description = null)

            => new UpdateRoamingAuthorisationListResponse(Request,
                                                          Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The update roaming authorisation list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateRoamingAuthorisationListResponse NotAuthorized(UpdateRoamingAuthorisationListRequest  Request,
                                                                           String                                 Description = null)

            => new UpdateRoamingAuthorisationListResponse(Request,
                                                          Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The update roaming authorisation list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateRoamingAuthorisationListResponse InvalidId(UpdateRoamingAuthorisationListRequest  Request,
                                                                       String                                 Description = null)

            => new UpdateRoamingAuthorisationListResponse(Request,
                                                          Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The update roaming authorisation list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateRoamingAuthorisationListResponse Server(UpdateRoamingAuthorisationListRequest  Request,
                                                                    String                                 Description = null)

            => new UpdateRoamingAuthorisationListResponse(Request,
                                                          Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The update roaming authorisation list request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateRoamingAuthorisationListResponse Format(UpdateRoamingAuthorisationListRequest  Request,
                                                                    String                                 Description = null)

            => new UpdateRoamingAuthorisationListResponse(Request,
                                                          Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP update roaming authorisation list response.
        /// </summary>
        /// <param name="Request">The update roaming authorisation list request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="RefusedRoamingAuthorisationInfos">An enumeration of refused authorisation card info updates.</param>
        public UpdateRoamingAuthorisationListResponse(UpdateRoamingAuthorisationListRequest  Request,
                                                      Result                                 Result,
                                                      IEnumerable<RoamingAuthorisationInfo>  RefusedRoamingAuthorisationInfos = null)

            : base(Request, Result)

        {

            this.RefusedRoamingAuthorisationInfos  = RefusedRoamingAuthorisationInfos;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:UpdateRoamingAuthorisationListResponse>
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
        //       </ns:UpdateRoamingAuthorisationListResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, UpdateRoamingAuthorisationListResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP update roaming authorisation list response.
        /// </summary>
        /// <param name="Request">The update roaming authorisation list request leading to this response.</param>
        /// <param name="UpdateRoamingAuthorisationListResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateRoamingAuthorisationListResponse Parse(UpdateRoamingAuthorisationListRequest  Request,
                                                                   XElement                               UpdateRoamingAuthorisationListResponseXML,
                                                                   OnExceptionDelegate                    OnException = null)
        {

            UpdateRoamingAuthorisationListResponse _UpdateRoamingAuthorisationListResponse;

            if (TryParse(Request, UpdateRoamingAuthorisationListResponseXML, out _UpdateRoamingAuthorisationListResponse, OnException))
                return _UpdateRoamingAuthorisationListResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, UpdateRoamingAuthorisationListResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP update roaming authorisation list response.
        /// </summary>
        /// <param name="Request">The update roaming authorisation list request leading to this response.</param>
        /// <param name="UpdateRoamingAuthorisationListResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateRoamingAuthorisationListResponse Parse(UpdateRoamingAuthorisationListRequest  Request,
                                                                   String                                 UpdateRoamingAuthorisationListResponseText,
                                                                   OnExceptionDelegate                    OnException = null)
        {

            UpdateRoamingAuthorisationListResponse _UpdateRoamingAuthorisationListResponse;

            if (TryParse(Request, UpdateRoamingAuthorisationListResponseText, out _UpdateRoamingAuthorisationListResponse, OnException))
                return _UpdateRoamingAuthorisationListResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, UpdateRoamingAuthorisationListResponseXML,  out UpdateRoamingAuthorisationListResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP update roaming authorisation list response.
        /// </summary>
        /// <param name="Request">The update roaming authorisation list request leading to this response.</param>
        /// <param name="UpdateRoamingAuthorisationListResponseXML">The XML to parse.</param>
        /// <param name="UpdateRoamingAuthorisationListResponse">The parsed update roaming authorisation list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(UpdateRoamingAuthorisationListRequest       Request,
                                       XElement                                    UpdateRoamingAuthorisationListResponseXML,
                                       out UpdateRoamingAuthorisationListResponse  UpdateRoamingAuthorisationListResponse,
                                       OnExceptionDelegate                         OnException  = null)
        {

            try
            {

                UpdateRoamingAuthorisationListResponse = new UpdateRoamingAuthorisationListResponse(

                                                             Request,

                                                             UpdateRoamingAuthorisationListResponseXML.MapElementOrFail(OCHPNS.Default + "result",
                                                                                                                        Result.Parse,
                                                                                                                        OnException),

                                                             UpdateRoamingAuthorisationListResponseXML.MapElements     (OCHPNS.Default + "refusedRoamingAuthorisationInfo",
                                                                                                                        RoamingAuthorisationInfo.Parse,
                                                                                                                        OnException)

                                                         );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, UpdateRoamingAuthorisationListResponseXML, e);

                UpdateRoamingAuthorisationListResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, UpdateRoamingAuthorisationListResponseText, out UpdateRoamingAuthorisationListResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP update roaming authorisation list response.
        /// </summary>
        /// <param name="Request">The update roaming authorisation list request leading to this response.</param>
        /// <param name="UpdateRoamingAuthorisationListResponseText">The text to parse.</param>
        /// <param name="UpdateRoamingAuthorisationListResponse">The parsed update roaming authorisation list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(UpdateRoamingAuthorisationListRequest       Request,
                                       String                                      UpdateRoamingAuthorisationListResponseText,
                                       out UpdateRoamingAuthorisationListResponse  UpdateRoamingAuthorisationListResponse,
                                       OnExceptionDelegate                         OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(UpdateRoamingAuthorisationListResponseText).Root,
                             out UpdateRoamingAuthorisationListResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, UpdateRoamingAuthorisationListResponseText, e);
            }

            UpdateRoamingAuthorisationListResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "UpdateRoamingAuthorisationListResponse",

                   Result.ToXML(),

                   RefusedRoamingAuthorisationInfos.Select(info => info.ToXML(OCHPNS.Default + "refusedRoamingAuthorisationInfo"))

               );

        #endregion


        #region Operator overloading

        #region Operator == (UpdateRoamingAuthorisationListResponse1, UpdateRoamingAuthorisationListResponse2)

        /// <summary>
        /// Compares two update roaming authorisation list responses for equality.
        /// </summary>
        /// <param name="UpdateRoamingAuthorisationListResponse1">An update roaming authorisation list response.</param>
        /// <param name="UpdateRoamingAuthorisationListResponse2">Another update roaming authorisation list response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (UpdateRoamingAuthorisationListResponse UpdateRoamingAuthorisationListResponse1, UpdateRoamingAuthorisationListResponse UpdateRoamingAuthorisationListResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(UpdateRoamingAuthorisationListResponse1, UpdateRoamingAuthorisationListResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) UpdateRoamingAuthorisationListResponse1 is null) || ((Object) UpdateRoamingAuthorisationListResponse2 is null))
                return false;

            return UpdateRoamingAuthorisationListResponse1.Equals(UpdateRoamingAuthorisationListResponse2);

        }

        #endregion

        #region Operator != (UpdateRoamingAuthorisationListResponse1, UpdateRoamingAuthorisationListResponse2)

        /// <summary>
        /// Compares two update roaming authorisation list responses for inequality.
        /// </summary>
        /// <param name="UpdateRoamingAuthorisationListResponse1">An update roaming authorisation list response.</param>
        /// <param name="UpdateRoamingAuthorisationListResponse2">Another update roaming authorisation list response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (UpdateRoamingAuthorisationListResponse UpdateRoamingAuthorisationListResponse1, UpdateRoamingAuthorisationListResponse UpdateRoamingAuthorisationListResponse2)

            => !(UpdateRoamingAuthorisationListResponse1 == UpdateRoamingAuthorisationListResponse2);

        #endregion

        #endregion

        #region IEquatable<UpdateRoamingAuthorisationListResponse> Members

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

            // Check if the given object is a update roaming authorisation list response.
            var UpdateRoamingAuthorisationListResponse = Object as UpdateRoamingAuthorisationListResponse;
            if ((Object) UpdateRoamingAuthorisationListResponse is null)
                return false;

            return this.Equals(UpdateRoamingAuthorisationListResponse);

        }

        #endregion

        #region Equals(UpdateRoamingAuthorisationListResponse)

        /// <summary>
        /// Compares two update roaming authorisation list responses for equality.
        /// </summary>
        /// <param name="UpdateRoamingAuthorisationListResponse">A update roaming authorisation list response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(UpdateRoamingAuthorisationListResponse UpdateRoamingAuthorisationListResponse)
        {

            if ((Object) UpdateRoamingAuthorisationListResponse is null)
                return false;

            return this.Result. Equals(UpdateRoamingAuthorisationListResponse.Result);

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

                return RefusedRoamingAuthorisationInfos is not null

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
