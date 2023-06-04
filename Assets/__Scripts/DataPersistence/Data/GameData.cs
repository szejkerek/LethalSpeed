using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float MasterVolume;
    public float SFXVolume;
    public float MusicVolume;
    public float DialogsVolume;

    public GameData() 
    {
        MasterVolume = 1.0f;
        SFXVolume = 1.0f;
        MusicVolume = 1.0f;
        DialogsVolume = 1.0f;
    }
}
