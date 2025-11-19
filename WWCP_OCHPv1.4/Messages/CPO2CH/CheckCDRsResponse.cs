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

namespace cloud.charging.open.protocols.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHP check charge details record response.
    /// </summary>
    public class CheckCDRsResponse : AResponse<CheckCDRsRequest,
                                               CheckCDRsResponse>
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
        /// <param name="Request">The check charge details record request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static CheckCDRsResponse OK(CheckCDRsRequest  Request,
                                           String            Description = null)

            => new CheckCDRsResponse(Request,
                                     Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The check charge details record request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static CheckCDRsResponse Partly(CheckCDRsRequest  Request,
                                               String            Description = null)

            => new CheckCDRsResponse(Request,
                                     Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The check charge details record request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static CheckCDRsResponse NotAuthorized(CheckCDRsRequest  Request,
                                                      String            Description = null)

            => new CheckCDRsResponse(Request,
                                     Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The check charge details record request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static CheckCDRsResponse InvalidId(CheckCDRsRequest  Request,
                                                  String            Description = null)

            => new CheckCDRsResponse(Request,
                                     Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The check charge details record request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static CheckCDRsResponse Server(CheckCDRsRequest  Request,
                                               String            Description = null)

            => new CheckCDRsResponse(Request,
                                     Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The check charge details record request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static CheckCDRsResponse Format(CheckCDRsRequest  Request,
                                               String            Description = null)

            => new CheckCDRsResponse(Request,
                                     Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP check charge details record response.
        /// </summary>
        /// <param name="Request">The check charge details record request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="ChargeDetailRecords">An enumeration of charge detail records.</param>
        public CheckCDRsResponse(CheckCDRsRequest      Request,
                                 Result                Result,
                                 IEnumerable<CDRInfo>  ChargeDetailRecords = null)

            : base(Request, Result)

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
        //      </ns:CheckCDRsResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, CheckCDRsResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP check charge details record response.
        /// </summary>
        /// <param name="Request">The check charge details record request leading to this response.</param>
        /// <param name="CheckCDRsResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static CheckCDRsResponse Parse(CheckCDRsRequest     Request,
                                              XElement             CheckCDRsResponseXML,
                                              OnExceptionDelegate  OnException = null)
        {

            CheckCDRsResponse _CheckCDRsResponse;

            if (TryParse(Request, CheckCDRsResponseXML, out _CheckCDRsResponse, OnException))
                return _CheckCDRsResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, CheckCDRsResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP check charge details record response.
        /// </summary>
        /// <param name="Request">The check charge details record request leading to this response.</param>
        /// <param name="CheckCDRsResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static CheckCDRsResponse Parse(CheckCDRsRequest     Request,
                                              String               CheckCDRsResponseText,
                                              OnExceptionDelegate  OnException = null)
        {

            CheckCDRsResponse _CheckCDRsResponse;

            if (TryParse(Request, CheckCDRsResponseText, out _CheckCDRsResponse, OnException))
                return _CheckCDRsResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, CheckCDRsResponseXML,  out CheckCDRsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP check charge details record response.
        /// </summary>
        /// <param name="Request">The check charge details record request leading to this response.</param>
        /// <param name="CheckCDRsResponseXML">The XML to parse.</param>
        /// <param name="CheckCDRsResponse">The parsed check charge details record response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static Boolean TryParse(CheckCDRsRequest       Request,
                                       XElement               CheckCDRsResponseXML,
                                       out CheckCDRsResponse  CheckCDRsResponse,
                                       OnExceptionDelegate    OnException  = null)
        {

            try
            {

                CheckCDRsResponse = new CheckCDRsResponse(

                                        Request,

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

                OnException?.Invoke(Timestamp.Now, CheckCDRsResponseXML, e);

                CheckCDRsResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, CheckCDRsResponseText, out CheckCDRsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP check charge details record response.
        /// </summary>
        /// <param name="Request">The check charge details record request leading to this response.</param>
        /// <param name="CheckCDRsResponseText">The text to parse.</param>
        /// <param name="CheckCDRsResponse">The parsed check charge details record response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static Boolean TryParse(CheckCDRsRequest       Request,
                                       String                 CheckCDRsResponseText,
                                       out CheckCDRsResponse  CheckCDRsResponse,
                                       OnExceptionDelegate    OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(CheckCDRsResponseText).Root,
                             out CheckCDRsResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, CheckCDRsResponseText, e);
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

                   Result.ToXML(),

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
            if (ReferenceEquals(CheckCDRsResponse1, CheckCDRsResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) CheckCDRsResponse1 is null) || ((Object) CheckCDRsResponse2 is null))
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

            if (Object is null)
                return false;

            // Check if the given object is a check charge details record response.
            var CheckCDRsResponse = Object as CheckCDRsResponse;
            if ((Object) CheckCDRsResponse is null)
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
        public override Boolean Equals(CheckCDRsResponse CheckCDRsResponse)
        {

            if ((Object) CheckCDRsResponse is null)
                return false;

            return Result.Equals(CheckCDRsResponse.Result);

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

                return ChargeDetailRecords is not null

                           ? Result.GetHashCode() * 11 ^
                             ChargeDetailRecords.SafeSelect(cdr => cdr.GetHashCode()).Aggregate((a, b) => a ^ b)

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
                             ChargeDetailRecords.Any()
                                 ? " " + ChargeDetailRecords.Count() + " charge detail records"
                                 : "");

        #endregion



        public class Builder : ABuilder
        {

            #region Properties

            /// <summary>
            /// An enumeration of charge detail records.
            /// </summary>
            public IEnumerable<CDRInfo> ChargeDetailRecords { get; set; }

            #endregion

            public Builder(CheckCDRsResponse CheckCDRsResponse = null)
            {

                if (CheckCDRsResponse is not null)
                {

                    this.ChargeDetailRecords = CheckCDRsResponse.ChargeDetailRecords;

                    if (CheckCDRsResponse.CustomData is not null)
                        foreach (var item in CheckCDRsResponse.CustomData)
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
