using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadingStateEnemyAI : StateEnemyAI
{
    public ReloadingStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory, string stateName) : base(context, factory, stateName) { }

    public override void EnterState()
    {
        Debug.Log($"{_context.gameObject.name} entered {stateName} state.");
    }
    public override void UpdateState()
    {
        CheckSwitchState();
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
        debugEnemyAIText.titleColor = Color.grey;
        debugEnemyAIText.stateName = "Reloading";
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }
}