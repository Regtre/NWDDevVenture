using NWDUnityEditor.Attributes;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDULongPropertyDrawer : NWDPropertyDrawer
    {
        public NWDULongPropertyDrawer() : base()
        {
        }
        public NWDULongPropertyDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            EditorGUI.BeginChangeCheck();
            string tTmpValue = sProperty.GetValue().ToString();
            tTmpValue = EditorGUI.TextField(sPosition, sDisplayName, tTmpValue);
            if (EditorGUI.EndChangeCheck())
            {
                if (ulong.TryParse(tTmpValue, out ulong rResult))
                {
                    sProperty.SetValue(rResult);
                }
            }
        }
    }
}
