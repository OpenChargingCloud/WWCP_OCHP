/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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
using System.Threading;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    /// <summary>
    /// An OCHP add charge detail records request.
    /// </summary>
    public class AddCDRsRequest : ARequest<AddCDRsRequest>
    {

        #region Properties

        /// <summary>
        /// An enumeration of charge detail records.
        /// </summary>
        public IEnumerable<CDRInfo>  CDRInfos   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP AddCDRs XML/SOAP request.
        /// </summary>
        /// <param name="CDRInfos">An enumeration of charge detail records.</param>
        /// 
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        public AddCDRsRequest(IEnumerable<CDRInfo>  CDRInfos,

                              DateTime?             Timestamp           = null,
                              CancellationToken?    CancellationToken   = null,
                              EventTracking_Id      EventTrackingId     = null,
                              TimeSpan?             RequestTimeout      = null)

            : base(Timestamp,
                   CancellationToken,
                   EventTrackingId,
                   RequestTimeout)

        {

            #region Initial checks

            if (CDRInfos.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(CDRInfos), "The given enumeration of charge detail records must not be null or empty!");

            #endregion

            this.CDRInfos  = CDRInfos;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:OCHP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <OCHP:AddCDRsRequest>
        //
        //         <!--1 or more repetitions:-->
        //         <OCHP:cdrInfoArray>
        //           ...
        //         </OCHP:cdrInfoArray>
        //
        //      </OCHP:AddCDRsRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(AddCDRsRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP add charge detail records request.
        /// </summary>
        /// <param name="AddCDRsRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AddCDRsRequest Parse(XElement             AddCDRsRequestXML,
                                           OnExceptionDelegate  OnException = null)
        {

            AddCDRsRequest _AddCDRsRequest;

            if (TryParse(AddCDRsRequestXML, out _AddCDRsRequest, OnException))
                return _AddCDRsRequest;

            return null;

        }

        #endregion

        #region (static) Parse(AddCDRsRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP add charge detail records request.
        /// </summary>
        /// <param name="AddCDRsRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static AddCDRsRequest Parse(String               AddCDRsRequestText,
                                           OnExceptionDelegate  OnException = null)
        {

            AddCDRsRequest _AddCDRsRequest;

            if (TryParse(AddCDRsRequestText, out _AddCDRsRequest, OnException))
                return _AddCDRsRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(AddCDRsRequestXML,  out AddCDRsRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP add charge detail records request.
        /// </summary>
        /// <param name="AddCDRsRequestXML">The XML to parse.</param>
        /// <param name="AddCDRsRequest">The parsed add charge detail records request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             AddCDRsRequestXML,
                                       out AddCDRsRequest   AddCDRsRequest,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                AddCDRsRequest = new AddCDRsRequest(

                                     AddCDRsRequestXML.MapElementsOrFail(OCHPNS.Default + "cdrInfoArray",
                                                                         CDRInfo.Parse,
                                                                         OnException)

                                 );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, AddCDRsRequestXML, e);

                AddCDRsRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(AddCDRsRequestText, out AddCDRsRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP add charge detail records request.
        /// </summary>
        /// <param name="AddCDRsRequestText">The text to parse.</param>
        /// <param name="AddCDRsRequest">The parsed add charge detail records request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               AddCDRsRequestText,
                                       out AddCDRsRequest   AddCDRsRequest,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(AddCDRsRequestText).Root,
                             out AddCDRsRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, AddCDRsRequestText, e);
            }

            AddCDRsRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "AddCDRsRequest",

                                CDRInfos.SafeSelect(cdr => cdr.ToXML(OCHPNS.Default + "cdrInfoArray"))

                           );

        #endregion


        #region Operator overloading

        #region Operator == (AddCDRsRequest1, AddCDRsRequest2)

        /// <summary>
        /// Compares two add charge detail records requests for equality.
        /// </summary>
        /// <param name="AddCDRsRequest1">A add charge detail records request.</param>
        /// <param name="AddCDRsRequest2">Another add charge detail records request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (AddCDRsRequest AddCDRsRequest1, AddCDRsRequest AddCDRsRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(AddCDRsRequest1, AddCDRsRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) AddCDRsRequest1 == null) || ((Object) AddCDRsRequest2 == null))
                return false;

            return AddCDRsRequest1.Equals(AddCDRsRequest2);

        }

        #endregion

        #region Operator != (AddCDRsRequest1, AddCDRsRequest2)

        /// <summary>
        /// Compares two add charge detail records requests for inequality.
        /// </summary>
        /// <param name="AddCDRsRequest1">A add charge detail records request.</param>
        /// <param name="AddCDRsRequest2">Another add charge detail records request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (AddCDRsRequest AddCDRsRequest1, AddCDRsRequest AddCDRsRequest2)

            => !(AddCDRsRequest1 == AddCDRsRequest2);

        #endregion

        #endregion

        #region IEquatable<AddCDRsRequest> Members

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

            // Check if the given object is a add charge detail records request.
            var AddCDRsRequest = Object as AddCDRsRequest;
            if ((Object) AddCDRsRequest == null)
                return false;

            return this.Equals(AddCDRsRequest);

        }

        #endregion

        #region Equals(AddCDRsRequest)

        /// <summary>
        /// Compares two add charge detail records requests for equality.
        /// </summary>
        /// <param name="AddCDRsRequest">A add charge detail records request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(AddCDRsRequest AddCDRsRequest)
        {

            if ((Object) AddCDRsRequest == null)
                return false;

            return CDRInfos.Count().Equals(AddCDRsRequest.CDRInfos.Count());

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

                return CDRInfos.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(CDRInfos.Count(), " charge detail record(s)");

        #endregion


    }

}
