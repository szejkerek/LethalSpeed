using UnityEngine;

public class CrouchingState : MovementState
{
    private PlayerMovement _pm;

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.MaxSpeed = pm.CrouchSpeed;
        _pm.Rigidbody.drag = _pm.GroundFriction;

        _pm.transform.localScale = new Vector3(_pm.transform.localScale.x, _pm.CrouchScaleY, _pm.transform.localScale.z);
        _pm.transform.position += Vector3.down * _pm.PlayerHeight * _pm.CrouchScaleY * 0.1f;
    }

    public void Update()
    {
        ClipGroundSpeed();
        CheckForModeChange();
    }

    public void Move(Vector3 normalizedWishDir)
    {
        _pm.Rigidbody.AddForce(normalizedWishDir * _pm.MaxSpeed * _pm.GroundAcceleration, ForceMode.Force);
    }

    public void End()
    {
        _pm.transform.localScale = new Vector3(_pm.transform.localScale.x, _pm.OriginalScaleY, _pm.transform.localScale.z);
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

        if(!Input.GetKey(_pm.CrouchKey) && !_pm.IsStuchCrouching)
        {
            _pm.ChangeMovementState(new RunningState());

            return;
        }
    }

    private void ClipGroundSpeed()
    {
        if (_pm.FlatVelocity.magnitude > _pm.MaxSpeed)
        {
            Vector3 newSpeed = _pm.FlatVelocity.normalized * _pm.MaxSpeed;

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }

        _pm.velocityText.text = $"Crouching velocity: {_pm.FlatVelocity.magnitude:0.##}ups - Y vel: {_pm.Velocity.y:0.##}";
    }
}
