using NWDFoundation.Models;
using NWDUnityEditor.Attributes;
using NWDUnityEditor.Tools;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.PropertyDrawers
{
    [NWDCustomPropertyDrawer(typeof(NWDReference), true)]
    public class NWDReferenceDrawer : NWDPropertyDrawer
    {
        static private Dictionary<NWDSerializedProperty, bool> Foldout = new Dictionary<NWDSerializedProperty, bool>();

        static private bool GetFoldout(NWDSerializedProperty sProperty)
        {
            if (sProperty == null)
            {
                return false;
            }

            if (!Foldout.ContainsKey(sProperty))
            {
                Foldout.Add(sProperty, false);
            }

            return Foldout[sProperty];
        }

        public NWDModelTypeInformation Info = null;
        public NWDSerializedProperty ReferenceProperty;
        public Type ReferenceType = typeof(NWDStudioData);
        public ulong Value;

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string slabel)
        {
            Init(sProperty);

            INWDSubModel sValue = (INWDSubModel)sProperty.GetValue();

            if (sValue == null)
            {
                sValue = (INWDSubModel)sProperty.PropertyType.GetConstructor(new Type[0]).Invoke(new object[0]); // TODO: automate this ?
            }

            ReferenceProperty.PropertyObject = sValue;

            bool tFoldout = GetFoldout(sProperty); // TO DO: should work by property path > Not ready yet in NWDSerializedProperty!
            EditorGUI.BeginChangeCheck();
            Value = NWDGUI.DataField(sPosition, new GUIContent(slabel), ReferenceType, (ulong)ReferenceProperty.GetValue(), ref tFoldout);
            if (EditorGUI.EndChangeCheck())
            {
                ReferenceProperty.SetValue(Value);
            }
            Foldout[sProperty] = tFoldout;

            sProperty.SetValue(sValue);
        }

        private void Init (NWDSerializedProperty sProperty)
        {
            if (Info == null)
            {
                if (sProperty.PropertyType.IsGenericType) // Use helper methods to do this ?
                {
                    Info = NWDModelType.ModelTypeInformationByType[sProperty.PropertyType.GetGenericTypeDefinition()];
                    ReferenceProperty = Info.Parent.Fields[0].GetProperty(); // TO DO: Use the property name instead !
                }
                else
                {
                    Info = NWDModelType.ModelTypeInformationByType[sProperty.PropertyType];
                    ReferenceProperty = Info.Fields[0].GetProperty();
                }

                Type tType = sProperty.PropertyType;
                while (tType != null)
                {
                    if (tType.IsGenericType && tType.GetGenericTypeDefinition() == typeof(NWDReference<>))
                    {
                        ReferenceType = tType.GetGenericArguments()[0]; // TODO: This should break the loop ?
                    }
                    tType = tType.BaseType;
                }
            }
        }
    }
}
