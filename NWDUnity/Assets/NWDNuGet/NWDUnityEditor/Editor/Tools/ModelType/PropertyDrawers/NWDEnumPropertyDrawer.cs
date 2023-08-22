using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    public class NWDEnumPropertyDrawer : NWDPropertyDrawer
    {
        bool IsFlag = false;

        public NWDEnumPropertyDrawer (Type sType) : base()
        {
            FlagsAttribute tAttribute = sType.GetCustomAttribute<FlagsAttribute>(false);
            IsFlag = tAttribute != null;
        }

        public NWDEnumPropertyDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
            FlagsAttribute tAttribute = sPropertyInfo.PropertyType.GetCustomAttribute<FlagsAttribute>(false);
            IsFlag = tAttribute != null;
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string sDisplayName)
        {
            Enum tValue = (Enum)sProperty.GetValue();
            if (IsFlag)
            {
                tValue = EditorGUI.EnumFlagsField(sPosition, sDisplayName, tValue);
            }
            else
            {
                tValue = EditorGUI.EnumPopup(sPosition, sDisplayName, tValue);
            }
            sProperty.SetValue(tValue);
        }
    }
}
