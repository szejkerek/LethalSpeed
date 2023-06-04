using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : Singleton<DataPersistenceManager>
{
    private GameData gameData;

    private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        if (this.gameData == null) 
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }
    }

    public void SaveGame() 
    { 
    
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
