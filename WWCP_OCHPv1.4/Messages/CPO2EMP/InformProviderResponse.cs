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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHPdirect inform provider response.
    /// </summary>
    public class InformProviderResponse : AResponse
    {

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static InformProviderResponse OK(String Description = null)

            => new InformProviderResponse(Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static InformProviderResponse Partly(String Description = null)

            => new InformProviderResponse(Result.Unknown(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static InformProviderResponse NotAuthorized(String Description = null)

            => new InformProviderResponse(Result.Unknown(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static InformProviderResponse InvalidId(String Description = null)

            => new InformProviderResponse(Result.Unknown(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static InformProviderResponse Server(String Description = null)

            => new InformProviderResponse(Result.Unknown(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static InformProviderResponse Format(String Description = null)

            => new InformProviderResponse(Result.Unknown(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHPdirect inform provider response.
        /// </summary>
        /// <param name="Result">A generic OHCP result.</param>
        public InformProviderResponse(Result  Result)
            : base(Result)
        { }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:InformProviderResponse>
        //
        //         <ns:result>
        //            <ns:resultCode>
        //               <ns:resultCode>?</ns:resultCode>
        //            </ns:resultCode>
        //            <ns:resultDescription>?</ns:resultDescription>
        //         </ns:result>
        //
        //      </ns:InformProviderResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(InformProviderResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHPdirect inform provider response.
        /// </summary>
        /// <param name="InformProviderResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static InformProviderResponse Parse(XElement             InformProviderResponseXML,
                                                   OnExceptionDelegate  OnException = null)
        {

            InformProviderResponse _InformProviderResponse;

            if (TryParse(InformProviderResponseXML, out _InformProviderResponse, OnException))
                return _InformProviderResponse;

            return null;

        }

        #endregion

        #region (static) Parse(InformProviderResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHPdirect inform provider response.
        /// </summary>
        /// <param name="InformProviderResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static InformProviderResponse Parse(String               InformProviderResponseText,
                                                   OnExceptionDelegate  OnException = null)
        {

            InformProviderResponse _InformProviderResponse;

            if (TryParse(InformProviderResponseText, out _InformProviderResponse, OnException))
                return _InformProviderResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(InformProviderResponseXML,  out InformProviderResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHPdirect inform provider response.
        /// </summary>
        /// <param name="InformProviderResponseXML">The XML to parse.</param>
        /// <param name="InformProviderResponse">The parsed inform provider response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                    InformProviderResponseXML,
                                       out InformProviderResponse  InformProviderResponse,
                                       OnExceptionDelegate         OnException  = null)
        {

            try
            {

                InformProviderResponse = new InformProviderResponse(

                                             InformProviderResponseXML.MapElementOrFail(OCHPNS.Default + "result",
                                                                                        Result.Parse,
                                                                                        OnException)

                                         );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, InformProviderResponseXML, e);

                InformProviderResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(InformProviderResponseText, out InformProviderResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHPdirect inform provider response.
        /// </summary>
        /// <param name="InformProviderResponseText">The text to parse.</param>
        /// <param name="InformProviderResponse">The parsed inform provider response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                      InformProviderResponseText,
                                       out InformProviderResponse  InformProviderResponse,
                                       OnExceptionDelegate         OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(InformProviderResponseText).Root,
                             out InformProviderResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, InformProviderResponseText, e);
            }

            InformProviderResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "InformProviderResponse",
                   new XElement(OCHPNS.Default + "result", Result.ToXML())
               );

        #endregion


        #region Operator overloading

        #region Operator == (InformProviderResponse1, InformProviderResponse2)

        /// <summary>
        /// Compares two inform provider responses for equality.
        /// </summary>
        /// <param name="InformProviderResponse1">An inform provider response.</param>
        /// <param name="InformProviderResponse2">Another inform provider response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (InformProviderResponse InformProviderResponse1, InformProviderResponse InformProviderResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(InformProviderResponse1, InformProviderResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) InformProviderResponse1 == null) || ((Object) InformProviderResponse2 == null))
                return false;

            return InformProviderResponse1.Equals(InformProviderResponse2);

        }

        #endregion

        #region Operator != (InformProviderResponse1, InformProviderResponse2)

        /// <summary>
        /// Compares two inform provider responses for inequality.
        /// </summary>
        /// <param name="InformProviderResponse1">An inform provider response.</param>
        /// <param name="InformProviderResponse2">Another inform provider response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (InformProviderResponse InformProviderResponse1, InformProviderResponse InformProviderResponse2)

            => !(InformProviderResponse1 == InformProviderResponse2);

        #endregion

        #endregion

        #region IEquatable<InformProviderResponse> Members

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

            // Check if the given object is an inform provider response.
            var InformProviderResponse = Object as InformProviderResponse;
            if ((Object) InformProviderResponse == null)
                return false;

            return this.Equals(InformProviderResponse);

        }

        #endregion

        #region Equals(InformProviderResponse)

        /// <summary>
        /// Compares two inform provider responses for equality.
        /// </summary>
        /// <param name="InformProviderResponse">An inform provider response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(InformProviderResponse InformProviderResponse)
        {

            if ((Object) InformProviderResponse == null)
                return false;

            return this.Result.Equals(InformProviderResponse.Result);

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
