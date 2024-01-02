/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// OCHP opening hours of a charge point.
    /// </summary>
    public class Hours
    {

        #region Properties

        /// <summary>
        /// True to represent 24 hours per day and 7 days per week, except the
        /// given exceptions. May be set to false if opening hours are defined
        /// only by exceptionalOpenings.
        /// </summary>
        public Boolean                         TwentyFourSeven        { get; }

        /// <summary>
        /// Regular hours, weekday based. Should not be set for representing 24/7
        /// as this is the most common case.
        /// </summary>
        public IEnumerable<RegularHours>       RegularHours           { get; }

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

        /// <summary>
        /// Open 24 hours a day.
        /// </summary>
        public static Hours Open24_7
            => new Hours(true);

        #region Constructor(s)

        #region Hours(ClosedCharging, ExceptionalOpenings = null, ExceptionalClosings= null)

        /// <summary>
        /// Create a new 24/7 open OCHP charge point.
        /// </summary>
        /// <param name="ClosedCharging">Should be set to true in case an EV can be charged when plugged in during off-times (i.e. when the location is closed according to the specified hours).</param>
        /// <param name="ExceptionalOpenings">Exceptions for specified calendar dates, time-range based. Periods the station is operating/accessible. For irregular hours or as addition to regular hours. May overlap regular rules.</param>
        /// <param name="ExceptionalClosings">Exceptions for specified calendar dates, time-range based. Periods the station is not operating/accessible. Overwriting regularHours and twentyfourseven. Should not overlap exceptionalOpenings.</param>
        public Hours(Boolean                         ClosedCharging,
                     IEnumerable<ExceptionalPeriod>  ExceptionalOpenings  = null,
                     IEnumerable<ExceptionalPeriod>  ExceptionalClosings  = null)

        {

            this.TwentyFourSeven      = true;
            this.RegularHours         = new RegularHours[0];
            this.ClosedCharging       = ClosedCharging;
            this.ExceptionalOpenings  = ExceptionalOpenings ?? new ExceptionalPeriod[0];
            this.ExceptionalClosings  = ExceptionalClosings ?? new ExceptionalPeriod[0];

        }

        #endregion

        #region Hours(RegularHours, ClosedCharging, ExceptionalOpenings = null, ExceptionalClosings = null)

        /// <summary>
        /// Create new OCHP opening hours for a charge point.
        /// </summary>
        /// <param name="RegularHours">Regular hours, weekday based. Should not be set for representing 24/7 as this is the most common case.</param>
        /// <param name="ClosedCharging">Should be set to true in case an EV can be charged when plugged in during off-times (i.e. when the location is closed according to the specified hours).</param>
        /// <param name="ExceptionalOpenings">Exceptions for specified calendar dates, time-range based. Periods the station is operating/accessible. For irregular hours or as addition to regular hours. May overlap regular rules.</param>
        /// <param name="ExceptionalClosings">Exceptions for specified calendar dates, time-range based. Periods the station is not operating/accessible. Overwriting regularHours and twentyfourseven. Should not overlap exceptionalOpenings.</param>
        public Hours(IEnumerable<RegularHours>       RegularHours,
                     Boolean                         ClosedCharging,
                     IEnumerable<ExceptionalPeriod>  ExceptionalOpenings  = null,
                     IEnumerable<ExceptionalPeriod>  ExceptionalClosings  = null)

        {

            #region Initial checks

            if (RegularHours != null || !RegularHours.Any())
                throw new ArgumentNullException(nameof(RegularHours),  "The given enumeration of regular hours must not be null or empty!");

            #endregion

            this.TwentyFourSeven      = false;
            this.RegularHours         = RegularHours;
            this.ClosedCharging       = ClosedCharging;
            this.ExceptionalOpenings  = ExceptionalOpenings ?? new ExceptionalPeriod[0];
            this.ExceptionalClosings  = ExceptionalClosings ?? new ExceptionalPeriod[0];

        }

        #endregion

        #endregion


        #region Documentation

        // <ns:openingTimes>
        //
        //    <!--You have a CHOICE of the next 2 items at this level-->
        //
        //    <!--1 to 7 repetitions:-->
        //    <ns:regularHours weekday = "?" periodBegin="?" periodEnd="?"/>
        //
        //    <ns:twentyfourseven>?</ns:twentyfourseven>
        //
        //    <!--Zero or more repetitions:-->
        //    <ns:exceptionalOpenings>
        //       <ns:periodBegin>
        //          <ns:DateTime>?</ns:DateTime>
        //       </ns:periodBegin>
        //       <ns:periodEnd>
        //          <ns:DateTime>?</ns:DateTime>
        //       </ns:periodEnd>
        //    </ns:exceptionalOpenings>
        //
        //    <!--Zero or more repetitions:-->
        //    <ns:exceptionalClosings>
        //       <ns:periodBegin>
        //          <ns:DateTime>?</ns:DateTime>
        //       </ns:periodBegin>
        //       <ns:periodEnd>
        //          <ns:DateTime>?</ns:DateTime>
        //       </ns:periodEnd>
        //    </ns:exceptionalClosings>
        //
        //    <ns:closedCharging>?</ns:closedCharging>
        //
        // </ns:openingTimes>

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

        #region (static) Parse(HoursXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of OCHP opening hours of a charge point.
        /// </summary>
        /// <param name="HoursXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Hours Parse(XElement             HoursXML,
                                  OnExceptionDelegate  OnException = null)
        {

            Hours _Hours;

            if (TryParse(HoursXML, out _Hours, OnException))
                return _Hours;

            return null;

        }

        #endregion

        #region (static) Parse(HoursText, OnException = null)

        /// <summary>
        /// Parse the given text representation of OCHP opening hours of a charge point.
        /// </summary>
        /// <param name="HoursText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Hours Parse(String               HoursText,
                                  OnExceptionDelegate  OnException = null)
        {

            Hours _Hours;

            if (TryParse(HoursText, out _Hours, OnException))
                return _Hours;

            return null;

        }

        #endregion

        #region (static) TryParse(HoursXML,  out Hours, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of OCHP opening hours of a charge point.
        /// </summary>
        /// <param name="HoursXML">The XML to parse.</param>
        /// <param name="Hours">The parsed opening hours.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             HoursXML,
                                       out Hours            Hours,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                // Is TwentyFourSeven...
                if (HoursXML.MapValueOrDefault(OCHPNS.Default + "twentyfourseven",
                                               s => s == "true",
                                               false))

                {

                    Hours = new Hours(

                                HoursXML.MapValueOrDefault(OCHPNS.Default + "closedCharging",
                                                           s => s == "true",
                                                           false),

                                HoursXML.MapElements      (OCHPNS.Default + "exceptionalOpenings",
                                                           XML_IO.ParseExceptionalPeriod),

                                HoursXML.MapElements      (OCHPNS.Default + "exceptionalClosings",
                                                           XML_IO.ParseExceptionalPeriod)
                            );

                }

                else
                {

                    Hours = new Hours(

                                HoursXML.MapElementsOrFail(OCHPNS.Default + "regularHours",
                                                           XML_IO.ParseRegularHours),

                                HoursXML.MapValueOrDefault(OCHPNS.Default + "closedCharging",
                                                           s => s == "true",
                                                           false),

                                HoursXML.MapElements      (OCHPNS.Default + "exceptionalOpenings",
                                                           XML_IO.ParseExceptionalPeriod),

                                HoursXML.MapElements      (OCHPNS.Default + "exceptionalClosings",
                                                           XML_IO.ParseExceptionalPeriod)

                            );

                }

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, HoursXML, e);

                Hours = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(HoursText, out Hours, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of OCHP opening hours of a charge point.
        /// </summary>
        /// <param name="HoursText">The text to parse.</param>
        /// <param name="Hours">The parsed opening hours.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               HoursText,
                                       out Hours            Hours,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(HoursText).Root,
                             out Hours,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(org.GraphDefined.Vanaheimr.Illias.Timestamp.Now, HoursText, e);
            }

            Hours = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">An alternative XML element name [default: "OCHPNS:openingTimes"]</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OCHPNS.Default + "openingTimes",

                   !TwentyFourSeven && RegularHours != null
                       ? RegularHours.SafeSelect(hours => hours.ToXML())
                       : null,

                   TwentyFourSeven
                       ? new XElement(OCHPNS.Default + "twentyfourseven", true)
                       : null,

                   ExceptionalOpenings?.SafeSelect(openings => openings.ToXML(OCHPNS.Default + "exceptionalOpenings")),

                   ExceptionalClosings?.SafeSelect(closings => closings.ToXML(OCHPNS.Default + "exceptionalClosings")),

                   new XElement(OCHPNS.Default + "closedCharging",  ClosedCharging)

               );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(TwentyFourSeven ? "24/7" : RegularHours.AggregateWith(", "));

        #endregion

    }

}
