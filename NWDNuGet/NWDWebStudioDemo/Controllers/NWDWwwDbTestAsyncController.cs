using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDWebRuntime.Tools;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;
using NWDFoundation.WebEdition.Models;
using NWDWebEditor.Controllers;
using NWDWebRuntime.Tools.Cookies;
using NWDWebStandard.Models;

namespace NWDWebStandard.Controllers
{
    [Serializable]
    public class NWDWwwDbTestModel : NWDDatabaseWebBasicModel
    {
        [NWDWebPropertyDescription("Title", NWDWebEditionStyle.Text,false, "", "my Title", "my Placeholder", true, false, true)]
        public string Title { set; get; } = NWDToolBox.UniqueRandomString(8);

        [NWDWebPropertyDescription("Description", NWDWebEditionStyle.RichText,false, "", "my Description", "my Description Placeholder", true, true, false)]
        public string Description { set; get; } = NWDToolBox.UniqueRandomString(16);

        [NWDWebPropertyDescription("TestOfBoolean", NWDWebEditionStyle.Bool,false, "", "my TestOfBoolean", "my TestOfBoolean Placeholder ")]
        public bool TestOfBoolean { set; get; } = true;

        [NWDWebPropertyDescription("Description", NWDWebEditionStyle.RichText,false, "", "my Description", "my Description Placeholder")]
        public string DescriptionC { set; get; } = NWDToolBox.UniqueRandomString(48);
    }

    public class NWDWwwDbTestAsyncController : NWDWebDBEditionAsyncController<NWDWwwDbTestModel>
    {
    }
    

}