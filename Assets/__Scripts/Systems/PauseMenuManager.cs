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
        _pauseMenuPanel.SetActive(true);
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

        if (_isPaused)
        {         
            UnpauseGame();
        }
        else
        {            
            PauseGame();
        }
        _isPaused = !_isPaused;

    }

    private void UnpauseGame()
    {
        Time.timeScale = 1.0f;
        Helpers.DisableCursor();
        GameManager.Instance.EnableInputs();
        _pauseMenuCanvas.SetActive(false);  
    }

    private void PauseGame() 
    {
        Time.timeScale = 0f;
        Helpers.EnableCursor();
        GameManager.Instance.EnableInputs(false);
        _pauseMenuCanvas.SetActive(true);
        _pauseMenuPanel.SetActive(true);
        _optionsPanel.SetActive(false);
    }

    public void OnResumeButtonClick()
    {
        UnpauseGame();
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
        SceneLoader.Instance.LoadMenu();
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
        Debug.Log("Application closed.");
    }
}
