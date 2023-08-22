using System;

namespace NWDFoundation.WebEdition.Models
{
    [Serializable]
    public class NWDWebEditionClassDescription
    {
        public string Title = string.Empty;
        public string BootstrapIcon = string.Empty;
        public string Description = string.Empty;
        public int[] ItemsPerPageOption = new int[]{5,10,15};
        public bool JsonClipboard = false;
        public bool ShowReference = false;
        public bool ShowButton = false;
        public NWDWebEditionClassDescription()
        {
        }
        public NWDWebEditionClassDescription(string sTitle, string sBootstrapIcon, string sDescription, int[] sItemsPerPageOption, bool sJsonClipboard, bool sShowReference, bool sShowButton)
        {
            Title = sTitle;
            BootstrapIcon = sBootstrapIcon;
            Description = sDescription;
            ItemsPerPageOption = sItemsPerPageOption;
            JsonClipboard = sJsonClipboard;
            ShowReference = sShowReference;
            ShowButton = sShowButton;
        }
    }
}