using UnityEngine;

[System.Serializable]
public struct DashProperties
{
    public float DashForce;
}

public class DashingState : MovementState
{
    private PlayerMovement _pm;
    private bool _dashForceApplied;
    private float _dashingTime = 0.1f;

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.Rigidbody.drag = 0.0f;
        _pm.Rigidbody.useGravity = false;

        _dashForceApplied = false;
    }

    public void Update()
    {
        CheckForModeChange();
    }

    public void Move(Vector3 normalizedWishDir)
    {
        if(!_dashForceApplied)
        {
            Vector3 dashDir = normalizedWishDir == Vector3.zero ? _pm.orientation.forward : normalizedWishDir;

            _pm.Velocity = Vector3.zero;
            _pm.Rigidbody.AddForce(dashDir * _pm.DashProps.DashForce, ForceMode.Impulse);

            _dashForceApplied = true;
        }
        else
        {
            _dashingTime -= Time.deltaTime;
        }
    }

    public void End()
    {

    }

    public void CheckForModeChange()
    {
        if(_dashingTime <= 0.0f)
        {
            EndDashForce();
            _pm.ChangeMovementState(_pm.IsGrounded ? new RunningState() : new AirState(_pm.CurrentMaxSpeed));

            return;
        }
    }

    public string GetStateName()
    {
        return "Dashing";
    }

    private void EndDashForce()
    {
        _pm.Velocity = _pm.FlatVelocity.normalized * _pm.CurrentMaxSpeed;
        _pm.Rigidbody.useGravity = true;
    }
}
