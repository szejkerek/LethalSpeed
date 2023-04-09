using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAISeekPlayerState : EnemyAIState
{
    public EnemyAISeekPlayerState(EnemyAIStateMachine context, EnemyAIStateFactory factory) : base(context, factory) { }

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
}