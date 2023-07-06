using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVision : MonoBehaviour
{
    [SerializeField] LayerMask visionBlockers;
    [SerializeField] float raycastCheckDelay;

    private float _delayTimer = 0.0f;

    private PlayerMovement _playerMovement;
    private PlayerCamera _playerCamera;
    GrappleProperties grappleProperties;
    SwingProperties swingProperties;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerCamera = GetComponent<Player>().PlayerCamera;
        grappleProperties = _playerMovement.GrappleProps;
        swingProperties = _playerMovement.SwingProps;
    }

    public bool GrappleObjectRaycast(out RaycastHit hit, bool applyMaterial = false)
    {
        if (ObjectRaycast(grappleProperties.GrappleAimError, grappleProperties.MaxDistance, grappleProperties.GrappleSurfaceMask, out RaycastHit grappleRayHit))
        {
            if (applyMaterial)
                ApplyOutline(grappleRayHit);

            hit = grappleRayHit;
            float dist = Vector3.Distance(_playerMovement.transform.position, grappleRayHit.point);
            return IsInVision(dist);
        }
        else
        { 
            hit = default;
            return false;
        }
    }

    public bool SwingObjectRaycast(out RaycastHit hit, bool applyMaterial = false)
    {
        if (ObjectRaycast(swingProperties.SwingAimError, swingProperties.MaxDistance, swingProperties.SwingSurfaceMask, out RaycastHit swingRayHit))
        {
            if (applyMaterial)
                ApplyOutline(swingRayHit);

            hit = swingRayHit;
            float dist = Vector3.Distance(_playerMovement.transform.position, swingRayHit.point);
            return IsInVision(dist);
        }
        else
        {
            hit = default;
            return false;
        }
    }

    bool ObjectRaycast(float aimError, float maxDistance, LayerMask targetLayers, out RaycastHit hit)
    {
        return Physics.SphereCast(_playerCamera.transform.position, aimError,_playerCamera.transform.forward, out hit, maxDistance, targetLayers);
    }


    bool IsInVision(float maxDistance)
    {
        bool isBlokced = Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out RaycastHit hit, maxDistance, visionBlockers);

        return !isBlokced;
    }

    void ApplyOutline(RaycastHit hit)
    {
        if (hit.collider.gameObject.TryGetComponent(out MaterialSwapper materialSwapper))
        {
            materialSwapper.ApplyThisFrame();
        }
    }

    void Update()
    {
        if(_delayTimer >= raycastCheckDelay)
        {
            GrappleObjectRaycast(out _, applyMaterial: true);
            SwingObjectRaycast(out _, applyMaterial: true);
        }

        _delayTimer += Time.deltaTime;
    }
}
