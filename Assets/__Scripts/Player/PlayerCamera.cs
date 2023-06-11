using UnityEngine;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour
{
    public Transform EnemyAimTarget => _enemyAimTarget;
    public bool EnableInputs { get => _enableInputs; set => _enableInputs = value; }

    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _enemyAimTarget;

    [Header("Camera settings")]
    [Range(0, 800)]
    [SerializeField] private float _sensX;

    [Range(0, 800)]
    [SerializeField] private float _sensY;

    [Range(0f, 3f)]
    [SerializeField] private float _deathHeight;

    [Range(0f, 3f)]
    [SerializeField] private float _crouchHeight;

    private Player _player;
    private Camera _camera;

    private bool _enableInputs = true;

    private float _xRot;
    private float _yRot;

    private Vector3 _cameraOffset;
    private Vector3 _cameraStandingOffset;
    private Vector3 _cameraCrouchingOffset;
    private Vector3 _cameraDeathOffset;

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

        if (!_enableInputs)
            return;

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
        _cameraCrouchingOffset = (Vector3.up * _crouchHeight);
        _cameraDeathOffset = (Vector3.up * _deathHeight);
        SetCrouchingCamera(crouching: false, duration: 0);
    }

    public void SetCrouchingCamera(bool crouching = true, float duration = 0.25f)
    {
        MoveCameraToOffset(_cameraCrouchingOffset, crouching, duration);
    }

    public void SetDeathCamera(bool dead = true, float duration = 0.55f)
    {
        MoveCameraToOffset(_cameraDeathOffset, dead, duration);
    }

    private void MoveCameraToOffset(Vector3 newOffset, bool enable, float duration)
    {
        Vector3 endValue = enable ? newOffset : _cameraStandingOffset;

        DOTween.To(() => _cameraOffset, x => _cameraOffset = x, endValue, duration);
    }
}