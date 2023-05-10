using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionEnemyAI : MonoBehaviour
{
    public bool TargerInVision => _targerInVision;
    bool _targerInVision = false;
    public bool TargerInVisionAbsolute => _targerInVisionAbsolute;
    bool _targerInVisionAbsolute = false;    
    public float LastSeenTimer => _lastSeenTimer;
    float _lastSeenTimer = float.MaxValue;
    
    [SerializeField] float _scanIntervals = 0.5f;
    [SerializeField] float _reactionTime = 0.75f;
    [SerializeField] Transform _eyeLevel;
    [SerializeField] LayerMask _player;
    [SerializeField] LayerMask _blockers;

    [Header("Vision errors")]
    [SerializeField] bool _debugErrors = false;
    [Range(0f,1f)]
    [SerializeField] float _topError = 0.32f;
    [Range(0f,0.5f)]
    [SerializeField] float _sideError = 0.32f;

    int targetsInRange;
    float lastScanInternalTimer = 0;
    float reactionInternalTimer = 0;
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

        if(_targerInVisionAbsolute && !_targerInVision)
        {
            reactionInternalTimer += Time.deltaTime;
            if(reactionInternalTimer >= _reactionTime)
            {
                _targerInVision = true;
            }
        }
        else if(!_targerInVisionAbsolute && _targerInVision)
        {
            reactionInternalTimer = 0f;
            _targerInVision = false;
        }
    }

    private void Scan()
    {
        if (Time.time - lastScanInternalTimer < _scanIntervals)
            return;

        targetsInRange = Physics.OverlapSphereNonAlloc(_eyeLevel.position, Mathf.Infinity, colliders, _player, QueryTriggerInteraction.Collide);
        lastScanInternalTimer = Time.time;

        if (colliders[0] is null)
        {
            Debug.LogError("There is no Player in enemy sight.");
            return;
        }

        Transform scannedPlayer = colliders[0].transform;
        UpdatePlayerBodyPartsPositions(scannedPlayer);

        bool inVision = IsInVision();

        _targerInVisionAbsolute = targetsInRange > 0 && inVision;

        if (_targerInVisionAbsolute)
            _lastSeenTimer = 0;
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
        topPosition = scannedPlayer.position + Vector3.up * (scaledPlayerHeight / 2 - _topError);
        bottomPosition = scannedPlayer.position + Vector3.down * (scaledPlayerHeight / 2 - _topError);
        rightPosition = scannedPlayer.position + -Vector3.Cross(Vector3.up, transform.position - scannedPlayer.position).normalized * _sideError;
        leftPosition = scannedPlayer.position + Vector3.Cross(Vector3.up, transform.position - scannedPlayer.position).normalized * _sideError;

        Vector3 myForward = -(scannedPlayer.position - transform.position).normalized;
        myForward.y = 0;

        forwardPosition = scannedPlayer.position + myForward * _sideError;
        backPosition = scannedPlayer.position + -myForward * _sideError;

    }

    bool isBlockedMiddle;
    bool isBlockedTop;
    bool isBlockedBottom;
    bool isBlockedLeft;
    bool isBlockedRight;
    bool isBlockedForward;
    bool isBlockedBack;
    private bool IsInVision()
    {
        isBlockedMiddle = Physics.Linecast(_eyeLevel.position, middlePosition, _blockers);
        isBlockedTop = Physics.Linecast(_eyeLevel.position, topPosition, _blockers);
        isBlockedBottom = Physics.Linecast(_eyeLevel.position, bottomPosition, _blockers);
        isBlockedLeft = Physics.Linecast(_eyeLevel.position, leftPosition, _blockers);
        isBlockedRight = Physics.Linecast(_eyeLevel.position, rightPosition, _blockers);
        isBlockedForward = Physics.Linecast(_eyeLevel.position, forwardPosition, _blockers);
        isBlockedBack = Physics.Linecast(_eyeLevel.position, backPosition, _blockers);

        bool seeCorePart = !isBlockedMiddle || !isBlockedTop || !isBlockedBottom;

        int sidePartsCount = 0;
        if (!isBlockedLeft) sidePartsCount++;
        if (!isBlockedRight) sidePartsCount++;
        if (!isBlockedForward) sidePartsCount++;
        if (!isBlockedBack) sidePartsCount++;

        //bool blocked = isBlockedMiddle && isBlockedTop && isBlockedBottom && isBlockedLeft && isBlockedRight && isBlockedForward && isBlockedBack;
        return seeCorePart || sidePartsCount > 1;
    }

    private List<Vector3> visibleBodyParts = new List<Vector3>(7);
    public List<Vector3> GetAvailableBodyParts()
    {
        visibleBodyParts.Clear();

        if (!isBlockedMiddle)
            visibleBodyParts.Add(middlePosition);

        if (!isBlockedTop)
            visibleBodyParts.Add(topPosition);

        if (!isBlockedLeft)
            visibleBodyParts.Add(leftPosition);

        if (!isBlockedRight)
            visibleBodyParts.Add(rightPosition);

        if (!isBlockedForward)
            visibleBodyParts.Add(forwardPosition);

        if (!isBlockedBottom)
            visibleBodyParts.Add(bottomPosition);

        if (!isBlockedBack)
            visibleBodyParts.Add(backPosition);

        return visibleBodyParts;
    }


    private void OnDrawGizmos()
    {
        if (!_debugErrors || !_targerInVisionAbsolute)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_eyeLevel.position, middlePosition);
        Gizmos.DrawLine(_eyeLevel.position, topPosition);
        Gizmos.DrawLine(_eyeLevel.position, bottomPosition);
        Gizmos.DrawLine(_eyeLevel.position, leftPosition);
        Gizmos.DrawLine(_eyeLevel.position, rightPosition);
        Gizmos.DrawLine(_eyeLevel.position, forwardPosition);
        Gizmos.DrawLine(_eyeLevel.position, backPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(middlePosition,0.01f);
        Gizmos.DrawSphere(topPosition, 0.01f);
        Gizmos.DrawSphere(bottomPosition, 0.01f);
        Gizmos.DrawSphere(leftPosition, 0.01f);
        Gizmos.DrawSphere(rightPosition, 0.01f);
        Gizmos.DrawSphere(forwardPosition, 0.01f);
        Gizmos.DrawSphere(backPosition, 0.01f);

        if(GetAvailableBodyParts().Count != 0)
        {
            Gizmos.DrawLine(_eyeLevel.position, GetAvailableBodyParts()[0]);
        }

    }
}
