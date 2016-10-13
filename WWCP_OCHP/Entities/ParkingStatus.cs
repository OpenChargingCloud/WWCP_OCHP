/*
 * Copyright (c) 2014-2016 GraphDefined GmbH
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

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// Specifies the status of an OCHP parking space.
    /// </summary>
    public class ParkingStatus
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
                             DateTime?           TTL)
        {

            #region Initial checks

            if (ParkingId == null)
                throw new ArgumentNullException(nameof(ParkingId),  "The given unique identification of a parking space must not be null!");

            #endregion

            this.ParkingId  = ParkingId;
            this.Status     = Status;
            this.TTL        = TTL;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv    = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:EVSEStatus = "http://www.hubject.com/b2b/services/evsestatus/EVSEData.0">
        //
        // [...]
        //
        //   <EVSEStatus:EvseStatusRecord>
        //      <EVSEStatus:EvseId>?</EVSEData:EvseId>
        //      <EVSEStatus:EvseStatus>?</EVSEData:EvseStatus>
        //   </EVSEStatus:EvseStatusRecord>
        //
        // [...]

        #endregion

        #region Parse(EVSEStatusRecordXML)

        ///// <summary>
        ///// Parse the EVSE identification and its current status from the given OCHP XML.
        ///// </summary>
        ///// <param name="EVSEStatusRecordXML">An OCHP XML.</param>
        //public static EVSEStatus Parse(XElement EVSEStatusRecordXML)
        //{

        //    try
        //    {

        //        if (EVSEStatusRecordXML.Name != OCHPNS.EVSEStatus + "EvseStatusRecord")
        //            throw new Exception("Illegal EVSEStatusRecord XML!");

        //        return new EVSEStatus(
        //            EVSE_Id.Parse(EVSEStatusRecordXML.ElementValueOrFail(OCHPNS.EVSEStatus + "EvseId")),
        //            (EVSEStatusTypes) Enum.Parse(typeof(EVSEStatusTypes), EVSEStatusRecordXML.ElementValueOrFail(OCHPNS.EVSEStatus + "EvseStatus"))
        //        );

        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }

        //}

        #endregion

        #region ToXML()

        ///// <summary>
        ///// Return an OCHP XML representation of this EVSE status record.
        ///// </summary>
        ///// <returns></returns>
        //public XElement ToXML()

        //    => new XElement(OCHPNS.EVSEStatus + "EvseStatusRecord",
        //           new XElement(OCHPNS.EVSEStatus + "EvseId",     Id.    OriginId),
        //           new XElement(OCHPNS.EVSEStatus + "EvseStatus", Status.ToString())
        //       );

        #endregion

    }

}
