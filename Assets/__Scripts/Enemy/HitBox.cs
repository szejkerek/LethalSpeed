using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class HitBox : MonoBehaviour
{
    public abstract void TakeHit(Vector3 direction); 
}
