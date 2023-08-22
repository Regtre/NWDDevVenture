using System.Reflection;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public interface INWDPropertyDrawer
    {
        public void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName = null);
        public float GetPropertyHeight(NWDSerializedProperty sProperty, string sDisplayName);
        public void SetPropertyInfo(PropertyInfo sPropertyInfo);
        public NWDSerializedProperty GetProperty();
        public bool IsValidUniqueKeyValue(NWDSerializedProperty sProperty);
        public bool IsFoldableProperty { get; }
    }
}

