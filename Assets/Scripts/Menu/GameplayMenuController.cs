using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayMenuController : MonoBehaviour
{
    [SerializeField]
    private int mainMenuSceneIndex;

    public void BackToMenu()
    {
        if(SceneManager.GetSceneByBuildIndex(mainMenuSceneIndex) != null)
        {
            SceneManager.LoadSceneAsync(mainMenuSceneIndex);
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
