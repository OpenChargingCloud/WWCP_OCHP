/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.WWCP.OCHPv1_4.EMP;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.UnitTests
{

    /// <summary>
    /// Unit tests for manipulating and requesting the list
    /// of roaming authorisations at the clearing house.
    /// </summary>
    [TestFixture]
    public class RoamingAuthorisationListTests : ASOAPTests
    {

        #region Constructor(s)

        /// <summary>
        /// Unit tests for manipulating and requesting the list
        /// of roaming authorisations at the clearing house.
        /// </summary>
        public RoamingAuthorisationListTests()
        {

            #region OnSetRoamingAuthorisationListRequest...

            ClearingHouseServer.OnSetRoamingAuthorisationListRequest    += (Timestamp,
                                                                            Sender,
                                                                            CancellationToken,
                                                                            EventTrackingId,

                                                                            RoamingAuthorisations,

                                                                            Timeout) => {

                                                                                var Now = DateTime.Now;

                                                                                ClearingHouse_RoamingAuthorisationInfos.Clear();

                                                                                foreach (var roamingauthorisation in RoamingAuthorisations)
                                                                                    ClearingHouse_RoamingAuthorisationInfos.AddOrUpdate(roamingauthorisation.EMTId,
                                                                                                                                        new Timestamped<RoamingAuthorisationInfo>(Now, roamingauthorisation),
                                                                                                                                        (a, b) => b);

                                                                                return Task.FromResult(
                                                                                           new EMP.SetRoamingAuthorisationListResponse(
                                                                                               new EMP.SetRoamingAuthorisationListRequest(RoamingAuthorisations),
                                                                                               Result.OK(),
                                                                                               new RoamingAuthorisationInfo[0]
                                                                                           )
                                                                                       );

                                                                            };

            #endregion

            #region OnUpdateRoamingAuthorisationListRequest...

            ClearingHouseServer.OnUpdateRoamingAuthorisationListRequest += (Timestamp,
                                                                            Sender,
                                                                            CancellationToken,
                                                                            EventTrackingId,

                                                                            RoamingAuthorisations,

                                                                            Timeout) => {

                                                                                var Now = DateTime.Now;

                                                                                foreach (var roamingauthorisation in RoamingAuthorisations)
                                                                                    ClearingHouse_RoamingAuthorisationInfos.AddOrUpdate(roamingauthorisation.EMTId,
                                                                                                                                        new Timestamped<RoamingAuthorisationInfo>(Now, roamingauthorisation),
                                                                                                                                        (a, b) => b);

                                                                                return Task.FromResult(
                                                                                           new EMP.UpdateRoamingAuthorisationListResponse(
                                                                                               new EMP.UpdateRoamingAuthorisationListRequest(RoamingAuthorisations),
                                                                                               Result.OK(),
                                                                                               new RoamingAuthorisationInfo[0]
                                                                                           )
                                                                                       );

                                                                            };

            #endregion

            #region OnGetRoamingAuthorisationListRequest...

            ClearingHouseServer.OnGetRoamingAuthorisationListRequest += (Timestamp,
                                                                         Sender,
                                                                         CancellationToken,
                                                                         EventTrackingId,
                                                                         Timeout) => {

                                                                             return Task.FromResult(
                                                                                        new CPO.GetRoamingAuthorisationListResponse(
                                                                                            new CPO.GetRoamingAuthorisationListRequest(),
                                                                                            Result.OK(),
                                                                                            ClearingHouse_RoamingAuthorisationInfos.Values.Select(item => item.Value)
                                                                                        )
                                                                                    );

                                                                         };

            #endregion

        }

        #endregion


        #region SetAndUpdateRoamingAuthorisationListTest()

        /// <summary>
        /// Set, get, update and delete the list of roaming authorisations.
        /// </summary>
        [Test]
        public async Task SetAndUpdateRoamingAuthorisationListTest()
        {

            #region Get - should be empty!

            using (var Response = await CPOClient.GetRoamingAuthorisationList())
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(0, Response.Content.RoamingAuthorisationInfos.Count(), "The number of roaming authorisations at the clearing house is invalid!");

            }

            #endregion


            #region Set

            using (var Response = await EMPClient.SetRoamingAuthorisationList(
                                                new RoamingAuthorisationInfo[] {

                                                    new RoamingAuthorisationInfo(
                                                        new EMT_Id(
                                                            "1234",
                                                            TokenRepresentations.Plain,
                                                            TokenTypes.RFID, 
                                                            TokenSubTypes.MifareClassic
                                                        ),
                                                        Contract_Id.Parse("DE-GEF-123456789"),
                                                        DateTime.Now + TimeSpan.FromDays(30),
                                                        "Card #1234"
                                                    )

                                                }
                                            ))
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(1, ClearingHouse_RoamingAuthorisationInfos.Count,             "The number of roaming authorisations at the clearing house is invalid!");
                Assert.AreEqual(0, Response.Content.RefusedRoamingAuthorisationInfos.Count(), "The number of refused roaming authorisations is invalid!");

            }

            #endregion

            #region Get - should return one!

            using (var Response = await CPOClient.GetRoamingAuthorisationList())
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(1, Response.Content.RoamingAuthorisationInfos.Count(), "The number of roaming authorisations at the clearing house is invalid!");

            }

            #endregion


            #region Update

            using (var Response = await EMPClient.UpdateRoamingAuthorisationList(
                                                new RoamingAuthorisationInfo[] {

                                                    new RoamingAuthorisationInfo(
                                                        new EMT_Id(
                                                            "5678",
                                                            TokenRepresentations.Plain,
                                                            TokenTypes.RFID, 
                                                            TokenSubTypes.MifareClassic
                                                        ),
                                                        Contract_Id.Parse("DE-GEF-567891234"),
                                                        DateTime.Now + TimeSpan.FromDays(30),
                                                        "Card #5678"
                                                    )

                                                }
                                            ))
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(2, ClearingHouse_RoamingAuthorisationInfos.Count,             "The number of roaming authorisations at the clearing house is invalid!");
                Assert.AreEqual(0, Response.Content.RefusedRoamingAuthorisationInfos.Count(), "The number of refused roaming authorisations is invalid!");

            }

            #endregion

            #region Get - should return two!

            using (var Response = await CPOClient.GetRoamingAuthorisationList())
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(2, Response.Content.RoamingAuthorisationInfos.Count(), "The number of roaming authorisations at the clearing house is invalid!");

            }

            #endregion


            #region Set2

            using (var Response = await EMPClient.SetRoamingAuthorisationList(
                                                new RoamingAuthorisationInfo[] {

                                                    new RoamingAuthorisationInfo(
                                                        new EMT_Id(
                                                            "3456",
                                                            TokenRepresentations.Plain,
                                                            TokenTypes.RFID, 
                                                            TokenSubTypes.MifareClassic
                                                        ),
                                                        Contract_Id.Parse("DE-GEF-345678912"),
                                                        DateTime.Now + TimeSpan.FromDays(30),
                                                        "Card #3456"
                                                    )

                                                }
                                            ))
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(1, ClearingHouse_RoamingAuthorisationInfos.Count,             "The number of roaming authorisations at the clearing house is invalid!");
                Assert.AreEqual(0, Response.Content.RefusedRoamingAuthorisationInfos.Count(), "The number of refused roaming authorisations is invalid!");

            }

            #endregion

            #region Get - should return one!

            using (var Response = await CPOClient.GetRoamingAuthorisationList())
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(1, Response.Content.RoamingAuthorisationInfos.Count(), "The number of roaming authorisations at the clearing house is invalid!");

            }

            #endregion


            #region Set zero

            using (var Response = await EMPClient.SetRoamingAuthorisationList(
                                                new RoamingAuthorisationInfo[0]
                                            ))
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(0, ClearingHouse_RoamingAuthorisationInfos.Count,             "The number of roaming authorisations at the clearing house is invalid!");
                Assert.AreEqual(0, Response.Content.RefusedRoamingAuthorisationInfos.Count(), "The number of refused roaming authorisations is invalid!");

            }

            #endregion

            #region Get - should return zero!

            using (var Response = await CPOClient.GetRoamingAuthorisationList())
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(0, Response.Content.RoamingAuthorisationInfos.Count(), "The number of roaming authorisations at the clearing house is invalid!");

            }

            #endregion

        }

        #endregion


        //ToDo: update zero

    }

}
