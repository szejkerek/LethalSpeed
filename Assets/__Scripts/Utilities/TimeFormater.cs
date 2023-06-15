using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class TimeFormater
{
    private Dictionary<TimeFormats, string> _timeFormats;

    public enum TimeFormats
    {
        SecondsMilliseconds,
        MinutesSeconds,
        MinutesSecondsMilliseconds,
        HoursMinutes,
        HoursMinutesSeconds
    }

    public TimeFormater()
    {
        _timeFormats = new Dictionary<TimeFormats, string>();
        InitializeTimeFormats();
    }

    private void InitializeTimeFormats()
    {
        _timeFormats.Add(TimeFormats.SecondsMilliseconds, "{2:D2} : {3:D2}");
        _timeFormats.Add(TimeFormats.MinutesSeconds, "{1:D2} : {2:D2}");
        _timeFormats.Add(TimeFormats.MinutesSecondsMilliseconds, "{1:D2} : {2:D2} : {3:D2}");
        _timeFormats.Add(TimeFormats.HoursMinutes, "{0:D2} : {1:D2}");
        _timeFormats.Add(TimeFormats.HoursMinutesSeconds, "{0:D2} : {1:D2} : {2:D2}");
    }

    public string FormatTime (TimeFormats fromat, float time)
    {
        int milliseconds = Mathf.FloorToInt(time * 100) % 100;
        int seconds = Mathf.FloorToInt(time) % 60;
        int minutes = Mathf.FloorToInt(time / 60) % 60;
        int hours = Mathf.FloorToInt(time / 60 / 60);
        string formatedTime = string.Format(_timeFormats[fromat], hours, minutes, seconds, milliseconds);
        return formatedTime;
    }

}
