using UnityEngine;

public class DeathZone : Zone
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HitBox player))// Assuming the player has a tag "Player"
        {
            player.TakeHit(Vector3.zero, other.ClosestPointOnBounds(transform.position));
        }
    }
}
