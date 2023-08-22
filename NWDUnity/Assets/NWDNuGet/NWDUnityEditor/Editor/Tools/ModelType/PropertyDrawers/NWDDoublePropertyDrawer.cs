using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDDoublePropertyDrawer : NWDPropertyDrawer
    {
        public NWDDoublePropertyDrawer() : base()
        {
        }
        public NWDDoublePropertyDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            sProperty.SetValue(EditorGUI.DoubleField(sPosition, sDisplayName, (double)sProperty.GetValue()));
        }
    }
}
