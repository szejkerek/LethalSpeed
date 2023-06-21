using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndOfLevelScreenManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _endOfLevelCanvas;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _finalTimeText;
    [SerializeField] private TMP_Text _bestTimeText;

    [Header("UI Settings")]
    [SerializeField] private string _defaultFinalTimeText;
    [SerializeField] private TimeFormater.TimeFormats _timeFormat;

    private TimerManager _timer;
    private TimeFormater _timeFormater;
    private ScoreManager _scoreManager;

    private void Awake()
    {
        _endOfLevelCanvas.SetActive(false);
        _timer = GetComponent<TimerManager>();
        _scoreManager = GetComponent<ScoreManager>();
        _timeFormater = new TimeFormater();
        _finalTimeText.text = _defaultFinalTimeText;
        AsignButton();
    }

    private void AsignButton()
    {
        _nextLevelButton.onClick.AddListener(() => SceneLoader.Instance.LoadNextLevel());
        _mainMenuButton.onClick.AddListener(() => SceneLoader.Instance.LoadMenu());
    }

    public void ShowEndOfLevelCanvas(bool enable = true)
    {
        if (enable) 
        {
            Helpers.EnableCursor();
            Time.timeScale = 0;
            _endOfLevelCanvas.SetActive(enable);
        }
        else 
        {
            Helpers.DisableCursor();
            Time.timeScale = 1;
            _endOfLevelCanvas.SetActive(!enable);
        }
    }

    public void InitializeEndOfLevelCanvasData()
    {
        float finalTime = _timer.GetTimerTime();
        string finalTimeFormated = _timeFormater.FormatTime(_timeFormat, finalTime);
        _finalTimeText.text = _finalTimeText.text + finalTimeFormated;
        ShowBestTime();
    }

    private void ShowBestTime()
    {
        float bestTime = _scoreManager.GetTimeFromActiveScene();
        float currentTime = _timer.GetTimerTime();
        float timeDifference = currentTime - bestTime;
        if (!_scoreManager.TimeWasSetForActiveScene()) 
        {
            _bestTimeText.text = "New best time: " + _timeFormater.FormatTime(TimeFormater.TimeFormats.MinutesSecondsMilliseconds, currentTime);
        }
        else if(timeDifference < 0)
        {
            _bestTimeText.text = "New best time: " + _timeFormater.FormatTime(TimeFormater.TimeFormats.MinutesSecondsMilliseconds, currentTime)
                + " " + _timeFormater.FormatTime(TimeFormater.TimeFormats.Decimal, timeDifference);
        }
        else
        {
            _bestTimeText.text = "Best Time: " + _timeFormater.FormatTime(TimeFormater.TimeFormats.MinutesSecondsMilliseconds, bestTime)
                + " +" + _timeFormater.FormatTime(TimeFormater.TimeFormats.Decimal, timeDifference);
        }
    }
}
