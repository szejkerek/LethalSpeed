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
    [SerializeField] private Image loadingScreenImage;


    [Header("Loading screen data")]
    [SerializeField] private List<LoadingScreenTipsData> tipsDataPool;
    [SerializeField] private List<LoadingScreenImageData> imageDataPool;

    private Slider progressBar;
    private CanvasGroup tipTextFieldCanvasGroup; //text field canvas group to manage fading
    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>(); //list of currently loading and unloading scenes
    private Dictionary<int, List<Sprite>> imageDataPoolHashTable = new Dictionary<int, List<Sprite>>();

    protected override void Awake()
    {
        base.Awake();
        progressBar = loadingScreen.GetComponentInChildren<Slider>();
        tipTextFieldCanvasGroup = tipTextField.GetComponent<CanvasGroup>();

        foreach (LoadingScreenImageData imageDataToCoopy in imageDataPool)
        {
            if(!imageDataPoolHashTable.ContainsKey(imageDataToCoopy.GetSceneBuildIndex()))
            {
                imageDataPoolHashTable.Add(imageDataToCoopy.GetSceneBuildIndex(), imageDataToCoopy.LoadinScreenBackground);
            }
            else
            {
                Debug.LogWarning("There can be only one background image data pool for each scene.");
            }
        }
    }

    public void LoadNextSceneInBuilder()
    {
        int sceneToUnloadBuildIndex = SceneManager.GetActiveScene().buildIndex;
        int sceneToLoadBuildIndex = SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).buildIndex;

        UnloadSceneAndLoadNewOneByName(sceneToUnloadBuildIndex, sceneToLoadBuildIndex);
    }

    public void ReloadScene()
    {
        int sceneToReloadBuildIndex = SceneManager.GetActiveScene().buildIndex;

        UnloadSceneAndLoadNewOneByName(sceneToReloadBuildIndex, sceneToReloadBuildIndex);
    }

    public void LoadNewSceneByBuildIndex(int sceneToLoadBuildIndex)
    {
        int sceneToUnloadBuildIndex = SceneManager.GetActiveScene().buildIndex; 

        UnloadSceneAndLoadNewOneByName(sceneToUnloadBuildIndex, sceneToLoadBuildIndex);
    }

    public void UnloadSceneAndLoadNewOneByName(int sceneToUnloadBuildIndex, int sceneToLoadBuildIndex)
    {
        SetBackGroundImage(sceneToLoadBuildIndex);
        loadingScreen.gameObject.SetActive(true);
        StartCoroutine(GenerateTips());
        scenesLoading.Add(SceneManager.UnloadSceneAsync(sceneToUnloadBuildIndex));
        scenesLoading.Add(SceneManager.LoadSceneAsync(sceneToLoadBuildIndex));
        StartCoroutine(GetSceneLoadProgress());
    }

    private void SetBackGroundImage(int sceneToLoadBuildIndex)
    {
        if(!IsImageDataPoolCorrect(sceneToLoadBuildIndex))
        {
            loadingScreenImage.color = Color.black;
            return;
        }

        loadingScreenImage.sprite = imageDataPoolHashTable[sceneToLoadBuildIndex][UnityEngine.Random.Range(0, imageDataPoolHashTable[sceneToLoadBuildIndex].Count)];
    }

    private bool IsImageDataPoolCorrect(int sceneToLoadBuildIndex)
    {
        if(imageDataPoolHashTable.Count == 0)
        {
            Debug.LogWarning("Loading screen background image data pool is empty.");
            return false;
        }
        if (!imageDataPoolHashTable.ContainsKey(sceneToLoadBuildIndex))
        {
            Debug.LogWarning("There is no corresponding background image data pool for currently loading scene.");
            return false;
        }
        if (imageDataPoolHashTable[sceneToLoadBuildIndex].Count == 0)
        {
            Debug.LogWarning("Loading screen background image data pool of chosen scene is empty.");
            return false;
        }

        return true;
    }

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

    private bool IsTipsDataPoolCorrect()
    {
        if (tipsDataPool.Count == 0)
        {
            Debug.LogWarning("TipsData is empty.");
            return false;
        }

        foreach (LoadingScreenTipsData tipsList in tipsDataPool)
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

        yield return new WaitForSeconds(10); //TODO: delete this line, it is used only for testing purposes
        loadingScreen.gameObject.SetActive(false);
    }
}