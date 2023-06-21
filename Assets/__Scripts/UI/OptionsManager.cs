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

    [Header("Data Save Options")]
    private PersistentDataManager<OptionsData> _optionsSavingManager;

    private void Awake()
    {
        OptionsData DefaultOptionsData = new OptionsData(1, 1, 1, 1);
        IDataService DataService = new JsonDataService();
        _optionsSavingManager = new PersistentDataManager<OptionsData>(DataService, DefaultOptionsData, "/options-data.json");
        LoadData();
        AssignUIButtons();
    }

    private void AssignUIButtons()
    {
        _applyChangesButton.onClick.AddListener(OnApplyChangesButtonClick);
    }

    private void OnApplyChangesButtonClick()
    {
        SaveData();
    }

    private void SaveData()
    {
        OptionsData optionsDataToSave = new OptionsData(
            this._masterVolumeSlider.value,
            this._SFXVolumeSlider.value,
            this._musicVolumeSlider.value,
            this._dialogsVolumeSlider.value);
        _optionsSavingManager.SerializeJson(optionsDataToSave);
    }

    private void LoadData()
    {
        OptionsData optionsDataToLoad = _optionsSavingManager.DeserializeJson();
        _masterVolumeSlider.value = optionsDataToLoad.MasterVolume;
        _SFXVolumeSlider.value = optionsDataToLoad.SFXVolume;
        _musicVolumeSlider.value = optionsDataToLoad.MusicVolume;
        _dialogsVolumeSlider.value = optionsDataToLoad.DialogsVolume;
    }
}
