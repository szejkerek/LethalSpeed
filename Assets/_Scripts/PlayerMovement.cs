using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Ground movement")]
    [SerializeField] private float _maxGroundSpeed;
    [SerializeField] private float _groundAcceleration;
    [SerializeField] private float _groundFriction;

    [Header("Air movement")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _airAcceleration;
    [SerializeField] private float _gravityForce;

    [Header("Ground check stuff")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundMask;

    [Header("Key bindings")]
    [SerializeField] private KeyCode _jumpKey;

    [Space]
    public Transform orientation;

    [Space]
    public TextMeshProUGUI velocityText;

    Rigidbody _rb;

    bool _isGrounded;
    bool _canJump;
    float _jumpCooldown;

    Vector3 _wishDir;
    float _horizontalInput;
    float _verticalInput;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _canJump = true;
        _jumpCooldown = 0.4f;
    }

    void Update()
    {
        _isGrounded = Physics.CheckSphere(transform.position, 0.25f, _groundMask);

        GetInput();
        ClipSpeed();
        ApplyFriction();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput   = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(_jumpKey) && _canJump && _isGrounded)
        {
            _canJump = false;
            
            Jump();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    private void ApplyFriction()
    {
        _rb.drag = _isGrounded ? _groundFriction : 0.0f;
    }

    private void ClipSpeed()
    {
        Vector3 flatVelocity = new Vector3(_rb.velocity.x, 0.0f, _rb.velocity.z);

        if (flatVelocity.magnitude > _maxGroundSpeed)
        {
            Vector3 newSpeed = flatVelocity.normalized * _maxGroundSpeed;

            _rb.velocity = new Vector3(newSpeed.x, _rb.velocity.y, newSpeed.z);
        }

        velocityText.text = $"Velocity: {flatVelocity.magnitude:0.##}ups";
    }

    private void MovePlayer()
    {
        _wishDir = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        float accelerationFactor = _isGrounded ? _groundAcceleration : _airAcceleration;

        _rb.AddForce(_wishDir.normalized * _maxGroundSpeed * accelerationFactor, ForceMode.Force);
        
        if(!_isGrounded)
        {
            _rb.AddForce(Vector3.down * _gravityForce, ForceMode.Force);
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0.0f, _rb.velocity.z);
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _canJump = true;
    }
}
