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
using System.Xml.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// A generic OCHPdirect result.
    /// </summary>
    public class DirectResult
    {

        #region Properties

        /// <summary>
        /// The machine-readable result code.
        /// </summary>
        public DirectResultCodes  DirectResultCode   { get; }

        /// <summary>
        /// A human-readable error description.
        /// </summary>
        public String             Description        { get; }

        #endregion

        #region Statics

        /// <summary>
        /// Unknowen result code.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static DirectResult Unknown(String Description = null)

            => new DirectResult(DirectResultCodes.Unknown,
                          Description);


        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static DirectResult OK(String Description = null)

            => new DirectResult(DirectResultCodes.Unknown,
                          Description);


        /// <summary>
        /// Only part of the data was accepted.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static DirectResult Partly(String Description = null)

            => new DirectResult(DirectResultCodes.Unknown,
                          Description);


        /// <summary>
        /// Wrong username and/or password.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static DirectResult NotAuthorized(String Description = null)

            => new DirectResult(DirectResultCodes.Unknown,
                          Description);


        /// <summary>
        /// One or more ID (EVSE/Contract) were not valid for this user.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static DirectResult InvalidId(String Description = null)

            => new DirectResult(DirectResultCodes.Unknown,
                          Description);


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static DirectResult Server(String Description = null)

            => new DirectResult(DirectResultCodes.Unknown,
                          Description);


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static DirectResult Format(String Description = null)

            => new DirectResult(DirectResultCodes.Unknown,
                          Description);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new generic OCHPdirect result.
        /// </summary>
        /// <param name="DirectResultCode">The machine-readable result code.</param>
        /// <param name="Description">A human-readable error description.</param>
        public DirectResult(DirectResultCodes  DirectResultCode,
                            String             Description = null)
        {

            this.DirectResultCode   = DirectResultCode;

            this.Description  = Description.IsNotNullOrEmpty()
                                    ? Description.Trim()
                                    : "";

        }

        #endregion


        #region Documentation

        // <ns:result>
        //
        //    <ns:resultCode>
        //       <ns:resultCode>?</ns:resultCode>
        //    </ns:resultCode>
        //
        //    <ns:resultDescription>?</ns:resultDescription>
        //
        // </ns:result>

        #endregion

        #region (static) Parse(DirectResultXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHPdirect result.
        /// </summary>
        /// <param name="DirectResultXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static DirectResult Parse(XElement             DirectResultXML,
                                         OnExceptionDelegate  OnException = null)
        {

            DirectResult _DirectResult;

            if (TryParse(DirectResultXML, out _DirectResult, OnException))
                return _DirectResult;

            return null;

        }

        #endregion

        #region (static) Parse(DirectResultText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHPdirect result.
        /// </summary>
        /// <param name="DirectResultText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static DirectResult Parse(String               DirectResultText,
                                         OnExceptionDelegate  OnException = null)
        {

            DirectResult _DirectResult;

            if (TryParse(DirectResultText, out _DirectResult, OnException))
                return _DirectResult;

            return null;

        }

        #endregion

        #region (static) TryParse(DirectResultXML,  out DirectResult, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHPdirect result.
        /// </summary>
        /// <param name="DirectResultXML">The XML to parse.</param>
        /// <param name="DirectResult">The parsed OCHPdirect result.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             DirectResultXML,
                                       out DirectResult     DirectResult,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                DirectResult = new DirectResult(

                             DirectResultXML.MapValueOrFail    (OCHPNS.Default + "resultCode",
                                                                OCHPNS.Default + "resultCode",
                                                                XML_IO.AsDirectResultCode),

                             DirectResultXML.ElementValueOrFail(OCHPNS.Default + "resultDescription")

                         );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.Now, DirectResultXML, e);

                DirectResult = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(DirectResultText, out DirectResult, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHPdirect result.
        /// </summary>
        /// <param name="DirectResultText">The text to parse.</param>
        /// <param name="DirectResult">The parsed OCHPdirect result.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               DirectResultText,
                                       out DirectResult     DirectResult,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(DirectResultText).Root,
                             out DirectResult,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.Now, DirectResultText, e);
            }

            DirectResult = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "result",

                   new XElement(OCHPNS.Default + "resultCode",
                       new XElement(OCHPNS.Default + "resultCode", DirectResultCode.ToString())
                   ),

                   new XElement(OCHPNS.Default + "resultDescription", Description)

               );

        #endregion


        #region Operator overloading

        #region Operator == (DirectResult1, DirectResult2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="DirectResult1">A result.</param>
        /// <param name="DirectResult2">Another result.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (DirectResult DirectResult1, DirectResult DirectResult2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(DirectResult1, DirectResult2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) DirectResult1 == null) || ((Object) DirectResult2 == null))
                return false;

            return DirectResult1.Equals(DirectResult2);

        }

        #endregion

        #region Operator != (DirectResult1, DirectResult2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="DirectResult1">A result.</param>
        /// <param name="DirectResult2">Another result.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (DirectResult DirectResult1, DirectResult DirectResult2)

            => !(DirectResult1 == DirectResult2);

        #endregion

        #endregion

        #region IEquatable<DirectResult> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)
        {

            if (Object == null)
                return false;

            // Check if the given object is a result.
            var DirectResult = Object as DirectResult;
            if ((Object) DirectResult == null)
                return false;

            return this.Equals(DirectResult);

        }

        #endregion

        #region Equals(DirectResult)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="DirectResult">An result to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(DirectResult DirectResult)
        {

            if ((Object) DirectResult == null)
                return false;

            return this.DirectResultCode. Equals(DirectResult.DirectResultCode) &&
                   this.Description.Equals(DirectResult.Description);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return DirectResultCode. GetHashCode() * 11 ^

                       (Description != null
                            ? Description.GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => DirectResultCode + (Description.IsNotNullOrEmpty() ? " - " + Description : "");

        #endregion

    }

}
