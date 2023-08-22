using System;
using System.Transactions;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;

namespace NWDPlayerStatistic.Exchanges.Payloads;

public class NWDPlayerStatisticGetDataForPlayerUpPayload : NWDPlayerStatisticUpPayload
{
    public NWDReference<NWDAccount> PlayerReference { get; set; }
    public string AssemblyQualifiedNameType { get; set; }

    public NWDPlayerStatisticGetDataForPlayerUpPayload() { }

    public NWDPlayerStatisticGetDataForPlayerUpPayload(ulong sProjectId, NWDEnvironmentKind sEnvironmentKind,NWDReference<NWDAccount> sPlayerReference, Type sStatisticType) 
    {
        ProjectId = sProjectId;
        Environment = sEnvironmentKind;
        PlayerReference = sPlayerReference;
        AssemblyQualifiedNameType = sStatisticType.AssemblyQualifiedName;
    }
}