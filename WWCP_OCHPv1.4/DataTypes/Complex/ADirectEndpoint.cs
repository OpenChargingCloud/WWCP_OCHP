/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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

using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using System;
using System.Text.RegularExpressions;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// An abstract OCHPdirect endpoint.
    /// </summary>
    public abstract class ADirectEndpoint
    {

        #region Properties

        /// <summary>
        /// The endpoint address.
        /// </summary>
        public URL      URL            { get; }

        /// <summary>
        /// The WSDL namespace definition.
        /// </summary>
        public String   NamespaceURL   { get; }

        // http://ochp.eu/1.3
        // http://ochp.eu/1.4

        /// <summary>
        /// The secret token to access this endpoint.
        /// </summary>
        public String  AccessToken    { get; }

        /// <summary>
        /// The date on which this endpoint/token combination is valid
        /// (plus overlap into day before and after).
        /// </summary>
        public String  ValidDate      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new abstract OCHPdirect endpoint.
        /// </summary>
        /// <param name="URL">The endpoint address.</param>
        /// <param name="NamespaceURL">The WSDL namespace definition.</param>
        /// <param name="AccessToken">The secret token to access this endpoint.</param>
        /// <param name="ValidDate">The date on which this endpoint/token combination is valid.</param>
        public ADirectEndpoint(URL     URL,
                               String  NamespaceURL,
                               String  AccessToken,
                               String  ValidDate)
        {

            this.URL           = URL;
            this.NamespaceURL  = NamespaceURL;
            this.AccessToken   = AccessToken;
            this.ValidDate     = ValidDate;

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(URL, " / ", NamespaceURL, " from ", ValidDate);

        #endregion

    }

}
