using System;
using UnityEngine;

public class EndZone : Zone
{
    public static event Action OnEndZonePlayerEnter;
    bool isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming the player has a tag "Player"
        {
            OnEndZonePlayerEnter?.Invoke();
        }
    }
   
}
