using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDLongPropertyDrawer : NWDPropertyDrawer
    {
        public NWDLongPropertyDrawer() : base()
        {
        }
        public NWDLongPropertyDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            sProperty.SetValue(EditorGUI.LongField(sPosition, sDisplayName, (long)sProperty.GetValue()));
        }
    }
}
