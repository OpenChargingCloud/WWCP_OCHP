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
    /// OCHP EMP client XML methods.
    /// </summary>
    public static class EMPClientXMLMethods
    {

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
        /// Create an OCHP get charge detail records XML/SOAP request.
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
                                                new XElement(OCHPNS.Default + "CdrStatusType", ObjectMapper.AsText(CDRStatus.Value))
                                            )
                                          : null

                                 ));

        #endregion


    }

}
