using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateEnemyAI : StateEnemyAI
{

    public IdleStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory) : base (context, factory){}


    public override void EnterState()
    {
        _context.Enemy.AimAtTargetRigController.TurnOffRig(_context.UnfocusDuration);
    }

    public override void UpdateStateInternally()
    {
    }

    public override void ExitState()
    {
    }
    public override void CheckSwitchState()
    {
        if(Vector3.Distance(_context.transform.position, _context.Player.transform.position) <= _context.IdleActivationRange)
        {
            SwitchState(_context.StatesFactory.ShootPlayer());
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
