using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public static event Action OnAllEnemiesKilled;

    [SerializeField] RectTransform EnemyCountUI;
    [SerializeField] float uiHideDistance = 150f;
    [SerializeField] TMP_Text EnemyCountText;

    Enemy[] allEnemies;
    bool _noEnemiesLeft = false;
    int _enemyOverallCount = 0;
    int _enemyCurrentCount;
    private Vector2 endPos;
    private Vector2 startingPos;

    public bool NoEnemiesLeft => _noEnemiesLeft;
    public int EnemyOverallCount => _enemyOverallCount;
    public int EnemyCurrentCount => _enemyCurrentCount;

    protected override void Awake()
    {
        base.Awake();
        allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.InstanceID);
        _enemyOverallCount = allEnemies.Length;
        _enemyCurrentCount = _enemyOverallCount;
        _noEnemiesLeft = (_enemyCurrentCount == 0); 

        endPos = EnemyCountUI.anchoredPosition;
        startingPos = endPos;
        startingPos.x += uiHideDistance;
        EnemyCountUI.anchoredPosition = startingPos;
        EnemyCountText.text = EnemyOverallCount.ToString();

        Enemy.OnEnemyDeath += HandleEnemyKilled;
        Player.onPlayerGetHit += HandlePlayerKilled;
    }

    private void Start()
    {
        ShowEnemyCountUI();
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyDeath -= HandleEnemyKilled;
        Player.onPlayerGetHit -= HandlePlayerKilled;
    }

    private void HandlePlayerKilled()
    {
        foreach (Enemy enemy in allEnemies)
        {
            enemy.StateMachine.CurrentState.SwitchState(enemy.StateMachine.StatesFactory.Patrol());
            enemy.AimAtTargetRigController.TurnOffRig(2f);
        }
    }

    void ShowEnemyCountUI(bool enebled = true)
    {
        if (enebled)
        {
            DOTween.To(() => EnemyCountUI.anchoredPosition,
                       x => EnemyCountUI.anchoredPosition = x,
                       endPos, .75f).SetDelay(2f);
        }
        else
        {
            DOTween.To(() => EnemyCountUI.anchoredPosition,
                       x => EnemyCountUI.anchoredPosition = x,
                       startingPos, .25f).SetDelay(1f);
        }
    }

    void HandleEnemyKilled()
    {
        _enemyCurrentCount--;
        EnemyCountText.text = _enemyCurrentCount.ToString();

        if (_enemyCurrentCount <= 0)
        {
            _enemyCurrentCount = 0;
            ShowEnemyCountUI(false);
            _noEnemiesLeft = true;
            OnAllEnemiesKilled?.Invoke();
        }
    }
}
