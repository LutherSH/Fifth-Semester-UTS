using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Charge Shot Weapon", menuName = "SO/Weapon Type/Charge")]
public class SO_ChargeShot : SO_BaseWeapon
{
    public override void Attack(WeaponContext context)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCharging(context);
        }
        
        if (context.state.isCharging && Input.GetButtonUp("Fire1"))
        {
            ReleaseCharge(context);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload(context);
        }
    }
    
    private void StartCharging(WeaponContext context)
    {
        if (!context.state.isReloading && context.state.currentAmmo > 0)
        {
            context.state.isCharging = true;
            context.state.chargeStartTime = Time.time;
            Debug.Log(gunName + " Charging...");
        }
    }
    
    private void ReleaseCharge(WeaponContext context)
    {
        if (context.state.isCharging && Time.time >= context.state.nextTimeToFire)
        {
            float chargeTime = Time.time - context.state.chargeStartTime;
            context.state.nextTimeToFire = Time.time + (1 / weaponStat.fireRate);
            HandleShoot(context, chargeTime);
            context.state.isCharging = false;
        }
    }
    
    private void HandleShoot(WeaponContext context, float chargeTime)
    {
        context.state.currentAmmo--;
        float chargedDamage = weaponStat.damage * Mathf.Clamp(chargeTime * 2f, 1f, 3f);
        Debug.Log(gunName + " Charged Shot! Damage: " + chargedDamage + ", Bullet left: " + context.state.currentAmmo);
        
        RaycastHit hit;
        if (Physics.Raycast(context.cameraTransform.position, context.cameraTransform.forward, 
            out hit, weaponStat.shootingRange, targetLayerMask))
        {
            context.onHitCallback?.Invoke(hit);
        }
    }
    
    private void TryReload(WeaponContext context)
    {
        if (!context.state.isReloading && context.state.currentAmmo < weaponStat.magazineSize)
        {
            Reload(context);
        }
    }

    public override void Reload(WeaponContext context)
    {
        context.coroutineRunner.StartCoroutine(ReloadCoroutine(context));
    }
    
    private IEnumerator ReloadCoroutine(WeaponContext context)
    {
        context.state.isReloading = true;
        Debug.Log(gunName + " Is reloading...");

        yield return new WaitForSeconds(weaponStat.reloadTime);

        context.state.currentAmmo = weaponStat.magazineSize;
        context.state.isReloading = false;

        Debug.Log(gunName + " Is reloaded");
    }

    public override void OnEquip(WeaponContext context)
    {
        // Reset state khusus untuk charge weapon
        context.state.isCharging = false;
        context.state.chargeStartTime = 0f;
    }
}