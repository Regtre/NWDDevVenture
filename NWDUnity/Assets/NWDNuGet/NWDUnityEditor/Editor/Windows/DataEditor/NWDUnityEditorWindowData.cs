using NWDEditor;
using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Tools;
using NWDUnityShared.Enumerations;
using NWDUnityShared.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static NWDUnityEditor.Tools.NWDSplitArea;

namespace NWDUnityEditor.Windows
{
    [Serializable]
    public abstract class NWDUnityEditorWindowData : NWDUnityEditorWindowBasis
    {
        public static GUIContent ListHeaderTitle = new GUIContent("Title", "Sort by title");
        public static int _kTabSelected = 0;
        protected const float kScrollControllSize = 13;
        protected const float kDataSplitHeaderSize = 180;

        public enum DataDisplayMode
        {
            EditionMode = 0,
            SelectionMode = 1,
        }

        private struct ListDisplayData
        {
            public ListDisplayData (float sLineBaseHeight, int sTotalLineAmount, float sScrollViewHeight, float sScrollBarPosition)
            {
                BaseLineHeight = sLineBaseHeight;
                LineAmount = sTotalLineAmount;
                LineHeight = BaseLineHeight;
                TotalHeight = LineAmount * LineHeight;

                if (sScrollBarPosition > TotalHeight - sScrollViewHeight)
                {
                    sScrollBarPosition = TotalHeight - sScrollViewHeight;
                }

                if (sScrollBarPosition < 0)
                {
                    VisibleLineFirstIndex = 0;
                }
                else
                {
                    VisibleLineFirstIndex = (int)(sScrollBarPosition / LineHeight);
                }
                float tVisibleDiff = sScrollBarPosition - VisibleLineFirstIndex * LineHeight;
                VisibleLineAmount = Mathf.CeilToInt((sScrollViewHeight - tVisibleDiff) / LineHeight) + 1;
                if (tVisibleDiff != 0)
                {
                    VisibleLineAmount++;
                }
                if (VisibleLineAmount + VisibleLineFirstIndex > LineAmount)
                {
                    VisibleLineAmount -= VisibleLineAmount + VisibleLineFirstIndex - LineAmount;
                }
            }

            public float BaseLineHeight;
            public float LineHeight;
            public int LineAmount;
            public int VisibleLineAmount;
            public int VisibleLineFirstIndex;
            public float TotalHeight;

            public float EmptySpaceBefore => VisibleLineFirstIndex * LineHeight;
            public float EmptySpaceAfter => (LineAmount - VisibleLineLastIndex) * LineHeight;
            public float VisibleLineLastIndex
            {
                get
                {
                    int rResult = VisibleLineFirstIndex + VisibleLineAmount;
                    if (rResult >= LineAmount)
                    {
                        rResult = LineAmount - 1;
                    }
                    return rResult;
                }
            }
        }

        public NWDSplitArea SplitArea = NWDSplitArea.NewArea(NWDSplitDirection.Vertical);
        public NWDSplitArea DataSplitArea = NWDSplitArea.NewArea(NWDSplitDirection.Vertical);
        public NWDBasisHelperPanel mPanelSelected = NWDBasisHelperPanel.Data;

        public Rect CurrentArea;
        public AreaOffset DataTrackDisplayOffset = null;

        public Vector2 ScrollPositionTable = Vector2.zero;
        public Vector2 ScrollPositionInfos = Vector2.zero;
        public Vector2 ScrollPositionAction = Vector2.zero;
        public Vector2 ScrollPositionData = Vector2.zero;

        public int tLarg = 0;
        public GUIContent[] mTabContentList;
        public GUIContent[] mPanelContentList;
        public List<NWDBasisWindowTabSelected> TabSelection = new List<NWDBasisWindowTabSelected>();

        int TabsTotalWidthExpected = 0;
        int TabWidthMax = 0;
        public int TabSelected = 0;
        public int Counter = 0;
        private bool ShowCreationWindow = false;

        private float TableHeightEstimateTop = 0;
        private float TableHeightEstimateBottom = 0;
        private Rect ScroolArea = Rect.zero;

        private ListDisplayData ListData;

        private NWDGUIAsyncOperation MetaDataSync = null;

        public NWDBasisWindowTabSelected TabSelectionSelected { get; private set; }

        public virtual DataDisplayMode DisplayMode ()
        {
            return DataDisplayMode.EditionMode;
        }

        //[NonSerialized]
        //private NWDEnvironmentUnityEditor EnvSelected;

        [NonSerialized]
        NWDTypeGUIInformation tInformation;

        [NonSerialized]
        static private Dictionary<NWDBasisHelperPanel, NWDUnityEditorMultiGUIContent> kPanelGuiContents = new Dictionary<NWDBasisHelperPanel, NWDUnityEditorMultiGUIContent>();

        protected abstract List<Type> TypeList();
        static GUIContent GetGUIContent(NWDBasisHelperPanel sPanel)
        {
            GUIContent rReturn = null;
            if (kPanelGuiContents.ContainsKey(sPanel) == true)
            {
                rReturn = kPanelGuiContents[sPanel].GetContent();
            }
            else
            {
                NWDUnityEditorMultiGUIContent tReturn = NWDUnityEditorMultiGUIContent.NewContent("NWDWindowPanel" + sPanel.ToString(), sPanel.ToString());
                kPanelGuiContents.Add(sPanel, tReturn);
                rReturn = tReturn.GetContent();
            }
            return rReturn;
        }

        public override string TutorialLink(string sLink = "")
        {
            return NWDUnityEngineEditor.Instance.GetConfig().WebEditorURL() + "/table-manager/";
        }

        public virtual void SelectTab(Type tType)
        {
            if (tType == null)
            {
                NWDLogger.Warning("tType is null");
            }
            if (TypeList().Contains(tType))
            {
                TabSelected = TypeList().IndexOf(tType);
                foreach (NWDBasisWindowTabSelected tTab in TabSelection)
                {
                    if (tTab.ClassName == tType.AssemblyQualifiedName)
                    {
                        TabSelectionSelected = tTab;
                        ScrollPositionTable.y = 0;
                        break;
                    }
                }
            }
            if (TabSelectionSelected == null)
            {
                NWDLogger.Warning("TabSelectionSelected is null");
                foreach (NWDBasisWindowTabSelected tTab in TabSelection)
                {
                    NWDLogger.Warning("tTab define is for type " + tTab.ClassName);
                }
            }
            tInformation = NWDTypeGUIInformation.GetForType(tType);
            RepaintMe();
            RemoveFieldFocus();
            //if (TabSelectionSelected != null)
            //{
            //    TabSelectionSelected.GetLastData(tInformation, this);
            //}
        }

        public void SelectTab(int sTabindex)
        {
            if (sTabindex < 0)
            {
                NWDLogger.Warning("Failed to find " + sTabindex);
            }
            if (sTabindex >= mTabContentList.Length)
            {
                sTabindex = 0;
            }
            List<Type> tTypeList = TypeList();
            if (tTypeList.Count > 0)
            {
                Type tType = TypeList()[sTabindex];
                SelectTab(tType);
            }
        }

        public void DefineTab()
        {
            TabsTotalWidthExpected = 0;
            TabWidthMax = 0;
            int tCounter = 0;
            List<GUIContent> tTabContentList = new List<GUIContent>();
            // check all type 
            foreach (Type tType in TypeList())
            {
                NWDModelType tTypeImplement = NWDModelType.GetForType(tType);
                if (tTypeImplement != null)
                {
                    if (TabWidthMax < tTypeImplement.MenuName.Length)
                    {
                        TabWidthMax = tTypeImplement.MenuName.Length;
                    }
                    //tTabContentList.Add(tTypeImplement.ClassMenuNameContent().GetContent());
                    tTabContentList.Add(new GUIContent(tType.Name));
                    tCounter++;
                    bool tFind = false;
                    foreach (NWDBasisWindowTabSelected tTab in TabSelection)
                    {
                        if (tTab.ClassName == tType.AssemblyQualifiedName)
                        {
                            tFind = true;
                            break;
                        }
                    }
                    if (tFind == false)
                    {
                        NWDBasisWindowTabSelected tNewATb = new NWDBasisWindowTabSelected(this, tType);
                        TabSelection.Add(tNewATb);
                    }
                }
                else
                {
                    NWDLogger.Warning("tTypeImplement is null for  " + tType.Name);
                }

            }

            TabsTotalWidthExpected = TabWidthMax * tCounter * 8;
            mTabContentList = tTabContentList.ToArray();

            List<GUIContent> tPanelContentList = new List<GUIContent>();
            foreach (string tEnumName in Enum.GetNames(typeof(NWDBasisHelperPanel)))
            {
                NWDBasisHelperPanel tValue = (NWDBasisHelperPanel)Enum.Parse(typeof(NWDBasisHelperPanel), tEnumName);
                tPanelContentList.Add(GetGUIContent(tValue));
            }
            mPanelContentList = tPanelContentList.ToArray();

            SelectTab(TabSelected);
        }

        public override void OnEnableFromConstructor()
        {
            //Debug.LogWarning(nameof(NWDUnityEditorWindowDatas) + " " + nameof(OnEnableFromConstructor) + " ()");
            NWDVirtualDataManager.AddWindow(this);
            NormalizeWidth = 1400;
            NormalizeHeight = 1000;
            SplitArea.MinAreaOne = 600;
            SplitArea.MinAreaTwo = 300;
            SplitArea.Split = 0.75F;
            DataSplitArea.MinAreaOne = 400;
            DataSplitArea.MinAreaTwo = 198;
            DataSplitArea.HeaderSize = kDataSplitHeaderSize;
            DataSplitArea.Split = 0.75f;
            foreach (NWDBasisWindowTabSelected tTab in TabSelection)
            {
                tTab.ClearAll();
            }
            OnEnableFromEverywhere();
        }

        public override void OnEnableFromSerialization()
        {
            //Debug.LogWarning(nameof(NWDUnityEditorWindowDatas) + " " + nameof(OnEnableFromSerialization) + " ()");
            NWDVirtualDataManager.AddWindow(this);
            foreach (NWDBasisWindowTabSelected tTab in TabSelection)
            {
                tTab.ClearAll();
            }
            OnEnableFromEverywhere();
        }

        void OnEnableFromEverywhere()
        {
            //Debug.LogWarning(nameof(NWDUnityEditorWindowDatas) +" "+nameof(OnEnableFromEverywhere) + " ()");
            SplitArea.ResizeSplit(this);

            DefineTab();
            if (DisplayMode() == DataDisplayMode.EditionMode)
            {
                foreach (NWDBasisWindowTabSelected tTab in TabSelection)
                {
                    foreach (NWDVirtualDataGUI tObject in tTab.Datas)
                    {
                        if (tObject.IsLockForEdition())
                        {
                            tTab.UnlockForEdition(this, tObject.MetaData);
                        }
                    }
                    tTab.ApplyFilter(this);
                    tTab.UpdateDataCache(this);
                }
            }
        }

