using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private float xRotation = 0.0f;

    public Transform playerTransform;

    [Header("Camera settings")]
    [SerializeField] private float mouseSensitivityX = 150.0f;
    [SerializeField] private float mouseSensitivityY = 150.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivityY * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        this.transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
        playerTransform.Rotate(Vector3.up, mouseX);
    }
}
