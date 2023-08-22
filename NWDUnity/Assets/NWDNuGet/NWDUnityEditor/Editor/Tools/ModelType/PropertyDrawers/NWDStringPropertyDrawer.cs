using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDStringPropertyDrawer : NWDPropertyDrawer
    {
        public NWDStringPropertyDrawer() : base()
        {
        }
        public NWDStringPropertyDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            string sValue = (string)sProperty.GetValue();
            if (sValue == null)
            {
                sValue = string.Empty;
            }

            if (sValue.Contains("\n"))
            {
                sValue = EditorGUI.TextArea(sPosition, sDisplayName, sValue);
            }
            else
            {
                sValue = EditorGUI.TextField(sPosition, sDisplayName, sValue);
            }
            sProperty.SetValue(sValue);
        }
    }
}
