using NWDFoundation.Models;
using NWDUnityEditor.Attributes;
using NWDUnityEditor.Tools;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.PropertyDrawers
{
    [NWDCustomPropertyDrawer(typeof(NWDReferencesQuantity), true)]
    public class NWDReferencesQuantityDrawer : NWDPropertyDrawer
    {
        static private Dictionary<Type, NWDDictionaryPropertyDrawer> DictionaryDrawers = new Dictionary<Type, NWDDictionaryPropertyDrawer>();

        public NWDModelTypeInformation Info = null;
        public NWDSerializedProperty ReferenceProperty;
        public Type ReferenceType = typeof(NWDStudioData);
        public ulong Value;

        public override float GetPropertyHeight(NWDSerializedProperty sProperty, string sDisplayName)
        {
            Init(sProperty);

            INWDSubModel sValue = (INWDSubModel)sProperty.GetValue();

            ReferenceProperty.PropertyObject = sValue;

            return DictionaryDrawers[ReferenceType].GetPropertyHeight(ReferenceProperty, sDisplayName);
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string slabel)
        {
            Init(sProperty);

            INWDSubModel sValue = (INWDSubModel)sProperty.GetValue();

            ReferenceProperty.PropertyObject = sValue;

            EditorGUI.BeginChangeCheck();

            DictionaryDrawers[ReferenceType].OnGUI(sPosition, ReferenceProperty, slabel);

            if (EditorGUI.EndChangeCheck())
            {
                //ReferenceProperty.SetValue(Value);
            }
            sProperty.SetValue(sValue);
        }

        private void Init (NWDSerializedProperty sProperty)
        {
            if (Info == null)
            {
                if (sProperty.PropertyType.IsGenericType)
                {
                    Info = NWDModelType.ModelTypeInformationByType[sProperty.PropertyType.GetGenericTypeDefinition()];
                    ReferenceProperty = Info.Parent.Fields[0].GetProperty();
                }
                else
                {
                    Info = NWDModelType.ModelTypeInformationByType[sProperty.PropertyType];
                    ReferenceProperty = Info.Fields[0].GetProperty();
                }


                Type tType = sProperty.PropertyType;
                while (tType != null)
                {
                    if (tType.IsGenericType && tType.GetGenericTypeDefinition() == typeof(NWDReferencesQuantity<>))
                    {
                        ReferenceType = tType.GetGenericArguments()[0];
                    }
                    tType = tType.BaseType;
                }

                if (ReferenceType != null && !DictionaryDrawers.ContainsKey(ReferenceType))
                {
                    NWDDictionaryPropertyDrawer tDrawer = new NWDDictionaryPropertyDrawer(typeof(ulong), new NWDReferencePropertyDrawer(ReferenceType), new NWDLongPropertyDrawer());
                    tDrawer.SetPropertyInfo(ReferenceProperty.PropertyInfo);
                    DictionaryDrawers.Add(ReferenceType, tDrawer);
                }
            }
        }
    }
}
