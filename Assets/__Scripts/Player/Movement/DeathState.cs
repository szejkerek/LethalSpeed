using UnityEngine;

[System.Serializable]
public struct DeathProperties
{
}

public class DeathState : MovementState
{
    private PlayerMovement _playerMovement;
    private PlayerCamera _playerCamera;
    private PlayerWeapon _playerWeapon;

    public DeathState()
    {

    }

    public void Begin(PlayerMovement pm)
    {
        _playerMovement = pm;
        _playerMovement.CurrentMaxSpeed = pm.GroundProps.MaxSpeed;
        _playerMovement.Velocity = _playerMovement.FlatVelocity;
        _playerMovement.Rigidbody.drag = _playerMovement.GroundProps.Friction;

        _playerCamera = _playerMovement.PlayerCamera;
        _playerCamera.SetDeathCamera();
        _playerCamera.EnableInputs = false;

        _playerWeapon = _playerMovement.PlayerWeapon;
        _playerWeapon.EnableInputs = false;

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
        _playerCamera.SetDeathCamera();
        _playerWeapon.EnableInputs = true;
        _playerCamera.EnableInputs = true;
    }

    public void CheckForModeChange()
    {
    }

    public string GetStateName()
    {
        return "Deathstate";
    }
}
