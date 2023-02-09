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

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// OCHP authentication methods.
    /// </summary>
    [Flags]
    public enum AuthMethodTypes
    {

        /// <summary>
        /// The authentication method is unknown.
        /// </summary>
        Unknown             = 0,

        /// <summary>
        /// Public accessible, no authorisation required.
        /// </summary>
        Public              = 1,

        /// <summary>
        /// A key or token can be received at the location.(i.e. at the hotel reception or in the restaurant).
        /// </summary>
        LocalKey            = 1 << 1,

        /// <summary>
        /// The EVSE can be accessed through direct payment in cash.
        /// </summary>
        DirectCash          = 1 << 2,

        /// <summary>
        /// The EVSE can be accessed through direct payment with credit card.
        /// </summary>
        DirectCreditcard    = 1 << 3,

        /// <summary>
        /// The EVSE can be accessed through direct payment with debit card.
        /// </summary>
        DirectDebitcard     = 1 << 4,

        /// <summary>
        /// Personal RFID token with roaming relation (Mifare Classic).
        /// </summary>
        RFIDMifareClassic   = 1 << 5,

        /// <summary>
        /// Personal token with roaming relation (Mifare Desfire).
        /// </summary>
        RFIDMifareDESFire   = 1 << 6,

        /// <summary>
        /// Personal RFID token with roaming relation (Calypso).
        /// </summary>
        RFIDCalypso         = 1 << 7,

        /// <summary>
        /// In-car access token as specified in IEC-15118.
        /// </summary>
        IEC15118            = 1 << 8,

        /// <summary>
        /// The EVSE can be accessed through a OCHP-direct capable provider app.
        /// </summary>
        OCHPDirectAuth      = 1 << 9,

        /// <summary>
        /// The EVSE can be accessed through a direct online payment to the operator.
        /// </summary>
        OperatorAuth        = 1 << 10,

    }

}
