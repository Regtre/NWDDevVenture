using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDBytePropertyDrawer : NWDPropertyDrawer
    {
        public NWDBytePropertyDrawer() : base()
        {
        }
        public NWDBytePropertyDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            sProperty.SetValue((byte)EditorGUI.IntField(sPosition, sDisplayName, (byte)sProperty.GetValue()));
        }
    }
}
