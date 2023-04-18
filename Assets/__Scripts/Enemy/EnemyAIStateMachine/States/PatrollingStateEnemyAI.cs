using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingStateEnemyAI : StateEnemyAI
{
    public PatrollingStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory) : base(context, factory) { }

    public override void EnterState()
    {
        
    }

    float PatrolRange = 10f;
    float PatrolCooldown = 5f;
    float PatrolVariation = 4f;

    public override void UpdateStateInternally()
    {
        _context.LocomotionEnemyAI.Patrol(PatrolRange);
    }

    public override void ExitState()
    {
        
    }
    public override void CheckSwitchState()
    {
        _context.ShootingActivationCheck();

    }

    public override DebugEnemyAIText GetDebugText()
    {
        DebugEnemyAIText debugEnemyAIText;
        debugEnemyAIText.titleColor = Color.yellow;
        debugEnemyAIText.stateName = "Patrolling";
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }
}