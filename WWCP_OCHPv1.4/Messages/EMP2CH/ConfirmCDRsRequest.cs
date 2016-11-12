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

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHP confirm charge detail records request.
    /// </summary>
    public class ConfirmCDRsRequest : ARequest<ConfirmCDRsRequest>
    {

        #region Properties

        /// <summary>
        /// An enumeration of approved charge detail records.
        /// </summary>
        public IEnumerable<EVSECDRPair>  Approved   { get; }

        /// <summary>
        /// An enumeration of provider endpoints.
        /// </summary>
        public IEnumerable<EVSECDRPair>  Declined   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP ConfirmCDRs XML/SOAP request.
        /// </summary>
        /// <param name="Approved">An enumeration of approved charge detail records.</param>
        /// <param name="Declined">An enumeration of declined charge detail records.</param>
        public ConfirmCDRsRequest(IEnumerable<EVSECDRPair>  Approved = null,
                                  IEnumerable<EVSECDRPair>  Declined = null)
        {

            #region Initial checks

            if (Approved.IsNullOrEmpty() && Declined.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Approved) + " & " + nameof(Declined),  "At least one of the two enumerations of charge detail records must be neither null nor empty!");

            #endregion

            this.Approved  = Approved ?? new EVSECDRPair[0];
            this.Declined  = Declined ?? new EVSECDRPair[0];

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:OCHP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <OCHP:ConfirmCDRsRequest>
        //
        //         <!--Zero or more repetitioOCHP:-->
        //         <OCHP:approved>
        //            <OCHP:cdrId>?</OCHP:cdrId>
        //            <OCHP:evseId>?</OCHP:evseId>
        //         </OCHP:approved>
        //
        //         <!--Zero or more repetitioOCHP:-->
        //         <OCHP:declined>
        //            <OCHP:cdrId>?</OCHP:cdrId>
        //            <OCHP:evseId>?</OCHP:evseId>
        //         </OCHP:declined>
        //
        //      </OCHP:ConfirmCDRsRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(ConfirmCDRsRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP confirm charge detail records request.
        /// </summary>
        /// <param name="ConfirmCDRsRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ConfirmCDRsRequest Parse(XElement             ConfirmCDRsRequestXML,
                                               OnExceptionDelegate  OnException = null)
        {

            ConfirmCDRsRequest _ConfirmCDRsRequest;

            if (TryParse(ConfirmCDRsRequestXML, out _ConfirmCDRsRequest, OnException))
                return _ConfirmCDRsRequest;

            return null;

        }

        #endregion

        #region (static) Parse(ConfirmCDRsRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP confirm charge detail records request.
        /// </summary>
        /// <param name="ConfirmCDRsRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ConfirmCDRsRequest Parse(String               ConfirmCDRsRequestText,
                                               OnExceptionDelegate  OnException = null)
        {

            ConfirmCDRsRequest _ConfirmCDRsRequest;

            if (TryParse(ConfirmCDRsRequestText, out _ConfirmCDRsRequest, OnException))
                return _ConfirmCDRsRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(ConfirmCDRsRequestXML,  out ConfirmCDRsRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP confirm charge detail records request.
        /// </summary>
        /// <param name="ConfirmCDRsRequestXML">The XML to parse.</param>
        /// <param name="ConfirmCDRsRequest">The parsed confirm charge detail records request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                ConfirmCDRsRequestXML,
                                       out ConfirmCDRsRequest  ConfirmCDRsRequest,
                                       OnExceptionDelegate     OnException  = null)
        {

            try
            {

                ConfirmCDRsRequest = new ConfirmCDRsRequest(

                                         ConfirmCDRsRequestXML.MapElements(OCHPNS.Default + "approved",
                                                                           EVSECDRPair.Parse,
                                                                           OnException),

                                         ConfirmCDRsRequestXML.MapElements(OCHPNS.Default + "declined",
                                                                           EVSECDRPair.Parse,
                                                                           OnException)

                                     );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, ConfirmCDRsRequestXML, e);

                ConfirmCDRsRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ConfirmCDRsRequestText, out ConfirmCDRsRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP confirm charge detail records request.
        /// </summary>
        /// <param name="ConfirmCDRsRequestText">The text to parse.</param>
        /// <param name="ConfirmCDRsRequest">The parsed confirm charge detail records request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                  ConfirmCDRsRequestText,
                                       out ConfirmCDRsRequest  ConfirmCDRsRequest,
                                       OnExceptionDelegate     OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ConfirmCDRsRequestText).Root,
                             out ConfirmCDRsRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, ConfirmCDRsRequestText, e);
            }

            ConfirmCDRsRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "ConfirmCDRsRequest",

                                Approved.Select(cdr => cdr.ToXML(OCHPNS.Default + "approved")).
                                         ToArray(),

                                Declined.Select(cdr => cdr.ToXML(OCHPNS.Default + "declined")).
                                         ToArray()

                           );

        #endregion


        #region Operator overloading

        #region Operator == (ConfirmCDRsRequest1, ConfirmCDRsRequest2)

        /// <summary>
        /// Compares two confirm charge detail records requests for equality.
        /// </summary>
        /// <param name="ConfirmCDRsRequest1">A confirm charge detail records request.</param>
        /// <param name="ConfirmCDRsRequest2">Another confirm charge detail records request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ConfirmCDRsRequest ConfirmCDRsRequest1, ConfirmCDRsRequest ConfirmCDRsRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ConfirmCDRsRequest1, ConfirmCDRsRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ConfirmCDRsRequest1 == null) || ((Object) ConfirmCDRsRequest2 == null))
                return false;

            return ConfirmCDRsRequest1.Equals(ConfirmCDRsRequest2);

        }

        #endregion

        #region Operator != (ConfirmCDRsRequest1, ConfirmCDRsRequest2)

        /// <summary>
        /// Compares two confirm charge detail records requests for inequality.
        /// </summary>
        /// <param name="ConfirmCDRsRequest1">A confirm charge detail records request.</param>
        /// <param name="ConfirmCDRsRequest2">Another confirm charge detail records request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ConfirmCDRsRequest ConfirmCDRsRequest1, ConfirmCDRsRequest ConfirmCDRsRequest2)

            => !(ConfirmCDRsRequest1 == ConfirmCDRsRequest2);

        #endregion

        #endregion

        #region IEquatable<ConfirmCDRsRequest> Members

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

            // Check if the given object is a confirm charge detail records request.
            var ConfirmCDRsRequest = Object as ConfirmCDRsRequest;
            if ((Object) ConfirmCDRsRequest == null)
                return false;

            return this.Equals(ConfirmCDRsRequest);

        }

        #endregion

        #region Equals(ConfirmCDRsRequest)

        /// <summary>
        /// Compares two confirm charge detail records requests for equality.
        /// </summary>
        /// <param name="ConfirmCDRsRequest">A confirm charge detail records request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(ConfirmCDRsRequest ConfirmCDRsRequest)
        {

            if ((Object) ConfirmCDRsRequest == null)
                return false;

            return Approved.Count().Equals(ConfirmCDRsRequest.Approved.Count()) &&
                   Declined.Count().Equals(ConfirmCDRsRequest.Declined.Count());

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

                return Approved.GetHashCode() * 11 ^
                       Declined.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Approved.Any()
                                 ? Approved.Count() + " approved charge detail record(s)"
                                 : "",

                             Declined.Any()
                                 ? Declined.Count() + " declined charge detail record(s)"
                                 : "");

        #endregion


    }

}
