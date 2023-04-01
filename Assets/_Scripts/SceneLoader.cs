using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : Singleton<SceneLoader>
{
    [Header("UI")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TMP_Text progressInfoText;
    [SerializeField] private TMP_Text tipText;

    [Header("Loading screen data")]
    [SerializeField] private LoadinScreenTipsData tipsData;

    private int tipCount;
    private List<string> tips;
    private Slider progressBar;
    private float totalSceneProgress;
    private CanvasGroup tipTextCanvasGroup;
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    protected override void Awake()
    {
        base.Awake();
        progressBar = loadingScreen.GetComponentInChildren<Slider>();
        tipTextCanvasGroup = tipText.GetComponent<CanvasGroup>();
        tips = tipsData.TipsList;
    }

    public void LoadGame()
    {
        loadingScreen.gameObject.SetActive(true);
        StartCoroutine(GenerateTips());
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Menu));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MovementScene, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress());
    }

    private IEnumerator GenerateTips()
    {
        tipCount = Random.Range(0, tips.Count);
        tipText.text = tips[tipCount];

        while(loadingScreen.activeInHierarchy)
        {
            yield return new WaitForSeconds(3f);
            tipTextCanvasGroup.DOFade(0, .5f);
            yield return new WaitForSeconds(.5f);
            tipCount++;

            if (tipCount >= tips.Count)
            {
                tipCount = 0;
            }

            tipText.text = tips[tipCount];
            tipTextCanvasGroup.DOFade(1, .5f);
        }
    }

    private IEnumerator GetSceneLoadProgress()
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
                yield return null;
            }
        }

        yield return new WaitForSeconds(10);
        loadingScreen.gameObject.SetActive(false);
    }
}
