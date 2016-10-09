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
using System.Collections.Generic;
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// OCHP opening hours of a charge point.
    /// </summary>
    public class Hours
    {

        #region Properties

        /// <summary>
        /// Regular hours, weekday based. Should not be set for representing 24/7
        /// as this is the most common case.
        /// </summary>
        public IEnumerable<RegularHours>       RegularHours           { get; }

        /// <summary>
        /// True to represent 24 hours per day and 7 days per week, except the
        /// given exceptions. May be set to false if opening hours are defined
        /// only by exceptionalOpenings.
        /// </summary>
        public Boolean                         TwentyFourSeven        { get; }

        /// <summary>
        /// Should be set to true in case an EV can be charged when plugged in
        /// during off-times (i.e. when the location is closed according to
        /// the specified hours).
        /// </summary>
        public Boolean                         ClosedCharging         { get; }

        /// <summary>
        /// Exceptions for specified calendar dates, time-range based. Periods the
        /// station is operating/accessible. For irregular hours or as addition to
        /// regular hours. May overlap regular rules.
        /// </summary>
        public IEnumerable<ExceptionalPeriod>  ExceptionalOpenings    { get; }

        /// <summary>
        /// Exceptions for specified calendar dates, time-range based. Periods the
        /// station is not operating/accessible. Overwriting regularHours and
        /// twentyfourseven. Should not overlap exceptionalOpenings.
        /// </summary>
        public IEnumerable<ExceptionalPeriod>  ExceptionalClosings    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create new OCHP opening hours for a charge point.
        /// </summary>
        /// <param name="RegularHours">Regular hours, weekday based. Should not be set for representing 24/7 as this is the most common case.</param>
        /// <param name="TwentyFourSeven">True to represent 24 hours per day and 7 days per week, except the given exceptions. May be set to false if opening hours are defined only by exceptionalOpenings.</param>
        /// <param name="ClosedCharging">Should be set to true in case an EV can be charged when plugged in during off-times (i.e. when the location is closed according to the specified hours).</param>
        /// <param name="ExceptionalOpenings">Exceptions for specified calendar dates, time-range based. Periods the station is operating/accessible. For irregular hours or as addition to regular hours. May overlap regular rules.</param>
        /// <param name="ExceptionalClosings">Exceptions for specified calendar dates, time-range based. Periods the station is not operating/accessible. Overwriting regularHours and twentyfourseven. Should not overlap exceptionalOpenings.</param>
        public Hours(IEnumerable<RegularHours>       RegularHours,
                     Boolean                         TwentyFourSeven,
                     Boolean                         ClosedCharging,
                     IEnumerable<ExceptionalPeriod>  ExceptionalOpenings,
                     IEnumerable<ExceptionalPeriod>  ExceptionalClosings)

        {

            this.RegularHours         = RegularHours;
            this.TwentyFourSeven      = TwentyFourSeven;
            this.ClosedCharging       = ClosedCharging;
            this.ExceptionalOpenings  = ExceptionalOpenings;
            this.ExceptionalClosings  = ExceptionalClosings;

        }

        #endregion


        #region Documentation

        // -- Operating 24/7 except for New Year 2015 -------------------------------
        //
        // <operatingTimes>
        //
        //    <twentyfourseven>true</twentyfourseven>
        //
        //    <exceptionalClosings>
        //       <periodBegin>
        //          <DateTime>2015-01-01T00:00:00Z</DateTime>
        //       </periodBegin>
        //       <periodEnd>
        //          <DateTime>2015-01-02T00:00:00Z</DateTime>
        //       </periodEnd>
        //    </exceptionalClosings>
        //
        // </operatingTimes>


        // -- Operating on Weekdays from 8am until 8pm and Saturdays from 10am ------
        //    until 4pm with one exceptional opening on 22/6/2014 and one
        //    exceptional closing the Tuesday after
        // 
        // <operatingTimes>
        //
        //      <regularHours weekday = "1" periodBegin="08:00" periodEnd="20:00">
        //      <regularHours weekday = "2" periodBegin="08:00" periodEnd="20:00">
        //      <regularHours weekday = "3" periodBegin="08:00" periodEnd="20:00">
        //      <regularHours weekday = "4" periodBegin="08:00" periodEnd="20:00">
        //      <regularHours weekday = "5" periodBegin="08:00" periodEnd="20:00">
        //      <regularHours weekday = "6" periodBegin="10:00" periodEnd="16:00">
        //
        //      <exceptionalOpenings>
        //         <periodBegin>
        //             <DateTime>2014-06-22T09:00:00Z</DateTime>
        //         </periodBegin>
        //         <periodEnd>
        //             <DateTime>2014-06-22T12:00:00Z</DateTime>
        //         </periodEnd>
        //      </exceptionalOpenings>
        //
        //      <exceptionalClosings>
        //         <periodBegin>
        //             <DateTime>2014-06-24T00:00:00Z</DateTime>
        //         </periodBegin>
        //         <periodEnd>
        //             <DateTime>2014-06-25T00:00:00Z</DateTime>
        //         </periodEnd>
        //      </exceptionalClosings>
        //
        // </operatingTimes>

        #endregion


    }

}
