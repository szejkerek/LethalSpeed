using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerHandsController : MonoBehaviour
{
    private LockableAnimation Idle;
    private LockableAnimation Death;
    private List<LockableAnimation> Attacks;

    [SerializeField] Animator _rightArmAnimator;
    float _rightLockedTill;
    private LockableAnimation _rightCurrentState;
    bool _attacked = false;
    bool _death = false;

    PlayerWeapon _playerWeapon;

    private void Awake()
    {
        _playerWeapon = GetComponent<PlayerWeapon>();
        Idle = new LockableAnimation("WeaponIdle", _rightArmAnimator);
        Death = new LockableAnimation("WeaponDeath", _rightArmAnimator);
        Attacks = new List<LockableAnimation>() { 
            new LockableAnimation("WeaponAttack1", _rightArmAnimator, AttackAnimation: true),
            new LockableAnimation("WeaponAttack2", _rightArmAnimator, AttackAnimation: true) };

        _rightLockedTill = 0;
        _rightCurrentState = Idle;
        _playerWeapon.onAttack += () => _attacked = true;
        Player.onPlayerGetHit += () => _death = true;
    }

    private void Update()
    {
        var state = GetState();
        _attacked = false;
        _death = false;

        if (state == _rightCurrentState) return;
        _rightArmAnimator.Play(state.AnimationHash, 0);
        _rightCurrentState = state;
    }

    private LockableAnimation GetState()
    {
        if (Time.time < _rightLockedTill) return _rightCurrentState;

        if (_attacked) return LockState(Attacks.PickRandomElement());
        if (_death) return LockState(Death, float.MaxValue);
        return Idle;

        LockableAnimation LockState(LockableAnimation state, float timer = 0.1f)
        {
            _rightLockedTill = Time.time + state.ClipTime + timer;
            return state;
        }
    }

}
