using System;
using System.Collections.Generic;
using System.Linq;
using NWDFoundation.WebEdition.Enums;
using NWDFoundation.WebEdition.Facades;
using NWDFoundation.WebEdition.Models;

namespace NWDFoundation.WebEdition.Attributes
{
    public class NWDWebPropertyHiddenDebugable : NWDWebPropertyDescriptionAttribute
    {
        public NWDWebPropertyHiddenDebugable()
        {
            Infos.Label = string.Empty;
            Infos.Style = NWDWebEditionStyle.HiddenDebugable;
            Infos.Required = false;
            Infos.BootstrapIcon = string.Empty;
            Infos.Description = string.Empty;
            Infos.Placeholder = string.Empty;
            Infos.UseAsSortBy = false;
            Infos.UseAsColumn = false;
            Infos.IsPrimaryColumn = false;
        }
    }
    public class NWDWebPropertyHidden : NWDWebPropertyDescriptionAttribute
    {
        public NWDWebPropertyHidden()
        {
            Infos.Label = string.Empty;
            Infos.Style = NWDWebEditionStyle.Hidden;
            Infos.Required = false;
            Infos.BootstrapIcon = string.Empty;
            Infos.Description = string.Empty;
            Infos.Placeholder = string.Empty;
            Infos.UseAsSortBy = false;
            Infos.UseAsColumn = false;
            Infos.IsPrimaryColumn = false;
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class NWDWebPropertyDescriptionAttribute : Attribute
    {
        public NWDWebEditionPropertyDescription Infos = new NWDWebEditionPropertyDescription();
        public NWDWebPropertyDescriptionAttribute()
        {
        }
        public NWDWebPropertyDescriptionAttribute(
            string sLabel,
            NWDWebEditionStyle sStyle,
            bool sRequired,
            string sBootstrapIcon,
            string sDescription,
            string sPlaceholder,
            bool sUseAsSortBy = false,
            bool sUseAsColumn = false,
            bool sIsPrimaryColumn = false)
        {
            Infos.Label = sLabel;
            Infos.Style = sStyle;
            Infos.Required = sRequired;
            Infos.BootstrapIcon = sBootstrapIcon;
            Infos.Description = sDescription;
            Infos.Placeholder = sPlaceholder;
            Infos.UseAsSortBy = sUseAsSortBy;
            Infos.UseAsColumn = sUseAsColumn;
            Infos.IsPrimaryColumn = sIsPrimaryColumn;
        }
        
        public NWDWebPropertyDescriptionAttribute(
            string sLabel, 
            NWDWebEditionStyle sStyle, 
            bool sRequired,
            string sBootstrapIcon, 
            string sDescription, 
            string sPlaceholder, Type sDropDownValues,
            bool sUseAsSortBy = false,
            bool sUseAsColumn = false,
            bool sIsPrimaryColumn = false
        )
        {
            Infos.Label = sLabel;
            Infos.Style = sStyle;
            Infos.Required = sRequired;
            Infos.BootstrapIcon = sBootstrapIcon;
            Infos.Description = sDescription;
            Infos.Placeholder = sPlaceholder;
            Infos.UseAsSortBy = sUseAsSortBy;
            Infos.UseAsColumn = sUseAsColumn;
            Infos.IsPrimaryColumn = sIsPrimaryColumn;
            Infos.DropDownValues = Enum.GetNames(sDropDownValues).ToList();
        }
        
        public NWDWebPropertyDescriptionAttribute(
            string sLabel, 
            NWDWebEditionStyle sStyle, 
            bool sRequired,
            string sBootstrapIcon, 
            string sDescription, 
            string sPlaceholder, string[] sDropDownValues,
            bool sUseAsSortBy = false,
            bool sUseAsColumn = false,
            bool sIsPrimaryColumn = false
        )
        {
            Infos.Label = sLabel;
            Infos.Style = sStyle;
            Infos.Required = sRequired;
            Infos.BootstrapIcon = sBootstrapIcon;
            Infos.Description = sDescription;
            Infos.Placeholder = sPlaceholder;
            Infos.UseAsSortBy = sUseAsSortBy;
            Infos.UseAsColumn = sUseAsColumn;
            Infos.IsPrimaryColumn = sIsPrimaryColumn;
            Infos.DropDownValues= sDropDownValues.ToList();
        }
        public NWDWebPropertyDescriptionAttribute(
            string sLabel,
            NWDWebEditionStyle sStyle,
            bool sRequired,
            string sBootstrapIcon,
            string sDescription,
            string sPlaceholder,
            Type sDataRetriever,
            Type sListType,
            bool sUseAsTitle = false,
            bool sUseAsSortBy = false,
            bool sUseAsDescription = false)
        {
            Infos.Label = sLabel;
            Infos.Style = sStyle;
            Infos.Required = sRequired;
            Infos.BootstrapIcon = sBootstrapIcon;
            Infos.Description = sDescription;
            Infos.Placeholder = sPlaceholder;
            Infos.UseAsSortBy = sUseAsSortBy;
            Infos.UseAsColumn = sUseAsDescription;
            Infos.IsPrimaryColumn = sUseAsTitle;
            Infos.ListType = sListType; 
            Infos.DataRetriever = Activator.CreateInstance(sDataRetriever) as IDataRetriever;
        }
    }
}