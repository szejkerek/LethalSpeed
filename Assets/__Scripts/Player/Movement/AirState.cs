using DG.Tweening;
using UnityEngine;

[System.Serializable]
public struct AirProperties
{
    public float MaxSpeed;
    public float Acceleration;
    public float GravityForce;
    public float JumpForce;
    public float CoyoteTime;
}

public class AirState : MovementState
{
    private PlayerMovement _pm;
    private float _initialVelocity;
    private float _jumpCommandBuffer;
    private float _coyoteTimer;
    private bool _enteredByJumping;

    public AirState(float startingVelocity, bool didJump = true)
    {
        _initialVelocity = startingVelocity;
        _enteredByJumping = didJump;
    }

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.CurrentMaxSpeed = pm.AirProps.MaxSpeed;
        _pm.Rigidbody.drag = 0.0f;

        _coyoteTimer = _enteredByJumping ? -1.0f : _pm.AirProps.CoyoteTime;
    }

    public void Update()
    {
        _jumpCommandBuffer -= Time.deltaTime;
        _coyoteTimer -= Time.deltaTime;

        if(_coyoteTimer > 0.0f && Input.GetKeyDown(_pm.JumpKey))
        {
            _pm.Velocity = _pm.FlatVelocity;
            _pm.Rigidbody.AddForce(Vector3.up * _pm.AirProps.JumpForce, ForceMode.Impulse);
            _coyoteTimer = -1.0f;
        }

        ClipAirSpeed();

        if(Input.GetKeyDown(_pm.CrouchKey))
        {
            _pm.transform.DOScaleY(_pm.SlideProps.ScaleY, 0.25f);
        }
        else if(Input.GetKeyUp(_pm.CrouchKey))
        {
            _pm.transform.DOScaleY(_pm.OriginalScaleY, 0.25f);
        }

        if(_initialVelocity > _pm.CurrentMaxSpeed)
        {
            float currectVelocity = _pm.FlatVelocity.magnitude;

            _initialVelocity = currectVelocity < _pm.CurrentMaxSpeed ? _pm.CurrentMaxSpeed : currectVelocity;
        }

        if(Input.GetKeyDown(_pm.JumpKey))
        {
            _jumpCommandBuffer = 0.5f;
        }

        CheckForModeChange();
    }

    public void Move(Vector3 normalizedWishDir)
    {
        _pm.Rigidbody.AddForce(normalizedWishDir * _pm.CurrentMaxSpeed * _pm.AirProps.Acceleration, ForceMode.Force);
        _pm.Rigidbody.AddForce(Vector3.down * _pm.AirProps.GravityForce, ForceMode.Force);
    }

    public void End()
    {
        
    }

    public void CheckForModeChange()
    {
        if(_pm.JustLanded && Input.GetKey(_pm.CrouchKey))
        {
            _pm.ChangeMovementState(new SlidingState(_pm.FlatVelocity.magnitude));

            return;
        }

        if (_pm.IsGrounded && !_pm.WasGrounded)
        {
            _pm.ChangeMovementState(new RunningState());

            return;
        }

        RaycastHit wallRayHit;

        if(Physics.Raycast(_pm.transform.position + Vector3.up * _pm.PlayerHeight / 2.0f, _pm.Orientation.right, out wallRayHit, 0.8f, _pm.WallrunProps.WallMask)
            && !Input.GetKey(_pm.CrouchKey) && (Input.GetKeyDown(_pm.JumpKey) || _jumpCommandBuffer >= 0.0f))
        {
            _pm.ChangeMovementState(new WallrunningState(wallRayHit.normal, _pm.Velocity.magnitude));

            return;
        }
        
        if(Physics.Raycast(_pm.transform.position + Vector3.up * _pm.PlayerHeight / 2.0f, -_pm.Orientation.right, out wallRayHit, 0.8f, _pm.WallrunProps.WallMask)
            && !Input.GetKey(_pm.CrouchKey) && (Input.GetKeyDown(_pm.JumpKey) || _jumpCommandBuffer >= 0.0f))
        {
            _pm.ChangeMovementState(new WallrunningState(wallRayHit.normal, _pm.Velocity.magnitude));
            
            return;
        }

        if(Input.GetKeyDown(_pm.DashKey) && _pm.CanDash)
        {
            _pm.JustDashed();
            _pm.ChangeMovementState(new DashingState(_pm.FlatVelocity.magnitude));

            return;
        }

        if (Input.GetKeyDown(_pm.GrappleKey))
        {
            _pm.ChangeMovementState(new GrapplingState());

            return;
        }

        if (Input.GetKeyDown(_pm.SwingKey))
        {
            _pm.ChangeMovementState(new SwingingState(_pm.Velocity.magnitude));

            return;
        }

        if (Input.GetKeyDown(_pm.JumpKey) 
            && Physics.Raycast(_pm.transform.position + Vector3.up * (_pm.PlayerHeight) + _pm.Orientation.forward * (0.5f + _pm.LedgeClimbingProps.MinDistance),
            Vector3.down, out RaycastHit rayHit, _pm.LedgeClimbingProps.RayPierceDistance, _pm.GroundMask))
        {
            _pm.ChangeMovementState(new LedgeClimbingState(rayHit));

            return;
        }
    }

    public string GetStateName()
    {
        return "Jumping";
    }

    private void ClipAirSpeed()
    {
        if (_initialVelocity > _pm.CurrentMaxSpeed)
        {
            Vector3 newSpeed = _pm.FlatVelocity.normalized * Mathf.Min(_initialVelocity, _pm.FlatVelocity.magnitude);

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }
        else if (_pm.FlatVelocity.magnitude > _pm.CurrentMaxSpeed)
        {
            Vector3 newSpeed = _pm.FlatVelocity.normalized * _pm.CurrentMaxSpeed;

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }
    }
}
