using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDUnityEditor.Attributes;
using NWDUnityEditor.Tools;
using NWDUnityShared.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.PropertyDrawers
{
    [NWDCustomPropertyDrawer(typeof(NWDVector2), true)]
    public class NWDVector2Drawer : NWDPropertyDrawer
    {
        public NWDVector2Drawer() : base()
        {
        }

        public NWDVector2Drawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override float GetPropertyHeight(NWDSerializedProperty sProperty, string sDisplayName)
        {
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string slabel)
        {
            NWDVector2 sValue = (NWDVector2)sProperty.GetValue();

            Vector2 tData = new Vector2(sValue.X, sValue.Y);

            tData = EditorGUI.Vector2Field(sPosition, slabel, tData);

            sValue.X = tData.x;
            sValue.Y = tData.y;

            sProperty.SetValue(sValue);
        }
    }
}
