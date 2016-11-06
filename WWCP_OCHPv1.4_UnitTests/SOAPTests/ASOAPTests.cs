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

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.UnitTests
{

    /// <summary>
    /// Abstract OCHP SOAP unit tests.
    /// </summary>
    public abstract class ASOAPTests
    {

        #region Data

        protected readonly CH.CHServer    ClearingHouseServer;
        protected readonly IPPort         ClearingHouseTCPPort  = IPPort.Parse(10000);

        protected readonly CPO.CPOClient  CPOClient;
        protected readonly CPO.CPOServer  CPOServer;
        protected readonly IPPort         CPOServerTCPPort      = IPPort.Parse(10001);

        protected readonly EMP.EMPClient  EMPClient;
        protected readonly EMP.EMPServer  EMPServer;
        protected readonly IPPort         EMPServerTCPPort      = IPPort.Parse(10002);

        #endregion

        public ASOAPTests()
        {

            var DNSClient = new DNSClient(SearchForIPv6DNSServers: false);

            // ClearingHouse
            this.ClearingHouseServer = new CH. CHServer (TCPPort:    ClearingHouseTCPPort,
                                                         DNSClient:  DNSClient);

            // CPO
            this.CPOClient           = new CPO.CPOClient("CPOClient #1",
                                                         "127.0.0.1",
                                                         ClearingHouseTCPPort,
                                                         DNSClient:  DNSClient);

            this.CPOServer           = new CPO.CPOServer(TCPPort:    CPOServerTCPPort,
                                                         DNSClient:  DNSClient);

            // EMP
            this.EMPClient           = new EMP.EMPClient("EMPClient #1",
                                                         "127.0.0.1",
                                                         ClearingHouseTCPPort,
                                                         DNSClient:  DNSClient);

            this.EMPServer           = new EMP.EMPServer(TCPPort:    EMPServerTCPPort,
                                                         DNSClient:  DNSClient);


        }

    }

}
