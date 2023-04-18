using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioData/SFXLib", fileName = "SFXLib")]
public class SFXLib : ScriptableObject
{
    [field: SerializeField] public AudioClip TestSound { private set; get; }
    [field: SerializeField] public AudioClip TestSound2 { private set; get; }
}
