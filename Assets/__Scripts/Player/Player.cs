using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    public static event Action onPlayerGetHit;

    [Header("Developer settings")]
    [SerializeField] private bool _enableGodmode = false;
    [Space(10)]
    [SerializeField] private PlayerCamera _playerCamera;

    bool isDead = false;
    private PlayerMovement _playerMovement;
    private PlayerAudio _playerAudio;

    public PlayerCamera PlayerCamera { get => _playerCamera; set => _playerCamera = value; }
    public bool IsDead => isDead;

    private void Awake()
    {
        ApplyHitboxToLimbs();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAudio = GetComponent<PlayerAudio>();
    }

    public void PlayerDeath()
    {
        if (isDead || _enableGodmode)
            return;
        onPlayerGetHit?.Invoke();
        _playerAudio.PlayPlayerDeathSound();
        _playerMovement.ChangeMovementState(new DeathState());
        isDead = true;
    }

    private void ApplyHitboxToLimbs()
    {
        Collider[] _colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in _colliders)
        {
            PlayerHitbox hitBox = collider.gameObject.AddComponent<PlayerHitbox>();
            hitBox.player = this;
        }
    }

}
