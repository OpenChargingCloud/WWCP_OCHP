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

namespace cloud.charging.open.protocols.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHP get tariff updates response.
    /// </summary>
    public class GetTariffUpdatesResponse : AResponse<GetTariffUpdatesRequest,
                                                      GetTariffUpdatesResponse>
    {

        #region Properties

        /// <summary>
        ///  An enumeration of tariff infos.
        /// </summary>
        public IEnumerable<TariffInfo>  TariffInfos   { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The get tariff updates request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetTariffUpdatesResponse OK(GetTariffUpdatesRequest  Request,
                                                  String                   Description = null)

            => new GetTariffUpdatesResponse(Request,
                                            Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The get tariff updates request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetTariffUpdatesResponse Partly(GetTariffUpdatesRequest  Request,
                                                      String                   Description = null)

            => new GetTariffUpdatesResponse(Request,
                                            Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The get tariff updates request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetTariffUpdatesResponse NotAuthorized(GetTariffUpdatesRequest  Request,
                                                             String                   Description = null)

            => new GetTariffUpdatesResponse(Request,
                                            Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The get tariff updates request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetTariffUpdatesResponse InvalidId(GetTariffUpdatesRequest  Request,
                                                         String                   Description = null)

            => new GetTariffUpdatesResponse(Request,
                                            Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The get tariff updates request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetTariffUpdatesResponse Server(GetTariffUpdatesRequest  Request,
                                                      String                   Description = null)

            => new GetTariffUpdatesResponse(Request,
                                            Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The get tariff updates request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static GetTariffUpdatesResponse Format(GetTariffUpdatesRequest  Request,
                                                      String                   Description = null)

            => new GetTariffUpdatesResponse(Request,
                                            Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP get tariff updates response.
        /// </summary>
        /// <param name="Request">The get tariff updates request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="TariffInfos">An enumeration of tariff infos.</param>
        public GetTariffUpdatesResponse(GetTariffUpdatesRequest  Request,
                                        Result                   Result,
                                        IEnumerable<TariffInfo>  TariffInfos = null)

            : base(Request, Result)

        {

            this.TariffInfos = TariffInfos ?? new TariffInfo[0];

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:GetTariffUpdatesRequest>
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
        //          <ns:TariffInfoArray>
        //             ...
        //          <ns:TariffInfoArray>
        //
        //       </ns:GetTariffUpdatesRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse   (Request, GetTariffUpdatesResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP get tariff updates response.
        /// </summary>
        /// <param name="Request">The get tariff updates request leading to this response.</param>
        /// <param name="GetTariffUpdatesResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetTariffUpdatesResponse Parse(GetTariffUpdatesRequest  Request,
                                                     XElement                 GetTariffUpdatesResponseXML,
                                                     OnExceptionDelegate      OnException = null)
        {

            GetTariffUpdatesResponse _GetTariffUpdatesResponse;

            if (TryParse(Request, GetTariffUpdatesResponseXML, out _GetTariffUpdatesResponse, OnException))
                return _GetTariffUpdatesResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, GetTariffUpdatesResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP get tariff updates response.
        /// </summary>
        /// <param name="Request">The get tariff updates request leading to this response.</param>
        /// <param name="GetTariffUpdatesResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static GetTariffUpdatesResponse Parse(GetTariffUpdatesRequest  Request,
                                                     String                   GetTariffUpdatesResponseText,
                                                     OnExceptionDelegate      OnException = null)
        {

            GetTariffUpdatesResponse _GetTariffUpdatesResponse;

            if (TryParse(Request, GetTariffUpdatesResponseText, out _GetTariffUpdatesResponse, OnException))
                return _GetTariffUpdatesResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, GetTariffUpdatesResponseXML,  out GetTariffUpdatesResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP get tariff updates response.
        /// </summary>
        /// <param name="Request">The get tariff updates request leading to this response.</param>
        /// <param name="GetTariffUpdatesResponseXML">The XML to parse.</param>
        /// <param name="GetTariffUpdatesResponse">The parsed get tariff updates response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetTariffUpdatesRequest       Request,
                                       XElement                      GetTariffUpdatesResponseXML,
                                       out GetTariffUpdatesResponse  GetTariffUpdatesResponse,
                                       OnExceptionDelegate           OnException  = null)
        {

            try
            {

                GetTariffUpdatesResponse = new GetTariffUpdatesResponse(

                                               Request,

                                               GetTariffUpdatesResponseXML.MapElementOrFail(OCHPNS.Default + "result",
                                                                                             Result.Parse,
                                                                                             OnException),

                                               GetTariffUpdatesResponseXML.MapElements      (OCHPNS.Default + "TariffInfoArray",
                                                                                             TariffInfo.Parse,
                                                                                             OnException)

                                           );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, GetTariffUpdatesResponseXML, e);

                GetTariffUpdatesResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, GetTariffUpdatesResponseText, out GetTariffUpdatesResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP get tariff updates response.
        /// </summary>
        /// <param name="Request">The get tariff updates request leading to this response.</param>
        /// <param name="GetTariffUpdatesResponseText">The text to parse.</param>
        /// <param name="GetTariffUpdatesResponse">The parsed get tariff updates response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(GetTariffUpdatesRequest       Request,
                                       String                        GetTariffUpdatesResponseText,
                                       out GetTariffUpdatesResponse  GetTariffUpdatesResponse,
                                       OnExceptionDelegate           OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(GetTariffUpdatesResponseText).Root,
                             out GetTariffUpdatesResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, GetTariffUpdatesResponseText, e);
            }

            GetTariffUpdatesResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "GetTariffUpdatesResponse",

                   Result.ToXML(),

                   TariffInfos.Any()
                       ? TariffInfos.SafeSelect(tariffinfo => tariffinfo.ToXML(OCHPNS.Default + "TariffInfoArray"))
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (GetTariffUpdatesResponse1, GetTariffUpdatesResponse2)

        /// <summary>
        /// Compares two get tariff update responses for equality.
        /// </summary>
        /// <param name="GetTariffUpdatesResponse1">A get tariff update response.</param>
        /// <param name="GetTariffUpdatesResponse2">Another get tariff update response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetTariffUpdatesResponse GetTariffUpdatesResponse1, GetTariffUpdatesResponse GetTariffUpdatesResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GetTariffUpdatesResponse1, GetTariffUpdatesResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) GetTariffUpdatesResponse1 == null) || ((Object) GetTariffUpdatesResponse2 == null))
                return false;

            return GetTariffUpdatesResponse1.Equals(GetTariffUpdatesResponse2);

        }

        #endregion

        #region Operator != (GetTariffUpdatesResponse1, GetTariffUpdatesResponse2)

        /// <summary>
        /// Compares two get tariff update responses for inequality.
        /// </summary>
        /// <param name="GetTariffUpdatesResponse1">A get tariff update response.</param>
        /// <param name="GetTariffUpdatesResponse2">Another get tariff update response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetTariffUpdatesResponse GetTariffUpdatesResponse1, GetTariffUpdatesResponse GetTariffUpdatesResponse2)

            => !(GetTariffUpdatesResponse1 == GetTariffUpdatesResponse2);

        #endregion

        #endregion

        #region IEquatable<GetTariffUpdatesResponse> Members

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

            // Check if the given object is a get tariff update response.
            var GetTariffUpdatesResponse = Object as GetTariffUpdatesResponse;
            if ((Object) GetTariffUpdatesResponse == null)
                return false;

            return this.Equals(GetTariffUpdatesResponse);

        }

        #endregion

        #region Equals(GetTariffUpdatesResponse)

        /// <summary>
        /// Compares two get tariff update responses for equality.
        /// </summary>
        /// <param name="GetTariffUpdatesResponse">A get tariff update response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(GetTariffUpdatesResponse GetTariffUpdatesResponse)
        {

            if ((Object) GetTariffUpdatesResponse == null)
                return false;

            return this.Result. Equals(GetTariffUpdatesResponse.Result);

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

                return TariffInfos != null

                           ? Result.GetHashCode() * 11 ^
                             TariffInfos.SafeSelect(tariffinfo => tariffinfo.GetHashCode()).Aggregate((a, b) => a ^ b)

                           : Result.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Result,
                             TariffInfos.Any()
                                 ? " " + TariffInfos.Count() + " tariff info(s)"
                                 : "");

        #endregion

    }

}
