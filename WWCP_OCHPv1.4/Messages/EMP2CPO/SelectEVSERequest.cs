/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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

namespace cloud.charging.open.protocols.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHPdirect select EVSE request.
    /// </summary>
    public class SelectEVSERequest : ARequest<SelectEVSERequest>
    {

        #region Properties

        /// <summary>
        /// The unique identification of the selected EVSE.
        /// </summary>
        public EVSE_Id      EVSEId          { get; }

        /// <summary>
        /// The unique identification of an e-mobility contract.
        /// </summary>
        public Contract_Id  ContractId      { get; }

        /// <summary>
        /// An optional timestamp till when then given EVSE should be reserved.
        /// </summary>
        public DateTime?    ReserveUntil    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHPdirect SelectEVSE XML/SOAP request.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the selected EVSE.</param>
        /// <param name="ContractId">The unique identification of an e-mobility contract.</param>
        /// <param name="ReserveUntil">An optional timestamp till when then given EVSE should be reserved.</param>
        public SelectEVSERequest(EVSE_Id      EVSEId,
                                 Contract_Id  ContractId,
                                 DateTime?    ReserveUntil)
        {

            #region Initial checks

            if (ReserveUntil.HasValue && ReserveUntil.Value <= org.GraphDefined.Vanaheimr.Illias.Timestamp.Now)
                throw new ArgumentException("The given reservation end time must be after than the current time!");

            #endregion

            this.EVSEId        = EVSEId;
            this.ContractId    = ContractId;
            this.ReserveUntil  = ReserveUntil;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //      <ns:SelectEvseRequest>
        //
        //         <ns:evseId>?</ns:evseId>
        //         <ns:contractId>?</ns:contractId>
        //
        //         <!--Optional:-->
        //         <ns:reserveUntil>
        //            <ns:DateTime>?</ns:DateTime>
        //         </ns:reserveUntil>
        //
        //      </ns:SelectEvseRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(SelectEVSERequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHPdirect select EVSE request.
        /// </summary>
        /// <param name="SelectEVSERequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SelectEVSERequest Parse(XElement             SelectEVSERequestXML,
                                              OnExceptionDelegate  OnException = null)
        {

            SelectEVSERequest _SelectEVSERequest;

            if (TryParse(SelectEVSERequestXML, out _SelectEVSERequest, OnException))
                return _SelectEVSERequest;

            return null;

        }

        #endregion

        #region (static) Parse(SelectEVSERequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHPdirect select EVSE request.
        /// </summary>
        /// <param name="SelectEVSERequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static SelectEVSERequest Parse(String               SelectEVSERequestText,
                                              OnExceptionDelegate  OnException = null)
        {

            SelectEVSERequest _SelectEVSERequest;

            if (TryParse(SelectEVSERequestText, out _SelectEVSERequest, OnException))
                return _SelectEVSERequest;

            return null;

        }

        #endregion

        #region (static) TryParse(SelectEVSERequestXML,  out SelectEVSERequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHPdirect select EVSE request.
        /// </summary>
        /// <param name="SelectEVSERequestXML">The XML to parse.</param>
        /// <param name="SelectEVSERequest">The parsed select EVSE request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement               SelectEVSERequestXML,
                                       out SelectEVSERequest  SelectEVSERequest,
                                       OnExceptionDelegate    OnException  = null)
        {

            try
            {

                SelectEVSERequest = new SelectEVSERequest(

                                        SelectEVSERequestXML.MapValueOrFail(OCHPNS.Default + "evseId",
                                                                            EVSE_Id.Parse),

                                        SelectEVSERequestXML.MapValueOrFail(OCHPNS.Default + "contractId",
                                                                            Contract_Id.Parse),

                                        SelectEVSERequestXML.MapValueOrNullable(OCHPNS.Default + "reserveUntil",
                                                                                OCHPNS.Default + "DateTime",
                                                                                DateTime.Parse)

                                    );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SelectEVSERequestXML, e);

                SelectEVSERequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(SelectEVSERequestText, out SelectEVSERequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHPdirect select EVSE request.
        /// </summary>
        /// <param name="SelectEVSERequestText">The text to parse.</param>
        /// <param name="SelectEVSERequest">The parsed select EVSE request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                 SelectEVSERequestText,
                                       out SelectEVSERequest  SelectEVSERequest,
                                       OnExceptionDelegate    OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(SelectEVSERequestText).Root,
                             out SelectEVSERequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, SelectEVSERequestText, e);
            }

            SelectEVSERequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "SelectEvseRequest",

                                      new XElement(OCHPNS.Default + "evseId",      EVSEId.    ToString()),
                                      new XElement(OCHPNS.Default + "contractId",  ContractId.ToString()),

                                      ReserveUntil.HasValue
                                          ? new XElement(OCHPNS.Default + "reserveUntil",
                                                new XElement(OCHPNS.Default + "DateTime",  ReserveUntil.Value.ToIso8601())
                                            )
                                          : null

                                 ));

        #endregion


        #region Operator overloading

        #region Operator == (SelectEVSERequest1, SelectEVSERequest2)

        /// <summary>
        /// Compares two select EVSE requests for equality.
        /// </summary>
        /// <param name="SelectEVSERequest1">A select EVSE request.</param>
        /// <param name="SelectEVSERequest2">Another select EVSE request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SelectEVSERequest SelectEVSERequest1, SelectEVSERequest SelectEVSERequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SelectEVSERequest1, SelectEVSERequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) SelectEVSERequest1 == null) || ((Object) SelectEVSERequest2 == null))
                return false;

            return SelectEVSERequest1.Equals(SelectEVSERequest2);

        }

        #endregion

        #region Operator != (SelectEVSERequest1, SelectEVSERequest2)

        /// <summary>
        /// Compares two select EVSE requests for inequality.
        /// </summary>
        /// <param name="SelectEVSERequest1">A select EVSE request.</param>
        /// <param name="SelectEVSERequest2">Another select EVSE request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SelectEVSERequest SelectEVSERequest1, SelectEVSERequest SelectEVSERequest2)

            => !(SelectEVSERequest1 == SelectEVSERequest2);

        #endregion

        #endregion

        #region IEquatable<SelectEVSERequest> Members

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

            // Check if the given object is a select EVSE request.
            var SelectEVSERequest = Object as SelectEVSERequest;
            if ((Object) SelectEVSERequest == null)
                return false;

            return this.Equals(SelectEVSERequest);

        }

        #endregion

        #region Equals(SelectEVSERequest)

        /// <summary>
        /// Compares two select EVSE requests for equality.
        /// </summary>
        /// <param name="SelectEVSERequest">A select EVSE request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(SelectEVSERequest SelectEVSERequest)
        {

            if ((Object) SelectEVSERequest == null)
                return false;

            return EVSEId.    Equals(SelectEVSERequest.EVSEId)     &&
                   ContractId.Equals(SelectEVSERequest.ContractId) &&

                   ((!ReserveUntil.HasValue && !SelectEVSERequest.ReserveUntil.HasValue) ||
                    ((ReserveUntil.HasValue && SelectEVSERequest.ReserveUntil.HasValue)
                          ? ReserveUntil.Value.Equals(SelectEVSERequest.ReserveUntil)
                          : false));

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => EVSEId.    GetHashCode() * 17 ^
               ContractId.GetHashCode() * 11 ^

               (ReserveUntil.HasValue
                    ? ReserveUntil.GetHashCode()
                    : 0);

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EVSEId,
                             " / ",
                             ContractId,

                             ReserveUntil.HasValue
                                 ? " reserved till " + ReserveUntil.Value.ToIso8601()
                                 : "");

        #endregion

    }

}
