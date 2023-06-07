using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour, IDataPersistence
{
    [Header("UI")]
    [SerializeField] Slider _masterVolumeSlider;
    [SerializeField] Slider _SFXVolumeSlider;
    [SerializeField] Slider _musicVolumeSlider;
    [SerializeField] Slider _dialogsVolumeSlider;
    [SerializeField] Button _applyChangesButton;

    private OptionsData OptionsData = new OptionsData();
    private IDataService DataService = new JsonDataService();
    private bool EncryptionEnabled;

    private void Start()
    {
        AssignUIButtons();
    }

    private void AssignUIButtons()
    {
        _applyChangesButton.onClick.AddListener(OnApplyChangesButtonClick);
    }

    public void SerializeJson()
    {
        if(!DataService.SaveData("/options-data.json", OptionsData, EncryptionEnabled))
        {
            Debug.LogError("Could not save file!");
        }
    }

    public void LoadData(GameData data)
    {
        this._masterVolumeSlider.value = data.MasterVolume;
        this._SFXVolumeSlider.value = data.SFXVolume;
        this._musicVolumeSlider.value = data.MusicVolume;
        this._dialogsVolumeSlider.value = data.DialogsVolume;
    }
    public void SaveData(ref GameData data)
    {
        data.MasterVolume = this._masterVolumeSlider.value;
        data.SFXVolume = this._SFXVolumeSlider.value;
        data.MusicVolume = this._musicVolumeSlider.value;
        data.DialogsVolume = this._dialogsVolumeSlider.value;
    }

    private void OnApplyChangesButtonClick()
    {
        OptionsData.MasterVolume = this._masterVolumeSlider.value;
        OptionsData.SFXVolume = this._SFXVolumeSlider.value;
        OptionsData.MusicVolume = this._musicVolumeSlider.value;
        OptionsData.DialogsVolume = this._dialogsVolumeSlider.value;
    }
}
