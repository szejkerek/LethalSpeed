using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour, ITimerManagment
{
    [Header("UI")]
    [SerializeField] private TMP_Text _timerDisplay;

    [Header("Timer Settings")]
    [SerializeField] private bool _timeIsRunning;
    [SerializeField] private bool _countDown;
    [SerializeField] private float _startingTime;

    [Header("Limit Settings")]
    [SerializeField] private bool _hasLimit;
    [SerializeField] private float _timerLimit;
    [SerializeField] private Color _timeOutColor;

    [Header("Format Settings")]
    [SerializeField] private int _decimalPlacesWithoutFormating;
    [SerializeField] private bool _hasFormat;
    [SerializeField] private TimerFormats _format;
    private Dictionary<TimerFormats, string> _timerFormats;

    private float _currentTimerTime;
    private enum TimerFormats
    {
        SecondsMilliseconds,
        MinutesSeconds,
        MinutesSecondsMilliseconds,
        HoursMinutes,
        HoursMinutesSeconds
    }

    private void Awake()
    {
        _currentTimerTime = _startingTime;
        _timerFormats = new Dictionary<TimerFormats, string>();
        InitializeTimeFormats();
        DisplayTime();
    }

    private void InitializeTimeFormats()
    {
        _timerFormats.Add(TimerFormats.SecondsMilliseconds, "{2:D2} : {3:D2}");
        _timerFormats.Add(TimerFormats.MinutesSecondsMilliseconds, "{1:D2} : {2:D2} : {3:D2}");
        _timerFormats.Add(TimerFormats.HoursMinutes, "{0:D2} : {1:D2}");
        _timerFormats.Add(TimerFormats.HoursMinutesSeconds, "{0:D2} : {1:D2} : {2:D2}");
    }
    void Update()
    {
        if(_timeIsRunning)
        {
            CountTime();
            DisplayTime();
        }
    }

    private void CountTime()
    {
        _currentTimerTime = _countDown ? _currentTimerTime -= Time.deltaTime : _currentTimerTime += Time.deltaTime;

        if(_hasLimit && ((_countDown && _currentTimerTime <= _timerLimit) || (!_countDown && _currentTimerTime >= _timerLimit)))
        {
            _currentTimerTime = _timerLimit;
             DisplayTime();
            _timerDisplay.color = _timeOutColor;
            _timeIsRunning = false;
        }
    }

    private void DisplayTime()
    {
        int milliseconds = Mathf.FloorToInt(_currentTimerTime * 100) % 100;
        int seconds = Mathf.FloorToInt(_currentTimerTime) % 60;
        int minutes = Mathf.FloorToInt(_currentTimerTime / 60) % 60;
        int hours = Mathf.FloorToInt(_currentTimerTime / 60 / 60);

        if(_hasFormat)
        {
            _timerDisplay.text = string.Format(_timerFormats[_format], hours, minutes, seconds, milliseconds);
        }
        else
        {
            _timerDisplay.text = Math.Round(_currentTimerTime, _decimalPlacesWithoutFormating).ToString();
        }
    }

    public float GetTimerTime()
    {
        return _currentTimerTime;
    }

    public void StartTimer()
    {
        _timeIsRunning = true;
    }

    public void StopTimer()
    {
        _timeIsRunning = false;
    }
}
