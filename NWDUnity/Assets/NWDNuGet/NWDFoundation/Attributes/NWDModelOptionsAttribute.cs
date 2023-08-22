using System;

namespace NWDFoundation.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class NWDModelOptionsAttribute : Attribute
    {
        public const string None = "";
        public const string Mandatory = "Mandatory";

        public string Module = None;
        public bool Default = true;
        public string MenuName = string.Empty;
        public string Description = string.Empty;
        public bool Editable = true;

        public NWDModelOptionsAttribute(string sModule, string sMenuName, string sDescription, bool sDefaultActive = true, bool sEditable = false)
        {
            this.Module = sModule;
            this.Description = sDescription;
            this.MenuName = sMenuName;
            this.Default = sDefaultActive;
            this.Editable = sEditable;
        }

        public NWDModelOptionsAttribute(string sMenuName, string sDescription, bool sDefaultActive = true, bool sEditable = false)
        {
            this.Module = None;
            this.Description = sDescription;
            this.MenuName = sMenuName;
            this.Default = sDefaultActive;
            this.Editable = sEditable;
        }

        public NWDModelOptionsAttribute(bool sDefaultActive = true)
        {
            this.Module = None;
            this.Default = sDefaultActive;
        }
    }
}
