/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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
    /// An OCHP price component.
    /// </summary>
    public class PriceComponent
    {

        #region Properties

        /// <summary>
        /// What dimension is part of this tariff element.
        /// </summary>
        public BillingItems  BillingItem    { get; }

        /// <summary>
        /// Price per unit of the billing item in the given currency.
        /// </summary>
        public Single        ItemPrice      { get; }

        /// <summary>
        /// Minimum amount to be billed (billing will happen in stepSize increments).
        /// In case the billingItem is a one time payment, stepSize is to be set to 1.
        /// </summary>
        public UInt16        StepSize       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP price component.
        /// </summary>
        /// <param name="BillingItem">What dimension is part of this tariff element.</param>
        /// <param name="ItemPrice">Price per unit of the billing item in the given currency.</param>
        /// <param name="StepSize">Minimum amount to be billed (billing will happen in stepSize increments). In case the billingItem is a one time payment, stepSize is to be set to 1.</param>
        public PriceComponent(BillingItems  BillingItem,
                              Single        ItemPrice,
                              UInt16        StepSize)
        {

            this.BillingItem  = BillingItem;
            this.ItemPrice    = ItemPrice;
            this.StepSize     = StepSize;

        }

        #endregion


        #region Documentation

        // <ns:priceComponent>
        //
        //   <ns:billingItem>
        //      <ns:BillingItemType>?</ns:BillingItemType>
        //   </ns:billingItem>
        //
        //   <ns:itemPrice>?</ns:itemPrice>
        //   <ns:stepSize>?</ns:stepSize>
        //
        // </ns:priceComponent>

        #endregion

        #region (static) Parse(PriceComponentXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of OCHP price component.
        /// </summary>
        /// <param name="PriceComponentXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static PriceComponent Parse(XElement             PriceComponentXML,
                                           OnExceptionDelegate  OnException = null)
        {

            PriceComponent _PriceComponent;

            if (TryParse(PriceComponentXML, out _PriceComponent, OnException))
                return _PriceComponent;

            return null;

        }

        #endregion

        #region (static) Parse(PriceComponentText, OnException = null)

        /// <summary>
        /// Parse the given text representation of OCHP price component.
        /// </summary>
        /// <param name="PriceComponentText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static PriceComponent Parse(String               PriceComponentText,
                                           OnExceptionDelegate  OnException = null)
        {

            PriceComponent _PriceComponent;

            if (TryParse(PriceComponentText, out _PriceComponent, OnException))
                return _PriceComponent;

            return null;

        }

        #endregion

        #region (static) TryParse(PriceComponentXML,  out PriceComponent, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of OCHP price component.
        /// </summary>
        /// <param name="PriceComponentXML">The XML to parse.</param>
        /// <param name="PriceComponent">The parsed price component.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             PriceComponentXML,
                                       out PriceComponent   PriceComponent,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                PriceComponent = new PriceComponent(

                                        PriceComponentXML.MapValueOrFail    (OCHPNS.Default + "billingItem",
                                                                             OCHPNS.Default + "BillingItemType",
                                                                             XML_IO.AsBillingItem),

                                        PriceComponentXML.MapValueOrFail    (OCHPNS.Default + "itemPrice",
                                                                             Single.Parse),

                                        PriceComponentXML.MapValueOrFail    (OCHPNS.Default + "stepSize",
                                                                             UInt16.Parse)

                                    );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, PriceComponentXML, e);

                PriceComponent = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(PriceComponentText, out PriceComponent, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of OCHP price component.
        /// </summary>
        /// <param name="PriceComponentText">The text to parse.</param>
        /// <param name="PriceComponent">The parsed price component.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               PriceComponentText,
                                       out PriceComponent   PriceComponent,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(PriceComponentText).Root,
                             out PriceComponent,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, PriceComponentText, e);
            }

            PriceComponent = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "priceComponent",

                   new XElement(OCHPNS.Default + "billingItem",
                       new XElement(OCHPNS.Default + "BillingItemType",  XML_IO.AsText(BillingItem))),

                   new XElement(OCHPNS.Default + "itemPrice",            ItemPrice),
                   new XElement(OCHPNS.Default + "stepSize",             StepSize)

               );

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(BillingItem, " for ", ItemPrice, ", step size ", StepSize);

        #endregion

    }

}
