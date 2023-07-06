
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
    [Range(0f,1f)]
    [SerializeField] float _outlineAlphaOnCooldown = 0.45f;

    private PlayerMovement _playerMovement;
    float _dashCooldown;

    private void Awake()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _dashCooldown = _playerMovement.DashProps.DashCooldown;
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
        Color c = _outline.color;
        c.a = _outlineAlphaOnCooldown;
        _outline.color = c;

        StartCoroutine(nameof(FillOutlineOverTime));
    }

    void HandleDashRestore()
    {
        StopCoroutine(nameof(FillOutlineOverTime));
        Color c = _outline.color;
        c.a = 1f;
        _outline.color = c;
        _outline.fillAmount = 1;
    }

    private IEnumerator FillOutlineOverTime()
    {
        float elapsedTime = 0;
        while (_outline.fillAmount < 1)
        {
            elapsedTime += Time.deltaTime;
            _outline.fillAmount = Mathf.Clamp01(elapsedTime / _dashCooldown);
            yield return null;
        }
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
