using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] CreditsData CreditsData;
    [SerializeField] GameObject creditLinePrefab;
    [SerializeField] Transform startingPos;
    [SerializeField] Transform endPos;
    [SerializeField] float textSpeed;

    [SerializeField] TMP_Text text;

    private void Start()
    {
        text.text = "";
        foreach (CreditLine line in CreditsData.CreditList)
        {
            TMP_Text creditLine = Instantiate(creditLinePrefab, startingPos.position, Quaternion.identity).GetComponent<TMP_Text>();
            creditLine.transform.SetParent(transform);
            creditLine.text = PopulateLine(line);
            text.text += creditLine.text;
        }

        Invoke(nameof(LoadMenu), textSpeed + 3f);
        text.transform.DOMove(endPos.position, textSpeed).SetEase(Ease.Linear);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            LoadMenu();
    }

    private void LoadMenu()
    {
        SceneLoader.Instance.LoadMenu();
    }

    string PopulateLine(CreditLine line)
    {
        string lineToShow = "";
        switch (line.TeamMember)
        {
            case TeamMembers.Bartlomiej_Gordon:
                lineToShow = "Bartłomiej Gordon - ";
                break;
            case TeamMembers.Mikolaj_Gajos:
                lineToShow = "Mikolaj Gajos - ";
                break;
            case TeamMembers.Pawel_Kupczak:
                lineToShow = "Pawel Kupczak - ";
                break;
            case TeamMembers.Szymon_Szedel:
                lineToShow = "Szymon Szedel - ";
                break;
            case TeamMembers.Jakub_Dusza:
                lineToShow = "Jakub Dusza - ";
                break;
            case TeamMembers.Marcin_Mitrega:
                lineToShow = "Marcin Mitrega - ";
                break;
        }

        lineToShow += line.content;
        lineToShow += "\n";
        return lineToShow;
    }
}
