using NWDFoundation.Models;
using NWDUnityEditor.Attributes;
using NWDUnityEditor.Tools;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.PropertyDrawers
{
    [NWDCustomPropertyDrawer(typeof(NWDReferences), true)]
    public class NWDReferencesDrawer : NWDPropertyDrawer
    {
        static private Dictionary<Type, NWDListPropertyDrawer> ListDrawers = new Dictionary<Type, NWDListPropertyDrawer>();

        public NWDModelTypeInformation Info = null;
        public NWDSerializedProperty ReferenceProperty;
        public Type ReferenceType = typeof(NWDStudioData);
        public ulong Value;

        public override float GetPropertyHeight(NWDSerializedProperty sProperty, string sDisplayName)
        {
            Init(sProperty);

            INWDSubModel sValue = (INWDSubModel)sProperty.GetValue();

            if (sValue == null)
            {
                sValue = (INWDSubModel)sProperty.PropertyType.GetConstructor(new Type[0]).Invoke(new object[0]);
            }

            ReferenceProperty.PropertyObject = sValue;

            return ListDrawers[ReferenceType].GetPropertyHeight(ReferenceProperty, sDisplayName);
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string slabel)
        {
            Init(sProperty);

            INWDSubModel sValue = (INWDSubModel)sProperty.GetValue();

            if (sValue == null)
            {
                sValue = (INWDSubModel)sProperty.PropertyType.GetConstructor(new Type[0]).Invoke(new object[0]);
            }

            ReferenceProperty.PropertyObject = sValue;

            EditorGUI.BeginChangeCheck();

            ListDrawers[ReferenceType].OnGUI(sPosition, ReferenceProperty, slabel);

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
                    if (tType.IsGenericType && tType.GetGenericTypeDefinition() == typeof(NWDReferences<>))
                    {
                        ReferenceType = tType.GetGenericArguments()[0];
                    }
                    tType = tType.BaseType;
                }

                if (ReferenceType != null && !ListDrawers.ContainsKey(ReferenceType))
                {
                    ListDrawers.Add(ReferenceType, new NWDListPropertyDrawer(ReferenceProperty.PropertyInfo, new NWDReferencePropertyDrawer(ReferenceType)));
                }
            }
        }
    }
}
