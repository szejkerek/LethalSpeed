using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateEnemyAI : StateEnemyAI
{
    public IdleStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory, string stateName) : base (context, factory, stateName){}
    bool _canPatrol;
    public override void EnterState()
    {
        Debug.Log($"{_context.gameObject.name} entered {stateName} state.");
        _context.LocomotionEnemyAI.ResetPath();
        _context.Enemy.AimAtTargetRigController.TurnOffRig(_context.UnfocusDuration);
        _canPatrol = true;
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
        if (_context.ShootingEnterCheck())
        {
            SwitchState(_context.StatesFactory.ShootPlayer());
        }
        else if (tooAwayFromInitialPosition)
        {
            SwitchState(_context.StatesFactory.Retrieve());
        }
        else if (_canPatrol)
        {
            _canPatrol = false;
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber <= _context.PartolChance)
            {
                SwitchState(_context.StatesFactory.Patrol());
            }
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
