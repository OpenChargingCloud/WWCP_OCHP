﻿/*
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

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// A general OCHP exception.
    /// </summary>
    public class OCHPException : ApplicationException
    {

        /// <summary>
        /// Create a new OCHP exception.
        /// </summary>
        /// <param name="Message">A message that describes the error.</param>
        public OCHPException(String Message)
            : base(Message)
        { }

        /// <summary>
        /// Create a new OCHP exception.
        /// </summary>
        /// <param name="Message">A message that describes the error.</param>
        /// <param name="InnerException">The exception that is the cause of the current exception.</param>
        public OCHPException(String Message, Exception InnerException)
            : base(Message, InnerException)
        { }

    }



    public class InvalidEVSEIdentificationException : OCHPException
    {

        public String EVSEId { get; }

        public InvalidEVSEIdentificationException(String EVSEId)
            : base("Invalid EVSE identification '" + EVSEId + "'!")
        {
            this.EVSEId = EVSEId;
        }

    }

}
