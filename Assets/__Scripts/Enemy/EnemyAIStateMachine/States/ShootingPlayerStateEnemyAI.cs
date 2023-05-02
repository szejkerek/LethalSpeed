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

    }

    public override void ExitState()
    {

    }
    public override void CheckSwitchState()
    {
        Debug.Log($"{_context.VisionEnemyAI.LastSeenTimer} last seen, {_context.AggroDuration} duration");
        bool playerTooLongNotSeen = _context.VisionEnemyAI.LastSeenTimer >= _context.AggroDuration;
        bool playerTooAway = _context.GetPlayerDistance() >= _context.AggroDistance;
        //Reload

        //Seek
        if (playerTooLongNotSeen || playerTooAway)
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