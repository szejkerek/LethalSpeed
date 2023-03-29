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
            Vector3 targetDirection = _player.transform.position - transform.position;
            float singleStep = _rotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
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
