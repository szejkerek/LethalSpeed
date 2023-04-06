using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAIStateMachine : MonoBehaviour
{
    Player _player;
    NavMeshAgent _navMeshAgent;
    Animator _animator;
    public Player Player => _player;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public Animator Animator => _animator;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    #region State Machine

    EnemyAIState _currentState;
    EnemyAIStateFactory _statesFactory;

    public EnemyAIState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public EnemyAIStateFactory StatesFactory => _statesFactory;
    private void Start()
    {
        _statesFactory = new EnemyAIStateFactory(this);
        _currentState = _statesFactory.SeekPlayer();
        _currentState.EnterState();
    }

    private void Update()
    {
        CurrentState.UpdateState();
    }

    #endregion
}
