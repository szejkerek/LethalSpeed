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
    protected override void Awake()
    {
        base.Awake();
        //SceneManager.LoadSceneAsync((int)SceneIndexes.MENU, LoadSceneMode.Single);
    }

    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void LoadGame()
    {
        //loadingScreen.gameObject.SetActive(true);

        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Menu));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.TestScene, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public IEnumerator GetSceneLoadProgress()
    {
        for ( int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                yield return null;
            }
        }

       //loadingScreen.gameObject.SetActive(false);
    }
}
