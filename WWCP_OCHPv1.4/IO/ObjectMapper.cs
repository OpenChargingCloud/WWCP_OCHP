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
using System.Collections.Generic;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// An OCHP Object Mapper for WWCP data structures.
    /// </summary>
    public static class ObjectMapper
    {

        #region AsWWCPEVSEStatus(EVSEMajorStatus, EVSEMinorStatus)

        /// <summary>
        /// Convert an OCHP EVSE status into a corresponding WWCP EVSE status.
        /// </summary>
        /// <param name="EVSEMajorStatus">An OCHP EVSE major status.</param>
        /// <param name="EVSEMinorStatus">An OCHP EVSE minor status.</param>
        /// <returns>The corresponding WWCP EVSE status.</returns>
        public static WWCP.EVSEStatusType AsWWCPEVSEStatus(EVSEMajorStatusTypes  EVSEMajorStatus,
                                                           EVSEMinorStatusTypes  EVSEMinorStatus)
        {

            switch (EVSEMajorStatus)
            {

                case EVSEMajorStatusTypes.Available:
                    switch (EVSEMinorStatus)
                    {

                        case EVSEMinorStatusTypes.Available:
                            return WWCP.EVSEStatusType.Available;

                        default:
                            return WWCP.EVSEStatusType.Unspecified;

                    }


                case EVSEMajorStatusTypes.NotAvailable:
                    switch (EVSEMinorStatus)
                    {

                        case EVSEMinorStatusTypes.Available:
                            return WWCP.EVSEStatusType.Available;

                        default:
                            return WWCP.EVSEStatusType.Unspecified;

                    }


                default:
                    return WWCP.EVSEStatusType.Unspecified;

            }

        }

        #endregion

        #region AsEVSEMajorStatus(this EVSEStatusType)

        /// <summary>
        /// Convert a WWCP EVSE status into a corresponding OCHP EVSE major status.
        /// </summary>
        /// <param name="EVSEStatusType">An WWCP EVSE status.</param>
        /// <returns>The corresponding OCHP EVSE major status.</returns>
        public static EVSEMajorStatusTypes AsEVSEMajorStatus(this WWCP.EVSEStatusType EVSEStatusType)
        {

            switch (EVSEStatusType)
            {

                case WWCP.EVSEStatusType.Available:
                    return EVSEMajorStatusTypes.Available;

                case WWCP.EVSEStatusType.Reserved:
                case WWCP.EVSEStatusType.Charging:
                case WWCP.EVSEStatusType.OutOfService:
                case WWCP.EVSEStatusType.UnknownEVSE:
                    return EVSEMajorStatusTypes.NotAvailable;

                default:
                    return EVSEMajorStatusTypes.Unknown;

            }

        }

        #endregion

        #region AsEVSEMinorStatus(this EVSEStatusType)

        /// <summary>
        /// Convert a WWCP EVSE status into a corresponding OCHP EVSE minor status.
        /// </summary>
        /// <param name="EVSEStatusType">An WWCP EVSE status.</param>
        /// <returns>The corresponding OCHP EVSE minor status.</returns>
        public static EVSEMinorStatusTypes AsEVSEMinorStatus(this WWCP.EVSEStatusType EVSEStatusType)
        {

            switch (EVSEStatusType)
            {

                case WWCP.EVSEStatusType.Available:
                    return EVSEMinorStatusTypes.Available;

                case WWCP.EVSEStatusType.Reserved:
                    return EVSEMinorStatusTypes.Reserved;

                case WWCP.EVSEStatusType.Charging:
                    return EVSEMinorStatusTypes.Charging;

                case WWCP.EVSEStatusType.OutOfService:
                    return EVSEMinorStatusTypes.OutOfOrder;

                case WWCP.EVSEStatusType.Blocked:
                    return EVSEMinorStatusTypes.Blocked;

                default:
                    return EVSEMinorStatusTypes.Unknown;

            }

        }

        #endregion


        public static IEnumerable<AuthMethodTypes> ToEnumeration(this AuthMethodTypes e)
        {

            return Enum.GetValues(typeof(AuthMethodTypes)).
                        Cast<AuthMethodTypes>().
                        Where(flag => e.HasFlag(flag) && flag != AuthMethodTypes.Unknown);

        }

        public static IEnumerable<RestrictionTypes> ToEnumeration(this RestrictionTypes e)
        {

            return Enum.GetValues(typeof(RestrictionTypes)).
                        Cast<RestrictionTypes>().
                        Where(flag => e.HasFlag(flag) && flag != RestrictionTypes.Unknown);

        }

    }

}