using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float _maxSlopeAngle;
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundMask;

    public float MaxSlopeAngle => _maxSlopeAngle;
    public float PlayerHeight => _playerHeight;
    public LayerMask GroundMask => _groundMask;


    [Space]
    [SerializeField] private GroundProperties _groundProps;
    public GroundProperties GroundProps => _groundProps;


    [Space]
    [SerializeField] private AirProperties _airProps;
    public AirProperties AirProps => _airProps;


    [Space]
    [SerializeField] private CrouchProperties _crouchProps;
    public CrouchProperties CrouchProps => _crouchProps;


    [Space]
    [SerializeField] private SlideProperties _slideProps;
    public SlideProperties SlideProps => _slideProps;


    [Space]
    [SerializeField] private WallrunProperties _wallrunProps;
    public WallrunProperties WallrunProps => _wallrunProps;


    [Space]
    [SerializeField] private DashProperties _dashProps;
    public DashProperties DashProps => _dashProps;


    [Space]
    [SerializeField] private GrappleProperties _grappleProps;
    public GrappleProperties GrappleProps => _grappleProps;


    [Space]
    [SerializeField] private SwingProperties _swingProps;
    public SwingProperties SwingProps => _swingProps;


    [Space]
    [SerializeField] private LedgeClimbingProperties _ledgeClimbProps;
    public LedgeClimbingProperties LedgeClimbingProps => _ledgeClimbProps;


    [Header("Key bindings")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode _dashKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode _grappleKey = KeyCode.Q;
    [SerializeField] private KeyCode _swingKey = KeyCode.Mouse1;

    public KeyCode JumpKey => _jumpKey;
    public KeyCode CrouchKey => _crouchKey;
    public KeyCode DashKey => _dashKey;
    public KeyCode GrappleKey => _grappleKey;
    public KeyCode SwingKey => _swingKey;

    [HideInInspector]
    public PlayerCamera _playerCamera;

    [Space]
    public Transform orientation;

    private MovementState _movementState;
    public MovementState CurrentMovementState => _movementState;

    private float _horizontalInput;
    private float _verticalInput;
    private bool _isGrounded;
    private bool _wasGroundedLastFrame;
    private Vector3 _wishDir;
    private float _currentMaxSpeed;
    private float _originalScaleY;
    private bool _isStuckCrouched;
    private bool _justLanded;
    private float _dashCooldown;
    private bool _canDash;
    private Rigidbody _rb;

    public bool IsGrounded => _isGrounded;
    public bool WasGrounded => _wasGroundedLastFrame;
    public float CurrentMaxSpeed { get { return _currentMaxSpeed; } set { _currentMaxSpeed = value; } }
    public float OriginalScaleY => _originalScaleY;
    public bool IsStuckCrouching => _isStuckCrouched;
    public bool JustLanded => _justLanded;
    public float DashCooldown => _dashCooldown;
    public bool CanDash { get { return _canDash; } set { _canDash = value; } }
    public Rigidbody Rigidbody => _rb;
    public Vector3 Velocity { get { return _rb.velocity; } set { _rb.velocity = value; } }
    public Vector3 FlatVelocity => Vector3.ProjectOnPlane(_rb.velocity, Vector3.up);

    

    public void ChangeMovementState(MovementState movementState)
    {
        _movementState.End();
        _movementState = movementState;
        _movementState.Begin(this);
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerCamera = FindObjectOfType<Player>().PlayerCamera;
        _grappleProps.HookLineRenderer = _playerCamera.GetComponentInChildren<LineRenderer>();
        _grappleProps.HookGunTip = _grappleProps.HookLineRenderer.transform.GetChild(0).transform;
    }

    void Start()
    {
        _rb.freezeRotation = true;

        _movementState = Physics.CheckSphere(transform.position, 0.25f, _groundMask) ? new RunningState() : new AirState(0.0f, true);
        _movementState.Begin(this);

        _originalScaleY = transform.localScale.y;

        _canDash = true;
        _dashCooldown = 1.0f;

        _grappleProps.HookLineRenderer.enabled = false;
    }

    void Update()
    {

        _isGrounded = Physics.CheckSphere(transform.position, 0.25f, _groundMask);
        _isStuckCrouched = Physics.Raycast(transform.position, Vector3.up, _playerHeight * 0.8f, _groundMask) 
            && (transform.localScale.y == CrouchProps.ScaleY || transform.localScale.y == SlideProps.ScaleY);
        _justLanded = _isGrounded && !_wasGroundedLastFrame;

        GetInput();
        _movementState.Update();
        _wasGroundedLastFrame = _isGrounded;

        if(_justLanded && !_canDash)
        {
            Invoke(nameof(ResetDash), _dashCooldown);
        }
    }

    void FixedUpdate()
    {
        _movementState.Move(_wishDir.normalized);
    }

    public bool IsOnSlope(out RaycastHit slopeRayHit)
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeRayHit, 0.2f))
        {
            float slopeAngle = Vector3.Angle(Vector3.up, slopeRayHit.normal);

            return slopeAngle < _maxSlopeAngle && slopeAngle != 0.0;
        }

        return false;
    }

    public void JustDashed()
    {
        _canDash = false;

        if(_isGrounded)
        {
            Invoke(nameof(ResetDash), _dashCooldown);
        }
    }

    public void ResetDash()
    {
        _canDash = true;
    }

    private void GetInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        
        _wishDir = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
    }
}
