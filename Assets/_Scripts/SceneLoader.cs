using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;


public class SceneLoader : Singleton<SceneLoader>
{
    public GameObject loadingScreen;
    public Slider progressBar;
    public TextMeshProUGUI progressInfoText;
    public TextMeshProUGUI tipText;
    public CanvasGroup alphaCanvas;
    public string[] tips;
    protected override void Awake()
    {
        base.Awake();
        //SceneManager.LoadSceneAsync((int)SceneIndexes.Menu, LoadSceneMode.Additive);
    }

    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void LoadGame()
    {
        loadingScreen.gameObject.SetActive(true);

        StartCoroutine(GenerateTips());

        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Menu));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MovementScene, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    private int tipCount;
    public IEnumerator GenerateTips()
    {
        tipCount = Random.Range(0, tips.Length);
        tipText.text = tips[tipCount];

        while(loadingScreen.activeInHierarchy)
        {
            yield return new WaitForSeconds(3f);

            alphaCanvas.DOFade(0, .5f);

            yield return new WaitForSeconds(.5f);

            tipCount++;

            if (tipCount >= tips.Length)
            {
                tipCount = 0;
            }

            tipText.text = tips[tipCount];

            alphaCanvas.DOFade(1, .5f);
        }

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

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) / .9f;

                progressBar.value = totalSceneProgress;

                progressInfoText.text = string.Format("Loading Map: {0}%", totalSceneProgress * 100f);

                Debug.Log($"{totalSceneProgress} dupa {progressBar.value}");

                yield return null;
            }
        }

        yield return new WaitForSeconds(10);

        loadingScreen.gameObject.SetActive(false);
    }
}
