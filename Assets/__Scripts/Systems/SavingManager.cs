using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingManager <T> where T : ISerializableClass
{
    private IDataService _dataService;
    private T _defaultSaveData;
    private string _relativeDataPath;
    private bool _isDataEncrypted;

    public SavingManager(IDataService dataService, T defaultSaveData, string relativeDataPath, bool isDataEncrypted)
    {
        _dataService = dataService;
        _defaultSaveData = defaultSaveData;
        _relativeDataPath = relativeDataPath;
        _isDataEncrypted = isDataEncrypted;
        _dataService.InitializeData(_relativeDataPath, _defaultSaveData, _isDataEncrypted);
    }

    public void SerializeJson(T dataToSerialize)
    {
        _dataService.SaveData(_relativeDataPath, dataToSerialize, _isDataEncrypted);
    }

    public T DeserializeJson()
    {
        return _dataService.LoadData<T>(_relativeDataPath, _isDataEncrypted);
    }
}
