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
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// This class defines a resource related to the charge point or charging station.
    /// </summary>
    public class RelatedResource
    {

        #region Data

        /// <summary>
        /// The regular expression for verifying an URI.
        /// </summary>
        public static readonly Regex URI_RegEx = new Regex(@"^(http|https):\/\/.+",
                                                           RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// Referencing uri to the resource.
        /// Must begin with a protocol of the list: http, https.
        /// </summary>
        public String            URI      { get; }

        /// <summary>
        /// The class of the related resource.
        /// </summary>
        public RelatedResources  Class    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new resource related to a charge point or charging station.
        /// </summary>
        /// <param name="URI">The machine-readable result code.</param>
        /// <param name="Class">A human-readable error description.</param>
        public RelatedResource(String            URI,
                               RelatedResources  Class)
        {

            #region Initial checks

            if (URI.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(URI),  "The given URI must not be null or empty!");

            if (!URI_RegEx.IsMatch(URI))
                throw new ArgumentException("The given URI is invalid!", nameof(URI));

            #endregion

            this.URI    = URI;
            this.Class  = Class;

        }

        #endregion


        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Class, " from ", URI);

        #endregion

    }

}
