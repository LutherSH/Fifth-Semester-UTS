using UnityEngine;

public class WeaponConfig : ScriptableObject
{
    public string weaponName;
    public enum WeaponType { Raycast, Projectile, Melee };
    public WeaponType weaponType;

    [Header("Stats")]
    public int damage;
    public float range;
    public float fireRate;

    [Header("Ammo")]
    public int magazineSize;
    public int maxReserveAmmo;
    public float reloadTime;

    [Header("Mode")]
    public FireMode fireMode;
    public enum FireMode { Auto, Semi, Burst };

    [Header("Burst Fire Config")]
    public int burstCount;
    public float burstDelay;

    [Header("Recoil")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;
    public float snappiness;
    public float returnSpeed;
    public float adsRecoilMultiplier;

    [Header("Sway")]
    public float positionalSway;
    public float rotationalSway;
    public float swaySmoothness;

    [Header("ADS Settings")]
    public bool canAim;
    public float adsFOV;
    public float adsZoomTime;
    public float aimTransitionSpeed;

    [Header("VFX")]
    public GameObject bulletTrailPrefab;
    public GameObject bulletHolePrefab;
    public float bulletSpeed;
}