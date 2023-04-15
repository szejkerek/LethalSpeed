using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
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

    [field:Header("Idle")] //IDLE
    [field: SerializeField] public float IdleActivationRange { get; private set; }

    [field: Tooltip("This chance is checked every Xseconds")]
    [field: SerializeField] public float IdleChance { get; private set; }

    [field: Tooltip("Distance that indicates from where enemy will try to come back to his inital spawn point")]
    [field: SerializeField] public float IdleTooAwayDistance { get; private set; }

    [field: Header("Patrolling")] // PATROL
    [field: SerializeField] public float PatrolInitialChance { get; private set; }

    [field: Tooltip("This chance is checked every Xseconds")]
    [field: SerializeField] public float PatrolChance { get; private set; }
    [field: SerializeField] public float PatrolRange { get; private set; }


    //

    public Player Player => _player;
    Player _player;
    public WeaponEnemyAI WeaponEnemyAI => _weaponEnemyAI;
    WeaponEnemyAI _weaponEnemyAI;
    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    NavMeshAgent _navMeshAgent;
    public Animator Animator => _animator;
    Animator _animator;
    public Ragdoll Ragdoll => _ragdoll;
    Ragdoll _ragdoll;
    public Enemy Enemy => _enemy;
    Enemy _enemy;
    public SkinnedMeshRenderer Mesh => _mesh;
    SkinnedMeshRenderer _mesh;
    public Vector3 InitPosition => _initPosition;
    Vector3 _initPosition;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _mesh = GetComponentInChildren<SkinnedMeshRenderer>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _ragdoll = GetComponent<Ragdoll>();
        _weaponEnemyAI = GetComponent<WeaponEnemyAI>();
        _enemy = GetComponent<Enemy>();

        _initPosition = transform.position;
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
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
    }

    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        if (debugEnemyAIStates.IdleShowGizmos)
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