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
    [Header("Development")]
    [SerializeField] private bool enableLoadChosenSceneOnGameStart; //TODO: delete this line, it is used only for testing purposes
    [SerializeField] private SceneBuildIndexes firstSceneToLoadOnGameStart; //TODO: delete this line, it is used only for testing purposes
    [SerializeField] private int additionalTimeOfLoadingScreenBeingActive; //TODO: delete this line, it is used only for testing purposes

    [Header("UI")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TMP_Text progressInfoTextField;
    [SerializeField] private TMP_Text tipTextField;
    [SerializeField] private Image loadingScreenImage;


    [Header("Loading screen data")]
    [SerializeField] private List<LoadingScreenTipsData> tipsDataPool;
    [SerializeField] private List<LoadingScreenImageData> imageDataPool;

    private Slider progressBar;
    private CanvasGroup tipTextFieldCanvasGroup;
    private AsyncOperation sceneLoadingAsyncOperation;
    private Dictionary<int, List<Sprite>> imageDataPoolHashTable = new Dictionary<int, List<Sprite>>();
    private List<string> tipsList = new List<string>();
    private List<string> usedTipsList = new List<string>();

    protected override void Awake()
    {
        base.Awake();
        progressBar = loadingScreen.GetComponentInChildren<Slider>();
        tipTextFieldCanvasGroup = tipTextField.GetComponent<CanvasGroup>();
        PopulateImageDataPoolHashTable();
        PopulateTipsList();
    }

    private void PopulateImageDataPoolHashTable()
    {
        foreach (LoadingScreenImageData imageDataToCoopy in imageDataPool)
        {
            if (imageDataToCoopy != null && imageDataToCoopy.IsCorrect())
            {
                if (!imageDataPoolHashTable.ContainsKey(imageDataToCoopy.GetSceneBuildIndex()))
                {
                    imageDataPoolHashTable.Add(imageDataToCoopy.GetSceneBuildIndex(), imageDataToCoopy.LoadinScreenBackgroundImages);
                }
                else
                {
                    Debug.LogWarning("SceneLoader: You can't have more than one background image data pool for a scene.");
                }
            }
        }
    }

    private void PopulateTipsList()
    {
        foreach (LoadingScreenTipsData tipsDataToCopy in tipsDataPool)
        {
            if(tipsDataToCopy != null && tipsDataToCopy.IsCorrect())
            {
                tipsList.AddRange(tipsDataToCopy.TipsList);
            }
        }
    }
    private void Start()
    {
        if (enableLoadChosenSceneOnGameStart)
        {
            LoadNewSceneByBuildIndex((int)firstSceneToLoadOnGameStart);
        }
    }

    public void LoadNextSceneInBuilder()
    {
        int sceneToUnloadBuildIndex = SceneManager.GetActiveScene().buildIndex;
        int sceneToLoadBuildIndex = sceneToUnloadBuildIndex + 1;

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
        StartCoroutine(GenerateTipsOnLoadingScreen());
        sceneLoadingAsyncOperation = SceneManager.LoadSceneAsync(sceneToLoadBuildIndex);
        StartCoroutine(BasedOnSceneLoadProgresGenerateDataOnLoadingScreen());
    }

    private void SetBackGroundImage(int sceneToLoadBuildIndex)
    {
        if (!IsImageDataPoolCorrect(sceneToLoadBuildIndex))
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
            Debug.LogWarning("SceneLoader: Loading screen background images data is empty.");
            return false;
        }
        if (!imageDataPoolHashTable.ContainsKey(sceneToLoadBuildIndex))
        {
            Debug.LogWarning("SceneLoader: There is no corresponding background image data pool for currently loading scene.");
            return false;
        }

        return true;
    }

    private IEnumerator GenerateTipsOnLoadingScreen()
    {
        tipTextFieldCanvasGroup.alpha = 0f;
        while (loadingScreen.activeInHierarchy)
        {
            if (tipsList.Count == 0)
            {
                if(usedTipsList.Count == 0)
                {
                    Debug.LogWarning("SceneLoader: Loading screen tips data is empty.");
                    yield break;
                }
                resetTipList();
            }

            tipTextFieldCanvasGroup.DOFade(1, .5f);
            tipTextField.text = GetRandomTipFromTipsList();
            yield return new WaitForSeconds(3f);
            tipTextFieldCanvasGroup.DOFade(0, .5f);
            yield return new WaitForSeconds(.5f);
        }
    }

    private void resetTipList()
    {
        tipsList.AddRange(usedTipsList);
        usedTipsList.Clear();
    }

    private string GetRandomTipFromTipsList()
    {
        int tipStringIndex;
        tipStringIndex = UnityEngine.Random.Range(0, tipsList.Count);
        string nextTipToGenerate = tipsList[tipStringIndex];
        tipsList.RemoveAt(tipStringIndex);
        usedTipsList.Add(nextTipToGenerate);
        return nextTipToGenerate;
    }

    private IEnumerator BasedOnSceneLoadProgresGenerateDataOnLoadingScreen()
    {
        while (!sceneLoadingAsyncOperation.isDone)
        {
            GenerateProgresBar();
            yield return null;
        }

        yield return new WaitForSeconds(additionalTimeOfLoadingScreenBeingActive); //TODO: delete this line, it is used only for testing purposes
        ResetLoadingScreenToDefaultState();
    }

    private void GenerateProgresBar()
    {
        float totalSceneProgress;
        totalSceneProgress = 0;
        totalSceneProgress += Mathf.Clamp01(sceneLoadingAsyncOperation.progress / .9f);
        progressBar.value = totalSceneProgress;
        progressInfoTextField.text = string.Format("Loading Map: {0}%", totalSceneProgress * 100f);
    }

    private void ResetLoadingScreenToDefaultState()
    {
        loadingScreen.gameObject.SetActive(false);
        loadingScreenImage.sprite = null;
        loadingScreenImage.color = Color.white;
        progressBar.value = 0;
        tipTextField.text = null;
        tipTextFieldCanvasGroup.alpha = 0f;
        resetTipList();
    }
}
