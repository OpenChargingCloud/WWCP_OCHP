/*
 * Copyright (c) 2014-2026 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

namespace cloud.charging.open.protocols.OCHPv1_4
{

    /// <summary>
    /// An OCHPdirect endpoint info collection.
    /// </summary>
    public class EndpointInfos
    {

        #region Data

        private readonly List<ProviderEndpoint>  _ProviderEndpointInfos;
        private readonly List<OperatorEndpoint>  _OperatorEndpointInfos;

        #endregion

        #region Properties

        /// <summary>
        /// The machine-readable result code.
        /// </summary>
        public Result Result  { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCHPdirect endpoint info collection.
        /// </summary>
        public EndpointInfos()
        {

            this._ProviderEndpointInfos = new List<ProviderEndpoint>();
            this._OperatorEndpointInfos = new List<OperatorEndpoint>();

        }

        #endregion


        public EndpointInfos Add(params ProviderEndpoint[]  ProviderEndpoints)
        {

            _ProviderEndpointInfos.AddRange(ProviderEndpoints);

            return this;

        }

        public EndpointInfos Add(params OperatorEndpoint[] OperatorEndpoints)
        {

            _OperatorEndpointInfos.AddRange(OperatorEndpoints);

            return this;

        }

        public EndpointInfos Add(Direct_Id DirectId, IEnumerable<OperatorEndpoint> EndpointInfo)
        {
            return this;
        }



        public IEnumerable<ProviderEndpoint> Get(params Contract_Id[] ContractIds)
        {

            var aa = _ProviderEndpointInfos.Where(endpoint =>
                         endpoint.WhiteList.Any(pattern => ContractIds.First().ToString().Contains(pattern)));


            return aa;

        }

        public IEnumerable<OperatorEndpoint> Get(params EVSE_Id[] EVSEIds)
        {

            var aa = _OperatorEndpointInfos.Where(endpoint =>
                         endpoint.WhiteList.Any(pattern => EVSEIds.First().ToString().Contains(pattern)));


            return aa;

        }

        public IEnumerable<OperatorEndpoint> Get(Direct_Id DirectId)
        {

            return null;

        }

        public Boolean Delete(Direct_Id DirectId)
        {

            return true;

        }


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(_ProviderEndpointInfos.Count + " provider endpoints",
                             _OperatorEndpointInfos.Count + " operator endpoints");

        #endregion

    }

}
