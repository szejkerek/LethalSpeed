using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LoadingScreen/ImageManager", fileName = "MusicLib")]
public class LoadingScreenImageManager : ScriptableObject
{
    [field: SerializeField] public int MapNumber { private set; get; }
    [field: SerializeField] public List<Sprite> LoadinScrees { private set; get; }
}
