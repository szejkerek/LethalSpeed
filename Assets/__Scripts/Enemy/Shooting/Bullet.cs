using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Action<Bullet> _onHitAction;
    float _speed;
    Vector3 _direction;
    float _timeInAir;
    float _maxTimeInAir;

    public void Init(Vector3 direction, float speed, Action<Bullet> onHitAction, float maxTimeInAir = 5f)
    {
        _timeInAir = 0;
        _direction = direction;
        _speed = speed;
        _onHitAction = onHitAction;
        _maxTimeInAir = maxTimeInAir;
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
        if (col.CompareTag("EnemyWeapon"))
            return;

        if (col.TryGetComponent(out HitBox hitBox))
        {
            hitBox.TakeHit();
        }

        _onHitAction(this);
    }

}
