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

    public PlayerCamera PlayerCamera { get => _playerCamera; set => _playerCamera = value; }
    public bool IsPlayerDead => isPlayerDead;

    private PlayerMovement _playerMovement;
    bool isPlayerDead = false;

    private void Awake()
    {
        ApplyHitboxToLimbs();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public void PlayerDeath()
    {
        if (isPlayerDead || _enableGodmode)
            return;
        onPlayerGetHit?.Invoke();
        _playerMovement.ChangeMovementState(new DeathState());
        isPlayerDead = true;
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
