using UnityEngine;
using DG.Tweening;

public class PlayerCam : MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;

    [Space]
    public Transform orientation;
    public Transform camHolder;

    private Camera _camera;

    private float _xRot;
    private float _yRot;
 
    void Start()
    {
        _camera = GetComponent<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * _sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _sensY * Time.deltaTime;

        _yRot += mouseX;
        _xRot -= mouseY;
        _xRot = Mathf.Clamp(_xRot, -90.0f, 90.0f);

        camHolder.rotation = Quaternion.Euler(_xRot, _yRot, 0.0f);
        orientation.rotation = Quaternion.Euler(0.0f, _yRot, 0.0f);
    }

    public void SetFOV(float targetFOV)
    {
        _camera.DOFieldOfView(targetFOV, 0.25f);
    }

    public void SetTilt(float targetTiltZ)
    {
        transform.DOLocalRotate(new Vector3(0.0f, 0.0f, targetTiltZ), 0.25f);
    }
}
