
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    [SerializeField] GameObject _crosshairCanvas;
    [SerializeField] Image _hitmarker;
    [SerializeField] Image _dot;
    [SerializeField] Image _outline;
    
    [SerializeField] float _hitmarkerTimer;

    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        PlayerWeapon.onHitRegistered += ShowHitmarker;
        PlayerMovement.OnPlayerDash += HandleJustDashed;
        PlayerMovement.OnPlayerDashRestored += HandleDashRestore;
        _hitmarker.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerWeapon.onHitRegistered -= ShowHitmarker;
        PlayerMovement.OnPlayerDash -= HandleJustDashed;
        PlayerMovement.OnPlayerDashRestored -= HandleDashRestore;
    }

    public void ShowCrosshair(bool enabled)
    {
        _crosshairCanvas.SetActive(enabled);
    }

    void HandleJustDashed()
    {
        _outline.fillAmount = 0;
    }

    void HandleDashRestore()
    {
        _outline.fillAmount = 1;
    }

    void ShowHitmarker()
    {
        StartCoroutine(EnableHitmarkerForTime());
        AudioManager.Instance.PlayGlobalSound(AudioManager.Instance.SFXLib.Hitmarker, 0.2f);
    }

    private IEnumerator EnableHitmarkerForTime()
    {
        _hitmarker.gameObject.SetActive(true);
        yield return new WaitForSeconds(_hitmarkerTimer);
        _hitmarker.gameObject.SetActive(false);
    }
}
