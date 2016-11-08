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

using System.Linq;
using System.Collections.Concurrent;

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

        //protected readonly IPPort                                              ClearingHouseTCPPort;
        protected readonly CH.CHServer                                         ClearingHouseServer;

        protected readonly CPO.CPOClient                                       CPOClient;
        protected readonly CPO.CPOServer                                       CPOServer;
        //protected readonly IPPort                                              CPOServerTCPPort;

        protected readonly EMP.EMPClient                                       EMPClient;
        protected readonly EMP.EMPServer                                       EMPServer;
        //protected readonly IPPort                                              EMPServerTCPPort;

        protected readonly ConcurrentDictionary<EVSE_Id,    ChargePointInfo>   ClearingHouseChargePointInfos;
        protected readonly ConcurrentDictionary<EVSE_Id,    EVSEStatus>        ClearingHouseEVSEStatus;
        protected readonly ConcurrentDictionary<Parking_Id, ParkingSpotInfo>   ClearingHouseParkingSpotInfos;
        protected readonly ConcurrentDictionary<Parking_Id, ParkingStatus>     ClearingHouseParkingStatus;
        protected readonly ConcurrentDictionary<Tariff_Id,  TariffInfo>        ClearingHouseTariffInfos;
        protected readonly ConcurrentDictionary<CDR_Id,     CDRInfo>           ClearingHouseCDRInfos;

        protected readonly ConcurrentBag<OperatorEndpoint>                     ClearingHouseOperatorEndpoints;
        protected readonly ConcurrentBag<ProviderEndpoint>                     ClearingHouseProviderEndpoints;

        #endregion

        public ASOAPTests()
        {

            var DNSClient = new DNSClient(SearchForIPv6DNSServers: false);


            //- ClearingHouse ----------------------------------------------------------------------------

            ClearingHouseServer              = new CH. CHServer (TCPPort:    IPPort.Parse(10000),
                                                                 DNSClient:  DNSClient);

            ClearingHouseEVSEStatus          = new ConcurrentDictionary<EVSE_Id,    EVSEStatus>();
            ClearingHouseChargePointInfos    = new ConcurrentDictionary<EVSE_Id,    ChargePointInfo>();
            ClearingHouseParkingStatus       = new ConcurrentDictionary<Parking_Id, ParkingStatus>();
            ClearingHouseParkingSpotInfos    = new ConcurrentDictionary<Parking_Id, ParkingSpotInfo>();
            ClearingHouseTariffInfos         = new ConcurrentDictionary<Tariff_Id,  TariffInfo>();
            ClearingHouseCDRInfos            = new ConcurrentDictionary<CDR_Id,     CDRInfo>();

            ClearingHouseOperatorEndpoints   = new ConcurrentBag<OperatorEndpoint>();
            ClearingHouseProviderEndpoints   = new ConcurrentBag<ProviderEndpoint>();


            //- CPO --------------------------------------------------------------------------------------

            CPOClient                        = new CPO.CPOClient("CPOClient #1",
                                                                 "127.0.0.1",
                                                                 ClearingHouseServer.IPPorts.First(),
                                                                 DNSClient:  DNSClient);

            CPOServer                        = new CPO.CPOServer(TCPPort:    IPPort.Parse(10001),
                                                                 DNSClient:  DNSClient);


            //- EMP --------------------------------------------------------------------------------------

            EMPClient                        = new EMP.EMPClient("EMPClient #1",
                                                                 "127.0.0.1",
                                                                 ClearingHouseServer.IPPorts.First(),
                                                                 DNSClient:  DNSClient);

            EMPServer                        = new EMP.EMPServer(TCPPort:    IPPort.Parse(10002),
                                                                 DNSClient:  DNSClient);

        }

    }

}
