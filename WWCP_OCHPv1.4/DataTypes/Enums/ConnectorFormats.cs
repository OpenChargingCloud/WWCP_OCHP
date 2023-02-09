﻿/*
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

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// OCHP connector formats.
    /// </summary>
    public enum ConnectorFormats
    {

        /// <summary>
        /// The connector formats is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The connector is a socket; the EV user needs to bring a fitting plug.
        /// </summary>
        Socket,

        /// <summary>
        /// The connector is an attached cable; the EV user car needs to have a fitting inlet.
        /// </summary>
        Cable

    }

}
