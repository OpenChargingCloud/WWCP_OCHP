/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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
    /// An OCHP tariff element.
    /// </summary>
    public class TariffElement
    {

        #region Properties

        /// <summary>
        /// Contains information about the pricing structure of the tariff element.
        /// </summary>
        public IEnumerable<PriceComponent>     PriceComponent      { get; }

        /// <summary>
        /// Contains information about when to apply the defined priceComponent / tariffElement.
        /// </summary>
        public IEnumerable<TariffRestriction>  TariffRestriction   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP tariff element.
        /// </summary>
        /// <param name="PriceComponent">Contains information about the pricing structure of the tariff element.</param>
        /// <param name="TariffRestriction">Contains information about when to apply the defined priceComponent / tariffElement.</param>
        public TariffElement(IEnumerable<PriceComponent>     PriceComponent,
                             IEnumerable<TariffRestriction>  TariffRestriction)
        {

            this.PriceComponent     = PriceComponent;
            this.TariffRestriction  = TariffRestriction;

        }

        #endregion


        #region Documentation

        // <ns:tariffElement>
        //
        //    <!--1 or more repetitions:-->
        //    <ns:priceComponent>
        //      ...
        //    <ns:priceComponent>
        //
        //    <!--1 or more repetitions:-->
        //    <ns:tariffRestriction>
        //       ...
        //    </ns:tariffRestriction>
        //
        // </ns:tariffElement>

        #endregion

        #region (static) Parse(TariffElementXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of OCHP tariff element.
        /// </summary>
        /// <param name="TariffElementXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static TariffElement Parse(XElement             TariffElementXML,
                                          OnExceptionDelegate  OnException = null)
        {

            TariffElement _TariffElement;

            if (TryParse(TariffElementXML, out _TariffElement, OnException))
                return _TariffElement;

            return null;

        }

        #endregion

        #region (static) Parse(TariffElementText, OnException = null)

        /// <summary>
        /// Parse the given text representation of OCHP tariff element.
        /// </summary>
        /// <param name="TariffElementText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static TariffElement Parse(String               TariffElementText,
                                          OnExceptionDelegate  OnException = null)
        {

            TariffElement _TariffElement;

            if (TryParse(TariffElementText, out _TariffElement, OnException))
                return _TariffElement;

            return null;

        }

        #endregion

        #region (static) TryParse(TariffElementXML,  out TariffElement, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of OCHP tariff element.
        /// </summary>
        /// <param name="TariffElementXML">The XML to parse.</param>
        /// <param name="TariffElement">The parsed tariff element.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             TariffElementXML,
                                       out TariffElement    TariffElement,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                TariffElement = new TariffElement(

                                    TariffElementXML.MapElements(OCHPNS.Default + "priceComponent",
                                                                 OCHPv1_4.PriceComponent.Parse,
                                                                 OnException),

                                    TariffElementXML.MapElements(OCHPNS.Default + "tariffRestriction",
                                                                 OCHPv1_4.TariffRestriction.Parse,
                                                                 OnException)

                                );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, TariffElementXML, e);

                TariffElement = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(TariffElementText, out TariffElement, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of OCHP tariff element.
        /// </summary>
        /// <param name="TariffElementText">The text to parse.</param>
        /// <param name="TariffElement">The parsed tariff element.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               TariffElementText,
                                       out TariffElement    TariffElement,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(TariffElementText).Root,
                             out TariffElement,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, TariffElementText, e);
            }

            TariffElement = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">An alternative XML element name [default: "OCHPNS:tariffElement"]</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OCHPNS.Default + "tariffElement",

                   PriceComponent.   SafeSelect(component   => component.  ToXML()),
                   TariffRestriction.SafeSelect(restriction => restriction.ToXML())

               );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(PriceComponent.Any()
                                 ? " " + PriceComponent.Count() + " price component(s), "
                                 : "",
                             TariffRestriction.Any()
                                 ? " " + TariffRestriction.Count() + " tariff restriction(s)"
                                 : "");

        #endregion

    }

}
