﻿/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// The exact type of the supplied instance for referencing purpose.
    /// </summary>
    public enum TokenSubTypes
    {

        /// <summary>
        /// Mifare Classic Card.
        /// </summary>
        MifareClassic,

        /// <summary>
        /// Mifare Desfire Card.
        /// </summary>
        MifareDES,

        /// <summary>
        /// Calypso Card.
        /// </summary>
        Calypso

    }

}
