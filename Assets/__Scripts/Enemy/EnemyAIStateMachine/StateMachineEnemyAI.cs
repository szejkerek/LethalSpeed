using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Ragdoll))]
[RequireComponent(typeof(WeaponEnemyAI))]
[RequireComponent(typeof(Enemy))]
public class StateMachineEnemyAI : MonoBehaviour
{
    //States proporties

    [SerializeField] private DebugEnemyAIStates debugEnemyAIStates;

    [field: Header("Common")]
    [field: SerializeField] public float FocusDuration { get; private set; }
    [field: SerializeField] public float UnfocusDuration { get; private set; }

    [field:Header("Idle")]
    [field: SerializeField] public float IdleActivationRange { get; private set; }

    [field: Header("Shoot")]
    //

    Player _player;
    WeaponEnemyAI _weaponEnemyAI;
    NavMeshAgent _navMeshAgent;
    Animator _animator;
    Ragdoll _ragdoll;
    Enemy _enemy;
    SkinnedMeshRenderer _mesh;
    public Player Player => _player;
    public WeaponEnemyAI WeaponEnemyAI => _weaponEnemyAI;
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
        _weaponEnemyAI = GetComponent<WeaponEnemyAI>();
        _enemy = GetComponent<Enemy>();
    }

    private void OnDrawGizmos()
    {
        if(debugEnemyAIStates.IdleShowGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, IdleActivationRange);
        }

        if (debugEnemyAIStates.SeekShowGizmos)
        {

        }

        if (debugEnemyAIStates.ShootShowGizmos)
        {

        }

        if (debugEnemyAIStates.CrouchShowGizmos)
        {

        }

        if (debugEnemyAIStates.WalkBackShowGizmos)
        {

        }

        if (debugEnemyAIStates.ReloadShowGizmos)
        {

        }

        if (debugEnemyAIStates.RagdollShowGizmos)
        {

        }

        if (debugEnemyAIStates.PatrollShowGizmos)
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

[System.Serializable]
public struct DebugEnemyAIStates
{
    public bool IdleShowGizmos;
    public bool ShootShowGizmos;
    public bool SeekShowGizmos;
    public bool CrouchShowGizmos;
    public bool ReloadShowGizmos;
    public bool WalkBackShowGizmos;
    public bool RagdollShowGizmos;
    public bool PatrollShowGizmos;
}