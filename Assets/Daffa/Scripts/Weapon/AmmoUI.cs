using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI currentAmmoText;
    public TextMeshProUGUI reserveAmmoText;
    

    private Gun currentGun;

    void Start()
    {
        FindAndSubscribeToActiveWeapon();
    }

    void Update()
    {
        // OPSIONAL: AUTO-REFRESH JIKA GUN BERUBAH
        // Jika diperlukan, bisa ditambahkan logika untuk detect weapon switch
    }

    private void FindAndSubscribeToActiveWeapon()
    {
        Gun[] allGuns = FindObjectsOfType<Gun>();
        
        foreach (Gun gun in allGuns)
        {
            if (gun.gameObject.activeInHierarchy)
            {
                SubscribeToGun(gun);
                break;
            }
        }
    }

    public void SubscribeToGun(Gun gun)
    {
        if (currentGun != null)
        {
            currentGun.OnAmmoChanged -= UpdateAmmoUI;
        }

        currentGun = gun;
        currentGun.OnAmmoChanged += UpdateAmmoUI;
        
        UpdateAmmoUI(currentGun.GetCurrentAmmo(), currentGun.GetReserveAmmo());
        
        UpdateWeaponName();
    }

    private void UpdateAmmoUI(float currentAmmo, float reserveAmmo)
    {
        // UPDATE TEXT CURRENT AMOO
        if (currentAmmoText != null)
            currentAmmoText.text = currentAmmo.ToString("0");
        
        // UPDATE TEXT RESERVE AMOO
        if (reserveAmmoText != null)
            reserveAmmoText.text = reserveAmmo.ToString("0");
    }

    private void UpdateWeaponName()
    {
        if (weaponNameText != null && currentGun != null && currentGun.gunData != null)
        {
            weaponNameText.text = currentGun.gunData.gunName;
        }
    }

    private void OnDestroy()
    {
        if (currentGun != null)
        {
            currentGun.OnAmmoChanged -= UpdateAmmoUI;
        }
    }
}
