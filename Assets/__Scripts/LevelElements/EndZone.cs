using System;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    public static event Action OnEndZonePlayerEnter;
    bool isOpen = false;

    private void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;        
    }

    private void OnTriggerEnter(Collider other)
    {
        isOpen = EnemyManager.Instance.NoEnemiesLeft;
        if (!isOpen)
            return;

        if (other.CompareTag("Player")) // Assuming the player has a tag "Player"
        {
            OnEndZonePlayerEnter?.Invoke();
        }
    }

   
}
