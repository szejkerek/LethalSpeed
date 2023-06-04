using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PauseMenuManager))]
public class GameManager : MonoBehaviour
{
    private PauseMenuManager pauseMenuMenager;

    private void Awake()
    {
        pauseMenuMenager = GetComponent<PauseMenuManager>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            if (pauseMenuMenager != null && pauseMenuMenager.IsPaused())
            {
                return;
            }

            SceneLoader.Instance.ReloadScene();
        }
    }
}
