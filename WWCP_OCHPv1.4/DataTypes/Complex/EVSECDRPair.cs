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
    /// A pair of an EVSE and a charge detail record identification.
    /// </summary>
    public readonly struct EVSECDRPair
    {

        #region Properties

        /// <summary>
        /// A charge detail record identification.
        /// </summary>
        public CDR_Id   CDRId    { get; }

        /// <summary>
        /// An EVSE identification.
        /// </summary>
        public EVSE_Id  EVSEId   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new pair of an EVSE and a charge detail record identification.
        /// </summary>
        /// <param name="CDRId">A charge detail record identification.</param>
        /// <param name="EVSEId">An EVSE identification.</param>
        public EVSECDRPair(CDR_Id   CDRId,
                           EVSE_Id  EVSEId)

        {

            this.CDRId   = CDRId;
            this.EVSEId  = EVSEId;

        }

        #endregion


        #region Documentation

        // <ns:approved>
        //    <ns:cdrId>?</ns:cdrId>
        //    <ns:evseId>?</ns:evseId>
        // </ns:approved>

        // <ns:declined>
        //    <ns:cdrId>?</ns:cdrId>
        //    <ns:evseId>?</ns:evseId>
        // </ns:declined>

        #endregion

        #region (static) Parse(EVSECDRPairXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of a pair of an EVSE and a charge detail record identification.
        /// </summary>
        /// <param name="EVSECDRPairXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static EVSECDRPair Parse(XElement             EVSECDRPairXML,
                                        OnExceptionDelegate  OnException = null)
        {

            EVSECDRPair _EVSECDRPair;

            if (TryParse(EVSECDRPairXML, out _EVSECDRPair, OnException))
                return _EVSECDRPair;

            return new EVSECDRPair();

        }

        #endregion

        #region (static) Parse(EVSECDRPairText, OnException = null)

        /// <summary>
        /// Parse the given text representation of a pair of an EVSE and a charge detail record identification.
        /// </summary>
        /// <param name="EVSECDRPairText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static EVSECDRPair Parse(String               EVSECDRPairText,
                                        OnExceptionDelegate  OnException = null)
        {

            EVSECDRPair _EVSECDRPair;

            if (TryParse(EVSECDRPairText, out _EVSECDRPair, OnException))
                return _EVSECDRPair;

            return new EVSECDRPair();

        }

        #endregion

        #region (static) TryParse(EVSECDRPairXML,  out EVSECDRPair, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of a pair of an EVSE and a charge detail record identification.
        /// </summary>
        /// <param name="EVSECDRPairXML">The XML to parse.</param>
        /// <param name="EVSECDRPair">The parsed EVSE and a charge detail record identification.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static Boolean TryParse(XElement             EVSECDRPairXML,
                                       out EVSECDRPair      EVSECDRPair,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                EVSECDRPair = new EVSECDRPair(

                                  EVSECDRPairXML.MapValueOrFail(OCHPNS.Default + "cdrId",
                                                                CDR_Id.Parse),

                                  EVSECDRPairXML.MapValueOrFail(OCHPNS.Default + "evseId",
                                                                EVSE_Id.Parse)

                              );


                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(Timestamp.Now, EVSECDRPairXML, e);

                EVSECDRPair = default(EVSECDRPair);
                return false;

            }

        }

        #endregion

        #region (static) TryParse(EVSECDRPairText, out EVSECDRPair, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of a pair of an EVSE and a charge detail record identification.
        /// </summary>
        /// <param name="EVSECDRPairText">The text to parse.</param>
        /// <param name="EVSECDRPair">The parsed EVSE and a charge detail record identification.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occurred.</param>
        public static Boolean TryParse(String               EVSECDRPairText,
                                       out EVSECDRPair      EVSECDRPair,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(EVSECDRPairText).Root,
                             out EVSECDRPair,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(Timestamp.Now, EVSECDRPairText, e);
            }

            EVSECDRPair = default(EVSECDRPair);
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">An alternative XML element name [default: "OCHPNS:approved"]</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OCHPNS.Default + "approved",

                   new XElement(OCHPNS.Default + "cdrId",   CDRId. ToString()),
                   new XElement(OCHPNS.Default + "evseId",  EVSEId.ToString())

               );

        #endregion


        #region Operator overloading

        #region Operator == (EVSECDRPair1, EVSECDRPair2)

        /// <summary>
        /// Compares two EVSE-CDR-paires for equality.
        /// </summary>
        /// <param name="EVSECDRPair1">An EVSE-CDR-pair.</param>
        /// <param name="EVSECDRPair2">Another EVSE-CDR-pair.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (EVSECDRPair EVSECDRPair1, EVSECDRPair EVSECDRPair2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(EVSECDRPair1, EVSECDRPair2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSECDRPair1 is null) || ((Object) EVSECDRPair2 is null))
                return false;

            return EVSECDRPair1.Equals(EVSECDRPair2);

        }

        #endregion

        #region Operator != (EVSECDRPair1, EVSECDRPair2)

        /// <summary>
        /// Compares two EVSE-CDR-paires for inequality.
        /// </summary>
        /// <param name="EVSECDRPair1">An EVSE-CDR-pair.</param>
        /// <param name="EVSECDRPair2">Another EVSE-CDR-pair.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EVSECDRPair EVSECDRPair1, EVSECDRPair EVSECDRPair2)

            => !(EVSECDRPair1 == EVSECDRPair2);

        #endregion

        #endregion

        #region IEquatable<EVSECDRPair> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object is null)
                return false;

            // Check if the given object is an EVSE-CDR-pair.
            if (!(Object is EVSECDRPair))
                return false;

            return this.Equals((EVSECDRPair) Object);

        }

        #endregion

        #region Equals(EVSECDRPair)

        /// <summary>
        /// Compares two EVSE-CDR-paires for equality.
        /// </summary>
        /// <param name="EVSECDRPair">An EVSE-CDR-pair to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSECDRPair EVSECDRPair)
        {

            if ((Object) EVSECDRPair is null)
                return false;

            return CDRId. Equals(EVSECDRPair.CDRId) &&
                   EVSEId.Equals(EVSECDRPair.EVSEId);

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

                return CDRId. GetHashCode() * 11 ^
                       EVSEId.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(CDRId, " / ", EVSEId);

        #endregion

    }

}
