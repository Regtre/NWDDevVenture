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
    [NWDCustomPropertyDrawer(typeof(NWDColor), true)]
    public class NWDColorDrawer : NWDPropertyDrawer
    {
        public NWDColorDrawer() : base()
        {
        }

        public NWDColorDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string slabel)
        {
            NWDColor tValue = (NWDColor)sProperty.GetValue();

            Color tData = new Color(tValue.Red, tValue.Green, tValue.Blue, tValue.Alpha);

            tData = EditorGUI.ColorField(sPosition, slabel, tData);

            tValue.Red = tData.r;
            tValue.Green = tData.g;
            tValue.Blue = tData.b;
            tValue.Alpha = tData.a;

            sProperty.SetValue(tValue);
        }
    }
}
