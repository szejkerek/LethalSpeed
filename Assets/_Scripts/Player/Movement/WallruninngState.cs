using UnityEngine;

[System.Serializable]
public struct WallrunProperties
{
    public float MaxSpeed;
    public float Acceleration;
    public float MaxWallrunningTime;
    public float WallrunJumpForce;
    public LayerMask WallMask;
}

public class WallrunningState : MovementState
{
    private PlayerMovement _pm;
    private PlayerCamera _pc;
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
        _pm.CurrentMaxSpeed = pm.WallrunProps.MaxSpeed;
        _pm.Velocity = _pm.FlatVelocity;
        _pm.Rigidbody.drag = _pm.GroundProps.Friction;

        _wallrunningTime = _pm.WallrunProps.MaxWallrunningTime;

        _pc = _pm.pc;
        _pc.SetFOV(70.0f);

        if(Physics.Raycast(_pm.transform.position, _pm.orientation.right, 0.8f, _pm.WallrunProps.WallMask))
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

        if (Physics.Raycast(_pm.transform.position, _pm.orientation.right, out _wallRayHit, 0.8f, _pm.WallrunProps.WallMask))
        {
            _wallNormal = _wallRayHit.normal;
        }
        else if(Physics.Raycast(_pm.transform.position, -_pm.orientation.right, out _wallRayHit, 0.8f, _pm.WallrunProps.WallMask))
        {
            _wallNormal = _wallRayHit.normal;
        }

        if (_initialSpeed > _pm.CurrentMaxSpeed)
        {
            float currectVelocity = _pm.FlatVelocity.magnitude;

            _initialSpeed = currectVelocity < _pm.CurrentMaxSpeed ? _pm.CurrentMaxSpeed : currectVelocity;
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

        _pm.Rigidbody.AddForce(wishedForwardDirMultiplier * forwardDir * _pm.Velocity.magnitude * _pm.WallrunProps.Acceleration, ForceMode.Force);
        _pm.Rigidbody.AddForce(Vector3.down * _pm.AirProps.GravityForce / 3.0f, ForceMode.Force);

        if(Input.GetKey(_pm.JumpKey))
        {
            _pm.Rigidbody.AddForce(Vector3.up * _pm.AirProps.JumpForce * 2.0f, ForceMode.Force);
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
            _pm.Rigidbody.AddForce(Vector3.up * _pm.WallrunProps.WallrunJumpForce, ForceMode.Impulse);
            _pm.ChangeMovementState(new AirState(_pm.FlatVelocity.magnitude));

            return;
        }

        if(!Physics.Raycast(_pm.transform.position, _pm.orientation.right, 0.8f, _pm.WallrunProps.WallMask) &&
           !Physics.Raycast(_pm.transform.position, -_pm.orientation.right, 0.8f, _pm.WallrunProps.WallMask) || _wallrunningTime <= 0.0f)
        {
            _pm.ChangeMovementState(new AirState(_pm.FlatVelocity.magnitude));

            return;
        }
    }

    public string GetStateName()
    {
        return "Wallrunning";
    }

    private void ClipWallrunSpeed()
    {
        if (_initialSpeed > _pm.CurrentMaxSpeed)
        {
            float drop = _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed > 1.0f ? 
                _pm.GroundProps.Deacceleration * Time.deltaTime / 50.0f : _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed;

            Vector3 newSpeed = _pm.FlatVelocity.normalized * Mathf.Min(_initialSpeed, _pm.FlatVelocity.magnitude - drop);

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }
        else if (_pm.FlatVelocity.magnitude > _pm.CurrentMaxSpeed)
        {
            float drop = _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed > 3.0f ? _pm.GroundProps.Deacceleration * Time.deltaTime : _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed;
            Vector3 newSpeed = _pm.FlatVelocity.normalized * (_pm.FlatVelocity.magnitude - drop);

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }
    }
}
