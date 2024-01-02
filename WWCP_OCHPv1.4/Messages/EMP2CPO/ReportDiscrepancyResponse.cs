/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
    /// An OCHPdirect report discrepancy response.
    /// </summary>
    public class ReportDiscrepancyResponse : AResponse<ReportDiscrepancyRequest,
                                                       ReportDiscrepancyResponse>
    {

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The report discrepancy request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ReportDiscrepancyResponse OK(ReportDiscrepancyRequest  Request,
                                                   String                    Description = null)

            => new ReportDiscrepancyResponse(Request,
                                             Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The report discrepancy request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ReportDiscrepancyResponse Partly(ReportDiscrepancyRequest  Request,
                                                       String                    Description = null)

            => new ReportDiscrepancyResponse(Request,
                                             Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The report discrepancy request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ReportDiscrepancyResponse NotAuthorized(ReportDiscrepancyRequest  Request,
                                                              String                    Description = null)

            => new ReportDiscrepancyResponse(Request,
                                             Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The report discrepancy request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ReportDiscrepancyResponse InvalidId(ReportDiscrepancyRequest  Request,
                                                          String                    Description = null)

            => new ReportDiscrepancyResponse(Request,
                                             Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The report discrepancy request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ReportDiscrepancyResponse Server(ReportDiscrepancyRequest  Request,
                                                       String                    Description = null)

            => new ReportDiscrepancyResponse(Request,
                                             Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The report discrepancy request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static ReportDiscrepancyResponse Format(ReportDiscrepancyRequest  Request,
                                                       String                    Description = null)

            => new ReportDiscrepancyResponse(Request,
                                             Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHPdirect report discrepancy response.
        /// </summary>
        /// <param name="Request">The report discrepancy request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        public ReportDiscrepancyResponse(ReportDiscrepancyRequest  Request,
                                         Result                    Result)

            : base(Request, Result)

        { }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:ReportDiscrepancyResponse>
        //
        //          <ns:result>
        //             <ns:resultCode>
        //                <ns:resultCode>?</ns:resultCode>
        //             </ns:resultCode>
        //             <ns:resultDescription>?</ns:resultDescription>
        //          </ns:result>
        //
        //       </ns:SelectEvseResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, ReportDiscrepancyResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHPdirect report discrepancy response.
        /// </summary>
        /// <param name="Request">The report discrepancy request leading to this response.</param>
        /// <param name="ReportDiscrepancyResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ReportDiscrepancyResponse Parse(ReportDiscrepancyRequest  Request,
                                                      XElement                  ReportDiscrepancyResponseXML,
                                                      OnExceptionDelegate       OnException = null)
        {

            ReportDiscrepancyResponse _ReportDiscrepancyResponse;

            if (TryParse(Request, ReportDiscrepancyResponseXML, out _ReportDiscrepancyResponse, OnException))
                return _ReportDiscrepancyResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, ReportDiscrepancyResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHPdirect report discrepancy response.
        /// </summary>
        /// <param name="Request">The report discrepancy request leading to this response.</param>
        /// <param name="ReportDiscrepancyResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ReportDiscrepancyResponse Parse(ReportDiscrepancyRequest  Request,
                                                      String                    ReportDiscrepancyResponseText,
                                                      OnExceptionDelegate       OnException = null)
        {

            ReportDiscrepancyResponse _ReportDiscrepancyResponse;

            if (TryParse(Request, ReportDiscrepancyResponseText, out _ReportDiscrepancyResponse, OnException))
                return _ReportDiscrepancyResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, ReportDiscrepancyResponseXML,  out ReportDiscrepancyResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHPdirect report discrepancy response.
        /// </summary>
        /// <param name="Request">The report discrepancy request leading to this response.</param>
        /// <param name="ReportDiscrepancyResponseXML">The XML to parse.</param>
        /// <param name="ReportDiscrepancyResponse">The parsed report discrepancy response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(ReportDiscrepancyRequest       Request,
                                       XElement                       ReportDiscrepancyResponseXML,
                                       out ReportDiscrepancyResponse  ReportDiscrepancyResponse,
                                       OnExceptionDelegate            OnException  = null)
        {

            try
            {

                ReportDiscrepancyResponse = new ReportDiscrepancyResponse(

                                                Request,

                                                ReportDiscrepancyResponseXML.MapElementOrFail  (OCHPNS.Default + "result",
                                                                                                Result.Parse,
                                                                                                OnException)

                                            );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, ReportDiscrepancyResponseXML, e);

                ReportDiscrepancyResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, ReportDiscrepancyResponseText, out ReportDiscrepancyResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHPdirect report discrepancy response.
        /// </summary>
        /// <param name="Request">The report discrepancy request leading to this response.</param>
        /// <param name="ReportDiscrepancyResponseText">The text to parse.</param>
        /// <param name="ReportDiscrepancyResponse">The parsed report discrepancy response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(ReportDiscrepancyRequest       Request,
                                       String                         ReportDiscrepancyResponseText,
                                       out ReportDiscrepancyResponse  ReportDiscrepancyResponse,
                                       OnExceptionDelegate            OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(ReportDiscrepancyResponseText).Root,
                             out ReportDiscrepancyResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, ReportDiscrepancyResponseText, e);
            }

            ReportDiscrepancyResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "ReportDiscrepancyResponse",

                   new XElement(OCHPNS.Default + "result", Result.ToXML())

               );

        #endregion


        #region Operator overloading

        #region Operator == (ReportDiscrepancyResponse1, ReportDiscrepancyResponse2)

        /// <summary>
        /// Compares two report discrepancy responses for equality.
        /// </summary>
        /// <param name="ReportDiscrepancyResponse1">A report discrepancy response.</param>
        /// <param name="ReportDiscrepancyResponse2">Another report discrepancy response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ReportDiscrepancyResponse ReportDiscrepancyResponse1, ReportDiscrepancyResponse ReportDiscrepancyResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ReportDiscrepancyResponse1, ReportDiscrepancyResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ReportDiscrepancyResponse1 == null) || ((Object) ReportDiscrepancyResponse2 == null))
                return false;

            return ReportDiscrepancyResponse1.Equals(ReportDiscrepancyResponse2);

        }

        #endregion

        #region Operator != (ReportDiscrepancyResponse1, ReportDiscrepancyResponse2)

        /// <summary>
        /// Compares two report discrepancy responses for inequality.
        /// </summary>
        /// <param name="ReportDiscrepancyResponse1">A report discrepancy response.</param>
        /// <param name="ReportDiscrepancyResponse2">Another report discrepancy response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ReportDiscrepancyResponse ReportDiscrepancyResponse1, ReportDiscrepancyResponse ReportDiscrepancyResponse2)

            => !(ReportDiscrepancyResponse1 == ReportDiscrepancyResponse2);

        #endregion

        #endregion

        #region IEquatable<ReportDiscrepancyResponse> Members

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

            // Check if the given object is a report discrepancy response.
            var ReportDiscrepancyResponse = Object as ReportDiscrepancyResponse;
            if ((Object) ReportDiscrepancyResponse == null)
                return false;

            return this.Equals(ReportDiscrepancyResponse);

        }

        #endregion

        #region Equals(ReportDiscrepancyResponse)

        /// <summary>
        /// Compares two report discrepancy responses for equality.
        /// </summary>
        /// <param name="ReportDiscrepancyResponse">A report discrepancy response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(ReportDiscrepancyResponse ReportDiscrepancyResponse)
        {

            if ((Object) ReportDiscrepancyResponse == null)
                return false;

            return this.Result.Equals(ReportDiscrepancyResponse.Result);

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Result.ToString();

        #endregion

    }

}
