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

using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// OCHP XML Namespaces
    /// </summary>
    public static class OCHPNS
    {

        /// <summary>
        /// The default namespace of the Open Clearing House Protocol (OCHP) Version 1.4.
        /// </summary>
        public static readonly XNamespace Default  = "http://ochp.eu/1.4";

        /// <summary>
        /// The namespace of the Open Clearing House Protocol (OCHP) Direct Extention Version 0.2.
        /// </summary>
        public static readonly XNamespace Direct   = "http://ochp.eu/direct/0.2/";

    }

}
