using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WalkBackwardsProperties
{
    public bool showGizmos;
}

public class WalkingBackwardsStateEnemyAI : StateEnemyAI
{
    public WalkingBackwardsStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory) : base(context, factory) { }

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
        debugEnemyAIText.titleColor = Color.cyan;
        debugEnemyAIText.stateName = "WalkBackwards";
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }
}