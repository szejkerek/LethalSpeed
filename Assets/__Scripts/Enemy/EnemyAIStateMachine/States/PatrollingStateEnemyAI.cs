using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingStateEnemyAI : StateEnemyAI
{
    public PatrollingStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory, string stateName) : base(context, factory, stateName) { }

    public override void EnterState()
    {
        Debug.Log($"{_context.gameObject.name} entered {stateName} state.");
        nextCooldown = CalculateNextCooldown();
    }

    private float nextCooldown = 0;
    private float timer = 0;

    public override void UpdateStateInternally()
    {
        if(_context.LocomotionEnemyAI.IsAtDestination())
        {
            timer += Time.deltaTime;
            if(timer >= nextCooldown)
            {
                timer = 0;
                nextCooldown = CalculateNextCooldown();
                _context.LocomotionEnemyAI.Patrol(_context.PatrolRange);
            }
        }
    }

    public override void ExitState()
    {
        _context.LocomotionEnemyAI.ResetPath();
    }
    public override void CheckSwitchState()
    {
        _context.ShootingActivationCheck();

    }

    public override DebugEnemyAIText GetDebugText()
    {
        DebugEnemyAIText debugEnemyAIText;
        debugEnemyAIText.titleColor = Color.yellow;
        debugEnemyAIText.stateName = "Patrolling";
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }

    private float CalculateNextCooldown()
    {
        float randomVariation = UnityEngine.Random.Range(-_context.PatrolVariation, _context.PatrolVariation);
        float newPatrolCooldown = _context.PatrolCooldown + randomVariation;

        return Math.Max(newPatrolCooldown, 0);
    }
}