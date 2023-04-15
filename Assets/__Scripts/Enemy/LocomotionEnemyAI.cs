using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class LocomotionEnemyAI : MonoBehaviour
{
    public Vector3 InitialPosition => _initialPosition;
    Vector3 _initialPosition;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    NavMeshAgent _navMeshAgent;

    Animator _animator;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _initialPosition = transform.position;
    }
    private void Update()
    {
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
    }

    public void SetDestination(Vector3 target)
    {
        _navMeshAgent.SetDestination(target);
    }

    public bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
