using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour
{
    public static event Action OnEndZonePlayerEnter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming the player has a tag "Player"
        {
            OnEndZonePlayerEnter?.Invoke();
        }
    }
}
