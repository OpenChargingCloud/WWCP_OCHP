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
    /// An OCHP add operator service endpoints response.
    /// </summary>
    public class AddOperatorServiceEndpointsResponse : AResponse
    {

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static AddOperatorServiceEndpointsResponse OK(String Description = null)

            => new AddOperatorServiceEndpointsResponse(Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static AddOperatorServiceEndpointsResponse Partly(String Description = null)

            => new AddOperatorServiceEndpointsResponse(Result.Unknown(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static AddOperatorServiceEndpointsResponse NotAuthorized(String Description = null)

            => new AddOperatorServiceEndpointsResponse(Result.Unknown(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static AddOperatorServiceEndpointsResponse InvalidId(String Description = null)

            => new AddOperatorServiceEndpointsResponse(Result.Unknown(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static AddOperatorServiceEndpointsResponse Server(String Description = null)

            => new AddOperatorServiceEndpointsResponse(Result.Unknown(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static AddOperatorServiceEndpointsResponse Format(String Description = null)

            => new AddOperatorServiceEndpointsResponse(Result.Unknown(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP add operator service endpoints response.
        /// </summary>
        /// <param name="Result">A generic OHCP result.</param>
        public AddOperatorServiceEndpointsResponse(Result  Result)
            : base(Result)
        { }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:AddServiceEndpointsResponse>
        //
        //         <ns:result>
        //            <ns:resultCode>
        //               <ns:resultCode>?</ns:resultCode>
        //            </ns:resultCode>
        //            <ns:resultDescription>?</ns:resultDescription>
        //         </ns:result>
        //
        //      </ns:AddServiceEndpointsResponse>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(AddOperatorServiceEndpointsResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP add operator service endpoints response.
        /// </summary>
        /// <param name="AddOperatorServiceEndpointsResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AddOperatorServiceEndpointsResponse Parse(XElement             AddOperatorServiceEndpointsResponseXML,
                                                                OnExceptionDelegate  OnException = null)
        {

            AddOperatorServiceEndpointsResponse _AddServiceEndpointsResponse;

            if (TryParse(AddOperatorServiceEndpointsResponseXML, out _AddServiceEndpointsResponse, OnException))
                return _AddServiceEndpointsResponse;

            return null;

        }

        #endregion

        #region (static) Parse(AddServiceEndpointsResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP add operator service endpoints response.
        /// </summary>
        /// <param name="AddServiceEndpointsResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AddOperatorServiceEndpointsResponse Parse(String               AddServiceEndpointsResponseText,
                                                                OnExceptionDelegate  OnException = null)
        {

            AddOperatorServiceEndpointsResponse _AddServiceEndpointsResponse;

            if (TryParse(AddServiceEndpointsResponseText, out _AddServiceEndpointsResponse, OnException))
                return _AddServiceEndpointsResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(AddOperatorServiceEndpointsResponseXML,  out AddServiceEndpointsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP add operator service endpoints response.
        /// </summary>
        /// <param name="AddOperatorServiceEndpointsResponseXML">The XML to parse.</param>
        /// <param name="AddServiceEndpointsResponse">The parsed add operator service endpoints response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                                 AddOperatorServiceEndpointsResponseXML,
                                       out AddOperatorServiceEndpointsResponse  AddServiceEndpointsResponse,
                                       OnExceptionDelegate                      OnException  = null)
        {

            try
            {

                AddServiceEndpointsResponse = new AddOperatorServiceEndpointsResponse(

                                                  AddOperatorServiceEndpointsResponseXML.MapElementOrFail(OCHPNS.Default + "result",
                                                                                                  Result.Parse,
                                                                                                  OnException)

                                              );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, AddOperatorServiceEndpointsResponseXML, e);

                AddServiceEndpointsResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(AddServiceEndpointsResponseText, out AddServiceEndpointsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP add operator service endpoints response.
        /// </summary>
        /// <param name="AddServiceEndpointsResponseText">The text to parse.</param>
        /// <param name="AddServiceEndpointsResponse">The parsed add operator service endpoints response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                                   AddServiceEndpointsResponseText,
                                       out AddOperatorServiceEndpointsResponse  AddServiceEndpointsResponse,
                                       OnExceptionDelegate                      OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(AddServiceEndpointsResponseText).Root,
                             out AddServiceEndpointsResponse,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, AddServiceEndpointsResponseText, e);
            }

            AddServiceEndpointsResponse = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "AddServiceEndpointsResponse",
                   new XElement(OCHPNS.Default + "result", Result.ToXML())
               );

        #endregion


        #region Operator overloading

        #region Operator == (AddServiceEndpointsResponse1, AddServiceEndpointsResponse2)

        /// <summary>
        /// Compares two add operator service endpoints responses for equality.
        /// </summary>
        /// <param name="AddServiceEndpointsResponse1">A add operator service endpoints response.</param>
        /// <param name="AddServiceEndpointsResponse2">Another add operator service endpoints response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AddOperatorServiceEndpointsResponse AddServiceEndpointsResponse1, AddOperatorServiceEndpointsResponse AddServiceEndpointsResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(AddServiceEndpointsResponse1, AddServiceEndpointsResponse2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AddServiceEndpointsResponse1 == null) || ((Object) AddServiceEndpointsResponse2 == null))
                return false;

            return AddServiceEndpointsResponse1.Equals(AddServiceEndpointsResponse2);

        }

        #endregion

        #region Operator != (AddServiceEndpointsResponse1, AddServiceEndpointsResponse2)

        /// <summary>
        /// Compares two add operator service endpoints responses for inequality.
        /// </summary>
        /// <param name="AddServiceEndpointsResponse1">A add operator service endpoints response.</param>
        /// <param name="AddServiceEndpointsResponse2">Another add operator service endpoints response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AddOperatorServiceEndpointsResponse AddServiceEndpointsResponse1, AddOperatorServiceEndpointsResponse AddServiceEndpointsResponse2)

            => !(AddServiceEndpointsResponse1 == AddServiceEndpointsResponse2);

        #endregion

        #endregion

        #region IEquatable<AddServiceEndpointsResponse> Members

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

            // Check if the given object is a add operator service endpoints response.
            var AddServiceEndpointsResponse = Object as AddOperatorServiceEndpointsResponse;
            if ((Object) AddServiceEndpointsResponse == null)
                return false;

            return this.Equals(AddServiceEndpointsResponse);

        }

        #endregion

        #region Equals(AddServiceEndpointsResponse)

        /// <summary>
        /// Compares two add operator service endpoints responses for equality.
        /// </summary>
        /// <param name="AddServiceEndpointsResponse">A add operator service endpoints response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(AddOperatorServiceEndpointsResponse AddServiceEndpointsResponse)
        {

            if ((Object) AddServiceEndpointsResponse == null)
                return false;

            return this.Result.Equals(AddServiceEndpointsResponse.Result);

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
