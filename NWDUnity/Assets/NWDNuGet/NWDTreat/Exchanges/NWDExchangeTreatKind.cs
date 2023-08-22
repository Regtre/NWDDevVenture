using System;
using NWDFoundation.Exchanges;

namespace NWDTreat.Exchanges
{
    [Serializable]
    public enum NWDExchangeTreatKind
    {
        /// <summary>
        /// No exchange kind (!)
        /// </summary>
        None = NWDExchangeKind.None,

        /// <summary>
        /// Use to test connection : ok or ko
        /// </summary>
        Test = NWDExchangeKind.Test,

        /// <summary>
        /// Not yet specified
        /// </summary>
        Unknown = NWDExchangeKind.Unknown,
        
        ServiceCreate = 75, //=> response NWDDownPayloadServiceCreate
        ServiceUpdate = 76, //=> response NWDDownPayloadServiceUpdate
        ServiceDelete = 77, //=> response NWDDownPayloadServiceDelete
        AssociateService = 78, //=> response NWDDownPayloadAssociateService
        AssociateSubService = 79, //=> response NWDDownPayloadAssociateSubService
        DissociateService = 80, //=> response NWDDownPayloadDissociateService
        GetSubServicesFromAccount = 81, //=> response NWDDownPayloadGetSubServicesFromAccount
    }
}