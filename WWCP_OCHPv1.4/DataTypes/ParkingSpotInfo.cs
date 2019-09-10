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

#region Usings

using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// The static data of an OCHP parking spot.
    /// </summary>
    public class ParkingSpotInfo
    {

        #region Data

        /// <summary>
        /// A regular expression to verify floor levels.
        /// </summary>
        public static readonly Regex FloorLevel_RegExpr         = new Regex(@"[A-Z0-9\-\+/]{1,4}", RegexOptions.IgnorePatternWhitespace);

        /// <summary>
        /// A regular expression to verify parking spot numbers.
        /// </summary>
        public static readonly Regex ParkingSpotNumber_RegExpr  = new Regex(@"[A-Z0-9\-\+/]{1,5}", RegexOptions.IgnorePatternWhitespace);

        #endregion

        #region Properties

        /// <summary>
        /// The unique identification of the parking space.
        /// </summary>
        public Parking_Id         ParkingId              { get; }

        /// <summary>
        /// Restrictions to the usage of this parking spot.
        /// </summary>
        public RestrictionTypes?  ParkingRestrictions    { get; }

        /// <summary>
        /// Level on which the charge station is located (in garage buildings)
        /// in the locally displayed numbering scheme.
        /// </summary>
        /// <example>"-2","P-5", "2", "+5"</example>
        public String             FloorLevel             { get; }

        /// <summary>
        /// Locally displayed parking slot number.
        /// </summary>
        /// <example>"10", "251","B25", "P-234"</example>
        public String             ParkingSpotNumber      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create new static data for an OCHP parking spot.
        /// </summary>
        /// <param name="ParkingId">The unique identification of the parking space.</param>
        /// <param name="ParkingRestrictions">Restrictions to the usage of this parking spot.</param>
        /// <param name="FloorLevel">Level on which the charge station is located (in garage buildings) in the locally displayed numbering scheme. Examples: "-2","P-5", "2", "+5"</param>
        /// <param name="ParkingSpotNumber">Locally displayed parking slot number. Examples: "10", "251","B25", "P-234"</param>
        public ParkingSpotInfo(Parking_Id         ParkingId,
                               RestrictionTypes?  ParkingRestrictions  = null,
                               String             FloorLevel           = null,
                               String             ParkingSpotNumber    = null)

        {

            #region Initial checks

            if (ParkingRestrictions.HasValue && ParkingRestrictions == RestrictionTypes.Unknown)
                throw new ArgumentNullException(nameof(ParkingRestrictions),  "The given parking restrictions must not be null!");

            if (FloorLevel        != null && !FloorLevel_RegExpr.       IsMatch(FloorLevel))
                throw new ArgumentException("The given floor level is invalid!",          nameof(FloorLevel));

            if (ParkingSpotNumber != null && !ParkingSpotNumber_RegExpr.IsMatch(ParkingSpotNumber))
                throw new ArgumentException("The given parking spot number is invalid!",  nameof(ParkingSpotNumber));

            #endregion

            this.ParkingId            = ParkingId;
            this.ParkingRestrictions  = ParkingRestrictions;
            this.FloorLevel           = FloorLevel;
            this.ParkingSpotNumber    = ParkingSpotNumber;

        }

        #endregion


        #region AllParkingRestrictions

        /// <summary>
        /// Return an enumeration of all parking restrictions.
        /// </summary>
        public IEnumerable<String> AllParkingRestrictions

            => ParkingRestrictions.HasValue

                   ? Enum.GetValues(typeof(RestrictionTypes)).
                          Cast<RestrictionTypes>().
                          Where (restriction => ParkingRestrictions.Value.HasFlag(restriction)).
                          Select(restriction => XML_IO.AsText(restriction))

                   : new String[0];

        #endregion


        #region Documentation

        // <ns:parkingSpot>
        //
        //    <ns:parkingId>?</ns:parkingId>
        //
        //    <!--Zero or more repetitions:-->
        //    <ns:restriction>
        //       <ns:RestrictionType>?</ns:RestrictionType>
        //    </ns:restriction>
        //
        //    <!--Optional:-->
        //    <ns:floorLevel>?</ns:floorLevel>
        //
        //    <!--Optional:-->
        //    <ns:parkingSpotNumber>?</ns:parkingSpotNumber>
        //
        // </ns:parkingSpot>

        #endregion

        #region (static) Parse(ParkingSpotInfoXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHP parking spot.
        /// </summary>
        /// <param name="ParkingSpotInfoXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ParkingSpotInfo Parse(XElement             ParkingSpotInfoXML,
                                            OnExceptionDelegate  OnException = null)
        {

            ParkingSpotInfo _ParkingSpotInfo;

            if (TryParse(ParkingSpotInfoXML, out _ParkingSpotInfo, OnException))
                return _ParkingSpotInfo;

            return null;

        }

        #endregion

        #region (static) Parse(ParkingSpotInfoText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHP parking spot.
        /// </summary>
        /// <param name="ParkingSpotInfoText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ParkingSpotInfo Parse(String               ParkingSpotInfoText,
                                            OnExceptionDelegate  OnException = null)
        {

            ParkingSpotInfo _ParkingSpotInfo;

            if (TryParse(ParkingSpotInfoText, out _ParkingSpotInfo, OnException))
                return _ParkingSpotInfo;

            return null;

        }

        #endregion

        #region (static) TryParse(ParkingSpotInfoXML,  out ParkingSpotInfo, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHP parking spot.
        /// </summary>
        /// <param name="ParkingSpotInfoXML">The XML to parse.</param>
        /// <param name="ParkingSpotInfo">The parsed parking spot.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement             ParkingSpotInfoXML,
                                       out ParkingSpotInfo  ParkingSpotInfo,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                ParkingSpotInfo = new ParkingSpotInfo(

                                      ParkingSpotInfoXML.MapValueOrFail       (OCHPNS.Default + "parkingId",
                                                                               Parking_Id.Parse,
                                                                               "Missing or invalid XML element 'parkingId'!"),

                                      ParkingSpotInfoXML.MapEnumValues        (OCHPNS.Default + "restriction",
                                                                               OCHPNS.Default + "RestrictionType",
                                                                               XML_IO.AsRestrictionType),

                                      ParkingSpotInfoXML.ElementValueOrDefault(OCHPNS.Default + "floorLevel"),

                                      ParkingSpotInfoXML.ElementValueOrDefault(OCHPNS.Default + "parkingSpotNumber")

                                  );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, ParkingSpotInfoXML, e);

                ParkingSpotInfo = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ParkingSpotInfoText, out ParkingSpotInfo, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHP parking spot.
        /// </summary>
        /// <param name="ParkingSpotInfoText">The text to parse.</param>
        /// <param name="ParkingSpotInfo">The parsed parking spot.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String               ParkingSpotInfoText,
                                       out ParkingSpotInfo  ParkingSpotInfo,
                                       OnExceptionDelegate  OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ParkingSpotInfoText).Root,
                             out ParkingSpotInfo,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, ParkingSpotInfoText, e);
            }

            ParkingSpotInfo = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new XElement(OCHPNS.Default + "parkingSpot",

                   new XElement(OCHPNS.Default + "parkingId", ParkingId.ToString()),

                   ParkingRestrictions != RestrictionTypes.Unknown
                       ? AllParkingRestrictions.Select(restriction => new XElement(OCHPNS.Default + "restriction",
                                                                          new XElement(OCHPNS.Default + "RestrictionType", restriction)
                                                                      ))
                       : null,

                   FloorLevel.IsNotNullOrEmpty()
                       ? new XElement(OCHPNS.Default + "floorLevel",         FloorLevel)
                       : null,

                   ParkingSpotNumber.IsNotNullOrEmpty()
                       ? new XElement(OCHPNS.Default + "parkingSpotNumber",  ParkingSpotNumber)
                       : null

               );

        #endregion


        #region Operator overloading

        #region Operator == (ParkingSpotInfo1, ParkingSpotInfo2)

        /// <summary>
        /// Compares two parking spots for equality.
        /// </summary>
        /// <param name="ParkingSpotInfo1">A parking spot.</param>
        /// <param name="ParkingSpotInfo2">Another parking spot.</param>
        /// <returns>True if both match; False otherwise.</returns
        public static Boolean operator == (ParkingSpotInfo ParkingSpotInfo1, ParkingSpotInfo ParkingSpotInfo2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ParkingSpotInfo1, ParkingSpotInfo2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ParkingSpotInfo1 == null) || ((Object) ParkingSpotInfo2 == null))
                return false;

            return ParkingSpotInfo1.Equals(ParkingSpotInfo2);

        }

        #endregion

        #region Operator != (ParkingSpotInfo1, ParkingSpotInfo2)

        /// <summary>
        /// Compares two parking spots for inequality.
        /// </summary>
        /// <param name="ParkingSpotInfo1">A parking spot.</param>
        /// <param name="ParkingSpotInfo2">Another parking spot.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ParkingSpotInfo ParkingSpotInfo1, ParkingSpotInfo ParkingSpotInfo2)

            => !(ParkingSpotInfo1 == ParkingSpotInfo2);

        #endregion

        #endregion

        #region IEquatable<ParkingSpotInfo> Members

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

            // Check if the given object is a parking spot.
            var ParkingSpotInfo = Object as ParkingSpotInfo;
            if ((Object) ParkingSpotInfo == null)
                return false;

            return this.Equals(ParkingSpotInfo);

        }

        #endregion

        #region Equals(ParkingSpotInfo)

        /// <summary>
        /// Compares two parking spots for equality.
        /// </summary>
        /// <param name="ParkingSpotInfo">An parking spot to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(ParkingSpotInfo ParkingSpotInfo)
        {

            if ((Object) ParkingSpotInfo == null)
                return false;

            return ParkingId.Equals(ParkingSpotInfo.ParkingId);

        }

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => ParkingId.GetHashCode();

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(ParkingId,
                             AllParkingRestrictions.Any()
                                 ? ", " + AllParkingRestrictions.AggregateWith(", ")
                                 : "",
                             FloorLevel.IsNotNullOrEmpty()
                                 ? ", " + FloorLevel
                                 : "",
                             ParkingSpotNumber.IsNotNullOrEmpty()
                                 ? ", " + ParkingSpotNumber
                                 : "");

        #endregion

    }

}
