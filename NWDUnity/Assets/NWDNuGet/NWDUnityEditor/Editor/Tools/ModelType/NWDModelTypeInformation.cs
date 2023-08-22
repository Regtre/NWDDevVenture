using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDModelTypeInformation
    {
        private static Dictionary<Type, bool> FoldoutData = new Dictionary<Type, bool>();
        private static bool GetFoldoutForType (Type type)
        {
            if (!FoldoutData.ContainsKey(type))
            {
                FoldoutData[type] = true;
            }

            return FoldoutData[type];
        }


        public Type Type;
        public NWDModelTypeInformation Parent;
        public INWDPropertyDrawer[] Fields;

        public NWDModelTypeInformation (Type sType, NWDModelTypeInformation sParent, INWDPropertyDrawer[] sFields)
        {
            Type = sType;
            Parent = sParent;
            Fields = sFields;
        }
        public NWDModelTypeInformation(Type sType, INWDPropertyDrawer[] sFields)
        {
            Type = sType;
            Parent = null;
            Fields = sFields;
        }

        public void OnGUI (Rect sPosition, object sObject)
        {
            Rect tPosition = sPosition;
            if (Parent != null)
            {
                tPosition.height = Parent.GetPropertyHeight(sObject);

                Parent.OnGUI (tPosition, sObject);
                tPosition.y += tPosition.height;
            }

            if (Fields.Length == 0)
            {
                return;
            }

            tPosition.y += EditorGUIUtility.standardVerticalSpacing;
            tPosition.height = EditorGUIUtility.singleLineHeight;

            FoldoutData[Type] = EditorGUI.BeginFoldoutHeaderGroup(tPosition, GetFoldoutForType(Type), Type.Name);

            tPosition.y += tPosition.height;
            if (FoldoutData[Type])
            {
                tPosition.xMin += NWDGUI.kFieldIndent;

                for (int i = 0; i < Fields.Length; i++)
                {
                    tPosition.y += EditorGUIUtility.standardVerticalSpacing;

                    NWDSerializedProperty sFieldProperty = Fields[i].GetProperty();
                    sFieldProperty.PropertyObject = sObject;

                    tPosition.height = Fields[i].GetPropertyHeight(sFieldProperty, sFieldProperty.Name);

                    Fields[i].OnGUI(tPosition, sFieldProperty, sFieldProperty.Name);

                    tPosition.y += tPosition.height;
                }
            }

            EditorGUI.EndFoldoutHeaderGroup();
        }

        public float GetPropertyHeight (object sObject)
        {
            float rResult = 0;

            if (Parent != null)
            {
                rResult += Parent.GetPropertyHeight(sObject);
            }

            if (Fields.Length > 0)
            {
                rResult += EditorGUIUtility.singleLineHeight;
                if (GetFoldoutForType(Type) == true)
                {
                    for (int i = 0; i < Fields.Length; i++)
                    {
                        NWDSerializedProperty sFieldProperty = Fields[i].GetProperty();
                        sFieldProperty.PropertyObject = sObject;

                        rResult += EditorGUIUtility.standardVerticalSpacing + Fields[i].GetPropertyHeight(sFieldProperty, sFieldProperty.Name);
                    }
                }
            }
            return rResult;
        }
    }
}
