using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public SO_BaseWeapon currentWeapon;
    public PlayerController playerController;
    public Transform cameraTransform;
    
    private WeaponContext context;
    private WeaponState state;

    void Start()
    {
        if (playerController == null)
            playerController = transform.root.GetComponent<PlayerController>();
        
        if (cameraTransform == null)
            cameraTransform = playerController.virtualCamera.transform;
        
        context = new WeaponContext
        {
            cameraTransform = cameraTransform,
            coroutineRunner = this,
            onHitCallback = HandleHit,
            state = new WeaponState()
        };
        
        if(currentWeapon != null)
            EquipWeapon(currentWeapon);
    }

    private void Update()
    {
        if(currentWeapon != null)
            currentWeapon.Attack(context);
    }
    
    public void EquipWeapon(SO_BaseWeapon newWeapon)
    {
        currentWeapon = newWeapon;
        
        // Reset state untuk weapon baru
        state.currentAmmo = newWeapon.weaponStat.magazineSize;
        state.nextTimeToFire = 0f;
        state.isReloading = false;
        state.isCharging = false;
        
        context.state = state;
        currentWeapon.OnEquip(context);
    }
    
    private void HandleHit(RaycastHit hit)
    {
        Debug.Log(currentWeapon.gunName + " Hit " + hit.collider.name);
        
    }
    
    public void StartReloadCoroutine(float reloadTime, System.Action onComplete)
    {
        StartCoroutine(ReloadCoroutine(reloadTime, onComplete));
    }
    
    private IEnumerator ReloadCoroutine(float reloadTime, System.Action onComplete)
    {
        state.isReloading = true;
        context.state = state;
        
        Debug.Log(currentWeapon.gunName + " Is reloading...");
        yield return new WaitForSeconds(reloadTime);
        
        state.currentAmmo = currentWeapon.weaponStat.magazineSize;
        state.isReloading = false;
        context.state = state;
        
        Debug.Log(currentWeapon.gunName + " Is reloaded");
        onComplete?.Invoke();
    }
    
    // Update state dari context (dipanggil setelah SO modify state)
    private void LateUpdate()
    {
        state = context.state;
    }
}