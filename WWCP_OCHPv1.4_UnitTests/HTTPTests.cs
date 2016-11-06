﻿/*
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
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.UnitTests
{

    /// <summary>
    /// OCHP parking status unit tests.
    /// </summary>
    [TestFixture]
    public class HTTPTests
    {

        private CH.CHServer    ClearingHouseServer;
        private CPO.CPOClient  CPOClient;

        public HTTPTests()
        {

            var DNSClient = new DNSClient(SearchForIPv6DNSServers: false);
            var TCPPort   = IPPort.Parse(10000);

            this.ClearingHouseServer = new CH. CHServer (TCPPort: TCPPort, DNSClient: DNSClient);

            this.CPOClient           = new CPO.CPOClient("Client #1",
                                                         "127.0.0.1",
                                                         TCPPort,
                                                         DNSClient: DNSClient);


            ClearingHouseServer.OnGetSingleRoamingAuthorisationRequest += (Timestamp,
                                                                           Sender,
                                                                           CancellationToken,
                                                                           EventTrackingId,

                                                                           EMTId,

                                                                           QueryTimeout) => {

                                                                               return Task.FromResult(new CPO.GetSingleRoamingAuthorisationResponse(new CPO.GetSingleRoamingAuthorisationRequest(EMTId),
                                                                                                                                                    Result.OK(),
                                                                                                                                                    new RoamingAuthorisationInfo(EMTId,
                                                                                                                                                                                 Contract_Id.Parse(""),
                                                                                                                                                                                 DateTime.Now,
                                                                                                                                                                                 "XXX")));

                                                                           };


            ClearingHouseServer.OnUpdateStatusRequest += (Timestamp,
                                                          Sender,
                                                          CancellationToken,
                                                          EventTrackingId,

                                                          EVSEStatus,
                                                          ParkingStatus,
                                                          DefaultTTL,

                                                          Timeout) => {

                                                              return Task.FromResult(new CPO.UpdateStatusResponse(new CPO.UpdateStatusRequest(EVSEStatus, ParkingStatus, DefaultTTL),
                                                                                                                  Result.OK()));

                                                          };

        }


        #region UpdateStatusTest1()

        [Test]
        public void UpdateStatusTest1()
        {

            var Task1 = CPOClient.UpdateStatus(new List<EVSEStatus> {
                                                   new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*1"), EVSEMajorStatusTypes.Available),
                                                   new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*2"), EVSEMajorStatusTypes.NotAvailable, EVSEMinorStatusTypes.Charging)
                                               },
                                               new List<ParkingStatus> {
                                                   new ParkingStatus(Parking_Id.Parse("DE*GEF*P5555*1"), ParkingStatusTypes.Available),
                                                   new ParkingStatus(Parking_Id.Parse("DE*GEF*P5555*2"), ParkingStatusTypes.NotAvailable)
                                               });

            Task1.Wait(TimeSpan.FromSeconds(30));

            var Response = Task1.Result.Content;

        }

        #endregion

        #region GetSingleRoamingAuthorisationTest1()

        [Test]
        public void GetSingleRoamingAuthorisationTest1()
        {

            var Task1 = CPOClient.GetSingleRoamingAuthorisation(new EMT_Id("1234",
                                                                           TokenRepresentations.Plain,
                                                                           TokenTypes.RFID,
                                                                           TokenSubTypes.MifareClassic));

            Task1.Wait(TimeSpan.FromSeconds(30));

            var Response = Task1.Result.Content;

        }

        #endregion


    }

}