using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeStateEnemyAI : StateEnemyAI
{
    public FleeStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory, string stateName) : base(context, factory, stateName) { }
    float _oldNavMeshSpeed;
    float _fleeNavMeshSpeed = 1f;
    float _speed = 0.3f;
    float _fleeDuration;
    public override void EnterState()
    {
        _fleeDuration = 0;
        _context.LocomotionEnemyAI.VelocityModifier = -1;
        _context.LocomotionEnemyAI.NavMeshAgent.updateRotation = false;
        _oldNavMeshSpeed = _context.LocomotionEnemyAI.NavMeshAgent.speed;
        _context.LocomotionEnemyAI.NavMeshAgent.speed = _fleeNavMeshSpeed;

    }

    public override void UpdateState()
    {
        _fleeDuration += Time.deltaTime;
        FleeFromPlayer();
        _context.WeaponEnemyAI.ShootingAtPlayer();
        _context.RotateTowardsPlayer();
        CheckSwitchState();
    }

    private void FleeFromPlayer()
    {
        Vector3 reversePlayerDirection = _context.Player.transform.position - _context.transform.position;
        reversePlayerDirection.y = 0;
        reversePlayerDirection = reversePlayerDirection.normalized * -_speed;
        _context.LocomotionEnemyAI.SetDestination(_context.transform.position + reversePlayerDirection);
    }

    public override void ExitState()
    {
        _context.LocomotionEnemyAI.NavMeshAgent.updateRotation = true;
        _context.LocomotionEnemyAI.VelocityModifier = 1;
        _context.LocomotionEnemyAI.NavMeshAgent.speed = _oldNavMeshSpeed;
        _context.ResetEmotionsTimer();
    }
    public override void CheckSwitchState()
    {
        if (_context.WeaponEnemyAI.CurrentAmmo <= 0)
        {
            SwitchState(_context.StatesFactory.Reload());
        }
        else if (_fleeDuration >= _context.FleeMaxDuration)
        {
            SwitchState(_context.StatesFactory.ShootPlayer());
        }
    }

    public override DebugEnemyAIText GetDebugText()
    {
        DebugEnemyAIText debugEnemyAIText;
        debugEnemyAIText.titleColor = Color.cyan;
        debugEnemyAIText.stateName = stateName;
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }
}