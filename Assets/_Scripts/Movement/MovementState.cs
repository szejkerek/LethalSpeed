using UnityEngine;

public interface MovementState
{
    public void Begin(PlayerMovement pm);

    public void Update();

    public void Move(Vector3 normalizedWishDir);

    public void End();

    public void CheckForModeChange();
}
