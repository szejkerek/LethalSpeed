using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingStateEnemyAI : StateEnemyAI
{
    public PatrollingStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory, string stateName) : base(context, factory, stateName) { }

    private float _nextCooldown = 0;
    private float _waitAtDestinationTimer = 0;
    private float _patrolingTimer = 0;

    public override void EnterState()
    {
        _nextCooldown = CalculateNextCooldown();
        _waitAtDestinationTimer = 0;
        _patrolingTimer = 0;
    }
    
    public override void UpdateState()
    {
        _patrolingTimer += Time.deltaTime;
        if (_context.LocomotionEnemyAI.IsAtDestination())
        {
            _waitAtDestinationTimer += Time.deltaTime;
            if(_waitAtDestinationTimer >= _nextCooldown)
            {
                _waitAtDestinationTimer = 0;
                _nextCooldown = CalculateNextCooldown();
                _context.LocomotionEnemyAI.Patrol(_context.PatrolRange);
            }
        }
        CheckSwitchState();
    }

    public override void ExitState()
    {

    }
    public override void CheckSwitchState()
    {
        if (_context.ShootingEnterCheck())
        {
            SwitchState(_context.StatesFactory.ShootPlayer());
        }
        else if(_patrolingTimer >= _context.PatrolDuration)
        {
            SwitchState(_context.StatesFactory.Idle());
        }
    }

    public override DebugEnemyAIText GetDebugText()
    {
        DebugEnemyAIText debugEnemyAIText;
        debugEnemyAIText.titleColor = Color.yellow;
        debugEnemyAIText.stateName = stateName;
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