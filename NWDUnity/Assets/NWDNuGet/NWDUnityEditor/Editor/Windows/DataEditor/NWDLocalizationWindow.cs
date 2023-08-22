using NWDEditor;
using NWDStandardModels.Models;
using NWDUnityEditor.Menus;
using NWDUnityEditor.Tools;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace NWDUnityEditor.Windows
{
    [Serializable]
    public class NWDLocalizationWindow : NWDUnityEditorWindowData
    {
        [MenuItem(NWDMenus.K_NETWORKEDDATAMODELS + "Localization", false, NWDMenus.K_MODULES_MANAGEMENT_INDEX + 1100)]
        public static void MenuMethod()
        {
            NWDUnityEditorWindowData rReturn = CreateWindow<NWDLocalizationWindow>();
            rReturn.ShowMe();
            rReturn.Focus();
        }

        public override NWDUnityEditorMultiGUIContent ReturnTitle()
        {
            return NWDUnityEditorMultiGUIContent.NewTitle<NWDLocalizationWindow>("Localization Window");
        }

        protected override List<Type> TypeList()
        {
            return new List<Type>() {
                typeof(NWDLocalization),
            };
        }
    }
}