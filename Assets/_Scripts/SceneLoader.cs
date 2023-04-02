using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SceneLoader : Singleton<SceneLoader>
{
    [Header("UI")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TMP_Text progressInfoText;
    [SerializeField] private TMP_Text tipText;

    [Header("Loading screen data")]
    [SerializeField] private List<LoadinScreenTipsData> tipsData;

    private Slider progressBar;
    private CanvasGroup tipTextCanvasGroup;
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    protected override void Awake()
    {
        base.Awake();
        progressBar = loadingScreen.GetComponentInChildren<Slider>();
        tipTextCanvasGroup = tipText.GetComponent<CanvasGroup>();
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
        try
        {
            CheckTipsDataCorrectness();
        }
        catch(Exception ex)
        {
            Debug.LogWarning(ex.Message);
            yield break;
        }

        tipTextCanvasGroup.alpha = 0f;
        while (loadingScreen.activeInHierarchy)
        {
            tipTextCanvasGroup.DOFade(1, .5f);
            tipText.text = GetTip();
            yield return new WaitForSeconds(3f);
            tipTextCanvasGroup.DOFade(0, .5f);
            yield return new WaitForSeconds(.5f);
        }
    }

    private void CheckTipsDataCorrectness()
    {
        if (tipsData.Count == 0)
        {
            throw new System.Exception("TipsData is empty.");
        }

        foreach (LoadinScreenTipsData tipsList in tipsData)
        {
            if (tipsList == null)
            {
                throw new System.Exception("One of tipsData tipsList is null.");
            }
            else if (tipsList.TipsList.Count == 0)
            {
                throw new System.Exception("One of tipsData tipsList is empty.");
            }
        }
    }

    private string GetTip()
    {
        int tipsDataIndex;
        int tipsListIndex;

        tipsDataIndex = UnityEngine.Random.Range(0, tipsData.Count);
        tipsListIndex = UnityEngine.Random.Range(0, tipsData[tipsDataIndex].TipsList.Count);

        return tipsData[tipsDataIndex].TipsList[tipsListIndex];
    }

    private IEnumerator GetSceneLoadProgress()
    {
        float totalSceneProgress;

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
