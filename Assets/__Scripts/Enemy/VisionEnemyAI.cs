using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionEnemyAI : MonoBehaviour
{
    public bool TargerInVision => targerInVision;
    bool targerInVision = false;
    
    [SerializeField] float scanIntervals = 0.5f;
    [SerializeField] float radious;
    [SerializeField] Transform eyeLevel;
    [SerializeField] LayerMask targets;
    [SerializeField] LayerMask blockers;
    
    int count;
    float lastScanTime = 0;
    Collider[] colliders = new Collider[1];

    private void Update()
    {
        Scan();
    }

    public void Scan()
    {  
        if (Time.time - lastScanTime < scanIntervals)
            return;

        count = Physics.OverlapSphereNonAlloc(eyeLevel.position, radious, colliders, targets, QueryTriggerInteraction.Collide);
        lastScanTime = Time.time;

        Transform scannedTarget = colliders[0].transform;

        bool isBlocked = Physics.Linecast(eyeLevel.position, scannedTarget.position, blockers);
       
        targerInVision = count > 0 && !isBlocked;
    }
}
