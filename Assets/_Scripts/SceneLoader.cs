using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class SceneLoader : Singleton<SceneLoader>
{
    public GameObject loadingScreen;
    public Slider progressBar;

    private void Awake()
    {
        SceneManager.LoadSceneAsync((int)SceneIndexes.MENU, LoadSceneMode.Additive);
    }

    private void LoadGame()
    {
        SceneManager.UnloadScene((int)SceneIndexes.MENU);
        SceneManager.LoadSceneAsync((int)SceneIndexes.MAP, LoadSceneMode.Additive);
    }
}
