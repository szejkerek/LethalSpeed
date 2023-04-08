using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    protected enum TrapState
    {
        Waiting,
        Activating,
        Active,
        Cooldown
    };

    protected TrapState _state;
    [SerializeField] protected float _delay = 0;
    protected float _delayTimer;
    [SerializeField] protected float _activeTime = 0;
    protected float _activeTimer;
    [SerializeField] protected float _cooldown = 0;
    protected float _cooldownTimer;

    protected virtual void Start()
    {
        _state = TrapState.Waiting;
        SetTimers();
    }

    private void Update()
    {
        if (_state == TrapState.Activating)
        {
            _delayTimer -= Time.deltaTime;
            if (_delayTimer > 0)
                return;
            _delayTimer = _delay;
            ActivateTrap();
        }
        else if(_state== TrapState.Active)
        {
            _activeTimer -= Time.deltaTime;
            if (_activeTimer > 0)
                return;
            _activeTimer = _activeTime;
            DeactivateTrap();
        }
        else if (_state == TrapState.Cooldown)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer > 0)
                return;
            _cooldownTimer = _cooldown;
            WaitForCooldown();
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if (_state == TrapState.Waiting)
        {
            _state = TrapState.Activating;
        }
        else if (_state == TrapState.Active)
        {
            if (other.TryGetComponent<Player>(out Player player)) 
            {
                player.Die();
            }
        }
    }

    private void SetTimers()
    {
        _delayTimer = _delay;
        _activeTimer = _activeTime;
        _cooldownTimer = _cooldown;
    }

    protected virtual void ActivateTrap()
    {
        _state = TrapState.Active;
    }

    protected virtual void DeactivateTrap()
    {
        _state = TrapState.Cooldown;
    }

    protected virtual void WaitForCooldown()
    {
        _state = TrapState.Waiting;
    }
}
