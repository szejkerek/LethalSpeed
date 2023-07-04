using System;
using UnityEngine;

public class EndZone : Zone
{
    public static event Action OnEndZonePlayerEnter;
    bool isOpen = false;

    private void OnTriggerEnter(Collider other)
    {
        isOpen = EnemyManager.Instance.NoEnemiesLeft;
        if (!isOpen)
        {
            Debug.Log("Kill all enemies!!!");
            return;
        }

        if (other.CompareTag("Player")) // Assuming the player has a tag "Player"
        {
            OnEndZonePlayerEnter?.Invoke();
        }
    }
   
}
