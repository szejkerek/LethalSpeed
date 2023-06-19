using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreData
{
    public Dictionary<int, float> BestTimesForLevels;

    public ScoreData() { }
    public ScoreData(Dictionary<int, float> bestTimesForLevels)
    {
        BestTimesForLevels = bestTimesForLevels;
    }
}