        public override void OnDisableWindow()
        {
            //Debug.LogWarning(nameof(NWDUnityEditorWindowDatas) + " " + nameof(OnDisableWindow) + " ()");
            NWDVirtualDataManager.RemoveWindow(this);
        }

        public override void OnPreventGUI_InEditorMode()
        {
            EditorGUI.BeginDisabledGroup(NWDUnityEngineEditor.Instance.GetDataManager().State <= NWDDataManagerState.Updating);
            //NWDBenchmark.Start();
            NWDGUI.LoadStyles();

            // draw bar top
            EditorGUI.DrawRect(new Rect(0, 0, position.width, NWDGUI.KTAB_BAR_HEIGHT), NWDGUI.KTAB_BAR_BACK_COLOR);
            EditorGUI.DrawRect(new Rect(0, NWDGUI.KTAB_BAR_HEIGHT, position.width, 1.0F), NWDGUI.KTAB_BAR_LINE_COLOR);
            EditorGUI.DrawRect(new Rect(0, NWDGUI.KTAB_BAR_HEIGHT + 1, position.width, 1.0F), NWDGUI.KTAB_BAR_HIGHLIGHT_COLOR);
            // split windows
            SplitArea.OnGUI(this);
            // define rect to use 
            Rect tAreaTableOrign = SplitArea.GetAreaOne();
            Rect tAreaPanelOrigin = SplitArea.GetAreaTwo();

            Rect tDataRect = new Rect(tAreaTableOrign.x, tAreaTableOrign.y + NWDGUI.KTAB_BAR_HEIGHT, tAreaTableOrign.width, tAreaTableOrign.height - NWDGUI.KTAB_BAR_HEIGHT * 2);
            DataSplitArea.OnGUI(this, tDataRect);
            // define rect without bar top
            Rect tAreaTable = SplitArea.GetAreaOne();
            Rect tAreaPanel = SplitArea.GetAreaTwo();
            tAreaPanel.y += NWDGUI.KTAB_BAR_HEIGHT;
            tAreaPanel.height -= NWDGUI.KTAB_BAR_HEIGHT;
            tAreaTable.y += NWDGUI.KTAB_BAR_HEIGHT;
            tAreaTable.height -= NWDGUI.KTAB_BAR_HEIGHT;

            // select the basishelper to use
            if (mTabContentList != null)
            {
                if (TypeList().Count() > 0)
                {
                    //tLarg = (int)Math.Floor((tAreaPanel.width - 24) / (float)NWDConfigUnityEditor.KConfig.ReturnAllEnabledEnvironments().Length);
                    tLarg = 100;
                    SplitArea.BeginAreaOne();
                    ClassSelection();

                    if (tInformation.IsEditorData())
                    {
                        DataSplitArea.BeginAreaOneHeader();
                        EditorFiltersInformations();
                        CalculateListDisplayData(tAreaTable);
                        EditorTableHeader();
                        DataSplitArea.EndAreaOneHeader();
                        DataSplitArea.BeginAreaOne();
                        EditorTableRows(tAreaTable);
                        DataSplitArea.EndAreaOne();
                        DataSplitArea.BeginAreaTwoHeader();
                        EditorDataTracksHeader(DataSplitArea.GetAreaTwoHeader());
                        DataSplitArea.EndAreaTwoHeader();
                        DataSplitArea.BeginAreaTwo(DataTrackDisplayOffset);
                        EditorDataTracks();
                        DataSplitArea.EndAreaTwo();
                        EditorTableFooter();
                    }
                    else
                    {
                        PlayerFiltersInformations();
                        CalculateListDisplayData(DataSplitArea.GetAreaOne());
                        PlayerTableHeader();
                        PlayerTableRows();
                        PlayerTableFooter();
                    }
                    SplitArea.EndAreaOne();
                    SplitArea.BeginAreaTwo();
                    PanelSelection();
                    //if (tInformation.tTypeImplement.ModelStyle == NWDTypeModelsStyle.Standard || NWDPlanResumeGUI.GetPlanResumeGUI(NWDUnityEngineEditor.Instance.GetConfig().Plan).DrawIfCustomClasses())
                    {
                        switch (mPanelSelected)
                        {
                            case NWDBasisHelperPanel.Infos:
                                {
                                    PanelInfos();
                                }
                                break;
                            case NWDBasisHelperPanel.Actions:
                                {
                                    if (tInformation.IsEditorData())
                                    {
                                        EditorPanelAction();
                                    }
                                    else
                                    {
                                        PlayerPanelAction();
                                    }
                                }
                                break;
                            case NWDBasisHelperPanel.Data:
                                {
                                    if (tInformation.IsEditorData())
                                    {
                                        EditorPanelData();
                                    }
                                    else
                                    {
                                        PlayerPanelData();
                                    }
                                }
                                break;
                        }
                    }
                    SplitArea.EndAreaTwo();
                }
            }
            //NWDBenchmark.Finish();
            EditorGUI.EndDisabledGroup();

            if (ShowCreationWindow)
            {
                ShowCreationWindow = false;
                NWDCreateNewData.SharedInstanceFocus(this);
            }
        }


        public void ScrollToSelection()
        {
            int tIndex = -1;
            if (TabSelectionSelected.ObjectSelected != null)
            {
                tIndex = TabSelectionSelected.Listed.IndexOf(TabSelectionSelected.ObjectSelected);
            }
            if (tIndex < 0)
            {
                ScrollPositionTable.y = 0;
            }
            else
            {
                ScrollPositionTable.y = ListData.LineHeight * tIndex;
            }
        }

        #region Class

        private void ClassSelection()
        {
            Rect tAreaTableOrign = SplitArea.GetAreaOne();
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab && Event.current.shift)
            {
                int tNExt = TabSelected + 1;
                SelectTab(tNExt);
                Event.current.Use();
            }
            // the next selected tab
            int tTabSelected = 0;
            // check if tab ids necessary
            if (mTabContentList.Length > 0)
            {
                GUILayout.Space(NWDGUI.kFieldMarge);
                if (tAreaTableOrign.width > TabsTotalWidthExpected)
                {
                    tTabSelected = GUILayout.Toolbar(TabSelected, mTabContentList, NWDGUI.KTableClassToolbar, GUILayout.Height(NWDGUI.KTAB_TOOLBAR_HEIGHT));
                }
                else
                {
                    tTabSelected = EditorGUILayout.Popup(TabSelected, mTabContentList, NWDGUI.KTableClassPopup, GUILayout.Height(NWDGUI.KTAB_TOOLBAR_HEIGHT));
                }
                GUILayout.Space(NWDGUI.kFieldMarge);
            }
            if (TypeList().Count() > 0)
            {

                if (TabSelected != tTabSelected || TabSelectionSelected == null)
                {
                    SelectTab(tTabSelected);
                }
            }
        }

        private void PanelSelection()
        {
            GUILayout.Space(NWDGUI.kFieldMarge);
            GUILayout.BeginHorizontal();
            mPanelSelected = (NWDBasisHelperPanel)GUILayout.Toolbar((int)mPanelSelected, mPanelContentList, NWDGUI.KTableClassToolbar, GUILayout.Height(NWDGUI.KTAB_TOOLBAR_HEIGHT));
            if (NWDUnityEngineEditor.Instance.GetConfig().GetShowLogo() == true)
            {
                GUILayout.Space(48.0F);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(NWDGUI.kFieldMarge);
        }

        public virtual void SelectVirtualData (NWDVirtualDataGUI sRow)
        {
            TabSelectionSelected.Select(sRow, this);
        }

        #endregion

        #region Player

        #region Table

        private void PlayerFiltersInformations()
        {

            //TODO:   faire le liste des utilisateurs fake  dans ce environnement ... et ajouter les accounts importé ? 


            GUILayout.Label("Not editor class ... just see data by environment", NWDGUI.KTableSearchTitle);
            //tInformation.EnvTabSeleced = GUILayout.Toolbar(tInformation.EnvTabSeleced, NWDUnityEngineEditor.Instance.GetConfig().ReturnAllEnabledEnvironmentsGUIContent(), NWDGUI.KTableClassToolbar, GUILayout.Height(NWDGUI.KTAB_TOOLBAR_HEIGHT));
            tInformation.EnvTabSeleced = GUILayout.Toolbar(tInformation.EnvTabSeleced, new string[] { "Test" }, NWDGUI.KTableClassToolbar, GUILayout.Height(NWDGUI.KTAB_TOOLBAR_HEIGHT));
        }

        private void PlayerTableHeader()
        {
            NWDGUILayout.LittleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.Label("No action with Player Data outside code!");
            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
            NWDGUILayout.LittleSpace();
        }

        private void PlayerTableRows()
        {
            NWDGUILayout.LittleSpace();
            //NWDEnvironmentUnityEditor tEnvSelected = (NWDEnvironmentUnityEditor)NWDUnityEngineEditor.Instance.GetConfig().ReturnAllEnabledEnvironments()[tInformation.EnvTabSeleced];

            //if (tEnvSelected.TypeActivateList.Contains(tInformation.ClassType.AssemblyQualifiedName))
            if (true)
            {
                GUILayout.Label("Enable", NWDGUI.KTableSearchTitle);
                //if (tInformation.tTypeImplement.GetDedicatedFactory(tEnvSelected) != null)
                if (true)
                {
                    GUILayout.Label("Factory!", NWDGUI.KTableSearchTitle);
                }
                else
                {
                    GUILayout.Label("NO Factory", NWDGUI.KTableSearchTitle);
                }
                NWDGUILayout.LittleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.Label("No action with Player Data outside code!");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical(GUILayout.Width(300));
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical(EditorStyles.helpBox);
                if (GUILayout.Button(tInformation.tClass.GetContent(), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4)))
                {
                    //NWEScriptOpener.OpenScript(tInformation.tPlan.TheClassType);
                }
                NWDGUILayout.Separator();
                GUILayout.Label("This class is not enabled in this environnement (see environment configuration)", NWDGUI.kInspectorReferenceCenter);
                NWDGUILayout.Separator();
                if (GUILayout.Button("Show environment configuration"))
                {
                    //NWDEnvironmentManagement.SharedInstanceFocus(tEnvSelected);
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }


        }
        private void PlayerTableFooter()
        {
            NWDGUILayout.LittleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.Label("No action with Player Data outside code!");
            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
            NWDGUILayout.LittleSpace();
        }

        #endregion

        #region Panels

        private void PlayerPanelAction()
        {
            /*if (GUILayout.Button(tInformation.tClass.GetContent(), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4)))
            {
                NWEScriptOpener.OpenScript(tInformation.tPlan.TheClassType);
            }*/
            NWDGUILayout.Line();
            ScrollPositionAction = GUILayout.BeginScrollView(ScrollPositionAction, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);

            NWDGUILayout.Section("Database table");
            GUILayout.BeginHorizontal();
            /*foreach (NWDEnvironmentUnityEditor tEnv in NWDUnityEngineEditor.Instance.GetConfig().ReturnAllEnabledEnvironments())
            {
                GUILayout.BeginVertical(GUILayout.Width(tLarg), GUILayout.ExpandWidth(false));
                tEnv.GUILayoutLabel();
                NWDGUILayout.LittleSpace();

                if (tEnv.TypeActivateList.Contains(tInformation.ClassType.AssemblyQualifiedName))
                {
                    if (GUILayout.Button("Reset table", NWDGUI.KTableSearchButton))
                    {
                        NWDDebug.Log("Reset table");
                        //tEnv.databa
                        //tInformation.tPlan.
                    }
                    string tDatabasePath = string.Empty;
                    if (tInformation.IsEditorData())
                    {
                        tDatabasePath = NWDDatabasePathTools.GetDatabaseName(NWDToolboxUnityEditor.GetDatabaseEditorPath() + "/" + NWDToolbox.UnixCleaner(tEnv.EnvName.ToString()), NWDDatabaseDestination.StudioDatabase);

                    }
                    else
                    {
                        tDatabasePath = NWDDatabasePathTools.GetDatabaseName(NWDToolboxUnityEditor.GetDatabaseEditorPath() + "/" + NWDToolbox.UnixCleaner(tEnv.EnvName.ToString()), NWDDatabaseDestination.PlayerDatabase);
                    }
                    if (File.Exists(tDatabasePath))
                    {
                        if (GUILayout.Button("Open database", NWDGUI.KTableSearchButton))
                        {
                            EditorUtility.OpenWithDefaultApp(tDatabasePath);
                        }
                    }
                }
                else
                {
                    GUILayout.Label("✕", NWDGUI.kInspectorReferenceCenter);
                }
                GUILayout.EndVertical();
            }*/
            GUILayout.EndHorizontal();



            GUILayout.EndScrollView();
        }

        private void PlayerPanelData()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(GUILayout.Width(300));
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical(EditorStyles.helpBox);
            if (GUILayout.Button(tInformation.tClass.GetContent(), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4)))
            {
                NWEScriptOpener.OpenScript(tInformation.ClassType);
            }
            NWDGUILayout.Separator();
            GUILayout.Label("You have no data in inpector!", NWDGUI.kInspectorReferenceCenter);
            NWDGUILayout.Separator();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        #endregion

