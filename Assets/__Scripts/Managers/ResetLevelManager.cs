using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLevelManager : MonoBehaviour
{
    [SerializeField] private PauseMenuManager pauseMenuMenager = null;
    void Update()
    {
        if (pauseMenuMenager != null && pauseMenuMenager.IsPaused())
        {
            return;
        }

        pauseMenuMenager.IsPaused();

        if (Input.GetKeyUp(KeyCode.R))
        {
            SceneLoader.Instance.ReloadScene();
        }
    }
}
