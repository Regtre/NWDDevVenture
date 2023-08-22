using NWDStandardModels.Models;
using NWDUnityShared.Engine;
using NWDUnityShared.Scripts;
using System;
using System.Collections;
using UnityEngine;

public class TestConnection : MonoBehaviour
{
    [Serializable]
    public class NWDTestConnection : NWDConnection<NWDTestStudioData> { }

    public NWDTestConnection Connection;

    public int[] Test;

    private IEnumerator Start()
    {
        yield return NWDUnityEngine.Instance.LaunchOperation;

        NWDTestStudioData tData = Connection;

        if (tData == null)
        {
            Debug.Log("Connection is empty!");
        }
        else
        {
            Debug.Log("Connection is set to: " + Connection.Reference + "\nTestString: " + tData.TestString);
        }
    }
}
