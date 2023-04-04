using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("Camera settings")]
    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;

    [Space]
    public Transform orientation;

    private float _xRot;
    private float _yRot;
 
    void Start()
    {
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

        transform.rotation = Quaternion.Euler(_xRot, _yRot, 0.0f);
        orientation.rotation = Quaternion.Euler(0.0f, _yRot, 0.0f);
    }
}
