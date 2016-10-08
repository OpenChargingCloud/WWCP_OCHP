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
    /// Specifies one exceptional period for opening or access hours.
    /// </summary>
    public class ExceptionalPeriods
    {

        #region Properties

        /// <summary>
        /// Starting time of period. Must be equal or later than startDateTime of the CDRInfo.
        /// </summary>
        public DateTime          Start           { get; }

        /// <summary>
        /// Ending time of the period. Must be equal or earlier than endDateTime of the CDRInfo.
        /// </summary>
        public DateTime          End             { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Specifies one exceptional period for opening or access hours.
        /// </summary>
        /// <param name="Start">Starting time of period. Must be equal or later than startDateTime of the CDRInfo.</param>
        /// <param name="End">Ending time of the period. Must be equal or earlier than endDateTime of the CDRInfo.</param>
        public ExceptionalPeriods(DateTime  Start,
                                  DateTime  End)

        {

            this.Start         = Start;
            this.End           = End;

        }

        #endregion


    }

}
