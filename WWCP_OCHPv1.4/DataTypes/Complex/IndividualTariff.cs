/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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
    /// An OCHP individual tariff.
    /// </summary>
    public class IndividualTariff
    {

        #region Properties

        /// <summary>
        /// Contains information about the pricing structure of the tariff element.
        /// </summary>
        public IEnumerable<TariffElement>  TariffElements   { get; }

        /// <summary>
        /// Identifies a recipient EMSP according to EMSP-ID without separators.
        /// If not provided, tariff element is considered the default tariff for
        /// this tariffId. Should never be returned by the CHS (i.e. only part
        /// of upload, not download).
        /// </summary>
        public IEnumerable<String>         Recipients      { get; }

        /// <summary>
        /// Contains information about the pricing structure of the tariff element.
        /// </summary>
        public Currency                    Currency        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP individual tariff.
        /// </summary>
        /// <param name="TariffElements">Contains information about the pricing structure of the tariff element.</param>
        /// <param name="Recipients">Identifies a recipient EMSP according to EMSP-ID without separators. If not provided, tariff element is considered the default tariff for this tariffId. Should never be returned by the CHS (i.e. only part of upload, not download).</param>
        /// <param name="Currency">Contains information about the pricing structure of the tariff element.</param>
        public IndividualTariff(IEnumerable<TariffElement>  TariffElements,
                                IEnumerable<String>         Recipients,
                                Currency                    Currency)
        {

            this.TariffElements  = TariffElements;
            this.Recipients      = Recipients;
            this.Currency        = Currency;

        }

        #endregion


        #region Documentation

        // <ns:individualTariff>
        //
        //    <!--1 or more repetitions:-->
        //    <ns:tariffElement>
        //      ...
        //    <ns:tariffElement>
        //
        //    <!--Zero or more repetitions:-->
        //    <ns:recipient>?</ns:recipient>
        //
        //    <ns:currency>?</ns:currency>
        //
        // </ns:individualTariff>

        #endregion

        #region (static) Parse(IndividualTariffXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of OCHP individual tariff.
        /// </summary>
        /// <param name="IndividualTariffXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static IndividualTariff Parse(XElement             IndividualTariffXML,
                                             OnExceptionDelegate  OnException = null)
        {

            IndividualTariff _IndividualTariff;

            if (TryParse(IndividualTariffXML, out _IndividualTariff, OnException))
                return _IndividualTariff;

            return null;

        }

        #endregion

        #region (static) Parse(IndividualTariffText, OnException = null)

        /// <summary>
        /// Parse the given text representation of OCHP individual tariff.
        /// </summary>
        /// <param name="IndividualTariffText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static IndividualTariff Parse(String               IndividualTariffText,
                                             OnExceptionDelegate  OnException = null)
        {

            IndividualTariff _IndividualTariff;

            if (TryParse(IndividualTariffText, out _IndividualTariff, OnException))
                return _IndividualTariff;

            return null;

        }

        #endregion

        #region (static) TryParse(IndividualTariffXML,  out IndividualTariff, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of OCHP individual tariff.
        /// </summary>
        /// <param name="IndividualTariffXML">The XML to parse.</param>
        /// <param name="IndividualTariff">The parsed individual tariff.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement              IndividualTariffXML,
                                       out IndividualTariff  IndividualTariff,
                                       OnExceptionDelegate   OnException  = null)
        {

            try
            {

                IndividualTariff = new IndividualTariff(

                                       IndividualTariffXML.MapElements   (OCHPNS.Default + "tariffElement",
                                                                          TariffElement.Parse,
                                                                          OnException),

                                       IndividualTariffXML.ElementValues (OCHPNS.Default + "recipient"),

                                       IndividualTariffXML.MapValueOrFail(OCHPNS.Default + "currency",
                                                                          Currency.ParseString)

                                   );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, IndividualTariffXML, e);

                IndividualTariff = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(IndividualTariffText, out IndividualTariff, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of OCHP individual tariff.
        /// </summary>
        /// <param name="IndividualTariffText">The text to parse.</param>
        /// <param name="IndividualTariff">The parsed individual tariff.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                IndividualTariffText,
                                       out IndividualTariff  IndividualTariff,
                                       OnExceptionDelegate   OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(IndividualTariffText).Root,
                             out IndividualTariff,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, IndividualTariffText, e);
            }

            IndividualTariff = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">An alternative XML element name [default: "OCHPNS:individualTariff"]</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OCHPNS.Default + "individualTariff",

                   TariffElements.SafeSelect(tariff    => tariff.ToXML()),

                   Recipients.    SafeSelect(recipient => new XElement(OCHPNS.Default + "recipient",  recipient)),

                   new XElement(OCHPNS.Default + "currency",  Currency.ISOCode)

               );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(TariffElements.Any()
                                 ? " " + TariffElements.Count() + " tariff element(s), "
                                 : "",
                             " in ",  Currency,
                             " for ", Recipients.AggregateWith(", "));

        #endregion

    }

}
