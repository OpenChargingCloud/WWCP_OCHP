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
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.UnitTests
{

    /// <summary>
    /// OCHP UpdateStatus unit tests.
    /// </summary>
    [TestFixture]
    public class UpdateStatusTests : ASOAPTests
    {

        #region Constructor(s)

        public UpdateStatusTests()
        {

            ClearingHouseServer.OnUpdateStatusRequest += (Timestamp,
                                                          Sender,
                                                          CancellationToken,
                                                          EventTrackingId,

                                                          EVSEStatus,
                                                          ParkingStatus,
                                                          DefaultTTL,

                                                          Timeout) => {

                                                              foreach (var status in EVSEStatus)
                                                                  ClearingHouseEVSEStatus.   AddOrUpdate(status.EVSEId,
                                                                                                         status,
                                                                                                         (a, b) => b);

                                                              foreach (var status in ParkingStatus)
                                                                  ClearingHouseParkingStatus.AddOrUpdate(status.ParkingId,
                                                                                                         status,
                                                                                                         (a, b) => b);

                                                              return Task.FromResult(
                                                                         new CPO.UpdateStatusResponse(
                                                                             new CPO.UpdateStatusRequest(EVSEStatus,
                                                                                                         ParkingStatus,
                                                                                                         DefaultTTL),
                                                                             Result.OK()
                                                                         )
                                                                     );

                                                          };

        }

        #endregion


        #region UpdateStatusTest1()

        [Test]
        public async Task UpdateStatusTest1()
        {

            var Now                 = DateTime.Parse(DateTime.Now.ToIso8601());

            var EVSEId1             = EVSE_Id.Parse("DE*GEF*E1234*1");
            var EVSEMajorStatus1_1  = EVSEMajorStatusTypes.Available;

            var EVSEId2             = EVSE_Id.Parse("DE*GEF*E1234*2");
            var EVSEMajorStatus2_1  = EVSEMajorStatusTypes.NotAvailable;
            var EVSEMinorStatus2_1  = EVSEMinorStatusTypes.Charging;

            var EVSEId3             = EVSE_Id.Parse("DE*GEF*E1234*3");
            var EVSEMajorStatus3_1  = EVSEMajorStatusTypes.NotAvailable;
            var EVSEMinorStatus3_1  = EVSEMinorStatusTypes.Blocked;

            var Response = await CPOClient.UpdateStatus(new List<EVSEStatus> {
                                                            new EVSEStatus   (EVSEId1, EVSEMajorStatus1_1),
                                                            new EVSEStatus   (EVSEId2, EVSEMajorStatus2_1, EVSEMinorStatus2_1),
                                                            new EVSEStatus   (EVSEId3, EVSEMajorStatus3_1, EVSEMinorStatus3_1, Now + TimeSpan.FromHours(1))
                                                        },
                                                        new List<ParkingStatus> {
                                                            new ParkingStatus(Parking_Id.Parse("DE*GEF*P5555*1"), ParkingStatusTypes.Available),
                                                            new ParkingStatus(Parking_Id.Parse("DE*GEF*P5555*2"), ParkingStatusTypes.NotAvailable)
                                                        });

            Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);


            Assert.AreEqual(3, ClearingHouseEVSEStatus.Count, "The number of EVSE status at the clearing house is invalid!");

            Assert.IsTrue  (ClearingHouseEVSEStatus.ContainsKey(EVSEId1));
            Assert.AreEqual(EVSEMajorStatus1_1, ClearingHouseEVSEStatus[EVSEId1].MajorStatus);
            Assert.IsFalse (ClearingHouseEVSEStatus[EVSEId1].MinorStatus.HasValue);
            Assert.IsFalse (ClearingHouseEVSEStatus[EVSEId1].TTL.        HasValue);

            Assert.IsTrue  (ClearingHouseEVSEStatus.ContainsKey(EVSEId2));
            Assert.AreEqual(EVSEMajorStatus2_1, ClearingHouseEVSEStatus[EVSEId2].MajorStatus);
            Assert.IsTrue  (ClearingHouseEVSEStatus[EVSEId2].MinorStatus.HasValue);
            Assert.AreEqual(EVSEMinorStatus2_1, ClearingHouseEVSEStatus[EVSEId2].MinorStatus);
            Assert.IsFalse (ClearingHouseEVSEStatus[EVSEId2].TTL.        HasValue);

            Assert.IsTrue  (ClearingHouseEVSEStatus.ContainsKey(EVSEId3));
            Assert.AreEqual(EVSEMajorStatus3_1, ClearingHouseEVSEStatus[EVSEId3].MajorStatus);
            Assert.IsTrue  (ClearingHouseEVSEStatus[EVSEId3].MinorStatus.HasValue);
            Assert.AreEqual(EVSEMinorStatus3_1, ClearingHouseEVSEStatus[EVSEId3].MinorStatus);
            Assert.IsTrue  (ClearingHouseEVSEStatus[EVSEId3].TTL.        HasValue);
            Assert.AreEqual(Now + TimeSpan.FromHours(1), ClearingHouseEVSEStatus[EVSEId3].TTL);


        }

        #endregion


    }

}
