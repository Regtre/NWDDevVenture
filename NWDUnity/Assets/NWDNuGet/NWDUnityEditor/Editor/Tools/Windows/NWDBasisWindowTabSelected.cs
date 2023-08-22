using NWDEditor;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Managers;
using NWDUnityEditor.Models;
using NWDUnityEditor.Requestors;
using NWDUnityEditor.Services;
using NWDUnityEditor.Windows;
using NWDUnityShared.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    [Serializable]
    public class NWDBasisWindowTabSelected
    {
        public string ClassName;
        public string InternalTitle;
        public string InternalDescription;
        public NWDTag Tag = NWDTag.NoTag;
        public NWDCheck CheckList = NWDCheck.None;
        public Int32 CustomTag = 0;
        public Int64 CustomCheck = 0;
        public bool EnableDatas = true;
        public bool DisableDatas = true;
        public bool TrashedDatas = true;
        public bool CorruptedDatas = true;
        public int FilteredTypeIndex = -1;
        public string[] TypeFilter;
        public bool Locked = false;
        public NWDBasisEditorDatasSortType SortBy = NWDBasisEditorDatasSortType.ByInternalKeyAscendant;
        [NonSerialized]
        public List<NWDVirtualDataGUI> Datas = new List<NWDVirtualDataGUI>();  // TODO: Get reed of this !

        private List<NWDMetaData> MetaDataList = new List<NWDMetaData>();
        public ulong LastUpdate = 0;
        private NWDGUIAsyncOperation<NWDMetaDataResult> creationTask = null;
        private NWDGUIAsyncOperation<NWDMetaDataResult> lockTask = null;
        private NWDGUIAsyncOperation<NWDMetaDataResult> unlockTask = null;

        public bool CanFilterChildren => TypeFilter.Length > 2;

        public NWDGUIAsyncOperation<NWDMetaDataResult> CreationTask
        {
            get
            {
                lock (this)
                {
                    return creationTask;
                }
            }
            set
            {
                lock (this)
                {
                    creationTask = value;
                }
            }
        }
        public NWDGUIAsyncOperation<NWDMetaDataResult> LockTask
        {
            get
            {
                lock (this)
                {
                    return lockTask;
                }
            }
            set
            {
                lock (this)
                {
                    lockTask = value;
                }
            }
        }
        public NWDGUIAsyncOperation<NWDMetaDataResult> UnlockTask
        {
            get
            {
                lock (this)
                {
                    return unlockTask;
                }
            }
            set
            {
                lock (this)
                {
                    unlockTask = value;
                }
            }
        }

        [NonSerialized]
        public List<NWDVirtualDataGUI> Listed = new List<NWDVirtualDataGUI>();

        [NonSerialized]
        public NWDVirtualDataGUI ObjectSelected;

        public NWDBasisWindowTabSelected (NWDUnityEditorWindowData sWindow, Type sType)
        {
            ClassName = sType.AssemblyQualifiedName;

            List<string> tTypeFilter = new List<string> { "All types" };
            tTypeFilter.AddRange(NWDModelType.ChildrenTypesByType[sType].Select(x => x.ClassType.Name).ToList());
            TypeFilter = tTypeFilter.ToArray();

            FilteredTypeIndex = -1;

            UpdateDataCache(sWindow);
        }

        public void ClearAll()
        {
            InternalTitle = string.Empty;
            InternalDescription = string.Empty;
            Tag = NWDTag.NoTag;
            CheckList = NWDCheck.None;
            CustomTag = 0;
            CustomCheck = 0;
            FilteredTypeIndex = -1;
            EnableDatas = true;
            DisableDatas = true;
            TrashedDatas = true;
            CorruptedDatas = true;
            Locked = false;
            SortBy = NWDBasisEditorDatasSortType.ByInternalKeyAscendant;
            Datas = new List<NWDVirtualDataGUI>();
            Listed = new List<NWDVirtualDataGUI>();
            ObjectSelected = null;
        }

        public void Select(ulong sReference, NWDUnityEditorWindowData sWindow)
        {
            NWDVirtualDataGUI tObject = Listed.FirstOrDefault(x => x.MetaData.Reference == sReference);

            if (tObject != null)
            {
                Select(tObject, sWindow);
            }
        }

        public void Select(NWDVirtualDataGUI sObject, NWDUnityEditorWindowData sWindow)
        {
            if (ObjectSelected != null)
            {
                UnlockForEdition(sWindow, ObjectSelected.MetaData);
            }
            ObjectSelected = sObject;
            if (sObject != null)
            {
                LockForEdition(sWindow, ObjectSelected.MetaData);
            }
            GUI.FocusControl(null);
        }

        public NWDVirtualDataGUI DupplicateData(NWDVirtualDataGUI sObject, NWDUnityEditorWindowBasis sWindow)
        {
            NWDVirtualDataGUI rReturn = new NWDVirtualDataGUI();
            rReturn.MetaData.Reference = NWDRandom.UnsignedLongNumeric(32);
            rReturn.MetaData.Title = sObject.MetaData.Title + " - copy";
            rReturn.MetaData.Description = sObject.MetaData.Description;
            rReturn.MetaData.ClassName = sObject.MetaData.ClassName;
            return rReturn;
        }

        public Type RepresentationOfType()
        {
            return Type.GetType(ClassName);
        }

        public void SelectAll()
        {
            foreach (NWDVirtualDataGUI tObject in Listed)
            {
                tObject.Selected = true;
            }
        }

        public void DeselectAll()
        {
            foreach (NWDVirtualDataGUI tObject in Listed)
            {
                tObject.Selected = false;
            }
        }

        public void InverseSelection()
        {
            foreach (NWDVirtualDataGUI tObject in Listed)
            {
                tObject.Selected = !tObject.Selected;
            }
        }

        //public void DupplicateSelection(NWDUnityEditorWindowBasis sWindow)
        //{
        //    NWDTypeGUIInformations tInformation = NWDTypeGUIInformations.GetForType(RepresentationOfType());
        //    List<NWDVirtualDataGUI> tListed = new List<NWDVirtualDataGUI>();
        //    foreach (NWDVirtualDataGUI tObject in Listed)
        //    {
        //        if (tObject.Selected == true)
        //        {
        //            tListed.Add(tObject);
        //        }
        //    }
        //    //foreach (NWDVirtualDataGUI tObject in tListed)
        //    //{
        //    //    DupplicateData(tInformation, tObject, sWindow);
        //    //}
        //}

        public void ApplyFilter(NWDUnityEditorWindowData sWindow)
        {
            Type tType = null;
            //Debug.Log("ApplyFilter()");
            //bool tFilterFound = false;
            int tNeed = 0;
            if (string.IsNullOrEmpty(InternalTitle) == false)
            {
                //Debug.Log("filtre with InternnalKey " + InternnalKey);
                tNeed++;
            }
            if (string.IsNullOrEmpty(InternalDescription) == false)
            {
                //Debug.Log("filtre with InternnalDescription " + InternnalDescription);
                tNeed++;
            }
            if (Tag != NWDTag.NoTag)
            {
                tNeed++;
            }
            if (CheckList != NWDCheck.None)
            {
                tNeed++;
            }
            if (CustomTag != 0)
            {
                tNeed++;
            }
            if (CustomCheck != 0)
            {
                tNeed++;
            }
            if (Locked == true)
            {
                tNeed++;
            }
            if (FilteredTypeIndex >= 0)
            {
                tType = RepresentationOfType();
                tNeed++;
            }
            Listed.Clear();

            if (sWindow.DisplayMode() == NWDUnityEditorWindowData.DataDisplayMode.SelectionMode)
            {
                NWDVirtualDataGUI tNoneVirtualData = new NWDVirtualDataGUI();
                NWDMetaData tNoneMetaData = new NWDMetaData();
                tNoneMetaData.Title = "None";

                tNoneVirtualData.SetMetaData(tNoneMetaData);

                Listed.Add(tNoneVirtualData);
            }

            foreach (NWDVirtualDataGUI tObject in Datas)
            {
                int tScore = 0;
                if (string.IsNullOrEmpty(InternalTitle) == false)
                {
                    if (tObject.MetaData.Title.Contains(InternalTitle) == true)
                    {
                        tScore++;
                    }
                }
                if (string.IsNullOrEmpty(InternalDescription) == false)
                {
                    if (tObject.MetaData.Description.Contains(InternalDescription) == true)
                    {
                        tScore++;
                    }
                }
                //if (Tag != NWDTag.NoTag)
                //{
                //    if (tObject.VirtualData.Tag == Tag)
                //    {
                //        tScore++;
                //    }
                //}
                //if (CheckList != NWDCheck.None)
                //{
                //    if ((tObject.VirtualData.Check & CheckList) != NWDCheck.None)
                //    {
                //        tScore++;
                //    }
                //}
                //if (CustomTag != 0)
                //{
                //    if (tObject.VirtualData.CustomTag == CustomTag)
                //    {
                //        tScore++;
                //    }
                //}
                //if (CustomCheck != 0)
                //{
                //    if ((tObject.VirtualData.CustomCheck & CustomCheck) != 0)
                //    {
                //        tScore++;
                //    }
                //}

                if (Locked == true)
                {
                    if (string.IsNullOrEmpty(tObject.MetaData.LockerName) == false)
                    {
                        tScore++;
                    }
                }
                if (tScore == tNeed)
                {
                    Listed.Add(tObject);
                }
                else
                {
                    // force deselect unused object 
                    tObject.Selected = false;
                }

                if(FilteredTypeIndex >= 0)
                {
                    if (tObject.MetaData.ClassName == NWDModelType.ChildrenTypesByType[tType][FilteredTypeIndex].ClassType.AssemblyQualifiedName)
                    {
                        Listed.Add(tObject);
                    }
                }
            }
            SortEditorTableDatas();
        }

        public void RemoveFilter()
        {
            InternalTitle = string.Empty;
            InternalDescription = string.Empty;
            Tag = NWDTag.NoTag;
            SortEditorTableDatas();
        }

        public void SortEditorTableDatas(NWDBasisEditorDatasSortType sSortType)
        {
            SortBy = sSortType;
            SortEditorTableDatas();
        }

        public void SortEditorTableDatas()
        {
            switch (SortBy)
            {
                case NWDBasisEditorDatasSortType.None:
                    {
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByInternalKeyAscendant:
                    {
                        Listed.Sort((x, y) => TitleComparer(x, y));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByInternalKeyDescendant:
                    {
                        Listed.Sort((x, y) => TitleComparer(x, y, true));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByReferenceAscendant:
                    {
                        Listed.Sort((x, y) => ReferenceComparer(x, y));
                    }
                    break;
                case NWDBasisEditorDatasSortType.ByReferenceDescendant:
                    {
                        Listed.Sort((x, y) => ReferenceComparer(x, y, true));
                    }
                    break;
                case NWDBasisEditorDatasSortType.BySelectAscendant:
                    {
                        Listed.Sort((x, y) => SelectedComparer(x, y));
                    }
                    break;
                case NWDBasisEditorDatasSortType.BySelectDescendant:
                    {
                        Listed.Sort((x, y) => SelectedComparer(x, y, true));
                    }
                    break;
            }
        }

        private int TitleComparer (NWDVirtualDataGUI sBefore, NWDVirtualDataGUI sAfter, bool sDescending = false)
        {
            int rResult;
            if (sBefore.MetaData.Reference == sAfter.MetaData.Reference)
            {
                rResult = 0;
            }
            else if (sBefore.MetaData.Reference == 0)
            {
                rResult = -1;
            }
            else if (sAfter.MetaData.Reference == 0)
            {
                rResult = 1;
            }
            else
            {
                rResult = string.Compare(sBefore.MetaData.Title, sAfter.MetaData.Title, StringComparison.OrdinalIgnoreCase);
                if (sDescending)
                {
                    rResult *= -1;
                }
            }
            return rResult;
        }

        private int ReferenceComparer(NWDVirtualDataGUI sBefore, NWDVirtualDataGUI sAfter, bool sDescending = false)
        {
            int rResult;
            if (sBefore.MetaData.Reference == sAfter.MetaData.Reference)
            {
                rResult = 0;
            }
            else if (sBefore.MetaData.Reference == 0)
            {
                rResult = -1;
            }
            else if (sAfter.MetaData.Reference == 0)
            {
                rResult = 1;
            }
            else
            {
                rResult = sBefore.MetaData.Reference.CompareTo(sAfter.MetaData.Reference);
                if (sDescending)
                {
                    rResult *= -1;
                }
            }
            return rResult;
        }

        private int SelectedComparer(NWDVirtualDataGUI sBefore, NWDVirtualDataGUI sAfter, bool sDescending = false)
        {
            int rResult;
            if (sBefore.MetaData.Reference == sAfter.MetaData.Reference)
            {
                rResult = 0;
            }
            else if (sBefore.MetaData.Reference == 0)
            {
                rResult = -1;
            }
            else if (sAfter.MetaData.Reference == 0)
            {
                rResult = 1;
            }
            else
            {
                rResult = sBefore.Selected.CompareTo(sAfter.Selected);
                if (sDescending)
                {
                    rResult *= -1;
                }
            }
            return rResult;
        }

        public void LockForEdition(NWDUnityEditorWindowData sWindow, NWDMetaData sMetaData)
        {
            LockTask = NWDUnityEngineEditor.Instance.GetDataManager().LockMetaData(sWindow, sMetaData);
            LockTask.OnDone += OnLockDone;
        }

        public void UnlockForEdition(NWDUnityEditorWindowData sWindow, NWDMetaData sMetaData)
        {
            UnlockTask = NWDUnityEngineEditor.Instance.GetDataManager().UnlockMetaData(sWindow, sMetaData);
            UnlockTask.OnDone += OnUnlockDone;
        }

        private void OnLockDone(NWDAsyncOperation sOperation)
        {
            UpdateDataCache(LockTask.Result.Window);
            LockTask = null;
        }

        private void OnUnlockDone(NWDAsyncOperation sOperation)
        {
            UpdateDataCache(UnlockTask.Result.Window);
            UnlockTask = null;
        }

        internal void NewData(NWDUnityEditorWindowData sWindow, int sTypeIndex)
        {
            Select(null, sWindow);
            Type tType = RepresentationOfType();
            if (sTypeIndex < 0)
            {
                sTypeIndex = 0;
            }
            tType = NWDModelType.ChildrenTypesByType[tType][sTypeIndex].ClassType;
            NWDTypeGUIInformation tInformation = NWDTypeGUIInformation.GetForType(tType);

            CreationTask = NWDUnityEngineEditor.Instance.GetDataManager().CreateMetaData(sWindow, tType);
            CreationTask.OnDone += OnCreationDone;
        }

        private void OnCreationDone (NWDAsyncOperation sOperation)
        {
            UpdateDataCache(CreationTask.Result.Window);
            lock (this)
            {
                ObjectSelected = Datas.FirstOrDefault(x => x.MetaData.Reference == CreationTask.Result.MetaData.Reference);
                if (ObjectSelected != null)
                {
                    LockForEdition(CreationTask.Result.Window, ObjectSelected.MetaData);
                    CreationTask.Result.Window.ScrollToSelection();
                }
                creationTask = null;
            }
        }

        public void UpdateDataCache(NWDUnityEditorWindowData sWindow)
        {
            Type tType = RepresentationOfType();

            string[] tTypes = null;

            if (CanFilterChildren)
            {
                if (FilteredTypeIndex < 0)
                {
                    tTypes = NWDModelType.ChildrenTypesByType[tType].Select(x => x.ClassType.AssemblyQualifiedName).ToArray();
                }
                else
                {
                    tTypes = new string[] { NWDModelType.ChildrenTypesByType[tType][FilteredTypeIndex].ClassType.AssemblyQualifiedName };
                }
            }
            else
            {
                tTypes = new string[] { NWDModelType.ChildrenTypesByType[tType][0].ClassType.AssemblyQualifiedName };
            }

            if (LastUpdate == 0)
            {
                LastUpdate = NWDUnityEngineEditor.Instance.GetDataManager().GetForClass(ref MetaDataList, tTypes);
            }
            else
            {
                LastUpdate = NWDUnityEngineEditor.Instance.GetDataManager().GetForClass(ref MetaDataList, tTypes, LastUpdate);
            }

            //NWDLogger.Debug(nameof(NWDBasisWindowTabSelected) + " " + nameof(AddData) + " (sDatas count :  " + sDatas.Length + "/ Datas count: " + Datas.Count + ")");
            NWDTypeGUIInformation tInformation = NWDTypeGUIInformation.GetForType(tType);
            string tUniqueName = NWDMetaDataEditorService.UniqueLockerName();
            if (MetaDataList != null)
            {
                foreach (NWDMetaData tDataDB in MetaDataList)
                {
                    NWDVirtualDataGUI tReturn = Datas.FirstOrDefault(x => x.MetaData.Reference == tDataDB.Reference);
                    if (tReturn == null)
                    {
                        tReturn = new NWDVirtualDataGUI();
                        Datas.Add(tReturn);
                        tReturn.SetMetaData(tDataDB);
                        tReturn.GenerateGuiContent(tInformation.tClass);
                    }
                    else
                    {
                        // if data already locked, waiting for update to update :-)  
                        if (tDataDB.LockerName != tUniqueName)
                        {
                            tReturn.SetMetaData(tDataDB);
                        }
                        else
                        {
                            tReturn.MetaData.IsLocked = tDataDB.IsLocked;
                            tReturn.MetaData.LockerName = tDataDB.LockerName;
                            tReturn.MetaData.LockLimit = tDataDB.LockLimit;
                        }
                        tReturn.SetMetaData(tDataDB);
                    }
                }
                ApplyFilter(sWindow);
            }
            sWindow.RepaintMe();
        }
    }
}
