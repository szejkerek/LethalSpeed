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
    public TextMeshProUGUI progressInfoText;
    protected override void Awake()
    {
        base.Awake();
        //SceneManager.LoadSceneAsync((int)SceneIndexes.Menu, LoadSceneMode.Additive);
    }

    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void LoadGame()
    {
        loadingScreen.gameObject.SetActive(true);

        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Menu));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MovementScene, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    private float totalSceneProgress;
    public IEnumerator GetSceneLoadProgress()
    {
        for ( int i = 0; i < scenesLoading.Count; i++)
        {

            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;

                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += Mathf.Clamp(.0f, .9f, operation.progress);
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count);

                progressBar.value = totalSceneProgress / .9f;

                progressInfoText.text = string.Format("Loading Map: {0}%", totalSceneProgress * 100f);

                Debug.Log($"{totalSceneProgress} dupa {progressBar.value}");

                yield return null;
            }
        }

        yield return new WaitForSeconds(2);

        loadingScreen.gameObject.SetActive(false);
    }
}
