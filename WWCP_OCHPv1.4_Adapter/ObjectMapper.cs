/*
 * Copyright (c) 2014-2018 GraphDefined GmbH
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
        public static EVSEStatusTypes AsWWCPEVSEStatus(EVSEMajorStatusTypes  EVSEMajorStatus,
                                                       EVSEMinorStatusTypes  EVSEMinorStatus)
        {

            switch (EVSEMajorStatus)
            {

                case EVSEMajorStatusTypes.Available:
                    switch (EVSEMinorStatus)
                    {

                        case EVSEMinorStatusTypes.Available:
                            return WWCP.EVSEStatusTypes.Available;

                        default:
                            return WWCP.EVSEStatusTypes.Unspecified;

                    }


                case EVSEMajorStatusTypes.NotAvailable:
                    switch (EVSEMinorStatus)
                    {

                        case EVSEMinorStatusTypes.Available:
                            return WWCP.EVSEStatusTypes.Available;

                        default:
                            return WWCP.EVSEStatusTypes.Unspecified;

                    }


                default:
                    return WWCP.EVSEStatusTypes.Unspecified;

            }

        }

        #endregion

        #region AsEVSEMajorStatus(this EVSEStatusType)

        /// <summary>
        /// Convert a WWCP EVSE status into a corresponding OCHP EVSE major status.
        /// </summary>
        /// <param name="EVSEStatusType">An WWCP EVSE status.</param>
        /// <returns>The corresponding OCHP EVSE major status.</returns>
        public static EVSEMajorStatusTypes AsEVSEMajorStatus(this EVSEStatusTypes EVSEStatusType)
        {

            switch (EVSEStatusType)
            {

                case EVSEStatusTypes.Available:
                    return EVSEMajorStatusTypes.Available;

                default:
                    return EVSEMajorStatusTypes.NotAvailable;

            }

        }

        #endregion

        #region AsEVSEMinorStatus(this EVSEStatusType)

        /// <summary>
        /// Convert a WWCP EVSE status into a corresponding OCHP EVSE minor status.
        /// </summary>
        /// <param name="EVSEStatusType">An WWCP EVSE status.</param>
        /// <returns>The corresponding OCHP EVSE minor status.</returns>
        public static EVSEMinorStatusTypes AsEVSEMinorStatus(this EVSEStatusTypes EVSEStatusType)
        {

            switch (EVSEStatusType)
            {

                // Unused: EVSEMinorStatusTypes.Blocked;

                case EVSEStatusTypes.Available:
                    return EVSEMinorStatusTypes.Available;

                case EVSEStatusTypes.Reserved:
                    return EVSEMinorStatusTypes.Reserved;

                case EVSEStatusTypes.Charging:
                    return EVSEMinorStatusTypes.Charging;

                default:
                    return EVSEMinorStatusTypes.OutOfOrder;

            }

        }

        #endregion


        #region Convert EVSE Ids...

        public static EVSE_Id ToOCHP(this WWCP.EVSE_Id EVSEId)
            => EVSE_Id.Parse(EVSEId.ToString());

        public static EVSE_Id? ToOCHP(this WWCP.EVSE_Id? EVSEId)
            => EVSEId.HasValue
                   ? EVSE_Id.Parse(EVSEId.ToString())
                   : new EVSE_Id?();


        public static WWCP.EVSE_Id ToWWCP(this EVSE_Id EVSEId)
            => WWCP.EVSE_Id.Parse(EVSEId.ToString());

        public static WWCP.EVSE_Id? ToWWCP(this EVSE_Id? EVSEId)
            => EVSEId.HasValue
                   ? WWCP.EVSE_Id.Parse(EVSEId.ToString())
                   : new WWCP.EVSE_Id?();

        #endregion

        #region Convert Provider Ids...

        public static Provider_Id ToOCHP(this eMobilityProvider_Id ProviderId)
            => Provider_Id.Parse(ProviderId.ToString());

        public static Provider_Id? ToOCHP(this eMobilityProvider_Id? ProviderId)
            => ProviderId.HasValue
                   ? Provider_Id.Parse(ProviderId.ToString())
                   : new Provider_Id?();


        public static eMobilityProvider_Id ToWWCP(this Provider_Id ProviderId)
            => eMobilityProvider_Id.Parse(ProviderId.ToString());

        public static eMobilityProvider_Id? ToWWCP(this Provider_Id? ProviderId)
            => ProviderId.HasValue
                   ? eMobilityProvider_Id.Parse(ProviderId.ToString())
                   : new eMobilityProvider_Id?();

        #endregion

        #region Convert ChargingSession Ids...

        public static CDR_Id ToOCHP(this ChargingSession_Id CDRId)
            => CDR_Id.Parse(CDRId.ToString());

        public static CDR_Id? ToOCHP(this ChargingSession_Id? CDRId)
            => CDRId.HasValue
                   ? CDR_Id.Parse(CDRId.ToString())
                   : new CDR_Id?();


        public static ChargingSession_Id ToWWCP(this CDR_Id CDRId)
            => ChargingSession_Id.Parse(CDRId.ToString());

        public static ChargingSession_Id? ToWWCP(this CDR_Id? CDRId)
            => CDRId.HasValue
                   ? ChargingSession_Id.Parse(CDRId.ToString())
                   : new ChargingSession_Id?();

        #endregion

        #region Convert Addresses...

        /// <summary>
        /// Maps a WWCP address to an OCHP address.
        /// </summary>
        /// <param name="WWCPAddress">A WWCP address.</param>
        public static Address ToOCHP(this Vanaheimr.Illias.Address WWCPAddress)

            => new Address(WWCPAddress.HouseNumber.ToUpper(),
                           WWCPAddress.Street,
                           WWCPAddress.City.FirstText(),
                           WWCPAddress.PostalCode,
                           WWCPAddress.Country);


        /// <summary>
        /// Maps an OCHP accessibility type to a WWCP accessibility type.
        /// </summary>
        /// <param name="OCHPAddress">A accessibility type.</param>
        public static Vanaheimr.Illias.Address ToWWCP(this Address OCHPAddress)

            => Vanaheimr.Illias.Address.Create(OCHPAddress.Country,
                                               OCHPAddress.ZIPCode,
                                               I18NString.Create(Languages.unknown, OCHPAddress.City),
                                               OCHPAddress.Street,
                                               OCHPAddress.HouseNumber);

        #endregion

        #region Convert AuthenticationModes...

        #region ToOCHP(AuthenticationModes)

        /// <summary>
        /// Maps a WWCP authentication mode to an OCHP authentication mode.
        /// </summary>
        /// <param name="AuthenticationModes">A WWCP-representation of an authentication mode.</param>
        public static AuthMethodTypes ToOCHP(this AuthenticationModes AuthenticationModes)
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

        public static AuthenticationModes ToWWCP(this AuthMethodTypes AuthMethodType)
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

        public static AuthMethodTypes AsOCHPAuthenticationModes(this IEnumerable<AuthenticationModes> WWCPAuthenticationModes)
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

        #region Convert SocketOutlets...

        public static ConnectorType ToOCHP(this SocketOutlet WWCPSocketOutlet)
        {

            switch (WWCPSocketOutlet.Plug)
            {

                //case PlugTypes.SmallPaddleInductive:
                //case PlugTypes.LargePaddleInductive:
                //case PlugTypes.AVCONConnector:

                case PlugTypes.TESLA_Roadster:
                    return new ConnectorType(ConnectorStandards.TESLA_R,
                                             ConnectorFormats.Socket);

                case PlugTypes.TESLA_ModelS:
                    return new ConnectorType(ConnectorStandards.TESLA_S,
                                             ConnectorFormats.Socket);

                //case PlugTypes.NEMA5_20:

                case PlugTypes.TypeEFrenchStandard:
                    return new ConnectorType(ConnectorStandards.DOMESTIC_E,
                                             ConnectorFormats.Socket);

                case PlugTypes.TypeFSchuko:
                    return new ConnectorType(ConnectorStandards.DOMESTIC_F,
                                             ConnectorFormats.Socket);

                case PlugTypes.TypeGBritishStandard:
                    return new ConnectorType(ConnectorStandards.DOMESTIC_G,
                                             ConnectorFormats.Socket);

                case PlugTypes.TypeJSwissStandard:
                    return new ConnectorType(ConnectorStandards.DOMESTIC_J,
                                             ConnectorFormats.Socket);

                case PlugTypes.Type1Connector_CableAttached:
                    return new ConnectorType(ConnectorStandards.IEC_62196_T1,
                                             ConnectorFormats.Socket);

                case PlugTypes.Type2Outlet:
                    return new ConnectorType(ConnectorStandards.IEC_62196_T2,
                                             ConnectorFormats.Socket);

                case PlugTypes.Type2Connector_CableAttached:
                    return new ConnectorType(ConnectorStandards.IEC_62196_T2,
                                             ConnectorFormats.Cable);

                //case PlugTypes.Type3Outlet:
                //return ConnectorStandards.IEC_62196_T3A;
                //return ConnectorStandards.IEC_62196_T3C;

                case PlugTypes.IEC60309SinglePhase:
                    return new ConnectorType(ConnectorStandards.IEC_60309_2_single_16,
                                             ConnectorFormats.Socket);

                case PlugTypes.IEC60309ThreePhase:
                    return new ConnectorType(ConnectorStandards.IEC_60309_2_three_16,
                                             ConnectorFormats.Socket);
                //return ConnectorStandards.IEC_60309_2_three_32;
                //return ConnectorStandards.IEC_60309_2_three_64;

                case PlugTypes.CCSCombo1Plug_CableAttached:
                    return new ConnectorType(ConnectorStandards.IEC_62196_T1_COMBO,
                                             ConnectorFormats.Cable);

                case PlugTypes.CCSCombo2Plug_CableAttached:
                    return new ConnectorType(ConnectorStandards.IEC_62196_T2_COMBO,
                                             ConnectorFormats.Cable);

                case PlugTypes.CHAdeMO:
                    return new ConnectorType(ConnectorStandards.Chademo,
                                             ConnectorFormats.Socket);


                //return ConnectorStandards.DOMESTIC_A;
                //return ConnectorStandards.DOMESTIC_B;
                //return ConnectorStandards.DOMESTIC_C;
                //return ConnectorStandards.DOMESTIC_D;
                //return ConnectorStandards.DOMESTIC_H;
                //return ConnectorStandards.DOMESTIC_I;
                //return ConnectorStandards.DOMESTIC_K;
                //return ConnectorStandards.DOMESTIC_L;


                default:
                    return new ConnectorType(ConnectorStandards.Unknown,
                                             ConnectorFormats.Unknown);

            }


        }

        #endregion



        #region Convert ChargeDetailRecords...

        /// <summary>
        /// Convert a WWCP charge detail record into a corresponding OCHP charge detail record.
        /// </summary>
        /// <param name="ChargeDetailRecord">A WWCP charge detail record.</param>
        public static CDRInfo ToOCHP(this ChargeDetailRecord         ChargeDetailRecord,
                                     Func<EMT_Id, Contract_Id>       ContractIdDelegate,
                                     CPO.CustomEVSEIdMapperDelegate  CustomEVSEIdMapper   = null)

        {

            var EMTId = new EMT_Id(ChargeDetailRecord.IdentificationStart.AuthToken.ToString(),
                                   TokenRepresentations.Plain,
                                   TokenTypes.RFID);


            var cdr = new CDRInfo(
                          CDR_Id.Parse(ChargeDetailRecord.EVSEId.Value.OperatorId.ToString().Replace("*", "").Replace("+49822", "DEBDO") +
                                      (ChargeDetailRecord.SessionId.ToString().Replace("-", "").SubstringMax(30).ToUpper())),
                          EMTId,
                          ContractIdDelegate(EMTId),// Contract_Id.Parse(ChargeDetailRecord.GetCustomDataAs<String>("ContractId")),

                          CustomEVSEIdMapper != null && ChargeDetailRecord.EVSEId.HasValue
                              ? CustomEVSEIdMapper(ChargeDetailRecord.EVSEId.Value)
                              : ChargeDetailRecord.EVSEId.ToOCHP().Value,
                          ChargeDetailRecord.EVSE.ToOCHP().Connectors.First().Standard.GetChargePointType(),// ChargePointTypes.AC,  // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                          ChargeDetailRecord.EVSE.ToOCHP().Connectors.First(),

                          CDRStatus.New,
                          ChargeDetailRecord.SessionTime.Value.StartTime,
                          ChargeDetailRecord.SessionTime.Value.EndTime.Value,
                          new CDRPeriod[] {
                              new CDRPeriod(ChargeDetailRecord.EnergyMeteringValues.First().Timestamp,
                                            ChargeDetailRecord.EnergyMeteringValues.Last(). Timestamp,
                                            BillingItems.Energy,
                                            ChargeDetailRecord.ConsumedEnergy,
                                            0)
                          },
                          Currency.EUR,

                          ChargeDetailRecord.EVSE?.ChargingStation?.ChargingPool?.Address?.ToOCHP(),
                          ChargeDetailRecord.Duration,
                          null, // Ratings
                          ChargeDetailRecord.EnergyMeterId?.ToString()
                          // TotalCosts
                      );

            return cdr;

        }


        /// <summary>
        /// Convert an OCHP charge detail record info into a corresponding WWCP charge detail record.
        /// </summary>
        /// <param name="CDRInfo">A WWCP charge detail record.</param>
        public static ChargeDetailRecord ToWWCP(this CDRInfo CDRInfo)

            => new ChargeDetailRecord(
                   ChargingSession_Id.Parse(CDRInfo.CDRId.ToString()),
                   new StartEndDateTime(CDRInfo.StartDateTime, CDRInfo.EndDateTime),
                   Duration:             CDRInfo.Duration,
                   EVSEId:               CDRInfo.EVSEId.ToWWCP(),
                   AuthenticationStart:  RemoteAuthentication.FromRemoteIdentification(eMobilityAccount_Id.Parse(CDRInfo.ContractId.ToString()))
               );

        #endregion








        #region ToOCHP(this EVSE, CustomEVSEIdMapper = null, EVSE2ChargePointInfo = null)

        /// <summary>
        /// Convert a WWCP EVSE into a corresponding OCHP charge point info.
        /// </summary>
        /// <param name="EVSE">A WWCP EVSE.</param>
        /// <param name="CustomEVSEIdMapper">A custom WWCP EVSE Id to OCHP EVSE Id mapper.</param>
        /// <param name="EVSE2ChargePointInfo">A delegate to process an OCHP charge point info, e.g. before pushing it to a roaming provider.</param>
        public static ChargePointInfo ToOCHP(this EVSE                         EVSE,
                                             CPO.CustomEVSEIdMapperDelegate    CustomEVSEIdMapper     = null,
                                             CPO.EVSE2ChargePointInfoDelegate  EVSE2ChargePointInfo   = null)
        {

            if (EVSE == null)
                return null;

            try
            {

                var _ChargePointInfo = new ChargePointInfo(CustomEVSEIdMapper != null
                                                               ? CustomEVSEIdMapper(EVSE.Id)
                                                               : EVSE.Id.ToOCHP(),
                                                           ChargePointInfo.LocationIdInverse_RegEx.Replace(EVSE.ChargingStation.ChargingPool.Id.ToString(), "").SubstringMax(15),
                                                           EVSE.ChargingStation.ChargingPool.Name.FirstText().ToUpper(),
                                                           EVSE.ChargingStation.ChargingPool.Name.First().Language.ToString(),
                                                           EVSE.ChargingStation.Address.ToOCHP(),
                                                           EVSE.ChargingStation.GeoLocation.Value,
                                                           GeneralLocationTypes.Other,
                                                           EVSE.ChargingStation.AuthenticationModes.
                                                                                    Select(mode => mode.ToOCHP()).
                                                                                    Where(mode => mode != AuthMethodTypes.Unknown).
                                                                                    Reduce(),
                                                           EVSE.SocketOutlets.SafeSelect(ToOCHP),
                                                           ChargePointTypes.AC,                 // FixMe: ChargePointTypes.AC!
                                                           DateTime.Now,                        // timestamp of last edit
                                                           new EVSEImageURL[0],
                                                           new RelatedResource[0],
                                                           new ExtendedGeoCoordinate[0],
                                                           null,                                // Timezone
                                                           Hours.Open24_7,
                                                           ChargePointStatus.Operative,
                                                           new ChargePointSchedule[0],
                                                           EVSE.ChargingStation.HotlinePhoneNumber.IsNeitherNullNorEmpty()
                                                               ? EVSE.ChargingStation.HotlinePhoneNumber.FirstText().Replace("+", "00") // ToDo: Bugfix for VW!
                                                               : null,
                                                           new ParkingSpotInfo[0],
                                                           RestrictionTypes.EVOnly,
                                                           null,                                // Ratings
                                                           null,                                // UserInterface language
                                                           null);                               // Max Reservation Time

                return EVSE2ChargePointInfo != null
                           ? EVSE2ChargePointInfo(EVSE, _ChargePointInfo)
                           : _ChargePointInfo;

            }
            catch (Exception)
            { }

            return null;

        }

        #endregion

    }

}