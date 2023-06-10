using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : HitBox
{
    [HideInInspector] public Player Player;
    public override void TakeHit(Vector3 direction, Vector3 hitPoint)
    {
        Player.PlayerDeath();
    }
}
