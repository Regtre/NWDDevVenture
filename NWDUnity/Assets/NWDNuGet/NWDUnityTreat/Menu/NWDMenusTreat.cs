using NWDUnityEditor.Menus;
using NWDUnityEditor.Windows;
using UnityEditor;
using UnityEngine;

namespace NWDUnityTreat.Menu
{
    static public class NWDMenusTreat
    {

        [MenuItem(NWDMenus.K_NETWORKEDDATA + NWDMenus.K_ENVIRONMENT + "Services and Treats", false, NWDMenus.K_EDITOR_INDEX + 21)]
        public static void NWDEnvironmentServices_Menu()
        {
            NWDServiceConfiguration.SharedInstanceFocus();
        }
    }
}

