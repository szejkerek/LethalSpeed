using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] Slider _masterVolumeSlider;
    [SerializeField] Slider _SFXVolumeSlider;
    [SerializeField] Slider _musicVolumeSlider;
    [SerializeField] Slider _dialogsVolumeSlider;

    public void LoadData(GameData data)
    {
        this._masterVolumeSlider.value = data.MasterVolume;
        this._SFXVolumeSlider.value = data.SFXVolume;
        this._musicVolumeSlider.value = data.MusicVolume;
        this._dialogsVolumeSlider.value = data.DialogsVolume;
    }
    public void SaveData(GameData data)
    {
        data.MasterVolume = this._masterVolumeSlider.value;
        data.SFXVolume = this._SFXVolumeSlider.value;
        data.MusicVolume = this._musicVolumeSlider.value;
        data.DialogsVolume = this._dialogsVolumeSlider.value;
    }
}
