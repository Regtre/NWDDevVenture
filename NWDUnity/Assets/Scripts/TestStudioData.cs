using NWDFoundation.Models;
using NWDStandardModels.Models;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using System.Collections.Generic;
using UnityEngine;

public class TestStudioData : MonoBehaviour
{
    NWDGUIAsyncOperation SyncOperation;
    Vector2 ListScrollView;
    Vector2 DisplayScrollView;

    NWDTestStudioData StudioData = null;
    List<NWDTestStudioData> StudioDataList = new List<NWDTestStudioData>();

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        ListStudioData();

        GUILayout.BeginVertical(GUI.skin.box, GUILayout.MinWidth(250));
        if (GUILayout.Button("Update list from cache"))
        {
            OnSyncDone(null);
        }
        GUI.enabled = SyncOperation == null || SyncOperation.IsDone;
        if (GUILayout.Button("Synchronization"))
        {
            SyncOperation = NWDUnityEngine.Instance.DataManager.Synchronize();
            SyncOperation.OnDone = OnSyncDone;
        }
        GUI.enabled = true;
        if (SyncOperation != null)
        {
            if (SyncOperation.IsDone)
            {
                GUILayout.Label(SyncOperation.State.ToString());
            }
            if (SyncOperation.Error != null)
            {
                GUILayout.Label(SyncOperation.Error.Message);
            }
        }
        StudioDataDisplayer();
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }

    private void ListStudioData()
    {
        ListScrollView = GUILayout.BeginScrollView(ListScrollView, GUI.skin.box, GUILayout.MinWidth(600), GUILayout.MaxHeight(400));
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label(nameof(NWDTestStudioData.Reference));
        GUILayout.Label("Actions", GUILayout.MaxWidth(60));
        GUILayout.EndHorizontal();

        foreach (NWDTestStudioData tStudioData in StudioDataList)
        {
            GUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label(tStudioData.Reference.ToString());
            if (GUILayout.Button("Select", GUILayout.MaxWidth(60)))
            {
                StudioData = tStudioData;
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }

    private void StudioDataDisplayer()
    {
        DisplayScrollView = GUILayout.BeginScrollView(DisplayScrollView, GUI.skin.box, GUILayout.MaxHeight(350));
        GUILayout.BeginVertical();
        if (StudioData != null)
        {
            GUI.enabled = false;

            GUILayout.Label(nameof(NWDDatabaseBasicModel));
            TestGUILayout.TextField(nameof(NWDDatabaseBasicModel.Creation), StudioData.Creation.ToString());
            TestGUILayout.TextField(nameof(NWDDatabaseBasicModel.Modification), StudioData.Modification.ToString());
            TestGUILayout.TextField(nameof(NWDDatabaseBasicModel.Active), StudioData.Active.ToString());
            TestGUILayout.TextField(nameof(NWDDatabaseBasicModel.Trashed), StudioData.Trashed.ToString());
            TestGUILayout.TextField(nameof(NWDDatabaseBasicModel.Reference), StudioData.Reference.ToString());

            GUILayout.Label(nameof(NWDBasicModel));
            TestGUILayout.TextField(nameof(NWDBasicModel.ProjectId), StudioData.ProjectId.ToString());

            GUILayout.Label(nameof(NWDStudioData));
            TestGUILayout.TextField(nameof(NWDStudioData.DataTrack), StudioData.DataTrack.ToString());
            TestGUILayout.TextField(nameof(NWDStudioData.AvailableForWeb), StudioData.AvailableForWeb.ToString());
            TestGUILayout.TextField(nameof(NWDStudioData.AvailableForGame), StudioData.AvailableForGame.ToString());
            TestGUILayout.TextField(nameof(NWDStudioData.AvailableForApp), StudioData.AvailableForApp.ToString());
            TestGUILayout.TextField(nameof(NWDStudioData.SyncDatetime), StudioData.SyncDatetime.ToString());
            TestGUILayout.TextField(nameof(NWDStudioData.Commit), StudioData.Commit.ToString());

            GUILayout.Label(nameof(NWDTestStudioData));
            TestGUILayout.TextField(nameof(NWDTestStudioData.TestString), StudioData.TestString.ToString());
            TestGUILayout.TextField(nameof(NWDTestStudioData.TestBool), StudioData.TestBool.ToString());
            TestGUILayout.TextField(nameof(NWDTestStudioData.TestByte), StudioData.TestByte.ToString());
            TestGUILayout.TextField(nameof(NWDTestStudioData.TestValue), StudioData.TestValue.ToString());
            TestGUILayout.TextField(nameof(NWDTestStudioData.TestInt), StudioData.TestInt.ToString());
            TestGUILayout.TextField(nameof(NWDTestStudioData.TestLong), StudioData.TestLong.ToString());
            TestGUILayout.TextField(nameof(NWDTestStudioData.TestFloat), StudioData.TestFloat.ToString());
            TestGUILayout.TextField(nameof(NWDTestStudioData.TestDouble), StudioData.TestDouble.ToString());
            TestGUILayout.TextField(nameof(NWDTestStudioData.TestSByte), StudioData.TestSByte.ToString());
            TestGUILayout.TextField(nameof(NWDTestStudioData.TestUShort), StudioData.TestUShort.ToString());
            TestGUILayout.TextField(nameof(NWDTestStudioData.TestUInt), StudioData.TestUInt.ToString());
            TestGUILayout.TextField(nameof(NWDTestStudioData.TestEnum), StudioData.TestEnum.ToString());
            TestGUILayout.TextField(nameof(NWDTestStudioData.TestFlag), StudioData.TestFlag.ToString());

            GUI.enabled = true;
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }

    private void OnSyncDone(NWDAsyncOperation sOperation)
    {
        StudioDataList = NWDUnityEngine.Instance.DataManager.GetAllStudioData<NWDTestStudioData>();
    }
}
