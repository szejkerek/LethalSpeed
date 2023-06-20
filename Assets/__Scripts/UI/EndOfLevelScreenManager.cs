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

    private void OnDestroy()
    {
        ShowEndOfLevelCanvas(false);
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
            InitializeEndOfLevelCanvasData();
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
    }
}
