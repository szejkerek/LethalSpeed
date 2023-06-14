using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndOfLevelScreenManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _mainMenuButton;

    [SerializeField] private TimerManager _timer;
    private void Awake()
    {
        AsignButton();
    }

    private void AsignButton()
    {
        _nextLevelButton.onClick.AddListener(LoadNextLevel);
        _mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    private void LoadNextLevel()
    {
        SceneLoader.Instance.LoadNextSceneInBuilder();
    }

    private void LoadMainMenu()
    {
        SceneLoader.Instance.LoadNewSceneByBuildIndex((int)SceneBuildIndexes.Menu);
    }
}
