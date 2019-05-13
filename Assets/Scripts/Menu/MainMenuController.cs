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
    [SerializeField]
    GameObject winPanel;
    [SerializeField]
    GameObject losePanel;

    public void StartGame()
    {
        if (SceneManager.GetSceneByBuildIndex(1) != null)
        {
            SceneManager.LoadSceneAsync(1);
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

    public void NextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            BackToMenu();
            return;
        }
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        if (SceneManager.GetSceneByBuildIndex(0) != null)
        {
            SceneManager.LoadSceneAsync(0);
        }
    }

    public void ShowLosePanel()
    {
        losePanel.SetActive(true);
    }

    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
    }

    public void GoToLastLevel()
    {
        SceneManager.LoadSceneAsync(4);
    }
}
