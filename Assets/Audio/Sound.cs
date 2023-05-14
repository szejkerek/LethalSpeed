using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip audioClip;
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
        audioSource.volume = Random.Range(volume - volumeVariation, volume);
        audioSource.pitch = Random.Range(pitch - pitchVariation, pitch + pitchVariation);
        audioSource.loop = loop;
        audioSource.maxDistance = maxRange;
        audioSource.minDistance = minRange;
        audioSource.PlayOneShot(audioClip);
    }

}
