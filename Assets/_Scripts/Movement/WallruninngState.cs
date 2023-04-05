using UnityEngine;

public class WallrunningState : MovementState
{
    private PlayerMovement _pm;
    private PlayerCam _pc;
    private RaycastHit _wallRayHit;
    private Vector3 _wallNormal;
    private float _initialSpeed;
    private float _wallrunningTime;

    public WallrunningState(Vector3 wallNormal, float initialVelocity)
    {
        _wallNormal = wallNormal;
        _initialSpeed = initialVelocity;
    }

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.MaxSpeed = pm.WallrunSpeed;
        _pm.Velocity = _pm.FlatVelocity;
        _pm.Rigidbody.drag = _pm.GroundFriction;

        _wallrunningTime = _pm.MaxWallrunTimeInSeconds;

        _pc = _pm.pc;
        _pc.SetFOV(70.0f);

        if(Physics.Raycast(_pm.transform.position, _pm.orientation.right, 0.8f, _pm.WallMask))
        {
            _pc.SetTilt(5.0f);
        }
        else
        {
            _pc.SetTilt(-5.0f);
        }
    }

    public void Update()
    {
        ClipWallrunSpeed();

        if (Physics.Raycast(_pm.transform.position, _pm.orientation.right, out _wallRayHit, 0.8f, _pm.WallMask))
        {
            _wallNormal = _wallRayHit.normal;
        }
        else if(Physics.Raycast(_pm.transform.position, -_pm.orientation.right, out _wallRayHit, 0.8f, _pm.WallMask))
        {
            _wallNormal = _wallRayHit.normal;
        }

        if (_initialSpeed > _pm.MaxSpeed)
        {
            float currectVelocity = _pm.FlatVelocity.magnitude;

            _initialSpeed = currectVelocity < _pm.MaxSpeed ? _pm.MaxSpeed : currectVelocity;
        }

        _wallrunningTime -= Time.deltaTime;

        CheckForModeChange();
    }

    public void Move(Vector3 normalizedWishDir)
    {
        Vector3 forwardDir = Vector3.Cross(_wallNormal, Vector3.up);

        if((_pm.orientation.forward - forwardDir).magnitude > (_pm.orientation.forward + forwardDir).magnitude)
        {
            forwardDir = -forwardDir;
        }

        float wishedForwardDirMultiplier = Vector3.Dot(normalizedWishDir, forwardDir);

        _pm.Rigidbody.AddForce(wishedForwardDirMultiplier * forwardDir * _pm.Velocity.magnitude * _pm.WallrunAcceleration, ForceMode.Force);
        _pm.Rigidbody.AddForce(Vector3.down * _pm.GravityForce / 3.0f, ForceMode.Force);

        if(Input.GetKey(_pm.JumpKey))
        {
            _pm.Rigidbody.AddForce(Vector3.up * _pm.JumpForce * 2.0f, ForceMode.Force);
        }

    }

    public void End()
    {
        _pc.SetFOV(60.0f);
        _pc.SetTilt(0.0f);
    }

    public void CheckForModeChange()
    {
        if(_pm.IsGrounded)
        {
            _pm.ChangeMovementState(new RunningState());

            return;
        }

        if(Input.GetKeyDown(_pm.JumpKey))
        {
            float velocity = _pm.Velocity.magnitude;
            Vector3 flatForward = Vector3.ProjectOnPlane(_pm.orientation.forward, Vector3.up);

            _pm.Velocity = new Vector3(flatForward.x, 0.0f, flatForward.z).normalized * velocity;
            _pm.Rigidbody.AddForce(Vector3.up * _pm.WallrunJumpForce, ForceMode.Impulse);
            _pm.ChangeMovementState(new AirState(_pm.FlatVelocity.magnitude));

            return;
        }

        if(!Physics.Raycast(_pm.transform.position, _pm.orientation.right, 0.8f, _pm.WallMask) &&
           !Physics.Raycast(_pm.transform.position, -_pm.orientation.right, 0.8f, _pm.WallMask) || _wallrunningTime <= 0.0f)
        {
            _pm.ChangeMovementState(new AirState(_pm.FlatVelocity.magnitude));

            return;
        }
    }

    private void ClipWallrunSpeed()
    {
        if (_initialSpeed > _pm.MaxSpeed)
        {
            float drop = _pm.FlatVelocity.magnitude - _pm.MaxSpeed > 1.0f ? 
                _pm.GroundDeacceleration * Time.deltaTime / 500.0f : _pm.FlatVelocity.magnitude - _pm.MaxSpeed;

            Vector3 newSpeed = _pm.FlatVelocity.normalized * Mathf.Min(_initialSpeed, _pm.FlatVelocity.magnitude - drop);

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }
        else if (_pm.FlatVelocity.magnitude > _pm.MaxSpeed)
        {
            float drop = _pm.FlatVelocity.magnitude - _pm.MaxSpeed > 3.0f ? _pm.GroundDeacceleration * Time.deltaTime : _pm.FlatVelocity.magnitude - _pm.MaxSpeed;
            Vector3 newSpeed = _pm.FlatVelocity.normalized * (_pm.FlatVelocity.magnitude - drop);

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }

        _pm.velocityText.text = $"Wallrunning velocity: {_pm.FlatVelocity.magnitude:0.##}ups - Y vel: {_pm.Velocity.y:0.##}";
    }
}
