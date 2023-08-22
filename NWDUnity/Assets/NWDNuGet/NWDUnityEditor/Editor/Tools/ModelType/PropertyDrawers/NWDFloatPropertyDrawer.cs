using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDFloatPropertyDrawer : NWDPropertyDrawer
    {
        public NWDFloatPropertyDrawer() : base()
        {
        }
        public NWDFloatPropertyDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            sProperty.SetValue (EditorGUI.FloatField(sPosition, sDisplayName, (float)sProperty.GetValue()));
        }
    }
}
