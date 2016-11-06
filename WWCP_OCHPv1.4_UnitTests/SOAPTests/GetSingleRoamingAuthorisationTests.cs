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

using NUnit.Framework;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4.UnitTests
{

    /// <summary>
    /// OCHP GetSingleRoamingAuthorisation unit tests.
    /// </summary>
    [TestFixture]
    public class GetSingleRoamingAuthorisationTests : ASOAPTests
    {

        public GetSingleRoamingAuthorisationTests()
        {

            ClearingHouseServer.OnGetSingleRoamingAuthorisationRequest

                += (Timestamp,
                    Sender,
                    CancellationToken,
                    EventTrackingId,

                    EMTId,

                    QueryTimeout) => {

                        switch (EMTId.Instance)
                        {

                            case "1234":
                                return Task.FromResult(
                                    new CPO.GetSingleRoamingAuthorisationResponse(
                                        new CPO.GetSingleRoamingAuthorisationRequest(EMTId),
                                        Result.OK(),
                                        new RoamingAuthorisationInfo(EMTId,
                                                                     Contract_Id.Parse("DE-GDF-123456789"),
                                                                     DateTime.Now,
                                                                     "User #123456789")
                                    )
                                );

                            default:
                                return Task.FromResult(
                                    new CPO.GetSingleRoamingAuthorisationResponse(
                                        new CPO.GetSingleRoamingAuthorisationRequest(EMTId),
                                        Result.InvalidId()
                                    )
                                );

                        }

                    };

        }


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

            Assert.AreEqual(ResultCodes.OK, Response.Result.ResultCode);

        }

        #endregion

        #region GetSingleRoamingAuthorisationTest2()

        [Test]
        public void GetSingleRoamingAuthorisationTest2()
        {

            var Task1 = CPOClient.GetSingleRoamingAuthorisation(new EMT_Id("567891",
                                                                           TokenRepresentations.Plain,
                                                                           TokenTypes.RFID,
                                                                           TokenSubTypes.MifareClassic));

            Task1.Wait(TimeSpan.FromSeconds(30));

            var Response = Task1.Result.Content;

            Assert.AreEqual(ResultCodes.InvalidId, Response.Result.ResultCode);

        }

        #endregion


    }

}
