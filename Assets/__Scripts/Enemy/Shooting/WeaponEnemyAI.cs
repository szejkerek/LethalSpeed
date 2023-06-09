using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class WeaponEnemyAI : MonoBehaviour
{
    [Range(-1f,1f)]
    [SerializeField] private float _yOffsetFromCamera;
    [SerializeField] private VisualEffect muzzleFlashEffect;

    [Header("Bullet proporties")]
    [SerializeField] private Bullet _bulletPrefab;

    [Header("Gun Proporties")]
    [SerializeField] private GameObject _weaponObject;
    [SerializeField] private Transform _barrelTip;

    [Header("Stats")]
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _firerate;
    [SerializeField] private int _magazineSize;
    [SerializeField] private float _reloadTime;
    [SerializeField]private float shotVariation;

    public int MagazineSize => _magazineSize;
    public int CurrentAmmo => _currentAmmo;
    private int _currentAmmo;

    public bool IsReloading => _isReloading;
    private bool _isReloading = false;

    private Enemy _enemy;
    private AudioEnemyAI _enemyAudio;
    private Rigidbody _weaponRigidbody;
    private VisionEnemyAI _visionEnemyAI;
    private float _timer;


    private ObjectPool<Bullet> _bulletPool;

    private void Awake()
    {
        _weaponRigidbody = _weaponObject.GetComponent<Rigidbody>();
        _visionEnemyAI = GetComponent<VisionEnemyAI>();
        _enemy = GetComponent<Enemy>();
        _enemyAudio = GetComponent<AudioEnemyAI>();
        _weaponRigidbody.isKinematic = true;

        _currentAmmo = _magazineSize;
        SetUpBulletPool();
    }

    private void SetUpBulletPool()
    {
        _bulletPool = new ObjectPool<Bullet>(() =>
        {
            Vector3 direction = _enemy.Player.PlayerCamera.transform.position - _barrelTip.position;
            return Instantiate(_bulletPrefab, _barrelTip.position, Quaternion.LookRotation(direction));
        }, bullet =>
        {
            Vector3 direction = (_enemy.Player.PlayerCamera.transform.position + Vector3.up * _yOffsetFromCamera - _barrelTip.position).normalized;
            bullet.transform.position = _barrelTip.position;
            bullet.transform.rotation = Quaternion.LookRotation(direction);
            bullet.Init(_enemy, direction, _bulletSpeed, DestroyBullet, 5f);
            bullet.gameObject.SetActive(true);
        }, bullet =>
        {
            bullet.gameObject.SetActive(false);
        }, bullet =>
        {
            Destroy(bullet.gameObject);
        }, true, 25, 200);
    }

    public void ShootingAtPlayer()
    {
        if (!_visionEnemyAI.TargerInVision || _isReloading)
            return;

        if (_timer <= 0)
        {
            Invoke(nameof(Shoot), Random.Range(0, shotVariation));
            _timer = 1 / _firerate;
        }

        _timer -= Time.deltaTime;
    }

    private void Shoot()
    {
        SpawnBullet();
        SpawnMuzzleFlash();
        _enemyAudio.EnemyAudioLib.Pistol.PlayRandomized(_enemyAudio.GunShotAudioSource);   
    }

    private void SpawnMuzzleFlash()
    {
        VisualEffect muzzleFlash = Instantiate(muzzleFlashEffect, _barrelTip.transform);
        muzzleFlash.Play();
        Destroy(muzzleFlash.gameObject, 1f);
    }

    private void SpawnBullet()
    {
        _bulletPool.Get();
        _currentAmmo--;
    }

    public void DropWeapon()
    {
        _weaponObject.transform.SetParent(null);
        _weaponRigidbody.isKinematic = false;
    }

    private void DestroyBullet(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

    public void TriggerReload()
    {
        StartCoroutine(Reload());
    }
    private IEnumerator Reload()
    {
        _isReloading = true;
        _enemy.Animator.SetBool("Reload", _isReloading);
        _enemyAudio.EnemyAudioLib.Reload.PlayRandomized(_enemyAudio.GunGeneralAudioSource);
        yield return new WaitForSeconds(_reloadTime);
        _currentAmmo = _magazineSize;
        _timer = 0;
        _isReloading = false;
        _enemy.Animator.SetBool("Reload", _isReloading);
    }

}
