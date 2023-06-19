using UnityEngine;

[System.Serializable]
public struct GrappleProperties
{
    public float MaxDistance;
    public float GrappleDelay;
    public float GrappleForce;
    public float AfterGrappleForce;
    public LayerMask GrappleSurfaceMask;
    public float GrappleAimError;
    public bool ShouldResetDash;
}

public class GrapplingState : MovementState
{
    private PlayerMovement _pm;
    private PlayerCamera _pc;
    private Vector3 _grappleTargetPoint;
    private Vector3 _trajectory;
    private RopeRenderer _ropeRenderer;
    private bool _preGrapple;

    private float _startGrapplingDelay;

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.CurrentMaxSpeed = pm.GroundProps.MaxSpeed;

        _pc = _pm.PlayerCamera;
        _ropeRenderer = _pm.RopeRenderer;

        _preGrapple = true;

        if(_pm.PlayerVision.GrappleObjectRaycast(out RaycastHit grappleRayHit))
        {
            _grappleTargetPoint = grappleRayHit.transform.position;
            _startGrapplingDelay = _pm.GrappleProps.GrappleDelay;

            if (_pm.SwingProps.ShouldResetDash)
            {
                _pm.ResetDash();
            }
        }
        else
        {
            StopGrappling();
        }
    }

    public void Update()
    {
        _startGrapplingDelay -= Time.deltaTime;

        _ropeRenderer.DrawRope(!_preGrapple, _grappleTargetPoint);

        if(_startGrapplingDelay <= 0.0f && _preGrapple)
        {
            StartGrappling();

            return;
        }

        if(!_preGrapple)
        {
            if(Vector3.Dot((_grappleTargetPoint - _pm.transform.position).normalized, _trajectory) < 0.98f)
            {
                _pm.Velocity = _pm.Velocity.normalized * _pm.GroundProps.MaxSpeed;
                StopGrappling();

                return;
            }
            
            CheckForModeChange();
        }
        else if(_pm.JustLanded)
        {
            _pm.CurrentMaxSpeed = _pm.GroundProps.MaxSpeed;
            _pm.Rigidbody.drag = _pm.GroundProps.Friction;
        }
    }

    public void Move(Vector3 normalizedWishDir)
    {
        if(_preGrapple && !_pm.IsGrounded)
        {
            _pm.Rigidbody.AddForce(Vector3.down * _pm.AirProps.GravityForce, ForceMode.Force);
        }
    }

    public void End()
    {

    }

    public void CheckForModeChange()
    {
        if(!_preGrapple &&(_grappleTargetPoint - _pm.transform.position).magnitude < 5.0f )
        {
            _pm.Velocity = _pm.Velocity.normalized * _pm.GroundProps.MaxSpeed * (Input.GetKey(_pm.GrappleKey) ? _pm.GrappleProps.AfterGrappleForce : 1.0f);
            StopGrappling();

            return;
        }
    }

    public string GetStateName()
    {
        return "Grappling";
    }

    private void StartGrappling()
    {
        _preGrapple = false;

        _pm.Velocity = Vector3.zero;
        _pm.Rigidbody.useGravity = false;
        _pm.Rigidbody.drag = 0.0f;
        _pm.Rigidbody.AddForce((_grappleTargetPoint - _pm.transform.position) * _pm.GrappleProps.GrappleForce, ForceMode.Impulse);
        _trajectory = (_grappleTargetPoint - _pm.transform.position).normalized;
    }

    private void StopGrappling()
    {
        _preGrapple = true;
        _ropeRenderer.RestartRope();

        _pm.Rigidbody.useGravity = true;
        _pm.Rigidbody.drag = _pm.IsGrounded ? _pm.GroundProps.Friction : 0.0f;
        _pm.ChangeMovementState(_pm.IsGrounded ? new RunningState() : new AirState(_pm.FlatVelocity.magnitude));
    }
}
