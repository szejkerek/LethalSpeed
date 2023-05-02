using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Enemy : MonoBehaviour
{
    public Player Player => _player;
    Player _player;

    RigWeightController _aimAtTargetRigController;
    public RigWeightController AimAtTargetRigController => _aimAtTargetRigController;

    StateMachineEnemyAI _stateMachine;
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _stateMachine = GetComponent<StateMachineEnemyAI>();
        _aimAtTargetRigController = GetComponentInChildren<RigWeightController>();
        ApplyHitboxToLimbs();
        SetUpRig();
    }

    public void Die()
    {
        _stateMachine.CurrentState.SwitchState(_stateMachine.StatesFactory.Ragdoll());
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
