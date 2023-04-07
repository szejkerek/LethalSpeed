using DG.Tweening;
using UnityEngine;

[System.Serializable]
public struct SlideProperties
{
    public float MaxSpeed;
    public float Acceleration;
    public float MaxSlidingTime;
    public float SlidingJumpForce;
    public float ScaleY;
}

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
        _pm.CurrentMaxSpeed = pm.SlideProps.MaxSpeed;
        _pm.Rigidbody.drag = 0.0f;

        _slidingTime = _pm.SlideProps.MaxSlidingTime;
    }

    public void Update()
    {
        _slidingTime -= Time.deltaTime;
        ClipSlidingSpeed();

        if (_initialSpeed > _pm.CurrentMaxSpeed)
        {
            float currectVelocity = _pm.FlatVelocity.magnitude;

            _initialSpeed = currectVelocity < _pm.CurrentMaxSpeed ? _pm.CurrentMaxSpeed : currectVelocity;
        }
        else if(!_pm.IsOnSlope(out _slopeRayHit) && _wasOnSlope && _pm.FlatVelocity.magnitude > _pm.CurrentMaxSpeed)
        {
            _initialSpeed = _pm.FlatVelocity.magnitude;
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
                Vector3.ProjectOnPlane(normalizedWishDir, _slopeRayHit.normal).normalized * _pm.SlideProps.Acceleration
                * (1.0f + slopeUpAngle / _pm.MaxSlopeAngle),
                ForceMode.Force
            );
            _pm.Rigidbody.useGravity = false;

            _slidingTime = _pm.SlideProps.MaxSlidingTime;
            
            return;
        }

        _pm.Rigidbody.AddForce(normalizedWishDir * _pm.SlideProps.Acceleration * _pm.CurrentMaxSpeed / 2.0f, ForceMode.Force);
        _pm.Rigidbody.useGravity = true;
    }

    public void End()
    {
        if (!Input.GetKey(_pm.CrouchKey))
        {
            _pm.transform.DOScaleY(_pm.OriginalScaleY, 0.25f);
        }

        _pm.Rigidbody.useGravity = true;
    }

    public void CheckForModeChange()
    {
        if (Input.GetKeyDown(_pm.JumpKey))
        {
            _pm.Velocity = _pm.FlatVelocity;
            _pm.Rigidbody.AddForce(Vector3.up * _pm.SlideProps.SlidingJumpForce, ForceMode.Impulse);
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

    public string GetStateName()
    {
        return "Sliding";
    }

    private void ClipSlidingSpeed()
    {
        if(_pm.IsOnSlope(out _slopeRayHit) && _pm.Velocity.y < -0.1f)
        {
            return;
        }

        if(_initialSpeed > _pm.CurrentMaxSpeed)
        {
            float drop = _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed > 1.0f ? 
                _pm.GroundProps.Deacceleration * Time.deltaTime / 50.0f : _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed;

            Vector3 newSpeed = _pm.FlatVelocity.normalized * Mathf.Min(_initialSpeed, _pm.FlatVelocity.magnitude - drop);

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }
        else if (_pm.FlatVelocity.magnitude > _pm.CurrentMaxSpeed)
        {
            float drop = _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed > 1.0f ?
                _pm.GroundProps.Deacceleration * Time.deltaTime : _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed;

            Vector3 newSpeed = _pm.FlatVelocity.normalized * (_pm.FlatVelocity.magnitude - drop);

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }
    }
}
