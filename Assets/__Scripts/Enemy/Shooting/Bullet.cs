using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Enemy _firedByReference;
    Action<Bullet> _onHitAction;
    float _speed;
    Vector3 _direction;
    float _timeInAir;
    float _maxTimeInAir;

    public void Init(Enemy firedByReference, Vector3 direction, float speed, Action<Bullet> onHitAction, float maxTimeInAir = 5f)
    {
        _timeInAir = 0;
        _direction = direction;
        _speed = speed;
        _onHitAction = onHitAction;
        _maxTimeInAir = maxTimeInAir;
        _firedByReference = firedByReference;
    }

    void Update()
    {
        _timeInAir += Time.deltaTime;
        transform.position += _direction * _speed * Time.deltaTime;

        if(_timeInAir > _maxTimeInAir)
        {
            _onHitAction(this);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (ShootHimself(col))
            return;

        if (col.TryGetComponent(out HitBox hitBox))
        {
            hitBox.TakeHit();
        }

        _onHitAction(this);
    }

    private bool ShootHimself(Collider hit)
    {
        foreach (Collider firedByCollider in _firedByReference.GetComponentsInChildren<Collider>())
        {
            if (hit == firedByCollider)
            {
                return true;
            }
        }

        return false;
    }
}
