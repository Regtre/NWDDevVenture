using UnityEngine;

public static class TestGUILayout
{
    static public string TextField(string sLabel, string sText, GUIStyle sStyle = null)
    {
        if (sStyle == null)
        {
            sStyle = GUIStyle.none;
        }

        GUILayout.BeginHorizontal(sStyle);
        GUILayout.Label(sLabel);
        string rResult = GUILayout.TextField(sText);
        GUILayout.EndHorizontal();
        return rResult;
    }
}
