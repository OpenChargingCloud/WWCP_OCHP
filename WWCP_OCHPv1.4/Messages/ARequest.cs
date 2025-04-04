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
    /// An abstract generic OCHP request message.
    /// </summary>
    public abstract class ARequest<T> : IRequest,
                                        IEquatable<T>

        where T : class

    {

        #region Data

        /// <summary>
        /// The default request timeout.
        /// </summary>
        public static readonly TimeSpan DefaultRequestTimeout = TimeSpan.FromSeconds(60);

        #endregion

        #region Properties

        /// <summary>
        /// The optional timestamp of the request.
        /// </summary>
        public DateTime           Timestamp            { get; }

        /// <summary>
        /// An optional event tracking identification for correlating this request with other events.
        /// </summary>
        public EventTracking_Id   EventTrackingId      { get; }

        /// <summary>
        /// An optional timeout for this request.
        /// </summary>
        public TimeSpan?          RequestTimeout       { get; }

        /// <summary>
        /// An optional token to cancel this request.
        /// </summary>
        public CancellationToken  CancellationToken    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new generic OCHP request message.
        /// </summary>
        /// <param name="Timestamp">The optional timestamp of the request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public ARequest(DateTime?          Timestamp           = null,
                        EventTracking_Id?  EventTrackingId     = null,
                        TimeSpan?          RequestTimeout      = null,
                        CancellationToken  CancellationToken   = default)
        {

            this.Timestamp          = Timestamp       ?? org.GraphDefined.Vanaheimr.Illias.Timestamp.Now;
            this.EventTrackingId    = EventTrackingId ?? EventTracking_Id.New;
            this.RequestTimeout     = RequestTimeout  ?? DefaultRequestTimeout;
            this.CancellationToken  = CancellationToken;

        }

        #endregion


        #region IEquatable<ARequest> Members

        /// <summary>
        /// Compare two requests for equality.
        /// </summary>
        /// <param name="ARequest">Another abstract generic OICP request.</param>
        public abstract Boolean Equals(T? ARequest);

        #endregion

    }

}
