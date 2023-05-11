using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "LoadingScreen/LoadinScreenTipsData", fileName = "DefaultLoadingScreenImageData")]
public class LoadingScreenImageData : ScriptableObject
{
    [field: SerializeField] public SceneBuildIndexes SceneBuildIndex { private set; get; }
    [field: SerializeField] public List<Sprite> LoadinScreenBackground { private set; get; }

    public int GetSceneBuildIndex()
    {
        return (int)SceneBuildIndex;
    }
}
