using NWDFoundation.Models;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDObjectPropertyDrawer : NWDPropertyDrawer
    {
        static private Dictionary<NWDSerializedProperty, bool> Foldouts = new Dictionary<NWDSerializedProperty, bool>();
        static public bool GetFoldout (NWDSerializedProperty sProperty)
        {
            bool rResult = false;
            if (sProperty != null && !Foldouts.TryGetValue(sProperty, out rResult))
            {
                rResult = false;
                Foldouts.Add(sProperty, rResult);
            }
            return rResult;
        }

        public NWDModelTypeInformation Info = null;

        public override float GetPropertyHeight(NWDSerializedProperty sProperty, string sDisplayName)
        {
            float tHeight = EditorGUIUtility.singleLineHeight;

            if (GetFoldout(sProperty))
            {
                for (int i = 0; i < Info.Fields.Length; i++)
                {
                    NWDSerializedProperty sFieldProperty = Info.Fields[i].GetProperty();
                    sFieldProperty.PropertyObject = sProperty.GetValue();

                    tHeight += Info.Fields[i].GetPropertyHeight(sFieldProperty, sFieldProperty.Name) + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return tHeight;
        }
        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            if (Info == null)
            {
                Info = NWDModelType.ModelTypeInformationByType[sProperty.PropertyType];
            }

            INWDSubModel sValue = (INWDSubModel)sProperty.GetValue();

            if (sValue == null)
            {
                sValue = (INWDSubModel)sProperty.PropertyType.GetConstructor(new Type[0]).Invoke(new object[0]);
            }

            sPosition.height = EditorGUIUtility.singleLineHeight;

            Foldouts[sProperty] = EditorGUI.Foldout(sPosition, GetFoldout(sProperty), sDisplayName);

            if (Foldouts[sProperty])
            {
                sPosition.xMin += NWDGUI.kFieldIndent;
                sPosition.y += sPosition.height;

                for (int i = 0; i < Info.Fields.Length; i++)
                {
                    sPosition.y += EditorGUIUtility.standardVerticalSpacing;

                    NWDSerializedProperty sFieldProperty = Info.Fields[i].GetProperty();
                    sFieldProperty.PropertyObject = sValue;

                    sPosition.height = Info.Fields[i].GetPropertyHeight(sFieldProperty, sFieldProperty.Name);
                    Info.Fields[i].OnGUI(sPosition, sFieldProperty, sFieldProperty.Name);
                    sPosition.y += sPosition.height;
                }
            }
            sProperty.SetValue(sValue);
        }
    }
}
