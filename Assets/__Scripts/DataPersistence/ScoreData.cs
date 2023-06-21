using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreData : ISerializableClass
{
    public Dictionary<SceneBuildIndexes, SceneScore> SceneRecords;

    public ScoreData() 
    {
        SceneRecords = new Dictionary<SceneBuildIndexes, SceneScore>();
    }
}

public class SceneScore
{
    public float BestTime;
    public bool WasSet;

    public SceneScore() {}

    public SceneScore(float bestTime, bool wasSet)
    {
        BestTime = bestTime;
        WasSet = wasSet;
    }

    public bool isTimeNewRecord(float newTime)
    {
        return newTime < BestTime;
    }
}
