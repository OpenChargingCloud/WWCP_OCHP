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
    /// An OCHP direct EVSE status request.
    /// </summary>
    public class DirectEVSEStatusRequest
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE identifications.
        /// </summary>
        public IEnumerable<EVSE_Id>  EVSEIds   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP DirectEVSEStatus XML/SOAP request.
        /// </summary>
        /// <param name="EVSEIds">An enumeration of EVSE identifications.</param>
        public DirectEVSEStatusRequest(IEnumerable<EVSE_Id>  EVSEIds)
        {

            #region Initial checks

            if (EVSEIds == null)
                throw new ArgumentNullException(nameof(EVSEIds),  "The given enumeration of EVSE identifications must not be null!");

            #endregion

            this.EVSEIds  = EVSEIds;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:DirectEvseStatusRequest>
        //
        //         <!--1 or more repetitions:-->
        //         <ns:requestedEvseId>?</ns:requestedEvseId>
        //
        //      </ns:DirectEvseStatusRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(DirectEVSEStatusRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP direct EVSE status request.
        /// </summary>
        /// <param name="DirectEVSEStatusRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static DirectEVSEStatusRequest Parse(XElement             DirectEVSEStatusRequestXML,
                                                    OnExceptionDelegate  OnException = null)
        {

            DirectEVSEStatusRequest _DirectEVSEStatusRequest;

            if (TryParse(DirectEVSEStatusRequestXML, out _DirectEVSEStatusRequest, OnException))
                return _DirectEVSEStatusRequest;

            return null;

        }

        #endregion

        #region (static) Parse(DirectEVSEStatusRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP direct EVSE status request.
        /// </summary>
        /// <param name="DirectEVSEStatusRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static DirectEVSEStatusRequest Parse(String               DirectEVSEStatusRequestText,
                                                    OnExceptionDelegate  OnException = null)
        {

            DirectEVSEStatusRequest _DirectEVSEStatusRequest;

            if (TryParse(DirectEVSEStatusRequestText, out _DirectEVSEStatusRequest, OnException))
                return _DirectEVSEStatusRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(DirectEVSEStatusRequestXML,  out DirectEVSEStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP direct EVSE status request.
        /// </summary>
        /// <param name="DirectEVSEStatusRequestXML">The XML to parse.</param>
        /// <param name="DirectEVSEStatusRequest">The parsed direct EVSE status request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                     DirectEVSEStatusRequestXML,
                                       out DirectEVSEStatusRequest  DirectEVSEStatusRequest,
                                       OnExceptionDelegate          OnException  = null)
        {

            try
            {

                DirectEVSEStatusRequest = new DirectEVSEStatusRequest(

                                              DirectEVSEStatusRequestXML.MapValuesOrFail(OCHPNS.Default + "requestedEvseId",
                                                                                         EVSE_Id.Parse)

                                          );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, DirectEVSEStatusRequestXML, e);

                DirectEVSEStatusRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(DirectEVSEStatusRequestText, out DirectEVSEStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP direct EVSE status request.
        /// </summary>
        /// <param name="DirectEVSEStatusRequestText">The text to parse.</param>
        /// <param name="DirectEVSEStatusRequest">The parsed direct EVSE status request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                       DirectEVSEStatusRequestText,
                                       out DirectEVSEStatusRequest  DirectEVSEStatusRequest,
                                       OnExceptionDelegate          OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(DirectEVSEStatusRequestText).Root,
                             out DirectEVSEStatusRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, DirectEVSEStatusRequestText, e);
            }

            DirectEVSEStatusRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "ReleaseEvseRequest",

                                      EVSEIds.Select(evseid => new XElement(OCHPNS.Default + "requestedEvseId",  evseid.ToString()))

                                 ));

        #endregion


        #region Operator overloading

        #region Operator == (DirectEVSEStatusRequest1, DirectEVSEStatusRequest2)

        /// <summary>
        /// Compares two direct EVSE status requests for equality.
        /// </summary>
        /// <param name="DirectEVSEStatusRequest1">A direct EVSE status request.</param>
        /// <param name="DirectEVSEStatusRequest2">Another direct EVSE status request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (DirectEVSEStatusRequest DirectEVSEStatusRequest1, DirectEVSEStatusRequest DirectEVSEStatusRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(DirectEVSEStatusRequest1, DirectEVSEStatusRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) DirectEVSEStatusRequest1 == null) || ((Object) DirectEVSEStatusRequest2 == null))
                return false;

            return DirectEVSEStatusRequest1.Equals(DirectEVSEStatusRequest2);

        }

        #endregion

        #region Operator != (DirectEVSEStatusRequest1, DirectEVSEStatusRequest2)

        /// <summary>
        /// Compares two direct EVSE status requests for inequality.
        /// </summary>
        /// <param name="DirectEVSEStatusRequest1">A direct EVSE status request.</param>
        /// <param name="DirectEVSEStatusRequest2">Another direct EVSE status request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (DirectEVSEStatusRequest DirectEVSEStatusRequest1, DirectEVSEStatusRequest DirectEVSEStatusRequest2)

            => !(DirectEVSEStatusRequest1 == DirectEVSEStatusRequest2);

        #endregion

        #endregion

        #region IEquatable<DirectEVSEStatusRequest> Members

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

            // Check if the given object is a direct EVSE status request.
            var DirectEVSEStatusRequest = Object as DirectEVSEStatusRequest;
            if ((Object) DirectEVSEStatusRequest == null)
                return false;

            return this.Equals(DirectEVSEStatusRequest);

        }

        #endregion

        #region Equals(DirectEVSEStatusRequest)

        /// <summary>
        /// Compares two direct EVSE status requests for equality.
        /// </summary>
        /// <param name="DirectEVSEStatusRequest">A direct EVSE status request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(DirectEVSEStatusRequest DirectEVSEStatusRequest)
        {

            if ((Object) DirectEVSEStatusRequest == null)
                return false;

            return EVSEIds.Count().Equals(DirectEVSEStatusRequest.EVSEIds.Count()) &&

                   EVSEIds.Any(evseid => DirectEVSEStatusRequest.EVSEIds.Contains(evseid));

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => EVSEIds.
                   Select(evseid => evseid.GetHashCode()).
                   Aggregate((a, b) => a ^ b);

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => EVSEIds.Count() + " EVSE identifications";

        #endregion

    }

}
