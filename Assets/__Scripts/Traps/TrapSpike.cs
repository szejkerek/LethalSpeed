using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpike : MonoBehaviour
{
    [SerializeField] Ease _ease;
    [Header("Timers")]
    [SerializeField] protected float _cooldown = 0;
    [SerializeField] protected float _activeTime = 0;
    [SerializeField] protected float _delay = 0;
    [Header("Trap proporties")]
    [SerializeField] AudioClip _clip;
    [SerializeField] protected float _damageTime = 0;
    [SerializeField] protected float _outDuration = 0;
    [SerializeField] protected float _inDuration = 0;

    private bool isTrapActive = false;
    private bool playerInTrigger = false;
    private AudioSource _audio;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        DeactivateTrap();
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInTrigger = true;
        if (!isTrapActive)
        {
            StartCoroutine(ActivateTrapRoutine(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
       playerInTrigger = false;
    }

    private IEnumerator ActivateTrapRoutine(Collider other)
    {
        yield return new WaitForSeconds(0.1f);
        isTrapActive = true;
        transform.DOMoveY(transform.position.y + 0.25f, _outDuration);
        PlaySpikeSound();

        if (other.TryGetComponent(out HitBox hitBox))
        {
            yield return new WaitForSeconds(_damageTime); 

            if (playerInTrigger)
            {
                hitBox.TakeHit(Vector3.up * 5f, other.ClosestPointOnBounds(transform.position));
            }
        }

        yield return new WaitForSeconds(_activeTime - _damageTime);

        DeactivateTrap();
        yield return new WaitForSeconds(_cooldown);
        isTrapActive = false;
    }

    private void PlaySpikeSound()
    {      
        _audio.spatialBlend = 1.0f;
        _audio.minDistance = 1f;
        _audio.maxDistance = 15f;
        _audio.pitch = Random.Range(0.7f, 1.4f);
        _audio.clip = _clip;
        _audio.Play();
    }

    private void DeactivateTrap()
    {
        transform.DOMoveY(transform.position.y - 0.25f, _inDuration).SetEase(_ease);
    }

}
