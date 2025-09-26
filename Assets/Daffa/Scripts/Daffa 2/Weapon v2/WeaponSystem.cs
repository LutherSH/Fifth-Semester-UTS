using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public SO_BaseWeapon currentWeapon;
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public Transform cameraTransform;
    
    private WeaponContext context;
    private WeaponState state;

    void Start()
    {
        if (playerController == null)
            playerController = transform.root.GetComponent<PlayerController>();
        
        if (cameraTransform == null)
            cameraTransform = playerController.playerCamera.transform;

        state = new WeaponState();
        context = new WeaponContext
        {
            cameraTransform = cameraTransform,
            coroutineRunner = this,
            onHitCallback = HandleHit,
            state = state
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
        
        // Reset state
        state.currentAmmo = newWeapon.weaponStat.magazineSize;
        state.nextTimeToFire = 0f;
        state.isReloading = false;
        state.isCharging = false;
        
        currentWeapon.OnEquip(context);
    }
    
    private void HandleHit(RaycastHit hit)
    {
        Debug.Log(currentWeapon.gunName + " Hit " + hit.collider.name);
    }
}