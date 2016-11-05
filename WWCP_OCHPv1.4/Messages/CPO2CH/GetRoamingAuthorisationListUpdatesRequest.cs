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

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHP get roaming authorisation list updates request.
    /// </summary>
    public class GetRoamingAuthorisationListUpdatesRequest : ARequest<GetRoamingAuthorisationListUpdatesRequest>
    {

        #region Properties

    /// <summary>
    /// The timestamp of the last roaming authorisation list update.
    /// </summary>
    public DateTime  LastUpdate   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP GetRoamingAuthorisationListUpdates XML/SOAP request.
        /// </summary>
        /// <param name="LastUpdate">The timestamp of the last roaming authorisation list update.</param>
        public GetRoamingAuthorisationListUpdatesRequest(DateTime  LastUpdate)
        {

            this.LastUpdate = LastUpdate;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:OCHP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:GetRoamingAuthorisationListUpdatesRequest>
        //
        //         <ns:lastUpdate>
        //            <ns:DateTime>?</ns:DateTime>
        //         </ns:lastUpdate>
        //
        //      </ns:GetRoamingAuthorisationListUpdatesRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(GetRoamingAuthorisationListUpdatesRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get roaming authorisation list updates request.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListUpdatesRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetRoamingAuthorisationListUpdatesRequest Parse(XElement             GetRoamingAuthorisationListUpdatesRequestXML,
                                                                      OnExceptionDelegate  OnException = null)
        {

            GetRoamingAuthorisationListUpdatesRequest _GetRoamingAuthorisationListUpdatesRequest;

            if (TryParse(GetRoamingAuthorisationListUpdatesRequestXML, out _GetRoamingAuthorisationListUpdatesRequest, OnException))
                return _GetRoamingAuthorisationListUpdatesRequest;

            return null;

        }

        #endregion

        #region (static) Parse(GetRoamingAuthorisationListUpdatesRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get roaming authorisation list updates request.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListUpdatesRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetRoamingAuthorisationListUpdatesRequest Parse(String               GetRoamingAuthorisationListUpdatesRequestText,
                                                                      OnExceptionDelegate  OnException = null)
        {

            GetRoamingAuthorisationListUpdatesRequest _GetRoamingAuthorisationListUpdatesRequest;

            if (TryParse(GetRoamingAuthorisationListUpdatesRequestText, out _GetRoamingAuthorisationListUpdatesRequest, OnException))
                return _GetRoamingAuthorisationListUpdatesRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(GetRoamingAuthorisationListUpdatesRequestXML,  out GetRoamingAuthorisationListUpdatesRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get roaming authorisation list updates request.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListUpdatesRequestXML">The XML to parse.</param>
        /// <param name="GetRoamingAuthorisationListUpdatesRequest">The parsed get roaming authorisation list updates request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                       GetRoamingAuthorisationListUpdatesRequestXML,
                                       out GetRoamingAuthorisationListUpdatesRequest  GetRoamingAuthorisationListUpdatesRequest,
                                       OnExceptionDelegate                            OnException  = null)
        {

            try
            {

                GetRoamingAuthorisationListUpdatesRequest = new GetRoamingAuthorisationListUpdatesRequest(

                                                                GetRoamingAuthorisationListUpdatesRequestXML.MapValueOrFail(OCHPNS.Default + "lastUpdate",
                                                                                                                            OCHPNS.Default + "DateTime",
                                                                                                                            DateTime.Parse)

                                                            );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, GetRoamingAuthorisationListUpdatesRequestXML, e);

                GetRoamingAuthorisationListUpdatesRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetRoamingAuthorisationListUpdatesRequestText, out GetRoamingAuthorisationListUpdatesRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get roaming authorisation list updates request.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListUpdatesRequestText">The text to parse.</param>
        /// <param name="GetRoamingAuthorisationListUpdatesRequest">The parsed get roaming authorisation list updates request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                         GetRoamingAuthorisationListUpdatesRequestText,
                                       out GetRoamingAuthorisationListUpdatesRequest  GetRoamingAuthorisationListUpdatesRequest,
                                       OnExceptionDelegate                            OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetRoamingAuthorisationListUpdatesRequestText).Root,
                             out GetRoamingAuthorisationListUpdatesRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, GetRoamingAuthorisationListUpdatesRequestText, e);
            }

            GetRoamingAuthorisationListUpdatesRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetRoamingAuthorisationListUpdatesRequest",
                                new XElement(OCHPNS.Default + "lastUpdate",
                                    new XElement(OCHPNS.Default + "DateTime",
                                        LastUpdate.ToIso8601()
                           )));

        #endregion


        #region Operator overloading

        #region Operator == (GetRoamingAuthorisationListUpdatesRequest1, GetRoamingAuthorisationListUpdatesRequest2)

        /// <summary>
        /// Compares two get roaming authorisation list updates requests for equality.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListUpdatesRequest1">A get roaming authorisation list updates request.</param>
        /// <param name="GetRoamingAuthorisationListUpdatesRequest2">Another get roaming authorisation list updates request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetRoamingAuthorisationListUpdatesRequest GetRoamingAuthorisationListUpdatesRequest1, GetRoamingAuthorisationListUpdatesRequest GetRoamingAuthorisationListUpdatesRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetRoamingAuthorisationListUpdatesRequest1, GetRoamingAuthorisationListUpdatesRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetRoamingAuthorisationListUpdatesRequest1 == null) || ((Object) GetRoamingAuthorisationListUpdatesRequest2 == null))
                return false;

            return GetRoamingAuthorisationListUpdatesRequest1.Equals(GetRoamingAuthorisationListUpdatesRequest2);

        }

        #endregion

        #region Operator != (GetRoamingAuthorisationListUpdatesRequest1, GetRoamingAuthorisationListUpdatesRequest2)

        /// <summary>
        /// Compares two get roaming authorisation list updates requests for inequality.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListUpdatesRequest1">A get roaming authorisation list updates request.</param>
        /// <param name="GetRoamingAuthorisationListUpdatesRequest2">Another get roaming authorisation list updates request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetRoamingAuthorisationListUpdatesRequest GetRoamingAuthorisationListUpdatesRequest1, GetRoamingAuthorisationListUpdatesRequest GetRoamingAuthorisationListUpdatesRequest2)

            => !(GetRoamingAuthorisationListUpdatesRequest1 == GetRoamingAuthorisationListUpdatesRequest2);

        #endregion

        #endregion

        #region IEquatable<GetRoamingAuthorisationListUpdatesRequest> Members

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

            // Check if the given object is a get roaming authorisation list updates request.
            var GetRoamingAuthorisationListUpdatesRequest = Object as GetRoamingAuthorisationListUpdatesRequest;
            if ((Object) GetRoamingAuthorisationListUpdatesRequest == null)
                return false;

            return this.Equals(GetRoamingAuthorisationListUpdatesRequest);

        }

        #endregion

        #region Equals(GetRoamingAuthorisationListUpdatesRequest)

        /// <summary>
        /// Compares two get roaming authorisation list updates requests for equality.
        /// </summary>
        /// <param name="GetRoamingAuthorisationListUpdatesRequest">A get roaming authorisation list updates request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetRoamingAuthorisationListUpdatesRequest GetRoamingAuthorisationListUpdatesRequest)
        {

            if ((Object) GetRoamingAuthorisationListUpdatesRequest == null)
                return false;

            return LastUpdate.Equals(GetRoamingAuthorisationListUpdatesRequest.LastUpdate);

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

                return LastUpdate.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => "last update " + LastUpdate.ToIso8601();

        #endregion


    }

}
