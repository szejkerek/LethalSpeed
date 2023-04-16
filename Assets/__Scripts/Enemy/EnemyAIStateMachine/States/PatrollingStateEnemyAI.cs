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
    public override void UpdateStateInternally()
    {
        
    }

    public override void ExitState()
    {
        
    }
    public override void CheckSwitchState()
    {
        _context.CheckIfEnemyNoticedPlayer();

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