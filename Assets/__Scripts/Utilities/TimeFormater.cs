using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using static TimeFormater;

public class TimeFormater : MonoBehaviour
{
    private Dictionary<TimeFormats, string> _timerFormats;

    public enum TimeFormats
    {
        SecondsMilliseconds,
        MinutesSeconds,
        MinutesSecondsMilliseconds,
        HoursMinutes,
        HoursMinutesSeconds
    }

    private void Awake()
    {
        InitializeTimeFormats();
    }

    private void InitializeTimeFormats()
    {
        _timerFormats.Add(TimeFormats.SecondsMilliseconds, "{2:D2} : {3:D2}");
        _timerFormats.Add(TimeFormats.MinutesSecondsMilliseconds, "{1:D2} : {2:D2} : {3:D2}");
        _timerFormats.Add(TimeFormats.HoursMinutes, "{0:D2} : {1:D2}");
        _timerFormats.Add(TimeFormats.HoursMinutesSeconds, "{0:D2} : {1:D2} : {2:D2}");
    }

    public string FormatTime (TimeFormats fromat, float time)
    {
        int milliseconds = Mathf.FloorToInt(time * 100) % 100;
        int seconds = Mathf.FloorToInt(time) % 60;
        int minutes = Mathf.FloorToInt(time / 60) % 60;
        int hours = Mathf.FloorToInt(time / 60 / 60);
        return string.Format(_timerFormats[fromat], hours, minutes, seconds, milliseconds);
    }

}
