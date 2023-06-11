using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PauseMenuManager))]
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject _deathScreen;

    private PauseMenuManager _pauseMenuMenager;
    private Player _player;

    protected override void Awake()
    {
        base.Awake();
        _pauseMenuMenager = GetComponent<PauseMenuManager>();
        _player = FindFirstObjectByType<Player>();
        _deathScreen.SetActive(false);

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
        _deathScreen.SetActive(false);
    }


    void Update()
    {
        GatherGameCommands();
    }

    private void GatherGameCommands()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_pauseMenuMenager.IsPaused)
            {
                return;
            }

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
        _deathScreen.SetActive(true);
    }
}
