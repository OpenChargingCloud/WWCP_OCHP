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
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// Specifies the OCHP ratings of a charge point.
    /// </summary>
    public class Ratings
    {

        #region Properties

        /// <summary>
        /// The maximum available power at this charge point at nominal voltage over all available phases of the line.
        /// </summary>
        public Single   MaximumPower      { get; }

        /// <summary>
        /// The minimum guaranteed mean power in case of load management. Should be set to maximum when no load management applied.
        /// </summary>
        public Single?  GuaranteedPower   { get; }

        /// <summary>
        /// The nominal voltage for the charge point.
        /// </summary>
        public UInt16?  NominalVoltage    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create new ratings of a charge point.
        /// </summary>
        /// <param name="MaximumPower">The maximum available power at this charge point at nominal voltage over all available phases of the line.</param>
        /// <param name="GuaranteedPower">The minimum guaranteed mean power in case of load management. Should be set to maximum when no load management applied.</param>
        /// <param name="NominalVoltage">The nominal voltage for the charge point.</param>
        public Ratings(Single   MaximumPower,
                       Single?  GuaranteedPower,
                       UInt16?  NominalVoltage)

        {

            this.MaximumPower     = MaximumPower;
            this.GuaranteedPower  = GuaranteedPower;
            this.NominalVoltage   = NominalVoltage;

        }

        #endregion


    }

}
