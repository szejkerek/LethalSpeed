using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SeekPlayerProperties
{
    public bool showGizmos;
}
public class SeekingPlayerStateEnemyAI : StateEnemyAI
{
    public SeekingPlayerStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory) : base(context, factory) { }

    public override void EnterState()
    {
        _context.Enemy.AimAtTargetRigController.TurnOnRig(1f);
    }
    public override void UpdateStateInternally()
    {
        _context.LocomotionEnemyAI.SetDestination(_context.Player.transform.position);
    }

    public override void ExitState()
    {
        
    }
    public override void CheckSwitchState()
    {
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