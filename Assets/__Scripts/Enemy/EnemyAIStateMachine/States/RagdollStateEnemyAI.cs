using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RagdollProperties
{
}
public class RagdollStateEnemyAI : StateEnemyAI
{
    public RagdollStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory) : base(context, factory) { }

    public override void EnterState()
    {
        _context.Ragdoll.SetRagdoll();
        _context.Mesh.updateWhenOffscreen = true;
        _context.NavMeshAgent.isStopped = true;
        _context.NavMeshAgent.ResetPath();
    }

    public override void UpdateStateInternally()
    {
        return;
    }

    public override void ExitState()
    {
        return;
    }

    public override void CheckSwitchState()
    {
        return;
    }

    public override DebugEnemyAIText GetDebugText()
    {
        DebugEnemyAIText debugEnemyAIText;
        debugEnemyAIText.titleColor = Color.white;
        debugEnemyAIText.stateName = "Ragdoll";
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }
}
