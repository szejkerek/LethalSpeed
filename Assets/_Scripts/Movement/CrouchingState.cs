using UnityEngine;
using DG.Tweening;

public class CrouchingState : MovementState
{
    private PlayerMovement _pm;

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.MaxSpeed = pm.CrouchSpeed;
        _pm.Rigidbody.drag = _pm.GroundFriction;

        _pm.transform.DOScaleY(_pm.CrouchScaleY, 0.25f);
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
        _pm.transform.DOScaleY(_pm.OriginalScaleY, 0.25f);
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

        if(!Input.GetKey(_pm.CrouchKey) && !_pm.IsStuckCrouching)
        {
            _pm.ChangeMovementState(new RunningState());

            return;
        }
    }

    private void ClipGroundSpeed()
    {
        if (_pm.FlatVelocity.magnitude > _pm.MaxSpeed)
        {
            float drop = _pm.FlatVelocity.magnitude - _pm.MaxSpeed > 3.0f ? _pm.GroundDeacceleration * Time.deltaTime : _pm.FlatVelocity.magnitude - _pm.MaxSpeed;
            Vector3 newSpeed = _pm.FlatVelocity.normalized * (_pm.FlatVelocity.magnitude - drop);

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }

        _pm.velocityText.text = $"Crouching velocity: {_pm.FlatVelocity.magnitude:0.##}ups - Y vel: {_pm.Velocity.y:0.##}";
    }
}
