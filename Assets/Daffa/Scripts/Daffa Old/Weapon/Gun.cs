using System.Collections;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public GunData gunData;
    public Transform gunMuzzle;
    public GameObject bulletHolePrefab;
    //public GameObject bulletHitParticlePrefab;
    [HideInInspector] public FpsController playerController;
    [HideInInspector] public Transform cameraTransform;

    private float currentAmmo = 0f;
    private float currentReserveAmmo = 0f;
    private float nextTimeToFire = 0f;
    private bool isReloading = false;
    private bool isAiming = false;

    // Components
    private Recoil recoilComponent;
    private WeaponAnim weaponAnimComponent;

    // New event for UI update
    public System.Action<float, float> OnAmmoChanged;
     public System.Action<bool> OnAimStateChanged;

    void Start()
    {
        currentAmmo = gunData.magazineSize;
        currentReserveAmmo = gunData.maxReserveAmmo;

        playerController = transform.root.GetComponent<FpsController>();
        cameraTransform = playerController.playerCamera.transform;

         // Initialize components
        InitializeRecoil();
        InitializeWeaponAnim();

        OnAmmoChanged?.Invoke(currentAmmo, currentReserveAmmo);

        //Debug.Log(gunData.gunName + " initialized. Ammo: " + currentAmmo + "/" + currentReserveAmmo);

    }

    public virtual void Update()
    {
        HandleAimInput();
    }

    private void InitializeRecoil()
    {
        recoilComponent = GetComponent<Recoil>();
        if (recoilComponent == null)
        {
            recoilComponent = gameObject.AddComponent<Recoil>();
        }
        //recoilComponent.Initialize(gunData);
    }

    private void InitializeWeaponAnim()
    {
        weaponAnimComponent = GetComponent<WeaponAnim>();
        if (weaponAnimComponent == null)
        {
            weaponAnimComponent = gameObject.AddComponent<WeaponAnim>();
        }
    }

    private void HandleAimInput()
    {
        if (gunData.canAim)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && !isReloading)
            {
                ToggleAim();
            }
            
            if (Input.GetKeyUp(KeyCode.Mouse1) && isAiming)
            {
                ToggleAim();
            }
        }
    }

    private void ToggleAim()
    {
        isAiming = !isAiming;
        
        if (isAiming)
        {
            playerController.Zoom(gunData.adsFOV, gunData.adsZoomTime);
            if (weaponAnimComponent != null)
                weaponAnimComponent.SetAiming(true); // Hanya kirim boolean
            if (recoilComponent != null)
                recoilComponent.SetAiming(true, gunData.adsRecoilMultiplier);
        }
        else
        {
            playerController.ResetZoom(gunData.adsZoomTime);
            if (weaponAnimComponent != null)
                weaponAnimComponent.SetAiming(false); // Hanya kirim boolean
            if (recoilComponent != null)
                recoilComponent.SetAiming(false, 1f);
        }

        OnAimStateChanged?.Invoke(isAiming);
    }

    private void OnDisable() => isReloading = false;

    public void TryReload()
    {
        if (!isReloading && currentAmmo < gunData.magazineSize && currentReserveAmmo > 0 && this.gameObject.activeSelf)
        {
            StartCoroutine(Reload());
        }
        else if (currentReserveAmmo <= 0)
        {
            //Debug.Log("No ammo left");
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        //Debug.Log(gunData.gunName + " Is reloading...");

        // Calculate ammo needed
        float ammoNeeded = gunData.magazineSize - currentAmmo;
        float ammoToReload = Mathf.Min(ammoNeeded, currentReserveAmmo);

        // Take reserve, add magazine
        currentReserveAmmo -= ammoToReload;
        currentAmmo += ammoToReload;

        yield return new WaitForSeconds(gunData.reloadTime);

        //currentAmmo = gunData.magazineSize;
        isReloading = false;

        //Debug.Log(gunData.gunName + " Reloaded. Ammo: " + currentAmmo + "/" + currentReserveAmmo);

        // Call event after reload
        OnAmmoChanged?.Invoke(currentAmmo, currentReserveAmmo);
    }

    public void TryShoot()
    {
        if (isReloading)
        {
            //Debug.Log(gunData.gunName + " Is reloading...");
            return;
        }

        if (currentAmmo <= 0f)
        {
            //Debug.Log(gunData.gunName + " Has no bullet left, Please reload");
            TryReload();
            return;
        }

        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1 / gunData.fireRate);
            HandleShoot();
        }
    }

    private void HandleShoot()
    {
        currentAmmo--;

        // Recoil();
        recoilComponent.RecoilFire();
        // Muzzleflash();

        //Debug.Log(gunData.gunName + " Shot! Ammo: " + currentAmmo + "/" + currentReserveAmmo);

        // Call event after shoot
        OnAmmoChanged?.Invoke(currentAmmo, currentReserveAmmo);

        Shoot();
    }

    public bool IsAiming()
    {
        return isAiming;
    }

    public bool IsReloading()
    {
        return isReloading;
    }

    // Method for ammo UI
    public float GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public float GetReserveAmmo()
    {
        return currentReserveAmmo;
    }

    // Pick up ammo
    public void AddAmmo(int amount)
    {
        float newReserveAmmo = currentReserveAmmo + amount;
        currentReserveAmmo = Mathf.Min(newReserveAmmo, gunData.maxReserveAmmo);
        
        //Debug.Log($"Picked up {amount} ammo for {gunData.gunName}. Reserve: {currentReserveAmmo}");
        
        // Update UI
        OnAmmoChanged?.Invoke(currentAmmo, currentReserveAmmo);
    }
    public abstract void Shoot();
}