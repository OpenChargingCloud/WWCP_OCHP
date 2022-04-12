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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// An OCHP tariff info.
    /// </summary>
    public class TariffInfo
    {

        #region Properties

        /// <summary>
        /// Identifies a tariff. Unique within one EVSE Operator. Must begin with the operator identification, without separators.
        /// </summary>
        public Tariff_Id                      TariffId           { get; }

        /// <summary>
        /// Element describing an individual tariff for a specific recipient. One default tariff without recipients must be provided.
        /// </summary>
        public IEnumerable<IndividualTariff>  IndividualTariff   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP tariff info.
        /// </summary>
        /// <param name="TariffId">Identifies a tariff. Unique within one EVSE Operator. Must begin with the operator identification, without separators.</param>
        /// <param name="IndividualTariff">Element describing an individual tariff for a specific recipient. One default tariff without recipients must be provided.</param>
        public TariffInfo(Tariff_Id                      TariffId,
                          IEnumerable<IndividualTariff>  IndividualTariff)
        {

            #region Initial checks

            if (TariffId == null)
                throw new ArgumentNullException(nameof(TariffId),  "The given tariff identification must not be null!");

            if (IndividualTariff == null || !IndividualTariff.Any())
                throw new ArgumentNullException(nameof(IndividualTariff), "The given enumeration of individual tariffs must not be null or empty!");

            #endregion

            this.TariffId          = TariffId;
            this.IndividualTariff  = IndividualTariff;

        }

        #endregion


        #region Documentation

        // <ns:TariffInfoArray>
        //
        //    <ns:tariffId>?</ns:tariffId>
        //
        //    <!--1 or more repetitions:-->
        //    <ns:individualTariff>
        //      ...
        //    </ns:individualTariff>
        //
        // </ns:TariffInfoArray>

        #endregion

        #region (static) Parse(TariffInfoXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of OCHP tariff info.
        /// </summary>
        /// <param name="TariffInfoXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static TariffInfo Parse(XElement             TariffInfoXML,
                                       OnExceptionDelegate  OnException = null)
        {

            TariffInfo _TariffInfo;

            if (TryParse(TariffInfoXML, out _TariffInfo, OnException))
                return _TariffInfo;

            return null;

        }

        #endregion

        #region (static) Parse(TariffInfoText, OnException = null)

        /// <summary>
        /// Parse the given text representation of OCHP tariff info.
        /// </summary>
        /// <param name="TariffInfoText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static TariffInfo Parse(String               TariffInfoText,
                                       OnExceptionDelegate  OnException = null)
        {

            TariffInfo _TariffInfo;

            if (TryParse(TariffInfoText, out _TariffInfo, OnException))
                return _TariffInfo;

            return null;

        }

        #endregion

        #region (static) TryParse(TariffInfoXML,  out TariffInfo, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of OCHP tariff info.
        /// </summary>
        /// <param name="TariffInfoXML">The XML to parse.</param>
        /// <param name="TariffInfo">The parsed tariff info.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             TariffInfoXML,
                                       out TariffInfo       TariffInfo,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                TariffInfo = new TariffInfo(

                                 TariffInfoXML.MapValueOrFail   (OCHPNS.Default + "tariffId",
                                                                 Tariff_Id.Parse),

                                 TariffInfoXML.MapElementsOrFail(OCHPNS.Default + "individualTariff",
                                                                 OCHPv1_4.IndividualTariff.Parse,
                                                                 OnException)

                             );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, TariffInfoXML, e);

                TariffInfo = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(TariffInfoText, out TariffInfo, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of OCHP tariff info.
        /// </summary>
        /// <param name="TariffInfoText">The text to parse.</param>
        /// <param name="TariffInfo">The parsed tariff info.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               TariffInfoText,
                                       out TariffInfo       TariffInfo,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(TariffInfoText).Root,
                             out TariffInfo,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, TariffInfoText, e);
            }

            TariffInfo = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">An alternative XML element name [default: "OCHPNS:tariffInfo"]</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OCHPNS.Default + "tariffInfo",

                   new XElement(OCHPNS.Default + "tariffId",  TariffId.ToString()),

                   IndividualTariff.SafeSelect(tariff => tariff.ToXML())

               );

        #endregion


        #region Operator overloading

        #region Operator == (TariffInfo1, TariffInfo2)

        /// <summary>
        /// Compares two tariff infos for equality.
        /// </summary>
        /// <param name="TariffInfo1">An tariff info.</param>
        /// <param name="TariffInfo2">Another tariff info.</param>
        /// <returns>True if both match; False otherwise.</returns
        public static Boolean operator == (TariffInfo TariffInfo1, TariffInfo TariffInfo2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(TariffInfo1, TariffInfo2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) TariffInfo1 == null) || ((Object) TariffInfo2 == null))
                return false;

            return TariffInfo1.Equals(TariffInfo2);

        }

        #endregion

        #region Operator != (TariffInfo1, TariffInfo2)

        /// <summary>
        /// Compares two tariff infos for inequality.
        /// </summary>
        /// <param name="TariffInfo1">An tariff info.</param>
        /// <param name="TariffInfo2">Another tariff info.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (TariffInfo TariffInfo1, TariffInfo TariffInfo2)

            => !(TariffInfo1 == TariffInfo2);

        #endregion

        #endregion

        #region IEquatable<TariffInfo> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is an tariff info.
            var TariffInfo = Object as TariffInfo;
            if ((Object) TariffInfo == null)
                return false;

            return this.Equals(TariffInfo);

        }

        #endregion

        #region Equals(TariffInfo)

        /// <summary>
        /// Compares two tariff infos for equality.
        /// </summary>
        /// <param name="TariffInfo">An tariff info to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(TariffInfo TariffInfo)
        {

            if ((Object) TariffInfo == null)
                return false;

            return TariffId.Equals(TariffInfo.TariffId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return TariffId.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(TariffId.ToString(), " / ",
                             IndividualTariff.Any()
                                 ? " " + IndividualTariff.Count() + " individual tariff(s), "
                                 : "");

        #endregion

    }

}
