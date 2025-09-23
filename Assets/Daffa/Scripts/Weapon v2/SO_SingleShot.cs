using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Single Shot Weapon", menuName = "SO/Weapon Type/Single")]
public class SO_SingleShot : SO_BaseWeapon
{
     public override void Attack(WeaponContext context)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            TryShoot(context);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload(context);
        }
    }
    
    private void TryShoot(WeaponContext context)
    {
        if (context.state.isReloading)
        {
            Debug.Log(gunName + " Is reloading...");
            return;
        }

        if (context.state.currentAmmo <= 0f)
        {
            Debug.Log(gunName + " Has no bullet left, Please reload");
            return;
        }

        if (Time.time >= context.state.nextTimeToFire)
        {
            context.state.nextTimeToFire = Time.time + (1 / weaponStat.fireRate);
            HandleShoot(context);
        }
    }
    
    private void TryReload(WeaponContext context)
    {
        if (!context.state.isReloading && context.state.currentAmmo < weaponStat.magazineSize)
        {
            Reload(context);
        }
    }
    
    private void HandleShoot(WeaponContext context)
    {
        context.state.currentAmmo--;
        Debug.Log(gunName + " Shot! Damage: " + weaponStat.damage + ", Bullet left: " + context.state.currentAmmo);
        
        // Raycast
        RaycastHit hit;
        if (Physics.Raycast(context.cameraTransform.position, context.cameraTransform.forward, 
            out hit, weaponStat.shootingRange, targetLayerMask))
        {
            context.onHitCallback?.Invoke(hit);
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
    }
}
