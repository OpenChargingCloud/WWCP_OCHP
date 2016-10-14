﻿/*
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

using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// A delegate which allows you to modify OCHP charge point information before
    /// sending them upstream.
    /// </summary>
    /// <param name="EVSE">A WWCP EVSE.</param>
    /// <param name="ChargePointInfo">A charge point information.</param>
    public delegate ChargePointInfo EVSE2ChargePointInfoDelegate (EVSE              EVSE,
                                                                  ChargePointInfo   ChargePointInfo);

    /// <summary>
    /// A delegate which allows you to modify the XML representation of charge point
    /// information before sending them upstream.
    /// </summary>
    /// <param name="RoamingNetwork">A roaming network.</param>
    /// <param name="ChargePointInfo">A charge point information.</param>
    /// <param name="XML">The XML representation of a charge point information.</param>
    public delegate XElement       ChargePointInfo2XMLDelegate  (RoamingNetwork     RoamingNetwork,
                                                                 ChargePointInfo    ChargePointInfo,
                                                                 XElement           XML);

    /// <summary>
    /// A delegate which allows you to modify the XML representation
    /// of EVSE status before sending them upstream.
    /// </summary>
    /// <param name="RoamingNetwork">An EVSE roaming network.</param>
    /// <param name="EVSEStatus">An EVSE status.</param>
    /// <param name="XML">The XML representation of an EVSE status record.</param>
    public delegate XElement       EVSEStatus2XMLDelegate       (RoamingNetwork     RoamingNetwork,
                                                                 EVSEStatus         EVSEStatus,
                                                                 XElement           XML);

    public delegate XElement       XMLPostProcessingDelegate    (XElement           XML);

}