using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMenuManager : Singleton<SceneMenuManager>
{
    int mainMenuBuildIndex = 0;
    int gameBuildIndex = 1;

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuBuildIndex);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameBuildIndex);
    }

    public void RestartGame()
    {
        StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
