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
    /// An OCHP update tariffs request.
    /// </summary>
    public class UpdateTariffsRequest : ARequest<UpdateTariffsRequest>
    {

        #region Properties

    /// <summary>
    /// An enumeration of tariff infos.
    /// </summary>
    public IEnumerable<TariffInfo>  TariffInfos   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHP UpdateTariffs XML/SOAP request.
        /// </summary>
        /// <param name="TariffInfos">An enumeration of tariff infos.</param>
        public UpdateTariffsRequest(IEnumerable<TariffInfo>  TariffInfos)
        {

            #region Initial checks

            if (TariffInfos.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(TariffInfos),  "The given enumeration of tariff infos must not be null!");

            #endregion

            this.TariffInfos = TariffInfos ?? new TariffInfo[0];

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:OCHP    = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:UpdateTariffsRequest>
        //
        //          <!--1 or more repetitions:-->
        //          <ns:TariffInfoArray>
        //
        //             <ns:tariffId>?</ns:tariffId>
        //
        //             <!--1 or more repetitions:-->
        //             <ns:individualTariff>
        //
        //                <!--1 or more repetitions:-->
        //                <ns:tariffElement>
        //
        //                   <ns:priceComponent>
        //
        //                      <ns:billingItem>
        //                         <ns:BillingItemType>?</ns:BillingItemType>
        //                      </ns:billingItem>
        //
        //                      <ns:itemPrice>?</ns:itemPrice>
        //                      <ns:stepSize>?</ns:stepSize>
        //
        //                   </ns:priceComponent>
        //
        //                   <ns:tariffRestriction>
        //
        //                      <!--0 to 14 repetitions:-->
        //                      <ns:regularHours weekday="?" periodBegin="?" periodEnd="?"/>
        //
        //                      <!--Optional:-->
        //                      <ns:startDateTime>
        //                         <ns:DateTime>?</ns:DateTime>
        //                      </ns:startDateTime>
        //
        //                      <!--Optional:-->
        //                      <ns:endDateTime>
        //                         <ns:DateTime>?</ns:DateTime>
        //                      </ns:endDateTime>
        //
        //                      <ns:minEnergy>?</ns:minEnergy>
        //                      <ns:maxEnergy>?</ns:maxEnergy>
        //                      <ns:minPower>?</ns:minPower>
        //                      <ns:maxPower>?</ns:maxPower>
        //                      <ns:minDuration>?</ns:minDuration>
        //                      <ns:maxDuration>?</ns:maxDuration>
        //
        //                   </ns:tariffRestriction>
        //
        //                </ns:tariffElement>
        //
        //                <!--Zero or more repetitions:-->
        //                <ns:recipient>?</ns:recipient>
        //
        //                <ns:currency>?</ns:currency>
        //
        //             </ns:individualTariff>
        //
        //          </ns:TariffInfoArray>
        //
        //       </ns:UpdateTariffsRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(UpdateTariffsRequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP update tariffs request.
        /// </summary>
        /// <param name="UpdateTariffsRequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateTariffsRequest Parse(XElement             UpdateTariffsRequestXML,
                                                 OnExceptionDelegate  OnException = null)
        {

            UpdateTariffsRequest _UpdateTariffsRequest;

            if (TryParse(UpdateTariffsRequestXML, out _UpdateTariffsRequest, OnException))
                return _UpdateTariffsRequest;

            return null;

        }

        #endregion

        #region (static) Parse(UpdateTariffsRequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP update tariffs request.
        /// </summary>
        /// <param name="UpdateTariffsRequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static UpdateTariffsRequest Parse(String               UpdateTariffsRequestText,
                                                 OnExceptionDelegate  OnException = null)
        {

            UpdateTariffsRequest _UpdateTariffsRequest;

            if (TryParse(UpdateTariffsRequestText, out _UpdateTariffsRequest, OnException))
                return _UpdateTariffsRequest;

            return null;

        }

        #endregion

        #region (static) TryParse(UpdateTariffsRequestXML,  out UpdateTariffsRequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP update tariffs request.
        /// </summary>
        /// <param name="UpdateTariffsRequestXML">The XML to parse.</param>
        /// <param name="UpdateTariffsRequest">The parsed update tariffs request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                  UpdateTariffsRequestXML,
                                       out UpdateTariffsRequest  UpdateTariffsRequest,
                                       OnExceptionDelegate       OnException  = null)
        {

            try
            {

                UpdateTariffsRequest = new UpdateTariffsRequest(

                                           UpdateTariffsRequestXML.MapElements(OCHPNS.Default + "TariffInfoArray",
                                                                               TariffInfo.Parse,
                                                                               OnException)

                                       );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, UpdateTariffsRequestXML, e);

                UpdateTariffsRequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(UpdateTariffsRequestText, out UpdateTariffsRequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP update tariffs request.
        /// </summary>
        /// <param name="UpdateTariffsRequestText">The text to parse.</param>
        /// <param name="UpdateTariffsRequest">The parsed update tariffs request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                    UpdateTariffsRequestText,
                                       out UpdateTariffsRequest  UpdateTariffsRequest,
                                       OnExceptionDelegate       OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(UpdateTariffsRequestText).Root,
                             out UpdateTariffsRequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, UpdateTariffsRequestText, e);
            }

            UpdateTariffsRequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "UpdateTariffsRequest",

                                TariffInfos.SafeSelect(tariffinfo => tariffinfo.ToXML(OCHPNS.Default + "TariffInfoArray"))

                           );

        #endregion


        #region Operator overloading

        #region Operator == (UpdateTariffsRequest1, UpdateTariffsRequest2)

        /// <summary>
        /// Compares two update tariffs requests for equality.
        /// </summary>
        /// <param name="UpdateTariffsRequest1">A update tariffs request.</param>
        /// <param name="UpdateTariffsRequest2">Another update tariffs request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (UpdateTariffsRequest UpdateTariffsRequest1, UpdateTariffsRequest UpdateTariffsRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(UpdateTariffsRequest1, UpdateTariffsRequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) UpdateTariffsRequest1 == null) || ((Object) UpdateTariffsRequest2 == null))
                return false;

            return UpdateTariffsRequest1.Equals(UpdateTariffsRequest2);

        }

        #endregion

        #region Operator != (UpdateTariffsRequest1, UpdateTariffsRequest2)

        /// <summary>
        /// Compares two update tariffs requests for inequality.
        /// </summary>
        /// <param name="UpdateTariffsRequest1">A update tariffs request.</param>
        /// <param name="UpdateTariffsRequest2">Another update tariffs request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (UpdateTariffsRequest UpdateTariffsRequest1, UpdateTariffsRequest UpdateTariffsRequest2)

            => !(UpdateTariffsRequest1 == UpdateTariffsRequest2);

        #endregion

        #endregion

        #region IEquatable<UpdateTariffsRequest> Members

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

            // Check if the given object is a update tariffs request.
            var UpdateTariffsRequest = Object as UpdateTariffsRequest;
            if ((Object) UpdateTariffsRequest == null)
                return false;

            return this.Equals(UpdateTariffsRequest);

        }

        #endregion

        #region Equals(UpdateTariffsRequest)

        /// <summary>
        /// Compares two update tariffs requests for equality.
        /// </summary>
        /// <param name="UpdateTariffsRequest">A update tariffs request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(UpdateTariffsRequest UpdateTariffsRequest)
        {

            if ((Object) UpdateTariffsRequest == null)
                return false;

            return TariffInfos.Count().Equals(UpdateTariffsRequest.TariffInfos.Count());

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

                return TariffInfos.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => TariffInfos.Count() + " tariff info(s)";

        #endregion


    }

}
