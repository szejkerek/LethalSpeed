using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxEnemyAI : HitBox
{
    [HideInInspector]
    public Enemy Enemy;
    public override void TakeHit(Vector3 direction)
    {
        Enemy.Die(direction);
    }
}
