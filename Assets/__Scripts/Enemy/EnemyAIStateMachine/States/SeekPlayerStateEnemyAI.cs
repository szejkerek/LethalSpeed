using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekPlayerStateEnemyAI : StateEnemyAI
{
    public SeekPlayerStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory) : base(context, factory) { }

    public override void EnterState()
    {
        
    }
    public override void UpdateStateInternally()
    {
         _context.NavMeshAgent.SetDestination(_context.Player.transform.position);
         _context.Animator.SetFloat("Speed", _context.NavMeshAgent.velocity.magnitude);
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
        debugEnemyAIText.titleColor = Color.blue;
        debugEnemyAIText.stateName = "Seek player";
        debugEnemyAIText.info = $"Speed: {_context.NavMeshAgent.velocity.magnitude}";
        return debugEnemyAIText;
    }
}