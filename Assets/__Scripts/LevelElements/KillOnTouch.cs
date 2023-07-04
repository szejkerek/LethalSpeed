using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnTouch : MonoBehaviour
{
    [SerializeField] bool playSound;
    [SerializeField] List<AudioClip> audioClip;

    GameObject audioSourceGameObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HitBox hitBox))
        {
            Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);
            hitBox.TakeHit(Vector3.zero, contactPoint);

            if(playSound)
            {
                audioSourceGameObject = new GameObject("AudioSourceXD");
                audioSourceGameObject.transform.position = contactPoint;
                audioSourceGameObject.AddComponent<AudioSource>();
                AudioSource audio = audioSourceGameObject.GetComponent<AudioSource>();
                audio.spatialBlend = 1.0f;
                audio.minDistance = 1f;
                audio.maxDistance = 15f;
                audio.pitch = Random.Range(0.7f, 1.4f);
                audio.clip = audioClip.PickRandomElement();
                audio.Play();
            }
        }
    }

}
