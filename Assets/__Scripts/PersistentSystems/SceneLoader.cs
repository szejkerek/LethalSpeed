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
    [SerializeField] private bool startUpWithChoosenScene; 
    [SerializeField] private SceneBuildIndexes startUpScene; 
    [SerializeField] private int additionalLoadingTime; 
    [SerializeField] private float textFadeTime; 
    [SerializeField] private float tipPersistTime; 
    [SerializeField] private float nextTipGenerationTime; 

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
        loadingScreen.SetActive(false);
        progressBar = loadingScreen.GetComponentInChildren<Slider>();
        tipTextFieldCanvasGroup = tipTextField.GetComponent<CanvasGroup>();
        PopulateImageDataPoolHashTable();
        PopulateTipsList();

        if (startUpWithChoosenScene)
        {
            LoadScene(startUpScene);
        }
    }

    public void LoadNextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings && nextIndex >= 0)
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            LoadMenu();
        }       
    }

    public void LoadMenu()
    {
        LoadScene(SceneBuildIndexes.Menu);
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(SceneBuildIndexes scene)
    {
        int sceneIndex = (int)scene;
        LoadScene(sceneIndex);
    }

    public void LoadScene(int sceneIndex)
    {
        SetBackgroundImage(sceneIndex);
        loadingScreen.gameObject.SetActive(true);
        StartCoroutine(GenerateTipsOnLoadingScreen());
        sceneLoadingAsyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        StartCoroutine(StartProgressBar());
    }

    private void SetBackgroundImage(int sceneToLoadBuildIndex)
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
                ResetTipList();
            }

            tipTextFieldCanvasGroup.DOFade(1, textFadeTime);
            tipTextField.text = GetRandomTip();
            yield return new WaitForSeconds(tipPersistTime);
            tipTextFieldCanvasGroup.DOFade(0, textFadeTime);
            yield return new WaitForSeconds(nextTipGenerationTime);
        }
    }

    private void ResetTipList()
    {
        tipsList.AddRange(usedTipsList);
        usedTipsList.Clear();
    }

    private string GetRandomTip()
    {
        int tipStringIndex;
        tipStringIndex = UnityEngine.Random.Range(0, tipsList.Count);
        string nextTipToGenerate = tipsList[tipStringIndex];
        tipsList.RemoveAt(tipStringIndex);
        usedTipsList.Add(nextTipToGenerate);
        return nextTipToGenerate;
    }

    private IEnumerator StartProgressBar()
    {
        while (!sceneLoadingAsyncOperation.isDone)
        {
            GenerateProgresBar();
            yield return null;
        }

        yield return new WaitForSeconds(additionalLoadingTime);
        ResetLoadingScreen();
    }

    private void GenerateProgresBar()
    {
        float totalSceneProgress;
        totalSceneProgress = 0;
        totalSceneProgress += Mathf.Clamp01(sceneLoadingAsyncOperation.progress / .9f);
        progressBar.value = totalSceneProgress;
        progressInfoTextField.text = string.Format("Loading Map: {0}%", totalSceneProgress * 100f);
    }

    private void ResetLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(false);
        loadingScreenImage.sprite = null;
        loadingScreenImage.color = Color.white;
        progressBar.value = 0;
        tipTextField.text = null;
        tipTextFieldCanvasGroup.alpha = 0f;
        ResetTipList();
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
            if (tipsDataToCopy != null && tipsDataToCopy.IsCorrect())
            {
                tipsList.AddRange(tipsDataToCopy.TipsList);
            }
        }
    }

}
