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
    /// An OCHP set charge point list response.
    /// </summary>
    public class SetChargePointListResponse
    {

        #region Properties

        /// <summary>
        /// The machine-readable result code.
        /// </summary>
        public Result                        Result                     { get; }

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
        public static SetChargePointListResponse OK(String Description = null)

            => new SetChargePointListResponse(Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static SetChargePointListResponse Partly(String Description = null)

            => new SetChargePointListResponse(Result.Unknown(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static SetChargePointListResponse NotAuthorized(String Description = null)

            => new SetChargePointListResponse(Result.Unknown(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static SetChargePointListResponse InvalidId(String Description = null)

            => new SetChargePointListResponse(Result.Unknown(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static SetChargePointListResponse Server(String Description = null)

            => new SetChargePointListResponse(Result.Unknown(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static SetChargePointListResponse Format(String Description = null)

            => new SetChargePointListResponse(Result.Unknown(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP set charge point list response.
        /// </summary>
        /// <param name="Result">A generic OHCP result.</param>
        /// <param name="RefusedChargePointInfos">An enumeration of refused charge point infos.</param>
        public SetChargePointListResponse(Result                        Result,
                                          IEnumerable<ChargePointInfo>  RefusedChargePointInfos = null)
        {

            this.Result                   = Result;
            this.RefusedChargePointInfos  = RefusedChargePointInfos ?? new ChargePointInfo[0];

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:SetChargePointListResponse>
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
        //       </ns:SetChargePointListResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(SetChargePointListResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP set charge point list response.
        /// </summary>
        /// <param name="SetChargePointListResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetChargePointListResponse Parse(XElement             SetChargePointListResponseXML,
                                                       OnExceptionDelegate  OnException = null)
        {

            SetChargePointListResponse _SetChargePointListResponse;

            if (TryParse(SetChargePointListResponseXML, out _SetChargePointListResponse, OnException))
                return _SetChargePointListResponse;

            return null;

        }

        #endregion

        #region (static) Parse(SetChargePointListResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP set charge point list response.
        /// </summary>
        /// <param name="SetChargePointListResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SetChargePointListResponse Parse(String               SetChargePointListResponseText,
                                                       OnExceptionDelegate  OnException = null)
        {

            SetChargePointListResponse _SetChargePointListResponse;

            if (TryParse(SetChargePointListResponseText, out _SetChargePointListResponse, OnException))
                return _SetChargePointListResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(SetChargePointListResponseXML,  out SetChargePointListResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP set charge point list response.
        /// </summary>
        /// <param name="SetChargePointListResponseXML">The XML to parse.</param>
        /// <param name="SetChargePointListResponse">The parsed set charge point list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                        SetChargePointListResponseXML,
                                       out SetChargePointListResponse  SetChargePointListResponse,
                                       OnExceptionDelegate             OnException  = null)
        {

            try
            {

                SetChargePointListResponse = new SetChargePointListResponse(

                                                 SetChargePointListResponseXML.MapElementOrFail (OCHPNS.Default + "result",
                                                                                                 "Missing or invalid XML element 'result'!",
                                                                                                 Result.Parse,
                                                                                                 OnException),

                                                 SetChargePointListResponseXML.MapElementsOrFail(OCHPNS.Default + "refusedChargePointInfo",
                                                                                                 "Missing or invalid XML element 'result'!",
                                                                                                 ChargePointInfo.Parse,
                                                                                                 OnException)

                                             );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, SetChargePointListResponseXML, e);

                SetChargePointListResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SetChargePointListResponseText, out SetChargePointListResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP set charge point list response.
        /// </summary>
        /// <param name="SetChargePointListResponseText">The text to parse.</param>
        /// <param name="SetChargePointListResponse">The parsed set charge point list response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                          SetChargePointListResponseText,
                                       out SetChargePointListResponse  SetChargePointListResponse,
                                       OnExceptionDelegate             OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SetChargePointListResponseText).Root,
                             out SetChargePointListResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, SetChargePointListResponseText, e);
            }

            SetChargePointListResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "SetChargePointListResponse",

                   new XElement(OCHPNS.Default + "result", Result.ToXML()),

                   RefusedChargePointInfos.SafeSelect(chargepointinfo => chargepointinfo.ToXML(OCHPNS.Default + "resultDescription"))

               );

        #endregion


        #region Operator overloading

        #region Operator == (SetChargePointListResponse1, SetChargePointListResponse2)

        /// <summary>
        /// Compares two set charge point list responses for equality.
        /// </summary>
        /// <param name="SetChargePointListResponse1">A set charge point list response.</param>
        /// <param name="SetChargePointListResponse2">Another set charge point list response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetChargePointListResponse SetChargePointListResponse1, SetChargePointListResponse SetChargePointListResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(SetChargePointListResponse1, SetChargePointListResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SetChargePointListResponse1 == null) || ((Object) SetChargePointListResponse2 == null))
                return false;

            return SetChargePointListResponse1.Equals(SetChargePointListResponse2);

        }

        #endregion

        #region Operator != (SetChargePointListResponse1, SetChargePointListResponse2)

        /// <summary>
        /// Compares two set charge point list responses for inequality.
        /// </summary>
        /// <param name="SetChargePointListResponse1">A set charge point list response.</param>
        /// <param name="SetChargePointListResponse2">Another set charge point list response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetChargePointListResponse SetChargePointListResponse1, SetChargePointListResponse SetChargePointListResponse2)

            => !(SetChargePointListResponse1 == SetChargePointListResponse2);

        #endregion

        #endregion

        #region IEquatable<SetChargePointListResponse> Members

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

            // Check if the given object is a set charge point list response.
            var SetChargePointListResponse = Object as SetChargePointListResponse;
            if ((Object) SetChargePointListResponse == null)
                return false;

            return this.Equals(SetChargePointListResponse);

        }

        #endregion

        #region Equals(SetChargePointListResponse)

        /// <summary>
        /// Compares two set charge point list responses for equality.
        /// </summary>
        /// <param name="SetChargePointListResponse">A set charge point list response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(SetChargePointListResponse SetChargePointListResponse)
        {

            if ((Object) SetChargePointListResponse == null)
                return false;

            return this.Result. Equals(SetChargePointListResponse.Result);

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
                return Result.GetHashCode();
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
