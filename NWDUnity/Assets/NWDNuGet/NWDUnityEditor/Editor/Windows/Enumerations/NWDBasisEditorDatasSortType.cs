using System;

namespace NWDUnityEditor.Windows
{
    [Serializable]
    public enum NWDBasisEditorDatasSortType
    {
        None,
        BySelectAscendant,
        BySelectDescendant,
        ByPrefabAscendant,
        ByPrefabDescendant,
        ByInternalKeyAscendant,
        ByInternalKeyDescendant,
        BySyncAscendant,
        BySyncDescendant,
        ByDevSyncAscendant,
        ByDevSyncDescendant,
        ByPreprodSyncAscendant,
        ByPreprodSyncDescendant,
        ByProdSyncAscendant,
        ByProdSyncDescendant,
        ByStatutAscendant,
        ByStatutDescendant,
        ByReferenceAscendant,
        ByReferenceDescendant,
        ByChecklistAscendant,
        ByChecklistDescendant,
        ByModelAscendant,
        ByModelDescendant,
        ByAgeAscendant,
        ByAgeDescendant,
    }
}