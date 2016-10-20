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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHPdirect release EVSE request.
    /// </summary>
    public class ReleaseEVSERequest
    {

        #region Properties

        /// <summary>
        /// The session id referencing the direct charging process to be released.
        /// </summary>
        public Direct_Id  DirectId   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHPdirect ReleaseEVSE XML/SOAP request.
        /// </summary>
        /// <param name="DirectId">The session identification of the direct charging process to be released.</param>
        public ReleaseEVSERequest(Direct_Id  DirectId)
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
        //      <ns:ReleaseEvseRequest>
        //
        //         <ns:directId>?</ns:directId>
        //
        //      </ns:ReleaseEvseRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(ReleaseEVSERequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHPdirect release EVSE request.
        /// </summary>
        /// <param name="ReleaseEVSERequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ReleaseEVSERequest Parse(XElement             ReleaseEVSERequestXML,
                                               OnExceptionDelegate  OnException = null)
        {

            ReleaseEVSERequest _ReleaseEVSERequest;

            if (TryParse(ReleaseEVSERequestXML, out _ReleaseEVSERequest, OnException))
                return _ReleaseEVSERequest;

            return null;

        }

        #endregion

        #region (static) Parse(ReleaseEVSERequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHPdirect release EVSE request.
        /// </summary>
        /// <param name="ReleaseEVSERequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ReleaseEVSERequest Parse(String               ReleaseEVSERequestText,
                                               OnExceptionDelegate  OnException = null)
        {

            ReleaseEVSERequest _ReleaseEVSERequest;

            if (TryParse(ReleaseEVSERequestText, out _ReleaseEVSERequest, OnException))
                return _ReleaseEVSERequest;

            return null;

        }

        #endregion

        #region (static) TryParse(ReleaseEVSERequestXML,  out ReleaseEVSERequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHPdirect release EVSE request.
        /// </summary>
        /// <param name="ReleaseEVSERequestXML">The XML to parse.</param>
        /// <param name="ReleaseEVSERequest">The parsed release EVSE request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                ReleaseEVSERequestXML,
                                       out ReleaseEVSERequest  ReleaseEVSERequest,
                                       OnExceptionDelegate     OnException  = null)
        {

            try
            {

                ReleaseEVSERequest = new ReleaseEVSERequest(

                                        ReleaseEVSERequestXML.MapValueOrFail(OCHPNS.Default + "directId",
                                                                             Direct_Id.Parse)

                                    );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, ReleaseEVSERequestXML, e);

                ReleaseEVSERequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ReleaseEVSERequestText, out ReleaseEVSERequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHPdirect release EVSE request.
        /// </summary>
        /// <param name="ReleaseEVSERequestText">The text to parse.</param>
        /// <param name="ReleaseEVSERequest">The parsed release EVSE request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                  ReleaseEVSERequestText,
                                       out ReleaseEVSERequest  ReleaseEVSERequest,
                                       OnExceptionDelegate     OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ReleaseEVSERequestText).Root,
                             out ReleaseEVSERequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, ReleaseEVSERequestText, e);
            }

            ReleaseEVSERequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "ReleaseEvseRequest",

                                      new XElement(OCHPNS.Default + "directId",  DirectId.ToString())

                                 ));

        #endregion


        #region Operator overloading

        #region Operator == (ReleaseEVSERequest1, ReleaseEVSERequest2)

        /// <summary>
        /// Compares two release EVSE requests for equality.
        /// </summary>
        /// <param name="ReleaseEVSERequest1">A release EVSE request.</param>
        /// <param name="ReleaseEVSERequest2">Another release EVSE request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ReleaseEVSERequest ReleaseEVSERequest1, ReleaseEVSERequest ReleaseEVSERequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ReleaseEVSERequest1, ReleaseEVSERequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ReleaseEVSERequest1 == null) || ((Object) ReleaseEVSERequest2 == null))
                return false;

            return ReleaseEVSERequest1.Equals(ReleaseEVSERequest2);

        }

        #endregion

        #region Operator != (ReleaseEVSERequest1, ReleaseEVSERequest2)

        /// <summary>
        /// Compares two release EVSE requests for inequality.
        /// </summary>
        /// <param name="ReleaseEVSERequest1">A release EVSE request.</param>
        /// <param name="ReleaseEVSERequest2">Another release EVSE request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ReleaseEVSERequest ReleaseEVSERequest1, ReleaseEVSERequest ReleaseEVSERequest2)

            => !(ReleaseEVSERequest1 == ReleaseEVSERequest2);

        #endregion

        #endregion

        #region IEquatable<ReleaseEVSERequest> Members

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

            // Check if the given object is a release EVSE request.
            var ReleaseEVSERequest = Object as ReleaseEVSERequest;
            if ((Object) ReleaseEVSERequest == null)
                return false;

            return this.Equals(ReleaseEVSERequest);

        }

        #endregion

        #region Equals(ReleaseEVSERequest)

        /// <summary>
        /// Compares two release EVSE requests for equality.
        /// </summary>
        /// <param name="ReleaseEVSERequest">A release EVSE request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ReleaseEVSERequest ReleaseEVSERequest)
        {

            if ((Object) ReleaseEVSERequest == null)
                return false;

            return DirectId.Equals(ReleaseEVSERequest.DirectId);

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
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(DirectId);

        #endregion

    }

}
