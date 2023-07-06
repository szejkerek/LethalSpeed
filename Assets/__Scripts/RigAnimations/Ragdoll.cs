using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] _rigidbodies;
    Animator _animator;
    void Awake()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        if (TryGetComponent(out Animator animator))
        {
            _animator = animator;
        }

        SetRagdoll(active: false);
    }
    public void ApplyForce(Vector3 force)
    {
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            rigidbody.AddForce(force);
        }
    }

    public void SetRagdoll(bool active = true)
    {
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = !active;

            if(active)
            {
                rigidbody.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            }     
        }
        
        if(_animator is not null)
        {
            _animator.enabled = !active;
        }


    }

}
