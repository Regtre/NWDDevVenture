using System;
using NWDFoundation.Models.Enums;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;

namespace NWDFoundation.Models
{
    [Serializable]
    public class NWDFrequentlyAskedQuestion : NWDDatabaseWebBasicModel
    {
        [NWDWebPropertyDescription("Visible", NWDWebEditionStyle.Bool, true, "",  "",  "")]
        public bool IsVisible { set; get; } = true;
        [NWDWebPropertyDescription("Domain / Controller", NWDWebEditionStyle.AsciiText, true, "",  "",  "", true,true,true)]
        public string Domain { set; get; } = "";
        [NWDWebPropertyDescription("SubDomain / Action", NWDWebEditionStyle.AsciiText, false, "",  "",  "", true, true, false)]
        public string SubDomain { set; get; } = "";
        [NWDWebPropertyDescription("Question", NWDWebEditionStyle.Text, false, "",  "",  "",  true,  true, false)]
        public string Question { set; get; } = "";
        [NWDWebPropertyDescription("Answer", NWDWebEditionStyle.Text, false, "",  "",  "")]
        public string Answer { set; get; } = "";
        [NWDWebPropertyDescription("App device", NWDWebEditionStyle.Flag, false, "",  "",  "")]
        public NWDNavigatorFlag AppConcerned { set; get; } = NWDNavigatorFlag.None;
        [NWDWebPropertyDescription("System concerned", NWDWebEditionStyle.Flag, false, "",  "",  "")]
        public NWDNavigatorOSFlag SystemConcerned { set; get; } = NWDNavigatorOSFlag.None;
        public NWDFrequentlyAskedQuestion()
        {
        }
    }
}