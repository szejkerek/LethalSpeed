using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIIdleState : EnemyAIState
{
    public EnemyAIIdleState(EnemyAIStateMachine context, EnemyAIStateFactory factory) : base (context, factory){}
    public override void CheckSwitchState()
    {
        throw new System.NotImplementedException();
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override DebugEnemyAIText GetDebugText()
    {
        DebugEnemyAIText debugEnemyAIText;
        debugEnemyAIText.titleColor = Color.yellow;
        debugEnemyAIText.stateName = "Idle";
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }

    public override void UpdateStateInternally()
    {
        throw new System.NotImplementedException();
    }
}
