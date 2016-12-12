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

using org.GraphDefined.Vanaheimr.Illias;

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


        #region ToOCHP(this WWCPAddress)

        /// <summary>
        /// Maps a WWCP address to an OCHP address.
        /// </summary>
        /// <param name="WWCPAddress">A WWCP address.</param>
        public static Address ToOCHP(this WWCP.Address WWCPAddress)

            => new Address(WWCPAddress.HouseNumber,
                           WWCPAddress.Street,
                           WWCPAddress.City.FirstText,
                           WWCPAddress.PostalCode,
                           WWCPAddress.Country);

        #endregion

        #region ToWWCP(this OCHPAddress)

        /// <summary>
        /// Maps an OCHP accessibility type to a WWCP accessibility type.
        /// </summary>
        /// <param name="OCHPAddress">A accessibility type.</param>
        public static WWCP.Address ToWWCP(this Address OCHPAddress)

            => new WWCP.Address(OCHPAddress.Street,
                                OCHPAddress.HouseNumber,
                                null,
                                OCHPAddress.ZIPCode,
                                null,
                                I18NString.Create(Languages.unknown, OCHPAddress.City),
                                OCHPAddress.Country);

        #endregion


        #region ToOCHP(this ChargeDetailRecord)

        /// <summary>
        /// Convert a WWCP charge detail record into a corresponding OCHP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">A WWCP charge detail record.</param>
        public static CDRInfo ToOCHP(this WWCP.ChargeDetailRecord ChargeDetailRecord)

            => new CDRInfo(
                   CDR_Id.Parse(ChargeDetailRecord.SessionId.ToString()),
                   EMT_Id.Parse(""),
                   Contract_Id.Parse(""),

                   ChargeDetailRecord.EVSEId.Value.ToOCHP(),
                   ChargePointTypes.Unknown,
                   ChargeDetailRecord.EVSE.ToOCHP().Connectors.First(),

                   CDRStatus.New,
                   ChargeDetailRecord.SessionTime.Value.StartTime,
                   ChargeDetailRecord.SessionTime.Value.EndTime.Value,
                   new CDRPeriod[0],

                   ChargeDetailRecord.Duration,
                   ChargeDetailRecord.ChargingPool?.Address?.ToOCHP(),
                   null, // Ratings
                   ChargeDetailRecord.EnergyMeterId?.ToString()
                   // TotalCosts
                   // Currency
               );

        #endregion



        public static EVSE_Id ToOCHP(this WWCP.EVSE_Id EVSEId)
            => EVSE_Id.Parse(EVSEId.ToString());

        public static WWCP.EVSE_Id ToWWCP(this EVSE_Id EVSEId)
            => WWCP.EVSE_Id.Parse(EVSEId.ToString());


        public static Provider_Id ToOCHP(this WWCP.eMobilityProvider_Id ProviderId)
            => Provider_Id.Parse(ProviderId.ToString());

        public static WWCP.eMobilityProvider_Id ToWWCP(this Provider_Id ProviderId)
            => WWCP.eMobilityProvider_Id.Parse(ProviderId.ToString());


        #region AuthenticationModes

        #region ToOCHP(AuthenticationModes)

        /// <summary>
        /// Maps a WWCP authentication mode to an OCHP authentication mode.
        /// </summary>
        /// <param name="WWCPAuthMode">A WWCP-representation of an authentication mode.</param>
        public static AuthMethodTypes ToOCHP(this WWCP.AuthenticationModes AuthenticationModes)
        {

            var _AuthenticationModes = AuthMethodTypes.Unknown;

            switch (AuthenticationModes.Type)
            {

                case "RFID":
                    _AuthenticationModes |= AuthMethodTypes.RFIDMifareClassic;
                    _AuthenticationModes |= AuthMethodTypes.RFIDMifareDESFire;
                    _AuthenticationModes |= AuthMethodTypes.RFIDCalypso;
                    break;

                case "ISO/IEC 15118 PLC":
                    _AuthenticationModes |= AuthMethodTypes.IEC15118;
                    break;

                case "REMOTE":
                    _AuthenticationModes |= AuthMethodTypes.OCHPDirectAuth;
                    break;

                case "Direct payment":
                    _AuthenticationModes |= AuthMethodTypes.DirectCash;
                    _AuthenticationModes |= AuthMethodTypes.DirectCreditcard;
                    _AuthenticationModes |= AuthMethodTypes.DirectDebitcard;
                    break;

            }

            return _AuthenticationModes;

        }

        #endregion

        #region ToWWCP(AuthMethodType)

        public static WWCP.AuthenticationModes ToWWCP(this AuthMethodTypes AuthMethodType)
        {

            switch (AuthMethodType)
            {

                //case AuthMethodTypes.NFC_RFID_Classic:
                //    return WWCP.AuthenticationModes.RFID(RFIDAuthenticationModes.MifareClassic);

                //case AuthMethodTypes.NFC_RFID_DESFire:
                //    return WWCP.AuthenticationModes.RFID(RFIDAuthenticationModes.MifareDESFire);

                //case AuthMethodTypes.PnC:
                //    return WWCP.AuthenticationModes.ISO15118_PLC;

                //case AuthMethodTypes.REMOTE:
                //    return WWCP.AuthenticationModes.REMOTE;

                //case AuthMethodTypes.DirectPayment:
                //    return WWCP.AuthenticationModes.DirectPayment;


                default:
                    return WWCP.AuthenticationModes.Unkown;

            }

        }

        #endregion

        public static AuthMethodTypes AsOCHPAuthenticationModes(this IEnumerable<WWCP.AuthenticationModes> WWCPAuthenticationModes)
        {

            var _AuthMethodTypes = AuthMethodTypes.Unknown;

            foreach (var AuthenticationMode in WWCPAuthenticationModes)
                _AuthMethodTypes |= AuthenticationMode.ToOCHP();

            return _AuthMethodTypes;

        }

        public static AuthMethodTypes Reduce(this IEnumerable<AuthMethodTypes> AuthenticationModes)
        {

            var _AuthMethodTypes = AuthMethodTypes.Unknown;

            foreach (var _AuthenticationMode in AuthenticationModes)
                _AuthMethodTypes |= _AuthenticationMode;

            return _AuthMethodTypes;

        }

        public static IEnumerable<AuthMethodTypes> ToEnumeration(this AuthMethodTypes e)

            => Enum.GetValues(typeof(AuthMethodTypes)).
                    Cast<AuthMethodTypes>().
                    Where(flag => e.HasFlag(flag) && flag != AuthMethodTypes.Unknown);

        #endregion


        public static ConnectorStandards ToOCHPConnectorStandard(this PlugTypes PlugType)
        {

            switch (PlugType)
            {

                case PlugTypes.Type2Outlet:
                    return ConnectorStandards.IEC_62196_T2;

                default:
                    return ConnectorStandards.Unknown;

            }

        }

        public static ConnectorFormats ToOCHPConnectorFormat(this PlugTypes PlugType)
        {

            switch (PlugType)
            {

                case PlugTypes.Type2Outlet:
                    return ConnectorFormats.Socket;

                default:
                    return ConnectorFormats.Unknown;

            }

        }

        public static ConnectorType ToOCHP(this SocketOutlet WWCPSocketOutlet)

            => new ConnectorType(WWCPSocketOutlet.Plug.ToOCHPConnectorStandard(),
                                 WWCPSocketOutlet.Plug.ToOCHPConnectorFormat());


        #region ToOCHP(this EVSE, CustomEVSEIdMapper = null, EVSE2ChargePointInfo = null)

        /// <summary>
        /// Convert a WWCP EVSE into a corresponding OCHP charge point info.
        /// </summary>
        /// <param name="EVSE">A WWCP EVSE.</param>
        /// <param name="CustomEVSEIdMapper">A custom WWCP EVSE Id to OCHP EVSE Id mapper.</param>
        /// <param name="EVSE2ChargePointInfo">A delegate to process an OCHP charge point info, e.g. before pushing it to a roaming provider.</param>
        public static ChargePointInfo ToOCHP(this EVSE                         EVSE,
                                             CPO.CustomEVSEIdMapperDelegate    CustomEVSEIdMapper    = null,
                                             CPO.EVSE2ChargePointInfoDelegate  EVSE2ChargePointInfo  = null)
        {

            var _ChargePointInfo = new ChargePointInfo(CustomEVSEIdMapper != null ? CustomEVSEIdMapper(EVSE.Id) : EVSE.Id.ToOCHP(),
                                                       ChargePointInfo.LocationIdInverse_RegEx.Replace(EVSE.ChargingStation.ChargingPool.Id.ToString(), "").SubstringMax(15),
                                                       EVSE.ChargingStation.ChargingPool.Name.FirstText.ToUpper(),
                                                       EVSE.ChargingStation.ChargingPool.Name.First().Language.ToString(),
                                                       EVSE.ChargingStation.Address.ToOCHP(),
                                                       EVSE.ChargingStation.GeoLocation.Value,
                                                       GeneralLocationTypes.Other,
                                                       EVSE.ChargingStation.AuthenticationModes.
                                                                                Select(mode => mode.ToOCHP()).
                                                                                Where (mode => mode != AuthMethodTypes.Unknown).
                                                                                Reduce(),
                                                       EVSE.SocketOutlets.SafeSelect(socketoutlet => socketoutlet.ToOCHP()),
                                                       ChargePointTypes.AC,                 // FixMe: ChargePointTypes.AC!
                                                       DateTime.Now,                        // timestamp of last edit
                                                       new EVSEImageURL[0],
                                                       new RelatedResource[0],
                                                       new ExtendedGeoCoordinate[0],
                                                       null,                                // Timezone
                                                       Hours.Open24_7,
                                                       ChargePointStatus.Operative,
                                                       new ChargePointSchedule[0],
                                                       EVSE.ChargingStation.HotlinePhoneNumber,
                                                       new ParkingSpotInfo[0],
                                                       RestrictionTypes.EVOnly,
                                                       null,                                // Ratings
                                                       null,                                // UserInterface language
                                                       null);                               // Max Reservation Time

            return EVSE2ChargePointInfo != null
                       ? EVSE2ChargePointInfo(EVSE, _ChargePointInfo)
                       : _ChargePointInfo;

        }

        #endregion

    }

}