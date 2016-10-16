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

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// OCHP EMP client XML methods.
    /// </summary>
    public static class EMPClientXMLMethods
    {

        // OCHP

        #region GetChargePointListXML()

        /// <summary>
        /// Create an OCHP GetChargePointList XML/SOAP request.
        /// </summary>
        public static XElement GetChargePointListXML()

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //
            //      <ns:GetChargePointListRequest />
            //
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "GetChargePointListRequest"));

        #endregion

        #region GetChargePointListUpdatesXML(LastUpdate)

        /// <summary>
        /// Create an OCHP GetChargePointListUpdates XML/SOAP request.
        /// </summary>
        /// <param name="LastUpdate">The timestamp of the last charge point list update.</param>
        public static XElement GetChargePointListUpdatesXML(DateTime LastUpdate)

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //      <ns:GetChargePointListUpdatesRequest>
            //
            //         <ns:lastUpdate>
            //            <ns:DateTime>?</ns:DateTime>
            //         </ns:lastUpdate>
            //
            //      </ns:GetChargePointListUpdatesRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "GetChargePointListUpdatesRequest",
                                      new XElement(OCHPNS.Default + "lastUpdate",
                                          new XElement(OCHPNS.Default + "DateTime",
                                              LastUpdate.ToIso8601()
                                 ))));

        #endregion

        #region GetStatusXML(LastRequest = null, StatusType = null)

        /// <summary>
        /// Create an OCHP GetStatus XML/SOAP request.
        /// </summary>
        /// <param name="LastRequest">Only return status data newer than the given timestamp.</param>
        /// <param name="StatusType">A status type filter.</param>
        public static XElement GetStatusXML(DateTime?     LastRequest  = null,
                                            StatusTypes?  StatusType   = null)

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //      <ns:GetStatusRequest>
            //
            //         <!--Optional:-->
            //         <ns:startDateTime>
            //            <ns:DateTime>?</ns:DateTime>
            //         </ns:startDateTime>
            //
            //         <!--Optional:-->
            //         <ns:statusType>?</ns:statusType>
            //
            //      </ns:GetStatusRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "GetStatusRequest",

                                      LastRequest.HasValue
                                          ? new XElement(OCHPNS.Default + "lastUpdate",
                                                new XElement(OCHPNS.Default + "DateTime",
                                                    LastRequest.Value.ToIso8601()))
                                          : null,

                                      StatusType.HasValue
                                          ? new XElement(OCHPNS.Default + "lastUpdate", StatusType.Value)
                                          : null

                                 ));

        #endregion


        #region SetRoamingAuthorisationListXML()

        /// <summary>
        /// Create an OCHP SetRoamingAuthorisationList XML/SOAP request.
        /// </summary>
        /// <param name="RoamingAuthorisationInfos">An enumeration of roaming authorisation infos.</param>
        public static XElement SetRoamingAuthorisationListXML(IEnumerable<RoamingAuthorisationInfo> RoamingAuthorisationInfos)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <OCHP:SetRoamingAuthorisationListRequest>
            //
            //          <!--1 or more repetitions:-->
            //          <OCHP:roamingAuthorisationInfoArray>
            //             ...
            //          </OCHP:roamingAuthorisationInfoArray>
            //
            //       </OCHP:SetRoamingAuthorisationListRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (RoamingAuthorisationInfos == null || !RoamingAuthorisationInfos.Any())
                throw new ArgumentNullException(nameof(RoamingAuthorisationInfos),  "The given enumeration of roaming authorisation infos must not be null or empty!");

            #endregion


            return SOAP.Encapsulation(new XElement(OCHPNS.Default + "SetRoamingAuthorisationListRequest",

                                          RoamingAuthorisationInfos.Select(infos => infos.ToXML(OCHPNS.Default + "roamingAuthorisationInfoArray")).
                                                                    ToArray()

                                     ));

        }

        #endregion

        #region UpdateRoamingAuthorisationListXML(LastUpdate)

        /// <summary>
        /// Create an OCHP UpdateRoamingAuthorisationList XML/SOAP request.
        /// </summary>
        /// <param name="RoamingAuthorisationInfos">An enumeration of roaming authorisation infos.</param>
        public static XElement UpdateRoamingAuthorisationListXML(IEnumerable<RoamingAuthorisationInfo> RoamingAuthorisationInfos)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <OCHP:UpdateRoamingAuthorisationListRequest>
            //
            //          <!--1 or more repetitions:-->
            //          <OCHP:roamingAuthorisationInfoArray>
            //             ...
            //          </OCHP:roamingAuthorisationInfoArray>
            //
            //       </OCHP:UpdateRoamingAuthorisationListRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (RoamingAuthorisationInfos == null || !RoamingAuthorisationInfos.Any())
                throw new ArgumentNullException(nameof(RoamingAuthorisationInfos),  "The given enumeration of roaming authorisation infos must not be null or empty!");

            #endregion


            return SOAP.Encapsulation(new XElement(OCHPNS.Default + "UpdateRoamingAuthorisationListRequest",

                                          RoamingAuthorisationInfos.Select(infos => infos.ToXML(OCHPNS.Default + "roamingAuthorisationInfoArray")).
                                                                    ToArray()

                                     ));

        }

        #endregion


        #region GetCDRsXML(CDRStatus = null)

        /// <summary>
        /// Create an OCHP GetCDRs XML/SOAP request.
        /// </summary>
        /// <param name="CDRStatus">The status of the requested charge detail records.</param>
        public static XElement GetCDRsXML(CDRStatus? CDRStatus = null)

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //      <OCHP:GetCDRsRequest>
            //
            //         <!--Optional:-->
            //         <OCHP:cdrStatus>
            //            <OCHP:CdrStatusType>?</OCHP:CdrStatusType>
            //         </OCHP:cdrStatus>
            //
            //      </OCHP:GetCDRsRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "GetCDRsRequest",

                                      CDRStatus.HasValue
                                          ? new XElement(OCHPNS.Default + "cdrStatus",
                                                new XElement(OCHPNS.Default + "CdrStatusType", XML_IO.AsText(CDRStatus.Value))
                                            )
                                          : null

                                 ));

        #endregion

        #region ConfirmCDRsXML(Approved = null, Declined = null)

        /// <summary>
        /// Create an OCHP ConfirmCDRs XML/SOAP request.
        /// </summary>
        /// <param name="Approved">An enumeration of approved charge detail records.</param>
        /// <param name="Declined">An enumeration of declined charge detail records.</param>
        public static XElement ConfirmCDRsXML(IEnumerable<EVSECDRPair>  Approved = null,
                                              IEnumerable<EVSECDRPair>  Declined = null)

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //      <OCHP:ConfirmCDRsRequest>
            //
            //         <!--Zero or more repetitions:-->
            //         <ns:approved>
            //            <ns:cdrId>?</ns:cdrId>
            //            <ns:evseId>?</ns:evseId>
            //         </ns:approved>
            //
            //         <!--Zero or more repetitions:-->
            //         <ns:declined>
            //            <ns:cdrId>?</ns:cdrId>
            //            <ns:evseId>?</ns:evseId>
            //         </ns:declined>
            //
            //      </OCHP:ConfirmCDRsRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "ConfirmCDRsRequest",

                                      Approved != null
                                          ? Approved.SafeSelect(pair => pair.ToXML(OCHPNS.Default + "approved"))
                                          : null,

                                      Declined != null
                                          ? Declined.SafeSelect(pair => pair.ToXML(OCHPNS.Default + "declined"))
                                          : null

                                 ));

        #endregion


        // OCHPdirect

        #region AddServiceEndpointsXML(ProviderEndpoints)

        /// <summary>
        /// Create an OCHP AddServiceEndpoints XML/SOAP request.
        /// </summary>
        /// <param name="ProviderEndpoints">An enumeration of provider endpoints.</param>
        public static XElement AddServiceEndpointsXML(IEnumerable<ProviderEndpoint>  ProviderEndpoints)
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

            if (ProviderEndpoints.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(ProviderEndpoint),  "The given enumeration of provider endpoints must not be null or empty!");

            #endregion


            return SOAP.Encapsulation(new XElement(OCHPNS.Default + "AddServiceEndpointsRequest",

                                          ProviderEndpoints.Select(endpoints => endpoints.ToXML(OCHPNS.Default + "providerEndpointArray")).
                                                            ToArray()

                                     ));

        }

        #endregion

        #region GetServiceEndpointsXML()

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

        #region SelectEVSEXML(EVSEId, ContractId, ReserveUntil)

        /// <summary>
        /// Create an OCHP AddServiceEndpoints XML/SOAP request.
        /// </summary>
        /// <param name="EVSEId">The unique identification of an EVSE.</param>
        /// <param name="ContractId">The unique identification of an e-mobility contract.</param>
        /// <param name="ReserveUntil">An optional timestamp till when then given EVSE should be reserved.</param>
        public static XElement SelectEVSEXML(EVSE_Id      EVSEId,
                                             Contract_Id  ContractId,
                                             DateTime?    ReserveUntil)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">
            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <OCHP:SelectEvseRequest>
            //
            //          <OCHP:evseId>?</OCHP:evseId>
            //          <OCHP:contractId>?</OCHP:contractId>
            //
            //          <!--Optional:-->
            //          <OCHP:reserveUntil>
            //             <OCHP:DateTime>?</OCHP:DateTime>
            //          </OCHP:reserveUntil>
            //
            //       </OCHP:SelectEvseRequest>            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),      "The given EVSE identification must not be null!");

            if (ContractId == null)
                throw new ArgumentNullException(nameof(ContractId),  "The given e-mobility contract identification must not be null!");

            #endregion


            return SOAP.Encapsulation(new XElement(OCHPNS.Default + "SelectEvseRequest",

                                          new XElement(OCHPNS.Default + "evseId",      EVSEId.    ToString()),
                                          new XElement(OCHPNS.Default + "contractId",  ContractId.ToString()),

                                          ReserveUntil.HasValue
                                              ? new XElement(OCHPNS.Default + "reserveUntil",
                                                    new XElement(OCHPNS.Default + "DateTime",  ReserveUntil.Value.ToIso8601())
                                                )
                                              : null

                                     ));

        }

        #endregion


    }

}
