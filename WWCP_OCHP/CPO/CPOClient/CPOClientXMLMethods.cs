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
using System.Xml.Linq;
using System.Collections.Generic;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace org.GraphDefined.WWCP.OCHPv1_4
{

    /// <summary>
    /// OCHP CPO Client XML methods.
    /// </summary>
    public static class CPOClientXMLMethods
    {

        #region PushEVSEDataXML  (ChargePointInfos)

        /// <summary>
        /// Create an OCHP SetChargePointList XML/SOAP request.
        /// </summary>
        /// <param name="ChargePointInfos">An enumeration of charge point infos.</param>
        public static XElement PushEVSEDataXML(IEnumerable<ChargePointInfo>  ChargePointInfos)
        {

            #region Documentation

            // <soapenv:Envelope xmlns:soapenv = "http://schemas.xmlsoap.org/soap/envelope/"
            //                   xmlns:OCHP    = "http://ochp.eu/1.4">            //
            //    <soapenv:Header/>
            //    <soapenv:Body>
            //       <OCHP:SetChargePointListRequest>
            //
            //          <!--1 or more repetitions:-->
            //          <OCHP:chargePointInfoArray>            //             ...
            //          </OCHP:chargePointInfoArray>            //
            //       </OCHP:SetChargePointListRequest>
            //    </soapenv:Body>
            // </soapenv:Envelope>

            #endregion

            #region Initial checks

            if (ChargePointInfos == null || !ChargePointInfos.Any())
                throw new ArgumentNullException(nameof(ChargePointInfos),  "The given enumeration of charge point infos must not be null or empty!");

            #endregion


            return SOAP.Encapsulation(new XElement(OCHPNS.Default + "SetChargePointListRequest",

                                          ChargePointInfos.Select(chargepointinfo => chargepointinfo.ToXML()).
                                                           ToArray()

                                     ));

        }

        #endregion

        //#region PushEVSEStatusXML(GroupedEVSEStatusRecords, OCHPAction = update,   Operator = null, OperatorNameSelector = null)

        ///// <summary>
        ///// Create an OCHP PushEVSEStatus XML/SOAP request.
        ///// </summary>
        ///// <param name="GroupedEVSEStatusRecords">EVSE status records grouped by their Charging Station Operator.</param>
        ///// <param name="OCHPAction">The server-side data management operation.</param>
        ///// <param name="Operator">An optional Charging Station Operator, which will be copied into the main OperatorID-section of the OCHP SOAP request.</param>
        ///// <param name="OperatorNameSelector">An optional delegate to select an Charging Station Operator name, which will be copied into the OperatorName-section of the OCHP SOAP request.</param>
        //public static XElement PushEVSEStatusXML(ILookup<ChargingStationOperator, EVSEStatusRecord>  GroupedEVSEStatusRecords,
        //                                         ActionTypes                               OCHPAction            = ActionTypes.update,
        //                                         ChargingStationOperator                             Operator              = null,
        //                                         ChargingStationOperatorNameSelectorDelegate         OperatorNameSelector  = null)
        //{

        //    #region Documentation

        //    // <soapenv:Envelope xmlns:soapenv    = "http://schemas.xmlsoap.org/soap/envelope/"
        //    //                   xmlns:EVSEStatus = "http://www.hubject.com/b2b/services/evsestatus/v2.0">
        //    // 
        //    //    <soapenv:Header/>
        //    // 
        //    //    <soapenv:Body>
        //    //       <EVSEStatus:eRoamingPushEvseStatus>
        //    //          <EVSEStatus:ActionType>fullLoad|update|insert|delete</EVSEStatus:ActionType>
        //    //          <EVSEStatus:OperatorEvseStatus>
        //    // 
        //    //             <EVSEStatus:OperatorID>DE*GEF</EVSEStatus:OperatorID>
        //    //             <!--Optional:-->
        //    //             <EVSEStatus:OperatorName>Test-CPO</EVSEStatus:OperatorName>
        //    // 
        //    //             <!--One or more repetitions:-->
        //    //             <EVSEStatus:EvseStatusRecord>
        //    //                <EVSEStatus:EvseId>DE*GEF*E1234*1</EVSEStatus:EvseId>
        //    //                <EVSEStatus:EvseStatus>Occupied</EVSEStatus:EvseStatus>
        //    //             </EVSEStatus:EvseStatusRecord>
        //    // 
        //    //          </EVSEStatus:OperatorEvseStatus>
        //    //       </EVSEStatus>
        //    //    </soapenv:Body>
        //    // 
        //    // </soapenv:Envelope>

        //    #endregion

        //    #region Initial checks

        //    if (GroupedEVSEStatusRecords == null || !GroupedEVSEStatusRecords.Any())
        //        throw new ArgumentNullException(nameof(GroupedEVSEStatusRecords),  "The given EVSE status lookup must not be null or empty!");

        //    if (Operator == null && GroupedEVSEStatusRecords.Any())
        //        Operator = GroupedEVSEStatusRecords.FirstOrDefault()?.Key;

        //    var OperatorName = OperatorNameSelector != null
        //                           ? OperatorNameSelector(Operator?.Name ?? GroupedEVSEStatusRecords.FirstOrDefault()?.Key?.Name)
        //                           : (Operator ?? GroupedEVSEStatusRecords.FirstOrDefault()?.Key).Name.FirstText;

        //    #endregion


        //    return SOAP.Encapsulation(new XElement(OCHPNS.EVSEStatus + "eRoamingPushEvseStatus",
        //                                  new XElement(OCHPNS.EVSEStatus + "ActionType",          OCHPAction.ToString()),

        //                                  new XElement(OCHPNS.EVSEStatus + "OperatorEvseStatus",

        //                                      new XElement(OCHPNS.EVSEStatus + "OperatorID", Operator.Id.OriginId),

        //                                      OperatorName.IsNotNullOrEmpty()
        //                                              ? new XElement(OCHPNS.EVSEStatus + "OperatorName", OperatorName)
        //                                              : null,

        //                                      // <EvseStatusRecord> ... </EvseStatusRecord>
        //                                      GroupedEVSEStatusRecords.
        //                                          Where     (group => group.Key != null).
        //                                          SelectMany(group => group.Where (evsestatusrecord => evsestatusrecord != null).
        //                                                                    Select(evsestatusrecord => evsestatusrecord.ToXML())).
        //                                          ToArray()

        //                                  )
        //                               ));

        //}

        //#endregion


        //#region AuthorizeStartXML(OperatorId, AuthToken, EVSEId = null, PartnerProductId = null, SessionId = null, PartnerSessionId = null)

        ///// <summary>
        ///// Create an OCHP Authorize Start XML request.
        ///// </summary>
        ///// <param name="OperatorId">An EVSE Operator identification.</param>
        ///// <param name="AuthToken">A (RFID) user identification.</param>
        ///// <param name="EVSEId">An optional EVSE identification.</param>
        ///// <param name="PartnerProductId">An optional partner product identification.</param>
        ///// <param name="SessionId">An optional session identification.</param>
        ///// <param name="PartnerSessionId">An optional partner session identification.</param>
        //public static XElement AuthorizeStartXML(ChargingStationOperator_Id     OperatorId,
        //                                         Auth_Token          AuthToken,
        //                                         EVSE_Id             EVSEId            = null,   // OCHP v2.0: Optional
        //                                         ChargingProduct_Id  PartnerProductId  = null,   // OCHP v2.0: Optional [100]
        //                                         ChargingSession_Id  SessionId         = null,   // OCHP v2.0: Optional
        //                                         ChargingSession_Id  PartnerSessionId  = null)   // OCHP v2.0: Optional [50]
        //{

        //    #region Documentation

        //    // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
        //    //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/EVSEData.0"
        //    //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/EVSEData.0">
        //    //
        //    //    <soapenv:Header/>
        //    //
        //    //    <soapenv:Body>
        //    //       <Authorization:eRoamingAuthorizeStart>
        //    //
        //    //          <!--Optional:-->
        //    //          <Authorization:SessionID>?</Authorization:SessionID>
        //    //          <!--Optional:-->
        //    //          <Authorization:PartnerSessionID>?</Authorization:PartnerSessionID>
        //    //
        //    //          <Authorization:OperatorID>?</Authorization:OperatorID>
        //    //
        //    //          <!--Optional:-->
        //    //          <Authorization:EVSEID>?</Authorization:EVSEID>
        //    //
        //    //          <Authorization:Identification>
        //    //
        //    //             <!--You have a CHOICE of the next 4 items at this level-->
        //    //             <CommonTypes:RFIDmifarefamilyIdentification>
        //    //                <CommonTypes:UID>?</CommonTypes:UID>
        //    //             </CommonTypes:RFIDmifarefamilyIdentification>
        //    //
        //    //             <CommonTypes:QRCodeIdentification>
        //    //
        //    //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //    //
        //    //                <!--You have a CHOICE of the next 2 items at this level-->
        //    //                <CommonTypes:PIN>?</CommonTypes:PIN>
        //    //
        //    //                <CommonTypes:HashedPIN>
        //    //                   <CommonTypes:Value>?</CommonTypes:Value>
        //    //                   <CommonTypes:Function>?</CommonTypes:Function>
        //    //                   <CommonTypes:Salt>?</CommonTypes:Salt>
        //    //                </CommonTypes:HashedPIN>
        //    //
        //    //             </CommonTypes:QRCodeIdentification>
        //    //
        //    //             <CommonTypes:PlugAndChargeIdentification>
        //    //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //    //             </CommonTypes:PlugAndChargeIdentification>
        //    //
        //    //             <CommonTypes:RemoteIdentification>
        //    //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //    //             </CommonTypes:RemoteIdentification>
        //    //
        //    //          </Authorization:Identification>
        //    //
        //    //          <!--Optional:-->
        //    //          <Authorization:PartnerProductID>?</Authorization:PartnerProductID>
        //    //
        //    //       </Authorization:eRoamingAuthorizeStart>
        //    //    </soapenv:Body>
        //    //
        //    // </soapenv:Envelope>

        //    #endregion

        //    #region Initial checks

        //    if (OperatorId == null)
        //        throw new ArgumentNullException(nameof(OperatorId), "The given Charging Station Operator identification must not be null!");

        //    if (AuthToken  == null)
        //        throw new ArgumentNullException(nameof(AuthToken),  "The given authentication token must not be null!");

        //    #endregion

        //    return SOAP.Encapsulation(new XElement(OCHPNS.Authorization + "eRoamingAuthorizeStart",

        //                                  SessionId        != null ? new XElement(OCHPNS.Authorization + "SessionID",        SessionId.       ToString()) : null,
        //                                  PartnerSessionId != null ? new XElement(OCHPNS.Authorization + "PartnerSessionID", PartnerSessionId.ToString()) : null,

        //                                  new XElement(OCHPNS.Authorization + "OperatorID", OperatorId.OriginId),

        //                                  EVSEId != null
        //                                      ? new XElement(OCHPNS.Authorization + "EVSEID", EVSEId.OriginId)
        //                                      : null,

        //                                  new XElement(OCHPNS.Authorization + "Identification",
        //                                      new XElement(OCHPNS.CommonTypes + "RFIDmifarefamilyIdentification",
        //                                         new XElement(OCHPNS.CommonTypes + "UID", AuthToken.ToString())
        //                                      )
        //                                  ),

        //                                  PartnerProductId != null
        //                                      ? new XElement(OCHPNS.Authorization + "PartnerProductID", PartnerProductId.ToString())
        //                                      : null

        //                             ));

        //}

        //#endregion

        //#region AuthorizeStopXML (OperatorId, SessionId, AuthToken, EVSEId = null, PartnerSessionId = null)

        ///// <summary>
        ///// Create an OCHP AuthorizeStop XML request.
        ///// </summary>
        ///// <param name="OperatorId">An Charging Station Operator identification.</param>
        ///// <param name="SessionId">The session identification.</param>
        ///// <param name="AuthToken">The (RFID) user identification.</param>
        ///// <param name="EVSEId">An optional EVSE identification.</param>
        ///// <param name="PartnerSessionId">An optional partner session identification.</param>
        //public static XElement AuthorizeStopXML(ChargingStationOperator_Id     OperatorId,
        //                                        ChargingSession_Id  SessionId,
        //                                        Auth_Token          AuthToken,
        //                                        EVSE_Id             EVSEId            = null,
        //                                        ChargingSession_Id  PartnerSessionId  = null)
        //{

        //    #region Documentation

        //    // <soapenv:Envelope xmlns:soapenv       = "http://schemas.xmlsoap.org/soap/envelope/"
        //    //                   xmlns:Authorization = "http://www.hubject.com/b2b/services/authorization/EVSEData.0"
        //    //                   xmlns:CommonTypes   = "http://www.hubject.com/b2b/services/commontypes/EVSEData.0">
        //    //
        //    //    <soapenv:Header/>
        //    //
        //    //    <soapenv:Body>
        //    //       <Authorization:eRoamingAuthorizeStop>
        //    // 
        //    //          <Authorization:SessionID>?</Authorization:SessionID>
        //    // 
        //    //          <!--Optional:-->
        //    //          <Authorization:PartnerSessionID>?</Authorization:PartnerSessionID>
        //    // 
        //    //          <Authorization:OperatorID>?</Authorization:OperatorID>
        //    // 
        //    //          <!--Optional:-->
        //    //          <Authorization:EVSEID>?</Authorization:EVSEID>
        //    // 
        //    //          <Authorization:Identification>
        //    // 
        //    //             <!--You have a CHOICE of the next 4 items at this level-->
        //    //             <CommonTypes:RFIDmifarefamilyIdentification>
        //    //                <CommonTypes:UID>?</CommonTypes:UID>
        //    //             </CommonTypes:RFIDmifarefamilyIdentification>
        //    // 
        //    //             <CommonTypes:QRCodeIdentification>
        //    //
        //    //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //    //
        //    //                <!--You have a CHOICE of the next 2 items at this level-->
        //    //                <CommonTypes:PIN>?</CommonTypes:PIN>
        //    //
        //    //                <CommonTypes:HashedPIN>
        //    //                   <CommonTypes:Value>?</CommonTypes:Value>
        //    //                   <CommonTypes:Function>?</CommonTypes:Function>
        //    //                   <CommonTypes:Salt>?</CommonTypes:Salt>
        //    //                </CommonTypes:HashedPIN>
        //    //
        //    //             </CommonTypes:QRCodeIdentification>
        //    // 
        //    //             <CommonTypes:PlugAndChargeIdentification>
        //    //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //    //             </CommonTypes:PlugAndChargeIdentification>
        //    // 
        //    //             <CommonTypes:RemoteIdentification>
        //    //                <CommonTypes:EVCOID>?</CommonTypes:EVCOID>
        //    //             </CommonTypes:RemoteIdentification>
        //    // 
        //    //          </Authorization:Identification>
        //    // 
        //    //       </Authorization:eRoamingAuthorizeStop>
        //    //    </soapenv:Body>
        //    //
        //    // </soapenv:Envelope>

        //    #endregion

        //    #region Initial checks

        //    if (OperatorId == null)
        //        throw new ArgumentNullException(nameof(OperatorId), "The given parameter must not be null!");

        //    if (SessionId  == null)
        //        throw new ArgumentNullException(nameof(SessionId),  "The given parameter must not be null!");

        //    if (AuthToken  == null)
        //        throw new ArgumentNullException(nameof(AuthToken),  "The given parameter must not be null!");

        //    #endregion

        //    return SOAP.Encapsulation(new XElement(OCHPNS.Authorization + "eRoamingAuthorizeStop",

        //                                  new XElement(OCHPNS.Authorization + "SessionID", SessionId.ToString()),

        //                                  PartnerSessionId != null ? new XElement(OCHPNS.Authorization + "PartnerSessionID", PartnerSessionId.ToString()) : null,

        //                                  new XElement(OCHPNS.Authorization + "OperatorID", OperatorId.OriginId),

        //                                  EVSEId != null
        //                                      ? new XElement(OCHPNS.Authorization + "EVSEID", EVSEId.OriginId)
        //                                      : null,

        //                                  new XElement(OCHPNS.Authorization + "Identification",
        //                                      new XElement(OCHPNS.CommonTypes + "RFIDmifarefamilyIdentification",
        //                                         new XElement(OCHPNS.CommonTypes + "UID", AuthToken.ToString())
        //                                      )
        //                                  )

        //                              ));

        //}

        //#endregion


        //#region PullAuthenticationDataXML(OperatorId)

        ///// <summary>
        ///// Create an OCHP PullAuthenticationData XML request.
        ///// </summary>
        ///// <param name="OperatorId">An Charging Station Operator identification.</param>
        //public static XElement PullAuthenticationDataXML(ChargingStationOperator_Id OperatorId)
        //{

        //    #region Documentation

        //    // <soapenv:Envelope xmlns:soapenv            = "http://schemas.xmlsoap.org/soap/envelope/"
        //    //                   xmlns:AuthenticationData = "http://www.hubject.com/b2b/services/authenticationdata/EVSEData.0">
        //    //
        //    //    <soapenv:Header/>
        //    //
        //    //    <soapenv:Body>
        //    //       <AuthenticationData:eRoamingPullAuthenticationData>
        //    //          <AuthenticationData:OperatorID>DE*GEF</AuthenticationData:OperatorID>
        //    //       </AuthenticationData:eRoamingPullAuthenticationData>
        //    //    </soapenv:Body>
        //    //
        //    // </soapenv:Envelope>

        //    #endregion

        //    #region Initial checks

        //    if (OperatorId == null)
        //        throw new ArgumentNullException(nameof(OperatorId), "The given parameter must not be null!");

        //    #endregion

        //    return SOAP.Encapsulation(new XElement(OCHPNS.AuthenticationData + "eRoamingPullAuthenticationData",
        //                                  new XElement(OCHPNS.AuthenticationData + "OperatorID", OperatorId.OriginId)
        //                              ));

        //}

        //#endregion

    }

}
