using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject ChooseSaveSlotPanel;
    [SerializeField] private GameObject ChooseLevelPanel;
    [SerializeField] private GameObject OptionPanel;
    public void StartGameButton()
    {
        ChooseSaveSlotPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }

    public void QuitChooseSaveSlotButton()
    {
        ChooseSaveSlotPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void ChooseLevelButton()
    {
        ChooseLevelPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }

    public void QuitChooseLevelButton()
    {
        ChooseLevelPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void OptionButton()
    {
        OptionPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }

    public void QuitOptionButton()
    {
        OptionPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Application closed.");
    }
}
