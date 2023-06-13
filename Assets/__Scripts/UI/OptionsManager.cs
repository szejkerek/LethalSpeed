using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Scrollbar _masterVolumeSlider;
    [SerializeField] Scrollbar _SFXVolumeSlider;
    [SerializeField] Scrollbar _musicVolumeSlider;
    [SerializeField] Scrollbar _dialogsVolumeSlider;
    [SerializeField] Button _applyChangesButton;

    private OptionsData DefaultOptionsData;
    private OptionsData OptionsData;
    private IDataService DataService;
    public bool EncryptionEnabled;

    private void Awake()
    {
        DefaultOptionsData = new OptionsData(1, 1, 1, 1);
        OptionsData = new OptionsData();
        DataService = new JsonDataService();
        InitializeJson();
        AssignUIButtons();
        DeserializeJson();
        _masterVolumeSlider.value = OptionsData.MasterVolume;
        _SFXVolumeSlider.value = OptionsData.SFXVolume;
        _musicVolumeSlider.value = OptionsData.MusicVolume;
        _dialogsVolumeSlider.value = OptionsData.DialogsVolume;
    }

    private void AssignUIButtons()
    {
        _applyChangesButton.onClick.AddListener(OnApplyChangesButtonClick);
    }

    public void InitializeJson()
    {
        DataService.InitializeData("/options-data.json", DefaultOptionsData, EncryptionEnabled);
    }

    public void SerializeJson()
    {
        DataService.SaveData("/options-data.json", OptionsData, EncryptionEnabled);
    }

    public void DeserializeJson() 
    {
        this.OptionsData = DataService.LoadData<OptionsData>("/options-data.json", EncryptionEnabled);
    }

    private void OnApplyChangesButtonClick()
    {
        OptionsData.MasterVolume = this._masterVolumeSlider.value;
        OptionsData.SFXVolume = this._SFXVolumeSlider.value;
        OptionsData.MusicVolume = this._musicVolumeSlider.value;
        OptionsData.DialogsVolume = this._dialogsVolumeSlider.value;
        SerializeJson();
    }
}
