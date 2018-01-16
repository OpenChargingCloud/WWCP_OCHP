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
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHP set roaming authorisation list request.
    /// </summary>
    public class SetRoamingAuthorisationListRequest : ARequest<SetRoamingAuthorisationListRequest>
    {

        #region Properties

        /// <summary>
        /// An enumeration of roaming authorisation infos.
        /// </summary>
        public IEnumerable<RoamingAuthorisationInfo>  RoamingAuthorisationInfos   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP SetRoamingAuthorisationList XML/SOAP request.
        /// </summary>
        /// <param name="RoamingAuthorisationInfos">An enumeration of roaming authorisation infos.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public SetRoamingAuthorisationListRequest(IEnumerable<RoamingAuthorisationInfo>  RoamingAuthorisationInfos,

                                                  DateTime?                              Timestamp           = null,
                                                  CancellationToken?                     CancellationToken   = null,
                                                  EventTracking_Id                       EventTrackingId     = null,
                                                  TimeSpan?                              RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            this.RoamingAuthorisationInfos = RoamingAuthorisationInfos ?? new RoamingAuthorisationInfo[0];

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:OCHP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <OCHP:SetRoamingAuthorisationListRequest>
        //
        //          <!--1 or more repetitions / We allow also 0 :-->
        //          <OCHP:roamingAuthorisationInfoArray>
        //             ...
        //          </OCHP:roamingAuthorisationInfoArray>
        //
        //       </OCHP:SetRoamingAuthorisationListRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(SetRoamingAuthorisationListRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP set roaming authorisation list request.
        /// </summary>
        /// <param name="SetRoamingAuthorisationListRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetRoamingAuthorisationListRequest Parse(XElement             SetRoamingAuthorisationListRequestXML,
                                                               OnExceptionDelegate  OnException = null)
        {

            SetRoamingAuthorisationListRequest _SetRoamingAuthorisationListRequest;

            if (TryParse(SetRoamingAuthorisationListRequestXML, out _SetRoamingAuthorisationListRequest, OnException))
                return _SetRoamingAuthorisationListRequest;

            return null;

        }

        #endregion

        #region (static) Parse(SetRoamingAuthorisationListRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP set roaming authorisation list request.
        /// </summary>
        /// <param name="SetRoamingAuthorisationListRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetRoamingAuthorisationListRequest Parse(String               SetRoamingAuthorisationListRequestText,
                                                               OnExceptionDelegate  OnException = null)
        {

            SetRoamingAuthorisationListRequest _SetRoamingAuthorisationListRequest;

            if (TryParse(SetRoamingAuthorisationListRequestText, out _SetRoamingAuthorisationListRequest, OnException))
                return _SetRoamingAuthorisationListRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(SetRoamingAuthorisationListRequestXML,  out SetRoamingAuthorisationListRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP set roaming authorisation list request.
        /// </summary>
        /// <param name="SetRoamingAuthorisationListRequestXML">The XML to parse.</param>
        /// <param name="SetRoamingAuthorisationListRequest">The parsed set roaming authorisation list request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                  SetRoamingAuthorisationListRequestXML,
                                       out SetRoamingAuthorisationListRequest  SetRoamingAuthorisationListRequest,
                                       OnExceptionDelegate       OnException  = null)
        {

            try
            {

                SetRoamingAuthorisationListRequest = new SetRoamingAuthorisationListRequest(

                                                         SetRoamingAuthorisationListRequestXML.MapElements(OCHPNS.Default + "roamingAuthorisationInfoArray",
                                                                                                           RoamingAuthorisationInfo.Parse,
                                                                                                           OnException)

                                                     );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, SetRoamingAuthorisationListRequestXML, e);

                SetRoamingAuthorisationListRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SetRoamingAuthorisationListRequestText, out SetRoamingAuthorisationListRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP set roaming authorisation list request.
        /// </summary>
        /// <param name="SetRoamingAuthorisationListRequestText">The text to parse.</param>
        /// <param name="SetRoamingAuthorisationListRequest">The parsed set roaming authorisation list request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                  SetRoamingAuthorisationListRequestText,
                                       out SetRoamingAuthorisationListRequest  SetRoamingAuthorisationListRequest,
                                       OnExceptionDelegate                     OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SetRoamingAuthorisationListRequestText).Root,
                             out SetRoamingAuthorisationListRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, SetRoamingAuthorisationListRequestText, e);
            }

            SetRoamingAuthorisationListRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "SetRoamingAuthorisationListRequest",

                                RoamingAuthorisationInfos.Any()
                                    ? RoamingAuthorisationInfos.SafeSelect(infos => infos.ToXML(OCHPNS.Default + "roamingAuthorisationInfoArray"))
                                    : null

                           );

        #endregion


        #region Operator overloading

        #region Operator == (SetRoamingAuthorisationListRequest1, SetRoamingAuthorisationListRequest2)

        /// <summary>
        /// Compares two set roaming authorisation list requests for equality.
        /// </summary>
        /// <param name="SetRoamingAuthorisationListRequest1">A set roaming authorisation list request.</param>
        /// <param name="SetRoamingAuthorisationListRequest2">Another set roaming authorisation list request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetRoamingAuthorisationListRequest SetRoamingAuthorisationListRequest1, SetRoamingAuthorisationListRequest SetRoamingAuthorisationListRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SetRoamingAuthorisationListRequest1, SetRoamingAuthorisationListRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SetRoamingAuthorisationListRequest1 == null) || ((Object) SetRoamingAuthorisationListRequest2 == null))
                return false;

            return SetRoamingAuthorisationListRequest1.Equals(SetRoamingAuthorisationListRequest2);

        }

        #endregion

        #region Operator != (SetRoamingAuthorisationListRequest1, SetRoamingAuthorisationListRequest2)

        /// <summary>
        /// Compares two set roaming authorisation list requests for inequality.
        /// </summary>
        /// <param name="SetRoamingAuthorisationListRequest1">A set roaming authorisation list request.</param>
        /// <param name="SetRoamingAuthorisationListRequest2">Another set roaming authorisation list request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetRoamingAuthorisationListRequest SetRoamingAuthorisationListRequest1, SetRoamingAuthorisationListRequest SetRoamingAuthorisationListRequest2)

            => !(SetRoamingAuthorisationListRequest1 == SetRoamingAuthorisationListRequest2);

        #endregion

        #endregion

        #region IEquatable<SetRoamingAuthorisationListRequest> Members

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

            // Check if the given object is a set roaming authorisation list request.
            var SetRoamingAuthorisationListRequest = Object as SetRoamingAuthorisationListRequest;
            if ((Object) SetRoamingAuthorisationListRequest == null)
                return false;

            return this.Equals(SetRoamingAuthorisationListRequest);

        }

        #endregion

        #region Equals(SetRoamingAuthorisationListRequest)

        /// <summary>
        /// Compares two set roaming authorisation list requests for equality.
        /// </summary>
        /// <param name="SetRoamingAuthorisationListRequest">A set roaming authorisation list request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SetRoamingAuthorisationListRequest SetRoamingAuthorisationListRequest)
        {

            if ((Object) SetRoamingAuthorisationListRequest == null)
                return false;

            return RoamingAuthorisationInfos.Count().Equals(SetRoamingAuthorisationListRequest.RoamingAuthorisationInfos.Count());

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
