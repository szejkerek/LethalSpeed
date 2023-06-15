using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfLevelScreenManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _endOfLevelCanvas;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _finalTimeText;

    [Header("UI Settings")]
    [SerializeField] private string _defaultFinalTimeText;
    [SerializeField] private TimeFormater.TimeFormats _timeFormat;

    private TimerManager _timer;
    private TimeFormater _timeFormater;
    private void Awake()
    {
        _endOfLevelCanvas.SetActive(false);
        _timer = GetComponent<TimerManager>();
        _timeFormater = new TimeFormater();
        _finalTimeText.text = _defaultFinalTimeText;
        AsignButton();
    }

    private void AsignButton()
    {
        _nextLevelButton.onClick.AddListener(LoadNextLevel);
        _mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    private void LoadNextLevel()
    {
        //TO DO: Move check if there is next scene in builder logic to SceneLoader
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneLoader.Instance.LoadNextSceneInBuilder();
        }
        else
        {
            SceneLoader.Instance.LoadNewSceneByBuildIndex((int)SceneBuildIndexes.Menu);
        }
    }

    private void LoadMainMenu()
    {
        SceneLoader.Instance.LoadNewSceneByBuildIndex((int)SceneBuildIndexes.Menu);
    }

    public void ShowEndOfLevelCanvas(bool enable = true)
    {
        if (enable) 
        {
            InitializeEndOfLevelCanvasData();
            Helpers.EnableCursor();
            _endOfLevelCanvas.SetActive(enable);
        }
        else 
        {
            Helpers.DisableCursor();
            _endOfLevelCanvas.SetActive(enable);
        }
    }

    public void InitializeEndOfLevelCanvasData()
    {
        float finalTime = _timer.GetTimerTime();
        string finalTimeFormated = _timeFormater.FormatTime(_timeFormat, finalTime);
        _finalTimeText.text = _finalTimeText.text + finalTimeFormated;  
    }
}
