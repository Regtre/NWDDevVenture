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
    public class NWDItemWindow : NWDUnityEditorWindowData
    {
        [MenuItem(NWDMenus.K_NETWORKEDDATAMODELS + "Items", false, NWDMenus.K_MODULES_MANAGEMENT_INDEX + 1000)]
        public static void MenuMethod()
        {
            NWDUnityEditorWindowData rReturn = CreateWindow<NWDItemWindow>();
            rReturn.ShowMe();
            rReturn.Focus();
        }

        public override NWDUnityEditorMultiGUIContent ReturnTitle()
        {
            return NWDUnityEditorMultiGUIContent.NewTitle<NWDItemWindow>("Items Window");
        }

        protected override List<Type> TypeList()
        {
            return new List<Type>() {
                typeof(NWDItem),
            };
        }
    }
}