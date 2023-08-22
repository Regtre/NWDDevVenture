using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDBoolPropertyDrawer : NWDPropertyDrawer
    {
        public NWDBoolPropertyDrawer() : base()
        {
        }
        public NWDBoolPropertyDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            sProperty.SetValue(EditorGUI.Toggle(sPosition, sDisplayName, (bool)sProperty.GetValue()));
        }
    }
}
