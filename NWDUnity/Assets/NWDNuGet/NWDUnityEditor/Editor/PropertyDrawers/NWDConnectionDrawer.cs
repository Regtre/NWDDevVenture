using NWDFoundation.Models;
using NWDUnityEditor.Tools;
using NWDUnityShared.Scripts;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(NWDConnection), true)]
    public class NWDConnectionDrawer : PropertyDrawer
    {
        static private Dictionary<string, bool> Foldout = new Dictionary<string, bool>();
        static private bool GetFoldout (SerializedProperty sProperty)
        {
            if (sProperty == null)
            {
                return false;
            }

            if (!Foldout.ContainsKey(sProperty.propertyPath))
            {
                Foldout.Add(sProperty.propertyPath, false);
            }

            return Foldout[sProperty.propertyPath];
        }

        public NWDConnection Connection = null;
        public Type ConnectionType = typeof(NWDStudioData);

        public override float GetPropertyHeight(SerializedProperty sProperty, GUIContent sLabel)
        {
            Init(sProperty);
            float tHeight = EditorGUIUtility.singleLineHeight;
            if (GetFoldout(sProperty))
            {

            }
            return tHeight;
        }

        public override void OnGUI(Rect sPosition, SerializedProperty sProperty, GUIContent slabel)
        {
            Init(sProperty);
            bool tFoldout = GetFoldout(sProperty);
            EditorGUI.BeginChangeCheck();
            Connection.Reference = NWDGUI.DataField(sPosition, slabel, ConnectionType, Connection.Reference, ref tFoldout);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(sProperty.serializedObject.targetObject);

            }
            Foldout[sProperty.propertyPath] = tFoldout;
        }

        private void Init (SerializedProperty sProperty)
        {
            if (Connection == null)
            {
                Connection = fieldInfo.GetValue(sProperty.serializedObject.targetObject) as NWDConnection;

                Type tType = Connection.GetType();
                while (tType != null)
                {
                    if (tType.IsGenericType && tType.GetGenericTypeDefinition() == typeof(NWDConnection<>))
                    {
                        ConnectionType = tType.GetGenericArguments()[0];
                    }
                    tType = tType.BaseType;
                }
            }
        }
    }
}
