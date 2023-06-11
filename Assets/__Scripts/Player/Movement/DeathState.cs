using UnityEngine;

[System.Serializable]
public struct DeathProperties
{
}

public class DeathState : MovementState
{
    private PlayerMovement _pm;
    private PlayerCamera _pc;

    public DeathState()
    {

    }

    public void Begin(PlayerMovement pm)
    {
        _pm = pm;
        _pm.CurrentMaxSpeed = pm.GroundProps.MaxSpeed;
        _pm.Velocity = _pm.FlatVelocity;
        _pm.Rigidbody.drag = _pm.GroundProps.Friction;

        _pc = _pm._playerCamera;
        //_pc.SetFOV(70.0f);
    }

    public void Update()
    {

    }

    public void Move(Vector3 normalizedWishDir)
    {

    }

    public void End()
    {
        //_pc.SetFOV(60.0f);
        //_pc.SetTilt(0.0f);
    }

    public void CheckForModeChange()
    {
    }

    public string GetStateName()
    {
        return "Deathstate";
    }
}
