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

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// An OCHP check charge details record response.
    /// </summary>
    public class CheckCDRsResponse : AResponse
    {

        #region Properties

        /// <summary>
        /// An enumeration of charge detail records.
        /// </summary>
        public IEnumerable<CDRInfo>  ChargeDetailRecords   { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static CheckCDRsResponse OK(String Description = null)

            => new CheckCDRsResponse(Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static CheckCDRsResponse Partly(String Description = null)

            => new CheckCDRsResponse(Result.Unknown(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static CheckCDRsResponse NotAuthorized(String Description = null)

            => new CheckCDRsResponse(Result.Unknown(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static CheckCDRsResponse InvalidId(String Description = null)

            => new CheckCDRsResponse(Result.Unknown(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static CheckCDRsResponse Server(String Description = null)

            => new CheckCDRsResponse(Result.Unknown(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static CheckCDRsResponse Format(String Description = null)

            => new CheckCDRsResponse(Result.Unknown(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP check charge details record response.
        /// </summary>
        /// <param name="Result">A generic OHCP result.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        public CheckCDRsResponse(Result                Result,
                                 IEnumerable<CDRInfo>  ChargeDetailRecords = null)

            : base(Result)

        {

            this.ChargeDetailRecords  = ChargeDetailRecords;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:CheckCDRsResponse>
        //
        //         <ns:result>
        //            <ns:resultCode>
        //               <ns:resultCode>?</ns:resultCode>
        //            </ns:resultCode>
        //            <ns:resultDescription>?</ns:resultDescription>
        //         </ns:result>
        //
        //         <!--Zero or more repetitions:-->
        //         <ns:cdrInfoArray>?</ns:cdrInfoArray>
        //
        //      </ns:CheckCDRsResponse>        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(CheckCDRsResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP check charge details record response.
        /// </summary>
        /// <param name="CheckCDRsResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static CheckCDRsResponse Parse(XElement             CheckCDRsResponseXML,
                                              OnExceptionDelegate  OnException = null)
        {

            CheckCDRsResponse _CheckCDRsResponse;

            if (TryParse(CheckCDRsResponseXML, out _CheckCDRsResponse, OnException))
                return _CheckCDRsResponse;

            return null;

        }

        #endregion

        #region (static) Parse(CheckCDRsResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP check charge details record response.
        /// </summary>
        /// <param name="CheckCDRsResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static CheckCDRsResponse Parse(String               CheckCDRsResponseText,
                                              OnExceptionDelegate  OnException = null)
        {

            CheckCDRsResponse _CheckCDRsResponse;

            if (TryParse(CheckCDRsResponseText, out _CheckCDRsResponse, OnException))
                return _CheckCDRsResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(CheckCDRsResponseXML,  out CheckCDRsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP check charge details record response.
        /// </summary>
        /// <param name="CheckCDRsResponseXML">The XML to parse.</param>
        /// <param name="CheckCDRsResponse">The parsed check charge details record response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement               CheckCDRsResponseXML,
                                       out CheckCDRsResponse  CheckCDRsResponse,
                                       OnExceptionDelegate    OnException  = null)
        {

            try
            {

                CheckCDRsResponse = new CheckCDRsResponse(

                                        CheckCDRsResponseXML.MapElementOrFail (OCHPNS.Default + "result",
                                                                             Result.Parse,
                                                                             OnException),

                                        CheckCDRsResponseXML.MapElementsOrFail(OCHPNS.Default + "cdrInfoArray",
                                                                             CDRInfo.Parse,
                                                                             OnException)

                                    );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, CheckCDRsResponseXML, e);

                CheckCDRsResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(CheckCDRsResponseText, out CheckCDRsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP check charge details record response.
        /// </summary>
        /// <param name="CheckCDRsResponseText">The text to parse.</param>
        /// <param name="CheckCDRsResponse">The parsed check charge details record response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                 CheckCDRsResponseText,
                                       out CheckCDRsResponse  CheckCDRsResponse,
                                       OnExceptionDelegate    OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(CheckCDRsResponseText).Root,
                             out CheckCDRsResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, CheckCDRsResponseText, e);
            }

            CheckCDRsResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "CheckCDRsResponse",

                   new XElement(OCHPNS.Default + "result", Result.ToXML()),

                   ChargeDetailRecords.Select(cdr => cdr.ToXML(OCHPNS.Default + "cdrInfoArray"))

               );

        #endregion


        #region Operator overloading

        #region Operator == (CheckCDRsResponse1, CheckCDRsResponse2)

        /// <summary>
        /// Compares two check charge details record responses for equality.
        /// </summary>
        /// <param name="CheckCDRsResponse1">A check charge details record response.</param>
        /// <param name="CheckCDRsResponse2">Another check charge details record response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (CheckCDRsResponse CheckCDRsResponse1, CheckCDRsResponse CheckCDRsResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(CheckCDRsResponse1, CheckCDRsResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) CheckCDRsResponse1 == null) || ((Object) CheckCDRsResponse2 == null))
                return false;

            return CheckCDRsResponse1.Equals(CheckCDRsResponse2);

        }

        #endregion

        #region Operator != (CheckCDRsResponse1, CheckCDRsResponse2)

        /// <summary>
        /// Compares two check charge details record responses for inequality.
        /// </summary>
        /// <param name="CheckCDRsResponse1">A check charge details record response.</param>
        /// <param name="CheckCDRsResponse2">Another check charge details record response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (CheckCDRsResponse CheckCDRsResponse1, CheckCDRsResponse CheckCDRsResponse2)

            => !(CheckCDRsResponse1 == CheckCDRsResponse2);

        #endregion

        #endregion

        #region IEquatable<CheckCDRsResponse> Members

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

            // Check if the given object is a check charge details record response.
            var CheckCDRsResponse = Object as CheckCDRsResponse;
            if ((Object) CheckCDRsResponse == null)
                return false;

            return this.Equals(CheckCDRsResponse);

        }

        #endregion

        #region Equals(CheckCDRsResponse)

        /// <summary>
        /// Compares two check charge details record responses for equality.
        /// </summary>
        /// <param name="CheckCDRsResponse">A check charge details record response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(CheckCDRsResponse CheckCDRsResponse)
        {

            if ((Object) CheckCDRsResponse == null)
                return false;

            return this.Result.Equals(CheckCDRsResponse.Result);

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

                return ChargeDetailRecords != null

                           ? Result.GetHashCode() * 11 ^
                             ChargeDetailRecords.SafeSelect(cdr => cdr.GetHashCode()).Aggregate((a, b) => a ^ b)

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
                             ChargeDetailRecords.Any()
                                 ? " " + ChargeDetailRecords.Count() + " charge detail records"
                                 : "");

        #endregion

    }

}
