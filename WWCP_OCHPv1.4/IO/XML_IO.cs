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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Aegir;
using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// OCHP XML I/O for WWCP data structures.
    /// </summary>
    public static class XML_IO
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


        #region AsRestrictionType(Text)

        public static RestrictionTypes AsRestrictionType(this String Text)
        {

            switch (Text)
            {

                case "evonly":
                    return RestrictionTypes.EVOnly;

                case "plugged":
                    return RestrictionTypes.Plugged;

                case "disabled":
                    return RestrictionTypes.Disabled;

                case "customers":
                    return RestrictionTypes.Customers;

                case "motorcycles":
                    return RestrictionTypes.Motorcycles;

                case "carsharing":
                    return RestrictionTypes.CarSharing;

                default:
                    return RestrictionTypes.Unknown;

            }

        }

        #endregion

        #region AsText(this AsRestrictionType)

        public static String AsText(this RestrictionTypes AsRestrictionType)
        {

            switch (AsRestrictionType)
            {

                case RestrictionTypes.EVOnly:
                    return "evonly";

                case RestrictionTypes.Plugged:
                    return "plugged";

                case RestrictionTypes.Disabled:
                    return "disabled";

                case RestrictionTypes.Customers:
                    return "customers";

                case RestrictionTypes.Motorcycles:
                    return "motorcycles";

                case RestrictionTypes.CarSharing:
                    return "carsharing";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsEVSEImageClasses(Text)

        public static EVSEImageClasses AsEVSEImageClasses(this String Text)
        {

            switch (Text)
            {

                case "networkLogo":
                    return EVSEImageClasses.NetworkLogo;

                case "operatorLogo":
                    return EVSEImageClasses.OperatorLogo;

                case "ownerLogo":
                    return EVSEImageClasses.OwnerLogo;

                case "stationPhoto":
                    return EVSEImageClasses.StationPhoto;

                case "locationPhoto":
                    return EVSEImageClasses.LocationPhoto;

                case "entrancePhoto":
                    return EVSEImageClasses.EntrancePhoto;

                case "otherPhoto":
                    return EVSEImageClasses.OtherPhoto;

                case "otherLogo":
                    return EVSEImageClasses.OtherLogo;

                case "otherGraphic":
                    return EVSEImageClasses.OtherGraphic;

                default:
                    return EVSEImageClasses.Unknown;

            }

        }

        #endregion

        #region AsText(this EVSEImageClass)

        public static String AsText(this EVSEImageClasses EVSEImageClass)
        {

            switch (EVSEImageClass)
            {

                case EVSEImageClasses.NetworkLogo:
                    return "networkLogo";

                case EVSEImageClasses.OperatorLogo:
                    return "operatorLogo";

                case EVSEImageClasses.OwnerLogo:
                    return "ownerLogo";

                case EVSEImageClasses.StationPhoto:
                    return "stationPhoto";

                case EVSEImageClasses.LocationPhoto:
                    return "locationPhoto";

                case EVSEImageClasses.EntrancePhoto:
                    return "entrancePhoto";

                case EVSEImageClasses.OtherPhoto:
                    return "otherPhoto";

                case EVSEImageClasses.OtherLogo:
                    return "otherLogo";

                case EVSEImageClasses.OtherGraphic:
                    return "otherGraphic";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsRelatedResource(Text)

        public static RelatedResources AsRelatedResource(this String Text)
        {

            switch (Text)
            {

                case "operatorMap":
                    return RelatedResources.OperatorMap;

                case "operatorPayment":
                    return RelatedResources.OperatorPayment;

                case "stationInfo":
                    return RelatedResources.StationInfo;

                case "surroundingInfo":
                    return RelatedResources.SurroundingInfo;

                case "ownerHomepage":
                    return RelatedResources.OwnerHomepage;

                case "feedbackForm":
                    return RelatedResources.FeedbackForm;

                default:
                    return RelatedResources.Unknown;

            }

        }

        #endregion

        #region AsText(this RelatedResource)

        public static String AsText(this RelatedResources RelatedResource)
        {

            switch (RelatedResource)
            {

                case RelatedResources.OperatorMap:
                    return "operatorMap";

                case RelatedResources.OperatorPayment:
                    return "operatorPayment";

                case RelatedResources.StationInfo:
                    return "stationInfo";

                case RelatedResources.SurroundingInfo:
                    return "surroundingInfo";

                case RelatedResources.OwnerHomepage:
                    return "ownerHomepage";

                case RelatedResources.FeedbackForm:
                    return "feedbackForm";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsGeoCoordinateType(Text)

        public static GeoCoordinateTypes AsGeoCoordinateType(this String Text)
        {

            switch (Text)
            {

                case "entrance":
                    return GeoCoordinateTypes.Entrance;

                case "exit":
                    return GeoCoordinateTypes.Exit;

                case "access":
                    return GeoCoordinateTypes.Access;

                case "ui":
                    return GeoCoordinateTypes.UI;

                case "other":
                    return GeoCoordinateTypes.Other;

                default:
                    return GeoCoordinateTypes.Unknown;

            }

        }

        #endregion

        #region AsText(this GeoCoordinateType)

        public static String AsText(this GeoCoordinateTypes GeoCoordinateType)
        {

            switch (GeoCoordinateType)
            {

                case GeoCoordinateTypes.Entrance:
                    return "entrance";

                case GeoCoordinateTypes.Exit:
                    return "exit";

                case GeoCoordinateTypes.Access:
                    return "access";

                case GeoCoordinateTypes.UI:
                    return "ui";

                case GeoCoordinateTypes.Other:
                    return "other";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsDayOfWeek(Number)

        public static DayOfWeek AsDayOfWeek(this Int32 Number)
        {

            switch (Number)
            {

                case 1:
                    return DayOfWeek.Monday;

                case 2:
                    return DayOfWeek.Tuesday;

                case 3:
                    return DayOfWeek.Wednesday;

                case 4:
                    return DayOfWeek.Thursday;

                case 5:
                    return DayOfWeek.Friday;

                case 6:
                    return DayOfWeek.Saturday;

                default:
                    return DayOfWeek.Sunday;

            }

        }

        public static DayOfWeek AsDayOfWeek(this String Number)
        {

            switch (Number)
            {

                case "1":
                    return DayOfWeek.Monday;

                case "2":
                    return DayOfWeek.Tuesday;

                case "3":
                    return DayOfWeek.Wednesday;

                case "4":
                    return DayOfWeek.Thursday;

                case "5":
                    return DayOfWeek.Friday;

                case "6":
                    return DayOfWeek.Saturday;

                default:
                    return DayOfWeek.Sunday;

            }

        }

        #endregion

        #region AsInt(this Weekday)

        public static Int32 AsInt(this DayOfWeek Weekday)
        {

            switch (Weekday)
            {

                case DayOfWeek.Monday:
                    return 1;

                case DayOfWeek.Tuesday:
                    return 2;

                case DayOfWeek.Wednesday:
                    return 3;

                case DayOfWeek.Thursday:
                    return 4;

                case DayOfWeek.Friday:
                    return 5;

                case DayOfWeek.Saturday:
                    return 6;

                default:
                    return 7;

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

                case "IEC_62196_T1":
                    return ConnectorStandards.IEC_62196_T1;

                case "IEC_62196_T1_COMBO":
                    return ConnectorStandards.IEC_62196_T1_COMBO;

                case "IEC_62196_T2":
                    return ConnectorStandards.IEC_62196_T2;

                case "IEC_62196_T2_COMBO":
                    return ConnectorStandards.IEC_62196_T2_COMBO;

                case "IEC_62196_T3A":
                    return ConnectorStandards.IEC_62196_T3A;

                case "IEC_62196_T3C":
                    return ConnectorStandards.IEC_62196_T3C;

                case "DOMESTIC_A":
                    return ConnectorStandards.DOMESTIC_A;

                case "DOMESTIC_B":
                    return ConnectorStandards.DOMESTIC_B;

                case "DOMESTIC_C":
                    return ConnectorStandards.DOMESTIC_C;

                case "DOMESTIC_D":
                    return ConnectorStandards.DOMESTIC_D;

                case "DOMESTIC_E":
                    return ConnectorStandards.DOMESTIC_E;

                case "DOMESTIC_F":
                    return ConnectorStandards.DOMESTIC_F;

                case "DOMESTIC_G":
                    return ConnectorStandards.DOMESTIC_G;

                case "DOMESTIC_H":
                    return ConnectorStandards.DOMESTIC_H;

                case "DOMESTIC_I":
                    return ConnectorStandards.DOMESTIC_I;

                case "DOMESTIC_J":
                    return ConnectorStandards.DOMESTIC_J;

                case "DOMESTIC_K":
                    return ConnectorStandards.DOMESTIC_K;

                case "DOMESTIC_L":
                    return ConnectorStandards.DOMESTIC_L;

                case "TESLA_R":
                    return ConnectorStandards.TESLA_R;

                case "TESLA_S":
                    return ConnectorStandards.TESLA_S;

                case "IEC_60309_2_single_16":
                    return ConnectorStandards.IEC_60309_2_single_16;

                case "IEC_60309_2_three_16":
                    return ConnectorStandards.IEC_60309_2_three_16;

                case "IEC_60309_2_three_32":
                    return ConnectorStandards.IEC_60309_2_three_32;

                case "IEC_60309_2_three_64":
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
                    return "IEC_62196_T1";

                case ConnectorStandards.IEC_62196_T1_COMBO:
                    return "IEC_62196_T1_COMBO";

                case ConnectorStandards.IEC_62196_T2:
                    return "IEC_62196_T2";

                case ConnectorStandards.IEC_62196_T2_COMBO:
                    return "IEC_62196_T2_COMBO";

                case ConnectorStandards.IEC_62196_T3A:
                    return "IEC_62196_T3A";

                case ConnectorStandards.IEC_62196_T3C:
                    return "IEC_62196_T3C";

                case ConnectorStandards.DOMESTIC_A:
                    return "DOMESTIC_A";

                case ConnectorStandards.DOMESTIC_B:
                    return "DOMESTIC_B";

                case ConnectorStandards.DOMESTIC_C:
                    return "DOMESTIC_C";

                case ConnectorStandards.DOMESTIC_D:
                    return "DOMESTIC_D";

                case ConnectorStandards.DOMESTIC_E:
                    return "DOMESTIC_E";

                case ConnectorStandards.DOMESTIC_F:
                    return "DOMESTIC_F";

                case ConnectorStandards.DOMESTIC_G:
                    return "DOMESTIC_G";

                case ConnectorStandards.DOMESTIC_H:
                    return "DOMESTIC_H";

                case ConnectorStandards.DOMESTIC_I:
                    return "DOMESTIC_I";

                case ConnectorStandards.DOMESTIC_J:
                    return "DOMESTIC_J";

                case ConnectorStandards.DOMESTIC_K:
                    return "DOMESTIC_K";

                case ConnectorStandards.DOMESTIC_L:
                    return "DOMESTIC_L";

                case ConnectorStandards.TESLA_R:
                    return "TESLA_R";

                case ConnectorStandards.TESLA_S:
                    return "TESLA_S";

                case ConnectorStandards.IEC_60309_2_single_16:
                    return "IEC_60309_2_single_16";

                case ConnectorStandards.IEC_60309_2_three_16:
                    return "IEC_60309_2_three_16";

                case ConnectorStandards.IEC_60309_2_three_32:
                    return "IEC_60309_2_three_32";

                case ConnectorStandards.IEC_60309_2_three_64:
                    return "IEC_60309_2_three_64";

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


        #region AsChargePointStatus(Text)

        public static ChargePointStatus AsChargePointStatus(this String Text)
        {

            switch (Text)
            {

                case "Operative":
                    return ChargePointStatus.Operative;

                case "Inoperative":
                    return ChargePointStatus.Inoperative;

                case "Planned":
                    return ChargePointStatus.Planned;

                case "Closed":
                    return ChargePointStatus.Closed;

                default:
                    return ChargePointStatus.Unknown;

            }

        }

        #endregion

        #region AsText(this ChargePointStatus)

        public static String AsText(this ChargePointStatus Status)
        {

            switch (Status)
            {

                case ChargePointStatus.Operative:
                    return "Operative";

                case ChargePointStatus.Inoperative:
                    return "Inoperative";

                case ChargePointStatus.Planned:
                    return "Planned";

                case ChargePointStatus.Closed:
                    return "Closed";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsChargePointType(Text)

        public static ChargePointTypes AsChargePointType(this String Text)
        {

            switch (Text)
            {

                case "AC":
                    return ChargePointTypes.AC;

                case "DC":
                    return ChargePointTypes.DC;

                default:
                    return ChargePointTypes.Unknown;

            }

        }

        #endregion

        #region AsText(this ChargePointType)

        public static String AsText(this ChargePointTypes ChargePointType)
        {

            switch (ChargePointType)
            {

                case ChargePointTypes.AC:
                    return "AC";

                case ChargePointTypes.DC:
                    return "DC";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsGeneralLocationType(Text)

        public static GeneralLocationTypes AsGeneralLocationType(this String Text)
        {

            switch (Text)
            {

                case "on-street":
                    return GeneralLocationTypes.OnStreet;

                case "parking-garage":
                    return GeneralLocationTypes.ParkingGarage;

                case "underground-garage":
                    return GeneralLocationTypes.UndergroundGarage;

                case "parking-lot":
                    return GeneralLocationTypes.ParkingLot;

                case "other":
                    return GeneralLocationTypes.Other;

                case "private":
                    return GeneralLocationTypes.Private;

                default:
                    return GeneralLocationTypes.Unknown;

            }

        }

        #endregion

        #region AsText(this GeneralLocation)

        public static String AsText(this GeneralLocationTypes GeneralLocation)
        {

            switch (GeneralLocation)
            {

                case GeneralLocationTypes.OnStreet:
                    return "on-street";

                case GeneralLocationTypes.ParkingGarage:
                    return "parking-garage";

                case GeneralLocationTypes.UndergroundGarage:
                    return "underground-garage";

                case GeneralLocationTypes.ParkingLot:
                    return "parking-lot";

                case GeneralLocationTypes.Other:
                    return "other";

                case GeneralLocationTypes.Private:
                    return "private";

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


        #region AsTokenRepresentation(Text)

        public static TokenRepresentations AsTokenRepresentation(this String Text)
        {

            switch (Text)
            {

                case "SHA160":
                    return TokenRepresentations.SHA160;

                case "SHA256":
                    return TokenRepresentations.SHA256;

                default:
                    return TokenRepresentations.Plain;

            }

        }

        #endregion

        #region AsText(this TokenRepresentation)

        public static String AsText(this TokenRepresentations TokenRepresentation)
        {

            switch (TokenRepresentation)
            {

                case TokenRepresentations.SHA160:
                    return "SHA160";

                case TokenRepresentations.SHA256:
                    return "SHA256";

                default:
                    return "plain";

            }

        }

        #endregion


        #region AsTokenType(Text)

        public static TokenTypes AsTokenType(this String Text)
        {

            switch (Text)
            {

                case "remote":
                    return TokenTypes.Remote;

                case "15118":
                    return TokenTypes.IEC15118;

                default:
                    return TokenTypes.RFID;

            }

        }

        #endregion

        #region AsText(this TokenType)

        public static String AsText(this TokenTypes TokenType)
        {

            switch (TokenType)
            {

                case TokenTypes.Remote:
                    return "remote";

                case TokenTypes.IEC15118:
                    return "15118";

                default:
                    return "rfid";

            }

        }

        #endregion


        #region AsTokenSubType(Text)

        public static TokenSubTypes AsTokenSubType(this String Text)
        {

            switch (Text)
            {

                case "mifareDes":
                    return TokenSubTypes.MifareDES;

                case "calypso":
                    return TokenSubTypes.Calypso;

                default:
                    return TokenSubTypes.MifareClassic;

            }

        }

        #endregion

        #region AsText(this TokenSubType)

        public static String AsText(this TokenSubTypes TokenSubType)
        {

            switch (TokenSubType)
            {

                case TokenSubTypes.MifareDES:
                    return "mifareDes";

                case TokenSubTypes.Calypso:
                    return "calypso";

                default:
                    return "mifareCls";

            }

        }

        #endregion


        #region AsCDRStatus(Text)

        public static CDRStatus AsCDRStatus(this String Text)
        {

            switch (Text)
            {

                case "new":
                    return CDRStatus.New;

                case "accepted":
                    return CDRStatus.Accepted;

                case "rejected":
                    return CDRStatus.Rejected;

                case "declined":
                    return CDRStatus.Declined;

                case "approved":
                    return CDRStatus.Approved;

                case "revised":
                    return CDRStatus.Revised;

                default:
                    return CDRStatus.Unknown;

            }

        }

        #endregion

        #region AsText(this CDRStatus)

        public static String AsText(this CDRStatus CdrStatus)
        {

            switch (CdrStatus)
            {

                case CDRStatus.New:
                    return "new";

                case CDRStatus.Accepted:
                    return "accepted";

                case CDRStatus.Rejected:
                    return "rejected";

                case CDRStatus.Declined:
                    return "declined";

                case CDRStatus.Approved:
                    return "approved";

                case CDRStatus.Revised:
                    return "revised";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsBillingItem(Text)

        public static BillingItems AsBillingItem(this String Text)
        {

            switch (Text)
            {

                case "parkingtime":
                    return BillingItems.ParkingTime;

                case "usagetime":
                    return BillingItems.UsageTime;

                case "energy":
                    return BillingItems.Energy;

                case "power":
                    return BillingItems.Power;

                case "serviceFee":
                    return BillingItems.ServiceFee;

                case "reservation":
                    return BillingItems.Reservation;

                case "reservationtime":
                    return BillingItems.ReservationTime;

                default:
                    return BillingItems.Unknown;

            }

        }

        #endregion

        #region AsText(this BillingItem)

        public static String AsText(this BillingItems BillingItem)
        {

            switch (BillingItem)
            {

                case BillingItems.ParkingTime:
                    return "parkingtime";

                case BillingItems.UsageTime:
                    return "usagetime";

                case BillingItems.Energy:
                    return "energy";

                case BillingItems.Power:
                    return "power";

                case BillingItems.ServiceFee:
                    return "serviceFee";

                case BillingItems.Reservation:
                    return "reservation";

                case BillingItems.ReservationTime:
                    return "reservationtime";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsStatusType(Text)

        public static StatusTypes AsStatusType(this String Text)
        {

            switch (Text)
            {

                case "evse":
                    return StatusTypes.EVSE;

                case "parking":
                    return StatusTypes.Parking;

                case "combined":
                    return StatusTypes.Combined;

                default:
                    return StatusTypes.Unknown;

            }

        }

        #endregion

        #region AsText(this StatusType)

        public static String AsText(this StatusTypes StatusType)
        {

            switch (StatusType)
            {

                case StatusTypes.EVSE:
                    return "evse";

                case StatusTypes.Parking:
                    return "parking";

                case StatusTypes.Combined:
                    return "combined";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsResultCode(Text)

        public static ResultCodes AsResultCode(this String Text)
        {

            switch (Text)
            {

                case "ok":
                    return ResultCodes.OK;

                case "partly":
                    return ResultCodes.Partly;

                case "not-authorized":
                    return ResultCodes.NotAuthorized;

                case "invalid-id":
                    return ResultCodes.InvalidId;

                case "server":
                    return ResultCodes.Server;

                case "format":
                    return ResultCodes.Format;

                default:
                    return ResultCodes.Unknown;

            }

        }

        #endregion

        #region AsText(this ResultCode)

        public static String AsText(this ResultCodes ResultCode)
        {

            switch (ResultCode)
            {

                case ResultCodes.OK:
                    return "ok";

                case ResultCodes.Partly:
                    return "partly";

                case ResultCodes.NotAuthorized:
                    return "not-authorized";

                case ResultCodes.InvalidId:
                    return "invalid-id";

                case ResultCodes.Server:
                    return "server";

                case ResultCodes.Format:
                    return "format";

                default:
                    return "unknown";

            }

        }

        #endregion


        #region AsDirectResultCode(Text)

        public static DirectResultCodes AsDirectResultCode(this String Text)
        {

            switch (Text)
            {

                case "ok":
                    return DirectResultCodes.OK;

                case "partly":
                    return DirectResultCodes.Partly;

                case "not-found":
                    return DirectResultCodes.NotFound;

                case "not-supported":
                    return DirectResultCodes.NotSupported;

                case "invalid-id":
                    return DirectResultCodes.InvalidId;

                case "server":
                    return DirectResultCodes.Server;

                default:
                    return DirectResultCodes.Unknown;

            }

        }

        #endregion

        #region AsText(this DirectResultCode)

        public static String AsText(this DirectResultCodes  DirectResultCode)
        {

            switch (DirectResultCode)
            {

                case DirectResultCodes.OK:
                    return "ok";

                case DirectResultCodes.Partly:
                    return "partly";

                case DirectResultCodes.NotFound:
                    return "not-found";

                case DirectResultCodes.NotSupported:
                    return "not-supported";

                case DirectResultCodes.InvalidId:
                    return "invalid-id";

                case DirectResultCodes.Server:
                    return "server";

                default:
                    return "unknown";

            }

        }

        #endregion



        // WWCP objects

        #region ParseGeoCoordinate    (XML, OnException = null)

        #region Documentation

        // <OCHPNS:chargePointLocation lat="?" lon="?" />

        #endregion

        public static GeoCoordinate ParseGeoCoordinate(XElement             XML,
                                                       OnExceptionDelegate  OnException = null)
        {

            try
            {

                return new GeoCoordinate(
                           new Latitude (Double.Parse(XML.Attribute(OCHPNS.Default + "lat").Value)),
                           new Longitude(Double.Parse(XML.Attribute(OCHPNS.Default + "lon").Value))
                       );

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, XML, e);

                return null;

            }

        }

        #endregion


        #region ParseRegularHours     (XML, OnException = null)

        #region Documentation

        // <OCHPNS:regularHours weekday="1" periodBegin="08:00" periodEnd="20:00">
        // <OCHPNS:regularHours weekday="2" periodBegin="08:00" periodEnd="20:00">
        // <OCHPNS:regularHours weekday="3" periodBegin="08:00" periodEnd="20:00">
        // <OCHPNS:regularHours weekday="4" periodBegin="08:00" periodEnd="20:00">
        // <OCHPNS:regularHours weekday="5" periodBegin="08:00" periodEnd="20:00">
        // <OCHPNS:regularHours weekday="6" periodBegin="10:00" periodEnd="16:00">

        #endregion

        public static RegularHours ParseRegularHours(XElement             XML,
                                                     OnExceptionDelegate  OnException = null)
        {

            try
            {

                return new RegularHours(

                           XML.MapAttributeValueOrFail(OCHPNS.Default + "weekday",
                                                       XML_IO.AsDayOfWeek,
                                                       "Invalid or missing XML attribute 'weekday'!"),

                           XML.MapAttributeValueOrFail(OCHPNS.Default + "periodBegin",
                                                       HourMin.Parse,
                                                       "Invalid or missing XML attribute 'periodBegin'!"),

                           XML.MapAttributeValueOrFail(OCHPNS.Default + "periodEnd",
                                                       HourMin.Parse,
                                                       "Invalid or missing XML attribute 'periodEnd'!")

                       );

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, XML, e);

                return default(RegularHours);

            }

        }

        #endregion

        #region ToXML(this RegularHours, XName = null)

        public static XElement ToXML(this RegularHours  RegularHours,
                                     XName              XName  = null)

            => new XElement(XName ?? OCHPNS.Default + "regularHours",

                                new XAttribute(OCHPNS.Default + "weekday",      XML_IO.AsInt(RegularHours.DayOfWeek)),
                                new XAttribute(OCHPNS.Default + "periodBegin",  RegularHours.PeriodBegin.ToString()),
                                new XAttribute(OCHPNS.Default + "periodEnd",    RegularHours.PeriodEnd.  ToString())

                            );

        #endregion


        #region ParseExceptionalPeriod(XML, OnException = null)

        public static ExceptionalPeriod ParseExceptionalPeriod(XElement             XML,
                                                               OnExceptionDelegate  OnException = null)
        {

            try
            {

                return new ExceptionalPeriod(

                           DateTime.Parse(XML.ElementOrFail(OCHPNS.Default + "periodBegin",
                                                            "The XML element 'periodBegin' is invalid or missing!").Value),

                           DateTime.Parse(XML.ElementOrFail(OCHPNS.Default + "periodEnd",
                                                            "The XML element 'periodEnd' is invalid or missing!").Value)

                       );

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, XML, e);

                return default(ExceptionalPeriod);

            }

        }

        #endregion

        #region ToXML(this ExceptionalPeriod, XName = null)

        public static XElement ToXML(this ExceptionalPeriod  ExceptionalPeriod,
                                     XName                   XName  = null)

            => new XElement(XName ?? OCHPNS.Default + "exceptionalOpenings",

                                new XElement(OCHPNS.Default + "periodBegin",
                                    new XElement(OCHPNS.Default + "DateTime", ExceptionalPeriod.Begin.ToIso8601())
                                ),

                                new XElement(OCHPNS.Default + "periodEnd",
                                    new XElement(OCHPNS.Default + "DateTime", ExceptionalPeriod.End.  ToIso8601())
                                )

                            );

        #endregion


    }

}