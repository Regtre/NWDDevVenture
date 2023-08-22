using NWDFoundation.Configuration.Environments;
using NWDFoundation.Configuration.Permissions;
using NWDUnityEditor.Constants;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace NWDUnityEditor.Windows
{
    [Serializable]
    public class NWDEnvironmentConfiguration : NWDUnityEditorWindowBasis
    {
        private GUIStyle TabStyle;

        private Dictionary<NWDEnvironmentKind, NWDDataTrackDescription[]> K_EnvironmentByKind = null;

        public NWDEnvironmentConfigurationPanel PanelSelected = NWDEnvironmentConfigurationPanel.Infos;
        public GUIContent[] mPanelContentList;

        public Vector2 ScrollPosition;
        public Vector2 ScrollPositionMenu;
        public NWDSplitArea SplitArea = NWDSplitArea.NewArea(NWDSplitDirection.Vertical);
        public AdvancedDropdownState EnvironmentDropdownState;

        private NWDCustomDropdown<NWDDataTrackDescription, NWDEnvironmentKind> EnvironmentDropdown;
        private NWDDataTrackDescription EnvironmentSelected;

        public static void SharedInstanceFocus()
        {
            NWDEnvironmentConfiguration rReturn = GetWindow<NWDEnvironmentConfiguration>();
            rReturn.ShowMe();
            rReturn.Focus();
        }
        public static void SharedInstanceFocus(NWDDataTrackDescription sSelectedEnvironmentUnityEditor)
        {
            NWDEnvironmentConfiguration rReturn = GetWindow<NWDEnvironmentConfiguration>();
            SharedInstanceFocus();
            rReturn.EnvironmentSelected = sSelectedEnvironmentUnityEditor;
        }

        private void OnFocus()
        {
            InitData();
        }

        [NonSerialized]
        static private Dictionary<NWDEnvironmentConfigurationPanel, NWDUnityEditorMultiGUIContent> kPanelGuiContents = new Dictionary<NWDEnvironmentConfigurationPanel, NWDUnityEditorMultiGUIContent>();
        static GUIContent GetGUIContent(NWDEnvironmentConfigurationPanel sPanel)
        {
            GUIContent rReturn = null;
            if (kPanelGuiContents.ContainsKey(sPanel) == true)
            {
                rReturn = kPanelGuiContents[sPanel].GetContent();
            }
            else
            {
                NWDUnityEditorMultiGUIContent tReturn = NWDUnityEditorMultiGUIContent.NewContent("NWDWindowPanel" + sPanel.ToString(), sPanel.ToString().Replace("_", " "));
                kPanelGuiContents.Add(sPanel, tReturn);
                rReturn = tReturn.GetContent();
            }
            return rReturn;
        }
        public void GUITitle()
        {
            NWDGUILayout.Title("Environments configurations");
        }

        public override void OnPreventGUI_InCompileTime()
        {
            GUITitle();
            base.OnPreventGUI_InCompileTime();
        }

        public override void OnPreventGUI_InPlayingMode()
        {
            GUITitle();
            SplitArea.OnGUILayout(this);

            SplitArea.BeginAreaOne();

            NWDGUILayout.Section("Runtime environment");
            NWDGUILayout.LittleSpace();
            NWDGUILayout.LineWhite();
            NWDDataTrackDescription tSelectedDataTrack = NWDUnityEngineEditor.Instance.GetConfig().GetSelectedEnvironment();
            if (tSelectedDataTrack == EnvironmentSelected)
            {
                NWDGUI.BeginRedArea();
            }
            if (GUILayout.Button(tSelectedDataTrack.GetGUIContent(), TabStyle))
            {
                EnvironmentSelected = tSelectedDataTrack;
                RemoveFieldFocus();
            }
            NWDGUI.EndRedArea();
            NWDGUILayout.Line();
            NWDGUILayout.LittleSpace();

            SplitArea.EndAreaOne();

            SplitArea.BeginAreaTwo();
            GUILayout.Label(EnvironmentSelected.GetGUIContent(), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4));
            //if (string.IsNullOrEmpty(EnvironmentSelected.InternalDescription) == true)
            //{
            //    EnvironmentSelected.InternalDescription = string.Empty;
            //}
            //GUILayout.Label(EnvironmentSelected.InternalDescription, NWDGUI.KTableSearchClassIcon);

            PanelSelection();
            NWDGUILayout.Line();
            switch (PanelSelected)
            {
                case NWDEnvironmentConfigurationPanel.Infos:
                    {
                        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        base.OnPreventGUI_InPlayingMode();
                        GUILayout.EndScrollView();
                        //EnvironmentSelected.LayoutInfosForm();
                    }
                    break;
                case NWDEnvironmentConfigurationPanel.Database:
                    {
                        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        //EnvironmentSelected.LayoutDatabaseForm();
                        GUILayout.EndScrollView();
                    }
                    break;
                case NWDEnvironmentConfigurationPanel.Fake_Account:
                    {
                        //EnvironmentSelected.LayoutFakeAccountForm(State, ref ScrollPosition);
                    }
                    break;
                case NWDEnvironmentConfigurationPanel.Classes_Models:
                    {
                        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        base.OnPreventGUI_InPlayingMode();
                        //EnvironmentSelected.LayoutClassesForm(K_ModuleTypeDico);
                        GUILayout.EndScrollView();
                    }
                    break;
                case NWDEnvironmentConfigurationPanel.Localization:
                    {
                        ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                        base.OnPreventGUI_InPlayingMode();
                        //EnvironmentSelected.LayoutLocalizationForm();
                        GUILayout.EndScrollView();
                    }
                    break;
                case NWDEnvironmentConfigurationPanel.Cluster:
                    ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    base.OnPreventGUI_InPlayingMode();
                    //EnvironmentSelected.LayoutClusterForm();
                    GUILayout.EndScrollView();
                    break;
            }

            SplitArea.EndAreaTwo();
        }

        public override void OnPreventGUI_InEditorMode()
        {
            GUITitle();

            SplitArea.OnGUILayout(this);

            SplitArea.BeginAreaOne();

            if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.DownArrow || Event.current.keyCode == KeyCode.UpArrow) && (SplitArea.AreaOne.Contains(Event.current.mousePosition) || SplitArea.AreaTwo.Contains(Event.current.mousePosition)))
            {
                int tAdd = 1;
                if (Event.current.keyCode == KeyCode.UpArrow)
                {
                    tAdd = -1;
                }
                // I merge all environement in Enable and disable env on one array

                NWDDataTrackDescription[] tListToUse = NWDUnityEngineEditor.Instance.GetConfig().GetEnvironments();
                //if (NWDUnityEngineEditor.Instance.GetConfig().EnvironmentsToShow == NWDEnvironmentState.All)
                //{
                //    foreach (NWDDataTrackDescription tEnv in NWDUnityEngineEditor.Instance.GetConfig().GetEnabledEnvironments())
                //    {
                //        tListToUse.Add(tEnv);
                //    }
                //    foreach (NWDDataTrackDescription tEnv in NWDUnityEngineEditor.Instance.GetConfig().GetDisabledEnvironments())
                //    {
                //        tListToUse.Add(tEnv);
                //    }
                //}
                //else if (NWDUnityEngineEditor.Instance.GetConfig().EnvironmentsToShow == NWDEnvironmentState.EnabledOnly)
                //{
                //    foreach (NWDDataTrackDescription tEnv in NWDUnityEngineEditor.Instance.GetConfig().GetEnabledEnvironments())
                //    {
                //        tListToUse.Add(tEnv);
                //    }
                //}
                //else if (NWDUnityEngineEditor.Instance.GetConfig().EnvironmentsToShow == NWDEnvironmentState.Disabled)
                //{
                //    foreach (NWDEnvironmentUnityEditor tEnv in NWDUnityEngineEditor.Instance.GetConfig().ReturnAllDisabledEnvironments())
                //    {
                //        tListToUse.Add(tEnv);
                //    }
                //}

                int tIndex = Array.IndexOf(tListToUse, EnvironmentSelected) + tAdd;
                if (tIndex >= 0 && tListToUse.Length > tIndex)
                {
                    EnvironmentSelected = tListToUse[tIndex];
                    RemoveFieldFocus();
                }
                RepaintMe();
                RemoveFieldFocus();
                Event.current.Use();
            }
            NWDDataTrackDescription tCurrentDataTrack = NWDUnityEngineEditor.Instance.GetConfig().GetSelectedEnvironment();
            ScrollPositionMenu = GUILayout.BeginScrollView(ScrollPositionMenu, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            NWDGUILayout.Section("Current Data Track");
            NWDGUILayout.LineWhite();

            Rect tRect = GUILayoutUtility.GetLastRect();
            tRect.y += 40;
            if (tCurrentDataTrack != null)
            {
                if (GUILayout.Button(tCurrentDataTrack.GetGUIContent(), TabStyle))
                {
                    EnvironmentDropdown.Show(tRect);
                }
            }
            else
            {
                if (GUILayout.Button(new GUIContent("None"), TabStyle))
                {
                    EnvironmentDropdown.Show(tRect);
                }
            }
            NWDGUILayout.Line();
            NWDGUILayout.LittleSpace();

            foreach (KeyValuePair<NWDEnvironmentKind, NWDDataTrackDescription[]> tEnvironments in K_EnvironmentByKind)
            {
                NWDGUILayout.Section(tEnvironments.Key.ToString());

                foreach (NWDDataTrackDescription tDataTrack in tEnvironments.Value)
                {
                    NWDGUILayout.LineWhite();
                    if (tDataTrack == EnvironmentSelected)
                    {
                        NWDGUI.BeginRedArea();
                    }
                    if (GUILayout.Button(tDataTrack.GetGUIContent(), TabStyle))
                    {
                        EnvironmentSelected = tDataTrack;
                        RemoveFieldFocus();
                    }
                    NWDGUI.EndRedArea();
                    NWDGUILayout.Line();
                }

                NWDGUILayout.LittleSpace();
            }

            GUILayout.EndScrollView();

            SplitArea.EndAreaOne();

            SplitArea.BeginAreaTwo();

            if (EnvironmentSelected != null)
            {
                if (GUILayout.Button(EnvironmentSelected.GetGUIContent(), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4)))
                {
                }
                //if (string.IsNullOrEmpty(EnvironmentSelected.InternalDescription) == false)
                //{
                //    GUILayout.Label(EnvironmentSelected.InternalDescription, NWDGUI.KTableSearchClassIcon);
                //}
                PanelSelection();
                NWDGUILayout.Line();
                EditorGUI.BeginChangeCheck();
                switch (PanelSelected)
                {
                    case NWDEnvironmentConfigurationPanel.Infos:
                        {
                            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                            //EnvironmentSelected.LayoutInfosForm();
                            GUILayout.EndScrollView();
                        }
                        break;
                    case NWDEnvironmentConfigurationPanel.Cluster:
                        {
                            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                            //EnvironmentSelected.LayoutClusterForm();
                            GUILayout.EndScrollView();
                        }
                        break;
                    case NWDEnvironmentConfigurationPanel.Localization:
                        {
                            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                            //EnvironmentSelected.LayoutLocalizationForm();
                            GUILayout.EndScrollView();
                        }
                        break;
                    case NWDEnvironmentConfigurationPanel.Database:
                        {
                            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                            //EnvironmentSelected.LayoutDatabaseForm();
                            GUILayout.EndScrollView();
                        }
                        break;
                    case NWDEnvironmentConfigurationPanel.Fake_Account:
                        {
                            //EnvironmentSelected.LayoutFakeAccountForm(State, ref ScrollPosition);
                        }
                        break;
                    case NWDEnvironmentConfigurationPanel.Classes_Models:
                        {
                            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                            //EnvironmentSelected.LayoutClassesForm(K_ModuleTypeDico);
                            GUILayout.EndScrollView();
                        }
                        break;
                    case NWDEnvironmentConfigurationPanel.Build_Options:
                        {
                            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                            //EnvironmentSelected.BuildOptionsForm();
                            GUILayout.EndScrollView();
                        }
                        break;
                }
                if (EditorGUI.EndChangeCheck() == true)
                {
                    //Debug.Log("Change detected!");
                    //EnvironmentSelected.Prepare();
                    EnvironmentSelected.RefereshGUIContent();
                    NWDUnityEditorWindowReimport.RepaintAll();
                }
            }
            else
            {
                NWDGUILayout.DrawMessage("No Data-track selected", "Please select a DataTrack.");
            }

            SplitArea.EndAreaTwo();
        }

        private void PanelSelection()
        {
            GUILayout.Space(NWDGUI.kFieldMarge);
            GUILayout.BeginHorizontal();
            PanelSelected = (NWDEnvironmentConfigurationPanel)GUILayout.Toolbar((int)PanelSelected, mPanelContentList, NWDGUI.KTableClassToolbar, GUILayout.Height(NWDGUI.KTAB_TOOLBAR_HEIGHT));
            GUILayout.EndHorizontal();
            GUILayout.Space(NWDGUI.kFieldMarge);
        }


        public override void OnEnableFromConstructor()
        {
            OnEnableCommons();
        }

        public override void OnEnableFromSerialization()
        {
            OnEnableCommons();
        }

        public override void OnDisableWindow()
        {
        }

        public override void ReloadStyle()
        {
            TabStyle = new GUIStyle(EditorStyles.toolbarButton);
            TabStyle.imagePosition = ImagePosition.ImageLeft;
            TabStyle.fixedHeight = 40;
            TabStyle.alignment = TextAnchor.MiddleLeft;

            List<GUIContent> tPanelContentList = new List<GUIContent>();
            foreach (string tEnumName in Enum.GetNames(typeof(NWDEnvironmentConfigurationPanel)))
            {
                NWDEnvironmentConfigurationPanel tValue = (NWDEnvironmentConfigurationPanel)Enum.Parse(typeof(NWDEnvironmentConfigurationPanel), tEnumName);
                tPanelContentList.Add(GetGUIContent(tValue));
            }
            mPanelContentList = tPanelContentList.ToArray();
        }
        public void OnEnableCommons()
        {
            SplitArea.Min = 180;
            SplitArea.Split = 0.25F;
            EnvironmentSelected = NWDUnityEngineEditor.Instance.GetConfig().GetSelectedEnvironment();
        }

        public override NWDUnityEditorMultiGUIContent ReturnTitle()
        {
            return NWDUnityEditorMultiGUIContent.NewTitle<NWDEnvironmentConfiguration>(NWDConstantsUnityEditor.K_ENVIRONMENTS_CONFIGURATION_TITLE);
        }

        private void InitData()
        {
            NWDDataTrackDescription[] tDataTracks = NWDUnityEngineEditor.Instance.GetConfig().GetEnvironments();

            K_EnvironmentByKind = new Dictionary<NWDEnvironmentKind, NWDDataTrackDescription[]>();

            foreach (NWDEnvironmentKind tKind in tDataTracks.Select(x => x.Kind).Distinct())
            {
                K_EnvironmentByKind.Add(tKind, tDataTracks.Where(x => x.Kind == tKind).ToArray());
            }

            EnvironmentDropdownState = new AdvancedDropdownState();
            EnvironmentDropdown = new NWDCustomDropdown<NWDDataTrackDescription, NWDEnvironmentKind>(EnvironmentDropdownState, NWDUnityEngineEditor.Instance.GetConfig().GetEnvironments(), "Data Track", x => x.Name, x => x.ToString(), x => x.Kind, x => NWDUnityEngineEditor.Instance.GetConfig().SetSelectedEnvironment(x));
        }
    }
}
