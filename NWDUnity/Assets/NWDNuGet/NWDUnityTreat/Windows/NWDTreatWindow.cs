using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Managers;
using NWDUnityEditor.Models;
using NWDUnityEditor.Tools;
using NWDUnityShared.Tools;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace NWDUnityEditor.Windows
{
    public class NWDTreatWindow : NWDUnityEditorWindowBasis
    {
        public string ULongField;
        public ulong ULongFieldValue;
        private NWDService Service;
        private NWDAccountService AccountService;
        private NWDAsyncOperation Operation;
        public Vector2 ScrollPosition;

        public static void SharedInstanceFocus(int sServiceIndex)
        {
            NWDTreatWindow rReturn = GetWindow<NWDTreatWindow>();

            rReturn.Service = NWDUnityTreatManager.Instance.GetService(sServiceIndex);
            rReturn.AccountService = rReturn.Service;

            rReturn.ShowModal();
            rReturn.Focus();
        }

        public void GUITitle()
        {
            NWDGUILayout.Title("Associate a service to an account");
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
            GUITitle();
            DrawInEditor(ref ScrollPosition);
        }

        public void DrawInEditor(ref Vector2 sScrollPosition)
        {
            EditorGUI.BeginDisabledGroup(Operation != null && Operation.State <= NWDAsyncOperationState.Running);
            sScrollPosition = GUILayout.BeginScrollView(sScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            NWDGUILayout.LineWhite();
            NWDGUILayout.SubSection(Service.Name);
            GUILayout.BeginHorizontal();
            //---------
            GUILayout.BeginVertical();
            GUILayout.Label("Service", NWDGUI.KTableSearchTitle);
            EditorGUILayout.LabelField("Reference", Service.Reference.ToString());
            EditorGUILayout.LabelField("Name", Service.Name);
            NWDGUILayout.BigSpace();
            GUILayout.EndVertical();
            //---------
            GUILayout.BeginVertical();
            GUILayout.Label("Message", NWDGUI.KTableSearchTitle);
            EditorGUILayout.LabelField(Service.Message, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2 + 2));
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            NWDGUILayout.Line();

            NWDGUILayout.LineWhite();
            NWDGUILayout.SubSection("Account Service information");
            GUILayout.BeginHorizontal();
            //---------
            GUILayout.BeginVertical();
            ULongField = AccountService.Account.ToString();
            ULongField = EditorGUILayout.TextField("Account Reference", ULongField);
            if (ulong.TryParse(ULongField, out ULongFieldValue))
            {
                AccountService.Account = ULongFieldValue;
            }

            EditorGUILayout.LabelField("Environment", AccountService.EnvironmentKind.ToString());
            NWDGUILayout.BigSpace();
            GUILayout.EndVertical();
            //---------
            GUILayout.BeginVertical();
            AccountService.Start = (int)NWDGUILayout.DateField(new GUIContent("Start date"), AccountService.Start);
            AccountService.End = (int)NWDGUILayout.DateField(new GUIContent("End date"), AccountService.End);

            if (AccountService.Start > AccountService.End)
            {
                AccountService.Start = AccountService.End;
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            NWDGUILayout.Line();

            GUILayout.EndScrollView();
            NWDGUILayout.Line();
            NWDGUILayout.LittleSpace();
            GUILayout.BeginHorizontal();
            if (Operation != null && Operation.State == NWDAsyncOperationState.Failure)
            {
                EditorGUILayout.HelpBox(Operation.Error.Message, MessageType.Error);
            }
            GUILayout.FlexibleSpace();

            bool tDisabled = GUI.enabled;
            GUI.enabled &= AccountService.Account != 0;
            if (GUILayout.Button("Associate service"))
            {
                Operation = NWDUnityTreatManager.Instance.Associate(AccountService);
            }
            GUI.enabled = tDisabled;
            GUILayout.EndHorizontal();
            NWDGUILayout.LittleSpace();
            EditorGUI.EndDisabledGroup();
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

        public override NWDUnityEditorMultiGUIContent ReturnTitle()
        {
            return NWDUnityEditorMultiGUIContent.NewTitle<NWDFakeDeviceConfiguration>("Service association");
        }
    }
}
