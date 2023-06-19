using UnityEngine;

[System.Serializable]
public struct DashProperties
{
    public float DashCooldown;
    public float DashForce;
    public bool KeepPreviousMomentum;
}

public class DashingState : MovementState
{
    private PlayerMovement _pm;
    private bool _dashForceApplied;
    private float _dashingTime = 0.1f;
    private float _preDashSpeed;

    public DashingState(float preDashSpeed)
    {
        _preDashSpeed = preDashSpeed;
    }

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.JustDashed();
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
            Vector3 dashDir = normalizedWishDir == Vector3.zero ? _pm.Orientation.forward : normalizedWishDir;

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
        _pm.Velocity = _pm.FlatVelocity.normalized * 
            (_pm.DashProps.KeepPreviousMomentum ? Mathf.Max(_pm.CurrentMaxSpeed, _preDashSpeed) : _pm.CurrentMaxSpeed);
        _pm.Rigidbody.useGravity = true;
    }

    public void CheckForModeChange()
    {
        if(_dashingTime <= 0.0f)
        {
            _pm.ChangeMovementState(_pm.IsGrounded ? new RunningState() : 
                new AirState((_pm.DashProps.KeepPreviousMomentum ? Mathf.Max(_pm.CurrentMaxSpeed, _preDashSpeed) : _pm.CurrentMaxSpeed)));

            return;
        }
    }

    public string GetStateName()
    {
        return "Dashing";
    }
}
