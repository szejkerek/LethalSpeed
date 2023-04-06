using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] _rigidbodies;
    Animator _animator;
    void Awake()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        if (TryGetComponent<Animator>(out Animator animator))
        {
            _animator = animator;
        }

        SetRagdoll(active: false);
    }
    public void SetRagdoll(bool active = true)
    {
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = !active;
        }
        
        if(_animator is not null)
        {
            _animator.enabled = !active;
        }
    }

}
