using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera playerCamera;

    //Shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    //Burst
    public int bulletPerBurst = 3;
    public int burstBulletsLeft;

    //Spread
    public float spreadIntensity;

    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    public void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletPerBurst;
    }

    void Update()
    {
        if (currentShootingMode == ShootingMode.Auto)
        {
            // Holding Down Left Mouse Button
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            // Clicking Left Mouse Button
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        // Instantiate Bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        // Pointing The Bullet To Face The Shooting Direction 
        bullet.transform.forward = shootingDirection;

        // Shoot The Bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        // Destroy The Bullet After Some Time
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        // Checking IF We Are Done Shooting
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        // Burst Mode
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) 
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        // Shooting Ray From The Middle Of The Screen To Check Where Are We Pointing At
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            // Hitting Something
            targetPoint = hit.point;
        }
        else
        {
            // ShootingMode Air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        // Returning The Shooting Direction And Spread
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}