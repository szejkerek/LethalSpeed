using UnityEngine;

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
        _pm.MaxSpeed = pm.MovementSpeed;

        _pc = _pm.pc;
        _grappleGunTip = _pm.GrappleTip;
        _lr = _pm.Lr;

        _preGrapple = true;

        RaycastHit grappleRayHit;

        if(Physics.Raycast(_pc.transform.position, _pc.transform.forward, out grappleRayHit, _pm.GrapplingMaxDistance, _pm.GrappleMask))
        {
            _grappleTargetPoint = grappleRayHit.point;
            _lr.enabled = true;
            _lr.SetPosition(1, _grappleTargetPoint);
            _startGrapplingDelay = _pm.GrapplingDelay;
        }
        else
        {
            StopGrappling();
        }
    }

    public void Update()
    {
        _pm.velocityText.text = $"Grappling velocity: {_pm.FlatVelocity.magnitude:0.##}ups - Y vel: {_pm.Velocity.y:0.##}";

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
            _pm.MaxSpeed = _pm.MovementSpeed;
            _pm.Rigidbody.drag = _pm.GroundFriction;
        }
    }

    public void Move(Vector3 normalizedWishDir)
    {
        if(_preGrapple && !_pm.IsGrounded)
        {
            _pm.Rigidbody.AddForce(Vector3.down * _pm.GravityForce, ForceMode.Force);
        }
    }

    public void End()
    {

    }

    public void CheckForModeChange()
    {
        if(!_preGrapple && (_grappleTargetPoint - _pm.transform.position).magnitude < 5.0f)
        {
            _pm.Velocity = _pm.Velocity.normalized * _pm.MovementSpeed;
            StopGrappling();

            return;
        }
    }

    private void StartGrappling()
    {
        _preGrapple = false;

        _pm.Velocity = Vector3.zero;
        _pm.Rigidbody.useGravity = false;
        _pm.Rigidbody.drag = 0.0f;
        _pm.Rigidbody.AddForce((_grappleTargetPoint - _pm.transform.position).normalized * _pm.GrappleForce, ForceMode.Impulse);
    }

    private void StopGrappling()
    {
        _preGrapple = true;
        _lr.enabled = false;

        _pm.Rigidbody.useGravity = true;
        _pm.Rigidbody.drag = _pm.IsGrounded ? _pm.GroundFriction : 0.0f;
        _pm.ChangeMovementState(_pm.IsGrounded ? new RunningState() : new AirState(_pm.FlatVelocity.magnitude));
    }
}
