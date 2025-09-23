using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public GunData gunData;
    public Transform gunMuzzle;
    public GameObject bulletHolePrefab;
    //public GameObject bulletHitParticlePrefab;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public Transform cameraTransform;

    private float currentAmmo = 0f;
    private float nextTimeToFire = 0f;

    private bool isReloading = false;

    void Start()
    {
        currentAmmo = gunData.magazineSize;

        playerController = transform.root.GetComponent<PlayerController>();
        cameraTransform = playerController.virtualCamera.transform;
    }

    public virtual void Update()
    {
    }

    private void OnDisable() => isReloading = false;
    public void TryReload()
    {
        if (!isReloading && currentAmmo < gunData.magazineSize && this.gameObject.activeSelf)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log(gunData.gunName + " Is reloading...");

        yield return new WaitForSeconds(gunData.reloadTime);

        currentAmmo = gunData.magazineSize;
        isReloading = false;

        Debug.Log(gunData.gunName + " Is reloaded");
    }

    public void TryShoot()
    {
        if (isReloading)
        {
            Debug.Log(gunData.gunName + " Is reloading...");
            return;
        }

        if (currentAmmo <= 0f)
        {
            Debug.Log(gunData.gunName + " Has no bullet left, Please reload");
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
        // Muzzleflash();

        Debug.Log(gunData.gunName + " Shot!, Bullet left: " + currentAmmo);
        Shoot();
    }

    public abstract void Shoot();
}