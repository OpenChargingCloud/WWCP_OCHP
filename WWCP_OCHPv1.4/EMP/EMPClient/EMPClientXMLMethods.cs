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


    }

}
