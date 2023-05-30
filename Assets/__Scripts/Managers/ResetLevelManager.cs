using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLevelManager : MonoBehaviour
{
    [SerializeField] private PauseMenuManager pauseMenuMenager;

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
