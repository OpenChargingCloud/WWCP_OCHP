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
using System.Threading.Tasks;
using System.Collections.Generic;

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.UnitTests
{

    /// <summary>
    /// Unit tests for manipulating and requesting the list
    /// of charge points status at the clearing house.
    /// </summary>
    [TestFixture]
    public class UpdateStatusTests : ASOAPTests
    {

        #region Constructor(s)

        /// <summary>
        /// Unit tests for manipulating and requesting the list
        /// of charge points status at the clearing house.
        /// </summary>
        public UpdateStatusTests()
        {

            #region OnUpdateStatusRequest...

            ClearingHouseServer.OnUpdateStatusRequest += (Timestamp,
                                                          Sender,
                                                          CancellationToken,
                                                          EventTrackingId,

                                                          EVSEStatus,
                                                          ParkingStatus,
                                                          DefaultTTL,

                                                          Timeout) => {

                                                              var Now = DateTime.Now;

                                                              foreach (var status in EVSEStatus)
                                                                  ClearingHouse_EVSEStatus.   AddOrUpdate(status.EVSEId,
                                                                                                          new Timestamped<EVSEStatus>   (Now, status),
                                                                                                          (a, b) => b);

                                                              foreach (var status in ParkingStatus)
                                                                  ClearingHouse_ParkingStatus.AddOrUpdate(status.ParkingId,
                                                                                                          new Timestamped<ParkingStatus>(Now, status),
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

            #endregion

            #region OnGetStatusRequest...

            ClearingHouseServer.OnGetStatusRequest += (Timestamp,
                                                       Sender,
                                                       CancellationToken,
                                                       EventTrackingId,

                                                       LastRequest,
                                                       StatusType,

                                                       QueryTimeout) => {

                                                                            return Task.FromResult(
                                                                                new EMP.GetStatusResponse(
                                                                                    new EMP.GetStatusRequest(LastRequest,
                                                                                                             StatusType),
                                                                                    Result.OK(),
                                                                                    ClearingHouse_EVSEStatus.   Values.Select(item => item.Value),
                                                                                    ClearingHouse_ParkingStatus.Values.Select(item => item.Value)
                                                                                )
                                                                            );

                                                                        };

            #endregion

        }

        #endregion


        #region UpdateStatusTest()

        [Test]
        public async Task UpdateStatusTest()
        {

            #region Get - should be empty!

            using (var Response = await EMPClient.GetStatus())
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(0, Response.Content.EVSEStatus.    Count(), "The number of charge point status at the clearing house is invalid!");
                Assert.AreEqual(0, Response.Content.ParkingStatus. Count(), "The number of parking status at the clearing house is invalid!");
                Assert.AreEqual(0, Response.Content.CombinedStatus.Count(), "The number of combined status at the clearing house is invalid!");

            }

            #endregion


            #region Set

            var Now1                = DateTime.Parse(DateTime.Now.ToIso8601());

            var EVSEId1             = EVSE_Id.Parse("DE*GEF*E1234*1");
            var EVSEMajorStatus1_1  = EVSEMajorStatusTypes.Available;

            var EVSEId2             = EVSE_Id.Parse("DE*GEF*E1234*2");
            var EVSEMajorStatus2_1  = EVSEMajorStatusTypes.NotAvailable;
            var EVSEMinorStatus2_1  = EVSEMinorStatusTypes.Charging;

            var EVSEId3             = EVSE_Id.Parse("DE*GEF*E1234*3");
            var EVSEMajorStatus3_1  = EVSEMajorStatusTypes.NotAvailable;
            var EVSEMinorStatus3_1  = EVSEMinorStatusTypes.Blocked;


            using (var Response = await CPOClient.UpdateStatus(new List<EVSEStatus> {
                                                                   new EVSEStatus   (EVSEId1, EVSEMajorStatus1_1),
                                                                   new EVSEStatus   (EVSEId2, EVSEMajorStatus2_1, EVSEMinorStatus2_1),
                                                                   new EVSEStatus   (EVSEId3, EVSEMajorStatus3_1, EVSEMinorStatus3_1, Now1 + TimeSpan.FromHours(1))
                                                               },
                                                               new List<ParkingStatus> {
                                                                   new ParkingStatus(Parking_Id.Parse("DE*GEF*P5555*1"), ParkingStatusTypes.Available),
                                                                   new ParkingStatus(Parking_Id.Parse("DE*GEF*P5555*2"), ParkingStatusTypes.NotAvailable)
                                                               }))
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(3, ClearingHouse_EVSEStatus.    Count, "The number of charge point status at the clearing house is invalid!");
                Assert.AreEqual(2, ClearingHouse_ParkingStatus. Count, "The number of parking status at the clearing house is invalid!");

                Assert.IsTrue  (ClearingHouse_EVSEStatus.ContainsKey(EVSEId1));
                Assert.AreEqual(EVSEMajorStatus1_1, ClearingHouse_EVSEStatus[EVSEId1].Value.MajorStatus);
                Assert.IsFalse (ClearingHouse_EVSEStatus[EVSEId1].Value.MinorStatus.HasValue);
                Assert.IsFalse (ClearingHouse_EVSEStatus[EVSEId1].Value.TTL.HasValue);

                Assert.IsTrue  (ClearingHouse_EVSEStatus.ContainsKey(EVSEId2));
                Assert.AreEqual(EVSEMajorStatus2_1, ClearingHouse_EVSEStatus[EVSEId2].Value.MajorStatus);
                Assert.IsTrue  (ClearingHouse_EVSEStatus[EVSEId2].Value.MinorStatus.HasValue);
                Assert.AreEqual(EVSEMinorStatus2_1, ClearingHouse_EVSEStatus[EVSEId2].Value.MinorStatus);
                Assert.IsFalse  (ClearingHouse_EVSEStatus[EVSEId2].Value.TTL.HasValue);

                Assert.IsTrue  (ClearingHouse_EVSEStatus.ContainsKey(EVSEId3));
                Assert.AreEqual(EVSEMajorStatus3_1, ClearingHouse_EVSEStatus[EVSEId3].Value.MajorStatus);
                Assert.IsTrue  (ClearingHouse_EVSEStatus[EVSEId3].Value.MinorStatus.HasValue);
                Assert.AreEqual(EVSEMinorStatus3_1, ClearingHouse_EVSEStatus[EVSEId3].Value.MinorStatus);
                Assert.IsTrue  (ClearingHouse_EVSEStatus[EVSEId3].Value.TTL.HasValue);
                Assert.AreEqual(Now1 + TimeSpan.FromHours(1), ClearingHouse_EVSEStatus[EVSEId3].Value.TTL);

            }

            #endregion

            #region Get - should be three/two/zero!

            using (var Response = await EMPClient.GetStatus())
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(3, Response.Content.EVSEStatus.    Count(), "The number of charge point status at the clearing house is invalid!");
                Assert.AreEqual(2, Response.Content.ParkingStatus. Count(), "The number of parking status at the clearing house is invalid!");
                Assert.AreEqual(0, Response.Content.CombinedStatus.Count(), "The number of combined status at the clearing house is invalid!");

            }

            #endregion



        }

        #endregion

    }

}
