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
    [field: SerializeField] public float RotationSpeed { get; private set; }
    [field: SerializeField] public float EmotionStatesCooldown { get; private set; }

    [field:Header("Idle")] //IDLE

    [field: Tooltip("Distance that indicates from where enemy will try to come back to his inital spawn point")]
    [field: SerializeField] public float IdleTooAwayDistance { get; private set; }

    [field: Header("Patroling")] // PATROL
    [field: SerializeField] public float PartolChance { get; private set; }
    [field: SerializeField] public float PatrolDuration { get; private set; }
    [field: SerializeField] public float PatrolRange { get; private set; }
    [field: SerializeField] public float PatrolCooldown { get; private set; }
    [field: SerializeField] public float PatrolVariation { get; private set; }

    [field: Header("Seeking")] // SEEKING
    [field: SerializeField] public float BoredAfterSeconds { get; private set; }

    [field: Header("Shooting")] //SHOOTING
    [field: SerializeField] public float ShootingActivationRange { get; private set; }
    [field: SerializeField] public float AggroDuration { get; private set; }

    [field: Header("Flee")] //Flee
    [field: SerializeField] public float DangerZoneRange { get; private set; }
    [field: SerializeField] public float FleeChance { get; private set; }
    [field: SerializeField] public float FleeMaxDuration { get; private set; }

    [field: Header("Engage")] //Flee
    [field: SerializeField] public float EngageStoppinDistance { get; private set; }
    [field: SerializeField] public float EngageChance { get; private set; }
    [field: SerializeField] public float EngageMaxDuration { get; private set; }



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

    public void OnValidate()
    {
        FleeChance = Mathf.Clamp(FleeChance, 0f, 1f);
        EngageChance = Mathf.Clamp(EngageChance, 0f, 1f);
        PartolChance = Mathf.Clamp(PartolChance, 0f, 1f);       

        if (PatrolCooldown <= PatrolVariation)
        {
            PatrolVariation = PatrolCooldown;
        }
        else if(PatrolVariation <= 0)
        {
            PatrolVariation = 0;
        }
    }

    #region State Machine

    StateEnemyAI _currentState;
    StateFactoryEnemyAI _statesFactory;
    float _lastEmotionStateExit;

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

    public bool ShootingEnterCheck()
    {
        bool playerInActivationRange = GetDistanceToPlayer() < ShootingActivationRange;
        bool playerInSight = VisionEnemyAI.TargerInVision;
        if (playerInActivationRange && playerInSight && !_player.IsDead)
        {
            return true;
        }
        return false;
    }

    public bool EmotionStateEnterCheck()
    {
        return Time.time - _lastEmotionStateExit >= EmotionStatesCooldown;
    }

    public void ResetEmotionsTimer()
    {
        _lastEmotionStateExit = Time.time;
    }

    public float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, Player.transform.position);
    }

    public void RotateTowardsPlayer()
    {
        Vector3 directionToPlayer = Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        float threshold = 30f;
        if (angle > threshold)
        {

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(directionToPlayer), RotationSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        if (debugEnemyAIStates.IdleShowGizmos)
        {

        }

        if (debugEnemyAIStates.SeekShowGizmos)
        {

        }

        if (debugEnemyAIStates.ShootShowGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, ShootingActivationRange);
        }

        if (debugEnemyAIStates.CrouchShowGizmos)
        {

        }

        if (debugEnemyAIStates.FleeShowGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, DangerZoneRange);           
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
        
        if (debugEnemyAIStates.EngageShowGizmos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, EngageStoppinDistance);      
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
    public bool EngageShowGizmos;
    public bool FleeShowGizmos;
    public bool RagdollShowGizmos;
    public bool PatrollShowGizmos;
}