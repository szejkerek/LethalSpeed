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
            Debug.LogWarning("List of loading screen tips of one of a LoadingScreenTipsData scriptable objects is empty.");
            return false;
        }

        foreach (string tip in TipsList)
        {
            if (tip == null)
            {
                Debug.LogWarning("Tip string of one of a LoadingScreenTipsData scriptable objects is null.");
                return false;
            }
        }

        return true;
    }
}
