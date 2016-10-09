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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// Contains all information concerning a charge data record.
    /// </summary>
    public class CDRInfo
    {

        #region Properties

        /// <summary>
        /// The unique identification of the charge data record.
        /// </summary>
        public CDR_Id                  CDRId                { get; }

        /// <summary>
        /// The unique identification of the EVSE of the charging process.
        /// </summary>
        public EVSE_Id                 EVSEId               { get; }

        /// <summary>
        /// Utilized token for this charging session.
        /// </summary>
        public EMT_Id                  EMTId                { get; }

        /// <summary>
        /// Identifies a customer in the electric mobility charging context.
        /// </summary>
        public Contract_Id             ContractId           { get; }

        /// <summary>
        /// References a live authorisation request to the clearing house. 
        /// </summary>
        public LiveAuth_Id             LiveAuthId           { get; }

        /// <summary>
        /// Current status of the CDR. Must be set to "new" by the issuing CMS.
        /// Shall not be changed by any partner but only by the CHS.
        /// </summary>
        public CDRStatusTypes          Status               { get; }

        /// <summary>
        /// Start date and time of the charge session (login with the RFID badge).
        /// Local time of the charge point is used.
        /// </summary>
        public DateTime                StartDateTime        { get; }

        /// <summary>
        /// End date and time of the charge session (log-off with the RFID badge
        /// or physical disconnect).
        /// Must be set in the local time of the charge point.
        /// </summary>
        public DateTime                EndDateTime          { get; }

        /// <summary>
        /// Duration of the charge session.
        /// </summary>
        public TimeSpan?               Duration             { get; }

        /// <summary>
        /// Optional address information of the charging point.
        /// </summary>
        public Address                 ChargePointAddress   { get; }

        /// <summary>
        /// The type of the charge point "AC" or "DC".
        /// </summary>
        public String                  ChargePointType      { get; }

        /// <summary>
        /// Type of the utilized socket or connector.
        /// </summary>
        public ConnectorType           ConnectorType        { get; }

        /// <summary>
        /// Maximum available power at the socket in kilowatts. Example: "3.7", "11", "22".
        /// </summary>
        public Single                  MaxSocketPower       { get; }

        /// <summary>
        /// Written identification number of the physical energy meter, provided by the manufacturer.
        /// </summary>
        public String                  MeterId              { get; }

        /// <summary>
        /// An enumeration of periods per item on the bill.
        /// </summary>
        public IEnumerable<CDRPeriod>  CDRPeriods           { get; }

        /// <summary>
        /// Total cost for the entire charging process. Should always equal the sum of the individual periodCosts.
        /// </summary>
        public Single                  TotalCost            { get; }

        /// <summary>
        /// The displayed and charged currency. Defined in ISO 4217 - Table A.1, alphabetic list.
        /// </summary>
        public String                  Currency             { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new charge data record information object.
        /// </summary>
        /// <param name="CDRId">The unique identification of the charge data record.</param>
        /// <param name="EVSEId">The unique identification of the EVSE of the charging process.</param>
        /// <param name="EMTId">Utilized token for this charging session.</param>
        /// <param name="ContractId">Identifies a customer in the electric mobility charging context.</param>
        /// <param name="LiveAuthId">References a live authorisation request to the clearing house. </param>
        /// <param name="Status">Current status of the CDR. Must be set to "new" by the issuing CMS. Shall not be changed by any partner but only by the CHS.</param>
        /// <param name="StartDateTime">Start date and time of the charge session (login with the RFID badge). Local time of the charge point is used.</param>
        /// <param name="EndDateTime">End date and time of the charge session (log-off with the RFID badge or physical disconnect). Must be set in the local time of the charge point.</param>
        /// <param name="Duration">Duration of the charge session.</param>
        /// <param name="ChargePointAddress">Optional address information of the charging point.</param>
        /// <param name="ChargePointType">The type of the charge point "AC" or "DC".</param>
        /// <param name="ConnectorType">Type of the utilized socket or connector.</param>
        /// <param name="MaxSocketPower">Maximum available power at the socket in kilowatts. Example: "3.7", "11", "22".</param>
        /// <param name="MeterId">Written identification number of the physical energy meter, provided by the manufacturer.</param>
        /// <param name="CDRPeriods">An enumeration of periods per item on the bill.</param>
        /// <param name="TotalCost">Total cost for the entire charging process. Should always equal the sum of the individual periodCosts.</param>
        /// <param name="Currency">The displayed and charged currency. Defined in ISO 4217 - Table A.1, alphabetic list.</param>
        public CDRInfo(CDR_Id                  CDRId,
                       EVSE_Id                 EVSEId,
                       EMT_Id                  EMTId,
                       Contract_Id             ContractId,
                       LiveAuth_Id             LiveAuthId,
                       CDRStatusTypes          Status,
                       DateTime                StartDateTime,
                       DateTime                EndDateTime,
                       TimeSpan?               Duration,
                       Address                 ChargePointAddress,
                       String                  ChargePointType,
                       ConnectorType           ConnectorType,
                       Single                  MaxSocketPower,
                       String                  MeterId,
                       IEnumerable<CDRPeriod>  CDRPeriods,
                       Single                  TotalCost,
                       String                  Currency)

        {

            #region Initial checks

            if (CDRId == null)
                throw new ArgumentNullException(nameof(CDRId),            "The given unique identification of a charge detail record must not be null!");

            if (EVSEId == null)
                throw new ArgumentNullException(nameof(EVSEId),           "The given unique identification of an EVSE must not be null!");

            if (EMTId == null)
                throw new ArgumentNullException(nameof(EMTId),            "The given unique identification of a e-mobility token must not be null!");

            if (ContractId == null)
                throw new ArgumentNullException(nameof(ContractId),       "The given unique identification of a contract must not be null!");

            if (ChargePointType.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(ChargePointType),  "The given charge point type information must not be null or empty!");

            if (CDRPeriods == null)
                throw new ArgumentNullException(nameof(CDRPeriods),       "The given enumeration of charge detail record periods must not be null!");

            if (CDRPeriods.Count() < 1)
                throw new ArgumentException("The given enumeration of charge detail record periods must have at least one item!", nameof(CDRPeriods));

            if (Currency.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Currency),         "The given currency information must not be null or empty!");

            #endregion

            this.CDRId               = CDRId;
            this.EVSEId              = EVSEId;
            this.EMTId               = EMTId;
            this.ContractId          = ContractId;
            this.LiveAuthId          = LiveAuthId;
            this.Status              = Status;
            this.StartDateTime       = StartDateTime;
            this.EndDateTime         = EndDateTime;
            this.Duration            = Duration;
            this.ChargePointAddress  = ChargePointAddress;
            this.ChargePointType     = ChargePointType;
            this.ConnectorType       = ConnectorType;
            this.MaxSocketPower      = MaxSocketPower;
            this.MeterId             = MeterId;
            this.CDRPeriods          = CDRPeriods;
            this.TotalCost           = TotalCost;
            this.Currency            = Currency;

        }

        #endregion


    }

}
