 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LoadingScreen/LoadinScreenTipsData", fileName = "DefaultLoadingScreenTipsData")]
public class LoadingScreenTipsData : ScriptableObject
{
    [field: SerializeField] public List<string> TipsList { private set; get; } = new List<string>();

    public bool IsCorrect()
    {
        if (TipsList.Count == 0)
        {
            Debug.LogWarning("Scriptable object LoadingScreenTipsData: List<string> TipsList can't be empty.");
            return false;
        }

        foreach (string tip in TipsList)
        {
            if (tip == "")
            {
                Debug.LogWarning("Scriptable object LoadingScreenTipsData: List<string> TipsList can't contain empty strings.");
                return false;
            }
        }

        return true;
    }
}
