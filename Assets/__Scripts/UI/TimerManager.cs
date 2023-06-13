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
    [SerializeField] private bool _hasLimit;
    [SerializeField] private float _timerLimit;

    private float _timerTime;

    private void Awake()
    {
        _timerTime = _startingTime;
    }
    void Update()
    {
        CountTime();
        DisplayTime();
    }

    private void CountTime()
    {
        _timerTime = _countDown ? _timerTime -= Time.deltaTime : _timerTime += Time.deltaTime;
    }

    private void DisplayTime()
    {
        _timerDisplay.text = string.Format("{0}", _timerTime);
    }
}
