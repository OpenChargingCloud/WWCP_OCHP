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

using System.Xml.Linq;

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.UnitTests
{

    /// <summary>
    /// OCHP EVSE status unit tests.
    /// </summary>
    [TestFixture]
    public class EVSEStatusTests
    {

        #region EVSEStatus_IllegalStatusCombinationsTest()

        [Test]
        public void EVSEStatus_IllegalStatusCombinationsTest()
        {

            Assert.Throws<IllegalEVSEStatusCombinationException>(() => new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*1"), EVSEMajorStatusTypes.Available,    EVSEMinorStatusTypes.Charging));
            Assert.Throws<IllegalEVSEStatusCombinationException>(() => new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*2"), EVSEMajorStatusTypes.NotAvailable, EVSEMinorStatusTypes.Available));

        }

        #endregion

        #region EVSEStatus_EqualityTest()

        [Test]
        public void EVSEStatus_EqualityTest()
        {

            var Now = DateTime.Now;

            Assert.AreEqual   (new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*1"), EVSEMajorStatusTypes.Available),
                               new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*1"), EVSEMajorStatusTypes.Available));

            Assert.AreEqual   (new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*2"), EVSEMajorStatusTypes.NotAvailable),
                               new EVSEStatus(EVSE_Id.Parse("DEGEFE1234*2"),   EVSEMajorStatusTypes.NotAvailable));

            Assert.AreEqual   (new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*3"), EVSEMajorStatusTypes.NotAvailable, EVSEMinorStatusTypes.Charging),
                               new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*3"), EVSEMajorStatusTypes.NotAvailable, EVSEMinorStatusTypes.Charging));

            Assert.AreEqual   (new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*3"), EVSEMajorStatusTypes.Available, TTL: Now),
                               new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*3"), EVSEMajorStatusTypes.Available, TTL: Now));

            Assert.AreEqual   (new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*3"), EVSEMajorStatusTypes.NotAvailable, EVSEMinorStatusTypes.Charging, Now),
                               new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*3"), EVSEMajorStatusTypes.NotAvailable, EVSEMinorStatusTypes.Charging, Now));

            Assert.AreNotEqual(new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*4"), EVSEMajorStatusTypes.Available),
                               new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*4"), EVSEMajorStatusTypes.NotAvailable));

            Assert.AreNotEqual(new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*4"), EVSEMajorStatusTypes.NotAvailable, EVSEMinorStatusTypes.Blocked),
                               new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*4"), EVSEMajorStatusTypes.NotAvailable, EVSEMinorStatusTypes.Charging));

            Assert.AreNotEqual(new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*4"), EVSEMajorStatusTypes.Available),
                               new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234*4"), EVSEMajorStatusTypes.Available, TTL: DateTime.Now));

        }

        #endregion

        #region EVSEStatus_XMLTest()

        [Test]
        public void EVSEStatus_XMLTest()
        {

            var Now = DateTime.Parse(DateTime.Now.ToIso8601()); // Avoid <ms issues!

            var EVSEStatus1 = new EVSEStatus(EVSE_Id.Parse("DE*GEF*E1234"), EVSEMajorStatusTypes.Available);
            Assert.AreEqual(EVSEStatus1, EVSEStatus.Parse(EVSEStatus1.ToXML()));

            Assert.AreEqual(new XElement(OCHPNS.Default + "evse",
                                new XAttribute(OCHPNS.Default + "major",   "available"),
                                new XElement  (OCHPNS.Default + "evseId",  "DE*GEF*E1234")
                            ).ToString(),
                            EVSEStatus1.ToXML().ToString());


            var EVSEStatus2 = new EVSEStatus(EVSE_Id.Parse("DEGEFE1234"), EVSEMajorStatusTypes.NotAvailable, TTL: Now);
            Assert.AreEqual(EVSEStatus2, EVSEStatus.Parse(EVSEStatus2.ToXML()));

            Assert.AreEqual(new XElement(OCHPNS.Default + "evse",
                                new XAttribute(OCHPNS.Default + "major",   "not-available"),
                                new XAttribute(OCHPNS.Default + "ttl",     Now.ToIso8601()),
                                new XElement  (OCHPNS.Default + "evseId",  "DE*GEF*E1234")
                            ).ToString(),
                            EVSEStatus2.ToXML().ToString());


            var EVSEStatus3 = new EVSEStatus(EVSE_Id.Parse("DEGEFE1234"), EVSEMajorStatusTypes.NotAvailable, EVSEMinorStatusTypes.Charging);
            Assert.AreEqual(EVSEStatus3, EVSEStatus.Parse(EVSEStatus3.ToXML()));

            Assert.AreEqual(new XElement(OCHPNS.Default + "evse",
                                new XAttribute(OCHPNS.Default + "major",   "not-available"),
                                new XAttribute(OCHPNS.Default + "minor",   "charging"),
                                new XElement  (OCHPNS.Default + "evseId",  "DE*GEF*E1234")
                            ).ToString(),
                            EVSEStatus3.ToXML().ToString());


            var EVSEStatus4 = new EVSEStatus(EVSE_Id.Parse("DEGEFE1234"), EVSEMajorStatusTypes.NotAvailable, EVSEMinorStatusTypes.Reserved, Now);
            Assert.AreEqual(EVSEStatus4, EVSEStatus.Parse(EVSEStatus4.ToXML()));

            Assert.AreEqual(new XElement(OCHPNS.Default + "evse",
                                new XAttribute(OCHPNS.Default + "major",   "not-available"),
                                new XAttribute(OCHPNS.Default + "minor",   "reserved"),
                                new XAttribute(OCHPNS.Default + "ttl",     Now.ToIso8601()),
                                new XElement  (OCHPNS.Default + "evseId",  "DE*GEF*E1234")
                            ).ToString(),
                            EVSEStatus4.ToXML().ToString());


        }

        #endregion

    }

}
