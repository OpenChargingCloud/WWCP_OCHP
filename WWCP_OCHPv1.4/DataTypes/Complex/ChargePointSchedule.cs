/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// An OCHP schedule for future charge point status periods.
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
        public ChargePointStatus  ChargePointStatus    { get; }

        /// <summary>
        /// Begin of the scheduled period.
        /// </summary>
        public DateTime           StartDate            { get; }

        /// <summary>
        /// End of the scheduled period, if known.
        /// </summary>
        public DateTimeOffset?    EndDate              { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP schedule of future status periods.
        /// </summary>
        /// <param name="ChargePointStatus">Status value during the scheduled period.</param>
        /// <param name="StartDate">Begin of the scheduled period.</param>
        /// <param name="EndDate">End of the scheduled period, if known.</param>
        public ChargePointSchedule(ChargePointStatus  ChargePointStatus,
                                   DateTime           StartDate,
                                   DateTimeOffset?    EndDate  = null)
        {

            this.ChargePointStatus  = ChargePointStatus;
            this.StartDate          = StartDate;
            this.EndDate            = EndDate;

        }

        #endregion


        #region Documentation

        // <ns:statusSchedule>
        //
        //    <ns:startDate>
        //       <ns:DateTime>?</ns:DateTime>
        //    </ns:startDate>
        //
        //    <!--Optional:-->
        //    <ns:endDate>
        //       <ns:DateTime>?</ns:DateTime>
        //    </ns:endDate>
        //
        //    <ns:status>
        //       <ns:ChargePointStatusType>?</ns:ChargePointStatusType>
        //    </ns:status>
        //
        // </ns:statusSchedule>

        #endregion

        #region (static) Parse(ChargePointScheduleXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP status schedule.
        /// </summary>
        /// <param name="ChargePointScheduleXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ChargePointSchedule Parse(XElement             ChargePointScheduleXML,
                                                OnExceptionDelegate  OnException = null)
        {

            ChargePointSchedule _ChargePointSchedule;

            if (TryParse(ChargePointScheduleXML, out _ChargePointSchedule, OnException))
                return _ChargePointSchedule;

            return null;

        }

        #endregion

        #region (static) Parse(ChargePointScheduleText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP status schedule.
        /// </summary>
        /// <param name="ChargePointScheduleText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ChargePointSchedule Parse(String               ChargePointScheduleText,
                                                OnExceptionDelegate  OnException = null)
        {

            ChargePointSchedule _ChargePointSchedule;

            if (TryParse(ChargePointScheduleText, out _ChargePointSchedule, OnException))
                return _ChargePointSchedule;

            return null;

        }

        #endregion

        #region (static) TryParse(ChargePointScheduleXML,  out ChargePointSchedule, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP status schedule.
        /// </summary>
        /// <param name="ChargePointScheduleXML">The XML to parse.</param>
        /// <param name="ChargePointSchedule">The parsed status schedule.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                 ChargePointScheduleXML,
                                       out ChargePointSchedule  ChargePointSchedule,
                                       OnExceptionDelegate      OnException  = null)
        {

            try
            {

                ChargePointSchedule = new ChargePointSchedule(

                                          ChargePointScheduleXML.MapValueOrFail    (OCHPNS.Default + "status",
                                                                                    OCHPNS.Default + "ChargePointStatusType",
                                                                                    XML_IO.AsChargePointStatus),

                                          ChargePointScheduleXML.MapValueOrFail    (OCHPNS.Default + "startDate",
                                                                                    OCHPNS.Default + "DateTime",
                                                                                    DateTime.Parse),

                                          ChargePointScheduleXML.MapValueOrNullable(OCHPNS.Default + "endDate",
                                                                                    OCHPNS.Default + "DateTime",
                                                                                    DateTime.Parse)

                                      );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, ChargePointScheduleXML, e);

                ChargePointSchedule = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ChargePointScheduleText, out ChargePointSchedule, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP status schedule.
        /// </summary>
        /// <param name="ChargePointScheduleText">The text to parse.</param>
        /// <param name="ChargePointSchedule">The parsed status schedule.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                   ChargePointScheduleText,
                                       out ChargePointSchedule  ChargePointSchedule,
                                       OnExceptionDelegate      OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ChargePointScheduleText).Root,
                             out ChargePointSchedule,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, ChargePointScheduleText, e);
            }

            ChargePointSchedule = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "statusSchedule",

                   new XElement(OCHPNS.Default + "startDate",
                       new XElement(OCHPNS.Default + "DateTime", StartDate.ToISO8601())
                   ),

                   EndDate.HasValue
                       ? new XElement(OCHPNS.Default + "endDate",
                             new XElement(OCHPNS.Default + "DateTime", EndDate.Value.ToISO8601())
                         )
                       : null,

                   new XElement(OCHPNS.Default + "status",
                       new XElement(OCHPNS.Default + "ChargePointStatusType", XML_IO.AsText(ChargePointStatus))
                   )

               );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(ChargePointStatus, " from ", StartDate.ToISO8601(), EndDate.HasValue ? " to " + EndDate.Value.ToISO8601() : "");

        #endregion

    }

}
