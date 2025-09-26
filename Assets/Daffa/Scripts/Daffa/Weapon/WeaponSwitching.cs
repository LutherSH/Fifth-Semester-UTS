using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform[] weapons;

    [Header("Keys")]
    [SerializeField] private KeyCode[] keys;

    [Header("Settings")]
    [SerializeField] private float switchTime;

    private int selectedWeapon;
    private float timeSinceLastSwitch;

    private AmmoUI ammoUI;

    private void Start()
    {
        SetWeapons();
        Select(selectedWeapon);

        ammoUI = FindObjectOfType<AmmoUI>();

        timeSinceLastSwitch = 0f;
    }

    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        // Keyboard Input
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]) && timeSinceLastSwitch >= switchTime)
            {
                selectedWeapon = i;
            }
        }

        // Scroll Wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0 && timeSinceLastSwitch >= switchTime)
        {
            if (scroll > 0)
            {
                selectedWeapon = (selectedWeapon + 1) % weapons.Length;
            }
            else
            {
                selectedWeapon = (selectedWeapon - 1 + weapons.Length) % weapons.Length;
            }
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            Select(selectedWeapon);
        }

        timeSinceLastSwitch += Time.deltaTime;
    }

    private void SetWeapons()
    {
        weapons = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            weapons[i] = transform.GetChild(i);
        }

        if (keys == null) keys = new KeyCode[weapons.Length];
    }

    private void Select(int weaponIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == weaponIndex);
        }

        timeSinceLastSwitch = 0f;

        // Update UI when weapon switched
        UpdateAmmoUIForWeapon(weaponIndex);

        Debug.Log("Selected weapon: " + weapons[weaponIndex].name);
    }
    
    private void UpdateAmmoUIForWeapon(int weaponIndex)
    {
        if (ammoUI == null) return;

        // Get gun component from selected weapon
        Gun selectedGun = weapons[weaponIndex].GetComponent<Gun>();
        if (selectedGun != null)
        {
            ammoUI.SubscribeToGun(selectedGun);
        }
    }
}