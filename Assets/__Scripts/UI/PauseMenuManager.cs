using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] private GameObject PauseMenuCanvas;
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private GameObject OptionsPanel;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        PauseMenuCanvas.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            TooglePasueMenu();
        }
    }

    public void TooglePasueMenu()
    {
        if (isPaused == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            EnableGame();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            DisableGame();
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    private void EnableGame()
    {
        Time.timeScale = 1.0f;
        PauseMenuCanvas.SetActive(false);
        isPaused = false;
    }

    private void DisableGame() 
    {
        Time.timeScale = .0f;
        PauseMenuCanvas.SetActive(true);
        isPaused = true;
    }

    public void ResumeButton()
    {
        EnableGame();
    }

    public void ResetButton()
    {
        SceneLoader.Instance.ReloadScene();
    }

    public void OptionButton()
    {
        OptionsPanel.SetActive(true);
        PauseMenuPanel.SetActive(false);
    }

    public void QuitOptionButton()
    {
        OptionsPanel.SetActive(false);
        PauseMenuPanel.SetActive(true);
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1.0f;
        SceneLoader.Instance.LoadNewSceneByBuildIndex((int)SceneBuildIndexes.Menu);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Application closed.");
    }
}
