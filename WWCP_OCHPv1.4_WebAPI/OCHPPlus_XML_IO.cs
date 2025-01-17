﻿/*
 * Copyright (c) 2014-2025 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.Hermod.SOAP;

using cloud.charging.open.protocols.WWCP;
using cloud.charging.open.protocols.OCHPv1_4.CPO;

#endregion

namespace cloud.charging.open.protocols.OCHPv1_4.WebAPI
{

    /// <summary>
    /// OCHP+ XML I/O.
    /// </summary>
    public static class OCHPPlus_XML_IO
    {

        #region ToXML(this ChargePointInfos, RoamingNetwork, XMLNamespaces = null, ChargePointInfo2XML = null, XMLPostProcessing = null)

        /// <summary>
        /// Convert the given enumeration of EVSE data records to XML.
        /// </summary>
        /// <param name="ChargePointInfos">An enumeration of charge points.</param>
        /// <param name="RoamingNetwork">The WWCP roaming network.</param>
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        /// <param name="ChargePointInfo2XML">An optional delegate to process a charge point information XML before sending it somewhere.</param>
        /// <param name="XMLPostProcessing">An optional delegate to process the XML after its final creation.</param>
        public static XElement ToXML(this IEnumerable<ChargePointInfo>   ChargePointInfos,
                                     RoamingNetwork                      RoamingNetwork,
                                     XMLNamespacesDelegate?              XMLNamespaces        = null,
                                     ChargePointInfo2XMLDelegate?        ChargePointInfo2XML  = null,
                                     CPO.XMLPostProcessingDelegate?      XMLPostProcessing    = null)
        {

            #region Initial checks

            if (ChargePointInfos == null)
                throw new ArgumentNullException(nameof(ChargePointInfos), "The given enumeration of charge points must not be null!");

            var _EVSEDataRecords = ChargePointInfos.ToArray();

            if (ChargePointInfo2XML == null)
                ChargePointInfo2XML = (rn, evsedatarecord, xml) => xml;

            if (XMLPostProcessing == null)
                XMLPostProcessing = xml => xml;

            #endregion

            return null;

            //return XMLPostProcessing(
            //           SOAP.Encapsulation(new XElement(OCHPNS.EVSEData + "eRoamingEvseData",
            //                                  new XElement(OCHPNS.EVSEData + "EvseData",

            //                                      _EVSEDataRecords.Any()

            //                                          ? _EVSEDataRecords.
            //                                                ToLookup(evsedatarecord => evsedatarecord.EVSE?.Operator).
            //                                                Select(group => group.Any(evsedatarecord => evsedatarecord != null)

            //                                                           ? new XElement(OCHPNS.EVSEData + "OperatorEvseData",

            //                                                                 new XElement(OCHPNS.EVSEData + "OperatorID", group.Key.Id.OriginId),

            //                                                                 group.Key.Name.Any()
            //                                                                     ? new XElement(OCHPNS.EVSEData + "OperatorName", group.Key.Name.FirstText)
            //                                                                     : null,

            //                                                                 new XElement(OCHPPlusNS.EVSEOperator + "DataLicenses",
            //                                                                     group.Key.DataLicenses.SafeSelect(license => new XElement(OCHPPlusNS.EVSEOperator + "DataLicense",
            //                                                                                                                      new XElement(OCHPPlusNS.EVSEOperator + "Id", license.Id),
            //                                                                                                                      new XElement(OCHPPlusNS.EVSEOperator + "Description", license.Description),
            //                                                                                                                      license.URIs.Any()
            //                                                                                                                          ? new XElement(OCHPPlusNS.EVSEOperator + "DataLicenseURIs",
            //                                                                                                                                license.URIs.SafeSelect(uri => new XElement(OCHPPlusNS.EVSEOperator + "DataLicenseURI", uri)))
            //                                                                                                                          : null
            //                                                                                                                  ))
            //                                                                 ),

            //                                                                 // <EvseDataRecord> ... </EvseDataRecord>
            //                                                                 group.Where(evsedatarecord => evsedatarecord != null).
            //                                                                       Select(evsedatarecord => ChargePointInfo2XML(RoamingNetwork, evsedatarecord, evsedatarecord.ToXML())).
            //                                                                       ToArray()

            //                                                             )

            //                                                           : null

            //                                                ).ToArray()

            //                                            : null

            //                                      )
            //                                  ),
            //                              XMLNamespaces));

        }

        #endregion

        #region ToXML(this EVSEs, XMLNamespaces = null, EVSEStatusRecord2XML = null, XMLPostProcessing = null)

        /// <summary>
        /// Convert the given enumeration of EVSEs into an EVSE status records XML.
        /// </summary>
        /// <param name="EVSEs">An enumeration of EVSEs.</param>
        /// <param name="RoamingNetwork">The WWCP roaming network.</param>
        /// <param name="XMLNamespaces">An optional delegate to process the XML namespaces.</param>
        /// <param name="EVSEStatusRecord2XML">An optional delegate to process an EVSE status record XML before sending it somewhere.</param>
        /// <param name="XMLPostProcessing">An optional delegate to process the XML after its final creation.</param>
        public static XElement ToXML(this IEnumerable<IEVSE>         EVSEs,
                                     RoamingNetwork                  RoamingNetwork,
                                     XMLNamespacesDelegate?          XMLNamespaces          = null,
                                     EVSEStatus2XMLDelegate?         EVSEStatusRecord2XML   = null,
                                     CPO.XMLPostProcessingDelegate?  XMLPostProcessing      = null)
        {

            #region Initial checks

            if (EVSEs == null)
                throw new ArgumentNullException(nameof(EVSEs),  "The given enumeration of EVSEs must not be null!");

            if (EVSEStatusRecord2XML == null)
                EVSEStatusRecord2XML = (rn, evsestatusrecord, xml) => xml;

            if (XMLPostProcessing == null)
                XMLPostProcessing = xml => xml;

            #endregion

            return null;

//            return XMLPostProcessing(
//                       SOAP.Encapsulation(new XElement(OCHPNS.EVSEStatus + "eRoamingEvseStatus",
//                                              new XElement(OCHPNS.EVSEStatus + "EvseStatuses",

//                                                  EVSEs.ToLookup(evse => evse.Operator,
//                                                                 evse => {

//                                                                     try
//                                                                     {
//                                                                         return new EVSEStatusRecord(evse);
//                                                                     }
//#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception.
//#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
//                                                                     catch
//#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
//#pragma warning restore RCS1075 // Avoid empty catch clause that catches System.Exception.
//                                                                     { }

//                                                                     return null;

//                                                                 }).

//                                                        Select(group => group.Any(evsestatusrecord => evsestatusrecord != null)

//                                                            ? new XElement(OCHPNS.EVSEStatus + "OperatorEvseStatus",

//                                                                new XElement(OCHPNS.EVSEStatus + "OperatorID", group.Key.Id.OriginId),

//                                                                group.Key.Name.Any()
//                                                                    ? new XElement(OCHPNS.EVSEStatus + "OperatorName", group.Key.Name.FirstText)
//                                                                    : null,

//                                                                new XElement(OCHPPlusNS.EVSEOperator + "DataLicenses",
//                                                                    group.Key.DataLicenses.SafeSelect(license => new XElement(OCHPPlusNS.EVSEOperator + "DataLicense",
//                                                                                                                     new XElement(OCHPPlusNS.EVSEOperator + "Id",           license.Id),
//                                                                                                                     new XElement(OCHPPlusNS.EVSEOperator + "Description",  license.Description),
//                                                                                                                     license.URIs.Any()
//                                                                                                                         ? new XElement(OCHPPlusNS.EVSEOperator + "DataLicenseURIs", 
//                                                                                                                               license.URIs.SafeSelect(uri => new XElement(OCHPPlusNS.EVSEOperator + "DataLicenseURI", uri)))
//                                                                                                                         : null
//                                                                                                                 ))
//                                                                ),

//                                                                // <EvseStatusRecord> ... </EvseStatusRecord>
//                                                                group.Where (evsestatusrecord => evsestatusrecord != null).
//                                                                      Select(evsestatusrecord => EVSEStatusRecord2XML(RoamingNetwork, evsestatusrecord, evsestatusrecord.ToXML())).
//                                                                      ToArray()

//                                                            )
//                                                          : null

//                                                      ).ToArray()

//                                                  )
//                                              ),
//                                          XMLNamespaces));


        }

        #endregion

    }

}
