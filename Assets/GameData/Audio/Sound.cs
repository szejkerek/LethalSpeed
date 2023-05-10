using UnityEngine;

public enum AudioType
{
    None,
    SFX,
    Music,
    Dialogue
}

[System.Serializable]
public class Sound
{
    public AudioClip audioClip;
    AudioType audioType;
    public float volume = 1f; 
    public float pitch = 1f;
    public bool loop = false;
    
    [Space]
    public float minRange = 1;
    public float maxRange = 500;
    [Space]
    public float pitchVariation = 0.1f;
    public float volumeVariation = 0.1f;


    public void PlayRandomized(AudioSource audioSource)
    {
        if (audioClip is null || audioSource is null)
        {
            throw new System.ArgumentNullException(nameof(audioClip));
        }


        audioSource.spatialBlend = 1f;
        audioSource.clip = audioClip;
        audioSource.volume = Random.Range(volume - volumeVariation, volume) /** GetVolumeModifier()*/;
        audioSource.pitch = Random.Range(pitch - pitchVariation, pitch + pitchVariation);
        audioSource.loop = loop;
        audioSource.maxDistance = maxRange;
        audioSource.minDistance = minRange;
        audioSource.Play();
    }

    private float GetVolumeModifier()
    {
        switch (audioType)
        {
            case AudioType.None:
                Debug.LogWarning($"{audioClip.name} has no given audio type. Setting its volume modifier to 1.");
                return 1f;
            case AudioType.SFX:
                return PlayerPrefs.GetFloat("SFXVolumePref");
            case AudioType.Music:
                return PlayerPrefs.GetFloat("MusicVolumePref");
            case AudioType.Dialogue:
                return PlayerPrefs.GetFloat("DialogueVolumePref");
            default:
                return 1f;
        }
    }

}
