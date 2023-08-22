using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDShortPropertyDrawer : NWDPropertyDrawer
    {
        public NWDShortPropertyDrawer() : base()
        {
        }
        public NWDShortPropertyDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            sProperty.SetValue((short)EditorGUI.IntField(sPosition, sDisplayName, (short)sProperty.GetValue()));
        }
    }
}
