using System;
using NWDFoundation.WebEdition.Models;

namespace NWDFoundation.WebEdition.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class NWDWebClassDescriptionAttribute : Attribute
    {
        public NWDWebEditionClassDescription Infos = new NWDWebEditionClassDescription();
        public NWDWebClassDescriptionAttribute(string sTitle, string sBootstrapIcon, string sDescription, int[] sItemsPerPageOption, bool sShowReference, bool sShowButton = false, bool sJsonClipboard = false)
        {
            Infos.Title = sTitle;
            Infos.BootstrapIcon = sBootstrapIcon;
            Infos.Description = sDescription;
            Infos.ItemsPerPageOption = sItemsPerPageOption;
            Infos.ShowReference = sShowReference;
            Infos.ShowButton = sShowButton;
            Infos.JsonClipboard = sJsonClipboard;
        }
    }
}