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
    /// OCHP CPO Client XML methods.
    /// </summary>
    public static class CPOClientXMLMethods
    {

        // OCHP

        #region SetChargePointListRequestXML   (ChargePointInfos)

        /// <summary>
        /// Create an OCHP SetChargePointList XML/SOAP request.
        /// </summary>
        /// <param name="ChargePointInfos">An enumeration of charge point infos.</param>
        public static XElement SetChargePointListRequestXML(IEnumerable<ChargePointInfo>  ChargePointInfos)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <OCHP:SetChargePointListRequest>
            //
            //          <!--1 or more repetitions:-->
            //          <OCHP:chargePointInfoArray>
            //             ...
            //          </OCHP:chargePointInfoArray>
            //
            //       </OCHP:SetChargePointListRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (ChargePointInfos == null || !ChargePointInfos.Any())
                throw new ArgumentNullException(nameof(ChargePointInfos),  "The given enumeration of charge point infos must not be null or empty!");

            #endregion


            return SOAP.Encapsulation(new XElement(OCHPNS.Default + "SetChargePointListRequest",

                                          ChargePointInfos.Select(chargepointinfo => chargepointinfo.ToXML()).
                                                           ToArray()

                                     ));

        }

        #endregion

        #region UpdateChargePointListRequestXML(ChargePointInfos)

        /// <summary>
        /// Create an OCHP UpdateChargePointList XML/SOAP request.
        /// </summary>
        /// <param name="ChargePointInfos">An enumeration of charge point infos.</param>
        public static XElement UpdateChargePointListRequestXML(IEnumerable<ChargePointInfo>  ChargePointInfos)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <OCHP:UpdateChargePointListRequest>
            //
            //          <!--1 or more repetitions:-->
            //          <OCHP:chargePointInfoArray>
            //             ...
            //          </OCHP:chargePointInfoArray>
            //
            //       </OCHP:UpdateChargePointListRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (ChargePointInfos == null || !ChargePointInfos.Any())
                throw new ArgumentNullException(nameof(ChargePointInfos),  "The given enumeration of charge point infos must not be null or empty!");

            #endregion


            return SOAP.Encapsulation(new XElement(OCHPNS.Default + "UpdateChargePointListRequest",

                                          ChargePointInfos.Select(chargepointinfo => chargepointinfo.ToXML()).
                                                           ToArray()

                                     ));

        }

        #endregion

        #region UpdateStatusXML(EVSEStatus, ParkingStatus = null, DefaultTTL = null)

        /// <summary>
        /// Create an OCHP add charge detail records XML/SOAP request.
        /// </summary>
        /// <param name="EVSEStatus">An enumeration of EVSE status.</param>
        /// <param name="ParkingStatus">An enumeration of parking status.</param>
        /// <param name="DefaultTTL">The default time to live for these status.</param>
        public static XElement UpdateStatusXML(IEnumerable<EVSEStatus>     EVSEStatus     = null,
                                               IEnumerable<ParkingStatus>  ParkingStatus  = null,
                                               DateTime?                   DefaultTTL     = null)

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <OCHP:UpdateStatusRequest>
            //
            //          <!--Zero or more repetitions:-->
            //          <OCHP:evse major="?" minor="?" ttl="?">
            //             <OCHP:evseId>?</OCHP:evseId>
            //          </OCHP:evse>
            //
            //          <!--Zero or more repetitions:-->
            //          <OCHP:parking status="?" ttl="?">
            //             <OCHP:parkingId>?</OCHP:parkingId>
            //          </OCHP:parking>
            //
            //          <!--Optional:-->
            //          <OCHP:ttl>
            //             <OCHP:DateTime>?</OCHP:DateTime>
            //          </OCHP:ttl>
            //
            //       </OCHP:UpdateStatusRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "UpdateStatusRequest",

                                      EVSEStatus.   SafeSelect(status => status.ToXML()),
                                      ParkingStatus.SafeSelect(status => status.ToXML()),

                                      DefaultTTL.HasValue
                                          ? new XElement(OCHPNS.Default + "ttl",
                                                new XElement(OCHPNS.Default + "DateTime",  DefaultTTL.Value.ToIso8601())
                                            )
                                          : null

                                 ));

        #endregion


        #region GetSingleRoamingAuthorisationXML(EMTId)

        /// <summary>
        /// Create an OCHP GetSingleRoamingAuthorisation XML/SOAP request.
        /// </summary>
        /// <param name="EMTId">An e-mobility token.</param>
        public static XElement GetSingleRoamingAuthorisationXML(EMT_Id  EMTId)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //      <ns:GetSingleRoamingAuthorisationRequest>
            //
            //         <ns:emtId representation="plain">
            //
            //            <ns:instance>?</ns:instance>
            //            <ns:tokenType>?</ns:tokenType>
            //
            //            <!--Optional:-->
            //            <ns:tokenSubType>?</ns:tokenSubType>
            //
            //         </ns:emtId>
            //
            //      </ns:GetSingleRoamingAuthorisationRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (EMTId == null)
                throw new ArgumentNullException(nameof(EMTId),  "The given e-mobility token must not be null!");

            #endregion


            return SOAP.Encapsulation(new XElement(OCHPNS.Default + "GetSingleRoamingAuthorisationRequest",
                                          EMTId.ToXML()
                                     ));

        }

        #endregion

        #region GetRoamingAuthorisationListXML()

        /// <summary>
        /// Create an OCHP GetRoamingAuthorisationList XML/SOAP request.
        /// </summary>
        public static XElement GetRoamingAuthorisationListXML()

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //
            //      <ns:GetRoamingAuthorisationListRequest />
            //
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "GetRoamingAuthorisationListRequest"));

        #endregion

        #region GetRoamingAuthorisationListUpdatesXML(LastUpdate)

        /// <summary>
        /// Create an OCHP GetRoamingAuthorisationListUpdates XML/SOAP request.
        /// </summary>
        /// <param name="LastUpdate">The timestamp of the last roaming authorisation list update.</param>
        public static XElement GetRoamingAuthorisationListUpdatesXML(DateTime LastUpdate)

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //      <ns:GetRoamingAuthorisationListUpdatesRequest>
            //
            //         <ns:lastUpdate>
            //            <ns:DateTime>?</ns:DateTime>
            //         </ns:lastUpdate>
            //
            //      </ns:GetRoamingAuthorisationListUpdatesRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "GetRoamingAuthorisationListUpdatesRequest",
                                      new XElement(OCHPNS.Default + "lastUpdate",
                                          new XElement(OCHPNS.Default + "DateTime",
                                              LastUpdate.ToIso8601()
                                 ))));

        #endregion


        #region AddCDRsXML(CDRInfos)

        /// <summary>
        /// Create an OCHP AddCDRs XML/SOAP request.
        /// </summary>
        /// <param name="CDRInfos">An enumeration of charge detail records.</param>
        public static XElement AddCDRsXML(IEnumerable<CDRInfo>  CDRInfos)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //      <ns:AddCDRsRequest>
            //
            //         <!--1 or more repetitions:-->
            //         <ns:cdrInfoArray>
            //           ...
            //         </ns:cdrInfoArray>
            //
            //      </ns:AddCDRsRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (CDRInfos == null || !CDRInfos.Any())
                throw new ArgumentNullException(nameof(CDRInfos),  "The given enumeration of charge detail records must not be null!");

            #endregion


            return SOAP.Encapsulation(new XElement(OCHPNS.Default + "AddCDRsRequest",

                                          CDRInfos.SafeSelect(cdr => cdr.ToXML(OCHPNS.Default + "cdrInfoArray"))

                                     ));

        }

        #endregion

        #region CheckCDRsXML(CDRStatus = null)

        /// <summary>
        /// Create an OCHP CheckCDRs XML/SOAP request.
        /// </summary>
        /// <param name="CDRStatus">The status of the requested charge detail records.</param>
        public static XElement CheckCDRsXML(CDRStatus? CDRStatus = null)

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //      <OCHP:CheckCDRsRequest>
            //
            //         <!--Optional:-->
            //         <OCHP:cdrStatus>
            //            <OCHP:CdrStatusType>?</OCHP:CdrStatusType>
            //         </OCHP:cdrStatus>
            //
            //      </OCHP:CheckCDRsRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "CheckCDRsRequest",

                                      CDRStatus.HasValue
                                          ? new XElement(OCHPNS.Default + "cdrStatus",
                                                new XElement(OCHPNS.Default + "CdrStatusType", XML_IO.AsText(CDRStatus.Value))
                                            )
                                          : null

                                 ));

        #endregion


        #region UpdateTariffsXML(CDRInfos)

        /// <summary>
        /// Create an OCHP UpdateTariffs XML/SOAP request.
        /// </summary>
        /// <param name="TariffInfos">An enumeration of tariff infos.</param>
        public static XElement UpdateTariffsXML(IEnumerable<TariffInfo>  TariffInfos)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //      <ns:UpdateTariffsRequest>
            //
            //         <!--1 or more repetitions:-->
            //         <ns:TariffInfoArray>
            //           ...
            //         </ns:TariffInfoArray>
            //
            //      </ns:UpdateTariffsRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (TariffInfos == null || !TariffInfos.Any())
                throw new ArgumentNullException(nameof(TariffInfos),  "The given enumeration of tariff infos must not be null!");

            #endregion


            return SOAP.Encapsulation(new XElement(OCHPNS.Default + "UpdateTariffsRequest",

                                          TariffInfos.SafeSelect(tariffinfo => tariffinfo.ToXML(OCHPNS.Default + "TariffInfoArray"))

                                     ));

        }

        #endregion


        // OCHPdirect

        #region AddServiceEndpointsXML(OperatorEndpoints)

        /// <summary>
        /// Create an OCHP AddServiceEndpoints XML/SOAP request.
        /// </summary>
        /// <param name="OperatorEndpoints">An enumeration of operator endpoints.</param>
        public static XElement AddServiceEndpointsXML(IEnumerable<OperatorEndpoint>  OperatorEndpoints)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <OCHP:AddServiceEndpointsRequest>
            //
            //          <!--Zero or more repetitions:-->
            //          <OCHP:providerEndpointArray>
            //             <OCHP:url>?</ns:url>
            //             <OCHP:namespaceUrl>?</ns:namespaceUrl>
            //             <OCHP:accessToken>?</ns:accessToken>
            //             <OCHP:validDate>?</ns:validDate>
            //             <!--1 or more repetitions:-->
            //             <OCHP:whitelist>?</ns:whitelist>
            //             <!--Zero or more repetitions:-->
            //             <OCHP:blacklist>?</ns:blacklist>
            //          </ns:providerEndpointArray>
            //
            //          <!--Zero or more repetitions:-->
            //          <OCHP:operatorEndpointArray>
            //             <OCHP:url>?</ns:url>
            //             <OCHP:namespaceUrl>?</ns:namespaceUrl>
            //             <OCHP:accessToken>?</ns:accessToken>
            //             <OCHP:validDate>?</ns:validDate>
            //             <!--1 or more repetitions:-->
            //             <OCHP:whitelist>?</ns:whitelist>
            //             <!--Zero or more repetitions:-->
            //             <OCHP:blacklist>?</ns:blacklist>
            //          </ns:operatorEndpointArray>
            //
            //       </ns:AddServiceEndpointsRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (OperatorEndpoints.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(OperatorEndpoints),  "The given enumeration of operator endpoints must not be null or empty!");

            #endregion


            return SOAP.Encapsulation(new XElement(OCHPNS.Default + "AddServiceEndpointsRequest",

                                          OperatorEndpoints.Select(endpoints => endpoints.ToXML(OCHPNS.Default + "operatorEndpointArray")).
                                                            ToArray()

                                     ));

        }

        #endregion

        #region GetServiceEndpointsXML(ProviderEndpoints)

        /// <summary>
        /// Create an OCHP GetServiceEndpoints XML/SOAP request.
        /// </summary>
        public static XElement GetServiceEndpointsXML()
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <OCHP:GetServiceEndpointsRequest />
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion


            return SOAP.Encapsulation(new XElement(OCHPNS.Default + "GetServiceEndpointsRequest"));

        }

        #endregion


    }

}
