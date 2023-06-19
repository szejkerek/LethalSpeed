using System.Collections;
using System.Collections.Generic;
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
}
