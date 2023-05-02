using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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

    private ObjectPool<Bullet> _bulletPool;
    private void Awake()
    {
        _weaponRigidbody = _weaponObject.GetComponent<Rigidbody>();
        _weaponCollider = _weaponObject.GetComponent<Collider>();
        _enemy = GetComponent<Enemy>();
        _weaponRigidbody.isKinematic = true;
        _weaponCollider.isTrigger = true;

        _bulletPool = new ObjectPool<Bullet>(() =>
        {
            Vector3 direction = _enemy.Player.PlayerCamera.transform.position - _barrelTip.position;
            return Instantiate(_bulletPrefab, _barrelTip.position, Quaternion.LookRotation(direction));
        }, bullet =>
        {
            Vector3 direction = _enemy.Player.PlayerCamera.transform.position - _barrelTip.position;
            bullet.transform.position = _barrelTip.position;
            bullet.transform.rotation = Quaternion.LookRotation(direction);
            bullet.Init(direction, _bulletSpeed, DestroyBullet, 5f);
            bullet.gameObject.SetActive(true);
        }, bullet =>
        {
            bullet.gameObject.SetActive(false);
        }, bullet =>
        {
            Destroy(bullet.gameObject);
        }, true, 10, 200);
    }

    public void ShootAtPlayer()
    {
        _lastShotTime = Time.time;
        SpawnBullet();
    }

    private void SpawnBullet()
    {
        _bulletPool.Get();
    }

    public void DropWeapon()
    {
        _weaponObject.transform.SetParent(null);
        _weaponRigidbody.isKinematic = false;
        _weaponCollider.isTrigger = false;
    }

    private void DestroyBullet(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

}
