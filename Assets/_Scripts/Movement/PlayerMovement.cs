using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Ground movement")]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _groundAcceleration;
    [SerializeField] private float _groundDeacceleration;
    [SerializeField] private float _groundFriction;

    public float MaxSpeed { get { return _maxSpeed; } set { _maxSpeed = value; } }
    public float MovementSpeed { get { return _movementSpeed; } set { _movementSpeed = value; } }
    public float GroundAcceleration { get { return _groundAcceleration; } }
    public float GroundDeacceleration { get { return _groundDeacceleration; } }
    public float GroundFriction { get { return _groundFriction; } }


    [Header("Air movement")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _airAcceleration;
    [SerializeField] private float _gravityForce;
    private Vector3 _wishDir;
    private float   _horizontalInput;
    private float   _verticalInput;
    private bool    _isGrounded;
    private bool    _wasGroundedLastFrame;

    public float JumpForce { get { return _jumpForce; } }
    public float AirAcceleration { get { return _airAcceleration; } }
    public bool IsGrounded { get { return _isGrounded; } }
    public bool WasGrounded { get { return _wasGroundedLastFrame; } }
    public float GravityForce { get { return _gravityForce; } }


    [Header("Crouching")]
    [SerializeField] private float _crouchSpeed;
    [SerializeField] private float _crouchScaleY;
    private float _originalScaleY;
    private bool _isStuckCrouched;

    public float CrouchSpeed { get { return _crouchSpeed; } }
    public float CrouchScaleY { get {  return _crouchScaleY; } }
    public float OriginalScaleY { get { return _originalScaleY; } }
    public bool IsStuchCrouching { get { return _isStuckCrouched; } }


    [Header("Slope movement")]
    [SerializeField] private float _maxSlopeAngle;

    public float MaxSlopeAngle { get { return _maxSlopeAngle; } }


    [Header("Sliding")]
    [SerializeField] private float _slidingSpeed;
    [SerializeField] private float _slidingAcceleration;
    [SerializeField] private float _maxSlidingTimeInSeconds;
    [SerializeField] private float _slideJumpForce;
    private bool _justLanded;

    public float SlidingSpeed { get { return _slidingSpeed; } }
    public float SlidingAcceleration { get {  return _slidingAcceleration; } }
    public float MaxSlidingTimeInSeconds { get { return _maxSlidingTimeInSeconds; } }
    public float SlideJumpForce { get { return _slideJumpForce; } }
    public bool JustLanded { get { return _justLanded; } }


    [Header("Ground check stuff")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundMask;

    public float PlayerHeight { get { return _playerHeight; } }


    [Header("Key bindings")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _crouchKey = KeyCode.LeftControl;

    public KeyCode JumpKey { get { return _jumpKey; } }
    public KeyCode CrouchKey { get { return _crouchKey; } }


    [Space]
    public Transform orientation;

    [Space]
    public TextMeshProUGUI velocityText;

    private Rigidbody _rb;

    public Rigidbody Rigidbody { get { return _rb; } }
    public Vector3 Velocity { get { return _rb.velocity; } set { _rb.velocity = value; } }
    public Vector3 FlatVelocity { get { return Vector3.ProjectOnPlane(_rb.velocity, Vector3.up); } }

    private MovementState _movementState;

    public void ChangeMovementState(MovementState movementState)
    {
        _movementState.End();
        _movementState = movementState;
        _movementState.Begin(this);
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _movementState = Physics.CheckSphere(transform.position, 0.25f, _groundMask) ? new RunningState() : new AirState(0.0f);
        _movementState.Begin(this);

        _originalScaleY = transform.localScale.y;
    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(transform.position, 0.25f, _groundMask);
        _isStuckCrouched = Physics.Raycast(transform.position, Vector3.up, _playerHeight * 0.8f, _groundMask) && transform.localScale.y == _crouchScaleY;
        _justLanded = _isGrounded && !_wasGroundedLastFrame;

        GetInput();

        _movementState.Update();
        _wasGroundedLastFrame = _isGrounded;
    }

    void FixedUpdate()
    {
        _movementState.Move(_wishDir.normalized);
    }

    private void GetInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        
        _wishDir = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
    }

    public bool IsOnSlope(out RaycastHit slopeRayHit)
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeRayHit, 0.2f))
        {
            float slopeAngle = Vector3.Angle(Vector3.up, slopeRayHit.normal);
    
            return slopeAngle < _maxSlopeAngle && slopeAngle != 0.0;
        }
    
        return false;
    }
}
