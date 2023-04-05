using UnityEngine;

public class RunningState : MovementState
{
    private PlayerMovement _pm;
    private RaycastHit _slopeRayHit;

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.MaxSpeed = pm.MovementSpeed;
        _pm.Rigidbody.drag = _pm.GroundFriction;
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
                Vector3.ProjectOnPlane(normalizedWishDir, _slopeRayHit.normal).normalized * _pm.MaxSpeed * _pm.GroundAcceleration, 
                ForceMode.Force
            );
            _pm.Rigidbody.useGravity = false;

            return;
        }

        _pm.Rigidbody.AddForce(normalizedWishDir * _pm.MaxSpeed * _pm.GroundAcceleration, ForceMode.Force);
        _pm.Rigidbody.useGravity = true;
    }

    public void End()
    {
        
    }

    public void CheckForModeChange()
    {
        if (Input.GetKey(_pm.JumpKey))
        {
            _pm.Velocity = _pm.FlatVelocity;
            _pm.Rigidbody.AddForce(Vector3.up * _pm.JumpForce, ForceMode.Impulse);
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && _pm.CanDash)
        {
            _pm.JustDashed();
            _pm.ChangeMovementState(new DashingState());

            return;
        }
    }

    private void ClipGroundSpeed()
    {
        if (_pm.IsOnSlope(out _slopeRayHit) && _pm.Velocity.magnitude > _pm.MaxSpeed)
        {
            _pm.Velocity = _pm.Velocity.normalized * _pm.MaxSpeed;
            _pm.velocityText.text = $"Slope velocity: {_pm.Velocity.magnitude:0.##}ups - Y vel: {_pm.Velocity.y:0.##}";

            return;
        }

        if(_pm.FlatVelocity.magnitude > _pm.MaxSpeed)
        {
            float drop = _pm.FlatVelocity.magnitude - _pm.MaxSpeed > 3.0f ? _pm.GroundDeacceleration * Time.deltaTime : _pm.FlatVelocity.magnitude - _pm.MaxSpeed;
            Vector3 newSpeed = _pm.FlatVelocity.normalized * (_pm.FlatVelocity.magnitude - drop);

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }

        _pm.velocityText.text = $"Ground velocity: {_pm.FlatVelocity.magnitude:0.##}ups - Y vel: {_pm.Velocity.y:0.##}";
    }
}
