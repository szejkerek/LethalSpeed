using UnityEngine;

[System.Serializable]
public struct GroundProperties
{
    public float MaxSpeed;
    public float Acceleration;
    public float Deacceleration;
    public float Friction;
}

public class RunningState : MovementState
{
    private PlayerMovement _pm;
    private RaycastHit _slopeRayHit;

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.CurrentMaxSpeed = pm.GroundProps.MaxSpeed;
        _pm.Rigidbody.drag = _pm.GroundProps.Friction;
    }

    public void Update()
    {
        ClipGroundSpeed();
        CheckForModeChange();
    }

    public void Move(Vector3 normalizedWishDir)
    {
        if (_pm.IsOnSlope(out _slopeRayHit))
        {
            _pm.Rigidbody.AddForce(
                Vector3.ProjectOnPlane(normalizedWishDir, _slopeRayHit.normal).normalized * _pm.CurrentMaxSpeed * _pm.GroundProps.Acceleration, 
                ForceMode.Force
            );

            return;
        }

        _pm.Rigidbody.AddForce(normalizedWishDir * _pm.CurrentMaxSpeed * _pm.GroundProps.Acceleration, ForceMode.Force);
    }

    public void End()
    {
        
    }

    public void CheckForModeChange()
    {
        if (Input.GetKey(_pm.JumpKey))
        {
            _pm.Velocity = _pm.FlatVelocity;
            _pm.Rigidbody.AddForce(Vector3.up * _pm.AirProps.JumpForce, ForceMode.Impulse);
            _pm.ChangeMovementState(new AirState(_pm.FlatVelocity.magnitude));

            return;
        }

        if (!_pm.IsGrounded && _pm.WasGrounded)
        {
            _pm.ChangeMovementState(new AirState(_pm.FlatVelocity.magnitude));

            return;
        }

        if(Input.GetKey(_pm.CrouchKey))
        {
            _pm.ChangeMovementState(new CrouchingState());

            return;
        }

        if (Input.GetKeyDown(_pm.DashKey) && _pm.CanDash)
        {
            _pm.JustDashed();
            _pm.ChangeMovementState(new DashingState());

            return;
        }

        if(Input.GetKeyDown(_pm.HookKey))
        {
            _pm.ChangeMovementState(new GrapplingState());

            return;
        }
    }

    public string GetStateName()
    {
        return "Running";
    }

    private void ClipGroundSpeed()
    {
        if (_pm.IsOnSlope(out _slopeRayHit) && _pm.Velocity.magnitude > _pm.CurrentMaxSpeed)
        {
            _pm.Velocity = _pm.Velocity.normalized * _pm.CurrentMaxSpeed;

            return;
        }

        if(_pm.FlatVelocity.magnitude > _pm.CurrentMaxSpeed)
        {
            float drop = _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed > 3.0f ? _pm.GroundProps.Deacceleration * Time.deltaTime : _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed;
            Vector3 newSpeed = _pm.FlatVelocity.normalized * (_pm.FlatVelocity.magnitude - drop);

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }
    }
}
