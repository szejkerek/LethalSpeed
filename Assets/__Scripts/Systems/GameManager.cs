using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PauseMenuManager))]
public class GameManager : Singleton<GameManager>
{
    private PauseMenuManager pauseMenuMenager;

    protected override void Awake()
    {
        base.Awake();
        pauseMenuMenager = GetComponent<PauseMenuManager>();
    }

    void Update()
    {
        GetPressedKeys();
    }

    private void GetPressedKeys()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (pauseMenuMenager.IsPaused)
            {
                return;
            }

            ResetGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuMenager.TooglePasueMenu();
        }
    }

    public void ResetGame()
    {
        SceneLoader.Instance.ReloadScene();
    }
}
