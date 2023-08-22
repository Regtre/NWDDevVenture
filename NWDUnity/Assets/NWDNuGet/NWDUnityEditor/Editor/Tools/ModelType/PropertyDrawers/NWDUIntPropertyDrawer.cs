using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDUIntPropertyDrawer : NWDPropertyDrawer
    {
        public NWDUIntPropertyDrawer() : base()
        {
        }
        public NWDUIntPropertyDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            sProperty.SetValue((uint)EditorGUI.LongField(sPosition, sDisplayName, (uint)sProperty.GetValue()));
        }
    }
}
