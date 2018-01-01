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

using System.Linq;
using System.Collections.Concurrent;

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.UnitTests
{

    /// <summary>
    /// Abstract OCHP SOAP unit tests.
    /// </summary>
    public abstract class ASOAPTests
    {

        #region Data

        protected readonly CH.CHServer     ClearingHouseServer;

        protected readonly CPO.CPOClient   CPOClient;
        protected readonly CPO.CPOServer   CPOServer;

        protected readonly EMP.EMPClient   EMPClient;
        protected readonly EMP.EMPServer   EMPServer;

        protected readonly ConcurrentBag<OperatorEndpoint>   ClearingHouse_OperatorEndpoints;
        protected readonly ConcurrentBag<ProviderEndpoint>   ClearingHouse_ProviderEndpoints;

        protected readonly ConcurrentDictionary<EVSE_Id,    Timestamped<ChargePointInfo>>            ClearingHouse_ChargePointInfos;
        protected readonly ConcurrentDictionary<EVSE_Id,    Timestamped<EVSEStatus>>                 ClearingHouse_EVSEStatus;
        protected readonly ConcurrentDictionary<Parking_Id, Timestamped<ParkingSpotInfo>>            ClearingHouse_ParkingSpotInfos;
        protected readonly ConcurrentDictionary<Parking_Id, Timestamped<ParkingStatus>>              ClearingHouse_ParkingStatus;
        protected readonly ConcurrentDictionary<Tariff_Id,  Timestamped<TariffInfo>>                 ClearingHouse_TariffInfos;
        protected readonly ConcurrentDictionary<CDR_Id,     Timestamped<CDRInfo>>                    ClearingHouse_CDRInfos;
        protected readonly ConcurrentDictionary<EMT_Id,     Timestamped<RoamingAuthorisationInfo>>   ClearingHouse_RoamingAuthorisationInfos;

        #endregion

        public ASOAPTests()
        {

            var DNSClient = new DNSClient(SearchForIPv6DNSServers: false);


            //- ClearingHouse ----------------------------------------------------------------------------------

            ClearingHouseServer                      = new CH. CHServer (TCPPort:    IPPort.Parse(10000),
                                                                         DNSClient:  DNSClient);

            ClearingHouse_OperatorEndpoints          = new ConcurrentBag<OperatorEndpoint>();
            ClearingHouse_ProviderEndpoints          = new ConcurrentBag<ProviderEndpoint>();

            ClearingHouse_EVSEStatus                 = new ConcurrentDictionary<EVSE_Id,    Timestamped<EVSEStatus>>();
            ClearingHouse_ChargePointInfos           = new ConcurrentDictionary<EVSE_Id,    Timestamped<ChargePointInfo>>();
            ClearingHouse_ParkingStatus              = new ConcurrentDictionary<Parking_Id, Timestamped<ParkingStatus>>();
            ClearingHouse_ParkingSpotInfos           = new ConcurrentDictionary<Parking_Id, Timestamped<ParkingSpotInfo>>();
            ClearingHouse_TariffInfos                = new ConcurrentDictionary<Tariff_Id,  Timestamped<TariffInfo>>();
            ClearingHouse_CDRInfos                   = new ConcurrentDictionary<CDR_Id,     Timestamped<CDRInfo>>();
            ClearingHouse_RoamingAuthorisationInfos  = new ConcurrentDictionary<EMT_Id,     Timestamped<RoamingAuthorisationInfo>>();


            //- CPO --------------------------------------------------------------------------------------------

            CPOClient                                = new CPO.CPOClient("CPOClient #1",
                                                                         "127.0.0.1",
                                                                         ClearingHouseServer.IPPorts.First(),
                                                                         DNSClient:  DNSClient);

            CPOServer                                = new CPO.CPOServer(TCPPort:    IPPort.Parse(10001),
                                                                         DNSClient:  DNSClient);


            //- EMP --------------------------------------------------------------------------------------------

            EMPClient                                = new EMP.EMPClient("EMPClient #1",
                                                                         "127.0.0.1",
                                                                         ClearingHouseServer.IPPorts.First(),
                                                                         DNSClient:  DNSClient);

            EMPServer                                = new EMP.EMPServer(TCPPort:    IPPort.Parse(10002),
                                                                         DNSClient:  DNSClient);

        }

    }

}
