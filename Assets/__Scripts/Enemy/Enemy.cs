using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Enemy : MonoBehaviour
{
    public Player Player => _player;
    Player _player;

    RigWeightController _aimAtTargetRigController;
    public Animator Animator => _animator;
    Animator _animator;
    public RigWeightController AimAtTargetRigController => _aimAtTargetRigController;

    StateMachineEnemyAI _stateMachine;

    [SerializeField] GameObject bloodEffect;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _stateMachine = GetComponent<StateMachineEnemyAI>();
        _animator = GetComponent<Animator>();
        _aimAtTargetRigController = GetComponentInChildren<RigWeightController>();
        ApplyHitboxToLimbs();
        SetUpRig();
    }

    public void Die(Vector3 direction, Vector3 hitPoint)
    {
        _stateMachine.CurrentState.SwitchState(_stateMachine.StatesFactory.Ragdoll());
        _stateMachine.Ragdoll.ApplyForce(direction);
        SpawnBloodSplash(direction, hitPoint);
    }

    private void SpawnBloodSplash(Vector3 direction, Vector3 hitPoint)
    {
        Vector3 rotation = Quaternion.LookRotation(-direction, Vector3.up).eulerAngles;
        rotation.x = 0;
        GameObject bloodSplash = Instantiate(bloodEffect, hitPoint, Quaternion.Euler(rotation));
        Destroy(bloodSplash, 3f);
    }

    private void ApplyHitboxToLimbs()
    {
        Rigidbody[] _rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            HitboxEnemyAI hitBox = rigidbody.gameObject.AddComponent<HitboxEnemyAI>();
            hitBox.Enemy = this;
        }
    }

    private void SetUpRig()
    {
        Transform source = FindObjectOfType<Player>().PlayerCamera.EnemyAimTarget;
        if (source != null)
        {
            foreach (MultiAimConstraint component in GetComponentsInChildren<MultiAimConstraint>())
            {
                var data = component.data.sourceObjects;
                data.SetTransform(0, source);
                component.data.sourceObjects = data;
            }
            RigBuilder rigs = GetComponent<RigBuilder>();
            rigs.Build();
        }
        else
        {
            Debug.LogError("Couldn't find player");
        }
    }
}
