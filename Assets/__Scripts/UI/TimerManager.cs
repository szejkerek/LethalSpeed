using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _timerDisplay;

    [Header("Timer Settings")]
    [SerializeField] float _startingTime;
    [SerializeField] bool _countDown;

    [Header("Limit Settings")]
    [SerializeField] private float _timerLimit;
    [SerializeField] private Color _timeOutColor;
    [SerializeField] private bool _hasLimit;

    private float _currentTimerTime;

    private void Awake()
    {
        _currentTimerTime = _startingTime;
    }
    void Update()
    {
        CountTime();
        DisplayTime();
    }

    private void CountTime()
    {
        _currentTimerTime = _countDown ? _currentTimerTime -= Time.deltaTime : _currentTimerTime += Time.deltaTime;

        if(_hasLimit && ((_countDown && _currentTimerTime <= _timerLimit) || (!_countDown && _currentTimerTime >= _timerLimit)))
        {
            _currentTimerTime = _timerLimit;
            _timerDisplay.color = _timeOutColor;
            enabled = false;
        }
    }

    private void DisplayTime()
    {
        _timerDisplay.text = string.Format("{0}", _currentTimerTime);
    }
}
