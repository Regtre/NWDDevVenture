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
    [NWDCustomPropertyDrawer(typeof(NWDVersion), true)]
    public class NWDVersionDrawer : NWDPropertyDrawer
    {
        static private string[] kVersionArray = null;

        static public string[] GetVersions()
        {
            if (kVersionArray == null)
            {
                kVersionArray = Enumerable.Range(0, 100).Select(x => x.ToString("00")).ToArray();
            }
            return kVersionArray;
        }

        public NWDVersionDrawer() : base()
        {
        }

        public NWDVersionDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string slabel)
        {
            NWDVersion sValue = (NWDVersion)sProperty.GetValue();

            sPosition = EditorGUI.PrefixLabel(sPosition, new GUIContent(slabel));

            float tControlWidth = (sPosition.width - EditorGUIUtility.standardVerticalSpacing * 2) / 3;

            sPosition.width = tControlWidth;
            sValue.Major = EditorGUI.Popup(sPosition, sValue.Major, GetVersions());

            sPosition.x += tControlWidth + EditorGUIUtility.standardVerticalSpacing;
            sValue.Minor = EditorGUI.Popup(sPosition, sValue.Minor, GetVersions());

            sPosition.x += tControlWidth + EditorGUIUtility.standardVerticalSpacing;
            sValue.Build = EditorGUI.Popup(sPosition, sValue.Build, GetVersions());

            sProperty.SetValue(sValue);
        }
    }
}
