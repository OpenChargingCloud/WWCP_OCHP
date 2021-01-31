/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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


        #region Documentation

        // <ns:ratings>
        //
        //    <ns:maximumPower>?</ns:maximumPower>
        //
        //    <!--Optional:-->
        //    <ns:guaranteedPower>?</ns:guaranteedPower>
        //
        //    <!--Optional:-->
        //    <ns:nominalVoltage>?</ns:nominalVoltage>
        //
        // </ns:ratings>

        #endregion

        #region (static) Parse(RatingsXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of OCHP ratings of a charge point.
        /// </summary>
        /// <param name="RatingsXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Ratings Parse(XElement             RatingsXML,
                                    OnExceptionDelegate  OnException = null)
        {

            Ratings _Ratings;

            if (TryParse(RatingsXML, out _Ratings, OnException))
                return _Ratings;

            return null;

        }

        #endregion

        #region (static) Parse(RatingsText, OnException = null)

        /// <summary>
        /// Parse the given text representation of OCHP ratings of a charge point.
        /// </summary>
        /// <param name="RatingsText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Ratings Parse(String               RatingsText,
                                    OnExceptionDelegate  OnException = null)
        {

            Ratings _Ratings;

            if (TryParse(RatingsText, out _Ratings, OnException))
                return _Ratings;

            return null;

        }

        #endregion

        #region (static) TryParse(RatingsXML,  out Ratings, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of OCHP ratings of a charge point.
        /// </summary>
        /// <param name="RatingsXML">The XML to parse.</param>
        /// <param name="Ratings">The parsed OCHP ratings of a charge point.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             RatingsXML,
                                       out Ratings          Ratings,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                Ratings = new Ratings(

                              RatingsXML.MapValueOrFail   (OCHPNS.Default + "maximumPower",
                                                           Single.Parse),

                              RatingsXML.MapValueOrDefault(OCHPNS.Default + "guaranteedPower",
                                                           Single.Parse),

                              RatingsXML.MapValueOrDefault(OCHPNS.Default + "nominalVoltage",
                                                           UInt16.Parse)

                          );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, RatingsXML, e);

                Ratings = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(RatingsText, out Ratings, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of OCHP ratings of a charge point.
        /// </summary>
        /// <param name="RatingsText">The text to parse.</param>
        /// <param name="Ratings">The parsed OCHP ratings of a charge point.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               RatingsText,
                                       out Ratings          Ratings,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(RatingsText).Root,
                             out Ratings,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, RatingsText, e);
            }

            Ratings = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "ratings",

                   new XElement(OCHPNS.Default + "maximumPower", MaximumPower),

                   GuaranteedPower.HasValue
                       ? new XElement(OCHPNS.Default + "guaranteedPower",  GuaranteedPower.Value)
                       : null,

                   NominalVoltage.HasValue
                       ? new XElement(OCHPNS.Default + "nominalVoltage",   NominalVoltage.Value)
                       : null

               );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(MaximumPower,
                             GuaranteedPower.HasValue
                                 ? ", " + GuaranteedPower
                                 : "",
                             NominalVoltage.HasValue
                                 ? ", " + NominalVoltage
                                 : "");

        #endregion

    }

}
