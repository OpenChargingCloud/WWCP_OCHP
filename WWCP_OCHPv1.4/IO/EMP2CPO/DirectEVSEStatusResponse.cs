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

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// An OCHP direct EVSE status response.
    /// </summary>
    public class DirectEVSEStatusResponse : AResponse
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
        public static DirectEVSEStatusResponse OK(String Description = null)

            => new DirectEVSEStatusResponse(Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static DirectEVSEStatusResponse Partly(String Description = null)

            => new DirectEVSEStatusResponse(Result.Unknown(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static DirectEVSEStatusResponse NotAuthorized(String Description = null)

            => new DirectEVSEStatusResponse(Result.Unknown(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static DirectEVSEStatusResponse InvalidId(String Description = null)

            => new DirectEVSEStatusResponse(Result.Unknown(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static DirectEVSEStatusResponse Server(String Description = null)

            => new DirectEVSEStatusResponse(Result.Unknown(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static DirectEVSEStatusResponse Format(String Description = null)

            => new DirectEVSEStatusResponse(Result.Unknown(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP get status response.
        /// </summary>
        /// <param name="Result">A generic OHCP result.</param>
        /// <param name="EVSEStatus">An enumeration of EVSE status.</param>
        /// <param name="ParkingStatus">An enumeration of parking status.</param>
        /// <param name="CombinedStatus">An enumeration of charge detail records.</param>
        public DirectEVSEStatusResponse(Result                      Result,
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
        //      <ns:DirectEVSEStatusResponse>
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
        //      </ns:DirectEVSEStatusResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(DirectEVSEStatusResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get status response.
        /// </summary>
        /// <param name="DirectEVSEStatusResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static DirectEVSEStatusResponse Parse(XElement             DirectEVSEStatusResponseXML,
                                                     OnExceptionDelegate  OnException = null)
        {

            DirectEVSEStatusResponse _DirectEVSEStatusResponse;

            if (TryParse(DirectEVSEStatusResponseXML, out _DirectEVSEStatusResponse, OnException))
                return _DirectEVSEStatusResponse;

            return null;

        }

        #endregion

        #region (static) Parse(DirectEVSEStatusResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get status response.
        /// </summary>
        /// <param name="DirectEVSEStatusResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static DirectEVSEStatusResponse Parse(String               DirectEVSEStatusResponseText,
                                                     OnExceptionDelegate  OnException = null)
        {

            DirectEVSEStatusResponse _DirectEVSEStatusResponse;

            if (TryParse(DirectEVSEStatusResponseText, out _DirectEVSEStatusResponse, OnException))
                return _DirectEVSEStatusResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(DirectEVSEStatusResponseXML,  out DirectEVSEStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get status response.
        /// </summary>
        /// <param name="DirectEVSEStatusResponseXML">The XML to parse.</param>
        /// <param name="DirectEVSEStatusResponse">The parsed get status response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                      DirectEVSEStatusResponseXML,
                                       out DirectEVSEStatusResponse  DirectEVSEStatusResponse,
                                       OnExceptionDelegate           OnException  = null)
        {

            try
            {

                DirectEVSEStatusResponse = new DirectEVSEStatusResponse(

                                               // Fake it until the specification will be updated!
                                               new Result(ResultCodes.OK),

                                               DirectEVSEStatusResponseXML.MapElements(OCHPNS.Default + "evse",
                                                                                       OCHPv1_4.EVSEStatus.Parse,
                                                                                       OnException),

                                               DirectEVSEStatusResponseXML.MapElements(OCHPNS.Default + "parking",
                                                                                       OCHPv1_4.ParkingStatus.Parse,
                                                                                       OnException),

                                               DirectEVSEStatusResponseXML.MapElements(OCHPNS.Default + "combined",
                                                                                       OCHPv1_4.EVSEStatus.Parse,
                                                                                       OnException)

                                           );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, DirectEVSEStatusResponseXML, e);

                DirectEVSEStatusResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(DirectEVSEStatusResponseText, out DirectEVSEStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get status response.
        /// </summary>
        /// <param name="DirectEVSEStatusResponseText">The text to parse.</param>
        /// <param name="DirectEVSEStatusResponse">The parsed get status response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                        DirectEVSEStatusResponseText,
                                       out DirectEVSEStatusResponse  DirectEVSEStatusResponse,
                                       OnExceptionDelegate           OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(DirectEVSEStatusResponseText).Root,
                             out DirectEVSEStatusResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, DirectEVSEStatusResponseText, e);
            }

            DirectEVSEStatusResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "DirectEVSEStatusResponse",

                   EVSEStatus.    Select(evse     => evse.    ToXML(OCHPNS.Default + "combined")),
                   ParkingStatus. Select(parking  => parking. ToXML(OCHPNS.Default + "parking")),
                   CombinedStatus.Select(combined => combined.ToXML(OCHPNS.Default + "evse"))

               );

        #endregion


        #region Operator overloading

        #region Operator == (DirectEVSEStatusResponse1, DirectEVSEStatusResponse2)

        /// <summary>
        /// Compares two get status responses for equality.
        /// </summary>
        /// <param name="DirectEVSEStatusResponse1">A get status response.</param>
        /// <param name="DirectEVSEStatusResponse2">Another get status response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (DirectEVSEStatusResponse DirectEVSEStatusResponse1, DirectEVSEStatusResponse DirectEVSEStatusResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(DirectEVSEStatusResponse1, DirectEVSEStatusResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) DirectEVSEStatusResponse1 == null) || ((Object) DirectEVSEStatusResponse2 == null))
                return false;

            return DirectEVSEStatusResponse1.Equals(DirectEVSEStatusResponse2);

        }

        #endregion

        #region Operator != (DirectEVSEStatusResponse1, DirectEVSEStatusResponse2)

        /// <summary>
        /// Compares two get status responses for inequality.
        /// </summary>
        /// <param name="DirectEVSEStatusResponse1">A get status response.</param>
        /// <param name="DirectEVSEStatusResponse2">Another get status response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (DirectEVSEStatusResponse DirectEVSEStatusResponse1, DirectEVSEStatusResponse DirectEVSEStatusResponse2)

            => !(DirectEVSEStatusResponse1 == DirectEVSEStatusResponse2);

        #endregion

        #endregion

        #region IEquatable<DirectEVSEStatusResponse> Members

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
            var DirectEVSEStatusResponse = Object as DirectEVSEStatusResponse;
            if ((Object) DirectEVSEStatusResponse == null)
                return false;

            return this.Equals(DirectEVSEStatusResponse);

        }

        #endregion

        #region Equals(DirectEVSEStatusResponse)

        /// <summary>
        /// Compares two get status responses for equality.
        /// </summary>
        /// <param name="DirectEVSEStatusResponse">A get status response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(DirectEVSEStatusResponse DirectEVSEStatusResponse)
        {

            if ((Object) DirectEVSEStatusResponse == null)
                return false;

            return ((EVSEStatus     != null && DirectEVSEStatusResponse.EVSEStatus     != null && EVSEStatus.    Count() == DirectEVSEStatusResponse.EVSEStatus.    Count()) ||
                    (EVSEStatus     == null && DirectEVSEStatusResponse.EVSEStatus     == null)) &&

                   ((ParkingStatus  != null && DirectEVSEStatusResponse.ParkingStatus  != null && ParkingStatus. Count() == DirectEVSEStatusResponse.ParkingStatus. Count()) ||
                    (ParkingStatus  == null && DirectEVSEStatusResponse.ParkingStatus  == null)) &&

                   ((CombinedStatus != null && DirectEVSEStatusResponse.CombinedStatus != null && CombinedStatus.Count() == DirectEVSEStatusResponse.CombinedStatus.Count()) ||
                    (CombinedStatus == null && DirectEVSEStatusResponse.CombinedStatus == null));

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
