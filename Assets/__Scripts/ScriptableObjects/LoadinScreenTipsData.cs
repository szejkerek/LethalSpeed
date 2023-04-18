using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LoadingScreen/LoadinScreenTipsData", fileName = "DefaultTipsData")]
public class LoadinScreenTipsData : ScriptableObject
{
    [field: SerializeField] public List<string> TipsList { private set; get; }
}
