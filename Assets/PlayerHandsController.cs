using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandsController : MonoBehaviour
{
    private LockableAnimation Idle;
    private LockableAnimation Attack1;

    [SerializeField] Animator _rightArmAnimator;
    float _rightLockedTill;
    private LockableAnimation _rightCurrentState;
    bool _attacked = false;

    PlayerWeapon _playerWeapon;

    private void Awake()
    {
        _playerWeapon = GetComponent<PlayerWeapon>();
        Idle = new LockableAnimation("WeaponIdle", _rightArmAnimator);
        Attack1 = new LockableAnimation("WeaponAttack1", _rightArmAnimator, AttackAnimation: true);
        Debug.Log(Attack1.ClipTime);
        _rightLockedTill = 0;
        _rightCurrentState = Idle;
        _playerWeapon.onAttack += () => _attacked = true;
    }

    private void Update()
    {
        var state = GetState();
        _attacked = false;

        if (state == _rightCurrentState) return;
        _rightArmAnimator.CrossFade(state.AnimationHash, 0.1f, 0);
        _rightCurrentState = state;
    }

    private LockableAnimation GetState()
    {
        if (Time.time < _rightLockedTill) return _rightCurrentState;

        if (_attacked) return LockState(Attack1);
        return Idle;

        LockableAnimation LockState(LockableAnimation state)
        {
            _rightLockedTill = Time.time + state.ClipTime + 0.05f;
            return state;
        }
    }

}
