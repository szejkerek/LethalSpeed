using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class RetrievePositionStateEnemyAI : StateEnemyAI
{
    public RetrievePositionStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory, string stateName) : base(context, factory, stateName) { }
    Vector3 _initialPosition;

    public override void EnterState()
    {
        _context.LocomotionEnemyAI.ResetPath();
        _initialPosition = _context.LocomotionEnemyAI.InitialPosition;
        _context.LocomotionEnemyAI.SetDestinationToRandomPoint(_context.LocomotionEnemyAI.InitialPosition, 0.5f);
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void ExitState()
    {
    }
    public override void CheckSwitchState()
    {
        if(_context.ShootingEnterCheck())
        {
            SwitchState(_context.StatesFactory.ShootPlayer());
        }
        else if (_context.LocomotionEnemyAI.IsAtDestination())
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
}