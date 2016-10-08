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

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// OCHP EVSE image classes.
    /// </summary>
    public enum EVSEImageClasses
    {

        /// <summary>
        /// Logo of a associated roaming network to be displayed with the EVSE for
        /// example in lists, maps and detailed information view;
        /// </summary>
        NetworkLogo,

        /// <summary>
        /// Logo of the charge points operator, for example a municipal, to be
        /// displayed with the EVSEs detailed information view or in lists and
        /// maps, if no networkLogo is present;
        /// </summary>
        OperatorLogo,

        /// <summary>
        /// Logo of the charge points owner, for example a local store, to be
        /// displayed with the EVSEs detailed information view;
        /// </summary>
        OwnerLogo,

        /// <summary>
        /// Full view photo of the station in field. Should show the station only;
        /// </summary>
        StationPhoto,

        /// <summary>
        /// Location overview photo. Should indicate the location of the station
        /// on the site or street.
        /// </summary>
        LocationPhoto,

        /// <summary>
        /// Location entrance photo. Should show the car entrance to the location
        /// from street side;
        /// </summary>
        EntrancePhoto,

        /// <summary>
        /// Other related photo to be displayed with the stations detailed
        /// information view;
        /// </summary>
        OtherPhoto,

        /// <summary>
        /// Other related logo to be displayed with the stations detailed
        /// information view;
        /// </summary>
        OtherLogo,

        /// <summary>
        /// Other related graphic to be displayed with the stations detailed
        /// information view;
        /// </summary>
        OtherGraphic

    }

}
