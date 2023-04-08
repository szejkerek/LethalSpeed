using UnityEngine;

[System.Serializable]
public struct GrappleProperties
{
    public float MaxDistance;
    public float GrappleDelay;
    public float GrappleForce;
    public float AfterGrappleForce;
    public LayerMask GrappleSurfaceMask;

    [HideInInspector] public LineRenderer HookLineRenderer;
    [HideInInspector] public Transform HookGunTip;
}

public class GrapplingState : MovementState
{
    private PlayerMovement _pm;
    private PlayerCam _pc;
    private Transform _grappleGunTip;
    private Vector3 _grappleTargetPoint;
    private LineRenderer _lr;
    private bool _preGrapple;

    private float _startGrapplingDelay;

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.CurrentMaxSpeed = pm.GroundProps.MaxSpeed;

        _pc = _pm.pc;
        _grappleGunTip = _pm.GrappleProps.HookGunTip;
        _lr = _pm.GrappleProps.HookLineRenderer;

        _preGrapple = true;

        RaycastHit grappleRayHit;

        if(Physics.Raycast(_pc.transform.position, _pc.transform.forward, out grappleRayHit, _pm.GrappleProps.MaxDistance, _pm.GrappleProps.GrappleSurfaceMask))
        {
            _grappleTargetPoint = grappleRayHit.point;
            _lr.enabled = true;
            _lr.SetPosition(1, _grappleTargetPoint);
            _startGrapplingDelay = _pm.GrappleProps.GrappleDelay;
        }
        else
        {
            StopGrappling();
        }
    }

    public void Update()
    {
        _lr.SetPosition(0, _grappleGunTip.position);
        _startGrapplingDelay -= Time.deltaTime;

        if(_startGrapplingDelay <= 0.0f && _preGrapple)
        {
            StartGrappling();

            return;
        }

        if(!_preGrapple)
        {
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
        if(!_preGrapple && (_grappleTargetPoint - _pm.transform.position).magnitude < 5.0f)
        {
            _pm.Velocity = _pm.Velocity.normalized * _pm.GroundProps.MaxSpeed * (Input.GetKey(_pm.HookKey) ? _pm.GrappleProps.AfterGrappleForce : 1.0f);
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
    }

    private void StopGrappling()
    {
        _preGrapple = true;
        _lr.enabled = false;

        _pm.Rigidbody.useGravity = true;
        _pm.Rigidbody.drag = _pm.IsGrounded ? _pm.GroundProps.Friction : 0.0f;
        _pm.ChangeMovementState(_pm.IsGrounded ? new RunningState() : new AirState(_pm.FlatVelocity.magnitude));
    }
}
