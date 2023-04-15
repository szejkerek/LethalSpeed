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
    [SerializeField] LayerMask player;
    [SerializeField] LayerMask blockers;
    
    int count;
    float lastScanTime = 0;
    Collider[] colliders = new Collider[1];
    PlayerMovement playerMovment;
    float playerHeight;
    float epsilon = 0.1f;


    private void Awake()
    {
        playerHeight = 0;
        playerMovment = FindObjectOfType<PlayerMovement>();
        playerHeight = playerMovment.PlayerHeight;
    }

    private void Update()
    {
        Scan();
    }

    public void Scan()
    {  
        if (Time.time - lastScanTime < scanIntervals)
            return;

        count = Physics.OverlapSphereNonAlloc(eyeLevel.position, radious, colliders, player, QueryTriggerInteraction.Collide);
        lastScanTime = Time.time;

        Transform scannedPlayer = colliders[0].transform;

        float scaledPlayerHeight = playerHeight * playerMovment.transform.localScale.y; 

        bool isBlockedMiddle = Physics.Linecast(eyeLevel.position, scannedPlayer.position, blockers);
        bool isBlockedTop = Physics.Linecast(eyeLevel.position, scannedPlayer.position + Vector3.up * (scaledPlayerHeight/2 - epsilon), blockers);
        bool isBlockedBottom = Physics.Linecast(eyeLevel.position, scannedPlayer.position - Vector3.up * (scaledPlayerHeight / 2 - epsilon), blockers);

        bool blocked = isBlockedMiddle && isBlockedTop && isBlockedBottom;
        targerInVision = count > 0 && !blocked;
    }
}
