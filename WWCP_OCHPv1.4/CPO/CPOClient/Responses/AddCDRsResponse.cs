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
    /// An OCHP add charge details record response.
    /// </summary>
    public class AddCDRsResponse : AResponse
    {

        #region Properties

        /// <summary>
        /// An enumeration of refused charge detail records.
        /// </summary>
        public IEnumerable<CDRInfo>  ImplausibleCDRs   { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse OK(String Description = null)

            => new AddCDRsResponse(Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse Partly(String Description = null)

            => new AddCDRsResponse(Result.Unknown(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse NotAuthorized(String Description = null)

            => new AddCDRsResponse(Result.Unknown(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse InvalidId(String Description = null)

            => new AddCDRsResponse(Result.Unknown(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse Server(String Description = null)

            => new AddCDRsResponse(Result.Unknown(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse Format(String Description = null)

            => new AddCDRsResponse(Result.Unknown(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP add charge details record response.
        /// </summary>
        /// <param name="Result">A generic OHCP result.</param>
        /// <param name="ImplausibleCDRs">An enumeration of refused charge detail records.</param>
        public AddCDRsResponse(Result                Result,
                               IEnumerable<CDRInfo>  ImplausibleCDRs = null)

            : base(Result)

        {

            this.ImplausibleCDRs  = ImplausibleCDRs;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:AddCDRsResponse>
        //
        //         <ns:result>
        //            <ns:resultCode>
        //               <ns:resultCode>?</ns:resultCode>
        //            </ns:resultCode>
        //            <ns:resultDescription>?</ns:resultDescription>
        //         </ns:result>
        //
        //         <!--Zero or more repetitions:-->
        //         <ns:implausibleCdrsArray>?</ns:implausibleCdrsArray>
        //
        //      </ns:AddCDRsResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(AddCDRsResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP add charge details record response.
        /// </summary>
        /// <param name="AddCDRsResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AddCDRsResponse Parse(XElement             AddCDRsResponseXML,
                                            OnExceptionDelegate  OnException = null)
        {

            AddCDRsResponse _AddCDRsResponse;

            if (TryParse(AddCDRsResponseXML, out _AddCDRsResponse, OnException))
                return _AddCDRsResponse;

            return null;

        }

        #endregion

        #region (static) Parse(AddCDRsResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP add charge details record response.
        /// </summary>
        /// <param name="AddCDRsResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AddCDRsResponse Parse(String               AddCDRsResponseText,
                                            OnExceptionDelegate  OnException = null)
        {

            AddCDRsResponse _AddCDRsResponse;

            if (TryParse(AddCDRsResponseText, out _AddCDRsResponse, OnException))
                return _AddCDRsResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(AddCDRsResponseXML,  out AddCDRsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP add charge details record response.
        /// </summary>
        /// <param name="AddCDRsResponseXML">The XML to parse.</param>
        /// <param name="AddCDRsResponse">The parsed add charge details record response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             AddCDRsResponseXML,
                                       out AddCDRsResponse  AddCDRsResponse,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                AddCDRsResponse = new AddCDRsResponse(

                                      AddCDRsResponseXML.MapElementOrFail(OCHPNS.Default + "result",
                                                                          Result.Parse,
                                                                          OnException),

                                      AddCDRsResponseXML.MapElements     (OCHPNS.Default + "implausibleCdrsArray",
                                                                          CDRInfo.Parse,
                                                                          OnException)

                                  );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, AddCDRsResponseXML, e);

                AddCDRsResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(AddCDRsResponseText, out AddCDRsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP add charge details record response.
        /// </summary>
        /// <param name="AddCDRsResponseText">The text to parse.</param>
        /// <param name="AddCDRsResponse">The parsed add charge details record response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               AddCDRsResponseText,
                                       out AddCDRsResponse  AddCDRsResponse,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(AddCDRsResponseText).Root,
                             out AddCDRsResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, AddCDRsResponseText, e);
            }

            AddCDRsResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "AddCDRsResponse",

                   new XElement(OCHPNS.Default + "result", Result.ToXML()),

                   ImplausibleCDRs.Select(cdr => cdr.ToXML(OCHPNS.Default + "implausibleCdrsArray"))

               );

        #endregion


        #region Operator overloading

        #region Operator == (AddCDRsResponse1, AddCDRsResponse2)

        /// <summary>
        /// Compares two add charge details record responses for equality.
        /// </summary>
        /// <param name="AddCDRsResponse1">A add charge details record response.</param>
        /// <param name="AddCDRsResponse2">Another add charge details record response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AddCDRsResponse AddCDRsResponse1, AddCDRsResponse AddCDRsResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(AddCDRsResponse1, AddCDRsResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AddCDRsResponse1 == null) || ((Object) AddCDRsResponse2 == null))
                return false;

            return AddCDRsResponse1.Equals(AddCDRsResponse2);

        }

        #endregion

        #region Operator != (AddCDRsResponse1, AddCDRsResponse2)

        /// <summary>
        /// Compares two add charge details record responses for inequality.
        /// </summary>
        /// <param name="AddCDRsResponse1">A add charge details record response.</param>
        /// <param name="AddCDRsResponse2">Another add charge details record response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AddCDRsResponse AddCDRsResponse1, AddCDRsResponse AddCDRsResponse2)

            => !(AddCDRsResponse1 == AddCDRsResponse2);

        #endregion

        #endregion

        #region IEquatable<AddCDRsResponse> Members

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

            // Check if the given object is a add charge details record response.
            var AddCDRsResponse = Object as AddCDRsResponse;
            if ((Object) AddCDRsResponse == null)
                return false;

            return this.Equals(AddCDRsResponse);

        }

        #endregion

        #region Equals(AddCDRsResponse)

        /// <summary>
        /// Compares two add charge details record responses for equality.
        /// </summary>
        /// <param name="AddCDRsResponse">A add charge details record response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(AddCDRsResponse AddCDRsResponse)
        {

            if ((Object) AddCDRsResponse == null)
                return false;

            return this.Result.Equals(AddCDRsResponse.Result);

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

                return ImplausibleCDRs != null

                           ? Result.GetHashCode() * 11 ^
                             ImplausibleCDRs.SafeSelect(cdr => cdr.GetHashCode()).Aggregate((a, b) => a ^ b)

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
                             ImplausibleCDRs.Any()
                                 ? " " + ImplausibleCDRs.Count() + " refused charge detail records"
                                 : "");

        #endregion

    }

}
