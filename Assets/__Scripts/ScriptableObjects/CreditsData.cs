using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Credits/CreditsData", fileName = "CreditsData")]

public class CreditsData : ScriptableObject
{
    [field: SerializeField] public List<CreditLine> CreditList { private set; get; }
}

[System.Serializable]
public struct CreditLine
{
    public TeamMembers TeamMember;
    public string content;
}

[System.Serializable]
public enum TeamMembers
{
    Bartlomiej_Gordon,
    Mikolaj_Gajos,
    Pawel_Kupczak,
    Szymon_Szedel,
    Jakub_Dusza,
    Marcin_Mitrega,
    Other
}