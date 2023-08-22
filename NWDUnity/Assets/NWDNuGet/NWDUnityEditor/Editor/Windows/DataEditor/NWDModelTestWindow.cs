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
    public class NWDModelTestWindow : NWDUnityEditorWindowData
    {
        [MenuItem(NWDMenus.K_NETWORKEDDATAMODELS + "Tests", false, NWDMenus.K_MODULES_MANAGEMENT_INDEX + 9999)]
        public static void MenuMethod()
        {
            NWDUnityEditorWindowData rReturn = CreateWindow<NWDModelTestWindow>();
            rReturn.ShowMe();
            rReturn.Focus();
        }

        public override NWDUnityEditorMultiGUIContent ReturnTitle()
        {
            return NWDUnityEditorMultiGUIContent.NewTitle<NWDModelTestWindow>("Models Tests Window");
        }

        protected override List<Type> TypeList()
        {
            return new List<Type>() {
                typeof(NWDTestStudioData),
                typeof(NWDInheritanceTestStudioData)
            };
        }
    }
}