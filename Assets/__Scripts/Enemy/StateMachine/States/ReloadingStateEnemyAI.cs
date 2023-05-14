using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadingStateEnemyAI : StateEnemyAI
{
    public ReloadingStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory, string stateName) : base(context, factory, stateName) { }

    public override void EnterState()
    {
        _context.LocomotionEnemyAI.ResetPath();
        _context.WeaponEnemyAI.TriggerReload();
    }
    public override void UpdateState()
    {
        _context.RotateTowardsPlayer();
        CheckSwitchState();
    }

    public override void ExitState()
    {
        
    }
    public override void CheckSwitchState()
    {
        if(!_context.WeaponEnemyAI.IsReloading)
        {
            SwitchState(_context.StatesFactory.ShootPlayer());
        }
    }

    public override DebugEnemyAIText GetDebugText()
    {
        DebugEnemyAIText debugEnemyAIText;
        debugEnemyAIText.titleColor = Color.grey;
        debugEnemyAIText.stateName = stateName;
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }
}