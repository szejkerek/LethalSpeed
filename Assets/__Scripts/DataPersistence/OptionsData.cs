using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public class OptionsData : ISerializableClass
{
    public float MasterVolume;
    public float SFXVolume;
    public float MusicVolume;
    public float DialogsVolume;

    public OptionsData() { }
    public OptionsData(float pMasterVolume, float pSFXVolume, float pMusicVolume, float pDialogsVolume) 
    {
        MasterVolume = pMasterVolume;
        SFXVolume = pSFXVolume;
        MusicVolume = pMusicVolume;
        DialogsVolume = pDialogsVolume;
    }
}