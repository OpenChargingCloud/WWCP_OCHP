/*
 * Copyright (c) 2014-2019 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// OCHP geo coordinate types.
    /// </summary>
    public enum GeoCoordinateTypes
    {

        /// <summary>
        /// Unknown geo coordinate type.
        /// </summary>
        Unknown,

        /// <summary>
        /// For larger sites entrances may be specified for navigation.
        /// </summary>
        Entrance,

        /// <summary>
        /// For larger sites exits may be specified for navigation purpose.
        /// </summary>
        Exit,

        /// <summary>
        /// Two directional entrance and exit.
        /// </summary>
        Access,

        /// <summary>
        /// Geographical location of the user interface for authorisation
        /// and payment means. If not specified the user interface is assumed to be at the location of the charge point.
        /// </summary>
        UI,

        /// <summary>
        /// Other relevant point. Name recommended.
        /// </summary>
        Other

    }

}
