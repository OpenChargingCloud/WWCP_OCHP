/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OCHPv1_4.CH
{

    /// <summary>
    /// An OCHP add service endpoints response.
    /// </summary>
    public class AddServiceEndpointsResponse : AResponse<AddServiceEndpointsRequest,
                                                         AddServiceEndpointsResponse>
    {

        #region Statics

        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Request">The add service endpoints request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddServiceEndpointsResponse OK(AddServiceEndpointsRequest  Request,
                                                     String                      Description = null)

            => new AddServiceEndpointsResponse(Request,
                                               Result.OK(Description));


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static AddServiceEndpointsResponse Partly(AddServiceEndpointsRequest  Request,
                                                         String                      Description = null)

            => new AddServiceEndpointsResponse(Request,
                                               Result.Partly(Description));


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Request">The add service endpoints request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddServiceEndpointsResponse NotAuthorized(AddServiceEndpointsRequest  Request,
                                                                String                      Description = null)

            => new AddServiceEndpointsResponse(Request,
                                               Result.NotAuthorized(Description));


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Request">The add service endpoints request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddServiceEndpointsResponse InvalidId(AddServiceEndpointsRequest  Request,
                                                            String                      Description = null)

            => new AddServiceEndpointsResponse(Request,
                                               Result.InvalidId(Description));


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Request">The add service endpoints request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddServiceEndpointsResponse Server(AddServiceEndpointsRequest  Request,
                                                         String                      Description = null)

            => new AddServiceEndpointsResponse(Request,
                                               Result.Server(Description));


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Request">The add service endpoints request leading to this response.</param>
        /// <param name="Description">A human-readable error description.</param>
        public static AddServiceEndpointsResponse Format(AddServiceEndpointsRequest  Request,
                                                         String                      Description = null)

            => new AddServiceEndpointsResponse(Request,
                                               Result.Format(Description));

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP add service endpoints response.
        /// </summary>
        /// <param name="Request">The add service endpoints request leading to this response.</param>
        /// <param name="Result">A generic OCHP result.</param>
        public AddServiceEndpointsResponse(AddServiceEndpointsRequest  Request,
                                           Result                      Result)

            : base(Request, Result)

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

        #region (static) Parse   (Request, AddServiceEndpointsResponseXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP add service endpoints response.
        /// </summary>
        /// <param name="Request">The add service endpoints request leading to this response.</param>
        /// <param name="AddServiceEndpointsResponseXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AddServiceEndpointsResponse Parse(AddServiceEndpointsRequest  Request,
                                                        XElement                    AddServiceEndpointsResponseXML,
                                                        OnExceptionDelegate         OnException = null)
        {

            AddServiceEndpointsResponse _AddServiceEndpointsResponse;

            if (TryParse(Request, AddServiceEndpointsResponseXML, out _AddServiceEndpointsResponse, OnException))
                return _AddServiceEndpointsResponse;

            return null;

        }

        #endregion

        #region (static) Parse   (Request, AddServiceEndpointsResponseText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP add service endpoints response.
        /// </summary>
        /// <param name="Request">The add service endpoints request leading to this response.</param>
        /// <param name="AddServiceEndpointsResponseText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AddServiceEndpointsResponse Parse(AddServiceEndpointsRequest  Request,
                                                        String                      AddServiceEndpointsResponseText,
                                                        OnExceptionDelegate         OnException = null)
        {

            AddServiceEndpointsResponse _AddServiceEndpointsResponse;

            if (TryParse(Request, AddServiceEndpointsResponseText, out _AddServiceEndpointsResponse, OnException))
                return _AddServiceEndpointsResponse;

            return null;

        }

        #endregion

        #region (static) TryParse(Request, AddServiceEndpointsResponseXML,  out AddServiceEndpointsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP add service endpoints response.
        /// </summary>
        /// <param name="Request">The add service endpoints request leading to this response.</param>
        /// <param name="AddServiceEndpointsResponseXML">The XML to parse.</param>
        /// <param name="AddServiceEndpointsResponse">The parsed add service endpoints response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(AddServiceEndpointsRequest       Request,
                                       XElement                         AddServiceEndpointsResponseXML,
                                       out AddServiceEndpointsResponse  AddServiceEndpointsResponse,
                                       OnExceptionDelegate              OnException  = null)
        {

            try
            {

                AddServiceEndpointsResponse = new AddServiceEndpointsResponse(

                                                  Request,

                                                  AddServiceEndpointsResponseXML.MapElementOrFail(OCHPNS.Default + "result",
                                                                                                  Result.Parse,
                                                                                                  OnException)

                                              );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, AddServiceEndpointsResponseXML, e);

                AddServiceEndpointsResponse = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(Request, AddServiceEndpointsResponseText, out AddServiceEndpointsResponse, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP add service endpoints response.
        /// </summary>
        /// <param name="Request">The add service endpoints request leading to this response.</param>
        /// <param name="AddServiceEndpointsResponseText">The text to parse.</param>
        /// <param name="AddServiceEndpointsResponse">The parsed add service endpoints response.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(AddServiceEndpointsRequest       Request,
                                       String                           AddServiceEndpointsResponseText,
                                       out AddServiceEndpointsResponse  AddServiceEndpointsResponse,
                                       OnExceptionDelegate              OnException  = null)
        {

            try
            {

                if (TryParse(Request,
                             XDocument.Parse(AddServiceEndpointsResponseText).Root,
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
        /// Compares two add service endpoints responses for equality.
        /// </summary>
        /// <param name="AddServiceEndpointsResponse1">A add service endpoints response.</param>
        /// <param name="AddServiceEndpointsResponse2">Another add service endpoints response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AddServiceEndpointsResponse AddServiceEndpointsResponse1, AddServiceEndpointsResponse AddServiceEndpointsResponse2)
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
        /// Compares two add service endpoints responses for inequality.
        /// </summary>
        /// <param name="AddServiceEndpointsResponse1">A add service endpoints response.</param>
        /// <param name="AddServiceEndpointsResponse2">Another add service endpoints response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AddServiceEndpointsResponse AddServiceEndpointsResponse1, AddServiceEndpointsResponse AddServiceEndpointsResponse2)

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

            // Check if the given object is a add service endpoints response.
            var AddServiceEndpointsResponse = Object as AddServiceEndpointsResponse;
            if ((Object) AddServiceEndpointsResponse == null)
                return false;

            return this.Equals(AddServiceEndpointsResponse);

        }

        #endregion

        #region Equals(AddServiceEndpointsResponse)

        /// <summary>
        /// Compares two add service endpoints responses for equality.
        /// </summary>
        /// <param name="AddServiceEndpointsResponse">A add service endpoints response to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AddServiceEndpointsResponse AddServiceEndpointsResponse)
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
