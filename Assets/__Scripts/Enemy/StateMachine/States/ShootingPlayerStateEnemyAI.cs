
using UnityEngine;

public class ShootingPlayerStateEnemyAI : StateEnemyAI
{
    public ShootingPlayerStateEnemyAI(StateMachineEnemyAI context, StateFactoryEnemyAI factory, string stateName) : base(context, factory, stateName) { }
    bool _canBeScared;
    bool _canEngage;
    public override void EnterState()
    {
        _context.LocomotionEnemyAI.ResetPath();
        _context.Enemy.AimAtTargetRigController.TurnOnRig(_context.FocusDuration);
        _canBeScared = true;
        _canEngage = true;
    }
    public override void UpdateState()
    {
        _context.WeaponEnemyAI.ShootingAtPlayer();
        _context.RotateTowardsPlayer();

        CheckSwitchState();
    }


    public override void ExitState()
    {

    }
    public override void CheckSwitchState()
    {
        bool playerTooLongNotSeen = _context.VisionEnemyAI.LastSeenTimer >= _context.AggroDuration;
        bool playerInDangerZone = _context.GetDistanceToPlayer() <= _context.DangerZoneRange;

        if(_context.WeaponEnemyAI.CurrentAmmo <= 0)
        {
            SwitchState(_context.StatesFactory.Reload());
        }
        else if (playerTooLongNotSeen)
        {
            if (_context.WeaponEnemyAI.CurrentAmmo != _context.WeaponEnemyAI.MagazineSize)
            {
                SwitchState(_context.StatesFactory.Reload());
            }
            else
            {
                SwitchState(_context.StatesFactory.SeekPlayer());
            }
        }
        else if(_context.EmotionStateEnterCheck())
        {
            if (_canBeScared && playerInDangerZone)
            {
                float randomNumber = Random.Range(0f, 1f);
                _canBeScared = false;
                if (randomNumber <= _context.FleeChance)
                {
                    SwitchState(_context.StatesFactory.Flee());
                }
            }
            else if (_canEngage)
            {
                _canEngage = false;
                float randomNumber = Random.Range(0f, 1f);
                if (randomNumber <= _context.EngageChance)
                {
                    SwitchState(_context.StatesFactory.Engage());
                }
            }
        }
    }

    public override DebugEnemyAIText GetDebugText()
    {
        DebugEnemyAIText debugEnemyAIText;
        debugEnemyAIText.titleColor = Color.red;
        debugEnemyAIText.stateName = stateName;
        debugEnemyAIText.info = "";
        return debugEnemyAIText;
    }
}