using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(LocomotionEnemyAI))]
[RequireComponent(typeof(Ragdoll))]
[RequireComponent(typeof(WeaponEnemyAI))]
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(VisionEnemyAI))]
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
    public LocomotionEnemyAI LocomotionEnemyAI => _locomotionEnemyAI;
    LocomotionEnemyAI _locomotionEnemyAI;
    public Animator Animator => _animator;
    Animator _animator;
    public Ragdoll Ragdoll => _ragdoll;
    Ragdoll _ragdoll;
    public Enemy Enemy => _enemy;
    Enemy _enemy;
    public SkinnedMeshRenderer Mesh => _mesh;
    SkinnedMeshRenderer _mesh;
    public VisionEnemyAI VisionEnemyAI => _visionEnemyAI;
    VisionEnemyAI _visionEnemyAI;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _mesh = GetComponentInChildren<SkinnedMeshRenderer>();

        _animator = GetComponent<Animator>();
        _ragdoll = GetComponent<Ragdoll>();
        _weaponEnemyAI = GetComponent<WeaponEnemyAI>();
        _enemy = GetComponent<Enemy>();
        _locomotionEnemyAI = GetComponent<LocomotionEnemyAI>();
        _visionEnemyAI = GetComponent<VisionEnemyAI>();
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

    public Vector3 PlayerPos()
    {
        return Player.transform.position;
    }

    public void CheckIfEnemyNoticedPlayer()
    {
        bool playerInActivationRange = Vector3.Distance(transform.position, Player.transform.position) <= IdleActivationRange;
        bool playerInSight = VisionEnemyAI.TargerInVision;
        if (playerInActivationRange && playerInSight)
        {
            _currentState.SwitchState(StatesFactory.ShootPlayer());
        }
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