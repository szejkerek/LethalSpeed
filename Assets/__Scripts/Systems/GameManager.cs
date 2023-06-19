using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PauseMenuManager))]
public class GameManager : Singleton<GameManager>
{
    private PauseMenuManager _pauseMenuMenager;
    private CrosshairManager _crosshairManager;
    private DeathScreenManager _deathScreenManager;
    private EndOfLevelScreenManager _endOfLevelScreenManager;
    private TimerManager _timerManager;
    private Player _player;
    private PlayerWeapon _playerWeapon;

    public bool EnableQuickRestarts { get => _enableQuickRestarts; set => _enableQuickRestarts = value; }
    private bool _enableQuickRestarts = true;

    protected override void Awake()
    {
        base.Awake();
        _player = FindFirstObjectByType<Player>();
        _pauseMenuMenager = GetComponent<PauseMenuManager>();
        _crosshairManager = GetComponent<CrosshairManager>();
        _deathScreenManager = GetComponent<DeathScreenManager>();
        _endOfLevelScreenManager = GetComponent<EndOfLevelScreenManager>();
        _timerManager = GetComponent<TimerManager>();
        _playerWeapon = _player.GetComponent<PlayerWeapon>();
        EndZone.OnEndZonePlayerEnter += HandleFinishLevel;

        if (_player is null)
        {
            Debug.LogError("Failed to find Player in scene");
        }
        else
        {
            Player.onPlayerGetHit += HandlePlayerDeath;
        }
    }

    private void OnDestroy()
    {
        Player.onPlayerGetHit -= HandlePlayerDeath;
        EndZone.OnEndZonePlayerEnter -= HandleFinishLevel;
        Helpers.DisableCursor();
        _enableQuickRestarts = true;
        _deathScreenManager.ResetGameSpeed();
    }


    void Update()
    {
        GatherGameCommands();
    }

    private void GatherGameCommands()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_pauseMenuMenager.IsPaused || !_enableQuickRestarts)
                return;
            ResetGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseMenuMenager.TooglePasueMenu();
        }
    }

    public void ResetGame()
    {
        SceneLoader.Instance.ReloadScene();
    }

    private void HandlePlayerDeath()
    {
        _deathScreenManager.ShowDeathScreen();
        _pauseMenuMenager.EnableInputs = false;
        _enableQuickRestarts = false;
        _crosshairManager.ShowCrosshair(enabled: false);
        Helpers.EnableCursor();
    }

    private void HandleFinishLevel()
    {
        _pauseMenuMenager.EnableInputs = false;
        _crosshairManager.ShowCrosshair(false);
        _timerManager.ShowTimer(false);
        _timerManager.StopTimer();
        _playerWeapon.EnableInputs = false;
        Time.timeScale = 0;
        _endOfLevelScreenManager.ShowEndOfLevelCanvas();
    }
}
