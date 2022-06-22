using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void LoadSceneInnMenu()
    {
        SceneManager.LoadScene("Scn_InnMenu");
    }

    public void LoadSceneMainMenu()
    {
        SceneManager.LoadScene("Scn_MainMenu");
    }

    public void LoadSceneMission_Level(int n)
    {
        string aux = "Scn_Mission_";
        aux += n.ToString();
        SceneManager.LoadScene(aux);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}