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
        Debug.Log($"{_context.gameObject.name} entered {stateName} state.");
        _context.LocomotionEnemyAI.ResetPath();
        _initialPosition = _context.LocomotionEnemyAI.InitialPosition;
        _context.LocomotionEnemyAI.SetDestinationToRandomPoint(_context.LocomotionEnemyAI.InitialPosition, 0.5f);
    }
    public override void UpdateStateInternally()
    {

    }

    public override void ExitState()
    {
        _context.LocomotionEnemyAI.ResetPath();
    }
    public override void CheckSwitchState()
    {
        _context.ShootingActivationCheck();

        if (_context.LocomotionEnemyAI.IsAtDestination())
        {
            _context.CurrentState.SwitchState(_context.StatesFactory.Idle());
        }


    }

    public override DebugEnemyAIText GetDebugText()
    {
        DebugEnemyAIText debugEnemyAIText;
        debugEnemyAIText.titleColor = Color.yellow;
        debugEnemyAIText.stateName = "RetrievePosition";
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }
}