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
    /// OCHP CPO Client XML methods.
    /// </summary>
    public static class CPOClientXMLMethods
    {

        #region SetChargePointListRequestXML  (ChargePointInfos)

        /// <summary>
        /// Create an OCHP SetChargePointList XML/SOAP request.
        /// </summary>
        /// <param name="ChargePointInfos">An enumeration of charge point infos.</param>
        public static XElement SetChargePointListRequestXML(IEnumerable<ChargePointInfo>  ChargePointInfos)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <OCHP:SetChargePointListRequest>
            //
            //          <!--1 or more repetitions:-->
            //          <OCHP:chargePointInfoArray>            //             ...
            //          </OCHP:chargePointInfoArray>            //
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

    }

}
