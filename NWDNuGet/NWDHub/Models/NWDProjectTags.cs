using NWDFoundation.Models;
using NWDFoundation.WebEdition.Attributes;
using NWDFoundation.WebEdition.Enums;

namespace NWDHub.Models
{
    [Serializable]
    public class NWDProjectTags : NWDProjectSubObject
    {
        [NWDWebPropertyDescription("", NWDWebEditionStyle.Text,false, "", "", "", false)]
        public string NoTag { set; get; } = NWDTag.NoTag.ToString();
        [NWDWebPropertyDescription("", NWDWebEditionStyle.Text,false, "", "", "", false)]
        public string TagOne { set; get; } = NWDTag.TagOne.ToString();
        [NWDWebPropertyDescription("", NWDWebEditionStyle.Text,false, "", "", "", false)]
        public string TagTwo { set; get; } = NWDTag.TagTwo.ToString();
        [NWDWebPropertyDescription("", NWDWebEditionStyle.Text,false, "", "", "", false)]
        public string TagThree { set; get; } = NWDTag.TagThree.ToString();
        [NWDWebPropertyDescription("", NWDWebEditionStyle.Text,false, "", "", "", false)]
        public string TagFour { set; get; } = NWDTag.TagFour.ToString();
        [NWDWebPropertyDescription("", NWDWebEditionStyle.Text,false, "", "", "", false)]
        public string TagFive { set; get; } = NWDTag.TagFive.ToString();
        [NWDWebPropertyDescription("", NWDWebEditionStyle.Text,false, "", "", "", false)]
        public string TagSix { set; get; } = NWDTag.TagSix.ToString();
        [NWDWebPropertyDescription("", NWDWebEditionStyle.Text,false, "", "", "", false)]
        public string TagSeven { set; get; } = NWDTag.TagSeven.ToString();
        [NWDWebPropertyDescription("", NWDWebEditionStyle.Text,false, "", "", "", false)]
        public string TagHeight { set; get; } = NWDTag.TagHeight.ToString();
        [NWDWebPropertyDescription("", NWDWebEditionStyle.Text,false, "", "", "", false)]
        public string TagNine { set; get; } = NWDTag.TagNine.ToString();
        [NWDWebPropertyDescription("", NWDWebEditionStyle.Text,false, "", "", "", false)]
        public string TagTen { set; get; } = NWDTag.TagTen.ToString();
        public NWDProjectTags()
        {
        }
        
        public NWDProjectTags(NWDProject sProject)
        {
            this.AssociateToProject(sProject);
        }
    }
}