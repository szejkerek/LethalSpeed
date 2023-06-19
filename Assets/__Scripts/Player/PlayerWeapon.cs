using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeapon : MonoBehaviour
{
    public static event Action onHitRegistered;
    public static event Action onAttack;

    [SerializeField] LayerMask AttackLayer;
    [SerializeField] float AttackCooldown = 1.5f;
    [SerializeField] float AttackDelay = 0.3f;
    [SerializeField] float AttackDistance = 2f;
    [SerializeField] float AttackRadius = 1f;
    [SerializeField] private float hitForce = 100f;

    bool _isAttacking = false;
    bool _readyToAttack = true;
    bool _enableInputs = true;
    PlayerAudio playerAudio;

    public bool EnableInputs { get => _enableInputs; set => _enableInputs = value; }
    private void Awake()
    {
        playerAudio = GetComponent<PlayerAudio>();
    }

    private void Update()
    {
        if (!_enableInputs)
            return;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (!_readyToAttack || _isAttacking) return;

        _isAttacking = true;
        _readyToAttack = false;

        playerAudio.PlaySwordWoosh();
        Invoke(nameof(ResetAttack), AttackCooldown);
        Invoke(nameof(PerformAttack), AttackDelay);
        
        onAttack?.Invoke();
    }

    private void ResetAttack() => _readyToAttack = true;
    private void PerformAttack()
    {
        if(Physics.SphereCast(Helpers.Camera.transform.position, AttackRadius, Helpers.Camera.transform.forward, out RaycastHit hit, AttackDistance, AttackLayer))
        {
            if (hit.transform.TryGetComponent(out HitBox hitBox))
            {
                Vector3 direction = hit.point - Helpers.Camera.transform.position;

                const float multiplier = 100f;
                hitBox.TakeHit(direction * hitForce * multiplier, hit.point);
                onHitRegistered?.Invoke();
            }
        }
        _isAttacking = false;
    }
}
