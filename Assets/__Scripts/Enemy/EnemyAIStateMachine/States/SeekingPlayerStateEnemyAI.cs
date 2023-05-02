using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingPlayerStateEnemyAI : StateEnemyAI
{
    public SeekingPlayerStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory, string stateName) : base(context, factory, stateName) { }

    float timeSeeking = 0;

    public override void EnterState()
    {
        Debug.Log($"{_context.gameObject.name} entered {stateName} state.");
        _context.Enemy.AimAtTargetRigController.TurnOnRig(1f);
    }
    public override void UpdateStateInternally()
    {

        _context.LocomotionEnemyAI.SetDestination(_context.Player.transform.position);

        timeSeeking += Time.deltaTime;
    }

    public override void ExitState()
    {
        
    }
    public override void CheckSwitchState()
    {
        _context.ShootingActivationCheck();
        if(timeSeeking > _context.BoredAfterSeconds)
        {
            SwitchState(_context.StatesFactory.Idle());
        }
    }

    public override DebugEnemyAIText GetDebugText()
    {
        DebugEnemyAIText debugEnemyAIText;
        debugEnemyAIText.titleColor = Color.blue;
        debugEnemyAIText.stateName = "Seek player";
        debugEnemyAIText.info = $"Speed: {_context.LocomotionEnemyAI.NavMeshAgent.velocity.magnitude}";
        return debugEnemyAIText;
    }
}