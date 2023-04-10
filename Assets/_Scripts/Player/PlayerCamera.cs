using UnityEngine;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _orientation;

    [Header("Camera settings")]
    [Range(0, 800)]
    [SerializeField] private float _sensX;

    [Range(0, 800)]
    [SerializeField] private float _sensY;

    [Range(0f, 3f)]
    [SerializeField] private float _cameraHeight;

    private Player _player;
    private Camera _camera;

    private float _xRot;
    private float _yRot;

    private Vector3 _cameraOffset;
    private Vector3 _cameraStandingOffset;
    private Vector3 _cameraCrouchingOffset;

    private void Awake()
    {
        _camera = Helpers.Camera;
        _player = FindObjectOfType<Player>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetupCameraView();
    }

    void Update()
    {
        MoveCamera();

        float mouseX = Input.GetAxisRaw("Mouse X") * _sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _sensY * Time.deltaTime;

        _yRot += mouseX;
        _xRot -= mouseY;
        _xRot = Mathf.Clamp(_xRot, -90.0f, 90.0f);

        transform.rotation = Quaternion.Euler(_xRot, _yRot, 0);
        _orientation.rotation = Quaternion.Euler(0.0f, _yRot, 0.0f);

        void MoveCamera()
        {
            transform.position = _player.transform.position + _cameraOffset;
        }
    }

    public void SetFOV(float targetFOV)
    {
        _camera.DOFieldOfView(targetFOV, 0.25f);
    }

    public void SetTilt(float targetTiltZ)
    {
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation.z = targetTiltZ;
        _camera.transform.DOLocalRotate(new Vector3(0f, 0f, targetTiltZ), 0.25f);
    }

    private void SetupCameraView()
    {
        transform.SetParent(null);
        _cameraStandingOffset = transform.position - _player.transform.position;
        _cameraCrouchingOffset = (Vector3.up * _cameraHeight);
        SetCameraPosition(crouching: false, duration: 0);
    }

    public void SetCameraPosition(bool crouching = false, float duration = 0.25f)
    {
        Vector3 endValue = crouching ? _cameraCrouchingOffset : _cameraStandingOffset;

        DOTween.To(() => _cameraOffset, x => _cameraOffset = x, endValue, duration);
    }
}