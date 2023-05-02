using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponEnemyAI : MonoBehaviour
{
    [Header("Gun Proporties")]
    [SerializeField] private GameObject weaponObject;
    [SerializeField] private Transform barrelTip;
    [SerializeField] private LayerMask targetMask;
    private Rigidbody weaponRigidbody;
    private Collider weaponCollider;
    private float lastShotTime;

    private void Awake()
    {
        weaponRigidbody = weaponObject.GetComponent<Rigidbody>();
        weaponCollider = weaponObject.GetComponent<Collider>();
        weaponRigidbody.isKinematic = true;
        weaponCollider.isTrigger = true;
    }

    public void ShootAtTarget()
    {
        lastShotTime = Time.time;
    }

    public void DropWeapon()
    {
        weaponObject.transform.SetParent(null);
        weaponRigidbody.isKinematic = false;
        weaponCollider.isTrigger = false;
    }

}
