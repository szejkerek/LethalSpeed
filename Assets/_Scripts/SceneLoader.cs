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
    [SerializeField] private TMP_Text progressInfoTextField;
    [SerializeField] private TMP_Text tipTextField;

    [Header("Loading screen data")]
    [SerializeField] private List<LoadinScreenTipsData> tipsDataPool; //tips data pool

    private Slider progressBar; //progress bar object
    private CanvasGroup tipTextFieldCanvasGroup; //text field canvas group to manage fading
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>(); //list of currently loading and unloading scenes

    protected override void Awake()
    {
        base.Awake();
        progressBar = loadingScreen.GetComponentInChildren<Slider>();
        tipTextFieldCanvasGroup = tipTextField.GetComponent<CanvasGroup>();
    }

    //Load new scene with loading screen interval
    public void LoadGame()
    {
        loadingScreen.gameObject.SetActive(true);
        StartCoroutine(GenerateTips());
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.Menu));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.MovementScene, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress());
    }

    //Generate random tips from data pool every few seconds
    private IEnumerator GenerateTips()
    {
        if (!IsTipsDataPoolCorrect())
        {
            yield break;
        }

        tipTextFieldCanvasGroup.alpha = 0f;
        while (loadingScreen.activeInHierarchy)
        {
            tipTextFieldCanvasGroup.DOFade(1, .5f);
            tipTextField.text = GetTip();
            yield return new WaitForSeconds(3f);
            tipTextFieldCanvasGroup.DOFade(0, .5f);
            yield return new WaitForSeconds(.5f);
        }
    }

    //Check correctens of passed tips data pool
    private bool IsTipsDataPoolCorrect()
    {
        if (tipsDataPool.Count == 0)
        {
            Debug.LogWarning("TipsData is empty.");
            return false;
        }

        foreach (LoadinScreenTipsData tipsList in tipsDataPool)
        {
            if (tipsList == null)
            {
                Debug.LogWarning("One of tipsData tipsList is null.");
                return false;
            }
            else if (tipsList.TipsList.Count == 0)
            {
                Debug.LogWarning("One of tipsData tipsList is empty.");
                return false;
            }
        }

        return true;
    }

    //Return one random tip from data pool
    private string GetTip()
    {
        int tipsDataIndex;
        int tipsListIndex;

        tipsDataIndex = UnityEngine.Random.Range(0, tipsDataPool.Count);
        tipsListIndex = UnityEngine.Random.Range(0, tipsDataPool[tipsDataIndex].TipsList.Count);

        return tipsDataPool[tipsDataIndex].TipsList[tipsListIndex];
    }

    //Based on loading scene progress calculate progress bar value
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
                progressInfoTextField.text = string.Format("Loading Map: {0}%", totalSceneProgress * 100f);
                yield return null;
            }
        }

        yield return new WaitForSeconds(10);
        loadingScreen.gameObject.SetActive(false);
    }
}
