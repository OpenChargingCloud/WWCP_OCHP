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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.UnitTests
{

    /// <summary>
    /// OCHP UpdateChargePointList unit tests.
    /// </summary>
    [TestFixture]
    public class UpdateChargePointListTests : ASOAPTests
    {

        #region Constructor(s)

        public UpdateChargePointListTests()
        {

            ClearingHouseServer.OnUpdateChargePointListRequest += (Timestamp,
                                                                   Sender,
                                                                   CancellationToken,
                                                                   EventTrackingId,

                                                                   ChargePointInfos,

                                                                   Timeout) => {

                                                                       foreach (var chargepointinfo in ChargePointInfos)
                                                                           ClearingHouseChargePointInfos.AddOrUpdate(chargepointinfo.EVSEId,
                                                                                                                     chargepointinfo,
                                                                                                                     (a, b) => b);

                                                                       return Task.FromResult(
                                                                                  new CPO.UpdateChargePointListResponse(
                                                                                      new CPO.UpdateChargePointListRequest(ChargePointInfos),
                                                                                      Result.OK()
                                                                                  )
                                                                              );

                                                                   };

        }

        #endregion


        #region UpdateChargePointListTest1()

        [Test]
        public async Task UpdateChargePointListTest1()
        {

            var Now = DateTime.Parse(DateTime.Now.ToIso8601());

            var Response = await CPOClient.UpdateChargePointList(
                               new ChargePointInfo[] {

                                   new ChargePointInfo(
                                       EVSE_Id.Parse("DE*GEF*E123456789*1"),
                                       "HERE2",
                                       "HERE2",
                                       "DEU",
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
                           );

            Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);


            Assert.AreEqual(1, ClearingHouseChargePointInfos.Count, "The number of charge point infos at the clearing house is invalid!");

            //Assert.IsTrue  (ClearingHouseEVSEStatus.ContainsKey(EVSEId1));
            //Assert.AreEqual(EVSEMajorStatus1_1, ClearingHouseEVSEStatus[EVSEId1].MajorStatus);
            //Assert.IsFalse (ClearingHouseEVSEStatus[EVSEId1].MinorStatus.HasValue);
            //Assert.IsFalse (ClearingHouseEVSEStatus[EVSEId1].TTL.        HasValue);

            //Assert.IsTrue  (ClearingHouseEVSEStatus.ContainsKey(EVSEId2));
            //Assert.AreEqual(EVSEMajorStatus2_1, ClearingHouseEVSEStatus[EVSEId2].MajorStatus);
            //Assert.IsTrue  (ClearingHouseEVSEStatus[EVSEId2].MinorStatus.HasValue);
            //Assert.AreEqual(EVSEMinorStatus2_1, ClearingHouseEVSEStatus[EVSEId2].MinorStatus);
            //Assert.IsFalse (ClearingHouseEVSEStatus[EVSEId2].TTL.        HasValue);

            //Assert.IsTrue  (ClearingHouseEVSEStatus.ContainsKey(EVSEId3));
            //Assert.AreEqual(EVSEMajorStatus3_1, ClearingHouseEVSEStatus[EVSEId3].MajorStatus);
            //Assert.IsTrue  (ClearingHouseEVSEStatus[EVSEId3].MinorStatus.HasValue);
            //Assert.AreEqual(EVSEMinorStatus3_1, ClearingHouseEVSEStatus[EVSEId3].MinorStatus);
            //Assert.IsTrue  (ClearingHouseEVSEStatus[EVSEId3].TTL.        HasValue);
            //Assert.AreEqual(Now + TimeSpan.FromHours(1), ClearingHouseEVSEStatus[EVSEId3].TTL);


        }

        #endregion


    }

}
