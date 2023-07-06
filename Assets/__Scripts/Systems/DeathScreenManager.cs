using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _deathScreen;
    [SerializeField] Button restartButton;
    [SerializeField] Button mainMenuButton;

    [Header("Slowdown effect")]
    [SerializeField] float slowingDownTime = 3f;
    [SerializeField] private float targetTimeScale = 0.2f;
    [SerializeField] private float volumeDecressePower = 25;

    private float originalTimeScale = 1f;
    private float originalVolume;

    private void Awake()
    {
        _deathScreen.SetActive(false);
        restartButton.onClick.AddListener(OnRestartButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
    }

    IEnumerator SlowdownGame()
    {
        float elapsedTime = 0f;
        AudioManager.Instance.Mixers.GetFloat("masterVolume", out originalVolume);
        while (elapsedTime < slowingDownTime)
        {
            float lerpValue = Mathf.Lerp(originalTimeScale, targetTimeScale, elapsedTime / slowingDownTime);
            Time.timeScale = lerpValue;
            AudioManager.Instance.Mixers.SetFloat("sfxPitch", Time.timeScale);
            AudioManager.Instance.Mixers.SetFloat("masterVolume", originalVolume - ((1 - lerpValue)* volumeDecressePower));
            yield return null;
            elapsedTime += Time.unscaledDeltaTime;
        }
    }

    public void ResetGameSpeed()
    {
        Time.timeScale = originalTimeScale;

        AudioManager.Instance?.Mixers.SetFloat("sfxPitch", Time.timeScale);
        AudioManager.Instance?.Mixers.SetFloat("masterVolume", originalVolume);
    }

    public void ShowDeathScreen(bool enabled = true)
    {
        _deathScreen.SetActive(enabled);
        if(enabled)
        {
            StartCoroutine(SlowdownGame());
        }
        else
        {
            ResetGameSpeed();
        }

    }
    void OnRestartButtonClick()
    {
        GameManager.Instance.ResetGame();
        ResetGameSpeed();
    }
    void OnMainMenuButtonClick()
    {
        SceneLoader.Instance.LoadMenu();
        ResetGameSpeed();
    }


}
