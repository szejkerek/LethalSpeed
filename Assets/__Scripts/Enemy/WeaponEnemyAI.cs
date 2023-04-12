using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponEnemyAI : MonoBehaviour
{
    [Header("Gun Proporties")]
    [SerializeField] private GameObject weaponObject;
    [SerializeField] private Transform barrelTip;
    [SerializeField] private Transform barrelBase;
    private Rigidbody weaponRigidbody;
    private Collider weaponCollider;

    private void Awake()
    {
        weaponRigidbody = weaponObject.GetComponent<Rigidbody>();
        weaponCollider = weaponObject.GetComponent<Collider>();
        weaponRigidbody.isKinematic = true;
        weaponCollider.isTrigger = true;
    }

    public void DropWeapon()
    {
        weaponObject.transform.SetParent(null);
        weaponRigidbody.isKinematic = false;
        weaponCollider.isTrigger = false;
    }

}
