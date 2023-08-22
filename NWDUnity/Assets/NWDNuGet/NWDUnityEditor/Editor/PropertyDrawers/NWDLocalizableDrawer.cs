using NWDFoundation.Localization;
using NWDFoundation.Models;
using NWDUnityEditor.Attributes;
using NWDUnityEditor.Tools;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.PropertyDrawers
{
    [NWDCustomPropertyDrawer(typeof(NWDLocalizable), true)]
    public class NWDLocalizableDrawer : NWDPropertyDrawer
    {
        static private Dictionary<Type, NWDDictionaryPropertyDrawer> ValuesDrawers = new Dictionary<Type, NWDDictionaryPropertyDrawer>();
        static private NWDStringPropertyDrawer ContextDrawer = null;

        public NWDModelTypeInformation Info = null;
        public NWDSerializedProperty ContextProperty;
        public NWDSerializedProperty ValuesProperty;
        public Type ReferenceType = typeof(NWDAsset);
        public ulong Value;
        public bool Foldout = false;

        public override float GetPropertyHeight(NWDSerializedProperty sProperty, string sDisplayName)
        {
            Init(sProperty);

            INWDSubModel sValue = (INWDSubModel)sProperty.GetValue();
            ContextProperty.PropertyObject = sValue;
            ValuesProperty.PropertyObject = sValue;

            float rResult = EditorGUIUtility.singleLineHeight;

            if (Foldout)
            {
                rResult += EditorGUIUtility.standardVerticalSpacing;
                rResult += ContextDrawer.GetPropertyHeight(ContextProperty, ContextProperty.Name);
                rResult += EditorGUIUtility.standardVerticalSpacing;
                rResult += ValuesDrawers[ReferenceType].GetPropertyHeight(ValuesProperty, ValuesProperty.Name);
            }

            //return DictionaryDrawers[ReferenceType].GetPropertyHeight(ReferenceProperty, sDisplayName);
            return rResult;
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string slabel)
        {
            Init(sProperty);

            sPosition.height = EditorGUIUtility.singleLineHeight;

            Foldout = EditorGUI.Foldout(sPosition, Foldout, slabel);

            if (Foldout)
            {
                INWDSubModel sValue = (INWDSubModel)sProperty.GetValue();
                ContextProperty.PropertyObject = sValue;
                ValuesProperty.PropertyObject = sValue;

                sPosition.xMin += NWDGUI.kFieldIndent;

                sPosition.height = ContextDrawer.GetPropertyHeight(ContextProperty, ContextProperty.Name);
                sPosition.y += EditorGUIUtility.standardVerticalSpacing + sPosition.height;

                ContextDrawer.OnGUI(sPosition, ContextProperty, ContextProperty.Name);

                sPosition.y += EditorGUIUtility.standardVerticalSpacing + sPosition.height;
                sPosition.height = ValuesDrawers[ReferenceType].GetPropertyHeight(ValuesProperty, ValuesProperty.Name);

                ValuesDrawers[ReferenceType].OnGUI(sPosition, ValuesProperty, ValuesProperty.Name);

                sProperty.SetValue(sValue);
            }
        }

        private void Init (NWDSerializedProperty sProperty)
        {
            if (Info == null)
            {
                if (sProperty.PropertyType.IsGenericType)
                {
                    Info = NWDModelType.ModelTypeInformationByType[sProperty.PropertyType.GetGenericTypeDefinition()];
                    ContextProperty = Info.Parent.Fields[0].GetProperty();
                    ValuesProperty = Info.Parent.Fields[1].GetProperty();
                }
                else
                {
                    Info = NWDModelType.ModelTypeInformationByType[sProperty.PropertyType];
                    ContextProperty = Info.Fields[0].GetProperty();
                    ValuesProperty = Info.Fields[1].GetProperty();
                }

                Type tType = sProperty.PropertyType;
                while (tType != null)
                {
                    if (tType.IsGenericType && tType.GetGenericTypeDefinition() == typeof(NWDLocalizable<>))
                    {
                        ReferenceType = tType.GetGenericArguments()[0];
                    }
                    tType = tType.BaseType;
                }

                ContextDrawer = new NWDStringPropertyDrawer();
                if (ReferenceType != null && !ValuesDrawers.ContainsKey(ReferenceType))
                {
                    NWDAssetDrawer tAssetDrawer = new NWDAssetDrawer();
                    tAssetDrawer.SetObjectType (ReferenceType);
                    NWDDictionaryPropertyDrawer tDrawer = new NWDDictionaryPropertyDrawer(typeof(NWDLangage), new NWDLangageDrawer(), tAssetDrawer, false);
                    ValuesDrawers.Add(ReferenceType, tDrawer);
                }
            }
        }
    }
}
