using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class VisionEnemyAI : MonoBehaviour
{
    public bool TargerInVision => targerInVision;
    bool targerInVision = false;
    
    [SerializeField] float scanIntervals = 0.5f;
    [SerializeField] Transform eyeLevel;
    [SerializeField] LayerMask player;
    [SerializeField] LayerMask blockers;

    [Header("Vision errors")]
    [SerializeField] bool debugErrors = false;
    [Range(0f,1f)]
    [SerializeField] float topError = 0.32f;
    [Range(0f,0.5f)]
    [SerializeField] float sideError = 0.32f;

    int count;
    float lastScanTime = 0;
    Collider[] colliders = new Collider[1];
    PlayerMovement playerMovment;
    Transform scannedPlayer;
    float scaledPlayerHeight;
    float playerHeight;

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

        count = Physics.OverlapSphereNonAlloc(eyeLevel.position, Mathf.Infinity, colliders, player, QueryTriggerInteraction.Collide);
        lastScanTime = Time.time;

        if (colliders[0] is null)
            return;

        scannedPlayer = colliders[0].transform;
        scaledPlayerHeight = playerHeight * playerMovment.transform.localScale.y;

        bool isBlockedMiddle = Physics.Linecast(eyeLevel.position, scannedPlayer.position, blockers);
        bool isBlockedTop = Physics.Linecast(eyeLevel.position, scannedPlayer.position + Vector3.up * (scaledPlayerHeight/2 - topError), blockers);
        bool isBlockedBottom = Physics.Linecast(eyeLevel.position, scannedPlayer.position + Vector3.down * (scaledPlayerHeight / 2 - topError), blockers);
        bool isBlockedLeft = Physics.Linecast(eyeLevel.position, scannedPlayer.position + Vector3.left * sideError, blockers);
        bool isBlockedRight = Physics.Linecast(eyeLevel.position, scannedPlayer.position + Vector3.right * sideError, blockers);

        bool blocked = isBlockedMiddle && isBlockedTop && isBlockedBottom && isBlockedLeft && isBlockedRight;

        targerInVision = (count > 0) && (!blocked);
    }

    private void OnDrawGizmos()
    {
        if (!debugErrors && !targerInVision)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(eyeLevel.position, scannedPlayer.position);
        Gizmos.DrawLine(eyeLevel.position, scannedPlayer.position + Vector3.up * (scaledPlayerHeight / 2 - topError));
        Gizmos.DrawLine(eyeLevel.position, scannedPlayer.position + Vector3.down * (scaledPlayerHeight / 2 - topError));
        Gizmos.DrawLine(eyeLevel.position, scannedPlayer.position + Vector3.left * sideError);
        Gizmos.DrawLine(eyeLevel.position, scannedPlayer.position + Vector3.right * sideError);

    }
}
