using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPlayerStateEnemyAI : StateEnemyAI
{
    public ShootingPlayerStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory) : base(context, factory) { }

    public override void EnterState()
    {
        _context.LocomotionEnemyAI.ResetPath();
        _context.Enemy.AimAtTargetRigController.TurnOnRig(_context.FocusDuration);
    }
    public override void UpdateStateInternally()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            _context.WeaponEnemyAI.ShootAtTarget();
        }
    }

    public override void ExitState()
    {
        
    }
    public override void CheckSwitchState()
    {
    }

    public override DebugEnemyAIText GetDebugText()
    {
        DebugEnemyAIText debugEnemyAIText;
        debugEnemyAIText.titleColor = Color.red;
        debugEnemyAIText.stateName = "ShootingPlayer";
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }
}