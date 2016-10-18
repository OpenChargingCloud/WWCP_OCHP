﻿/*
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

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// An OCHP select EVSE response.
    /// </summary>
    public class SelectEVSEResponse : AResponse
    {

        #region Properties

        /// <summary>
        /// The session identification for a direct charging process.
        /// </summary>
        public Direct_Id  DirectId        { get; }

        /// <summary>
        /// An optional timestamp until when the given EVSE is reserved.
        /// </summary>
        public DateTime?  ReservedUntil   { get; } 

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static SelectEVSEResponse OK(String Description = null)

            => new SelectEVSEResponse(Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static SelectEVSEResponse Partly(String Description = null)

            => new SelectEVSEResponse(Result.Unknown(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static SelectEVSEResponse NotAuthorized(String Description = null)

            => new SelectEVSEResponse(Result.Unknown(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static SelectEVSEResponse InvalidId(String Description = null)

            => new SelectEVSEResponse(Result.Unknown(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static SelectEVSEResponse Server(String Description = null)

            => new SelectEVSEResponse(Result.Unknown(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static SelectEVSEResponse Format(String Description = null)

            => new SelectEVSEResponse(Result.Unknown(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP select EVSE response.
        /// </summary>
        /// <param name="Result">A generic OHCP result.</param>
        /// <param name="DirectId">The session identification for a direct charging process.</param>
        /// <param name="ReservedUntil">An optional timestamp until when the given EVSE is reserved.</param>
        public SelectEVSEResponse(Result     Result,
                                  Direct_Id  DirectId       = null,
                                  DateTime?  ReservedUntil  = null)

            : base(Result)

        {

            #region Initial checks

            if (ReservedUntil.HasValue && ReservedUntil.Value <= DateTime.Now)
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

        #region (static) Parse(SelectEVSEResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP select EVSE response.
        /// </summary>
        /// <param name="SelectEVSEResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SelectEVSEResponse Parse(XElement             SelectEVSEResponseXML,
                                               OnExceptionDelegate  OnException = null)
        {

            SelectEVSEResponse _SelectEVSEResponse;

            if (TryParse(SelectEVSEResponseXML, out _SelectEVSEResponse, OnException))
                return _SelectEVSEResponse;

            return null;

        }

        #endregion

        #region (static) Parse(SelectEVSEResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP select EVSE response.
        /// </summary>
        /// <param name="SelectEVSEResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SelectEVSEResponse Parse(String               SelectEVSEResponseText,
                                               OnExceptionDelegate  OnException = null)
        {

            SelectEVSEResponse _SelectEVSEResponse;

            if (TryParse(SelectEVSEResponseText, out _SelectEVSEResponse, OnException))
                return _SelectEVSEResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(SelectEVSEResponseXML,  out SelectEVSEResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP select EVSE response.
        /// </summary>
        /// <param name="SelectEVSEResponseXML">The XML to parse.</param>
        /// <param name="SelectEVSEResponse">The parsed select EVSE response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                SelectEVSEResponseXML,
                                       out SelectEVSEResponse  SelectEVSEResponse,
                                       OnExceptionDelegate     OnException  = null)
        {

            try
            {

                SelectEVSEResponse = new SelectEVSEResponse(

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

                OnException?.Invoke(DateTime.Now, SelectEVSEResponseXML, e);

                SelectEVSEResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SelectEVSEResponseText, out SelectEVSEResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP select EVSE response.
        /// </summary>
        /// <param name="SelectEVSEResponseText">The text to parse.</param>
        /// <param name="SelectEVSEResponse">The parsed select EVSE response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                  SelectEVSEResponseText,
                                       out SelectEVSEResponse  SelectEVSEResponse,
                                       OnExceptionDelegate     OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SelectEVSEResponseText).Root,
                             out SelectEVSEResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, SelectEVSEResponseText, e);
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

                   new XElement(OCHPNS.Default + "result", Result.ToXML()),

                   DirectId != null
                       ? new XElement(OCHPNS.Default + "directId",  DirectId.ToString())
                       : null,

                   ReservedUntil.HasValue
                       ? new XElement(OCHPNS.Default + "ttl",
                             new XElement(OCHPNS.Default + "DateTime", ReservedUntil.Value.ToIso8601())
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
            if (Object.ReferenceEquals(SelectEVSEResponse1, SelectEVSEResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SelectEVSEResponse1 == null) || ((Object) SelectEVSEResponse2 == null))
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

            if (Object == null)
                return false;

            // Check if the given object is a select EVSE response.
            var SelectEVSEResponse = Object as SelectEVSEResponse;
            if ((Object) SelectEVSEResponse == null)
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
        public Boolean Equals(SelectEVSEResponse SelectEVSEResponse)
        {

            if ((Object) SelectEVSEResponse == null)
                return false;

            return this.Result.Equals(SelectEVSEResponse.Result) &&

                   (DirectId != null
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

                return (DirectId != null
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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Result,
                             DirectId != null
                                 ? " for " + DirectId
                                 : "",
                             ReservedUntil.HasValue
                                 ? " until " + ReservedUntil.Value.ToIso8601()
                                 : "");

        #endregion

    }

}