/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OCHPv1_4.CPO
{

    /// <summary>
    /// A delegate which allows you to modify the convertion from WWCP charge detail records to OCHP charge detail records.
    /// </summary>
    /// <param name="WWCPChargeDetailRecord">A WWCP charge detail record.</param>
    /// <param name="OCHPChargeDetailRecord">An OCHP charge detail record.</param>
    public delegate CDRInfo             WWCPChargeDetailRecord2ChargeDetailRecordDelegate(ChargeDetailRecord  WWCPChargeDetailRecord,
                                                                                          CDRInfo             OCHPChargeDetailRecord);

    /// <summary>
    /// A delegate which allows you to modify the convertion from OCHP charge detail records to WWCP charge detail records.
    /// </summary>
    /// <param name="OCHPChargeDetailRecord">An OCHP charge detail record.</param>
    /// <param name="WWCPChargeDetailRecord">A WWCP charge detail record.</param>
    public delegate ChargeDetailRecord  ChargeDetailRecord2WWCPChargeDetailRecordDelegate(CDRInfo             OCHPChargeDetailRecord,
                                                                                          ChargeDetailRecord  WWCPChargeDetailRecord);

    #region IncludeChargePoints

    /// <summary>
    /// A delegate for filtering charge points.
    /// </summary>
    /// <param name="ChargePointInfo">A charge point.</param>
    public delegate Boolean IncludeChargePointDelegate(ChargePointInfo ChargePointInfo);

    #endregion

    #region IncludeEVSEIds

    /// <summary>
    /// A delegate for filtering EVSE identifications.
    /// </summary>
    /// <param name="EVSEId">An EVSE identification.</param>
    public delegate Boolean IncludeEVSEIdsDelegate(EVSE_Id EVSEId);

    #endregion

    #region ChargePointInfo2XMLDelegate

    /// <summary>
    /// A delegate which allows you to modify the XML representation of charge point
    /// information before sending them upstream.
    /// </summary>
    /// <param name="RoamingNetwork">A roaming network.</param>
    /// <param name="ChargePointInfo">A charge point information.</param>
    /// <param name="XML">The XML representation of a charge point information.</param>
    public delegate XElement ChargePointInfo2XMLDelegate(RoamingNetwork    RoamingNetwork,
                                                         ChargePointInfo   ChargePointInfo,
                                                         XElement          XML);

    #endregion

    #region EVSEStatus2XMLDelegate

    /// <summary>
    /// A delegate which allows you to modify the XML representation
    /// of EVSE status before sending them upstream.
    /// </summary>
    /// <param name="RoamingNetwork">An EVSE roaming network.</param>
    /// <param name="EVSEStatus">An EVSE status.</param>
    /// <param name="XML">The XML representation of an EVSE status record.</param>
    public delegate XElement EVSEStatus2XMLDelegate(RoamingNetwork   RoamingNetwork,
                                                    EVSEStatus       EVSEStatus,
                                                    XElement         XML);

    #endregion

    #region XMLPostProcessingDelegate

    public delegate XElement XMLPostProcessingDelegate(XElement   XML);

    #endregion

}
