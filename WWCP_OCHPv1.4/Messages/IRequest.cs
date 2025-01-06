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

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// The common interface of an OCHP request message.
    /// </summary>
    public interface IRequest
    {

        /// <summary>
        /// The optional timestamp of the request.
        /// </summary>
        DateTime           Timestamp            { get; }

        /// <summary>
        /// An optional event tracking identification for correlating this request with other events.
        /// </summary>
        EventTracking_Id   EventTrackingId      { get; }

        /// <summary>
        /// An optional timeout for this request.
        /// </summary>
        TimeSpan?          RequestTimeout       { get; }

        /// <summary>
        /// An optional token to cancel this request.
        /// </summary>
        CancellationToken  CancellationToken    { get; }

    }

}
