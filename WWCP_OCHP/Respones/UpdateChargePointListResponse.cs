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
    /// An OCHP update charge point list response.
    /// </summary>
    public class UpdateChargePointListResponse : AResponse
    {

        #region Properties

        /// <summary>
        /// A human-readable error description.
        /// </summary>
        public IEnumerable<ChargePointInfo>  RefusedChargePointInfos    { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateChargePointListResponse OK(String Description = null)

            => new UpdateChargePointListResponse(Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateChargePointListResponse Partly(String Description = null)

            => new UpdateChargePointListResponse(Result.Unknown(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateChargePointListResponse NotAuthorized(String Description = null)

            => new UpdateChargePointListResponse(Result.Unknown(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateChargePointListResponse InvalidId(String Description = null)

            => new UpdateChargePointListResponse(Result.Unknown(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateChargePointListResponse Server(String Description = null)

            => new UpdateChargePointListResponse(Result.Unknown(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateChargePointListResponse Format(String Description = null)

            => new UpdateChargePointListResponse(Result.Unknown(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP update charge point list response.
        /// </summary>
        /// <param name="Result">A generic OHCP result.</param>
        /// <param name="RefusedChargePointInfos">An enumeration of refused charge point infos.</param>
        public UpdateChargePointListResponse(Result                        Result,
                                             IEnumerable<ChargePointInfo>  RefusedChargePointInfos = null)

            : base(Result)

        {

            this.RefusedChargePointInfos  = RefusedChargePointInfos ?? new ChargePointInfo[0];

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:UpdateChargePointListResponse>
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
        //          <ns:refusedChargePointInfo>
        //             ...
        //          <ns:refusedChargePointInfo>
        //
        //       </ns:UpdateChargePointListResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(UpdateChargePointListResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP update charge point list response.
        /// </summary>
        /// <param name="UpdateChargePointListResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateChargePointListResponse Parse(XElement             UpdateChargePointListResponseXML,
                                                          OnExceptionDelegate  OnException = null)
        {

            UpdateChargePointListResponse _UpdateChargePointListResponse;

            if (TryParse(UpdateChargePointListResponseXML, out _UpdateChargePointListResponse, OnException))
                return _UpdateChargePointListResponse;

            return null;

        }

        #endregion

        #region (static) Parse(UpdateChargePointListResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP update charge point list response.
        /// </summary>
        /// <param name="UpdateChargePointListResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateChargePointListResponse Parse(String               UpdateChargePointListResponseText,
                                                          OnExceptionDelegate  OnException = null)
        {

            UpdateChargePointListResponse _UpdateChargePointListResponse;

            if (TryParse(UpdateChargePointListResponseText, out _UpdateChargePointListResponse, OnException))
                return _UpdateChargePointListResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(UpdateChargePointListResponseXML,  out UpdateChargePointListResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP update charge point list response.
        /// </summary>
        /// <param name="UpdateChargePointListResponseXML">The XML to parse.</param>
        /// <param name="UpdateChargePointListResponse">The parsed update charge point list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                           UpdateChargePointListResponseXML,
                                       out UpdateChargePointListResponse  UpdateChargePointListResponse,
                                       OnExceptionDelegate                OnException  = null)
        {

            try
            {

                UpdateChargePointListResponse = new UpdateChargePointListResponse(

                                                    UpdateChargePointListResponseXML.MapElementOrFail (OCHPNS.Default + "result",
                                                                                                       "Missing or invalid XML element 'result'!",
                                                                                                       Result.Parse,
                                                                                                       OnException),

                                                    UpdateChargePointListResponseXML.MapElementsOrFail(OCHPNS.Default + "refusedChargePointInfo",
                                                                                                       "Missing or invalid XML element 'refusedChargePointInfo'!",
                                                                                                       ChargePointInfo.Parse,
                                                                                                       OnException)

                                                );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, UpdateChargePointListResponseXML, e);

                UpdateChargePointListResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(UpdateChargePointListResponseText, out UpdateChargePointListResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP update charge point list response.
        /// </summary>
        /// <param name="UpdateChargePointListResponseText">The text to parse.</param>
        /// <param name="UpdateChargePointListResponse">The parsed update charge point list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                             UpdateChargePointListResponseText,
                                       out UpdateChargePointListResponse  UpdateChargePointListResponse,
                                       OnExceptionDelegate                OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(UpdateChargePointListResponseText).Root,
                             out UpdateChargePointListResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, UpdateChargePointListResponseText, e);
            }

            UpdateChargePointListResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "UpdateChargePointListResponse",

                   new XElement(OCHPNS.Default + "result", Result.ToXML()),

                   RefusedChargePointInfos.SafeSelect(chargepointinfo => chargepointinfo.ToXML(OCHPNS.Default + "refusedChargePointInfo"))

               );

        #endregion


        #region Operator overloading

        #region Operator == (UpdateChargePointListResponse1, UpdateChargePointListResponse2)

        /// <summary>
        /// Compares two update charge point list responses for equality.
        /// </summary>
        /// <param name="UpdateChargePointListResponse1">A update charge point list response.</param>
        /// <param name="UpdateChargePointListResponse2">Another update charge point list response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (UpdateChargePointListResponse UpdateChargePointListResponse1, UpdateChargePointListResponse UpdateChargePointListResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(UpdateChargePointListResponse1, UpdateChargePointListResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) UpdateChargePointListResponse1 == null) || ((Object) UpdateChargePointListResponse2 == null))
                return false;

            return UpdateChargePointListResponse1.Equals(UpdateChargePointListResponse2);

        }

        #endregion

        #region Operator != (UpdateChargePointListResponse1, UpdateChargePointListResponse2)

        /// <summary>
        /// Compares two update charge point list responses for inequality.
        /// </summary>
        /// <param name="UpdateChargePointListResponse1">A update charge point list response.</param>
        /// <param name="UpdateChargePointListResponse2">Another update charge point list response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (UpdateChargePointListResponse UpdateChargePointListResponse1, UpdateChargePointListResponse UpdateChargePointListResponse2)

            => !(UpdateChargePointListResponse1 == UpdateChargePointListResponse2);

        #endregion

        #endregion

        #region IEquatable<UpdateChargePointListResponse> Members

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

            // Check if the given object is a update charge point list response.
            var UpdateChargePointListResponse = Object as UpdateChargePointListResponse;
            if ((Object) UpdateChargePointListResponse == null)
                return false;

            return this.Equals(UpdateChargePointListResponse);

        }

        #endregion

        #region Equals(UpdateChargePointListResponse)

        /// <summary>
        /// Compares two update charge point list responses for equality.
        /// </summary>
        /// <param name="UpdateChargePointListResponse">A update charge point list response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(UpdateChargePointListResponse UpdateChargePointListResponse)
        {

            if ((Object) UpdateChargePointListResponse == null)
                return false;

            return this.Result. Equals(UpdateChargePointListResponse.Result);

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

                return RefusedChargePointInfos != null

                           ? Result.GetHashCode() * 11 ^
                             RefusedChargePointInfos.SafeSelect(info => info.GetHashCode()).Aggregate((a, b) => a ^ b)

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
                             RefusedChargePointInfos.Any()
                                 ? " " + RefusedChargePointInfos.Count() + " refused charge point infos"
                                 : "");

        #endregion

    }

}
