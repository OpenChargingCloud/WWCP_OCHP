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
    /// Specifies the major and minor status of an EVSE.
    /// </summary>
    public class EVSEStatus
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
        /// The current minor status of the EVSE.
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
        /// <param name="MinorStatus">The current minor status of the given EVSE.</param>
        /// <param name="TTL">The time to live is set as the deadline until which the status value is to be considered valid. Should be set to the expected status change.</param>
        public EVSEStatus(EVSE_Id                EVSEId,
                          EVSEMajorStatusTypes   MajorStatus,
                          EVSEMinorStatusTypes?  MinorStatus,
                          DateTime?              TTL)
        {

            #region Initial checks

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),  "The given unique identification of an EVSE must not be null!");

            //if (!Definitions.EVSEIdRegExpr.IsMatch(Id.ToString()))
            //    throw new ArgumentException("The given EVSE identification '" + Id + "' does not match the OCHP definition!", nameof(Id));

            #endregion

            this.EVSEId       = EVSEId;
            this.MajorStatus  = MajorStatus;
            this.MinorStatus  = MinorStatus;
            this.TTL          = TTL;

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
