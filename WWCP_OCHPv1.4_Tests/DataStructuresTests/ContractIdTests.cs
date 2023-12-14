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

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.UnitTests
{

    /// <summary>
    /// OCHP contract identification unit tests.
    /// </summary>
    [TestFixture]
    public class ContractIdTests
    {

        #region ContractId_EqualityTest()

        [Test]
        public void ContractId_EqualityTest()
        {

            ClassicAssert.AreEqual(Contract_Id.Parse("DE-GDF-123456789-1"),
                            Contract_Id.Parse("DEGDF1234567891"));

        }

        #endregion

    }

}
