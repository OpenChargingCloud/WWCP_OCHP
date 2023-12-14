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

using NUnit.Framework;
using NUnit.Framework.Legacy;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

using cloud.charging.open.protocols.OCHPv1_4.CPO;
using cloud.charging.open.protocols.OCHPv1_4.EMP;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.UnitTests
{

    /// <summary>
    /// Unit tests for manipulating and requesting the list
    /// of charge points at the clearing house.
    /// </summary>
    [TestFixture]
    public class ChargePointListTests : ASOAPTests
    {

        #region Constructor(s)

        /// <summary>
        /// Unit tests for manipulating and requesting the list
        /// of charge points at the clearing house.
        /// </summary>
        public ChargePointListTests()
        {

            #region OnSetChargePointListRequest

            ClearingHouseServer.OnSetChargePointListRequest    += (Timestamp,
                                                                   Sender,
                                                                   CancellationToken,
                                                                   EventTrackingId,

                                                                   ChargePointInfos,

                                                                   Timeout) => {

                                                                       var Now = DateTime.Now;

                                                                       ClearingHouse_ChargePointInfos.Clear();

                                                                       foreach (var chargepointinfo in ChargePointInfos)
                                                                           ClearingHouse_ChargePointInfos.AddOrUpdate(chargepointinfo.EVSEId,
                                                                                                                      new Timestamped<ChargePointInfo>(Now, chargepointinfo),
                                                                                                                      (a, b) => b);

                                                                       return Task.FromResult(
                                                                                  new CPO.SetChargePointListResponse(
                                                                                      new CPO.SetChargePointListRequest(ChargePointInfos),
                                                                                      Result.OK()
                                                                                  )
                                                                              );

                                                                   };

            #endregion

            #region OnUpdateChargePointListRequest

            ClearingHouseServer.OnUpdateChargePointListRequest += (Timestamp,
                                                                   Sender,
                                                                   CancellationToken,
                                                                   EventTrackingId,

                                                                   ChargePointInfos,

                                                                   Timeout) => {

                                                                       var Now = DateTime.Now;

                                                                       foreach (var chargepointinfo in ChargePointInfos)
                                                                           ClearingHouse_ChargePointInfos.AddOrUpdate(chargepointinfo.EVSEId,
                                                                                                                      new Timestamped<ChargePointInfo>(Now, chargepointinfo),
                                                                                                                      (a, b) => b);

                                                                       return Task.FromResult(
                                                                                  new CPO.UpdateChargePointListResponse(
                                                                                      new CPO.UpdateChargePointListRequest(ChargePointInfos),
                                                                                      Result.OK()
                                                                                  )
                                                                              );

                                                                   };

            #endregion

            #region OnGetChargePointListRequest

            ClearingHouseServer.OnGetChargePointListRequest += (Timestamp,
                                                                Sender,
                                                                CancellationToken,
                                                                EventTrackingId,
                                                                Timeout) => {

                                                                    return Task.FromResult(
                                                                               new EMP.GetChargePointListResponse(
                                                                                   new EMP.GetChargePointListRequest(),
                                                                                   Result.OK(),
                                                                                   ClearingHouse_ChargePointInfos.Values.Select(item => item.Value)
                                                                               )
                                                                           );

                                                                };

            #endregion

        }

        #endregion


        #region SetAndUpdateChargePointListTest()

        /// <summary>
        /// Set, get, update and delete the list of charge points.
        /// </summary>
        [Test]
        public async Task SetAndUpdateChargePointListTest()
        {

            #region Get - should be empty!

            using (var Response = await EMPClient.GetChargePointList())
            {

                ClassicAssert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                ClassicAssert.AreEqual(0, Response.Content.ChargePoints.Count(), "The number of charge points at the clearing house is invalid!");

            }

            #endregion


            #region Set

            using (var Response = await CPOClient.SetChargePointList(
                                            new ChargePointInfo[] {

                                                new ChargePointInfo(
                                                    EVSE_Id.Parse("DE*GEF*E123456789*1"),
                                                    "HERE",
                                                    "HERE",
                                                    Languages3.deu,
                                                    new Address("18", "Bibergweg", "Jena", "07749", Country.Germany),
                                                    new GeoCoordinate(Latitude.Parse(12.1), Longitude.Parse(23.1)),
                                                    GeneralLocationTypes.OnStreet,
                                                    AuthMethodTypes.RFIDMifareClassic | AuthMethodTypes.RFIDMifareDESFire,
                                                    new ConnectorType[] {
                                                        new ConnectorType(
                                                            ConnectorStandards.IEC_62196_T2,
                                                            ConnectorFormats.Socket
                                                        )
                                                    },
                                                    ChargePointTypes.AC,
                                                    DateTime.Now,
                                                    new EVSEImageURL[0],
                                                    new RelatedResource[0],
                                                    new ExtendedGeoCoordinate[0],
                                                    "MEZ",
                                                    Hours.Open24_7,
                                                    ChargePointStatus.Operative,
                                                    new ChargePointSchedule[0],
                                                    "+49 172 555555",
                                                    new ParkingSpotInfo[0],
                                                    RestrictionTypes.EVOnly,
                                                    new Ratings(22, 3.7f, 240),
                                                    new String[] { "deu" },
                                                    TimeSpan.FromMinutes(15)
                                                )

                                            }
                                        ))
            {

                ClassicAssert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                ClassicAssert.AreEqual(1, ClearingHouse_ChargePointInfos.Count,             "The number of roaming authorisations at the clearing house is invalid!");
                ClassicAssert.AreEqual(0, Response.Content.RefusedChargePointInfos.Count(), "The number of refused charge points is invalid!");

            }

            #endregion

            #region Get - should return one!

            using (var Response = await EMPClient.GetChargePointList())
            {

                ClassicAssert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                ClassicAssert.AreEqual(1, Response.Content.ChargePoints.Count(), "The number of charge points at the clearing house is invalid!");

            }

            #endregion


            #region Update

            using (var Response = await CPOClient.UpdateChargePointList(
                                            new ChargePointInfo[] {

                                                new ChargePointInfo(
                                                    EVSE_Id.Parse("DE*GEF*E123456789*2"),
                                                    "HERE2",
                                                    "HERE2",
                                                    Languages3.deu,
                                                    new Address("18", "Bibergweg", "Jena", "07749", Country.Germany),
                                                    new GeoCoordinate(Latitude.Parse(12.1), Longitude.Parse(23.1)),
                                                    GeneralLocationTypes.OnStreet,
                                                    AuthMethodTypes.RFIDMifareClassic | AuthMethodTypes.RFIDMifareDESFire,
                                                    new ConnectorType[] {
                                                        new ConnectorType(
                                                            ConnectorStandards.IEC_62196_T2,
                                                            ConnectorFormats.Socket
                                                        )
                                                    },
                                                    ChargePointTypes.AC,
                                                    DateTime.Now,
                                                    new EVSEImageURL[0],
                                                    new RelatedResource[0],
                                                    new ExtendedGeoCoordinate[0],
                                                    "MEZ",
                                                    Hours.Open24_7,
                                                    ChargePointStatus.Operative,
                                                    new ChargePointSchedule[0],
                                                    "+49 172 555555",
                                                    new ParkingSpotInfo[0],
                                                    RestrictionTypes.EVOnly,
                                                    new Ratings(22, 3.7f, 240),
                                                    new String[] { "deu" },
                                                    TimeSpan.FromMinutes(15)
                                                )

                                            }
                                        ))
            {

                ClassicAssert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                ClassicAssert.AreEqual(2, ClearingHouse_ChargePointInfos.Count, "The number of charge point infos at the clearing house is invalid!");

            }

            #endregion

            #region Get - should return two!

            using (var Response = await EMPClient.GetChargePointList())
            {

                ClassicAssert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                ClassicAssert.AreEqual(2, Response.Content.ChargePoints.Count(), "The number of charge points at the clearing house is invalid!");

            }

            #endregion


            #region Set

            using (var Response = await CPOClient.SetChargePointList(
                                            new ChargePointInfo[] {

                                                new ChargePointInfo(
                                                    EVSE_Id.Parse("DE*GEF*E123456789*3"),
                                                    "HERE",
                                                    "HERE",
                                                    Languages3.deu,
                                                    new Address("18", "Bibergweg", "Jena", "07749", Country.Germany),
                                                    new GeoCoordinate(Latitude.Parse(12.1), Longitude.Parse(23.1)),
                                                    GeneralLocationTypes.OnStreet,
                                                    AuthMethodTypes.RFIDMifareClassic | AuthMethodTypes.RFIDMifareDESFire,
                                                    new ConnectorType[] {
                                                        new ConnectorType(
                                                            ConnectorStandards.IEC_62196_T2,
                                                            ConnectorFormats.Socket
                                                        )
                                                    },
                                                    ChargePointTypes.AC,
                                                    DateTime.Now,
                                                    new EVSEImageURL[0],
                                                    new RelatedResource[0],
                                                    new ExtendedGeoCoordinate[0],
                                                    "MEZ",
                                                    Hours.Open24_7,
                                                    ChargePointStatus.Operative,
                                                    new ChargePointSchedule[0],
                                                    "+49 172 555555",
                                                    new ParkingSpotInfo[0],
                                                    RestrictionTypes.EVOnly,
                                                    new Ratings(22, 3.7f, 240),
                                                    new String[] { "deu" },
                                                    TimeSpan.FromMinutes(15)
                                                )

                                            }
                                        ))
            {

                ClassicAssert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                ClassicAssert.AreEqual(1, ClearingHouse_ChargePointInfos.Count,             "The number of roaming authorisations at the clearing house is invalid!");
                ClassicAssert.AreEqual(0, Response.Content.RefusedChargePointInfos.Count(), "The number of refused charge points is invalid!");

            }

            #endregion

            #region Get - should return one!

            using (var Response = await EMPClient.GetChargePointList())
            {

                ClassicAssert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                ClassicAssert.AreEqual(1, Response.Content.ChargePoints.Count(), "The number of charge points at the clearing house is invalid!");

            }

            #endregion


            #region Set zero

            using (var Response = await CPOClient.SetChargePointList(
                                            new ChargePointInfo[0]
                                        ))
            {

                ClassicAssert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                ClassicAssert.AreEqual(0, ClearingHouse_ChargePointInfos.Count,             "The number of roaming authorisations at the clearing house is invalid!");
                ClassicAssert.AreEqual(0, Response.Content.RefusedChargePointInfos.Count(), "The number of refused charge points is invalid!");

            }

            #endregion

            #region Get - should return zero!

            using (var Response = await EMPClient.GetChargePointList())
            {

                ClassicAssert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);
                ClassicAssert.AreEqual(0, Response.Content.ChargePoints.Count(), "The number of charge points at the clearing house is invalid!");

            }

            #endregion

        }

        #endregion


        //ToDo: update zero

    }

}
