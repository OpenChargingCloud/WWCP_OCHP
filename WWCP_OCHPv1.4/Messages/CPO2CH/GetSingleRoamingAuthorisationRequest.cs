/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHP get single roaming authorisation request.
    /// </summary>
    public class GetSingleRoamingAuthorisationRequest : ARequest<GetSingleRoamingAuthorisationRequest>
    {

        #region Properties

    /// <summary>
    /// An e-mobility token.
    /// </summary>
    public EMT_Id  EMTId   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP GetSingleRoamingAuthorisation XML/SOAP request.
        /// </summary>
        /// <param name="EMTId">An e-mobility token.</param>
        public GetSingleRoamingAuthorisationRequest(EMT_Id  EMTId)
        {

            #region Initial checks

            if (EMTId == null)
                throw new ArgumentNullException(nameof(EMTId),  "The given e-mobility token must not be null!");

            #endregion

            this.EMTId  = EMTId;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:OCHP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:GetSingleRoamingAuthorisationRequest>
        //
        //         <ns:emtId representation="plain">
        //
        //            <ns:instance>?</ns:instance>
        //            <ns:tokenType>?</ns:tokenType>
        //
        //            <!--Optional:-->
        //            <ns:tokenSubType>?</ns:tokenSubType>
        //
        //         </ns:emtId>
        //
        //      </ns:GetSingleRoamingAuthorisationRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(GetSingleRoamingAuthorisationRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get single roaming authorisation request.
        /// </summary>
        /// <param name="GetSingleRoamingAuthorisationRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetSingleRoamingAuthorisationRequest Parse(XElement             GetSingleRoamingAuthorisationRequestXML,
                                                                 OnExceptionDelegate  OnException = null)
        {

            GetSingleRoamingAuthorisationRequest _GetSingleRoamingAuthorisationRequest;

            if (TryParse(GetSingleRoamingAuthorisationRequestXML, out _GetSingleRoamingAuthorisationRequest, OnException))
                return _GetSingleRoamingAuthorisationRequest;

            return null;

        }

        #endregion

        #region (static) Parse(GetSingleRoamingAuthorisationRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get single roaming authorisation request.
        /// </summary>
        /// <param name="GetSingleRoamingAuthorisationRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetSingleRoamingAuthorisationRequest Parse(String               GetSingleRoamingAuthorisationRequestText,
                                                                 OnExceptionDelegate  OnException = null)
        {

            GetSingleRoamingAuthorisationRequest _GetSingleRoamingAuthorisationRequest;

            if (TryParse(GetSingleRoamingAuthorisationRequestText, out _GetSingleRoamingAuthorisationRequest, OnException))
                return _GetSingleRoamingAuthorisationRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(GetSingleRoamingAuthorisationRequestXML,  out GetSingleRoamingAuthorisationRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get single roaming authorisation request.
        /// </summary>
        /// <param name="GetSingleRoamingAuthorisationRequestXML">The XML to parse.</param>
        /// <param name="GetSingleRoamingAuthorisationRequest">The parsed get single roaming authorisation request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                  GetSingleRoamingAuthorisationRequestXML,
                                       out GetSingleRoamingAuthorisationRequest  GetSingleRoamingAuthorisationRequest,
                                       OnExceptionDelegate                       OnException  = null)
        {

            try
            {

                GetSingleRoamingAuthorisationRequest = new GetSingleRoamingAuthorisationRequest(

                                                           GetSingleRoamingAuthorisationRequestXML.MapElementOrFail(OCHPNS.Default + "emtId",
                                                                                                                    EMT_Id.Parse,
                                                                                                                    OnException)

                                                       );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, GetSingleRoamingAuthorisationRequestXML, e);

                GetSingleRoamingAuthorisationRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetSingleRoamingAuthorisationRequestText, out GetSingleRoamingAuthorisationRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get single roaming authorisation request.
        /// </summary>
        /// <param name="GetSingleRoamingAuthorisationRequestText">The text to parse.</param>
        /// <param name="GetSingleRoamingAuthorisationRequest">The parsed get single roaming authorisation request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                    GetSingleRoamingAuthorisationRequestText,
                                       out GetSingleRoamingAuthorisationRequest  GetSingleRoamingAuthorisationRequest,
                                       OnExceptionDelegate                       OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetSingleRoamingAuthorisationRequestText).Root,
                             out GetSingleRoamingAuthorisationRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, GetSingleRoamingAuthorisationRequestText, e);
            }

            GetSingleRoamingAuthorisationRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetSingleRoamingAuthorisationRequest",
                                EMTId.ToXML()
                           );

        #endregion


        #region Operator overloading

        #region Operator == (GetSingleRoamingAuthorisationRequest1, GetSingleRoamingAuthorisationRequest2)

        /// <summary>
        /// Compares two get single roaming authorisation requests for equality.
        /// </summary>
        /// <param name="GetSingleRoamingAuthorisationRequest1">A get single roaming authorisation request.</param>
        /// <param name="GetSingleRoamingAuthorisationRequest2">Another get single roaming authorisation request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetSingleRoamingAuthorisationRequest GetSingleRoamingAuthorisationRequest1, GetSingleRoamingAuthorisationRequest GetSingleRoamingAuthorisationRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetSingleRoamingAuthorisationRequest1, GetSingleRoamingAuthorisationRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetSingleRoamingAuthorisationRequest1 == null) || ((Object) GetSingleRoamingAuthorisationRequest2 == null))
                return false;

            return GetSingleRoamingAuthorisationRequest1.Equals(GetSingleRoamingAuthorisationRequest2);

        }

        #endregion

        #region Operator != (GetSingleRoamingAuthorisationRequest1, GetSingleRoamingAuthorisationRequest2)

        /// <summary>
        /// Compares two get single roaming authorisation requests for inequality.
        /// </summary>
        /// <param name="GetSingleRoamingAuthorisationRequest1">A get single roaming authorisation request.</param>
        /// <param name="GetSingleRoamingAuthorisationRequest2">Another get single roaming authorisation request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetSingleRoamingAuthorisationRequest GetSingleRoamingAuthorisationRequest1, GetSingleRoamingAuthorisationRequest GetSingleRoamingAuthorisationRequest2)

            => !(GetSingleRoamingAuthorisationRequest1 == GetSingleRoamingAuthorisationRequest2);

        #endregion

        #endregion

        #region IEquatable<GetSingleRoamingAuthorisationRequest> Members

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

            // Check if the given object is a get single roaming authorisation request.
            var GetSingleRoamingAuthorisationRequest = Object as GetSingleRoamingAuthorisationRequest;
            if ((Object) GetSingleRoamingAuthorisationRequest == null)
                return false;

            return this.Equals(GetSingleRoamingAuthorisationRequest);

        }

        #endregion

        #region Equals(GetSingleRoamingAuthorisationRequest)

        /// <summary>
        /// Compares two get single roaming authorisation requests for equality.
        /// </summary>
        /// <param name="GetSingleRoamingAuthorisationRequest">A get single roaming authorisation request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetSingleRoamingAuthorisationRequest GetSingleRoamingAuthorisationRequest)
        {

            if ((Object) GetSingleRoamingAuthorisationRequest == null)
                return false;

            return EMTId.Equals(GetSingleRoamingAuthorisationRequest.EMTId);

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

                return EMTId.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => EMTId.ToString();

        #endregion


    }

}
