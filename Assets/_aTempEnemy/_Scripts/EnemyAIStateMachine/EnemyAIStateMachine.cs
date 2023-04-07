using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Ragdoll))]
public class EnemyAIStateMachine : MonoBehaviour
{
    Player _player;
    NavMeshAgent _navMeshAgent;
    Animator _animator;
    Ragdoll _ragdoll;
    Enemy _enemy;
    SkinnedMeshRenderer _mesh;
    public Player Player => _player;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public Animator Animator => _animator;
    public Ragdoll Ragdoll => _ragdoll;
    public Enemy Enemy => _enemy;
    public SkinnedMeshRenderer Mesh => _mesh;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _mesh = GetComponentInChildren<SkinnedMeshRenderer>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _ragdoll = GetComponent<Ragdoll>();
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
