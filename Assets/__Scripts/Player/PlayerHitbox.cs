using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : HitBox
{
    public override void TakeHit()
    {
        Debug.Log("Player die");
    }
}
