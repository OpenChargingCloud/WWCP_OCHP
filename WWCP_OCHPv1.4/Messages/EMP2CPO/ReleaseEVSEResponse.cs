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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHPdirect release EVSE response.
    /// </summary>
    public class ReleaseEVSEResponse : AResponse<ReleaseEVSERequest,
                                                 ReleaseEVSEResponse>
    {

        #region Properties

        /// <summary>
        /// The session identification for a direct charging process.
        /// </summary>
        public Direct_Id  DirectId           { get; }

        /// <summary>
        /// An optional timestamp when the given charging session will be invalidated
        /// </summary>
        public DateTime?  SessionTimeoutAt   { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The release EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ReleaseEVSEResponse OK(ReleaseEVSERequest  Request,
                                             String              Description = null)

            => new ReleaseEVSEResponse(Request,
                                       Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The release EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ReleaseEVSEResponse Partly(ReleaseEVSERequest  Request,
                                                 String              Description = null)

            => new ReleaseEVSEResponse(Request,
                                       Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The release EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ReleaseEVSEResponse NotAuthorized(ReleaseEVSERequest  Request,
                                                        String              Description = null)

            => new ReleaseEVSEResponse(Request,
                                       Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The release EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ReleaseEVSEResponse InvalidId(ReleaseEVSERequest  Request,
                                                    String              Description = null)

            => new ReleaseEVSEResponse(Request,
                                       Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The release EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ReleaseEVSEResponse Server(ReleaseEVSERequest  Request,
                                                 String              Description = null)

            => new ReleaseEVSEResponse(Request,
                                       Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The release EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ReleaseEVSEResponse Format(ReleaseEVSERequest  Request,
                                                 String              Description = null)

            => new ReleaseEVSEResponse(Request,
                                       Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHPdirect release EVSE response.
        /// </summary>
        /// <param name="Request">The release EVSE request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="DirectId">The session identification for a direct charging process.</param>
        /// <param name="SessionTimeoutAt">An optional timestamp when the given charging session will be invalidated.</param>
        public ReleaseEVSEResponse(ReleaseEVSERequest  Request,
                                   Result              Result,
                                   Direct_Id           DirectId          = null,
                                   DateTime?           SessionTimeoutAt  = null)

            : base(Request, Result)

        {

            #region Initial checks

            if (SessionTimeoutAt.HasValue && SessionTimeoutAt.Value <= DateTime.UtcNow)
                throw new ArgumentException("The given reservation end time must be after than the current time!");

            #endregion

            this.DirectId          = DirectId;
            this.SessionTimeoutAt  = SessionTimeoutAt ?? new DateTime?();

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:ReleaseEvseResponse>
        //
        //          <ns:result>
        //             <ns:resultCode>
        //                <ns:resultCode>?</ns:resultCode>
        //             </ns:resultCode>
        //             <ns:resultDescription>?</ns:resultDescription>
        //          </ns:result>
        //
        //          <!--Optional:-->
        //          <ns:directId>?</ns:directId>
        //
        //          <!--Optional:-->
        //          <ns:ttl>
        //             <ns:DateTime>?</ns:DateTime>
        //          </ns:ttl>
        //
        //       </ns:ReleaseEvseResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, ReleaseEVSEResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHPdirect release EVSE response.
        /// </summary>
        /// <param name="Request">The release EVSE request leading to this response.</param>
        /// <param name="ReleaseEVSEResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ReleaseEVSEResponse Parse(ReleaseEVSERequest   Request,
                                                XElement             ReleaseEVSEResponseXML,
                                                OnExceptionDelegate  OnException = null)
        {

            ReleaseEVSEResponse _ReleaseEVSEResponse;

            if (TryParse(Request, ReleaseEVSEResponseXML, out _ReleaseEVSEResponse, OnException))
                return _ReleaseEVSEResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, ReleaseEVSEResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHPdirect release EVSE response.
        /// </summary>
        /// <param name="Request">The release EVSE request leading to this response.</param>
        /// <param name="ReleaseEVSEResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ReleaseEVSEResponse Parse(ReleaseEVSERequest   Request,
                                                String               ReleaseEVSEResponseText,
                                                OnExceptionDelegate  OnException = null)
        {

            ReleaseEVSEResponse _ReleaseEVSEResponse;

            if (TryParse(Request, ReleaseEVSEResponseText, out _ReleaseEVSEResponse, OnException))
                return _ReleaseEVSEResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, ReleaseEVSEResponseXML,  out ReleaseEVSEResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHPdirect release EVSE response.
        /// </summary>
        /// <param name="Request">The release EVSE request leading to this response.</param>
        /// <param name="ReleaseEVSEResponseXML">The XML to parse.</param>
        /// <param name="ReleaseEVSEResponse">The parsed release EVSE response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(ReleaseEVSERequest       Request,
                                       XElement                 ReleaseEVSEResponseXML,
                                       out ReleaseEVSEResponse  ReleaseEVSEResponse,
                                       OnExceptionDelegate      OnException  = null)
        {

            try
            {

                ReleaseEVSEResponse = new ReleaseEVSEResponse(

                                          Request,

                                          ReleaseEVSEResponseXML.MapElementOrFail  (OCHPNS.Default + "result",
                                                                                    Result.Parse,
                                                                                    OnException),

                                          ReleaseEVSEResponseXML.MapValueOrNull    (OCHPNS.Default + "directId",
                                                                                    Direct_Id.Parse),

                                          ReleaseEVSEResponseXML.MapValueOrNullable(OCHPNS.Default + "ttl",
                                                                                    OCHPNS.Default + "DateTime",
                                                                                    DateTime.Parse)

                                      );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, ReleaseEVSEResponseXML, e);

                ReleaseEVSEResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, ReleaseEVSEResponseText, out ReleaseEVSEResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHPdirect release EVSE response.
        /// </summary>
        /// <param name="Request">The release EVSE request leading to this response.</param>
        /// <param name="ReleaseEVSEResponseText">The text to parse.</param>
        /// <param name="ReleaseEVSEResponse">The parsed release EVSE response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(ReleaseEVSERequest       Request,
                                       String                   ReleaseEVSEResponseText,
                                       out ReleaseEVSEResponse  ReleaseEVSEResponse,
                                       OnExceptionDelegate      OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(ReleaseEVSEResponseText).Root,
                             out ReleaseEVSEResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, ReleaseEVSEResponseText, e);
            }

            ReleaseEVSEResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "ReleaseEVSEResponse",

                   Result.ToXML(),

                   DirectId != null
                       ? new XElement(OCHPNS.Default + "directId",  DirectId.ToString())
                       : null,

                   SessionTimeoutAt.HasValue
                       ? new XElement(OCHPNS.Default + "ttl",
                             new XElement(OCHPNS.Default + "DateTime", SessionTimeoutAt.Value.ToIso8601())
                         )
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (ReleaseEVSEResponse1, ReleaseEVSEResponse2)

        /// <summary>
        /// Compares two release EVSE responses for equality.
        /// </summary>
        /// <param name="ReleaseEVSEResponse1">A release EVSE response.</param>
        /// <param name="ReleaseEVSEResponse2">Another release EVSE response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ReleaseEVSEResponse ReleaseEVSEResponse1, ReleaseEVSEResponse ReleaseEVSEResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ReleaseEVSEResponse1, ReleaseEVSEResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ReleaseEVSEResponse1 == null) || ((Object) ReleaseEVSEResponse2 == null))
                return false;

            return ReleaseEVSEResponse1.Equals(ReleaseEVSEResponse2);

        }

        #endregion

        #region Operator != (ReleaseEVSEResponse1, ReleaseEVSEResponse2)

        /// <summary>
        /// Compares two release EVSE responses for inequality.
        /// </summary>
        /// <param name="ReleaseEVSEResponse1">A release EVSE response.</param>
        /// <param name="ReleaseEVSEResponse2">Another release EVSE response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ReleaseEVSEResponse ReleaseEVSEResponse1, ReleaseEVSEResponse ReleaseEVSEResponse2)

            => !(ReleaseEVSEResponse1 == ReleaseEVSEResponse2);

        #endregion

        #endregion

        #region IEquatable<ReleaseEVSEResponse> Members

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

            // Check if the given object is a release EVSE response.
            var ReleaseEVSEResponse = Object as ReleaseEVSEResponse;
            if ((Object) ReleaseEVSEResponse == null)
                return false;

            return this.Equals(ReleaseEVSEResponse);

        }

        #endregion

        #region Equals(ReleaseEVSEResponse)

        /// <summary>
        /// Compares two release EVSE responses for equality.
        /// </summary>
        /// <param name="ReleaseEVSEResponse">A release EVSE response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(ReleaseEVSEResponse ReleaseEVSEResponse)
        {

            if ((Object) ReleaseEVSEResponse == null)
                return false;

            return this.Result.Equals(ReleaseEVSEResponse.Result) &&

                   (DirectId != null
                       ? DirectId.Equals(ReleaseEVSEResponse.DirectId)
                       : true) &&

                   (SessionTimeoutAt.HasValue
                       ? SessionTimeoutAt.Equals(ReleaseEVSEResponse.SessionTimeoutAt)
                       : true);

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

                return (DirectId != null
                            ? DirectId.GetHashCode() * 17
                            : 0) ^

                       (SessionTimeoutAt.HasValue
                            ? SessionTimeoutAt.GetHashCode() * 11
                            : 0) ^

                       Result.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Result,

                             DirectId != null
                                 ? " for " + DirectId
                                 : "",

                             SessionTimeoutAt.HasValue
                                 ? " timeout at " + SessionTimeoutAt.Value.ToIso8601()
                                 : "");

        #endregion

    }

}
