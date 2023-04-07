using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Transform cameraPosition;

    private void Awake()
    {
        cameraPosition = FindObjectOfType<Player>().CameraPosition;
    }

    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
