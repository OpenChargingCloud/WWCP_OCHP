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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// A time and billing period in an OCHP charge detail record.
    /// </summary>
    public class CDRPeriod
    {

        #region Properties

        /// <summary>
        /// Starting time of period. Must be equal or later than startDateTime of the CDRInfo.
        /// </summary>
        public DateTime      Start           { get; }

        /// <summary>
        /// Ending time of the period. Must be equal or earlier than endDateTime of the CDRInfo.
        /// </summary>
        public DateTime      End             { get; }

        /// <summary>
        /// Defines what the EVSP is charged for during this period.
        /// </summary>
        public BillingItems  BillingItem     { get; }

        /// <summary>
        /// The value the EVSP is charged for. The unit of the value depends on the billingItem.
        /// </summary>
        public Decimal       BillingValue    { get; }

        /// <summary>
        /// Price per unit of the billingItem in the given currency.
        /// </summary>
        public Decimal       ItemPrice       { get; }

        /// <summary>
        /// The cost of the period in the given currency.
        /// </summary>
        public Decimal?      PeriodCost      { get; }

        /// <summary>
        /// Tax rate in percent that is to be paid for charging processes in the country of origin.
        /// </summary>
        public Decimal?      TaxRate         { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new time and billing period of an OCHP charge detail record.
        /// </summary>
        /// <param name="Start">Starting time of period. Must be equal or later than startDateTime of the CDRInfo.</param>
        /// <param name="End">Ending time of the period. Must be equal or earlier than endDateTime of the CDRInfo.</param>
        /// <param name="BillingItem">Defines what the EVSP is charged for during this period.</param>
        /// <param name="BillingValue">The value the EVSP is charged for. The unit of the value depends on the billingItem.</param>
        /// <param name="ItemPrice">Price per unit of the billingItem in the given currency.</param>
        /// <param name="PeriodCost">The cost of the period in the given currency.</param>
        /// <param name="TaxRate">Tax rate in percent that is to be paid for charging processes in the country of origin.</param>
        public CDRPeriod(DateTime      Start,
                         DateTime      End,
                         BillingItems  BillingItem,
                         Decimal       BillingValue,
                         Decimal       ItemPrice,
                         Decimal?      PeriodCost  = null,
                         Decimal?      TaxRate     = null)

        {

            this.Start         = Start;
            this.End           = End;
            this.BillingItem   = BillingItem;
            this.BillingValue  = BillingValue;
            this.ItemPrice     = ItemPrice;
            this.PeriodCost    = PeriodCost ?? new Decimal?();
            this.TaxRate       = TaxRate    ?? new Decimal?();

        }

        #endregion


        #region Documentation

        // <ns:chargingPeriods>
        //
        //   <ns:startDateTime>
        //      <ns:LocalDateTime>?</ns:LocalDateTime>
        //   </ns:startDateTime>
        //
        //   <ns:endDateTime>
        //      <ns:LocalDateTime>?</ns:LocalDateTime>
        //   </ns:endDateTime>
        //
        //   <ns:billingItem>
        //      <ns:BillingItemType>?</ns:BillingItemType>
        //   </ns:billingItem>
        //
        //   <ns:billingValue>?</ns:billingValue>
        //   <ns:itemPrice>?</ns:itemPrice>
        //
        //   <!--Optional:-->
        //   <ns:periodCost>?</ns:periodCost>
        //
        //   <!--Optional:-->
        //   <ns:taxrate>?</ns:taxrate>
        //
        //</ns:chargingPeriods>

        #endregion

        #region (static) Parse(CDRPeriodXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP time and billing period.
        /// </summary>
        /// <param name="CDRPeriodXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static CDRPeriod Parse(XElement             CDRPeriodXML,
                                      OnExceptionDelegate  OnException  = null)
        {

            CDRPeriod _CDRPeriod;

            if (TryParse(CDRPeriodXML, out _CDRPeriod, OnException))
                return _CDRPeriod;

            return null;

        }

        #endregion

        #region (static) Parse(CDRPeriodText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP time and billing period
        /// </summary>
        /// <param name="CDRPeriodText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static CDRPeriod Parse(String               CDRPeriodText,
                                      OnExceptionDelegate  OnException  = null)
        {

            CDRPeriod _CDRPeriod;

            if (TryParse(CDRPeriodText, out _CDRPeriod, OnException))
                return _CDRPeriod;

            return null;

        }

        #endregion

        #region (static) TryParse(CDRPeriodXML,  out CDRPeriod, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP time and billing period
        /// </summary>
        /// <param name="CDRPeriodXML">The XML to parse.</param>
        /// <param name="CDRPeriod">The parsed time and billing period.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             CDRPeriodXML,
                                       out CDRPeriod        CDRPeriod,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                CDRPeriod = new CDRPeriod(

                                CDRPeriodXML.MapValueOrFail    (OCHPNS.Default + "startDateTime",
                                                                OCHPNS.Default + "LocalDateTime",
                                                                DateTime.Parse),

                                CDRPeriodXML.MapValueOrFail    (OCHPNS.Default + "endDateTime",
                                                                OCHPNS.Default + "LocalDateTime",
                                                                DateTime.Parse),

                                CDRPeriodXML.MapValueOrFail    (OCHPNS.Default + "billingItem",
                                                                OCHPNS.Default + "BillingItemType",
                                                                XML_IO.AsBillingItem),

                                CDRPeriodXML.MapValueOrFail    (OCHPNS.Default + "billingValue",
                                                                Decimal.Parse),

                                CDRPeriodXML.MapValueOrFail    (OCHPNS.Default + "itemPrice",
                                                                Decimal.Parse),

                                CDRPeriodXML.MapValueOrNullable(OCHPNS.Default + "periodCost",
                                                                Decimal.Parse),

                                CDRPeriodXML.MapValueOrNullable(OCHPNS.Default + "taxrate",
                                                                Decimal.Parse)

                            );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, CDRPeriodXML, e);

                CDRPeriod = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(CDRPeriodText, out CDRPeriod, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP time and billing period.
        /// </summary>
        /// <param name="CDRPeriodText">The text to parse.</param>
        /// <param name="CDRPeriod">The parsed time and billing period.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               CDRPeriodText,
                                       out CDRPeriod        CDRPeriod,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(CDRPeriodText).Root,
                             out CDRPeriod,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, CDRPeriodText, e);
            }

            CDRPeriod = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">An alternative XML element name [default: "OCHPNS:chargingPeriod"]</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OCHPNS.Default + "chargingPeriod",

                   new XElement(OCHPNS.Default + "startDateTime",
                       new XElement(OCHPNS.Default + "LocalDateTime",     Start.ToIso8601WithOffset(false))
                   ),

                   new XElement(OCHPNS.Default + "endDateTime",
                       new XElement(OCHPNS.Default + "LocalDateTime",     End.ToIso8601WithOffset(false))
                   ),

                   new XElement(OCHPNS.Default + "billingItem",
                       new XElement(OCHPNS.Default + "BillingItemType",   BillingItem.AsText())
                   ),

                   new XElement(OCHPNS.Default + "billingValue",          BillingValue.ToString(".####")),
                   new XElement(OCHPNS.Default + "itemPrice",             ItemPrice.ToString(".####")),

                   PeriodCost.HasValue
                       ? new XElement(OCHPNS.Default + "periodCost",      PeriodCost.Value.ToString(".####"))
                       : null,

                   TaxRate.HasValue
                       ? new XElement(OCHPNS.Default + "taxrate",         TaxRate.Value.ToString(".####"))
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (CDRPeriod1, CDRPeriod2)

        /// <summary>
        /// Compares two charge detail record periods for equality.
        /// </summary>
        /// <param name="CDRPeriod1">A charge detail record period.</param>
        /// <param name="CDRPeriod2">Another charge detail record period.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (CDRPeriod CDRPeriod1, CDRPeriod CDRPeriod2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(CDRPeriod1, CDRPeriod2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) CDRPeriod1 == null) || ((Object) CDRPeriod2 == null))
                return false;

            return CDRPeriod1.Equals(CDRPeriod2);

        }

        #endregion

        #region Operator != (CDRPeriod1, CDRPeriod2)

        /// <summary>
        /// Compares two charge detail record periods for inequality.
        /// </summary>
        /// <param name="CDRPeriod1">A charge detail record period.</param>
        /// <param name="CDRPeriod2">Another charge detail record period.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (CDRPeriod CDRPeriod1, CDRPeriod CDRPeriod2)

            => !(CDRPeriod1 == CDRPeriod2);

        #endregion

        #endregion

        #region IEquatable<CDRPeriod> Members

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

            // Check if the given object is an charge detail record period.
            var CDRPeriod = Object as CDRPeriod;
            if ((Object) CDRPeriod == null)
                return false;

            return this.Equals(CDRPeriod);

        }

        #endregion

        #region Equals(CDRPeriod)

        /// <summary>
        /// Compares two charge detail record periods for equality.
        /// </summary>
        /// <param name="CDRPeriod">An charge detail record period to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(CDRPeriod CDRPeriod)
        {

            if ((Object) CDRPeriod == null)
                return false;

            return Start.       Equals(CDRPeriod.Start)        &&
                   End.         Equals(CDRPeriod.End)          &&
                   BillingItem. Equals(CDRPeriod.BillingItem)  &&
                   BillingValue.Equals(CDRPeriod.BillingValue) &&
                   ItemPrice.   Equals(CDRPeriod.ItemPrice)    &&

                   ((!PeriodCost.HasValue && !CDRPeriod.PeriodCost.HasValue) ||
                     (PeriodCost.HasValue &&  CDRPeriod.PeriodCost.HasValue && PeriodCost.Value.Equals(CDRPeriod.PeriodCost.Value))) &&

                   ((!TaxRate.   HasValue && !CDRPeriod.TaxRate.   HasValue) ||
                     (TaxRate.   HasValue &&  CDRPeriod.TaxRate.   HasValue && TaxRate.   Value.Equals(CDRPeriod.TaxRate.   Value)));

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

                return Start.       GetHashCode() * 31 ^
                       End.         GetHashCode() * 23 ^
                       BillingItem. GetHashCode() * 19 ^
                       BillingValue.GetHashCode() * 17 ^
                       ItemPrice.   GetHashCode() * 11 ^

                       (PeriodCost.HasValue
                            ? PeriodCost.GetHashCode()
                            : 0) * 7 ^

                       (TaxRate.HasValue
                            ? TaxRate.GetHashCode()
                            : 0) * 5;


            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(BillingItem.ToString(),
                             " at ",   BillingValue,
                             " from ", Start.ToIso8601(),
                             " -> ",   End.  ToIso8601());

        #endregion

    }

}
