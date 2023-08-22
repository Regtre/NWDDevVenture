using NWDFoundation.Logger;
using NWDUnityEditor.Tools;
using NWDUnityShared.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NWDUnityEditor.Windows
{
    public class NWDDataPicker : NWDUnityEditorWindowData
    {
        static private Type FilterType = null;
        static private Dictionary<int, ulong> PickedReferences = new Dictionary<int, ulong>();

        static public bool TryGetPickedReference (int sControllId, out ulong sReference)
        {
            sReference = 0;

            if (PickedReferences.TryGetValue(sControllId, out sReference))
            {
                PickedReferences.Remove(sControllId);
                return true;
            }
            return false;
        }

        static public void ShowWindow(int sControllId, Type sFilterType)
        {
            FilterType = sFilterType;
            NWDDataPicker rReturn = CreateWindow<NWDDataPicker>();

            rReturn.TargetControllId = sControllId;

            rReturn.ShowUtility();
            rReturn.Focus();

            rReturn.DefineTab();

            rReturn.SelectTab(sFilterType);
        }

        public int TargetControllId = 0;

        public override NWDUnityEditorMultiGUIContent ReturnTitle()
        {
            return NWDUnityEditorMultiGUIContent.NewTitle<NWDDataPicker>("Select a " + FilterType.Name);
        }

        protected override List<Type> TypeList()
        {
            return new List<Type>() { FilterType };
        }

        public override DataDisplayMode DisplayMode()
        {
            return DataDisplayMode.SelectionMode;
        }

        public override void SelectVirtualData(NWDVirtualDataGUI sRow)
        {
            ulong tReference = 0;
            if (sRow != null)
            {
                tReference = sRow.MetaData.Reference;
            }
            if (PickedReferences.ContainsKey(TargetControllId))
            {
                // This shouldn't be
                NWDLogger.Error("A value already exist for controll id: " + TargetControllId + "!\nMake sure you are not pushing more controlls than you are poping.");
            }
            else
            {
                PickedReferences.Add(TargetControllId, tReference);
            }
            CloseAfterGUI();
        }

        public override void OnPreventGUI_InEditorMode()
        {
            Rect tRect = position;
            tRect.yMin = kDataSplitHeaderSize;
            EditorFiltersInformations();
            CalculateListDisplayData(tRect);
            EditorTableHeader();
            EditorTableRows(tRect);
        }

        private void OnLostFocus()
        {
            CloseAfterGUI();
        }
    }
}
