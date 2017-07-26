/*
 * Copyright (c) 2014-2017 GraphDefined GmbH
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

namespace org.GraphDefined.WWCP.OCHPv1_4.EMP
{

    /// <summary>
    /// An OCHPdirect control EVSE request.
    /// </summary>
    public class ControlEVSERequest : ARequest<ControlEVSERequest>
    {

        #region Properties

        /// <summary>
        /// The unique session identification of the direct charging process to be controlled.
        /// </summary>
        public Direct_Id         DirectId       { get; }

        /// <summary>
        /// The operation to be performed for the selected charge point.
        /// </summary>
        public DirectOperations  Operation      { get; }

        /// <summary>
        /// Maximum authorised power in kW.
        /// </summary>
        public Single?           MaxPower       { get; }

        /// <summary>
        /// Maximum authorised current in A.
        /// </summary>
        public Single?           MaxCurrent     { get; }

        /// <summary>
        /// Marks an AC-charging session to be limited to one-phase charging.
        /// </summary>
        public Boolean?          OnePhase       { get; }

        /// <summary>
        /// Maximum authorised energy in kWh.
        /// </summary>
        public Single?           MaxEnergy      { get; }

        /// <summary>
        /// Minimum required energy in kWh.
        /// </summary>
        public Single?           MinEnergy      { get; }

        /// <summary>
        /// Scheduled time of departure.
        /// </summary>
        public DateTime?         Departure      { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create an OCHPdirect ControlEVSE XML/SOAP request.
        /// </summary>
        /// <param name="DirectId">The unique session identification of the direct charging process to be controlled.</param>
        /// <param name="Operation">The operation to be performed for the selected charge point.</param>
        /// <param name="MaxPower">Maximum authorised power in kW.</param>
        /// <param name="MaxCurrent">Maximum authorised current in A.</param>
        /// <param name="OnePhase">Marks an AC-charging session to be limited to one-phase charging.</param>
        /// <param name="MaxEnergy">Maximum authorised energy in kWh.</param>
        /// <param name="MinEnergy">Minimum required energy in kWh.</param>
        /// <param name="Departure">Scheduled time of departure.</param>
        public ControlEVSERequest(Direct_Id         DirectId,
                                  DirectOperations  Operation,
                                  Single?           MaxPower   = null,
                                  Single?           MaxCurrent = null,
                                  Boolean?          OnePhase   = null,
                                  Single?           MaxEnergy  = null,
                                  Single?           MinEnergy  = null,
                                  DateTime?         Departure  = null)

        {

            #region Initial checks

            if (DirectId == null)
                throw new ArgumentNullException(nameof(DirectId),  "The given direct charging session identification must not be null!");

            #endregion

            this.DirectId    = DirectId;
            this.Operation   = Operation;
            this.MaxPower    = MaxPower;
            this.MaxCurrent  = MaxCurrent;
            this.OnePhase    = OnePhase;
            this.MaxEnergy   = MaxEnergy;
            this.MinEnergy   = MinEnergy;
            this.Departure   = Departure;

        }

        #endregion


        #region Documentation

        // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
        //                   xmlns:ns      = "http://ochp.eu/1.4">
        //
        //    <soapenv:Header/>
        //    <soapenv:Body>
        //       <ns:ControlEvseRequest>
        //
        //          <ns:directId>?</ns:directId>
        //
        //          <ns:operation>
        //             <ns:operation>?</ns:operation>
        //          </ns:operation>
        //
        //          <!--Optional:-->
        //          <ns:maxPower>?</ns:maxPower>
        //
        //          <!--Optional:-->
        //          <ns:maxCurrent>?</ns:maxCurrent>
        //
        //          <!--Optional:-->
        //          <ns:onePhase>?</ns:onePhase>
        //
        //          <!--Optional:-->
        //          <ns:maxEnergy>?</ns:maxEnergy>
        //
        //          <!--Optional:-->
        //          <ns:minEnergy>?</ns:minEnergy>
        //
        //          <!--Optional:-->
        //          <ns:departure>
        //             <ns:DateTime>?</ns:DateTime>
        //          </ns:departure>
        //
        //       </ns:ControlEvseRequest>
        //    </soapenv:Body>
        // </soapenv:Envelope>

        #endregion

        #region (static) Parse(ControlEVSERequestXML,  OnException = null)

        /// <summary>
        /// Parse the given XML representation of an OCHPdirect control EVSE request.
        /// </summary>
        /// <param name="ControlEVSERequestXML">The XML to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ControlEVSERequest Parse(XElement             ControlEVSERequestXML,
                                               OnExceptionDelegate  OnException = null)
        {

            ControlEVSERequest _ControlEVSERequest;

            if (TryParse(ControlEVSERequestXML, out _ControlEVSERequest, OnException))
                return _ControlEVSERequest;

            return null;

        }

        #endregion

        #region (static) Parse(ControlEVSERequestText, OnException = null)

        /// <summary>
        /// Parse the given text representation of an OCHPdirect control EVSE request.
        /// </summary>
        /// <param name="ControlEVSERequestText">The text to parse.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static ControlEVSERequest Parse(String               ControlEVSERequestText,
                                               OnExceptionDelegate  OnException = null)
        {

            ControlEVSERequest _ControlEVSERequest;

            if (TryParse(ControlEVSERequestText, out _ControlEVSERequest, OnException))
                return _ControlEVSERequest;

            return null;

        }

        #endregion

        #region (static) TryParse(ControlEVSERequestXML,  out ControlEVSERequest, OnException = null)

        /// <summary>
        /// Try to parse the given XML representation of an OCHPdirect control EVSE request.
        /// </summary>
        /// <param name="ControlEVSERequestXML">The XML to parse.</param>
        /// <param name="ControlEVSERequest">The parsed control EVSE request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(XElement                ControlEVSERequestXML,
                                       out ControlEVSERequest  ControlEVSERequest,
                                       OnExceptionDelegate     OnException  = null)
        {

            try
            {

                ControlEVSERequest = new ControlEVSERequest(

                                         ControlEVSERequestXML.MapValueOrFail     (OCHPNS.Default + "directId",
                                                                                   Direct_Id.Parse),

                                         ControlEVSERequestXML.MapEnumValuesOrFail(OCHPNS.Default + "operation",
                                                                                   OCHPNS.Default + "operation",
                                                                                   s => (DirectOperations) Enum.Parse(typeof(DirectOperations), s)),

                                         ControlEVSERequestXML.MapValueOrNullable (OCHPNS.Default + "maxPower",
                                                                                   Single.Parse),

                                         ControlEVSERequestXML.MapValueOrNullable (OCHPNS.Default + "maxCurrent",
                                                                                   Single.Parse),

                                         ControlEVSERequestXML.MapValueOrNullable (OCHPNS.Default + "onePhase",
                                                                                   s => s == "true"),

                                         ControlEVSERequestXML.MapValueOrNullable (OCHPNS.Default + "maxEnergy",
                                                                                   Single.Parse),

                                         ControlEVSERequestXML.MapValueOrNullable (OCHPNS.Default + "minEnergy",
                                                                                   Single.Parse),

                                         ControlEVSERequestXML.MapValueOrNullable (OCHPNS.Default + "departure",
                                                                                   OCHPNS.Default + "DateTime",
                                                                                   DateTime.Parse)

                                     );

                return true;

            }
            catch (Exception e)
            {

                OnException?.Invoke(DateTime.UtcNow, ControlEVSERequestXML, e);

                ControlEVSERequest = null;
                return false;

            }

        }

        #endregion

        #region (static) TryParse(ControlEVSERequestText, out ControlEVSERequest, OnException = null)

        /// <summary>
        /// Try to parse the given text representation of an OCHPdirect control EVSE request.
        /// </summary>
        /// <param name="ControlEVSERequestText">The text to parse.</param>
        /// <param name="ControlEVSERequest">The parsed control EVSE request.</param>
        /// <param name="OnException">An optional delegate called whenever an exception occured.</param>
        public static Boolean TryParse(String                  ControlEVSERequestText,
                                       out ControlEVSERequest  ControlEVSERequest,
                                       OnExceptionDelegate     OnException  = null)
        {

            try
            {

                if (TryParse(XDocument.Parse(ControlEVSERequestText).Root,
                             out ControlEVSERequest,
                             OnException))

                    return true;

            }
            catch (Exception e)
            {
                OnException?.Invoke(DateTime.UtcNow, ControlEVSERequestText, e);
            }

            ControlEVSERequest = null;
            return false;

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => SOAP.Encapsulation(new XElement(OCHPNS.Default + "ControlEvseRequest",

                                      new XElement(OCHPNS.Default + "directId",             DirectId.ToString()),
                                      new XElement(OCHPNS.Default + "operation",
                                          new XElement(OCHPNS.Default + "operation",        XML_IO.AsText(Operation))),

                                      MaxPower.HasValue
                                          ? new XElement(OCHPNS.Default + "MaxPower",       MaxPower.  Value)
                                          : null,

                                      MaxCurrent.HasValue
                                          ? new XElement(OCHPNS.Default + "maxCurrent",     MaxCurrent.Value)
                                          : null,

                                      OnePhase.HasValue
                                          ? new XElement(OCHPNS.Default + "onePhase",       OnePhase.  Value ? "true" : "false")
                                          : null,

                                      MaxEnergy.HasValue
                                          ? new XElement(OCHPNS.Default + "maxEnergy",      MaxEnergy. Value)
                                          : null,

                                      MinEnergy.HasValue
                                          ? new XElement(OCHPNS.Default + "minEnergy",      MinEnergy. Value)
                                          : null,

                                      Departure.HasValue
                                          ? new XElement(OCHPNS.Default + "departure",
                                                new XElement(OCHPNS.Default + "departure",  Departure. Value.ToIso8601())
                                            )
                                          : null

                                 ));

        #endregion


        #region Operator overloading

        #region Operator == (ControlEVSERequest1, ControlEVSERequest2)

        /// <summary>
        /// Compares two control EVSE requests for equality.
        /// </summary>
        /// <param name="ControlEVSERequest1">A control EVSE request.</param>
        /// <param name="ControlEVSERequest2">Another control EVSE request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ControlEVSERequest ControlEVSERequest1, ControlEVSERequest ControlEVSERequest2)
        {

            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(ControlEVSERequest1, ControlEVSERequest2))
                return true;

            // If one is null, but not both, return false.
            if (((Object) ControlEVSERequest1 == null) || ((Object) ControlEVSERequest2 == null))
                return false;

            return ControlEVSERequest1.Equals(ControlEVSERequest2);

        }

        #endregion

        #region Operator != (ControlEVSERequest1, ControlEVSERequest2)

        /// <summary>
        /// Compares two control EVSE requests for inequality.
        /// </summary>
        /// <param name="ControlEVSERequest1">A control EVSE request.</param>
        /// <param name="ControlEVSERequest2">Another control EVSE request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ControlEVSERequest ControlEVSERequest1, ControlEVSERequest ControlEVSERequest2)

            => !(ControlEVSERequest1 == ControlEVSERequest2);

        #endregion

        #endregion

        #region IEquatable<ControlEVSERequest> Members

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

            // Check if the given object is a control EVSE request.
            var ControlEVSERequest = Object as ControlEVSERequest;
            if ((Object) ControlEVSERequest == null)
                return false;

            return this.Equals(ControlEVSERequest);

        }

        #endregion

        #region Equals(ControlEVSERequest)

        /// <summary>
        /// Compares two control EVSE requests for equality.
        /// </summary>
        /// <param name="ControlEVSERequest">A control EVSE request to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public override Boolean Equals(ControlEVSERequest ControlEVSERequest)
        {

            if ((Object) ControlEVSERequest == null)
                return false;

            return DirectId.    Equals(ControlEVSERequest.DirectId)     &&
                   Operation.Equals(ControlEVSERequest.Operation) &&

                   ((!MaxPower.HasValue && !ControlEVSERequest.MaxPower.HasValue) ||
                    ((MaxPower.HasValue &&  ControlEVSERequest.MaxPower.HasValue)
                          ? MaxPower.Value.Equals(ControlEVSERequest.MaxPower)
                          : false)) &&

                   ((!MaxCurrent.HasValue && !ControlEVSERequest.MaxCurrent.HasValue) ||
                    ((MaxCurrent.HasValue &&  ControlEVSERequest.MaxCurrent.HasValue)
                          ? MaxCurrent.Value.Equals(ControlEVSERequest.MaxCurrent)
                          : false)) &&

                   ((!OnePhase.HasValue && !ControlEVSERequest.OnePhase.HasValue) ||
                    ((OnePhase.HasValue &&  ControlEVSERequest.OnePhase.HasValue)
                          ? OnePhase.Value.Equals(ControlEVSERequest.OnePhase)
                          : false)) &&

                   ((!MaxEnergy.HasValue && !ControlEVSERequest.MaxEnergy.HasValue) ||
                    ((MaxEnergy.HasValue &&  ControlEVSERequest.MaxEnergy.HasValue)
                          ? MaxEnergy.Value.Equals(ControlEVSERequest.MaxEnergy)
                          : false)) &&

                   ((!MinEnergy.HasValue && !ControlEVSERequest.MinEnergy.HasValue) ||
                    ((MinEnergy.HasValue &&  ControlEVSERequest.MinEnergy.HasValue)
                          ? MinEnergy.Value.Equals(ControlEVSERequest.MinEnergy)
                          : false)) &&

                   ((!Departure.HasValue && !ControlEVSERequest.Departure.HasValue) ||
                    ((Departure.HasValue &&  ControlEVSERequest.Departure.HasValue)
                          ? Departure.Value.Equals(ControlEVSERequest.Departure)
                          : false));

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

                return DirectId.           GetHashCode() * 29 ^
                       Operation.          GetHashCode() * 23 ^

                       (MaxPower.HasValue
                            ? MaxPower.    GetHashCode() * 19
                            : 0) ^

                       (MaxCurrent.HasValue
                            ? MaxCurrent.  GetHashCode() * 17
                            : 0) ^

                       (OnePhase.HasValue
                            ? OnePhase.    GetHashCode() * 13
                            : 0) ^

                       (MaxEnergy.HasValue
                            ? MaxEnergy.   GetHashCode() * 11
                            : 0) ^

                       (MinEnergy.HasValue
                            ? MinEnergy.   GetHashCode() *  7
                            : 0) ^

                       (Departure.HasValue
                            ? Departure.   GetHashCode()
                            : 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a string representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(DirectId,
                             " / ",
                             Operation,

                             MaxPower.HasValue
                                 ? ", max " + MaxPower.Value + " kW"
                                 : "",

                             MaxCurrent.HasValue
                                 ? ", max " + MaxCurrent.Value + " A"
                                 : "",

                             OnePhase.HasValue
                                 ? ", 1 phase"
                                 : "",

                             MaxEnergy.HasValue
                                 ? ", max " + MaxPower.Value + " kWh"
                                 : "",

                             MinEnergy.HasValue
                                 ? ", min " + MinEnergy.Value + " kWh"
                                 : "",

                             Departure.HasValue
                                 ? ", depature at " + Departure.Value.ToIso8601()
                                 : "");

        #endregion

    }

}
