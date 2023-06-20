using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private List<SceneBuildIndexes> _scenesToSaveScoreFor;
    [SerializeField] private bool _encryptionEnabled;
    private ScoreData _scoreData;
    private SavingManager<ScoreData> _scoreSavingManager;

    private void Awake()
    {
        IDataService dataService = new JsonDataService();
        _scoreSavingManager = new SavingManager<ScoreData> (dataService, InitializeDefaultScoreData(), "/score-data.json", _encryptionEnabled);
        LoadData();
    }

    private ScoreData InitializeDefaultScoreData()
    {
        ScoreData scoreData = new ScoreData();
        foreach(SceneBuildIndexes sceneIndex in _scenesToSaveScoreFor) 
        {
            if (scoreData.BestTimesForLevels.ContainsKey(sceneIndex)) break;
            scoreData.BestTimesForLevels.Add(sceneIndex, 0f);
        }
        return scoreData;
    }

    public void SetBetterTime(int sceneIndex, float time)
    {
        SetBetterTime((SceneBuildIndexes)sceneIndex, time);
    }

    public void SetBetterTime(SceneBuildIndexes sceneIndex, float time)
    {
        if(CompareTimes(sceneIndex, time) < 0)
        {
            SetNewTime(sceneIndex, time);
        }
    }

    public void SetNewTime(int sceneIndex, float time)
    {
        SetNewTime((SceneBuildIndexes)sceneIndex, time);
    }

    public void SetNewTime(SceneBuildIndexes sceneIndex, float time)
    {
        _scoreData.BestTimesForLevels[sceneIndex] = time;
        SaveData();
    }

    public float CompareTimes(int sceneIndex, float timeToCompare)
    {
        return CompareTimes((SceneBuildIndexes)sceneIndex, timeToCompare);
    }

    public float CompareTimes(SceneBuildIndexes sceneIndex ,float timeToCompare)
    {
        return _scoreData.BestTimesForLevels[sceneIndex] - timeToCompare;
    }

    private void LoadData()
    {
        _scoreData = _scoreSavingManager.DeserializeJson();
    }

    private void SaveData()
    {
        _scoreSavingManager.SerializeJson(_scoreData);
    }
}
