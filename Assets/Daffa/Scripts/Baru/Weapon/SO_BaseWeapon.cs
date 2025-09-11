using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class SO_BaseWeapon : ScriptableObject
{
    public WeaponStat weaponStat;
    public string gunName;
    public LayerMask targetLayerMask;
    
    // HANYA data, TIDAK ADA state
    public abstract void Attack(WeaponContext context);
    public abstract void Reload(WeaponContext context);
    public abstract void OnEquip(WeaponContext context);
}

[System.Serializable]
public struct WeaponStat
{
    public float damage;
    public float shootingRange;
    public float fireRate;
    public float magazineSize;
    public float reloadTime;
}

// Struct untuk menyimpan state weapon
public struct WeaponState
{
    public float currentAmmo;
    public float nextTimeToFire;
    public bool isReloading;
    public bool isCharging;
    public float chargeStartTime;
}

// Context yang berisi semua yang dibutuhkan weapon
public class WeaponContext
{
    public WeaponState state;
    public Transform cameraTransform;
    public MonoBehaviour coroutineRunner;
    public System.Action<RaycastHit> onHitCallback;
}