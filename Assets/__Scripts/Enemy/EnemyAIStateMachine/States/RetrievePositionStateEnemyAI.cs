using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrievePositionStateEnemyAI : StateEnemyAI
{
    public RetrievePositionStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory) : base(context, factory) { }

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
    }

    public override DebugEnemyAIText GetDebugText()
    {
        DebugEnemyAIText debugEnemyAIText;
        debugEnemyAIText.titleColor = Color.yellow;
        debugEnemyAIText.stateName = "RetrievePosition";
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }
}