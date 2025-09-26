using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class SO_BaseWeapon : ScriptableObject
{
    public WeaponStat weaponStat;
    public string gunName;
    public LayerMask targetLayerMask;
    
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

public class WeaponState
{
    public float currentAmmo;
    public float nextTimeToFire;
    public bool isReloading;
    public bool isCharging;
    public float chargeStartTime;
}

public class WeaponContext
{
    public WeaponState state;
    public Transform cameraTransform;
    public MonoBehaviour coroutineRunner;
    public System.Action<RaycastHit> onHitCallback;
}