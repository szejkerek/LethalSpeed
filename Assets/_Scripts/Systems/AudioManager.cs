using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{

    [Header("AudioSources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundSource;
    
    [Header("AudioSoundData")]
    [SerializeField] private SFXLib _sfxLib;
    [SerializeField] private MusicLib _musicLib;

    public SFXLib SFXLib { get => _sfxLib;}
    public MusicLib MusicLib { get => _musicLib;}

    public void PlayGlobalMusic(AudioClip clip)
    {
        if (_musicSource.isPlaying)
        {
            _musicSource.Stop();
        }

        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void PlayGlobalSound(AudioClip clip, float vol = 1)
    {
        _soundSource.PlayOneShot(clip, vol);
    }

}
