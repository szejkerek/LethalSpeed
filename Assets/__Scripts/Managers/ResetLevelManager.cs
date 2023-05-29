using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLevelManager : MonoBehaviour
{
    [SerializeField] private PauseMenuManager pauseMenuMenager;

    private void Awake()
    {
        pauseMenuMenager = null;
    }
    void Update()
    {
        if (pauseMenuMenager != null && pauseMenuMenager.IsPaused())
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneLoader.Instance.ReloadScene();
        }
    }
}
