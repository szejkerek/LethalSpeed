using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class ScoreSavingManager
{
    private IDataService _jsonDataService;
    private ScoreData _scoreData;
    private ScoreData _defaultScoreData;
    private string _relativeDataPath;
    private bool _encryptionEnabled;

    public ScoreSavingManager(string relativeDataPath, bool encryptionEnabled , ScoreData devaultScoreData)
    {
        _jsonDataService = new JsonDataService();
        _scoreData = new ScoreData();
        _defaultScoreData = devaultScoreData;
        _relativeDataPath = relativeDataPath;
        _encryptionEnabled = encryptionEnabled;
    }
    public void InitializeJson()
    {
        _jsonDataService.InitializeData(_relativeDataPath, _defaultScoreData, _encryptionEnabled);
    }

    public void SerializeJson()
    {
        _jsonDataService.SaveData(_relativeDataPath, _scoreData, _encryptionEnabled);
    }

    public void DeserializeJson()
    {
        this._scoreData = _jsonDataService.LoadData<ScoreData>(_relativeDataPath, _encryptionEnabled);
    }
}
