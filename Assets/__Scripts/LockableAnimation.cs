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