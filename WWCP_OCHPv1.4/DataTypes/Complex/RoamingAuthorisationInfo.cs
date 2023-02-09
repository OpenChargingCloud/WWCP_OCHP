/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
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
using Newtonsoft.Json.Linq;
using org.GraphDefined.Vanaheimr.Hermod.JSON;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// OCHP whitelisted authorisation card info.
    /// </summary>
    public class RoamingAuthorisationInfo
    {

        #region Properties

        /// <summary>
        /// Electrical vehicle contract identifier.
        /// </summary>
        public EMT_Id       EMTId           { get; }

        /// <summary>
        /// EMA-Id the token belongs to.
        /// </summary>
        public Contract_Id  ContractId      { get; }

        /// <summary>
        /// Tokens may be used until the date of expiry is reached. To be handled
        /// by the partners systems. Expired roaming authorisations may be erased
        /// locally by each partner's system.
        /// </summary>
        public DateTime     ExpiryDate      { get; }

        /// <summary>
        /// Might be used for manual authorisation.
        /// </summary>
        public String       PrintedNumber   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP whitelisted authorisation card info.
        /// </summary>
        /// <param name="EMTId">Electrical vehicle contract identifier.</param>
        /// <param name="ContractId">EMA-Id the token belongs to.</param>
        /// <param name="ExpiryDate">Tokens may be used until the date of expiry is reached. To be handled by the partners systems. Expired roaming authorisations may be erased locally by each partner's system.</param>
        /// <param name="PrintedNumber">Might be used for manual authorisation.</param>
        public RoamingAuthorisationInfo(EMT_Id       EMTId,
                                        Contract_Id  ContractId,
                                        DateTime     ExpiryDate,
                                        String       PrintedNumber = null)
        {

            this.EMTId          = EMTId;
            this.ContractId     = ContractId;
            this.ExpiryDate     = ExpiryDate;
            this.PrintedNumber  = PrintedNumber;

        }

        #endregion


        #region Documentation

        // <ns:roamingAuthorisationInfo>
        //
        //    <ns:EmtId representation="plain">
        //
        //       <ns:instance>?</ns:instance>
        //       <ns:tokenType>?</ns:tokenType>
        //
        //       <!--Optional:-->
        //       <ns:tokenSubType>?</ns:tokenSubType>
        //
        //    </ns:EmtId>
        //
        //    <ns:contractId>?</ns:contractId>
        //
        //    <!--Optional:-->
        //    <ns:printedNumber>?</ns:printedNumber>
        //
        //    <ns:expiryDate>
        //       <ns:DateTime>?</ns:DateTime>
        //    </ns:expiryDate>
        //
        // </ns:roamingAuthorisationInfo>

        #endregion

        #region (static) Parse(RoamingAuthorisationInfoXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP authorisation card info.
        /// </summary>
        /// <param name="RoamingAuthorisationInfoXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static RoamingAuthorisationInfo Parse(XElement             RoamingAuthorisationInfoXML,
                                                     OnExceptionDelegate  OnException = null)

            => TryParse(RoamingAuthorisationInfoXML,
                        out RoamingAuthorisationInfo _RoamingAuthorisationInfo,
                        OnException)

                   ? _RoamingAuthorisationInfo
                   : null;

        #endregion

        #region (static) Parse(RoamingAuthorisationInfoText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP authorisation card info.
        /// </summary>
        /// <param name="RoamingAuthorisationInfoText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static RoamingAuthorisationInfo Parse(String               RoamingAuthorisationInfoText,
                                                     OnExceptionDelegate  OnException = null)

            => TryParse(RoamingAuthorisationInfoText,
                        out RoamingAuthorisationInfo _RoamingAuthorisationInfo,
                        OnException)

                   ? _RoamingAuthorisationInfo
                   : null;

        #endregion

        #region (static) TryParse(RoamingAuthorisationInfoXML,  out RoamingAuthorisationInfo, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP authorisation card info.
        /// </summary>
        /// <param name="RoamingAuthorisationInfoXML">The XML to parse.</param>
        /// <param name="RoamingAuthorisationInfo">The parsed authorisation card info.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                      RoamingAuthorisationInfoXML,
                                       out RoamingAuthorisationInfo  RoamingAuthorisationInfo,
                                       OnExceptionDelegate           OnException  = null)
        {

            try
            {

                RoamingAuthorisationInfo = new RoamingAuthorisationInfo(

                                               RoamingAuthorisationInfoXML.MapElementOrFail     (OCHPNS.Default + "EmtId",
                                                                                                 EMT_Id.Parse,
                                                                                                 OnException),

                                               RoamingAuthorisationInfoXML.MapValueOrFail       (OCHPNS.Default + "contractId",
                                                                                                 Contract_Id.Parse),

                                               RoamingAuthorisationInfoXML.MapValueOrFail       (OCHPNS.Default + "expiryDate",
                                                                                                 OCHPNS.Default + "DateTime",
                                                                                                 DateTime.Parse),

                                               RoamingAuthorisationInfoXML.ElementValueOrDefault(OCHPNS.Default + "printedNumber")

                                           );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, RoamingAuthorisationInfoXML, e);

                RoamingAuthorisationInfo = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(RoamingAuthorisationInfoText, out RoamingAuthorisationInfo, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP authorisation card info.
        /// </summary>
        /// <param name="RoamingAuthorisationInfoText">The text to parse.</param>
        /// <param name="RoamingAuthorisationInfo">The parsed authorisation card info.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                        RoamingAuthorisationInfoText,
                                       out RoamingAuthorisationInfo  RoamingAuthorisationInfo,
                                       OnExceptionDelegate           OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(RoamingAuthorisationInfoText).Root,
                             out RoamingAuthorisationInfo,
                             OnException))
                {
                    return true;
                }

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, RoamingAuthorisationInfoText, e);
            }

            RoamingAuthorisationInfo = null;
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">An alternative XML element name [default: "OCHPNS:roamingAuthorisationInfo"]</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OCHPNS.Default + "roamingAuthorisationInfo",

                   EMTId.ToXML(OCHPNS.Default + "EmtId"),
                   new XElement(OCHPNS.Default + "contractId",           ContractId.ToString()),

                   PrintedNumber.IsNotNullOrEmpty()
                       ? new XElement(OCHPNS.Default + "printedNumber",  PrintedNumber)
                       : null,

                   new XElement(OCHPNS.Default + "expiryDate",
                       new XElement(OCHPNS.Default + "DateTime",         ExpiryDate.ToIso8601(false))
                   )

               );

        #endregion


        #region ToJSON(CustomRoamingAuthorisationInfoSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomRoamingAuthorisationInfoSerializer">A delegate to customize the serialization of RoamingAuthorisationInfo respones.</param>
        /// <param name="CustomEMTIdSerializer">A delegate to customize the serialization of EMT identification.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<RoamingAuthorisationInfo> CustomRoamingAuthorisationInfoSerializer  = null,
                              CustomJObjectSerializerDelegate<EMT_Id>                   CustomEMTIdSerializer                     = null)
        {

            var JSON = JSONObject.Create(

                           new JProperty("EMTId",                EMTId.     ToJSON(CustomEMTIdSerializer)),
                           new JProperty("contractId",           ContractId.ToString()),

                           PrintedNumber.IsNotNullOrEmpty()
                               ? new JProperty("printedNumber",  PrintedNumber)
                               : null,

                           new JProperty("expiryDate",           ExpiryDate.ToIso8601(false))

                       );

            return CustomRoamingAuthorisationInfoSerializer != null
                       ? CustomRoamingAuthorisationInfoSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (RoamingAuthorisationInfo1, RoamingAuthorisationInfo2)

        /// <summary>
        /// Compares two roaming authorisation infos for equality.
        /// </summary>
        /// <param name="RoamingAuthorisationInfo1">A roaming authorisation info.</param>
        /// <param name="RoamingAuthorisationInfo2">Another roaming authorisation info.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (RoamingAuthorisationInfo RoamingAuthorisationInfo1, RoamingAuthorisationInfo RoamingAuthorisationInfo2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(RoamingAuthorisationInfo1, RoamingAuthorisationInfo2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) RoamingAuthorisationInfo1 == null) || ((Object) RoamingAuthorisationInfo2 == null))
                return false;

            return RoamingAuthorisationInfo1.Equals(RoamingAuthorisationInfo2);

        }

        #endregion

        #region Operator != (RoamingAuthorisationInfo1, RoamingAuthorisationInfo2)

        /// <summary>
        /// Compares two roaming authorisation infos for inequality.
        /// </summary>
        /// <param name="RoamingAuthorisationInfo1">A roaming authorisation info.</param>
        /// <param name="RoamingAuthorisationInfo2">Another roaming authorisation info.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (RoamingAuthorisationInfo RoamingAuthorisationInfo1, RoamingAuthorisationInfo RoamingAuthorisationInfo2)

            => !(RoamingAuthorisationInfo1 == RoamingAuthorisationInfo2);

        #endregion

        #endregion

        #region IEquatable<RoamingAuthorisationInfo> Members

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

            // Check if the given object is a roaming authorisation info.
            var RoamingAuthorisationInfo = Object as RoamingAuthorisationInfo;
            if ((Object) RoamingAuthorisationInfo == null)
                return false;

            return this.Equals(RoamingAuthorisationInfo);

        }

        #endregion

        #region Equals(RoamingAuthorisationInfo)

        /// <summary>
        /// Compares two roaming authorisation infos for equality.
        /// </summary>
        /// <param name="RoamingAuthorisationInfo">An roaming authorisation info to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(RoamingAuthorisationInfo RoamingAuthorisationInfo)
        {

            if ((Object) RoamingAuthorisationInfo == null)
                return false;

            return this.EMTId.     Equals(RoamingAuthorisationInfo.EMTId) &&
                   this.ContractId.Equals(RoamingAuthorisationInfo.ContractId) &&
                   this.ExpiryDate.Equals(RoamingAuthorisationInfo.ExpiryDate);

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

                return EMTId.     GetHashCode() * 11 ^
                       ContractId.GetHashCode() *  7 ^
                       ExpiryDate.GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(EMTId, " / ", ContractId,

                             PrintedNumber.IsNotNullOrEmpty()
                                 ? " (" + PrintedNumber + ")"
                                 : "",

                             " expires ", ExpiryDate.ToIso8601());

        #endregion

    }

}
