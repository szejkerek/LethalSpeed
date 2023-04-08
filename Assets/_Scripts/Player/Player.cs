using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform CameraPosition;
    public Transform Orientation;
    public GameObject DeathScreen;

    private bool _isDead = false;

    private PlayerMovement _playerMovement;

    public void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public void Die()
    {
        if (_isDead) return;
        _isDead = true;
        _playerMovement.enabled = false;
        DeathScreen.SetActive(true);
    }
}
