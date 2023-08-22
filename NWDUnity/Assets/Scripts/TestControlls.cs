using UnityEngine;
using UnityEngine.SceneManagement;

public class TestControlls : MonoBehaviour
{
    private string[] Scenes = new string[]
    {
        "Assets/Scenes/Account Test Scene.unity",
        "Assets/Scenes/PlayerData Test Scene.unity",
        "Assets/Scenes/StudioData Test Scene.unity",
    };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int tIndex = GetCurrentSceneIndex();
            if (tIndex >= 0)
            {
                tIndex = (tIndex - 1) % Scenes.Length;
                if (tIndex < 0)
                {
                    tIndex = Scenes.Length - 1;
                }
                SceneManager.LoadScene(Scenes[tIndex], LoadSceneMode.Single);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int tIndex = GetCurrentSceneIndex();
            if (tIndex >= 0)
            {
                tIndex = (tIndex + 1) % Scenes.Length;
                SceneManager.LoadScene(Scenes[tIndex], LoadSceneMode.Single);
            }
        }
    }

    private int GetCurrentSceneIndex()
    {
        Scene tScene = SceneManager.GetActiveScene();
        for (int i = 0; i < Scenes.Length; i++)
        {
            if (Scenes[i] == tScene.path)
            {
                return i;
            }
        }
        Debug.LogWarning("Scene not found !");
        return -1;
    }
}
