using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public RectTransform gameOverText;
    public RectTransform restartButton;
    public RectTransform quitButton;

    private Vector2 gameOverTextStartingPos; 
    private Vector2 restartButtonStartingPos; 
    private Vector2 quitButtonStartingPos; 

    [Header("Timer")]
    [SerializeField] private float _textTimer;
    [SerializeField] private float _respawnButtonTimer;
    [SerializeField] private float _quitButtonTimer;

    private void Awake()
    {
        gameOverTextStartingPos = gameOverText.position;
        restartButtonStartingPos = restartButton.position;
        quitButtonStartingPos = quitButton.position;
    }

    private void OnEnable()
    {
        gameOverText.DOLocalMoveX(0, 0.35f).SetEase(Ease.InOutExpo).SetDelay(_textTimer);
        restartButton.DOLocalMoveY(-230, 0.4f).SetEase(Ease.InOutExpo).SetDelay(_respawnButtonTimer);
        quitButton.DOLocalMoveY(-330, 0.4f).SetEase(Ease.InOutExpo).SetDelay(_quitButtonTimer);
    }

    private void OnDisable()
    {
        gameOverText.position = gameOverTextStartingPos;
        restartButton.position = restartButtonStartingPos;
        quitButton.position = quitButtonStartingPos;
    }
}
