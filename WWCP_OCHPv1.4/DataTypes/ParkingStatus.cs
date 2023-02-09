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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// The the status and an optional timeout of this status of an OCHP parking space.
    /// </summary>
    public struct ParkingStatus
    {

        #region Properties

        /// <summary>
        /// The unique identification of the parking space.
        /// </summary>
        public Parking_Id          ParkingId    { get; }

        /// <summary>
        /// The current status of the parking space.
        /// </summary>
        public ParkingStatusTypes  Status       { get; }

        /// <summary>
        /// The time to live is set as the deadline until which the
        /// status value is to be considered valid. Should be set to
        /// the expected status change.
        /// </summary>
        public DateTime?           TTL          { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP parking space status.
        /// </summary>
        /// <param name="ParkingId">The unique identification of the parking space.</param>
        /// <param name="Status">The current status of the given parking space.</param>
        /// <param name="TTL">The time to live is set as the deadline until which the status value is to be considered valid. Should be set to the expected status change.</param>
        public ParkingStatus(Parking_Id          ParkingId,
                             ParkingStatusTypes  Status,
                             DateTime?           TTL = null)
        {

            this.ParkingId  = ParkingId;
            this.Status     = Status;
            this.TTL        = TTL ?? new DateTime?();

        }

        #endregion


        #region Documentation

        // <OCHP:parking status="?" ttl="?">
        //    <OCHP:parkingId>?</OCHP:parkingId>
        // </OCHP:evse>

        #endregion

        #region (static) Parse(ParkingStatusXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP parking status.
        /// </summary>
        /// <param name="ParkingStatusXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ParkingStatus Parse(XElement             ParkingStatusXML,
                                          OnExceptionDelegate  OnException = null)
        {

            ParkingStatus _ParkingStatus;

            if (TryParse(ParkingStatusXML, out _ParkingStatus, OnException))
                return _ParkingStatus;

            return default(ParkingStatus);

        }

        #endregion

        #region (static) Parse(ParkingStatusText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP parking status.
        /// </summary>
        /// <param name="ParkingStatusText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ParkingStatus Parse(String               ParkingStatusText,
                                          OnExceptionDelegate  OnException = null)
        {

            ParkingStatus _ParkingStatus;

            if (TryParse(ParkingStatusText, out _ParkingStatus, OnException))
                return _ParkingStatus;

            return default(ParkingStatus);

        }

        #endregion

        #region (static) TryParse(ParkingStatusXML,  out ParkingStatus, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP parking status.
        /// </summary>
        /// <param name="ParkingStatusXML">The XML to parse.</param>
        /// <param name="ParkingStatus">The parsed parking status.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             ParkingStatusXML,
                                       out ParkingStatus    ParkingStatus,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                ParkingStatus = new ParkingStatus(

                                    ParkingStatusXML.MapValueOrFail             (OCHPNS.Default + "parkingId",
                                                                                 Parking_Id.Parse),

                                    ParkingStatusXML.MapAttributeValueOrFail    (OCHPNS.Default + "status",
                                                                                 XML_IO.AsParkingStatusType),

                                    ParkingStatusXML.MapAttributeValueOrNullable(OCHPNS.Default + "ttl",
                                                                                 DateTime.Parse)

                                );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, ParkingStatusXML, e);

                ParkingStatus = default(ParkingStatus);
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ParkingStatusText, out ParkingStatus, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP parking status.
        /// </summary>
        /// <param name="ParkingStatusText">The text to parse.</param>
        /// <param name="ParkingStatus">The parsed parking status.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               ParkingStatusText,
                                       out ParkingStatus    ParkingStatus,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ParkingStatusText).Root,
                             out ParkingStatus,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, ParkingStatusText, e);
            }

            ParkingStatus = default(ParkingStatus);
            return false;

        }

        #endregion

        #region ToXML(XName = null)

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        /// <param name="XName">An alternative XML element name [default: "OCHPNS:cdrInfoArray"]</param>
        public XElement ToXML(XName XName = null)

            => new XElement(XName ?? OCHPNS.Default + "parking",

                   new XAttribute(OCHPNS.Default + "status",      XML_IO.AsText(Status)),

                   TTL.HasValue
                       ? new XAttribute(OCHPNS.Default + "ttl",   TTL.Value.ToIso8601())
                       : null,

                   new XElement  (OCHPNS.Default + "parkingId",   ParkingId.ToString())

               );

        #endregion


        #region Operator overloading

        #region Operator == (ParkingStatus1, ParkingStatus2)

        /// <summary>
        /// Compares two parking status for equality.
        /// </summary>
        /// <param name="ParkingStatus1">A parking status.</param>
        /// <param name="ParkingStatus2">Another parking status.</param>
        /// <returns>True if both match; False otherwise.</returns
        public static Boolean operator == (ParkingStatus ParkingStatus1, ParkingStatus ParkingStatus2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ParkingStatus1, ParkingStatus2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ParkingStatus1 == null) || ((Object) ParkingStatus2 == null))
                return false;

            return ParkingStatus1.Equals(ParkingStatus2);

        }

        #endregion

        #region Operator != (ParkingStatus1, ParkingStatus2)

        /// <summary>
        /// Compares two parking status for inequality.
        /// </summary>
        /// <param name="ParkingStatus1">A parking status.</param>
        /// <param name="ParkingStatus2">Another parking status.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ParkingStatus ParkingStatus1, ParkingStatus ParkingStatus2)

            => !(ParkingStatus1 == ParkingStatus2);

        #endregion

        #endregion

        #region IEquatable<ParkingStatus> Members

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

            // Check if the given object is a parking status.
            if (!(Object is ParkingStatus))
                return false;

            return Equals((ParkingStatus) Object);

        }

        #endregion

        #region Equals(ParkingStatus)

        /// <summary>
        /// Compares two parking status for equality.
        /// </summary>
        /// <param name="ParkingStatus">A parking status to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ParkingStatus ParkingStatus)
        {

            if ((Object) ParkingStatus == null)
                return false;

            return ParkingId.Equals(ParkingStatus.ParkingId) &&
                   Status.   Equals(ParkingStatus.Status)    &&

                   (( TTL.HasValue &&  ParkingStatus.TTL.HasValue && TTL.Value == ParkingStatus.TTL.Value) ||
                    (!TTL.HasValue && !ParkingStatus.TTL.HasValue));

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

                return Status.   GetHashCode() * 7 ^
                       ParkingId.GetHashCode() * 5 ^

                       (TTL.HasValue
                            ? TTL.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat("'", ParkingId, "' is '", Status, "'",
                             TTL.HasValue
                                 ? " till " + TTL.Value.ToIso8601()
                                 : "");

        #endregion

    }

}
