using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(menuName = "LoadingScreen/LoadinScreenTipsData", fileName = "DefaultLoadingScreenImageData")]
public class LoadingScreenImageData : ScriptableObject
{
    [field: SerializeField] public SceneBuildIndexes SceneBuildIndex { private set; get; }
    [field: SerializeField] public List<Sprite> LoadinScreenBackgroundImages { private set; get; }

    private void Awake()
    {
        checkIfLoadingScreenImageDataObjectIsCorrect();
    }

    private bool isCorrect = false;

    private void checkIfLoadingScreenImageDataObjectIsCorrect()
    {
        if(LoadinScreenBackgroundImages == null)
        {
            Debug.LogWarning("List of loading screen background images of one of a LoadingScreenImageData scriptable objects is null.");
            return; 
        }
        if(SceneManager.GetSceneByBuildIndex((int)SceneBuildIndex).buildIndex == -1)
        {
            Debug.LogWarning("One of LoadingScreenImageData scriptable objects is assigned to scene that do not occurs in builder.");
            return;
        }
        if (LoadinScreenBackgroundImages.Count == 0)
        {
            Debug.LogWarning("List of loading screen background images of one of a LoadingScreenImageData scriptable objects is empty.");
            return;
        }

        isCorrect = true;
    }

    public bool IsCorrect()
    {
        return isCorrect;
    }

    public int GetSceneBuildIndex()
    {
        return (int)SceneBuildIndex;
    }
}
