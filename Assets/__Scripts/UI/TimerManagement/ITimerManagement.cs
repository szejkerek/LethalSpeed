using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimerManagment
{
    float GetTimerTime();
    void StartTimer();
    void StopTimer();
}
