using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEnemyAI : MonoBehaviour
{
    public EnemyAudioLib EnemyAudioLib => _enemyAudioLib;
    [SerializeField] EnemyAudioLib _enemyAudioLib;

    public AudioSource GeneralAudioSource => _generalAudioSource;
    [SerializeField] AudioSource _generalAudioSource;

    public AudioSource FootsAutioSource => _footsAudioSource;
    [SerializeField] AudioSource _footsAudioSource;

    public AudioSource MouthAudioSource => _mouthAudioSource;
    [SerializeField] AudioSource _mouthAudioSource;

    public AudioSource GunGeneralAudioSource => _gunGeneralAudioSource;
    [SerializeField] AudioSource _gunGeneralAudioSource;
    public AudioSource GunShotAudioSource => _gunShotAudioSource;
    [SerializeField] AudioSource _gunShotAudioSource;

    public void Footstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5)
        {
            _enemyAudioLib.Footsteps.PlayRandomized(_footsAudioSource);
        }
    }

    public void PlayDeathSound()
    {
        _enemyAudioLib.Death.PlayRandomized(_mouthAudioSource);
    }
}
