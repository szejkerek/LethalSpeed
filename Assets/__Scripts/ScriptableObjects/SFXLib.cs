using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioData/SFXLib", fileName = "SFXLib")]
public class SFXLib : ScriptableObject
{
    [field: SerializeField] public AudioClip Hitmarker { private set; get; }
}
