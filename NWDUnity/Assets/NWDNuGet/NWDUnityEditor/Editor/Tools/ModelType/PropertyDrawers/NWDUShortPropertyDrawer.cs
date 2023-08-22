using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDUShortPropertyDrawer : NWDPropertyDrawer
    {
        public NWDUShortPropertyDrawer() : base()
        {
        }
        public NWDUShortPropertyDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            sProperty.SetValue((ushort)EditorGUI.IntField(sPosition, sDisplayName, (ushort)sProperty.GetValue()));
        }
    }
}
