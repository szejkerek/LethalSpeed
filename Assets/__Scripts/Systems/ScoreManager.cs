using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private List<SceneBuildIndexes> _scenesToSaveScoreFor;
    private ScoreData _scoreData;
    private PersistentDataManager<ScoreData> _scoreSavingManager;

    private void Awake()
    {
        _scoreSavingManager = new PersistentDataManager<ScoreData>(new JsonDataService(), InitializeDefaultScoreData(), "/score-data.json");
        LoadData();
    }

    public void SetTimeForActiveScene(float time)
    {
        SetTime((SceneBuildIndexes)SceneManager.GetActiveScene().buildIndex, time);
    }

    public void SetTime(SceneBuildIndexes sceneIndex, float time)
    {
        if (_scoreData.SceneRecords[sceneIndex].WasSet)
        {
            if (_scoreData.SceneRecords[sceneIndex].isTimeNewRecord(time))
            {
                _scoreData.SceneRecords[sceneIndex].BestTime = time;
                SaveData();
            }
        }
        else
        {
            _scoreData.SceneRecords[sceneIndex].BestTime = time;
            _scoreData.SceneRecords[sceneIndex].WasSet = true;
            SaveData();
        }
    }
    public float GetTimeFromActiveScene()
    {
        return GetTime((SceneBuildIndexes)SceneManager.GetActiveScene().buildIndex);
    }
    public float GetTime(SceneBuildIndexes sceneIndex)
    {
        return _scoreData.SceneRecords[sceneIndex].BestTime;
    }

    public bool TimeWasSetForActiveScene()
    {
        return _scoreData.SceneRecords[(SceneBuildIndexes)SceneManager.GetActiveScene().buildIndex].WasSet;
    }
    private ScoreData InitializeDefaultScoreData()
    {
        ScoreData scoreData = new ScoreData();
        foreach (SceneBuildIndexes sceneIndex in _scenesToSaveScoreFor)
        {
            if (scoreData.SceneRecords.ContainsKey(sceneIndex))
                break;

            scoreData.SceneRecords.Add(sceneIndex, new SceneScore(-1, false));
        }
        return scoreData;
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
