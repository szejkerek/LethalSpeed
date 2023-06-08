using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class LockableAnimation
{
    public readonly int AnimationHash;
    public float ClipTime = 0;
    public bool AttackAnimation = false;
    public string _animationName;

    private Animator _animator;

    public LockableAnimation(string animationName, Animator animator, bool AttackAnimation = false)
    {
        _animationName = animationName;
        _animator = animator;
        this.AttackAnimation = AttackAnimation;
        AnimationHash = Animator.StringToHash(animationName);
        UpdateAnimClipTimes();
    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == _animationName)
            {
                ClipTime = clip.length;
                break;
            }
        }
    }
}

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

        _rightLockedTill = 0;
        _rightCurrentState = Idle;
        _playerWeapon.onAttack += () => _attacked = true;
    }

    private void Update()
    {
        var state = GetState();
        _attacked = false;

        if (state == _rightCurrentState) return;
        _rightArmAnimator.CrossFade(state.AnimationHash, 0.2f, 0);
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
