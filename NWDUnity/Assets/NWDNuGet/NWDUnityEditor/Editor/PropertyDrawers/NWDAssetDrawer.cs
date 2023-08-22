using NWDFoundation.Models;
using NWDUnityEditor.Attributes;
using NWDUnityEditor.Tools;
using NWDUnityShared.Tools;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace NWDUnityEditor.PropertyDrawers
{
    [NWDCustomPropertyDrawer(typeof(NWDAsset), true)]
    public class NWDAssetDrawer : NWDPropertyDrawer
    {
        private const int kModeSize = 10;

        private Type AssetType;
        private UnityEngine.Object Asset;
        private UnityEngine.Object NewAsset;
        private NWDAssetUtility Utility;

        static public GUIContent[] RessourceTypes = new GUIContent[]
        {
            new GUIContent("!", "Invalid asset!\nThis asset is not a Ressource nor an Addressable."),
            new GUIContent("R", "Ressources"),
            new GUIContent("A", "Addressables")
        };

        public NWDAssetDrawer() : base()
        {
        }

        public NWDAssetDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string slabel)
        {
            Init(sProperty);
            NWDAsset sValue = (NWDAsset)(sProperty.GetValue());

            // Extract information
            Utility = NWDAssetUtility.Deserialize(sValue.UnityAsset);

            // Find Asset
            Asset = FindFromUtility(Utility);

            // Display asset
            sPosition.xMax -= kModeSize + EditorGUIUtility.standardVerticalSpacing;
            NewAsset = EditorGUI.ObjectField(sPosition, slabel, Asset, AssetType, false);

            sPosition.xMin = sPosition.xMax + EditorGUIUtility.standardVerticalSpacing;
            sPosition.width = kModeSize;
            EditorGUI.LabelField(sPosition, RessourceTypes[Utility?.Mode ?? 0]);

            if (NewAsset != Asset)
            {
                // Process asset information
                Utility = ProcessAsset(NewAsset, Utility);

                // Save changes
                sValue.UnityAsset = NWDAssetUtility.Serialize(Utility);
            }
            sProperty.SetValue(sValue);
        }

        public UnityEngine.Object FindFromUtility(NWDAssetUtility sUtility)
        {
            if (sUtility == null)
            {
                return null;
            }

            string tPath = AssetDatabase.GUIDToAssetPath(sUtility.GUID);
            UnityEngine.Object[] tAssets = AssetDatabase.LoadAllAssetsAtPath(tPath);
            for (int i = 0; i < tAssets.Length; i++)
            {
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(tAssets[i], out string sGUID, out long sLocalId) && sLocalId == sUtility.Index)
                {
                    return tAssets[i];
                }
            }

            return AssetDatabase.LoadMainAssetAtPath(tPath);
        }

        public NWDAssetUtility ProcessAsset(UnityEngine.Object sAsset, NWDAssetUtility sUtility)
        {
            if (sAsset == null)
            {
                return null;
            }

            if (sUtility == null)
            {
                sUtility = new NWDAssetUtility();
            }

            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(sAsset, out string tGUID, out long tLocalId))
            {
                sUtility.GUID = tGUID;
                sUtility.Index = tLocalId;
            }
            else
            {
                string tPath = AssetDatabase.GetAssetPath(sAsset);
                sUtility.GUID = AssetDatabase.AssetPathToGUID(tPath);
                sUtility.Index = 0;
            }

            if (GetAddressableKey (sAsset, sUtility) || GetRessourceKey(sAsset, sUtility))
            {
                return sUtility;
            }

            return sUtility;
        }

        private bool GetAddressableKey (UnityEngine.Object sAsset, NWDAssetUtility sUtility)
        {
            List<AddressableAssetGroup> tAGs = AddressableAssetSettingsDefaultObject.Settings?.groups;
            bool tSubAsset = false;

            if (tAGs == null)
            {
                return false;
            }

            UnityEngine.Object sActualAsset = sAsset;
            if (AssetDatabase.IsSubAsset(sAsset))
            {
                sActualAsset = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GetAssetPath(sAsset));
                tSubAsset = true;
            }

            foreach (AddressableAssetGroup tAG in tAGs)
            {
                if (tAG.entries == null)
                {
                    continue;
                }

                foreach (AddressableAssetEntry tEntry in tAG.entries)
                {
                    if (tEntry.TargetAsset == sActualAsset)
                    {
                        sUtility.Key = tEntry.address;
                        if (tSubAsset)
                        {
                            sUtility.Key += "[" + sAsset.name + "]";
                        }
                        sUtility.Mode = 2;
                        return true;
                    }
                }
            }

            return false;
        }

        public bool GetRessourceKey (UnityEngine.Object sAsset, NWDAssetUtility sUtility)
        {
            string tPath = AssetDatabase.GetAssetPath (sAsset);

            if (tPath.Contains("/Resources/"))
            {
                sUtility.Key = tPath;
                sUtility.Mode = 1;
                return true;
            }

            sUtility.Key = tPath;
            sUtility.Mode = 0;
            return false;
        }

        private void Init(NWDSerializedProperty sProperty)
        {
            if (AssetType == null)
            {
                AssetType = typeof(UnityEngine.Object);
                Type tType = sProperty.PropertyType;
                while (tType != null)
                {
                    if (tType.IsGenericType && tType.GetGenericTypeDefinition() == typeof(NWDAsset<>))
                    {
                        SetObjectType(tType);
                        break;
                    }
                    tType = tType.BaseType;
                }
            }
        }


        public void SetObjectType (Type sType)
        {
            if (sType.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                AssetType = sType;
            }
            else if (sType.IsSubclassOf(typeof(NWDAsset)))
            {
                sType = sType.GetGenericArguments()[0];
            }

            if (sType.GetInterface(nameof(INWDAsset)) != null)
            {
                AssetType = NWDAssetUtility.FacadeAssetToUnityAsset[sType];
            }
            else
            {
                AssetType = typeof(UnityEngine.Object);
            }
        }
    }
}
