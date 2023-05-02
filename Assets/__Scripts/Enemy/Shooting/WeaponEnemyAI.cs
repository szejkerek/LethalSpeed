using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEnemyAI : MonoBehaviour
{
    [Header("Bullet proporties")]
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _bulletSpeed;

    [Header("Gun Proporties")]
    [SerializeField] private GameObject _weaponObject;
    [SerializeField] private Transform _barrelTip;

    private Enemy _enemy;
    private Rigidbody _weaponRigidbody;
    private Collider _weaponCollider;
    private float _lastShotTime;

    private void Awake()
    {
        _weaponRigidbody = _weaponObject.GetComponent<Rigidbody>();
        _weaponCollider = _weaponObject.GetComponent<Collider>();
        _enemy = GetComponent<Enemy>();
        _weaponRigidbody.isKinematic = true;
        _weaponCollider.isTrigger = true;
    }

    public void ShootAtPlayer()
    {
        _lastShotTime = Time.time;
        SpawnBullet();
    }

    public void DropWeapon()
    {
        _weaponObject.transform.SetParent(null);
        _weaponRigidbody.isKinematic = false;
        _weaponCollider.isTrigger = false;
    }

    private void DestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    private void SpawnBullet()
    {
        Vector3 direction = _enemy.Player.PlayerCamera.transform.position - _barrelTip.position;
        Bullet bullet = Instantiate(_bulletPrefab, _barrelTip.position, _barrelTip.rotation);
        bullet.Init(direction, _bulletSpeed, DestroyBullet);
    }

}
