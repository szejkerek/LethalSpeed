using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGates : MonoBehaviour
{
    [SerializeField] float OpenDoorTime = 2f;
    [SerializeField] AudioClip _clip;
    [Header("Game objects")]
    [SerializeField] GameObject ClosedLeftDoor;
    [SerializeField] GameObject ClosedRightDoor;
    [Space]
    [SerializeField] GameObject OpenRightDoor;
    [SerializeField] GameObject OpenLeftDoor;
    [Space]
    [SerializeField] GameObject OpenSign;
    [SerializeField] GameObject ClosedSign;

    AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        OpenRightDoor.SetActive(false);
        OpenLeftDoor.SetActive(false);

        EnemyManager.OnAllEnemiesKilled += HandleAllEnemiesKilled;

        if (EnemyManager.Instance.NoEnemiesLeft)
            OpenGate(withSound: false);
    }

    private void OnDestroy()
    {
        EnemyManager.OnAllEnemiesKilled -= HandleAllEnemiesKilled;
    }
    private void HandleAllEnemiesKilled()
    {
        OpenGate();
    }

    void OpenGate(bool withSound = true)
    {
        if (withSound)
            PlaySpikeSound();

        ChangeSign(open: true);
        ClosedLeftDoor.transform.DOMove(OpenLeftDoor.transform.position, OpenDoorTime);
        ClosedRightDoor.transform.DOMove(OpenRightDoor.transform.position, OpenDoorTime);
    }

    void ChangeSign(bool open)
    {
        OpenSign.SetActive(open);
        ClosedSign.SetActive(!open);
    }

    private void PlaySpikeSound()
    {
        _audio.spatialBlend = 1.0f;
        _audio.minDistance = 3f;
        _audio.maxDistance = 100f;
        _audio.pitch = Random.Range(0.7f, 1.4f);
        _audio.clip = _clip;
        _audio.Play();
    }
}