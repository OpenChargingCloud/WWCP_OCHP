/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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
    /// An OCHPdirect get inform provider request.
    /// </summary>
    public class GetInformProviderRequest : ARequest<GetInformProviderRequest>
    {

        #region Properties

        /// <summary>
        /// The session identification referencing a direct charging process.
        /// </summary>
        public Direct_Id  DirectId   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHPdirect GetInformProvider XML/SOAP request.
        /// </summary>
        /// <param name="DirectId">The direct charging process session identification.</param>
        public GetInformProviderRequest(Direct_Id  DirectId)
        {

            #region Initial checks

            if (DirectId == null)
                throw new ArgumentNullException(nameof(DirectId),  "The given identification of an direct charging process must not be null!");

            #endregion

            this.DirectId  = DirectId;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:InformProviderRequest>
        //
        //         <ns:directId>?</ns:directId>
        //
        //      </ns:InformProviderRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(GetInformProviderRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHPdirect get inform provider request.
        /// </summary>
        /// <param name="GetInformProviderRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetInformProviderRequest Parse(XElement             GetInformProviderRequestXML,
                                                     OnExceptionDelegate  OnException = null)
        {

            GetInformProviderRequest _GetInformProviderRequest;

            if (TryParse(GetInformProviderRequestXML, out _GetInformProviderRequest, OnException))
                return _GetInformProviderRequest;

            return null;

        }

        #endregion

        #region (static) Parse(GetInformProviderRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHPdirect get inform provider request.
        /// </summary>
        /// <param name="GetInformProviderRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetInformProviderRequest Parse(String               GetInformProviderRequestText,
                                                     OnExceptionDelegate  OnException = null)
        {

            GetInformProviderRequest _GetInformProviderRequest;

            if (TryParse(GetInformProviderRequestText, out _GetInformProviderRequest, OnException))
                return _GetInformProviderRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(GetInformProviderRequestXML,  out GetInformProviderRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHPdirect get inform provider request.
        /// </summary>
        /// <param name="GetInformProviderRequestXML">The XML to parse.</param>
        /// <param name="GetInformProviderRequest">The parsed get inform provider request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                      GetInformProviderRequestXML,
                                       out GetInformProviderRequest  GetInformProviderRequest,
                                       OnExceptionDelegate           OnException  = null)
        {

            try
            {

                GetInformProviderRequest = new GetInformProviderRequest(

                                               GetInformProviderRequestXML.MapValueOrFail(OCHPNS.Default + "directId",
                                                                                          Direct_Id.Parse)

                                           );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, GetInformProviderRequestXML, e);

                GetInformProviderRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetInformProviderRequestText, out GetInformProviderRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHPdirect get inform provider request.
        /// </summary>
        /// <param name="GetInformProviderRequestText">The text to parse.</param>
        /// <param name="GetInformProviderRequest">The parsed get inform provider request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                        GetInformProviderRequestText,
                                       out GetInformProviderRequest  GetInformProviderRequest,
                                       OnExceptionDelegate           OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetInformProviderRequestText).Root,
                             out GetInformProviderRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, GetInformProviderRequestText, e);
            }

            GetInformProviderRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "InformProviderRequest",

                                      new XElement(OCHPNS.Default + "directId",  DirectId.ToString())

                                 ));

        #endregion


        #region Operator overloading

        #region Operator == (GetInformProviderRequest1, GetInformProviderRequest2)

        /// <summary>
        /// Compares two get inform provider requests for equality.
        /// </summary>
        /// <param name="GetInformProviderRequest1">A get inform provider request.</param>
        /// <param name="GetInformProviderRequest2">Another get inform provider request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetInformProviderRequest GetInformProviderRequest1, GetInformProviderRequest GetInformProviderRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetInformProviderRequest1, GetInformProviderRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetInformProviderRequest1 == null) || ((Object) GetInformProviderRequest2 == null))
                return false;

            return GetInformProviderRequest1.Equals(GetInformProviderRequest2);

        }

        #endregion

        #region Operator != (GetInformProviderRequest1, GetInformProviderRequest2)

        /// <summary>
        /// Compares two get inform provider requests for inequality.
        /// </summary>
        /// <param name="GetInformProviderRequest1">A get inform provider request.</param>
        /// <param name="GetInformProviderRequest2">Another get inform provider request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetInformProviderRequest GetInformProviderRequest1, GetInformProviderRequest GetInformProviderRequest2)

            => !(GetInformProviderRequest1 == GetInformProviderRequest2);

        #endregion

        #endregion

        #region IEquatable<GetInformProviderRequest> Members

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

            // Check if the given object is a get inform provider request.
            var GetInformProviderRequest = Object as GetInformProviderRequest;
            if ((Object) GetInformProviderRequest == null)
                return false;

            return this.Equals(GetInformProviderRequest);

        }

        #endregion

        #region Equals(GetInformProviderRequest)

        /// <summary>
        /// Compares two get inform provider requests for equality.
        /// </summary>
        /// <param name="GetInformProviderRequest">A get inform provider request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetInformProviderRequest GetInformProviderRequest)
        {

            if ((Object) GetInformProviderRequest == null)
                return false;

            return DirectId.Equals(GetInformProviderRequest.DirectId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => DirectId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => DirectId.ToString();

        #endregion

    }

}
