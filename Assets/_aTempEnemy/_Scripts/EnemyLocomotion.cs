using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLocomotion : MonoBehaviour
{
    public Transform _player;
    NavMeshAgent _agent;
    Animator animator;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _agent.SetDestination(_player.transform.position);
        animator.SetFloat("Speed", _agent.velocity.magnitude);
    }
}
