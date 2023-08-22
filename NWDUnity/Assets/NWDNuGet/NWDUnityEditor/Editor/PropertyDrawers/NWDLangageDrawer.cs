using NWDFoundation.Localization;
using NWDUnityEditor.Tools;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.PropertyDrawers
{
    public class NWDLangageDrawer : NWDPropertyDrawer
    {
        static private string[] Langages = null;
        static private int[] Values = null;

        public NWDLangageDrawer() : base()
        {
            InitData();
        }

        public NWDLangageDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
            InitData();
        }

        private void InitData ()
        {
            if (Langages == null)
            {
                Langages = NWDLocalizationISO.LanguageDico.Select(x => x.Name).ToArray();
                Values = NWDLocalizationISO.LanguageDico.Select(x => (int)x.Langage).ToArray();
            }
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string slabel)
        {
            NWDLangage tValue = (NWDLangage)sProperty.GetValue();
            tValue = (NWDLangage)EditorGUI.IntPopup(sPosition, slabel, (int)tValue, Langages, Values);
            sProperty.SetValue(tValue);
        }
    }
}
