/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using NUnit.Framework.Legacy;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.UnitTests
{

    /// <summary>
    /// OCHP parking status unit tests.
    /// </summary>
    [TestFixture]
    public class ParkingStatusTests
    {

        #region ParkingStatus_EqualityTest()

        [Test]
        public void ParkingStatus_EqualityTest()
        {

            var Now = DateTime.Now;

            ClassicAssert.AreEqual   (new ParkingStatus(Parking_Id.Parse("DE*GEF*P1234"), ParkingStatusTypes.Available),
                               new ParkingStatus(Parking_Id.Parse("DE*GEF*P1234"), ParkingStatusTypes.Available));

            ClassicAssert.AreEqual   (new ParkingStatus(Parking_Id.Parse("DE*GEF*P1234"), ParkingStatusTypes.NotAvailable),
                               new ParkingStatus(Parking_Id.Parse("DEGEFP1234"),   ParkingStatusTypes.NotAvailable));

            ClassicAssert.AreEqual   (new ParkingStatus(Parking_Id.Parse("DE*GEF*P1234"), ParkingStatusTypes.Available, Now),
                               new ParkingStatus(Parking_Id.Parse("DE*GEF*P1234"), ParkingStatusTypes.Available, Now));

            ClassicAssert.AreNotEqual(new ParkingStatus(Parking_Id.Parse("DE*GEF*P1234"), ParkingStatusTypes.Available),
                               new ParkingStatus(Parking_Id.Parse("DE*GEF*P5678"), ParkingStatusTypes.Available));

            ClassicAssert.AreNotEqual(new ParkingStatus(Parking_Id.Parse("DE*GEF*P1234"), ParkingStatusTypes.Available),
                               new ParkingStatus(Parking_Id.Parse("DE*GEF*P1234"), ParkingStatusTypes.NotAvailable));

            ClassicAssert.AreNotEqual(new ParkingStatus(Parking_Id.Parse("DE*GEF*P1234"), ParkingStatusTypes.Available),
                               new ParkingStatus(Parking_Id.Parse("DE*GEF*P1234"), ParkingStatusTypes.Available, DateTime.Now));

        }

        #endregion

        #region ParkingStatus_XMLTest()

        [Test]
        public void ParkingStatus_XMLTest()
        {

            var Now = DateTime.Parse(DateTime.Now.ToISO8601()); // Avoid <ms issues!

            var ParkingStatus1 = new ParkingStatus(Parking_Id.Parse("DE*GEF*P1234"), ParkingStatusTypes.Available);
            ClassicAssert.AreEqual(ParkingStatus1, ParkingStatus.Parse(ParkingStatus1.ToXML()));

            ClassicAssert.AreEqual(new XElement(OCHPNS.Default + "parking",
                                new XAttribute(OCHPNS.Default + "status",    "available"),
                                new XElement  (OCHPNS.Default + "parkingId", "DE*GEF*P1234")
                            ).ToString(),
                            ParkingStatus1.ToXML().ToString());


            var ParkingStatus2 = new ParkingStatus(Parking_Id.Parse("DEGEFP1234"), ParkingStatusTypes.NotAvailable, Now);
            ClassicAssert.AreEqual(ParkingStatus2, ParkingStatus.Parse(ParkingStatus2.ToXML()));

            ClassicAssert.AreEqual(new XElement(OCHPNS.Default + "parking",
                                new XAttribute(OCHPNS.Default + "status",    "not-available"),
                                new XAttribute(OCHPNS.Default + "ttl",       Now.ToISO8601()),
                                new XElement  (OCHPNS.Default + "parkingId", "DE*GEF*P1234")
                            ).ToString(),
                            ParkingStatus2.ToXML().ToString());


        }

        #endregion

    }

}
