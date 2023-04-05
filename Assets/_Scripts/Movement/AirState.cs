using UnityEngine;

public class AirState : MovementState
{
    private PlayerMovement _pm;
    private float _initialVelocity;

    public AirState(float startingVelocity)
    {
        _initialVelocity = startingVelocity;
    }

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.MaxSpeed = pm.MovementSpeed;
        _pm.Rigidbody.drag = 0.0f;
    }

    public void Update()
    {
        ClipAirSpeed();

        if(Input.GetKeyDown(_pm.CrouchKey))
        {
            _pm.transform.localScale = new Vector3(_pm.transform.localScale.x, _pm.CrouchScaleY, _pm.transform.localScale.z);
        }
        else if(Input.GetKeyUp(_pm.CrouchKey))
        {
            _pm.transform.localScale = new Vector3(_pm.transform.localScale.x, _pm.OriginalScaleY, _pm.transform.localScale.z);
        }

        if(_initialVelocity > _pm.MaxSpeed)
        {
            float currectVelocity = _pm.FlatVelocity.magnitude;

            _initialVelocity = currectVelocity < _pm.MaxSpeed ? _pm.MaxSpeed : currectVelocity;
        }

        CheckForModeChange();
    }

    public void Move(Vector3 normalizedWishDir)
    {
        _pm.Rigidbody.AddForce(normalizedWishDir * _pm.MaxSpeed * _pm.AirAcceleration, ForceMode.Force);
        _pm.Rigidbody.AddForce(Vector3.down * _pm.GravityForce, ForceMode.Force);
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
    }

    private void ClipAirSpeed()
    {
        if (_initialVelocity > _pm.MaxSpeed)
        {
            Vector3 newSpeed = _pm.FlatVelocity.normalized * Mathf.Min(_initialVelocity, _pm.FlatVelocity.magnitude);

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }
        else if (_pm.FlatVelocity.magnitude > _pm.MaxSpeed)
        {
            Vector3 newSpeed = _pm.FlatVelocity.normalized * _pm.MaxSpeed;

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }

        _pm.velocityText.text = $"Air velocity: {_pm.FlatVelocity.magnitude:0.##}ups - Y vel: {_pm.Velocity.y:0.##}";
    }
}
