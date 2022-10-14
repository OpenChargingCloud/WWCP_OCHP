/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

    public static class ConnectorStandardsExtensions
    {

        public static ChargePointTypes GetChargePointType(this ConnectorStandards ConnectorStandard)
        {

            switch (ConnectorStandard)
            {

                case ConnectorStandards.Chademo:
                case ConnectorStandards.IEC_62196_T1_COMBO:
                case ConnectorStandards.IEC_62196_T2_COMBO:
                    return ChargePointTypes.DC;

                default:
                    return ChargePointTypes.AC;

            }

        }

    }


    /// <summary>
    /// OCHP connector standards.
    /// </summary>
    public enum ConnectorStandards
    {

        /// <summary>
        /// The connector standard is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The connector type is CHAdeMO, DC.
        /// </summary>
        Chademo,

        /// <summary>
        /// The connector type is IEC 62196 Type 1 “SAE J1772”.
        /// </summary>
        IEC_62196_T1,

        /// <summary>
        /// The connector type is Combo Type 1 based, DC.
        /// </summary>
        IEC_62196_T1_COMBO,

        /// <summary>
        /// The connector type is IEC 62196 Type 2 “Mennekes”
        /// </summary>
        IEC_62196_T2,

        /// <summary>
        /// The connector type is Combo Type 2 based, DC
        /// </summary>
        IEC_62196_T2_COMBO,

        /// <summary>
        /// The connector type is IEC 62196 Type 3A.
        /// </summary>
        IEC_62196_T3A,

        /// <summary>
        /// The connector type is IEC 62196 Type 3C “Scame”
        /// </summary>
        IEC_62196_T3C,

        /// <summary>
        /// The connector type is Standard/Domestic household, type "A", NEMA 1-15, 2 pins.
        /// </summary>
        DOMESTIC_A,

        /// <summary>
        /// The connector type is Standard/Domestic household, type "B", NEMA 5-15, 3 pins.
        /// </summary>
        DOMESTIC_B,

        /// <summary>
        /// The connector type is Standard/Domestic household, type "C", CEE 7/17, 2 pins.
        /// </summary>
        DOMESTIC_C,

        /// <summary>
        /// The connector type is Standard/Domestic household, type "D", 3 pin.
        /// </summary>
        DOMESTIC_D,

        /// <summary>
        /// The connector type is Standard/Domestic household, type "E", CEE 7/5 3 pins.
        /// </summary>
        DOMESTIC_E,

        /// <summary>
        /// The connector type is Standard/Domestic household, type "F", CEE 7/4, Schuko, 3 pins.
        /// </summary>
        DOMESTIC_F,

        /// <summary>
        /// The connector type is Standard/Domestic household, type "G", BS 1363, Commonwealth, 3 pins.
        /// </summary>
        DOMESTIC_G,

        /// <summary>
        /// The connector type is Standard/Domestic household, type "H", SI-32, 3 pins.
        /// </summary>
        DOMESTIC_H,

        /// <summary>
        /// The connector type is Standard/Domestic household, type "I", AS 3112, 3 pins.
        /// </summary>
        DOMESTIC_I,

        /// <summary>
        /// The connector type is Standard/Domestic household, type "J", SEV 1011, 3 pins.
        /// </summary>
        DOMESTIC_J,

        /// <summary>
        /// The connector type is Standard/Domestic household, type "K", DS 60884-2-D1, 3 pins.
        /// </summary>
        DOMESTIC_K,

        /// <summary>
        /// The connector type is Standard/Domestic household, type "L", CEI 23-16-VII, 3 pins.
        /// </summary>
        DOMESTIC_L,

        /// <summary>
        /// The connector type is Tesla Connector “Roadster”-type (round, 4 pin).
        /// </summary>
        TESLA_R,

        /// <summary>
        /// The connector type is Tesla Connector “Model-S”-type (oval, 5 pin).
        /// </summary>
        TESLA_S,

        /// <summary>
        /// The connector type is IEC 60309-2 Industrial Connector single phase 16 Amperes (usually blue).
        /// </summary>
        IEC_60309_2_single_16,

        /// <summary>
        /// The connector type is IEC 60309-2 Industrial Connector three phase 16 Amperes (usually red).
        /// </summary>
        IEC_60309_2_three_16,

        /// <summary>
        /// The connector type is IEC 60309-2 Industrial Connector three phase 32 Amperes (usually red).
        /// </summary>
        IEC_60309_2_three_32,

        /// <summary>
        /// The connector type is IEC 60309-2 Industrial Connector three phase 64 Amperes (usually red).
        /// </summary>
        IEC_60309_2_three_64

    }

}
