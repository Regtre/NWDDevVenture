using NWDUnityEditor.Tools;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace NWDUnityEditor.Windows
{
    [Serializable]
    public class NWDModelWindow : NWDUnityEditorWindowData
    {
        static private Type LastAskedType = null;

        [SerializeField]
        private string AssemblySerialization = null;
        [SerializeField]
        private string TypeSerialization = null;
        private List<Type> DisplayedType = null;

        public static void ShowEditionWindowForType(Type sType, ulong sReference = 0)
        {
            LastAskedType = sType;
            NWDModelWindow rReturn = CreateWindow<NWDModelWindow>();

            rReturn.ShowMe();
            rReturn.Focus();

            rReturn.SelectTab(sType);

            rReturn.TabSelectionSelected.Select(sReference, rReturn);

            rReturn.ScrollToSelection();
        }

        private List<Type> GetDisplayedTypeList ()
        {
            if (DisplayedType == null)
            {
                if (!string.IsNullOrEmpty(TypeSerialization))
                {
                    foreach (Assembly tAssembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        if (tAssembly.FullName != AssemblySerialization)
                        {
                            continue;
                        }

                        Type tType = tAssembly.GetType(TypeSerialization);
                        if (tType != null)
                        {
                            DisplayedType = new List<Type> { tType };
                            break;
                        }
                    }
                }
                else if (LastAskedType != null)
                {
                    DisplayedType = new List<Type> { LastAskedType };
                    AssemblySerialization = LastAskedType.Assembly.FullName;
                    TypeSerialization = LastAskedType.FullName;
                }
            }

            return DisplayedType;
        }

        public override NWDUnityEditorMultiGUIContent ReturnTitle()
        {
            return NWDUnityEditorMultiGUIContent.NewTitle<NWDModelWindow>(GetDisplayedTypeList()[0].Name + " data edition window");
        }

        protected override List<Type> TypeList()
        {
            return GetDisplayedTypeList();
        }
    }
}