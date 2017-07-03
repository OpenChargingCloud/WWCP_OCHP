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
using System.Threading.Tasks;

using NUnit.Framework;

using org.GraphDefined.WWCP.OCHPv1_4.CPO;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.UnitTests
{

    /// <summary>
    /// Unit tests for manipulating and requesting the list
    /// of charge detail records at the clearing house.
    /// </summary>
    [TestFixture]
    public class CDRsTests : ASOAPTests
    {

        #region Constructor(s)

        /// <summary>
        /// Unit tests for manipulating and requesting the list
        /// of charge detail records at the clearing house.
        /// </summary>
        public CDRsTests()
        {

            #region OnAddCDRsRequest...

            ClearingHouseServer.OnAddCDRsRequest += (Timestamp,
                                                     Sender,
                                                     CancellationToken,
                                                     EventTrackingId,

                                                     CDRInfos,

                                                     Timeout) => {

                                                         var Now = DateTime.Now;

                                                         foreach (var cdrinfo in CDRInfos)
                                                             ClearingHouse_CDRInfos.AddOrUpdate(cdrinfo.CDRId,
                                                                                                new Timestamped<CDRInfo>(Now, cdrinfo),
                                                                                                (a, b) => b);

                                                         return Task.FromResult(
                                                                    new CPO.AddCDRsResponse(
                                                                        new CPO.AddCDRsRequest(CDRInfos),
                                                                        Result.OK()
                                                                    )
                                                                );

                                                     };

            #endregion

        }

        #endregion


        #region AddCDRsTests1()

        [Test]
        public async Task AddCDRsTests1()
        {

            var Now = DateTime.Parse(DateTime.Now.ToIso8601());

            var Response = await CPOClient.AddCDRs(
                               new CDRInfo[] {

                                   new CDRInfo(
                                       CDR_Id. Parse("DEGEF1234AABBCC5678"),
                                       new EMT_Id(
                                           "CAFEBABE23",
                                           TokenRepresentations.Plain,
                                           TokenTypes.Remote,
                                           TokenSubTypes.MifareClassic
                                       ),
                                       Contract_Id.Parse("DE-GDF-123456789"),

                                       EVSE_Id.Parse("DE*GEF*E123456789*1"),
                                       ChargePointTypes.AC,
                                       new ConnectorType(
                                           ConnectorStandards.IEC_62196_T2,
                                           ConnectorFormats.Socket
                                       ),

                                       CDRStatus.New,
                                       DateTime.Now - TimeSpan.FromHours(1),
                                       DateTime.Now,
                                       new CDRPeriod[] {

                                           new CDRPeriod(
                                               DateTime.Now - TimeSpan.FromHours(1),
                                               DateTime.Now,
                                               BillingItems.UsageTime,
                                               23.5f,
                                               23.5f
                                           )

                                       },
                                       Currency.EUR,

                                       TimeSpan.FromHours(1),
                                       new Address(
                                           "18",
                                           "Biberweg",
                                           "Jena",
                                           "07749",
                                           Country.Germany
                                       ),
                                       new Ratings(0.0f, 1.0f, 240),
                                       "MeterId #2305",
                                       23.5f
                                   )

                               }
                           ).ConfigureAwait(false);

            Assert.AreEqual(ResultCodes.OK, Response.Content.Result.ResultCode);


            Assert.AreEqual(1, ClearingHouse_CDRInfos.Count, "The number of charge detail records at the clearing house is invalid!");

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
