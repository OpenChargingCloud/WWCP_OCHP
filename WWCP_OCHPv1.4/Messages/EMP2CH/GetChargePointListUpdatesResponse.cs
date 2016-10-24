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

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHP get charge point list updates response.
    /// </summary>
    public class GetChargePointListUpdatesResponse : AResponse
    {

        #region Properties

        /// <summary>
        ///  An enumeration of charge points.
        /// </summary>
        public IEnumerable<ChargePointInfo>  ChargePoints    { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListUpdatesResponse OK(String Description = null)

            => new GetChargePointListUpdatesResponse(Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListUpdatesResponse Partly(String Description = null)

            => new GetChargePointListUpdatesResponse(Result.Unknown(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListUpdatesResponse NotAuthorized(String Description = null)

            => new GetChargePointListUpdatesResponse(Result.Unknown(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListUpdatesResponse InvalidId(String Description = null)

            => new GetChargePointListUpdatesResponse(Result.Unknown(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListUpdatesResponse Server(String Description = null)

            => new GetChargePointListUpdatesResponse(Result.Unknown(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static GetChargePointListUpdatesResponse Format(String Description = null)

            => new GetChargePointListUpdatesResponse(Result.Unknown(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP get charge point list response.
        /// </summary>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="ChargePoints">An enumeration of charge points.</param>
        public GetChargePointListUpdatesResponse(Result                        Result,
                                                 IEnumerable<ChargePointInfo>  ChargePoints = null)

            : base(Result)

        {

            this.ChargePoints  = ChargePoints ?? new ChargePointInfo[0];

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:GetChargePointListUpdatesResponse>
        //
        //          <ns:result>
        //
        //             <ns:resultCode>
        //                <ns:resultCode>?</ns:resultCode>
        //             </ns:resultCode>
        //
        //             <ns:resultDescription>?</ns:resultDescription>
        //
        //          </ns:result>
        //
        //          <!--Zero or more repetitions:-->
        //          <ns:chargePointInfoArray>
        //             ...
        //          <ns:chargePointInfoArray>
        //
        //       </ns:GetChargePointListUpdatesResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(GetChargePointListUpdatesResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get charge point list response.
        /// </summary>
        /// <param name="GetChargePointListUpdatesResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetChargePointListUpdatesResponse Parse(XElement             GetChargePointListUpdatesResponseXML,
                                                              OnExceptionDelegate  OnException = null)
        {

            GetChargePointListUpdatesResponse _GetChargePointListUpdatesResponse;

            if (TryParse(GetChargePointListUpdatesResponseXML, out _GetChargePointListUpdatesResponse, OnException))
                return _GetChargePointListUpdatesResponse;

            return null;

        }

        #endregion

        #region (static) Parse(GetChargePointListUpdatesResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get charge point list response.
        /// </summary>
        /// <param name="GetChargePointListUpdatesResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetChargePointListUpdatesResponse Parse(String               GetChargePointListUpdatesResponseText,
                                                              OnExceptionDelegate  OnException = null)
        {

            GetChargePointListUpdatesResponse _GetChargePointListUpdatesResponse;

            if (TryParse(GetChargePointListUpdatesResponseText, out _GetChargePointListUpdatesResponse, OnException))
                return _GetChargePointListUpdatesResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(GetChargePointListUpdatesResponseXML,  out GetChargePointListUpdatesResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get charge point list response.
        /// </summary>
        /// <param name="GetChargePointListUpdatesResponseXML">The XML to parse.</param>
        /// <param name="GetChargePointListUpdatesResponse">The parsed get charge point list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                               GetChargePointListUpdatesResponseXML,
                                       out GetChargePointListUpdatesResponse  GetChargePointListUpdatesResponse,
                                       OnExceptionDelegate                    OnException  = null)
        {

            try
            {

                GetChargePointListUpdatesResponse = new GetChargePointListUpdatesResponse(

                                                       GetChargePointListUpdatesResponseXML.MapElementOrFail (OCHPNS.Default + "result",
                                                                                                              Result.Parse,
                                                                                                              OnException),

                                                       GetChargePointListUpdatesResponseXML.MapElementsOrFail(OCHPNS.Default + "chargePointInfoArray",
                                                                                                              ChargePointInfo.Parse,
                                                                                                              OnException)

                                                   );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, GetChargePointListUpdatesResponseXML, e);

                GetChargePointListUpdatesResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(GetChargePointListUpdatesResponseText, out GetChargePointListUpdatesResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get charge point list response.
        /// </summary>
        /// <param name="GetChargePointListUpdatesResponseText">The text to parse.</param>
        /// <param name="GetChargePointListUpdatesResponse">The parsed get charge point list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                 GetChargePointListUpdatesResponseText,
                                       out GetChargePointListUpdatesResponse  GetChargePointListUpdatesResponse,
                                       OnExceptionDelegate                    OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(GetChargePointListUpdatesResponseText).Root,
                             out GetChargePointListUpdatesResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, GetChargePointListUpdatesResponseText, e);
            }

            GetChargePointListUpdatesResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetChargePointListUpdatesResponse",

                   new XElement(OCHPNS.Default + "result", Result.ToXML()),

                   ChargePoints.SafeSelect(chargepoint => chargepoint.ToXML(OCHPNS.Default + "chargePointInfoArray"))

               );

        #endregion


        #region Operator overloading

        #region Operator == (GetChargePointListUpdatesResponse1, GetChargePointListUpdatesResponse2)

        /// <summary>
        /// Compares two get charge point list update responses for equality.
        /// </summary>
        /// <param name="GetChargePointListUpdatesResponse1">A get charge point list update response.</param>
        /// <param name="GetChargePointListUpdatesResponse2">Another get charge point list update response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetChargePointListUpdatesResponse GetChargePointListUpdatesResponse1, GetChargePointListUpdatesResponse GetChargePointListUpdatesResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(GetChargePointListUpdatesResponse1, GetChargePointListUpdatesResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetChargePointListUpdatesResponse1 == null) || ((Object) GetChargePointListUpdatesResponse2 == null))
                return false;

            return GetChargePointListUpdatesResponse1.Equals(GetChargePointListUpdatesResponse2);

        }

        #endregion

        #region Operator != (GetChargePointListUpdatesResponse1, GetChargePointListUpdatesResponse2)

        /// <summary>
        /// Compares two get charge point list update responses for inequality.
        /// </summary>
        /// <param name="GetChargePointListUpdatesResponse1">A get charge point list update response.</param>
        /// <param name="GetChargePointListUpdatesResponse2">Another get charge point list update response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetChargePointListUpdatesResponse GetChargePointListUpdatesResponse1, GetChargePointListUpdatesResponse GetChargePointListUpdatesResponse2)

            => !(GetChargePointListUpdatesResponse1 == GetChargePointListUpdatesResponse2);

        #endregion

        #endregion

        #region IEquatable<GetChargePointListUpdatesResponse> Members

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

            // Check if the given object is a get charge point list update response.
            var GetChargePointListUpdatesResponse = Object as GetChargePointListUpdatesResponse;
            if ((Object) GetChargePointListUpdatesResponse == null)
                return false;

            return this.Equals(GetChargePointListUpdatesResponse);

        }

        #endregion

        #region Equals(GetChargePointListUpdatesResponse)

        /// <summary>
        /// Compares two get charge point list update responses for equality.
        /// </summary>
        /// <param name="GetChargePointListUpdatesResponse">A get charge point list update response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(GetChargePointListUpdatesResponse GetChargePointListUpdatesResponse)
        {

            if ((Object) GetChargePointListUpdatesResponse == null)
                return false;

            return this.Result. Equals(GetChargePointListUpdatesResponse.Result);

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

                return ChargePoints != null

                           ? Result.GetHashCode() * 11 ^
                             ChargePoints.SafeSelect(chargepoint => chargepoint.GetHashCode()).Aggregate((a, b) => a ^ b)

                           : Result.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Result,
                             ChargePoints.Any()
                                 ? " " + ChargePoints.Count() + " charge points"
                                 : "");

        #endregion

    }

}
