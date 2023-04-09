using UnityEngine;
using DG.Tweening;

[System.Serializable]
public struct CrouchProperties
{
    public float MaxSpeed;
    public float ScaleY;
}

public class CrouchingState : MovementState
{
    private PlayerMovement _pm;

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.CurrentMaxSpeed = pm.CrouchProps.MaxSpeed;
        _pm.Rigidbody.drag = _pm.GroundProps.Friction;

        _pm.transform.DOScaleY(_pm.CrouchProps.ScaleY, 0.25f);
        _pm.transform.position += Vector3.down * _pm.PlayerHeight * _pm.CrouchProps.ScaleY * 0.1f;
    }

    public void Update()
    {
        ClipGroundSpeed();
        CheckForModeChange();
    }

    public void Move(Vector3 normalizedWishDir)
    {
        _pm.Rigidbody.AddForce(normalizedWishDir * _pm.CurrentMaxSpeed * _pm.GroundProps.Acceleration, ForceMode.Force);
    }

    public void End()
    {
        _pm.transform.DOScaleY(_pm.OriginalScaleY, 0.25f);
    }

    public void CheckForModeChange()
    {
        if (Input.GetKey(_pm.JumpKey) && !_pm.IsStuckCrouching)
        {
            _pm.Velocity = _pm.FlatVelocity;
            _pm.Rigidbody.AddForce(Vector3.up * _pm.AirProps.JumpForce, ForceMode.Impulse);
            _pm.ChangeMovementState(new AirState(_pm.FlatVelocity.magnitude));

            return;
        }

        if (!_pm.IsGrounded && _pm.WasGrounded)
        {
            _pm.ChangeMovementState(new AirState(_pm.FlatVelocity.magnitude, false));

            return;
        }

        if(!Input.GetKey(_pm.CrouchKey) && !_pm.IsStuckCrouching)
        {
            _pm.ChangeMovementState(new RunningState());

            return;
        }
    }

    public string GetStateName()
    {
        return "Crouching";
    }

    private void ClipGroundSpeed()
    {
        if (_pm.FlatVelocity.magnitude > _pm.CurrentMaxSpeed)
        {
            float drop = _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed > 3.0f ? _pm.GroundProps.Deacceleration * Time.deltaTime : _pm.FlatVelocity.magnitude - _pm.CurrentMaxSpeed;
            Vector3 newSpeed = _pm.FlatVelocity.normalized * (_pm.FlatVelocity.magnitude - drop);

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }
    }
}
