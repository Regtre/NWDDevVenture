using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public abstract class NWDPropertyDrawer : INWDPropertyDrawer
    {
        protected NWDSerializedProperty Property;

        public virtual bool IsFoldableProperty => false;

        public NWDPropertyDrawer()
        {
            Property = null;
        }
        public NWDPropertyDrawer (PropertyInfo sPropertyInfo)
        {
            SetPropertyInfo (sPropertyInfo);
        }

        public abstract void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName);

        public virtual void SetPropertyInfo (PropertyInfo sPropertyInfo)
        {
            if (Property == null)
            {
                Property = new NWDSerializedProperty(sPropertyInfo);
            }
        }

        public virtual float GetPropertyHeight(NWDSerializedProperty sProperty, string sDisplayName)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public NWDSerializedProperty GetProperty()
        {
            return Property;
        }

        public virtual bool IsValidUniqueKeyValue (NWDSerializedProperty sProperty)
        {
            return true;
        }
    }

}