using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LoadingScreen/LoadinScreenTipsData", fileName = "DefaultLoadingScreenTipsData")]
public class LoadingScreenTipsData : ScriptableObject
{
    [field: SerializeField] public List<string> TipsList { private set; get; }
}
