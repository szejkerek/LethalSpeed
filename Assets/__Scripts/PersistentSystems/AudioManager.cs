using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{

    [Header("AudioSources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundSource;
    
    [Header("AudioSoundData")]
    [SerializeField] private SFXLib _sfxLib;
    [SerializeField] private MusicLib _musicLib;
    [SerializeField] private AudioMixer _mixers;

    public SFXLib SFXLib { get => _sfxLib;}
    public MusicLib MusicLib { get => _musicLib;}
    public AudioMixer Mixers { get => _mixers; }

    public void PlayGlobalMusic(AudioClip clip)
    {
        if (_musicSource.isPlaying)
        {
            _musicSource.Stop();
        }

        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void PlayGlobalSound(AudioClip clip, float vol = 1, bool randomPitch = false)
    {
        if (randomPitch)
            _soundSource.pitch = Random.Range(0.8f, 1.2f);

        _soundSource.PlayOneShot(clip, vol);

        if (randomPitch)
            _soundSource.pitch = 1;
    }

}
