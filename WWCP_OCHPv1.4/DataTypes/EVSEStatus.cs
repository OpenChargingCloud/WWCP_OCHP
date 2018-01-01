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
    /// The the major status, minor status and an optional timeout of this status of an OCHP EVSE.
    /// </summary>
    public struct EVSEStatus
    {

        #region Properties

        /// <summary>
        /// The unique identification of the EVSE.
        /// </summary>
        public EVSE_Id                EVSEId        { get; }

        /// <summary>
        /// The current major status of the EVSE.
        /// </summary>
        public EVSEMajorStatusTypes   MajorStatus   { get; }

        /// <summary>
        /// An optional current minor status of the EVSE.
        /// </summary>
        public EVSEMinorStatusTypes?  MinorStatus   { get; }

        /// <summary>
        /// The time to live is set as the deadline until which the
        /// status value is to be considered valid. Should be set to
        /// the expected status change.
        /// </summary>
        public DateTime?              TTL           { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP EVSE status.
        /// </summary>
        /// <param name="EVSEId">The unique identification of the EVSE.</param>
        /// <param name="MajorStatus">The current major status of the given EVSE.</param>
        /// <param name="MinorStatus">An optional current minor status of the given EVSE.</param>
        /// <param name="TTL">The time to live is set as the deadline until which the status value is to be considered valid. Should be set to the expected status change.</param>
        public EVSEStatus(EVSE_Id                EVSEId,
                          EVSEMajorStatusTypes   MajorStatus,
                          EVSEMinorStatusTypes?  MinorStatus  = null,
                          DateTime?              TTL          = null)
        {

            this.EVSEId       = EVSEId;
            this.MajorStatus  = MajorStatus;
            this.MinorStatus  = MinorStatus ?? new EVSEMinorStatusTypes?();
            this.TTL          = TTL         ?? new DateTime?();

            #region Check EVSE major/minor status combination

            if (MinorStatus.HasValue)
            {
                switch (MajorStatus)
                {

                    #region Available

                    case EVSEMajorStatusTypes.Available:

                        switch (MinorStatus)
                        {

                            case EVSEMinorStatusTypes.Available:
                            case EVSEMinorStatusTypes.Unknown:
                                break;

                            default:
                                throw new IllegalEVSEStatusCombinationException(EVSEId,
                                                                                MajorStatus,
                                                                                MinorStatus.Value);

                        }

                        break;

                    #endregion

                    #region NotAvailable

                    case EVSEMajorStatusTypes.NotAvailable:

                        switch (MinorStatus)
                        {

                            case EVSEMinorStatusTypes.Blocked:
                            case EVSEMinorStatusTypes.Charging:
                            case EVSEMinorStatusTypes.OutOfOrder:
                            case EVSEMinorStatusTypes.Reserved:
                            case EVSEMinorStatusTypes.Unknown:
                                break;

                            default:
                                throw new IllegalEVSEStatusCombinationException(EVSEId,
                                                                                MajorStatus,
                                                                                MinorStatus.Value);

                        }

                        break;

                        #endregion

                }
            }

            #endregion

        }

        #endregion


        #region Documentation

        // <OCHP:evse major="?" minor="?" ttl="?">
        //    <OCHP:evseId>?</OCHP:evseId>
        // </OCHP:evse>

        #endregion

        #region (static) Parse(EVSEStatusXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP EVSE status.
        /// </summary>
        /// <param name="EVSEStatusXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEStatus Parse(XElement             EVSEStatusXML,
                                       OnExceptionDelegate  OnException = null)
        {

            EVSEStatus _EVSEStatus;

            if (TryParse(EVSEStatusXML, out _EVSEStatus, OnException))
                return _EVSEStatus;

            return default(EVSEStatus);

        }

        #endregion

        #region (static) Parse(EVSEStatusText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP EVSE status.
        /// </summary>
        /// <param name="EVSEStatusText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static EVSEStatus Parse(String               EVSEStatusText,
                                       OnExceptionDelegate  OnException = null)
        {

            EVSEStatus _EVSEStatus;

            if (TryParse(EVSEStatusText, out _EVSEStatus, OnException))
                return _EVSEStatus;

            return default(EVSEStatus);

        }

        #endregion

        #region (static) TryParse(EVSEStatusXML,  out EVSEStatus, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP EVSE status.
        /// </summary>
        /// <param name="EVSEStatusXML">The XML to parse.</param>
        /// <param name="EVSEStatus">The parsed EVSE status.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             EVSEStatusXML,
                                       out EVSEStatus       EVSEStatus,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                EVSEStatus = new EVSEStatus(

                                 EVSEStatusXML.MapValueOrFail             (OCHPNS.Default + "evseId",
                                                                           EVSE_Id.Parse),

                                 EVSEStatusXML.MapAttributeValueOrFail    (OCHPNS.Default + "major",
                                                                           XML_IO.AsEVSEMajorStatusType),

                                 EVSEStatusXML.MapAttributeValueOrNullable(OCHPNS.Default + "minor",
                                                                           XML_IO.AsEVSEMinorStatusType),

                                 EVSEStatusXML.MapAttributeValueOrNullable(OCHPNS.Default + "ttl",
                                                                           DateTime.Parse)

                             );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, EVSEStatusXML, e);

                EVSEStatus = default(EVSEStatus);
                return false;

            }

        }

        #endregion

        #region (static) TryParse(EVSEStatusText, out EVSEStatus, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP EVSE status.
        /// </summary>
        /// <param name="EVSEStatusText">The text to parse.</param>
        /// <param name="EVSEStatus">The parsed EVSE status.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               EVSEStatusText,
                                       out EVSEStatus       EVSEStatus,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(EVSEStatusText).Root,
                             out EVSEStatus,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, EVSEStatusText, e);
            }

            EVSEStatus = default(EVSEStatus);
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">An alternative XML element name [default: "OCHPNS:cdrInfoArray"]</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OCHPNS.Default + "evse",

                   new XAttribute("major",         XML_IO.AsText(MajorStatus)),

                   MinorStatus.HasValue
                       ? new XAttribute("minor",   XML_IO.AsText(MinorStatus.Value))
                       : null,

                   TTL.HasValue
                       ? new XAttribute("ttl",     TTL.Value.ToIso8601())
                       : null,

                   new XElement  (OCHPNS.Default + "evseId",        EVSEId.ToString())

               );

        #endregion


        #region Operator overloading

        #region Operator == (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two EVSE status for equality.
        /// </summary>
        /// <param name="EVSEStatus1">An EVSE status.</param>
        /// <param name="EVSEStatus2">Another EVSE status.</param>
        /// <returns>True if both match; False otherwise.</returns
        public static Boolean operator == (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(EVSEStatus1, EVSEStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) EVSEStatus1 == null) || ((Object) EVSEStatus2 == null))
                return false;

            return EVSEStatus1.Equals(EVSEStatus2);

        }

        #endregion

        #region Operator != (EVSEStatus1, EVSEStatus2)

        /// <summary>
        /// Compares two EVSE status for inequality.
        /// </summary>
        /// <param name="EVSEStatus1">An EVSE status.</param>
        /// <param name="EVSEStatus2">Another EVSE status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (EVSEStatus EVSEStatus1, EVSEStatus EVSEStatus2)

            => !(EVSEStatus1 == EVSEStatus2);

        #endregion

        #endregion

        #region IEquatable<EVSEStatus> Members

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

            // Check if the given object is an EVSE status.
            if (!(Object is EVSEStatus))
                return false;

            return this.Equals((EVSEStatus) Object);

        }

        #endregion

        #region Equals(EVSEStatus)

        /// <summary>
        /// Compares two EVSE status for equality.
        /// </summary>
        /// <param name="EVSEStatus">An EVSE status to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(EVSEStatus EVSEStatus)
        {

            if ((Object) EVSEStatus == null)
                return false;

            return EVSEId.      Equals(EVSEStatus.EVSEId)      &&
                   MajorStatus. Equals(EVSEStatus.MajorStatus) &&

                   (( MinorStatus.HasValue &&  EVSEStatus.MinorStatus.HasValue && MinorStatus.Value == EVSEStatus.MinorStatus.Value) ||
                    (!MinorStatus.HasValue && !EVSEStatus.MinorStatus.HasValue)) &&

                   (( TTL.        HasValue &&  EVSEStatus.TTL.        HasValue && TTL.        Value == EVSEStatus.TTL.        Value) ||
                    (!TTL.        HasValue && !EVSEStatus.TTL.        HasValue));

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

                return MajorStatus.GetHashCode() * 17 ^
                       EVSEId.     GetHashCode() * 11 ^

                       (MinorStatus.HasValue
                            ? MinorStatus.GetHashCode()
                            : 0) * 5 ^

                       (TTL.HasValue
                            ? TTL.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("'", EVSEId,
                             "' is '", MajorStatus, "'",

                             MinorStatus.HasValue
                                 ? " / '" + MinorStatus.Value + "'"
                                 : "",

                             TTL.HasValue
                                 ? " till " + TTL.Value.ToIso8601()
                                 : "");

        #endregion

    }

}
