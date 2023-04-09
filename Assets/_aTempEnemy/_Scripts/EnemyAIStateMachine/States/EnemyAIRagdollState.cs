using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIRagdollState : EnemyAIState
{
    public EnemyAIRagdollState(EnemyAIStateMachine context, EnemyAIStateFactory factory) : base(context, factory) { }

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
        throw new System.NotImplementedException();
    }
}
