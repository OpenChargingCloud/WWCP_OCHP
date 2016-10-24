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
    /// An OCHP update status response.
    /// </summary>
    public class UpdateStatusResponse : AResponse
    {

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateStatusResponse OK(String Description = null)

            => new UpdateStatusResponse(Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateStatusResponse Partly(String Description = null)

            => new UpdateStatusResponse(Result.Unknown(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateStatusResponse NotAuthorized(String Description = null)

            => new UpdateStatusResponse(Result.Unknown(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateStatusResponse InvalidId(String Description = null)

            => new UpdateStatusResponse(Result.Unknown(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateStatusResponse Server(String Description = null)

            => new UpdateStatusResponse(Result.Unknown(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateStatusResponse Format(String Description = null)

            => new UpdateStatusResponse(Result.Unknown(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP update status response.
        /// </summary>
        /// <param name="Result">A generic OCHP result.</param>
        public UpdateStatusResponse(Result  Result)
            : base(Result)
        { }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:UpdateStatusResponse>
        //
        //         <ns:result>
        //            <ns:resultCode>
        //               <ns:resultCode>?</ns:resultCode>
        //            </ns:resultCode>
        //            <ns:resultDescription>?</ns:resultDescription>
        //         </ns:result>
        //
        //      </ns:UpdateStatusResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(UpdateStatusResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP update status response.
        /// </summary>
        /// <param name="UpdateStatusResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateStatusResponse Parse(XElement             UpdateStatusResponseXML,
                                                 OnExceptionDelegate  OnException = null)
        {

            UpdateStatusResponse _UpdateStatusResponse;

            if (TryParse(UpdateStatusResponseXML, out _UpdateStatusResponse, OnException))
                return _UpdateStatusResponse;

            return null;

        }

        #endregion

        #region (static) Parse(UpdateStatusResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP update status response.
        /// </summary>
        /// <param name="UpdateStatusResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateStatusResponse Parse(String               UpdateStatusResponseText,
                                                 OnExceptionDelegate  OnException = null)
        {

            UpdateStatusResponse _UpdateStatusResponse;

            if (TryParse(UpdateStatusResponseText, out _UpdateStatusResponse, OnException))
                return _UpdateStatusResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(UpdateStatusResponseXML,  out UpdateStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP update status response.
        /// </summary>
        /// <param name="UpdateStatusResponseXML">The XML to parse.</param>
        /// <param name="UpdateStatusResponse">The parsed update status response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                  UpdateStatusResponseXML,
                                       out UpdateStatusResponse  UpdateStatusResponse,
                                       OnExceptionDelegate       OnException  = null)
        {

            try
            {

                UpdateStatusResponse = new UpdateStatusResponse(

                                           UpdateStatusResponseXML.MapElementOrFail(OCHPNS.Default + "result",
                                                                                    Result.Parse,
                                                                                    OnException)

                                       );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, UpdateStatusResponseXML, e);

                UpdateStatusResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(UpdateStatusResponseText, out UpdateStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP update status response.
        /// </summary>
        /// <param name="UpdateStatusResponseText">The text to parse.</param>
        /// <param name="UpdateStatusResponse">The parsed update status response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                    UpdateStatusResponseText,
                                       out UpdateStatusResponse  UpdateStatusResponse,
                                       OnExceptionDelegate       OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(UpdateStatusResponseText).Root,
                             out UpdateStatusResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, UpdateStatusResponseText, e);
            }

            UpdateStatusResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "UpdateStatusResponse",
                   new XElement(OCHPNS.Default + "result", Result.ToXML())
               );

        #endregion


        #region Operator overloading

        #region Operator == (UpdateStatusResponse1, UpdateStatusResponse2)

        /// <summary>
        /// Compares two update status responses for equality.
        /// </summary>
        /// <param name="UpdateStatusResponse1">An update status response.</param>
        /// <param name="UpdateStatusResponse2">Another update status response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (UpdateStatusResponse UpdateStatusResponse1, UpdateStatusResponse UpdateStatusResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(UpdateStatusResponse1, UpdateStatusResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) UpdateStatusResponse1 == null) || ((Object) UpdateStatusResponse2 == null))
                return false;

            return UpdateStatusResponse1.Equals(UpdateStatusResponse2);

        }

        #endregion

        #region Operator != (UpdateStatusResponse1, UpdateStatusResponse2)

        /// <summary>
        /// Compares two update status responses for inequality.
        /// </summary>
        /// <param name="UpdateStatusResponse1">An update status response.</param>
        /// <param name="UpdateStatusResponse2">Another update status response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (UpdateStatusResponse UpdateStatusResponse1, UpdateStatusResponse UpdateStatusResponse2)

            => !(UpdateStatusResponse1 == UpdateStatusResponse2);

        #endregion

        #endregion

        #region IEquatable<UpdateStatusResponse> Members

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

            // Check if the given object is an update status response.
            var UpdateStatusResponse = Object as UpdateStatusResponse;
            if ((Object) UpdateStatusResponse == null)
                return false;

            return this.Equals(UpdateStatusResponse);

        }

        #endregion

        #region Equals(UpdateStatusResponse)

        /// <summary>
        /// Compares two update status responses for equality.
        /// </summary>
        /// <param name="UpdateStatusResponse">An update status response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(UpdateStatusResponse UpdateStatusResponse)
        {

            if ((Object) UpdateStatusResponse == null)
                return false;

            return this.Result.Equals(UpdateStatusResponse.Result);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => Result.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => Result.ToString();

        #endregion

    }

}
