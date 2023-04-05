using UnityEngine;

public class SlidingState : MovementState
{
    private PlayerMovement _pm;
    private RaycastHit _slopeRayHit;
    private float _slidingTime;

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.MaxSpeed = pm.SlidingSpeed;
        _pm.Rigidbody.drag = _pm.GroundFriction;

        _slidingTime = _pm.MaxSlidingTimeInSeconds;
    }

    public void Update()
    {
        _slidingTime -= Time.deltaTime;

        ClipSlidingSpeed();
        CheckForModeChange();
    }

    public void Move(Vector3 normalizedWishDir)
    {
        if (_pm.IsOnSlope(out _slopeRayHit) && _pm.Rigidbody.velocity.y < -0.1f)
        {
            float slopeUpAngle = Vector3.Angle(Vector3.up, _slopeRayHit.normal);

            _pm.Rigidbody.AddForce(Vector3.ProjectOnPlane(normalizedWishDir, _slopeRayHit.normal).normalized * _pm.MaxSpeed * _pm.SlidingAcceleration * (1.0f + slopeUpAngle / _pm.MaxSlopeAngle), ForceMode.Force);
            _pm.Rigidbody.useGravity = false;

            _slidingTime = _pm.MaxSlidingTimeInSeconds;

            return;
        }

        _pm.Rigidbody.AddForce(normalizedWishDir * _pm.MaxSpeed * _pm.SlidingAcceleration, ForceMode.Force);
        _pm.Rigidbody.useGravity = true;
    }

    public void End()
    {
        if (!Input.GetKey(_pm.CrouchKey))
        {
            _pm.transform.localScale = new Vector3(_pm.transform.localScale.x, _pm.OriginalScaleY, _pm.transform.localScale.z);
        }
    }

    public void CheckForModeChange()
    {
        if (Input.GetKeyDown(_pm.JumpKey))
        {
            _pm.Rigidbody.velocity = new Vector3(_pm.Rigidbody.velocity.x, 0.0f, _pm.Rigidbody.velocity.z);
            _pm.Rigidbody.AddForce(Vector3.up * _pm.JumpForce * _pm.SlideJumpForce, ForceMode.Impulse);
            _pm.ChangeMovementState(new AirState(new Vector3(_pm.Rigidbody.velocity.x, 0.0f, _pm.Rigidbody.velocity.z).magnitude));

            return;
        }

        if (!_pm.IsGrounded && _pm.WasGrounded)
        {
            _pm.ChangeMovementState(new AirState(new Vector3(_pm.Rigidbody.velocity.x, 0.0f, _pm.Rigidbody.velocity.z).magnitude));

            return;
        }
        
        if(_pm.Rigidbody.velocity.magnitude < 1.0f || _slidingTime <= 0.0f)
        {
            _pm.ChangeMovementState(new CrouchingState());

            return;
        }

        if(Input.GetKeyUp(_pm.CrouchKey))
        {
            _pm.ChangeMovementState(new RunningState());

            return;
        }
    }

    private void ClipSlidingSpeed()
    {
        if(_pm.IsOnSlope(out _slopeRayHit) && _pm.Rigidbody.velocity.y < -0.1f)
        {
            _pm.velocityText.text = $"Down sliding velocity: {_pm.Rigidbody.velocity.magnitude:0.##}ups - Y vel: {_pm.Rigidbody.velocity.y:0.##}";

            return;
        }

        Vector3 flatVel = new Vector3(_pm.Rigidbody.velocity.x, 0.0f, _pm.Rigidbody.velocity.z);

        if (flatVel.magnitude > _pm.MaxSpeed)
        {
            Vector3 newSpeed = flatVel.normalized * _pm.MaxSpeed;

            _pm.Rigidbody.velocity = new Vector3(newSpeed.x, _pm.Rigidbody.velocity.y, newSpeed.z);
        }

        _pm.velocityText.text = $"Sliding velocity: {flatVel.magnitude:0.##}ups - Y vel: {_pm.Rigidbody.velocity.y:0.##}";
    }
}
