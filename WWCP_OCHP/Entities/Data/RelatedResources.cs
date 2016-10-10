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
    /// OCHP related resources.
    /// </summary>
    public enum RelatedResources
    {

        /// <summary>
        /// Unknown resource.
        /// </summary>
        Unknown,

        /// <summary>
        /// Direct link to this charge point on a map of the operator.
        /// </summary>
        OperatorMap,

        /// <summary>
        /// Link to a payment page of the operator for contractless direct payment.
        /// </summary>
        OperatorPayment,

        /// <summary>
        /// Further information on the charging station.
        /// </summary>
        StationInfo,

        /// <summary>
        /// Further information on the surroundings of the charging station e.g. further POIs.
        /// </summary>
        SurroundingInfo,

        /// <summary>
        /// Website of the station owner (not operator) in case of hotels, restaurants, etc.
        /// </summary>
        OwnerHomepage,

        /// <summary>
        /// Form for user feedback on the charging station service.
        /// </summary>
        FeedbackForm

    }

}
