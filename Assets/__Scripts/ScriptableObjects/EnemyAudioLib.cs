using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioData/EnemyAudioLib", fileName = "EnemyAudioLib")]
public class EnemyAudioLib : ScriptableObject
{
    [field: SerializeField] public Sound Reload { private set; get; }
    [field: SerializeField] public Sound Pistol { private set; get; }
    [field: SerializeField] public List<Sound> Footsteps { private set; get; }
}