using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RetrievePositionStateEnemyAI : StateEnemyAI
{
    public RetrievePositionStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory) : base(context, factory) { }
    Vector3 _initialPosition;

    public override void EnterState()
    {
        _initialPosition = _context.LocomotionEnemyAI.InitialPosition;
        _context.LocomotionEnemyAI.SetDestinationToRandomPoint(_context.LocomotionEnemyAI.InitialPosition, 0.5f);
    }
    public override void UpdateStateInternally()
    {

    }

    public override void ExitState()
    {
        
    }
    public override void CheckSwitchState()
    {
        _context.ShootingActivationCheck();

        if (_context.LocomotionEnemyAI.IsAtDestination(_initialPosition))
        {
            _context.LocomotionEnemyAI.ResetPath();
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