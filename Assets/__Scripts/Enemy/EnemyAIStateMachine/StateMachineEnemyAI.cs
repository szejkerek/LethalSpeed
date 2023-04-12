using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Ragdoll))]
public class StateMachineEnemyAI : MonoBehaviour
{
    //States proporties
    public IdleProperties IdleProperties => _idleProperties;
    [SerializeField] IdleProperties _idleProperties;
    public SeekPlayerProperties SeekPlayerProperties => _seekPlayerProperties;
    [SerializeField] SeekPlayerProperties _seekPlayerProperties;
    public ShootingPlayerProperties ShootingPlayerProperties => _shootingPlayerProperties;
    [SerializeField] ShootingPlayerProperties _shootingPlayerProperties;
    public CrouchingProperties CrouchingProperties => _crouchingProperties;
    [SerializeField] CrouchingProperties _crouchingProperties;
    public WalkBackwardsProperties WalkBackwardsProperties => _walkBackwardsProperties;
    [SerializeField] WalkBackwardsProperties _walkBackwardsProperties;
    public ReloadingProperties ReloadingProperties => _reloadingProperties;
    [SerializeField] ReloadingProperties _reloadingProperties;
    public RagdollProperties RagdollProperties => _ragdollProperties;
    [SerializeField] RagdollProperties _ragdollProperties;
    //

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

    private void OnDrawGizmos()
    {
        if(_idleProperties.showGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _idleProperties.ActivationRange);
        }

        if (_seekPlayerProperties.showGizmos)
        {

        }

        if (_shootingPlayerProperties.showGizmos)
        {

        }

        if (_crouchingProperties.showGizmos)
        {

        }

        if (_walkBackwardsProperties.showGizmos)
        {

        }

        if (_reloadingProperties.showGizmos)
        {

        }

        if (_ragdollProperties.showGizmos)
        {

        }
    }

    #region State Machine

    StateEnemyAI _currentState;
    StateFactoryEnemyAI _statesFactory;

    public StateEnemyAI CurrentState { get { return _currentState; } set { _currentState = value; } }
    public StateFactoryEnemyAI StatesFactory => _statesFactory;
    private void Start()
    {
        _statesFactory = new StateFactoryEnemyAI(this);
        _currentState = _statesFactory.Idle();
        _currentState.EnterState();
    }

    private void Update()
    {
        CurrentState.UpdateState();
    }

    #endregion
}
