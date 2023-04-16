using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadingStateEnemyAI : StateEnemyAI
{
    public ReloadingStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory) : base(context, factory) { }

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
        debugEnemyAIText.titleColor = Color.grey;
        debugEnemyAIText.stateName = "Reloading";
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }
}