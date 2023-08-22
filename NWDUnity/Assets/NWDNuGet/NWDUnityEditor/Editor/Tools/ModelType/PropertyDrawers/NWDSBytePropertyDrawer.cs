using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDSBytePropertyDrawer : NWDPropertyDrawer
    {
        public NWDSBytePropertyDrawer() : base()
        {
        }
        public NWDSBytePropertyDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            sProperty.SetValue((sbyte)EditorGUI.IntField(sPosition, sDisplayName, (sbyte)sProperty.GetValue()));
        }
    }
}
