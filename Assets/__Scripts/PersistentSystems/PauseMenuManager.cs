using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : Singleton<PauseMenuManager>
{
    private bool isPaused;
    [SerializeField] private GameObject PauseMenuCanvas;
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private GameObject OptionsPanel;

    protected override void Awake()
    {
        base.Awake();
        Time.timeScale = 1.0f;
        isPaused = false;
        PauseMenuCanvas.SetActive(false);
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }

        if(Input.GetKeyUp(KeyCode.Escape)) 
        {
            if(isPaused == true) 
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Play();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Stop();
            }
        }
    }

    private void Play()
    {
        Time.timeScale = 1.0f;
        PauseMenuCanvas.SetActive(false);
        isPaused = false;
    }

    private void Stop() 
    {
        Time.timeScale = .0f;
        PauseMenuCanvas.SetActive(true);
        isPaused = true;
    }

    public void ResumeButton()
    {
        Play();
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
        Play();
        SceneLoader.Instance.LoadNewSceneByBuildIndex((int)SceneBuildIndexes.Menu);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Application closed.");
    }
}
