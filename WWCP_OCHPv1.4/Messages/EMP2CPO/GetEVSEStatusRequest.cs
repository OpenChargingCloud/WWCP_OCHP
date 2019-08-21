/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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
    /// An OCHPdirect get EVSE status request.
    /// </summary>
    public class GetEVSEStatusRequest : ARequest<GetEVSEStatusRequest>
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE identifications.
        /// </summary>
        public IEnumerable<EVSE_Id>  EVSEIds   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHPdirect get EVSEStatus XML/SOAP request.
        /// </summary>
        /// <param name="EVSEIds">An enumeration of EVSE identifications.</param>
        public GetEVSEStatusRequest(IEnumerable<EVSE_Id>  EVSEIds)
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

        #region (static) Parse(GetEVSEStatusRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHPdirect direct EVSE status request.
        /// </summary>
        /// <param name="GetEVSEStatusRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetEVSEStatusRequest Parse(XElement             GetEVSEStatusRequestXML,
                                                 OnExceptionDelegate  OnException = null)
        {

            GetEVSEStatusRequest _GetEVSEStatusRequest;

            if (TryParse(GetEVSEStatusRequestXML, out _GetEVSEStatusRequest, OnException))
                return _GetEVSEStatusRequest;

            return null;

        }

        #endregion

        #region (static) Parse(GetEVSEStatusRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHPdirect direct EVSE status request.
        /// </summary>
        /// <param name="GetEVSEStatusRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetEVSEStatusRequest Parse(String               GetEVSEStatusRequestText,
                                                 OnExceptionDelegate  OnException = null)
        {

            GetEVSEStatusRequest _GetEVSEStatusRequest;

            if (TryParse(GetEVSEStatusRequestText, out _GetEVSEStatusRequest, OnException))
                return _GetEVSEStatusRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(GetEVSEStatusRequestXML,  out GetEVSEStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHPdirect direct EVSE status request.
        /// </summary>
        /// <param name="GetEVSEStatusRequestXML">The XML to parse.</param>
        /// <param name="GetEVSEStatusRequest">The parsed direct EVSE status request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                  GetEVSEStatusRequestXML,
                                       out GetEVSEStatusRequest  GetEVSEStatusRequest,
                                       OnExceptionDelegate       OnException  = null)
        {

            try
            {

                GetEVSEStatusRequest = new GetEVSEStatusRequest(

                                           GetEVSEStatusRequestXML.MapValuesOrFail(OCHPNS.Default + "requestedEvseId",
                                                                                      EVSE_Id.Parse)

                                       );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, GetEVSEStatusRequestXML, e);

                GetEVSEStatusRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetEVSEStatusRequestText, out GetEVSEStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHPdirect direct EVSE status request.
        /// </summary>
        /// <param name="GetEVSEStatusRequestText">The text to parse.</param>
        /// <param name="GetEVSEStatusRequest">The parsed direct EVSE status request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                    GetEVSEStatusRequestText,
                                       out GetEVSEStatusRequest  GetEVSEStatusRequest,
                                       OnExceptionDelegate       OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetEVSEStatusRequestText).Root,
                             out GetEVSEStatusRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, GetEVSEStatusRequestText, e);
            }

            GetEVSEStatusRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "DirectEvseStatusRequest",

                                      EVSEIds.Select(evseid => new XElement(OCHPNS.Default + "requestedEvseId",  evseid.ToString()))

                                 ));

        #endregion


        #region Operator overloading

        #region Operator == (GetEVSEStatusRequest1, GetEVSEStatusRequest2)

        /// <summary>
        /// Compares two direct EVSE status requests for equality.
        /// </summary>
        /// <param name="GetEVSEStatusRequest1">A direct EVSE status request.</param>
        /// <param name="GetEVSEStatusRequest2">Another direct EVSE status request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetEVSEStatusRequest GetEVSEStatusRequest1, GetEVSEStatusRequest GetEVSEStatusRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetEVSEStatusRequest1, GetEVSEStatusRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetEVSEStatusRequest1 == null) || ((Object) GetEVSEStatusRequest2 == null))
                return false;

            return GetEVSEStatusRequest1.Equals(GetEVSEStatusRequest2);

        }

        #endregion

        #region Operator != (GetEVSEStatusRequest1, GetEVSEStatusRequest2)

        /// <summary>
        /// Compares two direct EVSE status requests for inequality.
        /// </summary>
        /// <param name="GetEVSEStatusRequest1">A direct EVSE status request.</param>
        /// <param name="GetEVSEStatusRequest2">Another direct EVSE status request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetEVSEStatusRequest GetEVSEStatusRequest1, GetEVSEStatusRequest GetEVSEStatusRequest2)

            => !(GetEVSEStatusRequest1 == GetEVSEStatusRequest2);

        #endregion

        #endregion

        #region IEquatable<GetEVSEStatusRequest> Members

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
            var GetEVSEStatusRequest = Object as GetEVSEStatusRequest;
            if ((Object) GetEVSEStatusRequest == null)
                return false;

            return this.Equals(GetEVSEStatusRequest);

        }

        #endregion

        #region Equals(GetEVSEStatusRequest)

        /// <summary>
        /// Compares two direct EVSE status requests for equality.
        /// </summary>
        /// <param name="GetEVSEStatusRequest">A direct EVSE status request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetEVSEStatusRequest GetEVSEStatusRequest)
        {

            if ((Object) GetEVSEStatusRequest == null)
                return false;

            return EVSEIds.Count().Equals(GetEVSEStatusRequest.EVSEIds.Count()) &&

                   EVSEIds.Any(evseid => GetEVSEStatusRequest.EVSEIds.Contains(evseid));

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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => EVSEIds.Count() + " EVSE identifications";

        #endregion

    }

}
