using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;


[CreateAssetMenu(menuName = "LoadingScreen/LoadinScreenTipsData", fileName = "DefaultLoadingScreenImageData")]
public class LoadingScreenImageData : ScriptableObject
{
    [field: SerializeField] public SceneBuildIndexes SceneBuildIndex { private set; get; }
    [field: SerializeField] public List<Sprite> LoadinScreenBackgroundImages { private set; get; } = new List<Sprite>();

    public bool IsCorrect()
    {
        if (LoadinScreenBackgroundImages.Count == 0)
        {
            Debug.LogWarning("Scriptable object LoadingScreenImageData: List<Sprite> LoadinScreenBackgroundImages can't be empty.");
            return false;
        }

        foreach(Sprite sprite in LoadinScreenBackgroundImages)
        {
            if (sprite == null)
            {
                Debug.LogWarning("Scriptable object LoadingScreenImageData: List<Sprite> LoadinScreenBackgroundImages can't contain null references.");
                return false;
            }
        }

        return true;
    }

    public int GetSceneBuildIndex()
    {
        return (int)SceneBuildIndex;
    }
}
