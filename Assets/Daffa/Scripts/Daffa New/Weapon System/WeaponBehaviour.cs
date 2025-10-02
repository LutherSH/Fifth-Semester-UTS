using System.Collections;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    [Header("Configuration")]
    public WeaponConfig config;

    [Header("References")]
    [HideInInspector] public FpsController playerController;
    public Transform muzzleTransform;
    public Transform cameraTransform;
    public Recoil recoilComponent;
    public WeaponAnim weaponAnimComponent;

    // State
    private int currentAmmo;
    private int reserveAmmo;
    private float nextFireTime;
    private bool isReloading;
    private bool isAiming;
    private Coroutine reloadCoroutine;

    // Events
    public System.Action<int, int> OnAmmoChanged;
    public System.Action<bool> OnAimStateChanged;

    private void Start()
    {
        playerController = transform.root.GetComponent<FpsController>();
        cameraTransform = playerController.playerCamera.transform;
        InitializeWeapon();

        OnAmmoChanged?.Invoke(currentAmmo, reserveAmmo);
    }

    private void OnEnable()
    {
        isReloading = false;
        isAiming = false;

        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
        }

        UpdateAmmoUI();
        
        if (weaponAnimComponent != null)
            weaponAnimComponent.SetAiming(false);
            
        if (recoilComponent != null)
            recoilComponent.SetAiming(false, 1f);
    }

    private void OnDisable()
    {
        if (isReloading && reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
            isReloading = false;
        }

        if (isAiming)
        {
            SetAimState(false);
        }
    }

    public void InitializeWeapon()
    {
        currentAmmo = config.magazineSize;
        reserveAmmo = config.maxReserveAmmo;

        // Initialize components with config data
        if (recoilComponent != null)
            recoilComponent.Initialize(config);

        if (weaponAnimComponent != null)
            weaponAnimComponent.Initialize(config);

        UpdateAmmoUI();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Handle shooting based on fire mode
        if (config.fireMode == WeaponConfig.FireMode.Auto && Input.GetButton("Fire1"))
        {
            TryShoot();
        }

        if (config.fireMode == WeaponConfig.FireMode.Semi && Input.GetButtonDown("Fire1"))
        {
            TryShoot();
        }

        // Handle reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
        }

        // Handle aim
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartAim();
        }
        
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            StopAim();
        }
    }

    private void StartAim()
    {
        if (!config.canAim || isReloading || isAiming) return;

        SetAimState(true);
    }

    private void StopAim()
    {
        if (!isAiming) return;

        SetAimState(false);
    }

    private void SetAimState(bool aiming)
    {
        isAiming = aiming;
        OnAimStateChanged?.Invoke(isAiming);

        // Handle ADS effects through components
        if (weaponAnimComponent != null)
            weaponAnimComponent.SetAiming(isAiming);

        if (recoilComponent != null)
            recoilComponent.SetAiming(isAiming, config.adsRecoilMultiplier);

        //Debug.Log($"Aim state: {isAiming}");
    }

    public void TryShoot()
    {
        if (isReloading) return;

        if (currentAmmo <= 0)
        {
            TryReload();
            return;
        }

        if (Time.time < nextFireTime) return;

        Shoot();
    }

    private void Shoot()
    {
        currentAmmo--;
        nextFireTime = Time.time + (1f / config.fireRate);

        // Apply recoil
        if (recoilComponent != null)
            recoilComponent.RecoilFire();

        // Shoot based on weapon type
        switch (config.weaponType)
        {
            case WeaponConfig.WeaponType.Raycast:
                PerformRaycastShoot();
                break;
            // case WeaponConfig.WeaponType.Projectile:
            //     PerformProjectileShoot();
            //     break;
        }

        UpdateAmmoUI();
    }

    private void PerformRaycastShoot()
    {
        Ray ray = new Ray(cameraTransform.position, muzzleTransform.forward);
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, config.range))
        {
            Debug.Log($"Hit: {hit.collider.name} for {config.damage} damage");
            targetPoint = hit.point;

            ApplyDamageToTarget(hit.collider, config.damage);
            
            BulletHitFX(hit);
        }
        else
        {
            targetPoint = cameraTransform.position + cameraTransform.forward * config.range;
        }

        if (config.bulletTrailPrefab != null)
            StartCoroutine(SpawnBulletTrail(targetPoint));
    }

    private void ApplyDamageToTarget(Collider targetCollider, float damage)
    {
        int damageAmount = Mathf.RoundToInt(damage);
        
        if (targetCollider.CompareTag("Player"))
        {
            PlayerBehaviour player = targetCollider.GetComponent<PlayerBehaviour>();
            if (player != null)
            {
                player.PlayerTakeDmg(damageAmount);
            }
        }
        else if (targetCollider.CompareTag("Enemy"))
        {
            EnemyBehaviour enemy = targetCollider.GetComponent<EnemyBehaviour>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(damageAmount);
            }
        }
        else
        {
            Debug.Log("Hit " + targetCollider.name + " but no specific health system applied");
        }
    }

     private void BulletHitFX(RaycastHit hit)
    {
        if (config.bulletHolePrefab == null) return;

        Vector3 hitPosition = hit.point + hit.normal * 0.01f;
        GameObject bulletHole = Instantiate(config.bulletHolePrefab, hitPosition, Quaternion.LookRotation(hit.normal));
        bulletHole.transform.parent = hit.collider.transform;
        Destroy(bulletHole, 5f);
    }

    private IEnumerator SpawnBulletTrail(Vector3 targetPosition)
    {
        GameObject trail = Instantiate(config.bulletTrailPrefab, muzzleTransform.position, Quaternion.identity);
        Vector3 startPosition = trail.transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);

        float speed = config.bulletSpeed;
        float time = distance / speed;
        float elapsedTime = 0f;

        while (elapsedTime < time && trail != null)
        {
            trail.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (trail != null)
        {
            trail.transform.position = targetPosition;
            Destroy(trail);
        }
    }

    public void TryReload()
    {
        if (isReloading || currentAmmo == config.magazineSize || reserveAmmo <= 0) return;
        reloadCoroutine = StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        // Reset ADS saat mulai reload
        if (isAiming)
        {
            SetAimState(false);
        }

        yield return new WaitForSeconds(config.reloadTime);

        if (!this.gameObject.activeInHierarchy) yield break;

        int ammoNeeded = config.magazineSize - currentAmmo;
        int ammoToAdd = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToAdd;
        reserveAmmo -= ammoToAdd;

        isReloading = false;
        UpdateAmmoUI();

    }

    public void ToggleAim()
    {
        if (!config.canAim || isReloading) return;

        isAiming = !isAiming;
        OnAimStateChanged?.Invoke(isAiming);

        // Handle ADS effect through components
        if (weaponAnimComponent != null)
            weaponAnimComponent.SetAiming(isAiming);

        if (recoilComponent != null)
            recoilComponent.SetAiming(isAiming, config.adsRecoilMultiplier);
    }

    private void UpdateAmmoUI()
    {
        OnAmmoChanged?.Invoke(currentAmmo, reserveAmmo);
    }

    // Public getters
    public int GetCurrentAmmo() => currentAmmo;
    public int GetReserveAmmo() => reserveAmmo;
    // public bool IsAiming() => isAiming;
    // public bool IsReloading() => isReloading;
}