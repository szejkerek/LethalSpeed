using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollStateEnemyAI : StateEnemyAI
{
    public RagdollStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory, string stateName) : base(context, factory, stateName) { }

    public override void EnterState()
    {
        _context.Ragdoll.SetRagdoll();
        _context.WeaponEnemyAI.DropWeapon();
        _context.Mesh.updateWhenOffscreen = true;
        _context.LocomotionEnemyAI.NavMeshAgent.isStopped = true;
        _context.LocomotionEnemyAI.NavMeshAgent.ResetPath();
    }

    public override void UpdateState()
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
        debugEnemyAIText.stateName = stateName;
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }
}
