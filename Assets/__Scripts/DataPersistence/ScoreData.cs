using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreData : ISerializableClass
{
    public Dictionary<SceneBuildIndexes, float> BestTimesForLevels;

    public ScoreData() { }
    public ScoreData(Dictionary<SceneBuildIndexes, float> bestTimesForLevels)
    {
        BestTimesForLevels = bestTimesForLevels;
    }
}
