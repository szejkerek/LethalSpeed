using DG.Tweening;
using UnityEngine;

public class AirState : MovementState
{
    private PlayerMovement _pm;
    private float _initialVelocity;
    private float _jumpCommandBuffer;

    public AirState(float startingVelocity)
    {
        _initialVelocity = startingVelocity;
    }

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.MaxSpeed = pm.MovementSpeed;
        _pm.Rigidbody.drag = 0.0f;
    }

    public void Update()
    {
        _jumpCommandBuffer -= Time.deltaTime;

        ClipAirSpeed();

        if(Input.GetKeyDown(_pm.CrouchKey))
        {
            _pm.transform.DOScaleY(_pm.SlideScaleY, 0.25f);
        }
        else if(Input.GetKeyUp(_pm.CrouchKey))
        {
            _pm.transform.DOScaleY(_pm.OriginalScaleY, 0.25f);
        }

        if(_initialVelocity > _pm.MaxSpeed)
        {
            float currectVelocity = _pm.FlatVelocity.magnitude;

            _initialVelocity = currectVelocity < _pm.MaxSpeed ? _pm.MaxSpeed : currectVelocity;
        }

        if(Input.GetKeyDown(_pm.JumpKey))
        {
            _jumpCommandBuffer = 0.5f;
        }

        CheckForModeChange();
    }

    public void Move(Vector3 normalizedWishDir)
    {
        _pm.Rigidbody.AddForce(normalizedWishDir * _pm.MaxSpeed * _pm.AirAcceleration, ForceMode.Force);
        _pm.Rigidbody.AddForce(Vector3.down * _pm.GravityForce, ForceMode.Force);
    }

    public void End()
    {
        
    }

    public void CheckForModeChange()
    {
        if(_pm.JustLanded && Input.GetKey(_pm.CrouchKey))
        {
            _pm.ChangeMovementState(new SlidingState(_pm.FlatVelocity.magnitude));

            return;
        }

        if (_pm.IsGrounded && !_pm.WasGrounded)
        {
            _pm.ChangeMovementState(new RunningState());

            return;
        }

        RaycastHit wallRayHit;

        if(Physics.Raycast(_pm.transform.position + Vector3.up * _pm.PlayerHeight / 2.0f, _pm.orientation.right, out wallRayHit, 0.8f, _pm.WallMask)
            && !Input.GetKey(_pm.CrouchKey) && (Input.GetKeyDown(_pm.JumpKey) || _jumpCommandBuffer >= 0.0f))
        {
            _pm.ChangeMovementState(new WallrunningState(wallRayHit.normal, _pm.Velocity.magnitude));

            return;
        }
        
        if(Physics.Raycast(_pm.transform.position + Vector3.up * _pm.PlayerHeight / 2.0f, -_pm.orientation.right, out wallRayHit, 0.8f, _pm.WallMask)
            && !Input.GetKey(_pm.CrouchKey) && (Input.GetKeyDown(_pm.JumpKey) || _jumpCommandBuffer >= 0.0f))
        {
            _pm.ChangeMovementState(new WallrunningState(wallRayHit.normal, _pm.Velocity.magnitude));
            
            return;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && _pm.CanDash)
        {
            _pm.JustDashed();
            _pm.ChangeMovementState(new DashingState());

            return;
        }
    }

    private void ClipAirSpeed()
    {
        if (_initialVelocity > _pm.MaxSpeed)
        {
            Vector3 newSpeed = _pm.FlatVelocity.normalized * Mathf.Min(_initialVelocity, _pm.FlatVelocity.magnitude);

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }
        else if (_pm.FlatVelocity.magnitude > _pm.MaxSpeed)
        {
            Vector3 newSpeed = _pm.FlatVelocity.normalized * _pm.MaxSpeed;

            _pm.Velocity = new Vector3(newSpeed.x, _pm.Velocity.y, newSpeed.z);
        }

        _pm.velocityText.text = $"Air velocity: {_pm.FlatVelocity.magnitude:0.##}ups - Y vel: {_pm.Velocity.y:0.##}";
    }
}
