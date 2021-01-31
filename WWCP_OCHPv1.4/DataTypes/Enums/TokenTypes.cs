/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// The type of the supplied instance for basic filtering.
    /// </summary>
    public enum TokenTypes
    {

        /// <summary>
        /// All kinds of RFID-Cards. Field tokenInstance holds the hexadecimal
        /// representation of the card's UID, Byte order: big endian, no zero-filling.
        /// </summary>
        RFID,

        /// <summary>
        /// All means of remote authentication through the backend. Field tokenInstance
        /// holds a reference to the remote authorization or session.
        /// In case of a OCHPdirect authorization the directId.
        /// </summary>
        Remote,

        /// <summary>
        /// All authentication means defined by ISO/IEC 15118 except RFID-cards.
        /// </summary>
        IEC15118

    }

}
