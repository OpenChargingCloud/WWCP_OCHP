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
using System.Threading;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHP update roaming authorisation list request.
    /// </summary>
    public class UpdateRoamingAuthorisationListRequest : ARequest<UpdateRoamingAuthorisationListRequest>
    {

        #region Properties

        /// <summary>
        /// An enumeration of roaming authorisation infos.
        /// </summary>
        public IEnumerable<RoamingAuthorisationInfo>  RoamingAuthorisationInfos   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP UpdateRoamingAuthorisationList XML/SOAP request.
        /// </summary>
        /// <param name="RoamingAuthorisationInfos">An enumeration of roaming authorisation infos.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public UpdateRoamingAuthorisationListRequest(IEnumerable<RoamingAuthorisationInfo>  RoamingAuthorisationInfos,

                                                     DateTimeOffset?                        Timestamp           = null,
                                                     EventTracking_Id?                      EventTrackingId     = null,
                                                     TimeSpan?                              RequestTimeout      = null,
                                                     CancellationToken                      CancellationToken   = default)

            : base(Timestamp,
                   EventTrackingId,
                   RequestTimeout,
                   CancellationToken)

        {

            this.RoamingAuthorisationInfos = RoamingAuthorisationInfos;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:OCHP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <OCHP:UpdateRoamingAuthorisationListRequest>
        //
        //          <!--1 or more repetitions:-->
        //          <OCHP:roamingAuthorisationInfoArray>
        //             ...
        //          </OCHP:roamingAuthorisationInfoArray>
        //
        //       </OCHP:UpdateRoamingAuthorisationListRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(UpdateRoamingAuthorisationListRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP update roaming authorisation list request.
        /// </summary>
        /// <param name="UpdateRoamingAuthorisationListRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static UpdateRoamingAuthorisationListRequest Parse(XElement             UpdateRoamingAuthorisationListRequestXML,
                                                                  OnExceptionDelegate  OnException = null)
        {

            UpdateRoamingAuthorisationListRequest _UpdateRoamingAuthorisationListRequest;

            if (TryParse(UpdateRoamingAuthorisationListRequestXML, out _UpdateRoamingAuthorisationListRequest, OnException))
                return _UpdateRoamingAuthorisationListRequest;

            return null;

        }

        #endregion

        #region (static) Parse(UpdateRoamingAuthorisationListRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP update roaming authorisation list request.
        /// </summary>
        /// <param name="UpdateRoamingAuthorisationListRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static UpdateRoamingAuthorisationListRequest Parse(String               UpdateRoamingAuthorisationListRequestText,
                                                                  OnExceptionDelegate  OnException = null)
        {

            UpdateRoamingAuthorisationListRequest _UpdateRoamingAuthorisationListRequest;

            if (TryParse(UpdateRoamingAuthorisationListRequestText, out _UpdateRoamingAuthorisationListRequest, OnException))
                return _UpdateRoamingAuthorisationListRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(UpdateRoamingAuthorisationListRequestXML,  out UpdateRoamingAuthorisationListRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP update roaming authorisation list request.
        /// </summary>
        /// <param name="UpdateRoamingAuthorisationListRequestXML">The XML to parse.</param>
        /// <param name="UpdateRoamingAuthorisationListRequest">The parsed update roaming authorisation list request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static Boolean TryParse(XElement                                   UpdateRoamingAuthorisationListRequestXML,
                                       out UpdateRoamingAuthorisationListRequest  UpdateRoamingAuthorisationListRequest,
                                       OnExceptionDelegate                        OnException  = null)
        {

            try
            {

                UpdateRoamingAuthorisationListRequest = new UpdateRoamingAuthorisationListRequest(

                                                            UpdateRoamingAuthorisationListRequestXML.MapElements(OCHPNS.Default + "roamingAuthorisationInfoArray",
                                                                                                                 RoamingAuthorisationInfo.Parse,
                                                                                                                 OnException)

                                                        );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, UpdateRoamingAuthorisationListRequestXML, e);

                UpdateRoamingAuthorisationListRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(UpdateRoamingAuthorisationListRequestText, out UpdateRoamingAuthorisationListRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP update roaming authorisation list request.
        /// </summary>
        /// <param name="UpdateRoamingAuthorisationListRequestText">The text to parse.</param>
        /// <param name="UpdateRoamingAuthorisationListRequest">The parsed update roaming authorisation list request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static Boolean TryParse(String                                     UpdateRoamingAuthorisationListRequestText,
                                       out UpdateRoamingAuthorisationListRequest  UpdateRoamingAuthorisationListRequest,
                                       OnExceptionDelegate                        OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(UpdateRoamingAuthorisationListRequestText).Root,
                             out UpdateRoamingAuthorisationListRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, UpdateRoamingAuthorisationListRequestText, e);
            }

            UpdateRoamingAuthorisationListRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "UpdateRoamingAuthorisationListRequest",

                                RoamingAuthorisationInfos.Select(infos => infos.ToXML(OCHPNS.Default + "roamingAuthorisationInfoArray")).
                                                          ToArray()

                           );

        #endregion


        #region Operator overloading

        #region Operator == (UpdateRoamingAuthorisationListRequest1, UpdateRoamingAuthorisationListRequest2)

        /// <summary>
        /// Compares two update roaming authorisation list requests for equality.
        /// </summary>
        /// <param name="UpdateRoamingAuthorisationListRequest1">A update roaming authorisation list request.</param>
        /// <param name="UpdateRoamingAuthorisationListRequest2">Another update roaming authorisation list request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (UpdateRoamingAuthorisationListRequest UpdateRoamingAuthorisationListRequest1, UpdateRoamingAuthorisationListRequest UpdateRoamingAuthorisationListRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(UpdateRoamingAuthorisationListRequest1, UpdateRoamingAuthorisationListRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) UpdateRoamingAuthorisationListRequest1 is null) || ((Object) UpdateRoamingAuthorisationListRequest2 is null))
                return false;

            return UpdateRoamingAuthorisationListRequest1.Equals(UpdateRoamingAuthorisationListRequest2);

        }

        #endregion

        #region Operator != (UpdateRoamingAuthorisationListRequest1, UpdateRoamingAuthorisationListRequest2)

        /// <summary>
        /// Compares two update roaming authorisation list requests for inequality.
        /// </summary>
        /// <param name="UpdateRoamingAuthorisationListRequest1">A update roaming authorisation list request.</param>
        /// <param name="UpdateRoamingAuthorisationListRequest2">Another update roaming authorisation list request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (UpdateRoamingAuthorisationListRequest UpdateRoamingAuthorisationListRequest1, UpdateRoamingAuthorisationListRequest UpdateRoamingAuthorisationListRequest2)

            => !(UpdateRoamingAuthorisationListRequest1 == UpdateRoamingAuthorisationListRequest2);

        #endregion

        #endregion

        #region IEquatable<UpdateRoamingAuthorisationListRequest> Members

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

            // Check if the given object is a update roaming authorisation list request.
            var UpdateRoamingAuthorisationListRequest = Object as UpdateRoamingAuthorisationListRequest;
            if ((Object) UpdateRoamingAuthorisationListRequest is null)
                return false;

            return this.Equals(UpdateRoamingAuthorisationListRequest);

        }

        #endregion

        #region Equals(UpdateRoamingAuthorisationListRequest)

        /// <summary>
        /// Compares two update roaming authorisation list requests for equality.
        /// </summary>
        /// <param name="UpdateRoamingAuthorisationListRequest">A update roaming authorisation list request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(UpdateRoamingAuthorisationListRequest UpdateRoamingAuthorisationListRequest)
        {

            if ((Object) UpdateRoamingAuthorisationListRequest is null)
                return false;

            return RoamingAuthorisationInfos.Count().Equals(UpdateRoamingAuthorisationListRequest.RoamingAuthorisationInfos.Count());

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

                return RoamingAuthorisationInfos.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => RoamingAuthorisationInfos.Count() + " roaming authorisation info(s)";

        #endregion


    }

}
