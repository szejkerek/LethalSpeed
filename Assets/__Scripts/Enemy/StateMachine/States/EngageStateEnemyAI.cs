using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngageStateEnemyAI : StateEnemyAI
{
    public EngageStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory, string stateName) : base(context, factory, stateName) { }
    float _engageDuration;
    public override void EnterState()
    {
        _engageDuration = 0;
    }

    public override void UpdateState()
    {
        _engageDuration += Time.deltaTime;
        EngageOnPlayer();
        _context.WeaponEnemyAI.ShootingAtPlayer();
        CheckSwitchState();
    }

    private void EngageOnPlayer()
    {
        _context.LocomotionEnemyAI.SetDestination(_context.PlayerPos());
    }

    public override void ExitState()
    {
        _context.ResetEmotionsTimer();
    }
    public override void CheckSwitchState()
    {
        bool playerCloseEnough = _context.GetDistanceToPlayer() <= _context.EngageStoppinDistance;

        if (_context.WeaponEnemyAI.CurrentAmmo <= 0)
        {
            SwitchState(_context.StatesFactory.Reload());
        }
        else if (_engageDuration >= _context.EngageMaxDuration)
        {
            SwitchState(_context.StatesFactory.ShootPlayer());
        }
        else if (playerCloseEnough)
        {
            SwitchState(_context.StatesFactory.ShootPlayer());
        }
    }

    public override DebugEnemyAIText GetDebugText()
    {
        DebugEnemyAIText debugEnemyAIText;
        debugEnemyAIText.titleColor = Color.green;
        debugEnemyAIText.stateName = stateName;
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }
}