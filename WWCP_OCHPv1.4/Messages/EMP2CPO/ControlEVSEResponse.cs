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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHPdirect control EVSE response.
    /// </summary>
    public class ControlEVSEResponse : AResponse<ControlEVSERequest,
                                                 ControlEVSEResponse>
    {

        #region Properties

        /// <summary>
        /// The session identification for a direct charging process.
        /// </summary>
        public Direct_Id  DirectId         { get; }

        /// <summary>
        /// An optional timestamp until when the session will timeout.
        /// </summary>
        public DateTime?  SessionTimeout   { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The control EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ControlEVSEResponse OK(ControlEVSERequest  Request,
                                             String              Description = null)

            => new ControlEVSEResponse(Request,
                                       Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The control EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ControlEVSEResponse Partly(ControlEVSERequest  Request,
                                                 String              Description = null)

            => new ControlEVSEResponse(Request,
                                       Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The control EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ControlEVSEResponse NotAuthorized(ControlEVSERequest  Request,
                                                        String              Description = null)

            => new ControlEVSEResponse(Request,
                                       Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The control EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ControlEVSEResponse InvalidId(ControlEVSERequest  Request,
                                                    String              Description = null)

            => new ControlEVSEResponse(Request,
                                       Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The control EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ControlEVSEResponse Server(ControlEVSERequest  Request,
                                                 String              Description = null)

            => new ControlEVSEResponse(Request,
                                       Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The control EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ControlEVSEResponse Format(ControlEVSERequest  Request,
                                                 String              Description = null)

            => new ControlEVSEResponse(Request,
                                       Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHPdirect control EVSE response.
        /// </summary>
        /// <param name="Request">The control EVSE request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="DirectId">The session identification for a direct charging process.</param>
        /// <param name="SessionTimeout">An optional timestamp until when the session will timeout.</param>
        public ControlEVSEResponse(ControlEVSERequest  Request,
                                   Result              Result,
                                   Direct_Id           DirectId        = null,
                                   DateTime?           SessionTimeout  = null)

            : base(Request, Result)

        {

            #region Initial checks

            if (SessionTimeout.HasValue && SessionTimeout.Value <= DateTime.Now)
                throw new ArgumentException("The given reservation end time must be after than the current time!");

            #endregion

            this.DirectId       = DirectId;
            this.SessionTimeout  = SessionTimeout ?? new DateTime?();

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:ControlEvseResponse>
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
        //       </ns:ControlEvseResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, ControlEVSEResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHPdirect control EVSE response.
        /// </summary>
        /// <param name="Request">The control EVSE request leading to this response.</param>
        /// <param name="ControlEVSEResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ControlEVSEResponse Parse(ControlEVSERequest   Request,
                                                XElement             ControlEVSEResponseXML,
                                                OnExceptionDelegate  OnException = null)
        {

            ControlEVSEResponse _ControlEVSEResponse;

            if (TryParse(Request, ControlEVSEResponseXML, out _ControlEVSEResponse, OnException))
                return _ControlEVSEResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, ControlEVSEResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHPdirect control EVSE response.
        /// </summary>
        /// <param name="Request">The control EVSE request leading to this response.</param>
        /// <param name="ControlEVSEResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ControlEVSEResponse Parse(ControlEVSERequest   Request,
                                                String               ControlEVSEResponseText,
                                                OnExceptionDelegate  OnException = null)
        {

            ControlEVSEResponse _ControlEVSEResponse;

            if (TryParse(Request, ControlEVSEResponseText, out _ControlEVSEResponse, OnException))
                return _ControlEVSEResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, ControlEVSEResponseXML,  out ControlEVSEResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHPdirect control EVSE response.
        /// </summary>
        /// <param name="Request">The control EVSE request leading to this response.</param>
        /// <param name="ControlEVSEResponseXML">The XML to parse.</param>
        /// <param name="ControlEVSEResponse">The parsed control EVSE response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(ControlEVSERequest       Request,
                                       XElement                 ControlEVSEResponseXML,
                                       out ControlEVSEResponse  ControlEVSEResponse,
                                       OnExceptionDelegate      OnException  = null)
        {

            try
            {

                ControlEVSEResponse = new ControlEVSEResponse(

                                          Request,

                                          ControlEVSEResponseXML.MapElementOrFail  (OCHPNS.Default + "result",
                                                                                    Result.Parse,
                                                                                    OnException),

                                          ControlEVSEResponseXML.MapValueOrNull    (OCHPNS.Default + "directId",
                                                                                    Direct_Id.Parse),

                                          ControlEVSEResponseXML.MapValueOrNullable(OCHPNS.Default + "ttl",
                                                                                    OCHPNS.Default + "DateTime",
                                                                                    DateTime.Parse)

                                      );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, ControlEVSEResponseXML, e);

                ControlEVSEResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, ControlEVSEResponseText, out ControlEVSEResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHPdirect control EVSE response.
        /// </summary>
        /// <param name="Request">The control EVSE request leading to this response.</param>
        /// <param name="ControlEVSEResponseText">The text to parse.</param>
        /// <param name="ControlEVSEResponse">The parsed control EVSE response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(ControlEVSERequest       Request,
                                       String                   ControlEVSEResponseText,
                                       out ControlEVSEResponse  ControlEVSEResponse,
                                       OnExceptionDelegate      OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(ControlEVSEResponseText).Root,
                             out ControlEVSEResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, ControlEVSEResponseText, e);
            }

            ControlEVSEResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "ControlEVSEResponse",

                   Result.ToXML(),

                   DirectId != null
                       ? new XElement(OCHPNS.Default + "directId",  DirectId.ToString())
                       : null,

                   SessionTimeout.HasValue
                       ? new XElement(OCHPNS.Default + "ttl",
                             new XElement(OCHPNS.Default + "DateTime", SessionTimeout.Value.ToIso8601())
                         )
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (ControlEVSEResponse1, ControlEVSEResponse2)

        /// <summary>
        /// Compares two control EVSE responses for equality.
        /// </summary>
        /// <param name="ControlEVSEResponse1">A control EVSE response.</param>
        /// <param name="ControlEVSEResponse2">Another control EVSE response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ControlEVSEResponse ControlEVSEResponse1, ControlEVSEResponse ControlEVSEResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ControlEVSEResponse1, ControlEVSEResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ControlEVSEResponse1 == null) || ((Object) ControlEVSEResponse2 == null))
                return false;

            return ControlEVSEResponse1.Equals(ControlEVSEResponse2);

        }

        #endregion

        #region Operator != (ControlEVSEResponse1, ControlEVSEResponse2)

        /// <summary>
        /// Compares two control EVSE responses for inequality.
        /// </summary>
        /// <param name="ControlEVSEResponse1">A control EVSE response.</param>
        /// <param name="ControlEVSEResponse2">Another control EVSE response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ControlEVSEResponse ControlEVSEResponse1, ControlEVSEResponse ControlEVSEResponse2)

            => !(ControlEVSEResponse1 == ControlEVSEResponse2);

        #endregion

        #endregion

        #region IEquatable<ControlEVSEResponse> Members

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

            // Check if the given object is a control EVSE response.
            var ControlEVSEResponse = Object as ControlEVSEResponse;
            if ((Object) ControlEVSEResponse == null)
                return false;

            return this.Equals(ControlEVSEResponse);

        }

        #endregion

        #region Equals(ControlEVSEResponse)

        /// <summary>
        /// Compares two control EVSE responses for equality.
        /// </summary>
        /// <param name="ControlEVSEResponse">A control EVSE response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(ControlEVSEResponse ControlEVSEResponse)
        {

            if ((Object) ControlEVSEResponse == null)
                return false;

            return this.Result.Equals(ControlEVSEResponse.Result) &&

                   (DirectId != null
                       ? DirectId.Equals(ControlEVSEResponse.DirectId)
                       : true) &&

                   (SessionTimeout.HasValue
                       ? SessionTimeout.Equals(ControlEVSEResponse.SessionTimeout)
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

                       (SessionTimeout.HasValue
                            ? SessionTimeout.GetHashCode() * 11
                            : 0) ^

                       Result.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Result,

                             DirectId != null
                                 ? " for " + DirectId
                                 : "",

                             SessionTimeout.HasValue
                                 ? " until " + SessionTimeout.Value.ToIso8601()
                                 : "");

        #endregion

    }

}
