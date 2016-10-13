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

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// OCHP XML I/O for WWCP data structures.
    /// </summary>
    public static class XML_IO
    {

        #region ParseGeoCoordinate    (XML, OnException = null)

        #region Documentation

        // <OCHPNS:chargePointLocation lat="?" lon="?" />

        #endregion

        public static GeoCoordinate ParseGeoCoordinate(XElement             XML,
                                                       OnExceptionDelegate  OnException = null)
        {

            try
            {

                return new GeoCoordinate(
                           new Latitude (Double.Parse(XML.Attribute(OCHPNS.Default + "lat").Value)),
                           new Longitude(Double.Parse(XML.Attribute(OCHPNS.Default + "lon").Value))
                       );

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, XML, e);

                return null;

            }

        }

        #endregion


        #region ParseRegularHours     (XML, OnException = null)

        #region Documentation

        // <OCHPNS:regularHours weekday="1" periodBegin="08:00" periodEnd="20:00">
        // <OCHPNS:regularHours weekday="2" periodBegin="08:00" periodEnd="20:00">
        // <OCHPNS:regularHours weekday="3" periodBegin="08:00" periodEnd="20:00">
        // <OCHPNS:regularHours weekday="4" periodBegin="08:00" periodEnd="20:00">
        // <OCHPNS:regularHours weekday="5" periodBegin="08:00" periodEnd="20:00">
        // <OCHPNS:regularHours weekday="6" periodBegin="10:00" periodEnd="16:00">

        #endregion

        public static RegularHours ParseRegularHours(XElement             XML,
                                                     OnExceptionDelegate  OnException = null)
        {

            try
            {

                return new RegularHours(

                           XML.MapAttributeValueOrFail(OCHPNS.Default + "weekday",
                                                       ObjectMapper.AsDayOfWeek,
                                                       "Invalid or missing XML attribute 'weekday'!"),

                           XML.MapAttributeValueOrFail(OCHPNS.Default + "periodBegin",
                                                       HourMin.Parse,
                                                       "Invalid or missing XML attribute 'periodBegin'!"),

                           XML.MapAttributeValueOrFail(OCHPNS.Default + "periodEnd",
                                                       HourMin.Parse,
                                                       "Invalid or missing XML attribute 'periodEnd'!")

                       );

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, XML, e);

                return default(RegularHours);

            }

        }

        #endregion

        #region ToXML(this RegularHours, XName = null)

        public static XElement ToXML(this RegularHours  RegularHours,
                                     XName              XName  = null)

            => new XElement(XName ?? OCHPNS.Default + "regularHours",

                                new XAttribute(OCHPNS.Default + "weekday",      ObjectMapper.AsInt(RegularHours.DayOfWeek)),
                                new XAttribute(OCHPNS.Default + "periodBegin",  RegularHours.PeriodBegin.ToString()),
                                new XAttribute(OCHPNS.Default + "periodEnd",    RegularHours.PeriodEnd.  ToString())

                            );

        #endregion


        #region ParseExceptionalPeriod(XML, OnException = null)

        public static ExceptionalPeriod ParseExceptionalPeriod(XElement             XML,
                                                               OnExceptionDelegate  OnException = null)
        {

            try
            {

                return new ExceptionalPeriod(

                           DateTime.Parse(XML.ElementOrFail(OCHPNS.Default + "periodBegin",
                                                            "The XML element 'periodBegin' is invalid or missing!").Value),

                           DateTime.Parse(XML.ElementOrFail(OCHPNS.Default + "periodEnd",
                                                            "The XML element 'periodEnd' is invalid or missing!").Value)

                       );

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, XML, e);

                return default(ExceptionalPeriod);

            }

        }

        #endregion

        #region ToXML(this ExceptionalPeriod, XName = null)

        public static XElement ToXML(this ExceptionalPeriod  ExceptionalPeriod,
                                     XName                   XName  = null)

            => new XElement(XName ?? OCHPNS.Default + "exceptionalOpenings",

                                new XElement(OCHPNS.Default + "periodBegin",
                                    new XElement(OCHPNS.Default + "DateTime", ExceptionalPeriod.Begin.ToIso8601())
                                ),

                                new XElement(OCHPNS.Default + "periodEnd",
                                    new XElement(OCHPNS.Default + "DateTime", ExceptionalPeriod.End.  ToIso8601())
                                )

                            );

        #endregion


    }

}