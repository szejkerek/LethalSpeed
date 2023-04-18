using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class VisionEnemyAI : MonoBehaviour
{
    public bool TargerInVision => targerInVision;
    bool targerInVision = false;    
    public float LastSeenTimer => _lastSeenTimer;
    float _lastSeenTimer = float.MaxValue;
    
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
    float playerHeight;

    private void Awake()
    {
        playerHeight = 0;
        playerMovment = FindObjectOfType<PlayerMovement>();
        playerHeight = playerMovment.PlayerHeight;
    }

    private void Update()
    {
        _lastSeenTimer += Time.deltaTime;
        
        Scan();
    }

    Vector3 topPosition;
    Vector3 middlePosition;
    Vector3 bottomPosition;
    Vector3 rightPosition;
    Vector3 leftPosition;
    Vector3 forwardPosition;
    Vector3 backPosition;
    private void UpdatePlayerBodyPartsPositions(Transform scannedPlayer)
    {
        float scaledPlayerHeight = playerHeight * playerMovment.transform.localScale.y;

        middlePosition = scannedPlayer.position;
        topPosition = scannedPlayer.position + Vector3.up * (scaledPlayerHeight / 2 - topError);
        bottomPosition = scannedPlayer.position + Vector3.down * (scaledPlayerHeight / 2 - topError);
        rightPosition = scannedPlayer.position + -Vector3.Cross(Vector3.up, transform.position - scannedPlayer.position).normalized * sideError;
        leftPosition = scannedPlayer.position + Vector3.Cross(Vector3.up, transform.position - scannedPlayer.position).normalized * sideError;

        Vector3 myForward = -(scannedPlayer.position - transform.position).normalized;
        myForward.y = 0;

        forwardPosition = scannedPlayer.position + myForward * sideError;
        backPosition = scannedPlayer.position +   -myForward * sideError;

    }
    private void Scan()
    {  
        if (Time.time - lastScanTime < scanIntervals)
            return;

        count = Physics.OverlapSphereNonAlloc(eyeLevel.position, Mathf.Infinity, colliders, player, QueryTriggerInteraction.Collide);
        lastScanTime = Time.time;

        if (colliders[0] is null)
        {
            Debug.LogError("There is no Player in enemy sight.");
            return;
        }

        Transform scannedPlayer = colliders[0].transform;
        UpdatePlayerBodyPartsPositions(scannedPlayer);

        bool isBlockedMiddle = Physics.Linecast(eyeLevel.position, middlePosition, blockers);
        bool isBlockedTop = Physics.Linecast(eyeLevel.position, topPosition, blockers);
        bool isBlockedBottom = Physics.Linecast(eyeLevel.position, bottomPosition, blockers);
        bool isBlockedLeft = Physics.Linecast(eyeLevel.position, leftPosition, blockers);
        bool isBlockedRight = Physics.Linecast(eyeLevel.position, rightPosition, blockers);
        bool isBlockedForward = Physics.Linecast(eyeLevel.position, forwardPosition, blockers);
        bool isBlockedBack = Physics.Linecast(eyeLevel.position, backPosition, blockers);

        bool blocked = isBlockedMiddle && isBlockedTop && isBlockedBottom && isBlockedLeft && isBlockedRight && isBlockedForward && isBlockedBack;

        targerInVision = (count > 0) && (!blocked);

        if (targerInVision)
            _lastSeenTimer = 0;
    }

    private void OnDrawGizmos()
    {
        if (!debugErrors && !targerInVision)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(eyeLevel.position, middlePosition);
        Gizmos.DrawLine(eyeLevel.position, topPosition);
        Gizmos.DrawLine(eyeLevel.position, bottomPosition);
        Gizmos.DrawLine(eyeLevel.position, leftPosition);
        Gizmos.DrawLine(eyeLevel.position, rightPosition);
        Gizmos.DrawLine(eyeLevel.position, forwardPosition);
        Gizmos.DrawLine(eyeLevel.position, backPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(middlePosition,0.01f);
        Gizmos.DrawSphere(topPosition, 0.01f);
        Gizmos.DrawSphere(bottomPosition, 0.01f);
        Gizmos.DrawSphere(leftPosition, 0.01f);
        Gizmos.DrawSphere(rightPosition, 0.01f);
        Gizmos.DrawSphere(forwardPosition, 0.01f);
        Gizmos.DrawSphere(backPosition, 0.01f);

    }
}
