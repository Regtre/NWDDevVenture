using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NWDFoundation.WebEdition.Enums;
using NWDFoundation.WebEdition.Facades;

namespace NWDFoundation.WebEdition.Models
{
    [Serializable]
    public class NWDWebEditionPropertyDescription
    {
        public string Label = string.Empty;
        public NWDWebEditionStyle Style = NWDWebEditionStyle.Text;
        public string BootstrapIcon = string.Empty;
        public bool Required = false;
        public string Description = string.Empty;
        public string Placeholder = string.Empty;
        public bool UseAsSortBy = false;
        public bool UseAsColumn = false;
        public bool IsPrimaryColumn = false;
        public List<string> DropDownValues = new List<string>();
        public IDataRetriever? DataRetriever;
        public Type? ListType { get; set; }

        public NWDWebEditionPropertyDescription()
        {
        }
        public NWDWebEditionPropertyDescription(
            string sLabel, 
            NWDWebEditionStyle sStyle, 
            bool sRequired,
            string sBootstrapIcon, 
            string sDescription, 
            string sPlaceholder,
            bool sUseAsSortBy = false,
            bool sUseAsDescription = false,
            bool sUseAsTitle = false
            )
        {
            Label = sLabel;
            Style = sStyle;
            Required = sRequired;
            BootstrapIcon = sBootstrapIcon;
            Description = sDescription;
            Placeholder = sPlaceholder;
            UseAsSortBy = sUseAsSortBy;
            UseAsColumn = sUseAsDescription;
            IsPrimaryColumn = sUseAsTitle;
        }
        
        public NWDWebEditionPropertyDescription(
            string sLabel,
            NWDWebEditionStyle sStyle,
            bool sRequired,
            string sBootstrapIcon,
            string sDescription,
            string sPlaceholder,
            Type sDropDownValuesFromType,
            bool sUseAsTitle = false,
            bool sUseAsSortBy = false,
            bool sUseAsDescription = false)
        {
            Label = sLabel;
            Style = sStyle;
            Required = sRequired;
            BootstrapIcon = sBootstrapIcon;
            Description = sDescription;
            Placeholder = sPlaceholder;
            UseAsSortBy = sUseAsSortBy;
            UseAsColumn = sUseAsDescription;
            IsPrimaryColumn = sUseAsTitle;
            DropDownValues = Enum.GetNames(sDropDownValuesFromType).ToList();
        }
        public NWDWebEditionPropertyDescription(string sLabel,
            NWDWebEditionStyle sStyle,
            bool sRequired,
            string sBootstrapIcon,
            string sDescription,
            string sPlaceholder,
            IEnumerable<string> sDropDownValues,
            bool sUseAsTitle = false,
            bool sUseAsSortBy = false,
            bool sUseAsDescription = false)
        {
            Label = sLabel;
            Style = sStyle;
            Required = sRequired;
            BootstrapIcon = sBootstrapIcon;
            Description = sDescription;
            Placeholder = sPlaceholder;
            UseAsSortBy = sUseAsSortBy;
            UseAsColumn = sUseAsDescription;
            IsPrimaryColumn = sUseAsTitle;
            DropDownValues = sDropDownValues.ToList();
        }
        public NWDWebEditionPropertyDescription(string sLabel,
            NWDWebEditionStyle sStyle,
            bool sRequired,
            string sBootstrapIcon,
            string sDescription,
            string sPlaceholder,
            IDataRetriever? sDataRetriever,
            Type sListType,
            bool sUseAsTitle = false,
            bool sUseAsSortBy = false,
            bool sUseAsDescription = false)
        {
            Label = sLabel;
            Style = sStyle;
            Required = sRequired;
            BootstrapIcon = sBootstrapIcon;
            Description = sDescription;
            Placeholder = sPlaceholder;
            UseAsSortBy = sUseAsSortBy;
            UseAsColumn = sUseAsDescription;
            IsPrimaryColumn = sUseAsTitle;
            ListType = sListType; 
            DataRetriever = sDataRetriever;
        }

    }
    
}