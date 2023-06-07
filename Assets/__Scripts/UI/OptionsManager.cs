using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
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

    private void OnApplyChangesButtonClick()
    {
        OptionsData.MasterVolume = this._masterVolumeSlider.value;
        OptionsData.SFXVolume = this._SFXVolumeSlider.value;
        OptionsData.MusicVolume = this._musicVolumeSlider.value;
        OptionsData.DialogsVolume = this._dialogsVolumeSlider.value;
    }
}
