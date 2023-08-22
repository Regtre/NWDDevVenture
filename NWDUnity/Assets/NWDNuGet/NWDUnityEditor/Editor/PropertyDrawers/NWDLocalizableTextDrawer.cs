using NWDFoundation.Localization;
using NWDFoundation.Models;
using NWDUnityEditor.Attributes;
using NWDUnityEditor.Tools;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.PropertyDrawers
{
    [NWDCustomPropertyDrawer(typeof(NWDLocalizableText), true)]
    public class NWDLocalizableTextDrawer : NWDPropertyDrawer
    {
        static private NWDDictionaryPropertyDrawer ValuesDrawer = null;
        static private NWDStringPropertyDrawer ContextDrawer = null;

        public NWDModelTypeInformation Info = null;
        public NWDSerializedProperty ContextProperty;
        public NWDSerializedProperty ValuesProperty;
        public ulong Value;
        public bool Foldout = false;

        public override float GetPropertyHeight(NWDSerializedProperty sProperty, string sDisplayName)
        {
            Init(sProperty);

            INWDSubModel sValue = (INWDSubModel)sProperty.GetValue();
            ContextProperty.PropertyObject = sValue;
            ValuesProperty.PropertyObject = sValue;

            float rResult = EditorGUIUtility.singleLineHeight;

            if (Foldout)
            {
                rResult += EditorGUIUtility.standardVerticalSpacing;
                rResult += ContextDrawer.GetPropertyHeight(ContextProperty, ContextProperty.Name);
                rResult += EditorGUIUtility.standardVerticalSpacing;
                rResult += ValuesDrawer.GetPropertyHeight(ValuesProperty, ValuesProperty.Name);
            }

            return rResult;
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string slabel)
        {
            Init(sProperty);

            sPosition.height = EditorGUIUtility.singleLineHeight;

            Foldout = EditorGUI.Foldout(sPosition, Foldout, slabel);

            if (Foldout)
            {
                INWDSubModel sValue = (INWDSubModel)sProperty.GetValue();
                ContextProperty.PropertyObject = sValue;
                ValuesProperty.PropertyObject = sValue;

                sPosition.xMin += NWDGUI.kFieldIndent;

                sPosition.height = ContextDrawer.GetPropertyHeight(ContextProperty, ContextProperty.Name);
                sPosition.y += EditorGUIUtility.standardVerticalSpacing + sPosition.height;

                ContextDrawer.OnGUI(sPosition, ContextProperty, ContextProperty.Name);

                sPosition.y += EditorGUIUtility.standardVerticalSpacing + sPosition.height;
                sPosition.height = ValuesDrawer.GetPropertyHeight(ValuesProperty, ValuesProperty.Name);

                ValuesDrawer.OnGUI(sPosition, ValuesProperty, ValuesProperty.Name);

                sProperty.SetValue(sValue);
            }
        }

        private void Init (NWDSerializedProperty sProperty)
        {
            if (Info == null)
            {
                Info = NWDModelType.ModelTypeInformationByType[sProperty.PropertyType];
                ContextProperty = Info.Fields[0].GetProperty();
                ValuesProperty = Info.Fields[1].GetProperty();

                ContextDrawer = new NWDStringPropertyDrawer();
                ValuesDrawer = new NWDDictionaryPropertyDrawer(typeof(NWDLangage), new NWDLangageDrawer(), new NWDStringPropertyDrawer(), false);
            }
        }
    }
}
