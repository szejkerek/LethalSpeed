using UnityEngine;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera settings")]
    [Range(0, 800)]
    [SerializeField] private float _sensX;

    [Range(0, 800)]
    [SerializeField] private float _sensY;

    private Player _player;
    private Camera _camera;
    private Transform _orientation;

    private float _xRot;
    private float _yRot;

    private void Awake()
    {
        _camera = Helpers.Camera;
        _player = FindObjectOfType<Player>();
        _orientation = _player.Orientation;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        transform.position = _player.CameraPosition;

        float mouseX = Input.GetAxisRaw("Mouse X") * _sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _sensY * Time.deltaTime;

        _yRot += mouseX;
        _xRot -= mouseY;
        _xRot = Mathf.Clamp(_xRot, -90.0f, 90.0f);

        transform.rotation = Quaternion.Euler(_xRot, _yRot, 0);
        _orientation.rotation = Quaternion.Euler(0.0f, _yRot, 0.0f);
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
}