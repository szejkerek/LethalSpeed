using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    [SerializeField] GameObject _crosshairCanvas;
    [SerializeField] GameObject _hitmarker;
    
    [SerializeField] float _hitmarkerTimer;


    private void Awake()
    {
        PlayerWeapon.onHitRegistered += ShowHitmarker;
        _hitmarker.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerWeapon.onHitRegistered -= ShowHitmarker;
    }

    public void ShowCrosshair(bool enabled)
    {
        _crosshairCanvas.SetActive(enabled);
    }

    void ShowHitmarker()
    {
        StartCoroutine(EnableHitmarkerForTime());
        AudioManager.Instance.PlayGlobalSound(AudioManager.Instance.SFXLib.Hitmarker, 0.2f);
    }

    private IEnumerator EnableHitmarkerForTime()
    {
        _hitmarker.SetActive(true);
        yield return new WaitForSeconds(_hitmarkerTimer);
        _hitmarker.SetActive(false);
    }
}
