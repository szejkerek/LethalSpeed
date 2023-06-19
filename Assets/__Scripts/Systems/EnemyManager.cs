using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static event Action OnAllEnemiesKilled;

    Enemy[] allEnemies;

    int _enemyOverallCount = 0;
    int _enemyCurrentCount;
    public int EnemyOverallCount => _enemyOverallCount;
    public int EnemyCurrentCount => _enemyCurrentCount;

    private void Awake()
    {
        allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.InstanceID);
        _enemyOverallCount = allEnemies.Length;
        _enemyCurrentCount = _enemyOverallCount;
        Enemy.OnEnemyDeath += HandleEnemyKilled;
        Player.onPlayerGetHit += HandlePlayerKilled;
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

    }

    void HandleEnemyKilled()
    {
        _enemyCurrentCount--;
        if(_enemyCurrentCount <= 0)
        {
            _enemyCurrentCount = 0;           
            OnAllEnemiesKilled?.Invoke();
        }
    }
}
