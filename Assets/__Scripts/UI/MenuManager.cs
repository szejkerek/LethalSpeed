using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject ChooseSaveSlotPanel;
    [SerializeField] private GameObject ChooseLevelPanel;
    [SerializeField] private GameObject OptionPanel;

    [Header("Butons")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _chooseLevelButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _quitChooseSaveSlotButton;
    [SerializeField] private Button _quitChooseLevelButton;
    [SerializeField] private Button _quitOptionsButton;
    [SerializeField] private List<Button> _levelButtons;

    protected void Awake()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        MainMenuPanel.SetActive(true);
        ChooseSaveSlotPanel.SetActive(false);
        ChooseLevelPanel.SetActive(false);
        OptionPanel.SetActive(false);
    }

    private void Start()
    {
        AssignUIButtons();
    }

    private void AssignUIButtons()
    {
        _startButton.onClick.AddListener(OnStartGameButtonClick);
        _chooseLevelButton.onClick.AddListener(OnChooseLevelButtonCLick);
        _optionsButton.onClick.AddListener(OnOptionButtonClick);
        _quitButton.onClick.AddListener(OnQuitButtonClick);
        _quitChooseSaveSlotButton.onClick.AddListener(OnQuitChooseSaveSlotButtonClick);
        _quitChooseLevelButton.onClick.AddListener(OnQuitChooseLevelButtonClick);
        _quitOptionsButton.onClick.AddListener(OnQuitOptionButtonClick);

        for(int i = 0; i < _levelButtons.Count; i++)
        {
            int levelToStart = i + 1;
            _levelButtons[i].onClick.AddListener(() => OnLoadLevelByBuildIndexButtonClick(levelToStart));
        }
    }

    public void OnStartGameButtonClick()
    {
        SceneLoader.Instance.LoadScene(SceneBuildIndexes.PrototypeLevel1);
    }

    public void OnQuitChooseSaveSlotButtonClick()
    {
        ChooseSaveSlotPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void OnChooseLevelButtonCLick()
    {
        ChooseLevelPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }

    public void OnLoadLevelByBuildIndexButtonClick(int levelBuildIndex)
    {
        SceneLoader.Instance.LoadScene(levelBuildIndex);
    }

    public void OnQuitChooseLevelButtonClick()
    {
        ChooseLevelPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void OnOptionButtonClick()
    {
        OptionPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }

    public void OnQuitOptionButtonClick()
    {
        OptionPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
        Debug.Log("Application closed.");
    }
}
