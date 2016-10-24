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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHP confirm charge details record response.
    /// </summary>
    public class ConfirmCDRsResponse : AResponse
    {

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static ConfirmCDRsResponse OK(String Description = null)

            => new ConfirmCDRsResponse(Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static ConfirmCDRsResponse Partly(String Description = null)

            => new ConfirmCDRsResponse(Result.Unknown(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static ConfirmCDRsResponse NotAuthorized(String Description = null)

            => new ConfirmCDRsResponse(Result.Unknown(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static ConfirmCDRsResponse InvalidId(String Description = null)

            => new ConfirmCDRsResponse(Result.Unknown(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static ConfirmCDRsResponse Server(String Description = null)

            => new ConfirmCDRsResponse(Result.Unknown(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static ConfirmCDRsResponse Format(String Description = null)

            => new ConfirmCDRsResponse(Result.Unknown(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP confirm charge details record response.
        /// </summary>
        /// <param name="Result">A generic OCHP result.</param>
        public ConfirmCDRsResponse(Result  Result)
            : base(Result)
        { }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:ConfirmCDRsResponse>
        //
        //         <ns:result>
        //            <ns:resultCode>
        //               <ns:resultCode>?</ns:resultCode>
        //            </ns:resultCode>
        //            <ns:resultDescription>?</ns:resultDescription>
        //         </ns:result>
        //
        //      </ns:ConfirmCDRsResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(ConfirmCDRsResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP confirm charge details record response.
        /// </summary>
        /// <param name="ConfirmCDRsResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ConfirmCDRsResponse Parse(XElement             ConfirmCDRsResponseXML,
                                                OnExceptionDelegate  OnException = null)
        {

            ConfirmCDRsResponse _ConfirmCDRsResponse;

            if (TryParse(ConfirmCDRsResponseXML, out _ConfirmCDRsResponse, OnException))
                return _ConfirmCDRsResponse;

            return null;

        }

        #endregion

        #region (static) Parse(ConfirmCDRsResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP confirm charge details record response.
        /// </summary>
        /// <param name="ConfirmCDRsResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ConfirmCDRsResponse Parse(String               ConfirmCDRsResponseText,
                                                OnExceptionDelegate  OnException = null)
        {

            ConfirmCDRsResponse _ConfirmCDRsResponse;

            if (TryParse(ConfirmCDRsResponseText, out _ConfirmCDRsResponse, OnException))
                return _ConfirmCDRsResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(ConfirmCDRsResponseXML,  out ConfirmCDRsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP confirm charge details record response.
        /// </summary>
        /// <param name="ConfirmCDRsResponseXML">The XML to parse.</param>
        /// <param name="ConfirmCDRsResponse">The parsed confirm charge details record response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                 ConfirmCDRsResponseXML,
                                       out ConfirmCDRsResponse  ConfirmCDRsResponse,
                                       OnExceptionDelegate      OnException  = null)
        {

            try
            {

                ConfirmCDRsResponse = new ConfirmCDRsResponse(

                                          ConfirmCDRsResponseXML.MapElementOrFail(OCHPNS.Default + "result",
                                                                                  Result.Parse,
                                                                                  OnException)

                                      );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, ConfirmCDRsResponseXML, e);

                ConfirmCDRsResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ConfirmCDRsResponseText, out ConfirmCDRsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP confirm charge details record response.
        /// </summary>
        /// <param name="ConfirmCDRsResponseText">The text to parse.</param>
        /// <param name="ConfirmCDRsResponse">The parsed confirm charge details record response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                   ConfirmCDRsResponseText,
                                       out ConfirmCDRsResponse  ConfirmCDRsResponse,
                                       OnExceptionDelegate      OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ConfirmCDRsResponseText).Root,
                             out ConfirmCDRsResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, ConfirmCDRsResponseText, e);
            }

            ConfirmCDRsResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "ConfirmCDRsResponse",
                   new XElement(OCHPNS.Default + "result", Result.ToXML())
               );

        #endregion


        #region Operator overloading

        #region Operator == (ConfirmCDRsResponse1, ConfirmCDRsResponse2)

        /// <summary>
        /// Compares two confirm charge details record responses for equality.
        /// </summary>
        /// <param name="ConfirmCDRsResponse1">A confirm charge details record response.</param>
        /// <param name="ConfirmCDRsResponse2">Another confirm charge details record response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ConfirmCDRsResponse ConfirmCDRsResponse1, ConfirmCDRsResponse ConfirmCDRsResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ConfirmCDRsResponse1, ConfirmCDRsResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ConfirmCDRsResponse1 == null) || ((Object) ConfirmCDRsResponse2 == null))
                return false;

            return ConfirmCDRsResponse1.Equals(ConfirmCDRsResponse2);

        }

        #endregion

        #region Operator != (ConfirmCDRsResponse1, ConfirmCDRsResponse2)

        /// <summary>
        /// Compares two confirm charge details record responses for inequality.
        /// </summary>
        /// <param name="ConfirmCDRsResponse1">A confirm charge details record response.</param>
        /// <param name="ConfirmCDRsResponse2">Another confirm charge details record response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ConfirmCDRsResponse ConfirmCDRsResponse1, ConfirmCDRsResponse ConfirmCDRsResponse2)

            => !(ConfirmCDRsResponse1 == ConfirmCDRsResponse2);

        #endregion

        #endregion

        #region IEquatable<ConfirmCDRsResponse> Members

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

            // Check if the given object is a confirm charge details record response.
            var ConfirmCDRsResponse = Object as ConfirmCDRsResponse;
            if ((Object) ConfirmCDRsResponse == null)
                return false;

            return this.Equals(ConfirmCDRsResponse);

        }

        #endregion

        #region Equals(ConfirmCDRsResponse)

        /// <summary>
        /// Compares two confirm charge details record responses for equality.
        /// </summary>
        /// <param name="ConfirmCDRsResponse">A confirm charge details record response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ConfirmCDRsResponse ConfirmCDRsResponse)
        {

            if ((Object) ConfirmCDRsResponse == null)
                return false;

            return this.Result.Equals(ConfirmCDRsResponse.Result);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => Result.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => Result.ToString();

        #endregion

    }

}
