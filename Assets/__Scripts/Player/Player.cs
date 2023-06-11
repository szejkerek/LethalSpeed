using System;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    public static event Action onPlayerGetHit;

    [SerializeField] private PlayerCamera _playerCamera;
    public PlayerCamera PlayerCamera { get => _playerCamera; set => _playerCamera = value; }

    private PlayerMovement _playerMovement;
    private PlayerWeapon _playerWeapon;

    private void Awake()
    {
        ApplyHitboxToLimbs();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerWeapon = GetComponent<PlayerWeapon>();
    }

    public void PlayerDeath()
    {
        onPlayerGetHit?.Invoke();       
    }

    public void SetPlayerInteraction(bool enabled)
    {
        //_playerMovement.enabled = enabled;
       //_playerWeapon.enabled = enabled;
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