        #endregion

        #region Editor

        #region Table

        protected void EditorFiltersInformations()
        {
            NWDGUILayout.LittleSpace();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();

            //GUILayout.BeginVertical();
            //GUILayout.Label("Show", NWX_GUI.KTableSearchTitle);
            //TabSelectionSelected.EnableDatas = EditorGUILayout.ToggleLeft("Enable Datas", TabSelectionSelected.EnableDatas);
            //TabSelectionSelected.DisableDatas = EditorGUILayout.ToggleLeft("Disable Datas", TabSelectionSelected.DisableDatas);
            //TabSelectionSelected.TrashedDatas = EditorGUILayout.ToggleLeft("Trashed Datas", TabSelectionSelected.TrashedDatas);
            //TabSelectionSelected.CorruptedDatas = EditorGUILayout.ToggleLeft("Corrupted Datas", TabSelectionSelected.CorruptedDatas);
            //GUILayout.EndVertical();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandWidth(true));
            GUILayout.Label("Filters", NWDGUI.KTableSearchTitle);
            NWDGUILayout.Line();
            EditorGUILayout.BeginHorizontal();
            TabSelectionSelected.InternalTitle = EditorGUILayout.TextField("Title", TabSelectionSelected.InternalTitle, GUI.skin.FindStyle("ToolbarSeachTextField"));
            //Rect tLastRect = GUILayoutUtility.GetLastRect();
            //Rect tButtonRect = new Rect(tLastRect.x + tLastRect.width - tLastRect.height, tLastRect.y, tLastRect.height, tLastRect.height);
            if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
            {
                TabSelectionSelected.InternalTitle = "";
                GUI.FocusControl(null);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            TabSelectionSelected.InternalDescription = EditorGUILayout.TextField("Description", TabSelectionSelected.InternalDescription, GUI.skin.FindStyle("ToolbarSeachTextField"));
            if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
            {
                TabSelectionSelected.InternalDescription = "";
                GUI.FocusControl(null);
            }
            EditorGUILayout.EndHorizontal();

            List<string> tTagList = new List<string>(NWDUnityEngineEditor.Instance.GetConfig().GetTagList());
            List<int> tTagIntList = new List<int>();
            foreach (NWDTag tg in Enum.GetValues(typeof(NWDTag)))
            {
                tTagIntList.Add((int)tg);
            }
            tTagIntList.Sort();
            TabSelectionSelected.Tag = (NWDTag)EditorGUILayout.IntPopup("Internal tag", (int)TabSelectionSelected.Tag, tTagList.ToArray(), tTagIntList.ToArray(), NWDGUI.KTableSearchEnum);
            TabSelectionSelected.CheckList = (NWDCheck)EditorGUILayout.EnumFlagsField("Internal check", TabSelectionSelected.CheckList, NWDGUI.KTableSearchEnum);
            /*if (tInformation.tTypeImplement.CustomTag != null)
            {
                Enum tEnum = NWDToolboxEnum.Int32ConvertToEnum(tInformation.tTypeImplement.CustomTag, TabSelectionSelected.CustomTag);
                GUIContent tTitle = new GUIContent("Custom tag ", tInformation.tTypeImplement.CustomTag.Name);
                TabSelectionSelected.CustomTag = NWDToolboxEnum.EnumConvertToInt32(EditorGUILayout.EnumPopup(tTitle, tEnum, NWDGUI.KTableSearchEnum));
            }
            if (tInformation.tTypeImplement.CustomCheck != null)
            {
                Enum tEnum = NWDToolboxEnum.Int64ConvertToFlag(tInformation.tTypeImplement.CustomCheck, TabSelectionSelected.CustomCheck);
                GUIContent tTitle = new GUIContent("Custom Check ", tInformation.tTypeImplement.CustomCheck.Name);
                TabSelectionSelected.CustomCheck = NWDToolboxEnum.FlagConvertToInt64(EditorGUILayout.EnumFlagsField(tTitle, tEnum, NWDGUI.KTableSearchEnum));
            }*/

            EditorGUI.BeginDisabledGroup(!TabSelectionSelected.CanFilterChildren);

            TabSelectionSelected.FilteredTypeIndex = EditorGUILayout.Popup("Displayed Types", TabSelectionSelected.FilteredTypeIndex + 1, TabSelectionSelected.TypeFilter, NWDGUI.KTableSearchEnum) - 1;

            EditorGUI.EndDisabledGroup();

            TabSelectionSelected.Locked = EditorGUILayout.Toggle("Locked data only", TabSelectionSelected.Locked);
            EditorGUILayout.EndVertical();

            if (DisplayMode() != DataDisplayMode.SelectionMode)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MaxWidth(110));
                GUILayout.Label("Results", NWDGUI.KTableSearchTitle, GUILayout.MaxWidth(120));
                GUIStyle tStyle = new GUIStyle(GUI.skin.label);
                tStyle.alignment = TextAnchor.MiddleRight;
                NWDGUILayout.Line(GUILayout.MaxWidth(120));
                EditorGUILayout.LabelField("Total data", EditorStyles.boldLabel, GUILayout.MaxWidth(120));
                EditorGUILayout.LabelField(TabSelectionSelected.Datas.Count().ToString(), tStyle, GUILayout.MaxWidth(120));
                EditorGUILayout.LabelField("Displayed data", EditorStyles.boldLabel, GUILayout.MaxWidth(120));
                EditorGUILayout.LabelField(TabSelectionSelected.Listed.Count().ToString(), tStyle, GUILayout.MaxWidth(120));
                NWDGUILayout.LittleSpace();
                EditorGUILayout.LabelField("Project Plan", EditorStyles.boldLabel, GUILayout.MaxWidth(120));
                EditorGUILayout.LabelField(NWDUnityEngineEditor.Instance.GetConfig().GetPlan().ToString(), tStyle, GUILayout.MaxWidth(120));
                //EditorGUILayout.LabelField("Datas limit", TabSelectionSelected.Datas.Count().ToString() + "/" + NWDInstanceLimitAttribute.GetMaxForPlan(tInformation.ClassType, NWDUnityEngineEditor.Instance.GetConfig().Plan).ToString());
                //GUILayout.FlexibleSpace();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck() == true)
            {
                TabSelectionSelected.ApplyFilter(this);
            }
        }

        protected void EditorTableHeader()
        {
            NWDGUILayout.Line();
            //NWDGUILayout.SubSection("Table");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(" ", NWDGUI.KTableSearchTitle, GUILayout.Width(20)))
            {
            }
            if (GUILayout.Button("Icon", NWDGUI.KTableSearchTitle, GUILayout.Width(NWDGUI.kTableRowHeight)))
            {
            }
            if (GUILayout.Button(ListHeaderTitle, NWDGUI.KTableSearchTitle, GUILayout.Width(200))) // TODO: Make the width dynamic !
            {
                NWDLogger.Debug("Sort by title");
                if (TabSelectionSelected.SortBy == NWDBasisEditorDatasSortType.ByInternalKeyAscendant)
                {
                    TabSelectionSelected.SortBy = NWDBasisEditorDatasSortType.ByInternalKeyDescendant;
                }
                else if (TabSelectionSelected.SortBy == NWDBasisEditorDatasSortType.ByInternalKeyDescendant)
                {
                    TabSelectionSelected.SortBy = NWDBasisEditorDatasSortType.ByInternalKeyAscendant;
                }
                else
                {
                    TabSelectionSelected.SortBy = NWDBasisEditorDatasSortType.ByInternalKeyDescendant;
                }
                TabSelectionSelected.SortEditorTableDatas();
            }
            GUILayout.Label(" ", NWDGUI.KTableSearchTitle, GUILayout.Width(20));

            EditorGUILayout.EndHorizontal();
            //GUILayout.FlexibleSpace();
            NWDGUILayout.Line();
            NWDGUILayout.Line();
        }

