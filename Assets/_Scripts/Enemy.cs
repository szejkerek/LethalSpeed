using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _triggerRange = 15f;

    private Player _player;
    private bool _playerInRange = false;
    void TryToFindPlayer()
    {
        _player = FindObjectOfType<Player>();    

        if(_player is null)
        {
            Debug.LogWarning("Player not found");
        }
    }

    void CheckIfPlayerInRange()
    {
        _playerInRange = Vector3.Distance(transform.position, _player.transform.position) <= _triggerRange;
    }

    void RotateTowardsPlayer()
    {
        if(_playerInRange)
        {
            // Determine which direction to rotate towards
            Vector3 targetDirection = _player.transform.position - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = _rotationSpeed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
        }    
    }

    void Start()
    {
        TryToFindPlayer();
    }

    void Update()
    {
        CheckIfPlayerInRange();
        RotateTowardsPlayer();
    }
}
