using NWDFoundation.Logger;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Requestors;
using NWDUnityEditor.SQLite;
using NWDUnityEditor.Tools;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Windows
{
    public class NWDOnBoarding : NWDUnityEditorWindowBasis
    {
        public Vector2 ScrollPosition;
        private string PublicToken = string.Empty;
        private string PrivateToken = string.Empty;
        private string Nickname = string.Empty;
        private bool OpenChangeUrlModal = false;

        private NWDRequestorRecurrentTask Reccurent = null;
        private NWDRequestorTask Task = null;

        static NWDOnBoarding kShareInstance = null;

        public override NWDUnityEditorMultiGUIContent ReturnTitle()
        {
            return NWDUnityEditorMultiGUIContent.NewTitle<NWDOnBoarding>("Welcome to Net-Worked-Data©");
        }

        public static void SharedInstanceFocus()
        {
            if (kShareInstance == null)
            {
                kShareInstance = (NWDOnBoarding)ScriptableObject.CreateInstance(typeof(NWDOnBoarding));
            }
            kShareInstance.Nickname = NWDUnityEngineEditor.Instance.GetConfig().GetNickname();
            kShareInstance.ShowUtility();
            kShareInstance.Focus();
        }

        public void GUITitle()
        {
            NWDGUILayout.Title("Welcome to Net-Worked-Data©");
        }

        public override void OnPreventGUI_InCompileTime()
        {
            GUITitle();
            base.OnPreventGUI_InCompileTime();
        }

        public override void OnPreventGUI_InPlayingMode()
        {
            GUITitle();
            base.OnPreventGUI_InPlayingMode();
        }

        public override void OnPreventGUI_InEditorMode()
        {
            if (Reccurent == null)
            {
                Reccurent = NWDSettingsRequestor.GetCurrent().GetRecurrentAuthentication();
            }
            OpenChangeUrlModal = false;
            GUITitle();
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            // Start scrollview
            if (NWDUnityEngineEditor.Instance.GetConfig().IsReady() == false)
            {
                NWDGUILayout.Section("Instructions");
                GUILayout.BeginVertical(EditorStyles.helpBox);
                //NWDGUILayout.Title("I have an account on Net-Worked-Data");
                GUILayout.BeginHorizontal();
                NWDGUILayout.Label(" • Go to website " + NWDUnityEngineEditor.Instance.GetConfig().WebEditorURL());
                if (GUILayout.Button("Change Hub", GUILayout.MaxWidth(200)))
                {
                    OpenChangeUrlModal = true;
                }
                EditorGUI.BeginDisabledGroup(NWDUnityEngineEditor.Instance.GetConfig().WebEditorURL() == NWDUnityEngineEditor.Instance.GetConfig().GetDefaultWebEditor());
                if (GUILayout.Button("Reset", GUILayout.MaxWidth(200)))
                {
                    NWDUnityEngineEditor.Instance.GetConfig().SetEditorURL(NWDUnityEngineEditor.Instance.GetConfig().GetDefaultWebEditor());
                }
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();
                NWDGUILayout.Label(" • Signup or signin");
                NWDGUILayout.Label(" • Create or select your project");
                NWDGUILayout.Label(" • Select your role and copy its tokens");
                NWDGUILayout.Label(" • Paste tokens in the fields below");
                PublicToken = EditorGUILayout.TextField("Role's Public Token", PublicToken);
                PrivateToken = EditorGUILayout.PasswordField("Role's Private Token", PrivateToken);
                NWDGUILayout.Label(" • Choose a nickname");

                EditorGUI.BeginChangeCheck();
                Nickname = EditorGUILayout.TextField("Nickname", Nickname);
                if (EditorGUI.EndChangeCheck())
                {
                    NWDUnityEngineEditor.Instance.GetConfig().SetNickname(Nickname);
                }
                NWDGUILayout.Label(" • Clik on \"Authentification\" ");

                EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(PrivateToken) || string.IsNullOrEmpty(PublicToken) || string.IsNullOrEmpty(Nickname) || (Task?.IsProcessing() ?? false));
                if (GUILayout.Button("Authentification"))
                {
                    Task = NWDSettingsRequestor.GetCurrent().EnqueueTask(new NWDAuthenticationRequestor(PublicToken, PrivateToken, true, null));
                    RemoveFieldFocus();
                }
                NWDGUILayout.LittleSpace();
                EditorGUI.EndDisabledGroup();
                if (Task != null && Task.State == NWDRequestorTaskState.Error)
                {
                    EditorGUILayout.HelpBox(Task.exception.Message, MessageType.Error);
                }
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(EditorStyles.helpBox);
                NWDGUILayout.Section("Project's informations");
                EditorGUILayout.LabelField("Name", NWDUnityEngineEditor.Instance.GetConfig().GetConfigName());
                EditorGUILayout.LabelField("Reference", NWDUnityEngineEditor.Instance.GetConfig().GetProjectId().ToString());
                NWDGUILayout.LittleSpace();

                NWDGUILayout.Section("Role's informations");
                EditorGUILayout.LabelField("Role Name", NWDUnityEngineEditor.Instance.GetConfig().GetRoleName());
                NWDGUILayout.Line();
                EditorGUILayout.LabelField("Create MetaData", NWDUnityEngineEditor.Instance.GetConfig().GetCanCreateMetaData().ToString());
                EditorGUILayout.LabelField("Edit MetaInfos", NWDUnityEngineEditor.Instance.GetConfig().GetCanEditMetaDataInfos().ToString());
                NWDGUILayout.Line();
                //foreach (NWDEnvironmentUnityEditor tEnv in NWDUnityEngineEditor.Instance.GetConfig().ReturnAllEnvironments())
                //{
                //    EditorGUILayout.LabelField(tEnv.GetGUIContent(), new GUIContent(tEnv.GetAccessRights().ToString()));
                //}
                //NWDGUILayout.Line();
                EditorGUI.BeginDisabledGroup(Reccurent.IsProcessing());
                if (GUILayout.Button("Refresh Rights"))
                {
                    Reccurent.ForceRun();
                }
                EditorGUI.EndDisabledGroup();
                NWDGUILayout.LittleSpace();
                if (Reccurent.State == NWDRequestorTaskState.Error)
                {
                    EditorGUILayout.HelpBox(Reccurent.exception.Message, MessageType.Error);
                    NWDGUILayout.LittleSpace();
                }

                NWDGUILayout.Section("Local informations");
                EditorGUI.BeginChangeCheck();
                Nickname = EditorGUILayout.TextField("Nickname", Nickname);
                if (EditorGUI.EndChangeCheck())
                {
                    NWDUnityEngineEditor.Instance.GetConfig().SetNickname(Nickname);
                }

                EditorGUILayout.LabelField("Footprint", SystemInfo.deviceUniqueIdentifier);

                EditorGUI.BeginChangeCheck();
                NWDUnityEditorWindowStyle tWindowStymle = NWDUnityEngineEditor.Instance.GetConfig().GetWindowStyle();
                tWindowStymle = (NWDUnityEditorWindowStyle)EditorGUILayout.EnumPopup("Window style", tWindowStymle);
                if (EditorGUI.EndChangeCheck())
                {
                    NWDUnityEngineEditor.Instance.GetConfig().SetWindowStyle(tWindowStymle);
                }

                EditorGUI.BeginChangeCheck();
                bool tShowLogo = NWDUnityEngineEditor.Instance.GetConfig().GetShowLogo();
                tShowLogo = EditorGUILayout.Toggle("Show logo content", tShowLogo);
                if (EditorGUI.EndChangeCheck())
                {
                    NWDUnityEngineEditor.Instance.GetConfig().SetShowLogo(tShowLogo);
                }
                NWDGUILayout.LittleSpace();

                NWDGUILayout.Section("Reset token");
                NWDGUI.BeginRedArea();
                if (GUILayout.Button("Reset token"))
                {
                    RemoveFieldFocus();
                    NWDUnityEngineEditor.Instance.GetConfig().ResetRoleTokens();
                    PrivateToken = string.Empty;
                    PublicToken = string.Empty;
                }
                NWDGUI.EndRedArea();
                NWDGUILayout.LittleSpace();

                NWDGUILayout.Section("Local Meta database");
                string sPath = SQLiteConnector.GetVirtualDatabasPath();
                EditorGUILayout.BeginHorizontal();
                if (File.Exists(sPath))
                {
                    if (GUILayout.Button("Reveal Meta database"))
                    {
                        EditorUtility.OpenWithDefaultApp(Path.GetDirectoryName(sPath));
                    }
                }
                else
                {
                    GUILayout.Label("Database " + sPath + " doesn't exist");
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(300));
                //NWDPlanDescription tPlanDescription = NWDPlanDescription.GetForPlan(NWDUnityEngineEditor.Instance.GetConfig().Plan);
                //tPlanDescription.Draw();
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.Space(10);
            }

            NWDGUILayout.Section("Visit Net-Worked-Data Website");
            if (GUILayout.Button("Visite Website"))
            {
                NWDUnityEngineEditor.Instance.GetConfig().ManagePlanByWebsite();
            }

            GUILayout.Space(20);
            // Finish scrollview
            GUILayout.EndScrollView();

            if (OpenChangeUrlModal) // Do this to prevent GUI Error on modal open.
            {
                NWDChangeWebsiteURL.SharedInstanceFocus();
            }
        }

        public override void OnEnableFromConstructor()
        {
        }

        public override void OnEnableFromSerialization()
        {
        }

        public override void OnDisableWindow()
        {
        }
    }
}