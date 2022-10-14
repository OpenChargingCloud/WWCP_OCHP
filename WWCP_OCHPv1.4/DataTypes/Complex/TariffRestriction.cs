/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// An OCHP tariff restriction.
    /// </summary>
    public class TariffRestriction
    {

        #region Properties

        /// <summary>
        /// Regular hours that this tariff element should be valid for (maximum of 14 entries).
        /// If always valid (24/7), don't set (as this is a tariff restriction).
        /// </summary>
        public IEnumerable<RegularHours>  RegularHours   { get; }

        /// <summary>
        /// Start date, for example: 2015-12-24, valid from this day (midnight, i.e. including this day).
        /// </summary>
        public DateTime?                  StartDate      { get; }

        /// <summary>
        /// End date, for example: 2015-12-27, valid until this day (midnight, i.e. excluding this day).
        /// </summary>
        public DateTime?                  EndDate        { get; }

        /// <summary>
        /// Minimum used energy in kWh, for example 20.0, valid from this amount of energy is used.
        /// </summary>
        public Single?                    MinEnergy      { get; }

        /// <summary>
        /// Maximum used energy in kWh, for example 50.0, valid until this amount of energy is used.
        /// </summary>
        public Single?                    MaxEnergy      { get; }

        /// <summary>
        /// Minimum power in kW, for example 0.0, valid from this charging speed.
        /// </summary>
        public Single?                    MinPower       { get; }

        /// <summary>
        /// Maximum power in kW, for example 20.0, valid up to this charging speed.
        /// </summary>
        public Single?                    MaxPower       { get; }

        /// <summary>
        /// Minimum duration, valid for the given amount of time.
        /// </summary>
        public TimeSpan?                  MinDuration    { get; }

        /// <summary>
        /// Maximum duration, valid for the given amount of time.
        /// </summary>
        public TimeSpan?                  MaxDuration    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP parking tariff restriction.
        /// </summary>
        /// <param name="RegularHours">An enumeration of regular hours that this tariff element should be valid for (maximum of 14 entries). If always valid (24/7), don't set (as this is a tariff restriction).</param>
        /// <param name="StartDate">Start date, for example: 2015-12-24, valid from this day (midnight, i.e. including this day).</param>
        /// <param name="EndDate">End date, for example: 2015-12-27, valid until this day (midnight, i.e. excluding this day).</param>
        /// <param name="MinEnergy">Minimum used energy in kWh, for example 20.0, valid from this amount of energy is used.</param>
        /// <param name="MaxEnergy">Maximum used energy in kWh, for example 50.0, valid until this amount of energy is used.</param>
        /// <param name="MinPower">Minimum power in kW, for example 0.0, valid from this charging speed.</param>
        /// <param name="MaxPower">Maximum power in kW, for example 20.0, valid up to this charging speed.</param>
        /// <param name="MinDuration">Minimum duration, valid for the given amount of time.</param>
        /// <param name="MaxDuration">Maximum duration, valid for the given amount of time.</param>
        public TariffRestriction(IEnumerable<RegularHours>  RegularHours,
                                 DateTime?                  StartDate,
                                 DateTime?                  EndDate,
                                 Single?                    MinEnergy,
                                 Single?                    MaxEnergy,
                                 Single?                    MinPower,
                                 Single?                    MaxPower,
                                 TimeSpan?                  MinDuration,
                                 TimeSpan?                  MaxDuration)
        {

            this.RegularHours  = RegularHours;
            this.StartDate     = StartDate;
            this.EndDate       = EndDate;
            this.MinEnergy     = MinEnergy;
            this.MaxEnergy     = MaxEnergy;
            this.MinPower      = MinPower;
            this.MaxPower      = MaxPower;
            this.MinDuration   = MinDuration;
            this.MaxDuration   = MaxDuration;

        }

        #endregion


        #region Documentation

        // <ns:tariffRestriction>
        //
        //    <!--0 to 14 repetitions:-->
        //    <ns:regularHours weekday = "?" periodBegin="?" periodEnd="?"/>
        //
        //    <!--Optional:-->
        //    <ns:startDateTime>
        //       <ns:DateTime>?</ns:DateTime>
        //    </ns:startDateTime>
        //
        //    <!--Optional:-->
        //    <ns:endDateTime>
        //       <ns:DateTime>?</ns:DateTime>
        //    </ns:endDateTime>
        //
        //    <ns:minEnergy>?</ns:minEnergy>
        //    <ns:maxEnergy>?</ns:maxEnergy>
        //    <ns:minPower>?</ns:minPower>
        //    <ns:maxPower>?</ns:maxPower>
        //    <ns:minDuration>?</ns:minDuration>
        //    <ns:maxDuration>?</ns:maxDuration>
        //
        // </ns:tariffRestriction>

        #endregion

        #region (static) Parse(TariffRestrictionXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of OCHP tariff restriction.
        /// </summary>
        /// <param name="TariffRestrictionXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static TariffRestriction Parse(XElement             TariffRestrictionXML,
                                              OnExceptionDelegate  OnException = null)
        {

            TariffRestriction _TariffRestriction;

            if (TryParse(TariffRestrictionXML, out _TariffRestriction, OnException))
                return _TariffRestriction;

            return null;

        }

        #endregion

        #region (static) Parse(TariffRestrictionText, OnException = null)

        /// <summary>
        /// Parse the given text representation of OCHP tariff restriction.
        /// </summary>
        /// <param name="TariffRestrictionText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static TariffRestriction Parse(String               TariffRestrictionText,
                                              OnExceptionDelegate  OnException = null)
        {

            TariffRestriction _TariffRestriction;

            if (TryParse(TariffRestrictionText, out _TariffRestriction, OnException))
                return _TariffRestriction;

            return null;

        }

        #endregion

        #region (static) TryParse(TariffRestrictionXML,  out TariffRestriction, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of OCHP tariff restriction.
        /// </summary>
        /// <param name="TariffRestrictionXML">The XML to parse.</param>
        /// <param name="TariffRestriction">The parsed tariff restriction.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement               TariffRestrictionXML,
                                       out TariffRestriction  TariffRestriction,
                                       OnExceptionDelegate    OnException  = null)
        {

            try
            {

                TariffRestriction = new TariffRestriction(

                                        TariffRestrictionXML.MapElements       (OCHPNS.Default + "regularHours",
                                                                                XML_IO.ParseRegularHours,
                                                                                OnException),

                                        TariffRestrictionXML.MapValueOrNullable(OCHPNS.Default + "startDateTime",
                                                                                OCHPNS.Default + "DateTime",
                                                                                DateTime.Parse),

                                        TariffRestrictionXML.MapValueOrNullable(OCHPNS.Default + "endDateTime",
                                                                                OCHPNS.Default + "DateTime",
                                                                                DateTime.Parse),

                                        TariffRestrictionXML.MapValueOrNullable(OCHPNS.Default + "minEnergy",
                                                                                Single.Parse),

                                        TariffRestrictionXML.MapValueOrNullable(OCHPNS.Default + "maxEnergy",
                                                                                Single.Parse),

                                        TariffRestrictionXML.MapValueOrNullable(OCHPNS.Default + "minPower",
                                                                                Single.Parse),

                                        TariffRestrictionXML.MapValueOrNullable(OCHPNS.Default + "maxPower",
                                                                                Single.Parse),

                                        TariffRestrictionXML.MapValueOrNullable(OCHPNS.Default + "minDuration",
                                                                                value => TimeSpan.FromSeconds(UInt32.Parse(value))),

                                        TariffRestrictionXML.MapValueOrNullable(OCHPNS.Default + "maxDuration",
                                                                                value => TimeSpan.FromSeconds(UInt32.Parse(value)))

                                    );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, TariffRestrictionXML, e);

                TariffRestriction = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(TariffRestrictionText, out TariffRestriction, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of OCHP tariff restriction.
        /// </summary>
        /// <param name="TariffRestrictionText">The text to parse.</param>
        /// <param name="TariffRestriction">The parsed tariff restriction.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                 TariffRestrictionText,
                                       out TariffRestriction  TariffRestriction,
                                       OnExceptionDelegate    OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(TariffRestrictionText).Root,
                             out TariffRestriction,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, TariffRestrictionText, e);
            }

            TariffRestriction = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">An alternative XML element name [default: "OCHPNS:tariffRestriction"]</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OCHPNS.Default + "tariffRestriction",

                   RegularHours != null
                       ? RegularHours.SafeSelect(hours => hours.ToXML())
                       : null,

                   StartDate.HasValue
                       ? new XElement(OCHPNS.Default + "startDateTime",
                             new XElement(OCHPNS.Default + "DateTime",  StartDate.Value.ToString("yyyy-MM-dd"))
                         )
                       : null,

                   EndDate.HasValue
                       ? new XElement(OCHPNS.Default + "endDateTime",
                             new XElement(OCHPNS.Default + "DateTime",  EndDate.  Value.ToString("yyyy-MM-dd"))
                         )
                       : null,

                   MinEnergy.HasValue
                       ? new XElement(OCHPNS.Default + "minEnergy",     MinEnergy.Value)
                       : null,

                   MaxEnergy.HasValue
                       ? new XElement(OCHPNS.Default + "maxEnergy",     MaxEnergy.Value)
                       : null,

                   MinPower.HasValue
                       ? new XElement(OCHPNS.Default + "minPower",      MinPower.Value)
                       : null,

                   MaxPower.HasValue
                       ? new XElement(OCHPNS.Default + "maxPower",      MaxPower.Value)
                       : null,

                   MinDuration.HasValue
                       ? new XElement(OCHPNS.Default + "minDuration",   MinDuration.Value)
                       : null,

                   MaxDuration.HasValue
                       ? new XElement(OCHPNS.Default + "maxDuration",   MaxDuration.Value)
                       : null

               );

        #endregion


    }

}
