using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 CameraPosition => transform.position + _cameraOffset;
    public PlayerCamera PlayerCamera => _playerCamera;

    private PlayerCamera _playerCamera;
    private Vector3 _cameraOffset;
    public Transform Orientation;

    private void Awake()
    {
        SetupCameraView();
    }

    private void SetupCameraView()
    {
        _playerCamera = GetComponentInChildren<PlayerCamera>();
        _cameraOffset = _playerCamera.gameObject.transform.position - transform.position;
    }
}
