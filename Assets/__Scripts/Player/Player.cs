using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event Action onPlayerDeath;

    public PlayerCamera PlayerCamera;

    private void Awake()
    {
        ApplyHitboxToLimbs();
    }

    public void PlayerDeath()
    {
        onPlayerDeath?.Invoke();
    }

    private void ApplyHitboxToLimbs()
    {
        Collider[] _colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in _colliders)
        {
            PlayerHitbox hitBox = collider.gameObject.AddComponent<PlayerHitbox>();
            hitBox.Player = this;
        }
    }

}
