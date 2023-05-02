using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    Action<Bullet> _onHitAction;
    float _speed;
    Vector3 _direction;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 direction, float speed, Action<Bullet> onHitAction)
    {
        _direction = direction;
        _speed = speed;
        _onHitAction = onHitAction;
    }

    void Update()
    {
        transform.position += _direction * _speed * Time.deltaTime;
        rb.velocity = _direction * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("EnemyWeapon"))  
            return;

        _onHitAction(this);
    }

}
