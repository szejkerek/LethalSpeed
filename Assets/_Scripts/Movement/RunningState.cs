using UnityEngine;

public class RunningState : MovementState
{
    private PlayerMovement _pm;

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
        _pm.Rigidbody.AddForce(normalizedWishDir * _pm.MaxSpeed * _pm.GroundAcceleration, ForceMode.Force);
    }

    public void End()
    {
        
    }

    public void CheckForModeChange()
    {
        if (Input.GetKey(_pm.JumpKey))
        {
            _pm.Rigidbody.velocity = new Vector3(_pm.Rigidbody.velocity.x, 0.0f, _pm.Rigidbody.velocity.z);
            _pm.Rigidbody.AddForce(Vector3.up * _pm.JumpForce, ForceMode.Impulse);
            _pm.ChangeMovementState(new AirState());

            return;
        }

        if (!_pm.IsGrounded && _pm.WasGrounded)
        {
            _pm.ChangeMovementState(new AirState());

            return;
        }

        if(Input.GetKey(_pm.CrouchKey))
        {
            _pm.ChangeMovementState(new CrouchingState());

            return;
        }
    }

    private void ClipGroundSpeed()
    {
        Vector3 flatVel = new Vector3(_pm.Rigidbody.velocity.x, 0.0f, _pm.Rigidbody.velocity.z);

        if(flatVel.magnitude > _pm.MaxSpeed)
        {
            Vector3 newSpeed = flatVel.normalized * _pm.MaxSpeed;

            _pm.Rigidbody.velocity = new Vector3(newSpeed.x, _pm.Rigidbody.velocity.y, newSpeed.z);
        }

        _pm.velocityText.text = $"Ground velocity: {flatVel.magnitude:0.##}ups - Y vel: {_pm.Rigidbody.velocity.y:0.##}";
    }
}
