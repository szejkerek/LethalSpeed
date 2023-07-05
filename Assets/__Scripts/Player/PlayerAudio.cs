using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] PlayerAudioLib playerAudio;
    public void PlaySwordWoosh()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.pitch = Random.Range(0.85f, 1.5f);
        audioSource.volume = Random.Range(0.8f, 1.2f);
        AudioClip clip = playerAudio.SwordWoosh.PickRandomElement();
        audioSource.PlayOneShot(clip);
        Destroy(audioSource, clip.length + 0.5f);
    }

    public void PlayPlayerDeathSound()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.volume = Random.Range(0.6f, 0.9f);
        AudioClip clip = playerAudio.PlayerDeath.PickRandomElement();
        audioSource.PlayOneShot(clip);
        Destroy(audioSource, clip.length + 0.5f);
    }
}
