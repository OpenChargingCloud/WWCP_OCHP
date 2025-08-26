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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHPdirect select EVSE response.
    /// </summary>
    public class SelectEVSEResponse : AResponse<SelectEVSERequest,
                                                SelectEVSEResponse>
    {

        #region Properties

        /// <summary>
        /// The session identification for a direct charging process.
        /// </summary>
        public Direct_Id        DirectId        { get; }

        /// <summary>
        /// An optional timestamp until when the given EVSE is reserved.
        /// </summary>
        public DateTimeOffset?  ReservedUntil   { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The select EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static SelectEVSEResponse OK(SelectEVSERequest  Request,
                                            String             Description = null)

            => new SelectEVSEResponse(Request,
                                      Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The select EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static SelectEVSEResponse Partly(SelectEVSERequest  Request,
                                                String             Description = null)

            => new SelectEVSEResponse(Request,
                                      Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The select EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static SelectEVSEResponse NotAuthorized(SelectEVSERequest  Request,
                                                       String             Description = null)

            => new SelectEVSEResponse(Request,
                                      Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The select EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static SelectEVSEResponse InvalidId(SelectEVSERequest  Request,
                                                   String             Description = null)

            => new SelectEVSEResponse(Request,
                                      Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The select EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static SelectEVSEResponse Server(SelectEVSERequest  Request,
                                                String             Description = null)

            => new SelectEVSEResponse(Request,
                                      Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The select EVSE request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static SelectEVSEResponse Format(SelectEVSERequest  Request,
                                                String             Description = null)

            => new SelectEVSEResponse(Request,
                                      Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHPdirect select EVSE response.
        /// </summary>
        /// <param name="Request">The select EVSE request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="DirectId">The session identification for a direct charging process.</param>
        /// <param name="ReservedUntil">An optional timestamp until when the given EVSE is reserved.</param>
        public SelectEVSEResponse(SelectEVSERequest  Request,
                                  Result             Result,
                                  Direct_Id          DirectId       = null,
                                  DateTimeOffset?    ReservedUntil  = null)

            : base(Request, Result)

        {

            #region Initial checks

            if (ReservedUntil.HasValue && ReservedUntil.Value <= Timestamp.Now)
                throw new ArgumentException("The given reservation end time must be after than the current time!");

            #endregion

            this.DirectId       = DirectId;
            this.ReservedUntil  = ReservedUntil ?? new DateTime?();

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:SelectEvseResponse>
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
        //       </ns:SelectEvseResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, SelectEVSEResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHPdirect select EVSE response.
        /// </summary>
        /// <param name="Request">The select EVSE request leading to this response.</param>
        /// <param name="SelectEVSEResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SelectEVSEResponse Parse(SelectEVSERequest    Request,
                                               XElement             SelectEVSEResponseXML,
                                               OnExceptionDelegate  OnException = null)
        {

            SelectEVSEResponse _SelectEVSEResponse;

            if (TryParse(Request, SelectEVSEResponseXML, out _SelectEVSEResponse, OnException))
                return _SelectEVSEResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, SelectEVSEResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHPdirect select EVSE response.
        /// </summary>
        /// <param name="Request">The select EVSE request leading to this response.</param>
        /// <param name="SelectEVSEResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SelectEVSEResponse Parse(SelectEVSERequest    Request,
                                               String               SelectEVSEResponseText,
                                               OnExceptionDelegate  OnException = null)
        {

            SelectEVSEResponse _SelectEVSEResponse;

            if (TryParse(Request, SelectEVSEResponseText, out _SelectEVSEResponse, OnException))
                return _SelectEVSEResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, SelectEVSEResponseXML,  out SelectEVSEResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHPdirect select EVSE response.
        /// </summary>
        /// <param name="Request">The select EVSE request leading to this response.</param>
        /// <param name="SelectEVSEResponseXML">The XML to parse.</param>
        /// <param name="SelectEVSEResponse">The parsed select EVSE response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SelectEVSERequest       Request,
                                       XElement                SelectEVSEResponseXML,
                                       out SelectEVSEResponse  SelectEVSEResponse,
                                       OnExceptionDelegate     OnException  = null)
        {

            try
            {

                SelectEVSEResponse = new SelectEVSEResponse(

                                         Request,

                                         SelectEVSEResponseXML.MapElementOrFail  (OCHPNS.Default + "result",
                                                                                  Result.Parse,
                                                                                  OnException),

                                         SelectEVSEResponseXML.MapValueOrNull    (OCHPNS.Default + "directId",
                                                                                  Direct_Id.Parse),

                                         SelectEVSEResponseXML.MapValueOrNullable(OCHPNS.Default + "ttl",
                                                                                  OCHPNS.Default + "DateTime",
                                                                                  DateTime.Parse)

                                     );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, SelectEVSEResponseXML, e);

                SelectEVSEResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, SelectEVSEResponseText, out SelectEVSEResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHPdirect select EVSE response.
        /// </summary>
        /// <param name="Request">The select EVSE request leading to this response.</param>
        /// <param name="SelectEVSEResponseText">The text to parse.</param>
        /// <param name="SelectEVSEResponse">The parsed select EVSE response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(SelectEVSERequest       Request,
                                       String                  SelectEVSEResponseText,
                                       out SelectEVSEResponse  SelectEVSEResponse,
                                       OnExceptionDelegate     OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(SelectEVSEResponseText).Root,
                             out SelectEVSEResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, SelectEVSEResponseText, e);
            }

            SelectEVSEResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "SelectEVSEResponse",

                   Result.ToXML(),

                   DirectId is not null
                       ? new XElement(OCHPNS.Default + "directId",  DirectId.ToString())
                       : null,

                   ReservedUntil.HasValue
                       ? new XElement(OCHPNS.Default + "ttl",
                             new XElement(OCHPNS.Default + "DateTime", ReservedUntil.Value.ToISO8601())
                         )
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (SelectEVSEResponse1, SelectEVSEResponse2)

        /// <summary>
        /// Compares two select EVSE responses for equality.
        /// </summary>
        /// <param name="SelectEVSEResponse1">A select EVSE response.</param>
        /// <param name="SelectEVSEResponse2">Another select EVSE response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SelectEVSEResponse SelectEVSEResponse1, SelectEVSEResponse SelectEVSEResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SelectEVSEResponse1, SelectEVSEResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SelectEVSEResponse1 is null) || ((Object) SelectEVSEResponse2 is null))
                return false;

            return SelectEVSEResponse1.Equals(SelectEVSEResponse2);

        }

        #endregion

        #region Operator != (SelectEVSEResponse1, SelectEVSEResponse2)

        /// <summary>
        /// Compares two select EVSE responses for inequality.
        /// </summary>
        /// <param name="SelectEVSEResponse1">A select EVSE response.</param>
        /// <param name="SelectEVSEResponse2">Another select EVSE response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SelectEVSEResponse SelectEVSEResponse1, SelectEVSEResponse SelectEVSEResponse2)

            => !(SelectEVSEResponse1 == SelectEVSEResponse2);

        #endregion

        #endregion

        #region IEquatable<SelectEVSEResponse> Members

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

            // Check if the given object is a select EVSE response.
            var SelectEVSEResponse = Object as SelectEVSEResponse;
            if ((Object) SelectEVSEResponse is null)
                return false;

            return this.Equals(SelectEVSEResponse);

        }

        #endregion

        #region Equals(SelectEVSEResponse)

        /// <summary>
        /// Compares two select EVSE responses for equality.
        /// </summary>
        /// <param name="SelectEVSEResponse">A select EVSE response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SelectEVSEResponse SelectEVSEResponse)
        {

            if ((Object) SelectEVSEResponse is null)
                return false;

            return Result.Equals(SelectEVSEResponse.Result) &&

                   (DirectId is not null
                       ? DirectId.Equals(SelectEVSEResponse.DirectId)
                       : true) &&

                   (ReservedUntil.HasValue
                       ? ReservedUntil.Equals(SelectEVSEResponse.ReservedUntil)
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

                return (DirectId is not null
                            ? DirectId.GetHashCode() * 17
                            : 0) ^

                       (ReservedUntil.HasValue
                            ? ReservedUntil.GetHashCode() * 11
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

                             DirectId is not null
                                 ? " for " + DirectId
                                 : "",

                             ReservedUntil.HasValue
                                 ? " until " + ReservedUntil.Value.ToISO8601()
                                 : "");

        #endregion

    }

}
