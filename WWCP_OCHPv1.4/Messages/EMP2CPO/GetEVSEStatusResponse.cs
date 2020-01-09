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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHPdirect EVSE status response.
    /// </summary>
    public class GetEVSEStatusResponse : AResponse<GetEVSEStatusRequest,
                                                   GetEVSEStatusResponse>
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
        /// <param name="Request">The EVSE status request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetEVSEStatusResponse OK(GetEVSEStatusRequest  Request,
                                               String                Description = null)

            => new GetEVSEStatusResponse(Request,
                                         Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The EVSE status request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetEVSEStatusResponse Partly(GetEVSEStatusRequest  Request,
                                                   String                Description = null)

            => new GetEVSEStatusResponse(Request,
                                         Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The EVSE status request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetEVSEStatusResponse NotAuthorized(GetEVSEStatusRequest  Request,
                                                          String                Description = null)

            => new GetEVSEStatusResponse(Request,
                                         Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The EVSE status request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetEVSEStatusResponse InvalidId(GetEVSEStatusRequest  Request,
                                                      String                Description = null)

            => new GetEVSEStatusResponse(Request,
                                         Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The EVSE status request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetEVSEStatusResponse Server(GetEVSEStatusRequest  Request,
                                                   String                Description = null)

            => new GetEVSEStatusResponse(Request,
                                         Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The EVSE status request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetEVSEStatusResponse Format(GetEVSEStatusRequest  Request,
                                                   String                Description = null)

            => new GetEVSEStatusResponse(Request,
                                         Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHPdirect get status response.
        /// </summary>
        /// <param name="Request">The EVSE status request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="EVSEStatus">An enumeration of EVSE status.</param>
        /// <param name="ParkingStatus">An enumeration of parking status.</param>
        /// <param name="CombinedStatus">An enumeration of charge detail records.</param>
        public GetEVSEStatusResponse(GetEVSEStatusRequest        Request,
                                     Result                      Result,
                                     IEnumerable<EVSEStatus>     EVSEStatus      = null,
                                     IEnumerable<ParkingStatus>  ParkingStatus   = null,
                                     IEnumerable<EVSEStatus>     CombinedStatus  = null)

            : base(Request, Result)

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

        #region (static) Parse   (Request, GetEVSEStatusResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHPdirect get status response.
        /// </summary>
        /// <param name="Request">The EVSE status request leading to this response.</param>
        /// <param name="GetEVSEStatusResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetEVSEStatusResponse Parse(GetEVSEStatusRequest  Request,
                                                  XElement              GetEVSEStatusResponseXML,
                                                  OnExceptionDelegate   OnException = null)
        {

            GetEVSEStatusResponse _GetEVSEStatusResponse;

            if (TryParse(Request, GetEVSEStatusResponseXML, out _GetEVSEStatusResponse, OnException))
                return _GetEVSEStatusResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, GetEVSEStatusResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHPdirect get status response.
        /// </summary>
        /// <param name="Request">The EVSE status request leading to this response.</param>
        /// <param name="GetEVSEStatusResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetEVSEStatusResponse Parse(GetEVSEStatusRequest  Request,
                                                  String                GetEVSEStatusResponseText,
                                                  OnExceptionDelegate   OnException = null)
        {

            GetEVSEStatusResponse _GetEVSEStatusResponse;

            if (TryParse(Request, GetEVSEStatusResponseText, out _GetEVSEStatusResponse, OnException))
                return _GetEVSEStatusResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, GetEVSEStatusResponseXML,  out GetEVSEStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHPdirect get status response.
        /// </summary>
        /// <param name="Request">The EVSE status request leading to this response.</param>
        /// <param name="GetEVSEStatusResponseXML">The XML to parse.</param>
        /// <param name="GetEVSEStatusResponse">The parsed get status response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetEVSEStatusRequest       Request,
                                       XElement                   GetEVSEStatusResponseXML,
                                       out GetEVSEStatusResponse  GetEVSEStatusResponse,
                                       OnExceptionDelegate        OnException  = null)
        {

            try
            {

                GetEVSEStatusResponse = new GetEVSEStatusResponse(

                                            Request,

                                            // Fake it until the specification will be updated!
                                            Result.OK(),

                                            GetEVSEStatusResponseXML.MapElements(OCHPNS.Default + "evse",
                                                                                    OCHPv1_4.EVSEStatus.Parse,
                                                                                    OnException),

                                            GetEVSEStatusResponseXML.MapElements(OCHPNS.Default + "parking",
                                                                                    OCHPv1_4.ParkingStatus.Parse,
                                                                                    OnException),

                                            GetEVSEStatusResponseXML.MapElements(OCHPNS.Default + "combined",
                                                                                    OCHPv1_4.EVSEStatus.Parse,
                                                                                    OnException)

                                        );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, GetEVSEStatusResponseXML, e);

                GetEVSEStatusResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, GetEVSEStatusResponseText, out GetEVSEStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHPdirect get status response.
        /// </summary>
        /// <param name="Request">The EVSE status request leading to this response.</param>
        /// <param name="GetEVSEStatusResponseText">The text to parse.</param>
        /// <param name="GetEVSEStatusResponse">The parsed get status response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetEVSEStatusRequest       Request,
                                       String                     GetEVSEStatusResponseText,
                                       out GetEVSEStatusResponse  GetEVSEStatusResponse,
                                       OnExceptionDelegate        OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(GetEVSEStatusResponseText).Root,
                             out GetEVSEStatusResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, GetEVSEStatusResponseText, e);
            }

            GetEVSEStatusResponse = null;
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

        #region Operator == (GetEVSEStatusResponse1, GetEVSEStatusResponse2)

        /// <summary>
        /// Compares two get status responses for equality.
        /// </summary>
        /// <param name="GetEVSEStatusResponse1">A get status response.</param>
        /// <param name="GetEVSEStatusResponse2">Another get status response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetEVSEStatusResponse GetEVSEStatusResponse1, GetEVSEStatusResponse GetEVSEStatusResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetEVSEStatusResponse1, GetEVSEStatusResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetEVSEStatusResponse1 == null) || ((Object) GetEVSEStatusResponse2 == null))
                return false;

            return GetEVSEStatusResponse1.Equals(GetEVSEStatusResponse2);

        }

        #endregion

        #region Operator != (GetEVSEStatusResponse1, GetEVSEStatusResponse2)

        /// <summary>
        /// Compares two get status responses for inequality.
        /// </summary>
        /// <param name="GetEVSEStatusResponse1">A get status response.</param>
        /// <param name="GetEVSEStatusResponse2">Another get status response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetEVSEStatusResponse GetEVSEStatusResponse1, GetEVSEStatusResponse GetEVSEStatusResponse2)

            => !(GetEVSEStatusResponse1 == GetEVSEStatusResponse2);

        #endregion

        #endregion

        #region IEquatable<GetEVSEStatusResponse> Members

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
            var GetEVSEStatusResponse = Object as GetEVSEStatusResponse;
            if ((Object) GetEVSEStatusResponse == null)
                return false;

            return this.Equals(GetEVSEStatusResponse);

        }

        #endregion

        #region Equals(GetEVSEStatusResponse)

        /// <summary>
        /// Compares two get status responses for equality.
        /// </summary>
        /// <param name="GetEVSEStatusResponse">A get status response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetEVSEStatusResponse GetEVSEStatusResponse)
        {

            if ((Object) GetEVSEStatusResponse == null)
                return false;

            return ((EVSEStatus     != null && GetEVSEStatusResponse.EVSEStatus     != null && EVSEStatus.    Count() == GetEVSEStatusResponse.EVSEStatus.    Count()) ||
                    (EVSEStatus     == null && GetEVSEStatusResponse.EVSEStatus     == null)) &&

                   ((ParkingStatus  != null && GetEVSEStatusResponse.ParkingStatus  != null && ParkingStatus. Count() == GetEVSEStatusResponse.ParkingStatus. Count()) ||
                    (ParkingStatus  == null && GetEVSEStatusResponse.ParkingStatus  == null)) &&

                   ((CombinedStatus != null && GetEVSEStatusResponse.CombinedStatus != null && CombinedStatus.Count() == GetEVSEStatusResponse.CombinedStatus.Count()) ||
                    (CombinedStatus == null && GetEVSEStatusResponse.CombinedStatus == null));

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

                return (EVSEStatus     != null
                            ? EVSEStatus.    GetHashCode() * 17
                            : 0) ^

                       (ParkingStatus  != null
                            ? ParkingStatus. GetHashCode() * 11
                            : 0) ^

                       (CombinedStatus != null
                            ? CombinedStatus.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
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
