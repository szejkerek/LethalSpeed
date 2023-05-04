using DG.Tweening.Plugins.Core.PathCore;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class LocomotionEnemyAI : MonoBehaviour
{
    public Vector3 InitialPosition => _initialPosition;
    Vector3 _initialPosition;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    NavMeshAgent _navMeshAgent;

    [SerializeField] float atDestinationDistance = 0.45f;

    NavMeshPath _path;

    Animator _animator;

    public float VelocityModifier = 1;

    void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _path = new NavMeshPath();
        _initialPosition = transform.position;
    }
    private void Update()
    {
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude * VelocityModifier);
    }

    public void SetDestination(Vector3 target)
    {
        GetPath(target);
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(target);
    }

    public void SetDestinationToRandomPoint(Vector3 target, float range)
    {
        Vector3 point;
        if (RandomPoint(InitialPosition, range, out point))
        {
            SetDestination(point);
        }
    }

    public bool IsAtDestination()
    {
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= atDestinationDistance)
        {
            if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }
        return false;
    }

    public void Patrol(float range)
    {
        SetDestinationToRandomPoint(transform.position, range);
    }

    public void ResetPath()
    {
        _navMeshAgent.ResetPath();
        _navMeshAgent.velocity = Vector3.zero;
        _navMeshAgent.isStopped = true;
    }

    public bool GetPath(Vector3 target)
    {
        _path.ClearCorners();

        if (_navMeshAgent.CalculatePath(target, _path) == false)
            return false;

        return true;
    }

    public float GetPathLength(Vector3 target)
    {
        float lng = 0.0f;
        if (!GetPath(target))
            return lng;

        if ((_path.status != NavMeshPathStatus.PathInvalid) && (_path.corners.Length > 1))
        {
            for (int i = 1; i < _path.corners.Length; ++i)
            {
                lng += Vector3.Distance(_path.corners[i - 1], _path.corners[i]);
            }
        }

        return lng;
    }

    public NavMeshPathStatus GetPathStatus()
    {
        return _path.status;
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
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
