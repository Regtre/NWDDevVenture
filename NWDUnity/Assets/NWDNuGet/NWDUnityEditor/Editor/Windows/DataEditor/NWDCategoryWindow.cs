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
    public class NWDCategoryWindow : NWDUnityEditorWindowData
    {
        [MenuItem(NWDMenus.K_NETWORKEDDATAMODELS + "Category", false, NWDMenus.K_MODULES_MANAGEMENT_INDEX + 1100)]
        public static void MenuMethod()
        {
            NWDUnityEditorWindowData rReturn = CreateWindow<NWDCategoryWindow>();
            rReturn.ShowMe();
            rReturn.Focus();
        }

        public override NWDUnityEditorMultiGUIContent ReturnTitle()
        {
            return NWDUnityEditorMultiGUIContent.NewTitle<NWDCategoryWindow>("Category Window");
        }

        protected override List<Type> TypeList()
        {
            return new List<Type>() {
                typeof(NWDCategory),
            };
        }
    }
}