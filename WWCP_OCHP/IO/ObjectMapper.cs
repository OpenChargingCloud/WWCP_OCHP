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

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// An OCHP Object Mapper.
    /// </summary>
    public static class ObjectMapper
    {

        #region AsAuthMethodType(Text)

        public static AuthMethodTypes AsAuthMethodType(this String Text)
        {

            switch (Text)
            {

                case "Public":
                    return AuthMethodTypes.Public;

                case "LocalKey":
                    return AuthMethodTypes.LocalKey;

                case "DirectCash":
                    return AuthMethodTypes.DirectCash;

                case "DirectCreditcard":
                    return AuthMethodTypes.DirectCreditcard;

                case "DirectDebitcard":
                    return AuthMethodTypes.DirectDebitcard;

                case "RfidMifareCls":
                    return AuthMethodTypes.RFIDMifareClassic;

                case "RfidMifareDes":
                    return AuthMethodTypes.RFIDMifareDESFire;

                case "RfidCalypso":
                    return AuthMethodTypes.RFIDCalypso;

                case "Iec15118":
                    return AuthMethodTypes.IEC15118;

                case "OchpDirectAuth":
                    return AuthMethodTypes.OCHPDirectAuth;

                case "OperatorAuth":
                    return AuthMethodTypes.OperatorAuth;

                default:
                    return AuthMethodTypes.Unknown;

            }

        }

        #endregion

        #region AsText(this AuthMethodType)

        public static String AsText(this AuthMethodTypes AuthMethodType)
        {

            switch (AuthMethodType)
            {

                case AuthMethodTypes.Public:
                    return "Public";

                case AuthMethodTypes.LocalKey:
                    return "LocalKey";

                case AuthMethodTypes.DirectCash:
                    return "DirectCash";

                case AuthMethodTypes.DirectCreditcard:
                    return "DirectCreditcard";

                case AuthMethodTypes.DirectDebitcard:
                    return "DirectDebitcard";

                case AuthMethodTypes.RFIDMifareClassic:
                    return "RfidMifareCls";

                case AuthMethodTypes.RFIDMifareDESFire:
                    return "RfidMifareDes";

                case AuthMethodTypes.RFIDCalypso:
                    return "RfidCalypso";

                case AuthMethodTypes.IEC15118:
                    return "Iec15118";

                case AuthMethodTypes.OCHPDirectAuth:
                    return "OchpDirectAuth";

                case AuthMethodTypes.OperatorAuth:
                    return "OperatorAuth";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsParkingRestrictionType(Text)

        public static ParkingRestrictionTypes AsParkingRestrictionType(this String Text)
        {

            switch (Text)
            {

                case "evonly":
                    return ParkingRestrictionTypes.EVOnly;

                case "plugged":
                    return ParkingRestrictionTypes.Plugged;

                case "disabled":
                    return ParkingRestrictionTypes.Disabled;

                case "customers":
                    return ParkingRestrictionTypes.Customers;

                case "motorcycles":
                    return ParkingRestrictionTypes.Motorcycles;

                case "carsharing":
                    return ParkingRestrictionTypes.CarSharing;

                default:
                    return ParkingRestrictionTypes.Unknown;

            }

        }

        #endregion

        #region AsText(this ParkingRestrictionType)

        public static String AsText(this ParkingRestrictionTypes ParkingRestrictionType)
        {

            switch (ParkingRestrictionType)
            {

                case ParkingRestrictionTypes.EVOnly:
                    return "evonly";

                case ParkingRestrictionTypes.Plugged:
                    return "plugged";

                case ParkingRestrictionTypes.Disabled:
                    return "disabled";

                case ParkingRestrictionTypes.Customers:
                    return "customers";

                case ParkingRestrictionTypes.Motorcycles:
                    return "motorcycles";

                case ParkingRestrictionTypes.CarSharing:
                    return "carsharing";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsConnectorStandard(Text)

        public static ConnectorStandards AsConnectorStandard(this String Text)
        {

            switch (Text)
            {

                case "Chademo":
                    return ConnectorStandards.Chademo;

                case "IEC-62196-T1":
                    return ConnectorStandards.IEC_62196_T1;

                case "IEC-62196-T1-COMBO":
                    return ConnectorStandards.IEC_62196_T1_COMBO;

                case "IEC-62196-T2":
                    return ConnectorStandards.IEC_62196_T2;

                case "IEC-62196-T2-COMBO":
                    return ConnectorStandards.IEC_62196_T2_COMBO;

                case "IEC-62196-T3A":
                    return ConnectorStandards.IEC_62196_T3A;

                case "IEC-62196-T3C":
                    return ConnectorStandards.IEC_62196_T3C;

                case "DOMESTIC-A":
                    return ConnectorStandards.DOMESTIC_A;

                case "DOMESTIC-B":
                    return ConnectorStandards.DOMESTIC_B;

                case "DOMESTIC-C":
                    return ConnectorStandards.DOMESTIC_C;

                case "DOMESTIC-D":
                    return ConnectorStandards.DOMESTIC_D;

                case "DOMESTIC-E":
                    return ConnectorStandards.DOMESTIC_E;

                case "DOMESTIC-F":
                    return ConnectorStandards.DOMESTIC_F;

                case "DOMESTIC-G":
                    return ConnectorStandards.DOMESTIC_G;

                case "DOMESTIC-H":
                    return ConnectorStandards.DOMESTIC_H;

                case "DOMESTIC-I":
                    return ConnectorStandards.DOMESTIC_I;

                case "DOMESTIC-J":
                    return ConnectorStandards.DOMESTIC_J;

                case "DOMESTIC-K":
                    return ConnectorStandards.DOMESTIC_K;

                case "DOMESTIC-L":
                    return ConnectorStandards.DOMESTIC_L;

                case "TESLA-R":
                    return ConnectorStandards.TESLA_R;

                case "TESLA-S":
                    return ConnectorStandards.TESLA_S;

                case "IEC-60309-2-single-16":
                    return ConnectorStandards.IEC_60309_2_single_16;

                case "IEC-60309-2-three-16":
                    return ConnectorStandards.IEC_60309_2_three_16;

                case "IEC-60309-2-three-32":
                    return ConnectorStandards.IEC_60309_2_three_32;

                case "IEC-60309-2-three-64":
                    return ConnectorStandards.IEC_60309_2_three_64;

                default:
                    return ConnectorStandards.Unknown;

            }

        }

        #endregion

        #region AsText(this ConnectorStandard)

        public static String AsText(this ConnectorStandards ConnectorStandard)
        {

            switch (ConnectorStandard)
            {

                case ConnectorStandards.Chademo:
                    return "Chademo";

                case ConnectorStandards.IEC_62196_T1:
                    return "IEC-62196-T1";

                case ConnectorStandards.IEC_62196_T1_COMBO:
                    return "IEC-62196-T1-COMBO";

                case ConnectorStandards.IEC_62196_T2:
                    return "IEC-62196-T2";

                case ConnectorStandards.IEC_62196_T2_COMBO:
                    return "IEC-62196-T2-COMBO";

                case ConnectorStandards.IEC_62196_T3A:
                    return "IEC-62196-T3A";

                case ConnectorStandards.IEC_62196_T3C:
                    return "IEC-62196-T3C";

                case ConnectorStandards.DOMESTIC_A:
                    return "DOMESTIC-A";

                case ConnectorStandards.DOMESTIC_B:
                    return "DOMESTIC-B";

                case ConnectorStandards.DOMESTIC_C:
                    return "DOMESTIC-C";

                case ConnectorStandards.DOMESTIC_D:
                    return "DOMESTIC-D";

                case ConnectorStandards.DOMESTIC_E:
                    return "DOMESTIC-E";

                case ConnectorStandards.DOMESTIC_F:
                    return "DOMESTIC-F";

                case ConnectorStandards.DOMESTIC_G:
                    return "DOMESTIC-G";

                case ConnectorStandards.DOMESTIC_H:
                    return "DOMESTIC-H";

                case ConnectorStandards.DOMESTIC_I:
                    return "DOMESTIC-I";

                case ConnectorStandards.DOMESTIC_J:
                    return "DOMESTIC-J";

                case ConnectorStandards.DOMESTIC_K:
                    return "DOMESTIC-K";

                case ConnectorStandards.DOMESTIC_L:
                    return "DOMESTIC-L";

                case ConnectorStandards.TESLA_R:
                    return "TESLA-R";

                case ConnectorStandards.TESLA_S:
                    return "TESLA-S";

                case ConnectorStandards.IEC_60309_2_single_16:
                    return "IEC-60309-2-single-16";

                case ConnectorStandards.IEC_60309_2_three_16:
                    return "IEC-60309-2-three-16";

                case ConnectorStandards.IEC_60309_2_three_32:
                    return "IEC-60309-2-three-32";

                case ConnectorStandards.IEC_60309_2_three_64:
                    return "IEC-60309-2-three-64";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsConnectorFormat(Text)

        public static ConnectorFormats AsConnectorFormat(this String Text)
        {

            switch (Text)
            {

                case "Socket":
                    return ConnectorFormats.Socket;

                case "Cable":
                    return ConnectorFormats.Cable;

                default:
                    return ConnectorFormats.Unknown;

            }

        }

        #endregion

        #region AsText(this ConnectorFormat)

        public static String AsText(this ConnectorFormats ConnectorFormat)
        {

            switch (ConnectorFormat)
            {

                case ConnectorFormats.Socket:
                    return "Socket";

                case ConnectorFormats.Cable:
                    return "Cable";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsEVSEMajorStatusType(Text)

        public static EVSEMajorStatusTypes AsEVSEMajorStatusType(this String Text)
        {

            switch (Text)
            {

                case "available":
                    return EVSEMajorStatusTypes.Available;

                case "not-available":
                    return EVSEMajorStatusTypes.NotAvailable;

                default:
                    return EVSEMajorStatusTypes.Unknown;

            }

        }

        #endregion

        #region AsText(this EVSEMajorStatusType)

        public static String AsText(this EVSEMajorStatusTypes EVSEMajorStatusType)
        {

            switch (EVSEMajorStatusType)
            {

                case EVSEMajorStatusTypes.Available:
                    return "available";

                case EVSEMajorStatusTypes.NotAvailable:
                    return "not-available";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsEVSEMinorStatusType(Text)

        public static EVSEMinorStatusTypes AsEVSEMinorStatusType(this String Text)
        {

            switch (Text)
            {

                case "available":
                    return EVSEMinorStatusTypes.Available;

                case "reserved":
                    return EVSEMinorStatusTypes.Reserved;

                case "charging":
                    return EVSEMinorStatusTypes.Charging;

                case "blocked":
                    return EVSEMinorStatusTypes.Blocked;

                case "outoforder":
                    return EVSEMinorStatusTypes.OutOfOrder;

                default:
                    return EVSEMinorStatusTypes.Unknown;

            }

        }

        #endregion

        #region AsText(this EVSEMinorStatusType)

        public static String AsText(this EVSEMinorStatusTypes EVSEMinorStatusType)
        {

            switch (EVSEMinorStatusType)
            {

                case EVSEMinorStatusTypes.Available:
                    return "available";

                case EVSEMinorStatusTypes.Reserved:
                    return "reserved";

                case EVSEMinorStatusTypes.Charging:
                    return "charging";

                case EVSEMinorStatusTypes.Blocked:
                    return "blocked";

                case EVSEMinorStatusTypes.OutOfOrder:
                    return "outoforder";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsParkingStatusType(Text)

        public static ParkingStatusTypes AsParkingStatusType(this String Text)
        {

            switch (Text)
            {

                case "available":
                    return ParkingStatusTypes.Available;

                case "not-available":
                    return ParkingStatusTypes.NotAvailable;

                default:
                    return ParkingStatusTypes.Unknown;

            }

        }

        #endregion

        #region AsText(this ParkingStatusType)

        public static String AsText(this ParkingStatusTypes ParkingStatusType)
        {

            switch (ParkingStatusType)
            {

                case ParkingStatusTypes.Available:
                    return "available";

                case ParkingStatusTypes.NotAvailable:
                    return "not-available";

                default:
                    return "unknown";

            }

        }

        #endregion




    }

}