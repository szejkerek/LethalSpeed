using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPlayerStateEnemyAI : StateEnemyAI
{
    public ShootingPlayerStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory, string stateName) : base(context, factory, stateName) { }

    public override void EnterState()
    {
        Debug.Log($"{_context.gameObject.name} entered {stateName} state.");
        _context.LocomotionEnemyAI.ResetPath();
        _context.Enemy.AimAtTargetRigController.TurnOnRig(_context.FocusDuration);
    }
    public override void UpdateState()
    {
        _context.WeaponEnemyAI.ShootingAtPlayer();

        CheckSwitchState();
    }

    public override void ExitState()
    {

    }
    public override void CheckSwitchState()
    {
        bool playerTooLongNotSeen = _context.VisionEnemyAI.LastSeenTimer >= _context.AggroDuration;
        //Debug.Log(_context.VisionEnemyAI.LastSeenTimer);
        //Reload

        //Seek
        if (playerTooLongNotSeen)
        {
            SwitchState(_context.StatesFactory.SeekPlayer());
        }
        //WalkBackward
        //Crouch
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