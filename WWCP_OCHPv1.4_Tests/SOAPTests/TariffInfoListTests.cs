/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;
using cloud.charging.open.protocols.OCHPv1_4.CPO;
using cloud.charging.open.protocols.OCHPv1_4.EMP;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.UnitTests
{

    /// <summary>
    /// Unit tests for manipulating and requesting the list
    /// of tariff infos at the clearing house.
    /// </summary>
    [TestFixture]
    public class TariffInfoListTests : ASOAPTests
    {

        #region Constructor(s)

        /// <summary>
        /// Unit tests for manipulating and requesting the list
        /// of tariff infos at the clearing house.
        /// </summary>
        public TariffInfoListTests()
        {

            #region OnUpdateTariffsRequest

            ClearingHouseServer.OnUpdateTariffsRequest += (Timestamp,
                                                           Sender,
                                                           CancellationToken,
                                                           EventTrackingId,

                                                           TariffInfos,

                                                           Timeout) => {

                                                               var Now = DateTime.Now;

            //                                                   ClearingHouse_TariffInfos.Clear();

                                                               foreach (var tariffinfo in TariffInfos)
                                                                   ClearingHouse_TariffInfos.AddOrUpdate(tariffinfo.TariffId,
                                                                                                         new Timestamped<TariffInfo>(Now, tariffinfo),
                                                                                                         (a, b) => b);

                                                               return Task.FromResult(
                                                                          new CPO.UpdateTariffsResponse(
                                                                              new CPO.UpdateTariffsRequest(TariffInfos),
                                                                              Result.OK()
                                                                          )
                                                                      );

                                                           };

            #endregion

            #region OnGetTariffUpdatesRequest

            ClearingHouseServer.OnGetTariffUpdatesRequest += (Timestamp,
                                                              Sender,
                                                              CancellationToken,
                                                              EventTrackingId,

                                                              LastUpdate,

                                                              Timeout) => {

                                                                  return Task.FromResult(
                                                                             new EMP.GetTariffUpdatesResponse(
                                                                                 new EMP.GetTariffUpdatesRequest(LastUpdate),
                                                                                 Result.OK(),
                                                                                 ClearingHouse_TariffInfos.Values.Select(item => item.Value)
                                                                             )
                                                                         );

                                                              };

            #endregion

        }

        #endregion


        #region SetAndUpdateTariffInfoListTest()

        /// <summary>
        /// Set, get, update and delete the list of tariff infos.
        /// </summary>
        [Test]
        public async Task SetAndUpdateTariffInfoListTest()
        {

            #region Get - should be empty!

            using (var Response = await EMPClient.GetTariffUpdates())
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(0, Response.Content.TariffInfos.Count(), "The number of tariff infos at the clearing house is invalid!");

            }

            #endregion


            #region Set

            using (var Response = await CPOClient.UpdateTariffs(
                                            new TariffInfo[] {

                                                new TariffInfo(

                                                    Tariff_Id.Parse("DE*GEF*T1000"),

                                                    new IndividualTariff[] {

                                                        new IndividualTariff(

                                                            new TariffElement[] {

                                                                new TariffElement(

                                                                    new PriceComponent[] {
                                                                        new PriceComponent(
                                                                            BillingItems.UsageTime,
                                                                            1.0f,
                                                                            60
                                                                        )
                                                                    },

                                                                    new TariffRestriction[] {

                                                                        new TariffRestriction(
                                                                            new RegularHours[] {
                                                                                new RegularHours(
                                                                                    DayOfWeek.Monday,
                                                                                    HourMin.Parse("11:00"),
                                                                                    HourMin.Parse("12:00")
                                                                                )
                                                                            },
                                                                            DateTime.Now,
                                                                            DateTime.Now + TimeSpan.FromDays(30),
                                                                            10.0f,
                                                                            20.0f,
                                                                            30.0f,
                                                                            40.0f,
                                                                            TimeSpan.FromMinutes(5),
                                                                            TimeSpan.FromHours(12)
                                                                        )

                                                                    }

                                                                )

                                                            },

                                                            new String[] {
                                                                "DE*GDF"
                                                            },

                                                            Currency.EUR

                                                        )

                                                    }

                                                )

                                            }
                                        ))
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(1, ClearingHouse_TariffInfos.Count,             "The number of charging tariffs at the clearing house is invalid!");
                Assert.AreEqual(0, Response.Content.RefusedTariffInfos.Count(), "The number of refused charging tariffs is invalid!");

            }

            #endregion

            #region Get - should return one!

            using (var Response = await EMPClient.GetTariffUpdates())
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(1, Response.Content.TariffInfos.Count(), "The number of tariff infos at the clearing house is invalid!");

            }

            #endregion


            #region Set

            using (var Response = await CPOClient.UpdateTariffs(
                                            new TariffInfo[] {

                                                new TariffInfo(

                                                    Tariff_Id.Parse("DE*GEF*T1001"),

                                                    new IndividualTariff[] {

                                                        new IndividualTariff(

                                                            new TariffElement[] {

                                                                new TariffElement(

                                                                    new PriceComponent[] {
                                                                        new PriceComponent(
                                                                            BillingItems.UsageTime,
                                                                            1.0f,
                                                                            60
                                                                        )
                                                                    },

                                                                    new TariffRestriction[] {

                                                                        new TariffRestriction(
                                                                            new RegularHours[] {
                                                                                new RegularHours(
                                                                                    DayOfWeek.Monday,
                                                                                    HourMin.Parse("11:00"),
                                                                                    HourMin.Parse("12:00")
                                                                                )
                                                                            },
                                                                            DateTime.Now,
                                                                            DateTime.Now + TimeSpan.FromDays(30),
                                                                            10.0f,
                                                                            20.0f,
                                                                            30.0f,
                                                                            40.0f,
                                                                            TimeSpan.FromMinutes(5),
                                                                            TimeSpan.FromHours(12)
                                                                        )

                                                                    }

                                                                )

                                                            },

                                                            new String[] {
                                                                "DE*GDF"
                                                            },

                                                            Currency.EUR

                                                        )

                                                    }

                                                )

                                            }
                                        ))
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(2, ClearingHouse_TariffInfos.Count,             "The number of charging tariffs at the clearing house is invalid!");
                Assert.AreEqual(0, Response.Content.RefusedTariffInfos.Count(), "The number of refused charging tariffs is invalid!");

            }

            #endregion

            #region Get - should return two!

            using (var Response = await EMPClient.GetTariffUpdates())
            {

                Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                Assert.AreEqual(2, Response.Content.TariffInfos.Count(), "The number of tariff infos at the clearing house is invalid!");

            }

            #endregion


        }

        #endregion


        //ToDo: update zero

    }

}
