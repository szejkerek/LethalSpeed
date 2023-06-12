using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenUIManager : MonoBehaviour
{
    [SerializeField] Button restartButton;
    [SerializeField] Button mainMenuButton;


    private void Awake()
    {
        restartButton.onClick.AddListener(OnRestartButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
    }

    void OnRestartButtonClick()
    {
        GameManager.Instance.ResetGame();
    }
    void OnMainMenuButtonClick()
    {
        SceneLoader.Instance.LoadNewSceneByBuildIndex(0);
    }
}
