using UnityEngine;

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
        _pm.MaxSpeed = pm.MovementSpeed;
        _pm.Rigidbody.drag = 0.0f;

        _pc = _pm.pc;
        _hookGunTip = _pm.GrappleTip;
        _lr = _pm.Lr;

        RaycastHit swingRayHit;

        if(Physics.Raycast(_pc.transform.position, _pc.transform.forward, out swingRayHit, _pm.MaxSwingingDistance, _pm.SwingMask))
        {
            _swingPoint = swingRayHit.point;
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
        _pm.Rigidbody.AddForce(Vector3.down * _pm.GravityForce, ForceMode.Force);
    }

    public void End()
    {
        _lr.enabled = false;
        MonoBehaviour.Destroy(_joint);
    }

    public void CheckForModeChange()
    {
        if(!Input.GetKey(_pm.HookKey))
        {
            _pm.Rigidbody.drag = _pm.IsGrounded ? _pm.GroundFriction : 0.0f;
            _pm.ChangeMovementState(_pm.IsGrounded ? new RunningState() : new AirState(_pm.FlatVelocity.magnitude));

            return;
        }
    }

    private void ClipSwingSpeed()
    {
        if (_initialSpeed > _pm.MaxSpeed)
        {
            _pm.velocityText.text = $"Swinging velocity: {_pm.FlatVelocity.magnitude:0.##}ups - Y vel: {_pm.Velocity.y:0.##}";
            
            float drop = _pm.Velocity.magnitude - _pm.MaxSpeed > 1.0f ?
                _pm.GroundDeacceleration * Time.deltaTime / 5000.0f : _pm.Velocity.magnitude - _pm.MaxSpeed;

            Vector3 newSpeed = _pm.Velocity.normalized * Mathf.Min(_initialSpeed, _pm.Velocity.magnitude - drop);

            _pm.Velocity = newSpeed;
        }
        else if (_pm.Velocity.magnitude > _pm.MaxSpeed)
        {
            float drop = _pm.Velocity.magnitude - _pm.MaxSpeed > 3.0f ? _pm.GroundDeacceleration * Time.deltaTime : _pm.Velocity.magnitude - _pm.MaxSpeed;
            Vector3 newSpeed = _pm.Velocity.normalized * (_pm.Velocity.magnitude - drop);

            _pm.Velocity = newSpeed;
        }

        _pm.velocityText.text = $"Swinging velocity: {_pm.FlatVelocity.magnitude:0.##}ups - Y vel: {_pm.Velocity.y:0.##}";
    }
}
