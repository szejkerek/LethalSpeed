using DG.Tweening;
using UnityEngine;

public class SlidingState : MovementState
{
    private PlayerMovement _pm;
    private RaycastHit _slopeRayHit;
    private bool _wasOnSlope;
    private float _slidingTime;
    private float _initialSpeed;

    public SlidingState(float initialSpeed)
    {
        _initialSpeed = initialSpeed;
    }

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

        if (_initialSpeed > _pm.MaxSpeed)
        {
            float currectVelocity = _pm.FlatVelocity.magnitude;

            _initialSpeed = currectVelocity < _pm.MaxSpeed ? _pm.MaxSpeed : currectVelocity;
            _pm.Rigidbody.drag = 0.0f;
        }
        else if(!_pm.IsOnSlope(out _slopeRayHit) && _wasOnSlope && _pm.FlatVelocity.magnitude > _pm.MaxSpeed)
        {
            _initialSpeed = _pm.FlatVelocity.magnitude;
            _pm.Rigidbody.drag = 0.0f;
        }
        else
        {
            _pm.Rigidbody.drag = _pm.GroundFriction;
        }

        _wasOnSlope = _pm.IsOnSlope(out _slopeRayHit);
        CheckForModeChange();
    }

    public void Move(Vector3 normalizedWishDir)
    {
        if (_pm.IsOnSlope(out _slopeRayHit) && _pm.Velocity.y < -0.1f)
        {
            float slopeUpAngle = Vector3.Angle(Vector3.up, _slopeRayHit.normal);

            _pm.Rigidbody.AddForce(
                Vector3.ProjectOnPlane(normalizedWishDir, _slopeRayHit.normal).normalized * _pm.SlidingAcceleration
                * (1.0f + slopeUpAngle / _pm.MaxSlopeAngle),
                ForceMode.Force
            );
            _pm.Rigidbody.drag = 0.0f;
            _pm.Rigidbody.useGravity = false;

            _slidingTime = _pm.MaxSlidingTimeInSeconds;
            
            return;
        }

        _pm.Rigidbody.AddForce(normalizedWishDir * _pm.MaxSpeed * _pm.SlidingAcceleration, ForceMode.Force);
        _pm.Rigidbody.drag = _pm.GroundFriction;
        _pm.Rigidbody.useGravity = true;
    }

    public void End()
    {
        if (!Input.GetKey(_pm.CrouchKey))
        {
            _pm.transform.DOScaleY(_pm.OriginalScaleY, 0.25f);
        }
    }

    public void CheckForModeChange()
    {
        if (Input.GetKeyDown(_pm.JumpKey))
        {
            _pm.Velocity = _pm.FlatVelocity;
            _pm.Rigidbody.AddForce(Vector3.up * _pm.JumpForce * _pm.SlideJumpForce, ForceMode.Impulse);
            _pm.ChangeMovementState(new AirState(_pm.FlatVelocity.magnitude));

            return;
        }

        if (!_pm.IsGrounded && _pm.WasGrounded)
        {
            _pm.ChangeMovementState(new AirState(_pm.FlatVelocity.magnitude));

            return;
        }
        
        if(_pm.Velocity.magnitude < 1.0f || _slidingTime <= 0.0f)
        {
            _pm.ChangeMovementState(new CrouchingState());

            return;
        }

        if(Input.GetKeyUp(_pm.CrouchKey))
        {
            _pm.ChangeMovementState(_pm.IsStuckCrouching ? new CrouchingState() : new RunningState());

            return;
        }
    }

    private void ClipSlidingSpeed()
    {
        if(_pm.IsOnSlope(out _slopeRayHit) && _pm.Velocity.y < -0.1f)
        {
            _pm.velocityText.text = $"Down sliding velocity: {_pm.Velocity.magnitude:0.##}ups - Y vel: {_pm.Velocity.y:0.##}";

            return;
        }

        if(_initialSpeed > _pm.MaxSpeed)
        {
            float drop = _pm.FlatVelocity.magnitude - _pm.MaxSpeed > 1.0f ? 
                _pm.GroundDeacceleration * Time.deltaTime / 5000.0f : _pm.FlatVelocity.magnitude - _pm.MaxSpeed;

            Vector3 newSpeed = _pm.FlatVelocity.normalized * Mathf.Min(_initialSpeed, _pm.FlatVelocity.magnitude - drop);

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }
        else if (_pm.FlatVelocity.magnitude > _pm.MaxSpeed)
        {
            float drop = _pm.FlatVelocity.magnitude - _pm.MaxSpeed > 1.0f ?
                _pm.GroundDeacceleration * Time.deltaTime : _pm.FlatVelocity.magnitude - _pm.MaxSpeed;

            Vector3 newSpeed = _pm.FlatVelocity.normalized * (_pm.FlatVelocity.magnitude - drop);

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }

        _pm.velocityText.text = $"Sliding velocity: {_pm.FlatVelocity.magnitude:0.##}ups - Y vel: {_pm.Velocity.y:0.##}";
    }
}
