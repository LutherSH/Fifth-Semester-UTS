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
    

    private WeaponBehaviour currentWeapon;

    void Start()
    {
        FindAndSubscribeToActiveWeapon();
    }

    void Update()
    {
        // OPSIONAL: AUTO-REFRESH JIKA WeaponBehaviour BERUBAH
        // Jika diperlukan, bisa ditambahkan logika untuk detect weapon switch
    }

    private void FindAndSubscribeToActiveWeapon()
    {
        WeaponBehaviour[] allWeapons = FindObjectsOfType<WeaponBehaviour>();
        
        foreach (WeaponBehaviour weapon in allWeapons)
        {
            if (weapon.gameObject.activeInHierarchy)
            {
                SubscribeToWeapon(weapon);
                break;
            }
        }
    }

    public void SubscribeToWeapon(WeaponBehaviour weapon)
    {
        if (currentWeapon != null)
        {
            currentWeapon.OnAmmoChanged -= UpdateAmmoUI;
        }

        currentWeapon = weapon;
        currentWeapon.OnAmmoChanged += UpdateAmmoUI;
        
        UpdateAmmoUI(currentWeapon.GetCurrentAmmo(), currentWeapon.GetReserveAmmo());
        UpdateWeaponName();
    }

    private void UpdateAmmoUI(int currentAmmo, int reserveAmmo)
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
        if (weaponNameText != null && currentWeapon != null && currentWeapon.config != null)
        {
            weaponNameText.text = currentWeapon.config.weaponName;
        }
    }

    private void OnDestroy()
    {
        if (currentWeapon != null)
        {
            currentWeapon.OnAmmoChanged -= UpdateAmmoUI;
        }
    }
}
