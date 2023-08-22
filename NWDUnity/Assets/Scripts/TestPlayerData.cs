using NWDStandardModels.Models;
using NWDUnityShared.Engine;
using NWDUnityShared.Tools;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerData : MonoBehaviour
{
    NWDGUIAsyncOperation SyncOperation;
    Vector2 ScrollView;

    NWDTestPlayerData PlayerData = new NWDTestPlayerData();

    List<NWDTestPlayerData> PlayerDataList = new List<NWDTestPlayerData>();

    private void OnGUI()
    {
        Debug.Log(Event.current.type);
        GUILayout.BeginHorizontal();
        ListPlayerData();

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
        PlayerDataCreation();
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }

    private void ListPlayerData()
    {
        ScrollView = GUILayout.BeginScrollView(ScrollView, GUI.skin.box, GUILayout.MinWidth(600), GUILayout.MaxHeight(400));
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label(nameof(NWDTestPlayerData.TestString));
        GUILayout.Label(nameof(NWDTestPlayerData.TestBool));
        GUILayout.Label(nameof(NWDTestPlayerData.TestInt));
        GUILayout.Label("Actions", GUILayout.MaxWidth(60));
        GUILayout.EndHorizontal();

        foreach (NWDTestPlayerData tPlayerData in PlayerDataList)
        {
            GUILayout.BeginHorizontal(GUI.skin.box);
            tPlayerData.TestString = GUILayout.TextField(tPlayerData.TestString);

            tPlayerData.TestBool = GUILayout.Toggle(tPlayerData.TestBool, "");

            string tInt = tPlayerData.TestInt.ToString();
            tInt = GUILayout.TextField(tInt);
            if (int.TryParse(tInt, out int tParsed))
            {
                tPlayerData.TestInt = tParsed;
            }

            if (GUILayout.Button("Save", GUILayout.MaxWidth(60)))
            {
                NWDUnityEngine.Instance.DataManager.SavePlayerData(tPlayerData);
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }

    private void PlayerDataCreation()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        PlayerData.TestString = TestGUILayout.TextField(nameof(PlayerData.TestString), PlayerData.TestString);
        PlayerData.TestBool = GUILayout.Toggle(PlayerData.TestBool, nameof(PlayerData.TestBool));
        string tInt = PlayerData.TestInt.ToString();
        tInt = TestGUILayout.TextField(nameof(PlayerData.TestInt), tInt);
        if (int.TryParse(tInt, out int tParsed))
        {
            PlayerData.TestInt = tParsed;
        }
        if (GUILayout.Button("Save data"))
        {
            NWDUnityEngine.Instance.DataManager.SavePlayerData(PlayerData);
            PlayerData = new NWDTestPlayerData();
        }
        GUILayout.EndVertical();
    }

    private void OnSyncDone(NWDAsyncOperation sOperation)
    {
        PlayerDataList = NWDUnityEngine.Instance.DataManager.GetAllPlayerData<NWDTestPlayerData>();
    }
}
