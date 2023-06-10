using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeapon : MonoBehaviour
{

    public event Action onAttack;

    [SerializeField] LayerMask AttackLayer;
    [SerializeField] float AttackCooldown = 1.5f;
    [SerializeField] float AttackDelay = 0.3f;
    [SerializeField] float AttackDistance = 2f;
    [SerializeField] private float hitForce = 100f;

    bool _isAttacking = false;
    bool _readyToAttack = true;

    private void Update()
    {
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

        Invoke(nameof(ResetAttack), AttackCooldown);
        Invoke(nameof(PerformAttack), AttackDelay);
        
        onAttack?.Invoke();
    }

    private void ResetAttack() => _readyToAttack = true;
    private void PerformAttack()
    {
        if(Physics.Raycast(Helpers.Camera.transform.position, Helpers.Camera.transform.forward, out RaycastHit hit, AttackDistance, AttackLayer))
        {
            if (hit.transform.TryGetComponent(out HitBox hitBox))
            {
                Vector3 direction = hit.point - Helpers.Camera.transform.position;
                hitBox.TakeHit(direction * hitForce);
            }
        }
        _isAttacking = false;
    }
}
