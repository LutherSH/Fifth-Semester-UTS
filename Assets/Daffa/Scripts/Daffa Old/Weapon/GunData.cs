using UnityEngine;

[CreateAssetMenu(fileName = "NewGunData", menuName = "Gun/GunData")]
public class GunData : ScriptableObject
{
    public string gunName;

    //public LayerMask targetLayerMask;

    [Header("Fire Config")]
    public float damage;
    public float shootingRange;
    public float fireRate;

    [Header("Reload Config")]
    public float magazineSize;
    public float maxReserveAmmo;
    public float reloadTime;

    [Header("Aim Down Sight (ADS)")]
    public bool canAim = true;
    public float adsFOV = 30f;
    public float adsZoomTime = 0.3f;
    public float adsPositionMultiplier = 0.8f;
    public float adsRecoilMultiplier = 0.5f;

    [Header("Recoil Config")]
    public float recoilX = 0.1f;
    public float recoilY = 0.1f;
    public float recoilZ = 0.05f;
    public float snappiness = 10f;
    public float returnSpeed = 5f;
    public float maxRecoilRotation = 30f;

    // [Header("Weapon Sway")]
    // public float positionalSway = 0.1f;
    // public float rotationalSway = 0.1f;
    // public float swaySmoothness = 1f;
    // public float adsSwayMultiplier = 0.3f;

    [Header("VFX")]
    public GameObject bulletTrailPrefab;
    public float bulletSpeed;
}