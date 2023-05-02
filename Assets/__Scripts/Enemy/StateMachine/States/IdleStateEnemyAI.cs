using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateEnemyAI : StateEnemyAI
{
    public IdleStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory, string stateName) : base (context, factory, stateName){}

    public override void EnterState()
    {
        Debug.Log($"{_context.gameObject.name} entered {stateName} state.");
        _context.LocomotionEnemyAI.ResetPath();
        _context.Enemy.AimAtTargetRigController.TurnOffRig(_context.UnfocusDuration);
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
        bool tooAwayFromInitialPosition = Vector3.Distance(_context.transform.position, _context.LocomotionEnemyAI.InitialPosition) > _context.PatrolRange + _context.IdleTooAwayDistance;
        if (_context.ShootingActivationCheck())
        {
            SwitchState(_context.StatesFactory.ShootPlayer());
        }
        else if (tooAwayFromInitialPosition)
        {
            SwitchState(_context.StatesFactory.Retrieve());
        }
    }


    public override DebugEnemyAIText GetDebugText()
    {
        DebugEnemyAIText debugEnemyAIText;
        debugEnemyAIText.titleColor = Color.yellow;
        debugEnemyAIText.stateName = "Idle";
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }


}
