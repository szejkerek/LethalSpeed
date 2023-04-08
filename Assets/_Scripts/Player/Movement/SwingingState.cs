using UnityEngine;

[System.Serializable]
public struct SwingProperties
{
    public float MaxDistance;
    public float SwingAimError;
    public LayerMask SwingSurfaceMask;
}

public class SwingingState : MovementState
{
    private PlayerMovement _pm;
    private PlayerCam _pc;
    private Transform _hookGunTip;
    private LineRenderer _lr;

    private Vector3 _swingPoint;
    private SpringJoint _joint;

    private float _initialSpeed;

    public SwingingState(float initialSpeed)
    {
        _initialSpeed = initialSpeed;
    }

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.CurrentMaxSpeed = pm.GroundProps.MaxSpeed;
        _pm.Rigidbody.drag = 0.0f;

        _pc = _pm.pc;
        _hookGunTip = _pm.GrappleProps.HookGunTip;
        _lr = _pm.GrappleProps.HookLineRenderer;

        RaycastHit swingRayHit;

        if(Physics.Raycast(_pc.transform.position, _pc.transform.forward, out swingRayHit, _pm.SwingProps.MaxDistance, _pm.SwingProps.SwingSurfaceMask)
            || Physics.SphereCast(_pc.transform.position, _pm.SwingProps.SwingAimError,
            _pc.transform.forward, out swingRayHit, _pm.SwingProps.MaxDistance, _pm.SwingProps.SwingSurfaceMask))
        {
            _swingPoint = swingRayHit.transform.position;
            _lr.enabled = true;
            _lr.SetPosition(1, _swingPoint);

            _joint = _pm.gameObject.AddComponent<SpringJoint>();
            _joint.autoConfigureConnectedAnchor = false;
            _joint.connectedAnchor = _swingPoint;

            float distance = Vector3.Distance(_pm.transform.position, _swingPoint);

            _joint.maxDistance = distance * 0.8f;
            _joint.minDistance = distance * 0.25f;
            _joint.spring = 4.5f;
            _joint.damper = 7.0f;
            _joint.massScale = 4.5f;
        }
        else
        {
            _pm.Rigidbody.drag = _pm.IsGrounded ? _pm.GroundProps.Friction : 0.0f;
            _pm.ChangeMovementState(_pm.IsGrounded ? new RunningState() : new AirState(_pm.FlatVelocity.magnitude));

            return;
        }
    }

    public void Update()
    {
        _lr.SetPosition(0, _hookGunTip.position);

        ClipSwingSpeed();
        CheckForModeChange();
    }

    public void Move(Vector3 normalizedWishDir)
    {
        _pm.Rigidbody.AddForce(normalizedWishDir * 10.0f, ForceMode.Force);
        _pm.Rigidbody.AddForce(Vector3.down * _pm.AirProps.GravityForce, ForceMode.Force);
    }

    public void End()
    {
        _lr.enabled = false;
        MonoBehaviour.Destroy(_joint);
    }

    public void CheckForModeChange()
    {
        if(!Input.GetKey(_pm.SwingKey))
        {
            _pm.Rigidbody.drag = _pm.IsGrounded ? _pm.GroundProps.Friction : 0.0f;
            _pm.ChangeMovementState(_pm.IsGrounded ? new RunningState() : new AirState(_pm.FlatVelocity.magnitude));

            return;
        }
    }

    public string GetStateName()
    {
        return "Swinging";
    }

    private void ClipSwingSpeed()
    {
        if (_initialSpeed > _pm.CurrentMaxSpeed)
        {
            float drop = _pm.Velocity.magnitude - _pm.CurrentMaxSpeed > 1.0f ?
                _pm.GroundProps.Deacceleration * Time.deltaTime / 5000.0f : _pm.Velocity.magnitude - _pm.CurrentMaxSpeed;

            Vector3 newSpeed = _pm.Velocity.normalized * Mathf.Min(_initialSpeed, _pm.Velocity.magnitude - drop);

            _pm.Velocity = newSpeed;
        }
        else if (_pm.Velocity.magnitude > _pm.CurrentMaxSpeed)
        {
            float drop = _pm.Velocity.magnitude - _pm.CurrentMaxSpeed > 3.0f ? _pm.GroundProps.Deacceleration * Time.deltaTime : _pm.Velocity.magnitude - _pm.CurrentMaxSpeed;
            Vector3 newSpeed = _pm.Velocity.normalized * (_pm.Velocity.magnitude - drop);

            _pm.Velocity = newSpeed;
        }
    }
}
