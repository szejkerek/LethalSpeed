using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    StateMachineEnemyAI _stateMachine;
    private void Awake()
    {
        _stateMachine = GetComponent<StateMachineEnemyAI>();
        ApplyHitboxesToLimbs();
    }

    public void Die()
    {
        _stateMachine.CurrentState.SwitchState(_stateMachine.StatesFactory.Ragdoll());
    }

    private void ApplyHitboxesToLimbs()
    {
        Rigidbody[] _rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            HitboxEnemyAI hitBox = rigidbody.gameObject.AddComponent<HitboxEnemyAI>();
            hitBox.Enemy = this;
        }
    }

    //Test 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }
}
