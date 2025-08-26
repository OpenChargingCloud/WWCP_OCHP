/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHP update status response.
    /// </summary>
    public class UpdateStatusResponse : AResponse<UpdateStatusRequest,
                                                  UpdateStatusResponse>
    {

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The update status request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateStatusResponse OK(UpdateStatusRequest  Request,
                                              String               Description = null)

            => new UpdateStatusResponse(Request,
                                        Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The update status request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateStatusResponse Partly(UpdateStatusRequest  Request,
                                                  String               Description = null)

            => new UpdateStatusResponse(Request,
                                        Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The update status request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateStatusResponse NotAuthorized(UpdateStatusRequest  Request,
                                                         String               Description = null)

            => new UpdateStatusResponse(Request,
                                        Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The update status request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateStatusResponse InvalidId(UpdateStatusRequest  Request,
                                                     String               Description = null)

            => new UpdateStatusResponse(Request,
                                        Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The update status request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateStatusResponse Server(UpdateStatusRequest  Request,
                                                  String               Description = null)

            => new UpdateStatusResponse(Request,
                                        Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The update status request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateStatusResponse Format(UpdateStatusRequest  Request,
                                                  String               Description = null)

            => new UpdateStatusResponse(Request,
                                        Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP update status response.
        /// </summary>
        /// <param name="Request">The update status request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        public UpdateStatusResponse(UpdateStatusRequest  Request,
                                    Result               Result)

            : base(Request, Result)

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

        #region (static) Parse   (Request, UpdateStatusResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP update status response.
        /// </summary>
        /// <param name="Request">The update status request leading to this response.</param>
        /// <param name="UpdateStatusResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateStatusResponse Parse(UpdateStatusRequest  Request,
                                                 XElement             UpdateStatusResponseXML,
                                                 OnExceptionDelegate  OnException = null)
        {

            UpdateStatusResponse _UpdateStatusResponse;

            if (TryParse(Request, UpdateStatusResponseXML, out _UpdateStatusResponse, OnException))
                return _UpdateStatusResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, UpdateStatusResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP update status response.
        /// </summary>
        /// <param name="Request">The update status request leading to this response.</param>
        /// <param name="UpdateStatusResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateStatusResponse Parse(UpdateStatusRequest  Request,
                                                 String               UpdateStatusResponseText,
                                                 OnExceptionDelegate  OnException = null)
        {

            UpdateStatusResponse _UpdateStatusResponse;

            if (TryParse(Request, UpdateStatusResponseText, out _UpdateStatusResponse, OnException))
                return _UpdateStatusResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, UpdateStatusResponseXML,  out UpdateStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP update status response.
        /// </summary>
        /// <param name="Request">The update status request leading to this response.</param>
        /// <param name="UpdateStatusResponseXML">The XML to parse.</param>
        /// <param name="UpdateStatusResponse">The parsed update status response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(UpdateStatusRequest       Request,
                                       XElement                  UpdateStatusResponseXML,
                                       out UpdateStatusResponse  UpdateStatusResponse,
                                       OnExceptionDelegate       OnException  = null)
        {

            try
            {

                UpdateStatusResponse = new UpdateStatusResponse(

                                           Request,

                                           UpdateStatusResponseXML.MapElementOrFail(OCHPNS.Default + "result",
                                                                                    Result.Parse,
                                                                                    OnException)

                                       );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, UpdateStatusResponseXML, e);

                UpdateStatusResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, UpdateStatusResponseText, out UpdateStatusResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP update status response.
        /// </summary>
        /// <param name="Request">The update status request leading to this response.</param>
        /// <param name="UpdateStatusResponseText">The text to parse.</param>
        /// <param name="UpdateStatusResponse">The parsed update status response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(UpdateStatusRequest       Request,
                                       String                    UpdateStatusResponseText,
                                       out UpdateStatusResponse  UpdateStatusResponse,
                                       OnExceptionDelegate       OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(UpdateStatusResponseText).Root,
                             out UpdateStatusResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, UpdateStatusResponseText, e);
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
                   Result.ToXML()
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
            if (ReferenceEquals(UpdateStatusResponse1, UpdateStatusResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) UpdateStatusResponse1 is null) || ((Object) UpdateStatusResponse2 is null))
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

            if (Object is null)
                return false;

            var UpdateStatusResponse = Object as UpdateStatusResponse;
            if ((Object) UpdateStatusResponse is null)
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
        public override Boolean Equals(UpdateStatusResponse UpdateStatusResponse)
        {

            if ((Object) UpdateStatusResponse is null)
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
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Result.ToString();

        #endregion


        public class Builder : ABuilder
        {

            public Builder(UpdateStatusResponse UpdateStatusResponse = null)
            {

                if (UpdateStatusResponse is not null)
                {

                    if (UpdateStatusResponse.CustomData is not null)
                        foreach (var item in UpdateStatusResponse.CustomData)
                            CustomData.Add(item.Key, item.Value);

                }

            }


            //public Acknowledgement<T> ToImmutable()

            //    => new Acknowledgement<T>(Request,
            //                              Result,
            //                              StatusCode,
            //                              SessionId,
            //                              PartnerSessionId,
            //                              CustomData);

        }

    }

}
