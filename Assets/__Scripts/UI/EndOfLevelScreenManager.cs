using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndOfLevelScreenManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _finalTimeText;

    [SerializeField] private TimerManager _timer;
    private void Awake()
    {
        AsignButton();
    }

    private void AsignButton()
    {
        _nextLevelButton.onClick.AddListener(LoadNextLevel);
        _mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    private void LoadNextLevel()
    {
        SceneLoader.Instance.LoadNextSceneInBuilder();
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
            _endOfLevelCanvas.SetActive(enable);
        }
        else 
        {
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
