/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHP update tariffs response.
    /// </summary>
    public class UpdateTariffsResponse : AResponse<UpdateTariffsRequest,
                                                   UpdateTariffsResponse>
    {

        #region Properties

        /// <summary>
        /// An enumeration of refused tariff infos.
        /// </summary>
        public IEnumerable<TariffInfo>  RefusedTariffInfos   { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The update tariffs request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateTariffsResponse OK(UpdateTariffsRequest  Request,
                                               String                Description = null)

            => new UpdateTariffsResponse(Request,
                                         Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The update tariffs request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateTariffsResponse Partly(UpdateTariffsRequest  Request,
                                                   String                Description = null)

            => new UpdateTariffsResponse(Request,
                                         Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The update tariffs request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateTariffsResponse NotAuthorized(UpdateTariffsRequest  Request,
                                                          String                Description = null)

            => new UpdateTariffsResponse(Request,
                                         Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The update tariffs request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateTariffsResponse InvalidId(UpdateTariffsRequest  Request,
                                                      String                Description = null)

            => new UpdateTariffsResponse(Request,
                                         Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The update tariffs request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateTariffsResponse Server(UpdateTariffsRequest  Request,
                                                   String                Description = null)

            => new UpdateTariffsResponse(Request,
                                         Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The update tariffs request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static UpdateTariffsResponse Format(UpdateTariffsRequest  Request,
                                                   String                Description = null)

            => new UpdateTariffsResponse(Request,
                                         Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP update tariffs response.
        /// </summary>
        /// <param name="Request">The update tariffs request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="RefusedTariffInfos">An enumeration of refused tariff infos.</param>
        public UpdateTariffsResponse(UpdateTariffsRequest     Request,
                                     Result                   Result,
                                     IEnumerable<TariffInfo>  RefusedTariffInfos = null)

            : base(Request, Result)

        {

            this.RefusedTariffInfos = RefusedTariffInfos ?? new TariffInfo[0];

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:UpdateTariffsResponse>
        //
        //         <ns:result>
        //            <ns:resultCode>
        //               <ns:resultCode>?</ns:resultCode>
        //            </ns:resultCode>
        //            <ns:resultDescription>?</ns:resultDescription>
        //         </ns:result>
        //
        //         <!--Zero or more repetitions:-->
        //         <ns:refusedTariffInfo>
        //           ...
        //         </ns:refusedTariffInfo>
        //
        //      </ns:UpdateTariffsResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, UpdateTariffsResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP update tariffs response.
        /// </summary>
        /// <param name="Request">The update tariffs request leading to this response.</param>
        /// <param name="UpdateTariffsResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateTariffsResponse Parse(UpdateTariffsRequest  Request,
                                                  XElement              UpdateTariffsResponseXML,
                                                  OnExceptionDelegate   OnException = null)
        {

            UpdateTariffsResponse _UpdateTariffsResponse;

            if (TryParse(Request, UpdateTariffsResponseXML, out _UpdateTariffsResponse, OnException))
                return _UpdateTariffsResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, UpdateTariffsResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP update tariffs response.
        /// </summary>
        /// <param name="Request">The update tariffs request leading to this response.</param>
        /// <param name="UpdateTariffsResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateTariffsResponse Parse(UpdateTariffsRequest  Request,
                                                  String                UpdateTariffsResponseText,
                                                  OnExceptionDelegate   OnException = null)
        {

            UpdateTariffsResponse _UpdateTariffsResponse;

            if (TryParse(Request, UpdateTariffsResponseText, out _UpdateTariffsResponse, OnException))
                return _UpdateTariffsResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, UpdateTariffsResponseXML,  out UpdateTariffsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP update tariffs response.
        /// </summary>
        /// <param name="Request">The update tariffs request leading to this response.</param>
        /// <param name="UpdateTariffsResponseXML">The XML to parse.</param>
        /// <param name="UpdateTariffsResponse">The parsed update tariffs response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(UpdateTariffsRequest       Request,
                                       XElement                   UpdateTariffsResponseXML,
                                       out UpdateTariffsResponse  UpdateTariffsResponse,
                                       OnExceptionDelegate        OnException  = null)
        {

            try
            {

                UpdateTariffsResponse = new UpdateTariffsResponse(

                                            Request,

                                            UpdateTariffsResponseXML.MapElementOrFail(OCHPNS.Default + "result",
                                                                                      Result.Parse,
                                                                                      OnException),

                                            UpdateTariffsResponseXML.MapElements     (OCHPNS.Default + "refusedTariffInfo",
                                                                                      TariffInfo.Parse,
                                                                                      OnException)

                                        );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, UpdateTariffsResponseXML, e);

                UpdateTariffsResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, UpdateTariffsResponseText, out UpdateTariffsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP update tariffs response.
        /// </summary>
        /// <param name="Request">The update tariffs request leading to this response.</param>
        /// <param name="UpdateTariffsResponseText">The text to parse.</param>
        /// <param name="UpdateTariffsResponse">The parsed update tariffs response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(UpdateTariffsRequest       Request,
                                       String                     UpdateTariffsResponseText,
                                       out UpdateTariffsResponse  UpdateTariffsResponse,
                                       OnExceptionDelegate        OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(UpdateTariffsResponseText).Root,
                             out UpdateTariffsResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, UpdateTariffsResponseText, e);
            }

            UpdateTariffsResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "UpdateTariffsResponse",

                   Result.ToXML(),

                   RefusedTariffInfos.SafeSelect(tariffinfos => tariffinfos.ToXML(OCHPNS.Default + "refusedTariffInfo"))

               );

        #endregion


        #region Operator overloading

        #region Operator == (UpdateTariffsResponse1, UpdateTariffsResponse2)

        /// <summary>
        /// Compares two update tariffs responses for equality.
        /// </summary>
        /// <param name="UpdateTariffsResponse1">An update tariffs response.</param>
        /// <param name="UpdateTariffsResponse2">Another update tariffs response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (UpdateTariffsResponse UpdateTariffsResponse1, UpdateTariffsResponse UpdateTariffsResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(UpdateTariffsResponse1, UpdateTariffsResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) UpdateTariffsResponse1 == null) || ((Object) UpdateTariffsResponse2 == null))
                return false;

            return UpdateTariffsResponse1.Equals(UpdateTariffsResponse2);

        }

        #endregion

        #region Operator != (UpdateTariffsResponse1, UpdateTariffsResponse2)

        /// <summary>
        /// Compares two update tariffs responses for inequality.
        /// </summary>
        /// <param name="UpdateTariffsResponse1">An update tariffs response.</param>
        /// <param name="UpdateTariffsResponse2">Another update tariffs response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (UpdateTariffsResponse UpdateTariffsResponse1, UpdateTariffsResponse UpdateTariffsResponse2)

            => !(UpdateTariffsResponse1 == UpdateTariffsResponse2);

        #endregion

        #endregion

        #region IEquatable<UpdateTariffsResponse> Members

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

            // Check if the given object is an update tariffs response.
            var UpdateTariffsResponse = Object as UpdateTariffsResponse;
            if ((Object) UpdateTariffsResponse == null)
                return false;

            return this.Equals(UpdateTariffsResponse);

        }

        #endregion

        #region Equals(UpdateTariffsResponse)

        /// <summary>
        /// Compares two update tariffs responses for equality.
        /// </summary>
        /// <param name="UpdateTariffsResponse">An update tariffs response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(UpdateTariffsResponse UpdateTariffsResponse)
        {

            if ((Object) UpdateTariffsResponse == null)
                return false;

            return this.Result.Equals(UpdateTariffsResponse.Result);

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

            => String.Concat(Result,
                             RefusedTariffInfos.Any()
                                 ? " " + RefusedTariffInfos.Count() + " refused tariff infos"
                                 : "");

        #endregion

    }

}
