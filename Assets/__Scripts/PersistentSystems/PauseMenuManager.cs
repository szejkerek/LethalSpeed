using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : Singleton<PauseMenuManager>
{
    private bool isPaused;
    [SerializeField] private GameObject PauseMenuCanvas;

    protected override void Awake()
    {
        base.Awake();
        Time.timeScale = 1.0f;
        isPaused = false;
        PauseMenuCanvas.SetActive(false);
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            return;
        }

        if(Input.GetKeyUp(KeyCode.Escape)) 
        {
            if(isPaused == true) 
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
    }

    private void Play()
    {
        Time.timeScale = 1.0f;
        PauseMenuCanvas.SetActive(false);
        isPaused = false;
    }

    private void Stop() 
    {
        Time.timeScale = .0f;
        PauseMenuCanvas.SetActive(true);
        isPaused = true;
    }
}
