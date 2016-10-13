﻿/*
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
    /// An OCHP get status response.
    /// </summary>
    public class GetStatusResponse: AResponse
    {

        #region Properties

        /// <summary>
        /// An enumeration of EVSE status.
        /// </summary>
        public IEnumerable<EVSEStatus>     EVSEStatus        { get; }

        /// <summary>
        /// An enumeration of parking status.
        /// </summary>
        public IEnumerable<ParkingStatus>  ParkingStatus     { get; }

        /// <summary>
        /// An enumeration of charge detail records.
        /// </summary>
        public IEnumerable<EVSEStatus>     CombinedStatus    { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetStatusResponse OK(String Description = null)

            => new GetStatusResponse(Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetStatusResponse Partly(String Description = null)

            => new GetStatusResponse(Result.Unknown(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetStatusResponse NotAuthorized(String Description = null)

            => new GetStatusResponse(Result.Unknown(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetStatusResponse InvalidId(String Description = null)

            => new GetStatusResponse(Result.Unknown(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetStatusResponse Server(String Description = null)

            => new GetStatusResponse(Result.Unknown(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetStatusResponse Format(String Description = null)

            => new GetStatusResponse(Result.Unknown(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP get status response.
        /// </summary>
        /// <param name="Result">A generic OHCP result.</param>
        /// <param name="EVSEStatus">An enumeration of EVSE status.</param>
        /// <param name="ParkingStatus">An enumeration of parking status.</param>
        /// <param name="CombinedStatus">An enumeration of charge detail records.</param>
        public GetStatusResponse(Result                      Result,
                                 IEnumerable<EVSEStatus>     EVSEStatus      = null,
                                 IEnumerable<ParkingStatus>  ParkingStatus   = null,
                                 IEnumerable<EVSEStatus>     CombinedStatus  = null)

            : base(Result)

        {

            this.EVSEStatus      = EVSEStatus;
            this.ParkingStatus   = ParkingStatus;
            this.CombinedStatus  = CombinedStatus;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:GetStatusResponse>
        //
        //        <!--Zero or more repetitions:-->
        //        <ns:combined major = "?" minor="?" ttl="?">
        //           <ns:evseId>?</ns:evseId>
        //        </ns:combined>
        //
        //        <!--Zero or more repetitions:-->
        //        <ns:evse major = "?" minor="?" ttl="?">
        //           <ns:evseId>?</ns:evseId>
        //        </ns:evse>
        //
        //        <!--Zero or more repetitions:-->
        //        <ns:parking status = "?" ttl="?">
        //           <ns:parkingId>?</ns:parkingId>
        //        </ns:parking>
        //
        //      </ns:GetStatusResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(GetStatusResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get status response.
        /// </summary>
        /// <param name="GetStatusResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetStatusResponse Parse(XElement             GetStatusResponseXML,
                                              OnExceptionDelegate  OnException = null)
        {

            GetStatusResponse _GetStatusResponse;

            if (TryParse(GetStatusResponseXML, out _GetStatusResponse, OnException))
                return _GetStatusResponse;

            return null;

        }

        #endregion

        #region (static) Parse(GetStatusResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get status response.
        /// </summary>
        /// <param name="GetStatusResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetStatusResponse Parse(String               GetStatusResponseText,
                                              OnExceptionDelegate  OnException = null)
        {

            GetStatusResponse _GetStatusResponse;

            if (TryParse(GetStatusResponseText, out _GetStatusResponse, OnException))
                return _GetStatusResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(GetStatusResponseXML,  out GetStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get status response.
        /// </summary>
        /// <param name="GetStatusResponseXML">The XML to parse.</param>
        /// <param name="GetStatusResponse">The parsed get status response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement               GetStatusResponseXML,
                                       out GetStatusResponse  GetStatusResponse,
                                       OnExceptionDelegate    OnException  = null)
        {

            try
            {

                GetStatusResponse = new GetStatusResponse(

                                        // Fake it until the specification will be updated!
                                        new Result(ResultCodes.OK),

                                        GetStatusResponseXML.MapElements(OCHPNS.Default + "evse",
                                                                         OCHPv1_4.EVSEStatus.Parse,
                                                                         OnException),

                                        GetStatusResponseXML.MapElements(OCHPNS.Default + "parking",
                                                                         OCHPv1_4.ParkingStatus.Parse,
                                                                         OnException),

                                        GetStatusResponseXML.MapElements(OCHPNS.Default + "combined",
                                                                         OCHPv1_4.EVSEStatus.Parse,
                                                                         OnException)

                                    );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, GetStatusResponseXML, e);

                GetStatusResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetStatusResponseText, out GetStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get status response.
        /// </summary>
        /// <param name="GetStatusResponseText">The text to parse.</param>
        /// <param name="GetStatusResponse">The parsed get status response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                 GetStatusResponseText,
                                       out GetStatusResponse  GetStatusResponse,
                                       OnExceptionDelegate    OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetStatusResponseText).Root,
                             out GetStatusResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, GetStatusResponseText, e);
            }

            GetStatusResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetStatusResponse",

                   EVSEStatus.    Select(evse     => evse.    ToXML(OCHPNS.Default + "combined")),
                   ParkingStatus. Select(parking  => parking. ToXML(OCHPNS.Default + "parking")),
                   CombinedStatus.Select(combined => combined.ToXML(OCHPNS.Default + "evse"))

               );

        #endregion


        #region Operator overloading

        #region Operator == (GetStatusResponse1, GetStatusResponse2)

        /// <summary>
        /// Compares two get status responses for equality.
        /// </summary>
        /// <param name="GetStatusResponse1">A get status response.</param>
        /// <param name="GetStatusResponse2">Another get status response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetStatusResponse GetStatusResponse1, GetStatusResponse GetStatusResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetStatusResponse1, GetStatusResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetStatusResponse1 == null) || ((Object) GetStatusResponse2 == null))
                return false;

            return GetStatusResponse1.Equals(GetStatusResponse2);

        }

        #endregion

        #region Operator != (GetStatusResponse1, GetStatusResponse2)

        /// <summary>
        /// Compares two get status responses for inequality.
        /// </summary>
        /// <param name="GetStatusResponse1">A get status response.</param>
        /// <param name="GetStatusResponse2">Another get status response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetStatusResponse GetStatusResponse1, GetStatusResponse GetStatusResponse2)

            => !(GetStatusResponse1 == GetStatusResponse2);

        #endregion

        #endregion

        #region IEquatable<GetStatusResponse> Members

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

            // Check if the given object is a get status response.
            var GetStatusResponse = Object as GetStatusResponse;
            if ((Object) GetStatusResponse == null)
                return false;

            return this.Equals(GetStatusResponse);

        }

        #endregion

        #region Equals(GetStatusResponse)

        /// <summary>
        /// Compares two get status responses for equality.
        /// </summary>
        /// <param name="GetStatusResponse">A get status response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(GetStatusResponse GetStatusResponse)
        {

            if ((Object) GetStatusResponse == null)
                return false;

            return ((EVSEStatus     != null && GetStatusResponse.EVSEStatus     != null && EVSEStatus.    Count() == GetStatusResponse.EVSEStatus.    Count()) ||
                    (EVSEStatus     == null && GetStatusResponse.EVSEStatus     == null)) &&

                   ((ParkingStatus  != null && GetStatusResponse.ParkingStatus  != null && ParkingStatus. Count() == GetStatusResponse.ParkingStatus. Count()) ||
                    (ParkingStatus  == null && GetStatusResponse.ParkingStatus  == null)) &&

                   ((CombinedStatus != null && GetStatusResponse.CombinedStatus != null && CombinedStatus.Count() == GetStatusResponse.CombinedStatus.Count()) ||
                    (CombinedStatus == null && GetStatusResponse.CombinedStatus == null));

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

                return (EVSEStatus     != null ? EVSEStatus.    GetHashCode() : 0) * 17 ^
                       (ParkingStatus  != null ? ParkingStatus. GetHashCode() : 0) * 11 ^
                       (CombinedStatus != null ? CombinedStatus.GetHashCode() : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEStatus.Any()
                                 ? " " + EVSEStatus.    Count() + " evse status, "
                                 : "",
                             ParkingStatus.Any()
                                 ? " " + ParkingStatus. Count() + " parking status, "
                                 : "",
                             CombinedStatus.Any()
                                 ? " " + CombinedStatus.Count() + " combined status"
                                 : "");

        #endregion

    }

}