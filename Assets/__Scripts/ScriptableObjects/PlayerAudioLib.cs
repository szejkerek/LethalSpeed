using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioData/PlayerAudioLib", fileName = "PlayerAudioLib")]
public class PlayerAudioLib : ScriptableObject
{
    [field: SerializeField] public List<AudioClip> SwordWoosh { private set; get; }
}