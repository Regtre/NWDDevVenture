using System.Collections.Generic;
using NWDEditor.Exchanges.Payloads;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Models;

namespace NWDEditor
{
    public class NWDDownPayloadGetProjectSettings : NWDDownPayloadEditor
    {
        public NWDProjectDescription? ProjectDescription { get; set; }
    }

}

