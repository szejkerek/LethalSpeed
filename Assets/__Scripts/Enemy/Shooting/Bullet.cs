using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    [SerializeField] VisualEffect _impactParticle;
    
    Enemy _firedByReference;
    Action<Bullet> _onHitAction;
    float _speed;
    Vector3 _direction;
    float _timeInAir;
    float _maxTimeInAir;
    bool _bulletReflected;

    public void Init(Enemy firedByReference, Vector3 direction, float speed, Action<Bullet> onHitAction, float maxTimeInAir = 5f, bool bullletReflected = false)
    {
        _timeInAir = 0;
        _direction = direction;
        _speed = speed;
        _onHitAction = onHitAction;
        _maxTimeInAir = maxTimeInAir;
        _firedByReference = firedByReference;
        _bulletReflected = bullletReflected;
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
        if (!ShouldDamageEnemy(col))
            return;

        if (col.TryGetComponent(out HitBox hitBox))
        {
            hitBox.TakeHit();
        }

        OnRegisteredImpact(col);
    }

    private void OnRegisteredImpact(Collider col)
    {
        SpawnImpactEffect();
        _onHitAction(this);
    }

    private void SpawnImpactEffect()
    {
        VisualEffect visualEffect = Instantiate(_impactParticle, transform.position, transform.rotation);
        visualEffect.Play();
        Destroy(visualEffect, 10f);
    }

    private bool ShootEnemy(Collider hit)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            foreach (Collider enemyCollider in enemy.GetComponentsInChildren<Collider>())
            {
                if (hit == enemyCollider)
                    return true;
            }            
        }
        return false;
    }

    private bool ShouldDamageEnemy(Collider hit)
    {       
        if(ShootEnemy(hit) && !_bulletReflected)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
}
