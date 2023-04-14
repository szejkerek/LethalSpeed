using DG.Tweening;
using UnityEngine;

[System.Serializable]
public struct LedgeClimbingProperties
{
    public float MinDistance;
    public float RayPierceDistance;
}

public class LedgeClimbingState : MovementState
{
    private PlayerMovement _pm;
    private Vector3 _targetPos;
    private float _climbTimer;
    private bool _finishedClimbing;

    public LedgeClimbingState(RaycastHit ledgeHit)
    {
        _targetPos = new Vector3(ledgeHit.point.x, ledgeHit.point.y + 0.1f, ledgeHit.point.z);
    }

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.transform.DOMoveY(_targetPos.y, 0.25f);
        _climbTimer = 0.5f;
        _finishedClimbing = false;
    }

    public void Update()
    {
        _climbTimer -= Time.deltaTime;

        if(_climbTimer < 0.25f && !_finishedClimbing)
        {
            _pm.transform.DOMove(_targetPos, 0.25f);
            _finishedClimbing = true;
        }

        CheckForModeChange();
    }

    public void Move(Vector3 normalizedWishDir)
    {
        
    }

    public void End()
    {

    }

    public void CheckForModeChange()
    {
        if (_climbTimer < 0.0f && _finishedClimbing)
        {
            _pm.ChangeMovementState(_pm.IsGrounded ? new RunningState() : new AirState(0.0f));

            return;
        }
    }

    public string GetStateName()
    {
        return "Ledge climbing";
    }
}
