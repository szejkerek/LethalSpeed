using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    private bool _isPaused = false;
    private bool _enableInputs = true;

    [Header("UI")]
    [SerializeField] private GameObject _pauseMenuCanvas;
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _optionsPanel;

    [Header("Butons")]
    [SerializeField] private Button _resetButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _backOptionsButton;

    public bool IsPaused { get => _isPaused; }
    public bool EnableInputs { get => _enableInputs; set => _enableInputs = value; }

    private void Awake()
    {
        Time.timeScale = 1.0f;
        _pauseMenuCanvas.SetActive(false);
        _optionsPanel.SetActive(false);
    }

    private void Start()
    {
        AssignUIButtons();
    }

    private void AssignUIButtons()
    {
        _resetButton.onClick.AddListener(OnResetButtonClick);
        _quitButton.onClick.AddListener(OnQuitButtonClick);
        _resumeButton.onClick.AddListener(OnResumeButtonClick);
        _optionsButton.onClick.AddListener(OnOptionButtonClick);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        _backOptionsButton.onClick.AddListener(OnBackOptionButtonClick);
    }

    public void TooglePasueMenu()
    {
        if (!_enableInputs)
            return;

        if (_isPaused == true)
        {
            Helpers.DisableCursor();
            EnableGame();
        }
        else
        {
            Helpers.EnableCursor();
            DisableGame();
        }
    }

    private void EnableGame()
    {
        Time.timeScale = 1.0f;
        _pauseMenuCanvas.SetActive(false);
        _isPaused = false;
    }

    private void DisableGame() 
    {
        Time.timeScale = .0f;
        _pauseMenuCanvas.SetActive(true);
        _isPaused = true;
    }

    public void OnResumeButtonClick()
    {
        EnableGame();
    }

    public void OnResetButtonClick()
    {
        GameManager.Instance.ResetGame();
    }

    public void OnOptionButtonClick()
    {
        _optionsPanel.SetActive(true);
        _pauseMenuPanel.SetActive(false);
    }

    public void OnBackOptionButtonClick()
    {
        _optionsPanel.SetActive(false);
        _pauseMenuPanel.SetActive(true);
    }

    public void OnMainMenuButtonClick()
    {
        Time.timeScale = 1.0f;
        SceneLoader.Instance.LoadNewSceneByBuildIndex((int)SceneBuildIndexes.Menu);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
        Debug.Log("Application closed.");
    }
}
