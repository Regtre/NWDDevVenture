using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDIntPropertyDrawer : NWDPropertyDrawer
    {
        public NWDIntPropertyDrawer() : base()
        {
        }
        public NWDIntPropertyDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            sProperty.SetValue(EditorGUI.IntField(sPosition, sDisplayName, (int)sProperty.GetValue()));
        }
    }
}
