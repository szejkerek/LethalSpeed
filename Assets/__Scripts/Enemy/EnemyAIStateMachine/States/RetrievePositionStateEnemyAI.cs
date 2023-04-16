using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrievePositionStateEnemyAI : StateEnemyAI
{
    public RetrievePositionStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory) : base(context, factory) { }

    public override void EnterState()
    {
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
        _context.CheckIfEnemyNoticedPlayer();

        bool atDestination = _context.LocomotionEnemyAI.GetPathLength(_context.LocomotionEnemyAI.InitialPosition) <= 1f;
        Debug.Log(_context.LocomotionEnemyAI.GetPathLength(_context.LocomotionEnemyAI.InitialPosition));
        if (atDestination)
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