        protected void CalculateListDisplayData(Rect sAreaTable)
        {
            ListData = new ListDisplayData(NWDGUI.kTableRowHeight, TabSelectionSelected.Listed.Count(), sAreaTable.height - kScrollControllSize, ScrollPositionTable.y);

            if (DataTrackDisplayOffset == null)
            {
                DataTrackDisplayOffset = new AreaOffset(0, (int)-ListData.LineHeight, 0, 0);
            }
        }

        protected void EditorTableRows(Rect sAreaTable)
        {
            if (TabSelectionSelected.Listed.Count() > 0)
            {
                //NWDGUILayout.Line();
                if (Event.current.type == EventType.Repaint)
                {
                    /*Rect tLastRect = GUILayoutUtility.GetLastRect();
                    TableHeightEstimateTop = tLastRect.y + tLastRect.height;
                    ScroolArea = new Rect(tLastRect.x, tLastRect.y, tLastRect.width, TableHeightEstimate);*/
                }
                // key s select
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.S && ScroolArea.Contains(Event.current.mousePosition))
                {
                    if (TabSelectionSelected.ObjectSelected != null)
                    {
                        TabSelectionSelected.ObjectSelected.Selected = true;
                    }
                    RepaintMe();
                    RemoveFieldFocus();
                    Event.current.Use();
                }
                // key d deselect
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.D && ScroolArea.Contains(Event.current.mousePosition))
                {
                    if (TabSelectionSelected.ObjectSelected != null)
                    {
                        TabSelectionSelected.ObjectSelected.Selected = false;
                    }
                    RepaintMe();
                    RemoveFieldFocus();
                    Event.current.Use();
                }
                // key f invert select
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.F && ScroolArea.Contains(Event.current.mousePosition))
                {
                    if (TabSelectionSelected.ObjectSelected != null)
                    {
                        TabSelectionSelected.ObjectSelected.Selected = !TabSelectionSelected.ObjectSelected.Selected;
                    }
                    RepaintMe();
                    RemoveFieldFocus();
                    Event.current.Use();
                }

                // key <- scroll to object in inspector
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.LeftArrow && ScroolArea.Contains(Event.current.mousePosition))
                {
                    if (TabSelectionSelected.ObjectSelected != null)
                    {
                        int tIndex = TabSelectionSelected.Listed.IndexOf(TabSelectionSelected.ObjectSelected);
                        if (tIndex >= 0 && TabSelectionSelected.Listed.Count > tIndex)
                        {
                            SelectVirtualData(TabSelectionSelected.Listed[tIndex]);
                            if (tIndex >= ListData.VisibleLineLastIndex || tIndex <= ListData.VisibleLineFirstIndex)
                            {
                                ScrollPositionTable = new Vector2(ScrollPositionTable.x, (tIndex) * ListData.LineHeight);
                            }
                        }
                        RepaintMe();
                        RemoveFieldFocus();
                        Event.current.Use();
                    }
                }
                // key DownArrow show next object in inspector 
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.DownArrow && ScroolArea.Contains(Event.current.mousePosition))
                {
                    int tIndex = -1;
                    if (TabSelectionSelected.ObjectSelected != null)
                    {
                        tIndex = TabSelectionSelected.Listed.IndexOf(TabSelectionSelected.ObjectSelected);
                    }
                    tIndex++;
                    if (tIndex >= 0 && TabSelectionSelected.Listed.Count > tIndex)
                    {
                        SelectVirtualData(TabSelectionSelected.Listed[tIndex]);
                        if (tIndex >= ListData.VisibleLineLastIndex || tIndex <= ListData.VisibleLineFirstIndex)
                        {
                            //Debug.Log("need scroll down");
                            ScrollPositionTable = new Vector2(ScrollPositionTable.x, ScrollPositionTable.y + ListData.LineHeight);
                        }
                    }
                    RepaintMe();
                    RemoveFieldFocus();
                    Event.current.Use();
                }
                // key DownArrow show preview object in inspector 
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.UpArrow && ScroolArea.Contains(Event.current.mousePosition))
                {
                    int tIndex = 1;
                    if (TabSelectionSelected.ObjectSelected != null)
                    {
                        tIndex = TabSelectionSelected.Listed.IndexOf(TabSelectionSelected.ObjectSelected);
                    }
                    tIndex--;
                    if (tIndex >= 0 && TabSelectionSelected.Listed.Count > tIndex)
                    {
                        SelectVirtualData(TabSelectionSelected.Listed[tIndex]);
                        if (tIndex >= ListData.VisibleLineLastIndex || tIndex <= ListData.VisibleLineFirstIndex)
                        {
                            //Debug.Log("need scroll up");
                            ScrollPositionTable = new Vector2(ScrollPositionTable.x, (tIndex) * ListData.LineHeight);
                        }
                    }
                    RepaintMe();
                    RemoveFieldFocus();
                    Event.current.Use();
                }

                try
                {
                    switch (DisplayMode())
                    {
                        case DataDisplayMode.SelectionMode:
                            ScrollPositionTable = EditorGUILayout.BeginScrollView(ScrollPositionTable, false, false, GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                            break;
                        default:
                            EditorGUILayout.BeginScrollView(ScrollPositionTable, false, false, GUIStyle.none, GUIStyle.none, GUIStyle.none, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                            break;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                GUILayout.Space(ListData.EmptySpaceBefore);
                for (int i = ListData.VisibleLineFirstIndex; i <= ListData.VisibleLineLastIndex; i++)
                {
                    NWDVirtualDataGUI tRow = TabSelectionSelected.Listed[i];
                    tRow.ReloadIcon();
                    tRow.GenerateGuiContent(tInformation.tClass);
                    Rect tLineRect = tRow.Draw(ListData.LineHeight, sAreaTable.width, i, TabSelectionSelected.ObjectSelected == tRow);
                    if (Event.current.type == EventType.MouseDown)
                    {
                        if (tLineRect.Contains(Event.current.mousePosition))
                        {
                            if (TabSelectionSelected.ObjectSelected == tRow)
                            {
                                SelectVirtualData(null);
                            }
                            else
                            {
                                //Debug.Log("selected");
                                SelectVirtualData(tRow);
                                mPanelSelected = NWDBasisHelperPanel.Data;
                            }
                            RepaintMe();
                            RemoveFieldFocus();
                            Event.current.Use();
                        }
                        //Event.current.Use();
                    }
                }
                GUILayout.Space(ListData.EmptySpaceAfter);
                EditorGUILayout.EndScrollView();
                GUILayout.Space(kScrollControllSize);

                if (Event.current.type == EventType.MouseDown && ScroolArea.Contains(Event.current.mousePosition))
                {
                    SelectVirtualData(null);
                    RepaintMe();
                    RemoveFieldFocus();
                    Event.current.Use();
                }

                if (Event.current.type == EventType.Repaint)
                {
                    Rect tLastRect = GUILayoutUtility.GetLastRect();
                    TableHeightEstimateBottom = tLastRect.y + tLastRect.height;
                }
                // Debug.Log("tNotTop = " + tNotTop.ToString());
                //int tIndexActuel = TabSelectionSelected.Listed.IndexOf(TabSelectionSelected.ObjectSelected);
                // Debug.Log("tIndexActuel = " + tIndexActuel.ToString());
                // Debug.Log("tNotBottom = " + tNotBottom.ToString());
                //Debug.Log("TableHeightEstimateBottom = " + TableHeightEstimateBottom.ToString());
                //Debug.Log("TableHeightEstimateTop = " + TableHeightEstimateTop.ToString());
                //Debug.Log("tTableHeight = " + TableHeightEstimate.ToString() + " tH = " + tH.ToString() + " then line max = " + tLineMax);
                //if (ScrollPositionTableLag.y != ScrollPositionTable.y)
                //{
                //    ScrollPositionTable = ScrollPositionTableLag;
                //    // repaint by update to refresh the GUI more once time, quickly
                //    RepaintMe();
                //}
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical(GUILayout.Width(300));
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical(EditorStyles.helpBox);
                if (GUILayout.Button(tInformation.tClass.GetContent(), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4)))
                {
                    //NWEScriptOpener.OpenScript(tInformation.tPlan.TheClassType);
                }
                NWDGUILayout.Separator();
                if (TabSelectionSelected.Datas.Count() == 0)
                {
                    GUILayout.Label("You have no data!", NWDGUI.kInspectorReferenceCenter);
                    //NWDGUILayout.Separator();
                    //if (GUILayout.Button("force Sync"))
                    //{
                    //    Debug.Log("force Sync");
                    //    TabSelectionSelected.GetAllData(tInformation, this);
                    //}
                    //if (GUILayout.Button("Import datas from file"))
                    //{
                    //    Debug.Log("Import datas from file");
                    //}
                    if (NWDUnityEngineEditor.Instance.GetConfig().GetCanCreateMetaData() == true)
                    {
                        GUILayout.Label("or", NWDGUI.kInspectorReferenceCenter);
                        if (GUILayout.Button("Create first data"))
                        {
                            CreateData();
                        }
                    }
                }
                else
                {
                    GUILayout.Label("You have no data found!", NWDGUI.kInspectorReferenceCenter);
                    NWDGUILayout.Separator();
                    GUILayout.Label("Change your filters!", NWDGUI.kInspectorReferenceCenter);
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            NWDGUILayout.Line();
        }

        public void EditorDataTracksHeader(Rect sAreaTable)
        {
            GUILayout.Space(sAreaTable.height - ListData.LineHeight * 2);

            EditorGUILayout.BeginScrollView(new Vector2(ScrollPositionTable.x, 0), false, false, GUIStyle.none, GUIStyle.none, GUIStyle.none, GUILayout.Width(sAreaTable.width - kScrollControllSize), GUILayout.MaxHeight(ListData.LineHeight));
            NWDVirtualDataGUI.DrawDataTracksTunnels(ListData.LineHeight);
            EditorGUILayout.EndScrollView();
        }

        private void EditorDataTracks()
        {
            Rect tAreaTable = DataSplitArea.GetAreaTwo();
            if (Event.current.type == EventType.Repaint)
            {
                CurrentArea = tAreaTable;
            }

            float tMaxDataTrackWidth = NWDVirtualDataGUI.DrawDataTracksWidth(ListData.LineHeight);

            Rect tScrollView = new Rect(0, 0, CurrentArea.width - kScrollControllSize, CurrentArea.height + ListData.LineHeight - kScrollControllSize);
            Rect tScrollContent = new Rect(Vector2.zero, new Vector2(tMaxDataTrackWidth, ListData.TotalHeight + ListData.LineHeight));

            GUI.BeginScrollView(tScrollView, ScrollPositionTable, tScrollContent, false, false, GUIStyle.none, GUIStyle.none);
            GUILayout.BeginArea(tScrollContent);
            GUILayout.Space(ListData.EmptySpaceBefore);

            if (ListData.VisibleLineFirstIndex == 0)
            {
                NWDVirtualDataGUI.DrawDataTracksTrain(ListData.LineHeight, tAreaTable.width);
            }
            else if (TabSelectionSelected.Listed.Count > 0)
            {
                NWDVirtualDataGUI tRow = TabSelectionSelected.Listed[ListData.VisibleLineFirstIndex - 1];
                tRow.DrawDataTracks(false, ListData.LineHeight, tAreaTable.width);
            }

            for (int i = ListData.VisibleLineFirstIndex; i <= ListData.VisibleLineLastIndex; i++)
            {
                NWDVirtualDataGUI tRow = TabSelectionSelected.Listed[i];
                tRow.DrawDataTracks(TabSelectionSelected.ObjectSelected == tRow, ListData.LineHeight, tAreaTable.width);
                //if (Event.current.type == EventType.MouseDown)
                //{
                //    if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                //    {
                //        if (TabSelectionSelected.ObjectSelected == tRow)
                //        {
                //            TabSelectionSelected.Select(null, this);
                //        }
                //        else
                //        {
                //            //Debug.Log("selected");
                //            TabSelectionSelected.Select(tRow, this);
                //            mPanelSelected = NWDBasisHelperPanel.Data;
                //        }
                //        RepaintMe();
                //        RemoveFieldFocus();
                //        Event.current.Use();
                //    }
                //    //Event.current.Use();
                //}
            }
            GUILayout.EndArea();
            GUI.EndScrollView();

            Rect tVScrollBar = new Rect(tScrollView.xMax, 0, kScrollControllSize, tScrollView.yMax);
            Rect tHScrollBar = new Rect(0, tScrollView.yMax, tScrollView.width, kScrollControllSize);
            if (tScrollView.height < ListData.TotalHeight + ListData.LineHeight)
            {
                ScrollPositionTable.y = GUI.VerticalScrollbar(tVScrollBar, ScrollPositionTable.y, tScrollView.height, 0, ListData.TotalHeight + ListData.LineHeight);
            }

            if (tScrollView.width < tMaxDataTrackWidth)
            {
                ScrollPositionTable.x = GUI.HorizontalScrollbar(tHScrollBar, ScrollPositionTable.x, tScrollView.width, 0, tMaxDataTrackWidth);
            }

            /*if (ScrollPositionTableLag != ScrollPositionTable)
            {
                ScrollPositionTable = ScrollPositionTableLag;
                // repaint by update to refresh the GUI more once time, quickly
                RepaintMe();
            }*/
            GUILayout.FlexibleSpace();
            NWDGUILayout.Line();
        }

        private void EditorTableFooter()
        {
            if (MetaDataSync == null)
            {
                MetaDataSync = NWDUnityEngineEditor.Instance.GetDataManager().AutoSynOperation;
            }
            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
            bool tUpToDate = TabSelectionSelected.LastUpdate == NWDUnityEngineEditor.Instance.GetDataManager().UpdateIndex;
            if (!tUpToDate)
            {
                TabSelectionSelected.UpdateDataCache(this);
            }
            if (!MetaDataSync.IsDone)
            {
                GUILayout.Label("Syncing...");
            }
            else
            {
                GUILayout.Label("Synced!");
            }
            //if (GUILayout.Button("force writing", GUILayout.Width(120.0F)))
            //{
            //    TabSelectionSelected.WriteOnDisk();
            //}

            //if (GUILayout.Button("flush", GUILayout.Width(120.0F)))
            //{
            //    Debug.Log("flush table");
            //    TabSelectionSelected.FlushAllData(tInformation, this);
            //}

            //if (GUILayout.Button("Last Sync", GUILayout.Width(120.0F)))
            //{
            //    Debug.Log("Last Sync");
            //    TabSelectionSelected.GetLastData(tInformation, this);
            //}

            //if (GUILayout.Button("force Sync", GUILayout.Width(120.0F)))
            //{
            //    Debug.Log("force Sync");
            //    TabSelectionSelected.GetAllData(tInformation, this);
            //}
            GUILayout.FlexibleSpace();

            //EditorGUI.BeginDisabledGroup(NWDInstanceLimitAttribute.GetMaxForPlan(tInformation.ClassType, NWDUnityEngineEditor.Instance.GetConfig().Plan) <= TabSelectionSelected.Datas.Count());
            //EditorGUI.BeginDisabledGroup(!NWDUnityEngineEditor.Instance.GetConfig().GetCanCreateMetaData());
            {
                if (GUILayout.Button("New data", GUILayout.Width(120.0F)))
                {
                    CreateData();
                }
            }
            EditorGUI.EndDisabledGroup();
            //EditorGUI.EndDisabledGroup();

            NWDGUILayout.LittleSpace();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(11);
            EditorGUILayout.EndVertical();
        }

        private void CreateData ()
        {
            if (TabSelectionSelected.CanFilterChildren && TabSelectionSelected.FilteredTypeIndex < 0)
            {
                ShowCreationWindow = true;
            }
            else
            {
                //Debug.Log("new data");
                TabSelectionSelected.NewData(this, TabSelectionSelected.FilteredTypeIndex);
            }
        }
        #endregion

        #region Panels

        private void PanelInfos()
        {
            /*if (GUILayout.Button(tInformation.tClass.GetContent(), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4)))
            {
                NWEScriptOpener.OpenScript(tInformation.tPlan.TheClassType);
            }*/
            NWDGUILayout.Line();
            ScrollPositionInfos = GUILayout.BeginScrollView(ScrollPositionInfos, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);

            NWDGUILayout.Section("Informations");
            EditorGUILayout.LabelField("Datas in factory", TabSelectionSelected.Datas.Count().ToString());
            EditorGUILayout.LabelField("Datas in scrollview", TabSelectionSelected.Listed.Count().ToString());
            /*foreach (NWDEnvironmentUnityEditor tEnv in NWDUnityEngineEditor.Instance.GetConfig().ReturnAllEnabledEnvironments())
            {
                if (tInformation.DatasByEnvironment.ContainsKey(tEnv) == false)
                {
                    tInformation.DatasByEnvironment.Add(tEnv, new List<NWDVirtualDataGUI>());
                }
                EditorGUILayout.LabelField("Datas in " + tEnv.EnvName.ToString(), tInformation.DatasByEnvironment[tEnv].Count().ToString());
            }*/

            NWDGUILayout.Section("Class Informations");
            EditorGUILayout.LabelField("Menu name", tInformation.ModelType.MenuName);
            EditorGUILayout.HelpBox(tInformation.ModelType.Description, MessageType.Info);

            EditorGUILayout.LabelField("Class", tInformation.ClassType.Name);
            EditorGUILayout.HelpBox(tInformation.ModelType.Description, MessageType.Info);

            NWDGUILayout.SubSection("Parent");

            NWDGUILayout.SubSection("Properties");

            foreach (KeyValuePair<string, string> tField in tInformation.DicoField)
            {
                EditorGUILayout.LabelField(tField.Key, tField.Value);
            }

            NWDGUILayout.SubSection("Security");
            NWDGUILayout.SubSection("Icon");
            if (GUILayout.Button("Reset icon file", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Debug("TODO: Reset icon file");
            }
            if (GUILayout.Button("Icon file generate", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Debug("TODO: Icon file generate");
            }

            NWDGUILayout.SubSection("Environments");
            NWDGUILayout.LittleSpace();

            GUILayout.BeginHorizontal();

            /*foreach (NWDEnvironmentUnityEditor tEnv in NWDUnityEngineEditor.Instance.GetConfig().ReturnAllEnabledEnvironments())
            {
                GUILayout.BeginVertical(GUILayout.Width(tLarg), GUILayout.ExpandWidth(false));
                tEnv.GUILayoutLabel();
                NWDGUILayout.LittleSpace();

                if (tEnv.TypeActivateList.Contains(tInformation.ClassType.AssemblyQualifiedName))
                {
                    GUILayout.Label("Enable", NWDGUI.KTableSearchTitle);
                    if (tInformation.tTypeImplement.GetDedicatedFactory(tEnv) != null)
                    {
                        GUILayout.Label("Factory!", NWDGUI.KTableSearchTitle);
                    }
                    else
                    {
                        GUILayout.Label("NO Factory", NWDGUI.KTableSearchTitle);
                    }

                }
                else
                {
                    GUILayout.Label("✕", NWDGUI.kInspectorReferenceCenter);
                }

                GUILayout.EndVertical();
            }*/

            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
        }

        private void EditorPanelAction()
        {
            /*if (GUILayout.Button(tInformation.tClass.GetContent(), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4)))
            {
                NWEScriptOpener.OpenScript(tInformation.tPlan.TheClassType);
            }*/
            NWDGUILayout.Line();


            ScrollPositionAction = GUILayout.BeginScrollView(ScrollPositionAction, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);

            NWDGUILayout.Section("Selection");
            NWDGUILayout.Label("xxx selected data");

            if (GUILayout.Button("Select All", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Information("Selected All");
                TabSelectionSelected.SelectAll();
            }
            if (GUILayout.Button("Deselect All", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Information("Deselected All");
                TabSelectionSelected.DeselectAll();
            }
            if (GUILayout.Button("Inverse selection", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Debug("Inversed selection");
                TabSelectionSelected.InverseSelection();
            }
            if (GUILayout.Button("Select all enable", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Debug("TODO: Select all enable");
            }
            if (GUILayout.Button("Select all disable", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Debug("TODO: Select all disable");
            }

            GUILayout.BeginHorizontal();
            /*foreach (NWDEnvironmentUnityEditor tEnv in NWDUnityEngineEditor.Instance.GetConfig().ReturnAllEnabledEnvironments())
            {
                GUILayout.BeginVertical(GUILayout.Width(tLarg), GUILayout.ExpandWidth(false));
                tEnv.GUILayoutLabel();
                NWDGUILayout.LittleSpace();
                if (tEnv.TypeActivateList.Contains(tInformation.ClassType.AssemblyQualifiedName))
                {
                    if (GUILayout.Button("Add", NWDGUI.KTableSearchButton))
                    {
                        Debug.Log("Add these " + tEnv);
                    }
                    if (GUILayout.Button("Substract", NWDGUI.KTableSearchButton))
                    {
                        Debug.Log("Remove these " + tEnv);
                    }
                }
                else
                {
                    GUILayout.Label("✕", NWDGUI.kInspectorReferenceCenter);
                }
                GUILayout.EndVertical();
            }*/
            GUILayout.EndHorizontal();

            NWDGUILayout.SubSection("Action on selection");
            //if (GUILayout.Button("Dupplicate selection", NWX_GUI.KTableSearchButton))
            //{
            //    Debug.Log("Dupplicate selection");
            //    TabSelectionSelected.DupplicateSelection(tInformation, this);
            //}
            if (GUILayout.Button("Active selection", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Debug("TODO: Active selection");
            }
            if (GUILayout.Button("Disactive selection", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Debug("TODO: Disactive selection");
            }

            NWDGUI.BeginRedArea();
            if (GUILayout.Button("Trash selection", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Debug("TODO: Trash selection");
            }
            if (GUILayout.Button("Untrash selection", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Debug("TODO: Untrash selection");
            }
            if (GUILayout.Button("Delete or trash selection", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Debug("TODO: Delete or trash selection");
            }
            NWDGUI.EndRedArea();

            NWDGUILayout.SubSection("Action on selection for translation");
            if (GUILayout.Button("Reorder transalation", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Debug("TODO: Reorder transalation");
            }
            if (GUILayout.Button("Export for translation", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Debug("TODO: Export for translation");
            }
            if (GUILayout.Button("Import file translation", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Debug("TODO: Import file translation");
            }

            NWDGUILayout.SubSection("Action on selection for export/import");
            if (GUILayout.Button("Export to file", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Debug("TODO: Export to file");
            }
            if (GUILayout.Button("Import from file", NWDGUI.KTableSearchButton))
            {
                NWDLogger.Debug("TODO: Import from file");
            }
            NWDGUILayout.Section("Database table");
            GUILayout.BeginHorizontal();
            /*foreach (NWDEnvironmentUnityEditor tEnv in NWDUnityEngineEditor.Instance.GetConfig().ReturnAllEnabledEnvironments())
            {
                GUILayout.BeginVertical(GUILayout.Width(tLarg), GUILayout.ExpandWidth(false));
                tEnv.GUILayoutLabel();
                NWDGUILayout.LittleSpace();

                if (tEnv.TypeActivateList.Contains(tInformation.ClassType.AssemblyQualifiedName))
                {
                    if (GUILayout.Button("Reset table", NWDGUI.KTableSearchButton))
                    {
                        Debug.Log("Reset table");
                        //tEnv.databa
                        //tInformation.tPlan.
                    }
                    string tDatabasePath = string.Empty;
                    if (tInformation.IsEditorData())
                    {
                        tDatabasePath = NWDDatabasePathTools.GetDatabaseName(NWDToolboxUnityEditor.GetDatabaseEditorPath() + "/" + NWDToolbox.UnixCleaner(tEnv.EnvName.ToString()), NWDDatabaseDestination.StudioDatabase);

                    }
                    else
                    {
                        tDatabasePath = NWDDatabasePathTools.GetDatabaseName(NWDToolboxUnityEditor.GetDatabaseEditorPath() + "/" + NWDToolbox.UnixCleaner(tEnv.EnvName.ToString()), NWDDatabaseDestination.PlayerDatabase);
                    }
                    if (File.Exists(tDatabasePath))
                    {
                        if (GUILayout.Button("Open database", NWDGUI.KTableSearchButton))
                        {
                            EditorUtility.OpenWithDefaultApp(tDatabasePath);
                        }
                    }
                }
                else
                {
                    GUILayout.Label("✕", NWDGUI.kInspectorReferenceCenter);
                }
                GUILayout.EndVertical();
            }*/
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
        }

        private void EditorPanelData()
        {
            NWDDataTrackDescription tCurrentDataTrack = NWDUnityEngineEditor.Instance.GetConfig().GetSelectedEnvironment();

            if (TabSelectionSelected.ObjectSelected != null)
            {
                NWDSubMetaData tSubMetaData = TabSelectionSelected.ObjectSelected.GetSubMetaData(tCurrentDataTrack);

                if (TabSelectionSelected.ObjectSelected.IsLockForEdition() == false)
                {
                    if (GUILayout.Button("Request to edit"))
                    {
                        TabSelectionSelected.LockForEdition(this, TabSelectionSelected.ObjectSelected.MetaData);
                    }
                }
                else
                {
                    if (GUILayout.Button("I finish to edit"))
                    {
                        TabSelectionSelected.UnlockForEdition(this, TabSelectionSelected.ObjectSelected.MetaData);
                    }
                }
                if (GUILayout.Button(TabSelectionSelected.ObjectSelected.IconContent.GetContent(), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4)))
                {
                    NWEScriptOpener.OpenScript(tInformation.ClassType);
                }

                EditorGUI.BeginDisabledGroup(!tSubMetaData.ExistsInDatabase());

                if (tSubMetaData.Trashed)
                {
                    if (GUILayout.Button("Untrash"))
                    {
                        NWDLogger.Information("Untrash data: (" + tCurrentDataTrack.Name + ")" + TabSelectionSelected.ObjectSelected.MetaData.Reference + " - " + TabSelectionSelected.ObjectSelected.MetaData.Title);
                        if (TabSelectionSelected.ObjectSelected.SetTrashState(tCurrentDataTrack, tSubMetaData, false))
                        {
                            TabSelectionSelected.ObjectSelected.Update();
                        }
                    }
                }
                else
                {
                    if (GUILayout.Button("Trash"))
                    {
                        NWDLogger.Information("Trashed data: (" + tCurrentDataTrack.Name + ")" + TabSelectionSelected.ObjectSelected.MetaData.Reference + " - " + TabSelectionSelected.ObjectSelected.MetaData.Title);
                        if (TabSelectionSelected.ObjectSelected.SetTrashState(tCurrentDataTrack, tSubMetaData, true))
                        {
                            TabSelectionSelected.ObjectSelected.Update();
                        }
                    }
                }

                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(TabSelectionSelected.ObjectSelected.IsLockForEdition() == false);

                NWDGUILayout.Line();

                EditorGUI.BeginChangeCheck();

                EditorGUILayout.LabelField("Reference", TabSelectionSelected.ObjectSelected.MetaData.Reference.ToString());
                //EditorGUILayout.LabelField("Preview GUID", TabSelectionSelected.ObjectSelected.VirtualData.PreviewGUIDstring);
                //if (TabSelectionSelected.ObjectSelected.PreviewObject == null)
                //{
                //    GUID tPreviewGUIDA;
                //    if (GUID.TryParse(TabSelectionSelected.ObjectSelected.VirtualData.PreviewGUIDstring, out tPreviewGUIDA))
                //    {
                //        TabSelectionSelected.ObjectSelected.PreviewObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(tPreviewGUIDA), typeof(UnityEngine.Object));
                //    }
                //}
                EditorGUI.BeginDisabledGroup(!NWDUnityEngineEditor.Instance.GetConfig().GetCanEditMetaDataInfos());

                TabSelectionSelected.ObjectSelected.PreviewObject = EditorGUILayout.ObjectField("Preview", TabSelectionSelected.ObjectSelected.PreviewObject, typeof(UnityEngine.Object), false);
                TabSelectionSelected.ObjectSelected.MetaData.Title = EditorGUILayout.TextField("Title", TabSelectionSelected.ObjectSelected.MetaData.Title);
                TabSelectionSelected.ObjectSelected.MetaData.Description = EditorGUILayout.TextField("Description", TabSelectionSelected.ObjectSelected.MetaData.Description);


                DateTime tDateTime = NWDTimestamp.TimeStampToDateTime(TabSelectionSelected.ObjectSelected.MetaData.ModificationDate);
                EditorGUILayout.LabelField("Modfication", tDateTime.ToString("g"));


                List<string> tTagList = new List<string>(NWDUnityEngineEditor.Instance.GetConfig().GetTagList());
                List<int> tTagIntList = new List<int>();
                foreach (NWDTag tg in Enum.GetValues(typeof(NWDTag)))
                {
                    tTagIntList.Add((int)tg);
                }
                tTagIntList.Sort();
                //TabSelectionSelected.ObjectSelected.VirtualData.Tag = (NWDTag)EditorGUILayout.IntPopup("Internal tag", (int)TabSelectionSelected.ObjectSelected.VirtualData.Tag, tTagList.ToArray(), tTagIntList.ToArray(), NWDGUI.KTableSearchEnum);
                //TabSelectionSelected.ObjectSelected.VirtualData.Check = (NWDCheck)EditorGUILayout.EnumFlagsField("Internal check", TabSelectionSelected.ObjectSelected.VirtualData.Check, NWDGUI.KTableSearchEnum);
                //if (tInformation.tTypeImplement.CustomTag != null)
                //{
                //    Enum tEnum = NWDToolboxEnum.Int32ConvertToEnum(tInformation.tTypeImplement.CustomTag, TabSelectionSelected.ObjectSelected.VirtualData.CustomTag);
                //    GUIContent tTitle = new GUIContent("Custom tag ", tInformation.tTypeImplement.CustomTag.Name);
                //    TabSelectionSelected.ObjectSelected.VirtualData.CustomTag = NWDToolboxEnum.EnumConvertToInt32(EditorGUILayout.EnumPopup(tTitle, tEnum, NWDGUI.KTableSearchEnum));
                //}
                //if (tInformation.tTypeImplement.CustomCheck != null)
                //{
                //    Enum tEnum = NWDToolboxEnum.Int64ConvertToFlag(tInformation.tTypeImplement.CustomCheck, TabSelectionSelected.ObjectSelected.VirtualData.CustomCheck);
                //    GUIContent tTitle = new GUIContent("Custom Check ", tInformation.tTypeImplement.CustomCheck.Name);
                //    TabSelectionSelected.ObjectSelected.VirtualData.CustomCheck = NWDToolboxEnum.FlagConvertToInt64(EditorGUILayout.EnumFlagsField(tTitle, tEnum, NWDGUI.KTableSearchEnum));
                //}



                EditorGUI.EndDisabledGroup();


                if (EditorGUI.EndChangeCheck() == true)
                {
                    //Debug.Log("EndChangeCheck()");
                    TabSelectionSelected.ObjectSelected.UpateInformation(tInformation);

                    TabSelectionSelected.ObjectSelected.Update();

                    //TabSelectionSelected.WriteOnDisk();
                }
                //if (TabSelectionSelected.ObjectSelected.PreviewTextureIsLoaded!= PreviewTextureState.D)
                //{
                //    RepaintMe();
                //}

                EditorGUI.EndDisabledGroup();
                NWDGUILayout.Line();
                EditorGUI.BeginChangeCheck();

                bool tVIsibleInBuild = tSubMetaData.State.HasFlag(NWDSubMetaDataState.VisibleInBuild);
                tVIsibleInBuild = EditorGUILayout.Toggle("Visible in build", tVIsibleInBuild);

                if (EditorGUI.EndChangeCheck())
                {
                    if (tVIsibleInBuild)
                    {
                        tSubMetaData.State |= NWDSubMetaDataState.VisibleInBuild;
                    }
                    else
                    {
                        tSubMetaData.State = tSubMetaData.State & ~NWDSubMetaDataState.VisibleInBuild;
                    }
                    TabSelectionSelected.ObjectSelected.Update();
                }
                NWDDataTrackDescription tOrigin = NWDUnityEngineEditor.Instance.GetConfig().GetEnvironment(tSubMetaData.Origin);
                EditorGUILayout.LabelField("Current DataTrack", tCurrentDataTrack.Name);
                if (tOrigin == null)
                {
                    EditorGUILayout.LabelField("Data origin", "None");
                }
                else
                {
                    if (tSubMetaData.State.HasFlag(NWDSubMetaDataState.Overriden))
                    {
                        EditorGUILayout.LabelField("Data origin", tCurrentDataTrack.Name);
                        if (tSubMetaData.State.HasFlag(NWDSubMetaDataState.Outdated))
                        {
                            EditorGUILayout.HelpBox("A new version is available from origin", MessageType.Info);
                        }
                        if (tCurrentDataTrack != tOrigin && GUILayout.Button("Revert"))
                        {
                            TabSelectionSelected.ObjectSelected.Revert(tOrigin, tCurrentDataTrack);
                            TabSelectionSelected.ObjectSelected.Update();
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField("Data origin", tOrigin.Name);
                        if (GUILayout.Button("Override data"))
                        {
                            tSubMetaData.State |= NWDSubMetaDataState.Overriden;
                            TabSelectionSelected.ObjectSelected.Update();
                        }
                    }

                    if (tCurrentDataTrack != tOrigin && GUILayout.Button("View data from origin"))
                    {
                        NWDUnityEngineEditor.Instance.GetConfig().SetSelectedEnvironment(tOrigin);
                    }
                }

                NWDGUILayout.Line();
                //int tTabSelect = GUILayout.Toolbar(_kTabSelected, NWDUnityEngineEditor.Instance.GetConfig().ReturnAllAccessibleEnvironmentsGUIContent(), NWDGUI.KTableClassToolbar, GUILayout.Height(NWDGUI.KTAB_TOOLBAR_HEIGHT));
                //if (tTabSelect != _kTabSelected)
                //{
                //    _kTabSelected = tTabSelect;
                //    if (_kTabSelected < 0)
                //    {
                //        _kTabSelected = 0;
                //    }
                //    GUI.FocusControl(null);
                //}

                EditorGUI.BeginDisabledGroup(!TabSelectionSelected.ObjectSelected.IsLockForEdition(tSubMetaData));
                try
                {
                    object tData = TabSelectionSelected.ObjectSelected.ExtractData(tCurrentDataTrack);

                    EditorGUIUtility.hierarchyMode = true;

                    ScrollPositionData = GUILayout.BeginScrollView(ScrollPositionData, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);

                    EditorGUI.BeginChangeCheck();

                    NWDModelTypeInformation tModelTypeDrawer = NWDModelType.ModelTypeInformationByType[TabSelectionSelected.ObjectSelected.DataType()];

                    Rect tPosition = GUILayoutUtility.GetRect(SplitArea.GetAreaTwo().width, tModelTypeDrawer.GetPropertyHeight(tData));
                    tPosition.xMax -= EditorGUIUtility.singleLineHeight;
                    tPosition.xMin += EditorGUIUtility.singleLineHeight;

                    tModelTypeDrawer.OnGUI(tPosition, tData);

                    if (EditorGUI.EndChangeCheck())
                    {
                        TabSelectionSelected.ObjectSelected.InsertSubMetaData (tCurrentDataTrack, tData);
                        TabSelectionSelected.ObjectSelected.Update();
                    }

                    GUILayout.EndScrollView();

                    EditorGUIUtility.hierarchyMode = false;
                }
                catch (Exception e)
                {
                    EditorGUILayout.HelpBox(e.Message, MessageType.Error);
                    Debug.LogException(e);
                }

                EditorGUI.EndDisabledGroup();

                //NWDEnvironmentUnityEditor[] tReturnAllAccessibleEnvironments = NWDUnityEngineEditor.Instance.GetConfig().ReturnAllAccessibleEnvironments();
                //if (_kTabSelected > tReturnAllAccessibleEnvironments.Length)
                //{
                //    _kTabSelected = 0;
                //}
                //if (tReturnAllAccessibleEnvironments.Length > _kTabSelected)
                //{
                //    NWDEnvironmentUnityEditor tEnvSelected = tReturnAllAccessibleEnvironments[_kTabSelected];

                //    EditorGUI.BeginDisabledGroup(TabSelectionSelected.ObjectSelected.IsLockForEdition() == false);
                //    if (tEnvSelected.TypeActivateList.Contains(tInformation.ClassType.AssemblyQualifiedName))
                //    {
                //        //GUILayout.Label(tEnvSelected.GetAccessRights().ToString());
                //        NWDEnvironmentRights tAccess = tEnvSelected.GetAccessRights();
                //        switch (tEnvSelected.EnvType)
                //        {
                //            case NWDEnvironmentType.None:
                //            case NWDEnvironmentType.Preprod:
                //            case NWDEnvironmentType.Prod:
                //            //case NWDEnvironmentType.Qualification:
                //            case NWDEnvironmentType.Postprod:
                //                {
                //                    tAccess = NWDEnvironmentRights.None;
                //                }
                //                break;
                //        }
                //        switch (tAccess)
                //        {
                //            case NWDEnvironmentRights.None:
                //                {
                //                    GUILayout.BeginHorizontal();
                //                    GUILayout.FlexibleSpace();
                //                    GUILayout.BeginVertical(GUILayout.Width(300));
                //                    GUILayout.FlexibleSpace();
                //                    GUILayout.BeginVertical(EditorStyles.helpBox);
                //                    if (GUILayout.Button(tEnvSelected.GetGUIContent(), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4)))
                //                    {
                //                    }
                //                    NWDGUILayout.Separator();
                //                    GUILayout.Label("No edition or no rights on this environment", NWDGUI.kInspectorReferenceCenter);
                //                    NWDGUILayout.Separator();
                //                    GUILayout.EndVertical();
                //                    GUILayout.FlexibleSpace();
                //                    GUILayout.EndVertical();
                //                    GUILayout.FlexibleSpace();
                //                    GUILayout.EndHorizontal();
                //                }
                //                break;
                //            case NWDEnvironmentRights.Read:
                //                {
                //                    ScrollPositionData = GUILayout.BeginScrollView(ScrollPositionData, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
                //                    if (tInformation.tTypeImplement.GetDedicatedFactory(tEnvSelected) != null)
                //                    {
                //                        //GUILayout.Label("Factory!", NWDGUI.KTableSearchTitle);
                //                        if (tEnvSelected.EnvType == NWDEnvironmentType.Qualification)
                //                        {
                //                            EditorGUILayout.LabelField("THE BUNDLE FOR EDITION", " BUNDLE SELECTED");
                //                        }
                //                        //foreach (KeyValuePair<string, string> tField in tInformation.DicoField)
                //                        //{
                //                        //    EditorGUILayout.LabelField(tField.Key, tField.Value);
                //                        //}
                //                        object tObjectItem = TabSelectionSelected.ObjectSelected.VirtualData.GetXXXPrefKeyB(tEnvSelected.EnvName) as object;
                //                        // TODO change for the real class and edit this object!
                //                        foreach (PropertyInfo tPropertyInfo in tObjectItem.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
                //                        {
                //                            object tValue = tPropertyInfo.GetValue(tObjectItem);
                //                            if (tValue == null) { tValue = string.Empty; }
                //                            EditorGUILayout.LabelField(tPropertyInfo.Name + " (" + tPropertyInfo.PropertyType.Name + ")", tValue.ToString());
                //                        }
                //                        //EditorGUILayout.LabelField("DataJSON", TabSelectionSelected.ObjectSelected.VirtualData.DataJSON);
                //                    }
                //                    else
                //                    {
                //                        GUILayout.Label("No Factory", NWDGUI.KTableSearchTitle);
                //                    };
                //                    GUILayout.EndScrollView();
                //                }
                //                break;
                //            case NWDEnvironmentRights.Write:
                //                {
                //                    ScrollPositionData = GUILayout.BeginScrollView(ScrollPositionData, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
                //                    if (tInformation.tTypeImplement.GetDedicatedFactory(tEnvSelected) != null)
                //                    {
                //                        //GUILayout.Label("Factory!", NWDGUI.KTableSearchTitle);
                //                        switch (tEnvSelected.EnvType)
                //                        {
                //                            case NWDEnvironmentType.Preview:
                //                            case NWDEnvironmentType.Playtest:
                //                                {
                //                                    EditorGUI.BeginChangeCheck();
                //                                    object tObjectItem = TabSelectionSelected.ObjectSelected.VirtualData.GetXXXPrefKeyB(tEnvSelected.EnvName) as object;
                //                                    // TODO change for the real class and edit this object!
                //                                    foreach (PropertyInfo tPropertyInfo in tObjectItem.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
                //                                    {
                //                                        XXXEditorTool.DrawEditorField(tPropertyInfo, tObjectItem);
                //                                        ////EditorGUILayout.LabelField(tPropertyInfo.Name, tPropertyInfo.GetValue(tObjectItem).ToString());
                //                                        ////EditorGUILayout.LabelField(tPropertyInfo.Name);
                //                                        ///*if (tPropertyInfo.PropertyType == typeof(string))
                //                                        //{
                //                                        //    string tValue = tPropertyInfo.GetValue(tObjectItem) as string;
                //                                        //    if (string.IsNullOrEmpty(tValue)) { tValue = string.Empty; }
                //                                        //    tPropertyInfo.SetValue(tObjectItem, EditorGUILayout.TextField(tPropertyInfo.Name + " (string)", tValue));
                //                                        //}
                //                                        //else*/ if (tPropertyInfo.PropertyType == typeof(String))
                //                                        //{
                //                                        //    String tValue = tPropertyInfo.GetValue(tObjectItem) as String;
                //                                        //    if (String.IsNullOrEmpty(tValue)) { tValue = String.Empty; }
                //                                        //    tPropertyInfo.SetValue(tObjectItem, EditorGUILayout.TextField(tPropertyInfo.Name + " (String)", tValue));
                //                                        //}
                //                                        //else if (tPropertyInfo.PropertyType == typeof(int))
                //                                        //{
                //                                        //    tPropertyInfo.SetValue(tObjectItem,
                //                                        //        (int)EditorGUILayout.IntField(tPropertyInfo.Name + " (int)",
                //                                        //        (int)tPropertyInfo.GetValue(tObjectItem, null)));
                //                                        //}
                //                                        //else if (tPropertyInfo.PropertyType == typeof(Int16))
                //                                        //{
                //                                        //    tPropertyInfo.SetValue(tObjectItem,
                //                                        //        (Int16)EditorGUILayout.IntField(tPropertyInfo.Name + " (Int16)",
                //                                        //        (Int16)tPropertyInfo.GetValue(tObjectItem, null)));
                //                                        //}
                //                                        //else if (tPropertyInfo.PropertyType == typeof(Int32))
                //                                        //{
                //                                        //    tPropertyInfo.SetValue(tObjectItem,
                //                                        //        (Int32)EditorGUILayout.IntField(tPropertyInfo.Name + " (Int32)",
                //                                        //        (Int32)tPropertyInfo.GetValue(tObjectItem, null)));
                //                                        //}
                //                                        //else if (tPropertyInfo.PropertyType == typeof(Int64))
                //                                        //{
                //                                        //    tPropertyInfo.SetValue(tObjectItem,
                //                                        //        (Int64)EditorGUILayout.LongField(tPropertyInfo.Name + " (Int64)",
                //                                        //        (Int64)tPropertyInfo.GetValue(tObjectItem, null)));
                //                                        //}
                //                                        //else if (tPropertyInfo.PropertyType == typeof(long))
                //                                        //{
                //                                        //    tPropertyInfo.SetValue(tObjectItem,
                //                                        //        (long)EditorGUILayout.LongField(tPropertyInfo.Name + " (long)",
                //                                        //        (long)tPropertyInfo.GetValue(tObjectItem, null)));
                //                                        //}
                //                                        //else if (tPropertyInfo.PropertyType == typeof(uint))
                //                                        //{
                //                                        //    tPropertyInfo.SetValue(tObjectItem,
                //                                        //        (uint)EditorGUILayout.LongField(tPropertyInfo.Name + " (uint)",
                //                                        //        (uint)tPropertyInfo.GetValue(tObjectItem, null)));
                //                                        //}
                //                                        //else if (tPropertyInfo.PropertyType == typeof(UInt16))
                //                                        //{
                //                                        //    tPropertyInfo.SetValue(tObjectItem,
                //                                        //        (UInt16)EditorGUILayout.IntField(tPropertyInfo.Name + " (Int16)",
                //                                        //        (UInt16)tPropertyInfo.GetValue(tObjectItem, null)));
                //                                        //}
                //                                        //else if (tPropertyInfo.PropertyType == typeof(UInt32))
                //                                        //{
                //                                        //    tPropertyInfo.SetValue(tObjectItem,
                //                                        //        (UInt32)EditorGUILayout.LongField(tPropertyInfo.Name + " (Int32)",
                //                                        //        (UInt32)tPropertyInfo.GetValue(tObjectItem, null)));
                //                                        //}
                //                                        //else if (tPropertyInfo.PropertyType == typeof(UInt64))
                //                                        //{
                //                                        //    EditorGUILayout.LabelField(tPropertyInfo.Name + " (UInt64)", "not supported");
                //                                        //    //tPropertyInfo.SetValue(tObjectItem,
                //                                        //    //    (UInt64)EditorGUILayout.LongField(tPropertyInfo.Name + " (UInt64)",
                //                                        //    //    (UInt64)tPropertyInfo.GetValue(tObjectItem, null)));
                //                                        //}
                //                                        //else if (tPropertyInfo.PropertyType == typeof(ulong))
                //                                        //{
                //                                        //    EditorGUILayout.LabelField(tPropertyInfo.Name + " (ulong)", "not supported");
                //                                        //    //tPropertyInfo.SetValue(tObjectItem,
                //                                        //    //    (ulong)EditorGUILayout.LongField(tPropertyInfo.Name + " (ulong)",
                //                                        //    //    (ulong)tPropertyInfo.GetValue(tObjectItem, null)));
                //                                        //}
                //                                        //else if (tPropertyInfo.PropertyType == typeof(bool))
                //                                        //{
                //                                        //    tPropertyInfo.SetValue(tObjectItem,
                //                                        //        (bool)EditorGUILayout.ToggleLeft(tPropertyInfo.Name + " (bool)",
                //                                        //        (bool)tPropertyInfo.GetValue(tObjectItem, null)));
                //                                        //}
                //                                        //else if (tPropertyInfo.PropertyType == typeof(Boolean))
                //                                        //{
                //                                        //    tPropertyInfo.SetValue(tObjectItem,
                //                                        //        (Boolean)EditorGUILayout.ToggleLeft(tPropertyInfo.Name + " (Boolean)",
                //                                        //        (Boolean)tPropertyInfo.GetValue(tObjectItem, null)));
                //                                        //}
                //                                        //else
                //                                        //{ 
                //                                        //EditorGUILayout.LabelField(tPropertyInfo.Name + " (" + tPropertyInfo.PropertyType.Name + ")", "not supported");
                //                                        //}
                //                                    }
                //                                    if (EditorGUI.EndChangeCheck() == true)
                //                                    {
                //                                        TabSelectionSelected.ObjectSelected.VirtualData.SetXXXPrefKeyB(tObjectItem, tEnvSelected.EnvName);
                //                                        TabSelectionSelected.ObjectSelected.Update();
                //                                    }
                //                                    //EditorGUILayout.LabelField("JSON", TabSelectionSelected.ObjectSelected.VirtualData.DataJSON);
                //                                }
                //                                break;
                //                            case NWDEnvironmentType.Qualification:
                //                                {
                //                                    EditorGUILayout.LabelField("SELECTED THE BUNDLE FOR EDITION", " BUNDLE LIST");
                //                                    foreach (NWDEnvironmentUnityEditor tEnvRedeable in NWDUnityEngineEditor.Instance.GetConfig().ReturnAllEnabledEnvironments())
                //                                    {
                //                                        if (tEnvRedeable != tEnvSelected && (tEnvRedeable.GetAccessRights() == NWDEnvironmentRights.Read || tEnvRedeable.GetAccessRights() == NWDEnvironmentRights.Write))
                //                                        {
                //                                            if (tEnvRedeable.EnvType == NWDEnvironmentType.Playtest)
                //                                            {
                //                                                GUILayout.Label("Copy from " + tEnvRedeable.InternalName, NWDGUI.KTableSearchTitle);
                //                                            }
                //                                        }
                //                                    }
                //                                    foreach (KeyValuePair<string, string> tField in tInformation.DicoField)
                //                                    {
                //                                        EditorGUILayout.LabelField(tField.Key, tField.Value);
                //                                    }
                //                                }
                //                                break;
                //                            default:
                //                                {
                //                                    foreach (KeyValuePair<string, string> tField in tInformation.DicoField)
                //                                    {
                //                                        EditorGUILayout.LabelField(tField.Key, tField.Value);
                //                                    }
                //                                }
                //                                break;
                //                        }
                //                    }
                //                    else
                //                    {
                //                        GUILayout.Label("No Factory", NWDGUI.KTableSearchTitle);
                //                    };
                //                    GUILayout.EndScrollView();
                //                }
                //                break;
                //        }

                //        EditorGUI.EndDisabledGroup();
                //    }
                //    else
                //    {
                //        GUILayout.BeginHorizontal();
                //        GUILayout.FlexibleSpace();
                //        GUILayout.BeginVertical(GUILayout.Width(300));
                //        GUILayout.FlexibleSpace();
                //        GUILayout.BeginVertical(EditorStyles.helpBox);
                //        if (GUILayout.Button(tInformation.tClass.GetContent(), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4)))
                //        {
                //            NWEScriptOpener.OpenScript(tInformation.tPlan.TheClassType);
                //        }
                //        NWDGUILayout.Separator();
                //        GUILayout.Label("This class is not enabled in this environnement (see environment configuration)", NWDGUI.kInspectorReferenceCenter);
                //        NWDGUILayout.Separator();
                //        if (GUILayout.Button("Show environment configuration"))
                //        {
                //            NWDEnvironmentManagement.SharedInstanceFocus(tEnvSelected);
                //        }
                //        GUILayout.EndVertical();
                //        GUILayout.FlexibleSpace();
                //        GUILayout.EndVertical();
                //        GUILayout.FlexibleSpace();
                //        GUILayout.EndHorizontal();
                //    }
                //}
                //else
                //{
                //    GUILayout.BeginHorizontal();
                //    GUILayout.FlexibleSpace();
                //    GUILayout.BeginVertical(GUILayout.Width(300));
                //    GUILayout.FlexibleSpace();
                //    GUILayout.BeginVertical(EditorStyles.helpBox);
                //    if (GUILayout.Button(tInformation.tClass.GetContent(), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4)))
                //    {
                //        NWEScriptOpener.OpenScript(tInformation.tPlan.TheClassType);
                //    }
                //    NWDGUILayout.Separator();
                //    GUILayout.Label("This class is not enabled in this environnement (see environment configuration)", NWDGUI.kInspectorReferenceCenter);
                //    NWDGUILayout.Separator();
                //    if (GUILayout.Button("Show environment configuration"))
                //    {
                //        NWDEnvironmentManagement.SharedInstanceFocus();
                //    }
                //    GUILayout.EndVertical();
                //    GUILayout.FlexibleSpace();
                //    GUILayout.EndVertical();
                //    GUILayout.FlexibleSpace();
                //    GUILayout.EndHorizontal();

                //}
            }
            else
            {
                //GUILayout.BeginHorizontal();
                //GUILayout.FlexibleSpace();
                //GUILayout.BeginVertical(GUILayout.Width(300));
                //GUILayout.FlexibleSpace();
                //GUILayout.BeginVertical(EditorStyles.helpBox);
                //if (GUILayout.Button(tInformation.tClass.GetContent(), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4)))
                //{
                //    NWEScriptOpener.OpenScript(tInformation.ClassType);
                //}
                //NWDGUILayout.Separator();
                //GUILayout.Label("You have no data in inpector!", NWDGUI.kInspectorReferenceCenter);
                //NWDGUILayout.Separator();
                //GUILayout.EndVertical();
                //GUILayout.FlexibleSpace();
                //GUILayout.EndVertical();
                //GUILayout.FlexibleSpace();
                //GUILayout.EndHorizontal();
            }
        }

        #endregion

        #endregion

    }
}
