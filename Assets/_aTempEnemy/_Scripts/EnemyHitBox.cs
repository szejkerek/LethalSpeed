using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : HitBox
{
    [HideInInspector]
    public Enemy Enemy;
    public override void TakeHit()
    {
        Enemy.Die();
    }
}
