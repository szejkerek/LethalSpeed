using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeapon : MonoBehaviour
{
    public event Action onAttack;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            onAttack?.Invoke();
        }
    }
}
