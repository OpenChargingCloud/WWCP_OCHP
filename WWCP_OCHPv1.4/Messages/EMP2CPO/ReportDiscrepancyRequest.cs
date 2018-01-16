/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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
    /// An OCHPdirect report discrepancy request sent whenever a provider wants
    /// to report an issue concerning the data, compatibility or status of an
    /// EVSE to the charge point operator.
    /// </summary>
    public class ReportDiscrepancyRequest : ARequest<ReportDiscrepancyRequest>
    {

        #region Properties

        /// <summary>
        /// The unique EVSE identification which is affected by the report.
        /// </summary>
        public EVSE_Id  EVSEId   { get; }

        /// <summary>
        /// The unique EVSE identification which is affected by the report.
        /// </summary>
        public String   Report   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHPdirect ReportDiscrepancy XML/SOAP request.
        /// </summary>
        /// <param name="EVSEId">The EVSE identification affected by this report.</param>
        /// <param name="Report">Textual or generated report of the discrepancy.</param>
        public ReportDiscrepancyRequest(EVSE_Id  EVSEId,
                                        String   Report)
        {

            #region Initial checks

            if (Report == null || Report.Trim().IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Report),  "The given EVSE report must not be null or empty!");

            #endregion

            this.EVSEId  = EVSEId;
            this.Report  = Report.Trim();

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:ReportDiscrepancyRequest>
        //
        //         <ns:evseId>?</ns:evseId>
        //         <ns:report>?</ns:report>
        //
        //      </ns:ReportDiscrepancyRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(ReportDiscrepancyRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHPdirect report discrepancy request.
        /// </summary>
        /// <param name="ReportDiscrepancyRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ReportDiscrepancyRequest Parse(XElement             ReportDiscrepancyRequestXML,
                                                     OnExceptionDelegate  OnException = null)
        {

            ReportDiscrepancyRequest _ReportDiscrepancyRequest;

            if (TryParse(ReportDiscrepancyRequestXML, out _ReportDiscrepancyRequest, OnException))
                return _ReportDiscrepancyRequest;

            return null;

        }

        #endregion

        #region (static) Parse(ReportDiscrepancyRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHPdirect report discrepancy request.
        /// </summary>
        /// <param name="ReportDiscrepancyRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ReportDiscrepancyRequest Parse(String               ReportDiscrepancyRequestText,
                                                     OnExceptionDelegate  OnException = null)
        {

            ReportDiscrepancyRequest _ReportDiscrepancyRequest;

            if (TryParse(ReportDiscrepancyRequestText, out _ReportDiscrepancyRequest, OnException))
                return _ReportDiscrepancyRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(ReportDiscrepancyRequestXML,  out ReportDiscrepancyRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHPdirect report discrepancy request.
        /// </summary>
        /// <param name="ReportDiscrepancyRequestXML">The XML to parse.</param>
        /// <param name="ReportDiscrepancyRequest">The parsed report discrepancy request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                      ReportDiscrepancyRequestXML,
                                       out ReportDiscrepancyRequest  ReportDiscrepancyRequest,
                                       OnExceptionDelegate           OnException  = null)
        {

            try
            {

                ReportDiscrepancyRequest = new ReportDiscrepancyRequest(

                                               ReportDiscrepancyRequestXML.MapValueOrFail    (OCHPNS.Default + "evseId",
                                                                                              EVSE_Id.Parse),

                                               ReportDiscrepancyRequestXML.ElementValueOrFail(OCHPNS.Default + "report")

                                           );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, ReportDiscrepancyRequestXML, e);

                ReportDiscrepancyRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ReportDiscrepancyRequestText, out ReportDiscrepancyRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHPdirect report discrepancy request.
        /// </summary>
        /// <param name="ReportDiscrepancyRequestText">The text to parse.</param>
        /// <param name="ReportDiscrepancyRequest">The parsed report discrepancy request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                        ReportDiscrepancyRequestText,
                                       out ReportDiscrepancyRequest  ReportDiscrepancyRequest,
                                       OnExceptionDelegate           OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ReportDiscrepancyRequestText).Root,
                             out ReportDiscrepancyRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, ReportDiscrepancyRequestText, e);
            }

            ReportDiscrepancyRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "ReportDiscrepancyRequest",

                                      new XElement(OCHPNS.Default + "evseId",  EVSEId.ToString()),
                                      new XElement(OCHPNS.Default + "report",  Report)

                                 ));

        #endregion


        #region Operator overloading

        #region Operator == (ReportDiscrepancyRequest1, ReportDiscrepancyRequest2)

        /// <summary>
        /// Compares two report discrepancy requests for equality.
        /// </summary>
        /// <param name="ReportDiscrepancyRequest1">A report discrepancy request.</param>
        /// <param name="ReportDiscrepancyRequest2">Another report discrepancy request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ReportDiscrepancyRequest ReportDiscrepancyRequest1, ReportDiscrepancyRequest ReportDiscrepancyRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ReportDiscrepancyRequest1, ReportDiscrepancyRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ReportDiscrepancyRequest1 == null) || ((Object) ReportDiscrepancyRequest2 == null))
                return false;

            return ReportDiscrepancyRequest1.Equals(ReportDiscrepancyRequest2);

        }

        #endregion

        #region Operator != (ReportDiscrepancyRequest1, ReportDiscrepancyRequest2)

        /// <summary>
        /// Compares two report discrepancy requests for inequality.
        /// </summary>
        /// <param name="ReportDiscrepancyRequest1">A report discrepancy request.</param>
        /// <param name="ReportDiscrepancyRequest2">Another report discrepancy request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ReportDiscrepancyRequest ReportDiscrepancyRequest1, ReportDiscrepancyRequest ReportDiscrepancyRequest2)

            => !(ReportDiscrepancyRequest1 == ReportDiscrepancyRequest2);

        #endregion

        #endregion

        #region IEquatable<ReportDiscrepancyRequest> Members

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

            // Check if the given object is a report discrepancy request.
            var ReportDiscrepancyRequest = Object as ReportDiscrepancyRequest;
            if ((Object) ReportDiscrepancyRequest == null)
                return false;

            return this.Equals(ReportDiscrepancyRequest);

        }

        #endregion

        #region Equals(ReportDiscrepancyRequest)

        /// <summary>
        /// Compares two report discrepancy requests for equality.
        /// </summary>
        /// <param name="ReportDiscrepancyRequest">A report discrepancy request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(ReportDiscrepancyRequest ReportDiscrepancyRequest)
        {

            if ((Object) ReportDiscrepancyRequest == null)
                return false;

            return EVSEId.Equals(ReportDiscrepancyRequest.EVSEId) &&
                   Report.Equals(ReportDiscrepancyRequest.Report);

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

                return EVSEId.GetHashCode() * 11 ^
                       Report.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEId.ToString(), " => ", Report.SubstringMax(30));

        #endregion

    }

}
