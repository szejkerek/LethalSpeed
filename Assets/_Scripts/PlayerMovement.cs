using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Ground movement")]
    [SerializeField] private float _maxGroundSpeed;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _groundAcceleration;
    [SerializeField] private float _groundFriction;

    [Header("Air movement")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _airAcceleration;
    [SerializeField] private float _gravityForce;
    private Vector3 _wishDir;
    private float   _horizontalInput;
    private float   _verticalInput;
    private bool    _isGrounded;
    private bool    _wasGroundedLastFrame;
    private bool    _canJump;
    private float   _jumpCooldown;

    [Header("Crouching")]
    [SerializeField] private float _crouchSpeed;
    [SerializeField] private float _crouchScaleY;
    private float _originalScaleY;
    private bool _isStuckCrouched;

    [Header("Slope movement")]
    [SerializeField] private float _maxSlopeAngle;
    private RaycastHit _slopeRayHit;

    [Header("Sliding")]
    [SerializeField] private float _slidingSpeed;
    [SerializeField] private float _slidingAcceleration;
    [SerializeField] private float _slidingTriggerVelocityY;
    [SerializeField] private float _maxSlidingTimeInSeconds;
    private float _slidingTime;
    private bool _justLanded;

    [Header("Ground check stuff")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundMask;

    [Header("Key bindings")]
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _crouchKey = KeyCode.LeftControl;

    [Space]
    public Transform orientation;

    [Space]
    public TextMeshProUGUI velocityText;

    Rigidbody _rb;

    public enum MoveMode
    {
        Normal,
        Crouching,
        Sliding
    };

    private MoveMode _moveMode;

    private string _prefix;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _canJump = true;
        _jumpCooldown = 0.3f;

        _moveMode = MoveMode.Normal;
        _prefix = "Normal";

        _originalScaleY = transform.localScale.y;
    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(transform.position, 0.25f, _groundMask);
        _isStuckCrouched = Physics.CheckSphere(transform.position + Vector3.up * _playerHeight, 0.25f, _groundMask) && transform.localScale.y == _crouchScaleY;
        _justLanded = _isGrounded && _wasGroundedLastFrame;

        UpdateMoveMode();
        GetInput();

        switch(_moveMode )
        {
            case MoveMode.Normal:
            case MoveMode.Crouching:
                UpdateGroundAirMove();
                break;

            case MoveMode.Sliding:
                UpdateSlideMove();
                break;
        }

        _wasGroundedLastFrame = _isGrounded;
    }

    void FixedUpdate()
    {
        switch (_moveMode)
        {
            case MoveMode.Normal:
            case MoveMode.Crouching:
                GroundAirMove();
                break;

            case MoveMode.Sliding:
                SlideMove();
                break;
        }
    }

    private void UpdateMoveMode()
    {
        if ((Input.GetKey(_crouchKey) || _isStuckCrouched) && _moveMode != MoveMode.Crouching && _moveMode != MoveMode.Sliding)
        {
            _moveMode = MoveMode.Crouching;
            StartCrouching();

            _prefix = "Crouching";

            return;
        }

        if(_moveMode == MoveMode.Crouching && !Input.GetKey(_crouchKey) && !_isStuckCrouched)
        {
            _moveMode = MoveMode.Normal;
            EndCrouching();

            _prefix = "Normal";

            return;
        }

        if(_moveMode != MoveMode.Sliding && Input.GetKey(_crouchKey) && _justLanded && 
            Vector3.ProjectOnPlane(_rb.velocity, Vector3.up).magnitude != 0.0f && _rb.velocity.y < _slidingTriggerVelocityY)
        {
            _moveMode = MoveMode.Sliding;
            StartSliding();

            _prefix = "Sliding";

            return;
        }

        if(_moveMode == MoveMode.Sliding && (!Input.GetKey(_crouchKey) || (_rb.velocity.magnitude < 2.0f) ||
            _slidingTime >= _maxSlidingTimeInSeconds) && !_isStuckCrouched)
        {
            _moveMode = MoveMode.Normal;
            _slidingTime = 0.0f;
            EndSliding();

            _prefix = "Normal";

            return;
        }
    }

    private void GetInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        
        _wishDir = orientation.forward * _verticalInput + orientation.right * _horizontalInput;
    }
    private void ClipSpeed()
    {
        // Slope movement corection
        if (IsOnSlope() && _rb.velocity.magnitude > _maxGroundSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * _maxGroundSpeed;
            velocityText.text = $"{_prefix} velocity: {_rb.velocity.magnitude:0.##}ups - Y vel: {_rb.velocity.y}";

            return;
        }

        // Ground / air movement correction
        Vector3 flatVelocity = new Vector3(_rb.velocity.x, 0.0f, _rb.velocity.z);

        if (flatVelocity.magnitude > _maxGroundSpeed)
        {
            Vector3 newSpeed = flatVelocity.normalized * _maxGroundSpeed;

            _rb.velocity = new Vector3(newSpeed.x, _rb.velocity.y, newSpeed.z);
        }

        velocityText.text = $"{_prefix} velocity: {_rb.velocity.magnitude:0.##}ups - Y vel: {_rb.velocity.y}";
    }


    #region Ground_air

    private void UpdateGroundAirMove()
    {
        ClipSpeed();

        _rb.drag = _isGrounded ? _groundFriction : 0.0f;
    }

    private void GroundAirMove()
    {
        // Check for jump move
        if (_isGrounded && _canJump && Input.GetKey(_jumpKey))
        {
            _canJump = false;

            _rb.velocity = new Vector3(_rb.velocity.x, 0.0f, _rb.velocity.z);
            _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump), _jumpCooldown);
        }
        
        if (IsOnSlope())
        {
            _rb.AddForce(Vector3.ProjectOnPlane(_wishDir, _slopeRayHit.normal).normalized * _maxGroundSpeed * _groundAcceleration, ForceMode.Force);
            _rb.useGravity = false;
            
            return;
        }

        _rb.useGravity = true;

        float acceleration = _isGrounded ? _groundAcceleration : _airAcceleration;

        _rb.AddForce(_wishDir.normalized * _maxGroundSpeed * acceleration, ForceMode.Force);

        if(!_isGrounded)
        {
            _rb.AddForce(Vector3.down * _gravityForce, ForceMode.Force);
        }
    }

    private bool IsOnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out _slopeRayHit, 0.2f))
        {
            float slopeAngle = Vector3.Angle(Vector3.up, _slopeRayHit.normal);
    
            return slopeAngle < _maxSlopeAngle && slopeAngle != 0.0;
        }
    
        return false;
    }

    #endregion


    #region Crouching

    private void StartCrouching()
    {
        transform.localScale = new Vector3(transform.localScale.x, _crouchScaleY, transform.localScale.z);
        transform.position += Vector3.down * _playerHeight * _crouchScaleY * 0.1f;
        _maxGroundSpeed = _crouchSpeed;
    }

    private void EndCrouching()
    {
        transform.localScale = new Vector3(transform.localScale.x, _originalScaleY, transform.localScale.z);
        _maxGroundSpeed = _movementSpeed;
    }

    #endregion


    #region Sliding

    private void UpdateSlideMove()
    {
        ClipSpeed();

        _slidingTime += Time.deltaTime;
        _rb.drag = _isGrounded ? _groundFriction : 0.0f;
    }

    private void SlideMove()
    {
        if (IsOnSlope())
        {
            _rb.AddForce(Vector3.ProjectOnPlane(_wishDir, _slopeRayHit.normal).normalized * _maxGroundSpeed * _slidingAcceleration, ForceMode.Force);
            _rb.useGravity = false;

            return;
        }
        
        _rb.AddForce(_wishDir.normalized * _maxGroundSpeed * _slidingAcceleration, ForceMode.Force);
        _rb.useGravity = true;
    }

    private void StartSliding()
    {
        _maxGroundSpeed = _slidingSpeed;
    }

    private void EndSliding()
    {
        EndCrouching();
    }

    #endregion


    private void ResetJump()
    {
        _canJump = true;
    }

    //void Update()
    //{
    //    _isGrounded = Physics.CheckSphere(transform.position, 0.25f, _groundMask);
    //    _isStuckCrouched = transform.localScale.y == _crouchScaleY && Physics.CheckSphere(transform.position + Vector3.up * _playerHeight, 0.25f, _groundMask);
    //    _justLanded = _isGrounded && _rb.velocity.y < 0.0f && !_isSliding;

    //    UpdateMoveMode();
    //    GetInput();
    //    ClipSpeed();
    //    ApplyFriction();
    //}

    //private void FixedUpdate()
    //{
    //    if(_isSliding)
    //    {
    //        SlideMove();

    //        return;
    //    }

    //    MovePlayer();
    //}

    //private void UpdateMoveMode()
    //{
    //    if (Input.GetKey(KeyCode.C) && _justLanded)
    //    {
    //        _moveMode = MoveMode.Sliding;
    //        StartSliding();

    //        return;
    //    }

    //    if (Input.GetKeyUp(KeyCode.C) && _isSliding)
    //    {
    //        _moveMode = MoveMode.Running;
    //        StopSliding();

    //        return;
    //    }

    //    if (Input.GetKey(_crouchKey) || _isStuckCrouched)
    //    {
    //        _moveMode = MoveMode.Crouching;
    //        _maxGroundSpeed = _isGrounded ? _crouchSpeed : _movementSpeed;

    //        return;
    //    }

    //    if(_isGrounded)
    //    {
    //        _moveMode = MoveMode.Running;
    //        _maxGroundSpeed = _movementSpeed;

    //        return;
    //    }
    //}

    //private void GetInput()
    //{
    //    _horizontalInput = Input.GetAxisRaw("Horizontal");
    //    _verticalInput   = Input.GetAxisRaw("Vertical");

    //    if(Input.GetKey(_jumpKey) && _canJump && _isGrounded)
    //    {
    //        _canJump = false;

    //        Jump();
    //        Invoke(nameof(ResetJump), _jumpCooldown);
    //    }

    //    if(Input.GetKeyDown(_crouchKey) && !_isStuckCrouched)
    //    {
    //        transform.localScale = new Vector3(transform.localScale.x, _crouchScaleY, transform.localScale.z);
    //        transform.position -= Vector3.down * _playerHeight * _crouchScaleY * 0.5f;
    //    }

    //    if((Input.GetKeyUp(_crouchKey) || _moveMode != MoveMode.Crouching && _moveMode != MoveMode.Sliding) && !_isStuckCrouched)
    //    {
    //        transform.localScale = new Vector3(transform.localScale.x, _originalScaleY, transform.localScale.z);
    //    }
    //}

    //private void ApplyFriction()
    //{
    //    if (_isSliding) return;
    //    _rb.drag = _isGrounded ? _groundFriction : 0.0f;
    //}

    //private void ClipSpeed()
    //{
    //    if(IsOnSlope() && _rb.velocity.magnitude > _maxGroundSpeed)
    //    {
    //        _rb.velocity = _rb.velocity.normalized * _maxGroundSpeed;
    //        velocityText.text = $"Velocity: {_rb.velocity.magnitude:0.##}ups";

    //        return;
    //    }

    //    Vector3 flatVelocity = new Vector3(_rb.velocity.x, 0.0f, _rb.velocity.z);

    //    if (flatVelocity.magnitude > (_isSliding ? _maxSlidingSpeed : _maxGroundSpeed))
    //    {
    //        Vector3 newSpeed = flatVelocity.normalized * (_isSliding ? _maxSlidingSpeed : _maxGroundSpeed);

    //        _rb.velocity = new Vector3(newSpeed.x, _rb.velocity.y, newSpeed.z);
    //    }

    //    velocityText.text = $"Velocity: {flatVelocity.magnitude:0.##}ups";
    //}

    //private void MovePlayer()
    //{
    //    _wishDir = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

    //    if (IsOnSlope())
    //    {
    //        _rb.AddForce(Vector3.ProjectOnPlane(_wishDir, _slopeRayHit.normal).normalized * _maxGroundSpeed * _groundAcceleration, ForceMode.Force);
    //        _rb.useGravity = false;

    //        return;
    //    }

    //    if(_isSliding)
    //    {
    //        _rb.AddForce(_wishDir.normalized * _maxSlidingSpeed * _slidingAcceleration, ForceMode.Force);
    //    }
    //    else
    //    {
    //        float accelerationFactor = _isGrounded ? _groundAcceleration : _airAcceleration;

    //        _rb.AddForce(_wishDir.normalized * _maxGroundSpeed * accelerationFactor, ForceMode.Force);
    //        _rb.useGravity = true;
    //    }

    //    if(!_isGrounded)
    //    {
    //        _rb.AddForce(Vector3.down * _gravityForce, ForceMode.Force);
    //    }
    //}

    //private void Jump()
    //{
    //    _rb.velocity = new Vector3(_rb.velocity.x, 0.0f, _rb.velocity.z);
    //    _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    //}

    //private void ResetJump()
    //{
    //    _canJump = true;
    //}

    //private bool IsOnSlope()
    //{
    //    if(Physics.Raycast(transform.position, Vector3.down, out _slopeRayHit, 0.2f))
    //    {
    //        float slopeAngle = Vector3.Angle(Vector3.up, _slopeRayHit.normal);

    //        return slopeAngle < _maxSlopeAngle && slopeAngle != 0.0;
    //    }

    //    return false;
    //}

    //private void StartSliding()
    //{
    //    _isSliding = true;
    //    transform.localScale = new Vector3(transform.localScale.x, _crouchScaleY, transform.localScale.z);
    //    _rb.AddForce(Vector3.down * 20.0f, ForceMode.Force);
    //}

    //private void SlideMove()
    //{
    //    _rb.AddForce(_wishDir.normalized * _slidingAcceleration, ForceMode.Force);
    //}

    //private void StopSliding()
    //{
    //    _isSliding = false;
    //    transform.localScale = new Vector3(transform.localScale.x, _originalScaleY, transform.localScale.z);
    //}
}
