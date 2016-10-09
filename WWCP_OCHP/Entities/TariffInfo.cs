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
    /// An OCHP tariff info.
    /// </summary>
    public class TariffInfo
    {

        #region Properties

        /// <summary>
        /// Identifies a tariff. Unique within one EVSE Operator. Must begin with the operator identification, without separators.
        /// </summary>
        public Tariff_Id                      TariffId           { get; }

        /// <summary>
        /// Element describing an individual tariff for a specific recipient. One default tariff without recipients must be provided.
        /// </summary>
        public IEnumerable<IndividualTariff>  IndividualTariff   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHP tariff info.
        /// </summary>
        /// <param name="TariffId">Identifies a tariff. Unique within one EVSE Operator. Must begin with the operator identification, without separators.</param>
        /// <param name="IndividualTariff">Element describing an individual tariff for a specific recipient. One default tariff without recipients must be provided.</param>
        public TariffInfo(Tariff_Id                      TariffId,
                          IEnumerable<IndividualTariff>  IndividualTariff)
        {

            #region Initial checks

            if (TariffId == null)
                throw new ArgumentNullException(nameof(TariffId),  "The given tariff identification must not be null!");

            if (IndividualTariff == null || !IndividualTariff.Any())
                throw new ArgumentNullException(nameof(IndividualTariff), "The given enumeration of individual tariffs must not be null or empty!");

            #endregion

            this.TariffId          = TariffId;
            this.IndividualTariff  = IndividualTariff;

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(TariffId, " for ", IndividualTariff.AggregateWith(", "));

        #endregion

    }

}
