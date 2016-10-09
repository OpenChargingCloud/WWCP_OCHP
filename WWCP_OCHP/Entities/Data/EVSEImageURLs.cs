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
using System.Xml.Linq;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// This class references images related to a EVSE in terms of a file name or URI.
    /// </summary>
    public class EVSEImageURLs
    {

        #region Data

        /// <summary>
        /// The regular expression for verifying an URI.
        /// </summary>
        public static readonly Regex URI_RegEx = new Regex(@"^(http|https|ftp|file):\/\/.+",
                                                           RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// URI from where the image data can be fetched.
        /// Must begin with a protocol of the list: http, https, file, ftp.
        /// </summary>
        public String        URI        { get; }

        /// <summary>
        /// The image class.
        /// </summary>
        public EVSEImageClasses  Class      { get; }

        /// <summary>
        /// The image type like: gif, jpeg, png, svg.
        /// </summary>
        public String        Type      { get; }

        /// <summary>
        /// Width of the full scale image.
        /// </summary>
        public UInt16        Width      { get; }

        /// <summary>
        /// Height of the full scale image.
        /// </summary>
        public UInt16        Height      { get; }

        /// <summary>
        /// URI from where a thumbnail of the image can be fetched.
        /// Must begin with a protocol of the list: http, https, file, ftp.
        /// </summary>
        public String        ThumbURI   { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// This class references images related to a EVSE in terms of a file name or URI.
        /// </summary>
        /// <param name="URI">URI from where the image data can be fetched. Must begin with a protocol of the list: http, https, file, ftp.</param>
        /// <param name="Class">The image class.</param>
        /// <param name="Type">The image type like: gif, jpeg, png, svg.</param>
        /// <param name="Width">Width of the full scale image.</param>
        /// <param name="Height">Height of the full scale image.</param>
        /// <param name="ThumbURI">URI from where a thumbnail of the image can be fetched. Must begin with a protocol of the list: http, https, file, ftp.</param>
        public EVSEImageURLs(String        URI,
                             EVSEImageClasses  Class,
                             String        Type,
                             UInt16        Width     = 0,
                             UInt16        Height    = 0,
                             String        ThumbURI  = null)
        {

            this.URI       = URI;
            this.ThumbURI  = ThumbURI;
            this.Class     = Class;
            this.Type      = Type;
            this.Width     = Width;
            this.Height    = Height;

        }

        #endregion


    }

}
