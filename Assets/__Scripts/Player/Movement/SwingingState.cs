using UnityEngine;

[System.Serializable]
public struct SwingProperties
{
    public float MaxDistance;
    public float Acceleration;
    public float SwingAimError;
    public float SpringForce;
    public float SpringDamper;
    public float MassScale;
    public LayerMask SwingSurfaceMask;
    public bool ShouldResetDash;
    public bool ShouldClipSpeed;
}

public class SwingingState : MovementState
{
    private PlayerMovement _pm;
    private PlayerCamera _pc;
    private Transform _hookGunTip;
    private RopeRenderer _ropeRenderer;

    private Vector3 _swingPoint;
    private SpringJoint _joint;

    private float _initialSpeed;
    private float _minSwingingTime;

    public SwingingState(float initialSpeed)
    {
        _initialSpeed = initialSpeed;
        _minSwingingTime = 1.0f;
    }

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.CurrentMaxSpeed = pm.GroundProps.MaxSpeed;
        _pm.Rigidbody.useGravity = true;
        _pm.Rigidbody.drag = 0.0f;

        _pc = _pm.PlayerCamera;
        _ropeRenderer = _pm.RopeRenderer;

        if(_pm.PlayerVision.SwingObjectRaycast(out RaycastHit swingRayHit))
        {
            _swingPoint = swingRayHit.transform.position;

            _joint = _pm.gameObject.AddComponent<SpringJoint>();
            _joint.autoConfigureConnectedAnchor = false;
            _joint.connectedAnchor = _swingPoint;

            float distance = Vector3.Distance(_pm.transform.position, _swingPoint);

            _joint.maxDistance = distance * 0.8f;
            _joint.minDistance = distance * 0.25f;
            _joint.spring = _pm.SwingProps.SpringForce;
            _joint.damper = _pm.SwingProps.SpringDamper;
            _joint.massScale = _pm.SwingProps.MassScale;

            if (_pm.SwingProps.ShouldResetDash)
            {
                _pm.ResetDash();
            }
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
        _minSwingingTime -= Time.deltaTime;
        _ropeRenderer.DrawRope(_joint != null, _swingPoint);
        
        if (_initialSpeed > _pm.CurrentMaxSpeed)
        {
            float currectVelocity = _pm.Velocity.magnitude;

            _initialSpeed = currectVelocity < _pm.CurrentMaxSpeed ? _pm.CurrentMaxSpeed : currectVelocity;
        }

        ClipSwingSpeed();
        CheckForModeChange();
    }

    public void Move(Vector3 normalizedWishDir)
    {
        _pm.Rigidbody.AddForce(normalizedWishDir * _pm.SwingProps.Acceleration, ForceMode.Force);
        _pm.Rigidbody.AddForce(Vector3.down * _pm.AirProps.GravityForce, ForceMode.Force);
    }

    public void End()
    {
        Object.Destroy(_joint);
    }

    public void CheckForModeChange()
    {
        if(!Input.GetKey(_pm.SwingKey))
        {
            _ropeRenderer.RestartRope();
            _pm.Rigidbody.drag = _pm.IsGrounded ? _pm.GroundProps.Friction : 0.0f;
            _pm.ChangeMovementState(_pm.IsGrounded ? new RunningState() : new AirState(_pm.FlatVelocity.magnitude));

            return;
        }

        if(_pm.Velocity.magnitude < 0.25f && Vector3.Dot((_pm.transform.position - _swingPoint).normalized, Vector3.down) > 0.98f && _minSwingingTime < 0.0f)
        {
            _ropeRenderer.RestartRope();
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
        if(!_pm.SwingProps.ShouldClipSpeed)
        {
            return;
        }

        if (_initialSpeed > _pm.CurrentMaxSpeed)
        {
            float drop = _pm.Velocity.magnitude - _pm.CurrentMaxSpeed > 1.0f ?
                _pm.GroundProps.Deacceleration * Time.deltaTime / 50.0f : _pm.Velocity.magnitude - _pm.CurrentMaxSpeed;
        
            Vector3 newSpeed = _pm.Velocity.normalized * Mathf.Min(_initialSpeed, _pm.Velocity.magnitude - drop);
        
            _pm.Velocity = newSpeed;
        }
        else if (_pm.Velocity.magnitude > _pm.CurrentMaxSpeed)
        {
            float drop = _pm.Velocity.magnitude - _pm.CurrentMaxSpeed > 3.0f ? _pm.GroundProps.Deacceleration * Time.deltaTime : 3.0f;
            Vector3 newSpeed = _pm.Velocity.normalized * (_pm.Velocity.magnitude - drop);
        
            _pm.Velocity = newSpeed;
        }
        else
        {
            float drop = 3.0f * Time.deltaTime;
            Vector3 newSpeed = _pm.Velocity.normalized * (_pm.Velocity.magnitude - drop);

            _pm.Velocity = newSpeed;
        }
    }
}
