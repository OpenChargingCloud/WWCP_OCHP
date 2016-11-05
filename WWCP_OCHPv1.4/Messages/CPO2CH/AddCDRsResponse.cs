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
    /// An OCHP add charge details records response.
    /// </summary>
    public class AddCDRsResponse : AResponse<AddCDRsRequest,
                                             AddCDRsResponse>
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
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse OK(AddCDRsRequest  Request,
                                         String          Description = null)

            => new AddCDRsResponse(Request,
                                   Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse Partly(AddCDRsRequest  Request,
                                             String          Description = null)

            => new AddCDRsResponse(Request,
                                   Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse NotAuthorized(AddCDRsRequest  Request,
                                                    String          Description = null)

            => new AddCDRsResponse(Request,
                                   Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse InvalidId(AddCDRsRequest  Request,
                                                String          Description = null)

            => new AddCDRsResponse(Request,
                                   Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse Server(AddCDRsRequest  Request,
                                             String          Description = null)

            => new AddCDRsResponse(Request,
                                   Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddCDRsResponse Format(AddCDRsRequest  Request,
                                             String          Description = null)

            => new AddCDRsResponse(Request,
                                   Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP add charge details records response.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        /// <param name="ImplausibleCDRs">An enumeration of refused charge detail records.</param>
        public AddCDRsResponse(AddCDRsRequest        Request,
                               Result                Result,
                               IEnumerable<CDRInfo>  ImplausibleCDRs = null)

            : base(Request, Result)

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

        #region (static) Parse   (Request, AddCDRsResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP add charge details records response.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="AddCDRsResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AddCDRsResponse Parse(AddCDRsRequest       Request,
                                            XElement             AddCDRsResponseXML,
                                            OnExceptionDelegate  OnException = null)
        {

            AddCDRsResponse _AddCDRsResponse;

            if (TryParse(Request, AddCDRsResponseXML, out _AddCDRsResponse, OnException))
                return _AddCDRsResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, AddCDRsResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP add charge details records response.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="AddCDRsResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AddCDRsResponse Parse(AddCDRsRequest       Request,
                                            String               AddCDRsResponseText,
                                            OnExceptionDelegate  OnException = null)
        {

            AddCDRsResponse _AddCDRsResponse;

            if (TryParse(Request, AddCDRsResponseText, out _AddCDRsResponse, OnException))
                return _AddCDRsResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, AddCDRsResponseXML,  out AddCDRsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP add charge details records response.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="AddCDRsResponseXML">The XML to parse.</param>
        /// <param name="AddCDRsResponse">The parsed add charge details records response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(AddCDRsRequest       Request,
                                       XElement             AddCDRsResponseXML,
                                       out AddCDRsResponse  AddCDRsResponse,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                AddCDRsResponse = new AddCDRsResponse(

                                      Request,

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

        #region (static) TryParse(Request, AddCDRsResponseText, out AddCDRsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP add charge details records response.
        /// </summary>
        /// <param name="Request">The add charge details records request leading to this response.</param>
        /// <param name="AddCDRsResponseText">The text to parse.</param>
        /// <param name="AddCDRsResponse">The parsed add charge details records response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(AddCDRsRequest       Request,
                                       String               AddCDRsResponseText,
                                       out AddCDRsResponse  AddCDRsResponse,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(AddCDRsResponseText).Root,
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
        /// Compares two add charge details records responses for equality.
        /// </summary>
        /// <param name="AddCDRsResponse1">A add charge details records response.</param>
        /// <param name="AddCDRsResponse2">Another add charge details records response.</param>
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
        /// Compares two add charge details records responses for inequality.
        /// </summary>
        /// <param name="AddCDRsResponse1">A add charge details records response.</param>
        /// <param name="AddCDRsResponse2">Another add charge details records response.</param>
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

            // Check if the given object is a add charge details records response.
            var AddCDRsResponse = Object as AddCDRsResponse;
            if ((Object) AddCDRsResponse == null)
                return false;

            return this.Equals(AddCDRsResponse);

        }

        #endregion

        #region Equals(AddCDRsResponse)

        /// <summary>
        /// Compares two add charge details records responses for equality.
        /// </summary>
        /// <param name="AddCDRsResponse">A add charge details records response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AddCDRsResponse AddCDRsResponse)
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
