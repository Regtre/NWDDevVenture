using NWDUnityEditor.Engine;
using NWDUnityEditor.Tools;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using System;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Windows
{
    public enum NWDUnityEditorWindowBasisState
    {
        InEngineStarting,
        InCompileTime,
        InPlayingMode,
        InEditorMode,
        InConfigLoading,
    }
    public interface INWDUnityEditorWindow
    {
        public void OnEnableFromConstructor();
        public void OnEnableFromSerialization();
        public NWDUnityEditorMultiGUIContent ReturnTitle();
    }

    [Serializable]
    public abstract class NWDUnityEditorWindowBasis : EditorWindow, INWDUnityEditorWindow, ISerializationCallbackReceiver, IHasCustomMenu
    {
       static private NWDUnityEditorMultiGUIContent _IconAndTitleCompile;//=  NWDUnityEditorMultiGUIContent.NewContent("NWDEditorWindowCompileTime");
        static public NWDUnityEditorMultiGUIContent IconAndTitleCompile
        {
            get
            {
                if (_IconAndTitleCompile == null)
                {
                    _IconAndTitleCompile = NWDUnityEditorMultiGUIContent.NewContent("NWDEditorWindowCompileTime");
                }
                
                return _IconAndTitleCompile;
            }
        }
        static private NWDUnityEditorMultiGUIContent _IconAndTitlePlaymode;//=  NWDUnityEditorMultiGUIContent.NewContent("NWDEditorWindowPlaymode");
        static public NWDUnityEditorMultiGUIContent IconAndTitlePlaymode { get { if (_IconAndTitlePlaymode == null) { _IconAndTitlePlaymode = NWDUnityEditorMultiGUIContent.NewContent("NWDEditorWindowPlaymode"); } return _IconAndTitlePlaymode; } }
        static private NWDUnityEditorMultiGUIContent _IconAndTitleEditmode;// =  NWDUnityEditorMultiGUIContent.NewContent("NWDEditorWindowEditmode");
        static public NWDUnityEditorMultiGUIContent IconAndTitleEditmode { get { if (_IconAndTitleEditmode == null) { _IconAndTitleEditmode = NWDUnityEditorMultiGUIContent.NewContent("NWDEditorWindowEditmode"); } return _IconAndTitleEditmode; } }

        public NWDUnityEditorMultiGUIContent TitleMulti;

        [NonSerialized]
        public NWDUnityEditorWindowBasisState State;
        /// <summary>
        /// Use to create standard window size to screenshoot
        /// </summary>
        public float NormalizeWidth = 300;
        /// <summary>
        /// Use to create standard window size to screenshoot
        /// </summary>
        public float NormalizeHeight = 300;
        /// <summary>
        /// Use to min width
        /// </summary>
        public float MinWidth = 100;
        /// <summary>
        /// Use to min height
        /// </summary>
        public float MinHeight = 100;
        /// <summary>
        /// Use to min width
        /// </summary>
        public float MaxWidth = 2048;
        /// <summary>
        /// Use to min height
        /// </summary>
        public float MaxHeight = 2048;

        protected NWDUnityEditorFromSerialization FromSerialization;
        protected bool _RemoveFieldFocus = false;
        protected int _RepaintMe = 0;
        protected bool _ReloadStyle = false;

        private bool CloseNow = false;

        private NWDGUIAsyncOperation EngineLaunchOperation;

        public void CloseAfterGUI()
        {
            CloseNow = true;
        }

        public virtual void ReloadStyle()
        {

        }
        /// <summary>
        /// Resize the window with constraints
        /// </summary>
        public void NormalizeSize()
        {
            minSize = new Vector2(NormalizeWidth, NormalizeHeight);
            maxSize = minSize;
            _RemoveFieldFocus = true;
        }

        /// <summary>
        /// Free size the window
        /// </summary>
        public void FreeSize()
        {
            minSize = new Vector2(MinWidth, MinHeight);
            maxSize = new Vector2(MaxWidth, MaxHeight);
            _RemoveFieldFocus = true;
        }

        public virtual void DetachAsWindow()
        {
            _RemoveFieldFocus = true;
            Type tType = this.GetType();
            NWDUnityEditorWindowBasis tNew = (NWDUnityEditorWindowBasis)ScriptableObject.CreateInstance(tType);
            tNew.ShowUtility();
            tNew.Focus();
            tNew._RemoveFieldFocus = true;
            Close();
        }

        public void TutorialOnline()
        {
            if (string.IsNullOrEmpty(TutorialLink()) == false)
            {
                Application.OpenURL(TutorialLink());
            }
        }

        public virtual string TutorialLink(string sLink = "")
        {
            return sLink;
        }

        public virtual void AddItemsToMenu(GenericMenu menu)
        {
            //menu.AddSeparator("NEW WINDOW SYSTEM");
            //menu.AddSeparator("");

            menu.AddItem(new GUIContent("Detach in window"), false, DetachAsWindow);
            menu.AddItem(new GUIContent("Normalize size window " + NormalizeWidth.ToString("0") + "x" + NormalizeHeight.ToString("0")), false, NormalizeSize);
            menu.AddItem(new GUIContent("Free size window " + position.width.ToString("0") + "x" + position.height.ToString("0") + ""), false, FreeSize);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Visualize script"), false, ScriptOpener, this.GetType());
            if (string.IsNullOrEmpty(TutorialLink()) == false)
            {
                menu.AddItem(new GUIContent("Tutorial online"), false, TutorialOnline);
            }
            menu.AddSeparator("");
        }

        public void OnGUI()
        {
            if (_ReloadStyle == false)
            {
                _ReloadStyle = true;
                ReloadStyle();
            }
            NWDGUI.LoadStyles();

            if (EngineLaunchOperation == null)
            {
                EngineLaunchOperation = NWDUnityEngine.Instance.LaunchOperation;
            }

            if (NWDUnityEngineEditor.Instance.GetConfig() != null)
            {
                if (TitleMulti != null)
                {
                    titleContent = TitleMulti.GetContent();
                }
                else
                {
                    titleContent = NWDUnityEditorMultiGUIContent.LogoEditor.GetContent();
                }
                if (!EngineLaunchOperation.IsDone)
                {
                    State = NWDUnityEditorWindowBasisState.InEngineStarting;
                    OnPreventGUI_InEngineStarting();
                }
                else if (NWDUnityEngineEditor.Instance.GetConfig() != null)
                {
                    if (EditorApplication.isCompiling == false)
                    {
                        if (Application.isPlaying == true)
                        {
                            State = NWDUnityEditorWindowBasisState.InPlayingMode;
                            OnPreventGUI_InPlayingMode();
                        }
                        else
                        {
                            State = NWDUnityEditorWindowBasisState.InEditorMode;
                            OnPreventGUI_InEditorMode();
                        }
                    }
                    else
                    {
                        State = NWDUnityEditorWindowBasisState.InCompileTime;
                        OnPreventGUI_InCompileTime();
                    }
                }
                else
                {
                    //WindowContent.Add_Loading();
                }
                if (NWDUnityEngineEditor.Instance.GetConfig().GetShowLogo() == true)
                {
                    GUI.Label(new Rect(position.width - 48.0F + 4F, -4F, 48.0F, 48.0F), NWDUnityEditorMultiGUIContent.LogoWindows.GetContent());
                }
                if (_RemoveFieldFocus == true)
                {
                    _RemoveFieldFocus = false;
                    _RepaintMe = 1;
                    GUI.FocusControl(null);
                }
            }
            else
            {
                State = NWDUnityEditorWindowBasisState.InConfigLoading;
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(IconAndTitleCompile.GetContent(), NWDGUI.kIconCenterStyle);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(10.0F);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("... loading config in progress ...");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }

            if (CloseNow == true)
            {
                Close();
            }
        }
        protected void RemoveFieldFocus()
        {
            _RemoveFieldFocus = true;
        }

        public void RepaintMe()
        {
            _RepaintMe = 2;
        }

        public void Awake()
        {
            //Debug.Log(this.GetType().Name + "." + nameof(Awake) + "();");
        }

        void Update()
        {
            if (_RepaintMe > 0)
            {
                _RepaintMe--;
                //Debug.Log("Update() EventType.ScrollWheel move detected on y updateNeed = " + _RepaintMe);
                Repaint();
            }
        }

        public void OnValidate()
        {
            //Debug.Log(this.GetType().Name + "." + nameof(OnValidate) + "();");
        }

        public void Reset()
        {
            //Debug.Log(this.GetType().Name + "." + nameof(Reset) + "();");
        }

        public void OnDestroy()
        {
            //Debug.Log(this.GetType().Name + "." + nameof(OnDestroy) + "();");
        }

        public void OnEnable()
        {
            TitleMulti = ReturnTitle();
            NWDUnityEditorWindowReimport.AddWindow(this);
            if (FromSerialization != NWDUnityEditorFromSerialization.fromSerialization)
            {
                FromSerialization = NWDUnityEditorFromSerialization.fromConstructor;
                OnEnableFromConstructor();
            }
            else
            {
                OnEnableFromSerialization();
            }
        }

        public virtual void OnConfigUpdate()
        {
            //Debug.Log(this.GetType().Name + "." + nameof(OnConfigUpdate) + "();");
        }

        public abstract void OnEnableFromConstructor();

        public abstract void OnEnableFromSerialization();

        public abstract void OnDisableWindow();

        public abstract NWDUnityEditorMultiGUIContent ReturnTitle();

        public void OnDisable()
        {
            NWDUnityEditorWindowReimport.RemoveWindow(this);
            OnDisableWindow();
        }

        public virtual void OnPreventGUI_InEngineStarting()
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(IconAndTitleCompile.GetContent(), NWDGUI.kIconCenterStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10.0F);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("... engine starting ...");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }

        public virtual void OnPreventGUI_InCompileTime()
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(IconAndTitleCompile.GetContent(), NWDGUI.kIconCenterStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10.0F);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("... compile in progress ...");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }

        public virtual void OnPreventGUI_InPlayingMode()
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(IconAndTitlePlaymode.GetContent(), NWDGUI.kIconCenterStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10.0F);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("... not editable in playing mode ...");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }

        public virtual void OnPreventGUI_InEditorMode()
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(IconAndTitleEditmode.GetContent(), NWDGUI.kIconCenterStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10.0F);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("... not editable in editor mode ...");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }

        public void ScriptOpener(object sObject)
        {
            NWEScriptOpener.OpenScript((Type)sObject);
        }

        public static bool ShowAsWindow()
        {
            return NWDUnityEngineEditor.Instance.GetConfig().GetWindowStyle() == NWDUnityEditorWindowStyle.Window;
        }

        public void ShowMe()
        {
            switch (NWDUnityEngineEditor.Instance.GetConfig().GetWindowStyle())
            {
                case NWDUnityEditorWindowStyle.Tab:
                    {
                        Show();
                    }
                    break;
                case NWDUnityEditorWindowStyle.Window:
                    {
                        ShowUtility();
                    }
                    break;
            }
        }

        public virtual void OnBeforeSerialize()
        {
            //Debug.Log(this.GetType().Name + "." + nameof(OnBeforeSerialize) + "() " + FromSerialization.ToString());

        }

        public virtual void OnAfterDeserialize()
        {
            //Debug.Log(this.GetType().Name + "." + nameof(OnAfterDeserialize) + "() " + FromSerialization.ToString());
            FromSerialization = NWDUnityEditorFromSerialization.fromSerialization;
            //Debug.Log(this.GetType().Name + "." + nameof(OnAfterDeserialize) + "() " + FromSerialization.ToString());
        }
    }
}