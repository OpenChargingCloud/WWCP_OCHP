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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// This type is used to schedule status periods in the future.
    /// The NSP can provide this information to the EV user for trip planning purpose.
    /// A period MAY have no end.
    /// Example: "This station will be running from tomorrow. Today it is still planned and under construction."
    /// </summary>
    public class ChargePointSchedule
    {

        #region Properties

        /// <summary>
        /// Status value during the scheduled period.
        /// </summary>
        public ChargePointStatusTypes  ChargePointStatus    { get; }

        /// <summary>
        /// Begin of the scheduled period.
        /// </summary>
        public DateTime                StartDate            { get; }

        /// <summary>
        /// End of the scheduled period, if known.
        /// </summary>
        public DateTime?               EndDate              { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new future status period schedule.
        /// </summary>
        /// <param name="ChargePointStatus">Status value during the scheduled period.</param>
        /// <param name="StartDate">Begin of the scheduled period.</param>
        /// <param name="EndDate">End of the scheduled period, if known.</param>
        public ChargePointSchedule(ChargePointStatusTypes  ChargePointStatus,
                                   DateTime                StartDate,
                                   DateTime?               EndDate  = null)
        {

            this.ChargePointStatus  = ChargePointStatus;
            this.StartDate          = StartDate;
            this.EndDate            = EndDate;

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(ChargePointStatus, " from ", StartDate.ToIso8601(), EndDate.HasValue ? " to " + EndDate.Value.ToIso8601() : "");

        #endregion

    }

}
