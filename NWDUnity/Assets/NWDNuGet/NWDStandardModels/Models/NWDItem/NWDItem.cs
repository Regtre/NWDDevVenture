using NWDFoundation.Models;
using System.Collections.Generic;

namespace NWDStandardModels.Models
{
    public class NWDItem : NWDStudioData
    {
        public NWDLocalizableText Name { get; set; }
        public NWDLocalizableText PluralName { get; set; }
        public NWDLocalizableText SubName { get; set; }
        public NWDLocalizableText Description { get; set; }

        //public NWDItemNotification FirstAcquisitionNotification { get; set; }
        //public NWDItemNotification AddItemNotification { get; set; }
        //public NWDItemNotification RemoveItemNotification { get; set; }
        //public NWDItemNotification NoMoreItemNotification { get; set; }

        public float Rarity { get; set; }
        public bool HiddenInGame { get; set; }
        public bool Stackable { get; set; }
        public bool Usable { get; set; }

        public NWDReferencesQuantity<NWDItem> ItemExtensionQuantity { get; set; }
        //public NWDReferences<NWDParameter> ParameterList { get; set; }

        public NWDAsset<INWDSpriteAsset>[] Sprites { get; set; }
        public NWDAsset<INWDTextureAsset>[] Textures { get; set; }
        public NWDColor[] Colors { get; set; }
        public NWDAsset<INWDPrefabAsset>[] Prefabs { get; set; }

        public NWDAsset<INWDPrefabAsset> EffectPrefab { get; set; }

        public string Json { get; set; }
        public string KeyValues { get; set; }
    }
}