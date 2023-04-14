using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public GameObject gameOverText;
    public GameObject restartButton;
    public GameObject quitButton;

    [Header("Timer")]
    [SerializeField] private float _text;
    [SerializeField] private float _respawnButton;
    [SerializeField] private float _quitButton;

    private void OnEnable()
    {
        StartCoroutine(DisplayDeathscreen());
    }

    private IEnumerator DisplayDeathscreen()
    {
        yield return new WaitForSeconds(_text);
        LeanTween.moveLocalX(gameOverText, 0, 0.35f).setEaseInOutExpo();
        yield return new WaitForSeconds(_respawnButton);
        LeanTween.moveLocalY(restartButton, -230, 0.4f).setEaseInOutExpo();
        yield return new WaitForSeconds(_quitButton);
        LeanTween.moveLocalY(quitButton, -330, 0.4f).setEaseInOutExpo();
    }
}
