using UnityEngine;

public class AirState : MovementState
{
    private PlayerMovement _pm;
    private float _startingVelocity;

    public AirState(float startingVelocity)
    {
        _startingVelocity = startingVelocity;
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

        if(_startingVelocity > _pm.MaxSpeed)
        {
            float currectVelocity = new Vector3(_pm.Rigidbody.velocity.x, 0.0f, _pm.Rigidbody.velocity.z).magnitude;

            _startingVelocity = currectVelocity < _pm.MaxSpeed ? _pm.MaxSpeed : currectVelocity;
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
            _pm.ChangeMovementState(new SlidingState());

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
        Vector3 flatVel = new Vector3(_pm.Rigidbody.velocity.x, 0.0f, _pm.Rigidbody.velocity.z);
        
        if (_startingVelocity > _pm.MaxSpeed)
        {
            Vector3 newSpeed = flatVel.normalized * Mathf.Min(_startingVelocity, flatVel.magnitude);

            _pm.Rigidbody.velocity = new Vector3(newSpeed.x, _pm.Rigidbody.velocity.y, newSpeed.z);
        }
        else if (flatVel.magnitude > _pm.MaxSpeed)
        {
            Vector3 newSpeed = flatVel.normalized * _pm.MaxSpeed;

            _pm.Rigidbody.velocity = new Vector3(newSpeed.x, _pm.Rigidbody.velocity.y, newSpeed.z);
        }

        _pm.velocityText.text = $"Air velocity: {flatVel.magnitude:0.##}ups - Y vel: {_pm.Rigidbody.velocity.y:0.##}";
    }
}
