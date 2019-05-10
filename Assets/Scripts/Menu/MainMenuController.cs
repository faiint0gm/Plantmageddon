using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private int gameplaySceneIndex = 0;
    [SerializeField]
    GameObject creditsPanel;
    [SerializeField]
    GameObject mainMenuPanel;

    public void StartGame()
    {
        if (SceneManager.GetSceneByBuildIndex(gameplaySceneIndex) != null)
        {
            SceneManager.LoadSceneAsync(gameplaySceneIndex);
        }
        else
        {
            Debug.LogError("No Scene with given index!");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowCredits(bool show)
    {
        if (creditsPanel != null && mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(!show);
            creditsPanel.SetActive(show);
        }
        else
        {
            Debug.LogError("No Credits Panel or Main Menu Panel set in inspector!");
        }
    }
}
