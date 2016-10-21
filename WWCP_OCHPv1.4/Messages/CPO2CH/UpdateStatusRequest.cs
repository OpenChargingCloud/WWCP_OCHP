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
    /// An OCHP update status request.
    /// </summary>
    public class UpdateStatusRequest
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE status.
        /// </summary>
        public IEnumerable<EVSEStatus>     EVSEStatus      { get; }

        /// <summary>
        /// An enumeration of parking status.
        /// </summary>
        public IEnumerable<ParkingStatus>  ParkingStatus   { get; }

        /// <summary>
        /// The default time to live for these status.
        /// </summary>
        public DateTime?                   DefaultTTL      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP UpdateStatus XML/SOAP request.
        /// </summary>
        /// <param name="EVSEStatus">An enumeration of EVSE status.</param>
        /// <param name="ParkingStatus">An enumeration of parking status.</param>
        /// <param name="DefaultTTL">The default time to live for these status.</param>
        public UpdateStatusRequest(IEnumerable<EVSEStatus>     EVSEStatus     = null,
                                   IEnumerable<ParkingStatus>  ParkingStatus  = null,
                                   DateTime?                   DefaultTTL     = null)
        {

            #region Initial checks

            if (EVSEStatus.IsNullOrEmpty() && ParkingStatus.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(EVSEStatus) + " + " + nameof(ParkingStatus),  "At least one of the given EVSE status or parking status enumeration must be neither null nor empty!");

            #endregion

            this.EVSEStatus     = EVSEStatus    ?? new EVSEStatus[0];
            this.ParkingStatus  = ParkingStatus ?? new ParkingStatus[0];
            this.DefaultTTL     = DefaultTTL    ?? new DateTime?();

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:OCHP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <OCHP:UpdateStatusRequest>
        //
        //          <!--Zero or more repetitions:-->
        //          <OCHP:evse major="?" minor="?" ttl="?">
        //             <OCHP:evseId>?</OCHP:evseId>
        //          </OCHP:evse>
        //
        //          <!--Zero or more repetitions:-->
        //          <OCHP:parking status="?" ttl="?">
        //             <OCHP:parkingId>?</OCHP:parkingId>
        //          </OCHP:parking>
        //
        //          <!--Optional:-->
        //          <OCHP:ttl>
        //             <OCHP:DateTime>?</OCHP:DateTime>
        //          </OCHP:ttl>
        //
        //       </OCHP:UpdateStatusRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(UpdateStatusRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP update status request.
        /// </summary>
        /// <param name="UpdateStatusRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateStatusRequest Parse(XElement             UpdateStatusRequestXML,
                                                OnExceptionDelegate  OnException = null)
        {

            UpdateStatusRequest _UpdateStatusRequest;

            if (TryParse(UpdateStatusRequestXML, out _UpdateStatusRequest, OnException))
                return _UpdateStatusRequest;

            return null;

        }

        #endregion

        #region (static) Parse(UpdateStatusRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP update status request.
        /// </summary>
        /// <param name="UpdateStatusRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateStatusRequest Parse(String               UpdateStatusRequestText,
                                                OnExceptionDelegate  OnException = null)
        {

            UpdateStatusRequest _UpdateStatusRequest;

            if (TryParse(UpdateStatusRequestText, out _UpdateStatusRequest, OnException))
                return _UpdateStatusRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(UpdateStatusRequestXML,  out UpdateStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP update status request.
        /// </summary>
        /// <param name="UpdateStatusRequestXML">The XML to parse.</param>
        /// <param name="UpdateStatusRequest">The parsed update status request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                 UpdateStatusRequestXML,
                                       out UpdateStatusRequest  UpdateStatusRequest,
                                       OnExceptionDelegate      OnException  = null)
        {

            try
            {

                UpdateStatusRequest = new UpdateStatusRequest(

                                          UpdateStatusRequestXML.MapElements(OCHPNS.Default + "UpdateStatusRequest",
                                                                             OCHPv1_4.EVSEStatus.Parse,
                                                                             OnException)

                                      );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, UpdateStatusRequestXML, e);

                UpdateStatusRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(UpdateStatusRequestText, out UpdateStatusRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP update status request.
        /// </summary>
        /// <param name="UpdateStatusRequestText">The text to parse.</param>
        /// <param name="UpdateStatusRequest">The parsed update status request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                   UpdateStatusRequestText,
                                       out UpdateStatusRequest  UpdateStatusRequest,
                                       OnExceptionDelegate      OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(UpdateStatusRequestText).Root,
                             out UpdateStatusRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, UpdateStatusRequestText, e);
            }

            UpdateStatusRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "UpdateStatusRequest",

                                EVSEStatus.   SafeSelect(status => status.ToXML()),
                                ParkingStatus.SafeSelect(status => status.ToXML()),

                                DefaultTTL.HasValue
                                    ? new XElement(OCHPNS.Default + "ttl",
                                          new XElement(OCHPNS.Default + "DateTime",  DefaultTTL.Value.ToIso8601())
                                      )
                                    : null

                           );

        #endregion


        #region Operator overloading

        #region Operator == (UpdateStatusRequest1, UpdateStatusRequest2)

        /// <summary>
        /// Compares two update status requests for equality.
        /// </summary>
        /// <param name="UpdateStatusRequest1">A update status request.</param>
        /// <param name="UpdateStatusRequest2">Another update status request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (UpdateStatusRequest UpdateStatusRequest1, UpdateStatusRequest UpdateStatusRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(UpdateStatusRequest1, UpdateStatusRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) UpdateStatusRequest1 == null) || ((Object) UpdateStatusRequest2 == null))
                return false;

            return UpdateStatusRequest1.Equals(UpdateStatusRequest2);

        }

        #endregion

        #region Operator != (UpdateStatusRequest1, UpdateStatusRequest2)

        /// <summary>
        /// Compares two update status requests for inequality.
        /// </summary>
        /// <param name="UpdateStatusRequest1">A update status request.</param>
        /// <param name="UpdateStatusRequest2">Another update status request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (UpdateStatusRequest UpdateStatusRequest1, UpdateStatusRequest UpdateStatusRequest2)

            => !(UpdateStatusRequest1 == UpdateStatusRequest2);

        #endregion

        #endregion

        #region IEquatable<UpdateStatusRequest> Members

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

            // Check if the given object is a update status request.
            var UpdateStatusRequest = Object as UpdateStatusRequest;
            if ((Object) UpdateStatusRequest == null)
                return false;

            return this.Equals(UpdateStatusRequest);

        }

        #endregion

        #region Equals(UpdateStatusRequest)

        /// <summary>
        /// Compares two update status requests for equality.
        /// </summary>
        /// <param name="UpdateStatusRequest">A update status request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(UpdateStatusRequest UpdateStatusRequest)
        {

            if ((Object) UpdateStatusRequest == null)
                return false;

            return EVSEStatus.   Count().Equals(UpdateStatusRequest.EVSEStatus.   Count()) &&
                   ParkingStatus.Count().Equals(UpdateStatusRequest.ParkingStatus.Count()) &&

                   ((DefaultTTL == null && UpdateStatusRequest.DefaultTTL == null) ||
                    (DefaultTTL != null && UpdateStatusRequest.DefaultTTL != null && DefaultTTL.Equals(UpdateStatusRequest.DefaultTTL)));


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

                return EVSEStatus.   GetHashCode() * 17 ^
                       ParkingStatus.GetHashCode() * 11 ^

                       (DefaultTTL.HasValue
                            ? DefaultTTL.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEStatus.   Count(), " EVSE status, ",
                             ParkingStatus.Count(), " parking status",

                             DefaultTTL.HasValue
                                 ? ", till " + DefaultTTL.Value
                                 : "");

        #endregion


    }

}
